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
    Public volume as Double = 0.5
    
    Public SharedHud As New StaticHud(ScreenGridWidth, ScreenGridHeight * HudHeightPercent)

    ''' <summary>
    ''' Gameloop - runs 60ish times a second, causing inputs/game to tick
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub GameLoop_Tick(sender As Object, e As EventArgs) Handles GameLoop.Tick
        CurrentScene.HandleInput()
        CurrentScene.UpdateTick(MapTimeCounter)
        if not OverlayActive
            ChangeQueue.UpdateTick()
            MapTimeCounter += 1
        End if

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
    Sub New(Optional enabled As boolean = True)
        InitalizeComponent()
        ' enable double buffering and custom paint
        
        Randomize()

        allMapScenes = LoadScenes()

        RunScene(MapEnum.None, True)
        


        ' only start loop after init has finished
        GameLoop.Enabled = enabled
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

    Private overlay as MenuControl
    ''' <summary>
    ''' Initalize components inside
    ''' </summary>
    Private Sub InitalizeComponent()
        GameLoop.Interval = 15
        SetStyle(ControlStyles.DoubleBuffer, True)
        SetStyle(ControlStyles.UserPaint, True)
        SetStyle(ControlStyles.OptimizedDoubleBuffer, True)
        SetStyle(ControlStyles.AllPaintingInWmPaint, True)


        overlay = new MenuControl(Me)
        HideOverlay()
        Me.Controls.Add(overlay)
    End Sub

    Public Sub DrawDebugStrings()
        AddStringBuffer(String.Format("fps: {0}", FPS))
        If Not ChangeQueue.IsEmpty
            AddStringBuffer("Upcoming changes:")
            For each item in ChangeQueue.queue
                Select Case item.GetType()
                    Case GetType(TransitionQueueObject)
                        Dim transition As TransitionQueueObject = item
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
        overlay.ScaleToParent(Me.size)
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

            If val = MapEnum.None 
                scenes.Add(MapEnum.None, MapScene.GetEmptyScene(Me))
                Continue For
            End If
            If val = MapEnum.StartScene
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
            if mapScene.Background IsNot nothing
                mapScene.BackgroundMusic.PlayBackground()
            End if
        End If
        If isNewStage
            MapTimeCounter = 0
        End If
    End Sub

    Private Sub GameControl_SizeChanged(sender As Object, e As EventArgs) Handles Me.SizeChanged
        ScreenGridWidth = Width
        ScreenGridHeight = Height
        overlay.ScaleToParent(me.Size)
    End Sub

    

    
  
    Public Sub AddTimedEvent(item As QueueObject)
        ChangeQueue.Add(item)
    End Sub

    Public Function GetCurrentScene() As BaseScene
        return CurrentScene
    End Function
   
    Public Property ChangeQueue As New ChangeQueueWrapper

    Friend Sub ReloadLevel(map As MapEnum)
        allMapScenes(map) = JsonMapReader.ReadMapFromResource(map.ToString.ToLower(), Me)
    End Sub

    Friend Function GetVolumeMultipler() As Double
        return volume
    End Function

    ''' <summary>
    ''' Includes putting a transition before map is loaded, loads map then adds another transitiotn
    ''' Returns the last transition object to be run synchronously
    ''' </summary>
    ''' <param name="map"></param>
    ''' <param name="mapInsertion"></param>
    ''' <param name="time"></param>
    Friend Function QueueMapChangeWithCircleAnimation(map As MapEnum, mapInsertion As Point?, centerToplayer As Boolean, Optional time As Integer = StandardPipeTime, optional animationLocation as Point? = nothing, optional before As QueueObject = Nothing) As QueueObject
        Dim firstTrans As New TransitionObject(TransitionType.Circle, TransitionDirection.Top, location := animationLocation)

        Dim firstTransition As New TransitionQueueObject(firstTrans, 0, Me)
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
        Dim firstTransition As New TransitionQueueObject(firstTrans, 0, Me)
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

    Friend Sub ReturnToMainMenu()
        Throw New NotImplementedException()
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
    Public Shared Escape as boolean
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
        if key = Keys.Escape
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
    None
    StartScene
    map1_1above
    map1_1under
    map2_1above
End Enum