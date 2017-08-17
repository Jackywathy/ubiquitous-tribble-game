Public Class EntGoomba
    Inherits Entity

    Public isDead As Boolean = False

    Public Sub New(location As Point, scene As Scene)
        MyBase.New(32, 32, location, Sprites.EntGoomba, scene)
    End Sub

    Public Overrides Sub Animate(numFrames As Integer)
        If isDead
            Me.RenderImage = SpriteSet.GetNext(2)
        Else 
            If veloc.x <> 0 And numFrames Mod 5 = 0 Then
                Me.RenderImage = SpriteSet.GetNext(SpriteState.GroundWalkRight)

            End If
            ' just to check if it works
            veloc.x = 3
        End if
       
    End Sub
    Public Overrides Sub CollisionTop(sender As Entity)
        If sender.GetType() = GetType(EntPlayer)
            isDead = True
            Sounds.Warp.Play()
        End If
    End Sub
End Class
