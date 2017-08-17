Public Class BlockMetal
    Inherits Block

    Sub New(location As Point, scene As Scene)
        MyBase.New(blockWidth, blockHeight, location, sprites.blockMetal, scene)
    End Sub

End Class
