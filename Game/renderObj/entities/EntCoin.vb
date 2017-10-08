Public Class EntCoin
    Inherits Entity


    Sub New(location As Point, mapScene As MapScene)
        MyBase.New(StandardWIdth, standardHeight, location, Sprites.coin, mapScene)
        Me.RenderImage = SpriteSet.GetFirst(SpriteState.ConstantRight)
    End Sub

    Sub New(params as Object(), theme as rendertheme, scene as MapScene)
        Me.New(New point(params(0)*32, params(1)*32), scene)
    End Sub

    

    Public Overrides Sub Animate()
        Dim index = Math.Floor(FramesSinceHit / (AnimationInterval * 2)) Mod 3
        Me.RenderImage = SpriteSet(SpriteState.ConstantRight)(index)
    End Sub

    Public Overrides Sub UpdateVeloc()
        Me.FramesSinceHit += 1
    End Sub

    Private Sub Collect(sender As EntPlayer)
        sender.PickupCoin()
        Me.Destroy()
    End Sub


    Public Overrides Sub CollisionBottom(sender As Entity)
        If sender.GetType = GetType(EntPlayer) Then
            Collect(sender)
        End If
    End Sub

    Public Overrides Sub CollisionTop(sender As Entity)
        If sender.GetType = GetType(EntPlayer) Then
            Collect(sender)
        End If
    End Sub

    Public Overrides Sub CollisionLeft(sender As Entity)
        If sender.GetType = GetType(EntPlayer) Then
            Collect(sender)
        End If
    End Sub

    Public Overrides Sub CollisionRight(sender As Entity)
        If sender.GetType = GetType(EntPlayer) Then
            Collect(sender)
        End If
    End Sub

End Class
