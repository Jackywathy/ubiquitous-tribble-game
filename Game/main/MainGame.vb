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

    Private Property SceneController As Scene

    Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        DoubleBuffered = True
        SetStyle(ControlStyles.UserPaint, True)
        SetStyle(ControlStyles.OptimizedDoubleBuffer, True)
        SetStyle(ControlStyles.AllPaintingInWmPaint, True)

        SceneController = Scene.ReadMapFromResource("testmap")


        MusicPlayer.PlayBackground("ground_theme")
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        MyBase.OnPaint(e)
        Dim g =e.Graphics
            g.InterpolationMode = InterpolationMode.NearestNeighbor
            SceneController.RenderScene(g)
            DrawFps(g)
            DrawLocation(g, "Mario: ", player1.Location, 36)
            DrawLocation(g, "Platform: ", SceneController.AllObjects(2).Location, 54)
    End Sub

    Public Sub DrawLocation(g As Graphics, str As String, loc As Point, height As Integer)
        ' height is the dist from top of screen
        Dim out = String.Format("{0}: {1}, {2}", str, loc.X, loc.Y)
        Dim offset = ScreenGridWidth - (g.MeasureString(out, fpsFont)).Width - 15
        g.DrawString(out, fpsFont, fpsBrush, offset, height)
    End Sub

    Private numFrames As Integer = 0
    Private FPS As Integer = 0
    Private ReadOnly fpsFont as New Font("Arial", 20)
    Private ReadOnly fpsBrush As New SolidBrush(Color.Red)
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
        SceneController.UpdatePhysics(numFrames)

        Me.Refresh()
    End Sub

    Private Sub handleInput()
        If KeyHandler.MoveUp And player1.allowJump = True Then
            Entities.player1.AccelerateY(Entities.player1.moveSpeed.y)
            player1.allowJump = False
        ElseIf KeyHandler.MoveUp = False Then
            player1.allowJump = True
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

    End Sub

    Private Sub MainGame_KeyUp(sender As Object, e As KeyEventArgs) Handles MyBase.KeyUp
        KeyHandler.KeyUp(e.KeyCode)
        
    End Sub

    Private Sub MainGame_Resize(sender As Object, e As EventArgs) Handles MyBase.Resize
        KeyHandler.Reset()
    End Sub

    Dim jump as New MusicPlayer("jump")

    Private Sub MainGame_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        Dim key As Keys = e.KeyData
        KeyHandler.KeyDown(key)
        If key=Keys.W Or key=keys.Up
            jump.Play(fromStart := True)
        End If

    End Sub

    Private Sub MainGame_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        
    End Sub


End Class
