Public Class TestImage
    Sub New(display As Image)
        ' This call is required by the designer.
        InitializeComponent()

        ' AddObj any initialization after the InitializeComponent() call.
        PictureBox1.Image = display
        heightLabel.Text = display.height
        widthLabel.Text = display.width

    End Sub
End Class