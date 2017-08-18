Public Class BlockBreakableBrick
    Inherits Block

    Public Sub New(location As Point, scene As Scene)
        MyBase.New(32, 32, location, Sprites.brickBlock, scene)
    End Sub
    
    ''' <summary>
    ''' 0 : x
    ''' 0 : y
    ''' </summary>
    ''' <param name="params"></param>
    Public Sub New(params As Object(), scene As Scene)
        Me.New(New Point(params(0), params(1)), scene)
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