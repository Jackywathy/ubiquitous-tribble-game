Public Class TitleScreen
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim helpTxt = "Super Mario Bros. HD 720p™ is a faithful recreation of the classic 1985 NES game. Use WASD to move, Space to shoot fireballs, and Esc to pause the game."
        MsgBox(helpTxt, MsgBoxStyle.Information, "Help")
    End Sub

    Private Sub StartButton_Click(sender As Object, e As EventArgs) Handles StartButton.Click
        Game.EnableTimer()

        'Game.game.QueueMapChangeWithStartScene(PlayerStartScreen, Nothing)
        Game.game.QueueMapChangeWithStartScene(MapEnum.None, Nothing)
        Game.ShowDialog()
        
    End Sub

    Sub New
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        game = new GameForm(False)
        
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