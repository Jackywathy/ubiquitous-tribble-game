Public Class StaticHudPowerup
    Inherits StaticImage

    Private powerupImage As StaticImage 

    Public Sub New(location As Point)
        MyBase.New(48, 48, location, Resize(My.Resources.HUDitemBox, 48, 48))
    End Sub


    Public Overrides Sub Render(g As Graphics)
        MyBase.Render(g)
        If powerupImage Isnot nothing
            powerupImage.Render(g)
        End if
    End Sub

    ''' <summary>
    ''' change the item in the box
    ''' give it Nothing to set it to nothing
    ''' </summary>
    ''' <param name="item"></param>
    Public Sub ChangeItem(item As StaticImage)
        Me.powerupImage = item
        if item.Width <> 32 Or item.Height <> 32
            Throw New Exception("Must be 32x32 image")
        End If
        item.Location = GetPowerLocationPoint()
    End Sub

    Private Function GetPowerLocationPoint() As Point
        Return New Point(location.X + 8, location.Y + 8)
    End Function

End Class

