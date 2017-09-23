Public Class BlockInvis1Up
    Inherits BlockInvisNone

    Private used as Boolean = False

    '  Kaiso Block
    ''' <summary>
    ''' 0 : x
    ''' 1 : y
    ''' </summary>
    ''' <param name="params"></param>
    Public Sub New(params As Object(), mapScene As MapScene) 
        MyBase.New(New Point(params(0)*32, params(1)*32), sprites.blockInvisQuestion, mapScene)
    End Sub

    Protected Overrides Sub Activate()
        MyBase.Activate()
        If Not used
            Dim powerup As New EntOneUp(New Point(Me.X, Me.Y+StandardHeight), MyScene)
            powerup.Spawn() 
            used = True
        End if
    End Sub
End Class
