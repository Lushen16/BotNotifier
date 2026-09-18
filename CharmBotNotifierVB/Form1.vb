Imports System
Imports System.IO
Imports System.Drawing
Imports System.Collections.Generic
Imports System.Windows.Forms
Imports System.Threading.Tasks

Partial Class Form1
    Inherits Form

    Private config As AppConfig
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

        ' Carregar configurações na interface
        txtWebhook.Text = config.WebhookUrl
        cmbMention.SelectedItem = If(String.IsNullOrEmpty(config.MentionType), "none", config.MentionType)
        txtMentionId.Text = config.MentionId
        chkLocalSound.Checked = config.PlayLocalSound

        ' Inicializar os 5 gatilhos
        InitTriggerRows()

        ' Iniciar captura automaticamente
        audio.StartCapture()

        LogActivity("Charm Bot Notifier iniciado. 5 categorias ativas.")
    End Sub

    Private Sub InitTriggerRows()
        For Each trig As TriggerConfig In config.Triggers
            Dim wavPath = Path.Combine(soundsDir, trig.SoundFile)
            audio.RegisterTrigger(trig.Id, trig.Name, wavPath, trig.Threshold, trig.Cooldown)

            ' Atualizar controles visuais correspondentes
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

        Dim trig = config.Triggers.Find(Function(t) t.Id = triggerId)
        Dim trigName = If(trig IsNot Nothing, trig.Name, triggerId.ToUpper())

        LogActivity("🚨 SOM DETECTADO: " & trigName & " (" & confidence.ToString() & "%)")

        ' Tocar alerta sonoro local se habilitado
        If chkLocalSound.Checked Then
            System.Media.SystemSounds.Exclamation.Play()
        End If

        ' Notificação na bandeja do Windows
        notifyIcon1.ShowBalloonTip(3000, "Charm Bot Notifier", "Detectado: " & trigName & " (" & confidence.ToString() & "%)", ToolTipIcon.Warning)

        ' Enviar para o Discord
        If Not String.IsNullOrEmpty(config.WebhookUrl) Then
            Dim colorMap As New Dictionary(Of String, String) From {
                {"gm", "#FF0844"},
                {"msg_player", "#FFB703"},
                {"teleport", "#A855F7"},
                {"pokemon", "#00F2FE"},
                {"seta", "#FF4D00"}
            }
            Dim hex = If(colorMap.ContainsKey(triggerId), colorMap(triggerId), "#FF0844")
            Dim threshold = If(trig IsNot Nothing, trig.Threshold, 80)

            Task.Factory.StartNew(Sub()
                Dim sent = DiscordNotifier.SendAlert(config.WebhookUrl, trigName, confidence, threshold, config.MentionType, config.MentionId, hex)
                If InvokeRequired Then
                    Invoke(Sub()
                        If sent Then
                            LogActivity("✅ Alerta enviado ao Discord com sucesso!")
                        Else
                            LogActivity("❌ Falha ao enviar alerta ao Discord.")
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

    ' Botões de Gravação Direta do Jogo
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

            ' Recarregar assinatura
            Dim trig = config.Triggers.Find(Function(t) t.Id = triggerId)
            If trig IsNot Nothing Then
                audio.RegisterTrigger(triggerId, trig.Name, targetWav, trig.Threshold, trig.Cooldown)
            End If

            LogActivity("Amostra gravada e associada com sucesso para " & triggerId & "!")
            MessageBox.Show("Amostra gravada com sucesso! O novo som já está memorizado.", "Charm Bot Notifier", MessageBoxButtons.OK, MessageBoxIcon.Information)
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

    ' Upload de Arquivo
    Private Sub SelectCustomAudioFile(triggerId As String)
        Using ofd As New OpenFileDialog()
            ofd.Filter = "Arquivos de Áudio (*.wav;*.mp3)|*.wav;*.mp3"
            If ofd.ShowDialog() = DialogResult.OK Then
                Dim targetWav = Path.Combine(soundsDir, triggerId & ".wav")
                File.Copy(ofd.FileName, targetWav, True)

                Dim trig = config.Triggers.Find(Function(t) t.Id = triggerId)
                If trig IsNot Nothing Then
                    audio.RegisterTrigger(triggerId, trig.Name, targetWav, trig.Threshold, trig.Cooldown)
                End If

                LogActivity("Arquivo de áudio importado para " & triggerId & "!")
                MessageBox.Show("Áudio carregado com sucesso para " & triggerId & "!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information)
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

    ' Sliders de Sensibilidade
    Private Sub UpdateSlider(triggerId As String, val As Integer, lbl As Label)
        lbl.Text = val.ToString() & "%"
        Dim trig = config.Triggers.Find(Function(t) t.Id = triggerId)
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

    ' Salvar Configurações
    Private Sub btnSaveConfig_Click(sender As Object, e As EventArgs) Handles btnSaveConfig.Click
        config.WebhookUrl = txtWebhook.Text.Trim()
        config.MentionType = If(cmbMention.SelectedItem IsNot Nothing, cmbMention.SelectedItem.ToString(), "none")
        config.MentionId = txtMentionId.Text.Trim()
        config.PlayLocalSound = chkLocalSound.Checked

        ConfigManager.Save(config)
        LogActivity("Configurações salvas no config.json.")
        MessageBox.Show("Configurações salvas com sucesso!", "Charm Bot Notifier", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    ' Testar Webhook do Discord
    Private Sub btnTestWebhook_Click(sender As Object, e As EventArgs) Handles btnTestWebhook.Click
        Dim url = txtWebhook.Text.Trim()
        If String.IsNullOrEmpty(url) Then
            MessageBox.Show("Insira a URL do Webhook do Discord!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        btnTestWebhook.Enabled = False
        btnTestWebhook.Text = "Enviando..."

        Dim mention = If(cmbMention.SelectedItem IsNot Nothing, cmbMention.SelectedItem.ToString(), "none")
        Dim mentionId = txtMentionId.Text.Trim()

        Task.Factory.StartNew(Sub()
            Dim sent = DiscordNotifier.SendTest(url, mention, mentionId)
            If InvokeRequired Then
                Invoke(Sub()
                    btnTestWebhook.Enabled = True
                    btnTestWebhook.Text = "🔔 Testar Webhook"

                    If sent Then
                        LogActivity("✅ Mensagem de teste enviada ao Discord com sucesso!")
                        MessageBox.Show("Mensagem de teste enviada com sucesso no Discord!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Else
                        LogActivity("❌ Erro ao enviar teste para o Discord.")
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

    ' Minimizar para o System Tray
    Private Sub Form1_Resize(sender As Object, e As EventArgs) Handles MyBase.Resize
        If WindowState = FormWindowState.Minimized Then
            Hide()
            notifyIcon1.Visible = True
            notifyIcon1.ShowBalloonTip(1500, "Charm Bot Notifier", "Rodando em segundo plano perto do relógio!", ToolTipIcon.Info)
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

    Private Sub menuExit_Click(sender As Object, e As EventArgs) Handles menuExit.Click
        audio.StopCapture()
        Application.Exit()
    End Sub
End Class
