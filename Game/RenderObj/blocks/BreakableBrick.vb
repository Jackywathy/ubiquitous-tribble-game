Public Class BreakableBrick
    Inherits Block
    


    Public Sub New(location As Point)
        MyBase.New(blockWidth, blockHeight, location, My.Resources.brick)
    End Sub

    Public Overrides Sub CollisionBottom(sender As Entity)
        MyBase.CollisionBottom(sender)
        Me.Location = New Point(-32,-32)
        ' TODO - DELETE
    End Sub
End Class
