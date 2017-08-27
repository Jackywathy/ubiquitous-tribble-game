Imports System.Drawing.Drawing2D
''' <summary>
''' Main Form that is run on startup - this is controlled using <see cref="MapScene"/> objects
''' </summary>
Public Class MainGame
    Public Shared Property CurrentMapScene As BaseScene

    ''' <summary>
    ''' Debug buffer - this is written to top right of mapScene each tick, only if IsDebug is set to False in Helper.vb
    ''' </summary>
    Private Readonly strBuffer As New List(Of String)

    ''' <summary>
    ''' Constructor for 
    ''' </summary>
    Sub New()
        ' This call is required by the designer.
        InitializeComponent()
        SetStyle(ControlStyles.DoubleBuffer, True)
        SetStyle(ControlStyles.UserPaint, True)
        SetStyle(ControlStyles.OptimizedDoubleBuffer, True)
        SetStyle(ControlStyles.AllPaintingInWmPaint, True)
    End Sub

    Private Sub GameLoop_Tick(sender As Object, e As EventArgs) Handles GameLoop.Tick
        CurrentMapScene.handleInput()
        CurrentMapScene.UpdateTick()
        Me.Refresh()
    End Sub



    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        MyBase.OnPaint(e)
        Dim g = e.Graphics
        g.InterpolationMode = InterpolationMode.NearestNeighbor
        
        CurrentMapScene.RenderScene(g)
        UpdateFPS()

        if isDebug And CurrentMapScene.player1 IsNot Nothing
            AddStringBuffer(String.Format("fps: {0}", FPS))
            AddStringBuffer(String.Format("Mario Location: {0}, {1}", CurrentMapScene.player1.Location.X, CurrentMapScene.player1.Location.Y))
            DrawStringBuffer(g)
        End if


    End Sub

   


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
        CurrentMapScene = MapScene.ReadMapFromResource("map_testmap")

        MusicPlayer.PlayBackground(BackgroundMusic.GroundTheme)
        ' only start loop after init has finished
        GameLoop.Enabled = True
    End Sub

    Public Class KeyHandler
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
End Class
