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

            Dim thirdPersonString As String = ""
            If Screen.Camera.Name = "Overworld" Then
                Dim c As OverworldCamera = CType(Screen.Camera, OverworldCamera)
                If c.ThirdPerson = True Then
                    thirdPersonString = " / " & c.ThirdPersonOffset.ToString()
                End If
            End If

            Dim s As String = GameController.GAMENAME & " " & GameController.GAMEDEVELOPMENTSTAGE & " " & GameController.GAMEVERSION & " / FPS: " & Math.Round(Core.GameInstance.FPSMonitor.Value, 0) & isDebugString & vbNewLine &
                Screen.Camera.Position.ToString() & thirdPersonString & vbNewLine & Screen.Camera.Yaw & "; " & Screen.Camera.Pitch & vbNewLine &
                "E: " & _drawnVertices.ToString() & "/" & _maxVertices.ToString() & vbNewLine &
                "C: " & _maxDistance.ToString() & " A: " & ActionscriptActive.ToString()

            If Core.GameOptions.ContentPackNames.Count() > 0 Then
                Dim contentPackString As String = ""
                For Each ContentPackName As String In Core.GameOptions.ContentPackNames
                    If contentPackString <> "" Then
                        contentPackString &= ", "
                    End If
                    contentPackString &= ContentPackName
                Next
                contentPackString = "Loaded ContentPacks: " & contentPackString
                s &= vbNewLine & contentPackString
            End If

            Core.SpriteBatch.DrawInterfaceString(FontManager.MainFont, s, New Vector2(7, 7), Color.Black)
            Core.SpriteBatch.DrawInterfaceString(FontManager.MainFont, s, New Vector2(5, 5), Color.White)
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