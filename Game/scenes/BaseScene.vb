Imports System.Drawing.Drawing2D
Imports WinGame

''' <summary>
''' Base scene:
''' given to GameControl to render 
''' </summary>
Public MustInherit Class BaseScene
    ''' <summary>
    ''' FrameCount for each scene
    ''' </summary>
    Public Shared GlobalFrameCount As Integer

    ''' <summary>
    ''' Run once per tick in game, updates all obejcts in scene, if necessary
    ''' </summary>
    Public MustOverride Sub UpdateTick(ticksElapsed As Integer)

    ''' <summary>
    ''' Renders scene ont a graphics object
    ''' </summary>
    Public Sub RenderScene(g As Graphics)
        RenderObjects(g)
        If IsTransitioning Then
            If CurrentTransition.IsFinished
                IsTransitioning = False
                CurrentTransition = Nothing
            Else
                CurrentTransition.RenderObject(g)
            End If
        End If
    End Sub

    Public Overridable Sub SetTime(seconds As Integer)

    End Sub

    Protected MustInherit Class TransitionDrawer
        Protected MustOverride Sub DrawTransition(g As Graphics)
        Public tranitionObject As TransitionObject
        Public TransitionLength As Integer
        Public TransitionTicksElapsed As Integer
        Public IsFinished As Boolean = False


        Sub New(transitionObject As TransitionObject)
            Me.tranitionObject = tranitionObject
            TransitionLength = transitionObject.time
            TransitionTicksElapsed = 0
        End Sub

        Public Sub RenderObject(g As Graphics)
            DrawTransition(g)
            If TransitionTicksElapsed > TransitionLength
                IsFinished = True
            End If

            TransitionTicksElapsed += 1
        End Sub
    End Class


    Protected Class FillTransitionDrawer
        Inherits TransitionDrawer
        Public color As Brush

        Protected Overrides Sub DrawTransition(g As Graphics)
            Dim progress As Double = TransitionTicksElapsed / TransitionLength
            Dim drawnRect As Rectangle
            Select Case Me.tranitionObject.tdir
                Case TransitionDirection.Right
                    ' top left of rectangle = length of form - progress
                    drawnRect.X = ScreenGridWidth - progress
                    drawnRect.Y = 0
                    drawnRect.Width = progress * ScreenGridWidth
                    drawnRect.Height = ScreenGridHeight
                Case TransitionDirection.Top
                    drawnRect.X = 0
                    drawnRect.Y = 0
                    drawnRect.Width = ScreenGridWidth
                    drawnRect.Height = progress * ScreenGridHeight
                Case Else
                    Throw New Exception("Transistion direction out of range")

            End Select
            g.FillRectangle(color, drawnRect)
        End Sub

        Sub New(transitionObject As TransitionObject)
            MyBase.New(transitionObject)
            Me.color = If(transitionObject.color, DrawingPrimitives.BlackBrush)
        End Sub
    End Class

    Protected Class StartSceneDrawer
        Inherits TransitionDrawer
        Private scene As mapscene
        Private worldMiddle as StaticText

        Protected Overrides Sub DrawTransition(g As Graphics)
            g.FillRectangle(DrawingPrimitives.BlackBrush, 0, 0, ScreenGridWidth, ScreenGridHeight)
            scene.HudElements.DisableTime()
            scene.HudElements.Render(g)
            scene.HudElements.EnableTime()
            worldMiddle.Render(g)
        End Sub

        Sub New(transitionObject As TransitionObject, scene As MapScene)
            MyBase.New(transitionObject)
            Me.scene = scene
            Refresh
        End Sub

        Public Sub Refresh
            worldMiddle = New StaticText(New Rectangle(ScreenGridWidth / 3, ScreenGridHeight / 3 * 2, ScreenGridWidth / 3, 50), "WORLD" + scene.HudElements.worldNumText.text,
                                         NES.GetFontFamily(), scene.HudElements.fontSize, DrawingPrimitives.WhiteBrush)

        End Sub
    End Class

    Protected Class CircleTransitionDrawer
        Inherits TransitionDrawer
        Public backgroundBrush As Brush
        Private location As Point


        Protected Overrides Sub DrawTransition(g As Graphics)
            Dim progress As Double = TransitionTicksElapsed / TransitionLength

            Dim circleDiameter As Integer = CInt(Math.Min(ScreenGridHeight, ScreenGridWidth)) * (1 - progress)
            Dim circleRadius As Integer = CInt(circleDiameter / 2)

            Dim fillRegion As New Region(New Rectangle(0, 0, ScreenGridWidth, ScreenGridHeight))
            Dim circlePath As New GraphicsPath()
            circlePath.AddEllipse(location.X - circleRadius, location.Y - circleRadius, circleDiameter, circleDiameter)
            fillRegion.Exclude(circlePath)
            g.FillRegion(backgroundBrush, fillRegion)
        End Sub


        Sub New(transitionObject As TransitionObject)
            MyBase.New(transitionObject)
            Me.backgroundBrush = If(transitionObject.color, DrawingPrimitives.BlackBrush)
            Me.location = transitionObject.location
        End Sub
    End Class


    ''' <summary>
    ''' a transition from one scene to another
    ''' </summary>
    Friend IsTransitioning As Boolean
    Protected CurrentTransition As TransitionDrawer

    ''' <summary>
    ''' Start a normal transition animation
    ''' </summary>
    Private Sub StartFillTransition(transition As TransitionObject)
        CurrentTransition = New FillTransitionDrawer(transition)
        IsTransitioning = True
    End Sub

    Public Sub StartTransition(transition As TransitionObject)
        Select Case transition.ttype
            Case TransitionType.Fill
                StartFillTransition(transition)
            Case TransitionType.Circle
                StartCircleTransition(transition)
            Case TransitionType.StartScene
                StartStartSceneTransition(transition)
            Case Else
                Throw New Exception("must be circle or fill")
        End Select

    End Sub

    Public Sub StartStartSceneTransition(transition As TransitionObject)
        CurrentTransition = New StartSceneDrawer(transition, me)
        IsTransitioning = True
    End Sub

    Private Sub StartCircleTransition(transition As TransitionObject)
        CurrentTransition = New CircleTransitionDrawer(transition)
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
    Public MustOverride Sub HandleInput()

    ''' <summary>
    ''' Has all of keys which are held down etc.
    ''' </summary>
    Friend Parent As GameControl

    ''' <summary>
    ''' holds the background music
    ''' </summary>
    Public Overridable Property BackgroundMusic As MusicPlayer

    Sub New(parent As GameControl)
        Me.Parent = parent
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
    Public ReadOnly Property AllStaticItems As New List(Of GameItem)
    Public Property DefaultLocation As Point

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

Public Class TransitionObject
    Public ttype As TransitionType
    Public tdir As TransitionDirection
    Public time As Integer
    Public color As Brush
    Public location As Point

    Sub New(ttype As TransitionType, tdir As TransitionDirection, Optional time As Integer = StandardTransitionTime, Optional fillColor As Brush = Nothing, Optional location As Point? = Nothing)
        Me.ttype = ttype
        Me.tdir = tdir
        Me.time = time
        Me.color = fillColor
        Me.location = If(location, New Point(0, 0))
    End Sub

End Class

Public Enum TransitionDirection
    Top
    Bottom
    Right
    Left
    Random
End Enum

Public Enum TransitionType
    Fill
    Circle
    StartScene
End Enum


Public Enum SwitchLevelType
    Normal
    Secret1
    Secret2
    Pipe
End Enum
