' ===========================
' Physics 
' ---------------------------

Public Module Forces
    ' may need to tweak
    ' CANNOT exceed moveSpeed values of any entity otherwise it will not be able to move
    Public Const gravity = 0.98
    Public Const friction = 1.5
    Public Const airResist = 1.5
End Module

Public Structure Velocity
    Dim x As Double
    Dim y As Double
    Sub New(x As Double, y As Double)
        Me.x = x
        Me.y = y
    End Sub
End Structure

Public Class Entity
    Inherits RenderObject

    Public veloc = New Velocity(0, 0)
    Public moveSpeed = New Velocity(2, 15)
    Private ReadOnly maxVeloc = New Velocity(10, -15)

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

        ' entity is hitting sender from bottom
        If (selfLowermost + (0.1 * Me.Height)) > blockUppermost And insideFromAbove And insideFromLeft And insideFromRight Then
            isGrounded = True
            lastGroundObject = sender
            Me.Location = New Point(Me.Location.X, blockUppermost)
            'Me.veloc.y = 0
            ' NOTE - LET THE RenderObject being collided with deal with changing Entities' speed
            ' For example, collision with a platform from below != veloc = 0, mario passes through the platform
            ' Moved it into CollisionBottom for RenderObject 
            sender.CollisionBottom(Me)

         
        ' entity is hitting sender From the top (probably stand on it)
        ElseIf Me.veloc.y > 0 And selfCentre.Y < blockLowermost And (selfUppermost + (0.05 * Me.Height)) > blockLowermost Then
            Me.Location = New Point(Me.Location.X, blockLowermost - Me.Height) ' - 0.2 * c ?
            sender.CollisionTop(Me)

            ' WEST

        ElseIf selfCentre.X < blockLeftmost And insideFromLeft And insideFromAbove And insideFromBelow Then
            Me.Location = New Point(blockLeftmost, Me.Location.Y)
            sender.CollisionLeft(Me)

            ' EAST

        ElseIf selfCentre.X > blockRightmost And insideFromRight And insideFromAbove And insideFromBelow Then
            Me.Location = New Point(blockRightmost - Me.Width, Me.Location.Y)
            sender.CollisionRight(Me)

            ' Check for falling off platform

        ElseIf lastGroundObject IsNot Nothing
            if sender = Me.lastGroundObject And Not (insideFromLeft And insideFromRight) Then
                isGrounded = False
            End if
        End If

    End Sub

    Public Sub AccelerateX(magnitude As Double)
        If isGrounded Then
            Me.veloc.x += magnitude
        Else
            Dim reduction = (Me.moveSpeed.x - Forces.airResist) * 0.7
            Me.veloc.x += magnitude - (magnitude / Math.Abs(magnitude)) * reduction
        End If

        If Math.Abs(Me.veloc.x) > Me.maxVeloc.X Then
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
    End Sub

    Public Overrides Property RenderImage As Image

    Sub New(width As Integer, height As Integer, location As Point, spriteSet As SpriteSet)
        MyBase.New(width, height, location)
        Me.RenderImage = Resize(spriteSet.idle, width, height)
        Me.groundAnimation = spriteSet.ground
    End Sub


    Public Sub Move(numFrames As Integer)
        Me.Location = New Point(Me.Location.X + Me.veloc.x, Me.Location.Y + Me.veloc.y)

        ' Animate
        ' new sub for this?

        Dim imageToDraw As Image

        If isGrounded Then

            ' Check direction
            If veloc.x < 0 And isFacingForward Then
                isFacingForward = False

            ElseIf veloc.x > 0 And Not isFacingForward Then
                isFacingForward = True
            End If

            ' Re-animate every 5 frames
            If veloc.x <> 0 And numFrames Mod 5 = 0 Then

                ' Must be cloned, otherwise the resource image itself gets flipped (an unfortunate side effect of classes being passed by reference...)
                imageToDraw = groundAnimation(0).Clone

                ' Cycle through the list, moving the last element to the first
                ' I miss being able to use a pop function
                groundAnimation.Insert(0, groundAnimation.Last)
                groundAnimation.RemoveAt(groundAnimation.Count - 1)

            ElseIf veloc.x = 0 Then
                imageToDraw = My.Resources.mario_small_1

            End If

        Else
            imageToDraw = My.Resources.mario_small_jump
        End If

#Disable Warning BC42104 ' Variable is used before it has been assigned a value
        ' stupid compiler, I'm checking if its null
        If imageToDraw IsNot Nothing Then
            If Not isFacingForward Then
                imageToDraw.RotateFlip(RotateFlipType.RotateNoneFlipX)
            End If
            RenderImage = imageToDraw
        End If
#Enable Warning BC42104 ' Variable is used before it has been assigned a value


    End Sub
End Class

' ===========================
' Entities
' ---------------------------

Public Module Entities
    Public player1 As New Entity(32, 32, New Point(0, 50), Sprites.player)
End Module



