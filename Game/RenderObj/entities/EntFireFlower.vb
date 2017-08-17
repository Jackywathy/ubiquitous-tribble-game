Imports WinGame

Public Class EntFireFlower
    Inherits EntPowerup

    Public Overrides Property state As UInt16 = 2
    Public Overrides Property moveSpeed As Distance = New Distance(0, 0)
    Public Overrides ReadOnly Property maxVeloc As Distance = New Distance(0, 0)
    Public Overrides Property SpriteSet As SpriteSet = Sprites.f_flower
    Private spawnCounter = 0

    ' TODO Replace with actual sound, or set to nothing for no sound
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

        If Not isSpawning And numFrames Mod 10 = 0 Then
            RenderImage = spriteSet.allSprites(2).First
            spriteSet.allSprites(2).Insert(0, spriteSet.allSprites(2).Last)
            spriteSet.allSprites(2).RemoveAt(spriteSet.allSprites(2).Count - 1)
        End If

    End Sub

    Sub New(width As Integer, height As Integer, location As Point, scene As Scene)
        MyBase.New(width, height, location, Sprites.f_flower, scene)

    End Sub

End Class
