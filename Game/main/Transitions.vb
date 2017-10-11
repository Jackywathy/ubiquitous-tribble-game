Public MustInherit Class QueueObject
    Friend time As integer
    Friend control as GameControl
    Friend [next] As QueueObject
    Friend HasRun as Boolean 
    Public Property IsFinished As Boolean

    Public Sub New(time As Integer, control As GameControl, Optional [next] as QueueObject = Nothing)
        Me.time = time
        Me.control = control
        Me.[next] = [next]
        HasRun = False
    End Sub

    Public Overridable Sub Setup()

    End Sub

    Public Overridable Sub Tick()

    End Sub


    Public Sub UpdateTick()
        If Not hasRun
            HasRun = True
            Setup()
        End If
        if time = 0
            TimerFinished()
            IsFinished = True
            if [next] IsNot nothing
                control.AddTimedEvent([next])
                [next].UpdateTick()
            End If
        Else
            Tick()
        End If
        time -= 1
    End Sub
    Protected MustOverride Sub TimerFinished()
End Class
 
Public Class TransitionQueueObject
     Inherits QueueObject
     Friend transition as TransitionObject

    Sub New(transition As TransitionObject, time As Integer, control As GameControl, Optional [next] As QueueObject = Nothing)
        MyBase.New(time, control, [next])
        Me.transition = transition
    End Sub

    Protected Overrides Sub TimerFinished()
         control.GetCurrentScene().StartTransition(transition)
     End Sub

     Public Overrides Sub Setup()
         MyBase.Setup()
     End Sub
 End Class

Public Class MapChangeQueueObject
    Inherits QueueObject
    Friend map as MapEnum
    Private insertion as point?
    Private isNewStage as Boolean
    Private centerToplayer As Boolean
    Private reset As Boolean


    Sub New(map As MapEnum, insertion As Point?, time As Integer, control As GameControl, Optional [next] As QueueObject = Nothing, Optional IsNewStage As Boolean = False, Optional CenterToPlayer As Boolean = True, Optional reset As Boolean = False)
        MyBase.New(time, control, [next])
        Me.map = map
        Me.insertion = insertion
        Me.isNewStage = IsNewStage
        Me.centerToplayer = CenterToPlayer
        Me.reset = reset
    End Sub

    Protected Overrides Sub TimerFinished()
        If reset Then
            control.GetCurrentScene().ReloadMap()
        End If
        control.RunScene(map, isNewStage, insertion)
        If centerToplayer Then
            control.GetCurrentScene().CenterToPlayer()
        End If


    End Sub
End Class

Public Class MarioPipeAnimationQueueObject
    Inherits QueueObject
    public player as EntPlayer
    public direction as PipeType
    public goingin as boolean
    public stopmusic as boolean
    Sub New(player As EntPlayer, direction As PipeType, goingIn as Boolean, control As GameControl, Optional [next] as QueueObject = Nothing, Optional time As integer = standardPipeTime, optional stopmusic as boolean = True)
        MyBase.New(time, control, [next])
        Me.player = player
        Me.direction = direction
        me.goingin = goingIn
        Me.stopmusic = stopmusic
    End Sub

    Public Overrides Sub Setup()
        Select Case direction
            Case PipeType.Vertical
                player.BeginVerticalPipe(goingIn, time)
            Case PipeType.Horizontal
                player.BeginHorizontalPipe(goingIn, time)
            case Else
                throw new Exception()
                
           
        End Select
        if stopmusic
            MusicPlayer.BackgroundPlayer.Stop()
        End if
        Sounds.Warp.Play()   
    End Sub

    Protected Overrides Sub TimerFinished()

    End Sub
End Class

Public Class WaitQueueObject
    Inherits QueueObject
    Private player As EntPlayer
 
    Sub New(player As EntPlayer, control As GameControl, Optional [next] As QueueObject = Nothing, Optional time As Integer = StandardPipeTime)
        MyBase.New(time, control, [next])
        Me.player = player
    End Sub

    Public Overrides Sub Setup()

    End Sub

    Protected Overrides Sub TimerFinished()
        player.IsFrozen = True
    End Sub

End Class

Public Class ChangeQueueWrapper
    Friend queue As New HashSet(Of QueueObject)


    Public Sub Add(item As QueueObject)
        queue.Add(item)
    End Sub

    Public Sub Remove(item As QueueObject)
        If Not queue.Remove(item) Then
            Throw New Exception("cannot remove item")
        End If
    End Sub

    Public Sub UpdateTick()
        For c = queue.Count - 1 To 0 Step -1
            Dim item = queue(c)
            item.UpdateTick()
            If item.IsFinished Then
                Remove(item)
            End If
        Next
    End Sub

    Public ReadOnly Property IsEmpty As Boolean
        Get
            Return queue.Count = 0
        End Get
    End Property
End Class



