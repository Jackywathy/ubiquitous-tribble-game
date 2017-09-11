Public Class StaticHudPowerup
    Inherits StaticImage

    Private powerupImage As StaticImage 

    Public Sub New(location As Point)
        MyBase.New(48, 48, location, Resize(My.Resources.HUDitemBox, 48, 48))
        powerupImage = New StaticCoin(New Point(location.X + 8, location.Y + 8))
    End Sub

    Public Overrides Sub AddToScene(scene As BaseScene)
        scene.AddStatic(Me)
    End Sub
End Class

