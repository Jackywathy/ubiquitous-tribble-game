﻿Imports Newtonsoft.Json

Public Enum TransitionDirection
    Top 
    Bottom
    Right
    Left
    Random
End Enum

Public Enum TransitionType
    Normal
    Circle
End Enum


Public Enum SwitchLevelType
    Normal
    Secret1
    Secret2
End Enum

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
        Dim point = parent.PointToClient(Cursor.Position)
        ' convert the point from Top Left to bottom left
        point.Y = ScreenGridHeight - point.y
        return point
    End Function
    
    Public Overrides Sub DrawDebugStrings(form as GameControl)
        form.AddStringBuffer(String.Format("Mario Location: {0}, {1}", player1.Location.X, player1.Location.Y))
        dim relativePoint = GetMouseRelativeLocation()
        form.AddStringBuffer(String.Format("Mouse - x: {0}, y: {1}", relativePoint.X, relativePoint.Y))
        form.AddStringBuffer(String.Format("Is over box: {0}", If(MouseOverBox, "yes", "no")))
    End Sub


    ''' <summary>
    ''' Switches the level to something else
    ''' call after mario has run past the end of the screen
    ''' </summary>
    ''' <param name="type"></param>
    Public Sub SwitchLevel(Optional type As SwitchLevelType = SwitchLevelType.Normal)
        Throw New NotImplementedException()
        Print("HI!")
    End Sub
    ''' <summary>
    ''' Background of scene
    ''' </summary>
    Friend Background As BackgroundRender

    ''' <summary>
    ''' Location that the scene is rendered at - default = 0, 0
    ''' </summary>
    Public ScreenLocation As New Point(0, 0)


    Private Function MouseOverBox As Boolean
        Dim point = GetMouseRelativeLocation()
        Return Me.HudPowerUp.GetRect().Contains(point)
    End Function

    Private isDragging = False

    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub HandleMouse()
        Dim cursorLocation = GetMouseRelativeLocation()
        ' Move image while dragging
        If isDragging Then
            Me.HudPowerUp.SetPowerupMiddleLocation(cursorLocation)
        End If

        ' handle left mouse button release
        If Control.MouseButtons <> MouseButtons.Left And isDragging Then
            isDragging = False
            If HudPowerUp.PowerupTouchesBox Then
                Me.HudPowerUp.ResetLocation()
            Else
                HudPowerUp.SpawnPowerup(Me)
            End If
        End If

        ' check if clicked
        If MouseOverBox() And Control.MouseButtons = MouseButtons.Left And HudPowerUp.HasPowerup Then
            isDragging = True
        End If

        'previousMouseLoc = Cursor.Position

    End Sub

    ''' <summary>
    ''' Constructor for <see cref="MapScene"/>
    ''' </summary>
    ''' <param name="parent"></param>
    Public Sub New(parent As Control)
        MyBase.New(parent)
    End Sub

    ''' <summary>
    ''' Gets/Updates the blocks that are in the mapScene and need to be rendened.
    ''' Should be called once per physics tick
    ''' </summary>
    ''' <returns>Objects in mapScene</returns>
    Public Function GetHitboxObjectsInScene() As List(Of HitBoxItem)
        InSceneItems.Clear()
        For Each item As HitboxItem In AllHitboxItems
            If item.InScene() Then
                InSceneItems.Add(item)
            End If
        Next
        Return InSceneItems
    End Function

    

    ' There are 4 types of GameObjects
    ' Static - stuff that doesnt move ever, e.g. HUD elements, points
    ' Moving - stuff that moves, but doesnt have collisions
    ' Hitbox - stuff that has a hitbox
    ' Entity - stuff that ALWAYS moves

     ''' <summary>
    ''' Contains all normal blocks
    ''' </summary>
    Public Readonly Property AllHitboxItems As New List(Of HitboxItem)

    ''' <summary>
    ''' Contains all Entities
    ''' </summary> 
    Public Readonly Property AllEntities As New List(Of Entity)

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

    Public HudPowerUp As StaticHudPowerup

    
   
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

    Public Overridable Sub SetPlayer(id as PlayerId, player As EntPlayer)
        if id = PlayerId.Player1
            player1 = player
        ElseIf id = PlayerId.Player2
            player2 = player
        Else
            throw New Exception()
        End If
    End Sub

    ''' <summary>
    ''' Sets the Background of the mapScene, using a hex color
    ''' </summary>
    ''' <param name="hexColor"></param>
    Sub SetBackground(hexColor As String, width As Integer, height As Integer)
        Background = New BackgroundRender(width, height, hexColor, Me)
    End Sub

    ''' <summary>
    ''' Handles/ticks input from the user
    ''' </summary>
    Public Overrides Sub HandleInput()
        Dim xToMove = 0
        Dim yToMove = 0
        
        ' LEFT
        If KeyHandler.MoveLeft Then
            If Not Player1.IsCrouching Then
                xToMove = -Player1.moveSpeed.x
            End If

            ' Yes this is really badly hard-coded
            If Player1.IsCrouching And Not Player1.AllowedToUncrouch And KeyHandler.MoveUp And Player1.AllowJumpInput Then
                xToMove = -Player1.moveSpeed.x
            End If
        End If

        ' RIGHT
        If KeyHandler.MoveRight Then
            If Not Player1.IsCrouching Then
                xToMove = Player1.moveSpeed.x

            End If
            If Player1.IsCrouching And Not Player1.AllowedToUncrouch And KeyHandler.MoveUp And Player1.AllowJumpInput Then
                xToMove = Player1.moveSpeed.x
            End If
        End If

        ' UP
        If KeyHandler.MoveUp And Player1.AllowJumpInput Then
            yToMove = Player1.moveSpeed.y
            Player1.AllowJumpInput = False
            Sounds.Jump.Play(fromStart:=True)
        ElseIf KeyHandler.MoveUp = False Then
            Player1.AllowJumpInput = True
        End If

        ' DOWN
        If KeyHandler.MoveDown Then
            If Player1.State > PlayerStates.Small And Player1.isGrounded = True Then 'crouch
                Player1.OnCrouch(True)
            End If
        ElseIf Player1.State > PlayerStates.Small And Player1.IsCrouching = True Then
            ' TODO - check for collision on above blocks before uncrouching
            Player1.OnCrouch(False)
        End If

        If Player1.State = PlayerStates.Fire And KeyHandler.MoveDown And Player1.AllowShoot Then
            Player1.TryShootFireball()
            Player1.AllowShoot = False
        ElseIf Not KeyHandler.MoveDown Then
            Player1.AllowShoot = True
        End If

        If Not Player1.isDead Then
            Player1.AccelerateX(xToMove)
            Player1.AccelerateY(yToMove, False)
        End If

        If Player1.IsBouncingOffEntity Then
            Player1.BounceOffEntity(KeyHandler.MoveUp)
            Player1.IsBouncingOffEntity = False
        End If
        ' handle mouse evenst
        handleMouse()
    End Sub

    Private deathAnimation As Boolean

    ''' <summary>
    ''' Updates the position of all Entities and items
    ''' </summary>
    Public Overrides Sub UpdateTick()
        ' Animate and update position of each entity
        If player1.IsDead Then
            player1.UpdateVeloc()
            player1.UpdateLocation()
            Player1.Animate()
            if player1.X < 0 And Not deathAnimation
                deathAnimation = True
                StartNormalTransition(TransitionDirection.Top)
            End If
        Else
            For Each item As HitboxItem In AllHitboxItems
                item.UpdateVeloc()
                item.UpdateLocation()
                item.Animate()
            Next
        End If

        AddAllAdded()
        RemoveAllDeleted()

        ' TODO - chuck into function - scrolls screen if player is close to edge
        If Player1.Location.X - Me.ScreenLocation.X > (ScreenGridWidth / 3 * 2) Then
            ' on right 1/3
            Me.Background.ScrollHorizontal((400 - (ScreenGridWidth - (Player1.Location.X - Me.ScreenLocation.X))) / 50)

        ElseIf Player1.Location.X - Me.ScreenLocation.X < (ScreenGridWidth / 4) Then
            ' on left 1/4
            'Me.Background.ScrollHorizontal(Player1.Location.X - RenderObject.ScreenLocation.X)
            Me.Background.ScrollHorizontal(-(400 - (Player1.Location.X - Me.ScreenLocation.X)) / 50)
        End If
    End Sub


    ''' <summary>
    ''' Renders scene onto g
    ''' </summary>
    ''' <param name="g"></param>
    Public Overrides Sub RenderObjects(g As Graphics)
        Background.Render(g)

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
        Me.GlobalFrameCount += 1
    End Sub
