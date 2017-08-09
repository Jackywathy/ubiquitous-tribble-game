Public Class Scene
    Private AllItems As New List(Of RenderObject)

    Private InSceneItems As New List(Of RenderObject)
    

    Public Function GetObjInScene() As List(Of RenderObject)
        InSceneItems.Clear()
        For each item As RenderObject In AllItems
            If item.InScene()
                InSceneItems.Add(item)
            End If
        Next
    Return InSceneItems
    End Function
    Sub New(ByVal ParamArray args() As RenderObject)
        For each item As RenderObject In args
            AllItems.Add(item)
        Next
    End Sub

    Sub Add(ByVal ParamArray args() as RenderObject)
        For each item As RenderObject In args
            AllItems.Add(item)
        Next
    End Sub
    Dim temp
    Sub LoadTestLevel()
        Dim background = New BackgroundRender(TotalGridWidth, TotalGridHeight, My.Resources.placeholderLevel)
        Dim brick As New BreakableBrick(100, 100, New Point(10, 300))
        Dim platform As New Platform(TotalGridWidth, 50, New Point(0, 0), My.Resources.platform)
        temp = platform
        ' the items added later are rendered later!
        Dim player = Entities.player1

        Me.Add(Background, brick, Platform, Player)
    End Sub
    Sub RenderScene(g As Graphics)
        for each item as RenderObject in GetObjInScene()
            item.Render(g)
        Next
        Entities.player1.CheckCollision(temp)
    End Sub
End Class
