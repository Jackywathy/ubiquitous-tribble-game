<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class LevelSelect
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.MapList = New System.Windows.Forms.ListBox()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.StartButton = New System.Windows.Forms.Button()
        Me.ReturnButton = New System.Windows.Forms.Button()
        CType(Me.PictureBox1,System.ComponentModel.ISupportInitialize).BeginInit
        Me.SuspendLayout
        '
        'MapList
        '
        Me.MapList.BackColor = System.Drawing.SystemColors.HotTrack
        Me.MapList.ForeColor = System.Drawing.Color.LimeGreen
        Me.MapList.FormattingEnabled = true
        Me.MapList.Location = New System.Drawing.Point(395, 12)
        Me.MapList.Name = "MapList"
        Me.MapList.Size = New System.Drawing.Size(222, 277)
        Me.MapList.TabIndex = 0
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = Global.WinGame.My.Resources.Resources.smbTitle
        Me.PictureBox1.Location = New System.Drawing.Point(12, 12)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(362, 256)
        Me.PictureBox1.TabIndex = 1
        Me.PictureBox1.TabStop = false
        '
        'StartButton
        '
        Me.StartButton.BackColor = System.Drawing.Color.FromArgb(CType(CType(192,Byte),Integer), CType(CType(74,Byte),Integer), CType(CType(0,Byte),Integer))
        Me.StartButton.Font = New System.Drawing.Font("Microsoft Sans Serif", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.StartButton.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255,Byte),Integer), CType(CType(224,Byte),Integer), CType(CType(192,Byte),Integer))
        Me.StartButton.Location = New System.Drawing.Point(411, 295)
        Me.StartButton.Name = "StartButton"
        Me.StartButton.Size = New System.Drawing.Size(197, 57)
        Me.StartButton.TabIndex = 2
        Me.StartButton.Text = "Start Game"
        Me.StartButton.UseVisualStyleBackColor = false
        '
        'ReturnButton
        '
        Me.ReturnButton.BackColor = System.Drawing.Color.FromArgb(CType(CType(192,Byte),Integer), CType(CType(74,Byte),Integer), CType(CType(0,Byte),Integer))
        Me.ReturnButton.Font = New System.Drawing.Font("Microsoft Sans Serif", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.ReturnButton.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255,Byte),Integer), CType(CType(224,Byte),Integer), CType(CType(192,Byte),Integer))
        Me.ReturnButton.Location = New System.Drawing.Point(411, 369)
        Me.ReturnButton.Name = "ReturnButton"
        Me.ReturnButton.Size = New System.Drawing.Size(197, 57)
        Me.ReturnButton.TabIndex = 3
        Me.ReturnButton.Text = "Return"
        Me.ReturnButton.UseVisualStyleBackColor = false
        '
        'LevelSelect
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.LightSkyBlue
        Me.Controls.Add(Me.ReturnButton)
        Me.Controls.Add(Me.StartButton)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.MapList)
        Me.Name = "LevelSelect"
        Me.Size = New System.Drawing.Size(639, 450)
        CType(Me.PictureBox1,System.ComponentModel.ISupportInitialize).EndInit
        Me.ResumeLayout(false)

End Sub

    Friend WithEvents MapList As ListBox
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents StartButton As Button
    Friend WithEvents ReturnButton As Button
End Class
