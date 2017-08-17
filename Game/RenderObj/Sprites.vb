Public Class SpriteSet
    Implements IList(Of List(Of Image))

    Public AllSprites As List(Of List(Of Image)) '2D Array

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
            ' increase the counter list
            counter.Add(0)
        Next

        Me.allSprites = resizeListList

    End Sub

    ''' <summary>
    ''' A list of integers, which represents how much each animation loop has progressed
    ''' </summary>
    Private counter As New List(Of Integer)

    Public Function GetNext(listNum As Integer) As Image
        Dim ret = AllSprites(listNum)(counter(listNum))
        counter(ListNum) += 1
        If counter(ListNum) > AllSprites(listNum).Count -1 
            ' if counter is greater than the size of array, reset back to zero
            counter(listNum) = 0
        End If
        return ret
    End Function

    Public ReadOnly Property Count As Integer Implements ICollection(Of List(Of Image)).Count
        Get
            Return AllSprites.Count
        End Get
    End Property

    Public ReadOnly Property IsReadOnly As Boolean Implements ICollection(Of List(Of Image)).IsReadOnly
        Get
            Return CType(Me.AllSprites,IList).IsReadOnly
        End Get
    End Property

    Default Public Property Item(index As Integer) As List(Of Image) Implements IList(Of List(Of Image)).Item
        Get
            Return AllSprites.Item(index)
        End Get
        Set(value As List(Of Image))
            AllSprites.Item(index) = value
        End Set
    End Property

    Public Sub Add(item As List(Of Image)) Implements ICollection(Of List(Of Image)).Add
        AllSprites.Add(item)
    End Sub

    Public Sub Clear() Implements ICollection(Of List(Of Image)).Clear
        AllSprites.Clear()
    End Sub

    Public Sub CopyTo(array() As List(Of Image), arrayIndex As Integer) Implements ICollection(Of List(Of Image)).CopyTo
        Me.AllSprites.CopyTo(array, ArrayIndex)
    End Sub

    Public Sub Insert(index As Integer, item As List(Of Image)) Implements IList(Of List(Of Image)).Insert
        AllSprites.Insert(index, item)
    End Sub

    Public Sub RemoveAt(index As Integer) Implements IList(Of List(Of Image)).RemoveAt
        AllSprites.RemoveAt(index)
    End Sub

    Public Function Contains(item As List(Of Image)) As Boolean Implements ICollection(Of List(Of Image)).Contains
        Return ALlSprites.Contains(item)
    End Function

    Public Function GetEnumerator() As IEnumerator(Of List(Of Image)) Implements IEnumerable(Of List(Of Image)).GetEnumerator
        Return AllSprites.GetEnumerator()
    End Function

    Public Function IndexOf(item As List(Of Image)) As Integer Implements IList(Of List(Of Image)).IndexOf
        Return AllSprites.IndexOf(item)
    End Function

    Public Function Remove(item As List(Of Image)) As Boolean Implements ICollection(Of List(Of Image)).Remove
        Return AllSprites.Remove(item)
    End Function

    Private Function IEnumerable_GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
        Return AllSprites.GetEnumerator()
    End Function
End Class

' ===========================
' Sprite sets
' ---------------------------

Public Module Sprites
    Public playerSmall = New SpriteSet(
        New List(Of List(Of Image)) From {
            New List(Of Image) From {My.Resources.mario_small_1, My.Resources.mario_small_2, My.Resources.mario_small_3, My.Resources.mario_small_4},
            New List(Of Image) From {My.Resources.mario_small_1},
            New List(Of Image) From {My.Resources.mario_small_jump}
        },
        MarioWidth,
        MarioHeightS
    )
    ' 0 - Ground animation (4)
    ' 1 - Idle (1)
    ' 2 - Jump (1)
    ' 3 - Crouch (1)

    Public playerBig = New SpriteSet(
        New List(Of List(Of Image)) From {
            New List(Of Image) From {My.Resources.mario_big_1, My.Resources.mario_big_2, My.Resources.mario_big_3, My.Resources.mario_big_4},
            New List(Of Image) From {My.Resources.mario_big_1},
            New List(Of Image) From {My.Resources.mario_big_jump},
            New List(Of Image) From {My.Resources.mario_big_crouch}
        },
        MarioWidth,
        MarioHeightB
    )
    ' 0 - Ground animation (4)
    ' 1 - Idle (1)
    ' 2 - Jump (1)
    ' 3 - Crouch (1)

    Public playerBigFire = New SpriteSet(
        New List(Of List(Of Image)) From {
            New List(Of Image) From {My.Resources.mario_bigf_1, My.Resources.mario_bigf_2, My.Resources.mario_bigf_3, My.Resources.mario_bigf_4},
            New List(Of Image) From {My.Resources.mario_bigf_1},
            New List(Of Image) From {My.Resources.mario_bigf_jump},
            New List(Of Image) From {My.Resources.mario_bigf_crouch}
        },
        MarioWidth,
        MarioHeightB
    )
    ' 0 - Ground animation (4)
    ' 1 - Idle (1)
    ' 2 - Jump (1)
    ' 3 - Crouch (1)

    Public playerFireball = New SpriteSet(
        New List(Of List(Of Image)) From {
            New List(Of Image) From {My.Resources.fireball}
        },
        16,
        16
    )
    ' 0 - Sprite (1) 
    ' (gets rotated by pi/2 for animation)

    Public f_flower = New SpriteSet(
        New List(Of List(Of Image)) From {
            New List(Of Image) From {My.Resources.f_flower_s1, My.Resources.f_flower_s2, My.Resources.f_flower_s3, My.Resources.f_flower_s4, My.Resources.f_flower_s5, My.Resources.f_flower_s6, My.Resources.f_flower_s7},
            New List(Of Image) From {My.Resources.f_flower_1},
            New List(Of Image) From {My.Resources.f_flower_1, My.Resources.f_flower_2, My.Resources.f_flower_3, My.Resources.f_flower_4}
        },
        MarioWidth,
        MarioHeightS
    )
    ' 0 - Spawn animation (7)
    ' 1 - Single frame (1)
    ' 2 - Idle animation (4)

    Public mushroom = New SpriteSet(
        New List(Of List(Of Image)) From {
            New List(Of Image) From {My.Resources.mushroom_s1, My.Resources.mushroom_s2, My.Resources.mushroom_s3, My.Resources.mushroom_s4, My.Resources.mushroom_s5, My.Resources.mushroom_s6, My.Resources.mushroom_s7},
            New List(Of Image) From {My.Resources.mushroom}
        },
        MarioWidth,
        MarioHeightS
    )
    ' 0 - Spawn animation (7)
    ' 1 - Single frame (1)

    Public itemBlock = New SpriteSet(
        New List(Of List(Of Image)) From {
            New List(Of Image) From {My.Resources.blockQuestion1, My.Resources.blockQuestion2, My.Resources.blockQuestion3},
            New List(Of Image) From {My.Resources.blockQuestion1}
        },
        MarioWidth,
        MarioHeightS
    )
    ' 0 - Constant animation
    ' 1 - Idle (unused?)

    Public brickBlock = New SpriteSet(
        New List(Of List(Of Image)) From {
            New List(Of Image) From {My.Resources.blockBrick}
        },
        MarioWidth,
        MarioHeightS
    )
    ' 0 - Constant

    Public blockMetal = New SpriteSet(
        New List(Of List(Of Image)) From {
            New List(Of Image) From {My.Resources.blockMetal}
        },
        MarioWidth,
        MarioHeightS
        )

    ' 0 - Constant
    Public KoopaRed = playerBigFire
End Module
