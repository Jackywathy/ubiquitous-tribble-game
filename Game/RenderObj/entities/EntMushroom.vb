﻿Public Class EntMushroom
    Inherits EntPowerup

    Private spawnCounter = 0
    Public Overrides Property state As UInt16 = PlayerStates.Big
    Public Overrides Property moveSpeed As Distance = New Distance(1, 0)
    Public Overrides ReadOnly Property maxVeloc As Distance = New Distance(1.5, Forces.terminalVeloc)

    Public Overrides Property PickupSound As MusicPlayer = Sounds.MushroomPickup

    Sub New(width As Integer, height As Integer, location As Point, scene As Scene)
        MyBase.New(width, height, location, Sprites.mushroom, scene)
    End Sub


    Public Overrides Sub Animate(numFrames As Integer)
        If isSpawning And numFrames Mod 5 = 0 Then
            If spawnCounter = 6 Then
                isSpawning = False
                RenderImage = spriteSet.SendToBack(1)
            Else
                RenderImage = spriteSet(0)(spawnCounter).Clone
                spawnCounter += 1
            End If
        End If
    End Sub

    Public Overrides Sub UpdatePos()
        If Not isSpawning Then
            Me.AccelerateX(moveSpeed.x)
        End If
        MyBase.UpdatePos()
    End Sub

   

End Class
