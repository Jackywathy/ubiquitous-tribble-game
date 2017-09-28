Public Class BlockPipe
    Inherits Block

    Public PossibleActions As New List(Of String) From {"none", "chomp_plant", "fire_plant"}
    Private pipeTop As BlockPipeTop
    Private pipeBottom As BlockPipeBottom

    

    Public Sub New(width As Integer, height As Integer, location As Point, action As String, action2 As string, mapScene As MapScene)
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
        pipeTop = New BlockPipeTop(width, 64, New Point(location.X, location.Y + (height - 64)), pipeSprite, Helper.StrToEnum(Of PipeContents)(action), action2, mapScene)
        If height > 64
            pipeSprite = New SpriteSet("pipeBottom", New Dictionary(Of SpriteState, List(Of Image)) From {
                                          {SpriteState.ConstantRight, New List(Of Image) From {My.Resources.pipe_bottom}}
                                          },
                width,
                height-64, Nothing, True 
            )
            pipeBottom = New BlockPipeBottom(width, height-64, New Point(location.X, location.Y), pipeSprite, mapScene)
        End if
    End Sub

    ''' <summary>
    ''' 0 : x
    ''' 1 : y
    ''' 2 : width
    ''' 3 : height
    ''' 4 : action
    ''' 5 : additional action
    ''' </summary>
    ''' <param name="params"></param>
    ''' <param name="mapScene"></param>
    Public Sub New(params As Object(), mapScene As MapScene)
        Me.New(params(2)*32, params(3)*32, New Point(params(0)*32, params(1)*32), params(4), params(5), mapScene)
    End Sub

    Public Overrides Sub AddSelfToScene()
        pipeTop.AddSelfToScene()
        if pipeBottom isnot nothing
            pipeBottom.AddSelfToScene()
        End if
    End Sub

End Class

Public Enum PipeContents
    None
    Plant
    Map
End Enum

Friend Class BlockPipeTop
    Inherits Block

    private action as PipeContents
    Private map As MapEnum

    Public Sub New(width As Integer, height As Integer, location As Point, spriteSet As SpriteSet, action As PipeContents, action2 as string, mapScene As MapScene)
        MyBase.New(width, height, location, spriteSet, mapScene)
        Me.Action = action
        if action = PipeContents.Map
            map = Helper.StrToEnum(Of MapEnum)(action2)
        End If
    End Sub

    Public Overrides Sub CollisionTop(sender As Entity)
        If action = PipeContents.Map
            if Helper.IsPlayer(sender) And KeyHandler.MoveDown
                Dim player as EntPlayer = sender
                MyScene.Parent.RunScene(map, false)
            End if
        ENd if
    End Sub
End Class



Friend Class BlockPipeBottom
    Inherits Block
    Public Action As string
    Public Sub New(width As Integer, height As Integer, location As Point,  spriteSet As SpriteSet, mapScene As MapScene)
        MyBase.New(width, height, location, spriteSet, mapScene)
    End Sub
End Class
