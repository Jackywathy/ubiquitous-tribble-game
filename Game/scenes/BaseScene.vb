
''' <summary>
''' Base scene:
''' given to GameControl to render 
''' </summary>
Public MustInherit Class BaseScene
    ''' <summary>
    ''' FrameCount for each scene
    ''' </summary>
    Public GlobalFrameCount As Integer

    ''' <summary>
    ''' Run once per tick in game, updates all obejcts in scene, if necessary
    ''' </summary>
    Public MustOverride Sub UpdateTick()

    ''' <summary>
    ''' Renders scene ont a graphics object
    ''' </summary>
    Public Sub RenderScene(g As graphics)
        RenderObjects(g)
        If isTransitioning Then
            DrawTransition(g)
        End if

    End Sub

    Private Sub DrawTransition(g As Graphics)
        Dim progress as Double = transistionTicksElapsed / transistionLength * ScreenGridWidth
        Dim drawnRect As Rectangle
        Select Case transistionDirection
            Case TransitionDirection.Right
                ' top left of rectangle = length of form - progress
                drawnRect.X = ScreenGridWidth - progress
                drawnRect.Y = 0
                drawnRect.Width = progress
                drawnRect.Height = ScreenGridHeight
            Case TransitionDirection.Top
                drawnRect.X = 0
                drawnRect.Y = 0
                drawnRect.Width = ScreenGridWidth
                drawnRect.Height = progress
            Case Else
                throw new Exception("Transistion direction out of range")

        End Select
        g.FillRectangle(transistionBrush, drawnRect)
        transistionTicksElapsed += 1
        If transistionTicksElapsed > transistionLength
            IsTransitioning = False
            transistionTicksElapsed = 0
            transistionLength = 0
        End If
    End Sub

    ''' <summary>
    ''' a transition from one scene to another
    ''' </summary>
    Friend IsTransitioning As Boolean 

    ''' <summary>
    ''' Number of ticks run
    ''' </summary>
    Private transistionTicksElapsed As Integer
    Private transistionLength As Integer
    Private transistionBrush As Brush

    private transistionDirection as TransitionDirection

    Private transitionType As TransitionType

    ''' <summary>
    ''' Start a normal transition animation
    ''' </summary>
    ''' <param name="direction"></param>
    ''' <param name="transitionTime"></param>
    ''' <param name="fillColor">brush used - default black</param>
    Public Sub StartNormalTransition(direction As TransitionDirection, Optional transitionTime As Integer = 30, Optional fillColor As Brush = Nothing)
        If fillColor Is Nothing
            fillcolor =  New SolidBrush(Color.Black)
        End If
        Me.transistionLength = transitionTime
        Me.transistionDirection = direction
        Me.transistionTicksElapsed = 0
        me.transistionBrush = fillcolor
        IsTransitioning = True
    End Sub


    ''' <summary>
    ''' Draws the completed scene onto the graphics object
    ''' </summary>
    ''' <param name="g"></param>
    Public MustOverride Sub RenderObjects(g As Graphics)

    ''' <summary>
    ''' Handles input
    ''' </summary>
    Public MustOVerride Sub HandleInput()

    ''' <summary>
    ''' Has all of keys which are held down etc.
    ''' </summary>
    Friend Parent As Control

    ''' <summary>
    ''' holds the background music
    ''' </summary>
    Public Overridable Property BackgroundMusic As MusicPlayer

    Sub New(parent As Control)
        Me.parent = parent
    End Sub

    Public Overridable Function GetPlayers() As IList(Of EntPlayer)
        Return Nothing
    End Function


    ''' <summary>
    ''' If the scene is frozen
    ''' </summary>
    ''' <returns></returns>
    Public Property IsFrozen As Boolean = False

    ' There are 4 types of GameObjects
    ' Static - stuff that doesnt move ever, e.g. HUD elements, points
    ' Moving - stuff that moves, but doesnt have collisions
    ' Hitbox - stuff that has a hitbox
    ' Entity - stuff that ALWAYS moves

    ''' <summary>
    ''' List of staticitems. Items will be rendered in order inserted
    ''' </summary>
    Public Readonly Property AllStaticItems As New List(Of GameItem)

    ''' <summary>
    ''' Adds a static object
    ''' </summary>
    ''' <param name="args"></param>
    Public Overridable Sub AddStatic(ByVal ParamArray args() As GameItem)
        For Each item As GameItem In args
            AllStaticItems.Add(item)
        Next
    End Sub

    Public Overridable Sub DrawDebugStrings(form As GameControl)

    End Sub
End Class