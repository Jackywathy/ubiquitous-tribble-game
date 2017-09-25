Imports System.Drawing.Drawing2D

''' <summary>
''' This control has the entire game in it!
''' </summary>
<ComponentModel.DesignerCategory("")>
Public Class GameControl
    Inherits Control
    Friend WithEvents GameLoop As New Timer
    Private Const HudHeightPercent = 0.1

    Public SharedHud As New StaticHud(ScreenGridWidth, ScreenGridHeight * HudHeightPercent)

    ''' <summary>
    ''' Gameloop - runs 60ish times a second, causing inputs/game to tick
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub GameLoop_Tick(sender As Object, e As EventArgs) Handles GameLoop.Tick
        CurrentScene.HandleInput()
        CurrentScene.UpdateTick()
        Me.Refresh()
    End Sub

    ''' <summary>
    ''' Paints to the screen using a graphics object
    ''' </summary>
    ''' <param name="e"></param>
    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        MyBase.OnPaint(e)
        Dim g = e.Graphics
        g.InterpolationMode = InterpolationMode.NearestNeighbor

        CurrentScene.RenderScene(g)
        

#If DEBUG Then
        updateFps()
        Me.DrawDebugStrings()
        CurrentScene.DrawDebugStrings(Me)

        DrawStringBuffer(g)
#End If

    End Sub


    ''' <summary>
    ''' Constructor for <see cref="GameControl"/>
    ''' </summary>
    Sub New()
        InitalizeComponent()
        ' enable double buffering and custom paint
        Randomize()

        allMapScenes = LoadScenes()
        CurrentScene = allMapScenes("map2_1")
        '"map1_1", 

        ' only start loop after init has finished
        GameLoop.Enabled = True
    End Sub

    ''' <summary>
    ''' Holds the currently loaded scene
    ''' </summary>
    ''' <returns></returns>
    Private Property CurrentScene As BaseScene

    ''' <summary>
    ''' All the names of the JsonMaps as resource name
    ''' </summary>
    Private ReadOnly JsonMaps As New List(Of String) From {"map1_1", "map2_1"}

    ''' <summary>
    ''' Debug buffer - this is written to top right of mapScene each tick, only if IsDebug is set to False in Helper.vb
    ''' </summary>
    Private ReadOnly strBuffer As New List(Of String)

    ''' <summary>
    ''' Handles/stores all the keys being pressed
    ''' </summary>
    Private ReadOnly keyControl As New KeyHandler()

    ''' <summary>
    ''' Contains all Scenes
    ''' </summary>
    Private Readonly property allMapScenes As Dictionary(Of String, MapScene)

    ''' <summary>
    ''' Initalize components inside
    ''' </summary>
    Private Sub InitalizeComponent()
        GameLoop.Interval = 15
        SetStyle(ControlStyles.DoubleBuffer, True)
        SetStyle(ControlStyles.UserPaint, True)
        SetStyle(ControlStyles.OptimizedDoubleBuffer, True)
        SetStyle(ControlStyles.AllPaintingInWmPaint, True)
    End Sub



    Public Sub DrawDebugStrings()
        AddStringBuffer(String.Format("fps: {0}", FPS))
    End Sub

    ''' <summary>
    ''' Adds string to debug. buffer
    ''' </summary>
    ''' <param name="str"></param>
    Public Sub AddStringBuffer(str As String)
        strBuffer.Add(str)
    End Sub 


    ''' <summary>
    ''' Draws string buffer onto graphics obj
    ''' </summary>
    ''' <param name="g"></param>
    Private Sub DrawStringBuffer(g As Graphics)
        Dim height = 0
        For Each str As String In strBuffer
            Dim size = TextRenderer.MeasureText(str, fpsFont)
            Dim offset = ScreenGridWidth - size.Width - 5
            g.DrawString(str, fpsFont, fpsBrush, offset, height)
            height += size.Height
        Next
        strBuffer.Clear()
    End Sub


    Private numFrames As Integer = 0
    Private FPS As Integer = 0
    Private ReadOnly fpsFont As New Font("Arial", 20)
    Private ReadOnly fpsBrush As New SolidBrush(Color.Red)
    Private lastFrame As DateTime = Date.UtcNow()

    Private Sub updateFps()
        Dim now = Date.UtcNow()
        If (now - lastFrame).Seconds >= 1 Then
            FPS = numFrames
            numFrames = 0
            lastFrame = now
        End If
        numFrames += 1
    End Sub


    Private Sub MainGame_KeyUp(sender As Object, e As KeyEventArgs) Handles MyBase.KeyUp
        keyControl.KeyUp(e.KeyCode)
    End Sub

    Private Sub MainGame_Resize(sender As Object, e As EventArgs) Handles MyBase.Resize
        keyControl.Reset()
    End Sub

    Private Sub MainGame_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        keyControl.KeyDown(e.KeyData)
    End Sub

    ''' <summary>
    ''' Returns a dictionary of all maps loaded 
    ''' </summary>
    ''' <returns></returns>
    Private Function LoadScenes() As Dictionary(Of String, MapScene)
        Dim scenes As New Dictionary(Of String, MapScene)
        For Each str As String In JsonMaps
            scenes.Add(str, JsonMapReader.ReadMapFromResource(str, Me))
        Next
        Return scenes
    End Function

    Public Sub RunScene()

    End Sub 

    Private Sub GameControl_SizeChanged(sender As Object, e As EventArgs) Handles Me.SizeChanged
        ScreenGridWidth = Width
        ScreenGridHeight = Height
    End Sub


End Class

Public Class KeyHandler
    Public Shared MoveRight As Boolean
    Public Shared MoveLeft As Boolean
    Public Shared MoveUp As Boolean
    Public Shared MoveDown As Boolean
    Private Sub KeyHelp(key As Keys, vset As Boolean)
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

    Public Sub KeyDown(key As Keys)
        KeyHelp(key, True)
    End Sub

    Public Sub KeyUp(key As Keys)
        KeyHelp(key, False)
    End Sub

    Public Sub Reset()
        MoveRight = False
        MoveLeft = False
        MoveUp = False
        MoveDown = False
    End Sub
End Class


Public Module MainProgram
    Public Sub Main()
        Application.EnableVisualStyles()
        Application.SetCompatibleTextRenderingDefault(False)
        Application.Run(New FormBootStrap)
    End Sub
End Module