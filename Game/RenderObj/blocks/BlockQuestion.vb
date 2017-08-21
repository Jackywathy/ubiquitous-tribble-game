Public Class BlockQuestion
    Inherits Block
    Public isUsed = False
    Public type As String

    Private Shared ReadOnly PossibleStates As New List(Of String) From { "fire", "default_fire", "mushroom", "coin" }

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
    Sub New(params As Object, scene As Scene)
        Me.New(New Point(params(0), params(1)), params(2), scene)
    End Sub

    Public Overrides Sub animate()
        ' IFC mod 15 = 0
        ' IFC = 0, 15, 30, 45 ...
        If MyScene.frameCount Mod (animationInterval * 3) = 0 And Not isUsed Then
            ' We need 0, 1 or 2
            RenderImage = spriteSet(SpriteState.Constant)((MyScene.frameCount / (animationInterval * 3)) Mod 3)
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
                        If player.state = PlayerStates.Small 
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
                    Case "coin"
                        player.PickupCoin()
                    
                End Select
                isUsed = True
                    Me.RenderImage = My.Resources.blockQuestionUsed
                End If
            End If

    End Sub


End Class
