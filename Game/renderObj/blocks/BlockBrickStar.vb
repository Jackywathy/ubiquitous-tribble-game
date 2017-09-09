Public Class BlockBrickStar
    Inherits BlockBumpable
    
    Private isUsed As Boolean = False

    Public Sub New(width As Integer, height As Integer, location As Point, spriteSet As SpriteSet, mapScene As MapScene)
        MyBase.New(width, height, location, spriteSet, mapScene)
    End Sub

    Public Overrides Sub CollisionBottom(sender As Entity)
        MyBase.CollisionBottom(sender)

        If Not isUsed And Helper.IsPlayer(sender) Then
            Dim player As EntPlayer = sender
            
            Dim star = New EntStar(32, 32, New Point(Me.Location.X, Me.Location.Y + Me.Height), MyScene)
            star.Spawn()
            isUsed = True
            StartBump()
        End If

    End Sub
End Class
