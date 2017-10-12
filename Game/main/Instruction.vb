Public Class Instruction
    Private font as font = New Font(NES, 12)
    Private Sub Instruction_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Label1.Font = font
        Label2.Font = font
        Label3.Font = font
        Label4.Font = font
        ReturnButton.Font = font
    End Sub

    Private Sub ReturnButton_Click(sender As Object, e As EventArgs) Handles ReturnButton.Click
        Me.Hide()
    End Sub
End Class
