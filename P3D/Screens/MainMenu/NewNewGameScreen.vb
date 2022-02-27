Namespace Screens.MainMenu

    ''' <summary>
    ''' The screen that is used to act out a New Game sequence.
    ''' </summary>
    Public Class NewNewGameScreen

        Inherits OverworldScreen

        Private Shared _fadeValue As Integer = 255 'Fade progress value for the black screen fade.

        Public Sub New(ByVal currentScreen As Screen)
            Identification = Identifications.NewGameScreen
            CanChat = False
            MouseVisible = False
            IsOverlay = True
            PreScreen = currentScreen
            CanBePaused = False
            UpdateFadeOut = True
            BattleSystem.GameModeAttackLoader.Load()

            'Set up 3D environment variables (Effect, Camera, SkyDome and Level):
            Effect = New BasicEffect(GraphicsDevice)
            Effect.FogEnabled = True

            'Reset Construct:
            ScriptStorage.Clear()
            Me.ActionScript.Scripts.Clear()


            Camera = New NewGameCamera()

            SkyDome = New SkyDome()
            Level = New Level()
            Level.Load(GameModeManager.ActiveGameMode.StartMap)

            Core.Player.Unload()

            MusicManager.PlayNoMusic()

            'Initialize the World information with the loaded level.
            Level.World.Initialize(Level.EnvironmentType, Level.WeatherType)

            Me.ActionScript.StartScript("newgame\intro", 0)
        End Sub

        Public Overrides Sub Update()
            Lighting.UpdateLighting(Effect) 'Update the lighting on the basic effect.

            'Update the Dialogues:
            ChooseBox.Update()
            If ChooseBox.Showing = False Then
                TextBox.Update()
            End If
            If PokemonImageView.Showing = True Then
                PokemonImageView.Update()
            End If
            If ImageView.Showing = True Then
                ImageView.Update()
            End If

            'If no dialogue is showing, do level update tasks:
            If TextBox.Showing = False And ChooseBox.Showing = False And PokemonImageView.Showing = False And ImageView.Showing = False Then
                If Me.ActionScript.IsReady Then
                    Camera.Update()
                    Level.Update()
                End If
                If ActionScript.Scripts.Count = 0 Then
                    If CurrentScreen.Identification = Screen.Identifications.NewGameScreen Then
                        If OverworldScreen.FadeValue > 0 Then
                            Core.SetScreen(New TransitionScreen(CurrentScreen, New OverworldScreen(), Color.Black, False, AddressOf RemoveFade))
                        Else
                            Core.SetScreen(New OverworldScreen())
                        End If
                    Else
                        'Dim fadeSpeed As Integer = 12
                        'If OverworldScreen.FadeValue > 0 Then
                        '    OverworldScreen.FadeValue -= fadeSpeed
                        '    If OverworldScreen.FadeValue <= 0 Then
                        '        OverworldScreen.FadeValue = 0
                        '    End If
                        'End If
                    End If
                Else
                    Me.ActionScript.Update() 'Update construct scripts.
                End If
            Else
                Camera.Update()
                Level.Update()
            End If

            SkyDome.Update()

            'Update the World with new environment variables.
            Level.World.Initialize(Level.EnvironmentType, Level.WeatherType)

        End Sub
        Public Sub RemoveFade()
            FadeValue = 0
        End Sub

        Public Overrides Sub Draw()
            SkyDome.Draw(Camera.FOV)

            Level.Draw()

            PokemonImageView.Draw()
            ImageView.Draw()
            TextBox.Draw()

            'Only draw the ChooseBox when it's the current screen, cause the same ChooseBox might get used on other screens.
            If IsCurrentScreen() = True Then
                ChooseBox.Draw()
            End If

            If FadeValue > 0 Then
                Canvas.DrawRectangle(windowSize, New Color(FadeColor.R, FadeColor.G, FadeColor.B, FadeValue))
            End If
        End Sub

        Public Shared Sub EndNewGame(ByVal map As String, ByVal x As Single, ByVal y As Single, ByVal z As Single, ByVal rot As Integer)
            Dim folderPath As String = Core.Player.Name.Replace("\", "_").Replace("/", "_").Replace(":", "_").Replace("*", "_").Replace("?", "_").Replace("""", "_").Replace("<", "_").Replace(">", "_").Replace("|", "_").Replace(",", "_").Replace(".", "_")
            Dim folderPrefix As Integer = 0

            If folderPath.ToLower() = "autosave" Then
                folderPath = "autosave0"
            End If

            Dim savePath As String = GameController.GamePath & "\Save\"

            While IO.Directory.Exists(savePath & folderPath) = True
                If folderPath <> Core.Player.Name Then
                    folderPath = folderPath.Remove(folderPath.Length - folderPrefix.ToString().Length, folderPrefix.ToString().Length)
                End If

                folderPath &= folderPrefix

                folderPrefix += 1
            End While

            If IO.Directory.Exists(GameController.GamePath & "\Save") = False Then
                IO.Directory.CreateDirectory(GameController.GamePath & "\Save")
            End If

            'IO.Directory.CreateDirectory(savePath & folderPath)

            Core.Player.filePrefix = folderPath
            Core.Player.GameStart = Date.Now

            Core.Player.startFOV = 60
            Core.Player.startFreeCameraMode = True
            Core.Player.startPosition = New Vector3(x, y, z)
            Core.Player.startMap = map
            Core.Player.startRotationSpeed = 12
            Core.Player.startSurfing = False
            Core.Player.startThirdPerson = False
            Core.Player.startRiding = False
            Core.Player.startRotation = CSng(MathHelper.Pi * (rot / 2))

            Core.Player.BerryData = CreateBerryData()
            Core.Player.AddVisitedMap("yourroom.dat")
            Core.Player.SaveCreated = GameController.GAMEDEVELOPMENTSTAGE & " " & GameController.GAMEVERSION

            Dim ot As String = Core.Random.Next(0, 999999).ToString()
            While ot.Length < 6
                ot = "0" & ot
            End While
            Core.Player.OT = ot
        End Sub

        Private Shared Function CreateBerryData() As String
            Dim s As String = "{route29.dat|13,0,5|6|2|0|2012,9,21,4,0,0|1}" & Environment.NewLine &
            "{route29.dat|14,0,5|6|2|0|2012,9,21,4,0,0|1}" & Environment.NewLine &
            "{route29.dat|15,0,5|6|2|0|2012,9,21,4,0,0|1}" & Environment.NewLine &
            "{azalea.dat|9,0,3|0|2|0|2012,9,21,4,0,0|1}" & Environment.NewLine &
            "{azalea.dat|9,0,4|1|1|0|2012,9,21,4,0,0|1}" & Environment.NewLine &
            "{azalea.dat|9,0,5|0|2|0|2012,9,21,4,0,0|1}" & Environment.NewLine &
            "{route30.dat|7,0,41|10|1|0|2012,9,21,4,0,0|1}" & Environment.NewLine &
            "{route30.dat|14,0,5|2|1|0|2012,9,21,4,0,0|1}" & Environment.NewLine &
            "{route30.dat|15,0,5|6|2|0|2012,9,21,4,0,0|1}" & Environment.NewLine &
            "{route30.dat|16,0,5|2|1|0|2012,9,21,4,0,0|1}" & Environment.NewLine &
            "{routes\route35.dat|0,0,4|7|2|0|2012,9,21,4,0,0|1}" & Environment.NewLine &
            "{routes\route35.dat|1,0,4|8|2|0|2012,9,21,4,0,0|1}" & Environment.NewLine &
            "{route36.dat|37,0,7|0|1|0|2012,9,21,4,0,0|1}" & Environment.NewLine &
            "{route36.dat|38,0,7|4|1|0|2012,9,21,4,0,0|1}" & Environment.NewLine &
            "{route36.dat|39,0,7|3|2|0|2012,9,21,4,0,0|1}" & Environment.NewLine &
            "{route39.dat|8,0,2|9|1|0|2012,9,21,4,0,0|1}" & Environment.NewLine &
            "{route39.dat|8,0,3|6|1|0|2012,9,21,4,0,0|1}" & Environment.NewLine &
            "{route38.dat|13,0,12|16|2|0|2012,9,21,4,0,0|1}" & Environment.NewLine &
            "{route38.dat|14,0,12|23|1|0|2012,9,21,4,0,0|1}" & Environment.NewLine &
            "{route38.dat|15,0,12|16|2|0|2012,9,21,4,0,0|1}" & Environment.NewLine &
            "{routes\route43.dat|13,0,45|23|2|0|2012,9,21,4,0,0|1}" & Environment.NewLine &
            "{routes\route43.dat|13,0,46|24|2|0|2012,9,21,4,0,0|1}" & Environment.NewLine &
            "{routes\route43.dat|13,0,47|25|2|0|2012,9,21,4,0,0|1}" & Environment.NewLine &
            "{safarizone\main.dat|3,0,11|5|2|0|2012,9,21,4,0,0|1}" & Environment.NewLine &
            "{safarizone\main.dat|4,0,11|0|2|0|2012,9,21,4,0,0|1}" & Environment.NewLine &
            "{safarizone\main.dat|5,0,11|6|3|0|2012,9,21,4,0,0|1}"

            Return s
        End Function



        ''' <summary>
        ''' A screen used to select from a range of skins.
        ''' </summary>
        Public Class CharacterSelectionScreen

            Inherits Screen

            Private _skins As New List(Of String)
            Private _names As New List(Of String)
            Private _colors As New List(Of Color)
            Private _genders As New List(Of String)
            Private _sprites As New List(Of Texture2D)

            Private _offset As Single = 0F
            Private _index As Integer = 0

            Private _fadeIn As Single = 0F

            Private Shared _selectedSkin As String = ""

            Public Shared Property SelectedSkin() As String
                Get
                    Return _selectedSkin
                End Get
                Set(value As String)
                    _selectedSkin = value
                End Set
            End Property

            Public Sub New(ByVal currentScreen As Screen)
                Identification = Identifications.CharacterSelectionScreen
                PreScreen = currentScreen
                CanBePaused = True
                CanChat = False
                CanDrawDebug = True
                CanGoFullscreen = True
                CanTakeScreenshot = True
                MouseVisible = True
                SelectedSkin = ""

                For Each skin As String In GameModeManager.ActiveGameMode.SkinFiles
                    _sprites.Add(TextureManager.GetTexture("Textures\NPC\" & skin))
                Next

                _skins = GameModeManager.ActiveGameMode.SkinFiles
                _names = GameModeManager.ActiveGameMode.SkinNames
                _genders = GameModeManager.ActiveGameMode.SkinGenders
                _colors = GameModeManager.ActiveGameMode.SkinColors
            End Sub

            Public Overrides Sub Update()
                If _fadeIn < 1.0F Then
                    _fadeIn = MathHelper.Lerp(1.0F, _fadeIn, 0.95F)
                    If _fadeIn + 0.01F >= 1.0F Then
                        _fadeIn = 1.0F
                    End If
                Else
                    If Controls.Left(True) And _index > 0 Then
                        _index -= 1
                        _offset -= 280
                    End If
                    If Controls.Right(True) And _index < _skins.Count - 1 Then
                        _index += 1
                        _offset += 280
                    End If

                    If _offset > 0F Then
                        _offset = MathHelper.Lerp(0F, _offset, 0.9F)
                        If _offset - 0.01F <= 0F Then
                            _offset = 0F
                        End If
                    ElseIf _offset < 0F Then
                        _offset = MathHelper.Lerp(0F, _offset, 0.9F)
                        If _offset + 0.01F >= 0F Then
                            _offset = 0F
                        End If
                    End If

                    If Controls.Accept(False, True, True) Then
                        SoundManager.PlaySound("select")
                        _selectedSkin = _skins(_index)
                        SetScreen(PreScreen)
                    End If
                    If Controls.Accept(True, False, False) Then
                        For i = 0 To _skins.Count - 1
                            If New Rectangle(CInt(windowSize.Width / 2 - 128 + i * 280 - _index * 280 + _offset), CInt(windowSize.Height / 2 - 128), 256, 256).Contains(MouseHandler.MousePosition) Then
                                If i = _index Then
                                    SoundManager.PlaySound("select")
                                    _selectedSkin = _skins(_index)
                                    SetScreen(PreScreen)
                                Else
                                    _offset += (i - _index) * 280
                                    _index = i
                                End If
                                Exit For
                            End If
                        Next
                    End If
                End If
            End Sub

            Public Overrides Sub Draw()
                PreScreen.Draw()
                Dim backcolor As New Color(_colors(_index), CInt(100 * _fadeIn))

                Canvas.DrawRectangle(windowSize, backcolor)

                SpriteBatch.DrawString(FontManager.MainFont, Localization.GetString("new_game_select_skin"), New Vector2(windowSize.Width / 2.0F - FontManager.MainFont.MeasureString(Localization.GetString("new_game_select_skin")).X + 2, 100 + 2), New Color(0, 0, 0, CInt(255 * _fadeIn)), 0F, Vector2.Zero, 2.0F, SpriteEffects.None, 0F)
                SpriteBatch.DrawString(FontManager.MainFont, Localization.GetString("new_game_select_skin"), New Vector2(windowSize.Width / 2.0F - FontManager.MainFont.MeasureString(Localization.GetString("new_game_select_skin")).X, 100), New Color(255, 255, 255, CInt(255 * _fadeIn)), 0F, Vector2.Zero, 2.0F, SpriteEffects.None, 0F)

                For i = 0 To _sprites.Count - 1
                    Dim sprite As Texture2D = _sprites(i)
                    Dim frameSize As New Size(CInt(sprite.Width / 3), CInt(sprite.Height / 4))

                    Dim outSize As Integer = 256 - Math.Abs(_index - i) * 30

                    SpriteBatch.Draw(sprite, New Rectangle(CInt(windowSize.Width / 2 - CInt(outSize / 2) + i * 280 - _index * 280 + _offset), CInt(windowSize.Height / 2 - 128), outSize, outSize), New Rectangle(0, frameSize.Height * 2, frameSize.Width, frameSize.Height), New Color(255, 255, 255, CInt(255 * _fadeIn)))
                Next

                SpriteBatch.DrawString(FontManager.MainFont, _names(_index), New Vector2(CInt(windowSize.Width / 2.0F - FontManager.MainFont.MeasureString(_names(_index)).X), CInt(windowSize.Height / 2.0F + 200)), New Color(255, 255, 255, CInt(255 * _fadeIn)), 0F, Vector2.Zero, 2.0F, SpriteEffects.None, 0F)
                SpriteBatch.DrawString(FontManager.MainFont, Localization.GetString("global_" & _genders(_index).ToLower(), _genders(_index)), New Vector2(CInt(windowSize.Width / 2.0F - FontManager.MainFont.MeasureString(Localization.GetString("global_" & _genders(_index).ToLower(), _genders(_index))).X / 2.0F), CInt(windowSize.Height / 2.0F + 300)), New Color(255, 255, 255, CInt(255 * _fadeIn)), 0F, Vector2.Zero, 1.0F, SpriteEffects.None, 0F)
            End Sub

        End Class

    End Class

End Namespace