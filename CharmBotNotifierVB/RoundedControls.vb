Imports System
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Windows.Forms

Public Class RoundedButton
    Inherits Button

    Public Property Radius As Integer = 10
    Public Property NormalColor As Color = Color.FromArgb(16, 23, 38)
    Public Property HoverColor As Color = Color.FromArgb(24, 35, 56)
    Public Property PressedColor As Color = Color.FromArgb(10, 16, 28)
    Public Property BorderColor As Color = Color.FromArgb(0, 229, 255)
    Public Property BorderThickness As Single = 1.0F

    Private isHovered As Boolean = False
    Private isPressed As Boolean = False

    Public Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or ControlStyles.OptimizedDoubleBuffer Or ControlStyles.ResizeRedraw, True)
        FlatStyle = FlatStyle.Flat
        FlatAppearance.BorderSize = 0
        Cursor = Cursors.Hand
        ForeColor = Color.FromArgb(240, 246, 252)
        Font = New Font("Segoe UI", 9.0F, FontStyle.Bold)
        BackColor = Color.Transparent
    End Sub

    Protected Overrides Sub OnMouseEnter(e As EventArgs)
        isHovered = True
        Invalidate()
        MyBase.OnMouseEnter(e)
    End Sub

    Protected Overrides Sub OnMouseLeave(e As EventArgs)
        isHovered = False
        Invalidate()
        MyBase.OnMouseLeave(e)
    End Sub

    Protected Overrides Sub OnMouseDown(mevent As MouseEventArgs)
        isPressed = True
        Invalidate()
        MyBase.OnMouseDown(mevent)
    End Sub

    Protected Overrides Sub OnMouseUp(mevent As MouseEventArgs)
        isPressed = False
        Invalidate()
        MyBase.OnMouseUp(mevent)
    End Sub

    Protected Overrides Sub OnPaint(pevent As PaintEventArgs)
        Dim g = pevent.Graphics
        g.SmoothingMode = SmoothingMode.AntiAlias
        g.PixelOffsetMode = PixelOffsetMode.HighQuality
        g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit

        Dim rect = New Rectangle(0, 0, Width - 1, Height - 1)
        Using path = GetRoundedRectangle(rect, Radius)
            ' Cor de fundo
            Dim bg = NormalColor
            If Not Enabled Then
                bg = Color.FromArgb(20, 26, 38)
            ElseIf isPressed Then
                bg = PressedColor
            ElseIf isHovered Then
                bg = HoverColor
            End If

            Using br As New SolidBrush(bg)
                g.FillPath(br, path)
            End Using

            ' Borda com brilho Sentinel Neon
            If BorderThickness > 0 Then
                Dim bColor = BorderColor
                If Not Enabled Then
                    bColor = Color.FromArgb(40, 50, 68)
                ElseIf isHovered Then
                    bColor = Color.FromArgb(0, 240, 255)
                End If

                Using pen As New Pen(bColor, BorderThickness)
                    g.DrawPath(pen, path)
                End Using
            End If

            ' Desenha texto
            Dim flags = TextFormatFlags.HorizontalCenter Or TextFormatFlags.VerticalCenter Or TextFormatFlags.WordBreak
            Dim textColor = If(Enabled, ForeColor, Color.FromArgb(100, 115, 135))
            TextRenderer.DrawText(g, Text, Font, rect, textColor, flags)
        End Using
    End Sub

    Private Function GetRoundedRectangle(rect As Rectangle, r As Integer) As GraphicsPath
        Dim path As New GraphicsPath()
        Dim d = r * 2
        If d > rect.Width Then d = rect.Width
        If d > rect.Height Then d = rect.Height
        If d <= 0 Then
            path.AddRectangle(rect)
            Return path
        End If

        path.AddArc(rect.X, rect.Y, d, d, 180, 90)
        path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90)
        path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90)
        path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90)
        path.CloseFigure()
        Return path
    End Function
End Class

