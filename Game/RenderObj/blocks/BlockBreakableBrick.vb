Public Class BlockBreakableBrick
    Inherits Block

    Overrides Property spriteset As SpriteSet = Sprites.brickBlock

    Public Sub New(location As Point)
        MyBase.New(blockWidth, blockHeight, location)
        Me.RenderImage = Resize(spriteset.allSprites(0)(0), Width, Height)
        Me.spriteset = spriteset
    End Sub

    Public Overrides Sub CollisionBottom(sender As Entity, scene As Scene)
        MyBase.CollisionBottom(sender, scene)
        If sender.GetType = GetType(EntPlayer) Then
            Dim player As EntPlayer = sender
            If player.state > 0 Then
                MainGame.SceneController.RemoveObj(Me)
                Sounds.BrickSmash.Play()
            Else
                ' could not break
            End If
        End If
    End Sub
End Class