Public Class EntStar
    Inherits EntPowerup

    Public Overrides Property moveSpeed As Distance = New Distance(0.4, 5)
    Public Overrides Property maxVeloc As Distance = New Distance(3, Forces.terminalVeloc)

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

    Public Overrides Sub Animate()
        If Not isSpawning And MyScene.GlobalFrameCount Mod (AnimationInterval) = 0 Then
            Me.RenderImage = Me.SpriteSet.SendToBack(SpriteState.ConstantRight)
        ElseIf isSpawning Then
            If (Math.Floor(spawnCounter / AnimationInterval) Mod 7) = Me.SpriteSet(SpriteState.Spawn).Count - 1 Then
                isSpawning = False
                Me.RenderImage =Me.SpriteSet(SpriteState.ConstantRight)(0)
            Else
                Me.spawnCounter += 1
                Me.RenderImage = Me.SpriteSet(SpriteState.Spawn)(Math.Floor(spawnCounter / AnimationInterval) Mod 7)
            End If

        End If
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
