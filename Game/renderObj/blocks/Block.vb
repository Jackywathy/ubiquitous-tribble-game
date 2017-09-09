Public MustInherit Class Block
    Inherits HitboxItem

    Public Const StandardWidth = 32
    Public Const StandardHeight = 32

    Public Enum QuestionBlockReward
        Fire
        DefaultFire 
        Mushroom
        Coin
        Star
    End Enum

    Public Overrides Property RenderImage As Image
    Friend ReadOnly Property SpriteSet As SpriteSet
    Friend DefaultLocationY As Integer


    Public Sub New(width As Integer, height As Integer, location As Point, spriteSet As SpriteSet, mapScene As MapScene)
        MyBase.New(width, height, location, mapScene)
        Me.spriteSet = spriteSet
        Me.RenderImage = spriteSet(0)(0)
        Me.defaultLocationY = location.Y
    End Sub

    Public Sub New(width As Integer, height As Integer, location As Point, mapScene As MapScene)
        MyBase.New(width, height, location, mapScene)
        Me.defaultLocationY = location.Y
    End Sub


    Public Overrides Sub CollisionTop(sender As Entity)
        MyBase.CollisionTop(sender)
    End Sub
    Public Overrides Sub CollisionBottom(sender As Entity)
        MyBase.CollisionBottom(sender)
    End Sub

    Public Overrides Sub CollisionLeft(sender As Entity)
        If sender.GetType = GetType(EntFireball) Then
            sender.Destroy()
        Else
            MyBase.CollisionLeft(sender)
        End If
    End Sub

    Public Overrides Sub CollisionRight(sender As Entity)

        If sender.GetType = GetType(EntFireball) Then
            sender.Destroy()
        Else
            MyBase.CollisionRight(sender)
        End If
    End Sub
End Class
