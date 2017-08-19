﻿Friend Enum PlayerStates
    Dead = 0
    Small = 1
    Big = 2
    Fire = 3
    'Ice = 4
End Enum


Public Class EntPlayer
    Inherits Entity

    Public Const CoinsToLives = 100
    Public Shared Property Lives = 5

    Public allowJumpInput = True
    Public allowShoot = True
    Public isCrouching = False
    Public allowedToUncrouch = True
    Public state As UInt16 = 0
    Public numFireballs As UInt16 = 0

    Public Overrides Property moveSpeed As Distance = New Distance(1, 15)
    Public Overrides ReadOnly Property maxVeloc As Distance = New Distance(6, -15)

    Sub New(width As Integer, height As Integer, location As Point, scene As Scene)
        MyBase.New(width, height, location, Sprites.playerSmall, scene)
    End Sub

    ' This should NEVER be called if Mario is small
    ''' <summary>
    ''' Makes Player crouch and not crouch 
    ''' </summary>
    Public Sub onCrouch(tryCrouch As Boolean)

        ' isCrouching only false if state is false and if player is allowed to uncrouch
        If (Not tryCrouch) And allowedToUncrouch Then
            isCrouching = False
        Else
            isCrouching = True
        End If

        If isCrouching Then ' IS crouching
            Me.CollisionHeight = MarioHeightS
        Else ' is NOT crouching
            Me.CollisionHeight = MarioHeightB
        End If

    End Sub

    ''' <summary>
    ''' Changes state of Player, from small to big and fire
    ''' </summary>
    Public Sub setState(change As Int16)
        state = change
        Select Case state
            Case PlayerStates.Dead
            Case PlayerStates.Small : Me.SpriteSet = Sprites.playerSmall
                Me.Height = MarioHeightS
                Me.CollisionHeight = MarioHeightS
            Case PlayerStates.Big : Me.SpriteSet = Sprites.playerBig
                Me.Height = MarioHeightB
                Me.CollisionHeight = MarioHeightB
            Case PlayerStates.Fire : Me.SpriteSet = Sprites.playerBigFire
                Me.Height = MarioHeightB
                Me.CollisionHeight = MarioHeightB
        End Select
    End Sub

    Public Sub decrementState()
        If Me.state > 0 Then
            'Me.state -= 1
        End If
    End Sub

    ' Do not call if state != PlayerStates.fire
    ''' <summary>
    ''' Attempts to shoot a fireball. Will not shoot if 2 are onscreen already.
	''' </summary>
    Public Sub tryShootFireball()
        If numFireballs < 2 Then
            Dim direction = 1
            Dim pointSpawn = Me.Location
            If Me.isFacingForward Then
                pointSpawn = New Point(Me.Location.X + Me.Width, Me.Location.Y + 0.5 * Me.Height)
            Else
                pointSpawn = New Point(Me.Location.X, Me.Location.Y + 0.5 * Me.Height)
                direction *= -1
            End If
            MainGame.SceneController.AddEntity(New EntFireball(16, 16, pointSpawn, direction, Me, MyScene))
            Me.numFireballs += 1
        End If
    End Sub

    Public Overrides Sub animate()

        ' Make sure this is exhaustive
        Dim spriteStateToUse = -2

        ' If crouching
        If isCrouching Then
            spriteStateToUse = SpriteState.Crouch

            ' If grounded or falling off a platform
        ElseIf isGrounded Or (Not isGrounded And Not didJumpAndNotFall) Then

            ' If moving
            If veloc.x <> 0 Then

                spriteStateToUse = SpriteState.Ground


            ElseIf veloc.x = 0 Then 'If still
                spriteStateToUse = SpriteState.Constant
            End If

            'If jumping
        ElseIf didJumpAndNotFall Then
            spriteStateToUse = SpriteState.Air
        End If

        If Not isFacingForward Then
            spriteStateToUse += 1
        End If

        Select Case spriteStateToUse
            ' Multi-frame
            Case SpriteState.Ground, SpriteState.GroundFlip
                If internalFrameCounter Mod animationInterval = 0 Then
                    Me.renderImage = SpriteSet.SendToBack(spriteStateToUse)
                End If

                ' Single frame
            Case Else
                Me.renderImage = SpriteSet(spriteStateToUse)(0)
        End Select
    End Sub

    Public Overrides Sub UpdatePos()
        If Me.isCrouching And Not isGrounded Then
            Me.onCrouch(False)
        End If
        MyBase.UpdatePos()
    End Sub

    Private Shared _coins As Integer = 0

    Public Shared Property Coins
        Get
            Return _coins
        End Get
        Set(value)
            If value >= CoinsToLives Then
                _coins = value - CoinsToLives
            Else
                _coins += 1
            End If

        End Set
    End Property
End Class
