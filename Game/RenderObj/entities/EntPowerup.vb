
Public MustInherit Class EntPowerup
    Inherits Entity

    Public isSpawning = True

    ' The state the powerup changes the player to
    Public MustOverride Property state As UInt16
    Public MustOverride Property PickupSound As MusicPlayer
    Public Overrides Property RenderImage As Image = Me.SpriteSet(SpriteState.Spawn)(0)

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

            If PickupSound IsNot Nothing Then
                Me.PickupSound.Play()
            End If

            MyScene.PrepareRemove(Me)
        End If
    End Sub

    Public Overrides Sub animate()
        If Math.Floor(internalFrameCounter / animationInterval) = Me.SpriteSet(SpriteState.Spawn).Count - 1 Then
            isSpawning = False
            Me.RenderImage = Me.SpriteSet(SpriteState.Constant)(0)
        ElseIf isSpawning Then
            Me.RenderImage = Me.SpriteSet(SpriteState.Spawn)(Math.Floor(internalFrameCounter / animationInterval))
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
