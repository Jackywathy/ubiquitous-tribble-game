Public Class EntCoinFromBlock
    Inherits Entity

    Public willDisappear As Boolean = False
    Public defaultY As Integer

    Sub New(width As Integer, height As Integer, location As Point, mapScene As MapScene)
        MyBase.New(width, height, location, Sprites.coinFromBlock, mapScene)
        Me.RenderImage = Me.SpriteSet(SpriteState.ConstantRight)(0)
        Me.defaultY = Me.Location.Y
    End Sub

    Public Overrides Sub Animate()
        Dim index As Integer = Math.Floor(framesSinceHit / animationInterval) Mod 4
        Me.RenderImage = Me.SpriteSet(SpriteState.ConstantRight)(index)
        If index = 3 Then
            Me.willDisappear = True
        End If
    End Sub

    Public Overrides Sub UpdateVeloc()
        Me.framesSinceHit += 1
        Me.Location = New Point(Me.Location.X, Me.defaultY + (framesSinceHit * 4))
        If Me.willDisappear Then
            Me.Destroy()
        End If
    End Sub

    Public Sub Spawn()
        MyScene.PrepareAdd(Me)
    End Sub
End Class
