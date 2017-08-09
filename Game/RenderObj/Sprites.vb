Public Class SpriteSet

    Public ground As List(Of Image)
    Public idle As Image
    Public jump As Image

    Sub New(ground As List(Of Image), idle As Image, jump As Image, width As Integer, height As Integer)
        ' resize ALL images to the right size
        Dim resizeList as New List(Of Image)
        For each img As Image in ground
            resizeList.Add(Resize(img, width, height))
            img.Dispose()
        Next

        Me.ground = resizeList
        Me.idle = Resize(idle, width, height)
        idle.Dispose()
        ' images MUST be disposed EXPLICITLY, or else they leak memory :(

        Me.jump = Resize(jump, width, height)
        jump.Dispose()
    End Sub

End Class

' ===========================
' Sprite sets
' ---------------------------

Public Module Sprites
    Public player = New SpriteSet(
        New List(Of Image) From {My.Resources.mario_small_1, My.Resources.mario_small_2, My.Resources.mario_small_3, My.Resources.mario_small_4},
        My.Resources.mario_small_1,
        My.Resources.mario_small_jump, MarioWidth, MarioHeight)
End Module
