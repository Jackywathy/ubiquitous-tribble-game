Public Class BlockBreakableBrick
    Inherits Block


    ''' <summary>
    ''' TODO make theme work!
    ''' </summary>
    ''' <param name="location"></param>
    ''' <param name="theme"></param>
    ''' <param name="mapScene"></param>
    Public Sub New(location As Point, theme As RenderTheme, mapScene As MapScene)
        MyBase.New(32, 32, location, Sprites.brickBlock, mapScene)

    End Sub

    Public Overrides Sub animate()
        If isMoving Then
            ' bumps block
            Me.frameCount += 1
            Dim x = frameCount / animationInterval

            ' Use displacement/time function
            ' f(x) = 6(2x - x^2)
            '      = -6x^2 + 12x

            '    y ^  
            '      |  .--.
            '      | /    \
            '      |/      \  x-intercept at 2
            ' -----+--------o------> x
            '     /|         \
            '    V            V

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
    Public Sub New(params As Object(), theme As RenderTheme, mapScene As MapScene)
        Me.New(New Point(params(0)*32, params(1)*32), theme, mapScene)
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
        End If
    End Sub
End Class
