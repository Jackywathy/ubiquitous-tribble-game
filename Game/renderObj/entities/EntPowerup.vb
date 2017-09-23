Public MustInherit Class EntPowerup
    Inherits Entity

    Friend IsSpawning As Boolean = True

    Public MustOverride ReadOnly Property PickupScore As Integer

    ' The state the powerup changes the player to
    Public MustOverride Property PickupSound As MusicPlayer

    Private spawnCounter As Integer = 0
    Private isUsed As Boolean = False
    ''' <summary>
    ''' Run when player hits the powerup
    ''' By default, plays a sound, adds score and removes itself
    ''' </summary>
    ''' <param name="sender"></param>
    Public Overridable Sub Activate(sender As EntPlayer)
        sender.Score += PickupScore
        If PickupSound IsNot Nothing Then

            Me.PickupSound.Play()

        End If
        MyScene.PrepareRemove(Me)

    End Sub

    Public Overrides Sub Animate()
        ' Dividing by AnimationInterval slows down the animation:
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
        If (Math.Floor(spawnCounter / AnimationInterval) Mod 7) = Me.SpriteSet(SpriteState.Spawn).Count - 1 Then
            isSpawning = False
            Me.RenderImage = Me.SpriteSet(SpriteState.ConstantRight)(0)
        ElseIf isSpawning Then
            Me.spawnCounter += 1
            Me.RenderImage = Me.SpriteSet(SpriteState.Spawn)(Math.Floor(spawnCounter / AnimationInterval) Mod 7)
        End If
    End Sub

    ''' <summary>
    ''' Adds a new instance into the mapScene
	''' </summary>
    Public Sub Spawn(Optional skipAnimation As Boolean = False)
        If skipAnimation Then
            Me.IsSpawning = False
        End If
        MyScene.PrepareAdd(Me)

    End Sub

    Sub New(width As Integer, height As Integer, location As Point, spriteSet As SpriteSet, mapScene As MapScene)
        MyBase.New(width, height, location, spriteSet, mapScene)
    End Sub

#Region "collisions"
    Public Overrides Sub CollisionBottom(sender As Entity)
        If Helper.IsPlayer(sender) And Not isUsed Then
            Me.Activate(sender)
            isUsed = True
        End If
    End Sub
    Public Overrides Sub CollisionTop(sender As Entity)
        If Helper.IsPlayer(sender) And Not isUsed  Then
            Me.Activate(sender)
            isUsed = True
        End If
    End Sub
    Public Overrides Sub CollisionLeft(sender As Entity)
        If Helper.IsPlayer(sender) And Not isUsed Then
            Me.Activate(sender)
            isUsed = True
        End If
    End Sub
    Public Overrides Sub CollisionRight(sender As Entity)
        If Helper.IsPlayer(sender) And Not isUsed  Then
            Me.Activate(sender)
            isUsed = True
        End If
    End Sub
#End Region

End Class
