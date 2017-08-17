
Public Class EntKoopa
    Inherits Entity

    Public Sub New(location As Point, scene As Scene)
        MyBase.New(32, 64, location, Sprites.KoopaRed, scene)
    End Sub


    Public Overrides Property RenderImage As Image = spriteSet.allSprites(0)(0)

End Class
