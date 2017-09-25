Imports Newtonsoft.Json

Public NotInheritable Class JsonMapReader
    ''' <summary>
    ''' Creates a new <see cref="MapScene">object, from a json file in resources</see>
    ''' </summary>
    ''' <param name="jsonName"></param>
    ''' <returns></returns>
    Public Shared Function ReadMapFromResource(jsonName As String, parent As Control) As MapScene
        Dim byteArray = CType(My.Resources.ResourceManager.GetObject(jsonName), Byte())
        If byteArray(0) = 239 And byteArray(1) = 187 And byteArray(2) = 191 Then
            byteArray = byteArray.Skip(3).Take(byteArray.Length - 2).ToArray()
        End If
        Dim str = Text.Encoding.UTF8.GetString(byteArray)


        Dim mapObject = JsonConvert.DeserializeObject(Of JsonMapObject)(str)

        Dim outScene As New MapScene(parent)

        ' add the Background
        outScene.SetBackground(mapObject.Background(0), mapObject.Background(1), mapObject.Background(2))

        ' add all blocks
        For Each pair As KeyValuePair(Of String, IList(Of Object())) In mapObject.Blocks
            Dim name = pair.Key
            For Each params As Object() In pair.Value
                RenderItemFactory(name, params, mapObject.Theme, outScene).AddSelfToScene()
            Next
        Next

        For Each pair As KeyValuePair(Of String, IList(Of Object())) In mapObject.Entities
            Dim name = pair.Key
            For Each params As Object() In pair.Value
                RenderItemFactory(name, params, mapObject.Theme, outScene).AddSelfToScene()
            Next
        Next


        ' add all Entities
        Dim player1 = New EntPlayer(32, 32, New Point(0, GroundHeight), outScene)

        outScene.SetPlayer(MapScene.PlayerId.Player1, player1)
        outScene.AddEntity(player1)

        outScene.AddStatic(New StaticText(New Rectangle(0, Helper.TopToBottom(0, ScreenGridHeight / 16), ScreenGridWidth / 4, ScreenGridHeight / 16), "MARIO", NES.GetFontFamily(), 18,
                                          New SolidBrush(Color.White), outScene))

        Dim scoreText = New StaticText(New Rectangle(0, Helper.TopToBottom(ScreenGridHeight / 16, ScreenGridHeight / 16), ScreenGridWidth / 4, ScreenGridHeight / 16), "000000", NES.GetFontFamily(), 18,
                                       New SolidBrush(Color.White), outScene, paddingChar:="0", paddingWidth:=6)
        outScene.AddStatic(scoreText)
        EntPlayer.ScoreCallback = scoreText


#If DEBUG
        DebugMapHook(outScene)
#End If

        'TODO REPLACE with actual backgroundMusic reader
        outScene.BackgroundMusic = BackgroundMusic.GroundTheme
        outScene.BackgroundMusic.Play()
        Return outScene

    End Function


    Public Class InvalidJsonException
        Inherits Exception
        Sub New(message As String)
            MyBase.New(message)
        End Sub

    End Class

    ''' <summary>
    ''' To add a new block:
    ''' add it to RenderTypes
    ''' put a case in RenderItemFactory
    ''' </summary>
    Public Enum RenderTypes

        GroundPlatform

        BlockBreakableBrick
        BlockBrickCoin
        BlockBrickPowerUp
        BlockBrickStar

        BlockInvis1Up
        BlockInvisNone
        BlockInvisCoin

        BlockQuestion
        BlockMushroom

        BlockMetal

        BlockPipe

        BlockCloud



        EntGoomba
        EntKoopa
        EntCoin

        Flag
    End Enum


    Public Shared Function RenderItemFactory(name As String, params As Object(), theme As RenderTheme, scene As MapScene) As HitboxItem
        Dim out As HitboxItem '
        Dim item As RenderTypes
        Try
            item = Helper.StrToEnum(Of RenderTypes)(name)
        Catch exception As Exception
            Throw New Exception(String.Format("Cannot find item: {0} in the RenderTypes Enum", name), exception)
        End Try
        Select Case item
            Case RenderTypes.BlockBreakableBrick
                AssertLength(name, 2, params)
                out = New BlockBreakableBrick(params, theme, scene)

            Case RenderTypes.GroundPlatform
                AssertLength(name, 4, params)
                out = New GroundPlatform(params, theme, scene)

            Case RenderTypes.BlockQuestion
                AssertLength(name, 3, params)
                out = New BlockQuestion(params, theme, scene)

            Case RenderTypes.BlockMetal
                AssertLength("blockMetal", 2, params)
                out = New BlockMetal(params, scene)

            Case RenderTypes.BlockPipe
                AssertLength("blockPipe", 5, params)
                out = New BlockPipe(params, scene)

            Case RenderTypes.BlockBrickCoin
                AssertLength("blockBrickCoin", 2, params)
                out = New BlockBrickCoin(params, scene)

            Case RenderTypes.BlockBrickStar
                AssertLength("blockBrickStar", 2, params)
                out = New BlockBrickStar(params, scene)

            Case RenderTypes.BlockBrickPowerUp
                AssertLength("blockBrickPowerUp", 2, params)
                out = New BlockBrickPowerUp(params, scene)

            Case RenderTypes.BlockCloud
                AssertLength("blockCloud", 2, params)
                out = New BlockCloud(params, scene)

            Case RenderTypes.EntGoomba
                AssertLength("entGoomba", 2, params)
                out = New EntGoomba(params, scene)

            Case RenderTypes.EntKoopa
                AssertLength("entKoopa", 2, params)
                out = New EntKoopa(params, scene)

            Case RenderTypes.Flag
                AssertLength("flag", 2, params)
                out = New Flag(params, scene)

            Case RenderTypes.BlockInvis1Up
                AssertLength("blockInvis1Up", 2, params)
                out = New BlockInvis1Up(params, scene)

            Case RenderTypes.BlockInvisNone
                AssertLength("blockInvisNone", 2, params)
                out = New BlockInvisNone(params, scene)

            Case RenderTypes.BlockInvisCoin
                AssertLength("BlockInvisCoin", 2, params)
                out = New BlockInvisCoin(params, scene)

            Case Else

                Throw New Exception(String.Format("No object with name {0}", name))
        End Select

        Return out

    End Function

    Private Shared Sub AssertLength(type As String, expected As Integer, array As Object())
        If expected = -1
            Return
        End If

        If expected <> array.Length Then
            Throw New JsonMapReader.InvalidJsonException(String.Format("Error in JSON, powerup={0}, given {1} elements when {2} expected - [ {3} ] ",
                                                                       type, array.Length, expected, String.Join(", ", array)))
        End If
    End Sub

    Private Shared Sub AssertLength(type As String, expected As Integer(), array As Object())

        If Not expected.Contains(array.Length) Then
            Throw New JsonMapReader.InvalidJsonException(String.Format("Error in JSON, powerup={0}, given {1} elements when {2} expected - [ {3} ] ",
                                                                       type, array.Length, expected, String.Join(", ", array)))
        End If
    End Sub

    ''' <summary>
    ''' Dont let this class be instantialised
    ''' </summary>
    Private Sub New
    End Sub
End Class

<JsonObject(ItemRequired:=Required.Always)>
Public Class JsonMapObject
    Public Property MapName As String
    Public Property Blocks As Dictionary(Of String, IList(Of Object()))
    Public Property Entities As Dictionary(Of String, IList(Of Object()))
    Public Property Background As List(Of Object)
    Public Property Theme As RenderTheme
End Class