Imports System.Drawing.Drawing2D

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
        Dim g = e.Graphics
        g.InterpolationMode = InterpolationMode.NearestNeighbor
        SceneController.RenderScene(g)
        UpdateFPS()

        if isDebug
            AddStringBuffer(String.Format("fps: {0}", FPS))
            AddStringBuffer(String.Format("Mario Location: {0}, {1}", player1.Location.X, player1.Location.Y))

            DrawStringBuffer(g)
        End if
    End Sub

    Private strBuffer As New List(Of String)
    Public Sub AddStringBuffer(str As String)
        strBuffer.Add(str)
    End Sub


    Const WidthError = 5
    Public Sub DrawStringBuffer(g As Graphics)
        Dim height as Integer = 0
        For each str As String In strBuffer
            Dim size = TextRenderer.MeasureText(str, fpsFont)
            Dim offset = ScreenGridWidth - size.Width - WidthError
            g.DrawString(str, fpsFont, fpsBrush,offset , height)
            height += size.Height
        Next
        strBuffer.Clear()
    End Sub


    Private numFrames As Integer = 0
    Private FPS As Integer = 0
    Private ReadOnly fpsFont as New Font("Arial", 20)
    Private ReadOnly fpsBrush As New SolidBrush(Color.Red)
    Private lastFrame As DateTime = DateTime.Now()
    

    Private Sub UpdateFPS()
        Dim now = DateTime.Now()
        If (Now-lastFrame).Seconds >= 1 Then
            fps = numFrames
            numFrames = 0
            lastFrame = now
        End If
        numFrames += 1
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
