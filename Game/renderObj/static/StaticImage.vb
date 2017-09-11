﻿''' <summary>
''' This is a non-changing image that does not SCROLL - UI elements like icon icon, lives icon
''' </summary>
Public MustInherit Class StaticImage
    Inherits GameImage

    Public Sub New(width As Integer, height As Integer, location As point, image As Image)
        MyBase.New(width, height, location, image)
    End Sub

    Public Overrides Sub Render(g As Graphics)
        if RenderImage IsNot Nothing
            Dim drawnRect As New Rectangle(Location.X,
                                      Dimensions.ScreenGridHeight - Height - Location.Y - ToolBarOffSet,
                                           width, height)
            ' top right x, top right y, width, heigh
            g.DrawImage(RenderImage, drawnRect)
            If ShowBoundingBox Then
                g.DrawRectangle(DrawingPrimitives.BluePen, drawnRect)
            End If
        End if
    End Sub
End Class