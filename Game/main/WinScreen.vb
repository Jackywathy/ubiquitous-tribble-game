Public Class WinScreen
    private font as New Font(Nes, 12)

    Private Sub WinScreen_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        label1.font = font
        label2.font = font
        label3.font = font
        label4.font = font
        label5.font = font
        label5.text = "Score " + EntPlayer.Score.ToString()
        Me.BringToFront()
    End Sub


End Class