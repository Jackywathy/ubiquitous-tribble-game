﻿Public Class EntKoopa
    Inherits EntEnemy

    Public deathTimer As Integer = 0
    Public inShell As Integer = False

    ' Amount of time the player cannot be killed by shell immediately after kicking it
    Public gracePeriodTimerForPlayer As Integer = 0

    Public gettingKicked = False

    Public Overrides Property moveSpeed As Velocity = New Velocity(1, 0)
    Public Overrides Property maxVeloc As Velocity = New Velocity(1.8, Forces.terminalVeloc)

    Public Sub New(location As Point,theme as RenderTheme,  mapScene As MapScene)
        MyBase.New(32, 64, location, Sprites.koopaGreen, mapScene)
        Me.CollisionHeight = 32
    End Sub

    ''' <summary>
    ''' 0 : x
    ''' 1 : y
    ''' </summary>
    Public Sub New(params As Object(),theme as RenderTheme,  mapScene As MapScene)
        Me.New(New Point(params(0) * 32, params(1) * 32),theme, mapScene)

    End Sub


    Public Sub GoIntoShell()
        inShell = True
        Me.veloc.x = 0
        Me.moveSpeed = New Velocity(0, 0)
        Me.RenderImage = Me.SpriteSet(SpriteState.Destroy)(1)
        Me.Height = 32

    End Sub

    Public Sub Kick(direction As Integer)
        Me.moveSpeed = New Velocity(8, 0)
        Me.maxVeloc = New Velocity(8, Forces.terminalVeloc)
        Me.directionMoving = direction
        Me.gettingKicked = True
    End Sub

    Public Overrides Sub Animate()
        If willDie Then
            Me.deathTimer += 1
            Me.Height = 32
            Me.RenderImage = SpriteSet(SpriteState.Destroy)(2)
            Me.Location = Me.BounceFunction(deathTimer)

        ElseIf Not Me.inShell Then
            If veloc.x <> 0 And MyScene.GlobalFrameCount Mod (2 * AnimationInterval) = 0 Then
                Dim index = MyScene.GlobalFrameCount / (2 * AnimationInterval)
                If isFacingForward Then
                    Me.RenderImage = SpriteSet(SpriteState.ConstantRight)(index Mod 2)
                Else
                    Me.RenderImage = SpriteSet(SpriteState.ConstantLeft)(index Mod 2)
                End If
            End If
        End If

    End Sub

    Public Overrides Sub UpdateVeloc()
        If Me.inShell And Me.veloc.x <> 0 Then
            Me.killsOnContact = True
        Else
            Me.killsOnContact = False
        End If

        If Me.gettingKicked Then
            Me.gracePeriodTimerForPlayer += 1
            If Me.gracePeriodTimerForPlayer > 10 Then
                Me.gettingKicked = False
                Me.gracePeriodTimerForPlayer = 0
            End If
        End If

        If Me.willDie Then
            Me.deathTimer += 1
        End If

        If Me.isDead Then
            Me.Destroy()
        End If


        Me.AccelerateX(directionMoving * Me.moveSpeed.x)

        MyBase.UpdateVeloc()

        AiBasicGround()
    End Sub

    Public Sub OnSenderCollision_Sides(sender As Entity, direction As Integer)
        If sender.GetType() = GetType(EntPlayer) Then
            If Me.inShell Then
                If veloc.x <> 0 Then
                    HurtPlayer(sender)
                Else
                    Me.Kick(direction)
                End If
            ElseIf Not Me.willDie Then
                HurtPlayer(sender)
            End If
        ElseIf sender.killsOnContact Then
            willDie = True
            CollisionActive = False
            Me.defaultY = Me.Location.Y
            If sender.GetType = GetType(EntFireball) Then
                Dim f As EntFireball = sender
                f.PrepareForDestroy()
            End If
        End If
    End Sub

    ''' <summary>
    '''  
    ''' </summary>
    ''' <param name="sender"></param>
    Public Overrides Sub CollisionTop(sender As Entity)
        If sender.GetType() = GetType(EntPlayer) And sender.veloc.y < 0 Then
            If Me.inShell Then
                If Me.veloc.x = 0 Then
                    Me.Kick(1)
                ElseIf Not gettingKicked Then
                    Me.veloc.x = 0
                    Me.moveSpeed = New Velocity(0, 0)
                    Dim player As EntPlayer = sender
                    player.IsBouncingOffEntity = True
                End If
            Else
                Me.GoIntoShell()
                Dim player As EntPlayer = sender
                player.IsBouncingOffEntity = True
                'player.veloc = New Distance(player.veloc.x, 0)
                'player.AccelerateY(player.moveSpeed.y * 0.75, True)
            End If
            sender.veloc.y = 0
        ElseIf sender.killsOnContact Then
            willDie = True
            CollisionActive = False
            Me.defaultY = Me.Location.Y
            If sender.GetType = GetType(EntFireball) Then
                Dim f As EntFireball = sender
                f.PrepareForDestroy()
            End If
        End If
    End Sub

    Public Overrides Sub CollisionBottom(sender As Entity)
        MyBase.CollisionBottom(sender)
        OnSenderCollision_Sides(sender, 1)
    End Sub

    Public Overrides Sub CollisionLeft(sender As Entity)
        MyBase.CollisionLeft(sender)
        OnSenderCollision_Sides(sender, 1)
    End Sub

    Public Overrides Sub CollisionRight(sender As Entity)
        MyBase.CollisionRight(sender)
        OnSenderCollision_Sides(sender, -1)
    End Sub

    Private Sub HurtPlayer(player As EntPlayer)
        If Not Me.gettingKicked Then
            player.PlayerGotHit()
        End If
    End Sub
End Class
