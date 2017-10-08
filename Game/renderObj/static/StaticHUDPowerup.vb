Public Class StaticHudPowerup
    Inherits StaticImage

    Public powerupImage As StaticImage
    

    Public Sub New(width As integer, height As integer, location As Point)
        MyBase.New(width, height, location, Resize(My.Resources.HUDitemBox, width, height))
    End Sub

    Public Function HasPowerup As Boolean
        Return me.storedpowerup <> PowerupType.Empty
    End Function

    Public Sub ResetLocation()
        Me.powerupImage.Location = GetPowerLocationPoint()
    End Sub

    ''' <summary>
    ''' Spawn the powerup, using staticImage's location, then deletes it
    ''' </summary>
    ''' <param name="scene"></param>
    Public Sub SpawnPowerup(scene As MapScene)
        Dim powerup As EntPowerup
        Dim spawnPoint = New Point(powerupImage.x+scene.ScreenLocation.X, powerupImage.y+scene.ScreenLocation.Y)
        Select Case StoredPowerup
            Case PowerupType.Fireflower
                powerup = New EntFireFlower(spawnPoint, scene)
            Case PowerupType.Mushroom
                powerup = New EntMushroom(spawnPoint, scene)
            Case Else
                Throw New Exception()
        End Select
        powerup.isGrounded = False
        powerup.fromHud = True
        powerup.Spawn(True)
        SetPowerupItem(PowerupType.Empty)
    End Sub

    Public Sub ChangeLocation(dx As Integer, dy As Integer)
        Me.powerupImage.Location = New Point(Me.powerupImage.Location.X + dx, Me.powerupImage.Location.Y + dy)
    End Sub

    ''' <summary>
    ''' Sets the location of the powerUp
    ''' </summary>
    ''' <param name="loc"></param>
    Public Sub SetPowerupLocation(loc As Point)
        SetPowerupLocation(loc.X, loc.y)
    End Sub

    ''' <summary>
    ''' Sets the location of the powerUp
    ''' </summary>
    ''' <param name="loc"></param>
    Public Sub SetPowerupMiddleLocation(loc As Point)
        SetPowerupLocation(loc.X - powerupImage.Width / 2, loc.y - powerupImage.Height /2)
    End Sub


    Public Sub SetPowerupLocation(x As Integer, y As Integer)
        
            Me.powerupImage.X = x
            Me.powerupImage.Y = y
    End Sub

    Public Overrides Sub Render(g As Graphics)
        MyBase.Render(g)
        If powerupImage Isnot nothing
            powerupImage.Render(g)
        End if
    End Sub

    ''' <summary>
    ''' Checks if the powerup image inside intersects with the actual HUD element
    ''' used to check if it should reset
    ''' </summary>
    ''' <returns></returns>
    Public Function PowerupTouchesBox As Boolean
        Return Me.GetRect().IntersectsWith(powerupImage.GetRect())
    End Function


    Private Function GetPowerLocationPoint() As Point
        Return New Point(location.X + Width/4, location.Y + Height/4)
    End Function

    Private Property StoredPowerup As PowerupType

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="item"></param>
    Public Sub SetPowerupItem(item As PowerupType)
        Select Case item
            Case PowerupType.Fireflower
                powerupImage = New StaticFireFlower(GetPowerLocationPoint)
            Case PowerupType.Mushroom
                powerupImage = New StaticMushroom(GetPowerLocationPoint)
            Case PowerupType.Empty
                powerupImage = Nothing
            Case Else
                Throw New Exception()
        End Select
        StoredPowerup = item
    End Sub

    Public Sub GetDr()

    End Sub
    
End Class

Public Enum PowerupType
    Fireflower
    Mushroom
    OneUp
    Empty
End Enum

