Imports System.Drawing.Drawing2D

Public Class Platform
    Inherits RenderObject

    Public Overrides Property RenderImage As Image
    Public Sub New(width As Integer, height As Integer, location As Point, image As Image)
        MyBase.New(width, height, location)
        RenderImage = New Bitmap(width, height)
        Using brush=New TextureBrush(image, WrapMode.Tile)
            Using g=Graphics.FromImage(RenderImage)
                g.FillRectangle(brush, 0,0, RenderImage.Width, RenderImage.Height)
            End Using
        End Using
    End Sub



End Class
