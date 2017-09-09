Public Class BlockQuestion
    Inherits BlockBumpable

    Private isUsed = False
    Private ReadOnly powerup As QuestionBlockReward = -1


    Sub New(location As Point, powerup As String, theme As RenderTheme, mapScene As MapScene)
        MyBase.New(StandardWidth, StandardHeight, location, Sprites.itemBlock, mapScene)
        Me.powerup = Helper.StrToEnum(Of QuestionBlockReward)(powerup)
    End Sub

    ''' <summary>
    ''' 0 : x
    ''' 1 :y 
    ''' 2 : "default_fire" or "fire" or "mushroom"
    ''' </summary>
    ''' <param name="params"></param>
    ''' <param name="mapScene"></param>
    Sub New(params As Object,  theme As RenderTheme, mapScene As MapScene)
        Me.New(New Point(params(0)*32, params(1)*32), params(2), theme, mapScene)
    End Sub

    Public Overrides Sub CollisionBottom(sender As Entity)
        MyBase.CollisionBottom(sender)

        If Not isUsed Then
            ' only player is allowed to activate block
            If Helper.IsPlayer(sender) Then
                Dim player As EntPlayer = sender
                Select Case powerup
                    Case QuestionBlockReward.DefaultFire
                        ' check the current status of mario, then spawn the right powerup
                        If player.state = PlayerStates.Small Then
                            Dim mushroom As New EntMushroom(32, 32, New Point(Me.Location.X, Me.Location.Y + Me.Height), MyScene)
                            mushroom.Spawn()
                        Else
                            Dim flower As New EntFireFlower(32, 32, New Point(Me.Location.X, Me.Location.Y + Me.Height), MyScene)
                            flower.Spawn()
                        End If
                    Case QuestionBlockReward.Fire
                        Dim flower As New EntFireFlower(32, 32, New Point(Me.Location.X, Me.Location.Y + Me.Height), MyScene)
                        flower.Spawn()
                    Case QuestionBlockReward.Mushroom
                        Dim mushroom As New EntMushroom(32, 32, New Point(Me.Location.X, Me.Location.Y + Me.Height), MyScene)
                        mushroom.Spawn()
                    Case QuestionBlockReward.Coin
                        player.PickupCoin()
                        Dim coin = New EntCoinFromBlock(32, 32, New Point(Me.Location.X, Me.Location.Y + Me.Height), MyScene)
                        coin.spawn()
                End Select
                StartBump()
                IsUsed = True
            End If
        End If
    End Sub

    Public Overrides Sub Animate()
        ' IFC mod 15 = 0
        ' IFC = 0, 15, 30, 45 ...
        If IsUsed Then
            Me.RenderImage = spriteSet(SpriteState.Destroy)(0)
        Else If MyScene.frameCount Mod (animationInterval * 3) = 0 And Not isUsed Then
            ' We need 0, 1 or 2
            RenderImage = spriteSet(SpriteState.ConstantRight)((MyScene.frameCount / (animationInterval * 3)) Mod 3)
        End If
    End Sub

End Class
