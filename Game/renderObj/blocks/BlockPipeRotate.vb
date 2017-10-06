Public Class BlockPipeRotate
    Implements ISceneAddable

    Private pipeLeft As BlockPipeLeft
    Private pipeRight As BlockPipeRight

    ' how many pixels on each side that is offset
    Public Const PipeBottomOffset = 4

    Public Const Height = GameImage.StandardHeight * 2



    Public Sub New(width As Integer, location As Point, mapScene As MapScene)
        If width < 64 Then
            Throw New Exception("Height must be >= 64 for pipes")
        End If

        ' 32 pixels wide, 64 high
        pipeLeft = New BlockPipeLeft(New Point(location.X, location.Y), mapScene)

        ' width-32 pixels wide, 64 high
        Dim sideSprite = New SpriteSet("pipeRight", New Dictionary(Of SpriteState, List(Of Image)) From {
                                              {SpriteState.ConstantRight, New List(Of Image) From {My.Resources.pipe_right}}
                                              },
                                           width - 32,
                                           BlockPipeRight.Height, Nothing, True
                                           )

        pipeRight = New BlockPipeRight(width - 32, New Point(location.X + 32, location.Y), sideSprite, mapScene)
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
                pipeLeft.SetAction(action)
            Case 5
                ' only map
                If action <> PipeContents.Map Then
                    Throw New Exception(String.Format("invalid length, 5 for array {0}, should contain 'map'", params))
                End If
                ' get map
                dim map = Helper.StrToEnum(Of MapEnum)(params(4))
                pipeLeft.SetMap(map)
            Case 7
                ' has x, y, height ,type, map  and x, y insertion point
                If action <> PipeContents.Map Then
                    Throw New Exception(String.Format("invalid length, 7 for array {0}, should contain 'map'", params))
                End If
                ' get map
                dim map = Helper.StrToEnum(Of MapEnum)(params(4))
                Dim location = New Point(params(5)*GameImage.StandardWidth, params(6)*GameImage.StandardHeight)
                pipeLeft.SetMap(map, location)
        End Select
    End Sub

    Public Sub AddSelfToScene() Implements ISceneAddable.AddSelfToScene
        pipeLeft.AddSelfToScene()
        If pipeRight IsNot Nothing Then
            pipeRight.AddSelfToScene()
        End If

    End Sub

    Protected Friend Class BlockPipeLeft
        Inherits InteractablePipe


        Public Sub New(location As Point, mapScene As MapScene)
            MyBase.New(StandardWidth, StandardHeight*2, location, Sprites.LeftPipeSprite, mapScene)
            ' 64 * 32
        End Sub

        Public Overrides Sub CollisionLeft(sender As Entity)
            If Helper.IsPlayer(sender) And KeyHandler.MoveRight Then
                Dim player As EntPlayer = sender

                If Action = PipeContents.Map And Not player.IsInPipe And Not MyScene.IsTransitioning Then
                    
                    player.EnterHorizontalPipeExitVertical(Me.Map, Me.MapLocation, True)
                End If
            End If
        End Sub
    End Class



    Protected Friend Class BlockPipeRight
        Inherits Block

        Public Const Height = BlockPipe.Width - BlockPipe.PipeBottomOffset * 2

        Public Sub New(width As Integer, location As Point, spriteSet As SpriteSet, mapScene As MapScene)
            MyBase.New(width, Height, location, spriteSet, mapScene)
        End Sub
    End Class

End Class


