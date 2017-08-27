Public Class BlockBrickPowerUp
    Inherits Block

    Public type As QuestionBlockReward = -1

    Public Sub New(location As Point, type as string, mapScene As MapScene)
        MyBase.New(32, 32, location, sprites.brickBlock, mapScene)
        Me.type = Helper.StrToEnum(Of QuestionBlockReward)(type)
    End Sub

    ''' <summary>
    ''' 0 : x
    ''' 1 : y
    ''' 2 : type
    ''' 3 : num of coins (of neccesarry)
    ''' </summary>
    ''' <param name="params"></param>
    ''' <param name="mapScene"></param>
    Public Sub New(params As Object(), mapScene As MapScene)
        Me.New(New Point(params(0)*32, params(1)*32), params(2), mapScene)
    End Sub
End Class
