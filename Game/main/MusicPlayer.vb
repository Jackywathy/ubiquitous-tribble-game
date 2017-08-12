Imports System.IO
Imports NAudio.Wave

Public NotInheritable Class MusicPlayer
    Implements IDisposable
    Private Shared Property backgroundPlayer As MusicPlayer

    Private reader As WaveStream
    Private channel As WaveStream
    Private player As IWavePlayer

    Public Shared Sub PlayBackground(music As MusicPlayer)
       backgroundPlayer = music
       backgroundPlayer.DoLoop(True)
    End Sub

 

    ''' <summary>
    ''' A wrapper allowing sound to be played.
    ''' </summary>
    ''' <param name="name"></param>
    Public Sub New(name As String, Optional volume As Single=1.0f)
        Me.New(New MemoryStream(CType(My.Resources.ResourceManager.GetObject(name), Byte())), volume)
        
    End Sub 

    Public Sub DoLoop(optional enable as boolean=True)
        AddHandler player.PlaybackStopped, AddressOf Repeat_audio
    End Sub

    Private sub Repeat_audio(sender As Object, e As EventArgs)
        Me.Play()
    End sub

    Public Sub New(stream As Stream, Optional volume As Single=1.0f)
        reader = New Mp3FileReader(stream) 
       
        channel = New WaveChannel32(reader, volume, 0)
       
        player = New DirectSoundOut()
        
        
        player.Init(channel)
    End Sub

    Public Sub Play(Optional fromStart As Boolean = True)
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

    

    Public Shared Sub Media_Repeat(sender As MusicPlayer, e As EventArgs)
        sender.Play(fromStart := True)
    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        reader.Dispose()
        channel.Dispose()
        player.Dispose()
    End Sub
End Class

Public Module Sounds
    Public Jump As New MusicPlayer("jump", 0.6)
    Public CoinPickup As New MusicPlayer("coin_pickup")
    Public MushroomPickup As New MusicPlayer("mushroom_pickup")
    Public BrickSmash As New MusicPlayer("brick_smash", 10)
End Module

Public Module BackgroundMusic
    Public GroundTheme As New MusicPlayer("ground_theme")
End Module