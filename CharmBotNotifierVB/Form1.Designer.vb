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

    ' Estúdio de Gravação e Classificação
    Friend WithEvents grpStudio As System.Windows.Forms.GroupBox
    Friend WithEvents btnRecordNew As RoundedButton
    Friend WithEvents lblRecStatus As System.Windows.Forms.Label
    Friend WithEvents btnImportFile As RoundedButton
    Friend WithEvents lblClassify As System.Windows.Forms.Label
    Friend WithEvents cmbClassify As System.Windows.Forms.ComboBox
    Friend WithEvents txtCustomName As System.Windows.Forms.TextBox
    Friend WithEvents btnPreviewRecorded As RoundedButton
    Friend WithEvents btnSaveNewTrigger As RoundedButton

    ' Área Dinâmica de Gatilhos Configurados
    Friend WithEvents grpTriggersList As System.Windows.Forms.GroupBox
    Friend WithEvents lblEmptyNotice As System.Windows.Forms.Label
    Friend WithEvents pnlTriggersContainer As System.Windows.Forms.Panel

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

        ' Estúdio de Gravação e Classificação
        Me.grpStudio = New System.Windows.Forms.GroupBox()
        Me.btnRecordNew = New RoundedButton()
        Me.lblRecStatus = New System.Windows.Forms.Label()
        Me.btnImportFile = New RoundedButton()
        Me.lblClassify = New System.Windows.Forms.Label()
        Me.cmbClassify = New System.Windows.Forms.ComboBox()
        Me.txtCustomName = New System.Windows.Forms.TextBox()
        Me.btnPreviewRecorded = New RoundedButton()
        Me.btnSaveNewTrigger = New RoundedButton()

        ' Área Dinâmica de Gatilhos Configurados
        Me.grpTriggersList = New System.Windows.Forms.GroupBox()
        Me.lblEmptyNotice = New System.Windows.Forms.Label()
        Me.pnlTriggersContainer = New System.Windows.Forms.Panel()

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

        ' Configurações do Form (Paleta Sentinel Stealth)
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
        Me.grpProfileBar.Location = New System.Drawing.Point(20, 76)
        Me.grpProfileBar.Size = New System.Drawing.Size(738, 88)

        Me.lblProfileLabel.Text = "Perfil:"
        Me.lblProfileLabel.ForeColor = System.Drawing.Color.FromArgb(148, 163, 184)
        Me.lblProfileLabel.Location = New System.Drawing.Point(12, 23)
        Me.lblProfileLabel.Size = New System.Drawing.Size(42, 20)

        Me.cmbProfiles.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbProfiles.BackColor = System.Drawing.Color.FromArgb(9, 13, 22)
        Me.cmbProfiles.ForeColor = System.Drawing.Color.FromArgb(241, 245, 249)
        Me.cmbProfiles.Location = New System.Drawing.Point(55, 20)
        Me.cmbProfiles.Size = New System.Drawing.Size(155, 23)

        Me.btnNewProfile.Text = "➕ Novo"
        Me.btnNewProfile.Radius = 8
        Me.btnNewProfile.NormalColor = System.Drawing.Color.FromArgb(14, 28, 48)
        Me.btnNewProfile.BorderColor = System.Drawing.Color.FromArgb(0, 229, 255)
        Me.btnNewProfile.HoverColor = System.Drawing.Color.FromArgb(20, 42, 72)
        Me.btnNewProfile.ForeColor = System.Drawing.Color.FromArgb(224, 242, 254)
        Me.btnNewProfile.Location = New System.Drawing.Point(215, 19)
        Me.btnNewProfile.Size = New System.Drawing.Size(72, 26)

        Me.btnDeleteProfile.Text = "🗑️ Excluir"
        Me.btnDeleteProfile.Radius = 8
        Me.btnDeleteProfile.NormalColor = System.Drawing.Color.FromArgb(45, 18, 26)
        Me.btnDeleteProfile.BorderColor = System.Drawing.Color.FromArgb(244, 63, 94)
        Me.btnDeleteProfile.HoverColor = System.Drawing.Color.FromArgb(65, 24, 36)
        Me.btnDeleteProfile.ForeColor = System.Drawing.Color.FromArgb(254, 205, 211)
        Me.btnDeleteProfile.Location = New System.Drawing.Point(292, 19)
        Me.btnDeleteProfile.Size = New System.Drawing.Size(74, 26)

        Me.lblCharName.Text = "Nome no Discord:"
        Me.lblCharName.ForeColor = System.Drawing.Color.FromArgb(148, 163, 184)
        Me.lblCharName.Location = New System.Drawing.Point(375, 23)
        Me.lblCharName.Size = New System.Drawing.Size(110, 20)

        Me.txtCharName.BackColor = System.Drawing.Color.FromArgb(9, 13, 22)
        Me.txtCharName.ForeColor = System.Drawing.Color.FromArgb(241, 245, 249)
        Me.txtCharName.Location = New System.Drawing.Point(490, 20)
        Me.txtCharName.Size = New System.Drawing.Size(235, 23)

        Me.lblTargetWindow.Text = "Janela / VM:"
        Me.lblTargetWindow.ForeColor = System.Drawing.Color.FromArgb(148, 163, 184)
        Me.lblTargetWindow.Location = New System.Drawing.Point(12, 54)
        Me.lblTargetWindow.Size = New System.Drawing.Size(75, 20)

        Me.cmbTargetWindow.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbTargetWindow.BackColor = System.Drawing.Color.FromArgb(9, 13, 22)
        Me.cmbTargetWindow.ForeColor = System.Drawing.Color.FromArgb(241, 245, 249)
        Me.cmbTargetWindow.Location = New System.Drawing.Point(90, 51)
        Me.cmbTargetWindow.Size = New System.Drawing.Size(350, 23)

        Me.btnRefreshWindows.Text = "🔄 Atualizar"
        Me.btnRefreshWindows.Radius = 8
        Me.btnRefreshWindows.NormalColor = System.Drawing.Color.FromArgb(14, 24, 40)
        Me.btnRefreshWindows.BorderColor = System.Drawing.Color.FromArgb(56, 189, 248)
        Me.btnRefreshWindows.HoverColor = System.Drawing.Color.FromArgb(20, 36, 60)
        Me.btnRefreshWindows.ForeColor = System.Drawing.Color.FromArgb(224, 242, 254)
        Me.btnRefreshWindows.Location = New System.Drawing.Point(445, 50)
        Me.btnRefreshWindows.Size = New System.Drawing.Size(100, 26)

        Me.chkFilterProcess.Text = "Isolar áudio da janela"
        Me.chkFilterProcess.ForeColor = System.Drawing.Color.White
        Me.chkFilterProcess.Location = New System.Drawing.Point(555, 52)
        Me.chkFilterProcess.Size = New System.Drawing.Size(168, 22)

        Me.grpProfileBar.Controls.AddRange(New System.Windows.Forms.Control() { _
            Me.lblProfileLabel, Me.cmbProfiles, Me.btnNewProfile, Me.btnDeleteProfile, _
            Me.lblCharName, Me.txtCharName, Me.lblTargetWindow, Me.cmbTargetWindow, _
            Me.btnRefreshWindows, Me.chkFilterProcess _
        })

        ' ESTÚDIO DE GRAVAÇÃO E CLASSIFICAÇÃO DE ÁUDIO
        Me.grpStudio.Text = "  🎙️ GRAVAÇÃO & CLASSIFICAÇÃO DE NOVO SOM DO JOGO  "
        Me.grpStudio.ForeColor = System.Drawing.Color.FromArgb(0, 229, 255)
        Me.grpStudio.BackColor = System.Drawing.Color.FromArgb(16, 23, 36)
        Me.grpStudio.Font = New System.Drawing.Font("Segoe UI", 9.0F, System.Drawing.FontStyle.Bold)
        Me.grpStudio.Location = New System.Drawing.Point(20, 168)
        Me.grpStudio.Size = New System.Drawing.Size(738, 140)

        ' Linha 1: Gravação
        Me.btnRecordNew.Text = "🎙️ Gravar Som do Jogo"
        Me.btnRecordNew.Radius = 10
        Me.btnRecordNew.NormalColor = System.Drawing.Color.FromArgb(36, 16, 26)
        Me.btnRecordNew.BorderColor = System.Drawing.Color.FromArgb(244, 63, 94)
        Me.btnRecordNew.HoverColor = System.Drawing.Color.FromArgb(64, 22, 34)
        Me.btnRecordNew.ForeColor = System.Drawing.Color.FromArgb(254, 205, 211)
        Me.btnRecordNew.Font = New System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold)
        Me.btnRecordNew.Location = New System.Drawing.Point(12, 25)
        Me.btnRecordNew.Size = New System.Drawing.Size(200, 36)

        Me.lblRecStatus.Text = "Clique em 'Gravar', faça o jogo tocar o som e clique em 'Concluir'."
        Me.lblRecStatus.ForeColor = System.Drawing.Color.FromArgb(148, 163, 184)
        Me.lblRecStatus.Font = New System.Drawing.Font("Segoe UI", 8.5F)
        Me.lblRecStatus.Location = New System.Drawing.Point(220, 26)
        Me.lblRecStatus.Size = New System.Drawing.Size(370, 34)

        Me.btnImportFile.Text = "📁 Importar WAV"
        Me.btnImportFile.Radius = 8
        Me.btnImportFile.NormalColor = System.Drawing.Color.FromArgb(14, 24, 40)
        Me.btnImportFile.BorderColor = System.Drawing.Color.FromArgb(56, 189, 248)
        Me.btnImportFile.HoverColor = System.Drawing.Color.FromArgb(20, 36, 60)
        Me.btnImportFile.ForeColor = System.Drawing.Color.FromArgb(224, 242, 254)
        Me.btnImportFile.Location = New System.Drawing.Point(600, 26)
        Me.btnImportFile.Size = New System.Drawing.Size(125, 34)

        ' Linha 2: Classificação
        Me.lblClassify.Text = "Classificar som como:"
        Me.lblClassify.ForeColor = System.Drawing.Color.FromArgb(148, 163, 184)
        Me.lblClassify.Font = New System.Drawing.Font("Segoe UI", 8.5F)
        Me.lblClassify.Location = New System.Drawing.Point(12, 72)
        Me.lblClassify.Size = New System.Drawing.Size(140, 18)

        Me.cmbClassify.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbClassify.BackColor = System.Drawing.Color.FromArgb(9, 13, 22)
        Me.cmbClassify.ForeColor = System.Drawing.Color.FromArgb(241, 245, 249)
        Me.cmbClassify.Items.AddRange(New Object() { _
            "🚨 MENSAGEM GM", _
            "💬 MENSAGEM DE JOGADOR", _
            "⚡ TELEPORT", _
            "🐉 POKEMON FORA DA HUNT", _
            "🎯 SOM DE SETA", _
            "✏️ Outro (Personalizado)" _
        })
        Me.cmbClassify.SelectedIndex = 0
        Me.cmbClassify.Location = New System.Drawing.Point(12, 94)
        Me.cmbClassify.Size = New System.Drawing.Size(200, 23)

        Me.txtCustomName.BackColor = System.Drawing.Color.FromArgb(9, 13, 22)
        Me.txtCustomName.ForeColor = System.Drawing.Color.FromArgb(241, 245, 249)
        Me.txtCustomName.Location = New System.Drawing.Point(220, 94)
        Me.txtCustomName.Size = New System.Drawing.Size(160, 23)
        Me.txtCustomName.Visible = False

        Me.btnPreviewRecorded.Text = "▶️ Ouvir Amostra"
        Me.btnPreviewRecorded.Radius = 8
        Me.btnPreviewRecorded.NormalColor = System.Drawing.Color.FromArgb(14, 24, 40)
        Me.btnPreviewRecorded.BorderColor = System.Drawing.Color.FromArgb(0, 229, 255)
        Me.btnPreviewRecorded.HoverColor = System.Drawing.Color.FromArgb(20, 36, 60)
        Me.btnPreviewRecorded.ForeColor = System.Drawing.Color.FromArgb(224, 242, 254)
        Me.btnPreviewRecorded.Enabled = False
        Me.btnPreviewRecorded.Location = New System.Drawing.Point(390, 90)
        Me.btnPreviewRecorded.Size = New System.Drawing.Size(130, 32)

        Me.btnSaveNewTrigger.Text = "➕ Salvar e Ativar Alerta"
        Me.btnSaveNewTrigger.Radius = 10
        Me.btnSaveNewTrigger.NormalColor = System.Drawing.Color.FromArgb(0, 180, 230)
        Me.btnSaveNewTrigger.HoverColor = System.Drawing.Color.FromArgb(0, 220, 255)
        Me.btnSaveNewTrigger.BorderColor = System.Drawing.Color.FromArgb(0, 255, 255)
        Me.btnSaveNewTrigger.ForeColor = System.Drawing.Color.FromArgb(8, 12, 22)
        Me.btnSaveNewTrigger.Font = New System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold)
        Me.btnSaveNewTrigger.Enabled = False
        Me.btnSaveNewTrigger.Location = New System.Drawing.Point(530, 88)
        Me.btnSaveNewTrigger.Size = New System.Drawing.Size(195, 36)

        Me.grpStudio.Controls.AddRange(New System.Windows.Forms.Control() { _
            Me.btnRecordNew, Me.lblRecStatus, Me.btnImportFile, _
            Me.lblClassify, Me.cmbClassify, Me.txtCustomName, Me.btnPreviewRecorded, Me.btnSaveNewTrigger _
        })

        ' ÁREA DINÂMICA DE GATILHOS ATIVOS
        Me.grpTriggersList.Text = "  🛡️ GATILHOS ATIVOS NESTE PERFIL (0)  "
        Me.grpTriggersList.ForeColor = System.Drawing.Color.FromArgb(0, 229, 255)
        Me.grpTriggersList.BackColor = System.Drawing.Color.FromArgb(16, 23, 36)
        Me.grpTriggersList.Font = New System.Drawing.Font("Segoe UI", 9.0F, System.Drawing.FontStyle.Bold)
        Me.grpTriggersList.Location = New System.Drawing.Point(20, 314)
        Me.grpTriggersList.Size = New System.Drawing.Size(738, 412)

        Me.lblEmptyNotice.Text = "Nenhum gatilho configurado para este perfil." & System.Environment.NewLine & "Use o estúdio de gravação acima para gravar um som do seu jogo e ativá-lo aqui!"
        Me.lblEmptyNotice.ForeColor = System.Drawing.Color.FromArgb(148, 163, 184)
        Me.lblEmptyNotice.Font = New System.Drawing.Font("Segoe UI", 10.0F, System.Drawing.FontStyle.Regular)
        Me.lblEmptyNotice.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.lblEmptyNotice.Location = New System.Drawing.Point(20, 150)
        Me.lblEmptyNotice.Size = New System.Drawing.Size(698, 60)
        Me.lblEmptyNotice.Visible = True

        Me.pnlTriggersContainer.Location = New System.Drawing.Point(10, 24)
        Me.pnlTriggersContainer.Size = New System.Drawing.Size(718, 376)
        Me.pnlTriggersContainer.AutoScroll = True
        Me.pnlTriggersContainer.BackColor = System.Drawing.Color.Transparent

        Me.grpTriggersList.Controls.AddRange(New System.Windows.Forms.Control() {Me.lblEmptyNotice, Me.pnlTriggersContainer})

        ' PAINEL LATERAL: DISCORD
        Me.grpDiscord.Text = "  🌐 NOTIFICAÇÕES DISCORD  "
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

        ' PAINEL DE LOGS
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
            Me.grpStudio, _
            Me.grpTriggersList, _
            Me.grpDiscord, Me.grpLogs _
        })

        Me.ResumeLayout(False)
    End Sub
End Class
