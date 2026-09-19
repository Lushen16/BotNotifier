Imports System
Imports System.IO
Imports System.Drawing
Imports System.Diagnostics
Imports System.Collections.Generic
Imports System.Windows.Forms
Imports System.Threading.Tasks

Public Class WindowItem
    Public Property ProcessId As Integer
    Public Property ProcessName As String
    Public Property WindowTitle As String

    Public Overrides Function ToString() As String
        Return String.Format("{0} [{1}] (PID: {2})", WindowTitle, ProcessName, ProcessId)
    End Function
End Class

Partial Class Form1
    Inherits Form

    Private config As AppConfig
    Private currentProfile As UserProfile
    Private WithEvents audio As AudioEngine
    Private soundsDir As String
    Private tempRecordedWavPath As String = ""
    Private triggerRowMap As New Dictionary(Of String, TriggerRowControl)
    Private isUpdatingProfilesList As Boolean = False

    Public Sub New()
        InitializeComponent()
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim baseDir = AppDomain.CurrentDomain.BaseDirectory

        ' Carregar ícone e logo (procurando em data\, dados\, recursos\, ou embutidos no .exe)
        Try
            Dim subDirs = {"data", "dados", "recursos", "assets"}
            Dim icoPaths As New List(Of String)()
            For Each sd In subDirs
                icoPaths.Add(Path.Combine(baseDir, sd, "sentinel.ico"))
            Next
            icoPaths.Add(Path.Combine(baseDir, "sentinel.ico"))

            For Each ip In icoPaths
                If File.Exists(ip) Then
                    Me.Icon = New Icon(ip)
                    notifyIcon1.Icon = Me.Icon
                    Exit For
                End If
            Next

            If Me.Icon Is Nothing Then
                Try
                    Me.Icon = Drawing.Icon.ExtractAssociatedIcon(Application.ExecutablePath)
                    notifyIcon1.Icon = Me.Icon
                Catch
                End Try
            End If

            Dim pngPaths As New List(Of String)()
            For Each sd In subDirs
                pngPaths.Add(Path.Combine(baseDir, sd, "sentinel.png"))
            Next
            pngPaths.Add(Path.Combine(baseDir, "sentinel.png"))

            For Each pp In pngPaths
                If File.Exists(pp) Then
                    picSentinelLogo.Image = Image.FromFile(pp)
                    Exit For
                End If
            Next

            If picSentinelLogo.Image Is Nothing Then
                Dim asm = Reflection.Assembly.GetExecutingAssembly()
                Using stm = asm.GetManifestResourceStream("SentinelBot.sentinel.png")
                    If stm IsNot Nothing Then
                        picSentinelLogo.Image = Image.FromStream(stm)
                    End If
                End Using
            End If
        Catch ex As Exception
        End Try

        ' Sons em data\sounds\ (ou dados\sounds, recursos\sounds, assets\sounds)
        Dim candidateSoundDirs = {
            Path.Combine(baseDir, "data", "sounds"),
            Path.Combine(baseDir, "dados", "sounds"),
            Path.Combine(baseDir, "recursos", "sounds"),
            Path.Combine(baseDir, "assets", "sounds"),
            Path.Combine(baseDir, "sounds")
        }
        soundsDir = ""
        For Each cDir In candidateSoundDirs
            If Directory.Exists(cDir) Then
                soundsDir = cDir
                Exit For
            End If
        Next
        If String.IsNullOrEmpty(soundsDir) Then
            soundsDir = Path.Combine(baseDir, "data", "sounds")
            Directory.CreateDirectory(soundsDir)
        End If
        AudioEngine.GenerateDefaultTones(soundsDir)

        config = ConfigManager.Load()
        audio = New AudioEngine()

        ' Carregar janelas ativas
        RefreshOpenWindows()

        ' Carregar Perfis
        LoadProfilesCombo()

        ' Iniciar escuta
        audio.StartCapture()

        LogActivity("SentinelBot pronto. Monitoramento acústico multiclient ativo.")
    End Sub

    Private Sub RefreshOpenWindows()
        cmbTargetWindow.Items.Clear()
        cmbTargetWindow.Items.Add(New WindowItem With {.ProcessId = 0, .ProcessName = "Todas", .WindowTitle = "Todas as Janelas / Global"})

        Dim procs = Process.GetProcesses()
        For Each p As Process In procs
            Try
                If Not String.IsNullOrEmpty(p.MainWindowTitle) AndAlso p.Id <> Process.GetCurrentProcess().Id Then
                    cmbTargetWindow.Items.Add(New WindowItem With {
                        .ProcessId = p.Id,
                        .ProcessName = p.ProcessName,
                        .WindowTitle = p.MainWindowTitle
                    })
                End If
            Catch
            End Try
        Next

        If cmbTargetWindow.Items.Count > 0 Then
            cmbTargetWindow.SelectedIndex = 0
        End If
    End Sub

    Private Sub btnRefreshWindows_Click(sender As Object, e As EventArgs) Handles btnRefreshWindows.Click
        RefreshOpenWindows()
        LogActivity("Lista de janelas abertas atualizada.")
    End Sub

    Private Sub LoadProfilesCombo()
        isUpdatingProfilesList = True
        cmbProfiles.Items.Clear()
        For Each prof As UserProfile In config.Profiles
            cmbProfiles.Items.Add(prof)
        Next

        Dim idx = config.Profiles.FindIndex(Function(p) p.Id = config.ActiveProfileId)
        If idx >= 0 Then
            cmbProfiles.SelectedIndex = idx
        ElseIf cmbProfiles.Items.Count > 0 Then
            cmbProfiles.SelectedIndex = 0
        End If
        isUpdatingProfilesList = False
    End Sub

    Private Sub cmbProfiles_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbProfiles.SelectedIndexChanged
        If isUpdatingProfilesList OrElse cmbProfiles.SelectedItem Is Nothing Then Return

        If currentProfile IsNot Nothing AndAlso Not Object.ReferenceEquals(currentProfile, cmbProfiles.SelectedItem) Then
            SaveCurrentProfileValues()
            ConfigManager.Save(config)
        End If

        currentProfile = CType(cmbProfiles.SelectedItem, UserProfile)
        config.ActiveProfileId = currentProfile.Id

        ' Carregar dados do perfil na tela
        txtCharName.Text = currentProfile.CharacterName
        txtWebhook.Text = currentProfile.WebhookUrl
        cmbMention.SelectedItem = If(String.IsNullOrEmpty(currentProfile.MentionType), "none", currentProfile.MentionType)
        txtMentionId.Text = currentProfile.MentionId
        chkLocalSound.Checked = currentProfile.PlayLocalSound
        chkFilterProcess.Checked = currentProfile.FilterByProcessAudio

        ' Janela alvo
        Dim foundWindow As Boolean = False
        For i As Integer = 0 To cmbTargetWindow.Items.Count - 1
            Dim wItem = CType(cmbTargetWindow.Items(i), WindowItem)
            If wItem.ProcessId = currentProfile.TargetPid OrElse (Not String.IsNullOrEmpty(currentProfile.TargetWindowTitle) AndAlso wItem.WindowTitle = currentProfile.TargetWindowTitle) Then
                cmbTargetWindow.SelectedIndex = i
                foundWindow = True
                Exit For
            End If
        Next
        If Not foundWindow AndAlso cmbTargetWindow.Items.Count > 0 Then
            cmbTargetWindow.SelectedIndex = 0
        End If

        audio.TargetPid = currentProfile.TargetPid
        audio.FilterByProcessAudio = currentProfile.FilterByProcessAudio

        ' Carregar gatilhos dinâmicos
        RenderTriggerRows()

        LogActivity("Perfil carregado: " & currentProfile.ProfileName & " [" & currentProfile.CharacterName & "]")
    End Sub

    Private Sub btnNewProfile_Click(sender As Object, e As EventArgs) Handles btnNewProfile.Click
        Dim charName = Microsoft.VisualBasic.Interaction.InputBox("Digite o nome do Personagem para este novo perfil:", "Novo Perfil / Personagem", "Personagem2")
        If String.IsNullOrEmpty(charName.Trim()) Then Return

        Dim newProf As New UserProfile With {
            .Id = Guid.NewGuid().ToString(),
            .ProfileName = "Perfil " & charName.Trim(),
            .CharacterName = charName.Trim(),
            .WebhookUrl = If(currentProfile IsNot Nothing, currentProfile.WebhookUrl, ""),
            .MentionType = "none",
            .Triggers = New List(Of TriggerConfig)()
        }

        config.Profiles.Add(newProf)
        config.ActiveProfileId = newProf.Id
        ConfigManager.Save(config)

        LoadProfilesCombo()
        MessageBox.Show("Perfil para o personagem '" & charName & "' criado com sucesso!", "Novo Perfil", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub btnDeleteProfile_Click(sender As Object, e As EventArgs) Handles btnDeleteProfile.Click
        If config.Profiles.Count <= 1 Then
            MessageBox.Show("Você deve manter pelo menos um perfil cadastrado!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        If MessageBox.Show("Deseja realmente excluir o perfil '" & currentProfile.ProfileName & "'?", "Confirmar Exclusão", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
            config.Profiles.Remove(currentProfile)
            config.ActiveProfileId = config.Profiles(0).Id
            ConfigManager.Save(config)
            LoadProfilesCombo()
        End If
    End Sub

    ' =========================================================================
    ' RENDERIZAÇÃO DINÂMICA DE GATILHOS ATIVOS
    ' =========================================================================
    Private Sub RenderTriggerRows()
        pnlTriggersContainer.SuspendLayout()
        pnlTriggersContainer.Controls.Clear()
        triggerRowMap.Clear()
        audio.ClearTriggers()

        If currentProfile Is Nothing OrElse currentProfile.Triggers.Count = 0 Then
            lblEmptyNotice.Visible = True
            grpTriggersList.Text = "  🛡️ GATILHOS ATIVOS NESTE PERFIL (0)  "
            pnlTriggersContainer.ResumeLayout()
            Return
        End If

        lblEmptyNotice.Visible = False
        grpTriggersList.Text = "  🛡️ GATILHOS ATIVOS NESTE PERFIL (" & currentProfile.Triggers.Count.ToString() & ")  "

        Dim yOffset As Integer = 0
        For Each trig In currentProfile.Triggers
            Dim wavPath = Path.Combine(soundsDir, trig.SoundFile)
            audio.RegisterTrigger(trig.Id, trig.Name, wavPath, trig.Threshold, trig.Cooldown)

            Dim row As New TriggerRowControl(trig)
            row.Location = New Point(0, yOffset)
            row.Width = pnlTriggersContainer.ClientSize.Width - 10

            AddHandler row.PlayRequested, AddressOf TriggerRow_PlayRequested
            AddHandler row.DeleteRequested, AddressOf TriggerRow_DeleteRequested
            AddHandler row.SettingsChanged, AddressOf TriggerRow_SettingsChanged

            triggerRowMap(trig.Id) = row
            pnlTriggersContainer.Controls.Add(row)
            yOffset += row.Height + 8
        Next

        pnlTriggersContainer.ResumeLayout()
    End Sub

    Private Sub TriggerRow_PlayRequested(sender As TriggerRowControl, trig As TriggerConfig)
        Dim wavPath = Path.Combine(soundsDir, trig.SoundFile)
        AudioEngine.PlayWav(wavPath)
    End Sub

    Private Sub TriggerRow_SettingsChanged(sender As TriggerRowControl, trig As TriggerConfig)
        audio.UpdateTriggerSettings(trig.Id, trig.Threshold, trig.Cooldown, trig.Enabled)
    End Sub

    Private Sub TriggerRow_DeleteRequested(sender As TriggerRowControl, trig As TriggerConfig)
        If MessageBox.Show("Deseja realmente remover o gatilho '" & trig.Name & "' deste perfil?", "Remover Gatilho", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
            currentProfile.Triggers.Remove(trig)
            audio.UnregisterTrigger(trig.Id)
            RenderTriggerRows()
            ConfigManager.Save(config)
            LogActivity("Gatilho '" & trig.Name & "' removido do perfil.")
        End If
    End Sub

    ' =========================================================================
    ' ESTÚDIO DE GRAVAÇÃO E CLASSIFICAÇÃO DE NOVO SOM
    ' =========================================================================
    Private Sub btnRecordNew_Click(sender As Object, e As EventArgs) Handles btnRecordNew.Click
        If Not audio.IsRecordingSample Then
            tempRecordedWavPath = Path.Combine(soundsDir, "sample_" & DateTime.Now.Ticks.ToString() & ".wav")
            audio.StartRecordingSample(tempRecordedWavPath)

            btnRecordNew.Text = "⏹️ Concluir Gravação"
            btnRecordNew.NormalColor = Color.FromArgb(220, 38, 38)
            btnRecordNew.BorderColor = Color.FromArgb(248, 113, 113)
            btnRecordNew.ForeColor = Color.White
            lblRecStatus.Text = "🔴 Gravando áudio do jogo... Faça o som tocar agora!"
            lblRecStatus.ForeColor = Color.FromArgb(244, 63, 94)

            btnPreviewRecorded.Enabled = False
            btnSaveNewTrigger.Enabled = False
            LogActivity("Iniciada gravação de áudio do jogo...")
        Else
            audio.StopRecordingSample()

            btnRecordNew.Text = "🎙️ Gravar Som do Jogo"
            btnRecordNew.NormalColor = Color.FromArgb(36, 16, 26)
            btnRecordNew.BorderColor = Color.FromArgb(244, 63, 94)
            btnRecordNew.ForeColor = Color.FromArgb(254, 205, 211)

            btnPreviewRecorded.Enabled = True
            btnSaveNewTrigger.Enabled = True

            lblRecStatus.Text = "✅ Amostra gravada! Ouça ou escolha a categoria e clique em 'Salvar e Ativar Alerta'."
            lblRecStatus.ForeColor = Color.FromArgb(0, 255, 194)
            LogActivity("Amostra de áudio capturada com sucesso.")
        End If
    End Sub

    Private Sub btnImportFile_Click(sender As Object, e As EventArgs) Handles btnImportFile.Click
        Using ofd As New OpenFileDialog()
            ofd.Filter = "Arquivos de Áudio (*.wav;*.mp3)|*.wav;*.mp3"
            If ofd.ShowDialog() = DialogResult.OK Then
                tempRecordedWavPath = Path.Combine(soundsDir, "sample_" & DateTime.Now.Ticks.ToString() & ".wav")
                File.Copy(ofd.FileName, tempRecordedWavPath, True)

                btnPreviewRecorded.Enabled = True
                btnSaveNewTrigger.Enabled = True

                lblRecStatus.Text = "✅ Arquivo importado! Escolha a categoria e clique em 'Salvar e Ativar Alerta'."
                lblRecStatus.ForeColor = Color.FromArgb(0, 255, 194)
                LogActivity("Arquivo de áudio importado: " & Path.GetFileName(ofd.FileName))
            End If
        End Using
    End Sub

    Private Sub cmbClassify_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbClassify.SelectedIndexChanged
        If cmbClassify.SelectedIndex = 5 Then
            txtCustomName.Visible = True
            txtCustomName.Focus()
        Else
            txtCustomName.Visible = False
        End If
    End Sub

    Private Sub btnPreviewRecorded_Click(sender As Object, e As EventArgs) Handles btnPreviewRecorded.Click
        If Not String.IsNullOrEmpty(tempRecordedWavPath) AndAlso File.Exists(tempRecordedWavPath) Then
            AudioEngine.PlayWav(tempRecordedWavPath)
        Else
            MessageBox.Show("Nenhuma amostra gravada ou importada ainda!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
    End Sub

    Private Sub btnSaveNewTrigger_Click(sender As Object, e As EventArgs) Handles btnSaveNewTrigger.Click
        If String.IsNullOrEmpty(tempRecordedWavPath) OrElse Not File.Exists(tempRecordedWavPath) Then
            MessageBox.Show("Grave ou importe um áudio antes de salvar!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim chosenName As String = ""
        Dim chosenId As String = ""

        If cmbClassify.SelectedIndex = 5 Then
            chosenName = txtCustomName.Text.Trim()
            If String.IsNullOrEmpty(chosenName) Then
                MessageBox.Show("Por favor, digite o nome do alerta personalizado!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtCustomName.Focus()
                Return
            End If
            chosenId = "custom_" & DateTime.Now.Ticks.ToString().Substring(10)
        Else
            chosenName = cmbClassify.SelectedItem.ToString()
            Select Case cmbClassify.SelectedIndex
                Case 0 : chosenId = "gm"
                Case 1 : chosenId = "msg_player"
                Case 2 : chosenId = "teleport"
                Case 3 : chosenId = "pokemon"
                Case 4 : chosenId = "seta"
            End Select
        End If

        ' Salvar arquivo definitivo
        Dim finalWavName = chosenId & ".wav"
        Dim finalWavPath = Path.Combine(soundsDir, finalWavName)
        File.Copy(tempRecordedWavPath, finalWavPath, True)

        ' Checar se já existe no perfil
        Dim existing = currentProfile.Triggers.Find(Function(t) t.Id = chosenId)
        If existing IsNot Nothing Then
            existing.Name = chosenName
            existing.SoundFile = finalWavName
        Else
            Dim newTrig As New TriggerConfig With {
                .Id = chosenId,
                .Name = chosenName,
                .SoundFile = finalWavName,
                .Threshold = 80,
                .Cooldown = 6,
                .Enabled = True
            }
            currentProfile.Triggers.Add(newTrig)
        End If

        RenderTriggerRows()
        ConfigManager.Save(config)

        btnPreviewRecorded.Enabled = False
        btnSaveNewTrigger.Enabled = False
        lblRecStatus.Text = "✅ Gatilho '" & chosenName & "' ativado com sucesso!"
        lblRecStatus.ForeColor = Color.FromArgb(0, 255, 194)

        LogActivity("Gatilho '" & chosenName & "' adicionado e ativo no perfil!")
        MessageBox.Show("Gatilho '" & chosenName & "' adicionado e ativado com sucesso!", "SentinelBot", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    ' =========================================================================
    ' EVENTOS DE ÁUDIO E DETECÇÃO
    ' =========================================================================
    Private Sub btnToggleCapture_Click(sender As Object, e As EventArgs) Handles btnToggleCapture.Click
        If btnToggleCapture.Text.Contains("Ativar") OrElse btnToggleCapture.Text.Contains("Ligar") Then
            audio.StartCapture()
        Else
            audio.StopCapture()
        End If
    End Sub

    Private Sub audio_StatusChanged(isConnected As Boolean, message As String) Handles audio.StatusChanged
        If InvokeRequired Then
            Invoke(Sub() audio_StatusChanged(isConnected, message))
            Return
        End If

        If isConnected Then
            lblStatus.Text = "🟢 SENTINELA ATIVO"
            lblStatus.ForeColor = Color.FromArgb(0, 255, 194)
            btnToggleCapture.Text = "⏹️ Parar Sentinela"
            btnToggleCapture.NormalColor = Color.FromArgb(220, 38, 38)
            btnToggleCapture.HoverColor = Color.FromArgb(239, 68, 68)
            btnToggleCapture.BorderColor = Color.FromArgb(248, 113, 113)
            btnToggleCapture.ForeColor = Color.White
        Else
            lblStatus.Text = "🔴 SISTEMA EM ESPERA"
            lblStatus.ForeColor = Color.FromArgb(244, 63, 94)
            btnToggleCapture.Text = "▶️ Ativar Sentinela"
            btnToggleCapture.NormalColor = Color.FromArgb(0, 180, 230)
            btnToggleCapture.HoverColor = Color.FromArgb(0, 220, 255)
            btnToggleCapture.BorderColor = Color.FromArgb(0, 255, 255)
            btnToggleCapture.ForeColor = Color.FromArgb(8, 12, 22)
        End If

        LogActivity(message)
    End Sub

    Private Sub audio_LevelChanged(level As Single) Handles audio.LevelChanged
        If InvokeRequired Then
            BeginInvoke(Sub() audio_LevelChanged(level))
            Return
        End If
        pbAudioLevel.Value = CInt(Math.Min(100, level * 100))
    End Sub

    Private Sub audio_MatchProgress(triggerId As String, percent As Integer) Handles audio.MatchProgress
        If InvokeRequired Then
            BeginInvoke(Sub() audio_MatchProgress(triggerId, percent))
            Return
        End If

        If triggerRowMap.ContainsKey(triggerId) Then
            triggerRowMap(triggerId).UpdateMatchPercent(percent)
        End If
    End Sub

    Private Sub audio_SoundDetected(triggerId As String, confidence As Integer) Handles audio.SoundDetected
        If InvokeRequired Then
            Invoke(Sub() audio_SoundDetected(triggerId, confidence))
            Return
        End If

        Dim trig = currentProfile.Triggers.Find(Function(t) t.Id = triggerId)
        If trig Is Nothing OrElse Not trig.Enabled Then Return

        Dim trigName = trig.Name
        Dim charName = If(Not String.IsNullOrEmpty(currentProfile.CharacterName), currentProfile.CharacterName, "Personagem")
        Dim winTitle = currentProfile.TargetWindowTitle

        LogActivity("🚨 [" & charName.ToUpper() & "] SOM DETECTADO: " & trigName & " (" & confidence.ToString() & "%)")

        If chkLocalSound.Checked Then
            System.Media.SystemSounds.Exclamation.Play()
        End If

        notifyIcon1.ShowBalloonTip(3500, "SentinelBot [" & charName & "]", "Detectado: " & trigName & " (" & confidence.ToString() & "%)", ToolTipIcon.Warning)

        If Not String.IsNullOrEmpty(currentProfile.WebhookUrl) Then
            Dim hex = "#00E5FF"
            If trig.Id = "gm" OrElse trig.Name.ToUpper().Contains("GM") Then hex = "#FF0844"
            If trig.Id = "msg_player" OrElse trig.Name.ToUpper().Contains("JOGADOR") Then hex = "#FFB703"
            If trig.Id = "teleport" OrElse trig.Name.ToUpper().Contains("TELEPORT") Then hex = "#A855F7"
            If trig.Id = "pokemon" OrElse trig.Name.ToUpper().Contains("POKEMON") Then hex = "#00F2FE"
            If trig.Id = "seta" OrElse trig.Name.ToUpper().Contains("SETA") Then hex = "#FF4D00"

            Dim threshold = trig.Threshold
            Dim url = currentProfile.WebhookUrl
            Dim menType = currentProfile.MentionType
            Dim menId = currentProfile.MentionId

            Task.Factory.StartNew(Sub()
                Dim sent = DiscordNotifier.SendAlert(url, charName, winTitle, trigName, confidence, threshold, menType, menId, hex)
                If InvokeRequired Then
                    Invoke(Sub()
                        If sent Then
                            LogActivity("✅ Alerta de [" & charName & "] enviado ao Discord!")
                        Else
                            LogActivity("❌ Falha ao enviar alerta de [" & charName & "] ao Discord.")
                        End If
                    End Sub)
                End If
            End Sub)
        End If
    End Sub

    ' =========================================================================
    ' SALVAMENTO & DISCORD
    ' =========================================================================
    Private Sub SaveCurrentProfileValues()
        If currentProfile Is Nothing Then Return

        currentProfile.CharacterName = txtCharName.Text.Trim()
        currentProfile.ProfileName = "Perfil " & currentProfile.CharacterName
        currentProfile.WebhookUrl = txtWebhook.Text.Trim()
        currentProfile.MentionType = If(cmbMention.SelectedItem IsNot Nothing, cmbMention.SelectedItem.ToString(), "none")
        currentProfile.MentionId = txtMentionId.Text.Trim()
        currentProfile.PlayLocalSound = chkLocalSound.Checked
        currentProfile.FilterByProcessAudio = chkFilterProcess.Checked

        If cmbTargetWindow.SelectedItem IsNot Nothing Then
            Dim wItem = CType(cmbTargetWindow.SelectedItem, WindowItem)
            currentProfile.TargetPid = wItem.ProcessId
            currentProfile.TargetProcessName = wItem.ProcessName
            currentProfile.TargetWindowTitle = wItem.WindowTitle
            audio.TargetPid = wItem.ProcessId
        End If
        audio.FilterByProcessAudio = currentProfile.FilterByProcessAudio
    End Sub

    Private Sub btnSaveConfig_Click(sender As Object, e As EventArgs) Handles btnSaveConfig.Click
        If currentProfile Is Nothing Then Return

        SaveCurrentProfileValues()
        ConfigManager.Save(config)
        LoadProfilesCombo()

        LogActivity("Perfil '" & currentProfile.CharacterName & "' e configurações salvos com sucesso!")
        MessageBox.Show("Perfil de '" & currentProfile.CharacterName & "' salvo com sucesso!", "SentinelBot", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub btnTestWebhook_Click(sender As Object, e As EventArgs) Handles btnTestWebhook.Click
        Dim url = txtWebhook.Text.Trim()
        If String.IsNullOrEmpty(url) Then
            MessageBox.Show("Insira a URL do Webhook do Discord neste perfil!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        btnTestWebhook.Enabled = False
        btnTestWebhook.Text = "Enviando..."

        Dim charName = txtCharName.Text.Trim()
        Dim winTitle = If(cmbTargetWindow.SelectedItem IsNot Nothing, cmbTargetWindow.SelectedItem.ToString(), "Janela Principal")
        Dim mention = If(cmbMention.SelectedItem IsNot Nothing, cmbMention.SelectedItem.ToString(), "none")
        Dim mentionId = txtMentionId.Text.Trim()

        Task.Factory.StartNew(Sub()
            Dim sent = DiscordNotifier.SendTest(url, charName, winTitle, mention, mentionId)
            If InvokeRequired Then
                Invoke(Sub()
                    btnTestWebhook.Enabled = True
                    btnTestWebhook.Text = "🔔 Testar Webhook"

                    If sent Then
                        LogActivity("✅ Teste de [" & charName & "] enviado ao Discord!")
                        MessageBox.Show("Mensagem de teste de '" & charName & "' enviada com sucesso ao Discord!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Else
                        LogActivity("❌ Erro ao enviar teste de [" & charName & "] ao Discord.")
                        MessageBox.Show("Falha ao enviar para o Discord. Verifique a URL do Webhook.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End If
                End Sub)
            End If
        End Sub)
    End Sub

    Private Sub LogActivity(msg As String)
        Dim timeStr = DateTime.Now.ToString("HH:mm:ss")
        lstLogs.Items.Insert(0, "[" & timeStr & "] " & msg)
        If lstLogs.Items.Count > 100 Then lstLogs.Items.RemoveAt(lstLogs.Items.Count - 1)
    End Sub

    Private Sub Form1_Resize(sender As Object, e As EventArgs) Handles MyBase.Resize
        If WindowState = FormWindowState.Minimized Then
            Hide()
            notifyIcon1.Visible = True
            Dim charName = If(currentProfile IsNot Nothing, currentProfile.CharacterName, "Personagem")
            notifyIcon1.ShowBalloonTip(1800, "SentinelBot [" & charName & "]", "Sentinela vigilante em segundo plano!", ToolTipIcon.Info)
        End If
    End Sub

    Private Sub notifyIcon1_DoubleClick(sender As Object, e As EventArgs) Handles notifyIcon1.DoubleClick
        Show()
        WindowState = FormWindowState.Normal
        BringToFront()
    End Sub

    Private Sub menuOpen_Click(sender As Object, e As EventArgs) Handles menuOpen.Click
        Show()
        WindowState = FormWindowState.Normal
        BringToFront()
    End Sub

    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Try
            SaveCurrentProfileValues()
            ConfigManager.Save(config)
            audio.StopCapture()
        Catch ex As Exception
        End Try
    End Sub

    Private Sub menuExit_Click(sender As Object, e As EventArgs) Handles menuExit.Click
        Try
            SaveCurrentProfileValues()
            ConfigManager.Save(config)
            audio.StopCapture()
        Catch ex As Exception
        End Try
        Application.Exit()
    End Sub
End Class
