
Public MustInherit Class EntPowerup
    Inherits Entity

    Public isSpawning = True

    ' The state the powerup changes the player to
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

            player.setState(Me.state)

            If PickupSound IsNot NothingThen
                Me.PickupSound.Play()
            End If

            MyScene.PrepareRemove(Me)
        End If
    End Sub

    ''' <summary>
    ''' Adds a new instance into the scene
	''' </summary>
    Public Sub Spawn()
        MyScene.PrepareAdd(Me)
    End Sub

    Sub New(width As Integer, height As Integer, location As Point, spriteSet As SpriteSet, scene As Scene)
        MyBase.New(width, height, location, spriteSet, scene)
    End Sub

End Class
