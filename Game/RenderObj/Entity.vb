Public Class Entity
    Inherits RenderObject

    Public veloc = New Velocity(0, 0)
    Public moveSpeed = New Velocity(2, 20)
    Private ReadOnly maxVeloc = New Velocity(10, -15)

    Public isGrounded = True
    Public isJumping = False

    Private lastGroundObject As RenderObject

    Public Sub CheckCollision(ByRef obj As RenderObject)

        Dim selfCentre = New Point((Me.Location.X + 0.5 * Me.Width), (Me.Location.Y + 0.5 * Me.Height))

        Dim selfRightmost = Me.Location.X + Me.Width
        Dim selfLeftmost = Me.Location.X
        Dim selfUppermost = Me.Location.Y + Me.Height
        Dim selfLowermost = Me.Location.Y

        Dim blockRightmost = obj.Location.X + obj.Width
        Dim blockLeftmost = obj.Location.X
        Dim blockUppermost = obj.Location.Y + obj.Height
        Dim blockLowermost = obj.Location.Y

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

        '     NORTH

        If (selfLowermost + (0.1 * Me.Height)) > blockUppermost And insideFromAbove And insideFromLeft And insideFromRight Then
            isGrounded = True
            lastGroundObject = obj
            Me.Location = New Point(Me.Location.X, blockUppermost)
            Me.veloc.y = 0

            ' SOUTH

        ElseIf Me.veloc.y > 0 And selfCentre.Y < blockLowermost And (selfUppermost + (0.05 * Me.Height)) > blockLowermost Then
            Me.Location = New Point(Me.Location.X, blockLowermost - Me.Height) ' - 0.2 * c ?
            Me.veloc.y = 0

            ' WEST

        ElseIf selfCentre.X < blockLeftmost And insideFromLeft And insideFromAbove And insideFromBelow Then
            Me.Location = New Point(blockLeftmost, Me.Location.Y)
            Me.veloc.x = 0

            ' EAST

        ElseIf selfCentre.X > blockRightmost And insideFromRight And insideFromAbove And insideFromBelow Then
            Me.Location = New Point(blockRightmost - Me.Width, Me.Location.Y)
            Me.veloc.x = 0

            ' Check for falling off platform

        ElseIf obj.ID = Me.lastGroundObject.ID And Not (insideFromLeft And insideFromRight) Then
            isGrounded = False

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

    Sub New(width As Integer, height As Integer, location As Point, sprite As Image)
        MyBase.New(width, height, location)
        Me.RenderImage = Resize(sprite, width, height)
    End Sub

    Public Sub Move()
        Me.Location = New Point(Me.Location.X + Me.veloc.x, Me.Location.Y + Me.veloc.y)
    End Sub
End Class

Public Structure Velocity
    Dim x As Double
    Dim y As Double
    Sub New(x As Double, y As Double)
        Me.x = x
        Me.y = y
    End Sub
End Structure

Public Module Forces
    ' should tweak
    ' CANNOT exceed self.moveSpeed values otherwise entity will not move
    Public Const gravity = 1.5
    Public Const friction = 1.5
    Public Const airResist = 2.0
End Module