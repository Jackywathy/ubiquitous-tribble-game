﻿Public Class EntFireFlower
    Inherits EntPowerup

    Public Overrides Property state As UInt16 = PlayerStates.Fire
    Public Overrides Property moveSpeed As Distance = New Distance(0, 0)
    Public Overrides Property maxVeloc As Distance = New Distance(0, 0)
    Private spawnCounter = 0

    ' TODO REPLACE WITH FIRE_FIRE SOUND
    Public Overrides Property PickupSound As MusicPlayer = Sounds.MushroomPickup

   

    Sub New(width As Integer, height As Integer, location As Point, scene As Scene)
        MyBase.New(width, height, location, Sprites.f_flower, scene)

    End Sub

    Public Overrides Sub animate()
        If Not isSpawning And MyScene.frameCount Mod (animationInterval) = 0 Then
            Me.RenderImage = Me.SpriteSet.SendToBack(SpriteState.ConstantRight)
        ElseIf isSpawning Then
            If (Math.Floor(spawnCounter / animationInterval) Mod 7) = Me.SpriteSet(SpriteState.Spawn).Count - 1 Then
                isSpawning = False
                Me.RenderImage = Me.SpriteSet(SpriteState.ConstantRight)(0)
            Else
                Me.spawnCounter += 1
                Me.RenderImage = Me.SpriteSet(SpriteState.Spawn)(Math.Floor(spawnCounter / animationInterval) Mod 7)
            End If

        End If
    End Sub

End Class
