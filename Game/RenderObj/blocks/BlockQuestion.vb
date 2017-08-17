Public Class BlockQuestion
    Inherits Block
    Public isUsed = False

    Sub New(location As Point, scene As Scene)
        MyBase.New(blockWidth, blockHeight, location, Sprites.itemBlock, scene)
        Me.spriteset = spriteset
    End Sub

    Public Overrides Sub Animate(numFrames As Integer)
        If numFrames Mod 10 = 0 And Not isUsed Then
            RenderImage = spriteSet.GetNext(0)
        End If
    End Sub

    Public Overrides Sub CollisionBottom(sender As Entity)
        MyBase.CollisionBottom(sender)
        If Not isUsed Then
            Dim mushroom As New EntMushroom(32, 32, New Point(Me.Location.X, Me.Location.Y + Me.Height), MyScene)
            MainGame.SceneController.AddEntity(mushroom)

            'Dim flower As New EntFireFlower(32, 32, New Point(Me.Location.X, Me.Location.Y + Me.Height), MyScene)
            'MainGame.SceneController.AddEntity(flower)

            isUsed = True
            Me.RenderImage = My.Resources.blockQuestionUsed
        End If
       
    End Sub


End Class
