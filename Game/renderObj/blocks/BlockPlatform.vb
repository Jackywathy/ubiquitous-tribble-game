Public Class BlockPlatform
    Inherits Block

    Public Sub New(location As Point, mapScene As MapScene)
        MyBase.New(96, 16, location, Sprites.blockPlatform, mapScene)
    End Sub

    Public Sub New(params As Object(), scene as MapScene)
        Me.New(New Point(params(0) * 32, params(1)* 32), scene)
    End Sub

    Const IsStationary as Boolean = False

    Public OVerrides Sub UpdateLocation
        If IsStationary
            me.Y -= 1
            if Me.Y < 0
                Me.Y = MyScene.Height
            End If
        End if
    End Sub
End Class
