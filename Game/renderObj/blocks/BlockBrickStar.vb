﻿Public Class BlockBrickStar
    Inherits BlockBumpable
    
    Private isUsed As Boolean = False

    ''' <summary>
    ''' params:
    ''' 0 : x
    ''' 1 : y
    ''' </summary>
    Public Sub New(params As Object(), mapScene As MapScene)
        Me.New(New Point(params(0)*32, params(1)*32), mapScene)
    End Sub

    Public Sub New(location As Point, mapScene As MapScene)
        MyBase.New(StandardWidth, StandardHeight, location, Sprites.brickBlock, mapScene)
    End Sub

    Public Overrides Sub CollisionBottom(sender As Entity)
        MyBase.CollisionBottom(sender)

        If Not isUsed And Helper.IsPlayer(sender) Then            
            Dim star = New EntStar(New Point(Me.Location.X, Me.Location.Y + Me.Height), MyScene)
            star.Spawn()
            isUsed = True
            StartBump()
        End If

    End Sub
    Public Overrides Sub Animate()
        if isUsed Then
            RenderImage = SpriteSet.GetFirst(SpriteState.Destroy)
        End If
    End Sub
End Class
