Imports System.Drawing.Drawing2D

Public Class GroundPlatform
    Inherits HitboxItem

    Private texture as TextureBrush

    ''' <summary>
    ''' TODO make theme work
    ''' </summary>
    ''' <param name="width"></param>
    ''' <param name="height"></param>
    ''' <param name="location"></param>
    ''' <param name="theme"></param>
    ''' <param name="mapScene"></param>
    Public Sub New(width As Integer, height As Integer, location As Point, theme As RenderTheme, mapScene As MapScene)
        MyBase.New(width, height, location, Nothing, mapScene)
        SetFloor(theme)
    End Sub
    Dim previousX as integer


    Public OVerrides Sub Render(g As Graphics)
        RefreshTransform()
        g.FillRectangle(texture, GetRenderRect())
    End Sub
        

    Private Sub SetFloor(theme As RenderTheme)
        dim tile as image
        Select Case Theme
            Case RenderTheme.Overworld
                tile = My.Resources.blockGround
            Case RenderTheme.Underground
                tile = My.Resources.blockBelowGround
            Case Else
                Throw New Exception()
        End Select

        texture = New TextureBrush(tile, WrapMode.Tile)
        
        previousX = 0
    End Sub

    Public Sub RefreshTransform
        texture.ResetTransform()
        texture.TranslateTransform(-MyScene.ScreenLocation.X, ScreenGridHeight Mod 32)
    End Sub

    ''' <summary>
    ''' 0 : x
    ''' 1 : y
    ''' 2 : width
    ''' 3 : height
    ''' </summary>
    ''' <param name="params"></param>
    Public Sub New (params As Object(), theme As RenderTheme, mapScene As MapScene)
        Me.New(params(2)*32, params(3)*32, New Point(params(0)*32, params(1)*32), theme, mapScene)
    End Sub

End Class
