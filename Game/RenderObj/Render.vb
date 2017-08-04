Public MustInherit Class RenderObject
    Public Property Width As Integer
    Public Property Height As Integer
    Private Shared toolBarOffSet As Integer = 29



    Private Shared _idCount As Integer
    
    Private Shared Function GetNewID() As Integer
        Dim temp = _idCount
        _idCount += 1
        return temp
    End Function

    Public Property ID As Integer = GetNewId()



    Public MustOverride Property RenderImage As Image
    ''' <summary>
    ''' Holds the current screen location, shared by RENDER OBJECT and all subclasses
    ''' </summary>
    Friend Shared screenLocation As New Point(0,0)

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
    Public Sub New(width As Integer, height As Integer, location As Point)
        Me.Width = width
        Me.Height = height
        Me.Location = location

        RenderImage = New Bitmap(width, height)
    End Sub

    Public Overridable Sub BeforeRender()
       
    End Sub

    Public Overridable Sub Render(g As Graphics)
        BeforeRender()
        g.DrawImage(RenderImage, New Point(Location.X - screenLocation.X, Helper.ScreenGridHeight - Height - Location.Y + screenLocation.Y - toolBarOffSet))
    End Sub

    Public Overridable Function InScene()
        ' checks if levelWidth/levelHeight is in the screen properly
        ' it is in sceen if the location of itself is in the scene to be rendered
        If Me.Location.X + Me.Width < screenLocation.X Then
            ' if most right point of block < most left point of screen
            ' left of the screen
            Return False
        Else If Me.Location.X > screenLocation.X + Helper.ScreenGridWidth
            ' if msot left of block > most right of screen
            ' it is to the right of the scene
            Return False

        Else If Me.Location.Y > screenLocation.Y + Helper.ScreenGridHeight Then
            ' if lowest bit of block > highest bit of screen
            ' it is above scene
            Return False
        ElseIf Me.Location.Y + Me.Height < screenLocation.Y Then
            ' if highest bit of block < lowest bit of screen
            ' it is below scene
            Return False
        End If
        Return True
    End Function





End Class
