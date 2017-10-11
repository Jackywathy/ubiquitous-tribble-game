Public Class StaticMario
    Inherits StaticImage
    ''' <summary>
    ''' Constructor for <see cref="StaticCoin"/>
    ''' </summary>
    ''' <param name="location"></param>
    Public Sub New(location As Point)
        MyBase.New( My.Resources.mario_small_1.width,  My.Resources.mario_small_1.height, location, My.Resources.mario_small_1)
    End Sub
End Class
