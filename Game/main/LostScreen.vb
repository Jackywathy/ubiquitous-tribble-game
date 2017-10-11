Public Class LostScreen
    private font = New Font(Nes, 12)
    Private Sub LostScreen_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Label1.Font = font
        Label2.FOnt = font
        label3.font = font
        label4.font = font
        label5.font = font
        label5.text = "Score " + EntPlayer.Score.tostring
        Me.BringToFront()
    End Sub
End Class