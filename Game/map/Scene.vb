Imports Newtonsoft.Json

Public Class Scene
    ' Contains all objects (no entityes)
    Public AllObjects As New HashSet(Of RenderObject)
    ' Contains all entityes (no objects)
    Public AllEntities As New HashSet(Of Entity)
    ' contains everything
    Public AllObjAndEnt As New HashSet(Of RenderObject)

    ' a reference to the background music playing
    Public BackgroundSound As MusicPlayer

    ' all the objects in the scene
    Private InSceneItems As New HashSet(Of RenderObject)

    ' dictionary containing all scenes : {map_name : scene}
    Private AllScenes As New Dictionary(Of String, Scene)

    Public player1 As EntPlayer

    Public screenLocation As Point

    ' background of scene
    Private background As BackgroundRender

    ''' <summary>
    ''' Gets/Updates the blocks that are in the scene and need to be rendened.
    ''' Should be called once per physics tick
    ''' </summary>
    ''' <returns>Objects in scene</returns>
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
    ''' Adds a obj (not entity to the scene)
    ''' </summary>
    ''' <param name="args"></param>
    Sub AddObj(ByVal ParamArray args() As RenderObject)
        For Each item As RenderObject In args
            AllObjects.Add(item)
            AllObjAndEnt.Add(item)
        Next
    End Sub

    ''' <summary>
    ''' Adds entity to the scene
    ''' </summary>
    ''' <param name="args"></param>
    Public Sub AddEntity(ByVal ParamArray args() As Entity)
        For Each item As Entity In args
            AllEntities.Add(item)
            AllObjAndEnt.Add(item)
        Next
    End Sub

    Private ToRemoveObjects As New HashSet(Of Entity)

    ''' <summary>
    ''' Adds entity to be removed once <see cref="RemoveAllDeleted">is run</see>
    ''' </summary>
    ''' <param name="args"></param>
    Sub PrepareRemove(ByVal ParamArray args() As RenderObject)
        For each item As RenderObject in args
            ToRemoveObjects.Add(item)
        Next
    End Sub


    Sub RemoveAllDeleted()
        For each item As RenderObject In ToRemoveObjects
            If item.GetType.IsSubclassOf(GetType(Entity))
                AllEntities.Remove(item)
            Else 
                AllObjects.Remove(item)
            End If
            AllObjAndEnt.Remove(item)
        Next
    End Sub

    ''' <summary>
    ''' Sets the background of the scene. Uses the resource name string.
    ''' </summary>
    ''' <param name="name"></param>
    Sub SetBackground(name As String)
        if background IsNot Nothing
            background.Dispose()
        End If
        Me.background = New BackgroundRender(TotalGridWidth, TotalGridHeight, My.Resources.ResourceManager.GetObject(name), Me)
    End Sub

    ''' <summary>
    ''' Sets background of scene. Uses an image
    ''' </summary>
    ''' <param name="image"></param>
    Sub SetBackground(image As Image)
        Me.Background = New BackgroundRender(TotalGridWidth, TotalGridHeight, image, Me)
    End Sub

    ''' <summary>
    ''' Handles/ticks input from the user
    ''' This also 
    ''' </summary>
    Public Sub HandleInput() 
        ' UP
        If MainGame.KeyHandler.MoveUp And player1.allowJump Then
            player1.AccelerateY(player1.moveSpeed.y)
            player1.allowJump = False
        ElseIf MainGame.KeyHandler.MoveUp = False Then
            player1.allowJump = True
        End If

        ' DOWN
        If MainGame.KeyHandler.MoveDown Then
            If player1.state > 0 And player1.isGrounded = True Then 'crouch
                player1.onCrouch(True)
            End If
        ElseIf player1.state > 0 And player1.isCrouching = True Then
            ' TO DO - check for collision on above blocks before uncrouching
            player1.onCrouch(False)
        End If

        If player1.state = 2 And MainGame.KeyHandler.MoveDown And player1.allowShoot Then
            player1.tryShootFireball()
            player1.allowShoot = False
        ElseIf Not MainGame.KeyHandler.MoveDown Then
            player1.allowShoot = True
        End If

        ' LEFT
        If MainGame.KeyHandler.MoveLeft Then
            player1.AccelerateX(-player1.moveSpeed.x)
        End If

        ' RIGHT
        If MainGame.KeyHandler.MoveRight Then
            player1.AccelerateX(player1.moveSpeed.x)

        End If



    End Sub

    ''' <summary>
    ''' Updates the physics for the game
    ''' </summary>
    ''' <param name="numframes"></param>
    Sub UpdatePhysics(numframes As Integer)
        ' animate and update position of each entity



        ' TODO - gravity is only applied to the player, in the handle input function
        For itemCount = 0 To (AllObjAndEnt.Count - 1)
            If AllObjAndEnt(itemCount) IsNot Nothing Then
                AllObjAndEnt(itemCount).Animate(numframes)
                If AllObjAndEnt(itemCount).GetType.IsSubclassOf(GetType(Entity)) Then
                    Dim ent As Entity = AllObjAndEnt(itemCount)
                    ent.UpdatePos()
                End If
            End If
        Next
        RemoveAllDeleted()

        ' TODO - chuck into function - scrolls screen if player is close to edge
        If Player1.Location.X - RenderObject.screenLocation.X > (ScreenGridWidth / 4 * 3) Then
            ' on right 1/4
            Me.Background.ScrollHorizontal((400 - (ScreenGridWidth - (Player1.Location.X - RenderObject.screenLocation.X))) / 50)

        ElseIf Player1.Location.X - RenderObject.screenLocation.X < (ScreenGridWidth / 4) Then
            ' on left 1/4
            'Me.Background.ScrollHorizontal(Player1.Location.X - RenderObject.screenLocation.X)
            Me.Background.ScrollHorizontal(-(400 - (Player1.Location.X - RenderObject.screenLocation.X)) / 50)
        End If

    End Sub

    ''' <summary>
    ''' Renders the entire scene onto a graphics object
    ''' </summary>
    ''' <param name="g"></param>
    Sub RenderScene(g As Graphics)

        Background.Render(g)

        Dim objects = GetObjInScene()

        ' OLD
        'For Each entity As Entity In AllEntities
        '    For Each obj As RenderObject In AllObjAndEnt
        '        entity.CheckCollision(obj)
        '    Next
        'Next


        

        ' render objects
        For Each item As RenderObject In objects
            item.Render(g)
        Next
        ' render entities
        For Each item In AllEntities
            item.Render(g)
        Next
    End Sub


    ''' <summary>
    ''' Creates a new <see cref="Scene">object, from a json file in resources</see>
    ''' </summary>
    ''' <param name="jsonName"></param>
    ''' <returns></returns>
    Public Shared Function ReadMapFromResource(jsonName As String) As Scene
        Dim byteArray = CType(My.Resources.ResourceManager.GetObject(jsonName), Byte())
        If byteArray(0) = 239 And byteArray(1) = 187 And byteArray(2) = 191 Then
            byteArray = byteArray.Skip(3).Take(byteArray.Length - 2).ToArray()
        End If
        Dim str = Text.Encoding.UTF8.GetString(byteArray)


        Dim mapObject = JsonConvert.DeserializeObject(Of MapObject)(str)

        Dim outScene As New Scene

        ' add the background
        outScene.SetBackground(mapObject.background)

        ' add all blocks
        For Each pair As KeyValuePair(Of String, IList(Of Object())) In mapObject.blocks
            Dim name = pair.Key
            For Each params As Object() In pair.Value
                outScene.AddObj(outScene.RenderItemFactory(name, params))
            Next
        Next
        ' add all entities
        Dim player1 = New EntPlayer(32, 32, New Point(0, GroundHeight), outScene)
        outScene.player1 = player1
        outScene.AddEntity(player1)
        outScene.AddEntity(New EntKoopa(New Point(320, 64), outScene))
        Return outScene
    End Function

    Private Sub AssertLength(type As String, expected As Integer, given As Integer, array As Object())
        If expected <> given Then
            Throw New InvalidJsonException(String.Format("Error in JSON, type={0}, given {1} elements when {2} expected - [ {3} ] ",
                                                         type, given, expected, String.Join(", ", array)))
        End If
    End Sub

    Private Function RenderItemFactory(name As String, params As Object()) As RenderObject
        Dim out As RenderObject
        Select Case name
            Case "blockBreakableBrick"
                AssertLength("bbrick", 2, params.Length, params)
                out = New BlockBreakableBrick(New Point(params(0), params(1)), Me)
            Case "brickplatform"
                AssertLength("brickplatform", 4, params.Length, params)
                out = New BrickPlatform(params(0), params(1), New Point(params(2), params(3)), Me)
            Case "blockQuestion"
                AssertLength("itemblock", 2, params.Length, params)
                out = New BlockQuestion(New Point(params(0), params(1)), Me)
            Case "blockMetal"
                AssertLength("blockmetal", 2, params.Length, params)
                out = New BlockMetal(New Point(params(0), params(1)), Me)
            Case Else

                Throw New Exception(String.Format("No object with name {0}", name))
        End Select

        Return out

    End Function


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
    Public Property background As String
    Public Property background_music As String



End Class