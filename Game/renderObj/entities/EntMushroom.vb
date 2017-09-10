﻿Public Class EntMushroom
    Inherits EntPowerup

    Public Overrides Property moveSpeed As Distance = New Distance(3, 0)
    Public Overrides Property maxVeloc As Distance = New Distance(2, Forces.terminalVeloc)
    
    Public Overrides ReadOnly Property PickupScore As Integer = PlayerPoints.Mushroom

    Public Overrides Property PickupSound As MusicPlayer = Sounds.MushroomPickup

    Sub New(width As Integer, height As Integer, location As Point, mapScene As MapScene)
        MyBase.New(width, height, location, Sprites.mushroom, mapScene)
        Me.RenderImage = Me.SpriteSet(SpriteState.Spawn)(0)
    End Sub


    Public Overrides Sub UpdateVeloc()
        If Not isSpawning Then
            Me.AccelerateX(moveSpeed.x)
        End If
        MyBase.UpdateVeloc()
    End Sub
    Public Overrides Sub Activate(sender As EntPlayer)
        MyBase.Activate(sender)
        sender.State = PlayerStates.Big
    End Sub

End Class
