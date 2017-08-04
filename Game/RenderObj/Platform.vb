Public Class Platform
    Inherits RenderObject

    Public Overrides Property RenderImage As Image
    Public Sub New(width As Integer, height As Integer, location As Point, image As Image)
        MyBase.New(width, height, location)
        RenderImage = Resize(image, width, height)
    End Sub



End Class
