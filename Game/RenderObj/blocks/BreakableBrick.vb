Public Class BreakableBrick
    Inherits Block
    Public Sub New(width As Integer, height As Integer, location As Point)
        MyBase.New(width, height, location, My.Resources.brick)
    End Sub

    Public Overrides Sub Collision(sender As RenderObject, dir As Directions)
        MyBase.Collision(sender, dir)
        Select Case dir
            Case Directions.Bottom

            Case Else

        End Select
    End Sub
End Class
