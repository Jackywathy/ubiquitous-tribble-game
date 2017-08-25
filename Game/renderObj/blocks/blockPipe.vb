Public Class BlockPipe
    Inherits Block

    Public PossibleActions As New List(Of String) From {"none", "chomp_plant", "fire_plant"}
    Private pipeTop As BlockPipeTop
    Private pipeBottom As BlockPipeBottom

    

    Public Sub New(width As Integer, height As Integer, location As Point, action As String, scene As Scene)
        MyBase.New(width, height, location, scene)
        If height <= 32
            Throw New Exception("Height must be greater that 32 for pipes")
        End If
        Dim pipeSprite As New SpriteSet(
            New Dictionary(Of SpriteState, List(Of Image)) From {
                {SpriteState.Constant, New List(Of Image) From {My.Resources.pipe_2x}}
            },
            width,
            64,
)
        pipeTop = New BlockPipeTop(width, 64, New Point(location.X, location.Y + (height - 32)), pipeSprite, scene)
        pipeSprite = New SpriteSet(
            New Dictionary(Of SpriteState, List(Of Image)) From {
                {SpriteState.Constant, New List(Of Image) From {My.Resources.pipe_bottom}}
            },
            width - width / 8,
            height - 32,
)
        pipeBottom = New BlockPipeBottom(width-width/8, height-32, New Point(location.X+width/16, location.Y), action, pipeSprite, scene)
    End Sub

    ''' <summary>
    ''' 0 : x
    ''' 1 : y
    ''' 2 : width
    ''' 3 : height
    ''' 4 : action
    ''' </summary>
    ''' <param name="params"></param>
    ''' <param name="scene"></param>
    Public Sub New(params As Object(), scene As Scene)
        Me.New(params(2)*32, params(3)*32, New Point(params(0)*32, params(1)*32), params(4), scene)
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
    Public Action As string
    Public Sub New(width As Integer, height As Integer, location As Point, action As String, spriteSet As SpriteSet, scene As Scene)
        MyBase.New(width, height, location, spriteSet, scene)
        Me.Action = action
    End Sub
End Class
