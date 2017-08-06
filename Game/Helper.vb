
Imports System.IO
Imports System.Windows.Media
Imports NAudio.Wave

Public Module Dimensions
    Public Const ScreenGridWidth As Integer = 1280
    Public Const ScreenGridHeight As Integer = 720
    Public TotalGridWidth As Integer = 4000
    Public TotalGridHeight As Integer = 1000
End Module

Module Debug
    Public Sub DbShowImage(image As Image)
        Dim x = (new TestImage(image))
        x.Show()
    End Sub
End Module

Public Module ImageManipulation

    Public Function Crop(image As Image, bottomLeft As Point, width As Integer, height As Integer) As Image
        ' width/height of rectangle output


        ' rectangle to be cut out
        Dim cropRect As New RectangleF(New PointF(bottomLeft.X, image.Height - height - bottomLeft.Y), New SizeF(width, height))

        Dim out As New Bitmap(width, height)
        Using g = Graphics.FromImage(out)
            g.DrawImage(image, New RectangleF(0, 0, cropRect.Width, cropRect.Height), cropRect, GraphicsUnit.Pixel)
        End Using
        Return out
    End Function

    Public Sub Crop(image As Image, out As Graphics, bottomLeft As Point, width As Integer, height As Integer)
        ' width/height of rectangle output


        ' rectangle to be cut out
        Dim cropRect As New RectangleF(New PointF(bottomLeft.X, image.Height - height - bottomLeft.Y), New SizeF(width, height))
        out.DrawImage(image, New RectangleF(0, 0, cropRect.Width, cropRect.Height), cropRect, GraphicsUnit.Pixel)
    End Sub

    Public Function Resize(image As Image, width As Integer, height As Integer) As Image
        ' width/height of rectangle output
        Dim out As New Bitmap(width, height)
        Using g = Graphics.FromImage(out)
            g.DrawImage(image, 0, 0, width, height)
        End Using
        Return out
    End Function

End Module


Public Enum BackgroundMusic
    Overworld
    Underground

End Enum

Public NotInheritable Class MusicPlayer
    Private Shared backgroundPlayer As New MediaPlayer()
    Dim Shared x As New MediaPlayer()
    

    Public Shared Sub PlaySound()
        

    End Sub



    Public Shared Sub PlayBackground(name As String)
        Dim meWave As New WaveFileReader(My.Resources.ResourceManager.GetStream(name))
        Dim volume As New WaveChannel32(meWave)
        Dim player As New WaveOutEvent()
        MsgBox(String.Format("Audio length: {0}",meWave.Length))
        player.Init(volume)
        player.Play()
        
        
      
    End Sub

    Public Shared Sub Media_Repeat(sender As MediaPlayer, e As EventArgs)

        sender.Position = TimeSpan.Zero
        sender.Play()
    End Sub
End Class

Friend Module UriCreator
    

    


    Private init =System.IO.Packaging.PackUriHelper.UriSchemePack
End Module
