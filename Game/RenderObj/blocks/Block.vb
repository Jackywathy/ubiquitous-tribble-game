Public MustInherit Class Block
    Inherits RenderObject
    Friend Const blockWidth = 32
    Friend Const blockHeight = 32

    Public Overrides Property RenderImage As Image
    Public MustOverride Property spriteSet As SpriteSet

    Public Sub New(width As Integer, height As Integer, location As Point, image As Image)
        MyBase.New(width, height, location)
    End Sub

    ' Just in case
    'RenderImage = New Bitmap(width, height)
    'Using g = Graphics.FromImage(RenderImage)
    '        g.DrawImage(image, 0, 0, width, height)
    'End Using


End Class
