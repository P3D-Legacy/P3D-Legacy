Imports System.Reflection
Imports System.Text
Imports NAudio.Wave

Public Class DebugDisplay

    Private Shared ReadOnly StringBuilder As StringBuilder = New StringBuilder()
    
    ''' <summary>
    ''' Renders the debug information.
    ''' </summary>
    Public Shared Sub Draw()
        StringBuilder.Clear()
        
        If Core.CurrentScreen.CanDrawDebug Then
            If GameController.IS_DEBUG_ACTIVE Then
                StringBuilder.AppendFormat("{0} {1} {2} / FPS: {3:F0} (Debugmode / {4})", 
                                           GameController.GAMENAME, 
                                           GameController.GAMEDEVELOPMENTSTAGE, 
                                           GameController.GAMEVERSION, 
                                           Core.GameInstance.FpsMonitor.Value, 
                                           File.GetLastWriteTime(Assembly.GetExecutingAssembly().Location).ToString())
                StringBuilder.AppendLine()
            Else
                StringBuilder.AppendFormat("{0} {1} {2} / FPS: {3:F0}", 
                                           GameController.GAMENAME, 
                                           GameController.GAMEDEVELOPMENTSTAGE, 
                                           GameController.GAMEVERSION, 
                                           Core.GameInstance.FpsMonitor.Value)
                StringBuilder.AppendLine()
            End If

            If Screen.Camera IsNot Nothing Then
                If Screen.Camera.Name = "Overworld" Then
                    Dim camera = CType(Screen.Camera, OverworldCamera)
                    
                    If camera.ThirdPerson Then
                        StringBuilder.AppendFormat("{0} / {1}", Screen.Camera.Position.ToString(), camera.ThirdPersonOffset.ToString())
                        StringBuilder.AppendLine()
                        StringBuilder.AppendFormat("{0:F} ; {1:F}", Screen.Camera.Yaw, Screen.Camera.Pitch)
                        StringBuilder.AppendLine()
                    Else
                        StringBuilder.AppendLine(Screen.Camera.Position.ToString())
                        StringBuilder.AppendFormat("{0:F} ; {1:F}", Screen.Camera.Yaw, Screen.Camera.Pitch)
                        StringBuilder.AppendLine()
                    End If
                Else
                    StringBuilder.AppendLine(Screen.Camera.Position.ToString())
                    StringBuilder.AppendFormat("{0:F} ; {1:F}", Screen.Camera.Yaw, Screen.Camera.Pitch)
                    StringBuilder.AppendLine()
                End If
            End If

            StringBuilder.AppendFormat("E: {0}/{1} (Draw Calls: {2})", _drawnVertices, _maxVertices, Core.GraphicsManager.GraphicsDevice.Metrics.DrawCount)
            StringBuilder.AppendLine()

            If TypeOf Core.CurrentScreen Is OverworldScreen Then
                StringBuilder.AppendFormat("C: {0} A: {1}", _maxDistance, CType(Core.CurrentScreen, OverworldScreen).ActionScript.IsReady)
                StringBuilder.AppendLine()
            Else 
                StringBuilder.AppendFormat("C: {0}", _maxDistance)
                StringBuilder.AppendLine()
            End If
            
            If Screen.Level IsNot Nothing Then
                StringBuilder.AppendFormat("Map Path: {0}", Screen.Level.LevelFile)
                StringBuilder.AppendLine()
            End If

            If Core.GameOptions.ContentPackNames.Length > 0 Then
                StringBuilder.AppendFormat("Loaded Content Packs: {0}", String.Join(", ", Core.GameOptions.ContentPackNames))
                StringBuilder.AppendLine()
            End If

            Dim resultString As String = StringBuilder.ToString()

            Core.SpriteBatch.DrawInterfaceString(FontManager.MainFont, resultString, New Vector2(7, 7), Color.Black)
            Core.SpriteBatch.DrawInterfaceString(FontManager.MainFont, resultString, New Vector2(5, 5), Color.White)
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