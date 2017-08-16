Public Class EntMushroom
    Inherits EntPowerup

    Public Overrides Property spriteSet As SpriteSet = Sprites.mushroom
    Private spawnCounter = 0
    Public Overrides Property state As UInt16 = 1
    Public Overrides Property moveSpeed As Distance = New Distance(1, 0)
    Public Overrides ReadOnly Property maxVeloc As Distance = New Distance(1.5, Forces.terminalVeloc)

    Public Overrides Property PickupSound As MusicPlayer = Sounds.MushroomPickup

    Public Overrides Sub Animate(numFrames As Integer)
        If isSpawning And numFrames Mod 5 = 0 Then
            If spawnCounter = 6 Then
                isSpawning = False
                RenderImage = spriteSet.allSprites(1)(0)
            Else
                RenderImage = spriteSet.allSprites(0)(spawnCounter).Clone
                spawnCounter += 1
            End If
        End If
    End Sub

    Public Overrides Sub UpdatePos()
        If Not isSpawning Then
            Me.AccelerateX(moveSpeed.x)
        End If
        MyBase.UpdatePos()
    End Sub

    Sub New(width As Integer, height As Integer, location As Point, scene As Scene)
        MyBase.New(width, height, location, scene)
    End Sub

End Class
