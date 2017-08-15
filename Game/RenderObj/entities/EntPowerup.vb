
Public MustInherit Class EntPowerup
    Inherits Entity

    Public isSpawning = True

    ' The state the powerup changes the player to
    ' 0 - small
    ' 1 - big 
    ' 2 - fire 
    Public MustOverride Property state As UInt16
    Public MustOverride Property PickupSound As MusicPlayer

    Public Overrides Sub CollisionBottom(sender As Entity)
        Me.TryActivate(sender)
    End Sub
    Public Overrides Sub CollisionTop(sender As Entity)
        Me.TryActivate(sender)
    End Sub
    Public Overrides Sub CollisionLeft(sender As Entity)
        Me.TryActivate(sender)
    End Sub
    Public Overrides Sub CollisionRight(sender As Entity)
        Me.TryActivate(sender)
    End Sub


    Public Sub TryActivate(sender As Entity)
        If sender.GetType = GetType(EntPlayer) Then
            Dim player As EntPlayer = sender
            
            player.changeState(Me.state)

            If PickupSound IsNot Nothing
                Me.PickupSound.Play()
            End If

            MyScene.RemoveEntity(Me)
        End If
    End Sub

                ''' <summary>
            ''' Adds a new instance into the scene
	''' <summary/>
    Public Sub Spawn(scene As Scene)
        scene.AddEntity(Me)
    End Sub

    Sub New(width As Integer, height As Integer, location As Point, scene As Scene)
        MyBase.New(width, height, location, scene)
    End Sub

End Class
