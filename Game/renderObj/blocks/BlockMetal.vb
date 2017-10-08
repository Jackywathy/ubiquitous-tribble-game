Public Class BlockMetal
    Inherits Block

    Sub New(location As Point, theme As RenderTheme, mapScene As MapScene)
        MyBase.New(StandardWidth, StandardHeight, location, GetSpriteSet(theme), mapScene)
    End Sub

    Sub New(params As Object(), theme As RenderTheme, mapScene As MapScene)
        Me.New(New Point(params(0)*32, params(1)*32),theme, mapScene)
    End Sub
    
    Private Shared Function GetSpriteSet(theme as RenderTheme) As SpriteSet
        Select Case theme
            Case RenderTheme.Overworld
                Return Sprites.blockmetal
            Case RenderTheme.Underground
                Return Sprites.blockMetalunder
            Case Else
                throw new Exception()
        End Select
    End Function
End Class
