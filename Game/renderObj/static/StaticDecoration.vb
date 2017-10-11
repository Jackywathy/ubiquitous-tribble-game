Public Class StaticDecoration
    Inherits ScrollAlongImage

    Public Sub New(width As Integer, height As Integer, location As Point, image As Image, scene As MapScene)
        MyBase.New(width, height, location, image, scene)
    End Sub

    Public Sub New(location as point, image as image, scene as mapscene)
        Me.New(image.width, image.height, location, image, scene)
    End Sub

    ' images
    Shared ReadOnly Property CloudSmall As Image = My.Resources.cloud_small
    Shared ReadOnly Property CloudBig As Image = My.Resources.cloud_big
    Shared ReadOnly Property HillSmall As Image = My.Resources.hill_small
    Shared ReadOnly Property HillBig As Image = My.Resources.hill_big
    Shared ReadOnly Property Castle As Image = My.Resources.castle

    Shared ReadOnly Property Mushroom As Image = My.Resources.mushroom
    Shared ReadOnly Property FireFlower As Image = My.Resources.f_flower_1

    Public Shared Property Mario As Image = My.Resources.mario_small_1

    Friend Shared Function GetRandomBrush(point As Point, scene As MapScene) As StaticDecoration
        Dim image As Image
        Select Case Helper.Random(0, 1)
            Case 0
                image = HillSmall
            Case 1
                image = HillBig
            Case Else
                Throw New Exception()
        End Select
        Return New StaticDecoration(point, image, scene)
    End Function


    Friend Shared Function GetRandomCloud(point As Point, scene as mapscene) As StaticDecoration
        Dim image as image
        Select Case Helper.Random(0, 1)
            Case 0
                image = CloudSmall
            Case 1
                image = CloudBig
            Case Else
                throw new exception()
        End Select
        Return new StaticDecoration(point, image, scene)
    End Function


    Public Overrides Sub AddSelfToScene()
        MyScene.AddScrollingImage(Me)
        ' oWo whats this?
    End Sub

    
End Class
