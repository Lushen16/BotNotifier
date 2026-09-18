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

    Friend WithEvents picSentinelLogo As System.Windows.Forms.PictureBox
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents lblSubtitle As System.Windows.Forms.Label
    Friend WithEvents lblStatus As System.Windows.Forms.Label
    Friend WithEvents btnToggleCapture As RoundedButton
    Friend WithEvents pbAudioLevel As System.Windows.Forms.ProgressBar
    Friend WithEvents lblVolume As System.Windows.Forms.Label

    ' Barra de Perfis e Janela Alvo
    Friend WithEvents grpProfileBar As System.Windows.Forms.GroupBox
    Friend WithEvents lblProfileLabel As System.Windows.Forms.Label
    Friend WithEvents cmbProfiles As System.Windows.Forms.ComboBox
    Friend WithEvents btnNewProfile As RoundedButton
    Friend WithEvents btnDeleteProfile As RoundedButton
    Friend WithEvents lblCharName As System.Windows.Forms.Label
    Friend WithEvents txtCharName As System.Windows.Forms.TextBox
    Friend WithEvents lblTargetWindow As System.Windows.Forms.Label
    Friend WithEvents cmbTargetWindow As System.Windows.Forms.ComboBox
    Friend WithEvents btnRefreshWindows As RoundedButton
    Friend WithEvents chkFilterProcess As System.Windows.Forms.CheckBox

    ' Categoria 1: GM
    Friend WithEvents grpGm As System.Windows.Forms.GroupBox
    Friend WithEvents chkGm As System.Windows.Forms.CheckBox
    Friend WithEvents pbGmMatch As System.Windows.Forms.ProgressBar
    Friend WithEvents lblGmMatch As System.Windows.Forms.Label
    Friend WithEvents tbGmThreshold As System.Windows.Forms.TrackBar
    Friend WithEvents lblGmThreshold As System.Windows.Forms.Label
    Friend WithEvents btnPlayGm As RoundedButton
    Friend WithEvents btnRecGm As RoundedButton
    Friend WithEvents btnFileGm As RoundedButton

    ' Categoria 2: Mensagem Jogador
    Friend WithEvents grpMsgPlayer As System.Windows.Forms.GroupBox
    Friend WithEvents chkMsgPlayer As System.Windows.Forms.CheckBox
    Friend WithEvents pbMsgPlayerMatch As System.Windows.Forms.ProgressBar
    Friend WithEvents lblMsgPlayerMatch As System.Windows.Forms.Label
    Friend WithEvents tbMsgPlayerThreshold As System.Windows.Forms.TrackBar
    Friend WithEvents lblMsgPlayerThreshold As System.Windows.Forms.Label
    Friend WithEvents btnPlayMsgPlayer As RoundedButton
    Friend WithEvents btnRecMsgPlayer As RoundedButton
    Friend WithEvents btnFileMsgPlayer As RoundedButton

    ' Categoria 3: Teleport
    Friend WithEvents grpTeleport As System.Windows.Forms.GroupBox
    Friend WithEvents chkTeleport As System.Windows.Forms.CheckBox
    Friend WithEvents pbTeleportMatch As System.Windows.Forms.ProgressBar
    Friend WithEvents lblTeleportMatch As System.Windows.Forms.Label
    Friend WithEvents tbTeleportThreshold As System.Windows.Forms.TrackBar
    Friend WithEvents lblTeleportThreshold As System.Windows.Forms.Label
    Friend WithEvents btnPlayTeleport As RoundedButton
    Friend WithEvents btnRecTeleport As RoundedButton
    Friend WithEvents btnFileTeleport As RoundedButton

    ' Categoria 4: Pokemon
    Friend WithEvents grpPoke As System.Windows.Forms.GroupBox
    Friend WithEvents chkPoke As System.Windows.Forms.CheckBox
    Friend WithEvents pbPokeMatch As System.Windows.Forms.ProgressBar
    Friend WithEvents lblPokeMatch As System.Windows.Forms.Label
    Friend WithEvents tbPokeThreshold As System.Windows.Forms.TrackBar
    Friend WithEvents lblPokeThreshold As System.Windows.Forms.Label
    Friend WithEvents btnPlayPoke As RoundedButton
    Friend WithEvents btnRecPoke As RoundedButton
    Friend WithEvents btnFilePoke As RoundedButton

    ' Categoria 5: Seta
    Friend WithEvents grpSeta As System.Windows.Forms.GroupBox
    Friend WithEvents chkSeta As System.Windows.Forms.CheckBox
    Friend WithEvents pbSetaMatch As System.Windows.Forms.ProgressBar
    Friend WithEvents lblSetaMatch As System.Windows.Forms.Label
    Friend WithEvents tbSetaThreshold As System.Windows.Forms.TrackBar
    Friend WithEvents lblSetaThreshold As System.Windows.Forms.Label
    Friend WithEvents btnPlaySeta As RoundedButton
    Friend WithEvents btnRecSeta As RoundedButton
    Friend WithEvents btnFileSeta As RoundedButton

    ' Painel Lateral
    Friend WithEvents grpDiscord As System.Windows.Forms.GroupBox
    Friend WithEvents lblWebUrl As System.Windows.Forms.Label
    Friend WithEvents txtWebhook As System.Windows.Forms.TextBox
    Friend WithEvents lblMenType As System.Windows.Forms.Label
    Friend WithEvents cmbMention As System.Windows.Forms.ComboBox
    Friend WithEvents lblMenId As System.Windows.Forms.Label
    Friend WithEvents txtMentionId As System.Windows.Forms.TextBox
    Friend WithEvents chkLocalSound As System.Windows.Forms.CheckBox
    Friend WithEvents btnTestWebhook As RoundedButton
    Friend WithEvents btnSaveConfig As RoundedButton

    Friend WithEvents grpLogs As System.Windows.Forms.GroupBox
    Friend WithEvents lstLogs As System.Windows.Forms.ListBox

    Friend WithEvents notifyIcon1 As System.Windows.Forms.NotifyIcon
    Friend WithEvents contextMenu1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents menuOpen As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents menuExit As System.Windows.Forms.ToolStripMenuItem

    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.picSentinelLogo = New System.Windows.Forms.PictureBox()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.lblSubtitle = New System.Windows.Forms.Label()
        Me.lblStatus = New System.Windows.Forms.Label()
        Me.btnToggleCapture = New RoundedButton()
        Me.pbAudioLevel = New System.Windows.Forms.ProgressBar()
        Me.lblVolume = New System.Windows.Forms.Label()

        ' Perfis e Janelas
        Me.grpProfileBar = New System.Windows.Forms.GroupBox()
        Me.lblProfileLabel = New System.Windows.Forms.Label()
        Me.cmbProfiles = New System.Windows.Forms.ComboBox()
        Me.btnNewProfile = New RoundedButton()
        Me.btnDeleteProfile = New RoundedButton()
        Me.lblCharName = New System.Windows.Forms.Label()
        Me.txtCharName = New System.Windows.Forms.TextBox()
        Me.lblTargetWindow = New System.Windows.Forms.Label()
        Me.cmbTargetWindow = New System.Windows.Forms.ComboBox()
        Me.btnRefreshWindows = New RoundedButton()
        Me.chkFilterProcess = New System.Windows.Forms.CheckBox()

        ' Categorias
        Me.grpGm = New System.Windows.Forms.GroupBox()
        Me.chkGm = New System.Windows.Forms.CheckBox()
        Me.pbGmMatch = New System.Windows.Forms.ProgressBar()
        Me.lblGmMatch = New System.Windows.Forms.Label()
        Me.tbGmThreshold = New System.Windows.Forms.TrackBar()
        Me.lblGmThreshold = New System.Windows.Forms.Label()
        Me.btnPlayGm = New RoundedButton()
        Me.btnRecGm = New RoundedButton()
        Me.btnFileGm = New RoundedButton()

        Me.grpMsgPlayer = New System.Windows.Forms.GroupBox()
        Me.chkMsgPlayer = New System.Windows.Forms.CheckBox()
        Me.pbMsgPlayerMatch = New System.Windows.Forms.ProgressBar()
        Me.lblMsgPlayerMatch = New System.Windows.Forms.Label()
        Me.tbMsgPlayerThreshold = New System.Windows.Forms.TrackBar()
        Me.lblMsgPlayerThreshold = New System.Windows.Forms.Label()
        Me.btnPlayMsgPlayer = New RoundedButton()
        Me.btnRecMsgPlayer = New RoundedButton()
        Me.btnFileMsgPlayer = New RoundedButton()

        Me.grpTeleport = New System.Windows.Forms.GroupBox()
        Me.chkTeleport = New System.Windows.Forms.CheckBox()
        Me.pbTeleportMatch = New System.Windows.Forms.ProgressBar()
        Me.lblTeleportMatch = New System.Windows.Forms.Label()
        Me.tbTeleportThreshold = New System.Windows.Forms.TrackBar()
        Me.lblTeleportThreshold = New System.Windows.Forms.Label()
        Me.btnPlayTeleport = New RoundedButton()
        Me.btnRecTeleport = New RoundedButton()
        Me.btnFileTeleport = New RoundedButton()

        Me.grpPoke = New System.Windows.Forms.GroupBox()
        Me.chkPoke = New System.Windows.Forms.CheckBox()
        Me.pbPokeMatch = New System.Windows.Forms.ProgressBar()
        Me.lblPokeMatch = New System.Windows.Forms.Label()
        Me.tbPokeThreshold = New System.Windows.Forms.TrackBar()
        Me.lblPokeThreshold = New System.Windows.Forms.Label()
        Me.btnPlayPoke = New RoundedButton()
        Me.btnRecPoke = New RoundedButton()
        Me.btnFilePoke = New RoundedButton()

        Me.grpSeta = New System.Windows.Forms.GroupBox()
        Me.chkSeta = New System.Windows.Forms.CheckBox()
        Me.pbSetaMatch = New System.Windows.Forms.ProgressBar()
        Me.lblSetaMatch = New System.Windows.Forms.Label()
        Me.tbSetaThreshold = New System.Windows.Forms.TrackBar()
        Me.lblSetaThreshold = New System.Windows.Forms.Label()
        Me.btnPlaySeta = New RoundedButton()
        Me.btnRecSeta = New RoundedButton()
        Me.btnFileSeta = New RoundedButton()

        ' Painel Lateral
        Me.grpDiscord = New System.Windows.Forms.GroupBox()
        Me.lblWebUrl = New System.Windows.Forms.Label()
        Me.txtWebhook = New System.Windows.Forms.TextBox()
        Me.lblMenType = New System.Windows.Forms.Label()
        Me.cmbMention = New System.Windows.Forms.ComboBox()
        Me.lblMenId = New System.Windows.Forms.Label()
        Me.txtMentionId = New System.Windows.Forms.TextBox()
        Me.chkLocalSound = New System.Windows.Forms.CheckBox()
        Me.btnTestWebhook = New RoundedButton()
        Me.btnSaveConfig = New RoundedButton()

        Me.grpLogs = New System.Windows.Forms.GroupBox()
        Me.lstLogs = New System.Windows.Forms.ListBox()

        Me.notifyIcon1 = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.contextMenu1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.menuOpen = New System.Windows.Forms.ToolStripMenuItem()
        Me.menuExit = New System.Windows.Forms.ToolStripMenuItem()

        Me.SuspendLayout()

        ' Configurações do Form (Paleta Sentinel Stealth Obsidian)
        Me.BackColor = System.Drawing.Color.FromArgb(10, 14, 22)
        Me.ForeColor = System.Drawing.Color.FromArgb(241, 245, 249)
        Me.ClientSize = New System.Drawing.Size(1040, 750)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "SentinelBot - Sistema de Vigilância Acústica Multi-Client"
        Me.Font = New System.Drawing.Font("Segoe UI", 9.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)

        ' Header Superior com Logo do Sentinel
        Me.picSentinelLogo.Location = New System.Drawing.Point(18, 12)
        Me.picSentinelLogo.Size = New System.Drawing.Size(56, 56)
        Me.picSentinelLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.picSentinelLogo.BackColor = System.Drawing.Color.Transparent

        Me.lblTitle.Text = "SENTINEL BOT"
        Me.lblTitle.Font = New System.Drawing.Font("Segoe UI", 16.0F, System.Drawing.FontStyle.Bold)
        Me.lblTitle.ForeColor = System.Drawing.Color.FromArgb(0, 229, 255)
        Me.lblTitle.Location = New System.Drawing.Point(82, 14)
        Me.lblTitle.Size = New System.Drawing.Size(350, 30)

        Me.lblSubtitle.Text = "Vigilância Acústica Multi-Client • Alertas Discord com Nome do Personagem"
        Me.lblSubtitle.ForeColor = System.Drawing.Color.FromArgb(148, 163, 184)
        Me.lblSubtitle.Location = New System.Drawing.Point(84, 44)
        Me.lblSubtitle.Size = New System.Drawing.Size(430, 18)

        Me.lblStatus.Text = "🟢 SENTINELA ATIVO"
        Me.lblStatus.Font = New System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold)
        Me.lblStatus.ForeColor = System.Drawing.Color.FromArgb(0, 255, 194)
        Me.lblStatus.Location = New System.Drawing.Point(520, 18)
        Me.lblStatus.Size = New System.Drawing.Size(210, 22)

        Me.btnToggleCapture.Text = "⏹️ Parar Sentinela"
        Me.btnToggleCapture.Radius = 10
        Me.btnToggleCapture.NormalColor = System.Drawing.Color.FromArgb(220, 38, 38)
        Me.btnToggleCapture.HoverColor = System.Drawing.Color.FromArgb(239, 68, 68)
        Me.btnToggleCapture.BorderColor = System.Drawing.Color.FromArgb(248, 113, 113)
        Me.btnToggleCapture.ForeColor = System.Drawing.Color.White
        Me.btnToggleCapture.Font = New System.Drawing.Font("Segoe UI", 9.0F, System.Drawing.FontStyle.Bold)
        Me.btnToggleCapture.Location = New System.Drawing.Point(520, 42)
        Me.btnToggleCapture.Size = New System.Drawing.Size(160, 30)

        Me.lblVolume.Text = "Volume:"
        Me.lblVolume.ForeColor = System.Drawing.Color.FromArgb(148, 163, 184)
        Me.lblVolume.Location = New System.Drawing.Point(688, 18)
        Me.lblVolume.Size = New System.Drawing.Size(55, 18)

        Me.pbAudioLevel.Location = New System.Drawing.Point(688, 42)
        Me.pbAudioLevel.Size = New System.Drawing.Size(70, 30)
        Me.pbAudioLevel.Value = 0

        ' PAINEL DE PERFIS E SELEÇÃO DE JANELA / VM
        Me.grpProfileBar.Text = "  👤 PERFIL DO PERSONAGEM & JANELA / VM ALVO  "
        Me.grpProfileBar.ForeColor = System.Drawing.Color.FromArgb(0, 229, 255)
        Me.grpProfileBar.BackColor = System.Drawing.Color.FromArgb(16, 23, 36)
        Me.grpProfileBar.Font = New System.Drawing.Font("Segoe UI", 9.0F, System.Drawing.FontStyle.Bold)
        Me.grpProfileBar.Location = New System.Drawing.Point(20, 78)
        Me.grpProfileBar.Size = New System.Drawing.Size(738, 95)

        Me.lblProfileLabel.Text = "Perfil:"
        Me.lblProfileLabel.ForeColor = System.Drawing.Color.FromArgb(148, 163, 184)
        Me.lblProfileLabel.Location = New System.Drawing.Point(12, 25)
        Me.lblProfileLabel.Size = New System.Drawing.Size(42, 20)

        Me.cmbProfiles.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbProfiles.BackColor = System.Drawing.Color.FromArgb(9, 13, 22)
        Me.cmbProfiles.ForeColor = System.Drawing.Color.FromArgb(241, 245, 249)
        Me.cmbProfiles.Location = New System.Drawing.Point(55, 22)
        Me.cmbProfiles.Size = New System.Drawing.Size(160, 23)

        Me.btnNewProfile.Text = "➕ Novo"
        Me.btnNewProfile.Radius = 8
        Me.btnNewProfile.NormalColor = System.Drawing.Color.FromArgb(14, 28, 48)
        Me.btnNewProfile.BorderColor = System.Drawing.Color.FromArgb(0, 229, 255)
        Me.btnNewProfile.HoverColor = System.Drawing.Color.FromArgb(20, 42, 72)
        Me.btnNewProfile.ForeColor = System.Drawing.Color.FromArgb(224, 242, 254)
        Me.btnNewProfile.Location = New System.Drawing.Point(220, 21)
        Me.btnNewProfile.Size = New System.Drawing.Size(72, 26)

        Me.btnDeleteProfile.Text = "🗑️ Excluir"
        Me.btnDeleteProfile.Radius = 8
        Me.btnDeleteProfile.NormalColor = System.Drawing.Color.FromArgb(45, 18, 26)
        Me.btnDeleteProfile.BorderColor = System.Drawing.Color.FromArgb(244, 63, 94)
        Me.btnDeleteProfile.HoverColor = System.Drawing.Color.FromArgb(65, 24, 36)
        Me.btnDeleteProfile.ForeColor = System.Drawing.Color.FromArgb(254, 205, 211)
        Me.btnDeleteProfile.Location = New System.Drawing.Point(296, 21)
        Me.btnDeleteProfile.Size = New System.Drawing.Size(74, 26)

        Me.lblCharName.Text = "Personagem (Nome no Discord):"
        Me.lblCharName.ForeColor = System.Drawing.Color.FromArgb(148, 163, 184)
        Me.lblCharName.Location = New System.Drawing.Point(380, 25)
        Me.lblCharName.Size = New System.Drawing.Size(175, 20)

        Me.txtCharName.BackColor = System.Drawing.Color.FromArgb(9, 13, 22)
        Me.txtCharName.ForeColor = System.Drawing.Color.FromArgb(241, 245, 249)
        Me.txtCharName.Location = New System.Drawing.Point(560, 22)
        Me.txtCharName.Size = New System.Drawing.Size(160, 23)

        Me.lblTargetWindow.Text = "Janela / VM:"
        Me.lblTargetWindow.ForeColor = System.Drawing.Color.FromArgb(148, 163, 184)
        Me.lblTargetWindow.Location = New System.Drawing.Point(12, 58)
        Me.lblTargetWindow.Size = New System.Drawing.Size(75, 20)

        Me.cmbTargetWindow.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbTargetWindow.BackColor = System.Drawing.Color.FromArgb(9, 13, 22)
        Me.cmbTargetWindow.ForeColor = System.Drawing.Color.FromArgb(241, 245, 249)
        Me.cmbTargetWindow.Location = New System.Drawing.Point(90, 55)
        Me.cmbTargetWindow.Size = New System.Drawing.Size(350, 23)

        Me.btnRefreshWindows.Text = "🔄 Atualizar Janelas"
        Me.btnRefreshWindows.Radius = 8
        Me.btnRefreshWindows.NormalColor = System.Drawing.Color.FromArgb(14, 24, 40)
        Me.btnRefreshWindows.BorderColor = System.Drawing.Color.FromArgb(56, 189, 248)
        Me.btnRefreshWindows.HoverColor = System.Drawing.Color.FromArgb(20, 36, 60)
        Me.btnRefreshWindows.ForeColor = System.Drawing.Color.FromArgb(224, 242, 254)
        Me.btnRefreshWindows.Location = New System.Drawing.Point(445, 54)
        Me.btnRefreshWindows.Size = New System.Drawing.Size(125, 26)

        Me.chkFilterProcess.Text = "Isolar áudio da janela"
        Me.chkFilterProcess.ForeColor = System.Drawing.Color.White
        Me.chkFilterProcess.Location = New System.Drawing.Point(578, 56)
        Me.chkFilterProcess.Size = New System.Drawing.Size(145, 22)

        Me.grpProfileBar.Controls.AddRange(New System.Windows.Forms.Control() { _
            Me.lblProfileLabel, Me.cmbProfiles, Me.btnNewProfile, Me.btnDeleteProfile, _
            Me.lblCharName, Me.txtCharName, Me.lblTargetWindow, Me.cmbTargetWindow, _
            Me.btnRefreshWindows, Me.chkFilterProcess _
        })

        ' Configuração de Grupo de Categoria (Y começa em 180)
        SetupCategoryGroup(Me.grpGm, "🚨 MENSAGEM GM", 180, Me.chkGm, Me.pbGmMatch, Me.lblGmMatch, Me.tbGmThreshold, Me.lblGmThreshold, Me.btnPlayGm, Me.btnRecGm, Me.btnFileGm)
        SetupCategoryGroup(Me.grpMsgPlayer, "💬 MENSAGEM DE JOGADOR", 290, Me.chkMsgPlayer, Me.pbMsgPlayerMatch, Me.lblMsgPlayerMatch, Me.tbMsgPlayerThreshold, Me.lblMsgPlayerThreshold, Me.btnPlayMsgPlayer, Me.btnRecMsgPlayer, Me.btnFileMsgPlayer)
        SetupCategoryGroup(Me.grpTeleport, "⚡ TELEPORT", 400, Me.chkTeleport, Me.pbTeleportMatch, Me.lblTeleportMatch, Me.tbTeleportThreshold, Me.lblTeleportThreshold, Me.btnPlayTeleport, Me.btnRecTeleport, Me.btnFileTeleport)
        SetupCategoryGroup(Me.grpPoke, "🐉 POKEMON FORA DA HUNT", 510, Me.chkPoke, Me.pbPokeMatch, Me.lblPokeMatch, Me.tbPokeThreshold, Me.lblPokeThreshold, Me.btnPlayPoke, Me.btnRecPoke, Me.btnFilePoke)
        SetupCategoryGroup(Me.grpSeta, "🎯 SOM DE SETA", 620, Me.chkSeta, Me.pbSetaMatch, Me.lblSetaMatch, Me.tbSetaThreshold, Me.lblSetaThreshold, Me.btnPlaySeta, Me.btnRecSeta, Me.btnFileSeta)

        ' Painel Lateral: Discord
        Me.grpDiscord.Text = "  🌐 NOTIFICAÇÕES DISCORD DO PERFIL  "
        Me.grpDiscord.ForeColor = System.Drawing.Color.FromArgb(0, 229, 255)
        Me.grpDiscord.BackColor = System.Drawing.Color.FromArgb(16, 23, 36)
        Me.grpDiscord.Font = New System.Drawing.Font("Segoe UI", 9.0F, System.Drawing.FontStyle.Bold)
        Me.grpDiscord.Location = New System.Drawing.Point(768, 12)
        Me.grpDiscord.Size = New System.Drawing.Size(252, 335)

        Me.lblWebUrl.Text = "URL do Webhook:"
        Me.lblWebUrl.ForeColor = System.Drawing.Color.FromArgb(148, 163, 184)
        Me.lblWebUrl.Location = New System.Drawing.Point(12, 25)
        Me.lblWebUrl.Size = New System.Drawing.Size(225, 18)

        Me.txtWebhook.Location = New System.Drawing.Point(12, 45)
        Me.txtWebhook.Size = New System.Drawing.Size(228, 23)
        Me.txtWebhook.PasswordChar = "*"c
        Me.txtWebhook.BackColor = System.Drawing.Color.FromArgb(9, 13, 22)
        Me.txtWebhook.ForeColor = System.Drawing.Color.FromArgb(241, 245, 249)

        Me.lblMenType.Text = "Menção no Alerta:"
        Me.lblMenType.ForeColor = System.Drawing.Color.FromArgb(148, 163, 184)
        Me.lblMenType.Location = New System.Drawing.Point(12, 75)
        Me.lblMenType.Size = New System.Drawing.Size(225, 18)

        Me.cmbMention.Items.AddRange(New Object() {"none", "everyone", "here", "role", "user"})
        Me.cmbMention.Location = New System.Drawing.Point(12, 95)
        Me.cmbMention.Size = New System.Drawing.Size(228, 23)
        Me.cmbMention.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbMention.BackColor = System.Drawing.Color.FromArgb(9, 13, 22)
        Me.cmbMention.ForeColor = System.Drawing.Color.FromArgb(241, 245, 249)

        Me.lblMenId.Text = "ID do Cargo / Usuário:"
        Me.lblMenId.ForeColor = System.Drawing.Color.FromArgb(148, 163, 184)
        Me.lblMenId.Location = New System.Drawing.Point(12, 125)
        Me.lblMenId.Size = New System.Drawing.Size(225, 18)

        Me.txtMentionId.Location = New System.Drawing.Point(12, 145)
        Me.txtMentionId.Size = New System.Drawing.Size(228, 23)
        Me.txtMentionId.BackColor = System.Drawing.Color.FromArgb(9, 13, 22)
        Me.txtMentionId.ForeColor = System.Drawing.Color.FromArgb(241, 245, 249)

        Me.chkLocalSound.Text = "Tocar som local no PC"
        Me.chkLocalSound.ForeColor = System.Drawing.Color.White
        Me.chkLocalSound.Location = New System.Drawing.Point(12, 180)
        Me.chkLocalSound.Size = New System.Drawing.Size(228, 24)
        Me.chkLocalSound.Checked = True

        Me.btnSaveConfig.Text = "💾 Salvar Perfil & Configs"
        Me.btnSaveConfig.Radius = 10
        Me.btnSaveConfig.NormalColor = System.Drawing.Color.FromArgb(0, 180, 230)
        Me.btnSaveConfig.HoverColor = System.Drawing.Color.FromArgb(0, 220, 255)
        Me.btnSaveConfig.BorderColor = System.Drawing.Color.FromArgb(0, 255, 255)
        Me.btnSaveConfig.ForeColor = System.Drawing.Color.FromArgb(8, 12, 22)
        Me.btnSaveConfig.Font = New System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold)
        Me.btnSaveConfig.Location = New System.Drawing.Point(12, 222)
        Me.btnSaveConfig.Size = New System.Drawing.Size(228, 36)

        Me.btnTestWebhook.Text = "🔔 Testar Webhook"
        Me.btnTestWebhook.Radius = 10
        Me.btnTestWebhook.NormalColor = System.Drawing.Color.FromArgb(16, 28, 48)
        Me.btnTestWebhook.HoverColor = System.Drawing.Color.FromArgb(24, 42, 72)
        Me.btnTestWebhook.BorderColor = System.Drawing.Color.FromArgb(0, 229, 255)
        Me.btnTestWebhook.ForeColor = System.Drawing.Color.FromArgb(224, 242, 254)
        Me.btnTestWebhook.Font = New System.Drawing.Font("Segoe UI", 9.0F, System.Drawing.FontStyle.Bold)
        Me.btnTestWebhook.Location = New System.Drawing.Point(12, 268)
        Me.btnTestWebhook.Size = New System.Drawing.Size(228, 36)

        Me.grpDiscord.Controls.AddRange(New System.Windows.Forms.Control() {Me.lblWebUrl, Me.txtWebhook, Me.lblMenType, Me.cmbMention, Me.lblMenId, Me.txtMentionId, Me.chkLocalSound, Me.btnSaveConfig, Me.btnTestWebhook})

        ' Painel de Logs
        Me.grpLogs.Text = "  📋 HISTÓRICO DE ALERTAS  "
        Me.grpLogs.ForeColor = System.Drawing.Color.FromArgb(0, 229, 255)
        Me.grpLogs.BackColor = System.Drawing.Color.FromArgb(16, 23, 36)
        Me.grpLogs.Font = New System.Drawing.Font("Segoe UI", 9.0F, System.Drawing.FontStyle.Bold)
        Me.grpLogs.Location = New System.Drawing.Point(768, 355)
        Me.grpLogs.Size = New System.Drawing.Size(252, 370)

        Me.lstLogs.Location = New System.Drawing.Point(12, 25)
        Me.lstLogs.Size = New System.Drawing.Size(228, 330)
        Me.lstLogs.BackColor = System.Drawing.Color.FromArgb(9, 13, 22)
        Me.lstLogs.ForeColor = System.Drawing.Color.FromArgb(200, 220, 240)
        Me.lstLogs.Font = New System.Drawing.Font("Consolas", 8.5F)
        Me.lstLogs.BorderStyle = System.Windows.Forms.BorderStyle.None

        Me.grpLogs.Controls.Add(Me.lstLogs)

        ' System Tray NotifyIcon
        Me.notifyIcon1.Text = "SentinelBot"
        Me.notifyIcon1.Icon = System.Drawing.SystemIcons.Shield
        Me.notifyIcon1.ContextMenuStrip = Me.contextMenu1

        Me.menuOpen.Text = "Abrir Janela"
        Me.menuExit.Text = "Sair"
        Me.contextMenu1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.menuOpen, Me.menuExit})

        ' Adiciona controles ao Form
        Me.Controls.AddRange(New System.Windows.Forms.Control() { _
            Me.picSentinelLogo, Me.lblTitle, Me.lblSubtitle, Me.lblStatus, Me.btnToggleCapture, Me.lblVolume, Me.pbAudioLevel, _
            Me.grpProfileBar, _
            Me.grpGm, Me.grpMsgPlayer, Me.grpTeleport, Me.grpPoke, Me.grpSeta, _
            Me.grpDiscord, Me.grpLogs _
        })

        Me.ResumeLayout(False)
    End Sub

    Private Sub SetupCategoryGroup(grp As System.Windows.Forms.GroupBox, title As String, top As Integer, chk As System.Windows.Forms.CheckBox, pbMatch As System.Windows.Forms.ProgressBar, lblMatch As System.Windows.Forms.Label, tb As System.Windows.Forms.TrackBar, lblThresh As System.Windows.Forms.Label, btnPlay As RoundedButton, btnRec As RoundedButton, btnFile As RoundedButton)
        grp.Text = "  " & title & "  "
        grp.ForeColor = System.Drawing.Color.FromArgb(0, 229, 255)
        grp.BackColor = System.Drawing.Color.FromArgb(16, 23, 36)
        grp.Font = New System.Drawing.Font("Segoe UI", 9.0F, System.Drawing.FontStyle.Bold)
        grp.Location = New System.Drawing.Point(20, top)
        grp.Size = New System.Drawing.Size(738, 102)

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
        pbMatch.Size = New System.Drawing.Size(130, 18)

        lblMatch.Text = "0%"
        lblMatch.ForeColor = System.Drawing.Color.White
        lblMatch.Location = New System.Drawing.Point(312, 28)
        lblMatch.Size = New System.Drawing.Size(45, 18)

        ' Slider de Sensibilidade
        Dim lblSens As New System.Windows.Forms.Label With {
            .Text = "Sensibilidade:",
            .ForeColor = System.Drawing.Color.FromArgb(148, 163, 184),
            .Location = New System.Drawing.Point(90, 58),
            .Size = New System.Drawing.Size(80, 18),
            .Font = New System.Drawing.Font("Segoe UI", 8.5F)
        }
        tb.Minimum = 50
        tb.Maximum = 98
        tb.Value = 80
        tb.TickStyle = System.Windows.Forms.TickStyle.None
        tb.Location = New System.Drawing.Point(175, 54)
        tb.Size = New System.Drawing.Size(130, 25)

        lblThresh.Text = "80%"
        lblThresh.ForeColor = System.Drawing.Color.White
        lblThresh.Location = New System.Drawing.Point(312, 58)
        lblThresh.Size = New System.Drawing.Size(45, 18)

        ' Botões de Ação com Arredondamento
        btnPlay.Text = "▶️ Ouvir"
        btnPlay.Radius = 8
        btnPlay.NormalColor = System.Drawing.Color.FromArgb(14, 24, 40)
        btnPlay.BorderColor = System.Drawing.Color.FromArgb(56, 189, 248)
        btnPlay.HoverColor = System.Drawing.Color.FromArgb(20, 36, 60)
        btnPlay.ForeColor = System.Drawing.Color.FromArgb(224, 242, 254)
        btnPlay.Location = New System.Drawing.Point(375, 34)
        btnPlay.Size = New System.Drawing.Size(105, 34)

        btnRec.Text = "🎙️ Gravar"
        btnRec.Radius = 8
        btnRec.NormalColor = System.Drawing.Color.FromArgb(36, 16, 26)
        btnRec.BorderColor = System.Drawing.Color.FromArgb(244, 63, 94)
        btnRec.HoverColor = System.Drawing.Color.FromArgb(54, 22, 38)
        btnRec.ForeColor = System.Drawing.Color.FromArgb(254, 205, 211)
        btnRec.Location = New System.Drawing.Point(490, 34)
        btnRec.Size = New System.Drawing.Size(110, 34)

        btnFile.Text = "📁 Arquivo"
        btnFile.Radius = 8
        btnFile.NormalColor = System.Drawing.Color.FromArgb(14, 24, 40)
        btnFile.BorderColor = System.Drawing.Color.FromArgb(0, 229, 255)
        btnFile.HoverColor = System.Drawing.Color.FromArgb(20, 36, 60)
        btnFile.ForeColor = System.Drawing.Color.FromArgb(224, 242, 254)
        btnFile.Location = New System.Drawing.Point(610, 34)
        btnFile.Size = New System.Drawing.Size(105, 34)

        grp.Controls.AddRange(New System.Windows.Forms.Control() {chk, lblRadar, pbMatch, lblMatch, lblSens, tb, lblThresh, btnPlay, btnRec, btnFile})
    End Sub
End Class
