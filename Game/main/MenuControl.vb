Imports System.ComponentModel

<Designer(getType(MenuControl))>
Public Class MenuControl
    Inherits Control
    Friend WithEvents SoundTrackbar As TrackBar
    Friend WithEvents SpeakerIcon As PictureBox
    Friend WithEvents ContinueButton As Label
    Friend WithEvents ExitButton As Label
    Friend WithEvents ControlButton As Label

    Friend buttons as new List(Of Label)
    Friend WithEvents ArrowIcon As PictureBox
    Friend WithEvents PauseLabel As Label
    Friend WithEvents WorldLabel As Label
    Friend WithEvents WorldNumLabel As Label

    Private buttonFont as New Font(NES.GetFontFamily(), 16, FontStyle.Bold)

    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MenuControl))
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
        Me.SoundTrackbar.Location = New System.Drawing.Point(0, 0)
        Me.SoundTrackbar.Name = "SoundTrackbar"
        Me.SoundTrackbar.Size = New System.Drawing.Size(104, 45)
        Me.SoundTrackbar.TabIndex = 0
        '
        'SpeakerIcon
        '
        Me.SpeakerIcon.Image = Global.WinGame.My.Resources.Resources.speaker
        Me.SpeakerIcon.Location = New System.Drawing.Point(0, 0)
        Me.SpeakerIcon.Name = "SpeakerIcon"
        Me.SpeakerIcon.Size = New System.Drawing.Size(100, 50)
        Me.SpeakerIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.SpeakerIcon.TabIndex = 0
        Me.SpeakerIcon.TabStop = false
        '
        'ContinueButton
        '
        Me.ContinueButton.AutoSize = true
        Me.ContinueButton.ForeColor = System.Drawing.Color.White
        Me.ContinueButton.Location = New System.Drawing.Point(0, 0)
        Me.ContinueButton.Name = "ContinueButton"
        Me.ContinueButton.Size = New System.Drawing.Size(63, 17)
        Me.ContinueButton.TabIndex = 0
        Me.ContinueButton.Text = "CONTINUE"
        Me.ContinueButton.UseCompatibleTextRendering = true
        '
        'ExitButton
        '
        Me.ExitButton.AutoSize = true
        Me.ExitButton.ForeColor = System.Drawing.Color.White
        Me.ExitButton.Location = New System.Drawing.Point(0, 0)
        Me.ExitButton.Name = "ExitButton"
        Me.ExitButton.Size = New System.Drawing.Size(68, 17)
        Me.ExitButton.TabIndex = 0
        Me.ExitButton.Text = "EXIT LEVEL"
        Me.ExitButton.UseCompatibleTextRendering = true
        '
        'ControlButton
        '
        Me.ControlButton.AutoSize = true
        Me.ControlButton.ForeColor = System.Drawing.Color.White
        Me.ControlButton.Location = New System.Drawing.Point(0, 0)
        Me.ControlButton.Name = "ControlButton"
        Me.ControlButton.Size = New System.Drawing.Size(100, 17)
        Me.ControlButton.TabIndex = 0
        Me.ControlButton.Text = "CONTROLS/HELP"
        Me.ControlButton.UseCompatibleTextRendering = true
        '
        'ArrowIcon
        '
        Me.ArrowIcon.Image = CType(resources.GetObject("ArrowIcon.Image"),System.Drawing.Image)
        Me.ArrowIcon.Location = New System.Drawing.Point(0, 0)
        Me.ArrowIcon.Name = "ArrowIcon"
        Me.ArrowIcon.Size = New System.Drawing.Size(32, 32)
        Me.ArrowIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.ArrowIcon.TabIndex = 0
        Me.ArrowIcon.TabStop = false
        '
        'PauseLabel
        '
        Me.PauseLabel.AutoSize = true
        Me.PauseLabel.Location = New System.Drawing.Point(0, 0)
        Me.PauseLabel.Name = "PauseLabel"
        Me.PauseLabel.Size = New System.Drawing.Size(43, 17)
        Me.PauseLabel.TabIndex = 0
        Me.PauseLabel.Text = "PAUSE"
        Me.PauseLabel.UseCompatibleTextRendering = true
        '
        'WorldLabel
        '
        Me.WorldLabel.AutoSize = true
        Me.WorldLabel.Location = New System.Drawing.Point(0, 0)
        Me.WorldLabel.Name = "WorldLabel"
        Me.WorldLabel.Size = New System.Drawing.Size(46, 17)
        Me.WorldLabel.TabIndex = 0
        Me.WorldLabel.Text = "WORLD"
        Me.WorldLabel.UseCompatibleTextRendering = true
        '
        'WorldNumLabel
        '
        Me.WorldNumLabel.AutoSize = true
        Me.WorldNumLabel.Location = New System.Drawing.Point(0, 0)
        Me.WorldNumLabel.Name = "WorldNumLabel"
        Me.WorldNumLabel.Size = New System.Drawing.Size(38, 17)
        Me.WorldNumLabel.TabIndex = 0
        Me.WorldNumLabel.Text = "Label1"
        Me.WorldNumLabel.UseCompatibleTextRendering = true
        '
        'MenuControl
        '
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
        Me.ForeColor = System.Drawing.Color.White
        CType(Me.SoundTrackbar,System.ComponentModel.ISupportInitialize).EndInit
        CType(Me.SpeakerIcon,System.ComponentModel.ISupportInitialize).EndInit
        CType(Me.ArrowIcon,System.ComponentModel.ISupportInitialize).EndInit
        Me.ResumeLayout(false)
        Me.PerformLayout

