Imports System.Drawing.Drawing2D
Imports WinGame

''' <summary>
''' This control has the entire game in it!
''' </summary>
<ComponentModel.DesignerCategory("")>
Public Class GameControl
    Inherits Control

    Friend WithEvents GameLoop As New Timer
    Private Const HudHeightPercent = 0.1

    ''' <summary>
    ''' Ticks that the map (including any underground levels etc )
    ''' </summary>
    Public MapTimeCounter As Integer = 0

    Public SharedHud As New StaticHud(ScreenGridWidth, ScreenGridHeight * HudHeightPercent)

    ''' <summary>
    ''' Gameloop - runs 60ish times a second, causing inputs/game to tick
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub GameLoop_Tick(sender As Object, e As EventArgs) Handles GameLoop.Tick
        CurrentScene.HandleInput()
        CurrentScene.UpdateTick(MapTimeCounter)
        ChangeQueue.UpdateTick()
        MapTimeCounter += 1

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
        RunScene(PlayerStartScreen, True)


        ' only start loop after init has finished
        GameLoop.Enabled = True
    End Sub

    ''' <summary>
    ''' Holds the currently loaded scene
    ''' </summary>
    ''' <returns></returns>
    Private Property CurrentScene As BaseScene

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
    Private ReadOnly Property allMapScenes As Dictionary(Of MapEnum, MapScene)

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

    Friend Sub SetScene(baseScene As BaseScene)
        CurrentScene = baseScene
    End Sub

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

    Friend Function GetMap(map As MapEnum) As BaseScene
        Try 
            return Me.allMapScenes(map)
        Catch ex As KeyNotFoundException
            throw New Exception(String.Format("Map ""{0}"" was not found in loaded scenes", map.ToString), ex)
        End Try
    End Function

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
    Private Function LoadScenes() As Dictionary(Of MapEnum, MapScene)
        Dim scenes As New Dictionary(Of MapEnum, MapScene)

        For Each str As String In [Enum].GetNames(GetType(MapEnum))
            str = str.ToLower()
            Dim val = Helper.StrToEnum(Of MapEnum)(str)

            If val = MapEnum.None Or val = MapEnum.StartScene
                Continue For
            End If

            scenes.Add(Helper.StrToEnum(Of MapEnum)(str), JsonMapReader.ReadMapFromResource(str, Me))
        Next
        Return scenes
    End Function

    

    ''' <summary>
    ''' Runs a scene from mapEnum
    ''' </summary>
    ''' <param name="map"></param>
    ''' <param name="isNewStage">Whether or not it resets the timer</param>
    Public Sub RunScene(map As MapEnum, isNewStage As Boolean, optional insertion As Point? = Nothing)
        CurrentScene = allMapScenes(map)

        If CurrentScene.GetType() = GetType(MapScene)
            dim mapScene as MapScene = CurrentScene
            Dim start as Point = If(insertion IsNot Nothing, insertion, mapScene.DefaultLocation)
            mapScene.GetPlayer(MapScene.PlayerId.Player1).Location = start
            If isNewStage
                MapTimeCounter = 0
            End If
            If mapScene.width < ScreenGridWidth
                ' center scene
                mapScene.Center()

            End If
        End If

        
    End Sub

    Private Sub GameControl_SizeChanged(sender As Object, e As EventArgs) Handles Me.SizeChanged
        ScreenGridWidth = Width
        ScreenGridHeight = Height
    End Sub

    
    Public MustInherit Class QueueObject
        Friend time As integer
        Friend control as GameControl
        Friend [next] As QueueObject

        Public Property IsFinished As Boolean

        Public Sub New(time As Integer, control As GameControl, Optional [next] as QueueObject = Nothing)
            Me.time = time
            Me.control = control
            Me.[next] = [next]
        End Sub

        Public Sub UpdateTick()
            if time = 0
                TimerFinished()
                IsFinished = True
                if [next] IsNot nothing
                    control.AddTimedEvent([next])
                End If
            End If
            time -= 1
        End Sub
        Protected MustOverride Sub TimerFinished()
    End Class

    Private Sub AddTimedEvent(item As QueueObject)
        ChangeQueue.Add(item)
    End Sub


    Public Class MapTransitionObject
        Inherits QueueObject
        Private transition as TransitionObject

        Sub New(transition As TransitionObject, time As Integer, control As GameControl, Optional [next] as QueueObject = Nothing)
            MyBase.New(time, control, [next])
            me.transition = transition
        End Sub

        Protected Overrides Sub TimerFinished()
           control.GetCurrentScene().StartTransition(transition)
        End Sub
    End Class

    Private Function GetCurrentScene() As BaseScene
        return CurrentScene
    End Function

    Public Class MapChangeObject
        Inherits QueueObject
        Private map as MapEnum
        Private insertion as point?
        Private isNewStage as boolean
        Sub New(map As MapEnum, insertion As Point?, time As Integer, control As GameControl, Optional [next] as QueueObject = Nothing, Optional IsNewStage As Boolean=False)
            MyBase.New(time, control, [next])
            Me.map = map
            Me.insertion = insertion
            Me.isNewStage = IsNewStage
        End Sub

        Protected Overrides Sub TimerFinished()
            control.RunScene(map, isNewStage, insertion)
        End Sub
    End Class
    
    Public Class ChangeQueueWrapper
        Private queue As New HashSet(Of QueueObject)

        Public Sub Add(item As QueueObject)
            queue.Add(item)
        End Sub

        Public Sub Remove(item As QueueObject)
            If Not queue.Remove(item)
                Throw new Exception("cannot remove item")
            End If
        End Sub
        Public Sub UpdateTick
            For count=queue.Count -1 To 0 Step -1
                Dim item = queue(count)
                item.UpdateTick()
                if item.IsFinished
                    Remove(item)
                End If
            Next
        End Sub
    End Class

    Public Property ChangeQueue As New ChangeQueueWrapper

    ''' <summary>
    ''' Includes putting a transition before map is loaded, loads map then adds another transitiotn
    ''' </summary>
    ''' <param name="map"></param>
    ''' <param name="insertion"></param>
    ''' <param name="time"></param>
    Friend Sub QueueSceneChange(map As MapEnum, insertion As Point?, Optional time As Integer = StandardPipeTime, optional location as Point? = nothing)
        Dim firstTrans As New TransitionObject(TransitionType.Circle, TransitionDirection.Top, location := location)

        Dim firstTransition As New MapTransitionObject(firstTrans, StandardPipeTime, Me)
        Dim mapChange As New MapChangeObject(map, insertion, StandardTransitionTime, Me)
        

        firstTransition.next = mapChange

        AddTimedEvent(firstTransition)
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

Public Enum MapEnum
    None
    StartScene
    map1_1above
    map1_1under
    map2_1above
End Enum