Public Class StaticCoin
    Inherits StaticImage
    ''' <summary>
    ''' Constructor for <see cref="StaticCoin"/>
    ''' </summary>
    ''' <param name="location"></param>
    Public Sub New(width As Integer, height As Integer, location As Point)
        MyBase.New(width, height, location, Resize(My.Resources.coin_idle_1, width, height))
    End Sub
End Class