End Sub

    Private game as gamecontrol

    Sub New(parent As GameControl)
        InitializeComponent()
        AddButtons()
        game = parent
       
        For each label in buttons
            label.font = buttonFont
        Next

        DoubleBuffered = True

        
        WorldNumLabel.Font = buttonFont

    
        selectedButton = ContinueButton
        AlignControls()
        ScaleToParent(parent.Size)
    End Sub

    Private Sub AddButtons()
        buttons.Add(ContinueButton)
        buttons.Add(ExitButton)
        buttons.Add(ControlButton)
    End Sub

    Private Sub RefreshArrow()
        ArrowIcon.Height = selectedButton.Height
        ArrowIcon.Width = selectedButton.Height
        dim buttonPoint = selectedButton.Location
        ArrowIcon.Location = New Point(buttonPoint.X - ArrowIcon.Width * 2, buttonPoint.Y)
    End Sub

    Private selectedButton as Label

    Private Sub AlignControls()
        Dim sectionHeight = 1/4 * height
        Dim sectionOffset = 0

        Dim quartWidth As Integer = Me.Width / 4
        dim threeQuart As Integer= me.Width / 4 * 3
        Dim eightWidth As Integer= me.Width / 8


        PauseLabel.Location = New Point(quartWidth, sectionHeight * 1/8)
        PauseLabel.Width = threeQuart
        PauseLabel.Height = sectionHeight * 3 / 4


        sectionOffset = sectionHeight
        sectionHeight = 1/4 * height

        ' WORLD
        WorldLabel.Location = New Point(quartWidth, sectionOffset)
        WorldLabel.Width = threeQuart
        PauseLabel.Height = sectionHeight / 4 

        ' 1-1
        WorldNumLabel.Location = New Point(quartWidth, sectionOffset+sectionHeight / 4)
        WorldLabel.Width = threeQuart
        PauseLabel.Height = sectionHeight / 4 


        ' bottom 1/2
        sectionOffset += sectionheight
        sectionHeight = 1/2 * height
        
        Dim rows as Integer = buttons.Count + 1
        Dim rowHeight as integer = sectionHeight / rows
        Dim rowSpace as Integer = rowHeight / 8

        Dim buttonX as Integer = Me.width / 4
        Dim bWidth as integer = buttonX * 2

        Dim bHeight As Integer = rowHeight * 3 / 4
        
        For i=0 to buttons.Count - 1
            dim button = buttons(i)
            dim y = i * rowHeight + rowSpace
            button.Location = New Point(buttonX, y+ sectionOffset)
            button.Width = bWidth
            button.Height = bHeight
        Next
        RefreshArrow()

        ' add the mute button
        SpeakerIcon.Location = new Point(0, buttons.Count*rowHeight+sectionOffset)
        SpeakerIcon.Width = bHeight /4*3
        SpeakerIcon.Height = SpeakerIcon.Width

        ' add slider
        SoundTrackbar.Location = new Point(SpeakerIcon.Width, buttons.Count*rowHeight+sectionOffset)
        SoundTrackbar.Width = me.Width / 3*2
        SoundTrackbar.Height = SpeakerIcon.Width

        SoundTrackbar.Value = CInt(game.GetVolumeMultipler() * 10)
    End Sub


    Private Sub Resize_Menucontrol(sender as Object, e As EventArgs) Handles Me.Resize, Me.Move
       AlignControls()
    End Sub

    Friend Sub ScaleToParent(size As Size)
        dim pWidth as integer = size.Width
        dim pHeight as integer = size.height
        Me.SuspendLayout()
        me.Location = new Point(pWidth * SideSpace, pHeight * TopSpace)
        Me.Width = pWidth * (1-SideSpace*2)
        Me.Height = pheight *(1-TopSpace*2)
        Me.ResumeLayout()
    End Sub

    Private Sub ContinueButton_Click(sender As Object, e As EventArgs) Handles ContinueButton.Click
        game.HideOverlay()
    End Sub
   


    Private Sub MouseOverButton(sender as Object, e As EventArgs) Handles ContinueButton.MouseEnter, ControlButton.MouseEnter, ExitButton.MouseEnter
        ' Move icon over
        selectedButton = sender
        RefreshArrow()
    End Sub

    Private Const SideSpace = 0.3
    Private Const TopSpace = 0.2

    Private valueWhenMuted as Integer = -1
    Private Sub SpeakerIcon_Click(sender As Object, e As EventArgs) Handles SpeakerIcon.Click
        If SoundTrackbar.Value = 0
            ' unmute
            if valueWhenMuted <> -1
                SoundTrackbar.Value = valueWhenMuted
                valueWhenMuted = -1
            Else 
                SoundTrackbar.Value = 5
            End If
        Else
            valueWhenMuted = SoundTrackbar.Value
            SoundTrackbar.Value = 0
            
        End If
        Me.Select()
    End Sub

    Private Sub SoundTrackbar_ValueChanged(sender As Object, e As EventArgs) Handles SoundTrackbar.ValueChanged
        If SoundTrackbar.Value = 0
            Mute
        Else
            UnMute
        End If
        Sounds.SetVolume(SoundTrackbar.Value / 10)
        Me.Select()
    End Sub

    Private Sub Mute
        SpeakerIcon.Image = My.Resources.Mute
    End Sub

    Private Sub Unmute
        SpeakerIcon.Image = My.Resources.Speaker
    End Sub

    Private Sub ExitButton_Click(sender As Object, e As EventArgs) Handles ExitButton.Click
        
        game.ReturnToMainMenu()
    End Sub

    Private Sub SoundTrackbar_Scroll(sender As Object, e As EventArgs) Handles SoundTrackbar.Scroll
        Me.Select()
    End Sub
End Class
