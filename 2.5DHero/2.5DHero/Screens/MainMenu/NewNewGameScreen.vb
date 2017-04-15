Namespace Screens.MainMenu

    ''' <summary>
    ''' The screen that is used to act out a New Game sequence.
    ''' </summary>
    Public Class NewNewGameScreen

        Inherits Screen

        Private Shared _fadeValue As Integer = 255 'Fade progress value for the black screen fade.

        Public Shared Property FadeValue() As Integer
            Get
                Return _fadeValue
            End Get
            Set(value As Integer)
                _fadeValue = value
            End Set
        End Property

        Public Sub New(ByVal currentScreen As Screen)
            Identification = Identifications.NewGameScreen
            CanChat = False
            MouseVisible = False
            IsOverlay = True
            PreScreen = currentScreen
            CanBePaused = False

            'Set up 3D environment variables (Effect, Camera, SkyDome and Level):
            Effect = New BasicEffect(GraphicsDevice)
            Effect.FogEnabled = True

            'Reset Construct:
            Construct.Controller.GetInstance().Reset()
            Construct.Controller.GetInstance().Context = Construct.ScriptContext.NewGame

            Camera = New NewGameCamera()

            SkyDome = New SkyDome()
            Level = New Level()
            Level.Load(GameModeManager.ActiveGameMode.StartupMap)

            Core.Player.Unload()

            MusicPlayer.GetInstance().Stop()

            'Initialize the World information with the loaded level.
            Level.World.Initialize(Level.EnvironmentType, Level.WeatherType)

            Construct.Controller.GetInstance().RunFromFile(GameModeManager.ActiveGameMode.StartupScript)
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

            'If no dialogue is showing, do level update tasks:
            If TextBox.Showing = False And ChooseBox.Showing = False And PokemonImageView.Showing = False Then
                If Construct.Controller.GetInstance().IsReady Then
                    Camera.Update()
                    Level.Update()
                End If

                Construct.Controller.GetInstance().Update() 'Update construct scripts.
            Else
                Camera.Update()
                Level.Update()
            End If

            SkyDome.Update()

            'Update the World with new environment variables.
            Level.World.Initialize(Level.EnvironmentType, Level.WeatherType)

        End Sub

        Public Overrides Sub Draw()
            SkyDome.Draw(Camera.FOV)

            Level.Draw()

            PokemonImageView.Draw()
            TextBox.Draw()

            'Only draw the ChooseBox when it's the current screen, cause the same ChooseBox might get used on other screens.
            If IsCurrentScreen() = True Then
                ChooseBox.Draw()
            End If

            If FadeValue > 0 Then
                Canvas.DrawRectangle(windowSize, New Color(0, 0, 0, FadeValue))
            End If
        End Sub

        Public Shared Sub EndNewGame(ByVal map As String, ByVal x As Single, ByVal y As Single, ByVal z As Single, ByVal rot As Integer)
            Dim folderPath As String = Core.Player.Name
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

            IO.Directory.CreateDirectory(savePath & folderPath)

            Core.Player.filePrefix = folderPath
            Core.Player.GameStart = Date.Now

            Core.Player.SaveGame()
            Core.Player.LoadGame(folderPath)

            Core.Player.startFOV = 60
            Core.Player.startFreeCameraMode = True
            Core.Player.startPosition = New Vector3(x, y, z)
            Core.Player.startMap = map
            Core.Player.startRotationSpeed = 12
            Core.Player.startSurfing = False
            Core.Player.startThirdPerson = False
            Core.Player.startRiding = False
            Core.Player.startRotation = CSng(MathHelper.Pi * (rot / 2))

            Construct.Controller.GetInstance().Context = Construct.ScriptContext.Overworld

            SetScreen(New TransitionScreen(CurrentScreen, New OverworldScreen(), Color.Black, False))
            Core.Player.SaveGame()
        End Sub

        ''' <summary>
        ''' A screen used to select from a range of skins.
        ''' </summary>
        Public Class CharacterSelectionScreen

            Inherits Screen

            Private _skins As String()
            Private _sprites As New List(Of Texture2D)

            Private _offset As Single = 0F
            Private _index As Integer = 0

            Private _fadeIn As Single = 0F

            Private Shared _selectedSkin As String = ""

            Public Shared ReadOnly Property SelectedSkin() As String
                Get
                    Return _selectedSkin
                End Get
            End Property

            Public Sub New(ByVal currentScreen As Screen, ByVal skins As String())
                Identification = Identifications.CharacterSelectionScreen
                PreScreen = currentScreen
                CanBePaused = True
                CanChat = False
                CanDrawDebug = True
                CanGoFullscreen = True
                CanTakeScreenshot = True
                MouseVisible = True

                For Each skin As String In skins
                    _sprites.Add(TextureManager.GetTexture("Textures\NPC\" & skin))
                Next

                _skins = skins
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
                        _selectedSkin = _skins(_index)
                        SetScreen(PreScreen)
                    End If
                    If Controls.Accept(True, False, False) Then
                        For i = 0 To _skins.Count - 1
                            If New Rectangle(CInt(windowSize.Width / 2 - 128 + i * 280 - _index * 280 + _offset), CInt(windowSize.Height / 2 - 128), 256, 256).Contains(MouseHandler.MousePosition) Then
                                If i = _index Then
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

                Canvas.DrawRectangle(windowSize, Screens.UI.ColorProvider.MainColor(False, CInt(100 * _fadeIn)))

                DrawGradients(255)

                SpriteBatch.DrawString(FontManager.MainFont, "Select your appearance", New Vector2(windowSize.Width / 2.0F - FontManager.MainFont.MeasureString("Select your appearance").X, 100), New Color(255, 255, 255, CInt(255 * _fadeIn)), 0F, Vector2.Zero, 2.0F, SpriteEffects.None, 0F)

                For i = 0 To _sprites.Count - 1
                    Dim sprite As Texture2D = _sprites(i)
                    Dim frameSize As New Size(CInt(sprite.Width / 3), CInt(sprite.Height / 4))

                    Dim outSize As Integer = 256 - Math.Abs(_index - i) * 30

                    SpriteBatch.Draw(sprite, New Rectangle(CInt(windowSize.Width / 2 - CInt(outSize / 2) + i * 280 - _index * 280 + _offset), CInt(windowSize.Height / 2 - 128), outSize, outSize), New Rectangle(0, frameSize.Height * 2, frameSize.Width, frameSize.Height), New Color(255, 255, 255, CInt(255 * _fadeIn)))
                Next

                SpriteBatch.DrawString(FontManager.MainFont, _skins(_index), New Vector2(windowSize.Width / 2.0F - FontManager.MainFont.MeasureString(_skins(_index)).X / 2.0F, windowSize.Height / 2.0F + 200), New Color(255, 255, 255, CInt(255 * _fadeIn)))
            End Sub

        End Class

    End Class

End Namespace