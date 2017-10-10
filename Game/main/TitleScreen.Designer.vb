<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class TitleScreen
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(TitleScreen))
        Me.HelpButton = New System.Windows.Forms.Button()
        Me.LevelSelectButton = New System.Windows.Forms.Button()
        Me.StartButton = New System.Windows.Forms.Button()
        Me.PictureBox9 = New System.Windows.Forms.PictureBox()
        Me.PictureBox8 = New System.Windows.Forms.PictureBox()
        Me.PictureBox7 = New System.Windows.Forms.PictureBox()
        Me.PictureBox6 = New System.Windows.Forms.PictureBox()
        Me.PictureBox5 = New System.Windows.Forms.PictureBox()
        Me.PictureBox4 = New System.Windows.Forms.PictureBox()
        Me.PictureBox3 = New System.Windows.Forms.PictureBox()
        Me.PictureBox2 = New System.Windows.Forms.PictureBox()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        CType(Me.PictureBox9,System.ComponentModel.ISupportInitialize).BeginInit
        CType(Me.PictureBox8,System.ComponentModel.ISupportInitialize).BeginInit
        CType(Me.PictureBox7,System.ComponentModel.ISupportInitialize).BeginInit
        CType(Me.PictureBox6,System.ComponentModel.ISupportInitialize).BeginInit
        CType(Me.PictureBox5,System.ComponentModel.ISupportInitialize).BeginInit
        CType(Me.PictureBox4,System.ComponentModel.ISupportInitialize).BeginInit
        CType(Me.PictureBox3,System.ComponentModel.ISupportInitialize).BeginInit
        CType(Me.PictureBox2,System.ComponentModel.ISupportInitialize).BeginInit
        CType(Me.PictureBox1,System.ComponentModel.ISupportInitialize).BeginInit
        Me.SuspendLayout
        '
        'HelpButton
        '
        Me.HelpButton.BackColor = System.Drawing.Color.FromArgb(CType(CType(192,Byte),Integer), CType(CType(74,Byte),Integer), CType(CType(0,Byte),Integer))
        Me.HelpButton.Font = New System.Drawing.Font("Microsoft Sans Serif", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.HelpButton.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255,Byte),Integer), CType(CType(224,Byte),Integer), CType(CType(192,Byte),Integer))
        Me.HelpButton.Location = New System.Drawing.Point(403, 314)
        Me.HelpButton.Name = "HelpButton"
        Me.HelpButton.Size = New System.Drawing.Size(197, 57)
        Me.HelpButton.TabIndex = 2
        Me.HelpButton.Text = "Help"
        Me.HelpButton.UseCompatibleTextRendering = true
        Me.HelpButton.UseVisualStyleBackColor = false
        '
        'LevelSelectButton
        '
        Me.LevelSelectButton.BackColor = System.Drawing.Color.FromArgb(CType(CType(192,Byte),Integer), CType(CType(74,Byte),Integer), CType(CType(0,Byte),Integer))
        Me.LevelSelectButton.Font = New System.Drawing.Font("Microsoft Sans Serif", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.LevelSelectButton.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255,Byte),Integer), CType(CType(224,Byte),Integer), CType(CType(192,Byte),Integer))
        Me.LevelSelectButton.Location = New System.Drawing.Point(403, 241)
        Me.LevelSelectButton.Name = "LevelSelectButton"
        Me.LevelSelectButton.Size = New System.Drawing.Size(197, 57)
        Me.LevelSelectButton.TabIndex = 1
        Me.LevelSelectButton.Text = "Level Select"
        Me.LevelSelectButton.UseCompatibleTextRendering = true
        Me.LevelSelectButton.UseVisualStyleBackColor = false
        '
        'StartButton
        '
        Me.StartButton.BackColor = System.Drawing.Color.FromArgb(CType(CType(192,Byte),Integer), CType(CType(74,Byte),Integer), CType(CType(0,Byte),Integer))
        Me.StartButton.Font = New System.Drawing.Font("Microsoft Sans Serif", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.StartButton.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255,Byte),Integer), CType(CType(224,Byte),Integer), CType(CType(192,Byte),Integer))
        Me.StartButton.Location = New System.Drawing.Point(403, 167)
        Me.StartButton.Name = "StartButton"
        Me.StartButton.Size = New System.Drawing.Size(197, 57)
        Me.StartButton.TabIndex = 0
        Me.StartButton.Text = "Start Game"
        Me.StartButton.UseCompatibleTextRendering = true
        Me.StartButton.UseVisualStyleBackColor = false
        '
        'PictureBox9
        '
        Me.PictureBox9.Image = Global.WinGame.My.Resources.Resources.cloud_small
        Me.PictureBox9.Location = New System.Drawing.Point(524, 80)
        Me.PictureBox9.Name = "PictureBox9"
        Me.PictureBox9.Size = New System.Drawing.Size(76, 64)
        Me.PictureBox9.TabIndex = 15
        Me.PictureBox9.TabStop = false
        '
        'PictureBox8
        '
        Me.PictureBox8.Image = Global.WinGame.My.Resources.Resources.cloud_big
        Me.PictureBox8.Location = New System.Drawing.Point(403, 28)
        Me.PictureBox8.Name = "PictureBox8"
        Me.PictureBox8.Size = New System.Drawing.Size(107, 64)
        Me.PictureBox8.TabIndex = 14
        Me.PictureBox8.TabStop = false
        '
        'PictureBox7
        '
        Me.PictureBox7.Image = Global.WinGame.My.Resources.Resources.pipe_top
        Me.PictureBox7.Location = New System.Drawing.Point(293, 337)
        Me.PictureBox7.Name = "PictureBox7"
        Me.PictureBox7.Size = New System.Drawing.Size(71, 32)
        Me.PictureBox7.TabIndex = 13
        Me.PictureBox7.TabStop = false
        '
        'PictureBox6
        '
        Me.PictureBox6.Image = Global.WinGame.My.Resources.Resources.pipe_bottom
        Me.PictureBox6.Location = New System.Drawing.Point(293, 368)
        Me.PictureBox6.Name = "PictureBox6"
        Me.PictureBox6.Size = New System.Drawing.Size(71, 32)
        Me.PictureBox6.TabIndex = 12
        Me.PictureBox6.TabStop = false
        '
        'PictureBox5
        '
        Me.PictureBox5.Image = Global.WinGame.My.Resources.Resources.goomba_1
        Me.PictureBox5.Location = New System.Drawing.Point(199, 368)
        Me.PictureBox5.Name = "PictureBox5"
        Me.PictureBox5.Size = New System.Drawing.Size(32, 32)
        Me.PictureBox5.TabIndex = 11
        Me.PictureBox5.TabStop = false
        '
        'PictureBox4
        '
        Me.PictureBox4.BackgroundImage = Global.WinGame.My.Resources.Resources.blockGround
        Me.PictureBox4.Location = New System.Drawing.Point(-32, 399)
        Me.PictureBox4.Name = "PictureBox4"
        Me.PictureBox4.Size = New System.Drawing.Size(714, 50)
        Me.PictureBox4.TabIndex = 10
        Me.PictureBox4.TabStop = false
        '
        'PictureBox3
        '
        Me.PictureBox3.Image = Global.WinGame.My.Resources.Resources.blockQuestion1
        Me.PictureBox3.Location = New System.Drawing.Point(99, 288)
        Me.PictureBox3.Name = "PictureBox3"
        Me.PictureBox3.Size = New System.Drawing.Size(32, 32)
        Me.PictureBox3.TabIndex = 9
        Me.PictureBox3.TabStop = false
        '
        'PictureBox2
        '
        Me.PictureBox2.Image = Global.WinGame.My.Resources.Resources.mario_big_jump
        Me.PictureBox2.Location = New System.Drawing.Point(82, 323)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(32, 64)
        Me.PictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox2.TabIndex = 8
        Me.PictureBox2.TabStop = false
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = Global.WinGame.My.Resources.Resources.smbTitle
        Me.PictureBox1.Location = New System.Drawing.Point(12, 12)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(362, 256)
        Me.PictureBox1.TabIndex = 0
        Me.PictureBox1.TabStop = false
        '
        'TitleScreen
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.LightSkyBlue
        Me.ClientSize = New System.Drawing.Size(623, 440)
        Me.Controls.Add(Me.PictureBox9)
        Me.Controls.Add(Me.PictureBox8)
        Me.Controls.Add(Me.PictureBox7)
        Me.Controls.Add(Me.PictureBox6)
        Me.Controls.Add(Me.PictureBox5)
        Me.Controls.Add(Me.PictureBox4)
        Me.Controls.Add(Me.PictureBox3)
        Me.Controls.Add(Me.PictureBox2)
        Me.Controls.Add(Me.StartButton)
        Me.Controls.Add(Me.LevelSelectButton)
        Me.Controls.Add(Me.HelpButton)
        Me.Controls.Add(Me.PictureBox1)
        Me.Icon = CType(resources.GetObject("$this.Icon"),System.Drawing.Icon)
        Me.MaximumSize = New System.Drawing.Size(639, 479)
        Me.MinimumSize = New System.Drawing.Size(639, 479)
        Me.Name = "TitleScreen"
        Me.Text = "Super Mario Bros. HD 720p"
        CType(Me.PictureBox9,System.ComponentModel.ISupportInitialize).EndInit
        CType(Me.PictureBox8,System.ComponentModel.ISupportInitialize).EndInit
        CType(Me.PictureBox7,System.ComponentModel.ISupportInitialize).EndInit
        CType(Me.PictureBox6,System.ComponentModel.ISupportInitialize).EndInit
        CType(Me.PictureBox5,System.ComponentModel.ISupportInitialize).EndInit
        CType(Me.PictureBox4,System.ComponentModel.ISupportInitialize).EndInit
        CType(Me.PictureBox3,System.ComponentModel.ISupportInitialize).EndInit
        CType(Me.PictureBox2,System.ComponentModel.ISupportInitialize).EndInit
        CType(Me.PictureBox1,System.ComponentModel.ISupportInitialize).EndInit
        Me.ResumeLayout(false)

End Sub

    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents HelpButton As Button
    Friend WithEvents LevelSelectButton As Button
    Friend WithEvents StartButton As Button
    Friend WithEvents PictureBox2 As PictureBox
    Friend WithEvents PictureBox3 As PictureBox
    Friend WithEvents PictureBox4 As PictureBox
    Friend WithEvents PictureBox5 As PictureBox
    Friend WithEvents PictureBox6 As PictureBox
    Friend WithEvents PictureBox7 As PictureBox
    Friend WithEvents PictureBox8 As PictureBox
    Friend WithEvents PictureBox9 As PictureBox
End Class
