Imports WinGame

Public Class EntCoin
    Inherits Entity

    Sub New(width As Integer, height As Integer, location As Point, scene As Scene)
        MyBase.New(width, height, location, Sprites.coin, scene)
        Me.renderImage = Me.SpriteSet(SpriteState.Constant)(0)
    End Sub

    Public Overrides Sub animate()
        Dim index = Math.Floor(frameCount / (animationInterval * 2)) Mod 3
        Me.renderImage = Me.SpriteSet(SpriteState.Constant)(index)
    End Sub

    Public Overrides Sub UpdatePos()
        Me.frameCount += 1
    End Sub

    Private Sub collect(collector As EntPlayer)
        collector.PickupCoin()
        Me.Destroy()
    End Sub

    Public Overrides Sub CollisionBottom(sender As Entity)
        If sender.GetType = GetType(EntPlayer) Then
            Me.collect(sender)
        End If
    End Sub

    Public Overrides Sub CollisionTop(sender As Entity)
        If sender.GetType = GetType(EntPlayer) Then
            Me.collect(sender)
        End If
    End Sub

    Public Overrides Sub CollisionLeft(sender As Entity)
        If sender.GetType = GetType(EntPlayer) Then
            Me.collect(sender)
        End If
    End Sub

    Public Overrides Sub CollisionRight(sender As Entity)
        If sender.GetType = GetType(EntPlayer) Then
            Me.collect(sender)
        End If
    End Sub

End Class
