Public Class BlockBrickPowerUp
    Inherits Block

    Public type As QuestionBlockReward = -1

    Public Sub New(location As Point, type as string, scene As Scene)
        MyBase.New(32, 32, location, sprites.brickBlock, scene)
        Me.type = Helper.StrToEnum(Of QuestionBlockReward)(type)
    End Sub

    ''' <summary>
    ''' 0 : x
    ''' 1 : y
    ''' 2 : type
    ''' 3 : num of coins (of neccesarry)
    ''' </summary>
    ''' <param name="params"></param>
    ''' <param name="scene"></param>
    Public Sub New(params As Object(), scene As Scene)
        Me.New(New Point(params(0)*32, params(1)*32), params(2), scene)
    End Sub
End Class
