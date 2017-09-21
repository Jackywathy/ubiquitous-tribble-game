' Disable the damm DESIGNER!
''' <summary>
''' GameControl, handles updating and rendering the screen
''' </summary>
<ComponentModel.DesignerCategory("")>
Public Class FormBootStrap
    Inherits Form

    Public WithEvents game As New GameControl

    Sub New
        InitalizeComponent()
    End Sub

    Public Sub InitalizeComponent()
        ' default DPI
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch

        Me.ClientSize = New System.Drawing.Size(1264, 681)
        Me.KeyPreview = True

        Me.MaximumSize = New System.Drawing.Size(0, 0)
        Me.MinimumSize = New System.Drawing.Size(1280, 30+720)
        Me.Size = New Size(1280, 30+720)

        Me.Text = "Mario123"

        ' set the gamecontrol
        game.Location = New Point(0,0)
        UpdateGameSize()
        Me.Controls.Add(game)
    End Sub

    ''' <summary>
    ''' Updates the game size, taking into account the size of the X thing in the top
    ''' </summary>
    Private Sub UpdateGameSize()
        If me.FormBorderStyle = FormBorderStyle.None
            game.Height = height
        Else
            game.Height = height-30
        End If
            
        game.Width = width
    End Sub

    Private Sub FormBootStrap_StyleChanged(sender As Object, e As EventArgs) Handles Me.SizeChanged
        UpdateGameSize()
    End Sub
End Class