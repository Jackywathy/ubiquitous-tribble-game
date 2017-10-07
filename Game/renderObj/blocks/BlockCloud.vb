Public Class BlockCloud
    Inherits Block

    Sub New(location As Point,theme as RenderTheme,  mapScene As MapScene)
        MyBase.New(StandardWidth, StandardHeight, location, sprites.blockMetal, mapScene)
    End Sub

    Sub New(params As Object(),theme as RenderTheme,  mapScene As MapScene)
        Me.New(New Point(params(0)*32, params(1)*32),theme, mapScene)
    End Sub
End Class
