Public MustInherit Class BlockBumpable
    Inherits Block
    Friend IsMoving As Boolean = False

    

    ' Takes a raw frame counter and returns a new point
    Friend Function bounceFunction(x As Integer) As Point
        x /= animationInterval
        Dim heightFunc = 6 * (2 * (x) - (x * x))

        ' f(x) = 0 when x = 2
        If framesSinceHit / animationInterval >= 2 Then
            ResetBump()
            Return New Point(Me.Location.X, defaultLocationY)
        Else
            Return New Point(Me.Location.X, defaultLocationY + heightFunc)
        End If
    End Function

    Public Sub New(width As Integer, height As Integer, location As Point, spriteSet As SpriteSet, mapScene As MapScene)
        MyBase.New(width, height, location, spriteSet, mapScene)
    End Sub


    Friend Sub StartBump()
        IsMoving = True
    End Sub

    Public Overridable Sub ResetBump()
        Me.isMoving = False
        Me.framesSinceHit = 0
    End Sub

    Public Overrides Sub UpdateItem()
        If isMoving Then
            ' bumps block
            Me.framesSinceHit += 1
            Me.Location = bounceFunction(framesSinceHit)
        End If
    End Sub
    
End Class
