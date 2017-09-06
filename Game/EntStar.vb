Imports WinGame

Public Class EntStar
    Inherits EntPowerup

    Public Overrides Property moveSpeed As Distance = New Distance(3, 5)
    Public Overrides Property maxVeloc As Distance = New Distance(6, Forces.terminalVeloc)

    Public Overrides Property state As UShort = 5
    Private spawnCounter = 0
    Public Overrides Property PickupSound As MusicPlayer
        Get
            Throw New NotImplementedException()
        End Get
        Set(value As MusicPlayer)
            Throw New NotImplementedException()
        End Set
    End Property

    Public Overrides Sub TryActivate(sender As Entity)
        If sender.GetType = GetType(EntPlayer) Then
            Dim player As EntPlayer = sender

            player.InvinicibilityTimer = player.StarInvincibilityDuration

            If PickupSound IsNot Nothing Then
                Me.PickupSound.Play()
            End If

            MyScene.PrepareRemove(Me)
        End If
    End Sub

    Public Overrides Sub animate()
        If Not isSpawning And MyScene.FrameCount Mod (animationInterval) = 0 Then
            Me.RenderImage = Me.SpriteSet.SendToBack(SpriteState.ConstantRight)
        ElseIf isSpawning Then
            If (Math.Floor(spawnCounter / animationInterval) Mod 7) = Me.SpriteSet(SpriteState.Spawn).Count - 1 Then
                isSpawning = False
                Me.RenderImage = Me.SpriteSet(SpriteState.ConstantRight)(0)
            Else
                Me.spawnCounter += 1
                Me.RenderImage = Me.SpriteSet(SpriteState.Spawn)(Math.Floor(spawnCounter / animationInterval) Mod 7)
            End If

        End If
    End Sub

    Public Overrides Sub UpdatePos()
        If IsOutOfMap() <> Direction.None Then
            Me.Destroy()
        End If

        Me.AccelerateX(moveSpeed.x)

        If Me.isGrounded Then
            Me.AccelerateY(moveSpeed.y, False)
        End If

        MyBase.UpdatePos()
    End Sub

    Sub New(width As Integer, height As Integer, location As Point, mapScene As MapScene)
        MyBase.New(width, height, location, Sprites.star, mapScene)
    End Sub

End Class
