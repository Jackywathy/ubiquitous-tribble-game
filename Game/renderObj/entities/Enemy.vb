Public MustInherit Class EntEnemy
    Inherits Entity
    Public Sub New(width As Integer, height As Integer, location As Point, spriteSet As SpriteSet, scene As Scene)
        MyBase.New(width, height, location, spriteSet, scene)
    End Sub
    #Region "AI"
    Public Sub BasicGround
        ' start by walking in the players direction, only change direction after hitting wall
    End Sub
    #End Region
End Class
