Public Class StaticMushroom
    Inherits StaticImage

    Public Sub New(location As Point)
        MyBase.New(StandardWidth, StandardHeight, location, My.Resources.mushroom)
    End Sub
End Class
