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
    Public Overrides ReadOnly Property maxVeloc As Velocity = New Velocity(8, -15)

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

    Public Overrides Sub UpdatePos(numFrames As Integer)

        ' Move sprite
        Me.Location = New Point(Me.Location.X + Me.veloc.x, Me.Location.Y + Me.veloc.y)

        ' Animate
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
                imageToDraw = spriteSet(0).Clone
                ' Cycle through the list, moving the last element to the first
                ' I miss being able to use a pop function
                spriteSet.Insert(0, spriteSet.Last)
                spriteSet.RemoveAt(spriteSet.Count - 1)
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

    Sub New(width As Integer, height As Integer, location As Point, spriteSet As SpriteSet)

        MyBase.New(width, height, location)
        Me.RenderImage = Resize(spriteSet.allSprites(1)(0), width, height)
        Me.spriteSet = spriteSet.allSprites(0)

    End Sub


End Class
