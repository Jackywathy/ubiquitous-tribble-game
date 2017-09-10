Public Class StaticText
    Inherits GameItem

    Private paddingWidth As Integer
    Private paddingChar As Char

    Private _text As String

    Public Property Text As String
        Get
            Return _text
        End Get
        Set(value As String)
            _text = value.PadLeft(paddingWidth, paddingChar)
        End Set
    End Property

    Private sf As StringFormat
    
    Private font As Font
    Private brush As brush
    Private drawnRect As RectangleF

    Public Overrides Sub Render(g As Graphics)
        g.DrawString(Text, font, brush, drawnRect)
    End Sub

    Sub New (drawnRect As RectangleF, str As String, fontFam As FontFamily, emSize As Integer, brush As Brush,scene As MapScene, Optional horAlignment As StringAlignment = StringAlignment.Near, Optional vertAlignment As StringAlignment=StringAlignment.Near, Optional paddingChar As Char = "0", Optional paddingWidth As Integer = 0)
        Me.New(drawnRect, str, New Font(fontFam, emSize), brush, scene, New StringFormat(), paddingWidth, paddingChar)       
        sf.Alignment = horAlignment
        sf.LineAlignment = vertAlignment
    End Sub

    Sub New (drawnRect As RectangleF, str As String, font As Font, brush As Brush, scene As MapScene, format As StringFormat, paddingWidth As integer, paddingChar As Char)
        MYBase.New(scene)
        Me.sf = format
        Me.paddingWidth = paddingWidth
        Me.paddingChar = paddingChar
        Me.brush = brush
        Me.Text = str
        Me.Font = font
        Me.drawnRect = drawnRect
    End Sub
End Class
