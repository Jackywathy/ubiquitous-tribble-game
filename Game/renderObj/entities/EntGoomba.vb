Public Class EntGoomba
    Inherits EntEnemy

    Public deathTimer As Integer
    Public squashed As Boolean = True
    Public willDie = False ' Set to true when enemy is killed (but not necessarily removed from the screen yet)
    Private defaultY As Integer


    Public Overrides Property moveSpeed As Distance = New Distance(1, 0)
    Public Overrides Property maxVeloc As Distance = New Distance(2, -15)

    Public Sub New(location As Point, mapScene As MapScene)
        MyBase.New(32, 32, location, Sprites.goomba, mapScene)
    End Sub


    ''' <summary>
    ''' 0 : x
    ''' 1 : y
    ''' </summary>
    ''' <param name="params"></param>
    ''' <param name="mapScene"></param>
    Public Sub New(params As Object(), mapScene As MapScene)
        Me.New(New Point(params(0)*32, params(1)*32), mapScene)
    End Sub

    Public Overrides Sub Animate()
        If willDie Then
            Me.deathTimer += 1
            If squashed Then
                Me.RenderImage = SpriteSet(SpriteState.Destroy)(0)
                If Math.Floor(Me.deathTimer / animationInterval) = 5 Then
                    Me.isDead = True
                End If
            Else
                Me.RenderImage = SpriteSet(SpriteState.Destroy)(1)
                Dim x = Me.deathTimer / (animationInterval * 5)

                ' Use displacement/time function
                ' f(x) = 50(2x - x^2)

                Dim heightFunc = 50 * (2 * (x) - (x * x))
                Me.Location = New Point(Me.Location.X, defaultY + heightFunc)
                If Me.Location.Y < 0 Then
                    Me.isDead = True
                End If
            End If

        Else
            If veloc.x <> 0 And MyScene.GlobalFrameCount Mod (3 * animationInterval) = 0 Then
                Me.RenderImage = SpriteSet(SpriteState.ConstantRight)((MyScene.GlobalFrameCount / (3 * animationInterval)) Mod 2)
            End If

        End If

    End Sub

    Public Overrides Sub UpdateVeloc()
        If Me.willDie Then
            Me.deathTimer += 1
        Else
            Me.AccelerateX(directionMoving * Me.moveSpeed.x)
        End If

        If Me.isDead Then
            Me.Destroy()
        End If



        MyBase.UpdateVeloc()

        AiBasicGround()
    End Sub

    Public Overrides Sub CollisionTop(sender As Entity)
        If Helper.IsPlayer(sender) Then
            If Not Me.willDie Then
                Dim player As EntPlayer = sender
                player.Score += PlayerPoints.Goomba
                player.IsBouncingOffEntity = True
            End If
            willDie = True
            squashed = True
            
        ElseIf sender.killsOnContact Then
            DirectCast(sender, EntPlayer).Score += PlayerPoints.Goomba
            willDie = True
            squashed = False
            Me.defaultY = Me.Location.Y
        End If
    End Sub

    Public Overrides Sub CollisionBottom(sender As Entity)
        MyBase.CollisionBottom(sender)
        If sender.GetType() = GetType(EntPlayer) Then
            If Not Me.willDie Then
                HurtPlayer(sender)
            End If
        ElseIf sender.killsOnContact Then
            willDie = True
            squashed = False
            Me.defaultY = Me.Location.Y
        End If
    End Sub

    Public Overrides Sub CollisionLeft(sender As Entity)
        MyBase.CollisionLeft(sender)
        If sender.GetType() = GetType(EntPlayer) Then
            If Not Me.willDie Then
                HurtPlayer(sender)
            End If
        ElseIf sender.killsOnContact Then
            willDie = True
            squashed = False
            Me.defaultY = Me.Location.Y
        End If
    End Sub

    Public Overrides Sub CollisionRight(sender As Entity)
        MyBase.CollisionRight(sender)
        If sender.GetType() = GetType(EntPlayer) Then
            If Not Me.willDie Then
                HurtPlayer(sender)
            End If
        ElseIf sender.killsOnContact Then
            willDie = True
            squashed = False
            Me.defaultY = Me.Location.Y
        End If
    End Sub

    Private Sub HurtPlayer(player As EntPlayer)
        player.PlayerGotHit()
    End Sub
End Class
