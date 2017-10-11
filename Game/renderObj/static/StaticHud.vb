﻿
Public Class StaticHud
    Inherits GameItem

    Private height As Integer
    Private width As Integer


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
    Private ReadOnly DragText As StaticText

    Private ReadOnly WorldText As StaticText
    Public ReadOnly WorldNumText As StaticText

    Private ReadOnly TimeText As StaticText
    Private ReadOnly TimeNumText As StaticText

    Public fontSize As Integer = 24

    Private Sub Reshuffle()

    End Sub

    ' pixel offset from the corner for mario
    Private Const MarioOffset = 2

    Friend Sub SetPowerup(type As PowerupType)
        Me.PowerupHolder.SetPowerupItem(type)
    End Sub

    Public Sub New(width As Integer, height As Integer)
        Me.height = height
        Me.width = width
        ' Mario text and score
        Dim halfWayHeight As Integer = height / 2
        ' Rectangle that MARIO is drawn in
        Dim marioTextRect As New Rectangle(marioLocation + MarioOffset, Helper.TopToBottom(0, halfWayHeight) - MarioOffset, ' X, Y
                                           width * marioScorePercent - MarioOffset, halfWayHeight - MarioOffset ' width, height
                                           )

        ' Rectangle that 0000 score is drawn in
        Dim scoreTextRect As New Rectangle(marioLocation + MarioOffset, Helper.TopToBottom(halfWayHeight, halfWayHeight) - MarioOffset, ' X, Y
                                           width * marioScorePercent - MarioOffset, halfWayHeight - MarioOffset
                                           )

        MarioText = New StaticText(marioTextRect, "MARIO", NES.GetFontFamily(), fontSize, DrawingPrimitives.WhiteBrush)
        ScoreText = New StaticText(scoreTextRect, "000000", NES.GetFontFamily(), fontSize, DrawingPrimitives.WhiteBrush,
                                       paddingChar:="0", paddingWidth:=6)

        EntPlayer.ScoreCallback = ScoreText

        ' Coin 
        ' 0 x 56
        ' each letter/symbol is 5% of the screen wide
        Dim coinLocation = New Point(coinsLocation * width, Helper.TopToBottom(halfWayHeight, halfWayHeight))
        CoinImage = New StaticCoin(width * 0.05, halfWayHeight, coinLocation)

        Dim xLocation = New Point((coinsLocation + 0.05) * width, Helper.TopToBottom(halfWayHeight, halfWayHeight))
        CrossImage = New StaticCross(width * 0.05, halfWayHeight, xLocation)

        ' 2 characters, 5% each
        Dim coinTextRect As New Rectangle((coinsLocation + 0.1) * width, Helper.TopToBottom(halfWayHeight, halfWayHeight), ' X, Y
                                          width * 0.1, halfWayHeight ' width ,height
                                          )

        CoinText = New StaticText(coinTextRect, "0",
                                  CustomFontFamily.NES.GetFontFamily(), fontSize, DrawingPrimitives.WhiteBrush,
                                  horAlignment:=StringAlignment.Center, vertAlignment:=StringAlignment.Center
                                  )

        EntPlayer.CoinCallback = CoinText

        ' Powerup box
        Dim powerupPoint = New Point(powerupLocation * width, Helper.TopToBottom(0, height))
        Dim widthOrHeight As Integer = Math.Min(width * powerupPercent, height)
        PowerupHolder = New StaticHudPowerup(widthOrHeight, widthOrHeight, powerupPoint)
        'PowerupHolder.SetPowerupItem(PowerupType.Mushroom)

        ' Drag me!

        '    |
        '    |
        '   \|/

        ' ideally, only render this when an item is present in the powerup box
        ' pls fix location

        Dim dragPromptRect = New Rectangle((powerupLocation - 0.02) * width, Helper.TopToBottom(2 * halfWayHeight, halfWayHeight),
                                      width * 0.1, halfWayHeight)

        ' text: "Drag me!"
        DragText = New StaticText(dragPromptRect, "pls fix",
                                  CustomFontFamily.NES.GetFontFamily(), 12, DrawingPrimitives.WhiteBrush,
                                  horAlignment:=StringAlignment.Center, vertAlignment:=StringAlignment.Center
                                  )

        ' World 
        Dim worldTextRect = New Rectangle(worldLocation * width, Helper.TopToBottom(0, halfWayHeight),
                                      width * worldPercent, halfWayHeight)

        Dim worldNumRect = New Rectangle(worldLocation * width, Helper.TopToBottom(halfWayHeight, halfWayHeight),
                                      width * worldPercent, halfWayHeight)
        WorldText = New StaticText(worldTextRect, "WORLD",
                                   CustomFontFamily.NES.GetFontFamily(), fontSize, DrawingPrimitives.WhiteBrush,
                                   horAlignment:=StringAlignment.Center, vertAlignment:=StringAlignment.Center
                                   )

        WorldNumText = New StaticText(worldNumRect, "1-1",
                                      CustomFontFamily.NES.GetFontFamily(), fontSize, DrawingPrimitives.WhiteBrush,
                                      horAlignment:=StringAlignment.Center, vertAlignment:=StringAlignment.Center
                                      )
        ' Time
        Dim timeTextRect = New Rectangle(timeLocation * width, Helper.TopToBottom(0, halfWayHeight),
                                          width * timePercent, halfWayHeight)

        Dim timeNumRect = New Rectangle(timeLocation * width, Helper.TopToBottom(halfWayHeight, halfWayHeight),
                                         width * timePercent, halfWayHeight)

        TimeText = New StaticText(timeTextRect, "TIME",
                                   CustomFontFamily.NES.GetFontFamily(), fontSize, DrawingPrimitives.WhiteBrush,
                                   horAlignment:=StringAlignment.Center, vertAlignment:=StringAlignment.Center
                                   )

        TimeNumText = New StaticText(timeNumRect, "999",
                                      CustomFontFamily.NES.GetFontFamily(), fontSize, DrawingPrimitives.WhiteBrush,
                                      horAlignment:=StringAlignment.Center, vertAlignment:=StringAlignment.Center
                                      )
    End Sub

    Friend Sub EnableTime()
        displayTime = True
    End Sub
    Private displayTime As Boolean = True
    Friend Sub DisableTime()
        displayTime = False
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
        DragText.Render(g)

        WorldText.Render(g)
        WorldNumText.Render(g)

        TimeText.Render(g)

        If displayTime
            TimeNumText.Render(g)
        End If
    End Sub

    Public Sub SetDimension(size As Size)
        Me.width = size.Width
        Me.height = size.Height
    End Sub

End Class
