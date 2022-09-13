Imports NAudio.Wave

Public Class DebugDisplay

    ''' <summary>
    ''' Renders the debug information.
    ''' </summary>
    Public Shared Sub Draw()
        If Core.CurrentScreen.CanDrawDebug = True Then
            Dim isDebugString As String = ""
            If GameController.IS_DEBUG_ACTIVE = True Then
                isDebugString = " (Debugmode / " & System.IO.File.GetLastWriteTime(System.Reflection.Assembly.GetExecutingAssembly.Location).ToString() & ")"
            End If

            Dim ActionscriptActive As Boolean = True
            If Core.CurrentScreen.Identification = Screen.Identifications.OverworldScreen Then
                ActionscriptActive = CType(Core.CurrentScreen, OverworldScreen).ActionScript.IsReady
            End If

            Dim cameraInformation = ""
            If Not Screen.Camera Is Nothing Then

                Dim thirdPersonString As String = ""
                If Screen.Camera.Name = "Overworld" Then
                    Dim c As OverworldCamera = CType(Screen.Camera, OverworldCamera)
                    If c.ThirdPerson = True Then
                        thirdPersonString = " / " & c.ThirdPersonOffset.ToString()
                    End If
                End If

                cameraInformation = Screen.Camera.Position.ToString() & thirdPersonString & Environment.NewLine & Screen.Camera.Yaw & "; " & Screen.Camera.Pitch & Environment.NewLine

            End If
            Dim MapPath As String = ""
            If Screen.Level IsNot Nothing Then
                MapPath = Environment.NewLine & "MapPath: " & Screen.Level.LevelFile.ToString
            End If

            Dim s As String = GameController.GAMENAME & " " & GameController.GAMEDEVELOPMENTSTAGE & " " & GameController.GAMEVERSION & " / FPS: " & Math.Round(Core.GameInstance.FPSMonitor.Value, 0) & isDebugString & Environment.NewLine &
                cameraInformation &
                "E: " & _drawnVertices.ToString() & "/" & _maxVertices.ToString() & Environment.NewLine &
                "C: " & _maxDistance.ToString() & " A: " & ActionscriptActive.ToString() &
                MapPath

            If Core.GameOptions.ContentPackNames.Count() > 0 Then
                Dim contentPackString As String = ""
                For Each ContentPackName As String In Core.GameOptions.ContentPackNames
                    If contentPackString <> "" Then
                        contentPackString &= ", "
                    End If
                    contentPackString &= ContentPackName
                Next
                contentPackString = "Loaded ContentPacks: " & contentPackString
                s &= Environment.NewLine & contentPackString
            End If

            Core.SpriteBatch.DrawInterfaceString(FontManager.MainFont, s, New Vector2(7, 7), Color.Black)
            Core.SpriteBatch.DrawInterfaceString(FontManager.MainFont, s, New Vector2(5, 5), Color.White)

            ' DrawMediaInfo() To test music
        End If
    End Sub

#Region "RenderDataTracking"

    'Values for tracking render data of the level.
    Private Shared _drawnVertices As Integer = 0
    Private Shared _maxVertices As Integer = 0
    Private Shared _maxDistance As Integer = 0

    ''' <summary>
    ''' The amount of vertices rendered in the last frame.
    ''' </summary>
    Public Shared Property DrawnVertices() As Integer
        Get
            Return _drawnVertices
        End Get
        Set(value As Integer)
            _drawnVertices = value
        End Set
    End Property

    ''' <summary>
    ''' The maximum amount of vertices that are present in the current scene.
    ''' </summary>
    Public Shared Property MaxVertices() As Integer
        Get
            Return _maxVertices
        End Get
        Set(value As Integer)
            _maxVertices = value
        End Set
    End Property

    ''' <summary>
    ''' MediaPlayer state tracking method.
    ''' </summary>
    Private Shared Sub DrawMediaInfo()
        Dim songName = "<NO SONG PLAYING>"
        If MediaPlayer.Queue IsNot Nothing AndAlso MediaPlayer.Queue.ActiveSong IsNot Nothing Then
            songName = MediaPlayer.Queue.ActiveSong.Name
            If MusicManager.CurrentSong IsNot Nothing Then
                songName += " (" + MusicManager.CurrentSong.Name + ")"
            End If
        End If

        Dim field = GetType(MediaPlayer).GetField("_sessionState", Reflection.BindingFlags.NonPublic Or Reflection.BindingFlags.Static)
        Dim sessionState = field.GetValue(Nothing).ToString()

        Dim _outputDevice As WaveOutEvent
        If Not MusicManager.outputDevice Is Nothing Then
            _outputdevice = MusicManager.outputDevice
        Else
            _outputDevice = New WaveOutEvent()
        End If
        Dim str = "Song: " + songName + Environment.NewLine +
            "Play position: " + _outputDevice.GetPosition.ToString() + Environment.NewLine +
            "Session state: " + sessionState + Environment.NewLine +
            "State: " + _outputDevice.PlaybackState.ToString() + Environment.NewLine +
            "Volume: " + MusicManager.MasterVolume.ToString() + Environment.NewLine +
            "Is Muted: " + MusicManager.Muted.ToString() + Environment.NewLine +
            "Is Repeating: " + MusicManager.IsLooping.ToString()

        Core.SpriteBatch.DrawInterfaceString(FontManager.MainFont, str, New Vector2(7, 7), Color.Black)
        Core.SpriteBatch.DrawInterfaceString(FontManager.MainFont, str, New Vector2(5, 5), Color.White)
    End Sub

    ''' <summary>
    ''' The distance of the vertex to the camera, that is the furthest away from the camera.
    ''' </summary>
    Public Shared Property MaxDistance() As Integer
        Get
            Return _maxDistance
        End Get
        Set(value As Integer)
            _maxDistance = value
        End Set
    End Property

#End Region

End Class