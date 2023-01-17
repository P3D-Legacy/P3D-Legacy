Imports P3D.Screens.UI
Public Class PartyScreen

    Inherits Screen
    Implements ISelectionScreen

    Dim POKEMON_TITLE As String = "Pokémon"

    'Private _translation As Globalization.Classes.LOCAL_PartyScreen

    ''' <summary>
    ''' Cursor index -> pointing to Pokémon (0-5).
    ''' </summary>
    Public _index As Integer = 0

    Private _texture As Texture2D
    Private _menuTexture As Texture2D

    'Animation:
    Private _closing As Boolean = False

    Private _enrollY As Single = 0F
    Private _interfaceFade As Single = 0F
    Private _cursorPosition As New Vector2
    Private _cursorDest As New Vector2

    'Pokémon animation:
    Private Class PokemonAnimation
        Public _shakeV As Single
        Public _shakeLeft As Boolean
        Public _shakeCount As Integer
    End Class

    Private _pokemonAnimations As New List(Of PokemonAnimation)

    Private _menu As UI.SelectMenu

    Private _isSwitching As Boolean = False
    Private _switchIndex As Integer = -1

    'Message display:
    Private _messageDelay As Single = 0F
    Private _messageText As String = ""
    Private _messageShowing As Boolean = False

    'Choose Mode
    Private ChooseMode As Boolean = True
    Private PokemonList As New List(Of Pokemon)
    Private AltPokemonList As New List(Of Pokemon)

    Public Shared Selected As Integer = -1
    Public Shared Exited As Boolean = False

    ''Public index As Integer = 0
    ''Dim MainTexture As Texture2D
    ''Dim Texture As Texture2D
    '' Dim yOffset As Single = 0

    Dim Item As Item
    ''Dim Title As String = ""

    Dim used As Boolean = False
    ''Dim canExit As Boolean = True

    Public CanChooseFainted As Boolean = True
    Public CanChooseEgg As Boolean = True
    Public CanChooseHMPokemon As Boolean = True
    Public CanChooseFusedPokemon As Boolean = True

    Public Delegate Sub DoStuff(ByVal PokeIndex As Integer)
    Dim ChoosePokemon As DoStuff
    Public ExitedSub As DoStuff

    Public LearnAttack As BattleSystem.Attack
    Public LearnType As Integer = 0
    Dim moveLearnArg As Object = Nothing

    'Stuff related to blurred PreScreens
    Private _preScreenTexture As RenderTarget2D
    Private _preScreenTarget As RenderTarget2D
    Private _blurScreens As Identifications() = {Identifications.BattleScreen,
                                                 Identifications.OverworldScreen,
                                                 Identifications.DirectTradeScreen,
                                                 Identifications.WonderTradeScreen,
                                                 Identifications.GTSSetupScreen,
                                                 Identifications.GTSTradeScreen,
                                                 Identifications.PVPLobbyScreen}

    Public Sub New(ByVal currentScreen As Screen, ByVal Item As Item, ByVal ChoosePokemon As DoStuff, ByVal Title As String, ByVal canExit As Boolean, ByVal canChooseFainted As Boolean, ByVal canChooseEgg As Boolean, Optional ByVal _pokemonList As List(Of Pokemon) = Nothing, Optional ByVal ChooseMode As Boolean = True)

        _preScreenTarget = New RenderTarget2D(GraphicsDevice, windowSize.Width, windowSize.Height, False, SurfaceFormat.Color, DepthFormat.Depth24Stencil8)
        _blur = New Resources.Blur.BlurHandler(windowSize.Width, windowSize.Height)

        Me.Item = Item
        Me.POKEMON_TITLE = Title
        Me.CanExit = canExit

        Me.ChooseMode = ChooseMode
        Me.CanChooseEgg = canChooseEgg
        Me.CanChooseFainted = canChooseFainted
        If ChoosePokemon IsNot Nothing Then
            Me.ChoosePokemon = ChoosePokemon
        End If
        Me.AltPokemonList = _pokemonList
        GetPokemonList()

        Identification = Identifications.PartyScreen
        PreScreen = currentScreen
        IsDrawingGradients = True

        Me.MouseVisible = False
        Me.CanChat = Me.PreScreen.CanChat
        Me.CanBePaused = Me.PreScreen.CanBePaused

        '_translation = New Globalization.Classes.LOCAL_PartyScreen()

        _index = Player.Temp.PokemonScreenIndex
        _texture = TextureManager.GetTexture("GUI\Menus\General")
        _menuTexture = TextureManager.GetTexture("GUI\Menus\PokemonInfo")



        If _index >= PokemonList.Count - 1 Then
            _index = 0
        End If
        _cursorDest = GetBoxPosition(_index)
        _cursorPosition = _cursorDest

        For i = 0 To PokemonList.Count - 1
            _pokemonAnimations.Add(New PokemonAnimation())
        Next

        CheckForLegendaryEmblem()
        CheckForUnoDosTresEmblem()
        CheckForBeastEmblem()
        CheckForOverkillEmblem()

        _menu = New UI.SelectMenu({""}.ToList(), 0, Nothing, 0)
        _menu.Visible = False

    End Sub
    Public Sub New(ByVal currentScreen As Screen, ByVal Item As Item, ByVal ChoosePokemon As DoStuff, ByVal Title As String, ByVal canExit As Boolean)
        Me.New(currentScreen, Item, ChoosePokemon, Title, canExit, True, True)
    End Sub
    Public Sub New(ByVal currentScreen As Screen)
        Me.New(currentScreen, New Items.Balls.Pokeball, Nothing, "Pokémon", True, True, True, Nothing, False)
    End Sub

    Private Sub GetPokemonList()
        If ChooseMode Then
            Me.PokemonList.Clear()
            If AltPokemonList IsNot Nothing Then
                For Each p As Pokemon In AltPokemonList
                    Me.PokemonList.Add(Pokemon.GetPokemonByData(p.GetSaveData()))
                Next
            Else
                For Each p As Pokemon In Core.Player.Pokemons
                    Me.PokemonList.Add(Pokemon.GetPokemonByData(p.GetSaveData()))
                Next
            End If
        Else
            Me.PokemonList = Core.Player.Pokemons
        End If
    End Sub

    Public Overrides Sub Draw()
        If _blurScreens.Contains(PreScreen.Identification) Then
            DrawPrescreen()
        Else
            PreScreen.Draw()
        End If

        DrawGradients(CInt(255 * _interfaceFade))

        DrawBackground()
        DrawPokemonArea()

        If _messageDelay > 0F Then
            Dim textFade As Single = 1.0F
            If _messageDelay <= 1.0F Then
                textFade = _messageDelay
            End If

            Canvas.DrawRectangle(New Rectangle(CInt(Core.windowSize.Width / 2 - 150), CInt(Core.windowSize.Height - 200), 300, 100), New Color(0, 0, 0, CInt(150 * textFade * _interfaceFade)))

            Dim text As String = _messageText.CropStringToWidth(FontManager.MainFont, 250) '''???
            Dim size As Vector2 = FontManager.MainFont.MeasureString(text)

            SpriteBatch.DrawString(FontManager.MainFont, text, New Vector2(CSng(Core.windowSize.Width / 2 - size.X / 2), CSng(Core.windowSize.Height - 150 - size.Y / 2)), New Color(255, 255, 255, CInt(255 * textFade * _interfaceFade)))
        End If
    End Sub


    Private _blur As Resources.Blur.BlurHandler

    Private Sub DrawPrescreen()
        If _preScreenTexture Is Nothing OrElse _preScreenTexture.IsContentLost Then
            SpriteBatch.EndBatch()

            Dim target As RenderTarget2D = _preScreenTarget
            GraphicsDevice.SetRenderTarget(target)
            GraphicsDevice.Clear(BackgroundColor)

            SpriteBatch.BeginBatch()

            PreScreen.Draw()

            SpriteBatch.EndBatch()

            GraphicsDevice.SetRenderTarget(Nothing)

            SpriteBatch.BeginBatch()

            _preScreenTexture = target
        End If

        If _interfaceFade < 1.0F Then
            SpriteBatch.Draw(_preScreenTexture, windowSize, Color.White)
        End If
        SpriteBatch.Draw(_blur.Perform(_preScreenTexture), windowSize, New Color(255, 255, 255, CInt(255 * _interfaceFade * 2).Clamp(0, 255)))
    End Sub


    Private Sub DrawBackground()
        Dim mainBackgroundColor As Color = Color.White
        If _closing And _messageDelay = 0 Then
            mainBackgroundColor = New Color(255, 255, 255, CInt(255 * _interfaceFade))
        End If

        Dim halfWidth As Integer = CInt(Core.windowSize.Width / 2)
        Dim halfHeight As Integer = CInt(Core.windowSize.Height / 2)

        Canvas.DrawRectangle(New Rectangle(halfWidth - 400, halfHeight - 232, 320, 32), New Color(84, 198, 216, mainBackgroundColor.A))
        Canvas.DrawRectangle(New Rectangle(halfWidth - 400 + 320, halfHeight - 216, 16, 16), New Color(84, 198, 216, mainBackgroundColor.A))
        SpriteBatch.Draw(_texture, New Rectangle(halfWidth - 400 + 320, halfHeight - 232, 16, 16), New Rectangle(80, 0, 16, 16), mainBackgroundColor)
        SpriteBatch.Draw(_texture, New Rectangle(halfWidth - 400 + 320 + 16, halfHeight - 216, 16, 16), New Rectangle(80, 0, 16, 16), mainBackgroundColor)

        SpriteBatch.DrawString(FontManager.MainFont, POKEMON_TITLE, New Vector2(halfWidth - 390, halfHeight - 228), mainBackgroundColor)

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

    Private Sub DrawPokemonArea()
        Canvas.DrawBorder(3, New Rectangle(CInt(_cursorPosition.X) - 3, CInt(_cursorPosition.Y) - 3, 300, 82), New Color(200, 80, 80, CInt(200 * _interfaceFade)))

        If _isSwitching Then
            Dim switchPosition As Vector2 = GetBoxPosition(_switchIndex)

            Canvas.DrawBorder(3, New Rectangle(CInt(switchPosition.X) - 6, CInt(switchPosition.Y) - 6, 306, 88), New Color(80, 80, 200, CInt(200 * _interfaceFade)))
        End If
        For i = 0 To PokemonList.Count - 1
            DrawPokemon(i)
        Next
    End Sub

    Private Sub DrawPokemon(ByVal index As Integer)
        Dim position As Vector2 = GetBoxPosition(index)

        Dim p As Pokemon = PokemonList(index)

        Dim backColor As Color = New Color(0, 0, 0, CInt(100 * _interfaceFade))
        If p.IsShiny And p.IsEgg() = False Then
            backColor = New Color(57, 59, 29, CInt(100 * _interfaceFade))
        End If

        Canvas.DrawGradient(New Rectangle(CInt(position.X), CInt(position.Y), 32, 76), New Color(0, 0, 0, 0), backColor, True, -1)
        Canvas.DrawRectangle(New Rectangle(CInt(position.X) + 32, CInt(position.Y), 228, 76), backColor)
        Canvas.DrawGradient(New Rectangle(CInt(position.X) + 260, CInt(position.Y), 32, 76), backColor, New Color(0, 0, 0, 0), True, -1)

        Dim pokeTexture = p.GetMenuTexture()
        Dim pokeXOffset = CInt((32 - pokeTexture.Width) / 2)
        Dim pokeYOffset = CInt((32 - pokeTexture.Height) / 2)

        If p.IsEgg() Then
            Dim percent As Integer = CInt((p.EggSteps / p.BaseEggSteps) * 100)
            Dim shakeMulti As Single = 1.0F
            If percent <= 33 Then
                shakeMulti = 0.2F
            ElseIf percent > 33 And percent <= 66 Then
                shakeMulti = 0.5F
            Else
                shakeMulti = 0.8F
            End If

            'menu image:
            SpriteBatch.Draw(pokeTexture, New Rectangle(CInt(position.X + 24 - pokeXOffset), CInt(position.Y + 32 + 6 - pokeYOffset), 64, 64), Nothing, New Color(255, 255, 255, CInt(255 * _interfaceFade)),
                             _pokemonAnimations(index)._shakeV * shakeMulti, New Vector2(CInt(pokeTexture.Width / 2), CInt(pokeTexture.Width / 4 * 3)), SpriteEffects.None, 0F)

            'name:
            GetFontRenderer().DrawString(FontManager.MainFont, p.GetDisplayName(), New Vector2(position.X + 156, position.Y + 27), New Color(255, 255, 255, CInt(255 * _interfaceFade)))
        Else
            Dim shakeMulti As Single = CSng((p.HP / p.MaxHP).Clamp(0.2F, 1.0F))

            'menu image:
            SpriteBatch.Draw(pokeTexture, New Rectangle(CInt(position.X + 24 - pokeXOffset), CInt(position.Y + 32 + 6 - pokeYOffset), 64, 64), Nothing, New Color(255, 255, 255, CInt(255 * _interfaceFade)),
                             _pokemonAnimations(index)._shakeV * shakeMulti, New Vector2(CInt(pokeTexture.Width / 2), CInt(pokeTexture.Width / 4 * 3)), SpriteEffects.None, 0F)


            'Item:
            If p.Item IsNot Nothing Then
                SpriteBatch.Draw(p.Item.Texture, New Rectangle(CInt(position.X) + 38, CInt(position.Y) + 32, 24, 24), New Color(255, 255, 255, CInt(255 * _interfaceFade)))
            End If

            'name:
            GetFontRenderer().DrawString(FontManager.MainFont, p.GetDisplayName(), New Vector2(position.X + 78, position.Y + 5), New Color(255, 255, 255, CInt(255 * _interfaceFade)))

            'Gender symbol:
            Select Case p.Gender
                Case Pokemon.Genders.Male
                    SpriteBatch.Draw(_menuTexture, New Rectangle(CInt(position.X + FontManager.MainFont.MeasureString(p.GetDisplayName()).X + 86), CInt(position.Y + 9), 7, 13), New Rectangle(25, 0, 7, 13), New Color(255, 255, 255, CInt(255 * _interfaceFade)))
                Case Pokemon.Genders.Female
                    SpriteBatch.Draw(_menuTexture, New Rectangle(CInt(position.X + FontManager.MainFont.MeasureString(p.GetDisplayName()).X + 85), CInt(position.Y + 9), 9, 13), New Rectangle(32, 0, 9, 13), New Color(255, 255, 255, CInt(255 * _interfaceFade)))
            End Select

            'Level:
            GetFontRenderer().DrawString(FontManager.MainFont, "Lv. " & p.Level.ToString(), New Vector2(position.X + 4, position.Y + 50), New Color(255, 255, 255, CInt(255 * _interfaceFade)))

            'HP Bar:
            SpriteBatch.Draw(_menuTexture, New Rectangle(CInt(position.X) + 102, CInt(position.Y) + 32, 111, 15), New Rectangle(16, 32, 74, 10), New Color(255, 255, 255, CInt(255 * _interfaceFade)))
            '108 pixels:
            With p
                Dim hpV As Double = .HP / .MaxHP
                Dim hpWidth As Integer
                If _closing Then
                    hpWidth = CInt(104 * hpV)
                Else
                    hpWidth = CInt((104 * _interfaceFade) * hpV)
                End If
                Dim hpColorX As Integer = 0
                If hpV < 0.5F Then
                    hpColorX = 5
                    If hpV < 0.1F Then
                        hpColorX = 10
                    End If
                End If
                If .HP > 0 And hpWidth = 0 Then
                    hpWidth = 1
                End If
                If hpWidth > 0 Then
                    Dim drawColor As Color = New Color(255, 255, 255, CInt(220 * _interfaceFade))

                    SpriteBatch.Draw(_menuTexture, New Rectangle(CInt(position.X) + 78 + 24, CInt(position.Y) + 35, 2, 8), New Rectangle(hpColorX, 42, 2, 6), drawColor)

                    SpriteBatch.Draw(_menuTexture, New Rectangle(CInt(position.X) + 78 + 24 + 2, CInt(position.Y) + 35, hpWidth, 8), New Rectangle(hpColorX + 2, 42, 1, 6), drawColor)

                    SpriteBatch.Draw(_menuTexture, New Rectangle(CInt(position.X) + 78 + 24 + 2 + hpWidth, CInt(position.Y) + 35, 2, 8), New Rectangle(hpColorX + 3, 42, 2, 6), drawColor)
                End If
            End With

            'HP display:
            GetFontRenderer().DrawString(FontManager.MainFont, p.HP & " / " & p.MaxHP, New Vector2(position.X + 100, position.Y + 50), New Color(255, 255, 255, CInt(255 * _interfaceFade)))

            'status condition
            Dim StatusTexture As Texture2D = BattleStats.GetStatImage(p.Status)
            If Not StatusTexture Is Nothing Then
                'Canvas.DrawRectangle(New Rectangle(CInt(position.X + 60), CInt(position.Y + 32), 42, 15), Color.Black)
                SpriteBatch.Draw(_menuTexture, New Rectangle(CInt(position.X) + 68, CInt(position.Y) + 32, 6, 15), New Rectangle(0, 32, 4, 10), New Color(255, 255, 255, CInt(255 * _interfaceFade)))
                SpriteBatch.Draw(_menuTexture, New Rectangle(CInt(position.X) + 74, CInt(position.Y) + 32, 28, 15), New Rectangle(16, 32, 4, 10), New Color(255, 255, 255, CInt(255 * _interfaceFade)))

                Core.SpriteBatch.Draw(StatusTexture, New Rectangle(CInt(position.X + 66), CInt(position.Y + 33), 38, 12), New Color(255, 255, 255, CInt(255 * _interfaceFade)))
            Else
                SpriteBatch.Draw(_menuTexture, New Rectangle(CInt(position.X) + 78, CInt(position.Y) + 32, 24, 15), New Rectangle(0, 32, 16, 10), New Color(255, 255, 255, CInt(255 * _interfaceFade)))
            End If

            'Able/unable display (when using TM/HM)
            Dim AttackLabel As String = ""
            If LearnType > 0 Then
                AttackLabel = "Unable!"
                Select Case LearnType
                    Case 1 ' Technical/Hidden Machine
                        If CType(moveLearnArg, Items.TechMachine).CanTeach(p) = "" Then
                            AttackLabel = "Able!"
                        End If
                End Select
            End If
            GetFontRenderer().DrawString(FontManager.MainFont, AttackLabel, New Vector2(position.X + 216, position.Y + 28), New Color(255, 255, 255, CInt(255 * _interfaceFade)))

        End If

        If _menu.Visible Then
            _menu.Draw()
        End If
    End Sub

    Protected Overrides Function GetFontRenderer() As SpriteBatch
        If IsCurrentScreen() And _interfaceFade + 0.01F >= 1.0F Then
            Return FontRenderer
        Else
            Return SpriteBatch
        End If
    End Function

    Private Function GetBoxPosition(ByVal index As Integer) As Vector2
        Dim position As New Vector2

        '292 x 76
        Dim halfWidth As Integer = CInt(Core.windowSize.Width / 2)
        Dim halfHeight As Integer = CInt(Core.windowSize.Height / 2)

        position.Y = CSng((Math.Floor(index / 2) * 128) + (halfHeight - 200) + 42)

        If index Mod 2 = 0 Then
            position.X = halfWidth - 328
        Else
            position.X = halfWidth + 36
        End If

        Return position
    End Function

    Public Overrides Sub Update()
        If _pokemonAnimations.Count > 0 Then
            Dim animation As PokemonAnimation = _pokemonAnimations(_index)
            If animation._shakeLeft Then
                animation._shakeV -= 0.035F
                If animation._shakeV <= -0.4F Then
                    animation._shakeCount -= 1
                    animation._shakeLeft = False
                End If
            Else
                animation._shakeV += 0.035F
                If animation._shakeV >= 0.4F Then
                    animation._shakeCount -= 1
                    animation._shakeLeft = True
                End If
            End If
        End If

        If _messageDelay > 0F Then
            _messageDelay -= 0.1F
            If _messageDelay <= 0F Then
                _messageDelay = 0F
            End If
        End If

        If _closing And _messageDelay = 0 Then
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
                SetScreen(PreScreen)
            End If
        Else
            If _closing And (Controls.Dismiss() Or Controls.Accept) Then
                _messageDelay = 0
            End If
            Dim maxWindowHeight As Integer = 400
            If _enrollY < maxWindowHeight Then
                _enrollY = MathHelper.Lerp(maxWindowHeight, _enrollY, 0.8F)
                If _enrollY >= maxWindowHeight Then
                    _enrollY = maxWindowHeight
                End If
            End If
            If _interfaceFade < 1.0F Then
                _interfaceFade = MathHelper.Lerp(1, _interfaceFade, 0.95F)
                If _interfaceFade > 1.0F Then
                    _interfaceFade = 1.0F
                End If
            End If

            If _menu.Visible Then
                _menu.Update()
            Else
                If Controls.Down(True, True, False, True, True, True) And _index < PokemonList.Count - 2 Then
                    _pokemonAnimations(_index)._shakeV = 0
                    _index += 2
                    _cursorDest = GetBoxPosition(_index)
                End If
                If Controls.Up(True, True, False, True, True, True) And _index > 1 Then
                    _pokemonAnimations(_index)._shakeV = 0
                    _index -= 2
                    _cursorDest = GetBoxPosition(_index)
                End If
                If Controls.Left(True) And _index > 0 Then
                    _pokemonAnimations(_index)._shakeV = 0
                    _index -= 1
                    _cursorDest = GetBoxPosition(_index)
                End If
                If Controls.Right(True) And _index < PokemonList.Count - 1 Then
                    _pokemonAnimations(_index)._shakeV = 0
                    _index += 1
                    _cursorDest = GetBoxPosition(_index)
                End If

                Player.Temp.PokemonScreenIndex = _index

                _cursorPosition.X = MathHelper.Lerp(_cursorDest.X, _cursorPosition.X, 0.8F)
                _cursorPosition.Y = MathHelper.Lerp(_cursorDest.Y, _cursorPosition.Y, 0.8F)

                If Controls.Accept() Then
                    If _isSwitching Then
                        _isSwitching = False

                        If _switchIndex <> _index Then
                            Dim p1 As Pokemon = PokemonList(_switchIndex)
                            Dim p2 As Pokemon = PokemonList(_index)
                            SoundManager.PlaySound("select")
                            PokemonList(_switchIndex) = p2
                            PokemonList(_index) = p1
                        End If
                    Else
                        _cursorPosition = _cursorDest
                        CreateMainMenu()
                    End If
                End If

                If Controls.Dismiss() And CanExit Then
                    If _isSwitching Then
                        _isSwitching = False
                    Else
                        Selected = -1
                        If Not ExitedSub Is Nothing Then
                            used = True
                            ExitedSub(_index)
                        End If
                        SoundManager.PlaySound("select")
                        _closing = True
                    End If
                End If
            End If
        End If

    End Sub

    Private Sub CreateMainMenu()
        If Mode = ISelectionScreen.ScreenMode.Default Then
            CreateNormalMenu("Summary")
        ElseIf Mode = ISelectionScreen.ScreenMode.Selection Then
            CreateSelectionMenu()
        End If
    End Sub

    Private Sub CreateSelectionMenu()
        Dim items As New List(Of String)
        items.Add("Select")
        items.Add("Summary")
        items.Add("Back")

        _menu = New UI.SelectMenu(items, 0, AddressOf SelectSelectionMenuItem, items.Count - 1)
    End Sub

    Private Sub SelectSelectionMenuItem(ByVal selectMenu As UI.SelectMenu)
        Select Case selectMenu.SelectedItem
            Case "Select"
                'When a Pokémon got selected in Selection Mode, raise the selected event and close the screen.
                If CanChoosePokemon(Me.PokemonList(_index)) = True Then
                    Selected = _index
                    FireSelectionEvent(_index)
                    GetPokemonList()
                    _closing = True
                Else
                    TextBox.Show("Cannot choose this~Pokémon.")
                End If
            Case "Summary"
                SetScreen(New SummaryScreen(Me, PokemonList.ToArray(), _index))
        End Select
    End Sub

    Private Function CanChoosePokemon(ByVal p As Pokemon) As Boolean
        If Me.CanChooseFainted = False Then
            If p.HP <= 0 Or p.Status = Pokemon.StatusProblems.Fainted Then
                Return False
            End If
        End If
        If Me.CanChooseEgg = False Then
            If p.IsEgg() = True Then
                Return False
            End If
        End If
        If Me.CanChooseHMPokemon = False Then
            If p.HasHMMove() = True Then
                Return False
            End If
        End If
        If Me.CanChooseFusedPokemon = False Then
            If p.OriginalNumber = 646 Then
                If p.AdditionalData = "black" Or p.AdditionalData = "white" Then
                    Return False
                End If
            End If
        End If
        Return True
    End Function


    Private Sub CreateNormalMenu(ByVal selectedItem As String)
        Dim p As Pokemon = PokemonList(_index)

        Dim items As New List(Of String)
        items.Add("Summary")

        If p.IsEgg() = False Then
            If CanUseMove(p, "Fly", Badge.HMMoves.Fly) Or
            CanUseMove(p, "Ride", Badge.HMMoves.Ride) Or
            CanUseMove(p, "Flash", Badge.HMMoves.Flash) Or
            CanUseMove(p, "Cut", Badge.HMMoves.Cut) Or
            CanUseMove(p, "Teleport", -1) Or
            CanUseMove(p, "Dig", -1) Then

                items.Add("Field Move")
            End If
        End If

        items.Add("Switch")

        If p.IsEgg() = False Then
            items.Add("Item")
        End If

        items.Add("Back")

        _menu = New UI.SelectMenu(items, items.IndexOf(selectedItem), AddressOf SelectedMainMenuItem, items.Count - 1)
    End Sub

    Private Sub CreateFieldMoveMenu()
        Dim p As Pokemon = PokemonList(_index)

        Dim items As New List(Of String)
        If CanUseMove(p, "Fly", Badge.HMMoves.Fly) Then
            items.Add("Fly")
        End If
        If CanUseMove(p, "Ride", Badge.HMMoves.Ride) Then
            items.Add("Ride")
        End If
        If CanUseMove(p, "Flash", Badge.HMMoves.Flash) Then
            items.Add("Flash")
        End If
        If CanUseMove(p, "Cut", Badge.HMMoves.Cut) Then
            items.Add("Cut")
        End If
        If CanUseMove(p, "Teleport", -1) Then
            items.Add("Teleport")
        End If
        If CanUseMove(p, "Dig", -1) Then
            items.Add("Dig")
        End If

        items.Add("Back")

        _menu = New UI.SelectMenu(items, 0, AddressOf SelectedFieldMoveMenuItem, items.Count - 1)
    End Sub

    Private Sub CreateItemMenu()
        Dim p As Pokemon = PokemonList(_index)

        Dim items As New List(Of String)

        items.Add("Give")
        If p.Item IsNot Nothing Then
            items.Add("Take")
        End If
        items.Add("Back")

        _menu = New UI.SelectMenu(items, 0, AddressOf SelectedItemMenuItem, items.Count - 1)
    End Sub

    Private Function CanUseMove(ByVal p As Pokemon, ByVal moveName As String, ByVal hmMove As Integer) As Boolean
        If GameController.IS_DEBUG_ACTIVE Then
            Return True
        End If
        If p.IsEgg() = False Then
            If hmMove > -1 Then
                If Badge.CanUseHMMove(CType(hmMove, Badge.HMMoves)) = False Then
                    Return False
                End If
            End If
            For Each a As BattleSystem.Attack In p.Attacks
                If a.Name.ToLower() = moveName.ToLower() Then
                    Return True
                End If
            Next
        End If
        Return False
    End Function

    Private Function CanUseMove(ByVal p As Pokemon, ByVal moveName As String, ByVal hmMove As Badge.HMMoves) As Boolean
        Return CanUseMove(p, moveName, CInt(hmMove))
    End Function

    Private Sub SelectedMainMenuItem(ByVal selectMenu As UI.SelectMenu)
        Select Case selectMenu.SelectedItem
            Case "Summary"
                SetScreen(New SummaryScreen(Me, PokemonList.ToArray(), _index))
            Case "Field Move"
                CreateFieldMoveMenu()
            Case "Switch"
                _switchIndex = _index
                _isSwitching = True
            Case "Item"
                CreateItemMenu()
        End Select
    End Sub

    Private Sub SelectedFieldMoveMenuItem(ByVal selectMenu As UI.SelectMenu)
        Select Case selectMenu.SelectedItem
            Case "Fly"
                UseFly()
            Case "Ride"
                UseRide()
            Case "Flash"
                UseFlash()
            Case "Cut"
                UseCut()
            Case "Teleport"
                UseTeleport()
            Case "Dig"
                UseDig()
            Case "Back"
                CreateNormalMenu("Field Move")
        End Select
    End Sub

    Private Sub SelectedItemMenuItem(ByVal selectMenu As UI.SelectMenu)
        Select Case selectMenu.SelectedItem
            Case "Give"

                Dim selScreen As New NewInventoryScreen(Core.CurrentScreen)
                selScreen.Mode = Screens.UI.ISelectionScreen.ScreenMode.Selection
                selScreen.CanExit = True

                AddHandler selScreen.SelectedObject, AddressOf GiveItemHandler

                Core.SetScreen(selScreen)
            Case "Take"
                Dim p As Pokemon = PokemonList(_index)

                If p.Item.IsMail And p.Item.AdditionalData <> "" Then
                    ShowMessage("The Mail was taken to your inbox on your PC.")

                    Core.Player.Mails.Add(Items.MailItem.GetMailDataFromString(p.Item.AdditionalData))

                    p.Item = Nothing
                Else
                    ShowMessage("Taken " & p.Item.Name & "  from " & p.GetDisplayName() & ".")

                    Core.Player.Inventory.AddItem(p.Item.ID, 1)
                    p.Item = Nothing
                End If
            Case "Back"
                CreateNormalMenu("Item")
        End Select
    End Sub

    ''' <summary>
    ''' A handler method to convert the incoming object array.
    ''' </summary>
    Private Sub GiveItemHandler(ByVal params As Object())
        GiveItem(CInt(params(0)))
    End Sub

    Private Sub GiveItem(ByVal itemID As Integer)
        Dim i As Item = Item.GetItemByID(itemID)

        If i.CanBeHold Then
            Dim p As Pokemon = PokemonList(_index)

            Core.Player.Inventory.RemoveItem(itemID, 1)

            Dim message As String = ""

            Dim reItem As Item = p.Item
            If reItem IsNot Nothing Then
                If reItem.IsMail And reItem.AdditionalData <> "" Then
                    Core.Player.Mails.Add(Items.MailItem.GetMailDataFromString(reItem.AdditionalData))

                    message = "Gave " & i.Name & " to " & p.GetDisplayName() & " and took the Mail to the PC."
                Else
                    Core.Player.Inventory.AddItem(reItem.ID, 1)

                    message = "Switched " & p.GetDisplayName() & "'s " & i.Name & " with a " & reItem.Name & "."
                End If
            Else
                message = "Gave " & p.GetDisplayName() & " a " & i.Name & "."
            End If

            p.Item = i

            ShowMessage(message)
        Else
            ShowMessage(i.Name & " cannot be given to a Pokémon.")
        End If
    End Sub

    Public Sub ShowMessage(ByVal text As String)
        _messageDelay = CSng(text.Length / 1.75)
        _messageText = text
    End Sub

    Public Overrides Sub SizeChanged()
        _cursorDest = GetBoxPosition(_index)
        _cursorPosition = _cursorDest
    End Sub

#Region "Emblems"

    Private Sub CheckForLegendaryEmblem()
        'This sub checks if Ho-Oh, Lugia and Suicune are in the player's party.
        Dim hasHoOh As Boolean = False
        Dim hasLugia As Boolean = False
        Dim hasSuicune As Boolean = False

        For Each p As Pokemon In PokemonList
            Select Case p.Number
                Case 245
                    hasSuicune = True
                Case 249
                    hasLugia = True
                Case 250
                    hasHoOh = True
            End Select
        Next

        If hasSuicune And hasLugia And hasHoOh Then
            GameJolt.Emblem.AchieveEmblem("legendary")
        End If
    End Sub

    Private Sub CheckForUnoDosTresEmblem()
        'This sub checks if Articuno, Zapdos and Moltres are in the player's party.
        Dim hasArticuno As Boolean = False
        Dim hasZapdos As Boolean = False
        Dim hasMoltres As Boolean = False

        For Each p As Pokemon In PokemonList
            Select Case p.Number
                Case 144
                    hasArticuno = True
                Case 145
                    hasZapdos = True
                Case 146
                    hasMoltres = True
            End Select
        Next

        If hasArticuno And hasZapdos And hasMoltres Then
            GameJolt.Emblem.AchieveEmblem("unodostres")
        End If
    End Sub

    Private Sub CheckForBeastEmblem()
        'This sub checks if Entei, Raikou and Suicune are in the player's party.
        Dim hasRaikou As Boolean = False
        Dim hasEntei As Boolean = False
        Dim hasSuicune2 As Boolean = False

        For Each p As Pokemon In PokemonList
            Select Case p.Number
                Case 243
                    hasRaikou = True
                Case 244
                    hasEntei = True
                Case 245
                    hasSuicune2 = True
            End Select
        Next

        If hasRaikou And hasEntei And hasSuicune2 Then
            GameJolt.Emblem.AchieveEmblem("beast")
        End If
    End Sub

    Private Sub CheckForOverkillEmblem()
        If PokemonList.Count = 6 Then
            Dim has100 As Boolean = True
            For i = 0 To 5
                If PokemonList(i).Level < 100 Then
                    has100 = False
                    Exit For
                End If
            Next
            If has100 Then
                GameJolt.Emblem.AchieveEmblem("overkill")
            End If
        End If
    End Sub

#End Region

#Region "Field Moves"

    'TEMPORARY
    Private Sub UseFlash()
        ChooseBox.Showing = False
        Core.SetScreen(Me.PreScreen)
        If Core.CurrentScreen.Identification = Identifications.MenuScreen Then
            Core.SetScreen(Core.CurrentScreen.PreScreen)
        End If
        If Screen.Level.IsDark = True Then
            Dim s As String = "version=2" & Environment.NewLine &
                              "@text.show(" & PokemonList(_index).GetDisplayName() & " used~Flash!)" & Environment.NewLine &
                              "@environment.toggledarkness" & Environment.NewLine &
                              "@sound.play(FieldMove_Flash)" & Environment.NewLine &
                              "@text.show(The area got lit up!)" & Environment.NewLine &
                              ":end"
            PlayerStatistics.Track("Flash used", 1)
            CType(Core.CurrentScreen, OverworldScreen).ActionScript.StartScript(s, 2)
        Else
            Dim s As String = "version=2" & Environment.NewLine &
                "@text.show(" & PokemonList(_index).GetDisplayName() & " used~Flash!)" & Environment.NewLine &
                                            "@sound.play(FieldMove_Flash)" & Environment.NewLine &
                                            "@text.show(The area is already~lit up!)" & Environment.NewLine &
                                            ":end"
            CType(Core.CurrentScreen, OverworldScreen).ActionScript.StartScript(s, 2)
        End If
    End Sub

    Private Sub UseFly()
        If Level.CanFly = True Or GameController.IS_DEBUG_ACTIVE = True Or Core.Player.SandBoxMode = True Then
            ChooseBox.Showing = False
            Core.SetScreen(Me.PreScreen)
            If Core.CurrentScreen.Identification = Identifications.MenuScreen Then
                Core.SetScreen(Core.CurrentScreen.PreScreen)
            End If

            If Screen.Level.CurrentRegion.Contains(",") = True Then
                Dim regions As List(Of String) = Screen.Level.CurrentRegion.Split(CChar(",")).ToList()
                Core.SetScreen(New TransitionScreen(Core.CurrentScreen, New MapScreen(Core.CurrentScreen, regions, 0, {"Fly", PokemonList(_index)}), Color.White, False))
            Else
                Dim startRegion As String = Screen.Level.CurrentRegion
                Core.SetScreen(New TransitionScreen(Core.CurrentScreen, New MapScreen(Core.CurrentScreen, startRegion, {"Fly", PokemonList(_index)}), Color.White, False))
            End If
        Else
            TextBox.Show("You cannot Fly~from here!", {}, True, False)
        End If
    End Sub

    Private Sub UseCut()
        Dim grassEntities = Grass.GetGrassTilesAroundPlayer(2.4F)
        If grassEntities.Count > 0 Then
            ChooseBox.Showing = False
            Core.SetScreen(Me.PreScreen)
            If Core.CurrentScreen.Identification = Identifications.MenuScreen Then
                Core.SetScreen(Core.CurrentScreen.PreScreen)
            End If

            PlayerStatistics.Track("Cut used", 1)
            TextBox.Show(PokemonList(_index).GetDisplayName() & "~used Cut!", {}, True, False)
            PokemonList(_index).PlayCry()
            For Each e As Entity In grassEntities
                e.CanBeRemoved = True
            Next
        Else
            TextBox.Show("There is nothing~to be Cut!", {}, True, False)
        End If
    End Sub

    Private Sub UseRide()
        If Screen.Level.Riding = True Then
            Screen.Level.Riding = False
            Screen.Level.OwnPlayer.SetTexture(Core.Player.TempRideSkin, True)
            Core.Player.Skin = Core.Player.TempRideSkin

            ChooseBox.Showing = False
            Core.SetScreen(Me.PreScreen)
            If Core.CurrentScreen.Identification = Identifications.MenuScreen Then
                Core.SetScreen(Core.CurrentScreen.PreScreen)
            End If

            If Screen.Level.IsRadioOn = False OrElse GameJolt.PokegearScreen.StationCanPlay(Screen.Level.SelectedRadioStation) = False Then
                MusicManager.Play(Level.MusicLoop)
            End If
        Else
            If Screen.Level.Surfing = False And Screen.Camera.IsMoving() = False And Screen.Camera.Turning = False And Level.CanRide() = True Then
                ChooseBox.Showing = False
                Core.SetScreen(Me.PreScreen)
                If Core.CurrentScreen.Identification = Identifications.MenuScreen Then
                    Core.SetScreen(Core.CurrentScreen.PreScreen)
                End If

                Screen.Level.Riding = True
                Core.Player.TempRideSkin = Core.Player.Skin

                Dim skin As String = "[POKEMON|"
                If PokemonList(_index).IsShiny = True Then
                    skin &= "S]"
                Else
                    skin &= "N]"
                End If
                skin &= PokemonList(_index).Number & PokemonForms.GetOverworldAddition(PokemonList(_index))

                Screen.Level.OwnPlayer.SetTexture(skin, False)

                SoundManager.PlayPokemonCry(PokemonList(_index).Number)

                TextBox.Show(PokemonList(_index).GetDisplayName() & " used~Ride!", {}, True, False)
                PlayerStatistics.Track("Ride used", 1)

                If Screen.Level.IsRadioOn = False OrElse GameJolt.PokegearScreen.StationCanPlay(Screen.Level.SelectedRadioStation) = False Then
                    MusicManager.Play("ride", True)
                End If
            Else
                TextBox.Show("You cannot Ride here!", {}, True, False)
            End If
        End If
    End Sub

    Private Sub UseDig()
        If Screen.Level.CanDig = True Or GameController.IS_DEBUG_ACTIVE = True Or Core.Player.SandBoxMode = True Then
            ChooseBox.Showing = False
            Core.SetScreen(Me.PreScreen)
            If Core.CurrentScreen.Identification = Identifications.MenuScreen Then
                Core.SetScreen(Core.CurrentScreen.PreScreen)
            End If

            Dim setToFirstPerson As Boolean = Not CType(Screen.Camera, OverworldCamera).ThirdPerson

            Dim s As String = "version=2
@text.show(" & PokemonList(_index).GetDisplayName() & " used Dig!)
@level.wait(20)
@camera.activatethirdperson
@camera.reset
@camera.fix
@player.turnto(0)
@sound.play(destroy)
:while:<player.position(y)>>" & (Screen.Camera.Position.Y - 1.4).ToString().ReplaceDecSeparator() & "
@player.turn(1)
@player.warp(~,~-0.1,~)
@level.wait(1)
:endwhile
@screen.fadeout
@camera.defix
@player.warp(" & Core.Player.LastRestPlace & "," & Core.Player.LastRestPlacePosition & ",0)" & Environment.NewLine &
"@player.turnto(2)"

            If setToFirstPerson = True Then
                s &= Environment.NewLine & "@camera.deactivatethirdperson"
            End If
            s &= Environment.NewLine &
"@level.update
@screen.fadein
:end"

            PlayerStatistics.Track("Dig used", 1)
            CType(Core.CurrentScreen, OverworldScreen).ActionScript.StartScript(s, 2)
            If Screen.Level.Surfing = True Then
                Screen.Level.Surfing = False
                Screen.Level.OwnPlayer.SetTexture(Core.Player.TempSurfSkin, True)
                Core.Player.Skin = Core.Player.TempSurfSkin

                Screen.Level.OverworldPokemon.warped = True
                Screen.Level.OverworldPokemon.Visible = False
            End If
        Else
            TextBox.Show("Cannot use Dig here.", {}, True, False)
        End If
    End Sub

    Private Sub UseTeleport()
        If Screen.Level.CanTeleport = True Or GameController.IS_DEBUG_ACTIVE = True Or Core.Player.SandBoxMode = True Then
            ChooseBox.Showing = False
            Core.SetScreen(Me.PreScreen)
            If Core.CurrentScreen.Identification = Identifications.MenuScreen Then
                Core.SetScreen(Core.CurrentScreen.PreScreen)
            End If

            Dim setToFirstPerson As Boolean = Not CType(Screen.Camera, OverworldCamera).ThirdPerson

            Dim yFinish As String = (Screen.Camera.Position.Y + 2.9F).ToString().ReplaceDecSeparator()

            Dim s As String = "version=2
@text.show(" & PokemonList(_index).GetDisplayName() & "~used Teleport!)
@level.wait(20)
@camera.activatethirdperson
@camera.reset
@camera.fix
@player.turnto(0)
@sound.play(teleport)
:while:<player.position(y)><" & yFinish & "
@player.turn(1)
@player.warp(~,~+0.1,~)
@level.wait(1)
:endwhile
@screen.fadeout
@camera.defix
@player.warp(" & Core.Player.LastRestPlace & "," & Core.Player.LastRestPlacePosition & ",0)
@player.turnto(2)"

            If setToFirstPerson = True Then
                s &= Environment.NewLine & "@camera.deactivatethirdperson"
            End If
            s &= Environment.NewLine &
"@level.update
@screen.fadein
:end"

            PlayerStatistics.Track("Teleport used", 1)
            CType(Core.CurrentScreen, OverworldScreen).ActionScript.StartScript(s, 2)
            If Screen.Level.Surfing = True Then
                Screen.Level.Surfing = False
                Screen.Level.OwnPlayer.SetTexture(Core.Player.TempSurfSkin, True)
                Core.Player.Skin = Core.Player.TempSurfSkin

                Screen.Level.OverworldPokemon.warped = True
                Screen.Level.OverworldPokemon.Visible = False
            End If
        Else
            TextBox.Show("Cannot use Teleport here.", {}, True, False)
        End If
    End Sub

#End Region

    Private _mode As ISelectionScreen.ScreenMode = ISelectionScreen.ScreenMode.Default
    Private _canExit As Boolean = True

    Public Event SelectedObject(params() As Object) Implements ISelectionScreen.SelectedObject

    Private Sub FireSelectionEvent(ByVal pokemonIndex As Integer)
        RaiseEvent SelectedObject(New Object() {pokemonIndex})
    End Sub

    ''' <summary>
    ''' The current mode of this screen.
    ''' </summary>
    Public Property Mode As ISelectionScreen.ScreenMode Implements ISelectionScreen.Mode
        Get
            Return _mode
        End Get
        Set(value As ISelectionScreen.ScreenMode)
            _mode = value
        End Set
    End Property

    ''' <summary>
    ''' If the user can quit the screen in selection mode without choosing an item.
    ''' </summary>
    Public Property CanExit As Boolean Implements ISelectionScreen.CanExit
        Get
            Return _canExit
        End Get
        Set(value As Boolean)
            _canExit = value
        End Set
    End Property

    Public Sub SetupLearnAttack(ByVal a As BattleSystem.Attack, ByVal learnType As Integer, ByVal arg As Object)
        Me.LearnAttack = a
        Me.LearnType = learnType
        Me.moveLearnArg = arg
    End Sub

End Class