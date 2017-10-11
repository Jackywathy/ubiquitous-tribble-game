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
    ' volume of sound effects
    Public volume As Double = 0.5
    Public SharedHud As New StaticHud(ScreenGridWidth, ScreenGridHeight * HudHeightPercent)

    ''' <summary>
    ''' Gameloop - runs 60ish times a second, causing inputs/game to tick
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub GameLoop_Tick(sender As Object, e As EventArgs) Handles GameLoop.Tick
        CurrentScene.HandleInput()
       CurrentScene.UpdateTick(MapTimeCounter)  
        If Not OverlayActive
            ChangeQueue.UpdateTick()
            MapTimeCounter += 1     
        End If
        BaseScene.GlobalFrameCount += 1
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

    Friend OverlayActive As Boolean = False

    Public Sub ShowOverlay
        OverlayActive = True
        overlay.UpdateTExt()
        overlay.Show()
    End Sub

    Public Sub HideOverlay
        OverlayActive = False
        overlay.Hide()
        Me.Select()
    End Sub



    ''' <summary>
    ''' Constructor for <see cref="GameControl"/>
    ''' </summary>
    Sub New(Optional enabled As Boolean = True)
        InitalizeComponent()
        ' enable double buffering and custom paint

        allMapScenes = LoadScenes()

        RunScene(MapEnum.None, True)

        ' only start loop after init has finished
        GameLoop.Enabled = enabled
    End Sub


    ''' <summary>
    ''' Resets the map, state in GameControl and overlay
    ''' </summary>
    Friend Sub Reset()
        CurrentScene.ReloadMap()
        HideOverlay()
        ChangeQueue.queue.Clear()
    End Sub

    Friend Sub ReturnToMainMenu()
        Reset
        DirectCast(Me.TopLevelControl, GameForm).AskQuit = False
        DirectCast(Me.TopLevelControl, Form).Close()
    End Sub


    ''' <summary>
    ''' Holds the currently loaded scene
    ''' </summary>
    ''' <returns></returns>
    Private Property CurrentScene As MapScene

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

    Private overlay As PauseMenu
    ''' <summary>
    ''' Initalize components inside
    ''' </summary>
    Private Sub InitalizeComponent()
        GameLoop.Interval = 15
        SetStyle(ControlStyles.DoubleBuffer, True)
        SetStyle(ControlStyles.UserPaint, True)
        SetStyle(ControlStyles.OptimizedDoubleBuffer, True)
        SetStyle(ControlStyles.AllPaintingInWmPaint, True)
       

        overlay = New PauseMenu(Me)
       
        HideOverlay()
        Me.Controls.Add(overlay)
    End Sub

    Public Sub DrawDebugStrings()
        AddStringBuffer(String.Format("fps: {0}", FPS))
        If Not ChangeQueue.IsEmpty
            AddStringBuffer("Upcoming changes:")
            For Each item In ChangeQueue.queue
                Select Case item.GetType()
                    Case GetType(TransitionQueueObject)
                        Dim transition As TransitionQueueObject = item
                        AddStringBuffer(String.Format("TransitionType: {0}, time: {1}", transition.transition.ttype.ToString, transition.time))
                    Case GetType(MapChangeQueueObject)
                        Dim change As MapChangeQueueObject = item
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

    Friend Function GetMap(map As MapEnum) As MapScene
        Try
            Return Me.allMapScenes(map)
        Catch ex As KeyNotFoundException
            Throw New Exception(String.Format("Map ""{0}"" was not found in loaded scenes", map.ToString), ex)
        End Try
    End Function

    Private Sub MainGame_KeyUp(sender As Object, e As KeyEventArgs) Handles MyBase.KeyUp
        keyControl.KeyUp(e.KeyCode)
    End Sub

    Private Sub MainGame_Resize(sender As Object, e As EventArgs) Handles MyBase.Resize
        keyControl.Reset()
        overlay.ScaleToParent(Me.Size)
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

        For Each map As MapEnum in [Enum].GetValues(GetType(MapEnum))
            scenes.Add(map, GetScene(map))
        Next
        Return scenes
    End Function

    Friend Sub ReloadLevel(map As MapEnum)
        allMapScenes(map) = GetScene(map)
    End Sub

    Public Function GetScene(map As MapEnum) As MapScene
        Dim str As String = map.ToString

        If map = MapEnum.None
            Return MapScene.GetEmptyScene(Me)
        End If
        If map = MapEnum.StartScene
            return nothing
        End If
        return JsonMapReader.ReadMapFromResource(str, map, Me)
    End Function

    Public Function GetPlayer(player As PlayerId) As EntPlayer
        Select Case player
            Case PlayerId.Player1
                Return Player1
            Case PlayerId.Player2
                Return Player2
            Case Else
                Throw New Exception("has to be player1 or 2")
        End Select
    End Function

    

    Public player1 As New EntPlayer(32, 32, New Point(0, 0), Nothing)
    Friend player2 As EntPlayer



    ''' <summary>s
    ''' Runs a scene from mapEnum
    ''' </summary>
    ''' <param name="map"></param>
    ''' <param name="isNewStage">Whether or not it resets the timer</param>
    Public Sub RunScene(map As MapEnum, isNewStage As Boolean, Optional insertion As Point? = Nothing)
        CurrentScene = allMapScenes(map)
        SharedHud.WorldNumText.Text = GetProperMapName(map)

        Dim start As Point = If(insertion, CurrentScene.DefaultPlayerLocation)

        CurrentScene.SetPlayer(PlayerId.Player1, player1, start)

        If CurrentScene.Width < ScreenGridWidth Then
            ' center scene, if width is too small
            CurrentScene.Center()
        End If
        If CurrentScene.BackgroundMusic IsNot Nothing Then
            CurrentScene.BackgroundMusic.PlayBackground()
        End If
        If isNewStage Then
            MapTimeCounter = 0
        End If
    End Sub

    Private Sub GameControl_SizeChanged(sender As Object, e As EventArgs) Handles Me.SizeChanged
        ScreenGridWidth = Width
        ScreenGridHeight = Height
        overlay.ScaleToParent(Me.Size)
    End Sub

    Public Sub AddTimedEvent(item As QueueObject)
        ChangeQueue.Add(item)
    End Sub

    Public Function GetCurrentScene() As MapScene
        Return CurrentScene
    End Function

    Public Property ChangeQueue As New ChangeQueueWrapper

    Friend Function GetVolumeMultipler() As Double
        Return volume
    End Function



    Friend Sub QueueFlagLevelChange(map As MapEnum, mapInsertion As Point?, player1 As EntPlayer, time As Integer)
        ' wait time, stop mario
        ' play circle transition on mario location

        Dim wait As New WaitQueueObject(player1, Me, time:=time)

        Dim firstTrans As New TransitionObject(TransitionType.Circle, TransitionDirection.Top,
                                               location:=player1.MyScene.GetScreenLocation(player1)
                                               )

        Dim secondTrans as New TransitionObject(TransitionType.StartScene, TransitionDirection.Bottom,
                                                tstring := GetProperMapName(map))

        Dim firstTransition As New TransitionQueueObject(firstTrans, 0, Me)
        Dim secondTransition As New TransitionQueueObject(secondTrans, StandardTransitionTime, Me)

        Dim mapChange As New MapChangeQueueObject(map, mapInsertion, StandardTransitionTime, Me, Reset:=True, CenterToPlayer := False)

        wait.next = firstTransition
        firstTransition.next = secondTransition
        secondTransition.next = mapChange

        AddTimedEvent(wait)
    End Sub

    ''' <summary>
    ''' Includes putting a transition before map is loaded, loads map then adds another transitiotn
    ''' Returns the last transition object to be run synchronously
    ''' </summary>
    ''' <param name="map"></param>
    ''' <param name="mapInsertion"></param>
    ''' <param name="time"></param>
    Friend Function QueueMapChangeWithCircleAnimation(map As MapEnum, animationLocation As Point,
                                                      mapInsertion As Point?, Optional time As Integer = StandardPipeTime, Optional before As QueueObject = Nothing, Optional centerToplayer As Boolean = True) As QueueObject
        
        ' convert Bottom based point to Top Based point
        animationLocation.Y = ScreenGridHeight - animationLocation.Y
       

        Dim firstTrans As New TransitionObject(TransitionType.Circle, TransitionDirection.Top, location:=animationLocation)


        Dim firstTransition As New TransitionQueueObject(firstTrans, 0, Me)
        Dim mapChange As New MapChangeQueueObject(map, mapInsertion, StandardTransitionTime, Me, CenterToPlayer:=centerToplayer)

        firstTransition.next = mapChange
        If before IsNot Nothing
            before.next = firstTransition
            AddTimedEvent(before)
        Else
            AddTimedEvent(firstTransition)
        End If

        Return mapChange
    End Function

    Friend Function QueueMapChangeWithStartScene(map As MapEnum, mapInsertion As Point?, Optional time As Integer = StandardStartScreenTime, Optional before As QueueObject = Nothing) As QueueObject
        Dim firstTrans As New TransitionObject(TransitionType.StartScene, tstring := GetProperMapName(map))
        
        ' play a startscene immediately (time = 0)
        Dim firstTransition As New TransitionQueueObject(firstTrans, 0, Me)
        Dim mapChange As New MapChangeQueueObject(map, mapInsertion, StandardTransitionTime, Me, CenterToPlayer:=False)

        firstTransition.next = mapChange
        If before IsNot Nothing
            before.next = firstTransition
            AddTimedEvent(before)
        Else
            AddTimedEvent(firstTransition)
        End If
        Return mapChange
    End Function

    Private Sub InitializeComponent()
        Me.SuspendLayout
        Me.ResumeLayout(False)
    End Sub
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
    Public Shared Escape As Boolean
    Public Shared keysPressed As New Dictionary(Of Keys, Boolean)

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
        If key = Keys.Escape
            Escape = vset
        End If
        keysPressed(key) = vset
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




Public Enum MapEnum
    none
    StartScene
    map1_1above
    map1_1under
    map1_2above
    map1_2under
    map2_1above
    map2_1under

End Enum