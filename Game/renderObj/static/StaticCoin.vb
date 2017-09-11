Public Class StaticCoin
    Inherits StaticImage
    ''' <summary>
    ''' Constructor for <see cref="StaticCoin"/>
    ''' </summary>
    ''' <param name="location"></param>
    Public Sub New(location As Point)
        MyBase.New(32, 32, location, Resize(My.Resources.coin_idle_1, 32, 32))
    End Sub
End Class
