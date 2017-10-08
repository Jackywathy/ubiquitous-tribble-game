Public Class BlockBrickPowerUp
    Inherits BlockBumpable

    Private isUsed As Boolean = False
    Private playerWhoHit As EntPlayer
    Private willSpawnPowerup As Boolean = False
    ''' <summary>
    ''' params:
    ''' 0 : x
    ''' 1 : y
    ''' </summary>
    Public Sub New(params As Object(),theme as RenderTheme,  mapScene As MapScene)
        Me.New(New Point(params(0)*32, params(1)*32), theme, mapScene)
    End Sub

    Public Sub New(location As Point,theme as RenderTheme,  mapScene As MapScene)
        MyBase.New(StandardWidth, StandardHeight, location, Sprites.brickBlock, mapScene)
    End Sub

    Private Sub SpawnPowerup(player As EntPlayer)
        Dim powerup As EntPowerup
        If player.State = PlayerStates.Small Then
            powerup = New EntMushroom(New Point(Me.Location.X, Me.Location.Y + Me.Height), MyScene)
        Else
            powerup = New EntFireFlower(New Point(Me.Location.X, Me.Location.Y + Me.Height), MyScene)
        End If
        powerup.Spawn()
    End Sub

    Public Overrides Sub CollisionBottom(sender As Entity)
        MyBase.CollisionBottom(sender)

        If Not isUsed And Helper.IsPlayer(sender) Then
            Dim player As EntPlayer = sender
            playerWhoHit = player
            isUsed = True
            StartBump()
            willSpawnPowerup = True
        End If

    End Sub
    Public Overrides Sub Animate()
        If isUsed Then
            RenderImage = SpriteSet.GetFirst(SpriteState.Destroy)
        End If
        If willSpawnPowerup And Not IsMoving Then
            Me.SpawnPowerup(playerWhoHit)
            Me.WillSpawnPowerup = False
        End If
    End Sub
End Class
