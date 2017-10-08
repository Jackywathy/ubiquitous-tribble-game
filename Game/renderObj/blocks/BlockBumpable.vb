Public MustInherit Class BlockBumpable
    Inherits Block

    Friend IsMoving As Boolean = False

    ' Takes a raw frame counter and returns a new point
    Friend Function BounceFunction(x As Integer) As Point
        x /= AnimationInterval
        Dim heightFunc = 2 * ((4 * (x)) - (x * x))

        ' f(x) = 0 when x = 2
        If FramesSinceHit / AnimationInterval >= 4 Then
            ResetBump()
            Return New Point(Me.Location.X, DefaultLocationY)
        Else
            Return New Point(Me.Location.X, defaultLocationY + heightFunc)
        End If
    End Function

    Public Sub New(width As Integer, height As Integer, location As Point, spriteSet As SpriteSet, mapScene As MapScene)
        MyBase.New(width, height, location, spriteSet, mapScene)
    End Sub

    Friend Sub StartBump(Optional playsound As boolean = False)
        IsMoving = True
        if playsound
            Sounds.Bump.play()
        End if
    End Sub

    Public Overridable Sub ResetBump()
        Me.isMoving = False
        Me.framesSinceHit = 0
    End Sub

    Public Overrides Sub UpdateVeloc()
        If isMoving Then
            ' bumps block
            Me.framesSinceHit += 1
            Me.Location = bounceFunction(framesSinceHit)
        End If
    End Sub

    Public Overrides Sub CollisionTop(sender As Entity)
        MyBase.CollisionTop(sender)
        If sender.GetType.IsSubclassOf(GetType(EntEnemy)) Then
            Dim e As EntEnemy = sender
            e.willDie = True
        ElseIf sender.GetType.IsSubclassOf(GetType(EntPowerup)) Then
            Dim p As EntPowerup = sender
            p.AccelerateY(p.moveSpeed.y, True)
        End If
    End Sub

End Class
