Public Class SpriteSet
    Public allSprites As List(Of List(Of Image)) '3D Array

    Sub New(spriteSet As List(Of List(Of Image)), width As Integer, height As Integer)
        Me.allSprites = spriteSet

        ' Resize EVERY image
        Dim resizeListList As New List(Of List(Of Image))

        For Each imgList As List(Of Image) In spriteSet

            Dim resizeList As New List(Of Image)

            For Each img As Image In imgList
                resizeList.Add(Resize(img, width, height))
                img.Dispose() ' images MUST be disposed EXPLICITLY, or else they leak memory :(
            Next

            resizeListList.Add(resizeList)

        Next

        Me.allSprites = resizeListList

    End Sub

End Class

' ===========================
' Sprite sets
' ---------------------------

Public Module Sprites
    Public player = New SpriteSet(
        New List(Of List(Of Image)) From {
            New List(Of Image) From {My.Resources.mario_small_1, My.Resources.mario_small_2, My.Resources.mario_small_3, My.Resources.mario_small_4},
            New List(Of Image) From {My.Resources.mario_small_1},
            New List(Of Image) From {My.Resources.mario_small_jump}
        },
        MarioWidth,
        MarioHeight
    )
    ' 1 - Ground animation
    ' 2 - Idle
    ' 3 - Jump
End Module
