Public Class EntFireball
    Inherits Entity

    Public Overrides Property moveSpeed As Distance = New Distance(3, 10)
    Public Overrides ReadOnly Property maxVeloc As Distance = New Distance(8, Forces.terminalVeloc)

    Public owner As EntPlayer
    Public willDestroy = False
    Public destroyAnimationComplete = False

    Sub New(width As Integer, height As Integer, location As Point, direction As Integer, shooter As EntPlayer, scene As Scene)
        MyBase.New(width, height, location, Sprites.playerFireball, scene)
        Me.moveSpeed = New Distance(Me.moveSpeed.x * direction, Me.moveSpeed.y)
        Me.owner = shooter
        Me.isGrounded = False
    End Sub

    Public Overrides Sub Animate()
        If Not willDestroy Then
            Me.renderImage = SpriteSet(SpriteState.Constant)(0)
            renderImage.RotateFlip(RotateFlipType.Rotate90FlipNone)
        Else
            If Math.Floor(MyScene.frameCount / animationInterval) = 3 Then
                destroyAnimationComplete = True
            Else
                Me.renderImage = SpriteSet(SpriteState.Destroy)(Math.Floor(MyScene.frameCount / animationInterval))
                Console.WriteLine(Me.Height)
            End If
        End If
    End Sub

    Public Overrides Sub UpdatePos()
        If IsOutOfMap() <> Direction.None
            Me.Destroy()
        End If

        If Not willDestroy Then
            Me.AccelerateX(moveSpeed.x)

            If Me.isGrounded Then
                Me.AccelerateY(moveSpeed.y, False)
            End If

            MyBase.UpdatePos()
        Else
            If destroyAnimationComplete Then
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
