''' <summary>
''' Ultimate base class for all items rended into the game
''' </summary>
Public MustInherit Class GameItem
    Public MustOverride Sub Render(g As Graphics)    

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


End Class

''' <summary>
''' Base class for all images rendered onto the game
''' </summary>
Public MustInherit Class GameImage
    Inherits GameItem

    Friend Const ToolBarOffSet As Integer = 29

    ''' <summary>
    ''' width of image
    ''' </summary>
    ''' <returns></returns>
    Public Property Width As Integer
    ''' <summary>
    ''' Height of image
    ''' </summary>
    ''' <returns></returns>
    Public Property Height As Integer
    ''' <summary>
    ''' Location of object from the very bottom left (0,0)
    ''' </summary>
    ''' <returns></returns>
    Public Property Location As Point
    ''' <summary>
    ''' Image to be drawn onto the screen in the next refresh   
    ''' </summary>
    ''' <returns></returns>
    Friend Property RenderImage As Image

    ''' <summary>
    ''' Constructor for <see cref="GameImage"/>
    ''' </summary>
    ''' <param name="width"></param>
    ''' <param name="height"></param>
    ''' <param name="location"></param>
    ''' <param name="image"></param>
     Public Sub New(width As Integer, height As Integer, location As point, image As Image)
        Me.Width = width
        Me.Height = height
        Me.RenderImage = image
        Me.location = location
    End Sub
End Class





''' <summary>
''' This is a non-changing image that moves with the player/game
''' </summary>
Public MustInherit Class MovingImage
    Inherits GameImage

    ''' <summary>
    ''' A reference to the scene to use it's screenlocation
    ''' </summary>
    ''' <returns></returns>
    Friend Property MyScene As MapScene


    ''' <summary>
    ''' Constructor for <see cref="MovingImage"/>
    ''' </summary>
    ''' <param name="width"></param>
    ''' <param name="height"></param>
    ''' <param name="location"></param>
    ''' <param name="image"></param>
    ''' <param name="scene"></param>
    Public Sub New(width As Integer, height As Integer, location As Point, image As Image, scene As MapScene)
        MyBase.New(width, height, location, image)
        MyScene = scene
    End Sub
   
    ''' <summary>
    ''' Draws the image into the graphics object given
    ''' </summary>
    ''' <param name="g"></param>
    Public Overrides Sub Render(g As Graphics)
        if RenderImage IsNot Nothing
            Dim drawnRect As New Rectangle(Location.X - MyScene.ScreenLocation.X,
                                      Dimensions.ScreenGridHeight - Height - Location.Y + MyScene.ScreenLocation.Y - ToolBarOffSet,
                                           width, height)
            ' top right x, top right y, width, heigh

            g.DrawImage(RenderImage, drawnRect)
            If ShowBoundingBox Then
                g.DrawRectangle(DrawingPrimitives.BlackPen, drawnRect)
            End If
        End if
    End Sub

End Class

''' <summary>
''' Items that have hitboxes
''' </summary>
Public MustInherit Class HitboxItem
    Inherits MovingImage


    Public Property CollisionHeight As Integer

    Friend FramesSinceHit As Integer = 0

    Public Const AnimationInterval As Integer = 5 ' Frames to wait before proceeding to next image of animation


    ''' <summary>
    ''' Constructor for <see cref="HitboxItem"/>
    ''' </summary>
    ''' <param name="width">Width, in grid units</param>
    ''' <param name="height">Height, in grid units</param>
    ''' <param name="location">Location, a point of grid units</param>
    Public Sub New(width As Integer, height As Integer, location As Point, image As Image, mapScene As MapScene)
        Mybase.New(width, height, location, image, mapScene)
        Me.CollisionHeight = height

    End Sub

    ''' <summary>
    ''' Overriding to add hitbox rendering
    ''' </summary>
    ''' <param name="g"></param>
    Public Overrides Sub Render(g As Graphics)
        MyBase.Render(g)
        If ShowBoundingBox Then
            Dim drawnRect As New Rectangle(Location.X - MyScene.ScreenLocation.X,
                    Dimensions.ScreenGridHeight - CollisionHeight - Location.Y + MyScene.ScreenLocation.Y - ToolBarOffSet,
                                       width, CollisionHeight)
            g.DrawRectangle(DrawingPrimitives.RedPen, drawnRect)
        End If
    End Sub

    ''' <summary>
    ''' Updates veloc
    ''' </summary>
    Public Overridable Sub UpdateVeloc()

    End Sub

    ''' <summary>
    ''' Updates Location by adding veloc to location, if nessecary
    ''' </summary>
    Public Overridable Sub UpdateLocation()

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
    End Sub

    ''' <summary>
    '''  Called when an entity collides into this RenderObject from the left
    ''' </summary>
    ''' <param name="sender"></param>
    Public Overridable Sub CollisionLeft(sender As Entity)
    End Sub

    ''' <summary>
    '''  Called when an entity collides into this RenderObject from the bottom
    ''' </summary>
    ''' <param name="sender"></param>
    Public Overridable Sub CollisionRight(sender As Entity)
    End Sub

    ''' <summary>
    ''' Changes the animation of the object, if necessary
    ''' </summary>
    Public Overridable Sub Animate()
    End Sub

    Public Overridable Sub AddSelfToScene()
        If Helper.IsEntity(Me) Then
            MyScene.AddEntity(Me)
        Else
            MyScene.AddObject(Me)
        End If
    End Sub

End Class
