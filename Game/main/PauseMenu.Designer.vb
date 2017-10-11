<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class PauseMenu
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(PauseMenu))
        Me.SoundTrackbar = New System.Windows.Forms.TrackBar()
        Me.SpeakerIcon = New System.Windows.Forms.PictureBox()
        Me.ContinueButton = New System.Windows.Forms.Label()
        Me.ExitButton = New System.Windows.Forms.Label()
        Me.ControlButton = New System.Windows.Forms.Label()
        Me.ArrowIcon = New System.Windows.Forms.PictureBox()
        Me.PauseLabel = New System.Windows.Forms.Label()
        Me.WorldLabel = New System.Windows.Forms.Label()
        Me.WorldNumLabel = New System.Windows.Forms.Label()
        CType(Me.SoundTrackbar,System.ComponentModel.ISupportInitialize).BeginInit
        CType(Me.SpeakerIcon,System.ComponentModel.ISupportInitialize).BeginInit
        CType(Me.ArrowIcon,System.ComponentModel.ISupportInitialize).BeginInit
        Me.SuspendLayout
        '
        'SoundTrackbar
        '
        Me.SoundTrackbar.BackColor = System.Drawing.Color.Black
        Me.SoundTrackbar.Location = New System.Drawing.Point(90, 630)
        Me.SoundTrackbar.Name = "SoundTrackbar"
        Me.SoundTrackbar.Size = New System.Drawing.Size(480, 90)
        Me.SoundTrackbar.TabIndex = 0
        '
        'SpeakerIcon
        '
        Me.SpeakerIcon.Image = Global.WinGame.My.Resources.Resources.speaker
        Me.SpeakerIcon.Location = New System.Drawing.Point(0, 630)
        Me.SpeakerIcon.Name = "SpeakerIcon"
        Me.SpeakerIcon.Size = New System.Drawing.Size(90, 90)
        Me.SpeakerIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.SpeakerIcon.TabIndex = 0
        Me.SpeakerIcon.TabStop = false
        '
        'ContinueButton
        '
        Me.ContinueButton.AutoSize = true
        Me.ContinueButton.ForeColor = System.Drawing.Color.White
        Me.ContinueButton.Location = New System.Drawing.Point(320, 372)
        Me.ContinueButton.Name = "ContinueButton"
        Me.ContinueButton.Size = New System.Drawing.Size(640, 68)
        Me.ContinueButton.TabIndex = 0
        Me.ContinueButton.Text = "CONTINUE"
        Me.ContinueButton.UseCompatibleTextRendering = true
        '
        'ExitButton
        '
        Me.ExitButton.AutoSize = true
        Me.ExitButton.ForeColor = System.Drawing.Color.White
        Me.ExitButton.Location = New System.Drawing.Point(320, 462)
        Me.ExitButton.Name = "ExitButton"
        Me.ExitButton.Size = New System.Drawing.Size(640, 68)
        Me.ExitButton.TabIndex = 0
        Me.ExitButton.Text = "EXIT LEVEL"
        Me.ExitButton.UseCompatibleTextRendering = true
        '
        'ControlButton
        '
        Me.ControlButton.AutoSize = true
        Me.ControlButton.ForeColor = System.Drawing.Color.White
        Me.ControlButton.Location = New System.Drawing.Point(320, 552)
        Me.ControlButton.Name = "ControlButton"
        Me.ControlButton.Size = New System.Drawing.Size(640, 68)
        Me.ControlButton.TabIndex = 0
        Me.ControlButton.Text = "CONTROLS/HELP"
        Me.ControlButton.UseCompatibleTextRendering = true
        '
        'ArrowIcon
        '
        Me.ArrowIcon.Image = CType(resources.GetObject("ArrowIcon.Image"),System.Drawing.Image)
        Me.ArrowIcon.Location =  New System.Drawing.Point(220, 372)
        Me.ArrowIcon.Name = "ArrowIcon"
        Me.ArrowIcon.Size = New System.Drawing.Size(68, 68)
        Me.ArrowIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.ArrowIcon.TabIndex = 0
        Me.ArrowIcon.TabStop = false
        '
        'PauseLabel
        '
        Me.PauseLabel.AutoSize = true
        Me.PauseLabel.ForeColor = System.Drawing.Color.White
        Me.PauseLabel.Location = New System.Drawing.Point(320, 23)
        Me.PauseLabel.Name = "PauseLabel"
        Me.PauseLabel.Size = New System.Drawing.Size(960, 135)
        Me.PauseLabel.TabIndex = 0
        Me.PauseLabel.Text = "PAUSE"
        Me.PauseLabel.UseCompatibleTextRendering = true
        '
        'WorldLabel
        '
        Me.WorldLabel.AutoSize = true
        Me.WorldLabel.ForeColor = System.Drawing.Color.White
        Me.WorldLabel.Location = New System.Drawing.Point(320, 180)
        Me.WorldLabel.Name = "WorldLabel"
        Me.WorldLabel.Size = New System.Drawing.Size(960, 45)
        Me.WorldLabel.TabIndex = 0
        Me.WorldLabel.Text = "WORLD"
        Me.WorldLabel.UseCompatibleTextRendering = true
        '
        'WorldNumLabel
        '
        Me.WorldNumLabel.AutoSize = true
        Me.WorldNumLabel.ForeColor = System.Drawing.Color.White
        Me.WorldNumLabel.Location = New System.Drawing.Point(320, 225)
        Me.WorldNumLabel.Name = "WorldNumLabel"
        Me.WorldNumLabel.Size = New System.Drawing.Size(960, 45)
        Me.WorldNumLabel.TabIndex = 0
        Me.WorldNumLabel.Text = "Label1"
        Me.WorldNumLabel.UseCompatibleTextRendering = true
        '
        'PauseMenu
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Black
        Me.Controls.Add(Me.ContinueButton)
        Me.Controls.Add(Me.ExitButton)
        Me.Controls.Add(Me.ControlButton)
        Me.Controls.Add(Me.ArrowIcon)
        Me.Controls.Add(Me.SoundTrackbar)
        Me.Controls.Add(Me.SpeakerIcon)
        Me.Controls.Add(Me.PauseLabel)
        Me.Controls.Add(Me.WorldLabel)
        Me.Controls.Add(Me.WorldNumLabel)
        Me.Name = "PauseMenu"
        Me.Size = New System.Drawing.Size(1280, 720)
        CType(Me.SoundTrackbar,System.ComponentModel.ISupportInitialize).EndInit
        CType(Me.SpeakerIcon,System.ComponentModel.ISupportInitialize).EndInit
        CType(Me.ArrowIcon,System.ComponentModel.ISupportInitialize).EndInit
        Me.ResumeLayout(false)
        Me.PerformLayout

End Sub

    Friend WithEvents ContinueButton As Label
    Friend WithEvents ExitButton As Label
    Friend WithEvents ControlButton As Label
    Friend WithEvents ArrowIcon As PictureBox
    Friend WithEvents SoundTrackbar As TrackBar
    Friend WithEvents SpeakerIcon As PictureBox
    Friend WithEvents PauseLabel As Label
    Friend WithEvents WorldLabel As Label
    Friend WithEvents WorldNumLabel As Label
End Class
