Public Class MetalBlock
    Inherits Block
    Public Overrides Property spriteSet As SpriteSet = Sprites.BrickMetalSprite

    Sub New(location As Point)
        Mybase.New(blockWidth, blockHeight, location, My.Resources.blockMetal)

    End Sub


End Class
