''' <summary>
''' Ultimate base class for all items rended into the game
''' </summary>
Public MustInherit Class GameItem
    Public MustOverride Sub Render(g As Graphics)
   ' Public Shared GlobalFrameCount As ULong = 0

    Friend Overridable Property MyScene As MapScene
    Private Shared _idCount As Integer

    Private Shared Function GetNewID() As Integer
        Dim temp = _idCount
        _idCount += 1

        Return temp
    End Function

    Public Property ID As Integer = GetNewID()


    Public Shared Operator =(left As GameItem, right As GameItem)
        Return left.ID = right.ID
    End Operator
    Public Shared Operator <>(left As GameItem, right As GameItem)
        If left IsNot Nothing And right IsNot Nothing
            Return left.ID <> right.ID
        Else
            Return IsNothing(left) And IsNothing(right)
        End If
    End Operator

    Sub New(scene As BaseScene)
        Me.MyScene = scene
    End Sub


End Class

''' <summary>
''' An image that is drawing into the game, using a default Render(). Supply it with 
''' </summary>
Public MustInherit Class StaticImage
    Inherits GameItem

    Public Overridable Property RenderImage As Image


    Public Property Width As Integer
    Public Property Height As Integer

    Friend Const ToolBarOffSet As Integer = 29

    Public Sub New(image As Image, scene As BaseScene)
        MyBase.New(scene)
        Me.RenderImage = image
    End Sub

    ''' <summary>
    ''' Location of object from the very bottom left (0,0)
    ''' </summary>
    ''' <returns></returns>
    Public Property Location As Point

    ''' <summary>
    ''' Draws the image into the graphics object given
    ''' </summary>
    ''' <param name="g"></param>
    Public Overrides Sub Render(g As Graphics)
        g.DrawImage(RenderImage, New Point(Location.X - MyScene.ScreenLocation.X, Dimensions.ScreenGridHeight - Height - Location.Y + MyScene.ScreenLocation.Y - ToolBarOffSet))
    End Sub

End Class

''' <summary>
''' Items that have hitboxes
''' </summary>
Public MustInherit Class HitboxItem
    Inherits StaticImage


    Public Property CollisionHeight As Integer

    Public localFrameCount As Integer = 0

    'Public internalFrameCounter = 0
    Public Const animationInterval As Integer = 5 ' Frames to wait before proceeding to next image of animation




    ''' <summary>
    ''' Base class
    ''' </summary>
    ''' <param name="width">Width, in grid units</param>
    ''' <param name="height">Height, in grid units</param>
    ''' <param name="location">Location, a point of grid units</param>
    Public Sub New(width As Integer, height As Integer, location As Point, mapScene As MapScene)
        Mybase.New(New Bitmap(width, height), mapScene)
        Me.Width = width
        Me.Height = height

        Me.CollisionHeight = height

        Me.Location = location
        Me.MyScene = mapScene

    End Sub

    ''' <summary>
    ''' Do nothing, by default
    ''' </summary>
    Public Overridable Sub UpdateItem()

    End Sub

    ''' <summary>
    ''' Checks if the StaticItem is in the current screen
    ''' </summary>
    ''' <returns></returns>
    Public Overridable Function InScene()
        ' checks if levelWidth/levelHeight is in the screen properly
        ' it is in sceen if the location of itself is in the mapScene to be rendered
        If Me.Location.X + Me.Width < MyScene.ScreenLocation.X Then
            ' if most right point of block < most left point of screen
            ' left of the screen
            Return False
        ElseIf Me.Location.X > MyScene.ScreenLocation.X + Dimensions.ScreenGridWidth Then
            ' if msot left of block > most right of screen
            ' it is to the right of the mapScene
            Return False

        ElseIf Me.Location.Y > MyScene.ScreenLocation.Y + Dimensions.ScreenGridHeight Then
            ' if lowest bit of block > highest bit of screen
            ' it is above mapScene
            Return False
        ElseIf Me.Location.Y + Me.Height < MyScene.ScreenLocation.Y Then
            ' if highest bit of block < lowest bit of screen
            ' it is below mapScene
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

    'Public collidedX As Boolean = False

    ''' <summary>
    '''  Called when an entity collides into this RenderObject from the left
    ''' </summary>
    ''' <param name="sender"></param>
    Public Overridable Sub CollisionLeft(sender As Entity)
        'sender.veloc.x = 0
        'collidedX = True
    End Sub

    ''' <summary>
    '''  Called when an entity collides into this RenderObject from the bottom
    ''' </summary>
    ''' <param name="sender"></param>
    Public Overridable Sub CollisionRight(sender As Entity)
        'sender.veloc.x = 0
        'collidedX = True
    End Sub

    ''' <summary>
    ''' Changes the animation of the object, if necessary
    ''' </summary>
    Public Overridable Sub animate()
        ' nothing by default
    End Sub



    Public Overridable Sub AddSelfToScene()
        If Me.GetType.IsSubclassOf(GetType(Entity)) Then
            MyScene.AddEntity(Me)
        Else
            MyScene.AddObject(Me)
        End If
    End Sub

End Class
