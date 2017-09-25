Public MustInherit Class EntEnemy
    Inherits Entity
    ' Positive for right
    ' Negative for left
    Public directionMoving As Integer = 1
    Public willDie As Integer = False
    Friend defaultY As Integer

    Public Sub New(width As Integer, height As Integer, location As Point, spriteSet As SpriteSet, mapScene As MapScene)
        MyBase.New(width, height, location, spriteSet, mapScene)
    End Sub

    ' Takes a raw frame counter and returns a new point
    Friend Function BounceFunction(frameCount As Integer) As Point
        Dim x = frameCount / (AnimationInterval * 5)
        ' Use displacement/time function
        ' f(x) = 20(2x - x^2)

        Dim heightFunc = 50 * (2 * (X) - (X * X))

        Return New Point(Me.Location.X, defaultY + heightFunc)

        If Me.Location.Y < 0 Then
            Me.isDead = True
        End If
    End Function

    Public Overrides Sub CollisionLeft(sender As Entity)
        If Helper.IsPlayer(sender)
            Dim player = DirectCast(sender, EntPlayer)
            If player.InvinicibilityTimer > 0 Then
                Me.Destroy()
            End If
        End If
    End Sub
    #Region "AI"
    Public Sub AiBasicGround
        ' start by walking in the players direction, only change direction after hitting wall
        If Me.willCollideFromLeft Or Me.willCollideFromRight Then
            Me.directionMoving *= -1
        End If
    End Sub
    #End Region
End Class
