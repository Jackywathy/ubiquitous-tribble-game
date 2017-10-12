Imports System.IO
Imports NAudio.Wave
Imports WinGame

''' <summary>
''' 
''' </summary>
Public NotInheritable Class MusicPlayer
    Implements IDisposable
    Public Shared Property BackgroundPlayer As MusicPlayer

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
    Public Sub New(name As String, Optional volume As Single = 1.0F, Optional repeat As Boolean = False)
        Me.New(New MemoryStream(CType(My.Resources.ResourceManager.GetObject(name), Byte())), volume)
        If repeat
            EnableLoop
        End If
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
        userStopped = False
    End Sub

    ''' <summary>
    ''' Stops playback - [stop] = stop cuz stop is a keyword in vb.Net for some reason
    ''' </summary>
    Public Sub [Stop]()
        player.Stop()
        userStopped = True
    End Sub

    Private userStopped As Boolean = False

    Public Sub PlayBackground()
        If BackgroundPlayer IsNot Me
            If BackgroundPlayer IsNot Nothing Then
                BackgroundPlayer.Stop()
            End If
            BackgroundPlayer = Me


        End If
        BackgroundPlayer.Play()
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
        If Not userStopped
            Me.Play()
        End If
    End Sub

    Public Property basevolume As Double

    Private multipler As Double

    Friend Sub SetMultiplier(multipler As Double)
        Me.multipler = multipler
        refreshVolume
    End Sub
End Class

Public NotInheritable Class Sounds
    Public Shared Property Jump As New MusicPlayer("jump", 0.1)
    Public Shared Property CoinPickup As New MusicPlayer("coin_pickup", 1)
    Public Shared Property MushroomPickup As New MusicPlayer("mushroom_pickup", 0.2)
    Public Shared Property BrickSmash As New MusicPlayer("brick_smash", 1.5)
    Public Shared Property PlayerDead As New MusicPlayer("player_dead", 0.5)
    Public Shared Property Warp As New MusicPlayer("warp", 0.8)
    Public Shared Property _1_Up As New MusicPlayer("_1_up")
    Public Shared Property Bump As New MusicPlayer("bump")
    Public Shared Property PowerupAppear As New MusicPlayer("appear")
    Private Sub New
    End Sub

    Public Shared Sub SetVolume(multipler As Double)
        If multipler > 1 Or multipler < 0
            Throw New Exception()
        End If

        For Each prop In GetType(Sounds).GetProperties()
            Dim music = DirectCast(prop.GetValue(Nothing, Nothing), MusicPlayer)
            music.SetMultiplier(multipler)
        Next

        For Each prop In GetType(BackgroundMusic).GetProperties()
            Dim music = DirectCast(prop.GetValue(Nothing, Nothing), MusicPlayer)
            music.SetMultiplier(multipler)
        Next
    End Sub
End Class

''' <summary>
''' Each will return a new instance of a musicplayer
''' </summary>
Public NotInheritable Class BackgroundMusic
    Public Shared Readonly Property CastleTheme As New MusicPlayer("castle_theme", 0.2, True)
    Public Shared ReadOnly Property GroundTheme As New MusicPlayer("ground_theme", 0.2, True)

    Public Shared ReadOnly Property UnderGroundTheme As New MusicPlayer("cave_theme", 0.5, True)

    Public Shared Sub SetVolume(multipler As Double)
        Sounds.SetVolume(multipler)
    End Sub
End Class