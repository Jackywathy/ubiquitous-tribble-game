Public Class StaticHud
    Inherits GameItem

    Private height as Integer
    Private width as Integer


    Public Sub SetTime(seconds As Integer)
       TimeNumText.Text = seconds.ToString
    End Sub


    ' Takes up 25% of screen width
    ' this is the start point (in percent)
    Private Const marioScorePercent = 0.25
    Private Const coinsPercent = 0.20
    Private Const powerupPercent = 0.15
    Private Const worldPercent = 0.2
    Private Const timePercent = 0.2

    Private Const marioLocation = 0
    Private Const coinsLocation = marioLocation + marioScorePercent
    Private Const powerupLocation = coinsLocation + coinsPercent
    Private Const worldLocation = powerupLocation + powerupPercent
    Private Const timeLocation = worldLocation + worldPercent

    Private ReadOnly MarioText As StaticText
    Private ReadOnly ScoreText As StaticText

    Private ReadOnly CoinImage As StaticCoin
    Private ReadOnly CrossImage As StaticCross
    Private ReadOnly CoinText As StaticText

    Public ReadOnly PowerupHolder As StaticHudPowerup

    Private ReadOnly WorldText As StaticText
    Private ReadOnly WorldNumText As StaticText

    Private ReadOnly TimeText As StaticText
    Private ReadOnly TimeNumText As StaticText

    Private fontSize as integer = 18

    Private Sub Reshuffle()

    End Sub

    ' pixel offset from the corner for mario
    Private const MarioOffset = 2

    Public Sub New(width As Integer, height As Integer)
        me.height = height
        Me.width = width
        ' Mario text and score
        Dim halfWayHeight As Integer = height / 2
        ' Rectangle that MARIO is drawn in
        Dim marioTextRect As New Rectangle(marioLocation+MarioOffset, Helper.TopToBottom(0, halfWayHeight)-marioOffset, ' X, Y
                                           width * marioScorePercent-MarioOffset, halfWayHeight-MarioOffset ' width, height
                                           )

        ' Rectangle that 0000 score is drawn in
        Dim scoreTextRect As New Rectangle(marioLocation+MarioOffset, Helper.TopToBottom(halfWayHeight, halfWayHeight)-MarioOffset, ' X, Y
                                           width * marioScorePercent-MarioOffset, halfWayHeight-MarioOffset
                                           )

        marioText = New StaticText(marioTextRect, "MARIO", NES.GetFontFamily(), fontSize, DrawingPrimitives.WhiteBrush)
        ScoreText = New StaticText(scoreTextRect, "000000", NES.GetFontFamily(), fontSize, DrawingPrimitives.WhiteBrush, 
                                       paddingChar:="0", paddingWidth:=6)

        EntPlayer.ScoreCallback = scoreText

        ' Coin 
        ' 0 x 56
        ' each letter/symbol is 5% of the screen wide
        Dim coinLocation = New Point(coinsLocation*width, Helper.TopToBottom(halfWayHeight, halfWayHeight))
        CoinImage = New StaticCoin(width*0.05, halfWayHeight, coinLocation)

        Dim xLocation = New Point((coinsLocation+0.05)*width, Helper.TopToBottom(halfWayHeight, halfWayHeight))
        CrossImage = New StaticCross(width*0.05, halfWayHeight, xLocation)

        ' 2 characters, 5% each
        Dim coinTextRect As New Rectangle((coinsLocation+0.1)*width, Helper.TopToBottom(halfWayHeight, halfWayHeight), ' X, Y
                                          width * 0.1, halfWayHeight ' width ,height
                                          )

        CoinText = New StaticText(coinTextRect, "0",
                                  CustomFontFamily.NES.GetFontFamily(), fontSize, DrawingPrimitives.WhiteBrush,
                                  horAlignment := StringAlignment.Center, vertAlignment := StringAlignment.Center
                                  )

        EntPlayer.CoinCallback = CoinText

        ' Powerup box
        Dim powerupPoint = New Point(powerupLocation*width, Helper.TopToBottom(0, height))
        Dim widthOrHeight As Integer = Math.Min(width*powerupPercent, height)
        PowerupHolder = New StaticHudPowerup(widthOrHeight, widthOrHeight, powerupPoint)
        PowerupHolder.SetPowerupItem(PowerupType.Mushroom)

        ' World 
        Dim worldTextRect = New Rectangle(worldLocation*width, Helper.TopToBottom(0, halfWayHeight),
                                      width*worldPercent, halfWayHeight)

        Dim worldNumRect = New Rectangle(worldLocation*width, Helper.TopToBottom(halfWayHeight, halfWayHeight),
                                      width*worldPercent, halfWayHeight)
        worldtext = New StaticText(worldTextRect, "WORLD", 
                                   CustomFontFamily.NES.GetFontFamily(), fontSize, DrawingPrimitives.WhiteBrush,
                                   horAlignment := StringAlignment.Center, vertAlignment := StringAlignment.Center
                                   )

        WorldNumText = New StaticText(worldNumRect, "1-1", 
                                      CustomFontFamily.NES.GetFontFamily(), fontSize, DrawingPrimitives.WhiteBrush,
                                      horAlignment := StringAlignment.Center, vertAlignment := StringAlignment.Center
                                      )
        ' Time
        Dim timeTextRect = New Rectangle(timeLocation*width, Helper.TopToBottom(0, halfWayHeight),
                                          width*timePercent, halfWayHeight)

        Dim timeNumRect = New Rectangle(timeLocation*width, Helper.TopToBottom(halfWayHeight, halfWayHeight),
                                         width*timePercent, halfWayHeight)

        TimeText = New StaticText(timeTextRect, "TIME", 
                                   CustomFontFamily.NES.GetFontFamily(), fontSize, DrawingPrimitives.WhiteBrush,
                                   horAlignment := StringAlignment.Center, vertAlignment := StringAlignment.Center
                                   )

        TimeNumText = New StaticText(timeNumRect, "999", 
                                      CustomFontFamily.NES.GetFontFamily(), fontSize, DrawingPrimitives.WhiteBrush,
                                      horAlignment := StringAlignment.Center, vertAlignment := StringAlignment.Center
                                      )
    End Sub

    Public Sub SetWorld(world As String)
        WorldNumText.Text = world
    End Sub

    Public Sub SetTime(time As String)
        TimeNumText.Text = time
    End Sub


    Public Overrides Sub Render(g As Graphics)
        MarioText.Render(g)
        ScoreText.Render(g)

        CoinImage.Render(g)
        CrossImage.Render(g)
        CoinText.Render(g)

        PowerupHolder.Render(g)

        WorldText.Render(g)
        WorldNumText.Render(g)

        TimeText.Render(g)
        TimeNumText.Render(g)
    End Sub

    Public Sub SetDimension(size As Size)
        Me.Width = size.Width
        me.height = size.Height
    End Sub

End Class
