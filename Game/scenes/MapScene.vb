Imports WinGame
''' <summary>
''' Scene that represents a map, (probably loaded from json using <see cref="JsonMapReader.ReadMapFromResource"/>
''' </summary>
Public Class MapScene
    Inherits BaseScene
    Implements IScene

#Region "Variables"
    Public HudElements As StaticHud = Parent.SharedHud
    Private _screenLocation As New Point
    ''' <summary>
    ''' Location that the scene is rendered at - default = 0, 0
    ''' </summary>
    Public Property ScreenLocation As Point
        Get
            Return _screenLocation
        End Get
        Set(value As Point)
            _screenLocation = value
            Background.location = value
        End Set
    End Property

    ''' <summary>
    ''' Player1
    ''' </summary>
    Private Player1 As EntPlayer

    ''' <summary>
    ''' Player2
    ''' </summary>
    Private Player2 As EntPlayer

    ''' <summary>
    ''' BackgroundColor of scene
    ''' </summary>
    Friend Background As BackgroundRender

    ''' <summary>
    ''' Total time that can be spent in scene, in seconds
    ''' </summary>
    Private TotalMapTime As Integer

    ''' <summary>
    ''' if the hud is being dragged
    ''' </summary>
    Private isDragging As Boolean = False

    ''' <summary>
    ''' Width of map in pixels
    ''' </summary>
    Public Width As Integer

    ''' <summary>
    ''' Height of map in pixels
    ''' </summary>
    Public Height As Integer

    ''' <summary>
    ''' Default place that player spawns
    ''' </summary>
    ''' <returns></returns>
    Public Property DefaultPlayerLocation As Point


    ' There are 4 types of GameObjects
    ' Static - stuff that doesnt move ever, e.g. HUD elements, points
    ' Moving - stuff that moves, but doesnt have collisions
    ' Hitbox - stuff that has a hitbox
    ' Entity - stuff that ALWAYS moves

    ''' <summary>
    ''' List of staticitems. Items will be rendered in order inserted
    ''' </summary>
    Private ReadOnly Property allStaticItems As New List(Of GameItem)

    ''' <summary>
    ''' Scrolling items
    ''' </summary>
    Private ReadOnly Property allScrollingItems As New List(Of ScrollAlongImage)

    ''' <summary>
    ''' Contains all normal blocks
    ''' </summary>
    Private ReadOnly Property allHitboxItems As New List(Of HitboxItem)

    ''' <summary>
    ''' Contains all Entities
    ''' </summary> 
    Private ReadOnly Property allEntities As New List(Of Entity)

    ''' <summary>
    ''' All images that scroll with player, e.g. decorations, clouds
    ''' </summary>

    Private ReadOnly inSceneHitboxItems As New List(Of HitboxItem)

    ''' <summary>
    ''' Contains objects that will be removed once <see cref="RemoveAllDeleted"/> is run
    ''' Used in for each loops to avoid mutating object immediately
    ''' </summary>
    Public ReadOnly toRemoveObjects As New HashSet(Of HitboxItem)




    ''' <summary>
    ''' Contains objects that will be added once <see cref="AddAllAdded"/> is run
    ''' Used in for each loops to avoid mutating object immediately
    ''' </summary>
    Public ReadOnly toAddObjects As New HashSet(Of HitboxItem)

    Public IsFinished As Boolean
    Private escPressed As Boolean = False

    Public mapName As MapEnum
    Public NextLevel As MapEnum

#End Region

    ''' <summary>
    ''' Level was finished flag touched
    ''' </summary>
    Friend Sub LevelFinished()
        IsFinished = True
        Parent.QueueFlagLevelChange(Me.NextLevel, Nothing, Player1, time:=StandardTransitionTime)
    End Sub

    Friend Shared Function GetEmptyScene(control As GameControl) As MapScene
        Dim scene = New MapScene(control, ScreenGridWidth, ScreenGridHeight, MapEnum.None, "#ffffff", -1, False)

        scene.AddHitbox(New GroundPlatform(ScreenGridWidth, 64, New Point(0, 0), RenderTheme.Overworld, scene))
        scene.DefaultPlayerLocation = New Point(ScreenGridWidth / 2 - 16, 64)
        Return scene
    End Function

   

#Region "Debug"
    Public Overrides Sub DrawDebugStrings(form As GameControl)
        If Player1 IsNot Nothing
            form.AddStringBuffer(String.Format("Mario Location: {0}, {1}", Player1.Location.X, Player1.Location.Y))
        End If
        Dim relativePoint = GetMouseRelativeLocation()
        form.AddStringBuffer(String.Format("Mouse - x: {0}, y: {1}", relativePoint.X, relativePoint.Y))
        form.AddStringBuffer(String.Format("Is over box: {0}", If(MouseOverBox, "yes", "no")))
    End Sub
#End Region

#Region "Mouse/Input"
    ''' <summary>
    ''' Gets the location of curser relative to the size of the Form and scaled to be from Bottom Left
    ''' </summary>
    ''' <returns></returns>
    Public Function GetMouseRelativeLocation() As Point
        Dim point = Parent.PointToClient(Cursor.Position)
        ' convert the point from Top Left to bottom left
        point.Y = ScreenGridHeight - point.Y
        Return point
    End Function

    ''' <summary>
    ''' Checks if mouse is over the box
    ''' </summary>
    ''' <returns></returns>
    Private Function MouseOverBox As Boolean
        Dim point = GetMouseRelativeLocation()
        Return Me.HudElements.PowerupHolder.GetRect().Contains(point)
    End Function

    ''' <summary>
    ''' Handles mouse movement
    ''' </summary>
    Private Sub HandleMouse()
        If IsTransitioning
            Return
        End If

        ' make sure there is a powerup
        If Me.HudElements.PowerupHolder.powerupImage IsNot Nothing
            Dim cursorLocation = GetMouseRelativeLocation()
            ' Move image while dragging
            If isDragging Then

                Me.HudElements.PowerupHolder.SetPowerupMiddleLocation(cursorLocation)
            End If

            ' handle left mouse button release
            If Control.MouseButtons <> MouseButtons.Left And isDragging Then
                isDragging = False
                If HudElements.PowerupHolder.PowerupTouchesBox Then
                    Me.HudElements.PowerupHolder.ResetLocation()
                Else
                    HudElements.PowerupHolder.SpawnPowerup(Me)
                End If
            End If

            ' check if clicked
            If MouseOverBox() And Control.MouseButtons = MouseButtons.Left And HudElements.PowerupHolder.HasPowerup Then
                isDragging = True
            End If
        End If
    End Sub

    ''' <summary>
    ''' Gets the screen location of an item, based off how far an image has scrolled
    ''' </summary>
    ''' <param name="item"></param>
    ''' <returns></returns>
    Friend Function GetScreenLocation(item As ScrollAlongImage) As Point
        Return New Point(item.X - ScreenLocation.X, item.Y - ScreenLocation.Y)
    End Function


    ''' <summary>
    ''' Handles/ticks input from the user
    ''' </summary>
    Public Overrides Sub HandleInput()
        ' if escape is pressed and this is the first press
        If KeyHandler.Escape And Not escPressed And Not IsTransitioning
            escPressed = True
            If Not Parent.OverlayActive
                Parent.ShowOverlay
            Else
                Parent.HideOverlay
            End If
        End If

        ' reset escPressed if key is no longer held
        If escPressed
            If Not KeyHandler.Escape
                escPressed = False
            End If
        End If

        ' handle mouse evenst
        HandleMouse()
    End Sub
#End Region

#Region "Constructor"
    ''' <summary>
    ''' Constructor for <see cref="MapScene"/>
    ''' </summary>
    ''' <param name="parent"></param>
    Public Sub New(parent As GameControl, mapWidth As Integer, mapHeight As Integer, mapName As MapEnum, backgroundColor As String, totalMapTime As Integer, Optional includeHud As Boolean = True)
        MyBase.New(parent)
        Me.Width = mapWidth * 32
        Me.Height = mapHeight * 32
        Me.mapName = mapName
        If includeHud
            HudElements = parent.SharedHud
        End If
        Me.Player1 = parent.player1
        Me.Player2 = parent.player2
        Me.TotalMapTime = totalMapTime
        SetBackground(backgroundColor, Width, Height)
    End Sub

    ''' <summary>
    ''' Sets the BackgroundColor of the mapScene, using a hex backgroundBrush
    ''' </summary>
    ''' <param name="hexColor"></param>
    Private Sub SetBackground(hexColor As String, width As Integer, height As Integer)
        Background = New BackgroundRender(width, height, hexColor, Me)
    End Sub
#End Region

#Region "InScene Vars"
    ''' <summary>
    ''' Gets 
    ''' </summary>
    Private Iterator Function GetScrollableInScene() As IEnumerable(Of ScrollAlongImage)
        For Each item As ScrollAlongImage In allScrollingItems
            If item.InScene() Then
                Yield item
            End If
        Next
    End Function

     Public Iterator Function GetAllGround() As IEnumerable(Of GroundPlatform)
        For each item as HitboxItem In allHitboxItems
            If item.GetType() = GetType(GroundPlatform) Then
                Yield item
            End If
        Next
    End Function

    ''' <summary>
    ''' Gets/Updates the blocks that are in the mapScene and need to be rendened.
    ''' Should be called once per physics tick
    ''' </summary>
    Public Sub RefreshHitboxInScene()
        inSceneHitboxItems.Clear()
        For Each item As HitboxItem In allHitboxItems
            If item.InScene() Then
                inSceneHitboxItems.Add(item)
            End If
        Next
    End Sub
#End Region

#Region "AddElements"
    ''' <summary>
    ''' Adds a static object
    ''' </summary>
    ''' <param name="args"></param>
    Public Overridable Sub AddStatic(ByVal ParamArray args() As GameItem)
        For Each item As GameItem In args
            allStaticItems.Add(item)
        Next
    End Sub

    ''' <summary>
    ''' Adds a scrolling image
    ''' </summary>
    ''' <param name="args"></param>
    Sub AddScrollingImage(ByVal ParamArray args() As ScrollAlongImage)
        If args.Length = 0
            Throw New Exception()
        End If
        For Each item As ScrollAlongImage In args
            allScrollingItems.Add(item)
        Next
    End Sub


    ''' <summary>
    ''' Adds a hitboxitem (not entity to the mapScene)
    ''' </summary>
    ''' <param name="args"></param>
    Sub AddHitbox(ByVal ParamArray args() As HitboxItem)
        If args.Length = 0
            Throw New Exception()
        End If
        For Each item As HitboxItem In args
            allHitboxItems.Add(item)
        Next
    End Sub

    ''' <summary>
    ''' Adds entity to the mapScene
    ''' </summary>
    ''' <param name="args"></param>
    Public Sub AddEntity(ByVal ParamArray args() As Entity)
        For Each item As Entity In args
            AllEntities.Add(item)
            allHitboxItems.Add(item)
        Next
    End Sub
#End region


    ''' <summary>
    ''' Prepares item to be removed once <see cref="RemoveAllDeleted">is run</see>
    ''' </summary>
    ''' <param name="args"></param>
    Sub PrepareRemove(ByVal ParamArray args() As HitboxItem)
        For Each item As HitboxItem In args
            toRemoveObjects.Add(item)
        Next
    End Sub

    ''' <summary>
    ''' Prepares item to be added
    ''' </summary>
    ''' <param name="args"></param>
    Sub PrepareAdd(ByVal ParamArray args() As HitboxItem)
        For Each item As HitboxItem In args
            toAddObjects.Add(item)
        Next
    End Sub

    ''' <summary>
    ''' Actually remove all Entities
    ''' </summary>
    Sub RemoveAllDeleted()
        For Each item As HitboxItem In toRemoveObjects
            If item.GetType.IsSubclassOf(GetType(Entity))
                AllEntities.Remove(item)
                allHitboxItems.Remove(item)
            Else
                allHitboxItems.Remove(item)
            End If
        Next
        toRemoveObjects.Clear()
    End Sub

    ''' <summary>
    ''' Actually add all items
    ''' </summary>
    Sub AddAllAdded()
        For Each item As HitboxItem In toAddObjects
            If item.GetType.IsSubclassOf(GetType(Entity))
                AllEntities.Add(item)
                allHitboxItems.Add(item)
            Else
                allHitboxItems.Add(item)
            End If
        Next
        toAddObjects.Clear()
    End Sub


    Friend Sub Center()
        ScreenLocation = New Point(-((ScreenGridWidth - Me.Width) / 2), 0)
    End Sub

    Friend Sub CenterToPlayer()
        ScreenLocation = New Point(Player1.X - ScreenGridWidth / 2, ScreenLocation.Y)
    End Sub

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

    Friend Iterator Function GetAllHitboxAndEntities() As IEnumerable(Of HitboxItem)
        For each item as HitboxItem in allHitboxItems
            yield item
        Next
        For each item as Entity in AllEntities
            yield item
        Next
    End Function

    Public Overridable Sub SetPlayer(id As PlayerId, player As EntPlayer, location As Point)
        If id = PlayerId.Player1
            Player1 = player
        ElseIf id = PlayerId.Player2
            Player2 = player
        Else
            Throw New Exception()
        End If
        player.MyScene = Me
        player.Location = location
        If Not AllEntities.Contains(player)
            player.AddSelfToScene()
        End If
        player.RefreshPlayerVars()
    End Sub




    ''' <summary>
    ''' Updates the position of all Entities and items
    ''' </summary>
    Public Overrides Sub UpdateTick(ticksElapsed As Integer) Implements IScene.UpdateTick
        ' Animate and update position of each entity
        RefreshSceneItems()
        If Parent.OverlayActive
            ' overlay is active, shouldnt happen :(
            Return

        ElseIf exclusiveTime <> 0
            ' exclusive for only one entity
            ' e.g. player dying
            exclusiveItem.UpdateVeloc()
            exclusiveItem.UpdateLocation()
            exclusiveItem.Animate()
            exclusiveTime -= 1

        ElseIf AllPlayersDead
            FailLevel()
        

        Else
            For Each item As HitboxItem In allHitboxItems
                item.UpdateVeloc()
                item.UpdateLocation()
                item.Animate()
            Next
        End If

        AddAllAdded()
        RemoveAllDeleted()
        If Player1 IsNot Nothing And Me.Background IsNot Nothing
            ' TODO - chuck into function - scrolls screen if player is close to edge
            If Player1.Location.X - Me.ScreenLocation.X > (ScreenGridWidth / 3 * 2) Then
                ' on right 1/3
                Me.Background.ScrollHorizontal((600 - (ScreenGridWidth - (Player1.Location.X - Me.ScreenLocation.X))) / 50)

            ElseIf Player1.Location.X - Me.ScreenLocation.X < (ScreenGridWidth / 4) Then
                ' on left 1/4
                'Me.BackgroundColor.ScrollHorizontal(Player1.Location.X - RenderObject.ScreenLocation.X)
                Me.Background.ScrollHorizontal(-(600 - (Player1.Location.X - Me.ScreenLocation.X)) / 50)
            End If
        End If
        HandleTime(ticksElapsed)
    End Sub

    Private Sub RefreshSceneItems()
        RefreshHitboxInScene
        
    End Sub

    Friend Sub RegisterDeath(entPlayer As EntPlayer)
        ' TODO check player 2 as well
        SetExclusiveControl(entPlayer, StandardDeathTime)
        Sounds.PlayerDead.Play()
        If MusicPlayer.BackgroundPlayer IsNot Nothing
            MusicPlayer.BackgroundPlayer.Stop()
        End If
    End Sub

    Friend Sub FailLevel()
        If Not IsTransitioning
            Dim map = Helper.StrToEnum(Of MapEnum)(Me.mapName)
            Parent.ReloadLevel(map)
            Parent.QueueMapChangeWithStartScene(map, Nothing)

            Player1.ResetPlayer()
            Player1.Location = New Point(0, 128)
        End If
    End Sub

    Private Function AllPlayersDead() As Boolean
        If Player2 IsNot Nothing
            Return Player1.isDead And Player2.isDead
        Else
            If Player1 IsNot Nothing
                Return Player1.isDead
            Else
                Return False
            End If
        End If
    End Function

    Private exclusiveTime As Integer
    Private exclusiveItem As Entity

    Private Sub SetExclusiveControl(ent As Entity, time As Integer)
        exclusiveTime = time
        exclusiveItem = ent
    End Sub

    Private Sub HandleTime(ticksElapsed As Integer)
        ' Skip if infinite time
        if TotalMapTime = -1
            Return 
        End If

        Dim timeRemaining = ticksElapsed / TicksPerSecond
        If timeRemaining < 0
            Player1.State = PlayerStates.Dead
        End If
        HudElements.SetTime(TotalMapTime - CInt(Math.Floor(timeRemaining)))
    End Sub



    Public IsAtStartScreen As Boolean

    Public Sub RunStartScreen
        IsAtStartScreen = True
    End Sub

    ''' <summary>
    ''' Renders scene onto g
    ''' </summary>
    ''' <param name="g"></param>
    Public Overrides Sub RenderObjects(g As Graphics) Implements IScene.RenderObjects
        'bakckground
        If Background IsNot Nothing
            Background.Render(g)
        End If

        ' decorations
        For Each item As ScrollAlongImage In GetScrollableInScene()
            item.Render(g)
        Next

        ' HUD elements
        If HudElements IsNot Nothing
            HudElements.Render(g)
        End If

        ' TODO Remove
        ' all text & stuff
        For Each item As GameItem In allStaticItems()
            item.Render(g)
        Next

        ' render objects
        For Each item As HitboxItem In inSceneHitboxItems
            item.Render(g)
        Next

        ' render Entities
        For Each item As Entity In AllEntities
            item.Render(g)
        Next

        If Parent.OverlayActive
            ' add a shade of gray
            g.FillRectangle(New SolidBrush(Color.FromArgb(192, 0, 0, 0)), 0, 0, ScreenGridWidth, ScreenGridHeight)
        End If
    End Sub




End Class

Public Enum PlayerId
    Player1
    Player2
End Enum


Public Enum RenderTheme
    Overworld
    Underground
    Castle
End Enum

Public Enum Maps
    Map_StartScene
    Map1_1Above
    Map1_1Below
End Enum

