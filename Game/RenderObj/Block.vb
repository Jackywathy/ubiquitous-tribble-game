Public Class Block
    Inherits RenderObject

    Public Overrides Property RenderImage As Image

    Public Sub New(width As Integer, height As Integer, location As Point)
        MyBase.New(width, height, location)
        RenderImage = New Bitmap(width, height)
        Using g = Graphics.FromImage(RenderImage)
            g.DrawImage(My.Resources.brick, 0, 0, width, height)
        End Using
    End Sub

End Class
