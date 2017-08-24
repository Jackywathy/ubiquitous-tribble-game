Imports System.Data.Common
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

    Private toRemoveObjects As New HashSet(Of RenderObject)
    Private toAddObjects As New HashSet(Of RenderObject)
    
    Public AllItems As New HashSet(Of RenderItem)


    Public Player1 As EntPlayer

    Public ScreenLocation As Point

    ' background of scene
    Private background As BackgroundRender

    ' global framecounter
    Public frameCount As Integer


    ' dictionary containing all scenes : {map_name : scene}
    Private AllScenes As New Dictionary(Of String, Scene)

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
    Sub AddObject(ByVal ParamArray args() As RenderObject)
        For Each item As RenderObject In args
            AllObjects.Add(item)
            AllObjAndEnt.Add(item)
        Next
    End Sub

    Sub AddItem(ByVal ParamArray args() As RenderItem)
        For Each item As RenderItem In args
            AllItems.Add(item)
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

    

    ''' <summary>
    ''' Prepares item to be removed once <see cref="RemoveAllDeleted">is run</see>
    ''' </summary>
    ''' <param name="args"></param>
    Sub PrepareRemove(ByVal ParamArray args() As RenderObject)
        For each item As RenderObject in args
            ToRemoveObjects.Add(item)
        Next
    End Sub

    ''' <summary>
    ''' Prepares item to be added
    ''' </summary>
    ''' <param name="args"></param>
    Sub PrepareAdd(ByVal ParamArray args() As RenderObject)
        For each item As RenderObject in args
            toAddObjects.Add(item)
        Next
    End Sub


    ''' <summary>
    ''' Actually remove all entities
    ''' </summary>
    Sub RemoveAllDeleted()
        For each item As RenderObject In ToRemoveObjects
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
        For each item As RenderObject In toAddObjects
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
    ''' Sets the background of the scene. Uses the resource name string.
    ''' </summary>
    ''' <param name="hexColor"></param>
    Sub SetBackground(hexColor As String, width As Integer, height As Integer)
        if background IsNot Nothing
            background.Dispose()
        End If
        Me.background = New BackgroundRender(width, height, hexColor, Me)
    End Sub

    ''' <summary>
    ''' Handles/ticks input from the user
    ''' This also 
    ''' </summary>
    Public Sub HandleInput()

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
            ' TO DO - check for collision on above blocks before uncrouching
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



    ''' <summary>
    ''' Updates the physics for the game
    ''' </summary>
    ''' <param name="numframes"></param>
    Sub UpdatePhysics(numframes As Integer)
        ' animate and update position of each entity

        ' TODO - gravity is only applied to the player, in the handle input function
        For Each item As RenderObject In AllObjAndEnt
            If item.GetType.IsSubclassOf(GetType(Entity)) Then
                If CType(item, Entity).isDead = False Then
                    CType(item, Entity).UpdatePos()
                End If
            End If
                item.animate()
        Next

        AddAllAdded()
        RemoveAllDeleted()

        ' TODO - chuck into function - scrolls screen if player is close to edge
        If Player1.Location.X - Me.screenLocation.X > (ScreenGridWidth / 4 * 3) Then
            ' on right 1/4
            Me.Background.ScrollHorizontal((400 - (ScreenGridWidth - (Player1.Location.X - Me.screenLocation.X))) / 50)

        ElseIf Player1.Location.X - Me.screenLocation.X < (ScreenGridWidth / 4) Then
            ' on left 1/4
            'Me.Background.ScrollHorizontal(Player1.Location.X - RenderObject.ScreenLocation.X)
            Me.Background.ScrollHorizontal(-(400 - (Player1.Location.X - Me.screenLocation.X)) / 50)
        End If

    End Sub

    ''' <summary>
    ''' Renders the entire scene onto a graphics object
    ''' </summary>
    ''' <param name="g"></param>
    Sub RenderScene(g As Graphics)

        Me.frameCount += 1

        background.Render(g)

        ' all text & stuff
        For Each item As RenderItem In AllItems
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
        outScene.SetBackground(mapObject.background(0), mapObject.background(1), mapObject.background(2))

        ' add all blocks
        For Each pair As KeyValuePair(Of String, IList(Of Object())) In mapObject.blocks
            Dim name = pair.Key
            For Each params As Object() In pair.Value
                outScene.RenderItemFactory(name, params).AddSelfToScene()
            Next
        Next
        ' add all entities
        Dim player1 = New EntPlayer(32, 32, New Point(0, GroundHeight), outScene)

        outScene.player1 = player1
        outScene.AddEntity(player1)
        outScene.AddEntity(New EntGoomba(New Point(320, 64), outScene))
        Dim x As New StaticText(New RectangleF(0,0,ScreenGridWidth/4, ScreenGridHeight/32), "MARIO", CustomFontFamily.NES.GetFontFamily(), 18, New SolidBrush(Color.White), StringAlignment.Near, StringAlignment.Near)
        outScene.AddItem(x)
        x = New StaticText(New RectangleF(0,ScreenGridHeight/32,ScreenGridWidth/4,ScreenGridHeight/16 ), "000000", CustomFontFamily.NES.GetFontFamily(), 18, New SolidBrush(Color.White), StringAlignment.Near, StringAlignment.Near)
        outScene.AddItem(x)
        

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
                AssertLength("blockBreakableBrick", 2, params.Length, params)
                out = New BlockBreakableBrick(params , Me)
            Case "brickPlatform"
                AssertLength("brickPlatform", 4, params.Length, params)
                out = New BrickPlatform(params, Me)
            Case "blockQuestion"
                AssertLength("blockQuestion", 3, params.Length, params)
                out = New BlockQuestion(params, Me)
            Case "blockMetal"
                AssertLength("blockMetal", 2, params.Length, params)
                out = New BlockMetal(params, Me)
            Case "blockPipe"
                AssertLength("blockPipe", 5, params.Length, params)
                out = New BlockPipe(params, Me)
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
    Public Property background As List(Of Object)
    Public Property background_music As String



End Class