Imports WinGame

Public Enum PlayerStates
    Dead = 0
    Small = 1
    Big = 2
    Fire = 3
    Ice = 4
End Enum


Public Class EntPlayer
    Inherits Entity
    Implements ISceneAddable


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
            CoinCallback.Text = _coins.ToString()
        End Set
    End Property
    ''' <summary>
    ''' Lives are shared between all players
    ''' </summary>
    ''' <returns></returns>
    Public Shared Property Lives As Integer
        Get
            Return _lives
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

            If ScoreCallback IsNot Nothing
                ScoreCallback.Text = TotalScore.ToString()
            End If
        End Set
    End Property

    Public Shared Property ScoreCallback As StaticText = Nothing
    Public Shared Property CoinCallback As StaticText = Nothing

    Public OnFlag As Boolean = False
    Public AllowJumpInput As Boolean = True
    Public AllowShoot As Boolean = True
    Public IsCrouching As Boolean = False
    Public AllowedToUncrouch As Boolean = True
    Public IsBouncingOffEntity As Boolean = False

    Public InvinicibilityTimer As Integer = 0

    Private _state As PlayerStates = PlayerStates.Small

    ''' <summary>
    ''' Sets/Gets the state of a player
    ''' If player state is set to Dead, will commence killing player animation etc
    ''' </summary>
    ''' <returns></returns>
    Public Property State As PlayerStates
        Get
            Return _state
        End Get
        Set(value As PlayerStates)
            If value = _state
                Return
            End If
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
                Case PlayerStates.Fire :
                    Me.SpriteSet = Sprites.playerBigFire
                    Me.Height = MarioHeightB
                    Me.CollisionHeight = MarioHeightB
            End Select
            _state = value
        End Set
    End Property

    private _isinpipe as boolean

    Public Property IsInPipe As Boolean 
        Get
            Return _isinpipe
        End Get
        Set(value As Boolean)
            _isinpipe = value
        End Set
    End Property
    


    Private Sub SetXToMiddle(x As integer)
        ' sets me.x to x + 1/2 of standard width
        me.X = x + StandardWidth/2
    End Sub

    Friend Sub EnterHorizontalPipeExitVertical(map As MapEnum, insertion As Point?, pipeOnOtherSide As Boolean, pipeLocation As Point)
        me.Y = pipeLocation.Y
        Me.veloc.x = 0
        me.veloc.y = 0

        InvinicibilityTimer = StandardPipeTime
        Dim point = MyScene.GetScreenLocation(Me)
        point.Y = BottomToTop(point.Y)
        Dim enterPipe as New MarioGoinDownPipeAnimationQueue(Me, PipeType.Horizontal,True, MyScene.Parent)

        Dim exitPipe As New MarioGoinDownPipeAnimationQueue(Me, PipeType.Vertical, False,  MyScene.Parent, stopmusic := False)

        Dim mapChange = MyScene.Parent.QueueMapChangeWithCircleAnimation(map, insertion, centerToplayer := True,animationLocation:=point, before := enterPipe)
        mapChange.next = exitPipe

    End Sub

    Public Sub EnterVerticalPipeExitNone(map As MapEnum, insertion As Point?, pipeLocation As Point)
        Me.X = pipeLocation.X + StandardWidth /2
        Me.veloc.x = 0
        me.veloc.y = 0

        InvinicibilityTimer = StandardPipeTime

        Dim point = MyScene.GetScreenLocation(Me)
        point.Y = BottomToTop(point.Y)
        Dim enterPipe as New MarioGoinDownPipeAnimationQueue(Me, PipeType.Vertical, True, MyScene.Parent)

        Dim mapChange = MyScene.Parent.QueueMapChangeWithCircleAnimation(map, insertion, centerToplayer := False, animationLocation:=point, before := enterPipe)
        
    End Sub

    Friend Sub BeginHorizontalPipe(goingIn As Boolean, Optional time As Integer = StandardPipeTime)
        SetPipeVars(time)
        IsInPipe = True
        Dim rounded = Location
        rounded.Y = CInt(Math.Floor(rounded.Y / 32))*32
        animator = New HorizontalAnimator(RenderImage, goingIn, rounded)
    End Sub

    Friend Sub BeginVerticalPipe(goingIn As boolean, Optional time As Integer = StandardPipeTime)
        SetPipeVars(time)
        animator = New VerticalAnimator(RenderImage, goingIn, Me.location)
    End Sub

    Private Sub SetPipeVars(time as integer)
        pipeElapsedTime = 0
        pipeMaxTime = time
        If animator IsNot Nothing
            animator.Dispose()
        End If
        IsInPipe = True
    End Sub

    Protected Class ImageSlice
        Public ReadOnly Property Height As Integer
            Get
                Return If(Image IsNot Nothing, Image.Height, 0)
            End Get
        End Property

        Public ReadOnly Image As Image

        Public ReadOnly Property Width As Integer
            Get
                Return If(Image IsNot Nothing, Image.Width, 0)
            End Get
        End Property

        Public Location as Point

        Public ReadOnly Visible As Boolean

        Sub New(image As Image, location as point, Optional visible As Boolean = True)
            Me.Image = image
            Me.location = location
            Me.Visible = visible
        End Sub

    End Class

    Protected Class VerticalAnimator
        Inherits MarioAnimator

        Public Sub New(image As Image, goingIn As Boolean, location As point)
            MyBase.New(image, goingIn, location)
        End Sub

        Public Overrides Function GetSlice(percent As Double) As ImageSlice
            ' percent is between 0 <= perecent <= 1
            If percent > 1 Or percent < 0
                Throw New Exception(String.Format("Percent, {0} out of bounds", percent))
            End If
            If Not goingIn
                percent = 1- percent
            End If

            Dim height = percent * image.Height
            Dim bottomLeft = New Point(0, CInt(height))
            Dim pixelHeight As Integer = (1 - percent) * image.Height
            If pixelHeight > 0
                Return New ImageSlice(Crop(Me.image, bottomLeft, image.Width, pixelHeight), location)
            Else
                Return New ImageSlice(Nothing, location, False)
            End If

        End Function
    End Class

    Protected MustInherit Class MarioAnimator
        Implements IDisposable
        Friend image As Image
        Friend goingIn As Boolean
        Friend startLocation As point
        Friend location as point
        Private slices as New List(Of Image)

        Sub New(image As Image, goingIn As Boolean, startLocation As point)
            Me.image = image
            Me.goingIn = goingIn
            Me.startLocation = startLocation
            Me.location = startLocation
        End Sub

        Public MustOverride Function GetSlice(percent As Double) As ImageSlice

#Region "IDisposable Support"
        Private disposedValue As Boolean ' To detect redundant calls

        ' IDisposable
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not disposedValue Then
                For each slice In slices
                    slice.dispose()
                Next

            End If
            disposedValue = True
        End Sub

        ' This code added by Visual Basic to correctly implement the disposable pattern.
        Public Sub Dispose() Implements IDisposable.Dispose
            ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
            Dispose(True)
            ' TODO: uncomment the following line if Finalize() is overridden above.
            ' GC.SuppressFinalize(Me)
        End Sub
#End Region
    End Class

    Protected Class HorizontalAnimator
        Inherits MarioAnimator

       
        Public Sub New(renderImage As Image, goingIn As Boolean, startLocation As point)
            MyBase.New(renderImage, goingIn, startLocation)
        End Sub

        Public Overrides Function GetSlice(percent As Double) As ImageSlice
            If percent > 1 Or percent < 0
                Throw New Exception(String.Format("Percent, {0} out of bounds", percent))
            End If

            If goingIn
                percent = 1- percent
            End If

            Dim width = percent * image.width
            Dim bottomLeft = New Point(0, 0)
            
            If width > 0
                location.X = startLocation.X + StandardWidth * (1-percent)
                Return New ImageSlice(Crop(Me.image, bottomLeft, width, image.Height), location)
            Else
                Return New ImageSlice(Nothing, location, False)
            End If
        End Function
    End Class

    Friend Sub Reset()
        Me.isGrounded = False
        Me.currentGroundObjects.Clear()
        Me.Width = StandardWidth
        Select Case state
            Case PlayerStates.Big, PlayerStates.Fire, PlayerStates.Ice
                 Me.Height = StandardHeight * 2
            Case Else
                 Me.Height = StandardHeight

        End Select
    End Sub

    Public NumFireballs As Integer = 0
    Private invulnerableTime As Integer = 0

    Public Overrides Property moveSpeed As Velocity = New Velocity(0.6, 12)
    Public Overrides Property maxVeloc As Velocity = New Velocity(5, -15)
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
    ''' TODO IMplement JUMP
    ''' </summary>
    Public Sub Jump()

    End Sub

    ''' <summary>
    ''' Makes Player crouch and not crouch 
    ''' This should NEVER be called if Mario is small
    ''' </summary>
    Public Sub OnCrouch(tryCrouch As Boolean)
        ' isCrouching only false if state is false and if player is allowed to uncrouch
        If (Not tryCrouch) And AllowedToUncrouch Then
            IsCrouching = False
        Else
            IsCrouching = True
        End If

        If IsCrouching Then ' IS crouching
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

    Public Sub PickupCoin()
        Sounds.CoinPickup.Play()
        Coins += 1
        Score += PlayerPoints.Coin
    End Sub

    ''' <summary>
    ''' DO NOT USE - Instead set player.state to Dead
    ''' Play outro mapScene and remove player / decrease lives
    ''' Do not use for player damage - use PlayerGotHit instead
    ''' </summary>
    Private Sub KillPlayer()
        If isDead Then
            Return
        End If

        Me.isDead = True
        Me.veloc.x = 0
        Me.veloc.y = 0
        Me.defaultY = Me.Location.Y
        Lives -= 1
        MyScene.RegisterDeath(Me)


    End Sub

    ''' <summary>
    ''' Attempts to shoot a fireball. Will not shoot if 2 are onscreen already.
    ''' Do not call if state != PlayerStates.fire
	''' </summary>
    Public Sub TryShootFireball()
        If NumFireballs < 2 Then
            Dim direction = 1
            Dim pointSpawn = Me.Location

            Dim spawnY = Me.Location.Y + 0.5 * Me.Height - 16
            '
            '  MARIO
            '   ___
            '  |   |
            '  |___|
            '  |  o| <--- spawn fireball here
            '  |___|
            '
            '
            If Me.isFacingForward Then
                pointSpawn = New Point(Me.Location.X + Me.Width - 16, spawnY)
            Else
                pointSpawn = New Point(Me.Location.X, spawnY)
                direction *= -1
            End If

            MyScene.PrepareAdd(New EntFireball(16, 16, pointSpawn, direction, Me, MyScene))
            Me.NumFireballs += 1
        End If
    End Sub

    Public Sub BounceOffEntity(holdingJump As Boolean)
        Me.veloc = New Velocity(Me.veloc.x, 0)
        If holdingJump Then
            Me.AccelerateY(Me.moveSpeed.y * 1.1, True)
        Else
            Me.AccelerateY(Me.moveSpeed.y * 0.8, True)
        End If
    End Sub


    Private defaultY As Integer
    Private deathTimer As Integer
    Private flagCounter As Integer

    Public Overrides Sub Render(g As Graphics)
        MyBase.Render(g)
    End Sub

    Public Overrides Sub Animate()
        If IsInPipe
            Dim slice = animator.GetSlice(pipeElapsedTime / pipeMaxTime)
            RenderImage = slice.Image
            Me.Height = slice.Height
            Me.Width = slice.Width
            Me.Location = slice.location

        ElseIf Not Me.isDead Then
            If Not Me.OnFlag Then
                ' Make sure this is exhaustive
                Dim spriteStateToUse = -2

                ' If crouching
                If IsCrouching Then
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
                        If MyScene.GlobalFrameCount Mod AnimationInterval = 0 Then
                            Me.RenderImage = SpriteSet.SendToBack(spriteStateToUse)
                        End If

                        ' Single frame
                    Case Else
                        Me.RenderImage = SpriteSet(spriteStateToUse)(0)
                End Select

                ' On flag
            Else
                If Me.flagCounter = 0 Then
                    Me.flagCounter = Me.Location.X
                End If
                If Me.Location.Y > (StandardHeight * 3) Then
                    Me.RenderImage = SpriteSet(SpriteState.Climb)(0)
                    Me.Location = New Point(Me.Location.X, Me.Location.Y - 3)
                Else
                    If Me.Location.Y > (2 * StandardHeight) And Me.Location.X - Me.flagCounter >= StandardWidth Then
                        Me.Location = New Point(Me.Location.X, Me.Location.Y - 3)
                    End If
                    If MyScene.GlobalFrameCount Mod AnimationInterval = 0 Then
                        Me.RenderImage = SpriteSet.SendToBack(SpriteState.GroundRight)
                    End If
                    Me.Location = New Point(Me.Location.X + 3, Me.Location.Y)

                    ' He will keep walking forward from this point on

                End If
            End If
        Else
            Me.RenderImage = SpriteSet(SpriteState.Destroy)(0)
            Me.deathTimer += 1
            If deathTimer > 60 Then
                Dim x = (Me.deathTimer - 60) / (AnimationInterval * 5)

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

    Public Sub UpdatePlayerInput
        Dim xToMove = 0
        Dim yToMove = 0

        ' LEFT
        If KeyHandler.MoveLeft Then
            If Not Me.IsCrouching Then
                xToMove = -Me.moveSpeed.x
            End If

            ' Yes this is really badly hard-coded
            If Me.IsCrouching And Not Me.AllowedToUncrouch And KeyHandler.MoveUp And Me.AllowJumpInput Then
                xToMove = -Me.moveSpeed.x
            End If
        End If

        ' RIGHT
        If KeyHandler.MoveRight Then
            If Not Me.IsCrouching Then
                xToMove = Me.moveSpeed.x

            End If
            If Me.IsCrouching And Not Me.AllowedToUncrouch And KeyHandler.MoveUp And Me.AllowJumpInput Then
                xToMove = Me.moveSpeed.x
            End If
        End If

        ' UP
        If KeyHandler.MoveUp And Me.AllowJumpInput Then
            yToMove = Me.moveSpeed.y
            Me.AllowJumpInput = False

        ElseIf KeyHandler.MoveUp = False Then
            Me.AllowJumpInput = True
        End If

        ' DOWN
        If KeyHandler.MoveDown Then
            If Me.State > PlayerStates.Small And Me.isGrounded = True Then 'crouch
                Me.OnCrouch(True)
            End If
        ElseIf Me.State > PlayerStates.Small And Me.IsCrouching = True Then
            ' TODO - check for collision on above blocks before uncrouching
            Me.OnCrouch(False)
        End If

        If Me.State = PlayerStates.Fire And KeyHandler.MoveDown And Me.AllowShoot Then
            Me.TryShootFireball()
            Me.AllowShoot = False
        ElseIf Not KeyHandler.MoveDown Then
            Me.AllowShoot = True
        End If

        If Not Me.isDead Then
            Me.AccelerateX(xToMove)
            Me.AccelerateY(yToMove, False)
        End If

        If Me.IsBouncingOffEntity Then
            Me.BounceOffEntity(KeyHandler.MoveUp)
            Me.IsBouncingOffEntity = False
        End If
    End Sub

    Private animator As MarioAnimator
    Private pipeElapsedTime As Integer = 0
    Private pipeMaxTime As Integer = 0

   
    Public Overrides Sub UpdateVeloc()
        If IsInPipe or MyScene.IsTransitioning
            If pipeElapsedTime >= pipeMaxTime
                IsInPipe = False
                
            Else
                pipeElapsedTime += 1
            End If
            

            Me.veloc.x = 0
            Me.veloc.y = 0
            Return
        End If
        UpdatePlayerInput
        MyBase.UpdateVeloc()

        for each item in Me.currentGroundObjects
            item.CollisionTop(me)
        Next

        Dim outOfMap = IsOutOfMap()
        If (outOfMap = Direction.Right And Me.veloc.x > 0) Or (outOfMap = Direction.Left And Me.veloc.x < 0)
            veloc.x = 0
        End If


        If Me.IsCrouching And Not isGrounded Then
            Me.OnCrouch(False)
        End If

        If invulnerableTime <> 0 Then
            invulnerableTime -= 1
        End If

        If InvinicibilityTimer > 0 Then
            InvinicibilityTimer -= 1
        End If
    End Sub

    Public Overrides Sub UpdateLocation()
        If Not Me.OnFlag  Then
            MyBase.UpdateLocation()
        End If
        If Me.Location.Y < 0 And Not isDead Then
            Me.State = PlayerStates.Dead
        End If
    End Sub

    Public Overloads Sub AddSelfToScene Implements ISceneAddable.AddSelfToScene
        MyScene.AddEntity(Me)
        MyScene.AddUnfreezableItem(Me)
    End Sub

    
End Class


Public Class PlayerPoints
    Public Const Coin = 100
    Public Const Goomba = 500
    Public Const Koopa = 700
    Public Const Mushroom = 2000
    Public Const Firefire = 4000

    Public Const Star = 5000
    Public Const OneUp = 1000
End Class