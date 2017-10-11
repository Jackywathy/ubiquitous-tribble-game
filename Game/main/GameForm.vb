' Disable the damm DESIGNER!
''' <summary>
''' GameControl, handles updating and rendering the screen
''' </summary>
<ComponentModel.DesignerCategory("")>
Public Class GameForm
    Inherits Form

    Public WithEvents game As GameControl

    
    Public Sub EnableTimer
        game.GameLoop.Enabled = true
    End Sub

    Public Sub DisableTimer
        game.GameLoop.Enabled = false
    End Sub

    Sub New(Optional enabled As boolean = True)
        InitalizeComponent(enabled)
    End Sub

    Friend Sub StartGame(map As MapEnum)
        EnableTimer()
        EntPlayer.Coins = 0
        EntPlayer.Score = 0
        game.player1.ResetPlayer()
        Game.QueueMapChangeWithStartScene(map, Nothing)
    End Sub

    Public Sub InitalizeComponent(Optional enabled As Boolean = True)
        ' default DPI
        Me.SuspendLayout()

        Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch

        Me.ClientSize = New System.Drawing.Size(1264, 681)
        Me.KeyPreview = True

        Me.MaximumSize = New System.Drawing.Size(1280, 30+720)
        Me.MinimumSize = New System.Drawing.Size(1280, 30+720)
        Me.Size = New Size(1280, 30+720)

        Me.Text = "Mario123"

        game = New GameControl(enabled)
        ' set the gamecontrol
        game.Location = New Point(0,0)
        UpdateGameSize()
        Me.Controls.Add(game)

        Me.ResumeLayout()
        
    End Sub

    ''' <summary>
    ''' Updates the game size, taking into account the size of the X thing in the top
    ''' </summary>
    Private Sub UpdateGameSize()
        if game isnot nothing
            If me.FormBorderStyle = FormBorderStyle.None
                game.Height = height
            Else
                game.Height = height-30
            End If
            
            game.Width = width
        End if
    End Sub

    Private Sub FormBootStrap_StyleChanged(sender As Object, e As EventArgs) Handles Me.SizeChanged
        UpdateGameSize()
    End Sub

    Private Sub GameForm_Closed(sender As Object, e As EventArgs) Handles Me.Closed
        if musicplayer.BackgroundPlayer IsNot nothing
            MusicPlayer.BackgroundPlayer.Stop()
        End if
        game.Reset()
        
        DisableTimer()
    End Sub
     
    Public AskQuit as Boolean = True

    Private Sub GameForm_willclose(sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        if askQuit
            game.ShowOverlay()
            If MessageBox.Show("Are you sure you want to exit this level?", "Confirm exit", MessageBoxButtons.YesNo) <> DialogResult.Yes Then
                e.Cancel = True
            End If
        End if
    End Sub

End Class