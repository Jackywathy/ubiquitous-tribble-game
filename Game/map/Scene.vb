Imports Newtonsoft.Json

Public Class Scene
    Public AllItems As New List(Of RenderObject)

    Private InSceneItems As New List(Of RenderObject)


    Public Function GetObjInScene() As List(Of RenderObject)
        InSceneItems.Clear()
        For Each item As RenderObject In AllItems
            If item.InScene() Then
                InSceneItems.Add(item)
            End If
        Next
        Return InSceneItems
    End Function
    Sub New(ByVal ParamArray args() As RenderObject)
        For Each item As RenderObject In args
            AllItems.Add(item)
        Next
    End Sub

    Sub Add(ByVal ParamArray args() As RenderObject)
        For Each item As RenderObject In args
            AllItems.Add(item)
        Next
    End Sub


    Public Background As BackgroundRender
    Public Player1 = Entities.player1

    Sub LoadTestLevel()
        Background = New BackgroundRender(TotalGridWidth, TotalGridHeight, My.Resources.placeholderLevel)

        Dim platform As New Platform(TotalGridWidth, 50, New Point(0, 0), My.Resources.platform)
        ' the items added later are rendered later!

        Dim brick As New BreakableBrick(New Point(100, 150))
        Dim brick1 As New BreakableBrick(New Point(300, 100))
        Dim brick2 As New BreakableBrick(New Point(332, 100))
        Dim brick3 As New BreakableBrick(New Point(364, 100))

        Dim brick4 As New BreakableBrick(New Point(396, 100))

        Dim question As New ItemBlock(New Point(428, 100))

        Me.Add(brick, platform, brick1, brick2, brick3, brick4, question)
        ReadMapFromResource("testmap")
    End Sub

    Sub RenderScene(g As Graphics)
        Background.Render(g)
        ' check collision before rending object
        Dim objects = GetObjInScene()

        For Each item As RenderObject In objects
            Entities.player1.CheckCollision(item)
        Next

        For Each item As RenderObject In objects
            item.Render(g)
        Next

        Entities.player1.Render(g)
    End Sub

    Public Sub ReadMapFromResource(name As String)
        Dim byteArray = CType(My.Resources.ResourceManager.GetObject(name), Byte())
        If byteArray(0) = 239 And byteArray(1) = 187 And byteArray(2) = 191 Then
            byteArray = byteArray.Skip(3).Take(byteArray.Length - 2).ToArray()
        End If
        Dim str = System.Text.Encoding.UTF8.GetString(byteArray)


        Console.Out.Write(str)
        Dim MapObject As MapObject = JsonConvert.DeserializeObject(Of MapObject)(str)


    End Sub
End Class

Public Class MapObject
    Public Property map_name As String
    Public Property blocks As Dictionary(Of String, IList(Of Integer()))



End Class