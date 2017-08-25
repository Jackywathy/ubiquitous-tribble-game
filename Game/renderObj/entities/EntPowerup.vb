
Public MustInherit Class EntPowerup
    Inherits Entity

    Public isSpawning = True

    ' The state the powerup changes the player to
    Public MustOverride Property state As UInt16
    Public MustOverride Property PickupSound As MusicPlayer
    Public Overrides Property RenderImage As Image = Me.SpriteSet(SpriteState.Spawn)(0)
    Private spawnCounter = 0

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

            player.State = Me.state

            If PickupSound IsNot Nothing Then
                Me.PickupSound.Play()
            End If

            MyScene.PrepareRemove(Me)
        End If
    End Sub

    Public Overrides Sub animate()
        ' Dividing by animationInterval slows down the animation:
        '
        ' Without division:
        '   Counter 0 1 2 3 4 5 6
        '   Mod 7   0 1 2 3 4 5 6
        '
        ' With division:
        '   Counter 0 1 2 3 4 5 6
        '   Divisn. 0 0 0 0 0 1 1
        '   Mod 7   0 0 0 0 0 1 1
        '
        If (Math.Floor(spawnCounter / animationInterval) Mod 7) = Me.SpriteSet(SpriteState.Spawn).Count - 1 Then
            isSpawning = False
            Me.RenderImage = Me.SpriteSet(SpriteState.ConstantRight)(0)
        ElseIf isSpawning Then
            Me.spawnCounter += 1
            Me.RenderImage = Me.SpriteSet(SpriteState.Spawn)(Math.Floor(spawnCounter / animationInterval) Mod 7)
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
