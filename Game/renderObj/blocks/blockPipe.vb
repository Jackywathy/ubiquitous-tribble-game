Public Class BlockPipe
    Inherits Block

    Public PossibleActions As New List(Of String) From {"none", "chomp_plant", "fire_plant"}
    Private pipeTop As BlockPipeTop
    Private pipeBottom As BlockPipeBottom

    

    Public Sub New(width As Integer, height As Integer, location As Point, action As String, mapScene As MapScene)
        MyBase.New(width, height, location, mapScene)
        If height < 64
            Throw New Exception("Height must be >= 64 for pipes")
        End If

        Dim pipeSprite As New SpriteSet("PipeTop", New Dictionary(Of SpriteState, List(Of Image)) From {
                                           {SpriteState.ConstantRight, New List(Of Image) From {My.Resources.pipe_2x}}
                                           },
            width,
            64,
, True)
        pipeTop = New BlockPipeTop(width, 64, New Point(location.X, location.Y + (height - 64)), pipeSprite, mapScene)
        If height > 64
            pipeSprite = New SpriteSet("pipeBottom", New Dictionary(Of SpriteState, List(Of Image)) From {
                                          {SpriteState.ConstantRight, New List(Of Image) From {My.Resources.pipe_bottom}}
                                          },
                width,
                height-64, Nothing, True 
            )
            pipeBottom = New BlockPipeBottom(width, height-64, New Point(location.X, location.Y), action, pipeSprite, mapScene)
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
    ''' <param name="mapScene"></param>
    Public Sub New(params As Object(), mapScene As MapScene)
        Me.New(params(2)*32, params(3)*32, New Point(params(0)*32, params(1)*32), params(4), mapScene)
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

    Public Sub New(width As Integer, height As Integer, location As Point, spriteSet As SpriteSet, mapScene As MapScene)
        MyBase.New(width, height, location, spriteSet, mapScene)
    End Sub
End Class



Friend Class BlockPipeBottom
    Inherits Block
    Public Action As string
    Public Sub New(width As Integer, height As Integer, location As Point, action As String, spriteSet As SpriteSet, mapScene As MapScene)
        MyBase.New(width, height, location, spriteSet, mapScene)
        Me.Action = action
    End Sub
End Class
