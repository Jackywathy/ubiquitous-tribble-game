Public Class EntKoopa
    Inherits Entity

    Public deathTimer As Integer = 0
    Public inShell As Integer  = False
    Public defaultY  As Integer = 0
    Public willDie As Integer = False

    ' Amount of time the player cannot be killed by shell immediately after kicking it
    Public gracePeriodTimerForPlayer As Integer = 0

    Public gettingKicked = False

    ' Positive for right
    ' Negative for left
    Public directionMoving As Integer = 1

    Public Overrides Property moveSpeed As Distance = New Distance(1, 0)
    Public Overrides Property maxVeloc As Distance = New Distance(1.8, -15)

    Public Overrides Property RenderImage As Image 

    Public Sub New(location As Point, mapScene As MapScene)
        MyBase.New(32, 64, location, Sprites.koopaGreen, mapScene)
        Me.CollisionHeight = 32
    End Sub

    ''' <summary>
    ''' 0 : x
    ''' 1 : y
    ''' </summary>
    Public Sub New(params As Object(), mapScene As MapScene)
        Me.New(New Point(params(0)*32, params(1)*32), mapScene)

    End Sub
    

    Public Sub GoIntoShell()
        inShell = True
        Me.veloc.x = 0
        Me.moveSpeed = New Distance(0, 0)
        Me.RenderImage = Me.SpriteSet(SpriteState.Destroy)(1)
        
        Me.Height = 32

    End Sub

    Public Sub Kick(direction As Integer)
        Me.moveSpeed = New Distance(8, 0)
        Me.maxVeloc = New Distance(8, 0)
        Me.directionMoving = direction
        Me.gettingKicked = True
    End Sub

    Public Overrides Sub animate()
        If willDie Then
            Me.deathTimer += 1
            Me.RenderImage = SpriteSet(SpriteState.Destroy)(2)
            Dim x = Me.deathTimer / (animationInterval * 5)

            ' Use displacement/time function
            ' f(x) = 50(2x - x^2)

            Dim heightFunc = 50 * (2 * (x) - (x * x))
            Me.Location = New Point(Me.Location.X, defaultY + heightFunc)
            If Me.Location.Y < 0 Then
                Me.isDead = True
            End If

        ElseIf Not Me.inShell Then
            If veloc.x <> 0 And MyScene.frameCount Mod (2 * animationInterval) = 0 Then
                Dim index = MyScene.frameCount / (2 * animationInterval)
                If isFacingForward Then
                    Me.RenderImage = SpriteSet(SpriteState.ConstantRight)(index Mod 2)
                Else
                    Me.RenderImage = SpriteSet(SpriteState.ConstantLeft)(index Mod 2)
                End If
            End If
        End If

    End Sub

    Public Overrides Sub UpdateItem()

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

        MyBase.UpdateItem()

        If Me.willCollideFromLeft Or Me.willCollideFromRight Then
            Me.directionMoving *= -1
        End If
    End Sub

    Public Overrides Sub CollisionTop(sender As Entity)
        If sender.GetType() = GetType(EntPlayer) Then
            If Me.inShell Then
                If Me.veloc.x = 0 Then
                    Me.Kick(1)
                ElseIf Not gettingKicked Then
                    Me.veloc.x = 0
                    Me.moveSpeed = New Distance(0, 0)
                    Dim player As EntPlayer = sender
                    player.IsBouncingOffEntity = True
                    'player.veloc = New Distance(player.veloc.x, 0)
                    'player.AccelerateY(player.moveSpeed.y * 0.75, True)
                End If
            Else
                Me.GoIntoShell()
                Dim player As EntPlayer = sender
                player.IsBouncingOffEntity = True
                'player.veloc = New Distance(player.veloc.x, 0)
                'player.AccelerateY(player.moveSpeed.y * 0.75, True)
            End If
        ElseIf sender.killsOnContact Then
            willDie = True
            Me.defaultY = Me.Location.Y
        End If
    End Sub

    Public Overrides Sub CollisionBottom(sender As Entity)
        MyBase.CollisionBottom(sender)
        If sender.GetType() = GetType(EntPlayer) Then
            If Me.inShell Then
                If veloc.x <> 0 Then
                    HurtPlayer(sender)
                End If
            ElseIf Not Me.willDie Then
                HurtPlayer(sender)
            End If
        ElseIf sender.killsOnContact Then
            willDie = True
            Me.defaultY = Me.Location.Y
        End If
    End Sub

    Public Overrides Sub CollisionLeft(sender As Entity)
        MyBase.CollisionLeft(sender)
        If sender.GetType() = GetType(EntPlayer) Then
            If Me.inShell Then
                If veloc.x <> 0 Then
                    HurtPlayer(sender)
                Else
                    Me.Kick(1)
                End If
            ElseIf Not Me.willDie Then
                HurtPlayer(sender)
            End If
        ElseIf sender.killsOnContact Then
            willDie = True
            Me.defaultY = Me.Location.Y
        End If
    End Sub

    Public Overrides Sub CollisionRight(sender As Entity)
        MyBase.CollisionRight(sender)
        If sender.GetType() = GetType(EntPlayer) Then
            If Me.inShell Then
                If veloc.x <> 0 Then
                    HurtPlayer(sender)
                Else
                    Me.Kick(-1)
                End If
            ElseIf Not Me.willDie Then
                HurtPlayer(sender)
            End If
        ElseIf sender.killsOnContact Then
            willDie = True
            Me.defaultY = Me.Location.Y
        End If
    End Sub

    Private Sub HurtPlayer(player As EntPlayer)
        If Not Me.gettingKicked Then
            player.PlayerGotHit()
        End If
    End Sub
End Class
