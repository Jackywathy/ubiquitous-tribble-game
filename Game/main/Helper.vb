''' <summary>
''' Module that stores global variables
''' </summary>
Public Module Dimensions
    Public Const ScreenGridWidth As Integer = 1280
    Public Const ScreenGridHeight As Integer = 720
    Public TotalGridWidth As Integer = 4000
    Public TotalGridHeight As Integer = 1000
    Public MarioWidth = 32
    Public MarioHeightS = 32
    Public MarioHeightB = 64
    Public GroundHeight = 64
End Module

Module Debug
    ''' <summary>
    ''' Shows an image as a form for debugging purposes
    ''' </summary>
    ''' <param name="image"></param>
    Public Sub DbShowImage(image As Image)
        Dim x = (new TestImage(image))
        x.Show()
    End Sub

    ''' <summary>
    ''' Prints an object to the console
    ''' </summary>
    ''' <param name="str"></param>
    Public Sub Print(str As Object)
        Console.Out.WriteLine(str.ToString)
    End Sub

    

#if DEBUG
    ''' <summary>
    ''' Use this to add additional objects on in DEBUG configuration
    ''' </summary>
    ''' <param name="scene"></param>
    Public Sub DebugMapHook(scene As MapScene)
        scene.AddItem(New StaticCoin(New Point(700,500), scene))
        scene.AddEntity(New EntCoin(32, 32, New Point(320, 96), scene))
        scene.AddObject(New BlockMultipleCoins(New Point(32*5, 32*5), scene))
    End Sub
    Public ShowBoundingBox As Boolean = True
#Else
    Public ShowBoundingBox As Boolean = False
#End If

End Module


Public Module ImageManipulation
    ''' <summary>
    ''' Crops an image, and returns a new image
    ''' </summary>
    ''' <param name="image"></param>
    ''' <param name="bottomLeft"></param>
    ''' <param name="width"></param>
    ''' <param name="height"></param>
    ''' <returns></returns>
    Public Function Crop(image As Image, bottomLeft As Point, width As Integer, height As Integer) As Image
        Dim cropRect As New RectangleF(New PointF(bottomLeft.X, image.Height - height - bottomLeft.Y), New SizeF(width, height))

        Dim out As New Bitmap(width, height)
        Using g = Graphics.FromImage(out)
            g.DrawImage(image, New RectangleF(0, 0, cropRect.Width, cropRect.Height), cropRect, GraphicsUnit.Pixel)
        End Using
        Return out
    End Function

    ''' <summary>
    ''' Crops an image, drawing it on the out graphics object
    ''' </summary>
    ''' <param name="image"></param>
    ''' <param name="out"></param>
    ''' <param name="bottomLeft"></param>
    ''' <param name="width"></param>
    ''' <param name="height"></param>
    Public Sub Crop(image As Image, out As Graphics, bottomLeft As Point, width As Integer, height As Integer)
        Dim cropRect As New RectangleF(New PointF(bottomLeft.X, image.Height - height - bottomLeft.Y), New SizeF(width, height))
        out.DrawImage(image, New RectangleF(0, 0, cropRect.Width, cropRect.Height), cropRect, GraphicsUnit.Pixel)
    End Sub

    ''' <summary>
    ''' Scales the image
    ''' </summary>
    ''' <param name="image"></param>
    ''' <param name="width"></param>
    ''' <param name="height"></param>
    ''' <returns></returns>
    Public Function Resize(image As Image, width As Integer, height As Integer) As Image
        ' width/height of rectangle output
        Dim out As New Bitmap(width, height)
        Using g = Graphics.FromImage(out)
            g.DrawImage(image, 0, 0, width, height)
        End Using
        Return out
    End Function

    ''' <summary>
    ''' Returns a flipped image
    ''' </summary>
    ''' <param name="image"></param>
    ''' <returns></returns>
    Public Function Flip(image As Image) As Image
        Dim img As Bitmap = image.Clone()
        img.RotateFlip(RotateFlipType.RotateNoneFlipX)
        return img
    End Function

End Module

Public NotInheritable Class Helper
    Public Shared Function StrToEnum(Of T)(valueToParse As String) As T
        Return [Enum].Parse(GetType(T), valueToParse, True)
    End Function

    Public Shared Function IsPlayer(item As HitboxItem) As boolean
        if item.GetType() = GetType(EntPlayer)
            Return True
        End If
        Return False
    End Function

    Public Shared Function IsEntity(item As HitboxItem) As Boolean
        If item.GetType.IsSubclassOf(GetType(Entity)) Then
            Return True
        Else
            return False
        End If
    End Function

    Private Sub New

    End Sub
    ''' <summary>
    ''' Generates a number between start (inclusive) and end (not exclusive)
    ''' This is because Rnd() returns a number &gt;= 0 , and &lt; 1
    ''' </summary>
    ''' <param name="start"></param>
    ''' <param name="end"></param>
    ''' <returns></returns>
    Public Shared Function Random(start As Integer, [end] as Double) As Integer
        return CInt(Math.Floor(([end] - start + 1) * Rnd())) + start

   End Function


       
End Class