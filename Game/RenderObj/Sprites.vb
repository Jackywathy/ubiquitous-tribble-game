Public Class SpriteSet

    Public ground As List(Of Image)
    Public idle As Image
    Public jump As Image

    Sub New(ground As List(Of Image), idle As Image, jump As Image)
        Me.ground = ground
        Me.idle = idle
        Me.jump = jump
    End Sub

End Class

' ===========================
' Sprite sets
' ---------------------------

Public Module Sprites
    Public player = New SpriteSet(
        New List(Of Image) From {My.Resources.mario_small_1, My.Resources.mario_small_2, My.Resources.mario_small_3, My.Resources.mario_small_4},
        My.Resources.mario_small_1,
        My.Resources.mario_small_jump)
End Module
