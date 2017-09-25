Public Class EntMushroom
    Inherits EntPowerup

    Public Overrides Property moveSpeed As Velocity = New Velocity(3, 0)
    Public Overrides Property maxVeloc As Velocity = New Velocity(2, Forces.terminalVeloc)
    
    Public Overrides ReadOnly Property PickupScore As Integer = PlayerPoints.Mushroom

    Public Overrides Property PickupSound As MusicPlayer = Sounds.MushroomPickup
    Private directionMoving As Integer = 1

    Sub New(location As Point, mapScene As MapScene)
        MyBase.New(StandardWidth, StandardHeight, location, Sprites.mushroom, mapScene)
        Me.RenderImage = Me.SpriteSet(SpriteState.Spawn)(0)
    End Sub


    Public Overrides Sub UpdateVeloc()
        If Not IsSpawning Then
            If Me.willCollideFromLeft Or Me.willCollideFromRight Then
                Me.directionMoving *= -1
            End If
            Me.AccelerateX(directionMoving * moveSpeed.x)
            MyBase.UpdateVeloc()
        End If
    End Sub

    Public Overrides Sub Activate(sender As EntPlayer)
        MyBase.Activate(sender)
        If sender.State = PlayerStates.Small Then
            sender.State = PlayerStates.Big
        Else
            ' TODO add to toolbar at top
        End If
    End Sub

End Class
