Public Class Scene
    Public AllItems As New List(Of RenderObject)

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

        DIm question As New ItemBlock(New Point(428, 100))

        Me.Add(brick, platform, brick1, brick2, brick3, brick4, question)
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
End Class