Public Class LevelSelect
    Public ReadOnly LoadableMaps as New Dictionary(Of String, MapEnum) From {
        {"1-1", MapEnum.map1_1above},
        {"1-2", MapEnum.map1_2above}
    }

    Public Sub RefreshBox
        MapList.Items.Clear()
       For each item in LoadableMaps
            MapList.Items.Add(New MapDescriptor(item.value, item.key))
       Next
    End Sub

    Private form as TitleScreen

    Sub New(parent As TitleScreen)
        ' This call is required by the designer.
        InitializeComponent()

        'Me.Width = If(width, Me.Width)
        'Me.Height = If(height, Me.Height)

        ' Add any initialization after the InitializeComponent() call.
        RefreshBox()
        MapList.Font = New Font(NES, 10, FontStyle.Bold)
        MapList.SelectedIndex = 0
        StartButton.Font = New Font(CustomFontFamily.NES, 12)
        ReturnButton.Font = New Font(CustomFontFamily.NES, 12)
        form = parent
    End Sub
    Private Sub LevelSelect_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub ReturnButton_Click(sender As Object, e As EventArgs) Handles ReturnButton.Click
        Me.Hide()
    End Sub

    Private Sub StartButton_Click(sender As Object, e As EventArgs) Handles StartButton.Click
        form.StartGame(MapList.selectedItem.map)
    End Sub
End Class

Class MapDescriptor
    Public Property name as String
    Public Property map As mapEnum
    Sub New(map As MapEnum, name As String)
        me.map = map
        Me.name = name
    End Sub

    Public Overrides Function ToString() As String
        Return name
    End Function

    
End Class