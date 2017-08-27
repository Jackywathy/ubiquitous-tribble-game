﻿Imports Newtonsoft.Json
Imports WinGame

''' <summary>
''' Base scene:
''' given to MainGame to render scenes
''' </summary>
Public MustInherit Class BaseScene
    ''' <summary>
    ''' FrameCount for each scene
    ''' </summary>
    Public FrameCount As Integer

    ''' <summary>
    ''' Background of scene
    ''' </summary>
    Friend Background As BackgroundRender

    ''' <summary>
    ''' Run once per tick in game
    ''' </summary>
    Public MustOverride Sub UpdateTick()

    ''' <summary>
    ''' Draws the completed scene onto the graphics object
    ''' </summary>
    ''' <param name="g"></param>
    Public MustOverride Sub RenderScene(g As Graphics)

    ''' <summary>
    ''' Handles input: default, do nothing
    ''' </summary>
    Public Overridable Sub HandleInput()

    End Sub

    ''' <summary>
    ''' Has all of keys which are held down etc.
    ''' </summary>
    Friend KeyControl As MainGame.KeyHandler

    Sub New(keyControl As MainGame.KeyHandler)
        Me.KeyControl = keyControl
    End Sub




End Class


''' <summary>
''' Scene that represents a map, (probably loaded from json using <see cref="MapScene.ReadMapFromResource"/>
''' </summary>
Public Class MapScene
    Inherits BaseScene

    ''' <summary>
    ''' Contains all normal blocks
    ''' </summary>
    Public AllObjects As New HashSet(Of RenderObject)

    ''' <summary>
    ''' Contains all entities
    ''' </summary>
    Public AllEntities As New HashSet(Of Entity)

    ''' <summary>
    ''' Contains all objects that have collisionss
    ''' </summary>
    Public AllObjAndEnt As New HashSet(Of RenderObject)

    ''' <summary>
    ''' Location that the scene is rendered at
    ''' </summary>
    Public ScreenLocation As Point

    ''' <summary>
    ''' Contains all objects in scene that need to be rendered
    ''' </summary>
    Private ReadOnly inSceneItems As New HashSet(Of RenderObject)
    
    ''' <summary>
    ''' Contains objects that will be removed once <see cref="RemoveAllDeleted"/> is run
    ''' Used in for each loops to avoid mutating object immediately
    ''' </summary>
    Private ReadOnly toRemoveObjects As New HashSet(Of RenderObject)

    ''' <summary>
    ''' Contains objects that will be added once <see cref="AddAllAdded"/> is run
    ''' Used in for each loops to avoid mutating object immediately
    ''' </summary>
    Private ReadOnly toAddObjects As New HashSet(Of RenderObject)

    ''' <summary>
    ''' List of staticitems. Items will be rendered in order inserted
    ''' </summary>
    Public AllStaticItems As New List(Of StaticItem)

    ''' <summary>
    ''' Player1
    ''' </summary>
    Public Player1 As EntPlayer

    ''' <summary>
    ''' Player2
    ''' </summary>
    Public Player2 As EntPlayer

   

    Public Sub New(keyControl As MainGame.KeyHandler)
        MyBase.New(keyControl)
    End Sub

    ''' <summary>
    ''' Creates a new <see cref="MapScene">object, from a json file in resources</see>
    ''' </summary>
    ''' <param name="jsonName"></param>
    ''' <returns></returns>
    Public Shared Function ReadMapFromResource(jsonName As String, keyControl As MainGame.KeyHandler) As MapScene
        Dim byteArray = CType(My.Resources.ResourceManager.GetObject(jsonName), Byte())
        If byteArray(0) = 239 And byteArray(1) = 187 And byteArray(2) = 191 Then
            byteArray = byteArray.Skip(3).Take(byteArray.Length - 2).ToArray()
        End If
        Dim str = Text.Encoding.UTF8.GetString(byteArray)


        Dim mapObject = JsonConvert.DeserializeObject(Of MapObject)(str)

        Dim outScene As New MapScene(keyControl)

        ' add the Background
        outScene.SetBackground(mapObject.background(0), mapObject.background(1), mapObject.background(2))

        ' add all blocks
        For Each pair As KeyValuePair(Of String, IList(Of Object())) In mapObject.blocks
            Dim name = pair.Key
            For Each params As Object() In pair.Value
                outScene.RenderItemFactory(name, params).AddSelfToScene()
            Next
        Next

        For Each pair As KeyValuePair(Of String, IList(Of Object())) In mapObject.entities
            Dim name = pair.Key
            For Each params As Object() In pair.Value
                outScene.RenderItemFactory(name, params).AddSelfToScene()
            Next
        Next

        ' add all entities
        Dim player1 = New EntPlayer(32, 32, New Point(0, GroundHeight), outScene)

        outScene.Player1 = player1
        outScene.AddEntity(player1)
        outScene.AddEntity(New EntCoin(32, 32, New Point(320, 96), outScene))

        Dim x As New StaticText(New RectangleF(0, 0, ScreenGridWidth / 4, ScreenGridHeight / 32), "MARIO", CustomFontFamily.NES.GetFontFamily(), 18, New SolidBrush(Color.White), StringAlignment.Near, StringAlignment.Near)
        outScene.AddItem(x)
        x = New StaticText(New RectangleF(0, ScreenGridHeight / 32, ScreenGridWidth / 4, ScreenGridHeight / 16), "000000", CustomFontFamily.NES.GetFontFamily(), 18, New SolidBrush(Color.White), StringAlignment.Near, StringAlignment.Near)
        outScene.AddItem(x)

        Return outScene
    End Function

    Public Enum RenderTypes
        BlockBreakableBrick
        BrickPlatform
        BlockQuestion
        BlockMetal
        BlockPipe
        BlockInvis
        BlockBrickPowerUp

        EntGoomba
        EntKoopa
    End Enum

    Private Function RenderItemFactory(name As String, params As Object()) As RenderObject
        Dim out As RenderObject
        Select Case Helper.StrToEnum(Of RenderTypes)(name)
            Case RenderTypes.BlockBreakableBrick
                AssertLength("blockBreakableBrick", 2, params.Length, params)
                out = New BlockBreakableBrick(params, Me)
            Case RenderTypes.BrickPlatform
                AssertLength("brickPlatform", 4, params.Length, params)
                out = New BrickPlatform(params, Me)
            Case RenderTypes.BlockQuestion
                AssertLength("blockQuestion", 3, params.Length, params)
                out = New BlockQuestion(params, Me)
            Case RenderTypes.BlockMetal
                AssertLength("blockMetal", 2, params.Length, params)
                out = New BlockMetal(params, Me)
            Case RenderTypes.BlockInvis
                AssertLength("blockInvis", 2, params.Length, params)
                out = New BlockInvis(params, Me)
            Case RenderTypes.BlockBrickPowerUp
                AssertLength("blockPowerBrickPowerUp", New Integer() {3, 4}, params.Length, params)
                out = New BlockBrickPowerUp(params, Me)
            Case RenderTypes.BlockPipe
                AssertLength("blockPipe", 5, params.Length, params)
                out = New BlockPipe(params, Me)

            Case RenderTypes.EntGoomba
                AssertLength("entGoomba", 2, params.Length, params)
                out = New EntGoomba(params, Me)
            Case RenderTypes.EntKoopa
                AssertLength("entKoopa", 2, params.Length, params)
                out = New EntKoopa(params, Me)

            Case Else

                Throw New Exception(String.Format("No object with name {0}", name))
        End Select

        Return out

    End Function

    ''' <summary>
    ''' Gets/Updates the blocks that are in the mapScene and need to be rendened.
    ''' Should be called once per physics tick
    ''' </summary>
    ''' <returns>Objects in mapScene</returns>
    Public Function GetObjInScene() As HashSet(Of RenderObject)
        InSceneItems.Clear()
        For Each item As RenderObject In AllObjects
            If item.InScene() Then
                InSceneItems.Add(item)
            End If
        Next
        Return InSceneItems
    End Function

    ''' <summary>
    ''' Adds a obj (not entity to the mapScene)
    ''' </summary>
    ''' <param name="args"></param>
    Sub AddObject(ByVal ParamArray args() As RenderObject)
        For Each item As RenderObject In args
            AllObjects.Add(item)
            AllObjAndEnt.Add(item)
        Next
    End Sub

    Sub AddItem(ByVal ParamArray args() As StaticItem)
        For Each item As StaticItem In args
            AllStaticItems.Add(item)
        Next
    End Sub

    ''' <summary>
    ''' Adds entity to the mapScene
    ''' </summary>
    ''' <param name="args"></param>
    Public Sub AddEntity(ByVal ParamArray args() As Entity)
        For Each item As Entity In args
            AllEntities.Add(item)
            AllObjAndEnt.Add(item)
        Next
    End Sub

    ''' <summary>
    ''' Prepares item to be removed once <see cref="RemoveAllDeleted">is run</see>
    ''' </summary>
    ''' <param name="args"></param>
    Sub PrepareRemove(ByVal ParamArray args() As RenderObject)
        For Each item As RenderObject In args
            toRemoveObjects.Add(item)
        Next
    End Sub

    ''' <summary>
    ''' Prepares item to be added
    ''' </summary>
    ''' <param name="args"></param>
    Sub PrepareAdd(ByVal ParamArray args() As RenderObject)
        For Each item As RenderObject In args
            toAddObjects.Add(item)
        Next
    End Sub

    ''' <summary>
    ''' Actually remove all entities
    ''' </summary>
    Sub RemoveAllDeleted()
        For Each item As RenderObject In toRemoveObjects
            If item.GetType.IsSubclassOf(GetType(Entity))
                AllEntities.Remove(item)
            Else
                AllObjects.Remove(item)
            End If
            AllObjAndEnt.Remove(item)
        Next
        toRemoveObjects.Clear()
    End Sub

    ''' <summary>
    ''' Actually add all items
    ''' </summary>
    Sub AddAllAdded()
        For Each item As RenderObject In toAddObjects
            If item.GetType.IsSubclassOf(GetType(Entity))
                AllEntities.Add(item)
            Else
                AllObjects.Add(item)
            End If
            AllObjAndEnt.Add(item)
        Next
        toAddObjects.Clear()
    End Sub

    ''' <summary>
    ''' Sets the Background of the mapScene, using a hex color
    ''' </summary>
    ''' <param name="hexColor"></param>
    Sub SetBackground(hexColor As String, width As Integer, height As Integer)
        If Background IsNot Nothing
            Background.Dispose()
        End If
        Me.Background = New BackgroundRender(width, height, hexColor, Me)
    End Sub

    ''' <summary>
    ''' Handles/ticks input from the user
    ''' </summary>
    Public Overrides Sub HandleInput()
        Dim xToMove = 0
        Dim yToMove = 0

        ' LEFT
        If MainGame.KeyHandler.MoveLeft Then
            If Not Player1.IsCrouching Then
                xToMove = -Player1.moveSpeed.x
            End If

            ' Yes this is really badly hard-coded
            If Player1.IsCrouching And Not Player1.AllowedToUncrouch And MainGame.KeyHandler.MoveUp And Player1.AllowJumpInput Then
                xToMove = -Player1.moveSpeed.x
            End If
        End If

        ' RIGHT
        If MainGame.KeyHandler.MoveRight Then
            If Not Player1.IsCrouching Then
                xToMove = Player1.moveSpeed.x

            End If
            If Player1.IsCrouching And Not Player1.AllowedToUncrouch And MainGame.KeyHandler.MoveUp And Player1.AllowJumpInput Then
                xToMove = Player1.moveSpeed.x
            End If
        End If

        ' UP
        If MainGame.KeyHandler.MoveUp And Player1.AllowJumpInput Then
            yToMove = Player1.moveSpeed.y
            Player1.AllowJumpInput = False
            Sounds.Jump.Play(fromStart:=True)
        ElseIf MainGame.KeyHandler.MoveUp = False Then
            Player1.AllowJumpInput = True
        End If

        ' DOWN
        If MainGame.KeyHandler.MoveDown Then
            If Player1.State > PlayerStates.Small And Player1.isGrounded = True Then 'crouch
                Player1.OnCrouch(True)
            End If
        ElseIf Player1.State > PlayerStates.Small And Player1.IsCrouching = True Then
            ' TODO - check for collision on above blocks before uncrouching
            Player1.OnCrouch(False)
        End If

        If Player1.State = PlayerStates.Fire And MainGame.KeyHandler.MoveDown And Player1.AllowShoot Then
            Player1.TryShootFireball()
            Player1.AllowShoot = False
        ElseIf Not MainGame.KeyHandler.MoveDown Then
            Player1.AllowShoot = True
        End If

        If Not Player1.isDead Then
            Player1.AccelerateX(xToMove)
            Player1.AccelerateY(yToMove, False)
        End If


    End Sub

    Public Overrides Sub UpdateTick()
        ' animate and update position of each entity

        For Each item As RenderObject In AllObjAndEnt
            If Helper.IsEntity(item) Then
                Dim ent As Entity = item
                ent.UpdatePos()
                'End If
            End If
            item.animate()
        Next

        AddAllAdded()
        RemoveAllDeleted()

        ' TODO - chuck into function - scrolls screen if player is close to edge
        If Player1.Location.X - Me.ScreenLocation.X > (ScreenGridWidth / 4 * 3) Then
            ' on right 1/4
            Me.Background.ScrollHorizontal((400 - (ScreenGridWidth - (Player1.Location.X - Me.ScreenLocation.X))) / 50)

        ElseIf Player1.Location.X - Me.ScreenLocation.X < (ScreenGridWidth / 4) Then
            ' on left 1/4
            'Me.Background.ScrollHorizontal(Player1.Location.X - RenderObject.ScreenLocation.X)
            Me.Background.ScrollHorizontal(-(400 - (Player1.Location.X - Me.ScreenLocation.X)) / 50)
        End If
        FrameCount += 1
    End Sub

    Public Overrides Sub RenderScene(g As Graphics)


        Background.Render(g)

        ' all text & stuff
        For Each item As StaticItem In AllStaticItems
            item.Render(g)
        Next

        Dim objects = GetObjInScene()

        ' render objects
        For Each item As RenderObject In objects
            item.Render(g)
        Next
        ' render entities
        For Each item In AllEntities
            item.Render(g)
        Next
        Me.FrameCount += 1
    End Sub


    Private Sub AssertLength(type As String, expected As Integer, given As Integer, array As Object())
        If expected = -1
            Return
        End If

        If expected <> given Then
            Throw New InvalidJsonException(String.Format("Error in JSON, type={0}, given {1} elements when {2} expected - [ {3} ] ",
                                                         type, given, expected, String.Join(", ", array)))
        End If
    End Sub

    Private Sub AssertLength(type As String, expected As Integer(), given As Integer, array As Object())

        If Not expected.Contains(given) Then
            Throw New InvalidJsonException(String.Format("Error in JSON, type={0}, given {1} elements when {2} expected - [ {3} ] ",
                                                         type, given, expected, String.Join(", ", array)))
        End If
    End Sub




    Private Class InvalidJsonException
        Inherits Exception
        Sub New(message As String)
            MyBase.New(message)
        End Sub

    End Class
End Class

<JsonObject(ItemRequired:=Required.Always)>
Public Class MapObject
    Public Property map_name As String
    Public Property blocks As Dictionary(Of String, IList(Of Object()))
    Public Property entities As Dictionary(Of String, IList(Of Object()))
    Public Property background As List(Of Object)
    Public Property background_music As String


End Class