﻿Imports System.Drawing.Drawing2D

''' <summary>
''' This class represents the background image (just decoration)
''' NO Bricks, players, items, enemies etc, are drawn on (this should just be a backgroud image)
''' </summary>
Public Class BackgroundRender
    Inherits RenderObject

    Public Overrides Property RenderImage As Image = New Bitmap(ScreenGridWidth, ScreenGridHeight)
    Private backgroundNeedsUpdate As Boolean = True


    Public ActualImage As Image

    Private ReadOnly levelWidth As Integer
    Private ReadOnly levelHeight As Integer

    
    Public Overrides Sub Render(g As Graphics)
        ' Overriding the background Render() func for optimization

        ' update RenderImage if it needs to
        if backgroundNeedsUpdate
            Using gfx=Graphics.FromImage(RenderImage)
                ' Interpolation and Alpha values dont matter
                gfx.SmoothingMode = SmoothingMode.None
                gfx.CompositingMode = CompositingMode.SourceCopy
                gfx.InterpolationMode = InterpolationMode.NearestNeighbor
                Crop(ActualImage, gfx, Me.Location, ScreenGridWidth, ScreenGridHeight)
                backgroundNeedsUpdate = False
            End Using
        End If

        g.DrawImage(RenderImage, New Point(0, -toolBarOffSet))
    End Sub



    Sub New(levelWidth As Integer, levelHeight As Integer, backgroundImage As Image)
        MyBase.New(Dimensions.ScreenGridWidth, Dimensions.ScreenGridHeight, New Point(0, 0))
        Me.levelWidth = levelWidth
        me.levelHeight = levelHeight
        ActualImage = New Bitmap(levelWidth, levelHeight)


        Using g=Graphics.FromImage(ActualImage)
            g.DrawImage(backgroundImage, 0, 0, levelWidth, levelHeight)
        End Using
        DbShowImage(ActualImage)

    End Sub

    Public Function CanScrollHorizontal(Optional amount As Integer = 0) As Boolean
        If Me.Location.X + amount + Dimensions.ScreenGridWidth >= levelWidth Then
            ' it went to the right of the screen
            Return False
        ElseIf Me.Location.X + amount < 0 Then
            ' went to left
            Return False
        End If
        Return True
    End Function

    Public Function CanScrollVertical(Optional amount As Integer = 0) As Boolean
        If Me.Location.Y + amount + Dimensions.screenGridHeight >= levelHeight Then
            ' when above max levelHeight
            Return False
        ElseIf Me.Location.Y + amount < 0 Then
            ' when below max levelHeight
            Return False
        End If
        Return True
    End Function

    Public Function ScrollHorizontal(amount As Integer) As Boolean
        Dim canScroll = CanScrollHorizontal(amount)
        If canScroll Then
            Location = New Point(Location.X + amount, Location.Y)
            screenLocation = location
        End If
        backgroundNeedsUpdate = True
        Return canScroll
    End Function

    Public Function ScrollVertical(amount As Integer) As Boolean
        Dim canScroll = CanScrollVertical(amount)
        If canScroll Then
            Location = New Point(Location.X, Location.Y+amount)
            screenLocation = location
        End If
        backgroundNeedsUpdate = True
        Return canScroll
    End Function

   
End Class


