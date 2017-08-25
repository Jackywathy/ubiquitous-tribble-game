Public Class StaticText
    Inherits StaticItem


    Private sf As StringFormat
    Public str As String
    Private font As Font
    Private brush As brush
    Private drawnRect As RectangleF

    Public Overrides Sub Render(g As Graphics)
        g.DrawString(str, font, brush, drawnRect, sf)
    End Sub

    Sub New (drawnRect As RectangleF, str As String, fontFam As FontFamily, emSize As Integer, brush As Brush, horAlignment As StringAlignment, vertAlignment As StringAlignment)
        sf = New StringFormat()
        sf.Alignment = horAlignment
        sf.LineAlignment = vertAlignment
        Me.brush = brush
        Me.Str = str
        Me.Font = New Font(fontFam, emSize)
        Me.drawnRect = drawnRect
    End Sub
End Class
