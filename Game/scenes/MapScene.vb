Imports WinGame
''' <summary>
''' Scene that represents a map, (probably loaded from json using <see cref="JsonMapReader.ReadMapFromResource"/>
''' </summary>
Public Class MapScene
    Inherits BaseScene
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

    Public Overrides Sub DrawDebugStrings(form As GameControl)
        form.AddStringBuffer(String.Format("Mario Location: {0}, {1}", Player1.Location.X, Player1.Location.Y))
        Dim relativePoint = GetMouseRelativeLocation()
        form.AddStringBuffer(String.Format("Mouse - x: {0}, y: {1}", relativePoint.X, relativePoint.Y))
        form.AddStringBuffer(String.Format("Is over box: {0}", If(MouseOverBox, "yes", "no")))
    End Sub


    ''' <summary>
    ''' Background of scene
    ''' </summary>
    Friend Background As BackgroundRender

    ''' <summary>
    ''' Total time that can be spent in scene, in seconds
    ''' </summary>
    Private MapTime As Integer

    ''' <summary>
    ''' Location that the scene is rendered at - default = 0, 0
    ''' </summary>
    Public ScreenLocation As New Point(0, 0)


    Private Function MouseOverBox As Boolean
        Dim point = GetMouseRelativeLocation()
        Return Me.HudElements.PowerupHolder.GetRect().Contains(point)
    End Function

    Private isDragging = False

    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub HandleMouse()
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

        'previousMouseLoc = Cursor.Position

    End Sub

    ''' <summary>
    ''' Constructor for <see cref="MapScene"/>
    ''' </summary>
    ''' <param name="parent"></param>
    Public Sub New(parent As GameControl, Optional includeHud As Boolean = True)
        MyBase.New(parent)
        HudElements = parent.SharedHud
    End Sub

    ''' <summary>
    ''' Gets/Updates the blocks that are in the mapScene and need to be rendened.
    ''' Should be called once per physics tick
    ''' </summary>
    ''' <returns>Objects in mapScene</returns>
    Public Function GetHitboxObjectsInScene() As List(Of HitboxItem)
        inSceneItems.Clear()
        For Each item As HitboxItem In AllHitboxItems
            If item.InScene() Then
                inSceneItems.Add(item)
            End If
        Next
        Return inSceneItems
    End Function



    ' There are 4 types of GameObjects
    ' Static - stuff that doesnt move ever, e.g. HUD elements, points
    ' Moving - stuff that moves, but doesnt have collisions
    ' Hitbox - stuff that has a hitbox
    ' Entity - stuff that ALWAYS moves

    ''' <summary>
    ''' Contains all normal blocks
    ''' </summary>
    Public ReadOnly Property AllHitboxItems As New List(Of HitboxItem)

    ''' <summary>
    ''' Contains all Entities
    ''' </summary> 
    Public ReadOnly Property AllEntities As New List(Of Entity)

    ''' <summary>
    ''' Adds a hitboxitem (not entity to the mapScene)
    ''' </summary>
    ''' <param name="args"></param>
    Sub AddHitbox(ByVal ParamArray args() As HitboxItem)
        For Each item As HitboxItem In args
            AllHitboxItems.Add(item)
        Next
    End Sub

    ''' <summary>
    ''' Adds entity to the mapScene
    ''' </summary>
    ''' <param name="args"></param>
    Public Sub AddEntity(ByVal ParamArray args() As Entity)
        For Each item As Entity In args
            AllEntities.Add(item)
            AllHitboxItems.Add(item)
        Next
    End Sub

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
                AllHitboxItems.Remove(item)
            Else
                AllHitboxItems.Remove(item)
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
                AllHitboxItems.Add(item)
            Else
                AllHitboxItems.Add(item)
            End If
        Next
        toAddObjects.Clear()
    End Sub

    Friend Function GetPlayer(player As PlayerId) As EntPlayer
        Select Case player
            Case PlayerId.Player1
                Return player1
            Case PlayerId.Player2
                Return player2
            Case else
                Throw New Exception("has to be player1 or 2")
        End Select
        
    End Function



    ''' <summary>
    ''' Contains all objects in scene that need to be rendered
    ''' </summary>
    Public ReadOnly Property inSceneItems As New List(Of HitboxItem)

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

    Public HudElements As StaticHud = Parent.SharedHud



    ''' <summary>
    ''' Player1
    ''' </summary>
    Private Player1 As EntPlayer

    ''' <summary>
    ''' Player2
    ''' </summary>
    Private Player2 As EntPlayer

    Public Enum PlayerId
        Player1
        Player2
    End Enum

    Public Overridable Sub SetPlayer(id As PlayerId, player As EntPlayer)
        If id = PlayerId.Player1
            Player1 = player
        ElseIf id = PlayerId.Player2
            Player2 = player
        Else
            Throw New Exception()
        End If
    End Sub

    ''' <summary>
    ''' Sets the Background of the mapScene, using a hex color
    ''' </summary>
    ''' <param name="hexColor"></param>
    Public Sub SetBackground(hexColor As String, width As Integer, height As Integer)
        Background = New BackgroundRender(width, height, hexColor, Me)
    End Sub

    Public Sub SetBackground(backgroundRender As BackgroundRender)
        Background = backgroundRender
    End Sub

    Public Sub SetMapTime(time As Integer)
        MapTime = time
    End Sub

    ''' <summary>
    ''' Handles/ticks input from the user
    ''' </summary>
    Public Overrides Sub HandleInput()

        ' handle mouse evenst
        HandleMouse()
    End Sub

    ''' <summary>
    ''' Updates the position of all Entities and items
    ''' </summary>
    Public Overrides Sub UpdateTick(ticksElapsed As Integer)
        ' Animate and update position of each entity
        If IsFrozen Then
            For each item As Entity in allUnfreezableItems
                item.UpdateVeloc()
                item.UpdateLocation()
                item.Animate()
            Next
        Else
            For Each item As HitboxItem In AllHitboxItems
                item.UpdateVeloc()
                item.UpdateLocation()
                item.Animate()
            Next
        End If

        For Each item In Player1.currentGroundObjects
            item.CollisionTop(Player1)
        Next

        AddAllAdded()
        RemoveAllDeleted()


        ' TODO - chuck into function - scrolls screen if player is close to edge
        If Player1.Location.X - Me.ScreenLocation.X > (ScreenGridWidth / 3 * 2) Then
            ' on right 1/3
            Me.Background.ScrollHorizontal((600 - (ScreenGridWidth - (Player1.Location.X - Me.ScreenLocation.X))) / 50)

        ElseIf Player1.Location.X - Me.ScreenLocation.X < (ScreenGridWidth / 4) Then
            ' on left 1/4
            'Me.Background.ScrollHorizontal(Player1.Location.X - RenderObject.ScreenLocation.X)
            Me.Background.ScrollHorizontal(-(600 - (Player1.Location.X - Me.ScreenLocation.X)) / 50)
        End If
        HandleTime(ticksElapsed)
    End Sub

    Friend Sub RegisterDeath(entPlayer As EntPlayer)
        ' TODO check player 2 as well
        If AllPlayersDead
            BackgroundMusic.Stop()
            Sounds.PlayerDead.Play()
            SetExclusiveControl(entPlayer, StandardDeathTime)
            
        End If
    End Sub

    Private Function AllPlayersDead() As Boolean
        ' TODO fix
        Return True
    End Function

    Private exclusiveTime as Integer
    Private exclusiveItem as entity

    Private Sub SetExclusiveControl(ent As Entity, time As integer)
        exclusiveTime = time
        exclusiveItem = ent
    End Sub

    Private Sub HandleTime(ticksElapsed As Integer)
        Dim timeRemaining = ticksElapsed / TicksPerSecond
        If timeRemaining < 0
            Player1.State = PlayerStates.Dead
        End If
        HudElements.SetTime(MapTime - CInt(Math.Floor(timeRemaining)))
    End Sub

    ''' <summary>
    ''' Renders scene onto g
    ''' </summary>
    ''' <param name="g"></param>
    Public Overrides Sub RenderObjects(g As Graphics)
        Background.Render(g)
        HudElements.Render(g)

        ' all text & stuff
        For Each item As GameItem In AllStaticItems
            item.Render(g)
        Next

        Dim objects = GetHitboxObjectsInScene()

        ' render objects
        For Each item As HitboxItem In objects
            item.Render(g)
        Next

        ' render Entities
        For Each item In AllEntities
            item.Render(g)
        Next
        GlobalFrameCount += 1
    End Sub
    Private ReadOnly allUnfreezableItems as New List(Of Entity)

    Friend Sub AddUnfreezableItem(sender As Entity)
        allUnfreezableItems.Add(sender)
    End Sub
End Class


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

