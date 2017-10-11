Public Class BlockPlatform
    Inherits Block

    Public Sub New(location As Point, mapScene As MapScene)
        MyBase.New(96, 16, location, Sprites.blockPlatform, mapScene)
    End Sub

    Public Sub New(params As Object(), scene as MapScene)
        Me.New(New Point(params(0) * 32, params(1)* 32), scene)
    End Sub

    Public OVerrides Sub UpdateLocation
        me.Y += 1
        if Me.Y > MyScene.Height
            Me.Y = 0
        End If
    End Sub
End Class
