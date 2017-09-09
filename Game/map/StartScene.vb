Public Class StartScene
    Inherits MapScene

    Private text As List(Of StaticText)

    Public Overrides Sub HandleInput()
        ' scroll select 1/2 player
    End Sub

    Sub New(handler As KeyHandler)
        Mybase.New(handler)

    End Sub

End Class
