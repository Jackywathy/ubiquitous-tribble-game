''' <summary>
''' This class represents the Background image (just decoration)
''' NO Bricks, players, items, enemies etc, are drawn on (just color)
''' </summary>
Public Class BackgroundRender
    Inherits GameItem

    Private ReadOnly levelWidth As Integer
    Private ReadOnly levelHeight As Integer

    Private ReadOnly backgroundColor As SolidBrush

    Private myScene As MapScene

    Private location As Point

    Public Overrides Sub Render(g As Graphics)
        g.FillRectangle(BackgroundColor, New Rectangle(0,0,ScreenGridWidth, ScreenGridHeight))
    End Sub


    Sub New(width As Integer, height As Integer, backgroundColor As String, mapScene As MapScene)
        Me.BackgroundColor = New SolidBrush(New ColorConverter().ConvertFrom(backgroundColor))
        levelWidth = width
        levelHeight = height
        Me.myScene = mapScene
    End Sub

    Public Function CanScrollHorizontal(Optional amount As Integer = 0) As Boolean
        If MyScene.ScreenLocation.X + amount + Dimensions.ScreenGridWidth >= levelWidth Then
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
            MyScene.ScreenLocation = location
        End If
        Return canScroll
    End Function

    Public Function ScrollVertical(amount As Integer) As Boolean
        Dim canScroll = CanScrollVertical(amount)
        If canScroll Then
            Location = New Point(Location.X, Location.Y+amount)
            MyScene.ScreenLocation = location
        End If
        Return canScroll
    End Function

End Class


