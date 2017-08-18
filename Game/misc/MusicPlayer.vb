Imports System.IO
Imports NAudio.Wave

Public NotInheritable Class MusicPlayer
    Implements IDisposable
    Private Shared Property backgroundPlayer As MusicPlayer

    Private ReadOnly reader As WaveStream
    Private ReadOnly channel As WaveChannel32
    Private ReadOnly player As IWavePlayer

    Public Shared Sub PlayBackground(music As MusicPlayer)
        If backgroundPlayer IsNot Nothing Then
            backgroundPlayer.Dispose()
        End If
        backgroundPlayer = music
        backgroundPlayer.DoLoop(True)
        backgroundPlayer.Play()
    End Sub



    ''' <summary>
    ''' A wrapper allowing sound to be played.
    ''' </summary>
    ''' <param name="name"></param>
    Public Sub New(name As String, Optional volume As Single = 1.0F)
        Me.New(New MemoryStream(CType(My.Resources.ResourceManager.GetObject(name), Byte())), volume)

    End Sub

    Public Sub DoLoop(Optional enable As Boolean = True)
        AddHandler player.PlaybackStopped, AddressOf Repeat_audio
    End Sub

    Private Sub Repeat_audio(sender As Object, e As EventArgs)
        Me.Play()
    End Sub

    Public Sub New(stream As Stream, Optional volume As Single = 1.0F)
        reader = New Mp3FileReader(stream)

        channel = New WaveChannel32(reader, volume, 0)
        channel.PadWithZeroes = False

        player = New DirectSoundOut()


        player.Init(channel)
    End Sub

    Public Sub Play(Optional fromStart As Boolean = True)
        If fromStart Then

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
        If backgroundPlayer IsNot Nothing Then
            backgroundPlayer.Dispose()
        End If
        backgroundPlayer = New MusicPlayer(name)
        backgroundPlayer.Play()
    End Sub



    Public Shared Sub Media_Repeat(sender As MusicPlayer, e As EventArgs)
        sender.Play(fromStart:=True)
    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        If reader IsNot Nothing Then
            reader.Dispose()
        End If
        If reader IsNot Nothing Then
            channel.Dispose()
        End If
        If reader IsNot Nothing Then
            player.Dispose()
        End If
    End Sub
End Class

Public NotInheritable Class Sounds
    Public Shared Jump As New MusicPlayer("jump", 0.6)
    Public Shared CoinPickup As New MusicPlayer("coin_pickup")
    Public Shared MushroomPickup As New MusicPlayer("mushroom_pickup")
    Public Shared BrickSmash As New MusicPlayer("brick_smash", 10)
    Public Shared PlayerDead As New MusicPlayer("player_dead")
    Public Shared Warp As New MusicPlayer("warp")
    Public Shared _1_Up As New MusicPlayer("_1_up")
    Private Sub New
    End Sub
End Class

Public Module BackgroundMusic
    Public GroundTheme As New MusicPlayer("ground_theme")
End Module