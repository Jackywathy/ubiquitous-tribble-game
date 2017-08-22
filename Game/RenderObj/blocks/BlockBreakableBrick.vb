Public Class BlockBreakableBrick
    Inherits Block

    Public isMoving As Boolean = False
    Public defaultLocationY As Integer
    Public frameCount As Integer = 0

    Public Sub New(location As Point, scene As Scene)
        MyBase.New(32, 32, location, Sprites.brickBlock, scene)
        Me.defaultLocationY = location.Y
    End Sub

    Public Overrides Sub animate()
        If isMoving Then
            ' bumps block
            Me.frameCount += 1
            Dim x = frameCount / animationInterval

            ' Use displacement/time function
            ' f(x) = 6(2x - x^2)
            '      = -6x^2 + 12x

            '    y ^  ____
            '      | /    \
            '      |/      \ x-intercept at 2
            ' -----+--------o------> x
            '     /|         \

            Dim heightFunc = 6 * (2 * (x) - (x * x))
            Me.Location = New Point(Me.Location.X, defaultLocationY + heightFunc)

            ' f(x) = 0 when x = 2
            If frameCount / animationInterval >= 2 Then
                Me.isMoving = False
            End If
        End If

    End Sub

    ''' <summary>
    ''' 0 : x
    ''' 0 : y
    ''' </summary>
    ''' <param name="params"></param>
    Public Sub New(params As Object(), scene As Scene)
        Me.New(New Point(params(0), params(1)), scene)
    End Sub

    Public Overrides Sub CollisionBottom(sender As Entity)
        MyBase.CollisionBottom(sender)
        If sender.GetType = GetType(EntPlayer) Then
            Dim player As EntPlayer = sender
            If player.State > PlayerStates.Small Then
                MyScene.PrepareRemove(Me)
                Sounds.BrickSmash.Play()
            Else
                ' bump block
                frameCount = 0
                Me.isMoving = True

            End If
        Else

        End If
    End Sub
End Class