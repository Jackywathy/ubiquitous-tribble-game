Imports System.Drawing.Drawing2D

Public Class BelowGroundPlatform
    Inherits HitboxItem


    ''' <summary>
    ''' TODO make theme work
    ''' </summary>
    ''' <param name="width"></param>
    ''' <param name="height"></param>
    ''' <param name="location"></param>
    ''' <param name="theme"></param>
    ''' <param name="mapScene"></param>
    Public Sub New(width As Integer, height As Integer, location As Point, theme As RenderTheme, mapScene As MapScene)
        MyBase.New(width, height, location, GetFloor(width, height, theme), mapScene)
    End Sub
        
    Private Shared Function GetFloor(width As Integer, height as Integer, theme As RenderTheme) As Image
        Dim image = New Bitmap(width, height)
        Using brush=New TextureBrush(My.Resources.blockBelowGround, WrapMode.Tile)
            brush.TranslateTransform(0, 32)
            Using g=Graphics.FromImage(image)
                g.FillRectangle(brush, 0,0, image.Width, image.Height)
            End Using
        End Using
        Return image
    End Function

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

    Public Overrides Sub Render(g As Graphics)
        MyBase.Render(g)
    End Sub
End Class
