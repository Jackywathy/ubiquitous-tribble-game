Public Class EntFireball
    Inherits Entity

    Public Overrides Property moveSpeed As Distance = New Distance(3, 4)
    Public Overrides Property maxVeloc As Distance = New Distance(8, Forces.terminalVeloc)

    Public owner As EntPlayer
    Public willDestroy = False
    Public destroyTimer As Integer = 0

    Sub New(width As Integer, height As Integer, location As Point, direction As Integer, shooter As EntPlayer, mapScene As MapScene)
        MyBase.New(width, height, location, Sprites.playerFireball, mapScene)
        Me.moveSpeed = New Distance(Me.moveSpeed.x * direction, Me.moveSpeed.y)
        Me.owner = shooter
        Me.isGrounded = False
        Me.killsOnContact = True
    End Sub

    Public Overrides Sub Animate()
        If Not willDestroy Then
            Me.renderImage = SpriteSet(SpriteState.ConstantRight)(0)
            renderImage.RotateFlip(RotateFlipType.Rotate90FlipNone)
        Else
            Me.renderImage = SpriteSet(SpriteState.Destroy)(Math.Floor(MyScene.GlobalFrameCount / animationInterval) Mod 3)
            Me.destroyTimer += 1
        End If
    End Sub

    Public Overrides Sub UpdateVeloc()
        If IsOutOfMap() <> Direction.None
            Me.Destroy()
        End If

        If Not willDestroy Then
            Me.AccelerateX(moveSpeed.x)

            If Me.isGrounded Then
                Me.AccelerateY(moveSpeed.y, False)
            End If

            MyBase.UpdateVeloc()
        Else
            If Me.destroyTimer >= 3 Then
                MyBase.Destroy()
            End If
        End If
    End Sub

    Public Overrides Sub Destroy()

        If Not willDestroy Then
            owner.NumFireballs -= 1
            Me.Width = 32
            Me.Height = 32
        End If
        willDestroy = True


    End Sub



End Class
