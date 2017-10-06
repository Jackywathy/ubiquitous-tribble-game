Imports System.Drawing.Drawing2D

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
        QueueMapChangeWithStartScene(PlayerStartScreen, Nothing)


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
        If Not ChangeQueue.IsEmpty
            AddStringBuffer("Upcoming changes:")
            For each item in ChangeQueue.queue
                Select Case item.GetType()
                    Case GetType(PlayTransitionObject)
                        Dim transition As PlayTransitionObject = item
                        AddStringBuffer(String.Format("TransitionType: {0}, time: {1}", transition.transition.ttype.ToString, transition.time))
                    Case GetType(MapChangeObject)
                        Dim change As MapChangeObject = item
                        AddStringBuffer(String.Format("Changing map: {0}, time: {1}", change.map.ToString, change.time))
                End Select
            Next
        End If
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

   Public Function ChangePlayerScene(scene As MapScene) As EntPlayer
        player1.MyScene = scene
        Return player1
   End Function

    Public player1 As New EntPlayer(32, 32, New Point(0, 0), Nothing)

    ''' <summary>s
    ''' Runs a scene from mapEnum
    ''' </summary>
    ''' <param name="map"></param>
    ''' <param name="isNewStage">Whether or not it resets the timer</param>
    Public Sub RunScene(map As MapEnum, isNewStage As Boolean, optional insertion As Point? = Nothing)
        CurrentScene = allMapScenes(map)

        If CurrentScene.GetType() = GetType(MapScene)
            dim mapScene as MapScene = CurrentScene
            Dim start as Point = If(insertion, mapScene.DefaultLocation)
            mapScene.SetPlayer(MapScene.PlayerId.Player1, player1, start)
                       
            If mapScene.width < ScreenGridWidth
                ' center scene
                mapScene.Center()
            End If
        End If
        If isNewStage
            MapTimeCounter = 0
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
        Friend HasRun as Boolean 
        Public Property IsFinished As Boolean

        Public Sub New(time As Integer, control As GameControl, Optional [next] as QueueObject = Nothing)
            Me.time = time
            Me.control = control
            Me.[next] = [next]
            HasRun = False
        End Sub

        Public Overridable Sub Setup()

        End Sub

        Public Overridable Sub Tick()

        End Sub


        Public Sub UpdateTick()
            If Not hasRun
                HasRun = True
                Setup()
            End If
            if time = 0
                TimerFinished()
                IsFinished = True
                if [next] IsNot nothing
                    control.AddTimedEvent([next])
                End If
            Else
                Tick()
            End If
            time -= 1
        End Sub
        Protected MustOverride Sub TimerFinished()
    End Class

    Private Sub AddTimedEvent(item As QueueObject)
        ChangeQueue.Add(item)
    End Sub


    Public Class PlayTransitionObject
        Inherits QueueObject
        Friend transition as TransitionObject

        Sub New(transition As TransitionObject, time As Integer, control As GameControl, Optional [next] as QueueObject = Nothing)
            MyBase.New(time, control, [next])
            me.transition = transition
        End Sub

        Protected Overrides Sub TimerFinished()
           control.GetCurrentScene().StartTransition(transition)
        End Sub

        Public Overrides Sub Setup()
            MyBase.Setup()
        End Sub
    End Class

    Private Function GetCurrentScene() As BaseScene
        return CurrentScene
    End Function

    Public Class MapChangeObject
        Inherits QueueObject
        Friend map as MapEnum
        Private insertion as point?
        Private isNewStage as boolean
        Private centerToplayer as boolean
        Sub New(map As MapEnum, insertion As Point?, time As Integer, control As GameControl, Optional [next] as QueueObject = Nothing, Optional IsNewStage As Boolean=False, optional CenterToPlayer As boolean = True)
            MyBase.New(time, control, [next])
            Me.map = map
            Me.insertion = insertion
            Me.isNewStage = IsNewStage
            Me.centerToplayer = CenterToPlayer
        End Sub

        Protected Overrides Sub TimerFinished()
            control.RunScene(map, isNewStage, insertion)
            If centerToplayer
                Dim mapScene as MapScene = control.GetCurrentScene()
                mapScene.CenterToPlayer()
            End If
        End Sub
    End Class
    
    Public Class ChangeQueueWrapper
        Friend queue As New HashSet(Of QueueObject)


        Public Sub Add(item As QueueObject)
            queue.Add(item)
        End Sub

        Public Sub Remove(item As QueueObject)
            If Not queue.Remove(item)
                Throw new Exception("cannot remove item")
            End If
        End Sub

        Public Sub UpdateTick
            For c=queue.Count -1 To 0 Step -1
                Dim item = queue(c)
                item.UpdateTick()
                if item.IsFinished
                    Remove(item)
                End If
            Next
        End Sub

        Public Readonly Property IsEmpty As Boolean
            Get
                Return queue.Count = 0
            End Get
        End Property
    End Class

    Public Property ChangeQueue As New ChangeQueueWrapper

    Friend Sub ReloadLevel(map As MapEnum)
        allMapScenes(map) = JsonMapReader.ReadMapFromResource(map.ToString.ToLower(), Me)
    End Sub

    ''' <summary>
    ''' Includes putting a transition before map is loaded, loads map then adds another transitiotn
    ''' Returns the last transition object to be run synchronously
    ''' </summary>
    ''' <param name="map"></param>
    ''' <param name="mapInsertion"></param>
    ''' <param name="time"></param>
    Friend Function QueueMapChangeWithCircleAnimation(map As MapEnum, mapInsertion As Point?, centerToplayer As Boolean, Optional time As Integer = StandardPipeTime, optional animationLocation as Point? = nothing, optional before As QueueObject = Nothing) As QueueObject
        Dim firstTrans As New TransitionObject(TransitionType.Circle, TransitionDirection.Top, location := animationLocation)

        Dim firstTransition As New PlayTransitionObject(firstTrans, StandardPipeTime, Me)
        Dim mapChange As New MapChangeObject(map, mapInsertion, StandardTransitionTime, Me, CenterToPlayer := centerToplayer)
        

        firstTransition.next = mapChange
        If before IsNot nothing
            before.next = firstTransition
            AddTimedEvent(before)
        Else
            AddTimedEvent(firstTransition)
        End If
        
        Return mapChange
    End Function

    Friend Function QueueMapChangeWithStartScene(map As MapEnum, mapInsertion As Point?, Optional time As Integer = StandardStartScreenTime, optional before As QueueObject = Nothing) As QueueObject
        Dim firstTrans As New TransitionObject(TransitionType.StartScene, TransitionDirection.Bottom)

        ' play a startscene immediately (time = 0)
        Dim firstTransition As New PlayTransitionObject(firstTrans, 0, Me)
        Dim mapChange As New MapChangeObject(map, mapInsertion,  StandardTransitionTime, Me,CenterToPlayer := False)

        firstTransition.next = mapChange
        If before IsNot nothing
            before.next = firstTransition
            AddTimedEvent(before)
        Else
            AddTimedEvent(firstTransition)
        End If
        Return mapChange
    End Function


    Public Class MarioPipeAnimationObject
        Inherits QueueObject
        public player as EntPlayer
        public direction as PipeType
        public goingin as boolean
        Sub New(player As EntPlayer, direction As PipeType, goingIn as Boolean, control As GameControl, Optional [next] as QueueObject = Nothing, Optional time As integer = standardPipeTime)
            MyBase.New(time, control, [next])
            Me.player = player
            Me.direction = direction
            me.goingin = goingIn
            
        End Sub

        Public Overrides Sub Setup()
            Select Case direction
                Case PipeType.Vertical
                    player.BeginVerticalPipe(goingIn, time)
                Case PipeType.Horizontal
                    player.BeginHorizontalPipe(goingIn, time)
                case Else
                    throw new Exception()
                
                  
            End Select
            
        End Sub

        Protected Overrides Sub TimerFinished()

        End Sub
    End Class
End Class
Public Enum PipeType
    Vertical
    Horizontal
End Enum
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