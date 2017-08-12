﻿Imports WinGame

Public MustInherit Class Powerup
    Inherits Entity

    Public isSpawning = True

    ' The state the powerup changes the player to
    ' 0 - small
    ' 1 - big 
    ' 2 - fire 
    Public MustOverride Property state As UInt16

    Public Overrides Sub CollisionBottom(sender As Entity)
        Me.TryActivate(sender)
    End Sub
    Public Overrides Sub CollisionTop(sender As Entity)
        Me.TryActivate(sender)
    End Sub
    Public Overrides Sub CollisionLeft(sender As Entity)
        Me.TryActivate(sender)
    End Sub
    Public Overrides Sub CollisionRight(sender As Entity)
        Me.TryActivate(sender)
    End Sub

    Public Sub TryActivate(sender As Entity)
        If sender.GetType = GetType(Player) Then
            Dim player As Player = sender
            player.changeState(Me.state)

            TO_DO__DELETE.Disappear(Me)
        End If
    End Sub

    Public Sub Spawn(location As Point)
        MainGame.SceneController.AddEntity(Me)
    End Sub



    Sub New(width As Integer, height As Integer, location As Point)
        MyBase.New(width, height, location)

    End Sub

End Class