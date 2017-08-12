Imports WinGame

Public Class Mushroom
    Inherits Powerup

    Public Overrides Property spriteSet As List(Of Image) = Sprites.mushroom

    Public Overrides Property state As UInt16 = 1
    Public Overrides Property moveSpeed As Velocity = New Velocity(5, 0)
    Public Overrides ReadOnly Property maxVeloc As Velocity = New Velocity(8, Forces.terminalVeloc)

    Public Sub Spawn(location As Point)

    End Sub

    Sub New(width As Integer, height As Integer, location As Point)
        MyBase.New(width, height, location)
    End Sub

End Class
