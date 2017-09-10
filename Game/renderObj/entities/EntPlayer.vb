Public Enum PlayerStates
    Dead = 0
    Small = 1
    Big = 2
    Fire = 3
    Ice = 4
End Enum


Public Class EntPlayer
    Inherits Entity

    
    ''' <summary>
    ''' Time invulnerable in ticks (60 ticks / second)
    ''' </summary>
    Public Const FramesInvulnerableIfHit = 60
    ''' <summary>
    ''' Time invulnerable due to star power ( 60 ticks / second)
    ''' </summary>
    Public Const StarInvincibilityDuration = 600
    ''' <summary>
    ''' Coin to lives conversion
    ''' </summary>
    Public Const CoinsToLives = 100
    ''' <summary>
    ''' how many points given per coin
    ''' </summary>
    

   
    Private Shared _lives As Integer = 5
    Private Shared _coins As Integer = 0

    Private _score As Integer = 0

    ''' <summary>
    ''' Total score accumated by all players. Shared!
    ''' </summary>
    ''' <returns></returns>
    Public Shared Property TotalScore As Integer
    ''' <summary>
    ''' Coins are shared between all players
    ''' </summary>
    ''' <returns></returns>
    Public Shared Property Coins
        Get
            Return _coins
        End Get
        Set(value)
            If value >= CoinsToLives Then
                _coins = value - CoinsToLives
                _lives += 1
            Else
                _coins += 1
            End If
        End Set
    End Property
    ''' <summary>
    ''' Lives are shared between all players
    ''' </summary>
    ''' <returns></returns>
    Public Shared Property Lives As Integer 
        Get
            return _lives
        End Get
        Set(value As Integer)
            _lives = value
            If _lives <= 0
                ' TODO make player unaviable etc
                'Throw New Exception("out of lives!")
            End If
        End Set
    End Property
    ''' <summary>
    ''' Score is not shared, but totalScore is
    ''' </summary>
    ''' <returns></returns>
    Public Property Score As Integer
        Get
            Return _score
        End Get
        Set(value As Integer)
            TotalScore += (value - Score)

            _score = value
            
            If ScoreCallBack IsNot Nothing
                ScoreCallback.Text = TotalScore.ToString()
            End If
        End Set
    End Property

    Public Shared Property ScoreCallback As StaticText = Nothing


    Public AllowJumpInput As Boolean = True
    Public AllowShoot As Boolean = True
    Public IsCrouching As Boolean = False
    Public AllowedToUncrouch As Boolean = True
    Public IsBouncingOffEntity As Boolean = False

    Public InvinicibilityTimer = 0

    Private _state As PlayerStates = PlayerStates.Small

    ''' <summary>
    ''' Sets/Gets the state of a player
    ''' If player state is set to Dead, will commence killing player animation etc
    ''' </summary>
    ''' <returns></returns>
    Public Property State As PlayerStates
        Get
            return _state
        End Get
        Set(value As PlayerStates)
            Select Case value
                Case PlayerStates.Dead
                    Me.KillPlayer()
                Case PlayerStates.Small :
                    Me.SpriteSet = Sprites.playerSmall
                    Me.Height = MarioHeightS
                    Me.CollisionHeight = MarioHeightS
                Case PlayerStates.Big :
                    Me.SpriteSet = Sprites.playerBig
                    Me.Height = MarioHeightB
                    Me.CollisionHeight = MarioHeightB
                Case PlayerStates.Fire : Me.SpriteSet = Sprites.playerBigFire
                    Me.Height = MarioHeightB
                    Me.CollisionHeight = MarioHeightB
            End Select
            _state = value
        End Set
    End Property

    Public NumFireballs As Integer = 0
    Private invulnerableTime As Integer = 0

    Public Overrides Property moveSpeed As Distance = New Distance(0.6, 12)
    Public Overrides Property maxVeloc As Distance = New Distance(6, -15)
    ''' <summary>
    ''' Creates a new player
    ''' </summary>
    ''' <param name="width"></param>
    ''' <param name="height"></param>
    ''' <param name="location"></param>
    ''' <param name="mapScene"></param>
    Sub New(width As Integer, height As Integer, location As Point, mapScene As MapScene)
        MyBase.New(width, height, location, Sprites.playerSmall, mapScene)
    End Sub

    ''' <summary>
    ''' Called when up/jump key is pressed
    ''' Will check if already jumping etc etc
    ''' </summary>
    Public Sub Jump()

    End Sub

    ''' <summary>
    ''' Makes Player crouch and not crouch 
    ''' This should NEVER be called if Mario is small
    ''' </summary>
    Public Sub OnCrouch(tryCrouch As Boolean)
        ' isCrouching only false if state is false and if player is allowed to uncrouch
        If (Not tryCrouch) And allowedToUncrouch Then
            isCrouching = False
        Else
            isCrouching = True
        End If

        If isCrouching Then ' IS crouching
            Me.CollisionHeight = MarioHeightS
        Else ' is NOT crouching
            Me.CollisionHeight = MarioHeightB
        End If

    End Sub

    ''' <summary>
    ''' kills the player if he is small, else loses a powerup
    ''' Will set/check invulnerability time
    ''' </summary>
    Public Sub PlayerGotHit()
        If InvinicibilityTimer = 0 And invulnerableTime = 0 Then
            Select Case Me.State
                Case PlayerStates.Small
                    State = PlayerStates.Dead
                Case PlayerStates.Big
                    State = PlayerStates.Small
                Case PlayerStates.Fire
                    State = PlayerStates.Big
                Case PlayerStates.Ice
                    State = PlayerStates.Big
            End Select
            invulnerableTime = FramesInvulnerableIfHit

            If Me.State <> PlayerStates.Dead Then
                Sounds.Warp.Play()
            End If
        End If
    End Sub

    Public Sub PickupCoin
        Sounds.CoinPickup.Play()
        Coins += 1
        Score += PlayerPoints.Coin
    End Sub

    ''' <summary>
    ''' Play outro mapScene and remove player / decrease lives
    ''' Do not use for player damage - use PlayerGotHit instead
    ''' DO NOT USE - Instead set player.state to Dead
    ''' </summary>
    Private Sub KillPlayer()
        Me.isDead = True
        Me.veloc.x = 0
        Me.veloc.y = 0
        Me.defaultY = Me.Location.Y
        Lives -= 1
        MusicPlayer.BackgroundPlayer.Stop()
        Sounds.PlayerDead.Play()
        

    End Sub

    ''' <summary>
    ''' Attempts to shoot a fireball. Will not shoot if 2 are onscreen already.
    ''' Do not call if state != PlayerStates.fire
	''' </summary>
    Public Sub TryShootFireball()
        If numFireballs < 2 Then
            Dim direction = 1
            Dim pointSpawn = Me.Location
            If Me.isFacingForward Then
                pointSpawn = New Point(Me.Location.X + Me.Width, Me.Location.Y + 0.5 * Me.Height)
            Else
                pointSpawn = New Point(Me.Location.X, Me.Location.Y + 0.5 * Me.Height)
                direction *= -1
            End If

            MyScene.PrepareAdd(New EntFireball(16, 16, pointSpawn, direction, Me, MyScene))
            Me.numFireballs += 1
        End If
    End Sub

    Public Sub BounceOffEntity(holdingJump As Boolean)
        Me.veloc = New Distance(Me.veloc.x, 0)
        If holdingJump Then
            Me.AccelerateY(Me.moveSpeed.y * 1.1, True)
        Else
            Me.AccelerateY(Me.moveSpeed.y * 0.8, True)
        End If
    End Sub


    Private defaultY As Integer
    Private deathTimer As Integer

    Public Overrides Sub Animate()
        If Not Me.isDead Then
            ' Make sure this is exhaustive
            Dim spriteStateToUse = -2

            ' If crouching
            If isCrouching Then
                spriteStateToUse = SpriteState.CrouchRight

                ' If grounded or falling off a platform
            ElseIf isGrounded Or (Not isGrounded And Not didJumpAndNotFall) Then

                ' If moving
                If veloc.x <> 0 Then

                    spriteStateToUse = SpriteState.GroundRight


                ElseIf veloc.x = 0 Then 'If still
                    spriteStateToUse = SpriteState.ConstantRight
                End If

                'If jumping
            ElseIf didJumpAndNotFall Then
                spriteStateToUse = SpriteState.AirRight
            End If

            If Not isFacingForward Then
                spriteStateToUse += 1
            End If

            Select Case spriteStateToUse
            ' Multi-frame
                Case SpriteState.GroundRight, SpriteState.GroundLeft
                    If MyScene.GlobalFrameCount Mod animationInterval = 0 Then
                        Me.renderImage = SpriteSet.SendToBack(spriteStateToUse)
                    End If

                    ' Single frame
                Case Else
                    Me.renderImage = SpriteSet(spriteStateToUse)(0)
            End Select
        Else
            Me.renderImage = SpriteSet(SpriteState.Destroy)(0)
            Me.deathTimer += 1
            If deathTimer > 60 Then
                Dim x = (Me.deathTimer - 60) / (animationInterval * 5)

                ' Use displacement/time function
                ' f(x) = 100(2x - x^2)

                Dim heightFunc = 100 * (2 * (x) - (x * x))
                Me.Location = New Point(Me.Location.X, defaultY + heightFunc)
                If Me.Location.Y < 0 Then
                    Me.KillPlayer()
                End If
            End If
        End If
    End Sub

    Public Overrides Sub UpdateVeloc()
        If Double.IsNan(me.veloc.X)
            Me.ID += 0
        End If

        MyBase.UpdateVeloc()

        Dim outOfMap = IsOutOfMap()
        if (outOfMap = Direction.Right And Me.veloc.X > 0) Or (outOfMap = Direction.Left And Me.veloc.X < 0)
            veloc.X = 0
        End If
          
            
        If Me.isCrouching And Not isGrounded Then
            Me.onCrouch(False)
        End If
        If invulnerableTime <> 0 Then
            invulnerableTime -= 1
        End If

        If InvinicibilityTimer > 0 Then
            InvinicibilityTimer -= 1
        End If

    End Sub

    
End Class


Public Class PlayerPoints
    Public Const Coin = 100
    Public Const Goomba = 500
    Public Const Koopa = 700
    Public Const Mushroom = 2000
    Public Const Firefire = 4000

    Public Const Star = 5000
End Class