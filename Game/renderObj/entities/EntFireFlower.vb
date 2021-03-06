﻿Public Class EntFireFlower
    Inherits EntPowerup

    Public Overrides Property moveSpeed As Velocity = New Velocity(0, 0)
    Public Overrides Property maxVeloc As Velocity = New Velocity(0, Forces.terminalVeloc)
    Public Overrides ReadOnly Property PickupScore As Integer = PlayerPoints.Firefire

    Private spawnCounter = 0

    ' TODO REPLACE WITH FIRE_FIRE SOUND
    Public Overrides Property PickupSound As MusicPlayer = Sounds.MushroomPickup



    Sub New(location As Point, mapScene As MapScene, Optional immediateSpawn As Boolean = False)
        MyBase.New(StandardWidth, StandardHeight, location, Sprites.f_flower, mapScene)
        If immediateSpawn Then
            Me.RenderImage = Me.SpriteSet(SpriteState.Constant)(0)
        Else
            Me.RenderImage = Me.SpriteSet(SpriteState.Spawn)(0)
        End If
    End Sub

    Public Overrides Sub Animate()
        If Not isSpawning And MyScene.GlobalFrameCount Mod (animationInterval) = 0 Then
            Me.RenderImage = Me.SpriteSet.SendToBack(SpriteState.ConstantRight)
        ElseIf isSpawning Then
            If (Math.Floor(spawnCounter / animationInterval) Mod 7) = Me.SpriteSet(SpriteState.Spawn).Count - 1 Then
                isSpawning = False
                Me.RenderImage = Me.SpriteSet.GetFirst(SpriteState.ConstantRight)
            Else
                Me.spawnCounter += 1
                Me.RenderImage = SpriteSet(SpriteState.Spawn)(Math.Floor(spawnCounter / animationInterval) Mod 7)
            End If

        End If
    End Sub

    Public Overrides Sub Activate(sender As EntPlayer)
        MyBase.Activate(sender)
        if sender.State <> PlayerStates.Fire
            sender.State = PlayerStates.Fire
        Else
            MyScene.HudElements.SetPowerup(PowerupType.Fireflower)
        End if
    End Sub

End Class
