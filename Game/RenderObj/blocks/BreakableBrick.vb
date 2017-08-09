Public Class BreakableBrick
    Inherits Block
    Public Sub New(width As Integer, height As Integer, location As Point)
        MyBase.New(width, height, location, My.Resources.brick)
    End Sub


End Class