Public Class RoundedPanel
    Inherits Panel

    Public Property Radius As Integer = 12
    Public Property BorderColor As Color = Color.FromArgb(26, 36, 54)
    Public Property BorderThickness As Single = 1.0F
    Public Property HeaderGlow As Boolean = False
    Public Property GlowColor As Color = Color.FromArgb(0, 229, 255)

    Public Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or ControlStyles.OptimizedDoubleBuffer Or ControlStyles.ResizeRedraw, True)
        BackColor = Color.FromArgb(16, 23, 36)
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        Dim g = e.Graphics
        g.SmoothingMode = SmoothingMode.AntiAlias
        g.PixelOffsetMode = PixelOffsetMode.HighQuality

        Dim rect = New Rectangle(0, 0, Width - 1, Height - 1)
        Using path = GetRoundedRectangle(rect, Radius)
            Using br As New SolidBrush(BackColor)
                g.FillPath(br, path)
            End Using

            If HeaderGlow Then
                Using glowPen As New Pen(GlowColor, 2.0F)
                    g.DrawLine(glowPen, Radius, 1, Width - Radius, 1)
                End Using
            End If

            If BorderThickness > 0 Then
                Using pen As New Pen(BorderColor, BorderThickness)
                    g.DrawPath(pen, path)
                End Using
            End If
        End Using
    End Sub

    Private Function GetRoundedRectangle(rect As Rectangle, r As Integer) As GraphicsPath
        Dim path As New GraphicsPath()
        Dim d = r * 2
        If d > rect.Width Then d = rect.Width
        If d > rect.Height Then d = rect.Height
        If d <= 0 Then
            path.AddRectangle(rect)
            Return path
        End If

        path.AddArc(rect.X, rect.Y, d, d, 180, 90)
        path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90)
        path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90)
        path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90)
        path.CloseFigure()
        Return path
    End Function
End Class

