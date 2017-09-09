
Public Class BlockMultipleCoins
    Inherits BlockBumpable

    ''' <summary> 
    ''' Set when block is first hit 
    ''' </summary> 
    Private FramesPassedSinceHit As Integer = -1 
    Private ShouldIncrementTimer As Boolean = False 
 
    Private TimerRanOut As Boolean = False 
 
    Private Const AmountOfTimeYouHaveToGetCoins As Integer = 60 * 3 
    Public isUsed = False 

    Public Sub New(location As Point, mapScene As MapScene)
        MyBase.New(32, 32, location, Sprites.brickBlock, mapScene)
    End Sub
    
    Public Overrides Sub UpdateItem()
        MyBase.UpdateItem()
        If ShouldIncrementTimer Then 
            FramesPassedSinceHit += 1 
        End If 
 
        If Not TimerRanOut And FramesPassedSinceHit >= AmountOfTimeYouHaveToGetCoins Then 
            ShouldIncrementTimer = False 
            TimerRanOut = True 
        End If 


    End Sub




    Public Overrides Sub CollisionBottom(sender As Entity)
        If Not Me.isMoving And Not Me.isUsed And Helper.IsPlayer(sender) Then
            Dim player As EntPlayer = sender
            player.PickupCoin()
            Dim coin = New EntCoinFromBlock(32, 32, New Point(Me.Location.X, Me.Location.Y + Me.Height), MyScene)
            coin.Spawn()

            If Not Me.ShouldIncrementTimer Then
                ShouldIncrementTimer = True
                FramesPassedSinceHit = 0
            End If
            If Me.TimerRanOut Then
                Me.isUsed = True
            End If
            StartBump()
        End If

    End Sub
    Private Sub HitBlock

    End Sub
End Class
