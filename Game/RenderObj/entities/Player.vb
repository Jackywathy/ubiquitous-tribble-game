Imports WinGame

Friend Enum PlayerStates
    Small = 0
    Big = 1
    Fire = 2
    'Ice = 4
End Enum


Public Class Player
    Inherits Entity

    Public Const CoinsToLives = 100

    Public Shared Property Lives = 5
    Public allowJump = True


    Public state As UInt16 = 0

    ' This is set when New() is called
    Public Overrides Property spriteSet As SpriteSet = Nothing

    Public Overrides Property moveSpeed As Velocity = New Velocity(1, 15)
    Public Overrides ReadOnly Property maxVeloc As Velocity = New Velocity(6, -15)

    Private Shared _coins As Integer = 0

    Public Shared Property Coins
        Get
            Return _coins
        End Get
        Set(value)
            If value >= CoinsToLives Then
                _coins = value - CoinsToLives
            Else
                _coins += 1
            End If

        End Set
    End Property

    Public Sub changeState(change As Int16)
        state = change
        Select Case state
            Case 0 : Me.spriteSet = Sprites.playerSmall
                Me.Height = 32
            Case 1 : Me.spriteSet = Sprites.playerBig
                Me.Height = 64
        End Select
    End Sub


    Public Overrides Sub Animate(numFrames As Integer)
        ' Animate
        Dim imageToDraw As Image
        If isGrounded Or (Not isGrounded And Not didJumpAndNotFall) Then
            ' Check direction
            If veloc.x < 0 And isFacingForward Then
                isFacingForward = False
            ElseIf veloc.x > 0 And Not isFacingForward Then
                isFacingForward = True
            End If

            ' Re-animate every 5 frames
            If veloc.x <> 0 And numFrames Mod 5 = 0 Then
                ' Must be cloned, otherwise the resource image itself gets flipped (an unfortunate side effect of classes being passed by reference...)
                imageToDraw = spriteSet.allSprites(0)(0).Clone
                ' Cycle through the list, moving the last element to the first
                ' I miss being able to use a pop function
                spriteSet.allSprites(0).Insert(0, spriteSet.allSprites(0).Last)
                spriteSet.allSprites(0).RemoveAt(spriteSet.allSprites(0).Count - 1)
            ElseIf veloc.x = 0 Then
                imageToDraw = spriteSet.allSprites(1)(0).Clone
            End If
        ElseIf didJumpAndNotFall Then
            imageToDraw = spriteSet.allSprites(2)(0).Clone
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

    Sub New(width As Integer, height As Integer, location As Point, spriteSet As SpriteSet)

        MyBase.New(width, height, location)
        Me.RenderImage = Resize(spriteSet.allSprites(1)(0), width, height)
        Me.spriteSet = spriteSet

    End Sub


End Class
