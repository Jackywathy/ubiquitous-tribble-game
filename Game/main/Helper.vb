
Imports System.IO
Imports NAudio.Wave

Public Module Dimensions
    Public Const ScreenGridWidth As Integer = 1280
    Public Const ScreenGridHeight As Integer = 720
    Public TotalGridWidth As Integer = 4000
    Public TotalGridHeight As Integer = 1000
    Public MarioWidth = 32
    Public MarioHeight = 32
End Module

Module Debug
    Public Sub DbShowImage(image As Image)
        Dim x = (new    TestImage(image))
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

Public Enum SoundEffects
    Jump

End Enum

Public NotInheritable Class MusicPlayer
    Implements IDisposable
    Private Shared backgroundPlayer As MusicPlayer

    Private reader As WaveFileReader
    Private volume As WaveChannel32
    Private player As IWavePlayer

    Private Shared Function getEffect(effect As SoundEffects) As String
        Dim out as String
        Select case effect
            Case SoundEffects.Jump
                out = "jump"
            Case Else:
                Throw New ArgumentException()
        End Select
        Return out
    End Function

    Public Sub New(effect As SoundEffects)
        Me.New(getEffect(effect))
    End Sub

    ''' <summary>
    ''' A wrapper allowing sound to be played.
    ''' </summary>
    ''' <param name="name"></param>
    Public Sub New(name As String, Optional useWaveOut As Boolean = False) 
        Me.New(My.Resources.ResourceManager.GetStream(name), useWaveOut)
    End Sub 

    

    Public Sub New(stream As Stream, Optional useWaveOut As Boolean = False)
        reader = New WaveFileReader(stream) 
        volume = New WaveChannel32(reader) 
        player = New DirectSoundOut()
        
        player.Init(volume)
        MsgBox(String.Format("Audio length: {0}", reader.Length)) 
    End Sub

    Public Sub Play(Optional fromStart As Boolean = False)
        If fromStart

            reader.CurrentTime = TimeSpan.Zero
            End If
        ' go to beginning
        '
        player.Play()
    End Sub

    Public Sub Pause()
        player.Stop()

    End Sub


    Public Shared Sub PlayBackground(name As String)
        if backgroundPlayer IsNot Nothing
            backgroundPlayer.Dispose()
        End if
        backgroundPlayer = New MusicPlayer(name, True)
        backgroundPlayer.Play()
    End Sub

    Public Shared Sub PlayBackground(music As BackgroundMusic)
        Select Case music
            Case BackgroundMusic.Overworld
                PlayBackground("overworld")
            Case BackgroundMusic.Underground
                PlayBackground("underground")
        End Select
    End Sub

    Public Shared Sub Media_Repeat(sender As MusicPlayer, e As EventArgs)
        sender.Play(fromStart := True)
    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        reader.Dispose()
        volume.Dispose()
        player.Dispose()
    End Sub
End Class

Friend Module UriCreator
    

    


End Module
