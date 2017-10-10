Public Interface IScene
    ''' <summary>
    ''' Renders all objects onto a graphics object
    ''' </summary>
    ''' <param name="g"></param>
    Sub RenderObjects(g as Graphics)

    ''' <summary>
    ''' Updates all items in this scene.
    ''' TickElapsed is num. ticks elapsed in this level
    ''' </summary>
    ''' <param name="timeElapsed"></param>
    Sub UpdateTick(timeElapsed As Integer)
End Interface
