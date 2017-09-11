Public Class StaticCross
    Inherits StaticImage

    Public Sub New(location As Point)
        MyBase.New(32, 32, location, Resize(My.Resources.cross,32,32))
    End Sub
End Class

