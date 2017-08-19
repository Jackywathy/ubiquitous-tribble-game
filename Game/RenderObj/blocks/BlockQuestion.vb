Public Class BlockQuestion
    Inherits Block
    Public isUsed = False
    Public type As String
    Private Shared ReadOnly PossibleStates As New List(Of String) From { "fire", "default_fire", "mushroom" }

    Sub New(location As Point, type As String, scene As Scene)
        MyBase.New(blockWidth, blockHeight, location, Sprites.itemBlock, scene)
        Me.spriteset = spriteset
        Me.type = type
        If Not PossibleStates.Contains(type)
            Throw New ArgumentException("type must be in PossibleStates (fire, default_fire, mushroom)")
        End If
    End Sub

    ''' <summary>
    ''' 0 : x
    ''' 1 :y 
    ''' 2 : "default_fire" or "fire" or "mushroom"
    ''' </summary>
    ''' <param name="params"></param>
    ''' <param name="scene"></param>
    SUb New(params As Object, scene As Scene)
        Me.New(New Point(params(0), params(1)), params(2), scene)
    End SUb

    Public Overrides Sub animate()
        If internalFrameCounter Mod (animationInterval * 3) = 0 And Not isUsed Then
            RenderImage = spriteSet.SendToBack(0)
        End If
    End Sub

    Public Overrides Sub CollisionBottom(sender As Entity)
        MyBase.CollisionBottom(sender)
        If Not isUsed Then
            ' only player is allowed to activate block
            If sender.GetType() = GetType(EntPlayer)
                Dim player As EntPlayer = sender
                Select Case type
                    Case "default_fire"
                        ' check the current status of mario, then spawn the right
                        If player.state = 0 ' TODO REMOVE
                            Dim mushroom As New EntMushroom(32, 32, New Point(Me.Location.X, Me.Location.Y + Me.Height), MyScene)
                            mushroom.Spawn()
                        Else
                            Dim flower As New EntFireFlower(32, 32, New Point(Me.Location.X, Me.Location.Y + Me.Height), MyScene)
                            flower.Spawn()
                        End If
                    Case "fire"
                        Dim flower As New EntFireFlower(32, 32, New Point(Me.Location.X, Me.Location.Y + Me.Height), MyScene)
                        flower.Spawn()
                    Case "mushroom"
                        Dim mushroom As New EntMushroom(32, 32, New Point(Me.Location.X, Me.Location.Y + Me.Height), MyScene)
                        mushroom.Spawn()
                    
                End Select
                isUsed = True
                    Me.RenderImage = My.Resources.blockQuestionUsed
                End If
            End If

    End Sub


End Class
