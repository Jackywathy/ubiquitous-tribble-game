Public Class BlockInvis
    Inherits Block

    Private revealed As Boolean = False

    Public Sub New(width As Integer, height As Integer, location As Point, spriteSet As SpriteSet, mapScene As MapScene)
        MyBase.New(width, height, location, spriteSet, mapScene)
    End Sub
    '  Kaiso Block
    ''' <summary>
    ''' 0 : x
    ''' 1 : y
    ''' item : 'coin', '1up'
    ''' </summary>
    ''' <param name="params"></param>
    Public Sub New(params As Object(), mapScene As MapScene) 
        Me.New(32, 32, New Point(params(0)*32, params(1)*32), sprites.blockInvis, mapScene)
    End SUb
End Class
