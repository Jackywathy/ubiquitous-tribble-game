' ===========================
' Physics 
' ---------------------------

Imports WinGame

Public Module Forces
    ' may need to tweak
    ' CANNOT exceed moveSpeed values of any entity otherwise it will not be able to move
    Public Const gravity = 0.98
    Public Const friction = 0.4
    Public Const airResist = 0.5
    Public Const terminalVeloc = -15.0
End Module

Public Structure Velocity
    Dim x As Double
    Dim y As Double
    Sub New(x As Double, y As Double)
        Me.x = x
        Me.y = y
    End Sub
End Structure

Public MustInherit Class Entity

    Inherits RenderObject
    Public veloc = New Velocity(0, 0)

    Public MustOverride Property moveSpeed As Velocity
    Public MustOverride ReadOnly Property maxVeloc As Velocity

    Public isGrounded = True
    Public isJumping = False
    Public isFacingForward = True
    Public didJumpAndNotFall = True

    ' animation sprites
    Public MustOverride Property spriteSet As SpriteSet

    Public currentGroundObjects As New List(Of RenderObject)

    Public Sub CheckCollision(ByRef sender As RenderObject)

        Dim selfCentre = New Point((Me.Location.X + (0.5 * Me.Width)), (Me.Location.Y + (0.5 * Me.Height)))

        Dim selfRightmost = Me.Location.X + Me.Width
        Dim selfLeftmost = Me.Location.X
        Dim selfUppermost = Me.Location.Y + Me.Height
        Dim selfLowermost = Me.Location.Y

        Dim blockRightmost = sender.Location.X + sender.Width
        Dim blockLeftmost = sender.Location.X
        Dim blockUppermost = sender.Location.Y + sender.Height
        Dim blockLowermost = sender.Location.Y

        Dim insideFromLeft = selfRightmost > blockLeftmost
        Dim insideFromRight = selfLeftmost < blockRightmost
        Dim insideFromBelow = selfUppermost > blockLowermost
        Dim insideFromAbove = selfLowermost < blockUppermost

        Dim senderIsEntity = sender.GetType.IsSubclassOf(GetType(Entity)) ' is not entity or subclass
        Dim newPositionToMoveTo As Point

        ' Me.Location:
        '  ___
        ' |   |
        ' |___|
        ' X <------ from here
        '

        '     

        ' entity is above the block, on TOP of block
        ' if the lowest part + a bit > highest bit of block
        ' AND    entity intersects inside from top
        ' AND    entity intersect inside from left
        ' AND    entity intersects inside from right

        ' COLLISION FROM:

        ' NORTH
        If Me.veloc.y < 0 And selfUppermost > blockUppermost And insideFromAbove And insideFromLeft And insideFromRight Then
            If Not senderIsEntity Then
                Me.isGrounded = True
            End If

            newPositionToMoveTo = New Point(Me.Location.X, blockUppermost)

            ' NOTE - LET THE RenderObject being collided with deal with changing Entities' speed
            ' Me.veloc.y = 0

            ' For example, collision with a platform from below != veloc = 0, mario passes through the platform
            ' Moved it into CollisionBottom for RenderObject 
            sender.CollisionTop(Me)

            ' SOUTH
        ElseIf selfCentre.Y < blockLowermost And (selfUppermost + (0.05 * Me.Height)) > blockLowermost And insideFromLeft And insideFromRight Then

            If Me.veloc.y = 0 And sender.GetType.IsSubclassOf(GetType(Entity)) Then

                ' NO entity's collisionBottom() should set veloc.y of sender to 0!
                sender.CollisionBottom(Me)

            ElseIf Me.veloc.y > 0 Then
                ' only triggers with positive veloc; necessary to stop infinite loop since veloc.y is set to 0 after collision
                newPositionToMoveTo = New Point(Me.Location.X, blockLowermost - Me.Height) ' - 0.2 * c ?
                sender.CollisionBottom(Me)

            End If

            ' WEST

        ElseIf selfCentre.X < blockLeftmost And insideFromLeft And insideFromAbove And insideFromBelow Then
            newPositionToMoveTo = New Point(blockLeftmost - Me.Width, Me.Location.Y)
            sender.CollisionLeft(Me)

            ' EAST

        ElseIf selfCentre.X > blockRightmost And insideFromRight And insideFromAbove And insideFromBelow Then
            newPositionToMoveTo = New Point(blockRightmost, Me.Location.Y)
            sender.CollisionRight(Me)

            ' Check for falling off platform

        End If


        'Just in case

        'If lastGroundObject IsNot Nothing Then
        'If sender = Me.lastGroundObject And isGrounded And Not (insideFromLeft And insideFromRight) Then
        'isGrounded = False
        'didJumpAndNotFall = False
        'End If
        'End If

        ' Handle falling off ledges and setting of didJumpAndNotFall
        If Not senderIsEntity Then
            If selfUppermost > blockUppermost And selfLowermost <= blockUppermost And insideFromLeft And insideFromRight And Not currentGroundObjects.Contains(sender) Then
            currentGroundObjects.Add(sender)

            'Console.WriteLine("Added")
            'Console.WriteLine(currentGroundObjects.Count)

        End If



        If ((Not (insideFromLeft And insideFromRight)) Or selfLowermost > blockUppermost) And currentGroundObjects.Contains(sender) Then
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


            If Not senderIsEntity And (newPositionToMoveTo <> Nothing) Then
                If Me.Location.X <> -32 Then
                    Me.Location = newPositionToMoveTo

                End If
            End If
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

    Public Sub AccelerateX(magnitude As Double)
        If isGrounded Then
            Me.veloc.x += magnitude
        Else
            Dim reduction = (Me.moveSpeed.x - Forces.airResist) * 0.7
            Me.veloc.x += magnitude - (magnitude / Math.Abs(magnitude)) * reduction
        End If

        If Math.Abs(Me.veloc.x) > Me.maxVeloc.x Then
            Me.veloc.x = (Me.veloc.x / Math.Abs(Me.veloc.x)) * Me.maxVeloc.x
        End If
    End Sub

    Public Sub AccelerateY(magnitude As Double)
        If magnitude > 0 And isGrounded Then
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

    Public Sub DecreaseMagnitude(ByRef velocity As Double, magnitude As Double)

        Dim signOfVelocBefore = velocity / Math.Abs(velocity)

        velocity -= signOfVelocBefore * magnitude

        Dim signOfVelocAfter = velocity / Math.Abs(velocity)

        If signOfVelocAfter <> signOfVelocBefore Then
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

    Public Sub ApplyConstantForces()
        If isGrounded Then
            DecreaseMagnitude(Me.veloc.x, Forces.friction)
        Else
            AccelerateY(-Forces.gravity)
            DecreaseMagnitude(Me.veloc.x, Forces.airResist)
        End If
        If Not player1.isGrounded And player1.veloc.y < 0 Then
            player1.veloc.y += 0
        End If
    End Sub

    Public Overrides Property RenderImage As Image

    Sub New(width As Integer, height As Integer, location As Point)
        MyBase.New(width, height, location)
    End Sub

    ' Handles movement
    Public Overridable Sub UpdatePos()

        If Me.Location.X + Me.veloc.X < 0 Then
            Me.veloc.X = 0
        ElseIf (Me.Location.X - screenLocation.X + Me.veloc.X) > ScreenGridWidth Then
            Me.veloc.X = 0
        End If

        Me.Location = New Point(Me.Location.X + Me.veloc.x, Me.Location.Y + Me.veloc.y)
    End Sub

    Public Overridable Sub Destroy()
        MainGame.SceneController.RemoveEntity(Me)
    End Sub

End Class

' ===========================
' Entities
' ---------------------------

Public Module Entities
    Public player1 As New EntPlayer(32, 32, New Point(0, GroundHeight), Sprites.playerSmall)
End Module



