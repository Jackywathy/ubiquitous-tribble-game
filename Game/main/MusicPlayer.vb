Imports System.IO
Imports NAudio.Wave

Public NotInheritable Class MusicPlayer
    Implements IDisposable
    Private Shared backgroundPlayer As MusicPlayer

    Private reader As WaveStream
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
    Public Sub New(name As String) 
        Me.New(New MemoryStream(CType(My.Resources.ResourceManager.GetObject(name), Byte())))
    End Sub 

    

    Public Sub New(stream As Stream)
        reader = New Mp3FileReader(stream) 
        volume = New WaveChannel32(reader) 
        player = New DirectSoundOut()
        
        player.Init(volume)
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
        backgroundPlayer = New MusicPlayer(name)
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