Public Class BlockQuestion
    Inherits Block
    
    Public isUsed = False
    Public type As QuestionBlockReward = -1


    Sub New(location As Point, type As String, scene As Scene)
        MyBase.New(blockWidth, blockHeight, location, Sprites.itemBlock, scene)
        Me.spriteset = spriteset
        Me.type = Helper.StrToEnum(Of QuestionBlockReward)(type)
        
    End Sub

    ''' <summary>
    ''' 0 : x
    ''' 1 :y 
    ''' 2 : "default_fire" or "fire" or "mushroom"
    ''' </summary>
    ''' <param name="params"></param>
    ''' <param name="scene"></param>
    Sub New(params As Object, scene As Scene)
        Me.New(New Point(params(0)*32, params(1)*32), params(2), scene)
    End Sub

    Public Overrides Sub animate()
        ' IFC mod 15 = 0
        ' IFC = 0, 15, 30, 45 ...
        If MyScene.frameCount Mod (animationInterval * 3) = 0 And Not isUsed Then
            ' We need 0, 1 or 2
            RenderImage = spriteSet(SpriteState.ConstantRight)((MyScene.frameCount / (animationInterval * 3)) Mod 3)
        End If
        If isMoving Then
            ' bumps block
            Me.frameCount += 1

            Me.Location = bounceFunction(Me.frameCount, Me.defaultLocationY)

            ' f(x) = 0 when x = 2
            If frameCount / animationInterval >= 2 Then
                Me.isMoving = False
            End If
        End If
    End Sub

    Public Overrides Sub CollisionBottom(sender As Entity)
        MyBase.CollisionBottom(sender)
        If Not isUsed Then
            ' only player is allowed to activate block
            If Helper.IsPlayer(sender) Then
                Dim player As EntPlayer = sender
                Select Case type
                    Case QuestionBlockReward.DefaultFire
                        ' check the current status of mario, then spawn the right
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
                        MyScene.PrepareAdd(New EntCoinFromBlock(32, 32, New Point(Me.Location.X, Me.Location.Y + Me.Height), MyScene))
                End Select
                If Not isUsed Then
                    frameCount = 0
                    Me.isMoving = True
                    Me.defaultLocationY = Me.Location.Y
                End If
                isUsed = True

                Me.RenderImage = My.Resources.blockQuestionUsed
            End If
        End If
    End Sub


End Class
