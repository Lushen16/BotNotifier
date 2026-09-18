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
