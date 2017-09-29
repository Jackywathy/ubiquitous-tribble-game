Public Class BlockRotatePipe
    Inherits Block
    Public Sub New(location As Point, mapScene As MapScene)
        MyBase.New(StandardWidth*2, StandardHeight*2, location, Sprites.PipeRotate, mapScene)
    End Sub

    Public Overrides Sub CollisionLeft(sender As Entity)
        MyBase.CollisionLeft(sender)

    End Sub
End Class
