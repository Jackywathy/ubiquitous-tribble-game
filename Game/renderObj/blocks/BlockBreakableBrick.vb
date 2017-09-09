Public Class BlockBreakableBrick
    Inherits BlockBumpable


    ''' <summary>
    ''' TODO make theme work!
    ''' </summary>
    ''' <param name="location"></param>
    ''' <param name="theme"></param>
    ''' <param name="mapScene"></param>
    Public Sub New(location As Point, theme As RenderTheme, mapScene As MapScene)
        MyBase.New(32, 32, location, Sprites.brickBlock, mapScene)

    End Sub

    ''' <summary>
    ''' 0 : x
    ''' 0 : y
    ''' </summary>
    ''' <param name="params"></param>
    Public Sub New(params As Object(), theme As RenderTheme, mapScene As MapScene)
        Me.New(New Point(params(0)*32, params(1)*32), theme, mapScene)
    End Sub

    Public Overrides Sub CollisionBottom(sender As Entity)
        MyBase.CollisionBottom(sender)
        If sender.GetType = GetType(EntPlayer) Then
            Dim player As EntPlayer = sender
            If player.State > PlayerStates.Small Then
                MyScene.PrepareRemove(Me)
                Sounds.BrickSmash.Play()
            Else
                StartBump()
            End If
        End If
    End Sub
End Class
