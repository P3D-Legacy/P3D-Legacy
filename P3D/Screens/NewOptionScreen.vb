Public Class NewOptionScreen

    Inherits Screen
    Dim TextSpeed As Integer = 2
    Dim CameraSpeed As Integer = 12
    Dim FOV As Single = 45.0F
    Dim Music As Integer = 50
    Dim Sound As Integer = 50
    Dim RenderDistance As Integer = 0
    Dim GraphicStyle As Integer = 1
    Dim ShowBattleAnimations As Integer = 1
    Dim DiagonalMovement As Boolean = True
    Dim Difficulty As Integer = 0
    Dim BattleStyle As Integer = 0
    Dim LoadOffsetMaps As Integer = 1
    Dim ViewBobbing As Boolean = True
    Dim ShowModels As Integer = 1
    Dim Muted As Integer = 0
    Dim GamePadEnabled As Boolean = True
    Dim RunMode As Boolean = True
    Dim PreferMultiSampling As Boolean = True
    Private _subMenu As Integer = 0
    Private _screenSize As Size = New Size(CInt(windowSize.Width), CInt(windowSize.Height))

    Private Property SelectPackNoiseDelay As Integer = 10

    Private Languages As New List(Of String)
    Private LanguageNames As New List(Of String)
    Private currentLanguage As String = Localization.CurrentLanguage
    Private TempLanguage As String = Localization.CurrentLanguage
    Public Shared languageMenuIndex(3) As Integer

    Private PackNames As New List(Of String)
    Private EnabledPackNames As New List(Of String)
    Public Shared isSelectedEnabled As Boolean = False

    Private packsMenuIndex(3) As Integer
    Private packInfoIndex As Integer = 0

    Dim savedOptions As Boolean = True

    Public Shared ScreenIndex As Integer = 0
    Dim _nextIndex As Integer = 0
    Dim ControlList As New List(Of Control)

    'New stuff
    ''' <summary>
    ''' Texture from file: GUI\Menus\General
    ''' </summary>
    Private _texture As Texture2D
    ''' <summary>
    ''' Texture from file: GUI\Menus\Options
    ''' </summary>
    Private _menuTexture As Texture2D

    'Interface animation state values:
    Private _interfaceFade As Single = 0F
    Private _closing As Boolean = False
    Private _opening As Boolean = False
    Private _enrollY As Single = 0F
    Private _itemIntro As Single = 0F

    Private _pageFade As Single = 1.0F
    Private _pageOpening As Boolean = False
    Private _pageClosing As Boolean = False

    'cursor animation:
    Private _cursorPosition As Vector2 = New Vector2(CInt(Core.windowSize.Width / 2) - 400 + 90, CInt(Core.windowSize.Height / 2) - 200 + 80)
    Private _cursorDestPosition As Vector2 = New Vector2(CInt(Core.windowSize.Width / 2) - 400 + 90, CInt(Core.windowSize.Height / 2) - 200 + 80)

    Private _selectedScrollBar As Boolean = False


    Public Sub New(ByVal currentScreen As Screen, Optional ByVal Submenu As Integer = 0)
        'New stuff
        _texture = TextureManager.GetTexture("GUI\Menus\General")
        _menuTexture = TextureManager.GetTexture("GUI\Menus\Options")

        '''
        Me.Identification = Identifications.OptionScreen
        Me.PreScreen = currentScreen
        Me.CanChat = False
        Me.MouseVisible = True
        Me.CanBePaused = False
        Me._opening = True
        GetLanguages()
        GetPacks()

        If Camera Is Nothing Then
            Camera = New OverworldCamera()
        End If
        SetFunctionality()
        If Submenu > 0 AndAlso PreScreen.Identification = Identifications.MainMenuScreen Then
            _subMenu = Submenu
            Select Case _subMenu
                Case 1
                    SwitchToLanguage()
                Case 2
                    SwitchToAudio()
                Case 3
                    SwitchToControls()
                Case 4
                    SwitchToContentPacks()
            End Select
        Else
            _subMenu = 0
            ScreenIndex = 0
        End If
    End Sub

    Private Sub SetFunctionality()
        If PreScreen.Identification <> Identifications.MainMenuScreen Then
            Camera = CType(Screen.Camera, OverworldCamera)
            Me.FOV = Camera.FOV
            Me.TextSpeed = TextBox.TextSpeed
            Me.CameraSpeed = CInt(Camera.RotationSpeed * 10000)
            Me.Difficulty = Core.Player.DifficultyMode
            Me.RunMode = Core.Player.RunMode
        End If
        Me.Music = CInt(MusicManager.MasterVolume * 100)
        Me.Sound = CInt(SoundManager.Volume * 100)
        Me.RenderDistance = Core.GameOptions.RenderDistance
        Me.GraphicStyle = Core.GameOptions.GraphicStyle
        Me.ShowBattleAnimations = Core.Player.ShowBattleAnimations
        Me.DiagonalMovement = Core.Player.DiagonalMovement
        Me.BattleStyle = Core.Player.BattleStyle
        Me.ShowModels = CInt(Core.Player.ShowModelsInBattle)
        Me.Muted = CInt(MusicManager.Muted.ToNumberString())
        If Core.GameOptions.LoadOffsetMaps = 0 Then
            Me.LoadOffsetMaps = 0
        Else
            Me.LoadOffsetMaps = 101 - Core.GameOptions.LoadOffsetMaps
        End If
        Me.ViewBobbing = Core.GameOptions.ViewBobbing
        Me.GamePadEnabled = Core.GameOptions.GamePadEnabled
        Me.PreferMultiSampling = Core.GraphicsManager.PreferMultiSampling
        Me.Language = Localization.GetAvailableLanguagesList.Where(Function(x) x.Value = Localization.GetLanguageName(Core.GameOptions.Language)).Select(Function(y) y.Key).FirstOrDefault()
    End Sub


    Public Overrides Sub Draw()
        PreScreen.Draw()
        DrawBackground()
        If ScreenIndex = 6 Then
            DrawLanguageMenu()
        End If
        If ScreenIndex = 7 Then
            DrawPacksMenu()
        End If
        If ScreenIndex = 8 Then
            DrawPackInformationMenu()
        End If
        DrawCurrentPage()
        DrawCursor()
        DrawMessage()

        TextBox.Draw()
        ChooseBox.Draw()

    End Sub

#Region "LanguageMenu"

    Private Sub DrawLanguageMenu()

        For i = 0 To 3
            Dim c As Color = New Color(255, 255, 255, CInt(255 * _interfaceFade * _pageFade))
            If i + languageMenuIndex(2) = languageMenuIndex(0) Then
                c = New Color(77, 147, 198, CInt(255 * _interfaceFade * _pageFade))
            End If

            Canvas.DrawRectangle(New Rectangle(CInt(windowSize.Width / 2 - 258), CInt(Core.windowSize.Height / 2 - 128 + i * 50), 480, 48), c, False)
        Next

        Canvas.DrawScrollBar(New Vector2(CInt(windowSize.Width / 2 + 250), CInt(Core.windowSize.Height / 2 - 128)), Languages.Count, 4, languageMenuIndex(2), New Size(4, 200), False, New Color(77, 147, 198, CInt(255 * _interfaceFade * _pageFade)), New Color(255, 255, 255, CInt(255 * _interfaceFade * _pageFade)), False)

        Dim x As Integer = Languages.Count - 1
        x = CInt(MathHelper.Clamp(x, 0, 3))

        For i = 0 To x
            Dim Name As String = LanguageNames(i + languageMenuIndex(2))

            If i + languageMenuIndex(2) = languageMenuIndex(0) Then
                SpriteBatch.DrawString(FontManager.InGameFont, Name, New Vector2(CInt(windowSize.Width / 2 - 246), CInt(Core.windowSize.Height / 2 - 128 + 8 + 2 + i * 50)), New Color(0, 0, 0, CInt(255 * _interfaceFade * _pageFade)))
                SpriteBatch.DrawString(FontManager.InGameFont, Name, New Vector2(CInt(windowSize.Width / 2 - 248), CInt(Core.windowSize.Height / 2 - 128 + 8 + i * 50)), New Color(255, 255, 255, CInt(255 * _interfaceFade * _pageFade)))
            Else
                SpriteBatch.DrawString(FontManager.InGameFont, Name, New Vector2(CInt(windowSize.Width / 2 - 248), CInt(Core.windowSize.Height / 2 - 128 + 8 + i * 50)), New Color(0, 0, 0, CInt(255 * _interfaceFade * _pageFade)))
            End If
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

            For i = 0 To 3
                If New Rectangle(CInt(windowSize.Width / 2) - 258, CInt(Core.windowSize.Height / 2 - 128 + i * 50), 480, 48).Contains(MouseHandler.MousePosition) = True Then
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
#End Region

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
#Region "PacksMenu"

    Private Sub DrawPacksMenu()

        For i = 0 To 3
            Dim c As Color = New Color(255, 255, 255, CInt(255 * _interfaceFade * _pageFade))
            If i + packsMenuIndex(2) = packsMenuIndex(0) Then
                c = New Color(77, 147, 198, CInt(255 * _interfaceFade * _pageFade))

                If EnabledPackNames.Count > 0 Then
                    If EnabledPackNames.Contains(PackNames(i + packsMenuIndex(2))) = True Then
                        isSelectedEnabled = True
                    Else
                        isSelectedEnabled = False
                    End If
                End If
            End If

            Canvas.DrawRectangle(New Rectangle(CInt(windowSize.Width / 2) - 328, CInt(Core.windowSize.Height / 2 - 128 + i * 50), 500, 48), c, False)
        Next

        Canvas.DrawScrollBar(New Vector2(CInt(windowSize.Width / 2) + 188, CInt(Core.windowSize.Height / 2 - 128)), PackNames.Count, 4, packsMenuIndex(2), New Size(4, 200), False, New Color(77, 147, 198, CInt(255 * _interfaceFade * _pageFade)), New Color(255, 255, 255, CInt(255 * _interfaceFade * _pageFade)), False)

        Dim x As Integer = PackNames.Count - 1
        x = CInt(MathHelper.Clamp(x, 0, 3))
        Dim textColor As Color = New Color(0, 0, 0, CInt(255 * _interfaceFade * _pageFade))

        If PackNames.Count > 0 Then
            For i = 0 To x
                Dim Name As String = PackNames(i + packsMenuIndex(2))
                If EnabledPackNames.Contains(Name) = True Then
                    If i + packsMenuIndex(2) = packsMenuIndex(0) Then
                        SpriteBatch.DrawString(FontManager.InGameFont, Name, New Vector2(CInt(windowSize.Width / 2) - 320 + 2, CInt(Core.windowSize.Height / 2 - 120 + i * 50 + 2)), New Color(0, 0, 0, CInt(255 * _interfaceFade * _pageFade)))
                        SpriteBatch.DrawString(FontManager.InGameFont, Name, New Vector2(CInt(windowSize.Width / 2) - 320, CInt(Core.windowSize.Height / 2 - 120 + i * 50)), New Color(181, 255, 82, CInt(255 * _interfaceFade * _pageFade)))
                    Else
                        SpriteBatch.DrawString(FontManager.InGameFont, Name, New Vector2(CInt(windowSize.Width / 2) - 320, CInt(Core.windowSize.Height / 2 - 120 + i * 50)), New Color(98, 205, 8, CInt(255 * _interfaceFade * _pageFade)))
                    End If
                Else
                    If i + packsMenuIndex(2) = packsMenuIndex(0) Then
                        SpriteBatch.DrawString(FontManager.InGameFont, Name, New Vector2(CInt(windowSize.Width / 2) - 320 + 2, CInt(Core.windowSize.Height / 2 - 120 + i * 50 + 2)), textColor)
                        SpriteBatch.DrawString(FontManager.InGameFont, Name, New Vector2(CInt(windowSize.Width / 2) - 320, CInt(Core.windowSize.Height / 2 - 120 + i * 50)), New Color(255, 255, 255, CInt(255 * _interfaceFade * _pageFade)))
                    Else
                        SpriteBatch.DrawString(FontManager.InGameFont, Name, New Vector2(CInt(windowSize.Width / 2) - 320, CInt(Core.windowSize.Height / 2 - 120 + i * 50)), textColor)
                    End If
                End If
            Next
        End If

    End Sub

    Private Sub UpdatePacksMenu()
        Dim currentIndex As Integer = packsMenuIndex(0)
        Dim currentControl As Control = Nothing

        For Each control As Control In ControlList
            If control._position = _cursorDestPosition Then
                currentControl = control
                Exit For
            End If
        Next

        If Controls.Up(True, True, True) = True Then
            If currentControl.ID > 4 Then
                packsMenuIndex(0) -= 1
                If packsMenuIndex(0) - packsMenuIndex(2) < 0 Then
                    packsMenuIndex(2) -= 1
                End If
                If SelectPackNoiseDelay = 0 Then
                    SoundManager.PlaySound("select", 0.0F, 0.0F, 0.5F, False)
                    SelectPackNoiseDelay = 10
                End If
            End If
        End If
        If Controls.Down(True, True, True) = True Then
            If currentControl.ID > 4 Then
                packsMenuIndex(0) += 1
                If packsMenuIndex(0) + packsMenuIndex(2) > 3 Then
                    packsMenuIndex(2) += 1
                End If
                If SelectPackNoiseDelay = 0 Then
                    SoundManager.PlaySound("select", 0.0F, 0.0F, 0.5F, False)
                    SelectPackNoiseDelay = 10
                End If
            End If
        End If
        If GameInstance.IsMouseVisible = True Then
            For i = 0 To 3
                If New Rectangle(CInt(windowSize.Width / 2 - 328), CInt(windowSize.Height / 2 - 128 + i * 50), 500, 48).Contains(MouseHandler.MousePosition) = True Then
                    If MouseHandler.ButtonPressed(MouseHandler.MouseButtons.LeftButton) = True Then
                        packsMenuIndex(0) = i + packsMenuIndex(2)
                        If SelectPackNoiseDelay = 0 Then
                            SoundManager.PlaySound("select", 0.0F, 0.0F, 0.5F, False)
                            SelectPackNoiseDelay = 10
                        End If
                    End If
                End If
            Next
        End If
        packsMenuIndex(0) = CInt(MathHelper.Clamp(packsMenuIndex(0), 0, PackNames.Count - 1))
        packsMenuIndex(2) = CInt(MathHelper.Clamp(packsMenuIndex(2), 0, PackNames.Count - 4))

        If SelectPackNoiseDelay > 0 Then
            SelectPackNoiseDelay -= 1
        End If

    End Sub

    Private Sub ButtonPackInformation()
        If PackNames.Count = 0 Then
            Exit Sub
        End If

        Dim packName As String = PackNames(packsMenuIndex(0))
        PInfoSplash = Nothing

        Try
            If IO.File.Exists(GameController.GamePath & "\ContentPacks\" & packName & "\splash.png") = True Then
                Using stream As IO.Stream = IO.File.Open(GameController.GamePath & "\ContentPacks\" & packName & "\splash.png", IO.FileMode.OpenOrCreate)
                    PInfoSplash = Texture2D.FromStream(GraphicsDevice, stream)
                End Using
            End If
        Catch ex As Exception
            Logger.Log(Logger.LogTypes.ErrorMessage, "MainMenuScreen.vb/ButtonPackInformation: An error occurred trying to load the splash image at """ & GameController.GamePath & "\ContentPacks\" & packName & "\splash.png" & """. This could have been caused by an invalid file header. (Exception: " & ex.Message & ")")
        End Try

        Dim contentPackPath As String = GameController.GamePath & "\ContentPacks\" & packName & "\"

        Dim s() As String = ContentPackManager.GetContentPackInfo(packName)

        If s.Length > 0 Then
            PInfoVersion = s(0).CropStringToWidth(FontManager.InGameFont, 540 - 16 - CInt(FontManager.InGameFont.MeasureString(Localization.Translate("option_screen_contentpacks_version")).X))
        End If
        If s.Length > 1 Then
            PInfoAuthor = s(1).CropStringToWidth(FontManager.InGameFont, 540 - 16 - CInt(FontManager.InGameFont.MeasureString(Localization.Translate("option_screen_contentpacks_by")).X))
        End If
        If s.Length > 2 Then
            PInfoDescription = s(2).CropStringToWidth(FontManager.InGameFont, 540)
        End If
        PInfoName = packName
    End Sub

    Private PInfoName As String = ""
    Private PInfoSplash As Texture2D = Nothing
    Private PInfoVersion As String = ""
    Private PInfoAuthor As String = ""
    Private PInfoDescription As String = ""

    Private Sub DrawPackInformationMenu()
        If Not PInfoSplash Is Nothing Then
            SpriteBatch.Draw(PInfoSplash, windowSize, Color.White)
        End If

        Dim CanvasTexture As Texture2D = TextureManager.GetTexture("GUI\Menus\Menu", New Rectangle(0, 0, 48, 48), "")

        Canvas.DrawRectangle(New Rectangle(CInt(windowSize.Width / 2) - CInt(FontManager.InGameFont.MeasureString(Localization.Translate("option_screen_contentpacks_name") & ": " & PInfoName).X / 2) - 32, CInt(Core.windowSize.Height / 2 - 144), CInt(FontManager.InGameFont.MeasureString(Localization.Translate("option_screen_contentpacks_name") & ": " & PInfoName).X) + 64, 64), New Color(77, 147, 198, CInt(255 * _interfaceFade * _pageFade)))
        SpriteBatch.DrawString(FontManager.InGameFont, Localization.Translate("option_screen_contentpacks_name") & ": " & PInfoName, New Vector2(CInt(windowSize.Width / 2) - CInt(FontManager.InGameFont.MeasureString(Localization.Translate("option_screen_contentpacks_name") & ": " & PInfoName).X / 2) + 2, CInt(Core.windowSize.Height / 2 - 128 + 2)), New Color(0, 0, 0, CInt(255 * _interfaceFade * _pageFade)))
        SpriteBatch.DrawString(FontManager.InGameFont, Localization.Translate("option_screen_contentpacks_name") & ": " & PInfoName, New Vector2(CInt(windowSize.Width / 2) - CInt(FontManager.InGameFont.MeasureString(Localization.Translate("option_screen_contentpacks_name") & ": " & PInfoName).X / 2), CInt(Core.windowSize.Height / 2 - 128)), New Color(255, 255, 255, CInt(255 * _interfaceFade * _pageFade)))

        Canvas.DrawRectangle(New Rectangle(CInt(windowSize.Width / 2) - 278, CInt(Core.windowSize.Height / 2 - 72), 556, 196), New Color(255, 255, 255, CInt(255 * _interfaceFade * _pageFade)))
        SpriteBatch.DrawString(FontManager.InGameFont, Localization.Translate("option_screen_contentpacks_version") & ": " & PInfoVersion & Environment.NewLine & Localization.Translate("option_screen_contentpacks_by") & ": " & PInfoAuthor & Environment.NewLine & Localization.Translate("option_screen_contentpacks_description") & ": " & Environment.NewLine & PInfoDescription.Replace("<br>", Environment.NewLine).Replace("~", Environment.NewLine), New Vector2(CInt(windowSize.Width / 2) - 278 + 16, CInt(Core.windowSize.Height / 2 - 64)), New Color(0, 0, 0, CInt(255 * _interfaceFade * _pageFade)))
    End Sub

    Private Sub UpdatePackInformationMenu()
        If KeyBoardHandler.KeyPressed(KeyBindings.EscapeKey) Or KeyBoardHandler.KeyPressed(KeyBindings.BackKey1) Or KeyBoardHandler.KeyPressed(KeyBindings.BackKey2) Or MouseHandler.ButtonPressed(MouseHandler.MouseButtons.RightButton) Or ControllerHandler.ButtonPressed(Buttons.B) Then
            SwitchToContentPacks()
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

    Private Sub PackEnabledToggle(ByVal c As ToggleButton)
        If PackNames.Count > 0 Then
            If EnabledPackNames.Contains(PackNames(packsMenuIndex(0) + packsMenuIndex(2))) Then
                isSelectedEnabled = False
                c.Toggled = False
                ButtonToggle(PackNames(packsMenuIndex(0) + packsMenuIndex(2)))
            Else
                isSelectedEnabled = True
                c.Toggled = True
                ButtonToggle(PackNames(packsMenuIndex(0) + packsMenuIndex(2)))
            End If
        Else
            isSelectedEnabled = False
            c.Toggled = False
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

    Private Sub PacksApply()
        If PackNames.Count > 0 Then
            Core.GameOptions.ContentPackNames = EnabledPackNames.ToArray()
            Core.GameOptions.SaveOptions()
            MusicManager.PlayNoMusic()
            ContentPackManager.Clear()
            Water.WaterSpeed = 8
            For Each s As String In Core.GameOptions.ContentPackNames
                ContentPackManager.Load(GameController.GamePath & "\ContentPacks\" & s & "\exceptions.dat")
            Next
            SoundManager.PlaySound("save", False)
            Core.GameOptions.ChangedPack = True
            Core.OffsetMaps.Clear()
            MusicManager.Play("title")
        End If
    End Sub

#End Region

    Private Sub DrawBackground()
        Dim mainBackgroundColor As Color = Color.White
        If _closing Then
            mainBackgroundColor = New Color(255, 255, 255, CInt(255 * _interfaceFade))
        End If

        Dim halfWidth As Integer = CInt(Core.windowSize.Width / 2)
        Dim halfHeight As Integer = CInt(Core.windowSize.Height / 2)

        Canvas.DrawRectangle(New Rectangle(halfWidth - 400, halfHeight - 232, 260, 32), New Color(84, 198, 216, mainBackgroundColor.A))
        Canvas.DrawRectangle(New Rectangle(halfWidth - 140, halfHeight - 216, 16, 16), New Color(84, 198, 216, mainBackgroundColor.A))
        SpriteBatch.Draw(_texture, New Rectangle(halfWidth - 140, halfHeight - 232, 16, 16), New Rectangle(80, 0, 16, 16), mainBackgroundColor)
        SpriteBatch.Draw(_texture, New Rectangle(halfWidth - 124, halfHeight - 216, 16, 16), New Rectangle(80, 0, 16, 16), mainBackgroundColor)

        SpriteBatch.DrawString(FontManager.ChatFont, Localization.Translate("option.title"), New Vector2(halfWidth - 390, halfHeight - 228), mainBackgroundColor)

        For y = 0 To CInt(_enrollY) Step 16
            For x = 0 To 800 Step 16
                SpriteBatch.Draw(_texture, New Rectangle(halfWidth - 400 + x, halfHeight - 200 + y, 16, 16), New Rectangle(64, 0, 4, 4), mainBackgroundColor)
            Next
        Next

        Dim modRes As Integer = CInt(_enrollY) Mod 16
        If modRes > 0 Then
            For x = 0 To 800 Step 16
                SpriteBatch.Draw(_texture, New Rectangle(halfWidth - 400 + x, CInt(_enrollY + (halfHeight - 200)), 16, modRes), New Rectangle(64, 0, 4, 4), mainBackgroundColor)
            Next
        End If
    End Sub
    Private Sub DrawCursor()
        Dim t As Texture2D = TextureManager.GetTexture("GUI\Menus\General", New Rectangle(0, 0, 16, 16), "")
        Core.SpriteBatch.Draw(t, New Rectangle(CInt(_cursorPosition.X) + 60, CInt(_cursorPosition.Y) - 28, 48, 48), New Rectangle(0, 0, 16, 16), New Color(255, 255, 255, CInt(255 * Me._interfaceFade)), 0.0F, Vector2.Zero, SpriteEffects.None, 0.0F)
    End Sub
    Private Sub DrawCurrentPage()
        For Each C As Control In ControlList
            C.Draw()
        Next
    End Sub
    Private Sub DrawMessage()

    End Sub

    Public Overrides Sub Update()

        Me.PreScreen.Update()
        'New stuff
        If _opening Then
            InitializeControls()
            _opening = False
        End If

        'Refresh button positions
        If windowSize.Width <> _screenSize.Width Or windowSize.Height <> _screenSize.Height Then
            _screenSize = New Size(CInt(windowSize.Width), CInt(windowSize.Height))
            InitializeControls()
        End If

        If _closing Then
            ' When the interface is closing, only update the closing animation
            ' Once the interface is completely closed, set to the previous screen.

            If _interfaceFade > 0F Then
                _interfaceFade = MathHelper.Lerp(0, _interfaceFade, 0.8F)
                If _interfaceFade < 0F Then
                    _interfaceFade = 0F
                End If
            End If
            If _enrollY > 0 Then
                _enrollY = MathHelper.Lerp(0, _enrollY, 0.8F)
                If _enrollY <= 0 Then
                    _enrollY = 0
                End If
            End If
            If _enrollY <= 2.0F Then
                'TODO: Set the interface state to PlayerTemp.
                SetScreen(PreScreen)
            End If
        Else
            'Update intro animation:
            Dim maxWindowHeight As Integer = 400
            If _enrollY < maxWindowHeight Then
                _enrollY = MathHelper.Lerp(maxWindowHeight, _enrollY, 0.8F)
                If _enrollY >= maxWindowHeight Then
                    _enrollY = maxWindowHeight
                End If
            End If
            If _interfaceFade < 1.0F Then
                _interfaceFade = MathHelper.Lerp(1.0F, _interfaceFade, 0.95F)
                If _interfaceFade > 1.0F Then
                    _interfaceFade = 1.0F
                End If
            End If
            If _itemIntro < 1.0F Then
                _itemIntro += 0.05F
                If _itemIntro > 1.0F Then
                    _itemIntro = 1.0F
                End If
            End If

            'Update the Dialogues:
            ChooseBox.Update()
            If ChooseBox.Showing = False Then
                TextBox.Update()
            End If

            If _pageClosing = True Then
                If _pageFade >= 0F Then
                    _pageFade -= 0.07F
                    If _pageFade <= 0F Then
                        _pageFade = 0F
                        _pageClosing = False
                        _pageOpening = True
                        ScreenIndex = _nextIndex
                        InitializeControls()
                    End If
                End If
            End If
            If _pageOpening = True Then
                If _pageFade <= 1.0F Then
                    _pageFade += 0.07F
                    If _pageFade >= 1.0F Then
                        _pageFade = 1.0F
                        _pageClosing = False
                        _pageOpening = False
                    End If
                End If
            End If

            If _cursorDestPosition.X <> _cursorPosition.X Or _cursorDestPosition.Y <> _cursorPosition.Y Then
                _cursorPosition.X = MathHelper.Lerp(_cursorDestPosition.X, _cursorPosition.X, 0.75F)
                _cursorPosition.Y = MathHelper.Lerp(_cursorDestPosition.Y, _cursorPosition.Y, 0.75F)

                If Math.Abs(_cursorDestPosition.X - _cursorPosition.X) < 0.1F Then
                    _cursorPosition.X = _cursorDestPosition.X
                End If
                If Math.Abs(_cursorDestPosition.Y - _cursorPosition.Y) < 0.1F Then
                    _cursorPosition.Y = _cursorDestPosition.Y
                End If
            End If
            If Not _selectedScrollBar Then
                If Controls.Up(True, True, False, True, True, True) = True Then
                    SetCursorPosition("up")
                End If
                If Controls.Down(True, True, False, True, True, True) = True Then
                    SetCursorPosition("down")
                End If
                If Controls.Right(True, True, False, True, True, True) = True Then
                    SetCursorPosition("right")
                End If
                If Controls.Left(True, True, False, True, True, True) = True Then
                    SetCursorPosition("left")
                End If
                If ScreenIndex <> 6 Then
                    If Controls.Left(False, False, True, False, False, False) = True Then
                        SetCursorPosition("previous")
                    End If
                    If Controls.Right(False, False, True, False, False, False) = True Then
                        SetCursorPosition("next")
                    End If
                End If

                If KeyBoardHandler.KeyPressed(KeyBindings.EscapeKey) Or KeyBoardHandler.KeyPressed(KeyBindings.BackKey1) Or KeyBoardHandler.KeyPressed(KeyBindings.BackKey2) Or MouseHandler.ButtonPressed(MouseHandler.MouseButtons.RightButton) Or ControllerHandler.ButtonPressed(Buttons.B) Then
                    If _pageClosing = False And _pageOpening = False Then
                        SoundManager.PlaySound("select")
                        If ScreenIndex = 0 Or _subMenu <> 0 Then
                            If ScreenIndex = 8 Then
                                SwitchToContentPacks()
                            Else
                                _closing = True
                            End If
                        Else
                            SwitchToMain()
                        End If
                    End If
                End If
            End If
            Select Case ScreenIndex
                Case 6
                    UpdateLanguageMenu()
                Case 7
                    UpdatePacksMenu()
                Case 8
                    UpdatePackInformationMenu()
            End Select
            For i = 0 To ControlList.Count
                If i <= ControlList.Count - 1 Then
                    ControlList(i).Update(Me)
                End If
            Next
        End If
    End Sub
    Private Sub SetCursorPosition(ByVal direction As String)
        Dim ScrollControl As Control = Nothing
        Dim pos = GetButtonPosition(direction)
        Dim cPosition As Vector2 = New Vector2(pos.X, pos.Y)
        'Dim cPosition As Vector2 = New Vector2(pos.X + 180, pos.Y - 42)
        For Each control As Control In ControlList
            If control._position = New Vector2(pos.X, pos.Y) Then
                If control.ControlType = "ScrollBar" Then
                    ScrollControl = control
                    Exit For
                End If
            End If
        Next
        If ScrollControl IsNot Nothing Then
            cPosition.X += 332
        End If

        _cursorDestPosition = cPosition
    End Sub

    Private Function GetButtonPosition(ByVal direction As String) As Vector2
        Dim EligibleControls As New List(Of Control)
        Dim currentControl As Control = Nothing

        For Each control As Control In ControlList
            If control.ControlType = "ScrollBar" Then
                If control._position.Y = _cursorDestPosition.Y Then
                    currentControl = control
                    Exit For
                End If
            Else
                If control._position = _cursorDestPosition Then
                    currentControl = control
                    Exit For
                End If
            End If
        Next

        For Each control As Control In ControlList
            Dim R2 As Vector2 = control._position
            Dim R1 As Vector2 = currentControl._position

            If R1 = R2 Then
                Continue For
            End If

            Select Case direction
                Case "up"
                    If ScreenIndex = 0 Then
                        Select Case currentControl.ID
                            Case 4
                                If control.ID = 1 Then
                                    EligibleControls.Add(control)
                                End If
                            Case 5
                                If control.ID = 3 Then
                                    EligibleControls.Add(control)
                                End If
                            Case 6, 7
                                If control.ID = 4 Then
                                    EligibleControls.Add(control)
                                End If
                            Case 8
                                If control.ID = 5 Then
                                    EligibleControls.Add(control)
                                End If
                        End Select
                    Else
                        If ScreenIndex = 7 Then
                            If currentControl.ID <= 4 Then
                                If control.ID = currentControl.ID - 1 Then
                                    EligibleControls.Add(control)
                                End If
                            End If
                        ElseIf ScreenIndex = 5 Then
                            If currentControl.ID > 3 Then
                                If control.ID = 3 Then
                                    EligibleControls.Add(control)
                                End If
                            ElseIf control.ID = currentControl.ID - 1 Then
                                EligibleControls.Add(control)
                            End If
                        ElseIf Math.Abs(R2.X - R1.X) <= -(R2.Y - R1.Y) Then 'because Y axis points down 
                            EligibleControls.Add(control)
                        End If
                    End If
                Case "down"
                    If ScreenIndex = 0 Then
                        Select Case currentControl.ID
                            Case 1, 2
                                If control.ID = 4 Then
                                    EligibleControls.Add(control)
                                End If
                            Case 3
                                If control.ID = 5 Then
                                    EligibleControls.Add(control)
                                End If
                            Case 4
                                If control.ID = 6 Then
                                    EligibleControls.Add(control)
                                End If
                            Case 5
                                If control.ID = 8 Then
                                    EligibleControls.Add(control)
                                End If
                        End Select
                    ElseIf ScreenIndex = 5 Then
                        If currentControl.ID < 4 Then
                            If control.ID = currentControl.ID + 1 Then
                                EligibleControls.Add(control)
                            End If
                        End If
                    ElseIf Math.Abs(R2.X - R1.X) <= -(R1.Y - R2.Y) Then 'because Y axis points down 
                        EligibleControls.Add(control)
                    End If
                Case "right"
                    If ScreenIndex = 7 Then
                        If currentControl.ID = 5 And control.ID = 6 Then
                            EligibleControls.Add(control)
                        ElseIf currentControl.ID = 6 And control.ID = 4 Then
                            EligibleControls.Add(control)
                        End If
                    ElseIf control.ID = currentControl.ID + 1 Then
                        EligibleControls.Add(control)
                    End If
                Case "left"
                    If ScreenIndex = 7 Then
                        If currentControl.ID <= 4 And control.ID = 6 Then
                            EligibleControls.Add(control)
                        ElseIf currentControl.ID = 6 And control.ID = 5 Then
                            EligibleControls.Add(control)
                        End If
                    ElseIf control.ID = currentControl.ID - 1 Then
                        EligibleControls.Add(control)
                    End If
                Case "next"
                    If ScreenIndex = 7 And currentControl.ID < 4 Then
                        If control.ID = currentControl.ID + 1 Then
                            EligibleControls.Add(control)
                        End If
                    ElseIf ScreenIndex <> 7 Then
                        If control.ID = currentControl.ID + 1 Then
                            EligibleControls.Add(control)
                        End If
                    End If
                Case "previous"
                    If ScreenIndex = 7 And currentControl.ID <= 4 Then
                        If control.ID = currentControl.ID - 1 Then
                            EligibleControls.Add(control)
                        End If
                    ElseIf ScreenIndex <> 7 Then
                        If control.ID = currentControl.ID - 1 Then
                            EligibleControls.Add(control)
                        End If
                    End If
            End Select
        Next

        Dim NextPosition As New Vector2(currentControl._position.X, currentControl._position.Y)

        Dim cDistance As Double = 99999D
        For Each control As Control In EligibleControls
            Dim R2 As Vector2 = control._position
            Dim R1 As Vector2 = currentControl._position
            Dim DeltaR As Vector2 = R2 - R1
            Dim Distance As Double = DeltaR.Length
            If Distance < cDistance Then
                NextPosition = control._position
                cDistance = Distance
            End If
        Next

        Return NextPosition
    End Function

    Private Sub InitializeControls()
        Me.ControlList.Clear()
        Me._selectedScrollBar = False

        Dim halfWidth As Integer = CInt(Core.windowSize.Width / 2)
        Dim halfHeight As Integer = CInt(Core.windowSize.Height / 2)

        Dim Delta_X As Integer = halfWidth - 400
        Dim Delta_Y As Integer = halfHeight - 200

        Select Case ScreenIndex
            Case 0 ' Main Options menu.
                Me.ControlList.Add(New CommandButton(New Vector2(Delta_X + 90, Delta_Y + 80), 1, 64, Localization.Translate("option_screen_game", "Game"), AddressOf SwitchToGame, 1))
                Me.ControlList.Add(New CommandButton(New Vector2(Delta_X + 310, Delta_Y + 80), 1, 64, Localization.Translate("option_screen_graphics", "Graphics"), AddressOf SwitchToGraphics, 2))
                Me.ControlList.Add(New CommandButton(New Vector2(Delta_X + 530, Delta_Y + 80), 1, 64, Localization.Translate("option_screen_battle", "Battle"), AddressOf SwitchToBattle, 3))
                Me.ControlList.Add(New CommandButton(New Vector2(Delta_X + 200, Delta_Y + 168), 1, 64, Localization.Translate("option_screen_controls", "Controls"), AddressOf SwitchToControls, 4))
                Me.ControlList.Add(New CommandButton(New Vector2(Delta_X + 420, Delta_Y + 168), 1, 64, Localization.Translate("option_screen_audio", "Audio"), AddressOf SwitchToAudio, 5))

                Me.ControlList.Add(New CommandButton(New Vector2(Delta_X + 90 + 24, Delta_Y + 336), 1, 48, Localization.Translate("global_apply", "Apply"), AddressOf Apply, 6))
                Me.ControlList.Add(New CommandButton(New Vector2(Delta_X + 286 + 24, Delta_Y + 336), 2, 48, Localization.Translate("option_screen_resetoptions", "Reset Options"), AddressOf Reset, 7))
                Me.ControlList.Add(New CommandButton(New Vector2(Delta_X + 530 + 24, Delta_Y + 336), 1, 48, Localization.Translate("global_close", "Close"), AddressOf Close, 8))

            Case 1 ' "Game" from the Options menu.
                Me.ControlList.Add(New ScrollBar(New Vector2(Delta_X + 100, Delta_Y + 60), 400, Localization.Translate("option_screen_game_textspeed", "Text Speed"), Me.TextSpeed, 1, 3, AddressOf ChangeTextspeed, 1))
                If CBool(GameModeManager.GetGameRuleValue("LockDifficulty", "0")) = False Then
                    Dim d As New Dictionary(Of Integer, String)
                    d.Add(0, Localization.Translate("option_screen_game_difficulty_easy", "Easy"))
                    d.Add(1, Localization.Translate("option_screen_game_difficulty_hard", "Hard"))
                    d.Add(2, Localization.Translate("option_screen_game_difficulty_superhard", "Super Hard"))

                    Me.ControlList.Add(New ScrollBar(New Vector2(Delta_X + 100, Delta_Y + 120), 400, Localization.Translate("option_screen_game_difficulty", "Difficulty"), Me.Difficulty, 0, 2, AddressOf ChangeDifficulty, d, 2))
                End If
                Me.ControlList.Add(New ToggleButton(New Vector2(Delta_X + 100, Delta_Y + 180), 3, 64, Localization.Translate("option_screen_game_viewbobbing", "View Bobbing"), Me.ViewBobbing, AddressOf ToggleBobbing, {Localization.Translate("global_off", "Off"), Localization.Translate("global_on", "On")}.ToList(), 3))
                Me.ControlList.Add(New CommandButton(New Vector2(Delta_X + 530 + 24, Delta_Y + 327), 1, 48, Localization.Translate("global_back", "Back"), AddressOf SwitchToMain, 4))

            Case 2 ' "Graphics" from the Options menu.
                Me.ControlList.Add(New ScrollBar(New Vector2(Delta_X + 100, Delta_Y + 40), 400, Localization.Translate("option_screen_graphics_fov", "Field of View"), CInt(Me.FOV), 45, 120, AddressOf ChangeFOV, 1))

                Dim d As New Dictionary(Of Integer, String)
                d.Add(0, Localization.Translate("option_screen_graphics_renderdistance_tiny", "Tiny"))
                d.Add(1, Localization.Translate("option_screen_graphics_renderdistance_small", "Small"))
                d.Add(2, Localization.Translate("option_screen_graphics_renderdistance_normal", "Normal"))
                d.Add(3, Localization.Translate("option_screen_graphics_renderdistance_far","Far"))
                d.Add(4, Localization.Translate("option_screen_graphics_renderdistance_extreme", "Extreme"))
                Me.ControlList.Add(New ScrollBar(New Vector2(Delta_X + 100, Delta_Y + 100), 400, Localization.Translate("option_screen_graphics_renderdistance", "Render Distance"), Me.RenderDistance, 0, 4, AddressOf ChangeRenderDistance, d, 2))

                Dim d1 As New Dictionary(Of Integer, String)
                d1.Add(0, "Off")
                Me.ControlList.Add(New ScrollBar(New Vector2(Delta_X + 100, Delta_Y + 160), 400, Localization.Translate("option_screen_graphics_offset_mapquality", "Offset Map Quality"), Me.LoadOffsetMaps, 0, 100, AddressOf ChangeOffsetMaps, d1, 3))

                Me.ControlList.Add(New ToggleButton(New Vector2(Delta_X + 100, Delta_Y + 220), 3, 64, Localization.Translate("option_screen_graphics_graphics", "Graphics"), CBool(Me.GraphicStyle), AddressOf ToggleGraphicsStyle, {Localization.Translate("option_screen_graphics_graphics_fast", "Fast"), Localization.Translate("option_screen_graphics_graphics_fancy", "Fancy")}.ToList(), 4))
                Me.ControlList.Add(New ToggleButton(New Vector2(Delta_X + 100, Delta_Y + 300), 3, 64, Localization.Translate("option_screen_graphics_multisampling", "Multi Sampling"), Me.PreferMultiSampling, AddressOf ToggleMultiSampling, {"Off", "On"}.ToList(), 5))

                Me.ControlList.Add(New CommandButton(New Vector2(Delta_X + 530 + 24, Delta_Y + 327), 1, 48, Localization.Translate("global_back", "Back"), AddressOf SwitchToMain, 6))

            Case 3 ' "Battle" from the Options menu.
                Me.ControlList.Add(New ToggleButton(New Vector2(Delta_X + 100 + 20, Delta_Y + 100), 2, 64, Localization.Translate("option_screen_battle_3dmodels", "3D Models"), CBool(ShowModels), AddressOf ToggleShowModels, {Localization.Translate("global_off", "Off"), Localization.Translate("global_on", "On")}.ToList(), 1))
                Me.ControlList.Add(New ToggleButton(New Vector2(Delta_X + 400 + 20, Delta_Y + 100), 2, 64, Localization.Translate("option_screen_battle_animations", "Animations"), CBool(Me.ShowBattleAnimations), AddressOf ToggleAnimations, {Localization.Translate("global_off", "Off"), Localization.Translate("global_on", "On")}.ToList(), 2))
                Me.ControlList.Add(New ToggleButton(New Vector2(Delta_X + 100 + 20, Delta_Y + 200), 2, 64, Localization.Translate("option_screen_battle_battlestyle", "Battle Style"), CBool(Me.BattleStyle), AddressOf ToggleBattleStyle, {Localization.Translate("option_screen_battle_battlestyle_shift", "Shift"), Localization.Translate("option_screen_battle_battlestyle_set", "Set")}.ToList(), 3))

                Me.ControlList.Add(New CommandButton(New Vector2(Delta_X + 530 + 24, Delta_Y + 327), 1, 48, Localization.Translate("global_back", "Back"), AddressOf SwitchToMain, 4))

            Case 4 ' "Controls" from the Options menu.
                If PreScreen.Identification = Identifications.MainMenuScreen Then
                    Me.ControlList.Add(New ToggleButton(New Vector2(Delta_X + 100, Delta_Y + 100), 5, 64, Localization.Translate("option_screen_controls_xboxgamepad", "Xbox Gamepad"), Me.GamePadEnabled, AddressOf ToggleXBOX360Controller, {"Disabled", "Enabled"}.ToList(), 1))
                    Me.ControlList.Add(New CommandButton(New Vector2(Delta_X + 100, Delta_Y + 200), 3, 64, Localization.Translate("option_screen_controls_resetkeybindings", "Reset Key Bindings"), AddressOf ResetKeyBindings, 2))
                    Me.ControlList.Add(New CommandButton(New Vector2(Delta_X + 90 + 24, Delta_Y + 336), 1, 48, Localization.Translate("global_apply", "Apply"), AddressOf ControlsApply, 3))
                    Me.ControlList.Add(New CommandButton(New Vector2(Delta_X + 530 + 24, Delta_Y + 327), 1, 48, Localization.Translate("global_back", "Back"), AddressOf Close, 4))
                Else
                    Dim d As New Dictionary(Of Integer, String)
                    d.Add(1, Localization.Translate("option_screen_controls_cameraspeed_slow", "...Slow..."))
                    d.Add(12, Localization.Translate("option_screen_controls_cameraspeed_medium", "Standard"))
                    d.Add(38, Localization.Translate("option_screen_controls_cameraspeed_fast", "Super fast!"))
                    d.Add(50, Localization.Translate("option_screen_controls_cameraspeed_fastest", "SPEED OF LIGHT!"))
                    Me.ControlList.Add(New ScrollBar(New Vector2(Delta_X + 100, Delta_Y + 60), 400, Localization.Translate("option_screen_controls_cameraspeed", "Camera Speed"), Me.CameraSpeed, 1, 50, AddressOf ChangeCameraSpeed, d, 1))
                    Me.ControlList.Add(New ToggleButton(New Vector2(Delta_X + 100, Delta_Y + 120), 5, 64, Localization.Translate("option_screen_controls_xboxgamepad", "Xbox Gamepad"), Me.GamePadEnabled, AddressOf ToggleXBOX360Controller, {"Disabled", "Enabled"}.ToList(), 2))
                    Me.ControlList.Add(New ToggleButton(New Vector2(Delta_X + 100, Delta_Y + 200), 5, 64, Localization.Translate("option_screen_controls_running", "Running"), Me.RunMode, AddressOf ToggleRunningToggle, {"Hold", "Toggle"}.ToList(), 3))
                    Me.ControlList.Add(New CommandButton(New Vector2(Delta_X + 100, Delta_Y + 280), 3, 64, Localization.Translate("option_screen_controls_resetkeybindings", "Reset Key Bindings"), AddressOf ResetKeyBindings, 4))
                    Me.ControlList.Add(New CommandButton(New Vector2(Delta_X + 530 + 24, Delta_Y + 327), 1, 48, Localization.Translate("global_back", "Back"), AddressOf SwitchToMain, 5))
                End If

            Case 5 ' "Audio" from the Options menu.
                Me.ControlList.Add(New ScrollBar(New Vector2(Delta_X + 100, Delta_Y + 60), 400, Localization.Translate("option_screen_audio_volume_music", "Music Volume"), Me.Music, 0, 100, AddressOf ChangeMusicVolume, 1))
                Me.ControlList.Add(New ScrollBar(New Vector2(Delta_X + 100, Delta_Y + 120), 400, Localization.Translate("option_screen_audio_volume_sfx", "SoundFX Volume"), Me.Sound, 0, 100, AddressOf ChangeSoundVolume, 2))
                Me.ControlList.Add(New ToggleButton(New Vector2(Delta_X + 100, Delta_Y + 200), 1, 64, Localization.Translate("option_screen_audio_muted", "Muted"), CBool(Me.Muted), AddressOf ToggleMute, {"No", "Yes"}.ToList(), 3))
                If PreScreen.Identification = Identifications.MainMenuScreen Then
                    Me.ControlList.Add(New CommandButton(New Vector2(Delta_X + 90 + 24, Delta_Y + 327), 1, 48, Localization.Translate("global_apply", "Apply"), AddressOf AudioApply, 4))
                    Me.ControlList.Add(New CommandButton(New Vector2(Delta_X + 530 + 24, Delta_Y + 327), 1, 48, Localization.Translate("global_back", "Back"), AddressOf Close, 5))
                Else
                    Me.ControlList.Add(New CommandButton(New Vector2(Delta_X + 530 + 24, Delta_Y + 327), 1, 48, Localization.Translate("global_back", "Back"), AddressOf SwitchToMain, 4))
                End If

            Case 6 ' "Language" from the Options menu.
                Me.ControlList.Add(New CommandButton(New Vector2(Delta_X + 90 + 24, Delta_Y + 336), 1, 48, Localization.Translate("global_apply", "Apply"), AddressOf LanguageApply, 1))
                Me.ControlList.Add(New CommandButton(New Vector2(Delta_X + 530 + 24, Delta_Y + 327), 1, 48, Localization.Translate("global_back", "Back"), AddressOf Close, 2))

            Case 7 ' "ContentPacks" from the Options menu.
                Me.ControlList.Add(New CommandButton(New Vector2(Delta_X + 604, Delta_Y + 64), 2, 48, Localization.Translate("option_screen_contentpacks_up"), AddressOf ButtonUp, 1))
                Me.ControlList.Add(New CommandButton(New Vector2(Delta_X + 604, Delta_Y + 120), 2, 48, Localization.Translate("option_screen_contentpacks_down"), AddressOf ButtonDown, 2))
                Me.ControlList.Add(New ToggleButton(New Vector2(Delta_X + 604, Delta_Y + 176), 2, 48, "", isSelectedEnabled, AddressOf PackEnabledToggle, {Localization.Translate("global_enable"), Localization.Translate("global_disable")}.ToList(), 3))
                Me.ControlList.Add(New CommandButton(New Vector2(Delta_X + 604, Delta_Y + 232), 2, 48, Localization.Translate("option_screen_contentpacks_information"), AddressOf SwitchToPackInformation, 4))

                Me.ControlList.Add(New CommandButton(New Vector2(Delta_X + 90 + 24, Delta_Y + 327), 1, 48, Localization.Translate("global_apply", "Apply"), AddressOf PacksApply, 5))
                Me.ControlList.Add(New CommandButton(New Vector2(Delta_X + 310 + 24, Delta_Y + 327), 1, 48, Localization.Translate("global_back", "Back"), AddressOf Close, 6))

            Case 8 ' "Information" from the ContentPacks menu.
                Me.ControlList.Add(New ToggleButton(New Vector2(Delta_X + 90 + 24, Delta_Y + 336), 2, 48, Localization.Translate("global_enabled"), isSelectedEnabled, AddressOf PackEnabledToggle, {Localization.Translate("global_no"), Localization.Translate("global_yes")}.ToList(), 1))
                Me.ControlList.Add(New CommandButton(New Vector2(Delta_X + 530 + 24, Delta_Y + 336), 1, 48, Localization.Translate("global_back", "Back"), AddressOf SwitchToContentPacks, 2))
        End Select

        If ScreenIndex <> 7 Then
            If ControlList(0).ControlType = "ScrollBar" Then
                _cursorDestPosition = New Vector2(ControlList(0)._position.X + 332, ControlList(0)._position.Y)
            Else
                _cursorDestPosition = ControlList(0)._position
            End If

        Else
            If ControlList(0).ControlType = "ScrollBar" Then
                _cursorDestPosition = New Vector2(ControlList(4)._position.X + 332, ControlList(4)._position.Y)
            Else
                _cursorDestPosition = ControlList(4)._position
            End If
        End If
    End Sub

    Private Sub Apply()
        Save()
        Close()
    End Sub

    Private Sub Close()
        If currentLanguage <> TempLanguage Then
            Localization.Load(TempLanguage)
        End If
        _closing = True
    End Sub
    Private Sub ControlsApply()
        Core.GameOptions.GamePadEnabled = Me.GamePadEnabled
        Core.GameOptions.SaveOptions()
        SoundManager.PlaySound("save")
        _closing = True
    End Sub

    Private Sub Reset()
        Me.FOV = 45.0F
        Me.TextSpeed = 2
        Me.CameraSpeed = 12
        Me.Music = 50
        Me.Sound = 50
        Me.RenderDistance = 2
        Me.GraphicStyle = 1
        Me.ShowBattleAnimations = 1
        Me.DiagonalMovement = False
        Me.Difficulty = 0
        Me.BattleStyle = 1
        Me.LoadOffsetMaps = 100
        Me.ViewBobbing = True
        Me.ShowModels = 1
        Me.Muted = 0
        Me.GamePadEnabled = True
        Me.PreferMultiSampling = True
        Me.Language = 0
        MusicManager.Muted = CBool(Me.Muted)
        SoundManager.Muted = CBool(Me.Muted)
    End Sub

    Private Sub Save()
        MusicManager.MasterVolume = CSng(Me.Music / 100)
        SoundManager.Volume = CSng(Me.Sound / 100)
        MusicManager.Muted = CBool(Me.Muted)
        SoundManager.Muted = CBool(Me.Muted)
        Core.GameOptions.RenderDistance = Me.RenderDistance
        Core.GameOptions.GraphicStyle = Me.GraphicStyle
        If PreScreen.Identification <> Identifications.MainMenuScreen Then
            Camera.CreateNewProjection(Me.FOV)
            TextBox.TextSpeed = Me.TextSpeed
            Camera.RotationSpeed = CSng(Me.CameraSpeed / 10000)
            Screen.Level.World.Initialize(Screen.Level.EnvironmentType, Screen.Level.WeatherType)
            Core.Player.RunMode = Me.RunMode
            Me.PreScreen.Update()
        End If
        Core.Player.ShowBattleAnimations = Me.ShowBattleAnimations
        Core.Player.DiagonalMovement = Me.DiagonalMovement
        Core.Player.DifficultyMode = Me.Difficulty
        Core.Player.BattleStyle = Me.BattleStyle
        Core.Player.ShowModelsInBattle = CBool(Me.ShowModels)
        Core.GameOptions.GamePadEnabled = Me.GamePadEnabled
        Core.GraphicsManager.PreferMultiSampling = Me.PreferMultiSampling
        If LoadOffsetMaps = 0 Then
            Core.GameOptions.LoadOffsetMaps = Me.LoadOffsetMaps
        Else
            Core.GameOptions.LoadOffsetMaps = 101 - Me.LoadOffsetMaps
        End If
        Core.GameOptions.ViewBobbing = Me.ViewBobbing
        Dim NewLanguage As String = Localization.GetLanguageISO(Localization.GetAvailableLanguagesList.Item(Me.Language))
        If Core.GameOptions.Language IsNot NewLanguage Then
            Core.GameOptions.Language = NewLanguage
            Logger.Debug("NewOptionScreen.vb Changed Language: " & NewLanguage)
            Localization.Load(NewLanguage)
        End If
        Core.GameOptions.SaveOptions()

        SoundManager.PlaySound("save")
    End Sub

    Public Overrides Sub ToggledMute()
        If ScreenIndex = 5 Then
            Me.Muted = CInt(MusicManager.Muted)
            InitializeControls()
        End If
    End Sub

#Region "ControlCommands"

#Region "Switch"

    Private Sub SwitchToMain()
        Me._nextIndex = 0
        Me._pageClosing = True
    End Sub

    Private Sub SwitchToGame()
        Me._nextIndex = 1
        Me._pageClosing = True
    End Sub

    Private Sub SwitchToGraphics()
        Me._nextIndex = 2
        Me._pageClosing = True
    End Sub

    Private Sub SwitchToBattle()
        Me._nextIndex = 3
        Me._pageClosing = True
    End Sub

    Private Sub SwitchToControls()
        Me._nextIndex = 4
        If PreScreen.Identification = Identifications.MainMenuScreen Then
            ScreenIndex = _nextIndex
            InitializeControls()
        Else
            Me._pageClosing = True
        End If
    End Sub

    Private Sub SwitchToAudio()
        Me._nextIndex = 5
        If PreScreen.Identification = Identifications.MainMenuScreen Then
            ScreenIndex = _nextIndex
            InitializeControls()
        Else
            Me._pageClosing = True
        End If
    End Sub

    Private Sub SwitchToLanguage()
        GetLanguages()
        If Languages.Contains(currentLanguage) = True Then
            languageMenuIndex(0) = Languages.IndexOf(currentLanguage)
            TempLanguage = currentLanguage
        End If
        languageMenuIndex(1) = 0
        languageMenuIndex(2) = 0
        Me._nextIndex = 6
        ScreenIndex = _nextIndex
        InitializeControls()
    End Sub

    Private Sub SwitchToContentPacks()
        If ScreenIndex <> 7 And ScreenIndex <> 8 Then
            GetPacks()
            packsMenuIndex(0) = 0
            packsMenuIndex(1) = 0
            packsMenuIndex(2) = 0
        End If
        Me._nextIndex = 7
        If ScreenIndex = 8 Then
            Me._pageClosing = True
        Else
            ScreenIndex = _nextIndex
        End If
        InitializeControls()
    End Sub
    Private Sub SwitchToPackInformation()
        If PackNames.Count > 0 Then
            Me._nextIndex = 8
            Me._pageClosing = True
        End If
        ButtonPackInformation()
    End Sub

#End Region

#Region "SettingsGraphics"

    Private Sub ChangeFOV(ByVal c As ScrollBar)
        Me.FOV = c.Value
    End Sub

    Private Sub ChangeRenderDistance(ByVal c As ScrollBar)
        Me.RenderDistance = c.Value
    End Sub

    Private Sub ToggleGraphicsStyle(ByVal c As ToggleButton)
        If c.Toggled = True Then
            Me.GraphicStyle = 1
        Else
            Me.GraphicStyle = 0
        End If
    End Sub

    Private Sub ChangeOffsetMaps(ByVal c As ScrollBar)
        Me.LoadOffsetMaps = c.Value
    End Sub

    Private Sub ToggleMultiSampling(ByVal c As ToggleButton)
        Me.PreferMultiSampling = Not Me.PreferMultiSampling
    End Sub

#End Region

#Region "SettingsGame"

    Private Sub ToggleBobbing(ByVal c As ToggleButton)
        Me.ViewBobbing = Not Me.ViewBobbing
    End Sub

    Private Sub ChangeTextspeed(ByVal c As ScrollBar)
        Me.TextSpeed = c.Value
    End Sub

    Private Sub ChangeDifficulty(ByVal c As ScrollBar)
        Me.Difficulty = c.Value
    End Sub

#End Region

#Region "SettingsBattle"

    Private Sub ToggleShowModels(ByVal c As ToggleButton)
        If Me.ShowModels = 0 Then
            Me.ShowModels = 1
        Else
            Me.ShowModels = 0
        End If
    End Sub

    Private Sub ToggleAnimations(ByVal c As ToggleButton)
        If Me.ShowBattleAnimations <> 1 Then
            Me.ShowBattleAnimations = 1
        Else
            Me.ShowBattleAnimations = 0
        End If
    End Sub

    Private Sub ToggleBattleStyle(ByVal c As ToggleButton)
        If Me.BattleStyle = 0 Then
            Me.BattleStyle = 1
        Else
            Me.BattleStyle = 0
        End If
    End Sub

#End Region

#Region "SettingsControls"

    Private Sub ToggleXBOX360Controller(ByVal c As ToggleButton)
        Me.GamePadEnabled = Not Me.GamePadEnabled
    End Sub
    Private Sub ToggleRunningToggle(ByVal c As ToggleButton)
        Me.RunMode = Not Me.RunMode
    End Sub

    Private Sub ChangeCameraSpeed(ByVal c As ScrollBar)
        Me.CameraSpeed = c.Value
    End Sub

    Private Sub ResetKeyBindings(ByVal c As CommandButton)
        KeyBindings.CreateKeySave(True)
        KeyBindings.LoadKeys()
    End Sub

#End Region

#Region "SettingsAudio"

    Private Sub ChangeMusicVolume(ByVal c As ScrollBar)
        Me.Music = c.Value
        ApplyMusicChange()
    End Sub

    Private Sub ChangeSoundVolume(ByVal c As ScrollBar)
        Me.Sound = c.Value
        ApplyMusicChange()
    End Sub

    Private Sub ToggleMute(ByVal c As ToggleButton)
        If Me.Muted = 0 Then
            Me.Muted = 1
        Else
            Me.Muted = 0
        End If
        ApplyMusicChange()
    End Sub

    Private Sub ApplyMusicChange()
        MusicManager.Muted = CBool(Me.Muted)
        SoundManager.Muted = CBool(Me.Muted)
        MusicManager.MasterVolume = CSng(Me.Music / 100)
        SoundManager.Volume = CSng(Me.Sound / 100)
    End Sub


    Private Sub AudioApply()
        MusicManager.MasterVolume = CSng(Me.Music / 100)
        SoundManager.Volume = CSng(Me.Sound / 100)
        MusicManager.Muted = CBool(Me.Muted)
        SoundManager.Muted = CBool(Me.Muted)
        Core.GameOptions.SaveOptions()
        SoundManager.PlaySound("save")
        _closing = True
    End Sub

#End Region
#Region "SettingsLanguage"
    Private Sub LanguageApply()
        If currentLanguage <> Languages(languageMenuIndex(0)) Then
            currentLanguage = Languages(languageMenuIndex(0))
        End If
        Localization.Load(currentLanguage)
        Core.GameOptions.SaveOptions()
        SoundManager.PlaySound("save")
        _closing = True
    End Sub
#End Region
#End Region

#Region "Controls"

    MustInherit Class Control
        Public Property ControlType As String
        Public MustOverride Sub Draw()
        Public MustOverride Sub Update(ByRef s As NewOptionScreen)
        Public _position As Vector2 = New Vector2(0)
        Public _size As Integer = 1
        Public Property ID As Integer

        Public Property Size As Integer
            Get
                Return Me._size
            End Get
            Set(value As Integer)
                Me._size = value
            End Set
        End Property

        Sub New()

        End Sub
    End Class

    Class ToggleButton

        Inherits Control

        Private _buttonWidth As Integer = 1
        Private _text As String = ""
        Private _toggled As Boolean = False

        Public Property Position As Vector2
            Get
                Return _position
            End Get
            Set(value As Vector2)
                Me._position = value
            End Set
        End Property

        Public Property ButtonWidth As Integer
            Get
                Return Me._buttonWidth
            End Get
            Set(value As Integer)
                Me._buttonWidth = value
            End Set
        End Property

        Public Property Text As String
            Get
                Return Me._text
            End Get
            Set(value As String)
                Me._text = value
            End Set
        End Property

        Public Property Toggled As Boolean
            Get
                Return Me._toggled
            End Get
            Set(value As Boolean)
                Me._toggled = value
            End Set
        End Property

        Public Delegate Sub OnToggle(ByVal T As ToggleButton)
        Public OnToggleTrigger As OnToggle

        Public Settings As New List(Of String)

        Public Sub New(ByVal TriggerSub As OnToggle)
            MyBase.New
            Me.OnToggleTrigger = TriggerSub
        End Sub

        Public Sub New(ByVal Position As Vector2, ByVal ButtonWidth As Integer, ByVal Size As Integer, ByVal Text As String, ByVal Toggled As Boolean, ByVal TriggerSub As OnToggle, ID As Integer)
            Me.New(Position, Size, ButtonWidth, Text, Toggled, TriggerSub, New List(Of String), ID)
        End Sub

        Public Sub New(ByVal Position As Vector2, ByVal ButtonWidth As Integer, ByVal Size As Integer, ByVal Text As String, ByVal Toggled As Boolean, ByVal TriggerSub As OnToggle, ByVal Settings As List(Of String), ID As Integer)
            MyBase.New
            Me._position = Position
            Me._buttonWidth = ButtonWidth
            Me.ControlType = "ToggleButton"
            Me.Size = Size
            Me._text = Text
            Me._toggled = Toggled
            Me.ID = ID
            Me.OnToggleTrigger = TriggerSub
            Me.Settings = Settings
        End Sub


        Public Overrides Sub Draw()
            Dim s As NewOptionScreen = CType(CurrentScreen, NewOptionScreen)
            Dim pos As Vector2 = Me.Position
            Dim c As Color = New Color(255, 255, 255, CInt(255 * s._interfaceFade * s._pageFade))

            Dim size As Integer = Me.Size
            Dim ToggleDivider As String = ": "
            If Me.Text = "" Then
                ToggleDivider = ""
            End If
            Dim B As New Vector2
            Dim t As String = Me.Text
            Dim textColor As New Color
            If Toggled Then
                t &= ToggleDivider & Settings(1)
                B.X = 16
                B.Y = 32
                textColor = (New Color(255, 255, 255, CInt(255 * s._interfaceFade * s._pageFade)))
            Else
                t &= ToggleDivider & Settings(0)
                B.X = 16
                B.Y = 16
                textColor = (New Color(0, 0, 0, CInt(255 * s._interfaceFade * s._pageFade)))
            End If

            Core.SpriteBatch.Draw(s._menuTexture, New Rectangle(CInt(pos.X), CInt(pos.Y), size, size), New Rectangle(CInt(B.X), CInt(B.Y), 16, 16), c)
            Core.SpriteBatch.Draw(s._menuTexture, New Rectangle(CInt(pos.X) + size, CInt(pos.Y), size * ButtonWidth, size), New Rectangle(CInt(B.X) + 16, CInt(B.Y), 16, 16), c)
            Core.SpriteBatch.Draw(s._menuTexture, New Rectangle(CInt(pos.X) + size * (ButtonWidth + 1), CInt(pos.Y), size, size), New Rectangle(CInt(B.X), CInt(B.Y), 16, 16), c, 0.0F, Vector2.Zero, SpriteEffects.FlipHorizontally, 0.0F)

            Dim fontWidth As Integer = CInt(FontManager.MainFont.MeasureString(t).X * 1.0)
            Core.SpriteBatch.DrawString(FontManager.MainFont, t, New Vector2(CInt((pos.X + (size * (2 + ButtonWidth) - fontWidth) * 0.5F)), CInt(pos.Y) + CInt(16 * size / 64)), textColor, 0.0F, Vector2.Zero, 1.0F, SpriteEffects.None, 0.0F)
        End Sub

        Public Overrides Sub Update(ByRef s As NewOptionScreen)
            If ScreenIndex = 7 And Me.ID = 3 Then
                If isSelectedEnabled = False Then
                    Me.Toggled = False
                Else
                    Me.Toggled = True
                End If
            ElseIf ScreenIndex = 8 Then
                If isSelectedEnabled = False Then
                    Me.Toggled = False
                Else
                    Me.Toggled = True
                End If
            End If

            Dim r As New Rectangle(CInt(_position.X), CInt(_position.Y), (2 + ButtonWidth) * Size, Size)

            If r.Contains(MouseHandler.MousePosition) = True Then
                If P3D.Controls.Accept(True, False, False) = True Then
                    Me._toggled = Not Me._toggled
                    OnToggleTrigger(Me)
                    SoundManager.PlaySound("select")
                End If
            End If

            If Controls.Accept(False, True, True) Then
                If Position = s._cursorDestPosition Then
                    Me._toggled = Not Me._toggled
                    OnToggleTrigger(Me)
                    SoundManager.PlaySound("select")
                End If
            End If
        End Sub
    End Class

    Class CommandButton

        Inherits Control
        Private _buttonWidth As Integer = 1
        Private _text As String = ""
        Private TextureY As Integer

        Public Property Position As Vector2
            Get
                Return _position
            End Get
            Set(value As Vector2)
                Me._position = value
            End Set
        End Property

        Public Property ButtonWidth As Integer
            Get
                Return Me._buttonWidth
            End Get
            Set(value As Integer)
                Me._buttonWidth = value
            End Set
        End Property

        Public Property Text As String
            Get
                Return Me._text
            End Get
            Set(value As String)
                Me._text = value
            End Set
        End Property

        Public Delegate Sub OnClick(ByVal C As CommandButton)
        Public OnClickTrigger As OnClick

        Public Sub New(ByVal ClickSub As OnClick)
            MyBase.New
            Me.OnClickTrigger = ClickSub
        End Sub

        Public Sub New(ByVal Position As Vector2, ByVal ButtonWidth As Integer, ByVal Size As Integer, ByVal Text As String, ByVal ClickSub As OnClick, ByVal ID As Integer)
            MyBase.New
            Me._position = Position
            Me._buttonWidth = ButtonWidth
            Me.ControlType = "CommandButton"
            Me.Size = Size
            Me._text = Text
            Me.ID = ID
            Me.OnClickTrigger = ClickSub
            TextureY = 16
        End Sub

        Public Overrides Sub Draw()
            Dim s As NewOptionScreen = CType(CurrentScreen, NewOptionScreen)

            Dim pos As Vector2 = Me.Position
            Dim c As Color = New Color(255, 255, 255, CInt(255 * s._interfaceFade * s._pageFade))

            Core.SpriteBatch.Draw(s._menuTexture, New Rectangle(CInt(pos.X), CInt(pos.Y), Size, Size), New Rectangle(16, TextureY, 16, 16), c)
            Core.SpriteBatch.Draw(s._menuTexture, New Rectangle(CInt(pos.X) + Size, CInt(pos.Y), Size * ButtonWidth, Size), New Rectangle(32, TextureY, 16, 16), c)
            Core.SpriteBatch.Draw(s._menuTexture, New Rectangle(CInt(pos.X) + Size * (ButtonWidth + 1), CInt(pos.Y), Size, Size), New Rectangle(16, TextureY, 16, 16), c, 0.0F, Vector2.Zero, SpriteEffects.FlipHorizontally, 0.0F)

            Dim fontWidth As Integer = CInt(FontManager.MainFont.MeasureString(Text).X * 1.0)
            Core.SpriteBatch.DrawString(FontManager.MainFont, Text, New Vector2(CInt((pos.X + (Size * (2 + ButtonWidth) - fontWidth) * 0.5F)), CInt(pos.Y) + CInt(16 * Size / 64)), New Color(0, 0, 0, CInt(255 * s._interfaceFade * s._pageFade)), 0.0F, Vector2.Zero, 1.0F, SpriteEffects.None, 0.0F)
        End Sub

        Public Overrides Sub Update(ByRef s As NewOptionScreen)
            Dim r As New Rectangle(CInt(_position.X), CInt(_position.Y), (2 + ButtonWidth) * Size, Size)
            Dim Click As Boolean = False
            If s._pageClosing = False And s._pageOpening = False Then
                If r.Contains(MouseHandler.MousePosition) = True Then
                    If P3D.Controls.Accept(True, False, False) = True Then
                        SoundManager.PlaySound("select")
                        Click = True
                        OnClickTrigger(Me)
                    End If
                End If
                If Click = True Then
                    TextureY = 32
                End If
                If MouseHandler.ButtonUp(MouseHandler.MouseButtons.LeftButton) Then
                    TextureY = 16
                    Click = False
                End If

                If Controls.Accept(False, True, True) = True Then
                    If Position = s._cursorDestPosition Then
                        SoundManager.PlaySound("select")
                        OnClickTrigger(Me)
                    End If
                End If
                If KeyBoardHandler.KeyDown(KeyBindings.EnterKey1) = True Or KeyBoardHandler.KeyDown(KeyBindings.EnterKey2) = True Or ControllerHandler.ButtonDown(Buttons.A) = True Then
                    If Position = s._cursorDestPosition Then
                        TextureY = 32
                    Else
                        TextureY = 16
                    End If
                End If
            Else
                Click = False
            End If
        End Sub
    End Class

    Class ScrollBar

        Inherits Control

        Private _value As Integer = 0
        Private _max As Integer = 0
        Private _min As Integer = 0
        Private _text As String = ""
        Private _drawPercentage As Boolean = False

        Public Property Position As Vector2
            Get
                Return _position
            End Get
            Set(value As Vector2)
                Me._position = value
            End Set
        End Property

        Public Property Value As Integer
            Get
                Return Me._value
            End Get
            Set(value As Integer)
                Me._value = value
            End Set
        End Property

        Public Property Max As Integer
            Get
                Return Me._max
            End Get
            Set(value As Integer)
                Me._max = value
            End Set
        End Property

        Public Property Min As Integer
            Get
                Return Me._min
            End Get
            Set(value As Integer)
                Me._min = value
            End Set
        End Property

        Public Property Text As String
            Get
                Return Me._text
            End Get
            Set(value As String)
                Me._text = value
            End Set
        End Property

        Public Property DrawPercentage As Boolean
            Get
                Return Me._drawPercentage
            End Get
            Set(value As Boolean)
                Me._drawPercentage = value
            End Set
        End Property

        Public Delegate Sub OnChange(ByVal S As ScrollBar)
        Public OnChangeTrigger As OnChange

        Public Settings As New Dictionary(Of Integer, String)

        Dim Selected As Boolean = False
        Dim Clicked As Boolean = False

        Public Sub New(ByVal ChangeSub As OnChange)
            MyBase.New
            Me.OnChangeTrigger = ChangeSub
        End Sub

        Public Sub New(ByVal Position As Vector2, ByVal Size As Integer, ByVal Text As String, ByVal Value As Integer, ByVal Min As Integer, ByVal Max As Integer, ByVal ChangeSub As OnChange, ID As Integer)
            Me.New(Position, Size, Text, Value, Min, Max, ChangeSub, New Dictionary(Of Integer, String), ID)
        End Sub

        Public Sub New(ByVal Position As Vector2, ByVal Size As Integer, ByVal Text As String, ByVal Value As Integer, ByVal Min As Integer, ByVal Max As Integer, ByVal ChangeSub As OnChange, ByVal Settings As Dictionary(Of Integer, String), ID As Integer)
            MyBase.New
            Me._position = Position
            Me.Size = Size
            Me._text = Text
            Me._value = Value
            Me._max = Max
            Me._min = Min
            Me.ControlType = "ScrollBar"
            Me.Settings = Settings
            Me.OnChangeTrigger = ChangeSub
            Me.ID = ID
        End Sub

        Public Overrides Sub Draw()
            Dim length As Integer = Size + 16
            Dim height As Integer = 36

            Dim s As NewOptionScreen = CType(CurrentScreen, NewOptionScreen)
            Dim pos As Vector2 = Me.Position
            Dim c As Color = New Color(255, 255, 255, CInt(255 * s._interfaceFade * s._pageFade))

            Dim BarRectangle1 As Rectangle
            Dim BarRectangle2 As Rectangle
            Dim SliderRectangle As Rectangle
            Dim TextColor As Color
            If Selected OrElse Clicked Then
                BarRectangle1 = New Rectangle(0, 60, 12, 12)
                BarRectangle2 = New Rectangle(12, 60, 12, 12)
                SliderRectangle = New Rectangle(6, 32, 6, 12)
                TextColor = New Color(25, 67, 91, CInt(255 * s._interfaceFade * s._pageFade))
            Else
                BarRectangle1 = New Rectangle(0, 48, 12, 12)
                BarRectangle2 = New Rectangle(12, 48, 12, 12)
                SliderRectangle = New Rectangle(0, 32, 6, 12)
                TextColor = New Color(0, 0, 0, CInt(255 * s._interfaceFade * s._pageFade))
            End If

            Core.SpriteBatch.Draw(s._menuTexture, New Rectangle(CInt(pos.X), CInt(pos.Y), height, height), BarRectangle1, c)
            Core.SpriteBatch.Draw(s._menuTexture, New Rectangle(CInt(pos.X) + 36, CInt(pos.Y), length - 72, height), BarRectangle2, c)
            Core.SpriteBatch.Draw(s._menuTexture, New Rectangle(CInt(pos.X) + length - 36, CInt(pos.Y), height, height), BarRectangle1, c, 0.0F, Vector2.Zero, SpriteEffects.FlipHorizontally, 0.0F)

            Core.SpriteBatch.Draw(s._menuTexture, GetSliderBox, SliderRectangle, c)


            Dim t As String = Text & ": "

            If Settings.ContainsKey(Value) = True Then
                t &= Settings(Value)
            Else
                If Me._drawPercentage = True Then
                    t &= CStr(Me._value / (Me._max - Me._min) * 100)
                Else
                    t &= Me._value.ToString()
                End If
            End If
            Core.SpriteBatch.DrawString(FontManager.MainFont, t, New Vector2(Me.Position.X + CSng((400 / 2) - (FontManager.MainFont.MeasureString(t).X / 2)), Me._position.Y + 6 - 32), TextColor)
        End Sub

        Public Overrides Sub Update(ByRef s As NewOptionScreen)
            If MouseHandler.ButtonDown(MouseHandler.MouseButtons.LeftButton) Then
                If GetSliderBox().Contains(MouseHandler.MousePosition.X, MouseHandler.MousePosition.Y) And Clicked = False Then
                    Clicked = True
                    Selected = False
                    s._selectedScrollBar = False
                End If
                If Clicked = True Then
                    Dim x As Double = MouseHandler.MousePosition.X - Me._position.X
                    If x < 0 Then
                        x = 0D
                    End If
                    If x > Me.Size + 16 Then
                        x = Me.Size + 16
                    End If

                    Me.Value = CInt(x * ((Me._max - Min) / 100) * (100 / Me._size)) + Min
                    Me.Value = Value.Clamp(Min, Max)

                    OnChangeTrigger(Me)
                End If
            Else
                Clicked = False
                If Selected Then
                    If Controls.Dismiss(False, True, True) OrElse Controls.Accept(False, True, True) Then
                        Selected = False
                        s._selectedScrollBar = False
                    ElseIf Controls.Left(True) Then
                        Me.Value = Me.Value - 1
                        Me.Value = Value.Clamp(Min, Max)
                        OnChangeTrigger(Me)
                    ElseIf Controls.Right(True) Then
                        Me.Value = Me.Value + 1
                        Me.Value = Value.Clamp(Min, Max)
                        OnChangeTrigger(Me)
                    End If
                Else
                    If Controls.Accept(False, True, True) Then
                        If s._cursorDestPosition.Y = Me.Position.Y Then
                            Selected = True
                            s._selectedScrollBar = True
                        End If
                    End If
                End If
            End If
        End Sub

        Private Function GetSliderBox() As Rectangle
            Dim x As Integer = CInt(((100 / (Me._max - Min)) * (Me._value - Min)) * (Size / 100))

            If Me._value = Min Then
                x = 0
            Else
                If x = 0 And _value > 0 Then
                    x = 1
                End If
            End If

            Return New Rectangle(x + CInt(Me.Position.X), CInt(Me.Position.Y), 18, 36)
        End Function

    End Class

#End Region



End Class