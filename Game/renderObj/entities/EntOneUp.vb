﻿Public Class EntOneUp
    Inherits EntPowerup

    Public Overrides Property moveSpeed As Distance = New Distance(3, 0)
    Public Overrides Property maxVeloc As Distance = New Distance(2, Forces.terminalVeloc)
    
    Public Overrides ReadOnly Property PickupScore As Integer = PlayerPoints.OneUp

    Public Overrides Property PickupSound As MusicPlayer = Sounds.MushroomPickup

    Sub New(location As Point, mapScene As MapScene)
        MyBase.New(StandardWidth, StandardHeight, location, Sprites.lifeshroom, mapScene)
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
        EntPlayer.Lives += 1
    End Sub
End Class
