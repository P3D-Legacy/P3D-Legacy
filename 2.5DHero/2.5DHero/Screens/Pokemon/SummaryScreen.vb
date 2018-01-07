﻿Public Class SummaryScreen

    Inherits Screen

    Private _party As Pokemon()

    Private _pageIndex As Integer = 0
    Private _partyIndex As Integer = -1
    Private _selectedPokemon As Pokemon = Nothing

    Private _texture As Texture2D

    Private _isFront As Boolean = True

    'Pointer
    Private _pointerDest As Integer = 0
    Private _pointerPos As Single = 0F
    Private _pokemonDest As Integer = 0
    Private _pokemonPos As Single = 0F
    Private _previewerOffset As Integer = 0

    'Fade in animation:
    Private _fadeIn As Single = 0F
    Private _interfaceFade As Single = 0F

    'Pixel animation:
    Private _pixelFade As Single = 0F
    Private pixeledPokemonTexture As Texture2D

    'Enroll animation:
    Private _enrollY As Single = 0F

    Private _closing As Boolean = False

    'Y offset for Pokémon draw:
    Dim _yOffset As Integer = 0

    'Page animation:
    Private _pageFade As Single = 1.0F
    Private _pageClosing As Boolean = False
    Private _pageOpening As Boolean = False

    'Move display:
    Private _moveIndex As Integer = 0
    Private _moveSelected As Boolean = False
    Private _moveFade As Single = 0F
    Private _moveSelectionFade As Single = 0F
    Private _moveSelectorPosition As Single = 0F

    'Move switching:
    Private _switchingMoves As Boolean = False
    Private _switchMoveIndex As Integer = -1

    Public Sub New(ByVal currentScreen As Screen, ByVal party As Pokemon(), ByVal partyIndex As Integer)
        PreScreen = currentScreen
        Identification = Identifications.SummaryScreen

        _texture = TextureManager.GetTexture("GUI\Menus\General")

        '_pageIndex = Player.Temp.PokemonSummaryPageIndex
        _pageIndex = Player.Temp.PokemonSummaryPageIndex
        _partyIndex = partyIndex
        _party = party

        SetDest(_partyIndex)
        GetYOffset()
        _pointerPos = _pointerDest
        _pokemonPos = _pokemonDest
        _moveSelectorPosition = GetMoveSelectorDest(_moveIndex)
    End Sub

    Public Sub New(ByVal currentScreen As Screen, ByVal selectedPokemon As Pokemon)
        PreScreen = currentScreen
        Identification = Identifications.SummaryScreen

        _texture = TextureManager.GetTexture("GUI\Menus\General")

        _pageIndex = Player.Temp.PokemonSummaryPageIndex
        _selectedPokemon = selectedPokemon

        SetDest(_partyIndex)
        GetYOffset()
        _pointerPos = _pointerDest
        _pokemonPos = _pokemonDest
        _moveSelectorPosition = GetMoveSelectorDest(_moveIndex)
    End Sub

    Private Function GetMoveSelectorDest(ByVal moveIndex As Integer) As Single
        Return 172 + moveIndex * 96
    End Function

    Public Overrides Sub Draw()
        PreScreen.Draw()

        DrawGradients(CInt(255 * _interfaceFade))

        DrawMain()

        If GetPokemon().IsEgg() = False Then
            Select Case _pageIndex
                Case 0
                    DrawPage1()
                Case 1
                    DrawPage2()
            End Select
        Else
            DrawEgg()
        End If
    End Sub

    Public Overrides Sub Render()

        _pixelFade = 1  'Remove when pixel fading effect is properly implemented.

        Dim pixelSize As Integer = CInt(256 * _pixelFade).Clamp(16, 256)
        If pixelSize = 256 Or Core.GraphicsManager.IsFullScreen = True Then
            pixeledPokemonTexture = GetPokemon().GetTexture(True)
        Else
            Dim pixeled As New RenderTarget2D(GraphicsDevice, pixelSize, pixelSize, False, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.PreserveContents)
            GraphicsDevice.SetRenderTarget(pixeled)
            GraphicsDevice.Clear(Color.Transparent)
            Dim s As New SpriteBatch(GraphicsDevice)
            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise)
            s.Draw(GetPokemon().GetTexture(True), New Rectangle(0, 0, pixelSize, pixelSize), Color.White)
            s.End()

            Dim dePixeled As New RenderTarget2D(GraphicsDevice, 256, 256, False, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.PreserveContents)
            GraphicsDevice.SetRenderTarget(dePixeled)
            GraphicsDevice.Clear(Color.Transparent)
            s = New SpriteBatch(GraphicsDevice)
            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise)
            s.Draw(pixeled, New Rectangle(0, 0, 256, 256), Color.White)
            s.End()
            pixeledPokemonTexture = dePixeled

            GraphicsDevice.SetRenderTarget(Nothing)
        End If
    End Sub

    Private Sub DrawMain()
        'Draw pointer and team:
        Dim mainBackgroundColor As Color = Color.White
        If _closing = True Then
            mainBackgroundColor = New Color(255, 255, 255, CInt(255 * _interfaceFade))
        End If

        If _partyIndex > -1 Then
            For i = 0 To _party.Count - 1
                Dim pokemonPos As Double = GetPokemonDest(i) - 16 - (64 + 16) * (_pokemonDest - _pokemonPos)

                Core.SpriteBatch.Draw(_party(i).GetMenuTexture(), New Rectangle(CInt(pokemonPos), 16, 64, 64), mainBackgroundColor)
            Next

            SpriteBatch.Draw(TextureManager.GetTexture("GUI\Menus\PokemonInfo"), New Rectangle(CInt(_pointerPos), 80, 32, 16), New Rectangle(0, 16, 32, 16), mainBackgroundColor)
        End If

        Dim onePixelLineColor As Color = New Color(84, 198, 216)
        If _closing = True Then
            onePixelLineColor.A = CByte(255 * _interfaceFade)
        End If

        Canvas.DrawRectangle(New Rectangle(50, 96, CInt(Math.Ceiling((Core.windowSize.Width - 100) / 16) * 16), 1), onePixelLineColor)

        'Draw background:
        Dim t As Texture2D = TextureManager.GetTexture("GUI\Menus\PokemonInfo")

        For y = 0 To CInt(_enrollY) Step 16
            For x = 0 To Core.windowSize.Width - 100 Step 16
                SpriteBatch.Draw(_texture, New Rectangle(50 + x, y + 97, 16, 16), New Rectangle(64, 0, 4, 4), mainBackgroundColor)
            Next
        Next

        Dim modRes As Integer = CInt(_enrollY) Mod 16
        If modRes > 0 Then
            For x = 0 To Core.windowSize.Width - 100 Step 16
                SpriteBatch.Draw(_texture, New Rectangle(50 + x, CInt(_enrollY + 97), 16, modRes), New Rectangle(64, 0, 4, 4), mainBackgroundColor)
            Next
        End If

        'If the moves are selected and the second page is open, draw a gray backdrop:
        If _pageIndex = 1 And _moveSelected = True Then
            Canvas.DrawRectangle(New Rectangle(50, 96, CInt(Math.Ceiling((Core.windowSize.Width - 100) / 16) * 16), CInt(Math.Ceiling((Core.windowSize.Height - 146) / 16) * 16) + 1), New Color(0, 0, 0, CInt(40 * _moveFade)))
            If _partyIndex > -1 Then
                SpriteBatch.Draw(t, New Rectangle(CInt(_pointerPos), 80, 32, 16), New Rectangle(0, 16, 32, 16), New Color(0, 0, 0, CInt(40 * _moveFade)))
            End If
        End If

        Dim shinyGradientColor As Color = New Color(0, 0, 0, CInt(30 * _fadeIn))
        If GetPokemon().IsShiny = True And GetPokemon().IsEgg() = False Then
            shinyGradientColor = New Color(232, 195, 75, CInt(30 * _fadeIn))
        End If

        Canvas.DrawRectangle(New Rectangle(50, 96, 50, CInt(Math.Ceiling((_enrollY) / 16) * 16) + 1), shinyGradientColor)
        Canvas.DrawGradient(New Rectangle(100, 96, 250, CInt(Math.Ceiling((_enrollY) / 16) * 16) + 1), shinyGradientColor, New Color(shinyGradientColor.ToVector3()) With {.A = 0}, True, -1)

        'Draw Pokémon preview:
        If _enrollY >= 160 Then
            Dim height As Integer = CInt(_enrollY - 160).Clamp(0, 256)
            Dim pokemonTexture = GetPokemon().GetTexture(_isFront)
            Dim textureHeight As Integer = CInt(pokemonTexture.Height * (height / 256))

            Dim pokemonTextureOffset As Integer = 32
            If GetPokemon().IsEgg() = True Then
                pokemonTextureOffset = 64
            End If

            SpriteBatch.Draw(pokemonTexture, New Rectangle(70 + 10, 160 - _yOffset + pokemonTextureOffset + 10, 256, height), New Rectangle(0, 0, pokemonTexture.Width, textureHeight), New Color(0, 0, 0, 150))
            SpriteBatch.Draw(pokemonTexture, New Rectangle(70, 160 - _yOffset + pokemonTextureOffset, 256, height), New Rectangle(0, 0, pokemonTexture.Width, textureHeight), Color.White)
        End If

        'Draw main infos:
        Canvas.DrawRectangle(New Rectangle(50, 108, 250, 32), New Color(0, 0, 0, CInt(100 * _interfaceFade)))
        Canvas.DrawGradient(New Rectangle(300, 108, 50, 32), New Color(0, 0, 0, CInt(100 * _interfaceFade)), New Color(0, 0, 0, 0), True, -1)
        Canvas.DrawRectangle(New Rectangle(50, 140, 250, 32), New Color(0, 0, 0, CInt(70 * _interfaceFade)))
        Canvas.DrawGradient(New Rectangle(300, 140, 50, 32), New Color(0, 0, 0, CInt(70 * _interfaceFade)), New Color(0, 0, 0, 0), True, -1)

        SpriteBatch.DrawString(FontManager.ChatFont, GetPokemon().GetDisplayName(), New Vector2(60, 112), New Color(255, 255, 255, CInt(220 * _fadeIn)))

        If GetPokemon().IsEgg() = False Then
            SpriteBatch.DrawString(FontManager.ChatFont, "Lv. " & GetPokemon().Level, New Vector2(100, 144), New Color(255, 255, 255, CInt(220 * _fadeIn)))

            'Draw shiny star:
            If GetPokemon().IsShiny = True Then
                SpriteBatch.Draw(t, New Rectangle(69, 430, 18, 18), New Rectangle(16, 0, 9, 9), New Color(255, 255, 255, CInt(255 * _fadeIn)))
            End If

            'Draw gender:
            Select Case GetPokemon().Gender
                Case Pokemon.Genders.Male
                    SpriteBatch.Draw(t, New Rectangle(240, 111, 14, 26), New Rectangle(25, 0, 7, 13), New Color(255, 255, 255, CInt(220 * _fadeIn)))
                Case Pokemon.Genders.Female
                    SpriteBatch.Draw(t, New Rectangle(238, 111, 18, 26), New Rectangle(32, 0, 9, 13), New Color(255, 255, 255, CInt(220 * _fadeIn)))
            End Select

            'Draw Catch ball:
            If GetPokemon().CatchBall IsNot Nothing Then
                SpriteBatch.Draw(GetPokemon().CatchBall.Texture, New Rectangle(65, 144, 24, 24), New Color(255, 255, 255, CInt(255 * _fadeIn)))
            End If
        End If

        Canvas.DrawRectangle(New Rectangle(50, 458, 250, 32), New Color(0, 0, 0, CInt(100 * _interfaceFade)))
        Canvas.DrawGradient(New Rectangle(300, 458, 50, 32), New Color(0, 0, 0, CInt(100 * _interfaceFade)), New Color(0, 0, 0, 0), True, -1)
        Canvas.DrawRectangle(New Rectangle(50, 490, 250, 32), New Color(0, 0, 0, CInt(70 * _interfaceFade)))
        Canvas.DrawGradient(New Rectangle(300, 490, 50, 32), New Color(0, 0, 0, CInt(70 * _interfaceFade)), New Color(0, 0, 0, 0), True, -1)
        Canvas.DrawRectangle(New Rectangle(50, 522, 250, 32), New Color(0, 0, 0, CInt(100 * _interfaceFade)))
        Canvas.DrawGradient(New Rectangle(300, 522, 50, 32), New Color(0, 0, 0, CInt(100 * _interfaceFade)), New Color(0, 0, 0, 0), True, -1)
        Canvas.DrawRectangle(New Rectangle(50, 554, 250, 32), New Color(0, 0, 0, CInt(70 * _interfaceFade)))
        Canvas.DrawGradient(New Rectangle(300, 554, 50, 32), New Color(0, 0, 0, CInt(70 * _interfaceFade)), New Color(0, 0, 0, 0), True, -1)
        Canvas.DrawRectangle(New Rectangle(50, 586, 250, 32), New Color(0, 0, 0, CInt(100 * _interfaceFade)))
        Canvas.DrawGradient(New Rectangle(300, 586, 50, 32), New Color(0, 0, 0, CInt(100 * _interfaceFade)), New Color(0, 0, 0, 0), True, -1)

        SpriteBatch.DrawString(FontManager.ChatFont, "Type", New Vector2(60, 462), New Color(255, 255, 255, CInt(220 * _interfaceFade)))
        SpriteBatch.DrawString(FontManager.ChatFont, "Item", New Vector2(60, 494), New Color(255, 255, 255, CInt(220 * _interfaceFade)))
        SpriteBatch.DrawString(FontManager.ChatFont, "Dex No.", New Vector2(60, 526), New Color(255, 255, 255, CInt(220 * _interfaceFade)))

        SpriteBatch.DrawString(FontManager.ChatFont, "OT", New Vector2(60, 558), New Color(255, 255, 255, CInt(220 * _interfaceFade)))
        SpriteBatch.DrawString(FontManager.ChatFont, GetPokemon().CatchTrainerName, New Vector2(144, 558), New Color(255, 255, 255, CInt(220 * _fadeIn)))

        SpriteBatch.DrawString(FontManager.ChatFont, "ID No.", New Vector2(60, 590), New Color(255, 255, 255, CInt(220 * _interfaceFade)))
        SpriteBatch.DrawString(FontManager.ChatFont, GetPokemon().OT, New Vector2(144, 590), New Color(255, 255, 255, CInt(220 * _fadeIn)))

        Dim pokedexNo As String = "???"

        If GetPokemon().IsEgg() = False Then
            'Type images draw:
            Core.SpriteBatch.Draw(TextureManager.GetTexture("GUI\Menus\Types"), New Rectangle(144, 466, 48, 16), GetPokemon().Type1.GetElementImage(), New Color(255, 255, 255, CInt(255 * _fadeIn)))
            If GetPokemon().Type2.Type <> Element.Types.Blank Then
                Core.SpriteBatch.Draw(TextureManager.GetTexture("GUI\Menus\Types"), New Rectangle(202, 466, 48, 16), GetPokemon().Type2.GetElementImage(), New Color(255, 255, 255, CInt(255 * _fadeIn)))
            End If
            'Item:
            If GetPokemon().Item IsNot Nothing Then
                SpriteBatch.Draw(GetPokemon().Item.Texture, New Rectangle(144, 494, 24, 24), New Color(255, 255, 255, CInt(220 * _fadeIn)))
                SpriteBatch.DrawString(FontManager.ChatFont, GetPokemon().Item.Name, New Vector2(170, 494), New Color(255, 255, 255, CInt(220 * _fadeIn)))
            Else
                SpriteBatch.DrawString(FontManager.ChatFont, "None", New Vector2(144, 494), New Color(255, 255, 255, CInt(220 * _fadeIn)))
            End If

            'Get dex no:
            For Each pokedex In Core.Player.Pokedexes
                If pokedex.IsActivated = True Then
                    If pokedex.HasPokemon(GetPokemon().Number, True) Then
                        pokedexNo = pokedex.GetPlace(GetPokemon().Number).ToString()
                    End If
                End If
            Next
            While pokedexNo.Length < 3
                pokedexNo = "0" & pokedexNo
            End While
        Else
            SpriteBatch.DrawString(FontManager.ChatFont, "???", New Vector2(144, 462), New Color(255, 255, 255, CInt(220 * _fadeIn)))
            SpriteBatch.DrawString(FontManager.ChatFont, "???", New Vector2(144, 494), New Color(255, 255, 255, CInt(220 * _fadeIn)))
        End If

        'Pokedex no.
        SpriteBatch.DrawString(FontManager.ChatFont, pokedexNo, New Vector2(144, 526), New Color(255, 255, 255, CInt(220 * _fadeIn)))
    End Sub

    Private Sub DrawPage1()
        With GetPokemon()
            'Draw stats:
            Dim colors As Color() = {New Color(120, 239, 155), New Color(241, 227, 154), New Color(255, 178, 114), New Color(151, 217, 205), New Color(137, 154, 255), New Color(213, 128, 255)}
            Dim statNames As String() = {"HP", "Attack", "Defense", "Sp. Atk", "Sp. Def", "Speed"}
            Dim statValues As String() = { .HP & " / " & .MaxHP, CStr(.Attack), CStr(.Defense), CStr(.SpAttack), CStr(.SpDefense), CStr(.Speed)}
            Dim evStats As Single() = { .EVHP, .EVAttack, .EVDefense, .EVSpAttack, .EVSpDefense, .EVSpeed}
            Dim ivStats As Single() = { .IVHP, .IVAttack, .IVDefense, .IVSpAttack, .IVSpDefense, .IVSpeed}

            For y = 0 To 5
                Dim fadeColor As Integer = 100
                If y Mod 2 = 1 Then
                    fadeColor = 70
                End If

                Dim statColor As Color = colors(y)
                statColor.A = CByte(255 * _interfaceFade * _pageFade)

                Dim yOffset As Integer = 32
                Dim height As Integer = 32
                If y = 0 Then
                    yOffset = 0
                    height = 64
                End If

                Canvas.DrawRectangle(New Rectangle(450, 172 + y * 32 + yOffset, 250, height), New Color(0, 0, 0, CInt(fadeColor * _interfaceFade * _pageFade)))
                Canvas.DrawRectangle(New Rectangle(450, 172 + y * 32 + yOffset, 6, height), statColor)
                SpriteBatch.DrawString(FontManager.ChatFont, statNames(y), New Vector2(466, 176 + y * 32 + yOffset), New Color(255, 255, 255, CInt(220 * _interfaceFade * _pageFade)))

                Dim natureStatMulti As Single = Nature.GetMultiplier(.Nature, statNames(y))
                Dim multiColor As Color = New Color(255, 255, 255, CInt(200 * _fadeIn * _pageFade))

                If natureStatMulti > 1.0F Then
                    multiColor = New Color(255, 180, 180, CInt(200 * _fadeIn * _pageFade))
                ElseIf natureStatMulti < 1.0F Then
                    multiColor = New Color(180, 180, 255, CInt(200 * _fadeIn * _pageFade))
                End If
                SpriteBatch.DrawString(FontManager.ChatFont, statValues(y), New Vector2(580, 176 + y * 32 + yOffset), multiColor)
            Next

            Dim pokeInfoTexture = TextureManager.GetTexture("GUI\Menus\PokemonInfo")

            'HP Bar:
            SpriteBatch.Draw(pokeInfoTexture, New Rectangle(555, 210, 135, 15), New Rectangle(0, 32, 90, 10), New Color(255, 255, 255, CInt(220 * _interfaceFade * _pageFade)))
            '108 pixels:
            Dim hpV As Double = .HP / .MaxHP
            Dim hpWidth As Integer = CInt((104 * _fadeIn) * hpV)
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
                Dim drawColor As Color = Color.White
                If _closing = True Then
                    drawColor = New Color(255, 255, 255, CInt(220 * _fadeIn))
                End If
                drawColor.A = CByte(drawColor.A * _pageFade)

                SpriteBatch.Draw(pokeInfoTexture, New Rectangle(555 + 24, 213, 2, 8), New Rectangle(hpColorX, 42, 2, 6), drawColor)

                SpriteBatch.Draw(pokeInfoTexture, New Rectangle(555 + 24 + 2, 213, hpWidth, 8), New Rectangle(hpColorX + 2, 42, 1, 6), drawColor)

                SpriteBatch.Draw(pokeInfoTexture, New Rectangle(555 + 24 + 2 + hpWidth, 213, 2, 8), New Rectangle(hpColorX + 3, 42, 2, 6), drawColor)
            End If

            'Draw ability:
            Canvas.DrawRectangle(New Rectangle(450, 458, 350, 32), New Color(0, 0, 0, CInt(100 * _interfaceFade * _pageFade)))
            Canvas.DrawRectangle(New Rectangle(450, 490, 350, 64), New Color(0, 0, 0, CInt(70 * _interfaceFade * _pageFade)))

            SpriteBatch.DrawString(FontManager.ChatFont, "Ability", New Vector2(460, 462), New Color(255, 255, 255, CInt(220 * _interfaceFade * _pageFade)))
            SpriteBatch.DrawString(FontManager.ChatFont, .Ability.Name, New Vector2(570, 462), New Color(255, 255, 255, CInt(220 * _fadeIn * _pageFade)))
            SpriteBatch.DrawString(FontManager.ChatFont, .Ability.Description.CropStringToWidth(FontManager.ChatFont, 1.0F, 300), New Vector2(460, 496), New Color(255, 255, 255, CInt(220 * _fadeIn * _pageFade)))

            'Draw nature:
            Canvas.DrawRectangle(New Rectangle(450, 586, 350, 32), New Color(0, 0, 0, CInt(100 * _interfaceFade * _pageFade)))
            SpriteBatch.DrawString(FontManager.ChatFont, "Nature", New Vector2(460, 590), New Color(255, 255, 255, CInt(220 * _interfaceFade * _pageFade)))
            SpriteBatch.DrawString(FontManager.ChatFont, .Nature.ToString(), New Vector2(570, 590), New Color(255, 255, 255, CInt(220 * _fadeIn * _pageFade)))

            'EXP:
            Canvas.DrawRectangle(New Rectangle(816, 458, 300, 32), New Color(0, 0, 0, CInt(100 * _interfaceFade * _pageFade)))
            SpriteBatch.DrawString(FontManager.ChatFont, "Exp. Points", New Vector2(826, 462), New Color(255, 255, 255, CInt(220 * _interfaceFade * _pageFade)))
            SpriteBatch.DrawString(FontManager.ChatFont, .Experience.ToString(), New Vector2(980, 462), New Color(255, 255, 255, CInt(220 * _fadeIn * _pageFade)))

            If .Level < CInt(GameModeManager.GetGameRuleValue("MaxLevel", "100")) Then
                Canvas.DrawRectangle(New Rectangle(816, 490, 300, 64), New Color(0, 0, 0, CInt(70 * _interfaceFade * _pageFade)))
                SpriteBatch.DrawString(FontManager.ChatFont, "To Next Lv.", New Vector2(826, 494), New Color(255, 255, 255, CInt(220 * _interfaceFade * _pageFade)))

                If .NeedExperience(.Level + 1) - .Experience > 0 Then
                    SpriteBatch.DrawString(FontManager.ChatFont, CStr(.NeedExperience(.Level + 1) - .Experience), New Vector2(980, 494), New Color(255, 255, 255, CInt(220 * _fadeIn * _pageFade)))

                    'EXP Bar:
                    Dim expV As Double = (.Experience - .NeedExperience(.Level)) / (.NeedExperience(.Level + 1) - .NeedExperience(.Level))
                    If .Level = 1 Then
                        expV = .Experience / .NeedExperience(2)
                    End If
                    Dim expWidth As Integer = CInt((107 * _fadeIn) * expV)
                    If .Experience > .NeedExperience(.Level) And expWidth = 0 Then
                        expWidth = 1
                    End If
                    If .Experience > .NeedExperience(.Level + 1) Then
                        expWidth = 107
                        expV = 1.0F
                    End If

                    Dim expLow As New Color(47, 204, 208, CInt(220 * _interfaceFade * _pageFade))
                    Dim expToColor As New Color(CInt(MathHelper.Lerp(47, 7, CSng(expV))), CInt(MathHelper.Lerp(204, 48, CSng(expV))), CInt(MathHelper.Lerp(208, 216, CSng(expV))), CInt(220 * _interfaceFade * _pageFade))

                    SpriteBatch.Draw(pokeInfoTexture, New Rectangle(796 + 96, 507 + 24, 141, 12), New Rectangle(0, 48, 94, 8), New Color(255, 255, 255, CInt(220 * _interfaceFade * _pageFade)))
                    If expWidth > 0 Then
                        Canvas.DrawGradient(New Rectangle(827 + 96, 510 + 24, expWidth, 6), expLow, expToColor, True, -1)
                    End If
                End If
            End If

            'Draw EV/IV stats:
            'Base diagramm:
            Canvas.DrawRectangle(New Rectangle(816, 172, 298, 224), New Color(0, 0, 0, CInt(70 * _interfaceFade * _pageFade)))

            Canvas.DrawLine(New Color(0, 0, 0, CInt(100 * _interfaceFade * _pageFade)), New Vector2(816, 172), New Vector2(816, 396), 2D)
            Canvas.DrawLine(New Color(0, 0, 0, CInt(100 * _interfaceFade * _pageFade)), New Vector2(1116, 172), New Vector2(1116, 396), 2D)
            Canvas.DrawLine(New Color(0, 0, 0, CInt(100 * _interfaceFade * _pageFade)), New Vector2(814, 396), New Vector2(1116, 396), 2D)

            For i = 0 To 5
                'Axis:
                Dim c As Color = colors(i)

                Canvas.DrawLine(New Color(c.R, c.G, c.B, CInt(220 * _interfaceFade * _pageFade)), New Vector2(823 + i * 56, 396), New Vector2(823 + i * 56, 396 - (224 * _fadeIn)), 3D)

                If i < 5 Then
                    Dim EVcurrentPointM As Double = evStats(i) / 256
                    Dim EVnextPointM As Double = evStats(i + 1) / 256

                    Canvas.DrawLine(New Color(255, 255, 255, CInt(255 * _interfaceFade * _pageFade)), New Vector2(824 + i * 56, 396 - CSng((224 * _fadeIn) * EVcurrentPointM)), New Vector2(824 + (i + 1) * 56, 396 - CSng((224 * _fadeIn) * EVnextPointM)), 2D)

                    Dim IVcurrentPointM As Double = ivStats(i) / 31
                    Dim IVnextPointM As Double = ivStats(i + 1) / 31

                    Canvas.DrawLine(New Color(84, 198, 216, CInt(255 * _interfaceFade * _pageFade)), New Vector2(824 + i * 56, 396 - CSng((224 * _fadeIn) * IVcurrentPointM)), New Vector2(824 + (i + 1) * 56, 396 - CSng((224 * _fadeIn) * IVnextPointM)), 2D)
                End If
            Next
        End With
    End Sub

    Private Sub DrawPage2()
        With GetPokemon()
            'Draw moves:
            For i = 0 To 3
                Dim pos As New Vector2(450, 172 + i * 96)
                Dim c As Color = New Color(255, 255, 255, CInt(255 * _interfaceFade * _pageFade))

                Core.SpriteBatch.Draw(_texture, New Rectangle(CInt(pos.X), CInt(pos.Y), 64, 64), New Rectangle(16, 16, 16, 16), c)
                Core.SpriteBatch.Draw(_texture, New Rectangle(CInt(pos.X) + 64, CInt(pos.Y), 64 * 3, 64), New Rectangle(32, 16, 16, 16), c)
                Core.SpriteBatch.Draw(_texture, New Rectangle(CInt(pos.X) + 64 * 4, CInt(pos.Y), 64, 64), New Rectangle(16, 16, 16, 16), c, 0.0F, Vector2.Zero, SpriteEffects.FlipHorizontally, 0.0F)

                If .Attacks.Count - 1 >= i Then
                    SpriteBatch.DrawString(FontManager.ChatFont, .Attacks(i).Name, New Vector2(pos.X + 24, pos.Y + 8), New Color(0, 0, 0, CInt(220 * _fadeIn * _pageFade)))
                    Core.SpriteBatch.Draw(TextureManager.GetTexture("GUI\Menus\Types"), New Rectangle(CInt(pos.X + 26), CInt(pos.Y + 36), 48, 16), .Attacks(i).Type.GetElementImage(), New Color(255, 255, 255, CInt(255 * _fadeIn * _pageFade)))
                    SpriteBatch.DrawString(FontManager.ChatFont, "PP " & .Attacks(i).CurrentPP & " / " & .Attacks(i).MaxPP, New Vector2(pos.X + 130, pos.Y + 32), New Color(0, 0, 0, CInt(220 * _fadeIn * _pageFade)))
                End If
            Next

            '*******MOVE SELECTED SECTION*******

            'Draw move selector:
            Canvas.DrawBorder(3, New Rectangle(450, CInt(_moveSelectorPosition), 64 * 5, 64), New Color(200, 80, 80, CInt(200 * _interfaceFade * _pageFade * _moveFade)))

            'Draw move switch selector:
            If _switchingMoves = True Then
                Canvas.DrawBorder(3, New Rectangle(447, CInt(GetMoveSelectorDest(_switchMoveIndex) - 3), 64 * 5 + 6, 70), New Color(80, 80, 200, CInt(200 * _interfaceFade * _pageFade * _moveFade)))
            End If

            'Draw selected move info:
            Canvas.DrawRectangle(New Rectangle(800, 172, 350, 32), New Color(0, 0, 0, CInt(100 * _interfaceFade * _pageFade * _moveFade)))
            Canvas.DrawRectangle(New Rectangle(800, 204, 350, 32), New Color(0, 0, 0, CInt(70 * _interfaceFade * _pageFade * _moveFade)))
            Canvas.DrawRectangle(New Rectangle(800, 236, 350, 32), New Color(0, 0, 0, CInt(100 * _interfaceFade * _pageFade * _moveFade)))
            Canvas.DrawRectangle(New Rectangle(800, 268, 350, 32), New Color(0, 0, 0, CInt(70 * _interfaceFade * _pageFade * _moveFade)))
            Canvas.DrawRectangle(New Rectangle(800, 300, 350, 32), New Color(0, 0, 0, CInt(100 * _interfaceFade * _pageFade * _moveFade)))
            Canvas.DrawRectangle(New Rectangle(800, 332, 350, 128), New Color(0, 0, 0, CInt(70 * _interfaceFade * _pageFade * _moveFade)))

            'Type:
            SpriteBatch.DrawString(FontManager.ChatFont, "Type:", New Vector2(810, 176), New Color(255, 255, 255, CInt(220 * _interfaceFade * _pageFade * _moveFade)))
            Core.SpriteBatch.Draw(TextureManager.GetTexture("GUI\Menus\Types"), New Rectangle(924, 182, 48, 16), .Attacks(_moveIndex).Type.GetElementImage(), New Color(255, 255, 255, CInt(255 * _fadeIn * _pageFade * _moveFade)))

            SpriteBatch.DrawString(FontManager.ChatFont, "PP:", New Vector2(810, 208), New Color(255, 255, 255, CInt(220 * _interfaceFade * _pageFade * _moveFade)))
            SpriteBatch.DrawString(FontManager.ChatFont, .Attacks(_moveIndex).CurrentPP & " / " & .Attacks(_moveIndex).MaxPP, New Vector2(924, 208), New Color(255, 255, 255, CInt(220 * _fadeIn * _pageFade * _moveFade)))

            'Stats:
            SpriteBatch.DrawString(FontManager.ChatFont, "Category:", New Vector2(810, 240), New Color(255, 255, 255, CInt(220 * _interfaceFade * _pageFade * _moveFade)))
            Core.SpriteBatch.Draw(.Attacks(_moveIndex).GetDamageCategoryImage(), New Rectangle(924, 241, 48, 24), New Color(255, 255, 255, CInt(255 * _fadeIn * _pageFade * _moveFade)))

            SpriteBatch.DrawString(FontManager.ChatFont, "Power:", New Vector2(810, 272), New Color(255, 255, 255, CInt(220 * _interfaceFade * _pageFade * _moveFade)))

            Dim power As String = .Attacks(_moveIndex).Power.ToString()
            If .Attacks(_moveIndex).Power <= 0 Then
                power = "-"
            End If

            SpriteBatch.DrawString(FontManager.ChatFont, power, New Vector2(924, 272), New Color(255, 255, 255, CInt(220 * _fadeIn * _pageFade * _moveFade)))

            Dim accuracy As String = .Attacks(_moveIndex).Accuracy.ToString()
            If .Attacks(_moveIndex).Accuracy <= 0 Then
                accuracy = "-"
            End If

            SpriteBatch.DrawString(FontManager.ChatFont, "Accuracy:", New Vector2(810, 304), New Color(255, 255, 255, CInt(220 * _interfaceFade * _pageFade * _moveFade)))
            SpriteBatch.DrawString(FontManager.ChatFont, accuracy, New Vector2(924, 304), New Color(255, 255, 255, CInt(220 * _fadeIn * _pageFade * _moveFade)))

            'Description:
            SpriteBatch.DrawString(FontManager.ChatFont, .Attacks(_moveIndex).Description.CropStringToWidth(FontManager.ChatFont, 300), New Vector2(820, 336), New Color(255, 255, 255, CInt(220 * _fadeIn * _pageFade * _moveFade)))
        End With
    End Sub

    Private Sub DrawEgg()
        Dim s As String = ""
        Dim percent As Integer = CInt((GetPokemon().EggSteps / GetPokemon().BaseEggSteps) * 100)
        If percent <= 33 Then
            s = "It looks like this Egg will" & Environment.NewLine & "take a long time to hatch."
        ElseIf percent > 33 And percent <= 66 Then
            s = "It's getting warmer and moves" & Environment.NewLine & "a little. It will hatch soon."
        Else
            s = "There is strong movement" & Environment.NewLine & "noticeable. It will hatch soon!"
        End If

        Canvas.DrawRectangle(New Rectangle(450, 172, 350, 32), New Color(0, 0, 0, CInt(100 * _fadeIn)))
        Canvas.DrawRectangle(New Rectangle(450, 204, 350, 96), New Color(0, 0, 0, CInt(70 * _fadeIn)))

        SpriteBatch.DrawString(FontManager.ChatFont, "The Egg Watch", New Vector2(460, 176), New Color(255, 255, 255, CInt(220 * _fadeIn)))
        SpriteBatch.DrawString(FontManager.ChatFont, s, New Vector2(460, 228), New Color(255, 255, 255, CInt(220 * _fadeIn)))
    End Sub

    Private Sub GetYOffset()
        Dim t As Texture2D = GetPokemon().GetTexture(True)
        _yOffset = -1

        Dim cArr(t.Width * t.Height - 1) As Color
        t.GetData(cArr)

        For y = 0 To t.Height - 1
            For x = 0 To t.Width - 1
                If cArr(x + y * t.Height) <> Color.Transparent Then
                    _yOffset = y
                    Exit For
                End If
            Next

            If _yOffset <> -1 Then
                Exit For
            End If
        Next
    End Sub

    Public Overrides Sub Update()
        If _closing = True Then
            If _fadeIn > 0F Then
                _fadeIn = MathHelper.Lerp(0, _fadeIn, 0.8F)
                If _fadeIn < 0F Then
                    _fadeIn = 0F
                End If
            End If
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
                Core.SetScreen(PreScreen)
            End If
        Else
            Dim enrollYDest As Integer = Core.windowSize.Height - 146
            If _enrollY < enrollYDest Then
                _enrollY = MathHelper.Lerp(enrollYDest, _enrollY, 0.8F)
                If _enrollY >= enrollYDest Then
                    _enrollY = enrollYDest
                End If
            End If
            If _fadeIn < 1.0F Then
                _fadeIn = MathHelper.Lerp(1.0F, _fadeIn, 0.95F)
                If _fadeIn > 1.0F Then
                    _fadeIn = 1.0F
                End If
            End If
            If _interfaceFade < 1.0F Then
                _interfaceFade = MathHelper.Lerp(1.0F, _interfaceFade, 0.95F)
                If _interfaceFade > 1.0F Then
                    _interfaceFade = 1.0F
                End If
            End If
            If _pixelFade < 1.0F Then
                _pixelFade += 0.03F
                If _pixelFade >= 1.0F Then
                    _pixelFade = 1.0F
                End If
            End If

            If _pageOpening = False And _pageClosing = False And _partyIndex > -1 And _moveSelected = False Then
                If _party.Length > 1 Then
                    If Controls.Left(True, True, False, True, False, True) Or ControllerHandler.ButtonPressed(Buttons.LeftShoulder) Then
                        If _partyIndex > 0 Then
                            _partyIndex -= 1
                            GetYOffset()
                            SetDest(_partyIndex)
                            _isFront = True
                        End If

                    End If
                    If Controls.Right(True, True, False, True, False, True) Or ControllerHandler.ButtonPressed(Buttons.RightShoulder) Then
                        If _partyIndex < _party.Count - 1 Then
                            _partyIndex += 1
                            GetYOffset()
                            SetDest(_partyIndex)
                            _isFront = True
                        End If

                    End If
                End If
            End If
            If _moveSelected = False Then
                If GetPokemon().IsEgg() = False Then
                    If Controls.Down(True, True, False, True, True, True) = True Then
                        If _pageIndex = 0 Then
                            _pageClosing = True
                            _pageOpening = False
                        End If
                    End If
                    If Controls.Up(True, True, False, True, True, True) = True Then
                        If _pageIndex = 1 Then
                            _pageClosing = True
                            _pageOpening = False
                        End If
                    End If
                    If Controls.Accept() = True Then
                        If _pageIndex = 0 Then
                            _isFront = Not _isFront
                        ElseIf _pageIndex = 1 Then
                            _moveSelected = True
                        End If
                    End If
                End If
                If Controls.Dismiss() = True Then
                    _closing = True
                End If
            Else
                If Math.Abs(_moveSelectorPosition - GetMoveSelectorDest(_moveIndex)) < 32.0F Then
                    If Controls.Down(True, True, True, True, True, True) = True Then
                        _moveIndex += 1

                        If _moveIndex > GetPokemon().Attacks.Count - 1 Then
                            _moveIndex = 0
                        End If
                    End If
                    If Controls.Up(True, True, True, True, True, True) = True Then
                        _moveIndex -= 1

                        If _moveIndex < 0 Then
                            _moveIndex = GetPokemon().Attacks.Count - 1
                        End If
                    End If
                End If
                If Controls.Accept() = True And _pageIndex = 1 And GetPokemon().IsEgg() = False Then
                    If _switchingMoves = True Then
                        Dim switchingMove As BattleSystem.Attack = GetPokemon().Attacks(_switchMoveIndex)
                        GetPokemon().Attacks.RemoveAt(_switchMoveIndex)
                        GetPokemon().Attacks.Insert(_moveIndex, switchingMove)

                        _switchingMoves = False
                        _switchMoveIndex = -1
                    Else
                        If GetPokemon().Attacks.Count > 1 Then
                            _switchingMoves = True
                            _switchMoveIndex = _moveIndex
                        End If
                    End If
                End If
                If Controls.Dismiss() = True Then
                    If _switchingMoves = True Then
                        _switchingMoves = False
                        _switchMoveIndex = -1
                    Else
                        _moveSelected = False
                    End If
                End If
            End If
        End If

        _moveIndex = _moveIndex.Clamp(0, GetPokemon().Attacks.Count - 1)
        If _moveSelectorPosition <> GetMoveSelectorDest(_moveIndex) Then
            _moveSelectorPosition = MathHelper.Lerp(GetMoveSelectorDest(_moveIndex), _moveSelectorPosition, 0.8F)
            If Math.Abs(_moveSelectorPosition - GetMoveSelectorDest(_moveIndex)) < 0.05F Then
                _moveSelectorPosition = GetMoveSelectorDest(_moveIndex)
            End If
        End If

        If _pageClosing = True Then
            If _pageFade >= 0F Then
                _pageFade -= 0.07F
                If _pageFade <= 0F Then
                    _pageFade = 0F
                    _pageClosing = False
                    _pageOpening = True
                    If _pageIndex = 0 Then
                        _pageIndex = 1
                    Else
                        _pageIndex = 0
                    End If
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

        If _pageIndex = 0 Then
            _moveSelected = False
            _moveIndex = 0
            _moveFade = 0F
            _switchingMoves = False
            _switchMoveIndex = -1
        Else
            If _moveSelected = True And _moveFade <= 1.0F Then
                _moveFade = MathHelper.Lerp(1.0F, _moveFade, 0.8F)
                If _moveFade >= 0.95F Then
                    _moveFade = 1.0F
                End If
            ElseIf _moveSelected = False And _moveFade > 0F Then
                _moveFade = MathHelper.Lerp(0.0F, _moveFade, 0.8F)
                If _moveFade <= 0.05F Then
                    _moveFade = 0F
                End If
            End If
        End If

        If _pointerPos < _pointerDest Then
            _pointerPos = MathHelper.Lerp(_pointerDest, _pointerPos, 0.8F)
            If _pointerPos >= _pointerDest Then
                _pointerPos = _pointerDest
            End If
        ElseIf _pointerPos > _pointerDest Then
            _pointerPos = MathHelper.Lerp(_pointerDest, _pointerPos, 0.8F)
            If _pointerPos <= _pointerDest Then
                _pointerPos = _pointerDest
            End If
        End If
        If _pokemonPos < _pokemonDest Then
            _pokemonPos = MathHelper.Lerp(_pokemonDest, _pokemonPos, 0.8F)
            If _pokemonPos >= _pokemonDest Then
                _pokemonPos = _pokemonDest
            End If
        ElseIf _pokemonPos > _pokemonDest Then
            _pokemonPos = MathHelper.Lerp(_pokemonDest, _pokemonPos, 0.8F)
            If _pokemonPos <= _pokemonDest Then
                _pokemonPos = _pokemonDest
            End If
        End If
    End Sub

    Private Function GetDest(ByVal partyIndex As Integer) As Integer
        Dim pointerX As Double = Core.ScreenSize.Width / 2
        If _party.Length Mod 2 = 0 Then
            'Even:
            Dim half As Integer = CInt(_party.Length / 2)
            pointerX += (8 + (64 + 16) * (partyIndex - half))
        Else
            'Odd:
            Dim half As Integer = CInt(Math.Floor(_party.Length / 2))
            pointerX += (64 + 16) * (partyIndex - half)
        End If

        Return CInt(pointerX)
    End Function

    Private Function GetPointerDest(ByVal partyIndex As Integer) As Integer
        Dim pointerX As Double = GetDest(partyIndex)
        _previewerOffset = 0
        While pointerX > Core.ScreenSize.Width - (64 + 16)
            pointerX -= (64 + 16)
            _previewerOffset -= 1
        End While
        While pointerX < (64)
            pointerX += (64 + 16)
            _previewerOffset += 1
        End While

        Return CInt(pointerX)
    End Function

    Private Function GetPokemonDest(ByVal partyIndex As Integer) As Integer
        Dim pointerX As Double = GetDest(partyIndex)
        pointerX += (_previewerOffset * (64 + 16))
        Return CInt(pointerX)
    End Function

    Private Sub SetDest(ByVal partyIndex As Integer)
        _fadeIn = 0F
        _pixelFade = 0F
        _pointerDest = GetPointerDest(partyIndex)
        _pokemonDest = _previewerOffset
        If GetPokemon().IsEgg() = False Then
            GetPokemon().PlayCry()
        End If
    End Sub

    Private Function GetPokemon() As Pokemon
        If _partyIndex > -1 Then
            Return _party(_partyIndex)
        Else
            Return _selectedPokemon
        End If
    End Function

End Class