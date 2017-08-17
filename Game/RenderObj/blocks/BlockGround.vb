' Please add support for this in .json so we can use this as the ground
' idea: make the bottom two rows of entire level filled with this by default and allow .json to specify columns where this block should be removed 

Public Class BlockGround
    Inherits Block
    Public Overrides Property spriteSet As SpriteSet = Sprites.groundBlock

    Sub New(location As Point, scene As Scene)
        MyBase.New(blockWidth, blockHeight, location, scene)
        Me.RenderImage = Resize(spriteSet.allSprites(0)(0), MarioWidth, MarioHeightS)
    End Sub

End Class
