Public Class GameOptions

    Public RenderDistance As Integer = 3
    Public ShowDebug As Integer = 0
    Public ShowGUI As Boolean = True
    Public GraphicStyle As Integer = 1
    Public LoadOffsetMaps As Integer = 10
    Public ContentPackNames() As String = {}
    Public ViewBobbing As Boolean = True
    Public LightingEnabled As Boolean = True
    Public GamePadEnabled As Boolean = True
    Public StartedOfflineGame As Boolean = False
    Public WindowSize As New Vector2(1200, 680)
    Public ForceMusic As Boolean = False
    Public MaxOffsetLevel As Integer = 0

    Public Sub LoadOptions()
        KeyBindings.CreateKeySave(False)
        If System.IO.Directory.Exists(GameController.GamePath & "\Save\") = False Then
            System.IO.Directory.CreateDirectory(GameController.GamePath & "\Save\")
        End If

        If System.IO.File.Exists(GameController.GamePath & "\Save\options.dat") = False Then
            CreateOptions()
        End If

        Dim Data() As String = System.IO.File.ReadAllText(GameController.GamePath & "\Save\options.dat").SplitAtNewline()

        Dim LanguageFound As Boolean = False

        For Each line As String In Data
            If line <> "" Then
                Dim name As String = line.GetSplit(0, "|")
                Dim value As String = line.GetSplit(1, "|")

                Select Case name.ToLower()
                    Case "volume"
                        MusicManager.MasterVolume = CSng(CDbl(value) / 100)
                        SoundManager.Volume = CSng(CDbl(value) / 100)
                    Case "music"
                        MusicManager.MasterVolume = CSng(CDbl(value) / 100)
                    Case "sound"
                        SoundManager.Volume = CSng(CDbl(value) / 100)
                    Case "muted"
                        SoundManager.Mute(CBool(value))
                        MediaPlayer.IsMuted = CBool(value)
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
                                    If System.IO.Directory.Exists(GameController.GamePath & "\ContentPacks\" & c) = False Then
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
                    Case "lightningenabled"
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
                            If IsNumeric(arg(0)) = True Then
                                Me.WindowSize.X = CSng(arg(0).Replace(".", GameController.DecSeparator)).Clamp(1, 4096)
                            End If
                            If IsNumeric(arg(1)) = True Then
                                Me.WindowSize.Y = CSng(arg(1).Replace(".", GameController.DecSeparator)).Clamp(1, 4096)
                            End If
                        End If
                    Case "forcemusic"
                        Me.ForceMusic = CBool(value)
                    Case "maxoffsetlevel"
                        Me.MaxOffsetLevel = CInt(value)
                End Select
            End If
        Next

        If LanguageFound = False Then
            Localization.Load("en")
        End If
    End Sub

    Public Sub SaveOptions()
        If MapPreviewScreen.MapViewMode = False Then
            Dim mutedString As String = MediaPlayer.IsMuted.ToNumberString()
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

            Dim Data As String = "Music|" & CInt(MusicManager.MasterVolume * 100) & vbNewLine &
                "Sound|" & CInt(SoundManager.Volume * 100) & vbNewLine &
                "Muted|" & mutedString & vbNewLine &
                "RenderDistance|" & Me.RenderDistance.ToString() & vbNewLine &
                "ShowDebug|" & showDebugString & vbNewLine &
                "ShowBoundingBoxes|" & Entity.drawViewBox.ToNumberString() & vbNewLine &
                "ShowDebugConsole|" & Logger.DisplayLog.ToNumberString() & vbNewLine &
                "ShowGUI|" & Me.ShowGUI.ToNumberString() & vbNewLine &
                "GraphicStyle|" & Me.GraphicStyle.ToString() & vbNewLine &
                "LoadOffsetMaps|" & Me.LoadOffsetMaps.ToString() & vbNewLine &
                "Language|" & Localization.LanguageSuffix & vbNewLine &
                "ViewBobbing|" & Me.ViewBobbing.ToNumberString() & vbNewLine &
                "GamePadEnabled|" & Me.GamePadEnabled.ToNumberString() & vbNewLine &
                "LightningEnabled|" & Me.LightingEnabled.ToNumberString() & vbNewLine &
                "StartedOfflineGame|" & Me.StartedOfflineGame.ToNumberString() & vbNewLine &
                "PreferMultiSampling|" & Core.GraphicsManager.PreferMultiSampling.ToNumberString() & vbNewLine &
                "ContentPacks|" & ContentPackString & vbNewLine &
                "WindowSize|" & Core.windowSize.Width.ToString() & "," & Core.windowSize.Height.ToString().Replace(GameController.DecSeparator, ".") & vbNewLine &
                "ForceMusic|" & Me.ForceMusic.ToNumberString() & vbNewLine &
                "MaxOffsetLevel|" & Me.MaxOffsetLevel.ToString()

            System.IO.File.WriteAllText(GameController.GamePath & "\Save\options.dat", Data)
            KeyBindings.SaveKeys()

            Logger.Debug("---Options saved---")
        End If
    End Sub

    Private Sub CreateOptions()
        Dim s As String = "Music|50" & vbNewLine &
            "Sound|50" & vbNewLine &
            "Muted|0" & vbNewLine &
            "RenderDistance|2" & vbNewLine &
            "ShowDebug|0" & vbNewLine &
            "ShowBoundingBoxes|0" & vbNewLine &
            "ShowDebugConsole|0" & vbNewLine &
            "ShowGUI|1" & vbNewLine &
            "GraphicStyle|1" & vbNewLine &
            "LoadOffsetMaps|10" & vbNewLine &
            "Language|en" & vbNewLine &
            "ViewBobbing|1" & vbNewLine &
            "GamePadEnabled|1" & vbNewLine &
            "LightningEnabled|1" & vbNewLine &
            "StartedOfflineGame|0" & vbNewLine &
            "PreferMultiSampling|1" & vbNewLine &
            "ContentPacks|" & vbNewLine &
            "WindowSize|1200,680" & vbNewLine &
            "ForceMusic|0" & vbNewLine &
            "MaxOffsetLevel|0"

        System.IO.File.WriteAllText(GameController.GamePath & "\Save\options.dat", s)
    End Sub

End Class