End Class

<JsonObject(ItemRequired:=Required.Always)>
Public Class JsonMapObject
    Public Property MapName As String
    Public Property Blocks As Dictionary(Of String, IList(Of Object()))
    Public Property Entities As Dictionary(Of String, IList(Of Object()))
    Public Property Background As List(Of Object)
    Public Property Theme As RenderTheme
End Class

Public Enum RenderTheme
    Overworld
    Underground
    Castle
End Enum

Public NotInheritable Class JsonMapReader
    ''' <summary>
    ''' Creates a new <see cref="MapScene">object, from a json file in resources</see>
    ''' </summary>
    ''' <param name="jsonName"></param>
    ''' <returns></returns>
    Public Shared Function ReadMapFromResource(jsonName As String, parent As Control) As MapScene
        Dim byteArray = CType(My.Resources.ResourceManager.GetObject(jsonName), Byte())
        If byteArray(0) = 239 And byteArray(1) = 187 And byteArray(2) = 191 Then
            byteArray = byteArray.Skip(3).Take(byteArray.Length - 2).ToArray()
        End If
        Dim str = Text.Encoding.UTF8.GetString(byteArray)


        Dim mapObject = JsonConvert.DeserializeObject(Of JsonMapObject)(str)

        Dim outScene As New MapScene(parent)

        ' add the Background
        outScene.SetBackground(mapObject.background(0), mapObject.background(1), mapObject.background(2))

        ' add all blocks
        For Each pair As KeyValuePair(Of String, IList(Of Object())) In mapObject.blocks
            Dim name = pair.Key
            For Each params As Object() In pair.Value
                RenderItemFactory(name, params, mapObject.theme, outScene).AddSelfToScene()
            Next
        Next

        For Each pair As KeyValuePair(Of String, IList(Of Object())) In mapObject.entities
            Dim name = pair.Key
            For Each params As Object() In pair.Value
                RenderItemFactory(name, params, mapObject.theme, outScene).AddSelfToScene()
            Next
        Next


        ' add all Entities
        Dim player1 = New EntPlayer(32, 32, New Point(0, GroundHeight), outScene)

        outScene.Setplayer(MapScene.PlayerId.Player1,player1)
        outScene.AddEntity(player1)

        OutScene.AddStatic(New StaticText(New Rectangle(0, Helper.TopToBottom(0, ScreenGridHeight/16), ScreenGridWidth / 4, ScreenGridHeight / 16), "MARIO", NES.GetFontFamily(), 18, 
                                        New SolidBrush(Color.White), outScene))

        Dim scoreText = New StaticText(New Rectangle(0, Helper.TopToBottom(ScreenGridHeight/16, ScreenGridHeight/16), ScreenGridWidth / 4, ScreenGridHeight / 16), "000000", NES.GetFontFamily(), 18, 
                                        New SolidBrush(Color.White), outScene, paddingChar := "0", paddingWidth := 6)
        outScene.AddStatic(scoreText)
        EntPlayer.ScoreCallback = scoreText

        
#If DEBUG
        DebugMapHook(outScene)
#End If
        
        'TODO REPLACE with actual backgroundMusic reader
        outScene.BackgroundMusic = BackgroundMusic.GroundTheme
        outScene.BackgroundMusic.Play()
        Return outScene

    End Function

   
    Public Class InvalidJsonException
        Inherits Exception
        Sub New(message As String)
            MyBase.New(message)
        End Sub

    End Class

    ''' <summary>
    ''' To add a new block:
    ''' add it to RenderTypes
    ''' put a case in RenderItemFactory
    ''' </summary>
    Public Enum RenderTypes
        
        GroundPlatform

        BlockBreakableBrick
        BlockBrickCoin
        BlockBrickPowerUp
        BlockBrickStar

        BlockInvis1Up
        BlockInvisNone
        BlockInvisCoin

        BlockQuestion
        BlockMushroom

        BlockMetal

        BlockPipe

        BlockCloud

        EntGoomba
        EntKoopa
        EntCoin 

        Flag
    End Enum


    Public Shared Function RenderItemFactory(name As String, params As Object(), theme As RenderTheme, scene As MapScene) As HitboxItem
        Dim out As HitboxItem'
        Dim item As RenderTypes
        Try
             item = Helper.StrToEnum(Of RenderTypes)(name)
        Catch exception As Exception
            Throw New Exception(String.Format("Cannot find item: {0} in the RenderTypes Enum", name), exception)
        End Try
        Select Case item
            Case RenderTypes.BlockBreakableBrick
                AssertLength(name, 2, params)
                out = New BlockBreakableBrick(params, theme, scene)

            Case RenderTypes.GroundPlatform
                AssertLength(name, 4, params)
                out = New GroundPlatform(params, theme, scene)

            Case RenderTypes.BlockQuestion
                AssertLength(name, 3, params)
                out = New BlockQuestion(params, theme, scene)

            Case RenderTypes.BlockMetal
                AssertLength("blockMetal", 2, params)
                out = New BlockMetal(params, scene)

            Case RenderTypes.BlockPipe
                AssertLength("blockPipe", 5, params)
                out = New BlockPipe(params, scene)
            
            Case RenderTypes.BlockBrickCoin
                AssertLength("blockBrickCoin", 2, params)
                out = New BlockBrickCoin(params, scene)

            Case RenderTypes.BlockBrickStar
                AssertLength("blockBrickStar", 2, params)
                out = New BlockBrickStar(params, scene)
            
            Case RenderTypes.BlockBrickPowerUp
                AssertLength("blockBrickPowerUp", 2, params)
                out = New BlockBrickPowerup(params, scene)

            Case RenderTypes.BlockCloud
                AssertLength("blockCloud", 2, params)
                out = New BlockCloud(params, scene)

            Case RenderTypes.EntGoomba
                AssertLength("entGoomba", 2, params)
                out = New EntGoomba(params, scene)

            Case RenderTypes.EntKoopa
                AssertLength("entKoopa", 2, params)
                out = New EntKoopa(params, scene)
            
            Case RenderTypes.Flag
                AssertLength("flag", 2, params)
                out = New Flag(params, scene)
            
            Case RenderTypes.BlockInvis1up
                AssertLength("blockInvis1Up", 2, params)
                out = New BlockInvis1Up(params, scene)
            
            Case RenderTypes.BlockInvisNone
                AssertLength("blockInvisNone", 2, params)
                out = New BlockInvisNone(params, scene)
            
            Case RenderTypes.BlockInvisCoin
                AssertLength("BlockInvisCoin", 2, params)
                out = New BlockInvisCoin(params, scene)

            Case Else

                Throw New Exception(String.Format("No object with name {0}", name))
        End Select

        Return out

    End Function

    Private Shared Sub AssertLength(type As String, expected As Integer, array As Object())
        If expected = -1
            Return
        End If

        If expected <> array.length Then
            Throw New JsonMapReader.InvalidJsonException(String.Format("Error in JSON, powerup={0}, given {1} elements when {2} expected - [ {3} ] ",
                                                                       type, array.length, expected, String.Join(", ", array)))
        End If
    End Sub

    Private Shared Sub AssertLength(type As String, expected As Integer(), array As Object())

        If Not expected.Contains(array.length) Then
            Throw New JsonMapReader.InvalidJsonException(String.Format("Error in JSON, powerup={0}, given {1} elements when {2} expected - [ {3} ] ",
                                                                       type, array.length, expected, String.Join(", ", array)))
        End If
    End Sub

    ''' <summary>
    ''' Dont let this class be instantialised
    ''' </summary>
    Private Sub New
    End Sub
End Class