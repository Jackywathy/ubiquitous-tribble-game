Public Class SpriteSet
    Implements IDictionary(Of SpriteState, List(Of Image))

    Public Readonly AllSprites As Dictionary(Of SpriteState, List(Of Image)) ' Dict (SpriteEnum.Walk, List(Of Image) )
    Private ReadOnly counter As New Dictionary(Of SpriteState, Integer) ' Dict (SpriteEnum, Integer)


    Sub New(spriteSet As Dictionary(Of SpriteState, List(Of Image)), width As Integer, height As Integer, Optional otherWidth As Dictionary(Of SpriteState, Size) = Nothing, Optional autoRotate As Boolean = True)
        If otherWidth Is Nothing
            otherWidth = New Dictionary(Of SpriteState, Size)
        End If

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
        'If autoRotate
        '    ' do auto-flipping if needed
        '    if AllSprites.ContainsKey(SpriteState.GroundWalkLeft) And Not AllSprites.ContainsKey(SpriteState.GroundWalkRight)
        '        Dim flipped As New List(Of Image)
        '        For each img As Image In AllSprites(SpriteState.GroundWalkLeft)
        '            flipped.Add(Flip(img))
        '        Next
        '        AllSprites.Add(SpriteState.GroundWalkRight, flipped)
        '    End If
        'End if
        Me.AllSprites = outDict
    End Sub
    
    ''' <summary>
    ''' Returns the current image in the selected sprite animation
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


    #Region "IDictionary"
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
    
    #End Region
End Class


' ===========================
' Sprite sets
' ---------------------------

Public Enum SpriteState
    ConstantRight = 0        ' The set to use if nothing is happening to the Ent. Also use .ConstantRight if only one main animation
    Constant = 0
    ConstantLeft = 1

    GroundRight = 2
    GroundLeft = 3

    AirRight = 4
    AirLeft = 5

    Spawn = 6
    Destroy = 7

    CrouchRight = 8
    CrouchLeft = 9
End Enum

