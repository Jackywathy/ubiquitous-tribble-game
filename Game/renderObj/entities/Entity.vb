' ===========================
' Physics 
' ---------------------------

Public MustInherit Class Entity
    Inherits RenderObject
    Public veloc As New Distance(0, 0)

    Public Property SpriteSet As SpriteSet
    Public Overrides Property renderImage As Image
    Public isDead As Boolean = False
    Public killsOnContact As Boolean = False

    Sub New(width As Integer, height As Integer, location As Point, spriteSet As SpriteSet, mapScene As MapScene)
        MyBase.New(width, height, location, mapScene)
        Me.SpriteSet = spriteSet
        Me.renderImage = Me.SpriteSet(SpriteState.ConstantRight)(0)
    End Sub

    ' all vincent
    Public Overridable Property moveSpeed As Distance
    Public Overridable Property maxVeloc As Distance

    Public willCollideFromAbove = False
    Public willCollideFromBelow = False
    Public willCollideFromLeft = False
    Public willCollideFromRight = False

    Public isGrounded = True
    Public isJumping = False
    Public isFacingForward = True
    Public didJumpAndNotFall = True

    Public currentGroundObjects As New List(Of RenderObject)

    ''' <summary>
    ''' Checks for overlap between Me and sender. Handles change of: location of Me, and variables such as isGrounded and didJumpAndNotFall.
    ''' </summary>
    ''' <param name="sender"></param>

    Public Sub CheckPotentialCollision(sender As RenderObject)
        'collidedX = False

        Dim selfNextPoint = New Point(Me.Location.X + Me.veloc.x, Me.Location.Y + Me.veloc.y)

        Dim selfCentre = New Point((Me.Location.X + (0.5 * Me.Width)), (Me.Location.Y + (0.5 * Me.CollisionHeight)))

        Dim blockRightmost = sender.Location.X + sender.Width
        Dim blockLeftmost = sender.Location.X
        Dim blockUppermost = sender.Location.Y + sender.CollisionHeight
        Dim blockLowermost = sender.Location.Y

        ' Potential locations
        Dim nextSelfRightmost = selfNextPoint.X + Me.Width
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
        Dim isInsideFromLeft = Me.Location.X + Me.Width > blockLeftmost
        Dim isInsideFromRight = Me.Location.X < blockRightmost
        ' Used for vertical collisions to add buffer space
        Dim isInsideFromLeft_v = Me.Location.X + Me.Width - (5) > blockLeftmost
        Dim isInsideFromRight_v = Me.Location.X + (5) < blockRightmost

        ' Potential sub-collisions
        Dim willInsideFromLeft = nextSelfRightmost >= blockLeftmost
        Dim willInsideFromRight = nextSelfLeftmost <= blockRightmost
        Dim willInsideFromBelow = nextSelfUppermost >= blockLowermost
        Dim willInsideFromAbove = nextSelfLowermost <= blockUppermost
        Dim willInsideFromLeft_v = nextSelfRightmost - (10) >= blockLeftmost
        Dim willInsideFromRight_v = nextSelfLeftmost + (10) <= blockRightmost

        Dim senderIsEntity = sender.GetType.IsSubclassOf(GetType(Entity)) ' is not entity or subclass

        Dim newPositionToMoveTo As Point

        ' NORTH
        If Me.veloc.y < 0 And isAbove And willInsideFromAbove And isInsideFromLeft_v And isInsideFromRight_v Then
            If Not senderIsEntity Then
                Me.willCollideFromAbove = True
                newPositionToMoveTo = New Point(Me.Location.X, blockUppermost)
            End If

            sender.CollisionTop(Me)

            ' SOUTH 
        ElseIf Me.veloc.y >= 0 And isBelow And willInsideFromBelow And willInsideFromLeft_v And willInsideFromRight_v Then

            If Not senderIsEntity Then
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

                    ' check for another collision; allows collision with several objects at once
                    'For objCount = 0 To (Me.MyScene.AllObjects.Count - 1)
                    'If Me.MyScene.AllObjects(objCount) <> sender And Me.MyScene.AllObjects(objCount) IsNot Nothing Then
                    'Dim otherBlock As RenderObject = Me.MyScene.AllObjects(objCount)
                    'If Me.veloc.y > 0 And Me.Location.Y + Me.Height <= otherBlock.Location.Y And (selfUppermost + (0.05 * Me.Height)) > otherBlock.Location.Y And selfRightmost - (10) > otherBlock.Location.X And selfLeftmost + (10) < otherBlock.Location.X + otherBlock.Width Then
                    'Me.MyScene.AllObjects(objCount).CollisionBottom(Me)
                    'End If
                    'End If
                    '   Next

                End If
                sender.CollisionBottom(Me)
            End If

            ' WEST
        ElseIf isLeftOf And willInsideFromLeft And isInsideFromAbove And isInsideFromBelow Then

            If Not senderIsEntity Then
                newPositionToMoveTo = New Point(blockLeftmost - Me.Width, Me.Location.Y)
                Me.willCollideFromLeft = True
            End If

            sender.CollisionLeft(Me)

            ' EAST
        ElseIf isRightOf And willInsideFromRight And isInsideFromAbove And isInsideFromBelow Then

            If Not senderIsEntity Then
                newPositionToMoveTo = New Point(blockRightmost, Me.Location.Y)
                Me.willCollideFromRight = True
            End If

            sender.CollisionRight(Me)

        End If

        ' Handle falling off ledges and setting of didJumpAndNotFall, isGrounded
        ' Also handle location correction due to collision

        ' NOTE: managing ground objects is NOT predictive, so it does NOT use predictive values

        If Not senderIsEntity Then

            If Me.Location.Y + Me.CollisionHeight > blockUppermost And Me.Location.Y <= blockUppermost And isInsideFromLeft And isInsideFromRight And Not currentGroundObjects.Contains(sender) Then

                currentGroundObjects.Add(sender)
                Me.isGrounded = True
                'Console.WriteLine("Added")
                'Console.WriteLine(currentGroundObjects.Count)

            End If

            If ((Not (isInsideFromLeft_v And isInsideFromRight_v)) Or Me.Location.Y > blockUppermost) And currentGroundObjects.Contains(sender) Then
                currentGroundObjects.RemoveAt(currentGroundObjects.IndexOf(sender))
                'Console.WriteLine("Removed")
                'Console.WriteLine(currentGroundObjects.Count)
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

            player.allowedToUncrouch = Not willCollideFromBelow
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
            Dim reduction = (Me.moveSpeed.x - Forces.airResist) * 0.7
            Dim sign = 1
            If magnitude < 0 Then
                sign = -1
            End If
            Me.veloc.x += magnitude - (sign * reduction)
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


    Public Function IsOutOfMap() As Direction
        If Me.Location.X < 0 Then
            Return Direction.Left
        ElseIf (Me.Location.X - MyScene.screenLocation.X + Me.veloc.X) > ScreenGridWidth Then
            Return Direction.Right
        End If
        Return Direction.None
    End Function

    ''' <summary>
    ''' Updates the position of this entity, using its velocity and location.
    ''' </summary>
    Public Overridable Sub UpdatePos()

        If Not Me.isDead Then

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

            ' check collision of all entities with all existing RenderObj, including other entities
            'For entityCount = 0 To (Me.MyScene.AllEntities.Count - 1)
            For Each other As RenderObject In MyScene.AllObjAndEnt

                ' Don't check collisions using the same obj
                ' and ensure entities are valid
                If other IsNot Nothing And Me <> other Then
                    Me.CheckPotentialCollision(other)
                    'If Me.nextMoveLocation <> Nothing Then
                    'Me.Location = Me.nextMoveLocation
                    'End If

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

        End If

        ' Update location
        Me.Location = New Point(Me.Location.X + Me.veloc.x, Me.Location.Y + Me.veloc.y)

    End Sub

    ''' <summary>
    ''' Removes entity from its mapScene
    ''' </summary>
    Public Overridable Sub Destroy()
        MyScene.PrepareRemove(Me)
    End Sub

End Class

Public Module Forces
    ' may need to tweak
    ' CANNOT exceed moveSpeed values of any entity otherwise it will not be able to move
    Public Const gravity = 0.8
    Public Const friction = 0.3
    ' ice map = 0.2
    Public Const airResist = 0.5
    Public Const terminalVeloc = -15.0
End Module

Public Structure Distance
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