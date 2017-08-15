
Public MustInherit Class EntPowerup
    Inherits Entity

    Public isSpawning = True

    ' The state the powerup changes the player to
    ' 0 - small
    ' 1 - big 
    ' 2 - fire 
    Public MustOverride Property state As UInt16
    Public MustOverride Property PickupSound As MusicPlayer

    Public Overrides Sub CollisionBottom(sender As Entity, scene As Scene)
        Me.TryActivate(sender, scene)
    End Sub
    Public Overrides Sub CollisionTop(sender As Entity, scene As Scene)
        Me.TryActivate(sender, scene)
    End Sub
    Public Overrides Sub CollisionLeft(sender As Entity, scene As Scene)
        Me.TryActivate(sender, scene)
    End Sub
    Public Overrides Sub CollisionRight(sender As Entity, scene As Scene)
        Me.TryActivate(sender, scene)
    End Sub

                ''' <summary>
            ''' Tries to activate Me on sender. Does not work if sender is not a Player.
	''' <summary/>
    Public Sub TryActivate(sender As Entity, scene As Scene)
        If sender.GetType = GetType(EntPlayer) Then
            Dim player As EntPlayer = sender
            
            player.changeState(Me.state)

            If PickupSound IsNot Nothing
                Me.PickupSound.Play()
            End If

            scene.RemoveEntity(Me)
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
