Public Class EntGoomba
    Inherits Entity

    Public isDead As Boolean = False
    Public shouldRemove = False

    Public Sub New(location As Point, scene As Scene)
        MyBase.New(32, 32, location, Sprites.goomba, scene)
    End Sub

    Public Sub New(params As Object(), scene As Scene)
        Me.New(New Point(params(0), params(1)), scene)
    End Sub

    Public Overrides Sub animate()
        If isDead Then
            Me.renderImage = SpriteSet(SpriteState.Destroy)(0)
            If Math.Floor(internalFrameCounter / animationInterval) = 5 Then
                Me.shouldRemove = True
            End If
        Else
            If veloc.x <> 0 And internalFrameCounter Mod (2 * animationInterval) = 0 Then
                Me.renderImage = SpriteSet.SendToBack(SpriteState.Constant)
            End If
            ' just to check if it works
            veloc.x = 1
        End If

    End Sub

    Public Overrides Sub UpdatePos()
        If Me.shouldRemove Then
            Me.Destroy()
        End If
        MyBase.UpdatePos()
    End Sub

    Public Overrides Sub CollisionTop(sender As Entity)
        If sender.GetType() = GetType(EntPlayer) Then
            If Not Me.isDead Then
                Dim player As EntPlayer = sender
                player.veloc = New Distance(player.veloc.x, 0)
                player.AccelerateY(player.moveSpeed.y, True)
            End If
            Me.internalFrameCounter = 0 ' Reset for timer
            isDead = True
        End If
    End Sub

    Public Overrides Sub CollisionBottom(sender As Entity)
        MyBase.CollisionBottom(sender)
        If sender.GetType() = GetType(EntPlayer) Then
            If Not Me.IsDead
                HurtPlayer(sender)
            End If
        End If
    End Sub

    Public Overrides Sub CollisionLeft(sender As Entity)
        MyBase.CollisionLeft(sender)
        If sender.GetType() = GetType(EntPlayer) Then
            If Not Me.IsDead
                HurtPlayer(sender)
            End If
        End If
    End Sub

    Public Overrides Sub CollisionRight(sender As Entity)
        MyBase.CollisionRight(sender)
        If sender.GetType() = GetType(EntPlayer) Then
            If Not Me.IsDead
                HurtPlayer(sender)
            End If
        End If
    End Sub
    
    Private Sub HurtPlayer(player As EntPlayer)
        player.PlayerGotHit()
    End Sub
End Class
