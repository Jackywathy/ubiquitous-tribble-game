Public Class BlockPipe
    Inherits Block
    
    Private pipeTop As BlockPipeTop
    Private pipeBottom As BlockPipeBottom

    Public Sub New(width As Integer, height As Integer, location As Point, scene As Scene)
        MyBase.New(width, height, location, scene)
        If height <= 32
            Throw New Exception("Height must be greater that 32 for pipes")
        End If
        Dim pipeSprite As New SpriteSet(
            New Dictionary(Of SpriteState,List(Of Image)) From {
                { SpriteState.Constant, New List(Of Image) From { My.Resources.pipetop } }
            },
            width,
            32,
        )
        pipeTop = New BlockPipeTop(width, 32, New Point(location.X, location.Y + (height-32)), pipeSprite, scene)
        pipeSprite = New SpriteSet(
            New Dictionary(Of SpriteState,List(Of Image)) From {
                { SpriteState.Constant, New List(Of Image) From { My.Resources.pipebottom } }
            },
            width - width/8,
            height-32,
            )
        pipeBottom = New BlockPipeBottom(width-width/8, height-32, New Point(location.X+width/16, location.Y), pipeSprite, scene)
    End Sub


    Public Overrides Sub AddSelfToScene()
        pipeTop.AddSelfToScene()
        pipeBottom.AddSelfToScene()
    End Sub
End Class

Friend Class BlockPipeTop
    Inherits Block

    Public Sub New(width As Integer, height As Integer, location As Point, spriteSet As SpriteSet, scene As Scene)
        MyBase.New(width, height, location, spriteSet, scene)
    End Sub
End Class



Friend Class BlockPipeBottom
    Inherits Block

    Public Sub New(width As Integer, height As Integer, location As Point, spriteSet As SpriteSet, scene As Scene)
        MyBase.New(width, height, location, spriteSet, scene)
    End Sub
End Class
