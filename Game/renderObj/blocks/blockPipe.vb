Public Class BlockPipe
    Implements ISceneAddable

    Private pipeTop As BlockPipeTop
    Private pipeBottom As BlockPipeBottom

    ' how many pixels on each side that is offset
    Public Const PipeBottomOffset = 4
    Public Const Width = GameImage.StandardWidth * 2



    Public Sub New(height As Integer, location As Point, mapScene As MapScene)
        If height < 64
            Throw New Exception("Height must be >= 64 for pipes")
        End If


        pipeTop = New BlockPipeTop(New Point(location.X, location.Y + (height - 32)),
                                   mapScene)

        Dim bottomSprite = New SpriteSet("pipeBottom", New Dictionary(Of SpriteState, List(Of Image)) From {
               {SpriteState.ConstantRight, New List(Of Image) From {My.Resources.pipe_bottom}}
               },
             BlockPipeBottom.Width,
             height - 32, Nothing, True
         )
        pipeBottom = New BlockPipeBottom(height - 32, New Point(location.X + PipeBottomOffset, location.Y), bottomSprite, mapScene)
    End Sub

    ''' <summary>
    ''' 0 : x
    ''' 1 : y
    ''' 2 : height
    ''' 3 : type
    ''' 4 : [map, if type = Map]
    ''' 5,  6 : x,y of map if required
    ''' </summary>
    ''' <param name="params"></param>
    ''' <param name="mapScene"></param>
    Public Sub New(params As Object(), mapScene As MapScene)

        Me.New(params(2) * 32, New Point(params(0) * 32, params(1) * 32), mapScene)
        Dim action As PipeContents = Helper.StrToEnum(Of PipeContents)(params(3))
        Select Case params.Length
            Case 4
                ' only x, y ,height and type
                ' dont do anything
                If action = PipeContents.Map
                    Throw New Exception(String.Format("array of length 4, {0} should not be a map", params))
                End If
                pipeTop.SetAction(action)
            Case 5
                ' only map
                If action <> PipeContents.Map Then
                    Throw New Exception(String.Format("invalid length, 5 for array {0}, should contain 'map'", params))
                End If
                ' get map
                Dim map = Helper.StrToEnum(Of MapEnum)(params(4))
                pipeTop.SetMap(map)
            Case 7
                ' has x, y, height ,type, map  and x, y insertion point
                If action <> PipeContents.Map Then
                    Throw New Exception(String.Format("invalid length, 7 for array {0}, should contain 'map'", params))
                End If
                ' get map
                Dim map = Helper.StrToEnum(Of MapEnum)(params(4))
                Dim location = New Point(params(5)*GameImage.StandardWidth, params(6)*GameImage.StandardHeight)
                pipeTop.SetMap(map, location)
        End Select

    End Sub

    Public Sub AddSelfToScene() Implements ISceneAddable.AddSelfToScene
        pipeTop.AddSelfToScene()
        pipeBottom.AddSelfToScene()
    End Sub

    Protected Class BlockPipeTop
        Inherits InteractablePipe
  
        Public Sub New(location As Point, mapScene As MapScene)
            MyBase.New(BlockPipe.Width, StandardHeight, location, Sprites.TopPipeSprite, mapScene)
            ' 64 * 32
        End Sub

        Public Overrides Sub CollisionTop(sender As Entity)

            If Helper.IsPlayer(sender) And KeyHandler.MoveDown Then
                Dim player As EntPlayer = sender

                If Not player.IsInPipe and Action = PipeContents.Map And Not MyScene.IsTransitioning  Then
                    player.EnterVerticalPipe(Me.Map, Me.MapLocation)
                End If
            End If
        End Sub
    End Class


    Protected Class BlockPipeBottom
        Inherits Block

        Public Const Width = BlockPipe.Width - BlockPipe.PipeBottomOffset * 2

        Public Sub New(height As Integer, location As Point, spriteSet As SpriteSet, mapScene As MapScene)
            MyBase.New(BlockPipeBottom.Width, height, location, spriteSet, mapScene)
        End Sub
    End Class

End Class

Public Enum PipeContents
    None
    Plant
    Map
End Enum

Public Class InteractablePipe
    Inherits Block
    Public Property Action As PipeContents = PipeContents.None
    Public Property Map As MapEnum
    Public Property MapLocation As Point?

    Public Sub New(width As Integer, height As Integer, location As Point, spriteSet As SpriteSet, mapScene As MapScene)
        MyBase.New(width, height, location, spriteSet, mapScene)
    End Sub

    Public Sub SetAction(action As PipeContents)
        Me.Action = action
    End Sub

    Public Sub SetMap(map As MapEnum, optional mapLocation As Point?=Nothing)
        Me.Action = PipeContents.Map
        Me.Map = map
        Me.MapLocation = mapLocation
    End Sub

    Public Function GetMapLocation([default] As Point) As Point
        If me.Action <> PipeContents.Map
            Throw New Exception("Must be a pipe with map")
        End If
        If MapLocation IsNot Nothing
            Return Me.MapLocation
        Else 
            Return [default]
        End If
    End Function

End Class

