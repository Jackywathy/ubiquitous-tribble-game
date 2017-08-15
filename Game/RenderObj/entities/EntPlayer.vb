﻿Friend Enum PlayerStates
    Small = 0
    Big = 1
    Fire = 2
    'Ice = 4
End Enum


Public Class EntPlayer
    Inherits Entity

    Public Const CoinsToLives = 100
    Public Shared Property Lives = 5
    Public allowJump = True
    Public allowShoot = True
    Public isCrouching = False
    Public state As UInt16 = 0
    Public numFireballs As UInt16 = 0

    ' This is set when New() is called
    Public Overrides Property spriteSet As SpriteSet = Sprites.playerSmall
    Public Overrides Property moveSpeed As Velocity = New Velocity(1, 15)
    Public Overrides ReadOnly Property maxVeloc As Velocity = New Velocity(6, -15)

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

    ' This should NEVER be called if Mario is small
    Public Sub onCrouch(state As Boolean)
        isCrouching = state
        If isCrouching Then ' IS crouching
            Me.moveSpeed = New Velocity(0, Me.moveSpeed.y)
            'Me.Height = MarioHeightS

        Else ' is NOT crouching
            Me.moveSpeed = New Velocity(1, Me.moveSpeed.y)
            'Me.Height = MarioHeightB

        End If
    End Sub

    Public Sub changeState(change As Int16)
        state = change
        Select Case state
            Case 0 : Me.spriteSet = Sprites.playerSmall
                Me.Height = MarioHeightS
            Case 1 : Me.spriteSet = Sprites.playerBig
                Me.Height = MarioHeightB
            Case 2 : Me.spriteSet = Sprites.playerBigFire
                Me.Height = MarioHeightB
        End Select
    End Sub

    ' Do not call if state != 2
    Public Sub tryShootFireball()
        If numFireballs < 2 Then
            Dim direction = 1
            If Not MyScene.player1.isFacingForward Then
                direction *= -1
            End If
            MainGame.SceneController.AddEntity(New EntFireball(16, 16, New Point(Me.Location.X + (Me.Width * 1.1), Me.Location.Y), direction, Me.isGrounded, MyScene))
        End If
    End Sub


    Public Overrides Sub Animate(numFrames As Integer)
        ' Animate
        Dim imageToDraw As Image
        If isGrounded Or (Not isGrounded And Not didJumpAndNotFall) Then
            ' Check direction
            If veloc.x < 0 And isFacingForward Then
                isFacingForward = False
            ElseIf veloc.x > 0 And Not isFacingForward Then
                isFacingForward = True
            End If

            ' Re-animate every 5 frames
            If veloc.x <> 0 And numFrames Mod 5 = 0 Then
                ' Must be cloned, otherwise the resource image itself gets flipped (an unfortunate side effect of classes being passed by reference...)
                imageToDraw = spriteSet.allSprites(0)(0).Clone
                ' Cycle through the list, moving the last element to the first
                ' I miss being able to use a pop function
                spriteSet.allSprites(0).Insert(0, spriteSet.allSprites(0).Last)
                spriteSet.allSprites(0).RemoveAt(spriteSet.allSprites(0).Count - 1)
            ElseIf veloc.x = 0 Then
                If Me.state > 0 And Me.isCrouching Then
                    'Crouch
                    imageToDraw = spriteSet.allSprites(3)(0).Clone
                Else
                    ' Idle
                    imageToDraw = spriteSet.allSprites(1)(0).Clone
                End If

            End If
        ElseIf didJumpAndNotFall Then
            ' Jump
            imageToDraw = spriteSet.allSprites(2)(0).Clone
        End If

#Disable Warning BC42104 ' Variable is used before it has been assigned a value
        ' stupid compiler, I'm checking if its null
        If imageToDraw IsNot Nothing Then
            If Not isFacingForward Then
                imageToDraw.RotateFlip(RotateFlipType.RotateNoneFlipX)
            End If
            RenderImage = imageToDraw
        End If
#Enable Warning BC42104 ' Variable is used before it has been assigned a value

    End Sub

    Sub New(width As Integer, height As Integer, location As Point, spriteSet As SpriteSet, scene As Scene)

        MyBase.New(width, height, location, scene)
        Me.RenderImage = Resize(spriteSet.allSprites(1)(0), width, height)
        'Me.spriteSet = spriteSet

    End Sub

End Class