Public MustInherit Class Sprites
    Public Shared playerSmall As New SpriteSet(
        New Dictionary(Of SpriteState, List(Of Image)) From {
            {SpriteState.GroundRight, New List(Of Image) From {My.Resources.mario_small_1, My.Resources.mario_small_2, My.Resources.mario_small_3, My.Resources.mario_small_4}},
            {SpriteState.ConstantRight, New List(Of Image) From {My.Resources.mario_small_1}},
            {SpriteState.AirRight, New List(Of Image) From {My.Resources.mario_small_jump}},
            {SpriteState.GroundLeft, New List(Of Image) From {My.Resources.mario_small_1r, My.Resources.mario_small_2r, My.Resources.mario_small_3r, My.Resources.mario_small_4r}},
            {SpriteState.ConstantLeft, New List(Of Image) From {My.Resources.mario_small_1r}},
            {SpriteState.AirLeft, New List(Of Image) From {My.Resources.mario_small_jumpr}},
            {SpriteState.Destroy, New List(Of Image) From {My.Resources.mario_small_dead}}
        },
        32,
        32
    )

    Public Shared playerBig As New SpriteSet(
        New Dictionary(Of SpriteState, List(Of Image)) From {
            {SpriteState.GroundRight, New List(Of Image) From {My.Resources.mario_big_1, My.Resources.mario_big_2, My.Resources.mario_big_3, My.Resources.mario_big_4}},
            {SpriteState.ConstantRight, New List(Of Image) From {My.Resources.mario_big_1}},
            {SpriteState.AirRight, New List(Of Image) From {My.Resources.mario_big_jump}},
            {SpriteState.CrouchRight, New List(Of Image) From {My.Resources.mario_big_crouch}},
            {SpriteState.GroundLeft, New List(Of Image) From {My.Resources.mario_big_1r, My.Resources.mario_big_2r, My.Resources.mario_big_3r, My.Resources.mario_big_4r}},
            {SpriteState.ConstantLeft, New List(Of Image) From {My.Resources.mario_big_1r}},
            {SpriteState.AirLeft, New List(Of Image) From {My.Resources.mario_big_jumpr}},
            {SpriteState.CrouchLeft, New List(Of Image) From {My.Resources.mario_big_crouchr}}
        },
        32,
        64
    )

    Public Shared playerBigFire As New SpriteSet(
        New Dictionary(Of SpriteState, List(Of Image)) From {
            {SpriteState.GroundRight, New List(Of Image) From {My.Resources.mario_bigf_1, My.Resources.mario_bigf_2, My.Resources.mario_bigf_3, My.Resources.mario_bigf_4}},
            {SpriteState.ConstantRight, New List(Of Image) From {My.Resources.mario_bigf_1}},
            {SpriteState.AirRight, New List(Of Image) From {My.Resources.mario_bigf_jump}},
            {SpriteState.CrouchRight, New List(Of Image) From {My.Resources.mario_bigf_crouch}},
            {SpriteState.GroundLeft, New List(Of Image) From {My.Resources.mario_bigf_1r, My.Resources.mario_bigf_2r, My.Resources.mario_bigf_3r, My.Resources.mario_bigf_4r}},
            {SpriteState.ConstantLeft, New List(Of Image) From {My.Resources.mario_bigf_1r}},
            {SpriteState.AirLeft, New List(Of Image) From {My.Resources.mario_bigf_jumpr}},
            {SpriteState.CrouchLeft, New List(Of Image) From {My.Resources.mario_bigf_crouchr}}
        },
        32,
        64
    )

    Public Shared playerFireball As New SpriteSet(
        New Dictionary(Of SpriteState, List(Of Image)) From {
            {SpriteState.ConstantRight, New List(Of Image) From {My.Resources.fireball}},
            {SpriteState.Destroy, New List(Of Image) From {My.Resources.fireball_expl_1, My.Resources.fireball_expl_2, My.Resources.fireball_expl_3}}
        },
        16,
        16,
        New Dictionary(Of SpriteState, Size) From {{SpriteState.Destroy, New Size(32, 32)}}
    )

    Public Shared f_flower As New SpriteSet(
        New Dictionary(Of SpriteState, List(Of Image)) From {
            {SpriteState.Spawn, New List(Of Image) From {My.Resources.f_flower_s1, My.Resources.f_flower_s2, My.Resources.f_flower_s3, My.Resources.f_flower_s4, My.Resources.f_flower_s5, My.Resources.f_flower_s6, My.Resources.f_flower_s7}},
            {SpriteState.ConstantRight, New List(Of Image) From {My.Resources.f_flower_1, My.Resources.f_flower_2, My.Resources.f_flower_3, My.Resources.f_flower_4}}
        },
        32,
        32
    )

    Public Shared mushroom As New SpriteSet(
        New Dictionary(Of SpriteState, List(Of Image)) From {
            {SpriteState.Spawn, New List(Of Image) From {My.Resources.mushroom_s1, My.Resources.mushroom_s2, My.Resources.mushroom_s3, My.Resources.mushroom_s4, My.Resources.mushroom_s5, My.Resources.mushroom_s6, My.Resources.mushroom_s7}},
            {SpriteState.ConstantRight, New List(Of Image) From {My.Resources.mushroom}}
        },
        32,
        32
    )

    Public Shared itemBlock As New SpriteSet(
        New Dictionary(Of SpriteState, List(Of Image)) From {
            {SpriteState.ConstantRight, New List(Of Image) From {My.Resources.blockQuestion1, My.Resources.blockQuestion2, My.Resources.blockQuestion3}}
        },
        32,
        32
    )

    Public Shared brickBlock As New SpriteSet(
        New Dictionary(Of SpriteState, List(Of Image)) From {
            {SpriteState.ConstantRight, New List(Of Image) From {My.Resources.blockBrick}}
        },
        32,
        32
    )

    Public Shared blockMetal As New SpriteSet(
        New Dictionary(Of SpriteState, List(Of Image)) From {
            {SpriteState.ConstantRight, New List(Of Image) From {My.Resources.blockMetal}}
        },
        32,
        32
        )

    Public Shared koopaGreen As New SpriteSet(
    New Dictionary(Of SpriteState, List(Of Image)) From {
            {SpriteState.ConstantRight, New List(Of Image) From {My.Resources.koopa_green_1, My.Resources.koopa_green_2}},
            {SpriteState.Destroy, New List(Of Image) From {My.Resources.koopa_green_shell1, My.Resources.koopa_green_shell2}}
        },
        32,
        64
    )

    Public Shared goomba As New SpriteSet(
        New Dictionary(Of SpriteState, List(Of Image)) From {
            {SpriteState.ConstantRight, New List(Of Image) From {My.Resources.goomba_1, My.Resources.goomba_2}},
            {SpriteState.Destroy, New List(Of Image) From {My.Resources.goomba_d, My.Resources.goomba_d2}}
        },
        32,
        32
    )

    Public Shared coin As New SpriteSet(
        New Dictionary(Of SpriteState, List(Of Image)) From {
            {SpriteState.ConstantRight, New List(Of Image) From {My.Resources.coin_idle_1, My.Resources.coin_idle_2, My.Resources.coin_idle_3}}
        },
        32,
        32
    )

    Public Shared coinFromBlock As New SpriteSet(
        New Dictionary(Of SpriteState, List(Of Image)) From {
            {SpriteState.ConstantRight, New List(Of Image) From {My.Resources.coin_hit_1, My.Resources.coin_hit_2, My.Resources.coin_hit_3, My.Resources.coin_hit_4}}
        },
        32,
        32
    )
    Public Shared blockInvis As New SpriteSet(
        New Dictionary(Of SpriteState,List(Of Image)) From {
            {SpriteState.Constant, New List(Of Image) From {My.Resources.blockInvis}}
        },
        32,
        32
    )


    Private Sub New()
        ' make this class un-intializable
    End Sub
End Class

