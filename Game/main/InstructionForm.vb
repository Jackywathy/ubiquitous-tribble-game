﻿Public Class InstructionForm
    Private Sub InstructionForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Controls.Add(New Instruction(true, me))
    End Sub
End Class
' DOnt use, only testing