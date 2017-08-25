Public Class BlockInvis
    Inherits Block

    Private revealed As Boolean = False

    Public Sub New(width As Integer, height As Integer, location As Point, spriteSet As SpriteSet, scene As Scene)
        MyBase.New(width, height, location, spriteSet, scene)
    End Sub
    '  Kaiso Block
    ''' <summary>
    ''' 0 : x
    ''' 1 : y
    ''' item : 'coin', '1up'
    ''' </summary>
    ''' <param name="params"></param>
    Public Sub New(params As Object(), scene As Scene) 
        Me.New(32, 32, New Point(params(0)*32, params(1)*32), sprites.blockInvis, scene)
    End SUb
End Class
