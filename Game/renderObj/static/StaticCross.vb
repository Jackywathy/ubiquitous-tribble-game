Public Class StaticCross
    Inherits StaticImage

    Public Sub New(width As integer, height as Integer, location As Point)
        MyBase.New(width, height, location, Resize(My.Resources.cross,width,height))
    End Sub
End Class

