Public Class StaticDecoration

    Inherits StaticImage

    ' images
    Shared ReadOnly Property CloudSmall As Image = My.Resources.cloud_small
    Shared ReadOnly Property CloudBig As Image = My.Resources.cloud_big
    Shared ReadOnly Property HillSmall As Image = My.Resources.hill_small
    Shared ReadOnly Property HillBig As Image = My.Resources.hill_big
    Shared ReadOnly Property Castle As Image = My.Resources.castle

    Shared ReadOnly Property Mushroom As Image = My.Resources.mushroom
    Shared ReadOnly Property FireFlower As Image = My.Resources.f_flower_1

    Public Sub New(location As Point, image As Image)
        MyBase.New(image.Width, image.Height, location, image)
    End Sub

End Class
