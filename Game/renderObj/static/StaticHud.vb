Public Class StaticHud
    Inherits GameItem

    Private height as Integer
    Private width as Integer

    ' Takes up 25% of screen width
    ' this is the start point (in percent)
    Private Const marioScorePercent = 0.25
    Private Const coinsPercent = 0.20
    Private Const powerupPercent = 0.15
    Private Const worldPercentr = 0.2
    Private Const timePercent = 0.2

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

    Public Sub New(width As Integer, height As Integer)
        me.height = height
        Me.width = width
        ' Mario text and score
        Dim halfWayHeight As Integer = height / 2
        ' Rectangle that MARIO is drawn in
        Dim marioTextRect As New Rectangle(0, Helper.TopToBottom(0, halfWayHeight), ' X, Y
                                           width * marioScorePercent, halfWayHeight ' width, height
                                           )

        ' Rectangle that 0000 score is drawn in
        Dim scoreTextRect As New Rectangle(0, Helper.TopToBottom(halfWayHeight, halfWayHeight), ' X, Y
                                           width * marioScorePercent, halfWayHeight
                                           )

        marioText = New StaticText(marioTextRect, "MARIO", NES.GetFontFamily(), fontSize, DrawingPrimitives.WhiteBrush)
        ScoreText = New StaticText(scoreTextRect, "000000", NES.GetFontFamily(), fontSize, DrawingPrimitives.WhiteBrush, 
                                       paddingChar:="0", paddingWidth:=6)

        EntPlayer.ScoreCallback = scoreText

        ' Coin 
        ' 0 x 56
        ' each letter/symbol is 5% of the screen wide
        Dim coinLocation = New Point(marioScorePercent*width, Helper.TopToBottom(halfWayHeight, halfWayHeight))
        CoinImage = New StaticCoin(width*0.05, halfWayHeight, coinLocation)

        Dim xLocation = New Point((marioScorePercent+0.05)*width, Helper.TopToBottom(halfWayHeight, halfWayHeight))
        CrossImage = New StaticCross(width*0.05, halfWayHeight, xLocation)

        ' 2 characters, 5% each
        Dim coinTextRect As New Rectangle((marioScorePercent+0.1)*width, Helper.TopToBottom(halfWayHeight, halfWayHeight), ' X, Y
                                          width * 0.1, halfWayHeight ' width ,height
                                          )

        CoinText = New StaticText(coinTextRect, "0",
                                  CustomFontFamily.NES.GetFontFamily(), fontSize, DrawingPrimitives.WhiteBrush,
                                  horAlignment := StringAlignment.Center, vertAlignment := StringAlignment.Center
                                  )

        EntPlayer.CoinCallback = CoinText

        ' Powerup box
        Dim powerupPoint = New Point((marioScorePercent+coinsPercent)*width, Helper.TopToBottom(0, height))
        Dim widthOrHeight As Integer = Math.Min(width*powerupPercent, height)
        PowerupHolder = New StaticHudPowerup(widthOrHeight, widthOrHeight, powerupPoint)
        PowerupHolder.SetPowerupItem(PowerupType.Mushroom)

    End Sub

    Public Overrides Sub Render(g As Graphics)
        MarioText.Render(g)
        ScoreText.Render(g)

        CoinImage.Render(g)
        CrossImage.Render(g)
        CoinText.Render(g)

        PowerupHolder.Render(g)

        'WorldText.Render(g)
        'WorldNumText.Render(g)

        'TimeText.Render(g)
        'TimeNumText.Render(g)
    End Sub

    Public Sub SetDimension(size As Size)
        Me.Width = size.Width
        me.height = size.Height
    End Sub

End Class
