Public Class BlockBrickStar
    Inherits BlockBumpable
    
    Private isUsed As Boolean = False
    Private playerWhoHit As EntPlayer
    Private willSpawnPowerup As Boolean = False

    ''' <summary>
    ''' params:
    ''' 0 : x
    ''' 1 : y
    ''' </summary>
    Public Sub New(params As Object(), theme as RenderTheme, mapScene As MapScene)
        Me.New(New Point(params(0)*32, params(1)*32), theme, mapScene)
    End Sub

    Public Sub New(location As Point,theme as RenderTheme,  mapScene As MapScene)
        MyBase.New(StandardWidth, StandardHeight, location, BlockBreakableBrick.GetSpriteSet(theme), mapScene)
    End Sub

    Public Overrides Sub CollisionBottom(sender As Entity)
        MyBase.CollisionBottom(sender)

        If Not isUsed And Helper.IsPlayer(sender) Then
            isUsed = True
            willSpawnPowerup = True
            StartBump(tRUE)
        End If

    End Sub
    Public Overrides Sub Animate()
        If isUsed Then
            RenderImage = SpriteSet.GetFirst(SpriteState.Destroy)
        End If
        If willSpawnPowerup And Not IsMoving Then
            Dim star = New EntStar(New Point(Me.Location.X, Me.Location.Y + Me.Height), MyScene)
            star.Spawn()
            Me.willSpawnPowerup = False
        End If
    End Sub
End Class
