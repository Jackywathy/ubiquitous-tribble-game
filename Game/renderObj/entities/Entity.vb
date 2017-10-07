''' <summary>
''' Entity with physics!
''' </summary>
Public MustInherit Class Entity
    Inherits HitboxItem
    Implements ISceneAddable

    Public veloc As New Velocity(0, 0)

    Public Property SpriteSet As SpriteSet

    Public Property isDead As Boolean = False
    Public killsOnContact As Boolean = False

    Sub New(width As Integer, height As Integer, location As Point, spriteSet As SpriteSet, mapScene As MapScene)
        MyBase.New(width, height, location, spriteSet.GetFirst(SpriteState.ConstantRight), mapScene)
        Me.SpriteSet = spriteSet
    End Sub

    Public Overridable Property moveSpeed As Velocity
    Public Overridable Property maxVeloc As Velocity

    Public willCollideFromAbove As Boolean = False
    Public willCollideFromBelow As Boolean = False
    Public willCollideFromLeft As Boolean = False
    Public willCollideFromRight As Boolean = False

    Public isGrounded As Boolean = True
    Public isJumping As Boolean = False
    Public isFacingForward As Boolean = True
    Public didJumpAndNotFall As Boolean = True

    Public currentGroundObjects As New List(Of HitboxItem)

    ''' <summary>
    ''' Checks for overlap between Me and sender. Handles change of: location of Me, and variables such as isGrounded and didJumpAndNotFall.
    ''' </summary>
    ''' <param name="sender"></param>
    Public Sub CheckPotentialCollision(sender As HitboxItem)
        'collidedX = False

        Dim selfNextPoint = New Point(Me.Location.X + Me.veloc.x, Me.Location.Y + Me.veloc.y)

        Dim selfCentre = New Point((Me.Location.X + (0.5 * Me.CollisionWidth)), (Me.Location.Y + (0.5 * Me.CollisionHeight)))

        Dim blockRightmost = sender.Location.X + sender.CollisionWidth
        Dim blockLeftmost = sender.Location.X
        Dim blockUppermost = sender.Location.Y + sender.CollisionHeight
        Dim blockLowermost = sender.Location.Y

        ' Potential locations
        Dim nextSelfRightmost = selfNextPoint.X + Me.CollisionWidth
        Dim nextSelfLeftmost = selfNextPoint.X
        Dim nextSelfUppermost = selfNextPoint.Y + Me.CollisionHeight
        Dim nextSelfLowermost = selfNextPoint.Y

        ' Current locations
        Dim isAbove = selfCentre.Y >= blockUppermost
        Dim isBelow = selfCentre.Y <= blockLowermost
        Dim isLeftOf = selfCentre.X < blockLeftmost
        Dim isRightOf = selfCentre.X > blockRightmost

        ' Current sub-collisions
        Dim isInsideFromAbove = Me.Location.Y < blockUppermost
        Dim isInsideFromBelow = Me.Location.Y + Me.CollisionHeight > blockLowermost
        Dim isInsideFromLeft = Me.Location.X + Me.CollisionWidth > blockLeftmost
        Dim isInsideFromRight = Me.Location.X < blockRightmost
        ' Used for vertical collisions to add buffer space
        Dim isInsideFromLeft_v = Me.Location.X + Me.CollisionWidth - (5) > blockLeftmost
        Dim isInsideFromRight_v = Me.Location.X + (5) < blockRightmost

        ' Potential sub-collisions
        Dim willInsideFromLeft = nextSelfRightmost >= blockLeftmost
        Dim willInsideFromRight = nextSelfLeftmost <= blockRightmost
        Dim willInsideFromBelow = nextSelfUppermost >= blockLowermost
        Dim willInsideFromAbove = nextSelfLowermost <= blockUppermost
        Dim willInsideFromLeft_v = nextSelfRightmost - (10) >= blockLeftmost
        Dim willInsideFromRight_v = nextSelfLeftmost + (10) <= blockRightmost

        Dim senderImplementsPhysicalCollision = (Not sender.GetType.IsSubclassOf(GetType(Entity))) And (Not sender.GetType.IsSubclassOf(GetType(BlockInvisNone)))

        If sender.GetType.IsSubclassOf(GetType(BlockInvisNone)) Then
            Dim b As BlockInvisNone = sender
            If b.Revealed Then
                senderImplementsPhysicalCollision = True
            End If
        End If

        Dim senderIsEntity = sender.GetType.IsSubclassOf(GetType(Entity)) ' is not entity or subclass thereof
        Dim playerFlagCollision = Me.GetType = GetType(EntPlayer) And (sender.GetType = GetType(Flag) Or sender.GetType = GetType(FlagTop))

        Dim newPositionToMoveTo As Point

        If Me.GetType = GetType(EntFireFlower) Then
            Me.ID += 0
        End If

        ' NORTH
        If Me.veloc.y < 0 And isAbove And willInsideFromAbove And isInsideFromLeft_v And isInsideFromRight_v Then
            If senderImplementsPhysicalCollision Then
                Me.willCollideFromAbove = True
                newPositionToMoveTo = New Point(Me.Location.X, blockUppermost)
            End If

            sender.CollisionTop(Me)

            ' SOUTH 
        ElseIf Me.veloc.y >= 0 And isBelow And willInsideFromBelow And willInsideFromLeft_v And willInsideFromRight_v Then

            If senderImplementsPhysicalCollision Then
                Me.willCollideFromBelow = True
            End If

            If Me.veloc.y = 0 And isInsideFromBelow And senderIsEntity Then

                ' NO entity's collisionBottom() should set veloc.y of sender to 0!
                sender.CollisionBottom(Me)

            ElseIf Me.veloc.y > 0 Then
                ' only triggers with positive veloc; necessary to stop infinite loop since veloc.y is set to 0 after collision

                If Not senderIsEntity Then

                    Me.willCollideFromBelow = True

                    newPositionToMoveTo = New Point(Me.Location.X, blockLowermost - Me.CollisionHeight) ' - 0.2 * c ?

                End If
                sender.CollisionBottom(Me)
            End If

            ' WEST
        ElseIf isLeftOf And willInsideFromLeft And isInsideFromAbove And isInsideFromBelow Then

            If playerFlagCollision Then
                Dim player As EntPlayer = Me
                player.OnFlag = True
                If player.Location.Y > (3 * StandardHeight) Then
                    If sender.GetType = GetType(FlagTop) Then
                        player.Location = New Point(sender.Location.X - player.Width + Flag.FlagWidth, player.Location.Y)
                    Else
                        player.Location = New Point(sender.Location.X - player.Width, player.Location.Y)
                    End If
                End If
            ElseIf senderImplementsPhysicalCollision Then
                newPositionToMoveTo = New Point(blockLeftmost - Me.Width, Me.Location.Y)
                Me.willCollideFromLeft = True
            End If



            sender.CollisionLeft(Me)

            ' EAST
        ElseIf isRightOf And willInsideFromRight And isInsideFromAbove And isInsideFromBelow Then

            If senderImplementsPhysicalCollision Then
                newPositionToMoveTo = New Point(blockRightmost, Me.Location.Y)
                Me.willCollideFromRight = True
            End If



            sender.CollisionRight(Me)

        End If

        ' Handle falling off ledges and setting of didJumpAndNotFall, isGrounded
        ' Also handle location correction due to collision

        ' NOTE: managing ground objects is NOT predictive, so it does NOT use predictive values

        If senderImplementsPhysicalCollision Then

            If Me.Location.Y + Me.CollisionHeight > blockUppermost And Me.Location.Y <= blockUppermost And isInsideFromLeft And isInsideFromRight And Not currentGroundObjects.Contains(sender) Then

                currentGroundObjects.Add(sender)
                Me.isGrounded = True

            End If

            If ((Not (isInsideFromLeft_v And isInsideFromRight_v)) Or Me.Location.Y > blockUppermost) And currentGroundObjects.Contains(sender) Then
                currentGroundObjects.RemoveAt(currentGroundObjects.IndexOf(sender))
                If currentGroundObjects.Count = 0 Then
                    Me.isGrounded = False
                    If Me.veloc.y <= 0 Then
                        didJumpAndNotFall = False
                    End If
                End If
            End If

            If (newPositionToMoveTo <> Nothing) Then
                If Me.Location.X <> -32 Then
                    Me.Location = newPositionToMoveTo
                End If
            End If
        End If

        ' Handle tracking of crouch allowance
        If Me.GetType = GetType(EntPlayer) Then
            Dim player As EntPlayer = Me

            player.AllowedToUncrouch = Not willCollideFromBelow
        End If

    End Sub

    Public Overrides Sub CollisionBottom(sender As Entity)
    End Sub
    Public Overrides Sub CollisionTop(sender As Entity)
    End Sub
    Public Overrides Sub CollisionLeft(sender As Entity)
    End Sub
    Public Overrides Sub CollisionRight(sender As Entity)
    End Sub

    ''' <summary>
    ''' Increases the absolute value of Me.veloc.x by magnitude.
    ''' Will not increase Me.veloc.x past Me.maxVeloc.x
    ''' </summary>

    Public Sub AccelerateX(magnitude As Double)
        If isGrounded Then
            Me.veloc.x += magnitude
        Else

            If ((magnitude < 0) = (Me.veloc.x + magnitude < 0)) Then
                Me.veloc.x += magnitude
            Else
                Dim sign = 1
                If magnitude < 0 Then
                    sign = -1
                End If
                Dim mag = magnitude
                Dim reduction = (Me.moveSpeed.x - Forces.airResist) * Forces.aerialDirectionReversePenalty
                DecreaseMagnitude(mag, reduction)
                Me.veloc.x += mag
            End If
        End If

        If Me.veloc.x <> 0 And Math.Abs(Me.veloc.x) > Me.maxVeloc.x Then
            Dim sign = 1
            If Me.veloc.x < 0 Then
                sign = -1
            End If
            Me.veloc.x = sign * Me.maxVeloc.x
        End If

    End Sub

    ''' <summary>
    ''' Increases the absolute value of Me.veloc.y by magnitude and changes variables of Me appropriately.
    ''' Will not increase Me.veloc.y past Me.maxVeloc.y (terminal velocity).
    ''' </summary>
    Public Sub AccelerateY(magnitude As Double, forceAccelerate As Boolean)
        If magnitude > 0 And (isGrounded Or forceAccelerate) Then
            Me.veloc.y += magnitude
            isJumping = True
            didJumpAndNotFall = True

        ElseIf magnitude < 0 Then
            Me.veloc.y += magnitude
        End If

        If Me.veloc.y < Me.maxVeloc.y Then
            Me.veloc.y = Me.maxVeloc.y
        End If

    End Sub

    ''' <summary>
    ''' Modifies the absolute value of velocity by magnitude.
    ''' </summary>
    Public Sub DecreaseMagnitude(ByRef velocity As Double, ByVal magnitude As Double)

        Dim signBeforeIsNegative As Boolean = velocity < 0

        velocity -= magnitude * If(signBeforeIsNegative, -1, 1)

        Dim signOfVelocAfter As Boolean = velocity < 0

        If signOfVelocAfter <> signBeforeIsNegative Then
            velocity = 0
        End If

        ' Just in case
        'If magnitude > 0 Then
        'If Math.Abs(velocity) < magnitude Then
        'velocity = 0
        'Else
        'velocity -= velocity / Math.Abs(velocity) * magnitude
        'End If
        'End If
    End Sub

    ''' <summary>
    ''' Applies gravity, friction and air resistance
    ''' </summary>
    Public Sub ApplyConstantForces()
        If isGrounded Then
            DecreaseMagnitude(Me.veloc.x, Forces.friction)
        Else
            AccelerateY(-Forces.gravity, False)
            DecreaseMagnitude(Me.veloc.x, Forces.airResist)
        End If

    End Sub

    ''' <summary>
    ''' Returns which direction that the item is out of 
    ''' </summary>
    ''' <returns></returns>
    Public Function IsOutOfMap() As Direction
        If Me.Location.X < 0 Then
            Return Direction.Left
        ElseIf (Me.Location.X - MyScene.ScreenLocation.X) > ScreenGridWidth Then
            Return Direction.Right
        ElseIf (Me.Location.Y < 0) Then
            Return Direction.Bottom
        ElseIf (Me.Location.Y - MyScene.ScreenLocation.Y > ScreenGridHeight) Then
            Return Direction.Top
        End If
        Return Direction.None
    End Function


    Public Overrides Sub UpdateVeloc()

        ' Reset collision variables
        Me.willCollideFromAbove = False
        Me.willCollideFromBelow = False
        Me.willCollideFromLeft = False
        Me.willCollideFromRight = False

        ' stop the player from walking off

        Me.ApplyConstantForces()

        ' Check direction
        If veloc.x < 0 And isFacingForward Then
            isFacingForward = False
        ElseIf veloc.x > 0 And Not isFacingForward Then
            isFacingForward = True
        End If

        ' check collision of all Entities with all existing RenderObj, including other Entities
        For Each other As HitboxItem In MyScene.AllHitboxItems
            ' Don't check collisions using the same obj
            If Me <> other And other.CollisionActive Then
                Me.CheckPotentialCollision(other)

                ' handle block bumping beneath Me
                If Me.currentGroundObjects.Contains(other) And other.GetType.IsSubclassOf(GetType(BlockBumpable)) Then
                    Dim b As BlockBumpable = other
                    If b.IsMoving Then
                        If Me.GetType.IsSubclassOf(GetType(EntEnemy)) Then
                            Dim e As EntEnemy = Me
                            e.willDie = True
                        ElseIf Not (Me.GetType.IsSubclassOf(GetType(EntPowerup)) AndAlso CType(Me, EntPowerup).IsSpawning) Then
                            Me.AccelerateY(Me.moveSpeed.y, True)
                        End If
                    End If
                End If

            End If
        Next

        ' If entity is going to collide, clear veloc so that it never moves INSIDE the object
        If Me.willCollideFromAbove And Me.veloc.y < 0 Then
            Me.veloc.y = 0
        End If
        If Me.willCollideFromBelow And Me.veloc.y > 0 Then
            Me.veloc.y = 0
        End If
        If Me.willCollideFromLeft And Me.veloc.x > 0 Then
            Me.veloc.x = 0
        End If
        If Me.willCollideFromRight And Me.veloc.x < 0 Then
            Me.veloc.x = 0
        End If

    End Sub

    Public Overrides Sub UpdateLocation()
        If Not Me.isDead Then
            Me.Location = New Point(Me.Location.X + Me.veloc.x, Me.Location.Y + Me.veloc.y)
        End If
    End Sub

    ''' <summary>
    ''' Removes entity from its mapScene
    ''' </summary>
    Public Overridable Sub Destroy()
        MyScene.PrepareRemove(Me)
    End Sub

    Public Overloads Sub AddSelfToScene() Implements ISceneAddable.AddSelfToScene
        MyScene.AddEntity(Me)
    End Sub

End Class

Public Module Forces
    ' may need to tweak
    ' CANNOT exceed moveSpeed values of any entity otherwise it will not be able to move
    Public Const gravity = 0.6
    Public Const friction = 0.3
    ' ice map = 0.2
    Public Const airResist = 0.3
    Public Const terminalVeloc = -15.0

    Public Const aerialDirectionReversePenalty = 3.0
End Module

Public Structure Velocity
    Dim x As Double
    Dim y As Double
    Sub New(x As Double, y As Double)
        Me.x = x
        Me.y = y
    End Sub
End Structure

Public Enum Direction
    None = 0
    Top
    Right
    Bottom
    Left
End Enum

