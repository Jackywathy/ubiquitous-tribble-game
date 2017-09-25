Public Class EntStar
    Inherits EntPowerup

    Public Overrides Property moveSpeed As Velocity = New Velocity(0.4, 5)
    Public Overrides Property maxVeloc As Velocity = New Velocity(3, Forces.terminalVeloc)

    Private spawnCounter = 0
    Public Overrides Property PickupSound As MusicPlayer

    Public Overrides ReadOnly Property PickupScore As Integer = PlayerPoints.Star

    Public Overrides Sub Activate(sender As EntPlayer)
        sender.InvinicibilityTimer = EntPlayer.StarInvincibilityDuration

        If PickupSound IsNot Nothing Then
            Me.PickupSound.Play()
        End If

        MyScene.PrepareRemove(Me)
    End Sub


    Public Overrides Sub UpdateVeloc()
        If IsOutOfMap() <> Direction.None Then
            Me.Destroy()
        End If

        If Not IsSpawning Then

            Me.AccelerateX(moveSpeed.x)

            If Me.isGrounded Then
                Me.AccelerateY(moveSpeed.y, False)
            End If
        End If
        MyBase.UpdateVeloc()
    End Sub

    Sub New(location As Point, mapScene As MapScene)
        MyBase.New(StandardWidth, StandardHeight, location, Sprites.star, mapScene)
    End Sub

End Class
