Public Class StaticText
    Inherits RenderItem


    Private sf As StringFormat
    Public str As String
    Private font As Font
    Private brush As brush
    Private drawnRect As RectangleF

    Public Overrides Sub Render(g As Graphics)
        g.DrawString(str, font, brush, drawnRect, sf)
    End Sub

    Sub New (drawnRect As RectangleF, str As String, font As Font, brush As Brush, alignment As StringAlignment)
        sf = New StringFormat()
        sf.Alignment = alignment
        sf.LineAlignment = alignment
        Me.brush = brush
        Me.Str = str
        Me.Font = font
        Me.drawnRect = drawnRect
    End Sub
End Class
