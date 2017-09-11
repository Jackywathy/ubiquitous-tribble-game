Public Class EntCoin
    Inherits Entity

    Sub New(width As Integer, height As Integer, location As Point, mapScene As MapScene)
        MyBase.New(width, height, location, Sprites.coin, mapScene)
        Me.RenderImage = SpriteSet.GetFirst(SpriteState.ConstantRight)
    End Sub

    Public Overrides Sub Animate()
        Dim index = Math.Floor(framesSinceHit / (animationInterval * 2)) Mod 3
        Me.RenderImage = SpriteSet(SpriteState.ConstantRight)(index)
    End Sub

    Public Overrides Sub UpdateVeloc()
        Me.framesSinceHit += 1
    End Sub

    Private Sub Collect(sender As EntPlayer)
        sender.PickupCoin()
        Me.Destroy()
    End Sub


    Public Overrides Sub CollisionBottom(sender As Entity)
        If sender.GetType = GetType(EntPlayer) Then
            collect(sender)
        End If
    End Sub

    Public Overrides Sub CollisionTop(sender As Entity)
        If sender.GetType = GetType(EntPlayer) Then
            collect(sender)
        End If
    End Sub

    Public Overrides Sub CollisionLeft(sender As Entity)
        If sender.GetType = GetType(EntPlayer) Then
            collect(sender)
        End If
    End Sub

    Public Overrides Sub CollisionRight(sender As Entity)
        If sender.GetType = GetType(EntPlayer) Then
            collect(sender)
        End If
    End Sub

End Class
