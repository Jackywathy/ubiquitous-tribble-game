Public Class EntFireball
    Inherits Entity

    Public Overrides Property moveSpeed As Distance = New Distance(3, 15)
    Public Overrides ReadOnly Property maxVeloc As Distance = New Distance(8, Forces.terminalVeloc)
    Public Overrides Property spriteSet As SpriteSet = Sprites.playerFireball



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
            Me.isGrounded = False
        End If

        MyBase.UpdatePos()

    End Sub

    Sub New(width As Integer, height As Integer, location As Point, direction As Integer, shooterIsGrounded As Boolean, scene As Scene)
        MyBase.New(width, height, location, scene)
        Me.moveSpeed = New Distance(Me.moveSpeed.x * direction, Me.moveSpeed.y)
        Me.isGrounded = shooterIsGrounded
    End Sub

End Class
