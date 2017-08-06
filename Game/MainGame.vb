Imports System.IO

Public Class MainGame
    Private NotInheritable Class KeyHandler
        Public Shared MoveRight As Boolean
        Public Shared MoveLeft As Boolean
        Public Shared MoveUp As Boolean
        Public Shared MoveDown As Boolean

        Private Shared Sub KeyHelp(key as Keys, vset As boolean)
            if key = keys.Right Or Key = Keys.D
                MoveRight = vset
            End if
            if key = keys.Left Or key = Keys.A
                moveLeft = vset
            End If
       
            If key = keys.Up Or Key=Keys.W
                moveUp = vset
            End If
            if key = keys.Down Or key = Keys.S
                moveDown = vset
            End If
        End Sub

        Public Shared Sub KeyDown(key As Keys)
           KeyHelp(key, True)
        End Sub

        Public Shared Sub KeyUp(key as Keys)
            KeyHelp(key, False)
        End Sub

        Public Shared Sub Reset()
            MoveRight = False
            MoveLeft = False
            MoveUp = False
            MoveDown = False
        End Sub
    End Class

    Private Background As BackgroundRender

    Private lastFrame As DateTime = DateTime.Now()

    Private player = Entities.player

    Private brick As New Block(100, 100, New Point(10, 300))
    Private Platform As New Platform(TotalGridWidth, 50, New Point(0, 0), My.Resources.platform)

    Sub New()
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Background = New BackgroundRender(TotalGridWidth, TotalGridHeight, My.Resources.placeholderLevel)
        DoubleBuffered = True
        SetStyle(ControlStyles.UserPaint, True)
        SetStyle(ControlStyles.OptimizedDoubleBuffer, True)
        SetStyle(ControlStyles.AllPaintingInWmPaint, True)

        MusicPlayer.PlayBackground("overworld")
        MusicPlayer.PlayBackground("underground")


    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        MyBase.OnPaint(e)
        Dim g As Graphics = e.Graphics
        Background.Render(g)
        Platform.Render(g)
        brick.Render(g)
        player.Render(g)
        DrawFps(g)
    End Sub

    Private numFrames As Integer = 0
    Private FPS As Integer = 0
    Private ReadOnly fpsFont as New Font("Arial", 20)
    Private ReadOnly fpsBrush As New SolidBrush(Color.Black)

    Private Sub DrawFps(g As Graphics)
        Dim now = DateTime.Now()
        If (Now-lastFrame).Seconds >= 1 Then
            fps = numFrames
            numFrames = 0
            lastFrame = now
        End If
        numFrames += 1
        Dim framesString = String.Format("{0}", FPS)
        g.DrawString(framesString, fpsFont, fpsBrush, ScreenGridWidth-50,18)
    End Sub

    Private Sub GameLoop_Tick(sender As Object, e As EventArgs) Handles GameLoop.Tick
        handleInput()
        player.Move(numFrames)

        Me.Refresh()
    End Sub

    Private Sub handleInput()
        If KeyHandler.MoveUp Then
            player.accelerateY(player.moveSpeed.y)
        End If
        If KeyHandler.MoveDown Then

        End If
        If KeyHandler.MoveLeft Then
            player.accelerateX(-player.moveSpeed.x)
        End If
        If KeyHandler.MoveRight Then
            player.AccelerateX(player.moveSpeed.x)

        End If

        player.ApplyConstantForces()

        ' Temporary "Ground"
        If player.Location.Y < 50 Then
            player.Location = New Point(player.Location.X, 50)
            player.isGrounded = True
        End If

    End Sub

    Private Sub MainGame_KeyUp(sender As Object, e As KeyEventArgs) Handles MyBase.KeyUp
        KeyHandler.KeyUp(e.KeyCode)
    End Sub

    Private Sub MainGame_Resize(sender As Object, e As EventArgs) Handles MyBase.Resize
        KeyHandler.Reset()
    End Sub

    Private Sub MainGame_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        Dim key As Keys = e.KeyData
        KeyHandler.KeyDown(key)
    End Sub

    Private Sub MainGame_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class
