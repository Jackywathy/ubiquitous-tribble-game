Friend Module PlayerConst
    Public Const CoinsToLives = 100
    Public Const StartLives = 5
End Module


Friend Enum PlayerStates
    Small = 1
    Big = 2
    Fire = 4
    Ice = 8
    
End Enum


Public Class Player
    Inherits Entity
    Public Shared Property Lives = StartLives
    Public allowJump = True

    Private Shared _coins As Integer = 0

    Public Shared Property Coins
        Get
            Return _coins
        End Get
        Set(value)
            If value >= CoinsToLives Then
                _coins = value - CoinsToLives
            Else
                _coins += 1
            End If

        End Set
    End Property

    Sub New(width As Integer, height As Integer, location As Point, sprites As SpriteSet)
        MyBase.New(width, height, location, sprites)
    End Sub


End Class
