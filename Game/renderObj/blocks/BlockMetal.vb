Public Class BlockMetal
    Inherits Block

    Sub New(location As Point, mapScene As MapScene)
        MyBase.New(blockWidth, blockHeight, location, sprites.blockMetal, mapScene)
    End Sub

    Sub New(params As Object(), mapScene As MapScene)
        Me.New(New Point(params(0)*32, params(1)*32), mapScene)
    End Sub
End Class
