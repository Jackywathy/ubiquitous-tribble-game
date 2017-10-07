Public Class BlockInvisNone
    Inherits BlockBumpable

    Friend Revealed As Boolean = False

    ''' <summary>
    ''' Params:
    ''' 0 : x
    ''' 1 : y
    ''' </summary>
    Public Sub New(params As Object(), theme As RenderTheme, mapScene As MapScene)
        Me.New(New Point(params(0)*32, params(1)*32), Sprites.blockInvis, mapScene)
    End Sub

    Public Sub New(location As Point, spriteset As SpriteSet, mapScene As MapScene)
        MyBase.New(StandardWidth, StandardHeight, location, spriteSet, mapScene)
    End Sub

   Protected Overridable Sub Activate

   End Sub

    Public Overrides Sub CollisionBottom(sender As Entity)
        MyBase.CollisionBottom(sender)
        If sender.GetType = GetType(EntPlayer)  Then
            revealed = True
            Activate()
        End If
    End Sub

    Public Overrides Sub Animate()
        If revealed
            RenderImage = SpriteSet(SpriteState.Revealed)(0)
        End If
    End Sub
End Class
