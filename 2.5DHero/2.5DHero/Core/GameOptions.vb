''' <summary>
''' A class to handle the game's public options.
''' </summary>
Public Class GameOptions

    Private _dataModel As DataModel.Json.PlayerData.OptionsModel
    Public Extras As New List(Of String)
    ''' <summary>
    ''' The render distance of the game (0-4).
    ''' </summary>
    ''' <returns></returns>
    Public Property RenderDistance() As Integer
        Get
            Return _dataModel.RenderDistance
        End Get
        Set(value As Integer)
            _dataModel.RenderDistance = value
        End Set
    End Property

    ''' <summary>
    ''' If the debug menu should show.
    ''' </summary>
    ''' <returns></returns>
    Public Property ShowDebug() As Integer
        Get
            Return _dataModel.ShowDebug
        End Get
        Set(value As Integer)
            _dataModel.ShowDebug = value
        End Set
    End Property

    ''' <summary>
    ''' If the GUI is being shown ingame.
    ''' </summary>
    ''' <returns></returns>
    Public Property ShowGUI() As Boolean
        Get
            Return _dataModel.ShowGUI
        End Get
        Set(value As Boolean)
            _dataModel.ShowGUI = value
        End Set
    End Property

    ''' <summary>
    ''' The graphic style of the game (0-1).
    ''' </summary>
    ''' <returns></returns>
    Public Property GraphicStyle() As Integer
        Get
            Return _dataModel.GraphicStyle
        End Get
        Set(value As Integer)
            _dataModel.GraphicStyle = value
        End Set
    End Property

    ''' <summary>
    ''' The update rate of the offset maps.
    ''' </summary>
    ''' <returns></returns>
    Public Property LoadOffsetMaps() As Integer
        Get
            Return _dataModel.LoadOffsetMaps
        End Get
        Set(value As Integer)
            _dataModel.LoadOffsetMaps = value
        End Set
    End Property

    ''' <summary>
    ''' List of activated ContentPacks.
    ''' </summary>
    ''' <returns></returns>
    Public Property ContentPackNames() As String()
        Get
            Return _dataModel.ContentPacks
        End Get
        Set(value As String())
            _dataModel.ContentPacks = value
        End Set
    End Property

    ''' <summary>
    ''' If player head bobbing should be activated.
    ''' </summary>
    ''' <returns></returns>
    Public Property ViewBobbing() As Boolean
        Get
            Return _dataModel.ViewBobbing
        End Get
        Set(value As Boolean)
            _dataModel.ViewBobbing = value
        End Set
    End Property

    ''' <summary>
    ''' if Lighting should be enabled.
    ''' </summary>
    ''' <returns></returns>
    Public Property LightingEnabled() As Boolean
        Get
            Return _dataModel.LightingEnabled
        End Get
        Set(value As Boolean)
            _dataModel.LightingEnabled = value
        End Set
    End Property

    ''' <summary>
    ''' If the XBOX GamePad is enabled.
    ''' </summary>
    ''' <returns></returns>
    Public Property GamePadEnabled() As Boolean
        Get
            Return _dataModel.GamePadEnabled
        End Get
        Set(value As Boolean)
            _dataModel.GamePadEnabled = value
        End Set
    End Property

    ''' <summary>
    ''' If the player has started an offline game before.
    ''' </summary>
    ''' <returns></returns>
    Public Property StartedOfflineGame() As Boolean
        Get
            Return _dataModel.StartedOfflineGame
        End Get
        Set(value As Boolean)
            _dataModel.StartedOfflineGame = value
        End Set
    End Property

    ''' <summary>
    ''' The window size at startup.
    ''' </summary>
    ''' <returns></returns>
    Public Property WindowSize() As Vector2
        Get
            Return New Vector2(_dataModel.WindowSize.Width, _dataModel.WindowSize.Height)
        End Get
        Set(value As Vector2)
            _dataModel.WindowSize = New DataModel.Json.PlayerData.OptionsModel.WindowSizeModel() With {.Width = CInt(value.X), .Height = CInt(value.Y)}
        End Set
    End Property

    ''' <summary>
    ''' If the game should ignore XNA's "no sound playback device found" error message and try to play music anyways.
    ''' </summary>
    ''' <returns></returns>
    Public Property ForceMusic() As Boolean
        Get
            Return _dataModel.ForceMusic
        End Get
        Set(value As Boolean)
            _dataModel.ForceMusic = value
        End Set
    End Property

    ''' <summary>
    ''' The max iteration of Offset maps that load.
    ''' </summary>
    ''' <returns></returns>
    Public Property MaxOffsetLevel() As Integer
        Get
            Return _dataModel.MaxOffsetLevel
        End Get
        Set(value As Integer)
            _dataModel.MaxOffsetLevel = value
        End Set
    End Property

    ''' <summary>
    ''' Loads the options from the options.dat file.
    ''' </summary>
    Public Sub Load()
        KeyBindings.CreateDefaultData(False)

        If IO.Directory.Exists(GameController.GamePath & "\Save\") = False Then
            IO.Directory.CreateDirectory(GameController.GamePath & "\Save\")
        End If

        If IO.File.Exists(GameController.GamePath & "\Save\options.dat") = False Then
            'Create default data model:
            CreateDefaultData()
        Else
            'Load data model from file:
            Try
                Dim jsonData As String = IO.File.ReadAllText(GameController.GamePath & "\Save\options.dat")
                _dataModel = DataModel.Json.JsonDataModel.FromString(Of DataModel.Json.PlayerData.OptionsModel)(jsonData)
            Catch ex As Exception
                Logger.Log("300", Logger.LogTypes.Message, "Failed to load options.dat properly. Restore default settings.")
                CreateDefaultData()
            End Try

            ApplyLoadedOptions()
        End If
    End Sub

    Private Sub ApplyLoadedOptions()
        With _dataModel
            MusicPlayer.GetInstance().MasterVolume = CSng(CDbl(.Music) / 100)
            SoundManager.Volume = CSng(CDbl(.Sound) / 100)
            SoundManager.Mute(.Muted)
            MediaPlayer.IsMuted = .Muted
            Entity.drawViewBox = .ShowBoundingBoxes
            Logger.DisplayLog = .ShowDebugConsole
            Localization.Load(.Language)
            ContentPackManager.CreateContentPackFolder()

            If _dataModel.ContentPacks.Length > 0 Then
                For Each c As String In _dataModel.ContentPacks
                    If IO.Directory.Exists(GameController.GamePath & "\ContentPacks\" & c) = False Then
                        Dim cList As List(Of String) = ContentPackNames.ToList()
                        cList.Remove(c)
                        ContentPackNames = cList.ToArray()
                    Else
                        ContentPackManager.LoadTextureReplacements(GameController.GamePath & "\ContentPacks\" & c & "\exceptions.dat")
                    End If
                Next
            End If

        End With
    End Sub

    Private Sub CreateDefaultData()
        _dataModel = New DataModel.Json.PlayerData.OptionsModel() With
        {
            .Music = 50,
            .Sound = 50,
            .RenderDistance = 2,
            .ShowDebug = 0,
            .ShowBoundingBoxes = False,
            .ShowDebugConsole = False,
            .ShowGUI = True,
            .GraphicStyle = 1,
            .LoadOffsetMaps = 10,
            .Language = "en",
            .ViewBobbing = True,
            .GamePadEnabled = True,
            .LightingEnabled = True,
            .StartedOfflineGame = False,
            .PreferMultiSampling = True,
            .ContentPacks = New String() {},
            .WindowSize = New DataModel.Json.PlayerData.OptionsModel.WindowSizeModel() With {
                .Width = 1200,
                .Height = 680
            },
            .ForceMusic = False,
            .MaxOffsetLevel = 0,
            .Muted = False
        }
        InternalSave(False)
    End Sub

    Private Sub InternalSave(ByVal apply As Boolean)
        If apply Then
            _dataModel.Music = CInt(MusicPlayer.GetInstance().MasterVolume * 100)
            _dataModel.Sound = CInt(SoundManager.Volume * 100)
            _dataModel.ShowBoundingBoxes = Entity.drawViewBox
            _dataModel.Muted = MediaPlayer.IsMuted
            _dataModel.ShowDebugConsole = Logger.DisplayLog
            _dataModel.Language = Localization.LanguageSuffix
        End If

        IO.File.WriteAllText(GameController.GamePath & "\Save\options.dat", _dataModel.ToString("    "))

        'Also save the key bindings at this point:
        KeyBindings.Save()
    End Sub

    ''' <summary>
    ''' Saves the options to the options.dat file.
    ''' </summary>
    Public Sub Save()
        InternalSave(True)
    End Sub

End Class