Public Class BlockMetal
    Inherits Block

    Sub New(location As Point, scene As Scene)
        MyBase.New(blockWidth, blockHeight, location, sprites.blockMetal, scene)
    End Sub

    Sub New(params As Object(), scene As Scene)
        Me.New(New Point(params(0), params(1)), scene)
    End Sub
End Class
