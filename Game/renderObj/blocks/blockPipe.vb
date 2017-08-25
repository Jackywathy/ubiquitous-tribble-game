Public Class BlockPipe
    Inherits Block

    Public PossibleActions As New List(Of String) From {"none", "chomp_plant", "fire_plant"}
    Private pipeTop As BlockPipeTop
    Private pipeBottom As BlockPipeBottom

    

    Public Sub New(width As Integer, height As Integer, location As Point, action As String, scene As Scene)
        MyBase.New(width, height, location, scene)
        If height < 64
            Throw New Exception("Height must be >= 64 for pipes")
        End If

        Dim pipeSprite As New SpriteSet(
            New Dictionary(Of SpriteState, List(Of Image)) From {
                {SpriteState.ConstantRight, New List(Of Image) From {My.Resources.pipe_2x}}
            },
            width,
            64,
        )
        pipeTop = New BlockPipeTop(width, 64, New Point(location.X, location.Y + (height - 64)), pipeSprite, scene)
        If height > 64
            pipeSprite = New SpriteSet(
                New Dictionary(Of SpriteState, List(Of Image)) From {
                    {SpriteState.ConstantRight, New List(Of Image) From {My.Resources.pipe_bottom}}
                },
                width,
                height-64
            )
            pipeBottom = New BlockPipeBottom(width, height-64, New Point(location.X, location.Y), action, pipeSprite, scene)
        End if
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
        if pipeBottom isnot nothing
            pipeBottom.AddSelfToScene()
        End if
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
