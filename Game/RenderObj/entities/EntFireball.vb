Public Class EntFireball
    Inherits Entity

    Public Overrides Property moveSpeed As Distance = New Distance(3, 10)
    Public Overrides ReadOnly Property maxVeloc As Distance = New Distance(8, Forces.terminalVeloc)

    Public owner As EntPlayer
    Public willDestroy = False

    Sub New(width As Integer, height As Integer, location As Point, direction As Integer, shooter As EntPlayer, scene As Scene)
        MyBase.New(width, height, location, Sprites.playerFireball, scene)
        Me.moveSpeed = New Distance(Me.moveSpeed.x * direction, Me.moveSpeed.y)
        Me.owner = shooter
        Me.isGrounded = False
    End Sub

    Public Overrides Sub Animate(numFrames As Integer)
        If numFrames Mod 5 = 0 Then
            Me.RenderImage = spriteSet.allSprites(0)(0)
            spriteSet.allSprites(0)(0).RotateFlip(RotateFlipType.Rotate90FlipNone)
        End If
    End Sub

    Public Overrides Sub UpdatePos()
        Me.AccelerateX(moveSpeed.x)

        If Me.isGrounded Then
            Me.AccelerateY(moveSpeed.y)
            'Me.isGrounded = False
        End If

        MyBase.UpdatePos()

    End Sub

    Public Overrides Sub Destroy()

        If Not willDestroy Then
            owner.numFireballs -= 1
        End If
        willDestroy = True
        MyBase.Destroy()
    End Sub



End Class
