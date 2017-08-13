Imports System.Drawing.Drawing2D

Public Class MainGame
    Private Class KeyHandler
        Public Shared MoveRight As Boolean
        Public Shared MoveLeft As Boolean
        Public Shared MoveUp As Boolean
        Public Shared MoveDown As Boolean

        Private Shared Sub KeyHelp(key As Keys, vset As Boolean)

            If key = Keys.Right Or key = Keys.D Then
                MoveRight = vset

            End If

            If key = Keys.Left Or key = Keys.A Then
                MoveLeft = vset
            End If

            If key = Keys.Up Or key = Keys.W Then
                MoveUp = vset
            End If

            If key = Keys.Down Or key = Keys.S Then
                MoveDown = vset
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
    

    Public Shared Property SceneController As Scene

    Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' AddObj any initialization after the InitializeComponent() call.

        SetStyle(ControlStyles.DoubleBuffer, True)
        SetStyle(ControlStyles.UserPaint, True)
        SetStyle(ControlStyles.OptimizedDoubleBuffer, True)
        SetStyle(ControlStyles.AllPaintingInWmPaint, True)

        SceneController = Scene.ReadMapFromResource("map_testmap")


        MusicPlayer.PlayBackground(BackgroundMusic.GroundTheme)
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
        Dim height = 0
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
    Private lastFrame As DateTime = DateTime.UtcNow()
    

    Private Sub UpdateFPS()
        Dim now = DateTime.UtcNow()
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

    Private Sub handleInput() ' this supplements the player's updatePos()

        ' UP
        If KeyHandler.MoveUp And player1.allowJump Then
            Entities.player1.AccelerateY(Entities.player1.moveSpeed.y)
            player1.allowJump = False
        ElseIf KeyHandler.MoveUp = False Then
            player1.allowJump = True
        End If

        ' DOWN
        If KeyHandler.MoveDown Then
            If player1.state > 0 And player1.isGrounded = True Then 'crouch
                player1.onCrouch(True)
            End If
        ElseIf player1.state > 0 And player1.isCrouching = True Then
            ' TO DO - check for collision on above blocks before uncrouching
            player1.onCrouch(False)
        End If

        If player1.state = 2 And KeyHandler.MoveDown And player1.allowShoot Then
            player1.tryShootFireball()
            player1.allowShoot = False
        ElseIf Not KeyHandler.MoveDown Then
            player1.allowShoot = True
        End If

        ' LEFT
        If KeyHandler.MoveLeft Then
            Entities.player1.AccelerateX(-Entities.player1.moveSpeed.x)
        End If

        ' RIGHT
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


    Private Sub MainGame_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        Dim key As Keys = e.KeyData
        KeyHandler.KeyDown(key)
        If key=Keys.W Or key=keys.Up
            Sounds.jump.Play(fromStart := True)
        End If

    End Sub

    Private Sub MainGame_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        
    End Sub


End Class
