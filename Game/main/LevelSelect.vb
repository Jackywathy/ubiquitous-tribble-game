Public Class LevelSelect
    Public ReadOnly LoadableMaps as New Dictionary(Of String, MapEnum) From {
        {"1-1", MapEnum.map1_1above},
        {"1-2", MapEnum.map1_2above}
    }

    Public Sub RefreshBox
        MapList.Items.Clear()
       For each item in LoadableMaps
            MapList.Items.Add(item.Key)
       Next
    End Sub
    Sub New(optional width as Integer? = Nothing, Optional height As integer? = Nothing)
        ' This call is required by the designer.
        InitializeComponent()

        'Me.Width = If(width, Me.Width)
        'Me.Height = If(height, Me.Height)

        ' Add any initialization after the InitializeComponent() call.
        RefreshBox()
    End Sub
    Private Sub LevelSelect_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class

Class MapDescriptor
    Private name as String
    Private map As mapEnum
    Sub New(map As MapEnum, name As String)
        me.map = map
        Me.name = name
    End Sub

    Public Overrides Function ToString() As String
        Return name
    End Function
End Class