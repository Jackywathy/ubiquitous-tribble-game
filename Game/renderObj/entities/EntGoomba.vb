Public Class EntGoomba
    Inherits Entity

    Public deathTimer As Integer
    Public squashed As Boolean = True
    Public shouldRemove = False
    Private defaultY As Integer

    Public Sub New(location As Point, scene As Scene)
        MyBase.New(32, 32, location, Sprites.goomba, scene)
    End Sub

    Public Sub New(params As Object(), scene As Scene)
        Me.New(New Point(params(0), params(1)), scene)
    End Sub

    Public Overrides Sub animate()
        If isDead Then
            If squashed Then
                Me.renderImage = SpriteSet(SpriteState.Destroy)(0)
                If Math.Floor(Me.deathTimer / animationInterval) = 5 Then
                    Me.shouldRemove = True
                End If
            Else
                Me.renderImage = SpriteSet(SpriteState.Destroy)(1)
                Me.deathTimer += 1
                Dim x = Me.deathTimer / (animationInterval * 5)

                ' Use displacement/time function
                ' f(x) = 50(2x - x^2)

                Dim heightFunc = 50 * (2 * (x) - (x * x))
                Me.Location = New Point(Me.Location.X, defaultY + heightFunc)
                If Me.Location.Y < 0 Then
                    Me.shouldRemove = True
                End If
            End If

        Else
            If veloc.x <> 0 And MyScene.frameCount Mod (2 * animationInterval) = 0 Then
                Me.renderImage = SpriteSet.SendToBack(SpriteState.Constant)
            End If
            ' just to check if it works
            If collidedX Then
                collidedX = False
                temp *= -1
            End If
            Me.Veloc.X = temp
        End If

    End Sub
    Dim temp As Integer = -1

    Public Overrides Sub UpdatePos()
        If Me.isDead Then
            Me.deathTimer += 1
        End If
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
                player.AccelerateY(player.moveSpeed.y * 0.75, True)
            End If
            isDead = True
            squashed = True
        ElseIf sender.GetType() = GetType(EntFireball) Then
            isDead = True
            squashed = False
            Me.defaultY = Me.Location.Y
        End If
    End Sub

    Public Overrides Sub CollisionBottom(sender As Entity)
        MyBase.CollisionBottom(sender)
        If sender.GetType() = GetType(EntPlayer) Then
            If Not Me.IsDead Then
                HurtPlayer(sender)
            End If
        ElseIf sender.GetType() = GetType(EntFireball) Then
            isDead = True
            squashed = False
            Me.defaultY = Me.Location.Y
        End If
    End Sub

    Public Overrides Sub CollisionLeft(sender As Entity)
        MyBase.CollisionLeft(sender)
        If sender.GetType() = GetType(EntPlayer) Then
            If Not Me.IsDead Then
                HurtPlayer(sender)
            End If
        ElseIf sender.GetType() = GetType(EntFireball) Then
            isDead = True
            squashed = False
            Me.defaultY = Me.Location.Y
        End If
    End Sub

    Public Overrides Sub CollisionRight(sender As Entity)
        MyBase.CollisionRight(sender)
        If sender.GetType() = GetType(EntPlayer) Then
            If Not Me.IsDead Then
                HurtPlayer(sender)
            End If
        ElseIf sender.GetType() = GetType(EntFireball) Then
            isDead = True
            squashed = False
            Me.defaultY = Me.Location.Y
        End If
    End Sub
    
    Private Sub HurtPlayer(player As EntPlayer)
        player.PlayerGotHit()
    End Sub
End Class
