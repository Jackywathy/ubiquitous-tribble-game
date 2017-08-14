Public Class BlockQuestion
    Inherits Block
    Public isUsed = False
    Overrides Property spriteset As SpriteSet = Sprites.itemBlock

    Sub New(location As Point, scene As Scene)
        MyBase.New(blockWidth, blockHeight, location, scene)
        Me.RenderImage = Resize(spriteset.allSprites(0)(0), Width, Height)
        Me.spriteset = spriteset
    End Sub

    Public Overrides Sub Animate(numFrames As Integer)
        If numFrames Mod 10 = 0 And Not isUsed Then
            RenderImage = spriteset.allSprites(0).First
            spriteset.allSprites(0).Insert(0, spriteset.allSprites(0).Last)
            spriteset.allSprites(0).RemoveAt(spriteset.allSprites(0).Count - 1)
        End If
    End Sub

    Public Overrides Sub CollisionBottom(sender As Entity, scene As Scene)
        MyBase.CollisionBottom(sender, scene)
        If Not isUsed Then
            'Dim mushroom As New EntMushroom(32, 32, New Point(Me.Location.X, Me.Location.Y + Me.Height))
            'MainGame.SceneController.AddEntity(mushroom)

            Dim flower As New EntFireFlower(32, 32, New Point(Me.Location.X, Me.Location.Y + Me.Height), scene)
            MainGame.SceneController.AddEntity(flower)

            isUsed = True
            Me.RenderImage = My.Resources.blockQuestionUsed
        End If
       
    End Sub


End Class
