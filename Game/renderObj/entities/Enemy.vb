Public MustInherit Class EntEnemy
    Inherits Entity
    ' Positive for right
    ' Negative for left
    Public directionMoving As Integer = 1
    Public willDie As Integer = False

    Public Sub New(width As Integer, height As Integer, location As Point, spriteSet As SpriteSet, mapScene As MapScene)
        MyBase.New(width, height, location, spriteSet, mapScene)
    End Sub

    Public Overrides Sub CollisionLeft(sender As Entity)
        If Helper.IsPlayer(sender)
            Dim player = DirectCast(sender, EntPlayer)
            If player.InvinicibilityTimer > 0 Then
                Me.Destroy()
            End If
        End If
    End Sub
    #Region "AI"
    Public Sub AiBasicGround
        ' start by walking in the players direction, only change direction after hitting wall
        If Me.willCollideFromLeft Or Me.willCollideFromRight Then
            Me.directionMoving *= -1
        End If
    End Sub
    #End Region
End Class
