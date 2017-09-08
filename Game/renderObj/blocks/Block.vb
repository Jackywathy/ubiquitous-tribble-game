Public MustInherit Class Block
    Inherits HitboxItem

    Public Enum QuestionBlockReward
        Fire
        DefaultFire 
        Mushroom
        Coin
        Star
    End Enum


    Friend Const blockWidth = 32
    Friend Const blockHeight = 32

    Public Overrides Property RenderImage As Image
    Public Property spriteSet As SpriteSet
    Public defaultLocationY As Integer

    Public isMoving As Boolean = False

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

    ' Takes a raw frame counter and returns a new point
    Public Function bounceFunction(x As Integer) As Point
        x /= animationInterval
        Dim heightFunc = 6 * (2 * (x) - (x * x))

        ' f(x) = 0 when x = 2
        If frameCount / animationInterval >= 2 Then
            Me.isMoving = False
            Return New Point(Me.Location.X, defaultLocationY)
        Else
            Return New Point(Me.Location.X, defaultLocationY + heightFunc)
        End If
    End Function


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
