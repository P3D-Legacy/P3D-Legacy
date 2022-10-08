Public Class MainMenuScreen

    Inherits Screen

    Dim mainmenuIndex As Integer = 0
    Dim loadMenuIndex(3) As Integer
    Dim languageMenuIndex(3) As Integer
    Dim packsMenuIndex(3) As Integer
    Dim gameModeMenuIndex(3) As Integer
    Dim packInfoIndex As Integer = 0
    Dim deleteIndex As Integer = 0
    Public menuIndex As Integer = 0
    Dim loadGameJoltIndex As Integer = 0

    Dim currentLevel As Integer = -1
    Dim levelChangeDelay As Integer = 0
    'Dim mainMenuMaps As DataModel.Json.Game.MainMenuMapModel()

    Dim mainTexture As Texture2D

    Dim Saves As New List(Of String)
    Dim SaveNames As New List(Of String)

    Dim Languages As New List(Of String)
    Dim LanguageNames As New List(Of String)
    Dim currentLanguage As String = "en"

    Dim PackNames As New List(Of String)
    Dim EnabledPackNames As New List(Of String)

    Dim ModeNames As New List(Of String)

    Dim tempLoadDisplay As String = ""

    Public Overrides Function GetScreenStatus() As String
        Dim s As String = "MenuIndex=" & menuIndex & vbNewLine &
            "CurrentLevel=" & currentLevel & vbNewLine &
            "LevelChangeDelay=" & levelChangeDelay.ToString()

        Return s
    End Function

    Public Sub New()
        GameModeManager.SetGameModePointer("Kolben")

        Identification = Identifications.MainMenuScreen
        CanBePaused = False
        MouseVisible = True
        CanChat = False
        currentLanguage = Localization.LanguageSuffix

        TextBox.Showing = False
        PokemonImageView.Showing = False
        ChooseBox.Showing = False

        Effect = New BasicEffect(GraphicsDevice)
        Effect.FogEnabled = True
        SkyDome = New SkyDome()
        Camera = New MainMenuCamera()

        renderTarget = New RenderTarget2D(Core.GraphicsDevice, Core.windowSize.Width, Core.windowSize.Height, False, Core.GraphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.Depth24)
        blurEffect = Core.Content.Load(Of Effect)("Effects\BlurEffect")

        Core.Player.Skin = "Hilbert"
        Level = New Level()
        ChangeLevel()

        mainTexture = TextureManager.GetTexture("GUI\Menus\Menu")

        Level.World.Initialize(Level.EnvironmentType, Level.WeatherType)

        If IO.Directory.Exists(GameController.GamePath & "\Save\") = False Then
            IO.Directory.CreateDirectory(GameController.GamePath & "\Save\")
        End If

        GetSaves()
        GetLanguages()
        GetPacks()

        GameJolt.Emblem.ClearOnlineSpriteCache()
    End Sub

    Private Sub GetPacks(Optional ByVal reload As Boolean = False)
        PackNames.Clear()

        If reload = False Then
            EnabledPackNames.Clear()
            EnabledPackNames.AddRange(Core.GameOptions.ContentPackNames)
        End If

        PackNames.AddRange(EnabledPackNames)

        If IO.Directory.Exists(GameController.GamePath & "\ContentPacks\") = True Then
            For Each ContentPackFolder As String In IO.Directory.GetDirectories(GameController.GamePath & "\ContentPacks\")
                Dim newContentPack As String = ContentPackFolder.Remove(0, (GameController.GamePath & "\ContentPacks\").Length)
                If PackNames.Contains(newContentPack) = False Then
                    PackNames.Add(newContentPack)
                End If
            Next
        End If
    End Sub

    Private Sub GetLanguages()
        Languages.Clear()
        LanguageNames.Clear()

        For Each file As String In IO.Directory.GetFiles(GameController.GamePath & "\Content\Localization\")
            If file.EndsWith(".dat") = True Then
                Dim content() As String = IO.File.ReadAllLines(file)
                file = IO.Path.GetFileNameWithoutExtension(file)

                If file.StartsWith("Tokens_") = True Then
                    Dim TokenName As String = file.Remove(0, 7)
                    Dim LanguageName As String = ""

                    For Each line As String In content
                        If line.StartsWith("language_name,") = True Then
                            LanguageName = content(0).GetSplit(1)

                            Languages.Add(TokenName)
                            LanguageNames.Add(LanguageName)
                            Exit For
                        End If
                    Next
                End If
            End If
        Next
    End Sub

    Private Sub GetSaves()
        If IO.File.Exists(GameController.GamePath & "\Save\lastSession.id") = True Then
            Dim idData As String = IO.File.ReadAllText(GameController.GamePath & "\Save\lastSession.id")
            If IO.Directory.Exists(GameController.GamePath & "\Save\" & idData) = False Then
                IO.File.Delete(GameController.GamePath & "\Save\lastSession.id")
            End If
        End If

        Saves.Clear()
        SaveNames.Clear()

        For Each Folder As String In IO.Directory.GetDirectories(GameController.GamePath & "\Save")
            If Player.IsSaveGameFolder(Folder) = True Then
                Saves.Add(Folder)
            End If
        Next

        For i = 0 To Saves.Count - 1
            If i <= Saves.Count - 1 Then
                Dim entry As String = Saves(i)

                Dim Data() As String = IO.File.ReadAllText(entry & "\Player.dat").SplitAtNewline()
                Dim Name As String = "Missingno."
                Dim Autosave As Boolean = False

                For Each Line As String In Data
                    If Line.StartsWith("Name|") = True Then
                        Name = Line.GetSplit(1, "|")
                    End If
                    If Line.StartsWith("AutoSave|") = True Then
                        Autosave = True
                    End If
                Next

                If Autosave = True Then
                    Saves.RemoveAt(i)
                    i -= 1
                Else
                    SaveNames.Add(Name)
                End If
            End If
        Next
    End Sub

    Private Sub GetGameModes()
        ModeNames.Clear()

        For Each folder As String In IO.Directory.GetDirectories(GameController.GamePath & "\GameModes\")
            If IO.File.Exists(folder & "\GameMode.dat") = True Then
                Dim directory As String = folder
                If directory.EndsWith("\") = True Then
                    directory = directory.Remove(directory.Length - 1, 1)
                End If
                directory = directory.Remove(0, directory.LastIndexOf("\") + 1)

                ModeNames.Add(directory)
            End If
        Next
    End Sub

    Private Sub ChangeLevel()
        Dim levelCount As Integer = 0
        For Each levelPath As String In System.IO.Directory.GetFiles(GameController.GamePath & "\maps\mainmenu\")
            Dim levelFile As String = System.IO.Path.GetFileName(levelPath)
            If levelFile.StartsWith("mainmenu") = True And levelFile.EndsWith(".dat") = True Then
                levelCount += 1
            End If
        Next

        Dim levelID As Integer = Core.Random.Next(0, levelCount)

        If levelCount > 1 Then
            While levelID = currentLevel
                levelID = Core.Random.Next(0, levelCount)
            End While
        End If

        Select Case levelID
            Case 0
                Camera.Position = New Vector3(13, 2, 14)
            Case 1
                Camera.Position = New Vector3(23, 2, 10)
            Case 2
                Camera.Position = New Vector3(23, 2, 12)
            Case 3
                Camera.Position = New Vector3(24, 2, 14)
        End Select

        If Me.currentLevel <> levelID Then
            Me.currentLevel = levelID
            Level.Load("mainmenu\mainmenu" & levelID & ".dat")
        End If

        levelChangeDelay = 1000
    End Sub

    Public Overrides Sub Update()
        Lighting.UpdateLighting(Effect)

        Camera.Update()
        Level.Update()
        SkyDome.Update()
        Level.World.Initialize(Level.EnvironmentType, Level.WeatherType)

        If GameInstance.IsActive = True Then
            Select Case menuIndex
                Case 0
                    UpdateMainMenu()
                Case 1
                    UpdateLoadMenu()
                Case 2
                    UpdateDeleteMenu()
                Case 3
                    UpdateLanguageMenu()
                Case 4
                    UpdatePacksMenu()
                Case 5
                    UpdatePackInformationMenu()
                Case 6
                    UpdateNewGameMenu()
                Case 7
                    UpdateLoadGameJoltSaveMenu()
            End Select
        End If

        If levelChangeDelay <= 0 Then
            If Random.Next(0, 1000) = 0 Then
                ChangeLevel()
            End If
        Else
            levelChangeDelay -= 1
        End If
    End Sub

    Dim renderTarget As RenderTarget2D
    Dim blurEffect As Effect

#Region "MainMenu"

    Public Overrides Sub Draw()
        'Core.GraphicsDevice.SetRenderTarget(renderTarget)

        'Core.GraphicsDevice.Clear(Core.BackgroundColor)

        SkyDome.Draw(45.0F)
        Level.Draw()
        World.DrawWeather(Level.World.CurrentMapWeather)

        'Core.GraphicsDevice.SetRenderTarget(Nothing)

        'Core.SpriteBatch.EndBatch()

        'blurEffect.CurrentTechnique = blurEffect.Techniques("GaussianBlur")
        'blurEffect.Parameters("TextureWidth").SetValue(Core.windowSize.Width)

        'Core.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise, blurEffect)

        'Core.SpriteBatch.Draw(renderTarget, New Vector2(0, 0), Color.White)

        'Core.SpriteBatch.End()
        'Core.SpriteBatch.BeginBatch()

        Select Case menuIndex
            Case 0
                DrawMainMenu()
            Case 1
                DrawLoadMenu()
            Case 2
                DrawDeleteMenu()
            Case 3
                DrawLanguageMenu()
            Case 4
                DrawPacksMenu()
            Case 5
                DrawPackInformationMenu()
            Case 6
                DrawNewGameMenu()
            Case 7
                DrawLoadGameJoltSaveMenu()
        End Select
        SpriteBatch.DrawInterfaceString(FontManager.InGameFont, GameController.DEVELOPER_NAME, New Vector2(7, ScreenSize.Height - FontManager.InGameFont.MeasureString(GameController.DEVELOPER_NAME).Y - 1), Color.Black)
        SpriteBatch.DrawInterfaceString(FontManager.InGameFont, GameController.DEVELOPER_NAME, New Vector2(4, ScreenSize.Height - FontManager.InGameFont.MeasureString(GameController.DEVELOPER_NAME).Y - 4), Color.White)
        SpriteBatch.DrawInterface(TextureManager.GetTexture("GUI\Logos\P3D"), New Rectangle(CInt(ScreenSize.Width / 2) - 260, 40, 500, 110), Color.White)

        If Core.GameOptions.ShowDebug = 0 Then
            Dim s As String = GameController.GAMENAME & " " & GameController.GAMEDEVELOPMENTSTAGE & " " & GameController.GAMEVERSION
            SpriteBatch.DrawInterfaceString(FontManager.MainFont, s, New Vector2(7, 7), Color.Black)
            SpriteBatch.DrawInterfaceString(FontManager.MainFont, s, New Vector2(5, 5), Color.White)
        End If
    End Sub

    Private Sub DrawMainMenu()
        Dim CanvasTexture As Texture2D

        For i = 0 To 7
            Dim Text As String = ""
            Select Case i
                Case 0
                    Text = Localization.GetString("main_menu_continue")
                Case 1
                    Text = Localization.GetString("main_menu_load_game")
                Case 2
                    Text = Localization.GetString("main_menu_new_game")
                Case 3
                    Text = Localization.GetString("main_menu_quit_game")
                Case 7
                    Text = "Play online"
            End Select

            If i = mainmenuIndex Then
                CanvasTexture = TextureManager.GetTexture("GUI\Menus\Menu", New Rectangle(0, 48, 48, 48), "")
            Else
                If i < 2 And Saves.Count = 0 Or i = 0 And IO.Directory.Exists(GameController.GamePath & "\Save\autosave") = False Then
                    CanvasTexture = TextureManager.GetTexture("GUI\Menus\Menu", New Rectangle(48, 0, 48, 48), "")
                Else
                    CanvasTexture = TextureManager.GetTexture("GUI\Menus\Menu", New Rectangle(0, 0, 48, 48), "")
                End If
            End If

            If i = 4 Then
                If i = mainmenuIndex Then
                    SpriteBatch.DrawInterface(mainTexture, New Rectangle(ScreenSize.Width - 64, 0, 64, 64), New Rectangle(96, 80, 16, 16), Color.White)
                Else
                    SpriteBatch.DrawInterface(mainTexture, New Rectangle(ScreenSize.Width - 64, 0, 64, 64), New Rectangle(96, 64, 16, 16), Color.White)
                End If
            ElseIf i = 5 Then
                If i = mainmenuIndex Then
                    SpriteBatch.DrawInterface(mainTexture, New Rectangle(ScreenSize.Width - 64, 64, 64, 64), New Rectangle(112, 80, 16, 16), Color.White)
                Else
                    SpriteBatch.DrawInterface(mainTexture, New Rectangle(ScreenSize.Width - 64, 64, 64, 64), New Rectangle(112, 64, 16, 16), Color.White)
                End If
            ElseIf i = 6 Then
                If Security.FileValidation.IsValid(False) = True Then
                    If GameJolt.API.LoggedIn = True Then
                        If i = mainmenuIndex Then
                            SpriteBatch.DrawInterface(mainTexture, New Rectangle(ScreenSize.Width - 196, ScreenSize.Height - 60, 192, 56), New Rectangle(160, 96, 96, 28), Color.White)
                        Else
                            SpriteBatch.DrawInterface(mainTexture, New Rectangle(ScreenSize.Width - 196, ScreenSize.Height - 60, 192, 56), New Rectangle(160, 65, 96, 28), Color.White)
                        End If
                        SpriteBatch.DrawInterfaceString(FontManager.MiniFont, "Logged in as", New Vector2(ScreenSize.Width - 148, ScreenSize.Height - 54), Color.White)
                        SpriteBatch.DrawInterfaceString(FontManager.MiniFont, GameJolt.API.username, New Vector2(ScreenSize.Width - 148, ScreenSize.Height - 34), New Color(204, 255, 0))
                    Else
                        If i = mainmenuIndex Then
                            SpriteBatch.DrawInterface(mainTexture, New Rectangle(ScreenSize.Width - 60, ScreenSize.Height - 60, 56, 56), New Rectangle(129, 96, 28, 28), Color.White)
                        Else
                            SpriteBatch.DrawInterface(mainTexture, New Rectangle(ScreenSize.Width - 60, ScreenSize.Height - 60, 56, 56), New Rectangle(129, 65, 28, 28), Color.White)
                        End If
                    End If
                Else
                    SpriteBatch.DrawInterfaceString(FontManager.MiniFont, "File Validation failed. Download a new copy of the game to fix this.", New Vector2(220, ScreenSize.Height - 30), Color.White)
                End If
            ElseIf i = 7 Then
                If GameJolt.API.LoggedIn = True And Security.FileValidation.IsValid(False) = True Then
                    Canvas.DrawImageBorder(CanvasTexture, 2, New Rectangle(CInt(ScreenSize.Width / 2), 160 + 128, 320, 64), True)
                    SpriteBatch.DrawInterfaceString(FontManager.InGameFont, Text, New Vector2(CInt(ScreenSize.Width / 2) - (FontManager.InGameFont.MeasureString(Text).X / 2) + 160 + 20, 196 + 128), Color.Black)
                End If
            ElseIf i = 1 And GameJolt.API.LoggedIn = True Then
                Canvas.DrawImageBorder(CanvasTexture, 2, New Rectangle(CInt(ScreenSize.Width / 2) - 180 - 160 - 20, 160 + i * 128, 320, 64), True)
                SpriteBatch.DrawInterfaceString(FontManager.InGameFont, Text, New Vector2(CInt(ScreenSize.Width / 2) - (FontManager.InGameFont.MeasureString(Text).X / 2) - 10 - 160 - 20, 196 + i * 128), Color.Black)
            Else
                Canvas.DrawImageBorder(CanvasTexture, 2, New Rectangle(CInt(ScreenSize.Width / 2) - 180, 160 + i * 128, 320, 64), True)
                SpriteBatch.DrawInterfaceString(FontManager.InGameFont, Text, New Vector2(CInt(ScreenSize.Width / 2) - (FontManager.InGameFont.MeasureString(Text).X / 2) - 10, 196 + i * 128), Color.Black)
            End If
        Next

        Dim d As New Dictionary(Of Buttons, String)
        d.Add(Buttons.A, "Accept")
        If GameJolt.API.LoggedIn = True Then
            DrawGamePadControls(d, New Vector2(ScreenSize.Width - 170, ScreenSize.Height - 100))
        Else
            DrawGamePadControls(d, New Vector2(ScreenSize.Width - 234, ScreenSize.Height - 40))
        End If
    End Sub

    Private Sub UpdateMainMenu()
        If Controls.Up(True, True) = True Then
            mainmenuIndex -= 1
        End If
        If Controls.Down(True, True) = True Then
            mainmenuIndex += 1
        End If

        If GameInstance.IsMouseVisible = True Then
            For i = 0 To 7
                If i = 4 Then
                    If ScaleScreenRec(New Rectangle(ScreenSize.Width - 64, 0, 64, 64)).Contains(MouseHandler.MousePosition) = True Then
                        mainmenuIndex = 4

                        If MouseHandler.ButtonPressed(MouseHandler.MouseButtons.LeftButton) = True Then
                            LanguageButton()
                        End If
                    End If
                ElseIf i = 5 Then
                    If ScaleScreenRec(New Rectangle(ScreenSize.Width - 64, 64, 64, 64)).Contains(MouseHandler.MousePosition) = True Then
                        mainmenuIndex = 5

                        If MouseHandler.ButtonPressed(MouseHandler.MouseButtons.LeftButton) = True Then
                            PacksButton()
                        End If
                    End If
                ElseIf i = 6 Then
                    Dim r As Rectangle = ScaleScreenRec(New Rectangle(ScreenSize.Width - 196, ScreenSize.Height - 60, 192, 56))
                    If GameJolt.API.LoggedIn = False Then
                        r = ScaleScreenRec(New Rectangle(ScreenSize.Width - 64, ScreenSize.Height - 64, 64, 64))
                    End If

                    If r.Contains(MouseHandler.MousePosition) = True Then
                        mainmenuIndex = 6

                        If MouseHandler.ButtonPressed(MouseHandler.MouseButtons.LeftButton) = True Then
                            GameJoltButton()
                        End If
                    End If
                ElseIf i = 1 And GameJolt.API.LoggedIn = True Then
                    If ScaleScreenRec(New Rectangle(CInt(ScreenSize.Width / 2) - 180 - 160 - 20, 160 + i * 128, 320 + 32, 64 + 32)).Contains(MouseHandler.MousePosition) = True Then
                        mainmenuIndex = i
                        If MouseHandler.ButtonPressed(MouseHandler.MouseButtons.LeftButton) = True Then
                            LoadGameButton()
                        End If
                    End If
                ElseIf i = 7 And GameJolt.API.LoggedIn = True Then
                    If ScaleScreenRec(New Rectangle(CInt(ScreenSize.Width / 2), 160 + 128, 320 + 32, 64 + 32)).Contains(MouseHandler.MousePosition) = True Then
                        mainmenuIndex = i
                        If MouseHandler.ButtonPressed(MouseHandler.MouseButtons.LeftButton) = True Then
                            LoadGameJoltButton()
                        End If
                    End If
                Else
                    If ScaleScreenRec(New Rectangle(CInt(ScreenSize.Width / 2) - 180, 160 + i * 128, 320 + 32, 64 + 32)).Contains(MouseHandler.MousePosition) = True Then
                        mainmenuIndex = i

                        If MouseHandler.ButtonPressed(MouseHandler.MouseButtons.LeftButton) = True Then
                            Select Case mainmenuIndex
                                Case 0
                                    ContinueButton()
                                Case 1
                                    LoadGameButton()
                                Case 2
                                    NewGameButton()
                                Case 3
                                    CloseGameButton()
                            End Select
                        End If
                    End If
                End If
            Next
        End If

        If Security.FileValidation.IsValid(False) = True Then
            If GameJolt.API.LoggedIn = True Then
                mainmenuIndex = CInt(MathHelper.Clamp(mainmenuIndex, 0, 7))
            Else
                mainmenuIndex = CInt(MathHelper.Clamp(mainmenuIndex, 0, 6))
            End If
        Else
            mainmenuIndex = mainmenuIndex.Clamp(0, 5)
        End If

        If Controls.Accept(False, True) = True Then
            Select Case mainmenuIndex
                Case 0
                    ContinueButton()
                Case 1
                    LoadGameButton()
                Case 2
                    NewGameButton()
                Case 3
                    CloseGameButton()
                Case 4
                    LanguageButton()
                Case 5
                    PacksButton()
                Case 6
                    GameJoltButton()
                Case 7
                    LoadGameJoltButton()
            End Select
        End If
    End Sub

    Private Sub PacksButton()
        GetPacks()
        packsMenuIndex(0) = 0
        packsMenuIndex(2) = 0

        menuIndex = 4
    End Sub

    Private Sub LanguageButton()
        GetLanguages()
        If Languages.Contains(currentLanguage) = True Then
            languageMenuIndex(0) = Languages.IndexOf(currentLanguage)
        End If

        menuIndex = 3
    End Sub

    Private Sub ContinueButton()
        If Saves.Count > 0 And Player.IsSaveGameFolder(GameController.GamePath & "\Save\autosave") = True Then
            Core.Player.IsGameJoltSave = False
            Core.Player.LoadGame("autosave")

            SetScreen(New JoinServerScreen(Me))
        End If
    End Sub

    Private Sub LoadGameButton()
        GetSaves()

        If Saves.Count > 0 Then
            menuIndex = 1
        End If
    End Sub

    Private Sub LoadGameJoltButton()
        If Security.FileValidation.IsValid(False) = True Then
            If GameJolt.API.LoggedIn = True Then
                GameJoltSave.DownloadSave(GameJolt.LogInScreen.LoadedGameJoltID, True)
            End If

            menuIndex = 7
        End If
    End Sub

    Private Sub CloseGameButton()
        Core.GameOptions.SaveOptions()
        GameInstance.Exit()
    End Sub

    Private Sub GameJoltButton()
        If Security.FileValidation.IsValid(False) = True Then
            SetScreen(New GameJolt.LogInScreen(Me))
        End If
    End Sub

#End Region

#Region "LoadMenu"

    Private Sub DrawLoadMenu()
        Dim CanvasTexture As Texture2D
        CanvasTexture = TextureManager.GetTexture("GUI\Menus\Menu", New Rectangle(0, 0, 48, 48), "")

        For i = 0 To 3
            Dim c As Color = Color.White
            If i + loadMenuIndex(2) = loadMenuIndex(0) Then
                c = New Color(101, 142, 255)
            End If

            Canvas.DrawRectangle(New Rectangle(CInt(ScreenSize.Width / 2) - 258, 180 + i * 50, 480, 48), c, True)
        Next

        Canvas.DrawScrollBar(New Vector2(CInt(ScreenSize.Width / 2) + 250, 180), Saves.Count, 4, loadMenuIndex(2), New Size(4, 200), False, New Color(190, 190, 190), New Color(63, 63, 63), True)

        Dim x As Integer = Saves.Count - 1
        x = CInt(MathHelper.Clamp(x, 0, 3))

        For i = 0 To x
            Dim Name As String = SaveNames(i + loadMenuIndex(2))

            If i + loadMenuIndex(2) = loadMenuIndex(0) Then
                SpriteBatch.DrawInterfaceString(FontManager.InGameFont, Name, New Vector2(CInt(ScreenSize.Width / 2) - 245, 191 + i * 50), Color.Black)
                SpriteBatch.DrawInterfaceString(FontManager.InGameFont, Name, New Vector2(CInt(ScreenSize.Width / 2) - 248, 188 + i * 50), Color.White)
            Else
                SpriteBatch.DrawInterfaceString(FontManager.InGameFont, Name, New Vector2(CInt(ScreenSize.Width / 2) - 248, 188 + i * 50), Color.Black)
            End If
        Next

        Canvas.DrawImageBorder(CanvasTexture, 2, New Rectangle(CInt(ScreenSize.Width / 2) - 272, 388, 512, 128), True)

        If tempLoadDisplay = "" Then
            Dim dispName As String = "(Unknown)"
            Dim dispBadges As String = "(Unknown)"
            Dim dispPlayTime As String = "(Unknown)"
            Dim dispLocation As String = "(Unknown)"
            Dim dispGameMode As String = "Kolben"

            Dim Data() As String = IO.File.ReadAllText(Saves(loadMenuIndex(0)) & "\Player.dat").SplitAtNewline()
            For Each Line As String In Data
                If Line.Contains("|") = True Then
                    Dim ID As String = Line.Remove(Line.IndexOf("|"))
                    Dim Value As String = Line.Remove(0, Line.IndexOf("|") + 1)
                    Select Case ID
                        Case "Name"
                            dispName = Value
                        Case "Badges"
                            Dim bCount As Integer = 0
                            If Value = "0" Then
                                bCount = 0
                            Else
                                If Value.Contains(",") = False Then
                                    bCount = 1
                                Else
                                    Dim s() As String = Value.Split(CChar(","))
                                    bCount = s.Length
                                End If
                            End If
                            dispBadges = bCount.ToString()
                        Case "PlayTime"
                            Dim dd() As String = Value.Split(CChar(","))

                            Dim tSpan As TimeSpan = Nothing
                            If dd.Count = 3 Then
                                tSpan = New TimeSpan(CInt(dd(0)), CInt(dd(1)), CInt(dd(2)))
                            ElseIf dd.Count = 4 Then
                                tSpan = New TimeSpan(CInt(dd(3)), CInt(dd(0)), CInt(dd(1)), CInt(dd(2)))
                            End If

                            dispPlayTime = TimeHelpers.GetDisplayTime(tSpan, True)
                        Case "location"
                            dispLocation = Value
                        Case "GameMode"
                            dispGameMode = Value
                    End Select
                End If
            Next

            tempLoadDisplay = Localization.GetString("load_menu_name") & ": " & dispName & vbNewLine &
                Localization.GetString("load_menu_gamemode") & ": " & dispGameMode & vbNewLine &
                Localization.GetString("load_menu_badges") & ": " & dispBadges & vbNewLine &
                Localization.GetString("load_menu_location") & ": " & Localization.GetString("Places_" & dispLocation) & vbNewLine &
                Localization.GetString("load_menu_time") & ": " & dispPlayTime
        End If

        SpriteBatch.DrawInterfaceString(FontManager.MiniFont, tempLoadDisplay, New Vector2(CInt(ScreenSize.Width / 2) - 252, 416), Color.Black)

        For i = 0 To 2
            Dim Text As String = ""
            Select Case i
                Case 0
                    Text = Localization.GetString("load_menu_load")
                Case 1
                    Text = Localization.GetString("load_menu_delete")
                Case 2
                    Text = Localization.GetString("load_menu_back")
            End Select

            If i = loadMenuIndex(1) Then
                CanvasTexture = TextureManager.GetTexture("GUI\Menus\Menu", New Rectangle(0, 48, 48, 48), "")
            Else
                CanvasTexture = TextureManager.GetTexture("GUI\Menus\Menu", New Rectangle(0, 0, 48, 48), "")
            End If

            Canvas.DrawImageBorder(CanvasTexture, 2, New Rectangle(CInt(ScreenSize.Width / 2) - 272 + i * 192, 550, 128, 64), True)
            SpriteBatch.DrawInterfaceString(FontManager.InGameFont, Text, New Vector2(CInt(ScreenSize.Width / 2) - 254 + i * 192, 582), Color.Black)
        Next

        Dim d As New Dictionary(Of Buttons, String)
        d.Add(Buttons.A, "Accept")
        d.Add(Buttons.B, "Back")
        DrawGamePadControls(d)
    End Sub

    Private Sub UpdateLoadMenu()
        If Controls.Up(True, True, True) = True Then
            loadMenuIndex(0) -= 1
            If loadMenuIndex(0) - loadMenuIndex(2) < 0 Then
                loadMenuIndex(2) -= 1
            End If
            tempLoadDisplay = ""
        End If
        If Controls.Down(True, True, True) = True Then
            loadMenuIndex(0) += 1
            If loadMenuIndex(0) + loadMenuIndex(2) > 3 Then
                loadMenuIndex(2) += 1
            End If
            tempLoadDisplay = ""
        End If

        If GameInstance.IsMouseVisible = True Then
            For i = 0 To 2
                If ScaleScreenRec(New Rectangle(CInt(ScreenSize.Width / 2) - 272 + i * 192, 550, 128 + 32, 64 + 32)).Contains(MouseHandler.MousePosition) = True Then
                    loadMenuIndex(1) = i

                    If MouseHandler.ButtonPressed(MouseHandler.MouseButtons.LeftButton) = True Then
                        Select Case loadMenuIndex(1)
                            Case 0
                                Core.Player.IsGameJoltSave = False
                                Core.Player.LoadGame(IO.Path.GetFileName(Saves(loadMenuIndex(0))))

                                SetScreen(New JoinServerScreen(Me))
                            Case 1
                                menuIndex = 2
                            Case 2
                                menuIndex = 0

                                tempLoadDisplay = ""
                        End Select
                    End If
                End If
            Next
        End If

        For i = 0 To 3
            If ScaleScreenRec(New Rectangle(CInt(ScreenSize.Width / 2) - 258, 180 + i * 50, 480, 48)).Contains(MouseHandler.MousePosition) = True Then
                If MouseHandler.ButtonPressed(MouseHandler.MouseButtons.LeftButton) = True Then
                    loadMenuIndex(0) = i + loadMenuIndex(2)
                    tempLoadDisplay = ""
                End If
            End If
        Next

        loadMenuIndex(0) = CInt(MathHelper.Clamp(loadMenuIndex(0), 0, Saves.Count - 1))
        loadMenuIndex(2) = CInt(MathHelper.Clamp(loadMenuIndex(2), 0, Saves.Count - 4))

        If Controls.Right(True, True, False) = True Then
            loadMenuIndex(1) += 1
        End If
        If Controls.Left(True, True, False) = True Then
            loadMenuIndex(1) -= 1
        End If

        loadMenuIndex(1) = CInt(MathHelper.Clamp(loadMenuIndex(1), 0, 2))

        If Controls.Accept(False, True) = True Then
            Select Case loadMenuIndex(1)
                Case 0
                    Core.Player.IsGameJoltSave = False
                    Core.Player.LoadGame(IO.Path.GetFileName(Saves(loadMenuIndex(0))))

                    SetScreen(New JoinServerScreen(Me))
                Case 1
                    menuIndex = 2
                Case 2
                    menuIndex = 0

                    tempLoadDisplay = ""
            End Select
        End If

        If Controls.Dismiss() = True Then
            menuIndex = 0
        End If
    End Sub

#End Region

#Region "LoadGameJoltSaveMenu"

    Private Sub DrawLoadGameJoltSaveMenu()
        If GameJoltSave.DownloadFailed = False Then
            Dim downloaded As Boolean = GameJoltSave.DownloadFinished

            If downloaded = True Then
                Dim r As New Rectangle(CInt(ScreenSize.Width / 2) - 256, 300, 512, 128)
                If ScaleScreenRec(r).Contains(MouseHandler.MousePosition) = True And GameInstance.IsMouseVisible = True Or GameInstance.IsMouseVisible = False And loadGameJoltIndex = 0 Then
                    Canvas.DrawRectangle(ScaleScreenRec(New Rectangle(CInt(ScreenSize.Width / 2) - 264, 292, 528, 144)), New Color(255, 255, 255, 150))
                End If

                If GameJolt.LogInScreen.UserBanned(GameJoltSave.GameJoltID) = True Then
                    Dim reason As String = GameJolt.LogInScreen.GetBanReasonByID(GameJolt.LogInScreen.BanReasonIDForUser(GameJoltSave.GameJoltID))
                    SpriteBatch.DrawInterfaceString(FontManager.MainFont, reason, New Vector2(CSng(ScreenSize.Width / 2 - FontManager.MainFont.MeasureString(reason).X / 2) + 2, 260 + 2), Color.Black)
                    SpriteBatch.DrawInterfaceString(FontManager.MainFont, reason, New Vector2(CSng(ScreenSize.Width / 2 - FontManager.MainFont.MeasureString(reason).X / 2), 260), Color.Red)
                End If

                GameJolt.Emblem.Draw(GameJolt.API.username, GameJoltSave.GameJoltID, GameJoltSave.Points, GameJoltSave.Gender, GameJoltSave.Emblem, ScaleScreenVec(New Vector2(CSng(ScreenSize.Width / 2) - 256, 300)), CSng(4 * SpriteBatch.InterfaceScale), GameJoltSave.DownloadedSprite)

                Dim y As Integer = 0
                If ScaleScreenRec(New Rectangle(r.X + 32 + r.Width, r.Y, 32, 32)).Contains(MouseHandler.MousePosition) = True And GameInstance.IsMouseVisible = True Or GameInstance.IsMouseVisible = False And loadGameJoltIndex = 1 Then
                    y = 16

                    SpriteBatch.DrawInterfaceString(FontManager.MiniFont, "Change to male.", New Vector2(r.X + 64 + 4 + r.Width, r.Y + 4), Color.White)
                End If
                SpriteBatch.DrawInterface(mainTexture, New Rectangle(r.X + 32 + r.Width, r.Y, 32, 32), New Rectangle(144, 32 + y, 16, 16), Color.White)

                y = 0
                If ScaleScreenRec(New Rectangle(r.X + 32 + r.Width, r.Y + 48, 32, 32)).Contains(MouseHandler.MousePosition) = True And GameInstance.IsMouseVisible = True Or GameInstance.IsMouseVisible = False And loadGameJoltIndex = 2 Then
                    y = 16

                    SpriteBatch.DrawInterfaceString(FontManager.MiniFont, "Change to female.", New Vector2(r.X + 64 + 4 + r.Width, r.Y + 4 + 48), Color.White)
                End If
                SpriteBatch.DrawInterface(mainTexture, New Rectangle(r.X + 32 + r.Width, r.Y + 48, 32, 32), New Rectangle(160, 32 + y, 16, 16), Color.White)

                y = 0
                If ScaleScreenRec(New Rectangle(r.X + 32 + r.Width, r.Y + 48 + 48, 32, 32)).Contains(MouseHandler.MousePosition) = True And GameInstance.IsMouseVisible = True Or GameInstance.IsMouseVisible = False And loadGameJoltIndex = 3 Then
                    y = 16

                    SpriteBatch.DrawInterfaceString(FontManager.MiniFont, "Reset save.", New Vector2(r.X + 64 + 4 + r.Width, r.Y + 4 + 48 + 48), Color.White)
                End If
                SpriteBatch.DrawInterface(mainTexture, New Rectangle(r.X + 32 + r.Width, r.Y + 48 + 48, 32, 32), New Rectangle(176, 32 + y, 16, 16), Color.White)
            Else
                Dim downloadProgress As Integer = GameJoltSave.DownloadProgress
                Dim total As Integer = GameJoltSave.TotalDownloadItems

                Dim downloadtext As String = "Downloading profile"
                SpriteBatch.DrawInterfaceString(FontManager.MainFont, downloadtext & LoadingDots.Dots, New Vector2(CSng(ScreenSize.Width / 2 - FontManager.MainFont.MeasureString(downloadtext).X / 2) + 2, 322), Color.Black)
                SpriteBatch.DrawInterfaceString(FontManager.MainFont, downloadtext & LoadingDots.Dots, New Vector2(CSng(ScreenSize.Width / 2 - FontManager.MainFont.MeasureString(downloadtext).X / 2), 320), Color.White)

                Canvas.DrawScrollBar(New Vector2(CInt(ScreenSize.Width / 2) - 256, 400), total, downloadProgress, 0, New Size(512, 8), True, Color.Black, Color.White, True)
            End If
        Else
            Dim failText As String = "The download failed! Please try again."

            SpriteBatch.DrawInterfaceString(FontManager.MainFont, failText, New Vector2(CSng(ScreenSize.Width / 2 - FontManager.MainFont.MeasureString(failText).X / 2) + 2, 322), Color.Black)
            SpriteBatch.DrawInterfaceString(FontManager.MainFont, failText, New Vector2(CSng(ScreenSize.Width / 2 - FontManager.MainFont.MeasureString(failText).X / 2), 320), Color.DarkRed)
        End If

        If ControllerHandler.IsConnected() = False Then
            Dim text As String = "Right-Click to quit to the main menu"
            SpriteBatch.DrawInterfaceString(FontManager.MainFont, text, New Vector2(CSng(ScreenSize.Width / 2 - FontManager.MainFont.MeasureString(text).X / 2) + 2, 502), Color.Black)
            SpriteBatch.DrawInterfaceString(FontManager.MainFont, text, New Vector2(CSng(ScreenSize.Width / 2 - FontManager.MainFont.MeasureString(text).X / 2), 500), Color.White)
        End If

        Dim d As New Dictionary(Of Buttons, String)
        d.Add(Buttons.A, "Select")
        d.Add(Buttons.B, "Back")
        DrawGamePadControls(d)
    End Sub

    Private Sub UpdateLoadGameJoltSaveMenu()
        Dim downloaded As Boolean = GameJoltSave.DownloadFinished

        If downloaded = True Then
            If Controls.Down(True, True, False, True, True) = True Or Controls.Right(True, True, False, True, True) = True Then
                loadGameJoltIndex += 1
            End If
            If Controls.Up(True, True, False, True, True) = True Or Controls.Left(True, True, False, True, True) = True Then
                loadGameJoltIndex -= 1
            End If

            loadGameJoltIndex = loadGameJoltIndex.Clamp(0, 3)

            If Controls.Accept(True, True) = True Then
                If GameInstance.IsMouseVisible = False And loadGameJoltIndex = 0 Or ScaleScreenRec(New Rectangle(CInt(ScreenSize.Width / 2) - 256, 300, 512, 128)).Contains(MouseHandler.MousePosition) = True And GameInstance.IsMouseVisible = True Then
                    Core.Player.IsGameJoltSave = True
                    Core.Player.LoadGame("GAMEJOLTSAVE")

                    SetScreen(New JoinServerScreen(Me))
                End If

                Dim r As Rectangle = ScaleScreenRec(New Rectangle(CInt(ScreenSize.Width / 2) - 256, 300, 512, 128))
                If GameInstance.IsMouseVisible = False And loadGameJoltIndex = 1 Or ScaleScreenRec(New Rectangle(r.X + 32 + r.Width, r.Y, 32, 32)).Contains(MouseHandler.MousePosition) = True And GameInstance.IsMouseVisible = True Then
                    ButtonChangeMale()
                End If
                If GameInstance.IsMouseVisible = False And loadGameJoltIndex = 2 Or ScaleScreenRec(New Rectangle(r.X + 32 + r.Width, r.Y + 48, 32, 32)).Contains(MouseHandler.MousePosition) = True And GameInstance.IsMouseVisible = True Then
                    ButtonChangeFemale()
                End If
                If GameInstance.IsMouseVisible = False And loadGameJoltIndex = 3 Or ScaleScreenRec(New Rectangle(r.X + 32 + r.Width, r.Y + 48 + 48, 32, 32)).Contains(MouseHandler.MousePosition) = True And GameInstance.IsMouseVisible = True Then
                    ButtonResetSave()
                End If
            End If
        End If

        If Controls.Dismiss(True, True) = True Then
            menuIndex = 0
        End If
    End Sub

    Private Sub ButtonChangeMale()
        GameJoltSave.Gender = "0"

        Core.Player.Skin = GameJolt.Emblem.GetPlayerSpriteFile(GameJolt.Emblem.GetPlayerLevel(GameJoltSave.Points), GameJoltSave.GameJoltID, GameJoltSave.Gender)
        Core.Player.Gender = "Male"
    End Sub

    Private Sub ButtonChangeFemale()
        GameJoltSave.Gender = "1"

        Core.Player.Skin = GameJolt.Emblem.GetPlayerSpriteFile(GameJolt.Emblem.GetPlayerLevel(GameJoltSave.Points), GameJoltSave.GameJoltID, GameJoltSave.Gender)
        Core.Player.Gender = "Female"
    End Sub

    Private Sub ButtonResetSave()
        GameJoltSave.ResetSave()
    End Sub

#End Region

#Region "LanguageMenu"

    Private Sub DrawLanguageMenu()
        Dim CanvasTexture As Texture2D

        For i = 0 To 3
            Dim c As Color = Color.White
            If i + languageMenuIndex(2) = languageMenuIndex(0) Then
                c = New Color(101, 142, 255)
            End If

            Canvas.DrawRectangle(New Rectangle(CInt(ScreenSize.Width / 2) - 258, 180 + i * 50, 480, 48), c, True)
        Next

        Canvas.DrawScrollBar(New Vector2(CInt(ScreenSize.Width / 2) + 250, 180), Languages.Count, 4, languageMenuIndex(2), New Size(4, 200), False, New Color(190, 190, 190), New Color(63, 63, 63), True)

        Dim x As Integer = Languages.Count - 1
        x = CInt(MathHelper.Clamp(x, 0, 3))

        For i = 0 To x
            Dim Name As String = LanguageNames(i + languageMenuIndex(2))

            If i + languageMenuIndex(2) = languageMenuIndex(0) Then
                SpriteBatch.DrawInterfaceString(FontManager.InGameFont, Name, New Vector2(CInt(ScreenSize.Width / 2) - 245, 191 + i * 50), Color.Black)
                SpriteBatch.DrawInterfaceString(FontManager.InGameFont, Name, New Vector2(CInt(ScreenSize.Width / 2) - 248, 188 + i * 50), Color.White)
            Else
                SpriteBatch.DrawInterfaceString(FontManager.InGameFont, Name, New Vector2(CInt(ScreenSize.Width / 2) - 248, 188 + i * 50), Color.Black)
            End If
        Next

        CanvasTexture = TextureManager.GetTexture("GUI\Menus\Menu", New Rectangle(0, 0, 48, 48), "")

        For i = 0 To 1
            Dim Text As String = ""
            Select Case i
                Case 0
                    Text = Localization.GetString("language_menu_apply")
                Case 1
                    Text = Localization.GetString("language_menu_back")
            End Select

            If i = languageMenuIndex(1) Then
                CanvasTexture = TextureManager.GetTexture("GUI\Menus\Menu", New Rectangle(0, 48, 48, 48), "")
            Else
                CanvasTexture = TextureManager.GetTexture("GUI\Menus\Menu", New Rectangle(0, 0, 48, 48), "")
            End If

            Canvas.DrawImageBorder(CanvasTexture, 2, New Rectangle(CInt(ScreenSize.Width / 2) - 208 + i * 192, 550, 128, 64), True)
            SpriteBatch.DrawInterfaceString(FontManager.InGameFont, Text, New Vector2(CInt(ScreenSize.Width / 2) - 190 + i * 192, 582), Color.Black)
        Next
    End Sub

    Private Sub UpdateLanguageMenu()
        Dim currentIndex As Integer = languageMenuIndex(0)

        If Controls.Up(True, True, True) = True Then
            languageMenuIndex(0) -= 1
            If languageMenuIndex(0) - languageMenuIndex(2) < 0 Then
                languageMenuIndex(2) -= 1
            End If
        End If
        If Controls.Down(True, True, True) = True Then
            languageMenuIndex(0) += 1
            If languageMenuIndex(0) + languageMenuIndex(2) > 3 Then
                languageMenuIndex(2) += 1
            End If
        End If

        If GameInstance.IsMouseVisible = True Then
            For i = 0 To 1
                If ScaleScreenRec(New Rectangle(CInt(ScreenSize.Width / 2) - 208 + i * 192, 550, 128 + 32, 64 + 32)).Contains(MouseHandler.MousePosition) = True Then
                    languageMenuIndex(1) = i

                    If MouseHandler.ButtonPressed(MouseHandler.MouseButtons.LeftButton) = True Then
                        Select Case languageMenuIndex(1)
                            Case 0
                                currentLanguage = Languages(languageMenuIndex(0))
                                Core.GameOptions.SaveOptions()
                                menuIndex = 0
                            Case 1
                                Localization.Load(currentLanguage)
                                menuIndex = 0
                        End Select
                    End If
                End If
            Next

            For i = 0 To 3
                If ScaleScreenRec(New Rectangle(CInt(ScreenSize.Width / 2) - 258, 180 + i * 50, 480, 48)).Contains(MouseHandler.MousePosition) = True Then
                    If MouseHandler.ButtonPressed(MouseHandler.MouseButtons.LeftButton) = True Then
                        languageMenuIndex(0) = i + languageMenuIndex(2)
                    End If
                End If
            Next
        End If

        languageMenuIndex(0) = CInt(MathHelper.Clamp(languageMenuIndex(0), 0, Languages.Count - 1))
        languageMenuIndex(2) = CInt(MathHelper.Clamp(languageMenuIndex(2), 0, Languages.Count - 4))

        If languageMenuIndex(0) <> currentIndex Then
            Localization.Load(Languages(languageMenuIndex(0)))
        End If

        If Controls.Right(True, True, False) = True Then
            languageMenuIndex(1) += 1
        End If
        If Controls.Left(True, True, False) = True Then
            languageMenuIndex(1) -= 1
        End If

        languageMenuIndex(1) = CInt(MathHelper.Clamp(languageMenuIndex(1), 0, 1))

        If Controls.Accept(False, True) = True Then
            Select Case languageMenuIndex(1)
                Case 0
                    currentLanguage = Languages(languageMenuIndex(0))
                    Core.GameOptions.SaveOptions()
                    menuIndex = 0
                Case 1
                    Localization.Load(currentLanguage)
                    menuIndex = 0
            End Select
        End If

        If Controls.Dismiss() = True Then
            menuIndex = 0
        End If
    End Sub

#End Region

#Region "PackMenu"

    Private Sub DrawPacksMenu()
        Dim CanvasTexture As Texture2D
        Dim isSelectedEnabled As Boolean = False

        For i = 0 To 3
            Dim c As Color = Color.White
            If i + packsMenuIndex(2) = packsMenuIndex(0) Then
                c = New Color(101, 142, 255)

                If EnabledPackNames.Count > 0 Then
                    If EnabledPackNames.Contains(PackNames(i + packsMenuIndex(2))) = True Then
                        isSelectedEnabled = True
                    End If
                End If
            End If

            Canvas.DrawRectangle(New Rectangle(CInt(ScreenSize.Width / 2) - 258, 180 + i * 50, 480, 48), c, True)
        Next

        Canvas.DrawScrollBar(New Vector2(CInt(ScreenSize.Width / 2) + 250, 180), PackNames.Count, 4, packsMenuIndex(2), New Size(4, 200), False, New Color(190, 190, 190), New Color(63, 63, 63), True)

        Dim x As Integer = PackNames.Count - 1
        x = CInt(MathHelper.Clamp(x, 0, 3))

        If PackNames.Count > 0 Then
            For i = 0 To x
                Dim Name As String = PackNames(i + packsMenuIndex(2))
                Dim textColor As Color = Color.Gray

                If EnabledPackNames.Contains(Name) = True Then
                    Name &= " (" & Localization.GetString("pack_menu_enabled") & ")"
                    textColor = Color.Black
                End If

                If i + packsMenuIndex(2) = packsMenuIndex(0) Then
                    SpriteBatch.DrawInterfaceString(FontManager.InGameFont, Name, New Vector2(CInt(ScreenSize.Width / 2) - 245, 191 + i * 50), textColor)
                    SpriteBatch.DrawInterfaceString(FontManager.InGameFont, Name, New Vector2(CInt(ScreenSize.Width / 2) - 248, 188 + i * 50), Color.White)
                Else
                    SpriteBatch.DrawInterfaceString(FontManager.InGameFont, Name, New Vector2(CInt(ScreenSize.Width / 2) - 248, 188 + i * 50), textColor)
                End If
            Next
        End If

        CanvasTexture = TextureManager.GetTexture("GUI\Menus\Menu", New Rectangle(0, 0, 48, 48), "")

        For i = 0 To 1
            Dim Text As String = ""
            Select Case i
                Case 0
                    Text = Localization.GetString("pack_menu_apply")
                Case 1
                    Text = Localization.GetString("pack_menu_back")
            End Select

            If i = packsMenuIndex(1) Then
                CanvasTexture = TextureManager.GetTexture("GUI\Menus\Menu", New Rectangle(0, 48, 48, 48), "")
            Else
                CanvasTexture = TextureManager.GetTexture("GUI\Menus\Menu", New Rectangle(0, 0, 48, 48), "")
            End If

            Canvas.DrawImageBorder(CanvasTexture, 2, New Rectangle(CInt(ScreenSize.Width / 2) - 208 + i * 192, 550, 128, 64), True)
            SpriteBatch.DrawInterfaceString(FontManager.InGameFont, Text, New Vector2(CInt(ScreenSize.Width / 2) - 190 + i * 192, 582), Color.Black)
        Next
        For i = 2 To 5
            Dim Text As String = ""
            Select Case i
                Case 2
                    Text = Localization.GetString("pack_menu_up")
                Case 3
                    Text = Localization.GetString("pack_menu_down")
                Case 4
                    If isSelectedEnabled = True Then
                        Text = Localization.GetString("pack_menu_toggle_off")
                    Else
                        Text = Localization.GetString("pack_menu_toggle_on")
                    End If
                Case 5
                    Text = Localization.GetString("pack_menu_information")
            End Select

            If i = packsMenuIndex(1) Then
                If i = 2 Or i = 3 Or PackNames.Count = 0 Then
                    If isSelectedEnabled = True Then
                        CanvasTexture = TextureManager.GetTexture("GUI\Menus\Menu", New Rectangle(0, 48, 48, 48), "")
                    Else
                        CanvasTexture = TextureManager.GetTexture("GUI\Menus\Menu", New Rectangle(48, 0, 48, 48), "")
                    End If
                Else
                    CanvasTexture = TextureManager.GetTexture("GUI\Menus\Menu", New Rectangle(0, 48, 48, 48), "")
                End If
            Else
                CanvasTexture = TextureManager.GetTexture("GUI\Menus\Menu", New Rectangle(0, 0, 48, 48), "")
            End If

            Canvas.DrawImageBorder(CanvasTexture, 2, New Rectangle(CInt((ScreenSize.Width / 2) + 280), ((i - 2) * 64) + 180, 160, 32), True)
            SpriteBatch.DrawInterfaceString(FontManager.InGameFont, Text, New Vector2(CInt((ScreenSize.Width / 2) + 280) + 15, ((i - 2) * 64) + 16 + 180), Color.Black)
        Next
    End Sub

    Private Sub UpdatePacksMenu()
        Dim currentIndex As Integer = packsMenuIndex(0)

        If Controls.Up(True, True, True) = True Then
            packsMenuIndex(0) -= 1
            If packsMenuIndex(0) - packsMenuIndex(2) < 0 Then
                packsMenuIndex(2) -= 1
            End If
        End If
        If Controls.Down(True, True, True) = True Then
            packsMenuIndex(0) += 1
            If packsMenuIndex(0) + packsMenuIndex(2) > 3 Then
                packsMenuIndex(2) += 1
            End If
        End If

        If GameInstance.IsMouseVisible = True Then
            For i = 0 To 1
                If ScaleScreenRec(New Rectangle(CInt(ScreenSize.Width / 2) - 208 + i * 192, 550, 128 + 32, 64 + 32)).Contains(MouseHandler.MousePosition) = True Then
                    packsMenuIndex(1) = i

                    If MouseHandler.ButtonPressed(MouseHandler.MouseButtons.LeftButton) = True Then
                        Select Case packsMenuIndex(1)
                            Case 0
                                ButtonApplyPacks()
                            Case 1
                                menuIndex = 0
                        End Select
                    End If
                End If
            Next

            For i = 2 To 5
                If ScaleScreenRec(New Rectangle(CInt((ScreenSize.Width / 2) + 280), ((i - 2) * 64) + 180, 160 + 32, 32 + 32)).Contains(MouseHandler.MousePosition) = True Then
                    packsMenuIndex(1) = i

                    If MouseHandler.ButtonPressed(MouseHandler.MouseButtons.LeftButton) = True Then
                        Select Case packsMenuIndex(1)
                            Case 2 'up
                                ButtonUp()
                            Case 3 'down
                                ButtonDown()
                            Case 4 'toggle
                                If PackNames.Count > 0 Then
                                    ButtonToggle(PackNames(packsMenuIndex(0)))
                                End If
                            Case 5 'packinformation
                                ButtonPackInformation()
                        End Select
                    End If
                End If
            Next
        End If

        For i = 0 To 3
            If ScaleScreenRec(New Rectangle(CInt(ScreenSize.Width / 2) - 258, 180 + i * 50, 480, 48)).Contains(MouseHandler.MousePosition) = True Then
                If MouseHandler.ButtonPressed(MouseHandler.MouseButtons.LeftButton) = True Then
                    packsMenuIndex(0) = i + packsMenuIndex(2)
                End If
            End If
        Next

        packsMenuIndex(0) = CInt(MathHelper.Clamp(packsMenuIndex(0), 0, PackNames.Count - 1))
        packsMenuIndex(2) = CInt(MathHelper.Clamp(packsMenuIndex(2), 0, PackNames.Count - 4))

        If Controls.Right(True, True, False) = True Then
            packsMenuIndex(1) += 1
        End If
        If Controls.Left(True, True, False) = True Then
            packsMenuIndex(1) -= 1
        End If

        packsMenuIndex(1) = CInt(MathHelper.Clamp(packsMenuIndex(1), 0, 5))

        If Controls.Accept(False, True) = True Then
            Select Case packsMenuIndex(1)
                Case 0
                    ButtonApplyPacks()
                Case 1
                    menuIndex = 0
                Case 2
                    ButtonUp()
                Case 3
                    ButtonDown()
                Case 4
                    If PackNames.Count > 0 Then
                        ButtonToggle(PackNames(packsMenuIndex(0)))
                    End If
                Case 5
                    ButtonPackInformation()
            End Select
        End If

        If Controls.Dismiss() = True Then
            menuIndex = 0
        End If
    End Sub

    Private Sub ButtonPackInformation()
        If PackNames.Count = 0 Then
            Exit Sub
        End If

        menuIndex = 5

        Dim packName As String = PackNames(packsMenuIndex(0))
        PInfoSlpash = Nothing
        PInfoContent = ""

        Try
            If IO.File.Exists(GameController.GamePath & "\ContentPacks\" & packName & "\splash.png") = True Then
                Using stream As IO.Stream = IO.File.Open(GameController.GamePath & "\ContentPacks\" & packName & "\splash.png", IO.FileMode.OpenOrCreate)
                    PInfoSlpash = Texture2D.FromStream(GraphicsDevice, stream)
                End Using
            End If
        Catch ex As Exception
            Logger.Log(Logger.LogTypes.ErrorMessage, "MainMenuScreen.vb/ButtonPackInformation: An error occurred trying to load the splash image at """ & GameController.GamePath & "\ContentPacks\" & packName & "\splash.png" & """. This could have been caused by an invalid file header. (Exception: " & ex.Message & ")")
        End Try

        Dim contentPackPath As String = GameController.GamePath & "\ContentPacks\" & packName & "\"
        If IO.Directory.Exists(contentPackPath & "Songs") = True Then
            Dim hasWMA As Boolean = False
            Dim hasXNB As Boolean = False
            Dim hasMP3 As Boolean = False
            For Each file As String In IO.Directory.GetFiles(contentPackPath & "Songs")
                If IO.Path.GetExtension(file).ToLower() = ".xnb" Then
                    hasXNB = True
                End If
                If IO.Path.GetExtension(file).ToLower() = ".wma" Then
                    hasWMA = True
                End If
                If IO.Path.GetExtension(file).ToLower() = ".mp3" Then
                    hasMP3 = True
                End If
            Next

            If hasMP3 = True Or hasWMA = True And hasXNB = True Then
                PInfoContent = Localization.GetString("pack_menu_songs")
            End If
        End If
        If IO.Directory.Exists(contentPackPath & "Sounds") = True Then
            Dim hasWMA As Boolean = False
            Dim hasXNB As Boolean = False
            Dim hasWAV As Boolean = False
            For Each file As String In IO.Directory.GetFiles(contentPackPath & "Sounds")
                If IO.Path.GetExtension(file).ToLower() = ".xnb" Then
                    hasXNB = True
                End If
                If IO.Path.GetExtension(file).ToLower() = ".wma" Then
                    hasWMA = True
                End If
                If IO.Path.GetExtension(file).ToLower() = ".wav" Then
                    hasWAV = True
                End If
            Next

            If hasWAV = True Or hasWMA = True And hasXNB = True Then
                If PInfoContent <> "" Then
                    PInfoContent &= ", "
                End If

                PInfoContent &= Localization.GetString("pack_menu_sounds")
            End If
        End If

        Dim textureDirectories() As String = {"Textures", "GUI", "Items", "Pokemon", "SkyDomeResource"}
        For Each folder As String In textureDirectories
            If IO.Directory.Exists(contentPackPath & folder) = True Then
                Dim hasXNB As Boolean = False
                Dim hasPNG As Boolean = False
                For Each file As String In IO.Directory.GetFiles(contentPackPath & folder, "*.*", IO.SearchOption.AllDirectories)
                    If IO.Path.GetExtension(file).ToLower() = ".xnb" Then
                        hasXNB = True
                    End If
                    If IO.Path.GetExtension(file).ToLower() = ".png" Then
                        hasPNG = True
                    End If
                Next

                If hasXNB = True Or hasPNG = True Then
                    If PInfoContent <> "" Then
                        PInfoContent &= ", "
                    End If

                    PInfoContent &= Localization.GetString("pack_menu_textures")
                    Exit For
                End If
            End If
        Next

        Dim s() As String = ContentPackManager.GetContentPackInfo(packName)

        If s.Length > 0 Then
            PInfoVersion = s(0)
        End If
        If s.Length > 1 Then
            PInfoAuthor = s(1)
        End If
        If s.Length > 2 Then
            PInfoDescription = s(2)
        End If
        PInfoName = packName
    End Sub

    Private PInfoName As String = ""
    Private PInfoSlpash As Texture2D = Nothing
    Private PInfoVersion As String = ""
    Private PInfoAuthor As String = ""
    Private PInfoDescription As String = ""
    Private PInfoContent As String = ""

    Private Sub DrawPackInformationMenu()
        Dim isEnabled As Boolean = False
        Dim packName As String = PInfoName
        If EnabledPackNames.Contains(packName) = True Then
            isEnabled = True
        End If

        If Not PInfoSlpash Is Nothing Then
            SpriteBatch.DrawInterface(PInfoSlpash, ScreenSize, Color.White)
        End If

        Dim CanvasTexture As Texture2D = TextureManager.GetTexture("GUI\Menus\Menu", New Rectangle(0, 0, 48, 48), "")

        Canvas.DrawImageBorder(CanvasTexture, 2, New Rectangle(CInt(ScreenSize.Width / 2) - 256, 160, 480, 64), True)
        SpriteBatch.DrawInterfaceString(FontManager.InGameFont, Localization.GetString("pack_menu_name") & ": " & PInfoName, New Vector2(CInt(ScreenSize.Width / 2) - CInt(FontManager.InGameFont.MeasureString(Localization.GetString("pack_menu_name") & ": " & PInfoName).X / 2), 195), Color.Black)

        Canvas.DrawImageBorder(CanvasTexture, 2, New Rectangle(CInt(ScreenSize.Width / 2) - 256, 288, 480, 224), True)
        SpriteBatch.DrawInterfaceString(FontManager.MiniFont, Localization.GetString("pack_menu_version") & ": " & PInfoVersion & vbNewLine & Localization.GetString("pack_menu_by") & ": " & PInfoAuthor & vbNewLine & Localization.GetString("pack_menu_content") & ": " & PInfoContent & vbNewLine & Localization.GetString("pack_menu_description") & ": " & PInfoDescription.Replace("<br>", vbNewLine), New Vector2(CInt(ScreenSize.Width / 2) - 220, 323), Color.Black)

        For i = 0 To 1
            If i = packInfoIndex Then
                CanvasTexture = TextureManager.GetTexture("GUI\Menus\Menu", New Rectangle(0, 48, 48, 48), "")
            Else
                CanvasTexture = TextureManager.GetTexture("GUI\Menus\Menu", New Rectangle(0, 0, 48, 48), "")
            End If

            Dim Text As String = Localization.GetString("pack_menu_back")

            Select Case i
                Case 0
                    If isEnabled = True Then
                        Text = Localization.GetString("pack_menu_toggle_off")
                    Else
                        Text = Localization.GetString("pack_menu_toggle_on")
                    End If
                Case 1
                    Text = Localization.GetString("pack_menu_back")
            End Select

            Canvas.DrawImageBorder(CanvasTexture, 2, New Rectangle(CInt(ScreenSize.Width / 2) - 180 + (200 * i), 550, 128, 64), True)
            SpriteBatch.DrawInterfaceString(FontManager.InGameFont, Text, New Vector2(CInt(ScreenSize.Width / 2) - 160 + (200 * i), 582), Color.Black)
        Next
    End Sub

    Private Sub UpdatePackInformationMenu()
        Dim packName As String = PInfoName

        If GameInstance.IsMouseVisible = True Then
            For i = 0 To 1
                If ScaleScreenRec(New Rectangle(CInt(ScreenSize.Width / 2) - 180 + (200 * i), 550, 160, 96)).Contains(MouseHandler.MousePosition) = True Then
                    packInfoIndex = i

                    If MouseHandler.ButtonPressed(MouseHandler.MouseButtons.LeftButton) = True Then
                        Select Case i
                            Case 0
                                ButtonToggle(packName)
                            Case 1
                                menuIndex = 4
                        End Select
                    End If
                End If
            Next
        End If

        If Controls.Right(True, True, True, True) = True Then
            packInfoIndex += 1
        End If
        If Controls.Left(True, True, True, True) = True Then
            packInfoIndex -= 1
        End If

        packInfoIndex = CInt(MathHelper.Clamp(packInfoIndex, 0, 1))

        If Controls.Accept(False) = True Then
            Select Case packInfoIndex
                Case 0
                    ButtonToggle(packName)
                Case 1
                    menuIndex = 4
            End Select
        End If

        If Controls.Dismiss(False) = True Then
            menuIndex = 4
        End If
    End Sub

    Private Sub ButtonUp()
        If PackNames.Count > 0 Then
            If EnabledPackNames.Contains(PackNames(packsMenuIndex(0))) = True Then
                Dim idx As Integer = EnabledPackNames.IndexOf(PackNames(packsMenuIndex(0)))
                If idx > 0 Then
                    Dim tempString As String = EnabledPackNames(idx - 1)
                    EnabledPackNames(idx - 1) = EnabledPackNames(idx)
                    EnabledPackNames(idx) = tempString
                    GetPacks(True)
                End If
            End If
        End If
    End Sub

    Private Sub ButtonDown()
        If PackNames.Count > 0 Then
            If EnabledPackNames.Contains(PackNames(packsMenuIndex(0))) = True Then
                Dim idx As Integer = EnabledPackNames.IndexOf(PackNames(packsMenuIndex(0)))
                If idx < EnabledPackNames.Count - 1 Then
                    Dim tempString As String = EnabledPackNames(idx + 1)
                    EnabledPackNames(idx + 1) = EnabledPackNames(idx)
                    EnabledPackNames(idx) = tempString
                    GetPacks(True)
                End If
            End If
        End If
    End Sub

    Private Sub ButtonToggle(ByVal PackName As String)
        If PackNames.Count > 0 Then
            If EnabledPackNames.Contains(PackName) = True Then
                EnabledPackNames.Remove(PackName)
                GetPacks(True)
            Else
                EnabledPackNames.Add(PackName)
                GetPacks(True)
            End If
        Else
            GetPacks(True)
        End If
    End Sub

    Private Sub ButtonApplyPacks()
        If PackNames.Count > 0 Then
            Core.GameOptions.ContentPackNames = EnabledPackNames.ToArray()
            Core.GameOptions.SaveOptions()
            MediaPlayer.Stop()
            ContentPackManager.Clear()
            For Each s As String In Core.GameOptions.ContentPackNames
                ContentPackManager.Load(GameController.GamePath & "\ContentPacks\" & s & "\exceptions.dat")
            Next
            MusicManager.PlayNoMusic()
            Core.OffsetMaps.Clear()
            Core.SetScreen(New MainMenuScreen)
        End If
        Me.menuIndex = 0
    End Sub

#End Region

#Region "DeleteMenu"

    Private Sub DrawDeleteMenu()
        Dim CanvasTexture As Texture2D = TextureManager.GetTexture("GUI\Menus\Menu", New Rectangle(0, 0, 48, 48), "")

        Canvas.DrawImageBorder(CanvasTexture, 2, New Rectangle(CInt(ScreenSize.Width / 2 - 352), 172, 704, 96), Color.White, True)

        SpriteBatch.DrawInterfaceString(FontManager.InGameFont, Localization.GetString("delete_menu_delete_confirm"), New Vector2(CInt(ScreenSize.Width / 2) - CInt(FontManager.InGameFont.MeasureString(Localization.GetString("delete_menu_delete_confirm")).X / 2), 200), Color.Black)

        SpriteBatch.DrawInterfaceString(FontManager.InGameFont, """" & SaveNames(loadMenuIndex(0)) & """ ?", New Vector2(CInt(ScreenSize.Width / 2) - CInt(FontManager.InGameFont.MeasureString("""" & SaveNames(loadMenuIndex(0)) & """ ?").X / 2), 240), Color.Black)

        For i = 0 To 1
            Dim Text As String = Localization.GetString("delete_menu_delete")

            If i = 1 Then
                Text = Localization.GetString("delete_menu_cancel")
            End If

            If i = deleteIndex Then
                CanvasTexture = TextureManager.GetTexture("GUI\Menus\Menu", New Rectangle(0, 48, 48, 48), "")
            Else
                CanvasTexture = TextureManager.GetTexture("GUI\Menus\Menu", New Rectangle(0, 0, 48, 48), "")
            End If

            Canvas.DrawImageBorder(CanvasTexture, 2, New Rectangle(CInt(ScreenSize.Width / 2) - 182 + i * 192, 370, 128, 64), True)
            SpriteBatch.DrawInterfaceString(FontManager.InGameFont, Text, New Vector2(CInt(ScreenSize.Width / 2) - 164 + i * 192, 402), Color.Black)
        Next
    End Sub

    Private Sub UpdateDeleteMenu()
        If Controls.Right(True, True, False) = True Then
            deleteIndex = 1
        End If
        If Controls.Left(True, True, False) = True Then
            deleteIndex = 0
        End If

        If GameInstance.IsMouseVisible = True Then
            For i = 0 To 1
                If ScaleScreenRec(New Rectangle(CInt(ScreenSize.Width / 2) - 182 + i * 192, 370, 128 + 32, 64 + 32)).Contains(MouseHandler.MousePosition) = True Then
                    deleteIndex = i

                    If MouseHandler.ButtonPressed(MouseHandler.MouseButtons.LeftButton) = True Then
                        Select Case deleteIndex
                            Case 0
                                Delete()
                            Case 1
                                menuIndex = 1
                        End Select
                    End If
                End If
            Next
        End If

        If Controls.Accept(False, True) = True Then
            Select Case deleteIndex
                Case 0
                    Delete()
                Case 1
                    menuIndex = 1
            End Select
        End If
    End Sub

    Private Sub Delete()
        IO.Directory.Delete(Saves(loadMenuIndex(0)), True)

        Dim deleteAutosave As Boolean = False
        For Each f As String In IO.Directory.GetDirectories(GameController.GamePath & "\Save\")
            If IO.File.Exists(f & "\Player.dat") = True Then
                Dim Data() As String = IO.File.ReadAllText(f & "\Player.dat").SplitAtNewline()
                Dim Autosave As Boolean = False

                For Each Line As String In Data
                    If Line.StartsWith("AutoSave|") = True Then
                        Dim autosaveName As String = Line.GetSplit(1, "|")
                        If autosaveName = Saves(loadMenuIndex(0)).Remove(0, Saves(loadMenuIndex(0)).LastIndexOf("\") + 1) Then
                            deleteAutosave = True
                        End If
                    End If
                Next
            End If
        Next
        If deleteAutosave = True Then
            IO.Directory.Delete(GameController.GamePath & "\Save\autosave", True)
        End If

        tempLoadDisplay = ""
        GetSaves()
        loadMenuIndex(0) = 0
        loadMenuIndex(1) = 0
        loadMenuIndex(2) = 0
        If Saves.Count = 0 Then
            menuIndex = 0
        Else
            menuIndex = 1
        End If
    End Sub

#End Region

#Region "NewGameMenu"

    Public Sub NewGameButton()
        If Core.GameOptions.StartedOfflineGame = True Then
            If GameModeManager.GameModeCount < 2 Then
                GameModeManager.SetGameModePointer("Kolben")
                'SetScreen(New TransitionScreen(Me, New NewGameScreen(), Color.Black, False))
            Else
                GetGameModes()
                GameModeSplash = Nothing
                menuIndex = 6
            End If
        Else
            Core.GameOptions.StartedOfflineGame = True
            Core.GameOptions.SaveOptions()
            SetScreen(New OfflineGameWarningScreen(Me))
        End If
    End Sub

    Private tempGameModesDisplay As String = ""
    Private GameModeSplash As Texture2D = Nothing

    Private Sub DrawNewGameMenu()
        If Not GameModeSplash Is Nothing Then
            SpriteBatch.DrawInterface(GameModeSplash, ScreenSize, Color.White)
        End If

        Dim CanvasTexture As Texture2D

        For i = 0 To 3
            Dim c As Color = Color.White
            If i + gameModeMenuIndex(2) = gameModeMenuIndex(0) Then
                c = New Color(101, 142, 255)
            End If

            Canvas.DrawRectangle(New Rectangle(CInt(ScreenSize.Width / 2) - 258, 180 + i * 50, 480, 48), c, True)
        Next

        Canvas.DrawScrollBar(New Vector2(CInt(ScreenSize.Width / 2) + 250, 180), ModeNames.Count, 4, gameModeMenuIndex(2), New Size(4, 200), False, New Color(190, 190, 190), New Color(63, 63, 63), True)

        Dim x As Integer = ModeNames.Count - 1
        x = CInt(MathHelper.Clamp(x, 0, 3))

        For i = 0 To x
            Dim Name As String = ModeNames(i + gameModeMenuIndex(2))

            If i + gameModeMenuIndex(2) = gameModeMenuIndex(0) Then
                SpriteBatch.DrawInterfaceString(FontManager.InGameFont, Name, New Vector2(CInt(ScreenSize.Width / 2) - 245, 191 + i * 50), Color.Black)
                SpriteBatch.DrawInterfaceString(FontManager.InGameFont, Name, New Vector2(CInt(ScreenSize.Width / 2) - 248, 188 + i * 50), Color.White)
            Else
                SpriteBatch.DrawInterfaceString(FontManager.InGameFont, Name, New Vector2(CInt(ScreenSize.Width / 2) - 248, 188 + i * 50), Color.Black)
            End If
        Next

        CanvasTexture = TextureManager.GetTexture("GUI\Menus\Menu", New Rectangle(0, 0, 48, 48), "")
        Canvas.DrawImageBorder(CanvasTexture, 2, New Rectangle(CInt(ScreenSize.Width / 2) - 272, 388, 512, 128), True)

        If tempGameModesDisplay = "" Then
            Dim GameMode As GameMode = GameModeManager.GetGameMode(ModeNames(gameModeMenuIndex(0)))

            Dim dispName As String = GameMode.Name
            Dim dispDescription As String = GameMode.Description
            Dim dispVersion As String = GameMode.Version
            Dim dispAuthor As String = GameMode.Author
            Dim dispContentPath As String = GameMode.ContentPath

            tempGameModesDisplay = Localization.GetString("gamemode_menu_name") & ": " & dispName & vbNewLine &
                Localization.GetString("gamemode_menu_version") & ": " & dispVersion & vbNewLine &
                Localization.GetString("gamemode_menu_author") & ": " & dispAuthor & vbNewLine &
                Localization.GetString("gamemode_menu_contentpath") & ": " & dispContentPath & vbNewLine &
                Localization.GetString("gamemode_menu_description") & ": " & dispDescription
        End If

        SpriteBatch.DrawInterfaceString(FontManager.MiniFont, tempGameModesDisplay, New Vector2(CInt(ScreenSize.Width / 2) - 252, 416), Color.Black)

        For i = 0 To 1
            If i = gameModeMenuIndex(1) Then
                CanvasTexture = TextureManager.GetTexture("GUI\Menus\Menu", New Rectangle(0, 48, 48, 48), "")
            Else
                CanvasTexture = TextureManager.GetTexture("GUI\Menus\Menu", New Rectangle(0, 0, 48, 48), "")
            End If

            Dim Text As String = Localization.GetString("gamemode_menu_back")

            Select Case i
                Case 0
                    Text = Localization.GetString("gamemode_menu_create")
                Case 1
                    Text = Localization.GetString("gamemode_menu_back")
            End Select

            Canvas.DrawImageBorder(CanvasTexture, 2, New Rectangle(CInt(ScreenSize.Width / 2) - 180 + (200 * i), 550, 128, 64), True)
            SpriteBatch.DrawInterfaceString(FontManager.InGameFont, Text, New Vector2(CInt(ScreenSize.Width / 2) - 160 + (200 * i), 582), Color.Black)
        Next
    End Sub

    Private Sub UpdateNewGameMenu()
        If Controls.Up(True, True, True) = True Then
            gameModeMenuIndex(0) -= 1
            If gameModeMenuIndex(0) - gameModeMenuIndex(2) < 0 Then
                gameModeMenuIndex(2) -= 1
            End If
            tempGameModesDisplay = ""
            GameModeSplash = Nothing
        End If
        If Controls.Down(True, True, True) = True Then
            gameModeMenuIndex(0) += 1
            If gameModeMenuIndex(0) + gameModeMenuIndex(2) > 3 Then
                gameModeMenuIndex(2) += 1
            End If
            tempGameModesDisplay = ""
            GameModeSplash = Nothing
        End If

        If GameInstance.IsMouseVisible = True Then
            For i = 0 To 1
                If ScaleScreenRec(New Rectangle(CInt(ScreenSize.Width / 2) - 180 + (200 * i), 550, 160, 96)).Contains(MouseHandler.MousePosition) = True Then
                    gameModeMenuIndex(1) = i

                    If MouseHandler.ButtonPressed(MouseHandler.MouseButtons.LeftButton) = True Then
                        Select Case gameModeMenuIndex(1)
                            Case 0
                                AcceptGameMode()
                            Case 1
                                menuIndex = 0

                                tempGameModesDisplay = ""
                        End Select
                    End If
                End If
            Next
        End If

        For i = 0 To 3
            If ScaleScreenRec(New Rectangle(CInt(ScreenSize.Width / 2) - 258, 180 + i * 50, 480, 48)).Contains(MouseHandler.MousePosition) = True Then
                If MouseHandler.ButtonPressed(MouseHandler.MouseButtons.LeftButton) = True Then
                    gameModeMenuIndex(0) = i + gameModeMenuIndex(2)
                    tempGameModesDisplay = ""
                    GameModeSplash = Nothing
                End If
            End If
        Next

        gameModeMenuIndex(0) = CInt(MathHelper.Clamp(gameModeMenuIndex(0), 0, ModeNames.Count - 1))
        gameModeMenuIndex(2) = CInt(MathHelper.Clamp(gameModeMenuIndex(2), 0, ModeNames.Count - 4))

        If Controls.Right(True, True, False) = True Then
            gameModeMenuIndex(1) += 1
        End If
        If Controls.Left(True, True, False) = True Then
            gameModeMenuIndex(1) -= 1
        End If

        gameModeMenuIndex(1) = CInt(MathHelper.Clamp(gameModeMenuIndex(1), 0, 1))

        If Controls.Accept(False, True) = True Then
            Select Case gameModeMenuIndex(1)
                Case 0
                    AcceptGameMode()
                Case 1
                    menuIndex = 0

                    tempGameModesDisplay = ""
            End Select
        End If

        If GameModeSplash Is Nothing Then
            Try
                Dim fileName As String = GameController.GamePath & "\GameModes\" & ModeNames(gameModeMenuIndex(0)) & "\GameMode.png"
                If IO.File.Exists(fileName) = True Then
                    Using stream As IO.Stream = IO.File.Open(fileName, IO.FileMode.OpenOrCreate)
                        GameModeSplash = Texture2D.FromStream(GraphicsDevice, stream)
                    End Using
                End If
            Catch ex As Exception
                Logger.Log(Logger.LogTypes.ErrorMessage, "MainMenuScreen.vb/UpdateNewGameMenu: An error occurred trying to load the splash image at """ & GameController.GamePath & "\GameModes\" & ModeNames(gameModeMenuIndex(0)) & "\GameMode.png"". This could have been caused by an invalid file header. (Exception: " & ex.Message & ")")
            End Try
        End If

        If Controls.Dismiss() = True Then
            menuIndex = 0
        End If
    End Sub

    Private Sub AcceptGameMode()
        GameModeManager.SetGameModePointer(ModeNames(gameModeMenuIndex(0)))
        'SetScreen(New TransitionScreen(Me, New NewGameScreen(), Color.Black, False))
    End Sub

#End Region

    Public Overrides Sub ChangeTo()
        Core.Player.Unload()
        Core.Player.Skin = "Hilbert"
        TextBox.Hide()
        TextBox.CanProceed = True
        OverworldScreen.FadeValue = 0

        MusicManager.Play("title", True, 0.0F, 0.0F)
    End Sub

End Class