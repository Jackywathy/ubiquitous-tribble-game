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


Public Class  Player
    Inherits Entity
    Public Shared Property Lives = StartLives


    Private Shared _coins as integer = 0
    Public Shared Property Coins
        Get
            return _coins
        End Get
        Set(value)
            If _coins + value > CoinsToLives
                _coins = (_coins+value)-CoinsToLives
            Else
                _coins += 1
            End If

        End Set
    End Property

    Sub New(width as integer, height as integer, location as Point, sprites As spriteset)
        MyBase.New(width, height, location, sprites)
    End Sub
End Class
