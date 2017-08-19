﻿

Public Module Dimensions
    Public Const ScreenGridWidth As Integer = 1280
    Public Const ScreenGridHeight As Integer = 720
    Public TotalGridWidth As Integer = 4000
    Public TotalGridHeight As Integer = 1000
    Public MarioWidth = 32
    Public MarioHeightS = 32
    Public MarioHeightB = 64
    Public GroundHeight = 64
End Module

Module Debug
    Public Sub DbShowImage(image As Image)
        Dim x = (new    TestImage(image))
        x.Show()
    End Sub
    Public Sub Print(str As Object)
        Console.Out.WriteLine(str.ToString)
    End Sub
    Public isDebug = True
End Module

Public Module ImageManipulation

    Public Function Crop(image As Image, bottomLeft As Point, width As Integer, height As Integer) As Image
        ' width/height of rectangle output


        ' rectangle to be cut out
        Dim cropRect As New RectangleF(New PointF(bottomLeft.X, image.Height - height - bottomLeft.Y), New SizeF(width, height))

        Dim out As New Bitmap(width, height)
        Using g = Graphics.FromImage(out)
            g.DrawImage(image, New RectangleF(0, 0, cropRect.Width, cropRect.Height), cropRect, GraphicsUnit.Pixel)
        End Using
        Return out
    End Function

    Public Sub Crop(image As Image, out As Graphics, bottomLeft As Point, width As Integer, height As Integer)
        ' width/height of rectangle output


        ' rectangle to be cut out
        Dim cropRect As New RectangleF(New PointF(bottomLeft.X, image.Height - height - bottomLeft.Y), New SizeF(width, height))
        out.DrawImage(image, New RectangleF(0, 0, cropRect.Width, cropRect.Height), cropRect, GraphicsUnit.Pixel)
    End Sub

    Public Function Resize(image As Image, width As Integer, height As Integer) As Image
        ' width/height of rectangle output
        Dim out As New Bitmap(width, height)
        Using g = Graphics.FromImage(out)
            g.DrawImage(image, 0, 0, width, height)
        End Using
        Return out
    End Function

    Public Function Flip(image As Image) As Image
        Dim img As Bitmap = image.Clone()
        img.RotateFlip(RotateFlipType.RotateNoneFlipX)
        return img
    End Function

End Module




Friend Module UriCreator
    

    


End Module
