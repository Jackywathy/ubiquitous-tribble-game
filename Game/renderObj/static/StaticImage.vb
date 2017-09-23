''' <summary>
''' This is a non-changing image that does not SCROLL - UI elements like icon icon, lives icon
''' </summary>
Public MustInherit Class StaticImage
    Inherits GameImage
    ''' <summary>
    ''' Constructor for <see cref="StaticImage"/>
    ''' </summary>
    ''' <param name="width"></param>
    ''' <param name="height"></param>
    ''' <param name="location"></param>
    ''' <param name="image"></param>
    Public Sub New(width As Integer, height As Integer, location As point, image As Image)
        MyBase.New(width, height, location, image)
    End Sub

    ''' <summary>
    ''' Renders the image onto a graphics object
    ''' </summary>
    ''' <param name="g"></param>
    Public Overrides Sub Render(g As Graphics)
        if RenderImage IsNot Nothing
            Dim drawnRect As New Rectangle(Location.X,
                                      Dimensions.ScreenGridHeight - Height - Location.Y,
                                           width, height)
            ' top right x, top right y, width, heigh
            g.DrawImage(RenderImage, drawnRect)
            If ShowBoundingBox Then
                g.DrawRectangle(DrawingPrimitives.BluePen, drawnRect)
            End If
        End if
    End Sub

    ''' <summary>
    ''' Gets a rectangle
    ''' </summary>
    ''' <returns></returns>
    Public Overridable Function GetTopLeftRect As Rectangle
        REturn New Rectangle(location.X, GetTopBasedY(Location.y), width, height)
    End Function

    Public Function GetRect As Rectangle
        Return New Rectangle(location.X, Location.y, Width, height)
    End Function
End Class