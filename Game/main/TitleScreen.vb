Public Class TitleScreen
    

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles HelpButton.Click
        instructions.Show()
        'Dim helpTxt = "Super Mario Bros. HD 720p™ is a faithful recreation of the classic 1985 NES game. In Super Mario Bros., run and jump around the level, collecting coins and squashing bad guys. Your goal - to reach the end of each level." + vbNewLine + vbNewLine + "Controls:" + vbNewLine + vbNewLine + "Move and jump: W A S D" + vbNewLine + "Shoot fireballs: Space" + vbNewLine + "Pause game: Esc"
        'MsgBox(helpTxt, MsgBoxStyle.Information, "Help")
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles LevelSelectButton.Click
        levelSelect.Show()
    End Sub

    Private levelSelect As LevelSelect


    Private Sub TitleScreen_WillClose(sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        If MessageBox.Show("Are you sure you want to quit?", "Confirm exit", MessageBoxButtons.YesNo) <> DialogResult.Yes Then
            e.Cancel = True
        End If
    End Sub
    
    Public Sub StartGame(map As MapEnum)
        Game.StartGame(map)
        Game.ShowDialog()
    End Sub

    Private Sub StartButton_Click(sender As Object, e As EventArgs) Handles StartButton.Click
        StartGame(MapEnum.map1_1above)
    End Sub

    Private instructions as Instruction


    Sub New()
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        game = New GameForm(False)
        instructions = New Instruction()
        
        levelSelect = New LevelSelect(Me)

        Me.Controls.Add(levelSelect)
        Me.Controls.Add(instructions)

        levelSelect.BringToFront()
        levelSelect.Hide()

        instructions.BringToFront()
        instructions.Hide

        HelpButton.Font = buttonFont
        StartButton.Font = buttonFont
        LevelSelectButton.Font = buttonFont

        Randomize()

    End Sub
    Private game As GameForm
    Private buttonFont as New Font(CustomFontFamily.NES, 12)
    
    Private Sub TitleScreen_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        If CheckBox1.checked
            TotalLives = 1
        Else
            TotalLives = 5
        End If
    End Sub

    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click

    End Sub
End Class


Public Module MainProgram
    Public Sub Main()
        Application.EnableVisualStyles()
        Application.SetCompatibleTextRenderingDefault(True)
        Application.Run(New TitleScreen)
    End Sub
End Module

