Public Class BlockMetal
    Inherits Block
    Public Overrides Property spriteSet As SpriteSet = Sprites.metalBlock

    Sub New(location As Point, scene As Scene)
        MyBase.New(blockWidth, blockHeight, location, scene)
        Me.RenderImage = Resize(spriteset.allSprites(0)(0), MarioWidth, MarioHeightS)
    End Sub

End Class
