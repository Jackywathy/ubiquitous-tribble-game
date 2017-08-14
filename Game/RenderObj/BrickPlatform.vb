Imports System.Drawing.Drawing2D

Public Class BrickPlatform
    Inherits RenderObject

    Public Overrides Property RenderImage As Image
    Public Sub New(width As Integer, height As Integer, location As Point, scene As Scene)
        MyBase.New(width, height, location, scene)
        RenderImage = New Bitmap(width, height)
        Using brush=New TextureBrush(My.Resources.platform, WrapMode.Tile)
            Using g=Graphics.FromImage(RenderImage)
                g.FillRectangle(brush, 0,0, RenderImage.Width, RenderImage.Height)
            End Using
        End Using
    End Sub



End Class
