Public Class StaticDecoration
    Inherits ScrollAlongImage

    Public Sub New(width As Integer, height As Integer, location As Point, image As Image, scene As MapScene)
        MyBase.New(width, height, location, image, scene)
    End Sub

    ' images
    Shared ReadOnly Property CloudSmall As Image = My.Resources.cloud_small
    Shared ReadOnly Property CloudBig As Image = My.Resources.cloud_big
    Shared ReadOnly Property HillSmall As Image = My.Resources.hill_small
    Shared ReadOnly Property HillBig As Image = My.Resources.hill_big
    Shared ReadOnly Property Castle As Image = My.Resources.castle

    Shared ReadOnly Property Mushroom As Image = My.Resources.mushroom
    Shared ReadOnly Property FireFlower As Image = My.Resources.f_flower_1


    Public Overrides Sub AddSelfToScene()
        MyScene.AddStatic()
        ' oWo whats this?
    End Sub
End Class
