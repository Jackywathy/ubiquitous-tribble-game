Public Class BlockMetal
    Inherits Block
    Public Overrides Property spriteSet As SpriteSet = Sprites.BrickMetalSprite

    Sub New(location As Point)
        Mybase.New(blockWidth, blockHeight, location)
        Me.RenderImage = Resize(spriteset.allSprites(0)(0), MarioWidth, MarioHeightS)
    End Sub


End Class
