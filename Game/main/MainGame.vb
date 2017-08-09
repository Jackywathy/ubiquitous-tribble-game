Imports System.Drawing.Drawing2D
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

    Private Property SceneController As New Scene


   
    

    Sub New()
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        
        DoubleBuffered = True
        SetStyle(ControlStyles.UserPaint, True)
        SetStyle(ControlStyles.OptimizedDoubleBuffer, True)
        SetStyle(ControlStyles.AllPaintingInWmPaint, True)

        SceneController.LoadTestLevel()


        MusicPlayer.PlayBackground(BackgroundMusic.Overworld)
    End Sub

    Private JumpEffect As New MusicPlayer("jump")

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        MyBase.OnPaint(e)
        Dim g As Graphics = e.Graphics
        g.InterpolationMode = InterpolationMode.NearestNeighbor
        SceneController.RenderScene(g)
        DrawFps(g)
    End Sub

    Private numFrames As Integer = 0
    Private FPS As Integer = 0
    Private ReadOnly fpsFont as New Font("Arial", 20)
    Private ReadOnly fpsBrush As New SolidBrush(Color.Black)
    Private lastFrame As DateTime = DateTime.Now()

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
        Entities.player1.Move(numFrames)

        Me.Refresh()
    End Sub

    Private Sub handleInput()
        If KeyHandler.MoveUp Then
            Entities.player1.accelerateY(Entities.player1.moveSpeed.y)
        End If
        If KeyHandler.MoveDown Then

        End If
        If KeyHandler.MoveLeft Then
            Entities.player1.accelerateX(-Entities.player1.moveSpeed.x)
        End If
        If KeyHandler.MoveRight Then
            Entities.player1.AccelerateX(Entities.player1.moveSpeed.x)

        End If

        Entities.player1.ApplyConstantForces()

        ' Temporary "Ground"
        If Entities.player1.Location.Y < 50 Then
            Entities.player1.Location = New Point(Entities.player1.Location.X, 50)
            Entities.player1.isGrounded = True
        End If

    End Sub

    Private Sub MainGame_KeyUp(sender As Object, e As KeyEventArgs) Handles MyBase.KeyUp
        KeyHandler.KeyUp(e.KeyCode)
        
    End Sub

    Private Sub MainGame_Resize(sender As Object, e As EventArgs) Handles MyBase.Resize
        KeyHandler.Reset()
    End Sub

    Dim y As MusicPlayer()
    Private Sub MainGame_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        Dim key As Keys = e.KeyData
        KeyHandler.KeyDown(key)
        If key = Keys.W Or key = Keys.Up
            JumpEffect.Play(True)
        End If
        

    End Sub

    Private Sub MainGame_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class
