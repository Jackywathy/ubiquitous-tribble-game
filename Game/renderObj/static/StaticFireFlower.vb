Public Class StaticFireFlower
    Inherits StaticImage

    Public Sub New(location As Point)
        MyBase.New(32, 32, location, Resize(My.Resources.f_flower_1, 32, 32))
    End Sub
End Class
