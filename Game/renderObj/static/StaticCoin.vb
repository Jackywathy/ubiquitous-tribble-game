Public Class StaticCoin
    Inherits StaticImage

    Public Sub New(location As Point, scene As BaseScene)
        MyBase.New(32, 32, location, Resize(My.Resources.coin_idle_1, 32, 32), scene)
    End Sub
End Class
