Public Class TitleScreen
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim helpTxt = "Super Mario Bros. HD 720p™ is a faithful recreation of the classic 1985 NES game. Use WASD to move, Space to shoot fireballs, and Esc to pause the game."
        MsgBox(helpTxt, MsgBoxStyle.Information, "Help")
    End Sub

    Private Sub TitleScreen_WillClose(sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        If MessageBox.Show("Are you sure you want to quit?", "Confirm exit", MessageBoxButtons.YesNo) <> DialogResult.Yes Then
            e.Cancel = True
        End If
    End Sub

    Private Sub StartButton_Click(sender As Object, e As EventArgs) Handles StartButton.Click
        game.EnableTimer()

        'Game.game.QueueMapChangeWithStartScene(PlayerStartScreen, Nothing)
        game.game.QueueMapChangeWithStartScene(MapEnum.None, Nothing)
        game.ShowDialog()

    End Sub

    Sub New()
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        game = New GameForm(False)

    End Sub
    Private game As GameForm
End Class


Public Module MainProgram
    Public Sub Main()
        Application.EnableVisualStyles()
        Application.SetCompatibleTextRenderingDefault(False)
        Application.Run(New TitleScreen)
    End Sub
End Module