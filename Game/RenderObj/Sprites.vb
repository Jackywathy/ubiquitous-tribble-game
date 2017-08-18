Imports WinGame

Public Class SpriteSet
    Implements IDictionary(Of SpriteState, List(Of Image))

    Public AllSprites As Dictionary(Of SpriteState, List(Of Image)) ' Dict (SpriteEnum.Walk, List(Of Image) )
    Private counter As New Dictionary(Of SpriteState, Integer) ' Dict (SpriteEnum, Integer)


    Sub New(spriteSet As Dictionary(Of SpriteState, List(Of Image)), width As Integer, height As Integer, Optional otherWidth As Dictionary(Of SpriteState, Size) = Nothing)
        If otherWidth Is Nothing
            otherWidth = New Dictionary(Of SpriteState, Size)
        End If

        Me.AllSprites = spriteSet


        ' images do NOT have to be disposed here - they are references to 
        ' my.resources.<image>, and by disposing them, you wil make them
        ' unavaiable 
        Dim outDict As New Dictionary(Of SpriteState, List(Of Image))

        For Each imgPair As KeyValuePair(Of SpriteState, List(Of Image)) In spriteSet
            Dim state As SpriteState = imgPair.Key

            Dim currentWidth As Integer
            Dim currentHeight As Integer
            ' update the image size for this list of images
            Dim pair As Size
            If otherWidth.TryGetValue(imgPair.Key, pair) Then
                ' succeeded in gettng point from widthDictionary
                currentWidth = pair.Width
                currentHeight = pair.Height
            Else
                currentWidth = width
                currentHeight = height
            End If

            ' create new list
            Dim resizeList As New List(Of Image)
            For Each img As Image In imgPair.Value
                resizeList.Add(Resize(img, currentWidth, currentHeight))
            Next
            ' add completed list to out dict
            outDict.Add(state, resizeList)
            ' create animation state counter
            counter.Add(state, 0)
        Next

        Me.AllSprites = outDict

    End Sub

    Public ReadOnly Property Count As Integer Implements ICollection(Of KeyValuePair(Of SpriteState, List(Of Image))).Count
        Get
            Return AllSprites.Count
        End Get
    End Property

    Public ReadOnly Property IsReadOnly As Boolean Implements ICollection(Of KeyValuePair(Of SpriteState, List(Of Image))).IsReadOnly
        Get
            Return CType(AllSprites, IDictionary).IsReadOnly()
        End Get
    End Property

    Default Public Property Item(key As SpriteState) As List(Of Image) Implements IDictionary(Of SpriteState, List(Of Image)).Item
        Get
            Return AllSprites.Item(key)
        End Get

        Set(value As List(Of Image))
            AllSprites.Item(Key) = value
        End set
    End Property

    Public ReadOnly Property Keys As ICollection(Of SpriteState) Implements IDictionary(Of SpriteState, List(Of Image)).Keys
        Get
            Return AllSprites.Keys
        End Get
    End Property

    Public ReadOnly Property Values As ICollection(Of List(Of Image)) Implements IDictionary(Of SpriteState, List(Of Image)).Values
        Get
            Return AllSprites.Values
        End Get
    End Property

    Public Sub Add(item As KeyValuePair(Of SpriteState, List(Of Image))) Implements ICollection(Of KeyValuePair(Of SpriteState, List(Of Image))).Add
        AllSprites.Add(item.Key, item.Value)
        Counter.Add(item.Key, 0)
    End Sub

    Public Sub Add(key As SpriteState, value As List(Of Image)) Implements IDictionary(Of SpriteState, List(Of Image)).Add
        AllSprites.Add(key, value)
        counter.Add(key, 0)
    End Sub

    Public Sub Clear() Implements ICollection(Of KeyValuePair(Of SpriteState, List(Of Image))).Clear
        AllSprites.Clear()
        counter.Clear()
    End Sub

    Public Sub CopyTo(array() As KeyValuePair(Of SpriteState, List(Of Image)), arrayIndex As Integer) Implements ICollection(Of KeyValuePair(Of SpriteState, List(Of Image))).CopyTo
        Throw New NotImplementedException()
    End Sub

    Public Function Contains(item As KeyValuePair(Of SpriteState, List(Of Image))) As Boolean Implements ICollection(Of KeyValuePair(Of SpriteState, List(Of Image))).Contains
        Return AllSprites.Contains(item)
    End Function

    Public Function ContainsKey(key As SpriteState) As Boolean Implements IDictionary(Of SpriteState, List(Of Image)).ContainsKey
        Return AllSprites.ContainsKey(key)
    End Function

    Public Function GetEnumerator() As IEnumerator(Of KeyValuePair(Of SpriteState, List(Of Image))) Implements IEnumerable(Of KeyValuePair(Of SpriteState, List(Of Image))).GetEnumerator
        Return AllSprites.GetEnumerator()
    End Function

    Public Function Remove(item As KeyValuePair(Of SpriteState, List(Of Image))) As Boolean Implements ICollection(Of KeyValuePair(Of SpriteState, List(Of Image))).Remove
        counter.Remove(item.Key)
        return AllSprites.Remove(item.key)
    End Function

    Public Function Remove(key As SpriteState) As Boolean Implements IDictionary(Of SpriteState, List(Of Image)).Remove
        counter.Remove(key)
        return AllSprites.Remove(key)
    End Function
    
    Public Function TryGetValue(key As SpriteState, ByRef value As List(Of Image)) As Boolean Implements IDictionary(Of SpriteState, List(Of Image)).TryGetValue
        Return AllSprites.TryGetValue(key, value)
    End Function

    Private Function IEnumerable_GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
        Return AllSprites.GetEnumerator()
    End Function
    
    #Region "IDictionary"

    ''' <summary>
    ''' A list of integers, which represents how much each animation loop has progressed
    ''' </summary>
    Public Function SendToBack(state As SpriteState) As Image
        Dim ret = AllSprites(state)(counter(state))
        counter(state) += 1
        If counter(state) > AllSprites(state).Count - 1
            ' if counter is greater than the size of array, reset back to zero
            counter(state) = 0
        End If
        Return ret
    End Function

    






#End Region
End Class

' ===========================
' Sprite sets
' ---------------------------

Public MustInherit Class Sprites
    ' 1 - Idle/Constant (1)
    ' 0 - Ground animation (4)
    ' 2 - Jump (1)

    ' todo remove replace with SpriteStates instead of raw numbers
    Public Shared playerSmall As New SpriteSet(
        New Dictionary(Of SpriteState, List(Of Image)) From {
            {0, New List(Of Image) From {My.Resources.mario_small_1, My.Resources.mario_small_2, My.Resources.mario_small_3, My.Resources.mario_small_4}},
            {1, New List(Of Image) From {My.Resources.mario_small_1}},
            {2, New List(Of Image) From {My.Resources.mario_small_jump}}
        },
        MarioWidth,
        MarioHeightS
    )

    ' 1 - Idle/Constant (1)
    ' 0 - Ground animation (4)
    ' 2 - Jump (1)
    ' 3 - Crouch (1)

    Public Shared playerBig As New SpriteSet(
        New Dictionary(Of SpriteState, List(Of Image)) From {
            {0, New List(Of Image) From {My.Resources.mario_big_1, My.Resources.mario_big_2, My.Resources.mario_big_3, My.Resources.mario_big_4}},
            {1, New List(Of Image) From {My.Resources.mario_big_1}},
            {2, New List(Of Image) From {My.Resources.mario_big_jump}},
            {3, New List(Of Image) From {My.Resources.mario_big_crouch}}
        },
        MarioWidth,
        MarioHeightB,
        New Dictionary(Of SpriteState, Size) From {{3, New Size(32, 32)}}
    )
    ' 0 - Ground animation (4)
    ' 1 - Idle (1)
    ' 2 - Jump (1)
    ' 3 - Crouch (1)

    Public Shared playerBigFire As New SpriteSet(
        New Dictionary(Of SpriteState, List(Of Image)) From {
            {0, New List(Of Image) From {My.Resources.mario_bigf_1, My.Resources.mario_bigf_2, My.Resources.mario_bigf_3, My.Resources.mario_bigf_4}},
            {1, New List(Of Image) From {My.Resources.mario_bigf_1}},
            {2, New List(Of Image) From {My.Resources.mario_bigf_jump}},
            {3, New List(Of Image) From {My.Resources.mario_bigf_crouch}}
        },
        MarioWidth,
        MarioHeightB
    )
    ' 0 - Ground animation (4)
    ' 1 - Idle (1)
    ' 2 - Jump (1)
    ' 3 - Crouch (1)

    Public Shared playerFireball As New SpriteSet(
        New Dictionary(Of SpriteState, List(Of Image)) From {
            {0, New List(Of Image) From {My.Resources.fireball}}
        },
        16,
        16
    )
    ' 0 - Sprite (1) 
    ' (gets rotated by pi/2 for animation)

    Public Shared f_flower As New SpriteSet(
        New Dictionary(Of SpriteState, List(Of Image)) From {
            {0, New List(Of Image) From {My.Resources.f_flower_s1, My.Resources.f_flower_s2, My.Resources.f_flower_s3, My.Resources.f_flower_s4, My.Resources.f_flower_s5, My.Resources.f_flower_s6, My.Resources.f_flower_s7}},
            {1, New List(Of Image) From {My.Resources.f_flower_1}},
            {2, New List(Of Image) From {My.Resources.f_flower_1, My.Resources.f_flower_2, My.Resources.f_flower_3, My.Resources.f_flower_4}}
        },
        MarioWidth,
        MarioHeightS
    )
    ' 0 - Spawn animation (7)
    ' 1 - Single frame (1)
    ' 2 - Idle animation (4)

    Public Shared mushroom As New SpriteSet(
        New Dictionary(Of SpriteState, List(Of Image)) From {
            {0, New List(Of Image) From {My.Resources.mushroom_s1, My.Resources.mushroom_s2, My.Resources.mushroom_s3, My.Resources.mushroom_s4, My.Resources.mushroom_s5, My.Resources.mushroom_s6, My.Resources.mushroom_s7}},
            {1, New List(Of Image) From {My.Resources.mushroom}}
        },
        MarioWidth,
        MarioHeightS
    )
    ' 0 - Spawn animation (7)
    ' 1 - Single frame (1)

    Public Shared itemBlock As New SpriteSet(
        New Dictionary(Of SpriteState, List(Of Image)) From {
            {0, New List(Of Image) From {My.Resources.blockQuestion1, My.Resources.blockQuestion2, My.Resources.blockQuestion3}},
            {1, New List(Of Image) From {My.Resources.blockQuestion1}}
        },
        MarioWidth,
        MarioHeightS
    )
    ' 0 - Constant animation
    ' 1 - Idle (unused?)

    Public Shared brickBlock As New SpriteSet(
        New Dictionary(Of SpriteState, List(Of Image)) From {
            {0, New List(Of Image) From {My.Resources.blockBrick}}
        },
        MarioWidth,
        MarioHeightS
    )
    ' 0 - Constant

    Public Shared blockMetal As New SpriteSet(
        New Dictionary(Of SpriteState, List(Of Image)) From {
            {0, New List(Of Image) From {My.Resources.blockMetal}}
        },
        MarioWidth,
        MarioHeightS
        )

    ' 0 - Constant
    Public Shared KoopaRed = playerBigFire


    ' 0 - Idle
    ' 1 - GroundWalkRight
    ' 2 - GroundWalkLeft

    Public Shared EntGoomba As New SpriteSet(
    New Dictionary(Of SpriteState, List(Of Image)) From {
            {0, New List(Of Image) From {My.Resources.goomba_1}},
            {1, New List(Of Image) From {My.Resources.goomba_1, My.Resources.goomba_2}},
            {2, New List(Of Image) From {My.Resources.goomba_d}}
        },
        32,
        32
        )





    Private Sub New
        ' make this class un-intializable
    End Sub
End Class

Public Enum SpriteState
    Constant = 0
    GroundWalkRight = 1
    GroundWalkLeft = 2
    JumpRight = 3
    JumpLeft = 4
End Enum