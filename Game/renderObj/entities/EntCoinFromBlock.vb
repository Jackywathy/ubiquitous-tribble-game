Public Class EntCoinFromBlock

    Inherits Entity

    Public willDisappear As Boolean = False
    Public defaultY As Integer

    Sub New(width As Integer, height As Integer, location As Point, scene As Scene)
        MyBase.New(width, height, location, Sprites.coinFromBlock, scene)
        Me.renderImage = Me.SpriteSet(SpriteState.ConstantRight)(0)
        Me.defaultY = Me.Location.Y
    End Sub

    Public Overrides Sub animate()
        Dim index = Math.Floor(frameCount / animationInterval) Mod 4
        Me.renderImage = Me.SpriteSet(SpriteState.ConstantRight)(index)
        If index = 3 Then
            Me.willDisappear = True
        End If
    End Sub

    Public Overrides Sub UpdatePos()
        Me.frameCount += 1
        Me.Location = New Point(Me.Location.X, Me.defaultY + (frameCount * 4))
        If Me.willDisappear Then
            Me.Destroy()
        End If
    End Sub
End Class
