Public Class BlockBreakableBrick
    Inherits Block

    Overrides Property spriteset As SpriteSet = Sprites.brickBlock

    Public Sub New(location As Point)
        MyBase.New(blockWidth, blockHeight, location, My.Resources.blockBrick)
        Me.RenderImage = Resize(spriteset.allSprites(0)(0), Width, Height)
        Me.spriteset = spriteset
    End Sub

    Public Overrides Sub CollisionBottom(sender As Entity)
        MyBase.CollisionBottom(sender)
        If sender.GetType = GetType(EntPlayer) Then
            Dim player As EntPlayer = sender
            If player.state > 0 Then
                TO_DO__DELETE.Disappear(Me)
            Else
                ' could not break
            End If
        End If
    End Sub
End Class