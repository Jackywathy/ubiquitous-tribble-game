Public Class StartScene
    Inherits BaseScene

    Private text As List(Of StaticText)

    Public Overrides Sub HandleInput()
        ' scroll select 1/2 player
    End Sub

    Sub New(handler As KeyHandler)
        Mybase.New(handler)
    End Sub

    Public Overrides Sub UpdateTick()
        Throw New NotImplementedException()
    End Sub

    Public Overrides Sub RenderScene(g As Graphics)
        Throw New NotImplementedException()
    End Sub
End Class
