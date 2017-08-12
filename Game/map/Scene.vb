Imports Newtonsoft.Json

Public Class Scene
    ' Contains all objects (no entityes)
    Public AllObjects As New List(Of RenderObject)
    ' Contains all entityes (no objects)
    Public AllEntites As New List(Of Entity)
    ' contains everything
    Public AllObjAndEnt As New List(Of RenderObject)


    Private InSceneItems As New List(Of RenderObject)

    Public Function GetObjInScene() As List(Of RenderObject)
        InSceneItems.Clear()
        For Each item As RenderObject In AllObjects
            If item.InScene() Then
                InSceneItems.Add(item)
            End If
        Next
        Return InSceneItems
    End Function

    Sub New(ByVal ParamArray args() As RenderObject)
        For Each item As RenderObject In args
            AllObjects.Add(item)
            AllObjAndEnt.Add(item)
        Next
    End Sub

    Sub Add(ByVal ParamArray args() As RenderObject)
        For Each item As RenderObject In args
            AllObjects.Add(item)
            AllObjAndEnt.Add(item)
        Next
    End Sub

    Public Sub AddEntity(ByVal ParamArray args() As Entity)
        For Each item As Entity In args
            AllEntites.Add(item)
            AllObjAndEnt.Add(item)
        Next
    End Sub


    Sub SetBackground(name As String)
        Me.Background = New BackgroundRender(TotalGridWidth, TotalGridHeight, My.Resources.ResourceManager.GetObject(name))
    End Sub
    Sub SetBackground(image as Image)
        Me.Background = New BackgroundRender(TotalGridWidth, TotalGridHeight, image)
    End Sub

    Public Background As BackgroundRender
    Public Player1 = Entities.player1


   

    Sub UpdatePhysics(numframes As integer)
        ' update each entity's position
        For each item in AllEntites
            item.UpdatePos(numFrames)
        Next
        if player1.Location.X - RenderObject.screenLocation.X  > (ScreenGridWidth  /4 * 3)
            ' on right 1/4
            Me.Background.ScrollHorizontal(10)

        Else if player1.Location.X - RenderObject.screenLocation.X < (ScreenGridWidth  /4)
            ' on left 1/4
            Me.Background.ScrollHorizontal(-10)
        End If
    End Sub

    Sub RenderScene(g As Graphics)
        
        Background.Render(g)
        
        Dim objects = GetObjInScene()

        ' check collision of all entitys with all other obj, including entities
        For each entity As Entity in AllEntites
            For each obj As RenderObject In AllObjAndEnt
                entity.CheckCollision(obj)
            Next
        Next

        ' render objects
        For Each item As RenderObject In objects
            item.Render(g)
        Next
        ' render entities
        For each item in AllEntites
            item.Render(g)
        Next
    End Sub



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
        For each pair as KeyValuePair(Of String, Ilist(Of Integer())) in mapObject.objects
            Dim name = pair.Key
            For each params As Integer() In pair.Value
                outScene.Add(outScene.RenderItemFactory(name, params))
            Next
        Next
        outScene.AddEntity(Entities.player1)
        Return outScene
    End Function
    
    Private Sub AssertLength(type As String, expected As Integer, given As Integer, array As Integer())
        if expected <> given
            Throw New InvalidJsonException(String.Format("Error in JSON, type={0}, given {1} elements when {2} expected - [ {3} ] ", 
                                                         type, given, expected, String.Join(", ", array)))
        End If
    End Sub

    Private Function RenderItemFactory(name As String, params As Integer()) As RenderObject
        Dim out as RenderObject
        Select Case name
            Case "breakablebrick"
                AssertLength("bbrick", 2, params.Length, params)
                out = New BreakableBrick(New Point(params(0), params(1)))
            Case "brickplatform"
               AssertLength("brickplatform", 4, params.Length, params)
                out = New BrickPlatform(params(0), params(1), New Point(params(2), params(3)))
            Case "itemblock"
                AssertLength("itemblock", 2, params.Length, params)
                out = New ItemBlock(New Point(params(0), params(1)))
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

<JsonObject(ItemRequired:= Required.Always)>
Public Class MapObject
    Public Property map_name As String
    Public Property objects As Dictionary(Of String, IList(Of Integer()))
    Public Property background As String



End Class