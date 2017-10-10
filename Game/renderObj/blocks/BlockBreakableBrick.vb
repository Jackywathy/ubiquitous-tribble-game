Public Class BlockBreakableBrick
    Inherits BlockBumpable


    ''' <summary>
    ''' TODO make theme work!
    ''' </summary>
    ''' <param name="location"></param>
    ''' <param name="theme"></param>
    ''' <param name="mapScene"></param>
    Public Sub New(location As Point, theme As RenderTheme, mapScene As MapScene)
        MyBase.New(StandardWidth, StandardHeight, location, GetSpriteSet(theme), mapScene)

    End Sub

    Public Shared Function GetSpriteSet(theme as RenderTheme) As SpriteSet
        Select Case theme
            Case RenderTheme.Overworld
                Return Sprites.brickBlock
            Case RenderTheme.Underground
                Return Sprites.brickBlockUnder
            Case Else
                throw new Exception()
        End Select
    End Function

    ''' <summary>
    ''' 0 : x
    ''' 0 : y
    ''' </summary>
    ''' <param name="params"></param>
    Public Sub New(params As Object(), theme As RenderTheme, mapScene As MapScene)
        Me.New(New Point(params(0) * 32, params(1) * 32), theme, mapScene)
    End Sub

    Public Overrides Sub CollisionBottom(sender As Entity)
        MyBase.CollisionBottom(sender)
        If sender.GetType = GetType(EntPlayer) Then
            Dim player As EntPlayer = sender
            If player.State > PlayerStates.Small Then
                MyScene.PrepareRemove(Me)
                Sounds.BrickSmash.Play()
            Else
                StartBump(True)
            End If
        End If
    End Sub
End Class
