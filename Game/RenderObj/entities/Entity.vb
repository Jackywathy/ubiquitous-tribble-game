' ===========================
' Physics 
' ---------------------------

Public Module Forces
    ' may need to tweak
    ' CANNOT exceed moveSpeed values of any entity otherwise it will not be able to move
    Public Const gravity = 0.98
    Public Const friction = 0.5
    Public Const airResist = 0.5
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
    '8, -15
    Public isGrounded = True
    Public isJumping = False
    Public isFacingForward = True

    ' list of frames to use in animating ground movement
    Public groundAnimation As List(Of Image)

    Private lastGroundObject As RenderObject

    Public Sub CheckCollision(ByRef sender As RenderObject)

        Dim selfCentre = New Point((Me.Location.X + 0.5 * Me.Width), (Me.Location.Y + 0.5 * Me.Height))

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

        If selfLowermost = 50 And player1.veloc.y < 0 Then
            selfLowermost += 0
        End If


        ' NORTH
        If Me.veloc.y < 0 And selfCentre.Y > blockUppermost And insideFromAbove And insideFromLeft And insideFromRight Then
            isGrounded = True
            lastGroundObject = sender
            Me.Location = New Point(Me.Location.X, blockUppermost)
            'Me.veloc.y = 0
            ' NOTE - LET THE RenderObject being collided with deal with changing Entities' speed
            ' For example, collision with a platform from below != veloc = 0, mario passes through the platform
            ' Moved it into CollisionBottom for RenderObject 
            sender.CollisionTop(Me)


            ' entity is underneath block, on BOTTOM of block
            'SOUTH
        ElseIf Me.veloc.y > 0 And selfCentre.Y < blockLowermost And (selfUppermost + (0.05 * Me.Height)) > blockLowermost And insideFromLeft And insideFromRight Then
            Me.Location = New Point(Me.Location.X, blockLowermost - Me.Height) ' - 0.2 * c ?
            sender.CollisionBottom(Me)

            ' WEST

        ElseIf selfCentre.X < blockLeftmost And insideFromLeft And insideFromAbove And insideFromBelow Then
            Me.Location = New Point(blockLeftmost - Me.Width, Me.Location.Y)
            sender.CollisionLeft(Me)

            ' EAST

        ElseIf selfCentre.X > blockRightmost And insideFromRight And insideFromAbove And insideFromBelow Then
            Me.Location = New Point(blockRightmost, Me.Location.Y)
            sender.CollisionRight(Me)

            ' Check for falling off platform

        ElseIf lastGroundObject IsNot Nothing Then

            If sender = Me.lastGroundObject And Not (insideFromLeft And insideFromRight) Then
                isGrounded = False
            End If
        End If

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
            isGrounded = False
        ElseIf magnitude < 0 Then
            Me.veloc.y += magnitude
        End If

        If Me.veloc.y < Me.maxVeloc.y Then
            Me.veloc.y = Me.maxVeloc.y
        End If

    End Sub

    Public Sub DecreaseMagnitude(ByRef velocity As Double, magnitude As Double)
        If magnitude > 0 Then
            If Math.Abs(velocity) < magnitude Then
                velocity = 0
            Else
                velocity -= velocity / Math.Abs(velocity) * magnitude
            End If
        End If
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


    Public Overridable Sub Move(numFrames As Integer)
        Me.Location = New Point(Me.Location.X + Me.veloc.x, Me.Location.Y + Me.veloc.y)
    End Sub

End Class

' ===========================
' Entities
' ---------------------------

Public Module Entities
    Public player1 As New Player(32, 32, New Point(0, 50), Sprites.player)
End Module



