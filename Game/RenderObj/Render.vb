''' <summary>
''' items that are simply added onto the screen - no hitboxes
''' </summary>
Public MustInherit Class RenderItem
    Public MustOverride Sub Render(g As Graphics)

End Class

''' <summary>
''' Items that have hitboxes
''' </summary>
Public MustInherit Class RenderObject
    Inherits RenderItem
    Public Property Width As Integer
    Public Property Height As Integer

    Public Property CollisionHeight As Integer

    Friend Shared toolBarOffSet As Integer = 29

    Friend Property MyScene As Scene
    Private Shared _idCount As Integer
    
    Private Shared Function GetNewID() As Integer
        Dim temp = _idCount
        _idCount += 1

        Return temp
    End Function

    Public Property ID As Integer = GetNewID()

    Public internalFrameCounter = 0
    Public Const animationInterval As Integer = 5 ' Frames to wait before proceeding to next image of animation

    Public MustOverride Property RenderImage As Image

    ''' <summary>
    ''' Location of object from the very bottom left (0,0)
    ''' </summary>
    ''' <returns></returns>
    Public Property Location As Point

    ''' <summary>
    ''' Base class
    ''' </summary>
    ''' <param name="width">Width, in grid units</param>
    ''' <param name="height">Height, in grid units</param>
    ''' <param name="location">Location, a point of grid units</param>
    Public Sub New(width As Integer, height As Integer, location As Point, scene As Scene)
        Me.Width = width
        Me.Height = height

        Me.CollisionHeight = height

        Me.Location = location
        Me.MyScene = scene

        RenderImage = New Bitmap(width, height)
    End Sub

    ''' <summary>
    ''' Function run before RenderImage is rendered  - psst you can change it here if you dont wanna override Render()
    ''' </summary>
    Public Overridable Sub BeforeRender()
       
    End Sub

    ''' <summary>
    ''' Draws the image into the graphics object given
    ''' </summary>
    ''' <param name="g"></param>
    Public Overrides Sub Render(g As Graphics)
        BeforeRender()
        g.DrawImage(RenderImage, New Point(Location.X - MyScene.screenLocation.X, Dimensions.ScreenGridHeight - Height - Location.Y + MyScene.screenLocation.Y - toolBarOffSet))
    End Sub

    ''' <summary>
    ''' Checks if the RenderItem is in the current screen
    ''' </summary>
    ''' <returns></returns>
    Public Overridable Function InScene()
        ' checks if levelWidth/levelHeight is in the screen properly
        ' it is in sceen if the location of itself is in the scene to be rendered
        If Me.Location.X + Me.Width < MyScene.screenLocation.X Then
            ' if most right point of block < most left point of screen
            ' left of the screen
            Return False
        ElseIf Me.Location.X > MyScene.screenLocation.X + Dimensions.ScreenGridWidth Then
            ' if msot left of block > most right of screen
            ' it is to the right of the scene
            Return False

        ElseIf Me.Location.Y > MyScene.screenLocation.Y + Dimensions.ScreenGridHeight Then
            ' if lowest bit of block > highest bit of screen
            ' it is above scene
            Return False
        ElseIf Me.Location.Y + Me.Height < MyScene.screenLocation.Y Then
            ' if highest bit of block < lowest bit of screen
            ' it is below scene
            Return False
        End If
        Return True
    End Function

    ''' <summary>
    ''' Called when an entity collides into this RenderObject from the bottom
    ''' </summary>
    ''' <param name="sender"></param>
    Public Overridable Sub CollisionBottom(sender As Entity)
        ' default behaviour = stop player
        'sender.veloc.y = 0
    End Sub

    ''' <summary>
    ''' Called when an entity collides into this RenderObject from the top
    ''' </summary>
    ''' <param name="sender"></param>
    Public Overridable Sub CollisionTop(sender As Entity)
        'sender.veloc.y = 0
    End Sub

    ''' <summary>
    '''  Called when an entity collides into this RenderObject from the left
    ''' </summary>
    ''' <param name="sender"></param>
    Public Overridable Sub CollisionLeft(sender As Entity)
        'sender.veloc.x = 0
    End Sub

    ''' <summary>
    '''  Called when an entity collides into this RenderObject from the bottom
    ''' </summary>
    ''' <param name="sender"></param>
    Public Overridable Sub CollisionRight(sender As Entity)
        'sender.veloc.x = 0
    End Sub

    ''' <summary>
    ''' Changes the animation of the object, if necessary
    ''' </summary>
    Public Overridable Sub animate()
        ' nothing by default
    End Sub

    Public Shared Operator =(left as RenderObject, right as RenderObject)
        return left.ID = right.ID
    End Operator
    Public Shared Operator <>(left as RenderObject, right As RenderObject)
        if left IsNot Nothing And right IsNot nothing
            return left.ID <> right.ID
        Else 
            return IsNothing(left) and IsNothing(right)
        End if
        
    End Operator

End Class
