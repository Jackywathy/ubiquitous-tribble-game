Public Class TitleScreen
    

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles HelpButton.Click
        Dim helpTxt = "Super Mario Bros. HD 720p™ is a faithful recreation of the classic 1985 NES game. Use WASD to move, Space to shoot fireballs, and Esc to pause the game."
        MsgBox(helpTxt, MsgBoxStyle.Information, "Help")
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles LevelSelectButton.Click
        levelSelect.Show()
    End Sub

    Private levelSelect as New LevelSelect


    Private Sub TitleScreen_WillClose(sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        If MessageBox.Show("Are you sure you want to quit?", "Confirm exit", MessageBoxButtons.YesNo) <> DialogResult.Yes Then
            e.Cancel = True
        End If
    End Sub

    Private Sub StartButton_Click(sender As Object, e As EventArgs) Handles StartButton.Click
        Game.StartGame(MapEnum.map1_1above)
        
        Game.ShowDialog()
        
    End Sub



    Sub New()
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        game = New GameForm(False)
        levelSelect = New LevelSelect(me.Width, me.Height)
        Me.Controls.Add(levelSelect)
        levelSelect.BringToFront()
        levelSelect.Hide()

        HelpButton.Font = buttonFont
        StartButton.Font = buttonFont
        LevelSelectButton.Font = buttonFont

        Randomize()

    End Sub
    Private game As GameForm
    Private buttonFont as New Font(CustomFontFamily.NES, 12)


End Class


Public Module MainProgram
    Public Sub Main()
        Application.EnableVisualStyles()
        Application.SetCompatibleTextRenderingDefault(True)
        Application.Run(New TitleScreen)
    End Sub
End Module

