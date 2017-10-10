Public Class BlockBrickCoin
    Inherits BlockBumpable

    ''' <summary> 
    ''' Set when block is first hit 
    ''' </summary> 
    Private framesPastSinceFirstHit As Integer = -1 

    ''' <summary>
    ''' Set after first time player hits the block
    ''' </summary>
    Private hasBeenHit As Boolean = False 

    ''' <summary>
    ''' Timer ran out - next hit will set isUsed to True and change the image
    ''' </summary>
    Private timerRanOut As Boolean = False

    ''' <summary>
    ''' Set after the block has been used (timer ran out)
    ''' </summary>
    Private isUsed As Boolean = False 
 
    ''' <summary>
    ''' The total time before the block gets used
    ''' </summary>
    Private ReadOnly amountOfTimeYouHaveToGetCoins As Integer = Helper.Random(3*60, 4*60)
    

    Public Sub New(location As Point, theme as RenderTheme, mapScene As MapScene)
        MyBase.New(StandardWidth, StandardHeight, location, BlockBreakableBrick.GetSpriteSet(theme), mapScene)
    End Sub



    ''' <summary>
    ''' 0 : x
    ''' 1 : y
    ''' </summary>
    ''' <param name="params"></param>
    ''' <param name="mapScene"></param>
    Public Sub New(params As Object(), theme as RenderTheme,  mapScene As MapScene)
        Me.New(New Point(params(0)*StandardWidth, params(1)*StandardHeight), theme, mapScene)
    End Sub
    
    Public Overrides Sub UpdateVeloc()
        MyBase.UpdateVeloc()

        If HasBeenHit Then 
            framesPastSinceFirstHit += 1
            If Not timerRanOut And framesPastSinceFirstHit >= AmountOfTimeYouHaveToGetCoins Then 
                timerRanOut = True
            End If 
        End If 
 
        
    End Sub

    Public Overrides Sub Animate
        If IsUsed
            Me.RenderImage = spriteSet.GetFirst(SpriteState.Destroy)
        End If
    End Sub




    Public Overrides Sub CollisionBottom(sender As Entity)
        If Not Me.isMoving And Not isUsed And Helper.IsPlayer(sender) Then
            Dim player As EntPlayer = sender
            player.PickupCoin()
            Dim coin = New EntCoinFromBlock(New Point(Me.Location.X, Me.Location.Y + Me.Height), MyScene)
            coin.Spawn()

            If Not Me.HasBeenHit Then
                HasBeenHit = True
                framesPastSinceFirstHit = 0
            End If


            If timerRanOut Then
                isUsed = True
            Else
                StartBump()
            End If
                
        End If

        

    End Sub
   
End Class
