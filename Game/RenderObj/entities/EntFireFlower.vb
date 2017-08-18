Public Class EntFireFlower
    Inherits EntPowerup

    Public Overrides Property state As UInt16 = PlayerStates.Fire
    Public Overrides Property moveSpeed As Distance = New Distance(0, 0)
    Public Overrides ReadOnly Property maxVeloc As Distance = New Distance(0, 0)
    Private spawnCounter = 0

    ' TODO REPLACE WITH FIRE_FIRE SOUND
    Public Overrides Property PickupSound As MusicPlayer = Sounds.MushroomPickup

   

    Sub New(width As Integer, height As Integer, location As Point, scene As Scene)
        MyBase.New(width, height, location, Sprites.f_flower, scene)

    End Sub


    

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

    

End Class
