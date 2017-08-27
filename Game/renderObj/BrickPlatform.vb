Imports System.Drawing.Drawing2D

Public Class BrickPlatform
    Inherits RenderObject

    Public Overrides Property RenderImage As Image
    Public Sub New(width As Integer, height As Integer, location As Point, mapScene As MapScene)
        MyBase.New(width, height, location, mapScene)
        RenderImage = New Bitmap(width, height)
        Using brush=New TextureBrush(My.Resources.blockGround, WrapMode.Tile)
            brush.TranslateTransform(0, 32)
            Using g=Graphics.FromImage(RenderImage)
                g.FillRectangle(brush, 0,0, RenderImage.Width, RenderImage.Height)
            End Using
        End Using
    End Sub

    ''' <summary>
    ''' 0 : x
    ''' 1 : y
    ''' 2 : width
    ''' 3 : height
    ''' </summary>
    ''' <param name="params"></param>
    Public Sub New (params As Object(), mapScene As MapScene)
        Me.New(params(2)*32, params(3)*32, New Point(params(0)*32, params(1)*32), mapScene)
    End Sub
End Class
