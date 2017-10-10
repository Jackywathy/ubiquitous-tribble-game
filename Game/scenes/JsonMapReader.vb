Imports Newtonsoft.Json

Public NotInheritable Class JsonMapReader
    ''' <summary>
    ''' Creates a new <see cref="MapScene">object, from a json file in resources</see>
    ''' </summary>
    ''' <param name="jsonName"></param>
    ''' <returns></returns>
    Public Shared Function ReadMapFromResource(jsonName As String, map As MapEnum, parent As Control) As MapScene
        Dim byteArray = CType(My.Resources.ResourceManager.GetObject(jsonName), Byte())

        if byteArray Is Nothing
            Throw New Exception(String.Format("Cannot find json, {0}", jsonName))
        End If

        If byteArray(0) = 239 And byteArray(1) = 187 And byteArray(2) = 191 Then
            byteArray = byteArray.Skip(3).Take(byteArray.Length - 2).ToArray()
        End If
        Dim str = Text.Encoding.UTF8.GetString(byteArray)

        if str is Nothing or str.Length = 0
            Throw New Exception(String.Format("File is empty: , {0}", jsonName))
        End If

        Dim mapObject As JsonMapObject
        Try
             mapObject = JsonConvert.DeserializeObject(Of JsonMapObject)(str)
        Catch e As JsonException
            Throw 
            Throw New Exception(String.Format("Json invalid, name={0}", jsonname), e)
        End Try

        Dim outScene As New MapScene(parent, mapObject.width, mapObject.Height, 
                                     map, 
                                     mapObject.BackgroundColor, mapObject.MapTime,
                                     )


        ' add the BackgroundColor

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

        Dim defaultEntry As New Point(mapObject.DefaultEntry(0), mapObject.DefaultEntry(1))
        outScene.DefaultPlayerLocation = defaultEntry
        outScene.NextLevel = mapObject.nextLevel
        ' add all Entities
        'Dim player1 = New EntPlayer(32, 32, defaultEntry, outScene)

        
        'outScene.SetPlayer(MapScene.PlayerId.Player1, player1)
        'player1.AddSelfToScene()

#If DEBUG
        DebugJsonMap(outScene)
#End If

        Select Case mapObject.Theme
            Case RenderTheme.Overworld
                outScene.BackgroundMusic = BackgroundMusic.GroundTheme
                GenerateOverworldDecoration(outScene)
            Case RenderTheme.Underground
                outScene.BackgroundMusic = BackgroundMusic.UndergroundTheme
            Case Else
                Throw New Exception()
        End Select
        
       
        Return outScene

    End Function

    Public Shared Sub GenerateOverworldDecoration(outScene as MapScene)
        ' generate 1 cloud per 10 blocks
        ' start 1/3 down
        Dim nBlocks as integer = outScene.width / 32 

        Dim rLength = outScene.width / 10
        dim nClouds = nBlocks / 10

        For i = 0 To nClouds
            Dim start = rLength * i
            Dim _end = rLength * (i + 1)
            Dim decoration = StaticDecoration.GetRandomCloud(New Point(Helper.Random(start, _end), 
                                                                       Helper.Random(ScreenGridHeight / 3 * 2, ScreenGridHeight / 4 * 3)), outScene)
            decoration.AddSelfToScene()
        Next

        For Each item In outScene.GetAllGround()
            ' generate 1 per 8-12 blocks
            Dim nBush as Integer = (item.Width / 32) / Helper.Random(15, 20)
            if nBush > 0
                Dim sectionWidth as integer = item.Width / nBush
                For i = 0 to nBush-1 step 1
                    Dim x = i * sectionWidth + Helper.Random(0, sectionWidth-StaticDecoration.HillBig.Width) + item.X
                    Dim decoration = StaticDecoration.GetRandomBrush(New Point(x, item.Y + item.Height), outScene)
                    decoration.AddSelfToscene()
                Next
            End if
        Next


    End Sub


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
        BlockPipeRotate

        BlockCloud



        EntGoomba
        EntKoopa
        EntCoin

        Flag
    End Enum


    Public Shared Function RenderItemFactory(name As String, params As Object(), theme As RenderTheme, scene As MapScene) As ISceneAddable
        Dim out As ISceneAddable 
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
                out = New BlockMetal(params, theme, scene)

            Case RenderTypes.BlockPipe
                AssertLength("blockPipe", New Integer() {4, 5, 7}, params)
                out = New BlockPipe(params, theme, scene)

            Case RenderTypes.BlockPipeRotate
                AssertLength("blockpiperotate", New Integer() {4, 5, 7}, params)
                out = New BlockPipeRotate(params, theme, scene)

            Case RenderTypes.BlockBrickCoin
                AssertLength("blockBrickCoin", 2, params)
                out = New BlockBrickCoin(params, theme, scene)

            Case RenderTypes.BlockBrickStar
                AssertLength("blockBrickStar", 2, params)
                out = New BlockBrickStar(params, theme, scene)

            Case RenderTypes.BlockBrickPowerUp
                AssertLength("blockBrickPowerUp", 2, params)
                out = New BlockBrickPowerUp(params, theme, scene)

            Case RenderTypes.BlockCloud
                AssertLength("blockCloud", 2, params)
                out = New BlockCloud(params, theme, scene)

            Case RenderTypes.EntGoomba
                AssertLength("entGoomba", 2, params)
                out = New EntGoomba(params, theme, scene)

            Case RenderTypes.EntKoopa
                AssertLength("entKoopa", 2, params)
                out = New EntKoopa(params,theme,  scene)

            Case RenderTypes.Flag
                AssertLength("flag", 2, params)
                out = New Flag(params,theme, scene)

            Case RenderTypes.BlockInvis1Up
                AssertLength("blockInvis1Up", 2, params)
                out = New BlockInvis1Up(params, theme,scene)

            Case RenderTypes.BlockInvisNone
                AssertLength("blockInvisNone", 2, params)
                out = New BlockInvisNone(params, theme,scene)

            Case RenderTypes.BlockInvisCoin
                AssertLength("BlockInvisCoin", 2, params)
                out = New BlockInvisCoin(params,theme, scene)
            

            Case RenderTypes.EntCoin
                AssertLength("EntCoin", 2, params)
                out = New EntCoin(params, theme, scene)
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
                                                                       type, array.Length, String.Join(", ", array)))
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
    Public Property BackgroundColor As String
    Public Property Theme As RenderTheme
    Public Property Width as integer
    Public Property Height As integer
    Public Property MapTime As Integer
    Public Property DefaultEntry As Integer()
    Public Property NextLevel As MapEnum
End Class