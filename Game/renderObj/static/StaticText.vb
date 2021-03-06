﻿Public Class StaticText
    Inherits GameItem

    Private paddingWidth As Integer
    Private paddingChar As Char

    Private _text As String

    Public Property Text As String
        Get
            Return _text
        End Get
        Set(value As String)
            _text = value.PadLeft(paddingWidth, paddingChar)
        End Set
    End Property

    Private sf As StringFormat
    
    Private font As Font
    Private brush As brush
    Private drawnRect As Rectangle

    Public Overrides Sub Render(g As Graphics)
        g.DrawString(Text, font, brush, drawnRect, sf)
        If ShowBoundingBox
            g.DrawRectangle(DrawingPrimitives.BlackPen, drawnRect)
        End If
    End Sub

    Private Shared Function GetFormat(hor As StringAlignment, vert As StringAlignment)
        dim sf = New StringFormat()
        sf.Alignment = hor
        sf.LineAlignment = vert
       
        Return sf
    End Function

    Sub New (drawnRect As Rectangle, str As String, fontFam As FontFamily, emSize As Integer, brush As Brush, Optional horAlignment As StringAlignment = StringAlignment.Near, Optional vertAlignment As StringAlignment=StringAlignment.Near, Optional paddingChar As Char = "0", Optional paddingWidth As Integer = 0)
        Me.New(drawnRect, str, New Font(fontFam, emSize), brush, GetFormat(horAlignment, vertAlignment), paddingWidth, paddingChar)       
    End Sub

    Sub New (drawnRect As Rectangle, str As String, font As Font, brush As Brush, format As StringFormat, paddingWidth As integer, paddingChar As Char)
        MyBase.New()
        Me.sf = format
        Me.paddingWidth = paddingWidth
        Me.paddingChar = paddingChar
        Me.brush = brush
        Me.Text = str
        Me.Font = font
        Me.drawnRect = drawnRect
        Me.drawnRect.Y = Dimensions.ScreenGridHeight - drawnRect.Height - drawnRect.Y
    End Sub
End Class

Public NotInheritable Class DrawingPrimitives
    Public Shared BlackPen As New Pen(Color.Black)  
    Public Shared RedPen As New Pen(Color.Red) 
    Public Shared BluePen As New Pen(Color.Blue) 
    Public Shared WhiteBrush As New SolidBrush(color.White)
    Public Shared BlackBrush As New SolidBrush(color.Black)

    Private Sub New

    End Sub
End Class