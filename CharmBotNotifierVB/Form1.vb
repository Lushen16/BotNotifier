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
    Private activeRecordingTriggerId As String = ""

    Public Sub New()
        InitializeComponent()
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        soundsDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sounds")
        AudioEngine.GenerateDefaultTones(soundsDir)

        config = ConfigManager.Load()
        audio = New AudioEngine()

        ' Carregar lista de janelas ativas
        RefreshOpenWindows()

        ' Carregar Perfis
        LoadProfilesCombo()

        ' Iniciar captura automaticamente
        audio.StartCapture()

        LogActivity("Charm Bot Notifier pronto. Suporte a Múltiplos Personagens e Janelas ativo.")
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

    Private isUpdatingProfilesList As Boolean = False

    Private Sub LoadProfilesCombo()
        isUpdatingProfilesList = True
        cmbProfiles.Items.Clear()
        For Each prof As UserProfile In config.Profiles
            cmbProfiles.Items.Add(prof)
        Next

        ' Seleciona o perfil ativo
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

        ' Salva o perfil anterior automaticamente antes de trocar
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

        ' Tentar selecionar a janela associada no ComboBox
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

        ' Atualizar AudioEngine com o PID do perfil
        audio.TargetPid = currentProfile.TargetPid
        audio.FilterByProcessAudio = currentProfile.FilterByProcessAudio

        ' Carregar gatilhos do perfil
        InitTriggerRows()

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
            .MentionType = "none"
        }

        newProf.Triggers.Add(New TriggerConfig With {.Id = "gm", .Name = "MENSAGEM GM", .SoundFile = "gm.wav", .Threshold = 82, .Cooldown = 6})
        newProf.Triggers.Add(New TriggerConfig With {.Id = "msg_player", .Name = "MENSAGEM DE JOGADOR", .SoundFile = "msg_player.wav", .Threshold = 78, .Cooldown = 6})
        newProf.Triggers.Add(New TriggerConfig With {.Id = "teleport", .Name = "TELEPORT", .SoundFile = "teleport.wav", .Threshold = 80, .Cooldown = 5})
        newProf.Triggers.Add(New TriggerConfig With {.Id = "pokemon", .Name = "POKEMON FORA DA HUNT", .SoundFile = "pokemon.wav", .Threshold = 80, .Cooldown = 10})
        newProf.Triggers.Add(New TriggerConfig With {.Id = "seta", .Name = "SOM DE SETA", .SoundFile = "seta.wav", .Threshold = 80, .Cooldown = 5})

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

    Private Sub InitTriggerRows()
        audio.ClearTriggers()

        For Each trig As TriggerConfig In currentProfile.Triggers
            Dim wavPath = Path.Combine(soundsDir, trig.SoundFile)
            audio.RegisterTrigger(trig.Id, trig.Name, wavPath, trig.Threshold, trig.Cooldown)

            Select Case trig.Id
                Case "gm"
                    chkGm.Checked = trig.Enabled
                    tbGmThreshold.Value = trig.Threshold
                    lblGmThreshold.Text = trig.Threshold.ToString() & "%"
                Case "msg_player"
                    chkMsgPlayer.Checked = trig.Enabled
                    tbMsgPlayerThreshold.Value = trig.Threshold
                    lblMsgPlayerThreshold.Text = trig.Threshold.ToString() & "%"
                Case "teleport"
                    chkTeleport.Checked = trig.Enabled
                    tbTeleportThreshold.Value = trig.Threshold
                    lblTeleportThreshold.Text = trig.Threshold.ToString() & "%"
                Case "pokemon"
                    chkPoke.Checked = trig.Enabled
                    tbPokeThreshold.Value = trig.Threshold
                    lblPokeThreshold.Text = trig.Threshold.ToString() & "%"
                Case "seta"
                    chkSeta.Checked = trig.Enabled
                    tbSetaThreshold.Value = trig.Threshold
                    lblSetaThreshold.Text = trig.Threshold.ToString() & "%"
            End Select
        Next
    End Sub

    Private Sub btnToggleCapture_Click(sender As Object, e As EventArgs) Handles btnToggleCapture.Click
        If btnToggleCapture.Text.Contains("Ligar") Then
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
            lblStatus.Text = "🟢 OUVINDO ÁUDIO DO WINDOWS"
            lblStatus.ForeColor = Color.FromArgb(0, 255, 135)
            btnToggleCapture.Text = "⏹️ Desligar Leitura"
            btnToggleCapture.BackColor = Color.FromArgb(180, 20, 50)
        Else
            lblStatus.Text = "🔴 LEITURA PARADA"
            lblStatus.ForeColor = Color.FromArgb(240, 80, 80)
            btnToggleCapture.Text = "▶️ Ligar Leitura de Áudio"
            btnToggleCapture.BackColor = Color.FromArgb(0, 180, 216)
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

        Select Case triggerId
            Case "gm"
                pbGmMatch.Value = percent
                lblGmMatch.Text = percent.ToString() & "%"
            Case "msg_player"
                pbMsgPlayerMatch.Value = percent
                lblMsgPlayerMatch.Text = percent.ToString() & "%"
            Case "teleport"
                pbTeleportMatch.Value = percent
                lblTeleportMatch.Text = percent.ToString() & "%"
            Case "pokemon"
                pbPokeMatch.Value = percent
                lblPokeMatch.Text = percent.ToString() & "%"
            Case "seta"
                pbSetaMatch.Value = percent
                lblSetaMatch.Text = percent.ToString() & "%"
        End Select
    End Sub

    Private Sub audio_SoundDetected(triggerId As String, confidence As Integer) Handles audio.SoundDetected
        If InvokeRequired Then
            Invoke(Sub() audio_SoundDetected(triggerId, confidence))
            Return
        End If

        Dim trig = currentProfile.Triggers.Find(Function(t) t.Id = triggerId)
        Dim trigName = If(trig IsNot Nothing, trig.Name, triggerId.ToUpper())
        Dim charName = If(Not String.IsNullOrEmpty(currentProfile.CharacterName), currentProfile.CharacterName, "Personagem")
        Dim winTitle = currentProfile.TargetWindowTitle

        LogActivity("🚨 [" & charName.ToUpper() & "] SOM DETECTADO: " & trigName & " (" & confidence.ToString() & "%)")

        If chkLocalSound.Checked Then
            System.Media.SystemSounds.Exclamation.Play()
        End If

        notifyIcon1.ShowBalloonTip(3500, "Charm Bot Notifier [" & charName & "]", "Detectado: " & trigName & " (" & confidence.ToString() & "%)", ToolTipIcon.Warning)

        If Not String.IsNullOrEmpty(currentProfile.WebhookUrl) Then
            Dim colorMap As New Dictionary(Of String, String) From {
                {"gm", "#FF0844"},
                {"msg_player", "#FFB703"},
                {"teleport", "#A855F7"},
                {"pokemon", "#00F2FE"},
                {"seta", "#FF4D00"}
            }
            Dim hex = If(colorMap.ContainsKey(triggerId), colorMap(triggerId), "#FF0844")
            Dim threshold = If(trig IsNot Nothing, trig.Threshold, 80)
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

    ' Botões de Ouvir
    Private Sub btnPlayGm_Click(sender As Object, e As EventArgs) Handles btnPlayGm.Click
        AudioEngine.PlayWav(Path.Combine(soundsDir, "gm.wav"))
    End Sub
    Private Sub btnPlayMsgPlayer_Click(sender As Object, e As EventArgs) Handles btnPlayMsgPlayer.Click
        AudioEngine.PlayWav(Path.Combine(soundsDir, "msg_player.wav"))
    End Sub
    Private Sub btnPlayTeleport_Click(sender As Object, e As EventArgs) Handles btnPlayTeleport.Click
        AudioEngine.PlayWav(Path.Combine(soundsDir, "teleport.wav"))
    End Sub
    Private Sub btnPlayPoke_Click(sender As Object, e As EventArgs) Handles btnPlayPoke.Click
        AudioEngine.PlayWav(Path.Combine(soundsDir, "pokemon.wav"))
    End Sub
    Private Sub btnPlaySeta_Click(sender As Object, e As EventArgs) Handles btnPlaySeta.Click
        AudioEngine.PlayWav(Path.Combine(soundsDir, "seta.wav"))
    End Sub

    ' Gravação de Amostra
    Private Sub ToggleRecording(triggerId As String, targetBtn As Button)
        Dim targetWav = Path.Combine(soundsDir, triggerId & ".wav")

        If Not audio.IsRecordingSample Then
            audio.StartRecordingSample(targetWav)
            activeRecordingTriggerId = triggerId
            targetBtn.Text = "⏹️ Parar"
            targetBtn.BackColor = Color.FromArgb(220, 50, 50)
            LogActivity("Gravando áudio do sistema para " & triggerId & "... Toque o som no jogo agora!")
        Else
            audio.StopRecordingSample()
            targetBtn.Text = "🎙️ Gravar"
            targetBtn.BackColor = Color.FromArgb(40, 50, 75)
            activeRecordingTriggerId = ""

            Dim trig = currentProfile.Triggers.Find(Function(t) t.Id = triggerId)
            If trig IsNot Nothing Then
                audio.RegisterTrigger(triggerId, trig.Name, targetWav, trig.Threshold, trig.Cooldown)
            End If

            LogActivity("Amostra salva para " & triggerId & "!")
            MessageBox.Show("Amostra gravada com sucesso! O som foi associado ao perfil de " & currentProfile.CharacterName & ".", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub btnRecGm_Click(sender As Object, e As EventArgs) Handles btnRecGm.Click
        ToggleRecording("gm", btnRecGm)
    End Sub
    Private Sub btnRecMsgPlayer_Click(sender As Object, e As EventArgs) Handles btnRecMsgPlayer.Click
        ToggleRecording("msg_player", btnRecMsgPlayer)
    End Sub
    Private Sub btnRecTeleport_Click(sender As Object, e As EventArgs) Handles btnRecTeleport.Click
        ToggleRecording("teleport", btnRecTeleport)
    End Sub
    Private Sub btnRecPoke_Click(sender As Object, e As EventArgs) Handles btnRecPoke.Click
        ToggleRecording("pokemon", btnRecPoke)
    End Sub
    Private Sub btnRecSeta_Click(sender As Object, e As EventArgs) Handles btnRecSeta.Click
        ToggleRecording("seta", btnRecSeta)
    End Sub

    ' Arquivo
    Private Sub SelectCustomAudioFile(triggerId As String)
        Using ofd As New OpenFileDialog()
            ofd.Filter = "Arquivos de Áudio (*.wav;*.mp3)|*.wav;*.mp3"
            If ofd.ShowDialog() = DialogResult.OK Then
                Dim targetWav = Path.Combine(soundsDir, triggerId & ".wav")
                File.Copy(ofd.FileName, targetWav, True)

                Dim trig = currentProfile.Triggers.Find(Function(t) t.Id = triggerId)
                If trig IsNot Nothing Then
                    audio.RegisterTrigger(triggerId, trig.Name, targetWav, trig.Threshold, trig.Cooldown)
                End If

                LogActivity("Arquivo de áudio importado para " & triggerId & "!")
                MessageBox.Show("Áudio associado com sucesso a " & triggerId & "!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        End Using
    End Sub

    Private Sub btnFileGm_Click(sender As Object, e As EventArgs) Handles btnFileGm.Click
        SelectCustomAudioFile("gm")
    End Sub
    Private Sub btnFileMsgPlayer_Click(sender As Object, e As EventArgs) Handles btnFileMsgPlayer.Click
        SelectCustomAudioFile("msg_player")
    End Sub
    Private Sub btnFileTeleport_Click(sender As Object, e As EventArgs) Handles btnFileTeleport.Click
        SelectCustomAudioFile("teleport")
    End Sub
    Private Sub btnFilePoke_Click(sender As Object, e As EventArgs) Handles btnFilePoke.Click
        SelectCustomAudioFile("pokemon")
    End Sub
    Private Sub btnFileSeta_Click(sender As Object, e As EventArgs) Handles btnFileSeta.Click
        SelectCustomAudioFile("seta")
    End Sub

    ' Sliders
    Private Sub UpdateSlider(triggerId As String, val As Integer, lbl As Label)
        lbl.Text = val.ToString() & "%"
        Dim trig = currentProfile.Triggers.Find(Function(t) t.Id = triggerId)
        If trig IsNot Nothing Then
            trig.Threshold = val
            audio.UpdateTriggerSettings(trig.Id, trig.Threshold, trig.Cooldown, trig.Enabled)
        End If
    End Sub

    Private Sub tbGmThreshold_Scroll(sender As Object, e As EventArgs) Handles tbGmThreshold.Scroll
        UpdateSlider("gm", tbGmThreshold.Value, lblGmThreshold)
    End Sub
    Private Sub tbMsgPlayerThreshold_Scroll(sender As Object, e As EventArgs) Handles tbMsgPlayerThreshold.Scroll
        UpdateSlider("msg_player", tbMsgPlayerThreshold.Value, lblMsgPlayerThreshold)
    End Sub
    Private Sub tbTeleportThreshold_Scroll(sender As Object, e As EventArgs) Handles tbTeleportThreshold.Scroll
        UpdateSlider("teleport", tbTeleportThreshold.Value, lblTeleportThreshold)
    End Sub
    Private Sub tbPokeThreshold_Scroll(sender As Object, e As EventArgs) Handles tbPokeThreshold.Scroll
        UpdateSlider("pokemon", tbPokeThreshold.Value, lblPokeThreshold)
    End Sub
    Private Sub tbSetaThreshold_Scroll(sender As Object, e As EventArgs) Handles tbSetaThreshold.Scroll
        UpdateSlider("seta", tbSetaThreshold.Value, lblSetaThreshold)
    End Sub

    ' Salvar Perfil e Configurações
    Private Sub SaveCurrentProfileValues()
        If currentProfile Is Nothing Then Return

        currentProfile.CharacterName = txtCharName.Text.Trim()
        currentProfile.ProfileName = "Perfil " & currentProfile.CharacterName
        currentProfile.WebhookUrl = txtWebhook.Text.Trim()
        currentProfile.MentionType = If(cmbMention.SelectedItem IsNot Nothing, cmbMention.SelectedItem.ToString(), "none")
        currentProfile.MentionId = txtMentionId.Text.Trim()
        currentProfile.PlayLocalSound = chkLocalSound.Checked
        currentProfile.FilterByProcessAudio = chkFilterProcess.Checked

        ' Salvar janela selecionada
        If cmbTargetWindow.SelectedItem IsNot Nothing Then
            Dim wItem = CType(cmbTargetWindow.SelectedItem, WindowItem)
            currentProfile.TargetPid = wItem.ProcessId
            currentProfile.TargetProcessName = wItem.ProcessName
            currentProfile.TargetWindowTitle = wItem.WindowTitle
            audio.TargetPid = wItem.ProcessId
        End If
        audio.FilterByProcessAudio = currentProfile.FilterByProcessAudio

        ' Salvar estado e limiares dos 5 gatilhos
        For Each trig In currentProfile.Triggers
            Select Case trig.Id
                Case "gm"
                    trig.Enabled = chkGm.Checked
                    trig.Threshold = tbGmThreshold.Value
                Case "msg_player"
                    trig.Enabled = chkMsgPlayer.Checked
                    trig.Threshold = tbMsgPlayerThreshold.Value
                Case "teleport"
                    trig.Enabled = chkTeleport.Checked
                    trig.Threshold = tbTeleportThreshold.Value
                Case "pokemon"
                    trig.Enabled = chkPoke.Checked
                    trig.Threshold = tbPokeThreshold.Value
                Case "seta"
                    trig.Enabled = chkSeta.Checked
                    trig.Threshold = tbSetaThreshold.Value
            End Select
        Next
    End Sub

    Private Sub btnSaveConfig_Click(sender As Object, e As EventArgs) Handles btnSaveConfig.Click
        If currentProfile Is Nothing Then Return

        SaveCurrentProfileValues()
        ConfigManager.Save(config)
        LoadProfilesCombo()

        LogActivity("Perfil '" & currentProfile.CharacterName & "' e configurações salvos com sucesso!")
        MessageBox.Show("Perfil de '" & currentProfile.CharacterName & "' salvo com sucesso!", "Charm Bot Notifier", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    ' Testar Webhook do Perfil
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
                    btnTestWebhook.Text = "🔔 Testar Webhook do Personagem"

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
            notifyIcon1.ShowBalloonTip(1800, "Charm Bot Notifier [" & charName & "]", "Vigiando o personagem em segundo plano perto do relógio!", ToolTipIcon.Info)
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
