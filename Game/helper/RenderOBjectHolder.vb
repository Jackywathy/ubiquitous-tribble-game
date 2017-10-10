Public Class RenderObjectHolder(Of T As {GameItem})
    Implements ICollection(Of T)
    Private list as New List(Of T)
    
    Sub New

    End Sub
    #Region "ICollection"
    Public ReadOnly Property Count As Integer Implements ICollection(Of T).Count
        Get
            Return list.Count
        End Get
    End Property

    Public ReadOnly Property IsReadOnly As Boolean Implements ICollection(Of T).IsReadOnly
        Get
            Return DirectCast(list, ICollection(Of T)).IsReadOnly
        End Get
    End Property

    Public Sub Add(item As T) Implements ICollection(Of T).Add
        list.Add(item)
    End Sub

    Public Sub Clear() Implements ICollection(Of T).Clear
        list.Clear()
    End Sub

    Public Sub CopyTo(array() As T, arrayIndex As Integer) Implements ICollection(Of T).CopyTo
        list.CopyTo(array, arrayIndex)
    End Sub

    Public Function Contains(item As T) As Boolean Implements ICollection(Of T).Contains
        Return list.Contains(item)
    End Function

    Public Function GetEnumerator() As IEnumerator(Of T) Implements IEnumerable(Of T).GetEnumerator
        return list.GetEnumerator()
    End Function

    Public Function Remove(item As T) As Boolean Implements ICollection(Of T).Remove
        return list.Remove(item)
    End Function

    Private Function IEnumerable_GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
        Return GetEnumerator()
    End Function
    #End Region
End Class