Public Class TriggerRowControl
    Inherits Panel

    Public Property Trigger As TriggerConfig
    Public Event PlayRequested(sender As TriggerRowControl, trig As TriggerConfig)
    Public Event DeleteRequested(sender As TriggerRowControl, trig As TriggerConfig)
    Public Event SettingsChanged(sender As TriggerRowControl, trig As TriggerConfig)

    Private chkEnabled As CheckBox
    Private lblTitle As Label
    Private lblRadar As Label
    Private pbMatch As ProgressBar
    Private lblMatch As Label
    Private lblSens As Label
    Private tbSens As TrackBar
    Private lblSensVal As Label
    Private btnPlay As RoundedButton
    Private btnDelete As RoundedButton

    Public Sub New(trig As TriggerConfig)
        Me.Trigger = trig
        Me.Size = New Size(700, 52)
        Me.BackColor = Color.FromArgb(14, 20, 32)
        Me.Margin = New Padding(0, 0, 0, 8)
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or ControlStyles.OptimizedDoubleBuffer Or ControlStyles.ResizeRedraw, True)

        chkEnabled = New CheckBox With {
            .Text = "Ativo",
            .Checked = trig.Enabled,
            .ForeColor = Color.White,
            .Location = New Point(12, 15),
            .Size = New Size(56, 22),
            .Font = New Font("Segoe UI", 8.5F)
        }
        AddHandler chkEnabled.CheckedChanged, Sub()
            trig.Enabled = chkEnabled.Checked
            RaiseEvent SettingsChanged(Me, trig)
        End Sub

        lblTitle = New Label With {
            .Text = trig.Name,
            .Font = New Font("Segoe UI", 9.5F, FontStyle.Bold),
            .ForeColor = Color.FromArgb(0, 229, 255),
            .Location = New Point(70, 16),
            .Size = New Size(160, 20)
        }

        lblRadar = New Label With {
            .Text = "Radar:",
            .ForeColor = Color.FromArgb(148, 163, 184),
            .Location = New Point(234, 17),
            .Size = New Size(42, 18),
            .Font = New Font("Segoe UI", 8.5F)
        }

        pbMatch = New ProgressBar With {
            .Location = New Point(276, 17),
            .Size = New Size(80, 18),
            .Value = 0
        }

        lblMatch = New Label With {
            .Text = "0%",
            .ForeColor = Color.White,
            .Location = New Point(360, 17),
            .Size = New Size(38, 18),
            .Font = New Font("Segoe UI", 8.5F)
        }

        lblSens = New Label With {
            .Text = "Sens:",
            .ForeColor = Color.FromArgb(148, 163, 184),
            .Location = New Point(400, 17),
            .Size = New Size(38, 18),
            .Font = New Font("Segoe UI", 8.5F)
        }

        tbSens = New TrackBar With {
            .Minimum = 50,
            .Maximum = 98,
            .Value = Math.Max(50, Math.Min(98, trig.Threshold)),
            .TickStyle = TickStyle.None,
            .Location = New Point(438, 14),
            .Size = New Size(75, 24)
        }
        lblSensVal = New Label With {
            .Text = tbSens.Value.ToString() & "%",
            .ForeColor = Color.White,
            .Location = New Point(516, 17),
            .Size = New Size(38, 18),
            .Font = New Font("Segoe UI", 8.5F)
        }
        AddHandler tbSens.Scroll, Sub()
            trig.Threshold = tbSens.Value
            lblSensVal.Text = tbSens.Value.ToString() & "%"
            RaiseEvent SettingsChanged(Me, trig)
        End Sub

        btnPlay = New RoundedButton With {
            .Text = "▶️ Ouvir",
            .Radius = 8,
            .NormalColor = Color.FromArgb(16, 26, 44),
            .BorderColor = Color.FromArgb(56, 189, 248),
            .HoverColor = Color.FromArgb(24, 40, 68),
            .ForeColor = Color.FromArgb(224, 242, 254),
            .Font = New Font("Segoe UI", 8.5F, FontStyle.Bold),
            .Location = New Point(558, 10),
            .Size = New Size(68, 32)
        }
        AddHandler btnPlay.Click, Sub()
            RaiseEvent PlayRequested(Me, trig)
        End Sub

        btnDelete = New RoundedButton With {
            .Text = "🗑️",
            .Radius = 8,
            .NormalColor = Color.FromArgb(42, 18, 26),
            .BorderColor = Color.FromArgb(244, 63, 94),
            .HoverColor = Color.FromArgb(64, 22, 34),
            .ForeColor = Color.FromArgb(254, 205, 211),
            .Font = New Font("Segoe UI", 9.0F, FontStyle.Bold),
            .Location = New Point(634, 10),
            .Size = New Size(48, 32)
        }
        AddHandler btnDelete.Click, Sub()
            RaiseEvent DeleteRequested(Me, trig)
        End Sub

        Controls.AddRange(New Control() {chkEnabled, lblTitle, lblRadar, pbMatch, lblMatch, lblSens, tbSens, lblSensVal, btnPlay, btnDelete})
    End Sub

    Public Sub UpdateMatchPercent(percent As Integer)
        pbMatch.Value = Math.Max(0, Math.Min(100, percent))
        lblMatch.Text = percent.ToString() & "%"
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        Dim g = e.Graphics
        g.SmoothingMode = SmoothingMode.AntiAlias
        Dim rect = New Rectangle(0, 0, Width - 1, Height - 1)
        Using path = GetRoundedRectangle(rect, 10)
            Using br As New SolidBrush(BackColor)
                g.FillPath(br, path)
            End Using
            Using pen As New Pen(Color.FromArgb(28, 38, 58), 1.0F)
                g.DrawPath(pen, path)
            End Using
        End Using
    End Sub

    Private Function GetRoundedRectangle(rect As Rectangle, r As Integer) As GraphicsPath
        Dim path As New GraphicsPath()
        Dim d = r * 2
        If d > rect.Width Then d = rect.Width
        If d > rect.Height Then d = rect.Height
        If d <= 0 Then
            path.AddRectangle(rect)
            Return path
        End If
        path.AddArc(rect.X, rect.Y, d, d, 180, 90)
        path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90)
        path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90)
        path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90)
        path.CloseFigure()
        Return path
    End Function
End Class
