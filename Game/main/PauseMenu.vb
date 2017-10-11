
Public Class PauseMenu   

    Private buttonFont as New Font(NES.GetFontFamily(), 16, FontStyle.Bold)
    Friend buttons as new List(Of Label)

    Private game as gamecontrol

    Sub New(parent As GameControl)
        selectedButton = ContinueButton
        game = parent

        InitializeComponent()
        AddButtons()
        
       
        For each label in buttons
            label.font = buttonFont
        Next
        PauseLabel.Font = buttonFont
        WorldLabel.Font = buttonFont
        WorldNumLabel.Font = buttonFont
        LivesText.Font = buttonFont

        DoubleBuffered = True

        AlignControls()
        ScaleToParent(parent.Size)
        UpdateTExt()
    End Sub

    Private Sub AddButtons()
        buttons.Add(ContinueButton)
        buttons.Add(ExitButton)
        buttons.Add(ControlButton)
        selectedButton = ContinueButton
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
        sectionHeight = 1/2 * height ' 360
        
        Dim rows as Integer = buttons.Count + 1 ' 4
        Dim rowHeight as integer = sectionHeight / rows ' 90
        Dim rowSpace as Integer = rowHeight / 8 ' 12

        Dim buttonX as Integer = Me.width / 4 ' 320
        Dim bWidth as integer = buttonX * 2 ' 640

        Dim bHeight As Integer = rowHeight * 3 / 4 ' 68
        
        For i=0 to buttons.Count - 1
            dim button = buttons(i)
            dim y = i * rowHeight + rowSpace
            button.Location = New Point(buttonX, y+ sectionOffset)
            button.Width = bWidth
            button.Height = bHeight
        Next
        selectedButton = ContinueButton
        RefreshArrow()

        ' add the mute button
        SpeakerIcon.Location = new Point(0, buttons.Count*rowHeight+sectionOffset) ' 0, 630
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
        Me.Width = pWidth * (1-SideSpace*2) ' 
        Me.Height = pheight *(1-TopSpace*2)
        Me.ResumeLayout()
    End Sub

    Private Sub ContinueButton_Click(sender As Object, e As EventArgs) Handles ContinueButton.Click
        game.HideOverlay()
    End Sub
   
    Public Sub UpdateText
        Me.LivesText.Text = "x " + EntPlayer.Lives.ToString
        if game.GetCurrentScene() isnot nothing
            Me.WorldNumLabel.Text = GetProperMapName(game.GetCurrentScene().MapName)
        End if
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

    Private Sub ControlButton_Click(sender As Object, e As EventArgs) Handles ControlButton.Click
        Dim x as new InstructionForm
        x.ShowDialog()
        
    End Sub
End Class
