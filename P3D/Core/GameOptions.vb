Public Class GameOptions

    Public RenderDistance As Integer = 3
    Public ShowDebug As Integer = 0
    Public ShowGUI As Boolean = True
    Public GraphicStyle As Integer = 1
    Public LoadOffsetMaps As Integer = 1
    Public ContentPackNames() As String = {}
    Public ViewBobbing As Boolean = True
    Public LightingEnabled As Boolean = True
    Public GamePadEnabled As Boolean = True
    Public StartedOfflineGame As Boolean = False
    Public WindowSize As New Vector2(1200, 680)
    Public MaxOffsetLevel As Integer = 0
    Public UpdateDisabled As Boolean = False
    Public Extras As New List(Of String)
    Private _interfaceScale As Integer = 0

    Public Property InterfaceScale As Integer
        Get
            Return _interfaceScale
        End Get
        Set(value As Integer)
            _interfaceScale = CInt(value)
        End Set
    End Property
    Public Sub LoadOptions()
        KeyBindings.CreateKeySave(False)
        If Directory.Exists(GameController.GamePath & "\Save\") = False Then
            Directory.CreateDirectory(GameController.GamePath & "\Save\")
        End If

        If File.Exists(GameController.GamePath & "\Save\options.dat") = False Then
            CreateOptions()
        End If

        Dim Data() As String = File.ReadAllText(GameController.GamePath & "\Save\options.dat").SplitAtNewline()

        Dim LanguageFound As Boolean = False

        For Each line As String In Data
            If line <> "" Then
                Dim name As String = line.GetSplit(0, "|")
                Dim value As String = line.GetSplit(1, "|")

                Select Case name.ToLower()
                    Case "volume"
                        MusicManager.MasterVolume = CSng(CInt(value) / 100)
                        SoundManager.Volume = CSng(CInt(value) / 100)
                    Case "music"
                        MusicManager.MasterVolume = CSng(CInt(value) / 100)
                    Case "sound"
                        SoundManager.Volume = CSng(CInt(value) / 100)
                    Case "muted"
                        SoundManager.Muted = CBool(value)
                        MusicManager.Muted = CBool(value)
                    Case "renderdistance"
                        Me.RenderDistance = CInt(value)
                    Case "showdebug"
                        Me.ShowDebug = CInt(value)
                    Case "showboundingboxes"
                        Entity.drawViewBox = CBool(value)
                    Case "showdebugconsole"
                        Logger.DisplayLog = CBool(value)
                    Case "showgui"
                        Me.ShowGUI = CBool(value)
                    Case "graphicstyle"
                        Me.GraphicStyle = CInt(value)
                    Case "loadoffsetmaps"
                        Me.LoadOffsetMaps = CInt(value)
                    Case "language"
                        LanguageFound = True
                        Localization.Load(value)
                    Case "contentpack", "contentpacks"
                        ContentPackManager.CreateContentPackFolder()
                        If value <> "" Then
                            Me.ContentPackNames = value.Split(CChar(","))
                            If Me.ContentPackNames.Count > 0 Then
                                For Each c As String In Me.ContentPackNames
                                    If Directory.Exists(GameController.GamePath & "\ContentPacks\" & c) = False Then
                                        Dim cList As List(Of String) = Me.ContentPackNames.ToList()
                                        cList.Remove(c)
                                        Me.ContentPackNames = cList.ToArray()
                                    Else
                                        ContentPackManager.Load(GameController.GamePath & "\ContentPacks\" & c & "\exceptions.dat")
                                    End If
                                Next
                            End If
                        End If
                    Case "viewbobbing"
                        Me.ViewBobbing = CBool(value)
                    Case "lightingenabled", "lightningenabled"
                        Me.LightingEnabled = CBool(value)
                    Case "gamepadenabled"
                        Me.GamePadEnabled = CBool(value)
                    Case "startedofflinegame"
                        Me.StartedOfflineGame = CBool(value)
                    Case "prefermultisampling"
                        Core.GraphicsManager.PreferMultiSampling = CBool(value)
                    Case "windowsize"
                        If value.Contains(",") = True Then
                            Dim arg() As String = value.Split(CChar(","))
                            If StringHelper.IsNumeric(arg(0)) = True Then
                                Me.WindowSize.X = CSng(arg(0).Replace(".", GameController.DecSeparator)).Clamp(1, 4096)
                            End If
                            If StringHelper.IsNumeric(arg(1)) = True Then
                                Me.WindowSize.Y = CSng(arg(1).Replace(".", GameController.DecSeparator)).Clamp(1, 4096)
                            End If
                        End If
                    Case "maxoffsetlevel"
                        Me.MaxOffsetLevel = CInt(value)
                    Case "extras"
                        If Not String.IsNullOrEmpty(value) Then
                            Me.Extras = value.Split(";"c).ToList()
                        End If
                    Case "updatedisabled"
                        UpdateDisabled = CBool(value)
                    Case "interfacescale"
                        InterfaceScale = CInt(value)
                End Select
            End If
        Next

        If LanguageFound = False Then
            Localization.Load("en")
        End If
    End Sub

    Public Sub SaveOptions()
        If MapPreviewScreen.MapViewMode = False Then
            Dim mutedString As String = MusicManager.Muted.ToNumberString()
            Dim showDebugString As String = Me.ShowDebug.ToString()
            Dim ContentPackString As String = ""
            If Me.ContentPackNames.Count > 0 Then
                For Each c As String In Me.ContentPackNames
                    If ContentPackString <> "" Then
                        ContentPackString &= ","
                    End If
                    ContentPackString &= c
                Next
            End If
            If Core.windowSize.Width = 0 Then
                Core.windowSize.Width = 1280
            End If
            If Core.windowSize.Height = 0 Then
                Core.windowSize.Height = 720
            End If

            Dim Data As String = "Music|" & CInt(MusicManager.MasterVolume * 100) & Environment.NewLine &
                "Sound|" & CInt(SoundManager.Volume * 100) & Environment.NewLine &
                "Muted|" & mutedString & Environment.NewLine &
                "RenderDistance|" & Me.RenderDistance.ToString() & Environment.NewLine &
                "ShowDebug|" & showDebugString & Environment.NewLine &
                "ShowBoundingBoxes|" & Entity.drawViewBox.ToNumberString() & Environment.NewLine &
                "ShowDebugConsole|" & Logger.DisplayLog.ToNumberString() & Environment.NewLine &
                "ShowGUI|" & Me.ShowGUI.ToNumberString() & Environment.NewLine &
                "GraphicStyle|" & Me.GraphicStyle.ToString() & Environment.NewLine &
                "LoadOffsetMaps|" & Me.LoadOffsetMaps.ToString() & Environment.NewLine &
                "Language|" & Localization.LanguageSuffix & Environment.NewLine &
                "ViewBobbing|" & Me.ViewBobbing.ToNumberString() & Environment.NewLine &
                "GamePadEnabled|" & Me.GamePadEnabled.ToNumberString() & Environment.NewLine &
                "LightingEnabled|" & Me.LightingEnabled.ToNumberString() & Environment.NewLine &
                "StartedOfflineGame|" & Me.StartedOfflineGame.ToNumberString() & Environment.NewLine &
                "PreferMultiSampling|" & Core.GraphicsManager.PreferMultiSampling.ToNumberString() & Environment.NewLine &
                "ContentPacks|" & ContentPackString & Environment.NewLine &
                "WindowSize|" & Core.windowSize.Width.ToString() & "," & Core.windowSize.Height.ToString() & Environment.NewLine &
                "MaxOffsetLevel|" & Me.MaxOffsetLevel.ToString() & Environment.NewLine &
                "UpdateDisabled|" & Me.UpdateDisabled.ToNumberString() & Environment.NewLine &
                "InterfaceScale|" & Me.InterfaceScale.ToString() & Environment.NewLine &
                "Extras|" & String.Join(";", Me.Extras)

            File.WriteAllText(GameController.GamePath & "\Save\options.dat", Data)
            KeyBindings.SaveKeys()

            Logger.Debug("---Options saved---")
        End If
    End Sub

    Private Sub CreateOptions()
        Dim s As String = "Music|50" & Environment.NewLine &
            "Sound|50" & Environment.NewLine &
            "Muted|0" & Environment.NewLine &
            "RenderDistance|2" & Environment.NewLine &
            "ShowDebug|0" & Environment.NewLine &
            "ShowBoundingBoxes|0" & Environment.NewLine &
            "ShowDebugConsole|0" & Environment.NewLine &
            "ShowGUI|1" & Environment.NewLine &
            "GraphicStyle|1" & Environment.NewLine &
            "LoadOffsetMaps|1" & Environment.NewLine &
            "Language|en" & Environment.NewLine &
            "ViewBobbing|1" & Environment.NewLine &
            "GamePadEnabled|1" & Environment.NewLine &
            "LightingEnabled|1" & Environment.NewLine &
            "StartedOfflineGame|0" & Environment.NewLine &
            "PreferMultiSampling|1" & Environment.NewLine &
            "ContentPacks|" & Environment.NewLine &
            "WindowSize|1200,680" & Environment.NewLine &
            "MaxOffsetLevel|0" & Environment.NewLine &
            "UpdateDisabled|0" & Environment.NewLine &
            "InterfaceScale|0" & Environment.NewLine &
            "Extras|"

        File.WriteAllText(GameController.GamePath & "\Save\options.dat", s)
    End Sub

End Class