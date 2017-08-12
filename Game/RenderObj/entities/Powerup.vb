Imports WinGame

Public MustInherit Class Powerup
    Inherits Entity

    ' The state the powerup changes the player to
    ' 1 - big 
    ' 2 - fire 
    Public MustOverride Property state As UInt16

    Public Overrides Sub CollisionBottom(sender As Entity)
        Me.TryActivate(sender)
    End Sub
    Public Overrides Sub CollisionTop(sender As Entity)
        Me.TryActivate(sender)
    End Sub
    Public Overrides Sub CollisionLeft(sender As Entity)
        Me.TryActivate(sender)
    End Sub
    Public Overrides Sub CollisionRight(sender As Entity)
        Me.TryActivate(sender)
    End Sub

    Public Sub TryActivate(sender As Player)
        If sender.GetType = GetType(Player) Then
            sender.state = Me.state
        End If
    End Sub

    Sub New(width As Integer, height As Integer, location As Point)
        MyBase.New(width, height, location)
    End Sub

End Class
