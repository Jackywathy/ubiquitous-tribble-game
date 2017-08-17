Public Class BlockBreakableBrick
    Inherits Block

    Public Sub New(location As Point, scene As Scene)
        MyBase.New(32, 32, location, Sprites.brickBlock, scene)
        Me.RenderImage = Resize(spriteset(0)(0), Width, Height)
    End Sub

    Public Overrides Sub CollisionBottom(sender As Entity)
        MyBase.CollisionBottom(sender)
        If sender.GetType = GetType(EntPlayer) Then
            Dim player As EntPlayer = sender
            If player.state > 0 Then
                MainGame.SceneController.PrepareRemove(Me)
                Sounds.BrickSmash.Play()
            Else
                ' could not break
            End If
        End If
    End Sub
End Class