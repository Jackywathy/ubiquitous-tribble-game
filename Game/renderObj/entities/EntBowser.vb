Public Class EntBowser
    Inherits Entity
    Private jumpcooldown as integer

    Public Overrides Property Movespeed As New Velocity(1.5, 1)

    Public Sub New(location As Point, mapScene As MapScene)
        MyBase.New(32*2, 32*2, location, Sprites.bowser, mapScene)
    End Sub
    Public Overrides Sub UpdateVeloc
        If jumpcooldown = 0 And Helper.Random(0, 10) = 0
            AccelerateY(movespeed.y, False)
        End If
        If Helper.Random(0, 10) = 0
            IsFacingForward = Not IsFacingForward
            veloc.x = -veloc.x
        End If
        MyBase.UpdateVEloc()
    End Sub
    
End Class
