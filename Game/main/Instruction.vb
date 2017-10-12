Public Class Instruction
    Private font as font = New Font(NES, 12)
    private closeForm as boolean = false
    Private shadows parent As Form
    Private Sub Instruction_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Label1.Font = font
        Label2.Font = font
        Label3.Font = font
        Label4.Font = font
        ReturnButton.Font = font
    End Sub
    Sub New(Optional closeForm as Boolean = False, optional parent as Form = Nothing)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.closeForm = closeFOrm
        Me.parent = parent
    End Sub
    Private Sub ReturnButton_Click(sender As Object, e As EventArgs) Handles ReturnButton.Click
        If closeForm
            parent.close()
        End If
        Me.Hide()
    End Sub
End Class
