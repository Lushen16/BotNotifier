<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    Private components As System.ComponentModel.IContainer

    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents lblSubtitle As System.Windows.Forms.Label
    Friend WithEvents lblStatus As System.Windows.Forms.Label
    Friend WithEvents btnToggleCapture As System.Windows.Forms.Button
    Friend WithEvents pbAudioLevel As System.Windows.Forms.ProgressBar
    Friend WithEvents lblVolume As System.Windows.Forms.Label

    ' Categoria 1: GM
    Friend WithEvents grpGm As System.Windows.Forms.GroupBox
    Friend WithEvents chkGm As System.Windows.Forms.CheckBox
    Friend WithEvents pbGmMatch As System.Windows.Forms.ProgressBar
    Friend WithEvents lblGmMatch As System.Windows.Forms.Label
    Friend WithEvents tbGmThreshold As System.Windows.Forms.TrackBar
    Friend WithEvents lblGmThreshold As System.Windows.Forms.Label
    Friend WithEvents btnPlayGm As System.Windows.Forms.Button
    Friend WithEvents btnRecGm As System.Windows.Forms.Button
    Friend WithEvents btnFileGm As System.Windows.Forms.Button

    ' Categoria 2: Mensagem Jogador
    Friend WithEvents grpMsgPlayer As System.Windows.Forms.GroupBox
    Friend WithEvents chkMsgPlayer As System.Windows.Forms.CheckBox
    Friend WithEvents pbMsgPlayerMatch As System.Windows.Forms.ProgressBar
    Friend WithEvents lblMsgPlayerMatch As System.Windows.Forms.Label
    Friend WithEvents tbMsgPlayerThreshold As System.Windows.Forms.TrackBar
    Friend WithEvents lblMsgPlayerThreshold As System.Windows.Forms.Label
    Friend WithEvents btnPlayMsgPlayer As System.Windows.Forms.Button
    Friend WithEvents btnRecMsgPlayer As System.Windows.Forms.Button
    Friend WithEvents btnFileMsgPlayer As System.Windows.Forms.Button

    ' Categoria 3: Teleport
    Friend WithEvents grpTeleport As System.Windows.Forms.GroupBox
    Friend WithEvents chkTeleport As System.Windows.Forms.CheckBox
    Friend WithEvents pbTeleportMatch As System.Windows.Forms.ProgressBar
    Friend WithEvents lblTeleportMatch As System.Windows.Forms.Label
    Friend WithEvents tbTeleportThreshold As System.Windows.Forms.TrackBar
    Friend WithEvents lblTeleportThreshold As System.Windows.Forms.Label
    Friend WithEvents btnPlayTeleport As System.Windows.Forms.Button
    Friend WithEvents btnRecTeleport As System.Windows.Forms.Button
    Friend WithEvents btnFileTeleport As System.Windows.Forms.Button

    ' Categoria 4: Pokemon
    Friend WithEvents grpPoke As System.Windows.Forms.GroupBox
    Friend WithEvents chkPoke As System.Windows.Forms.CheckBox
    Friend WithEvents pbPokeMatch As System.Windows.Forms.ProgressBar
    Friend WithEvents lblPokeMatch As System.Windows.Forms.Label
    Friend WithEvents tbPokeThreshold As System.Windows.Forms.TrackBar
    Friend WithEvents lblPokeThreshold As System.Windows.Forms.Label
    Friend WithEvents btnPlayPoke As System.Windows.Forms.Button
    Friend WithEvents btnRecPoke As System.Windows.Forms.Button
    Friend WithEvents btnFilePoke As System.Windows.Forms.Button

    ' Categoria 5: Seta
    Friend WithEvents grpSeta As System.Windows.Forms.GroupBox
    Friend WithEvents chkSeta As System.Windows.Forms.CheckBox
    Friend WithEvents pbSetaMatch As System.Windows.Forms.ProgressBar
    Friend WithEvents lblSetaMatch As System.Windows.Forms.Label
    Friend WithEvents tbSetaThreshold As System.Windows.Forms.TrackBar
    Friend WithEvents lblSetaThreshold As System.Windows.Forms.Label
    Friend WithEvents btnPlaySeta As System.Windows.Forms.Button
    Friend WithEvents btnRecSeta As System.Windows.Forms.Button
    Friend WithEvents btnFileSeta As System.Windows.Forms.Button

    ' Painel Lateral
    Friend WithEvents grpDiscord As System.Windows.Forms.GroupBox
    Friend WithEvents lblWebUrl As System.Windows.Forms.Label
    Friend WithEvents txtWebhook As System.Windows.Forms.TextBox
    Friend WithEvents lblMenType As System.Windows.Forms.Label
    Friend WithEvents cmbMention As System.Windows.Forms.ComboBox
    Friend WithEvents lblMenId As System.Windows.Forms.Label
    Friend WithEvents txtMentionId As System.Windows.Forms.TextBox
    Friend WithEvents chkLocalSound As System.Windows.Forms.CheckBox
    Friend WithEvents btnTestWebhook As System.Windows.Forms.Button
    Friend WithEvents btnSaveConfig As System.Windows.Forms.Button

    Friend WithEvents grpLogs As System.Windows.Forms.GroupBox
    Friend WithEvents lstLogs As System.Windows.Forms.ListBox

    Friend WithEvents notifyIcon1 As System.Windows.Forms.NotifyIcon
    Friend WithEvents contextMenu1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents menuOpen As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents menuExit As System.Windows.Forms.ToolStripMenuItem

    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.lblSubtitle = New System.Windows.Forms.Label()
        Me.lblStatus = New System.Windows.Forms.Label()
        Me.btnToggleCapture = New System.Windows.Forms.Button()
        Me.pbAudioLevel = New System.Windows.Forms.ProgressBar()
        Me.lblVolume = New System.Windows.Forms.Label()

        Me.grpGm = New System.Windows.Forms.GroupBox()
        Me.chkGm = New System.Windows.Forms.CheckBox()
        Me.pbGmMatch = New System.Windows.Forms.ProgressBar()
        Me.lblGmMatch = New System.Windows.Forms.Label()
        Me.tbGmThreshold = New System.Windows.Forms.TrackBar()
        Me.lblGmThreshold = New System.Windows.Forms.Label()
        Me.btnPlayGm = New System.Windows.Forms.Button()
        Me.btnRecGm = New System.Windows.Forms.Button()
        Me.btnFileGm = New System.Windows.Forms.Button()

        Me.grpMsgPlayer = New System.Windows.Forms.GroupBox()
        Me.chkMsgPlayer = New System.Windows.Forms.CheckBox()
        Me.pbMsgPlayerMatch = New System.Windows.Forms.ProgressBar()
        Me.lblMsgPlayerMatch = New System.Windows.Forms.Label()
        Me.tbMsgPlayerThreshold = New System.Windows.Forms.TrackBar()
        Me.lblMsgPlayerThreshold = New System.Windows.Forms.Label()
        Me.btnPlayMsgPlayer = New System.Windows.Forms.Button()
        Me.btnRecMsgPlayer = New System.Windows.Forms.Button()
        Me.btnFileMsgPlayer = New System.Windows.Forms.Button()

        Me.grpTeleport = New System.Windows.Forms.GroupBox()
        Me.chkTeleport = New System.Windows.Forms.CheckBox()
        Me.pbTeleportMatch = New System.Windows.Forms.ProgressBar()
        Me.lblTeleportMatch = New System.Windows.Forms.Label()
        Me.tbTeleportThreshold = New System.Windows.Forms.TrackBar()
        Me.lblTeleportThreshold = New System.Windows.Forms.Label()
        Me.btnPlayTeleport = New System.Windows.Forms.Button()
        Me.btnRecTeleport = New System.Windows.Forms.Button()
        Me.btnFileTeleport = New System.Windows.Forms.Button()

        Me.grpPoke = New System.Windows.Forms.GroupBox()
        Me.chkPoke = New System.Windows.Forms.CheckBox()
        Me.pbPokeMatch = New System.Windows.Forms.ProgressBar()
        Me.lblPokeMatch = New System.Windows.Forms.Label()
        Me.tbPokeThreshold = New System.Windows.Forms.TrackBar()
        Me.lblPokeThreshold = New System.Windows.Forms.Label()
        Me.btnPlayPoke = New System.Windows.Forms.Button()
        Me.btnRecPoke = New System.Windows.Forms.Button()
        Me.btnFilePoke = New System.Windows.Forms.Button()

        Me.grpSeta = New System.Windows.Forms.GroupBox()
        Me.chkSeta = New System.Windows.Forms.CheckBox()
        Me.pbSetaMatch = New System.Windows.Forms.ProgressBar()
        Me.lblSetaMatch = New System.Windows.Forms.Label()
        Me.tbSetaThreshold = New System.Windows.Forms.TrackBar()
        Me.lblSetaThreshold = New System.Windows.Forms.Label()
        Me.btnPlaySeta = New System.Windows.Forms.Button()
        Me.btnRecSeta = New System.Windows.Forms.Button()
        Me.btnFileSeta = New System.Windows.Forms.Button()

        Me.grpDiscord = New System.Windows.Forms.GroupBox()
        Me.lblWebUrl = New System.Windows.Forms.Label()
        Me.txtWebhook = New System.Windows.Forms.TextBox()
        Me.lblMenType = New System.Windows.Forms.Label()
        Me.cmbMention = New System.Windows.Forms.ComboBox()
        Me.lblMenId = New System.Windows.Forms.Label()
        Me.txtMentionId = New System.Windows.Forms.TextBox()
        Me.chkLocalSound = New System.Windows.Forms.CheckBox()
        Me.btnTestWebhook = New System.Windows.Forms.Button()
        Me.btnSaveConfig = New System.Windows.Forms.Button()

        Me.grpLogs = New System.Windows.Forms.GroupBox()
        Me.lstLogs = New System.Windows.Forms.ListBox()

        Me.notifyIcon1 = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.contextMenu1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.menuOpen = New System.Windows.Forms.ToolStripMenuItem()
        Me.menuExit = New System.Windows.Forms.ToolStripMenuItem()

        Me.SuspendLayout()

        ' Form Settings
        Me.BackColor = System.Drawing.Color.FromArgb(12, 16, 28)
        Me.ForeColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(1020, 720)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "CHARM BOT NOTIFIER - Windows Desktop v1.0"
        Me.Font = New System.Drawing.Font("Segoe UI", 9.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)

        ' Header
        Me.lblTitle.Text = "🚨 CHARM BOT NOTIFIER"
        Me.lblTitle.Font = New System.Drawing.Font("Segoe UI", 16.0F, System.Drawing.FontStyle.Bold)
        Me.lblTitle.ForeColor = System.Drawing.Color.FromArgb(0, 242, 254)
        Me.lblTitle.Location = New System.Drawing.Point(20, 15)
        Me.lblTitle.Size = New System.Drawing.Size(400, 32)

        Me.lblSubtitle.Text = "Monitor Acústico Desktop • WASAPI Loopback 24/7"
        Me.lblSubtitle.ForeColor = System.Drawing.Color.FromArgb(148, 163, 184)
        Me.lblSubtitle.Location = New System.Drawing.Point(22, 47)
        Me.lblSubtitle.Size = New System.Drawing.Size(350, 18)

        Me.lblStatus.Text = "🟢 OUVINDO ÁUDIO DO WINDOWS"
        Me.lblStatus.Font = New System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold)
        Me.lblStatus.ForeColor = System.Drawing.Color.FromArgb(0, 255, 135)
        Me.lblStatus.Location = New System.Drawing.Point(400, 22)
        Me.lblStatus.Size = New System.Drawing.Size(250, 24)

        Me.btnToggleCapture.Text = "⏹️ Desligar Leitura"
        Me.btnToggleCapture.BackColor = System.Drawing.Color.FromArgb(180, 20, 50)
        Me.btnToggleCapture.ForeColor = System.Drawing.Color.White
        Me.btnToggleCapture.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnToggleCapture.Font = New System.Drawing.Font("Segoe UI", 9.0F, System.Drawing.FontStyle.Bold)
        Me.btnToggleCapture.Location = New System.Drawing.Point(400, 48)
        Me.btnToggleCapture.Size = New System.Drawing.Size(180, 32)

        Me.lblVolume.Text = "Volume do Jogo:"
        Me.lblVolume.ForeColor = System.Drawing.Color.FromArgb(148, 163, 184)
        Me.lblVolume.Location = New System.Drawing.Point(595, 25)
        Me.lblVolume.Size = New System.Drawing.Size(100, 18)

        Me.pbAudioLevel.Location = New System.Drawing.Point(595, 48)
        Me.pbAudioLevel.Size = New System.Drawing.Size(80, 32)
        Me.pbAudioLevel.Value = 0

        ' Helper de Configuração de Grupo de Categoria
        SetupCategoryGroup(Me.grpGm, "🚨 MENSAGEM GM", 90, Me.chkGm, Me.pbGmMatch, Me.lblGmMatch, Me.tbGmThreshold, Me.lblGmThreshold, Me.btnPlayGm, Me.btnRecGm, Me.btnFileGm)
        SetupCategoryGroup(Me.grpMsgPlayer, "💬 MENSAGEM DE JOGADOR", 205, Me.chkMsgPlayer, Me.pbMsgPlayerMatch, Me.lblMsgPlayerMatch, Me.tbMsgPlayerThreshold, Me.lblMsgPlayerThreshold, Me.btnPlayMsgPlayer, Me.btnRecMsgPlayer, Me.btnFileMsgPlayer)
        SetupCategoryGroup(Me.grpTeleport, "⚡ TELEPORT", 320, Me.chkTeleport, Me.pbTeleportMatch, Me.lblTeleportMatch, Me.tbTeleportThreshold, Me.lblTeleportThreshold, Me.btnPlayTeleport, Me.btnRecTeleport, Me.btnFileTeleport)
        SetupCategoryGroup(Me.grpPoke, "🐉 POKEMON FORA DA HUNT", 435, Me.chkPoke, Me.pbPokeMatch, Me.lblPokeMatch, Me.tbPokeThreshold, Me.lblPokeThreshold, Me.btnPlayPoke, Me.btnRecPoke, Me.btnFilePoke)
        SetupCategoryGroup(Me.grpSeta, "🎯 SOM DE SETA", 550, Me.chkSeta, Me.pbSetaMatch, Me.lblSetaMatch, Me.tbSetaThreshold, Me.lblSetaThreshold, Me.btnPlaySeta, Me.btnRecSeta, Me.btnFileSeta)

        ' Painel Lateral: Discord
        Me.grpDiscord.Text = "Notificações Discord"
        Me.grpDiscord.ForeColor = System.Drawing.Color.FromArgb(0, 242, 254)
        Me.grpDiscord.Location = New System.Drawing.Point(700, 20)
        Me.grpDiscord.Size = New System.Drawing.Size(300, 320)

        Me.lblWebUrl.Text = "URL do Webhook:"
        Me.lblWebUrl.ForeColor = System.Drawing.Color.FromArgb(148, 163, 184)
        Me.lblWebUrl.Location = New System.Drawing.Point(15, 25)
        Me.lblWebUrl.Size = New System.Drawing.Size(260, 18)

        Me.txtWebhook.Location = New System.Drawing.Point(15, 45)
        Me.txtWebhook.Size = New System.Drawing.Size(265, 23)
        Me.txtWebhook.PasswordChar = "*"c
        Me.txtWebhook.BackColor = System.Drawing.Color.FromArgb(8, 12, 24)
        Me.txtWebhook.ForeColor = System.Drawing.Color.White

        Me.lblMenType.Text = "Menção:"
        Me.lblMenType.ForeColor = System.Drawing.Color.FromArgb(148, 163, 184)
        Me.lblMenType.Location = New System.Drawing.Point(15, 75)
        Me.lblMenType.Size = New System.Drawing.Size(260, 18)

        Me.cmbMention.Items.AddRange(New Object() {"none", "everyone", "here", "role", "user"})
        Me.cmbMention.Location = New System.Drawing.Point(15, 95)
        Me.cmbMention.Size = New System.Drawing.Size(265, 23)
        Me.cmbMention.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbMention.BackColor = System.Drawing.Color.FromArgb(8, 12, 24)
        Me.cmbMention.ForeColor = System.Drawing.Color.White

        Me.lblMenId.Text = "ID do Cargo / Usuário:"
        Me.lblMenId.ForeColor = System.Drawing.Color.FromArgb(148, 163, 184)
        Me.lblMenId.Location = New System.Drawing.Point(15, 125)
        Me.lblMenId.Size = New System.Drawing.Size(260, 18)

        Me.txtMentionId.Location = New System.Drawing.Point(15, 145)
        Me.txtMentionId.Size = New System.Drawing.Size(265, 23)
        Me.txtMentionId.BackColor = System.Drawing.Color.FromArgb(8, 12, 24)
        Me.txtMentionId.ForeColor = System.Drawing.Color.White

        Me.chkLocalSound.Text = "Tocar sirene/som local no PC"
        Me.chkLocalSound.ForeColor = System.Drawing.Color.White
        Me.chkLocalSound.Location = New System.Drawing.Point(15, 180)
        Me.chkLocalSound.Size = New System.Drawing.Size(260, 24)
        Me.chkLocalSound.Checked = True

        Me.btnSaveConfig.Text = "💾 Salvar Configurações"
        Me.btnSaveConfig.BackColor = System.Drawing.Color.FromArgb(0, 120, 215)
        Me.btnSaveConfig.ForeColor = System.Drawing.Color.White
        Me.btnSaveConfig.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSaveConfig.Font = New System.Drawing.Font("Segoe UI", 9.0F, System.Drawing.FontStyle.Bold)
        Me.btnSaveConfig.Location = New System.Drawing.Point(15, 220)
        Me.btnSaveConfig.Size = New System.Drawing.Size(265, 34)

        Me.btnTestWebhook.Text = "🔔 Testar Webhook"
        Me.btnTestWebhook.BackColor = System.Drawing.Color.FromArgb(88, 101, 242)
        Me.btnTestWebhook.ForeColor = System.Drawing.Color.White
        Me.btnTestWebhook.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnTestWebhook.Font = New System.Drawing.Font("Segoe UI", 9.0F, System.Drawing.FontStyle.Bold)
        Me.btnTestWebhook.Location = New System.Drawing.Point(15, 265)
        Me.btnTestWebhook.Size = New System.Drawing.Size(265, 34)

        Me.grpDiscord.Controls.AddRange(New System.Windows.Forms.Control() {Me.lblWebUrl, Me.txtWebhook, Me.lblMenType, Me.cmbMention, Me.lblMenId, Me.txtMentionId, Me.chkLocalSound, Me.btnSaveConfig, Me.btnTestWebhook})

        ' Painel de Logs
        Me.grpLogs.Text = "Histórico de Atividades"
        Me.grpLogs.ForeColor = System.Drawing.Color.FromArgb(0, 242, 254)
        Me.grpLogs.Location = New System.Drawing.Point(700, 350)
        Me.grpLogs.Size = New System.Drawing.Size(300, 345)

        Me.lstLogs.Location = New System.Drawing.Point(15, 25)
        Me.lstLogs.Size = New System.Drawing.Size(270, 305)
        Me.lstLogs.BackColor = System.Drawing.Color.FromArgb(8, 12, 24)
        Me.lstLogs.ForeColor = System.Drawing.Color.FromArgb(200, 215, 230)
        Me.lstLogs.Font = New System.Drawing.Font("Consolas", 8.5F)
        Me.lstLogs.BorderStyle = System.Windows.Forms.BorderStyle.None

        Me.grpLogs.Controls.Add(Me.lstLogs)

        ' System Tray NotifyIcon
        Me.notifyIcon1.Text = "Charm Bot Notifier"
        Me.notifyIcon1.Icon = System.Drawing.SystemIcons.Shield
        Me.notifyIcon1.ContextMenuStrip = Me.contextMenu1

        Me.menuOpen.Text = "Abrir Janela"
        Me.menuExit.Text = "Sair"
        Me.contextMenu1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.menuOpen, Me.menuExit})

        ' Adiciona tudo ao Form
        Me.Controls.AddRange(New System.Windows.Forms.Control() { _
            Me.lblTitle, Me.lblSubtitle, Me.lblStatus, Me.btnToggleCapture, Me.lblVolume, Me.pbAudioLevel, _
            Me.grpGm, Me.grpMsgPlayer, Me.grpTeleport, Me.grpPoke, Me.grpSeta, _
            Me.grpDiscord, Me.grpLogs _
        })

        Me.ResumeLayout(False)
    End Sub

    Private Sub SetupCategoryGroup(grp As System.Windows.Forms.GroupBox, title As String, top As Integer, chk As System.Windows.Forms.CheckBox, pbMatch As System.Windows.Forms.ProgressBar, lblMatch As System.Windows.Forms.Label, tb As System.Windows.Forms.TrackBar, lblThresh As System.Windows.Forms.Label, btnPlay As System.Windows.Forms.Button, btnRec As System.Windows.Forms.Button, btnFile As System.Windows.Forms.Button)
        grp.Text = title
        grp.ForeColor = System.Drawing.Color.FromArgb(0, 242, 254)
        grp.Font = New System.Drawing.Font("Segoe UI", 9.0F, System.Drawing.FontStyle.Bold)
        grp.Location = New System.Drawing.Point(20, top)
        grp.Size = New System.Drawing.Size(660, 105)

        chk.Text = "Ativo"
        chk.Checked = True
        chk.ForeColor = System.Drawing.Color.White
        chk.Location = New System.Drawing.Point(15, 25)
        chk.Size = New System.Drawing.Size(65, 24)

        ' Barra de Match em tempo real
        Dim lblRadar As New System.Windows.Forms.Label With {
            .Text = "Semelhança:",
            .ForeColor = System.Drawing.Color.FromArgb(148, 163, 184),
            .Location = New System.Drawing.Point(90, 28),
            .Size = New System.Drawing.Size(80, 18),
            .Font = New System.Drawing.Font("Segoe UI", 8.5F)
        }
        pbMatch.Location = New System.Drawing.Point(175, 27)
        pbMatch.Size = New System.Drawing.Size(120, 18)

        lblMatch.Text = "0%"
        lblMatch.ForeColor = System.Drawing.Color.White
        lblMatch.Location = New System.Drawing.Point(300, 28)
        lblMatch.Size = New System.Drawing.Size(45, 18)

        ' Slider de Sensibilidade
        Dim lblSens As New System.Windows.Forms.Label With {
            .Text = "Sensibilidade:",
            .ForeColor = System.Drawing.Color.FromArgb(148, 163, 184),
            .Location = New System.Drawing.Point(90, 60),
            .Size = New System.Drawing.Size(80, 18),
            .Font = New System.Drawing.Font("Segoe UI", 8.5F)
        }
        tb.Minimum = 50
        tb.Maximum = 98
        tb.Value = 80
        tb.TickStyle = System.Windows.Forms.TickStyle.None
        tb.Location = New System.Drawing.Point(175, 55)
        tb.Size = New System.Drawing.Size(120, 25)

        lblThresh.Text = "80%"
        lblThresh.ForeColor = System.Drawing.Color.White
        lblThresh.Location = New System.Drawing.Point(300, 60)
        lblThresh.Size = New System.Drawing.Size(45, 18)

        ' Botões de Ação
        btnPlay.Text = "▶️ Ouvir"
        btnPlay.BackColor = System.Drawing.Color.FromArgb(30, 41, 65)
        btnPlay.ForeColor = System.Drawing.Color.White
        btnPlay.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        btnPlay.Location = New System.Drawing.Point(360, 35)
        btnPlay.Size = New System.Drawing.Size(85, 32)

        btnRec.Text = "🎙️ Gravar"
        btnRec.BackColor = System.Drawing.Color.FromArgb(40, 50, 75)
        btnRec.ForeColor = System.Drawing.Color.White
        btnRec.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        btnRec.Location = New System.Drawing.Point(455, 35)
        btnRec.Size = New System.Drawing.Size(95, 32)

        btnFile.Text = "📁 Arquivo"
        btnFile.BackColor = System.Drawing.Color.FromArgb(30, 41, 65)
        btnFile.ForeColor = System.Drawing.Color.White
        btnFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        btnFile.Location = New System.Drawing.Point(560, 35)
        btnFile.Size = New System.Drawing.Size(85, 32)

        grp.Controls.AddRange(New System.Windows.Forms.Control() {chk, lblRadar, pbMatch, lblMatch, lblSens, tb, lblThresh, btnPlay, btnRec, btnFile})
    End Sub
End Class
