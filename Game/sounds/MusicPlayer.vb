Imports System.IO
Imports NAudio.Wave

''' <summary>
''' 
''' </summary>
Public NotInheritable Class MusicPlayer
    Implements IDisposable
    Public  Shared Property BackgroundPlayer As MusicPlayer

    Public ReadOnly Property volume As Double
        Get
            Return basevolume * multipler
        End Get
        
    End Property

    Private Sub refreshVolume
        channel.Volume = volume
    End Sub

    Private ReadOnly reader As WaveStream
    Private ReadOnly channel As WaveChannel32
    Private ReadOnly player As IWavePlayer


    ''' <summary>
    ''' A wrapper allowing sound to be player 
    ''' </summary>
    ''' <param name="name">Resource name of mp3 file</param>
    Public Sub New(name As String, Optional volume As Single = 1.0F)
        Me.New(New MemoryStream(CType(My.Resources.ResourceManager.GetObject(name), Byte())), volume)
    End Sub


    ''' <summary>
    ''' Makes sound loop itself after it finishes
    ''' </summary>
    ''' <param name="enable"></param>
    Public Sub EnableLoop(Optional enable As Boolean = True)
        AddHandler player.PlaybackStopped, AddressOf Repeat_audio
    End Sub

    Public Sub New(stream As Stream, Optional volume As Single = 1.0F)
        reader = New Mp3FileReader(stream)
        basevolume = volume

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

    ''' <summary>
    ''' Stops playback - [stop] = stop cuz stop is a keyword in vb.Net for some reason
    ''' </summary>
    Public Sub [Stop]()
            player.Stop()
    End Sub


    Public Sub PlayBackground()
        if BackgroundPlayer Isnot me
            If backgroundPlayer IsNot Nothing Then
                backgroundPlayer.Stop()
            End If
            backgroundPlayer = Me
            backgroundPlayer.Play()
        End if
    End Sub


    ''' <summary>
    ''' Dispose all unmanaged audio resources
    ''' </summary>
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

    ''' <summary>
    ''' Event handler to play sound
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Repeat_audio(sender As Object, e As EventArgs)
        Me.Play()
    End Sub

    Public Property basevolume As double

    Private multipler as double

    Friend Sub SetMultiplier(multipler As Double)
        me.multipler = multipler
        refreshVolume
    End Sub
End Class

Public NotInheritable Class Sounds
    Public Shared Property Jump As New MusicPlayer("jump", 0.6)
    Public Shared Property CoinPickup As New MusicPlayer("coin_pickup", 10)
    Public Shared Property MushroomPickup As New MusicPlayer("mushroom_pickup")
    Public Shared Property BrickSmash As New MusicPlayer("brick_smash", 10)
    Public Shared Property PlayerDead As New MusicPlayer("player_dead")
    Public Shared Property Warp As New MusicPlayer("warp")
    Public Shared Property _1_Up As New MusicPlayer("_1_up")
    Private Sub New
    End Sub

    Public Shared Sub SetVolume(multipler as double)
        if multipler > 1 Or multipler  < 0
            Throw new Exception()
        End if

        for each prop in GetType(Sounds).GetProperties()
             dim music = DirectCast(prop.GetValue(Nothing, Nothing), MusicPlayer)
            music.SetMultiplier(multipler)
        Next

        for each prop in GetType(BackgroundMusic).GetProperties()
            dim music = DirectCast(prop.GetValue(Nothing, Nothing), MusicPlayer)
            music.SetMultiplier(multipler)
        Next
    End Sub
End Class

''' <summary>
''' Each will return a new instance of a musicplayer
''' </summary>
Public NotInheritable Class BackgroundMusic
    Public Shared ReadOnly Property GroundTheme As New MusicPlayer("ground_theme")

    Public Shared ReadOnly Property UnderGroundTheme As New MusicPlayer("cave_theme")

    Public Shared Sub SetVolume(multipler as double)
        Sounds.SetVolume(multipler)
    End Sub
End Class