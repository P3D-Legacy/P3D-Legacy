﻿Public Class PokedexSelectScreen

    Inherits Screen

    Dim texture As Texture2D
    Dim Profiles As New List(Of PokedexProfile)
    Dim Cursor As Integer = 0

    Public Sub New(ByVal currentScreen As Screen)
        Me.Identification = Identifications.PokedexScreen

        Me.PreScreen = currentScreen
        Me.texture = TextureManager.GetTexture("GUI\Menus\General")

        Me.MouseVisible = True
        Me.CanMuteAudio = True
        Me.CanBePaused = True

        For Each p As Pokedex In Core.Player.Pokedexes
            If p.IsActivated = True Then
                Me.Profiles.Add(New PokedexProfile With {.Pokedex = p, .Obtained = p.Obtained, .Seen = p.Seen})
            End If
        Next

        Me.AchievePokedexEmblems()
    End Sub

    Private Sub AchievePokedexEmblems()
        ' Eevee:
        Dim eevee() As Integer = {134, 135, 136, 196, 197, 470, 471, 700}
        Dim hasEevee As Boolean = True
        For Each e As Integer In eevee
            If Pokedex.GetEntryType(Core.Player.PokedexData, e.ToString) < 2 Then
                hasEevee = False
                Exit For
            End If
        Next
        If hasEevee = True Then
            GameJolt.Emblem.AchieveEmblem("eevee")
        End If

        ' Pokédex:
        If Core.Player.IsGameJoltSave = True Then
            If Me.Profiles(0).Pokedex.Obtained >= Me.Profiles(0).Pokedex.Count Then
                GameJolt.Emblem.AchieveEmblem("pokedex")
            End If
        End If
    End Sub

    Structure PokedexProfile
        Dim Obtained As Integer
        Dim Seen As Integer
        Dim Pokedex As Pokedex
    End Structure

    Public Overrides Sub Draw()
        Canvas.DrawRectangle(Core.windowSize, New Color(84, 198, 216))

        For y = -64 To Core.windowSize.Height Step 64
            Core.SpriteBatch.Draw(Me.texture, New Rectangle(Core.windowSize.Width - 128, y + PokedexScreen.TileOffset, 128, 64), New Rectangle(48, 0, 16, 16), Color.White)
        Next

        Canvas.DrawGradient(New Rectangle(0, 0, CInt(Core.windowSize.Width), 200), New Color(42, 167, 198), New Color(42, 167, 198, 0), False, -1)
        Canvas.DrawGradient(New Rectangle(0, CInt(Core.windowSize.Height - 200), CInt(Core.windowSize.Width), 200), New Color(42, 167, 198, 0), New Color(42, 167, 198), False, -1)

        Core.SpriteBatch.DrawString(FontManager.MainFont, Localization.GetString("pokedex_select", "Select a Pokédex"), New Vector2(100, 24), Color.White, 0.0F, Vector2.Zero, 2.0F, SpriteEffects.None, 0.0F)

        For i = 0 To Me.Profiles.Count
            If i = Me.Profiles.Count Then
                Core.SpriteBatch.Draw(Me.texture, New Rectangle(100, 100 + i * 96, 64, 64), New Rectangle(16, 16, 16, 16), Color.White)
                Core.SpriteBatch.Draw(Me.texture, New Rectangle(100 + 64, 100 + i * 96, 64 * 5, 64), New Rectangle(32, 16, 16, 16), Color.White)
                Core.SpriteBatch.Draw(Me.texture, New Rectangle(100 + 64 * 6, 100 + i * 96, 64, 64), New Rectangle(16, 16, 16, 16), Color.White, 0.0F, Vector2.Zero, SpriteEffects.FlipHorizontally, 0.0F)

                Core.SpriteBatch.DrawString(FontManager.MainFont, Localization.GetString("pokedex_habitat_dex_name", "Habitat-Dex"), New Vector2(120, 120 + i * 96), Color.Black, 0.0F, Vector2.Zero, 1.0F, SpriteEffects.None, 0.0F)
            Else
                Dim p As Pokedex = Me.Profiles(i).Pokedex

                Core.SpriteBatch.Draw(Me.texture, New Rectangle(100, 100 + i * 96, 64, 64), New Rectangle(16, 16, 16, 16), Color.White)
                Core.SpriteBatch.Draw(Me.texture, New Rectangle(100 + 64, 100 + i * 96, 64 * 5, 64), New Rectangle(32, 16, 16, 16), Color.White)
                Core.SpriteBatch.Draw(Me.texture, New Rectangle(100 + 64 * 6, 100 + i * 96, 64, 64), New Rectangle(16, 16, 16, 16), Color.White, 0.0F, Vector2.Zero, SpriteEffects.FlipHorizontally, 0.0F)

                Core.SpriteBatch.DrawString(FontManager.MainFont, p.Name, New Vector2(120, 120 + i * 96), Color.Black, 0.0F, Vector2.Zero, 1.0F, SpriteEffects.None, 0.0F)
                Core.SpriteBatch.DrawString(FontManager.MainFont, Me.Profiles(i).Obtained.ToString(), New Vector2(460, 120 + i * 96), Color.Black, 0.0F, Vector2.Zero, 1.0F, SpriteEffects.None, 0.0F)

                If Me.Profiles(i).Obtained >= Me.Profiles(i).Pokedex.Count Then
                    Core.SpriteBatch.Draw(TextureManager.GetTexture("GUI\Menus\pokedexhabitat", New Rectangle(160, 160, 10, 10), ""), New Rectangle(430, 122 + i * 96, 20, 20), Color.White)
                Else
                    If Me.Profiles(i).Seen + Me.Profiles(i).Obtained >= Me.Profiles(i).Pokedex.Count Then
                        Core.SpriteBatch.Draw(TextureManager.GetTexture("GUI\Menus\pokedexhabitat", New Rectangle(160, 170, 10, 10), ""), New Rectangle(430, 122 + i * 96, 20, 20), Color.White)
                    End If
                End If
            End If
        Next

        DrawCursor()
    End Sub

    Private Sub DrawCursor()
        Dim cPosition As Vector2 = New Vector2(512, 96 + Me.Cursor * 96 - 40)
        Dim t As Texture2D = TextureManager.GetTexture("GUI\Menus\General", New Rectangle(0, 0, 16, 16), "")
        Core.SpriteBatch.Draw(t, New Rectangle(CInt(cPosition.X), CInt(cPosition.Y), 64, 64), Color.White)
    End Sub

    Public Overrides Sub Update()
        If Controls.Up(True, True, True, True, True, True) = True Then
            Me.Cursor -= 1
            If Controls.ShiftDown() = True Then
                Me.Cursor -= 4
            End If
        End If
        If Controls.Down(True, True, True, True, True, True) = True Then
            Me.Cursor += 1
            If Controls.ShiftDown() = True Then
                Me.Cursor += 4
            End If
        End If

        Me.Cursor = Me.Cursor.Clamp(0, Me.Profiles.Count)

        If Controls.Accept(True, False, False) = True Then
            For i = 0 To Me.Profiles.Count
                If New Rectangle(100, 100 + i * 96, 64 * 7, 64).Contains(MouseHandler.MousePosition) = True Then
                    If i = Cursor Then
                        If Me.Cursor = Me.Profiles.Count Then
                            SoundManager.PlaySound("select")
                            Core.SetScreen(New PokedexHabitatScreen(Me))
                        Else
                            SoundManager.PlaySound("select")
                            Core.SetScreen(New PokedexScreen(Me, Me.Profiles(Me.Cursor), Nothing))
                        End If
                    Else
                        Cursor = i
                    End If
                End If
            Next
        End If

        If Controls.Accept(False, True, True) = True Then
            If Me.Cursor = Me.Profiles.Count Then
                Core.SetScreen(New PokedexHabitatScreen(Me))
                SoundManager.PlaySound("select")
            Else
                Core.SetScreen(New PokedexScreen(Me, Me.Profiles(Me.Cursor), Nothing))
                SoundManager.PlaySound("select")
            End If
        End If

        If Controls.Dismiss(True, True, True) = True Then
            SoundManager.PlaySound("select")
            Core.SetScreen(New TransitionScreen(Core.CurrentScreen, Me.PreScreen, Color.White, False))
        End If

        PokedexScreen.TileOffset += 1
        If PokedexScreen.TileOffset >= 64 Then
            PokedexScreen.TileOffset = 0
        End If
    End Sub

End Class

Public Class PokedexHabitatScreen

    Inherits Screen

    Dim texture As Texture2D
    Dim HabitatList As New List(Of PokedexScreen.Habitat)
    Dim Cursor As Integer = 0
    Dim Scroll As Integer = 0

    Public Sub New(ByVal currentScreen As Screen)
        Me.Identification = Identifications.PokedexHabitatScreen

        Me.PreScreen = currentScreen
        Me.texture = TextureManager.GetTexture("GUI\Menus\General")

        Me.MouseVisible = True
        Me.CanMuteAudio = True
        Me.CanBePaused = True

        For Each file As String In System.IO.Directory.GetFiles(GameController.GamePath & GameModeManager.ActiveGameMode.PokeFilePath, "*.*", IO.SearchOption.AllDirectories)
            If file.EndsWith(".poke") = True Then
                Dim DexInclude As Boolean = True
                Dim data() As String = System.IO.File.ReadAllLines(file)
                For Each line As String In data
                    If line.ToLower().StartsWith("dexinclude=") = True Then
                        DexInclude = CBool(line.Remove(0, 11))
                        Exit For
                    End If
                Next

                If DexInclude = True Then
                    Dim fileName As String = file.Remove(0, (GameController.GamePath & GameModeManager.ActiveGameMode.PokeFilePath & "\").Length - 1)
                    Dim newHabitat As New PokedexScreen.Habitat(file)
                    Dim exists As Boolean = False
                    For Each h As PokedexScreen.Habitat In Me.HabitatList
                        If h.Name.ToLower() = newHabitat.Name.ToLower() Then
                            exists = True
                            h.Merge(newHabitat)
                            Exit For
                        End If
                    Next
                    If exists = False AndAlso Core.Player.PokeFiles.Contains(fileName) = True Then
                        HabitatList.Add(New PokedexScreen.Habitat(file))
                    End If
                End If
            End If
        Next
        Me.HabitatList = (From h As PokedexScreen.Habitat In Me.HabitatList Order By h.Name Ascending).ToList()
    End Sub

    Public Overrides Sub Draw()
        Canvas.DrawRectangle(Core.windowSize, New Color(84, 198, 216))

        For y = -64 To Core.windowSize.Height Step 64
            Core.SpriteBatch.Draw(Me.texture, New Rectangle(Core.windowSize.Width - 128, y + PokedexScreen.TileOffset, 128, 64), New Rectangle(48, 0, 16, 16), Color.White)
        Next

        Canvas.DrawGradient(New Rectangle(0, 0, CInt(Core.windowSize.Width), 200), New Color(42, 167, 198), New Color(42, 167, 198, 0), False, -1)
        Canvas.DrawGradient(New Rectangle(0, CInt(Core.windowSize.Height - 200), CInt(Core.windowSize.Width), 200), New Color(42, 167, 198, 0), New Color(42, 167, 198), False, -1)

        Core.SpriteBatch.DrawString(FontManager.MainFont, Localization.GetString("pokedex_habitat_select", "Select a Habitat"), New Vector2(100, 24), Color.White, 0.0F, Vector2.Zero, 2.0F, SpriteEffects.None, 0.0F)

        For i = Scroll To Scroll + 5
            If i <= Me.HabitatList.Count - 1 Then
                Dim p As Integer = i - Scroll

                Core.SpriteBatch.Draw(Me.texture, New Rectangle(100, 100 + p * 96, 64, 64), New Rectangle(16, 16, 16, 16), Color.White)
                Core.SpriteBatch.Draw(Me.texture, New Rectangle(100 + 64, 100 + p * 96, 64 * 8, 64), New Rectangle(32, 16, 16, 16), Color.White)
                Core.SpriteBatch.Draw(Me.texture, New Rectangle(100 + 64 * 9, 100 + p * 96, 64, 64), New Rectangle(16, 16, 16, 16), Color.White, 0.0F, Vector2.Zero, SpriteEffects.FlipHorizontally, 0.0F)

                Core.SpriteBatch.Draw(HabitatList(i).Texture, New Rectangle(120, 108 + p * 96, 64, 48), Color.White)
                Core.SpriteBatch.DrawString(FontManager.MainFont, HabitatList(i).Name, New Vector2(200, 120 + p * 96), Color.Black, 0.0F, Vector2.Zero, 1.0F, SpriteEffects.None, 0.0F)

                Dim t As String = HabitatList(i).PokemonCaught.ToString() & "/" & HabitatList(i).PokemonList.Count
                Core.SpriteBatch.DrawString(FontManager.MainFont, t, New Vector2(680 - CSng((FontManager.MainFont.MeasureString(t).X * 1.0F) / 2.0F), 120 + p * 96), Color.Black, 0.0F, Vector2.Zero, 1.0F, SpriteEffects.None, 0.0F)

                Dim progressTexture As Texture2D = Me.HabitatList(i).ProgressTexture
                If Not progressTexture Is Nothing Then
                    Core.SpriteBatch.Draw(progressTexture, New Rectangle(CInt(650 - CSng((FontManager.MainFont.MeasureString(t).X * 1.0F) / 2.0F)), 120 + p * 96, 20, 20), Color.White)
                End If
            End If
        Next

        DrawCursor()
    End Sub

    Private Sub DrawCursor()
        Dim cPosition As Vector2 = New Vector2(520, 100 + Me.Cursor * 96 - 42)

        Dim t As Texture2D = TextureManager.GetTexture("GUI\Menus\General", New Rectangle(0, 0, 16, 16), "")
        Core.SpriteBatch.Draw(t, New Rectangle(CInt(cPosition.X), CInt(cPosition.Y), 64, 64), Color.White)
    End Sub

    Public Overrides Sub Update()
        If Controls.Down(True, True, True, True, True, True) = True Then
            Me.Cursor += 1
            If Controls.ShiftDown() = True Then
                Me.Cursor += 4
            End If
        End If
        If Controls.Up(True, True, True, True, True, True) = True Then
            Me.Cursor -= 1
            If Controls.ShiftDown() = True Then
                Me.Cursor -= 4
            End If
        End If

        While Me.Cursor > 5
            Me.Cursor -= 1
            Me.Scroll += 1
        End While
        While Me.Cursor < 0
            Me.Cursor += 1
            Me.Scroll -= 1
        End While

        If Me.HabitatList.Count < 7 Then
            Me.Scroll = 0
        Else
            Me.Scroll = Me.Scroll.Clamp(0, Me.HabitatList.Count - 6)
        End If

        If Me.HabitatList.Count < 6 Then
            Me.Cursor = Me.Cursor.Clamp(0, Me.HabitatList.Count - 1)
        Else
            Me.Cursor = Me.Cursor.Clamp(0, 5)
        End If

        If Me.HabitatList.Count > 0 Then
            If Controls.Accept(True, False, False) = True Then
                For i = Scroll To Scroll + 5
                    If i <= Me.HabitatList.Count - 1 Then
                        If New Rectangle(100, 100 + (i - Scroll) * 96, 640, 64).Contains(MouseHandler.MousePosition) = True Then
                            If i = Cursor + Scroll Then
                                SoundManager.PlaySound("select")
                                Core.SetScreen(New PokedexScreen(Me, Nothing, Me.HabitatList(Cursor + Scroll)))
                            Else
                                Cursor = i - Scroll
                            End If
                        End If
                    End If
                Next
            End If

            If Controls.Accept(False, True, True) = True Then
                SoundManager.PlaySound("select")
                Core.SetScreen(New PokedexScreen(Me, Nothing, Me.HabitatList(Cursor + Scroll)))
            End If
        End If

        If Controls.Dismiss(True, True, True) = True Then
            Core.SetScreen(Me.PreScreen)
            SoundManager.PlaySound("select")
        End If

        PokedexScreen.TileOffset += 1
        If PokedexScreen.TileOffset >= 64 Then
            PokedexScreen.TileOffset = 0
        End If
    End Sub

End Class

Public Class PokedexScreen

    Inherits Screen

    Public Shared TileOffset As Integer = 0

    Public Enum OrderType
        Numeric
        Weight
        Height
        Alphabetically
    End Enum

    Public Enum FilterType
        Type1
        Type2
        Name
    End Enum

    Structure Filter
        Public FilterType As FilterType
        Public FilterValue As String
    End Structure

    Dim texture As Texture2D

    Public ReverseOrder As Boolean = False
    Public Order As OrderType = OrderType.Numeric

    Public Filters As New List(Of Filter)
    Public Profile As PokedexSelectScreen.PokedexProfile
    Public CHabitat As Habitat = Nothing

    Dim Scroll As Integer = 0
    Dim Cursor As New Vector2(0)

    Public Shared TempPokemonStorage As New Dictionary(Of Integer, Pokemon)
    Shared TempPokemonDexType As New Dictionary(Of Integer, Integer)

    Public PokemonList As New List(Of Pokemon)
    Dim menu As SelectMenu

    Public SelectIndexMain As Integer = 0
    Public OrderIndexMain As Integer = 0
    Public OrderIndexType As Integer = 0
    Public FilterIndexMain As Integer = 0
    Public FilterIndexName As Integer = 0
    Public FilterIndexType1 As Integer = 0
    Public FilterIndexType2 As Integer = 0

    Public Sub New(ByVal currentScreen As Screen, ByVal Profile As PokedexSelectScreen.PokedexProfile, ByVal Habitat As Habitat)
        Me.Identification = Identifications.PokedexScreen
        Me.PreScreen = currentScreen

        Me.texture = TextureManager.GetTexture("GUI\Menus\General")
        Me.Profile = Profile
        Me.CHabitat = Habitat

        Me.MouseVisible = True
        Me.CanMuteAudio = True
        Me.CanBePaused = True

        TempPokemonStorage.Clear()
        TempPokemonDexType.Clear()

        SetList()

        Me.menu = New SelectMenu({""}.ToList(), 0, Nothing, 0)
        Me.menu.Visible = False
    End Sub

    Private Sub SetList()
        PokemonList.Clear()
        TempPokemonStorage.Clear()
        TempPokemonDexType.Clear()

        Dim neededEntryType As Integer = 0
        Select Case Me.Order
            Case OrderType.Alphabetically
                neededEntryType = 1
            Case OrderType.Height, OrderType.Weight
                neededEntryType = 2
        End Select

        For Each f As Filter In Me.Filters
            Dim thisType As Integer = 0
            If f.FilterType = FilterType.Name Then
                thisType = 1
            Else
                thisType = 2
            End If
            If thisType > neededEntryType Then
                neededEntryType = thisType
            End If
        Next

        Dim pokeSearchList As New List(Of String)

        If CHabitat Is Nothing Then
            ' Add any external Pokémon if specified to do so:
            If Profile.Pokedex.IncludeExternalPokemon = True Then
                If Pokedex.PokemonMaxCount > 0 Then
                    For i = 1 To Pokedex.PokemonMaxCount
                        If Me.Profile.Pokedex.HasPokemon(i.ToString, False) = False Then
                            If Pokedex.GetEntryType(Core.Player.PokedexData, i.ToString) > 0 Then
                                Profile.Pokedex.PokemonList.Add(Profile.Pokedex.PokemonList.Count + 1, i.ToString)
                            End If
                        End If
                    Next
                End If
            End If
            For Each i As String In Profile.Pokedex.PokemonList.Values
                pokeSearchList.Add(i)
            Next
        Else
            For Each i As String In CHabitat.PokemonList
                pokeSearchList.Add(i)
            Next
        End If

        For i = 0 To pokeSearchList.Count - 1
            If Pokemon.PokemonDataExists(pokeSearchList(i).GetSplit(0, "_")) = True OrElse Pokemon.PokemonDataExists(pokeSearchList(i).GetSplit(0, ";")) = True Then

                Dim pID As Integer
                Dim pAD As String = ""

                If pokeSearchList(i).Contains(";") Then
                    pID = CInt(pokeSearchList(i).GetSplit(0, ";"))
                    pAD = pokeSearchList(i).GetSplit(1, ";")
                ElseIf pokeSearchList(i).Contains("_") Then
                    Dim additionalValue As String = PokemonForms.GetAdditionalValueFromDataFile(pokeSearchList(i))
                    pID = CInt(pokeSearchList(i).GetSplit(0, "_"))
                    If additionalValue <> "" Then
                        pAD = additionalValue
                    End If
                Else
                    pID = CInt(pokeSearchList(i))
                End If

                Dim p As Pokemon
                If pAD <> "" Then
                    p = Pokemon.GetPokemonByID(pID, pAD)
                Else
                    p = Pokemon.GetPokemonByID(pID, pAD, True)
                End If


                If Pokedex.GetEntryType(Core.Player.PokedexData, pokeSearchList(i)) >= neededEntryType Then

                    Dim valid As Boolean = True
                    For Each F As Filter In Me.Filters
                        Select Case F.FilterType
                            Case FilterType.Name
                                If p.GetName(True).ToUpper().StartsWith(F.FilterValue.ToUpper()) = False Then
                                    valid = False
                                    Exit For
                                End If
                            Case FilterType.Type1
                                If p.Type1.Type <> BattleSystem.GameModeElementLoader.GetElementByName(F.FilterValue).Type Then
                                    valid = False
                                    Exit For
                                End If
                            Case FilterType.Type2
                                If p.Type2.Type <> BattleSystem.GameModeElementLoader.GetElementByName(F.FilterValue).Type Then
                                    valid = False
                                    Exit For
                                End If
                        End Select
                    Next

                    If valid = True Then
                        If Profile.Pokedex IsNot Nothing Then
                            If Profile.Pokedex.GetPlace(pokeSearchList(i)) <> -1 Then
                                Me.PokemonList.Add(p)
                            End If
                        Else
                            Me.PokemonList.Add(p)
                        End If
                    End If
                End If
            End If
        Next

        Select Case Me.Order
            Case OrderType.Numeric
                If CHabitat Is Nothing Then
                    If Me.ReverseOrder = True Then
                        Me.PokemonList = (From p As Pokemon In Me.PokemonList Order By Profile.Pokedex.GetPlace(PokemonForms.GetPokemonDataFileName(p.Number, p.AdditionalData, True)) Descending).ToList()
                    Else
                        Me.PokemonList = (From p As Pokemon In Me.PokemonList Order By Profile.Pokedex.GetPlace(PokemonForms.GetPokemonDataFileName(p.Number, p.AdditionalData, True)) Ascending).ToList()
                    End If
                Else
                    If Me.ReverseOrder = True Then
                        Me.PokemonList = (From p As Pokemon In Me.PokemonList Order By PokemonForms.GetPokemonDataFileName(p.Number, p.AdditionalData, True) Descending).ToList()
                    Else
                        Me.PokemonList = (From p As Pokemon In Me.PokemonList Order By PokemonForms.GetPokemonDataFileName(p.Number, p.AdditionalData, True) Ascending).ToList()
                    End If
                End If
            Case OrderType.Alphabetically
                If Me.ReverseOrder = True Then
                    Me.PokemonList = (From p As Pokemon In Me.PokemonList Order By p.GetName(True) Descending).ToList()
                Else
                    Me.PokemonList = (From p As Pokemon In Me.PokemonList Order By p.GetName(True) Ascending).ToList()
                End If
            Case OrderType.Weight
                If Me.ReverseOrder = True Then
                    Me.PokemonList = (From p As Pokemon In Me.PokemonList Order By p.PokedexEntry.Weight Descending).ToList()
                Else
                    Me.PokemonList = (From p As Pokemon In Me.PokemonList Order By p.PokedexEntry.Weight Ascending).ToList()
                End If
            Case OrderType.Height
                If Me.ReverseOrder = True Then
                    Me.PokemonList = (From p As Pokemon In Me.PokemonList Order By p.PokedexEntry.Height Descending).ToList()
                Else
                    Me.PokemonList = (From p As Pokemon In Me.PokemonList Order By p.PokedexEntry.Height Ascending).ToList()
                End If
        End Select

        Me.ClampCursor()
    End Sub

    Public Overrides Sub Draw()
        Canvas.DrawRectangle(Core.windowSize, New Color(84, 198, 216))

        For y = -64 To Core.windowSize.Height Step 64
            Core.SpriteBatch.Draw(Me.texture, New Rectangle(Core.windowSize.Width - 128, y + TileOffset, 128, 64), New Rectangle(48, 0, 16, 16), Color.White)
        Next

        Canvas.DrawGradient(New Rectangle(0, 0, CInt(Core.windowSize.Width), 200), New Color(42, 167, 198), New Color(42, 167, 198, 0), False, -1)
        Canvas.DrawGradient(New Rectangle(0, CInt(Core.windowSize.Height - 200), CInt(Core.windowSize.Width), 200), New Color(42, 167, 198, 0), New Color(42, 167, 198), False, -1)

        Canvas.DrawRectangle(New Rectangle(50, 30, 564, 90), New Color(42, 167, 198, 150))

        If CHabitat Is Nothing Then
            Core.SpriteBatch.DrawString(FontManager.MainFont, Profile.Pokedex.Name, New Vector2(60, 55), Color.White, 0.0F, Vector2.Zero, 1.5F, SpriteEffects.None, 0.0F)
            Core.SpriteBatch.DrawString(FontManager.MainFont, Localization.GetString("pokedex_seen", "Seen:") & " " & Environment.NewLine & Environment.NewLine & Localization.GetString("pokedex_obtained", "Obtained:") & " ", New Vector2(420, 45), Color.White)
            Dim SeenCaughtTitleWidth As Integer = CInt(FontManager.MainFont.MeasureString(Localization.GetString("pokedex_seen", "Seen:") & " " & Environment.NewLine & Environment.NewLine & Localization.GetString("pokedex_obtained", "Obtained:") & " ").X)
            Core.SpriteBatch.DrawString(FontManager.MainFont, Profile.Seen + Profile.Obtained & Environment.NewLine & Environment.NewLine & Profile.Obtained, New Vector2(420 + SeenCaughtTitleWidth, 45), Color.Black)
        Else
            Core.SpriteBatch.DrawString(FontManager.MainFont, CHabitat.Name, New Vector2(60, 80), Color.White, 0.0F, Vector2.Zero, 1.5F, SpriteEffects.None, 0.0F)
            Core.SpriteBatch.Draw(CHabitat.Texture, New Rectangle(60, 32, 64, 48), Color.White)
            Core.SpriteBatch.DrawString(FontManager.MainFont, Localization.GetString("pokedex_available", "Available:") & Environment.NewLine & Environment.NewLine & Localization.GetString("pokedex_obtained", "Obtained:"), New Vector2(420, 45), Color.White)
            Dim AvailableObtainedTitleWidth As Integer = CInt(FontManager.MainFont.MeasureString(Localization.GetString("pokedex_available", "Available:") & Environment.NewLine & Environment.NewLine & Localization.GetString("pokedex_obtained", "Obtained:")).X)
            Core.SpriteBatch.DrawString(FontManager.MainFont, CHabitat.PokemonList.Count & Environment.NewLine & Environment.NewLine & CHabitat.PokemonCaught, New Vector2(420 + AvailableObtainedTitleWidth, 45), Color.Black)

            Dim progressTexture As Texture2D = Me.CHabitat.ProgressTexture
            If Not progressTexture Is Nothing Then
                Core.SpriteBatch.Draw(progressTexture, New Rectangle(134, 46, 20, 20), Color.White)
            End If
        End If

        If PokemonList.Count > 0 Then
            For x = 0 To 5
                For y = 0 To 4
                    Dim id As Integer = (y + Scroll) * 6 + x

                    If id <= Me.PokemonList.Count - 1 Then
                        Dim dexID As String = PokemonForms.GetPokemonDataFileName(Me.PokemonList(id).Number, Me.PokemonList(id).AdditionalData, True)

                        If Not CHabitat Is Nothing OrElse Me.Profile.Pokedex.OriginalCount >= Profile.Pokedex.GetPlace(dexID) Then
                            Canvas.DrawRectangle(New Rectangle(50 + x * 100, 140 + y * 100, 64, 92), New Color(42, 167, 198, 150))
                        Else
                            Canvas.DrawBorder(3, New Rectangle(50 + x * 100, 140 + y * 100, 64, 92), New Color(42, 167, 198, 150))
                        End If

                        Dim p As Pokemon = Nothing
                        Dim entryType As Integer = 0

                        If TempPokemonStorage.ContainsKey(id + 1) = False Then
                            TempPokemonStorage.Add(id + 1, Me.PokemonList(id))
                            TempPokemonDexType.Add(id + 1, Pokedex.GetEntryType(Core.Player.PokedexData, dexID))
                        End If
                        p = TempPokemonStorage(id + 1)
                        entryType = TempPokemonDexType(id + 1)

                        If Cursor = New Vector2(x, y) Then
                            DrawPokemonPreview(p)
                        End If

                        Dim drawBlack As Boolean = False
                        If dexID.Contains(";") = False Then
                            If entryType = 0 Then
                                Dim formEntry As Integer = Pokedex.HasAnyForm(Me.PokemonList(id).Number)
                                If formEntry > 0 Then
                                    entryType = formEntry
                                    Dim pForms As List(Of String) = PokemonForms.GetAdditionalDataForms(Me.PokemonList(id).Number)
                                    If dexID.Contains("_") = True Then
                                        If pForms Is Nothing Then
                                            drawBlack = True
                                        End If
                                    End If
                                End If
                            End If
                        End If

                        Dim c As Color = Color.Gray
                        If entryType > 0 Then
                            If entryType > 1 Then
                                c = Color.White
                            End If
                            If drawBlack = True Then
                                c = Color.Black
                            End If
                            Dim pokeTexture = p.GetMenuTexture()
                            Dim pokeTextureScale As Vector2 = New Vector2(CSng(32 / pokeTexture.Width * 2), CSng(32 / pokeTexture.Height * 2))
                            Core.SpriteBatch.Draw(pokeTexture, New Rectangle(50 + x * 100, 140 + y * 100, CInt(pokeTexture.Width * pokeTextureScale.X), CInt(pokeTexture.Height * pokeTextureScale.Y)), c)
                        End If

                        Dim no As String = "000"

                        If CHabitat Is Nothing Then
                            no = Profile.Pokedex.GetPlace(dexID).ToString()
                        Else
                            no = p.Number.ToString()
                        End If
                        While no.Length < 3
                            no = "0" & no
                        End While
                        Core.SpriteBatch.DrawString(FontManager.MainFont, no, New Vector2(50 + x * 100 + CInt(32 - FontManager.MainFont.MeasureString(no).X / 2), 206 + y * 100), Color.White)
                    End If
                Next
            Next
        Else
            Canvas.DrawGradient(New Rectangle(50, 300, 80, 90), New Color(84, 198, 216), New Color(42, 167, 198, 150), True, -1)
            Canvas.DrawRectangle(New Rectangle(130, 300, 404, 90), New Color(42, 167, 198, 150))
            Canvas.DrawGradient(New Rectangle(534, 300, 80, 90), New Color(42, 167, 198, 150), New Color(84, 198, 216), True, -1)

            Core.SpriteBatch.DrawString(FontManager.MainFont, Localization.GetString("pokedex_search_no_results", "No search results."), New Vector2(50 + CInt(564 / 2) - CInt(FontManager.MainFont.MeasureString(Localization.GetString("pokedex_search_no_results", "No search results.")).X / 2), 330), Color.White)
        End If

        Canvas.DrawRectangle(New Rectangle(670, 30, 480, 90), New Color(42, 167, 198, 150))
        Dim orderText As String = Localization.GetString("pokedex_order_numeric", "Numeric")
        Select Case Me.Order
            Case OrderType.Alphabetically
                orderText = Localization.GetString("pokedex_order_alphabetically", "A-Z")
            Case OrderType.Height
                orderText = Localization.GetString("pokedex_order_height", "Height")
            Case OrderType.Weight
                orderText = Localization.GetString("pokedex_order_weight", "Weight")
        End Select
        Dim filterText As String = Localization.GetString("pokedex_filter_none", "None")
        If Filters.Count > 0 Then
            filterText = ""
            For Each f As Filter In Me.Filters
                If filterText <> "" Then
                    filterText &= ", "
                End If
                Select Case f.FilterType
                    Case FilterType.Name
                        filterText &= Localization.GetString("pokedex_filter_name", "Name")
                    Case FilterType.Type1
                        filterText &= Localization.GetString("pokedex_filter_type1", "Type 1")
                    Case FilterType.Type2
                        filterText &= Localization.GetString("pokedex_filter_type2", "Type 2")
                End Select
            Next
        End If
        Dim OrderFilterTitleWidth As Integer = CInt(FontManager.MainFont.MeasureString(Localization.GetString("pokedex_order", "Order") & ":" & Environment.NewLine & Localization.GetString("pokedex_filter", "Filter") & ":").X)
        Core.SpriteBatch.DrawString(FontManager.MainFont, Localization.GetString("pokedex_order", "Order") & ":" & Environment.NewLine & Localization.GetString("pokedex_filter", "Filter") & ":" & Environment.NewLine & Localization.GetString("pokedex_search_hint", "Press [<system.button(special)>] or Select to search."), New Vector2(685, 45), Color.White)
        Core.SpriteBatch.DrawString(FontManager.MainFont, orderText & Environment.NewLine & filterText, New Vector2(685 + OrderFilterTitleWidth + CInt(FontManager.MainFont.MeasureString(" ").X), 45), Color.Black)

        If menu.Visible = True Then
            menu.Draw()
        Else
            If PokemonList.Count > 0 Then
                DrawCursor()
            End If
        End If
    End Sub

    Private Function GetPlace(ByVal PokemonNumber As Integer) As Integer
        For i = 0 To PokemonList.Count - 1
            If PokemonList(i).Number = PokemonNumber Then
                Return i + 1
            End If
        Next

        Return -1
    End Function

    Private Sub DrawPokemonPreview(ByVal p As Pokemon)
        Dim dexID As String = PokemonForms.GetPokemonDataFileName(p.Number, p.AdditionalData, True)
        
        Dim entryType As Integer = Pokedex.GetEntryType(Core.Player.PokedexData, dexID)

        If dexID.Contains("_") = False AndAlso dexID.Contains(";") = False Then
            If entryType = 0 Then
                Dim formEntry As Integer = Pokedex.HasAnyForm(p.Number)
                If formEntry > 0 Then
                    entryType = formEntry
                    Dim pForms As List(Of String) = PokemonForms.GetAdditionalDataForms(p.Number)
                End If
            End If
        End If

        For i = 0 To 4
            Canvas.DrawGradient(New Rectangle(650, 300 + i * 40, 50, 2), New Color(255, 255, 255, 10), New Color(255, 255, 255, 255), True, -1)
            Canvas.DrawRectangle(New Rectangle(700, 300 + i * 40, 350, 2), Color.White)
            Canvas.DrawGradient(New Rectangle(1050, 300 + i * 40, 50, 2), New Color(255, 255, 255, 255), New Color(255, 255, 255, 10), True, -1)
        Next

        Dim no As String = "000"
        If CHabitat Is Nothing Then
            no = Profile.Pokedex.GetPlace(dexID).ToString()
        Else
            no = p.Number.ToString()
        End If
        While no.Length < 3
            no = "0" & no
        End While

        If entryType = 0 Then
            Core.SpriteBatch.DrawString(FontManager.MainFont, "???" & Environment.NewLine & Environment.NewLine & Localization.GetString("No.", "No.") & " " & no, New Vector2(864, 200), Color.White)
        Else
            Core.SpriteBatch.DrawString(FontManager.MainFont, p.GetName(True) & Environment.NewLine & Environment.NewLine & Localization.GetString("No.", "No.") & " " & no, New Vector2(864, 200), Color.White)
            Core.SpriteBatch.Draw(p.GetTexture(True), New Rectangle(CInt(680 - p.GetTexture(True).Width / 4), CInt(140 - p.GetTexture(True).Height / 4), MathHelper.Min(CInt(p.GetTexture(True).Width * 2), 256), MathHelper.Min(CInt(p.GetTexture(True).Height * 2), 256)), Color.White)

            Core.SpriteBatch.DrawString(FontManager.MainFont, Localization.GetString("pokedex_data_species", "SPECIES"), New Vector2(680, 310), Color.Black)
            Core.SpriteBatch.DrawString(FontManager.MainFont, Localization.GetString("pokedex_data_type", "TYPE"), New Vector2(680, 350), Color.Black)
            Core.SpriteBatch.DrawString(FontManager.MainFont, Localization.GetString("pokedex_data_height", "HEIGHT"), New Vector2(680, 390), Color.Black)
            Core.SpriteBatch.DrawString(FontManager.MainFont, Localization.GetString("pokedex_data_weight", "WEIGHT"), New Vector2(680, 430), Color.Black)

            Canvas.DrawRectangle(New Rectangle(670, 480, 480, 152), New Color(42, 167, 198, 150))

            If Not CHabitat Is Nothing Then
                Dim encounterTypes As New List(Of Integer)
                For Each ec As PokedexScreen.Habitat.EncounterPokemon In CHabitat.ObtainTypeList
                    If ec.PokemonID = PokemonForms.GetPokemonDataFileName(p.Number, p.AdditionalData) Then
                        If encounterTypes.Contains(ec.EncounterType) = False Then
                            encounterTypes.Add(ec.EncounterType)
                        End If
                    End If
                Next
                For i = 0 To encounterTypes.Count - 1
                    Dim encounterType As Integer = encounterTypes(i)
                    Core.SpriteBatch.Draw(PokedexScreen.Habitat.GetEncounterTypeImage(encounterType), New Rectangle(824 + i * 32, 266, 32, 32), Color.White)
                Next
            End If

            If entryType > 1 Then
                Dim DexEntrySpecies As String = p.PokedexEntry.Species
                Dim FormName As String = PokemonForms.GetFormName(p)
                If FormName = "" Then
                    FormName = p.Name
                End If
                If Localization.TokenExists("pokemon_species_" & FormName) = True Then
                    DexEntrySpecies = Localization.GetString("pokemon_species_" & FormName, p.PokedexEntry.Species)
                End If

                Core.SpriteBatch.DrawString(FontManager.MainFont, DexEntrySpecies, New Vector2(850, 310), Color.Black)
                Core.SpriteBatch.DrawString(FontManager.MainFont, "", New Vector2(850, 350), Color.Black)
                Core.SpriteBatch.DrawString(FontManager.MainFont, p.PokedexEntry.Height & " m", New Vector2(850, 390), Color.Black)
                Core.SpriteBatch.DrawString(FontManager.MainFont, p.PokedexEntry.Weight & " kg", New Vector2(850, 430), Color.Black)

                Core.SpriteBatch.Draw(TextureManager.GetTexture(Element.GetElementTexturePath()), New Rectangle(850, 356, 48, 16), p.Type1.GetElementImage(), Color.White)
                If p.Type2 IsNot Nothing AndAlso p.Type2.Type <> Element.Types.Blank Then
                    Core.SpriteBatch.Draw(TextureManager.GetTexture(Element.GetElementTexturePath()), New Rectangle(900, 356, 48, 16), p.Type2.GetElementImage(), Color.White)
                End If

                Dim DexEntryText As String = p.PokedexEntry.Text

                If Localization.TokenExists("pokemon_desc_" & FormName) = True Then
                    DexEntryText = Localization.GetString("pokemon_desc_" & FormName, p.PokedexEntry.Text)
                End If
                Core.SpriteBatch.DrawString(FontManager.MainFont, DexEntryText.CropStringToWidth(FontManager.MainFont, 440), New Vector2(688, 490), Color.Black)

                Core.SpriteBatch.Draw(TextureManager.GetTexture("GUI\Menus\pokedexhabitat", New Rectangle(160, 160, 10, 10), ""), New Rectangle(992, 242, 20, 20), Color.White)

                If entryType > 2 Then
                    SpriteBatch.Draw(TextureManager.GetTexture("GUI\Menus\PokemonInfo"), New Rectangle(1033, 243, 18, 18), New Rectangle(16, 0, 9, 9), Color.White)
                End If
            Else
                Core.SpriteBatch.DrawString(FontManager.MainFont, "??? Pokémon", New Vector2(850, 310), Color.Black)
                Core.SpriteBatch.DrawString(FontManager.MainFont, "???", New Vector2(850, 350), Color.Black)
                Core.SpriteBatch.DrawString(FontManager.MainFont, "??? m", New Vector2(850, 390), Color.Black)
                Core.SpriteBatch.DrawString(FontManager.MainFont, "??? kg", New Vector2(850, 430), Color.Black)

                Core.SpriteBatch.Draw(TextureManager.GetTexture("GUI\Menus\pokedexhabitat", New Rectangle(160, 170, 10, 10), ""), New Rectangle(992, 242, 20, 20), Color.White)
            End If
        End If
    End Sub

    Private Sub DrawCursor()
        Dim cPosition As Vector2 = New Vector2(50 + Me.Cursor.X * 100 + 42, 140 + Me.Cursor.Y * 100 - 42)

        Dim t As Texture2D = TextureManager.GetTexture("GUI\Menus\General", New Rectangle(0, 0, 16, 16), "")
        Core.SpriteBatch.Draw(t, New Rectangle(CInt(cPosition.X), CInt(cPosition.Y), 64, 64), Color.White)
    End Sub

    Public Overrides Sub Update()
        If menu.Visible = True Then
            menu.Update()
        Else
            If Controls.Left(True, True, False, True, True, True) = True Then
                Me.Cursor.X -= 1
            End If
            If Controls.Right(True, True, False, True, True, True) = True Then
                Me.Cursor.X += 1
            End If
            If Controls.Up(True, True, True, True, True, True) = True Then
                Me.Cursor.Y -= 1
                If Controls.ShiftDown() = True Then
                    Me.Cursor.Y -= 4
                End If
            End If
            If Controls.Down(True, True, True, True, True, True) = True Then
                Me.Cursor.Y += 1
                If Controls.ShiftDown() = True Then
                    Me.Cursor.Y += 4
                End If
            End If

            If Cursor.X > 5 Then
                Cursor.X = 0
                Cursor.Y += 1
            End If
            If Cursor.X < 0 Then
                Cursor.X = 5
                Cursor.Y -= 1
            End If

            While Cursor.Y < 0
                Me.Cursor.Y += 1
                Me.Scroll -= 1
            End While
            While Cursor.Y > 4
                Me.Cursor.Y -= 1
                Me.Scroll += 1
            End While

            If Controls.Accept(True, False, False) = True Then
                For i = 0 To 29
                    Dim x As Integer = i
                    Dim y As Integer = 0
                    While x > 5
                        x -= 6
                        y += 1
                    End While

                    If New Rectangle(50 + x * 100, 140 + y * 100, 64, 92).Contains(MouseHandler.MousePosition) = True Then
                        If Cursor.X + Cursor.Y * 6 = i Then
                            Dim dexIndex As Integer = CInt((Cursor.Y + Scroll) * 6 + Cursor.X + 1)
                            If TempPokemonDexType(dexIndex) > 0 Then
                                SoundManager.PlaySound("select")

                                Core.SetScreen(New PokedexViewScreen(Me, TempPokemonStorage(dexIndex), False, dexIndex - 1))
                            Else
                                Dim formEntry As Integer = Pokedex.HasAnyForm(TempPokemonStorage(dexIndex).Number)
                                If formEntry > 0 Then
                                    SoundManager.PlaySound("select")

                                    Core.SetScreen(New PokedexViewScreen(Me, TempPokemonStorage(dexIndex), False, dexIndex - 1))
                                End If
                            End If
                        Else
                            Cursor.X = x
                            Cursor.Y = y
                        End If
                    End If
                Next
            End If

            ClampCursor()

            If Controls.Accept(False, True, True) = True Then
                Dim dexIndex As Integer = CInt((Cursor.Y + Scroll) * 6 + Cursor.X + 1)
                If TempPokemonDexType(dexIndex) > 0 Then
                    SoundManager.PlaySound("select")
                    Core.SetScreen(New PokedexViewScreen(Me, TempPokemonStorage(dexIndex), False, dexIndex - 1))
                End If
            End If

            If KeyBoardHandler.KeyPressed(KeyBindings.SpecialKey) = True Or ControllerHandler.ButtonPressed(Buttons.Back) = True Then
                Me.menu = New SelectMenu({Localization.GetString("pokedex_order", "Order"), Localization.GetString("pokedex_filter", "Filter"), Localization.GetString("global_reset", "Reset"), Localization.GetString("global_back", "Back")}.ToList(), SelectIndexMain, AddressOf SelectMenu1, 3, "selectmain")
            End If

            If Controls.Dismiss(True, True, True) = True Then
                If Me.Filters.Count > 0 Or Me.Order <> OrderType.Numeric Or Me.ReverseOrder = True Then
                    SoundManager.PlaySound("select")
                    Me.Filters.Clear()
                    Me.ReverseOrder = False
                    Me.Order = OrderType.Numeric
                    Me.SetList()
                Else
                    SoundManager.PlaySound("select")
                    Core.SetScreen(Me.PreScreen)
                End If
            End If
        End If

        TileOffset += 1
        If TileOffset >= 64 Then
            TileOffset = 0
        End If
    End Sub

    Private Sub ClampCursor()
        Dim linesCount As Integer = CInt(Math.Ceiling(Me.PokemonList.Count / 6))

        If linesCount < 6 Then
            Me.Scroll = 0
        Else
            Me.Scroll = Me.Scroll.Clamp(0, linesCount - 5)
        End If

        Dim maxY As Integer = linesCount - Scroll - 1
        Me.Cursor.Y = Me.Cursor.Y.Clamp(0, maxY)

        If Me.Cursor.Y = maxY Then
            Dim maxX As Integer = Me.PokemonList.Count
            While maxX > 6
                maxX -= 6
            End While
            Me.Cursor.X = Me.Cursor.X.Clamp(0, maxX - 1)
        End If
    End Sub

    Public Overrides Sub ChangeTo()
        TempPokemonDexType.Clear()
        TempPokemonStorage.Clear()
    End Sub

#Region "Menus"

    Private Sub SelectMenu1(ByVal s As SelectMenu)
        Dim ReverseString As String = Localization.GetString("global_yes", "Yes")
        If Me.ReverseOrder = False Then
            ReverseString = Localization.GetString("global_no", "No")
        End If
        Select Case s.SelectedItem.ToLower()
            Case Localization.GetString("pokedex_order", "Order").ToLower
                Me.menu = New SelectMenu({Localization.GetString("pokedex_order_type", "Type"), Localization.GetString("pokedex_order_reverse", "Reverse") & ": " & ReverseString, Localization.GetString("global_back", "Back")}.ToList(), OrderIndexMain, AddressOf SelectMenuOrder, 2, "ordermain")
            Case Localization.GetString("pokedex_filter", "Filter").ToLower
                Me.menu = New SelectMenu({Localization.GetString("pokedex_filter_name", "Name"), Localization.GetString("pokedex_filter_type1", "Type 1"), Localization.GetString("pokedex_filter_type2", "Type 2"), Localization.GetString("global_clear", "Clear"), Localization.GetString("global_back", "Back")}.ToList(), FilterIndexMain, AddressOf SelectMenuFilter, 4, "filtermain")
            Case Localization.GetString("global_reset", "Reset").ToLower
                Me.Filters.Clear()
                Me.ReverseOrder = False
                Me.Order = OrderType.Numeric
                Me.SetList()
        End Select
    End Sub

    Private Sub SelectMenuFilter(ByVal s As SelectMenu)
        Select Case s.SelectedItem.ToLower()
            Case Localization.GetString("pokedex_filter_name", "Name").ToLower
                Me.menu = New SelectMenu({"A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", Localization.GetString("global_back", "Back")}.ToList(), FilterIndexName, AddressOf SelectMenuNameFilter, -1, "filtername")
            Case Localization.GetString("pokedex_filter_type1", "Type 1").ToLower
                Dim typeList As New List(Of String)
                typeList.AddRange({"Normal", "Fire", "Fighting", "Water", "Flying", "Grass", "Poison", "Electric", "Ground", "Psychic", "Rock", "Ice", "Bug", "Dragon", "Ghost", "Dark", "Steel", "Fairy"})
                For Each e As Element In BattleSystem.GameModeElementLoader.LoadedElements
                    typeList.Add(e.gmOriginalName)
                Next
                typeList.AddRange({"Blank", Localization.GetString("global_back", "Back")})
                Me.menu = New SelectMenu(typeList, FilterIndexType1, AddressOf SelectMenuType1Filter, -1, "filtertype1")
            Case Localization.GetString("pokedex_filter_type2", "Type 2").ToLower
                Dim typeList As New List(Of String)
                typeList.AddRange({"Normal", "Fire", "Fighting", "Water", "Flying", "Grass", "Poison", "Electric", "Ground", "Psychic", "Rock", "Ice", "Bug", "Dragon", "Ghost", "Dark", "Steel", "Fairy"})
                For Each e As Element In BattleSystem.GameModeElementLoader.LoadedElements
                    typeList.Add(e.gmOriginalName)
                Next
                typeList.AddRange({"Blank", Localization.GetString("global_back", "Back")})
                Me.menu = New SelectMenu(typeList, FilterIndexType2, AddressOf SelectMenuType2Filter, -1, "filtertype2")
            Case Localization.GetString("global_clear", "Clear").ToLower
                Me.Filters.Clear()
                Me.SetList()
            Case Localization.GetString("global_back", "Back").ToLower
                Me.menu = New SelectMenu({Localization.GetString("pokedex_order", "Order"), Localization.GetString("pokedex_filter", "Filter"), Localization.GetString("global_reset", "Reset"), Localization.GetString("global_back", "Back")}.ToList(), SelectIndexMain, AddressOf SelectMenu1, 3, "selectmain")
        End Select
    End Sub

    Private Sub SelectMenuType1Filter(ByVal s As SelectMenu)
        If s.SelectedItem <> Localization.GetString("global_back", "Back") Then
            For i = 0 To Filters.Count - 1
                If Filters(i).FilterType = FilterType.Type1 Then
                    Filters.RemoveAt(i)
                    Exit For
                End If
            Next

            Filters.Add(New Filter With {.FilterType = FilterType.Type1, .FilterValue = s.SelectedItem})
            SetList()
        Else
            Me.menu = New SelectMenu({Localization.GetString("pokedex_filter_name", "Name"), Localization.GetString("pokedex_filter_type1", "Type 1"), Localization.GetString("pokedex_filter_type2", "Type 2"), Localization.GetString("global_clear", "Clear"), Localization.GetString("global_back", "Back")}.ToList(), FilterIndexMain, AddressOf SelectMenuFilter, 4, "filtermain")
        End If
    End Sub

    Private Sub SelectMenuType2Filter(ByVal s As SelectMenu)
        If s.SelectedItem <> Localization.GetString("global_back", "Back") Then
            For i = 0 To Filters.Count - 1
                If Filters(i).FilterType = FilterType.Type2 Then
                    Filters.RemoveAt(i)
                    Exit For
                End If
            Next

            Filters.Add(New Filter With {.FilterType = FilterType.Type2, .FilterValue = s.SelectedItem})
            SetList()
        Else
            Me.menu = New SelectMenu({Localization.GetString("pokedex_filter_name", "Name"), Localization.GetString("pokedex_filter_type1", "Type 1"), Localization.GetString("pokedex_filter_type2", "Type 2"), Localization.GetString("global_clear", "Clear"), Localization.GetString("global_back", "Back")}.ToList(), FilterIndexMain, AddressOf SelectMenuFilter, 4, "filtermain")
        End If
    End Sub

    Private Sub SelectMenuNameFilter(ByVal s As SelectMenu)
        If s.SelectedItem <> Localization.GetString("global_back", "Back") Then
            For i = 0 To Filters.Count - 1
                If Filters(i).FilterType = FilterType.Name Then
                    Filters.RemoveAt(i)
                    Exit For
                End If
            Next

            Filters.Add(New Filter With {.FilterType = FilterType.Name, .FilterValue = s.SelectedItem})
            SetList()
        Else
            Me.menu = New SelectMenu({Localization.GetString("pokedex_filter_name", "Name"), Localization.GetString("pokedex_filter_type1", "Type 1"), Localization.GetString("pokedex_filter_type2", "Type 2"), Localization.GetString("global_clear", "Clear"), Localization.GetString("global_back", "Back")}.ToList(), FilterIndexMain, AddressOf SelectMenuFilter, 4, "filtermain")
        End If
    End Sub

    Private Sub SelectMenuOrder(ByVal s As SelectMenu)
        Dim ReverseString As String = Localization.GetString("global_yes", "Yes")
        If Me.ReverseOrder = False Then
            ReverseString = Localization.GetString("global_no", "No")
        End If

        Select Case s.SelectedItem.ToLower()
            Case Localization.GetString("pokedex_order_type", "Type").ToLower
                Me.menu = New SelectMenu({Localization.GetString("pokedex_order_numeric", "Numeric"), Localization.GetString("pokedex_order_alphabetically", "A-Z"), Localization.GetString("pokedex_order_weight", "Weight"), Localization.GetString("pokedex_order_height", "Height"), Localization.GetString("global_back", "Back")}.ToList(), OrderIndexType, AddressOf SelectMenuOrderType, 4, "ordertype")
            Case Localization.GetString("pokedex_order_reverse", "Reverse").ToLower & ": " & ReverseString.ToLower()
                Me.ReverseOrder = Not Me.ReverseOrder
                Me.menu = New SelectMenu({Localization.GetString("pokedex_order_type", "Type"), Localization.GetString("pokedex_order_reverse", "Reverse") & ": " & ReverseString.ToLower(), Localization.GetString("global_back", "Back")}.ToList(), OrderIndexMain, AddressOf SelectMenuOrder, 2, "ordermain")
                Me.SetList()
            Case Localization.GetString("global_back", "Back").ToLower
                Me.menu = New SelectMenu({Localization.GetString("pokedex_order", "Order"), Localization.GetString("pokedex_filter", "Filter"), Localization.GetString("global_reset", "Reset"), "Back"}.ToList(), SelectIndexMain, AddressOf SelectMenu1, 3, "selectmain")
        End Select
    End Sub

    Private Sub SelectMenuOrderType(ByVal s As SelectMenu)
        Dim ReverseString As String = Localization.GetString("global_yes", "Yes")
        If Me.ReverseOrder = False Then
            ReverseString = Localization.GetString("global_no", "No")
        End If
        Select Case s.SelectedItem.ToLower()
            Case Localization.GetString("pokedex_order_numeric", "Numeric").ToLower
                Me.Order = OrderType.Numeric
                Me.SetList()
            Case Localization.GetString("pokedex_order_alphabetically", "A-Z").ToLower
                Me.Order = OrderType.Alphabetically
                Me.SetList()
            Case Localization.GetString("pokedex_order_weight", "Weight").ToLower
                Me.Order = OrderType.Weight
                Me.SetList()
            Case Localization.GetString("pokedex_order_height", "Height").ToLower
                Me.Order = OrderType.Height
                Me.SetList()
            Case Localization.GetString("global_back", "Back")
                Me.menu = New SelectMenu({Localization.GetString("pokedex_order_type", "Type"), Localization.GetString("pokedex_order_reverse", "Reverse") & ": " & ReverseString, Localization.GetString("global_back", "Back")}.ToList(), OrderIndexMain, AddressOf SelectMenuOrder, 2, "ordermain")
        End Select
    End Sub

#End Region

    Private Class SelectMenu

        Dim Items As New List(Of String)
        Dim Index As Integer = 0
        Dim RememberVar As String = ""
        Public Delegate Sub ClickEvent(ByVal s As SelectMenu)
        Dim ClickHandler As ClickEvent = Nothing
        Dim BackIndex As Integer = 0
        Public Visible As Boolean = True
        Public Scroll As Integer = 0

        Dim t1 As Texture2D
        Dim t2 As Texture2D

        Public Sub New(ByVal Items As List(Of String), ByVal Index As Integer, ByVal ClickHandle As ClickEvent, ByVal BackIndex As Integer, Optional ByRef RememberVar As String = "")
            Me.Items = Items
            Me.Index = Index
            Me.ClickHandler = ClickHandle
            Me.BackIndex = BackIndex
            If RememberVar <> "" Then
                Me.RememberVar = RememberVar
            End If
            If Me.BackIndex < 0 Then
                Me.BackIndex = Me.Items.Count + Me.BackIndex
            End If
            Me.Visible = True

            t1 = TextureManager.GetTexture("GUI\Menus\General", New Rectangle(16, 16, 16, 16), "")
            t2 = TextureManager.GetTexture("GUI\Menus\General", New Rectangle(32, 16, 16, 16), "")
        End Sub

        Public Sub Update()
            If Visible = True Then
                If Controls.Up(True, True, True, True, True, True) = True Then
                    Me.Index -= 1
                End If
                If Controls.Down(True, True, True, True, True, True) = True Then
                    Me.Index += 1
                End If
                Me.Index = Me.Index.Clamp(0, Me.Items.Count - 1)

                Select Case Me.RememberVar.ToLower
                    Case "selectmain"
                        CType(CurrentScreen, PokedexScreen).SelectIndexMain = Me.Index
                    Case "ordermain"
                        CType(CurrentScreen, PokedexScreen).OrderIndexMain = Me.Index
                    Case "ordertype"
                        CType(CurrentScreen, PokedexScreen).OrderIndexType = Me.Index
                    Case "filtermain"
                        CType(CurrentScreen, PokedexScreen).FilterIndexMain = Me.Index
                    Case "filtername"
                        CType(CurrentScreen, PokedexScreen).FilterIndexName = Me.Index
                    Case "filtertype1"
                        CType(CurrentScreen, PokedexScreen).FilterIndexType1 = Me.Index
                    Case "filtertype2"
                        CType(CurrentScreen, PokedexScreen).FilterIndexType2 = Me.Index
                End Select

                For i = Scroll To Me.Scroll + 8
                    If i <= Me.Items.Count - 1 Then
                        If Controls.Accept(True, False, False) = True And i = Me.Index And New Rectangle(Core.windowSize.Width - 270, 66 * ((i + 1) - Scroll), 256, 64).Contains(MouseHandler.MousePosition) = True Or
                                    Controls.Accept(False, True, True) = True And i = Me.Index Or Controls.Dismiss(True, True, True) = True And Me.BackIndex = Me.Index Then

                            If Not ClickHandler Is Nothing Then
                                ClickHandler(Me)
                                SoundManager.PlaySound("select")
                            End If
                            Me.Visible = False
                        End If
                        If Controls.Dismiss(True, True, True) = True Then
                            Me.Index = Me.BackIndex
                            If Not ClickHandler Is Nothing Then
                                ClickHandler(Me)
                                SoundManager.PlaySound("select")
                            End If
                            Me.Visible = False
                            Exit For
                        End If
                        If New Rectangle(Core.windowSize.Width - 270, 66 * ((i + 1) - Scroll), 256, 64).Contains(MouseHandler.MousePosition) = True And Controls.Accept(True, False, False) = True Then
                            Me.Index = i
                        End If
                    End If
                Next

                If Index - Scroll > 8 Then
                    Scroll = Index - 8
                End If
                If Index - Scroll < 0 Then
                    Scroll = Index
                End If
            End If
        End Sub

        Public Sub Draw()
            If Visible = True Then
                For i = Scroll To Me.Scroll + 8
                    If i <= Me.Items.Count - 1 Then
                        Dim Entry As String = Items(i)
                        Dim Text As String = Entry
                        Select Case Entry
                            Case "Normal", "Fire", "Fighting", "Water", "Flying", "Grass", "Poison", "Electric", "Ground", "Psychic", "Rock", "Ice", "Bug", "Dragon", "Ghost", "Dark", "Steel", "Fairy", "Blank"
                                Text = Localization.GetString("global_pokemon_type_" & Entry.ToLower, Entry)
                        End Select

                        For Each e As Element In BattleSystem.GameModeElementLoader.LoadedElements
                            If Entry.ToLower = e.gmOriginalName.ToLower Then
                                Text = Localization.GetString("global_pokemon_type_" & e.gmOriginalName.ToLower, Entry)
                            End If
                        Next

                        Dim startPos As New Vector2(Core.windowSize.Width - 270, 66 * ((i + 1) - Scroll))

                        Core.SpriteBatch.Draw(t1, New Rectangle(CInt(startPos.X), CInt(startPos.Y), 64, 64), Color.White)
                        Core.SpriteBatch.Draw(t2, New Rectangle(CInt(startPos.X + 64), CInt(startPos.Y), 64, 64), Color.White)
                        Core.SpriteBatch.Draw(t2, New Rectangle(CInt(startPos.X + 128), CInt(startPos.Y), 64, 64), Color.White)
                        Core.SpriteBatch.Draw(t1, New Rectangle(CInt(startPos.X + 192), CInt(startPos.Y), 64, 64), Nothing, Color.White, 0.0F, New Vector2(0), SpriteEffects.FlipHorizontally, 0.0F)

                        Core.SpriteBatch.DrawString(FontManager.MainFont, Text, New Vector2(startPos.X + 128 - (FontManager.MainFont.MeasureString(Text).X * 1.4F) / 2, startPos.Y + 15), Color.Black, 0.0F, Vector2.Zero, 1.4F, SpriteEffects.None, 0.0F)

                        If Me.Index = i Then
                            Dim cPosition As Vector2 = New Vector2(startPos.X + 128, startPos.Y - 40)
                            Dim t As Texture2D = TextureManager.GetTexture("GUI\Menus\General", New Rectangle(0, 0, 16, 16), "")
                            Core.SpriteBatch.Draw(t, New Rectangle(CInt(cPosition.X), CInt(cPosition.Y), 64, 64), Color.White)
                        End If
                    End If
                Next
            End If
        End Sub

        Public ReadOnly Property SelectedItem() As String
            Get
                Return Items(Me.Index)
            End Get
        End Property

    End Class

    Public Class Habitat

        Public Enum HabitatTypes As Integer
            Grassland = 0
            Forest = 1
            WatersEdge = 2
            Sea = 3
            Cave = 4
            Mountain = 5
            RoughTerrain = 6
            City = 7
        End Enum

        Public Structure EncounterPokemon
            Public PokemonID As String
            Public EncounterType As Integer
            Public Daytimes() As Integer
        End Structure

        Dim MergeData() As String = {} ' Temporary data storage if needs to merge.

        Public File As String = ""
        Public Name As String = ""
        Public HabitatType As HabitatTypes = HabitatTypes.Grassland

        Public PokemonList As New List(Of String)
        Public ObtainTypeList As New List(Of EncounterPokemon)

        Public PokemonCaught As Integer = 0
        Public PokemonSeen As Integer = 0

        Public Sub New(ByVal file As String)
            Security.FileValidation.CheckFileValid(file, False, "PokedexScreen.vb")
            Dim data() As String = System.IO.File.ReadAllLines(file)
            Me.MergeData = data
            Me.File = file

            For Each line As String In data
                If line.ToLower().StartsWith("name=") = True Then
                    Me.Name = line.Remove(0, 5)
                ElseIf line.ToLower().StartsWith("type=") = True Then
                    Dim arg As String = line.Remove(0, 5)
                    Select Case arg.ToLower()
                        Case "grassland"
                            Me.HabitatType = HabitatTypes.Grassland
                        Case "forest"
                            Me.HabitatType = HabitatTypes.Forest
                        Case "watersedge"
                            Me.HabitatType = HabitatTypes.WatersEdge
                        Case "sea"
                            Me.HabitatType = HabitatTypes.Sea
                        Case "cave"
                            Me.HabitatType = HabitatTypes.Cave
                        Case "mountain"
                            Me.HabitatType = HabitatTypes.Mountain
                        Case "roughterrain"
                            Me.HabitatType = HabitatTypes.RoughTerrain
                        Case "city"
                            Me.HabitatType = HabitatTypes.City
                    End Select
                ElseIf line.StartsWith("{") = True And line.EndsWith("}") = True Then
                    Dim pokemonData() As String = line.Remove(line.Length - 1, 1).Remove(0, 1).Split(CChar("|"))

                    If Me.PokemonList.Contains(pokemonData(1)) = False Then
                        Me.PokemonList.Add(pokemonData(1))

                        Dim entryType As Integer = Pokedex.GetEntryType(Core.Player.PokedexData, pokemonData(1))

                        If entryType > 0 Then
                            Me.PokemonSeen += 1
                        End If
                        If entryType > 1 Then
                            Me.PokemonCaught += 1
                        End If
                    End If

                    Dim daytimesData As List(Of String) = pokemonData(3).Split(CChar(",")).ToList()
                    Dim dayTimes As New List(Of Integer)

                    For Each s As String In daytimesData
                        If StringHelper.IsNumeric(s) = True Then
                            Dim i As Integer = CInt(s)
                            If i > 0 Then
                                dayTimes.Add(i)
                            Else
                                dayTimes.Clear()
                                Exit For
                            End If
                        End If
                    Next

                    Me.ObtainTypeList.Add(New EncounterPokemon With {.PokemonID = pokemonData(1), .EncounterType = CInt(pokemonData(0)), .Daytimes = dayTimes.ToArray()})
                End If
            Next

            If Me.Name = "" Then
                Me.Name = System.IO.Path.GetFileNameWithoutExtension(file)
                Me.Name = Me.Name(0).ToString().ToUpper() & Me.Name.Remove(0, 1)
            End If
        End Sub

        Public ReadOnly Property Texture() As Texture2D
            Get
                Dim x As Integer = CInt(Me.HabitatType)
                Dim y As Integer = 0
                While x > 1
                    x -= 2
                    y += 1
                End While
                Return TextureManager.GetTexture("GUI\Menus\pokedexhabitat", New Rectangle(x * 64, y * 48, 64, 48), "")
            End Get
        End Property

        Public Shared Function GetEncounterTypeImage(ByVal encounterType As Integer) As Texture2D
            Dim x As Integer = 0
            Dim y As Integer = 4

            Select Case encounterType
                Case 0
                    x = 0
                    y = 3
                Case 1
                    x = 0
                    y = 4
                Case 2
                    x = 1
                    y = 0
                Case 3
                    x = 0
                    y = 0
                Case 31
                    x = 0
                    y = 1
                Case 32
                    x = 0
                    y = 2
            End Select

            Return TextureManager.GetTexture("GUI\Menus\pokedexhabitat", New Rectangle(128 + x * 32, y * 32, 32, 32), "")
        End Function

        Public ReadOnly Property ProgressTexture() As Texture2D
            Get
                If Me.PokemonCaught >= Me.PokemonList.Count Then
                    Return TextureManager.GetTexture("GUI\Menus\pokedexhabitat", New Rectangle(160, 160, 10, 10), "")
                Else
                    If Me.PokemonSeen >= Me.PokemonList.Count Then
                        Return TextureManager.GetTexture("GUI\Menus\pokedexhabitat", New Rectangle(160, 170, 10, 10), "")
                    End If
                End If
                Return Nothing
            End Get
        End Property

        Public Sub Merge(ByVal h As Habitat)
            Dim data() As String = h.MergeData

            For Each line As String In data
                If line.StartsWith("{") = True And line.EndsWith("}") = True Then
                    Dim pokemonData() As String = line.Remove(line.Length - 1, 1).Remove(0, 1).Split(CChar("|"))

                    If Me.PokemonList.Contains(pokemonData(1)) = False Then
                        Me.PokemonList.Add(pokemonData(1))

                        Dim entryType As Integer = Pokedex.GetEntryType(Core.Player.PokedexData, pokemonData(1))

                        If entryType > 0 Then
                            Me.PokemonSeen += 1
                        End If
                        If entryType > 1 Then
                            Me.PokemonCaught += 1
                        End If
                    End If

                    Me.ObtainTypeList.Add(New EncounterPokemon With {.PokemonID = pokemonData(1), .EncounterType = CInt(pokemonData(0))})
                End If
            Next
        End Sub

        Public Function HasPokemon(ByVal pokemonNumber As String) As Boolean
            Return Me.PokemonList.Contains(pokemonNumber)
        End Function

    End Class

End Class

Public Class PokedexViewScreen

    Inherits Screen

    Dim DexIndex As Integer = -1
    Dim Pokemon As Pokemon
    Dim texture As Texture2D
    Dim Page As Integer = 0
    Dim Forms As New List(Of String)
    Dim FormIndex As Integer = 0

    Dim EntryType As Integer = 0
    Dim _transitionOut As Boolean = False

    Dim yOffset As Integer = 0
    Dim FrontView As Boolean = True
    Dim ShinyView As Boolean = False

    Dim EvolutionLineConnections As New List(Of PokemonEvolutionLine)

    Dim GridMinimum As New Vector2(0, 0)
    Dim GridMaximum As New Vector2(0, 0)

    Dim HabitatList As New List(Of PokedexScreen.Habitat)

    Class PokemonEvolutionLine

        Public ConnectionList As New List(Of Tuple(Of Integer, Integer, Pokemon))

        Public Sub New(ByVal GridPositions As List(Of Vector2), ByVal PokemonIDs As List(Of String))
            If GridPositions.Count = PokemonIDs.Count Then
                For i = 0 To PokemonIDs.Count - 1
                    Dim DexID As Integer
                    Dim DexAD As String = ""

                    If PokemonIDs(i).Contains("_") Then
                        DexID = CInt(PokemonIDs(i).GetSplit(0, "_"))
                        DexAD = PokemonForms.GetAdditionalValueFromDataFile(PokemonIDs(i))
                    ElseIf PokemonIDs(i).Contains(";") Then
                        DexID = CInt(PokemonIDs(i).GetSplit(0, ";"))
                        DexAD = PokemonIDs(i).GetSplit(1, ";")
                    Else
                        DexID = CInt(PokemonIDs(i))
                    End If

                    Dim p As Pokemon = Pokemon.GetPokemonByID(DexID, DexAD)
                    Dim entry As New Tuple(Of Integer, Integer, Pokemon)(CInt(GridPositions(i).X), CInt(GridPositions(i).Y), p)
                    ConnectionList.Add(entry)
                Next
            End If
        End Sub

    End Class

    ''' <summary>
    ''' Creates a new instance of this class.
    ''' </summary>
    ''' <param name="currentScreen">The screen that is currently active.</param>
    ''' <param name="Pokemon">The Pokémon to display.</param>
    ''' <param name="transitionOut">If the screen should fade out when closed.</param>
    Public Sub New(ByVal currentScreen As Screen, ByVal Pokemon As Pokemon, ByVal transitionOut As Boolean, Optional DexIndex As Integer = -1)
        Me.PreScreen = currentScreen
        Me.Identification = Identifications.PokedexViewScreen
        Me.texture = TextureManager.GetTexture("GUI\Menus\General")

        Me.MouseVisible = True
        Me.CanMuteAudio = True
        Me.CanBePaused = True
        Me._transitionOut = transitionOut

        If Me.Forms.Count > 0 Then
            Me.Forms.Clear()
            Me.Forms.Add(Me.Pokemon.Number.ToString)
            Me.FormIndex = 0
        End If

        LoadPokemonData(DexIndex, Pokemon)

    End Sub

    Private Sub LoadPokemonData(ByVal newDexIndex As Integer, Optional ByVal newPokemon As Pokemon = Nothing, Optional playCry As Boolean = False)

        If newPokemon IsNot Nothing Then
            EvolutionLineConnections.Clear()
            Me.Pokemon = newPokemon
        End If
        If Me.PreScreen.Identification = Identifications.PokedexScreen Then
            Dim PDexScreen As PokedexScreen = CType(Me.PreScreen, PokedexScreen)
            If newDexIndex <> -1 AndAlso newDexIndex <> Me.DexIndex Then
                Me.Pokemon = PDexScreen.PokemonList(newDexIndex)
                Me.DexIndex = newDexIndex
                Me.Forms.Clear()
                Me.EvolutionLineConnections.Clear()
                Me.Forms.Add(Me.Pokemon.Number.ToString)
                Me.FormIndex = 0
            End If

        End If


        Dim dexID As String = PokemonForms.GetPokemonDataFileName(Me.Pokemon.Number, Me.Pokemon.AdditionalData)
        If dexID.Contains("_") = False Then
            If PokemonForms.GetAdditionalDataForms(Me.Pokemon.Number) IsNot Nothing AndAlso PokemonForms.GetAdditionalDataForms(Me.Pokemon.Number).Contains(Me.Pokemon.AdditionalData) Then
                dexID = Me.Pokemon.Number & ";" & Me.Pokemon.AdditionalData
            Else
                dexID = Me.Pokemon.Number.ToString
            End If
        End If

        Me.EntryType = Pokedex.GetEntryType(Core.Player.PokedexData, dexID)

        If dexID.Contains("_") = False AndAlso dexID.Contains(";") = False Then
            If EntryType = 0 Then
                Dim formEntry As Integer = Pokedex.HasAnyForm(Me.Pokemon.Number)
                If formEntry > 0 Then
                    Me.EntryType = formEntry
                End If
            End If
        End If

        Me.GetYOffset()
        Me.FillHabitats()
        Me.FillEvolutionGrid()

        If playCry = True Then
            Dim crySuffix As String = PokemonForms.GetCrySuffix(Me.Pokemon)
            SoundManager.PlayPokemonCry(Pokemon.Number, crySuffix)
        End If

    End Sub

    Private Sub FillEvolutionGrid()
        If Me.Pokemon.EvolutionLines.Count > 0 Then
            For e = 0 To Me.Pokemon.EvolutionLines.Count - 1
                Dim GridPositions As New List(Of Vector2)
                Dim PokemonIDs As New List(Of String)
                Dim DataEntries As String() = Me.Pokemon.EvolutionLines(e).Split(",")
                For i = 0 To DataEntries.Count - 1
                    PokemonIDs.Add(DataEntries(i).GetSplit(0, "\"))
                    Dim Position As New Vector2(CInt(DataEntries(i).GetSplit(1, "\")))
                    If DataEntries(i).Split("\").Count > 2 Then
                        Position.Y = CInt(DataEntries(i).GetSplit(2, "\"))
                    End If

                    If GridMinimum.X > Position.X Then
                        GridMinimum.X = Position.X
                    End If
                    If GridMinimum.Y > Position.Y Then
                        GridMinimum.Y = Position.Y
                    End If

                    If GridMaximum.X < Position.X Then
                        GridMaximum.X = Position.X
                    End If
                    If GridMaximum.Y < Position.Y Then
                        GridMaximum.Y = Position.Y
                    End If

                    GridPositions.Add(Position)
                Next
                Dim evoline As New PokemonEvolutionLine(GridPositions, PokemonIDs)
                EvolutionLineConnections.Add(evoline)

                For Each f As String In PokemonIDs
                    If CInt(f.GetSplit(0, "_").GetSplit(0, ";")) = Me.Pokemon.Number Then
                        If Me.Forms.Contains(f) = False Then
                            Me.Forms.Add(f)
                        End If
                    End If
                Next
            Next
        Else
            Dim GridPositions As New List(Of Vector2)
            Dim PokemonIDs As New List(Of String)

            If Me.Pokemon.Devolution <> "0" Then
                Dim DevoID As Integer = CInt(Me.Pokemon.Devolution.GetSplit(0, "_").GetSplit(0, ";"))
                Dim DevoAD As String = ""

                If Me.Pokemon.Devolution.Contains("_") Then
                    DevoAD = PokemonForms.GetAdditionalValueFromDataFile(Me.Pokemon.Devolution)
                ElseIf Me.Pokemon.Devolution.Contains(";") Then
                    DevoAD = Me.Pokemon.Devolution.GetSplit(1, ";")
                End If

                Dim DevoP As Pokemon = Pokemon.GetPokemonByID(DevoID, DevoAD, True)

                If DevoP.Devolution <> "0" Then
                    PokemonIDs.Add(DevoP.Devolution)
                End If
                PokemonIDs.Add(Me.Pokemon.Devolution)
            End If

            Dim DexData As String = PokemonForms.GetPokemonDataFileName(Me.Pokemon.Number, Me.Pokemon.AdditionalData, True)

            PokemonIDs.Add(DexData)

            If Me.Pokemon.EvolutionConditions.Count > 0 Then
                PokemonIDs.Add(Me.Pokemon.EvolutionConditions(0).Evolution)

                Dim EvoID As Integer = CInt(Me.Pokemon.EvolutionConditions(0).Evolution.GetSplit(0, "_").GetSplit(0, ";"))
                Dim EvoAD As String = ""

                If Me.Pokemon.EvolutionConditions(0).Evolution.Contains("_") Then
                    EvoAD = PokemonForms.GetAdditionalValueFromDataFile(Me.Pokemon.EvolutionConditions(0).Evolution)
                ElseIf Me.Pokemon.EvolutionConditions(0).Evolution.Contains(";") Then
                    EvoAD = Me.Pokemon.EvolutionConditions(0).Evolution.GetSplit(1, ";")
                End If

                Dim EvoP As Pokemon = Pokemon.GetPokemonByID(EvoID, EvoAD, True)

                If EvoP.EvolutionConditions.Count > 0 Then
                    PokemonIDs.Add(EvoP.EvolutionConditions(0).Evolution)
                End If
            End If

            Dim CenterIndex As Integer = 0
            For i = 0 To PokemonIDs.Count - 1
                If CInt(PokemonIDs(i).GetSplit(0, "_").GetSplit(0, ";")) = Me.Pokemon.Number Then
                    CenterIndex = i
                End If
            Next
            Select Case PokemonIDs.Count
                Case 1
                    GridMinimum = New Vector2(CenterIndex)
                    GridMaximum = New Vector2(CenterIndex)

                    GridPositions.Add(New Vector2(CenterIndex, 0))
                Case 2
                    GridMinimum = New Vector2(0 - 2 * CenterIndex, 0)
                    GridMaximum = New Vector2(2 - 2 * CenterIndex, 0)
                    GridPositions.Add(New Vector2(0 - 2 * CenterIndex, 0))
                    GridPositions.Add(New Vector2(2 - 2 * CenterIndex, 0))
                Case 3
                    GridMinimum = New Vector2(0 - 2 * CenterIndex, 0)
                    GridMaximum = New Vector2(4 - 2 * CenterIndex, 0)

                    GridPositions.Add(New Vector2(0 - 2 * CenterIndex, 0))
                    GridPositions.Add(New Vector2(2 - 2 * CenterIndex, 0))
                    GridPositions.Add(New Vector2(4 - 2 * CenterIndex, 0))
                Case 4
                    GridMinimum = New Vector2(0 - 2 * CenterIndex, 0)
                    GridMaximum = New Vector2(6 - 2 * CenterIndex, 0)

                    GridPositions.Add(New Vector2(0 - 2 * CenterIndex, 0))
                    GridPositions.Add(New Vector2(2 - 2 * CenterIndex, 0))
                    GridPositions.Add(New Vector2(4 - 2 * CenterIndex, 0))
                    GridPositions.Add(New Vector2(6 - 2 * CenterIndex, 0))
                Case 5
                    GridMinimum = New Vector2(0 - 2 * CenterIndex, 0)
                    GridMaximum = New Vector2(8 - 2 * CenterIndex, 0)

                    GridPositions.Add(New Vector2(0 - 2 * CenterIndex, 0))
                    GridPositions.Add(New Vector2(2 - 2 * CenterIndex, 0))
                    GridPositions.Add(New Vector2(4 - 2 * CenterIndex, 0))
                    GridPositions.Add(New Vector2(6 - 2 * CenterIndex, 0))
                    GridPositions.Add(New Vector2(8 - 2 * CenterIndex, 0))
            End Select

            Dim evoline As New PokemonEvolutionLine(GridPositions, PokemonIDs)
            EvolutionLineConnections.Add(evoline)

        End If

        Logger.Debug("Minimum level: " & GridMinimum.X & "x," & GridMinimum.Y & "y; Maximum level: " & GridMaximum.X & "x," & GridMaximum.Y & "y")
    End Sub

    Private Sub FillHabitats()
        For Each file As String In System.IO.Directory.GetFiles(GameController.GamePath & GameModeManager.ActiveGameMode.PokeFilePath, "*.*", IO.SearchOption.AllDirectories)
            If file.EndsWith(".poke") = True Then
                Dim fileName As String = file.Remove(0, (GameController.GamePath & GameModeManager.ActiveGameMode.PokeFilePath & "\").Length - 1)
                Dim newHabitat As New PokedexScreen.Habitat(file)
                Dim exists As Boolean = False
                For Each h As PokedexScreen.Habitat In Me.HabitatList
                    If h.Name.ToLower() = newHabitat.Name.ToLower() Then
                        exists = True
                        h.Merge(newHabitat)
                        Exit For
                    End If
                Next
                If exists = False AndAlso Core.Player.PokeFiles.Contains(fileName) = True Then
                    HabitatList.Add(New PokedexScreen.Habitat(file))
                End If
            End If
        Next
        Me.HabitatList = (From h As PokedexScreen.Habitat In Me.HabitatList Order By h.Name Ascending).ToList()

        For i = 0 To Me.HabitatList.Count - 1
            If i <= Me.HabitatList.Count - 1 Then
                Dim dexID As String = PokemonForms.GetPokemonDataFileName(Me.Pokemon.Number, Me.Pokemon.AdditionalData)
                If Me.HabitatList(i).HasPokemon(dexID) = False Then
                    Me.HabitatList.RemoveAt(i)
                    i -= 1
                End If
            End If
        Next
    End Sub

    Private Sub GetYOffset()
        Dim t As Texture2D = Pokemon.GetTexture(FrontView, ShinyView)
        Me.yOffset = -1

        Dim cArr(t.Width * t.Height - 1) As Color
        t.GetData(cArr)

        For y = 0 To t.Height - 1
            For x = 0 To t.Width - 1
                If cArr(x + y * t.Height) <> Color.Transparent Then
                    Me.yOffset = y
                    Exit For
                End If
            Next

            If Me.yOffset <> -1 Then
                Exit For
            End If
        Next
    End Sub

    Public Overrides Sub Draw()
        Canvas.DrawRectangle(Core.windowSize, New Color(84, 198, 216))

        For y = -64 To Core.windowSize.Height Step 64
            Core.SpriteBatch.Draw(Me.texture, New Rectangle(Core.windowSize.Width - 128, y + PokedexScreen.TileOffset, 128, 64), New Rectangle(48, 0, 16, 16), Color.White)
        Next

        Canvas.DrawGradient(New Rectangle(0, 0, CInt(Core.windowSize.Width), 200), New Color(42, 167, 198), New Color(42, 167, 198, 0), False, -1)
        Canvas.DrawGradient(New Rectangle(0, CInt(Core.windowSize.Height - 200), CInt(Core.windowSize.Width), 200), New Color(42, 167, 198, 0), New Color(42, 167, 198), False, -1)

        Select Case Me.Page
            Case 0
                DrawPage1()
            Case 1
                DrawPage2()
            Case 2
                DrawPage3()
        End Select

        Core.SpriteBatch.Draw(Me.texture, New Rectangle(20, 20, 64, 64), New Rectangle(16, 16, 16, 16), Color.White)

        Core.SpriteBatch.Draw(Me.texture, New Rectangle(20 + 64, 20, 64 * 8, 64), New Rectangle(32, 16, 16, 16), Color.White)
        Core.SpriteBatch.Draw(Me.texture, New Rectangle(20 + 64 * 9, 20, 64, 64), New Rectangle(16, 16, 16, 16), Color.White, 0.0F, Vector2.Zero, SpriteEffects.FlipHorizontally, 0.0F)

        Core.SpriteBatch.Draw(Me.texture, New Rectangle(20 + 64, 20, 64 * 5, 64), New Rectangle(32, 16, 16, 16), Color.White)
        Core.SpriteBatch.Draw(Me.texture, New Rectangle(20 + 64 * 6, 20, 64, 64), New Rectangle(16, 16, 16, 16), Color.White, 0.0F, Vector2.Zero, SpriteEffects.FlipHorizontally, 0.0F)

        Dim pokeTexture = Pokemon.GetMenuTexture()
        Dim pokeTextureScale As Vector2 = New Vector2(CSng(32 / pokeTexture.Width * 2), CSng(32 / pokeTexture.Height * 2))

        If EntryType > 0 Then
            Core.SpriteBatch.Draw(pokeTexture, New Rectangle(28, 20, CInt(pokeTexture.Width * pokeTextureScale.X), CInt(pokeTexture.Height * pokeTextureScale.Y)), Color.White)

            Core.SpriteBatch.DrawString(FontManager.MainFont, Pokemon.GetName(True), New Vector2(100, 36), Color.Black, 0.0F, Vector2.Zero, 1.0F, SpriteEffects.None, 0.0F)
        Else
            Core.SpriteBatch.Draw(pokeTexture, New Rectangle(28, 20, CInt(pokeTexture.Width * pokeTextureScale.X), CInt(pokeTexture.Height * pokeTextureScale.Y)), Color.Black)

            Core.SpriteBatch.DrawString(FontManager.MainFont, "???", New Vector2(100, 36), Color.Black, 0.0F, Vector2.Zero, 1.0F, SpriteEffects.None, 0.0F)
        End If

        If EntryType = 1 Then
            Core.SpriteBatch.Draw(TextureManager.GetTexture("GUI\Menus\pokedexhabitat", New Rectangle(160, 170, 10, 10), ""), New Rectangle(64 * 6 + 40, 42, 20, 20), Color.White)
        ElseIf EntryType > 1 Then
            Core.SpriteBatch.Draw(TextureManager.GetTexture("GUI\Menus\pokedexhabitat", New Rectangle(160, 160, 10, 10), ""), New Rectangle(64 * 6 + 40, 42, 20, 20), Color.White)
            If EntryType > 2 Then
                SpriteBatch.Draw(TextureManager.GetTexture("GUI\Menus\PokemonInfo"), New Rectangle(64 * 6 - 1, 42, 18, 18), New Rectangle(16, 0, 9, 9), Color.White)
            End If
        End If

        If Me.mLineLength = 100 Then
            If Me.Page = 0 Or Me.Page = 1 Then
                Core.SpriteBatch.Draw(Me.texture, New Rectangle(Core.windowSize.Width - 70, CInt(Core.windowSize.Height / 2 - 32), 64, 64), New Rectangle(0, 16, 16, 16), Color.White, 0.0F, Vector2.Zero, SpriteEffects.FlipHorizontally, 0.0F)
            End If
            If Me.Page = 1 Or Me.Page = 2 Then
                Core.SpriteBatch.Draw(Me.texture, New Rectangle(6, CInt(Core.windowSize.Height / 2 - 32), 64, 64), New Rectangle(0, 16, 16, 16), Color.White)
            End If
        End If

        If Me.Forms.Count > 1 Then
            Dim CanSwitchCount As Integer = 0
            For i = 0 To Me.Forms.Count - 1
                If Pokedex.GetEntryType(Core.Player.PokedexData, Me.Forms(i)) > 0 Then
                    CanSwitchCount += 1
                End If
            Next
            If CanSwitchCount > 1 OrElse CanSwitchCount = 1 AndAlso Pokedex.GetEntryType(Core.Player.PokedexData, PokemonForms.GetPokemonDataFileName(Me.Pokemon.Number, Me.Pokemon.AdditionalData, True)) = 0 Then
                Core.SpriteBatch.DrawString(FontManager.MainFont, Localization.GetString("pokedex_data_form_switch_hint", "Press [Shift] to switch forms"), New Vector2(CInt(windowSize.Width - FontManager.MainFont.MeasureString(Localization.GetString("pokedex_data_form_switch_hint", "Press [Shift] to switch forms")).X - 128), 36), Color.White, 0.0F, Vector2.Zero, 1.0F, SpriteEffects.None, 0.0F)
            End If
        End If

        Select Case Me.Page
            Case 0
                Core.SpriteBatch.DrawString(FontManager.MainFont, Localization.GetString("pokedex_data_page_details", "Details"), New Vector2(480, 36), Color.Black, 0.0F, Vector2.Zero, 1.0F, SpriteEffects.None, 0.0F)
            Case 1
                Core.SpriteBatch.DrawString(FontManager.MainFont, Localization.GetString("pokedex_data_page_habitat", "Habitat"), New Vector2(480, 36), Color.Black, 0.0F, Vector2.Zero, 1.0F, SpriteEffects.None, 0.0F)
            Case 2
                Core.SpriteBatch.DrawString(FontManager.MainFont, Localization.GetString("pokedex_data_page_evolution", "Evolution"), New Vector2(480, 36), Color.Black, 0.0F, Vector2.Zero, 1.0F, SpriteEffects.None, 0.0F)
        End Select
    End Sub

    Dim fadeMainImage As Integer = 0
    Dim vLineLength As Integer = 1
    Dim mLineLength As Integer = 1
    Dim playedCry As Boolean = False

    Private Sub DrawPage1()
        Dim v As Vector2 = Core.GetMiddlePosition(New Size(MathHelper.Min(Pokemon.GetTexture(Me.FrontView, ShinyView).Width * 4, 512), MathHelper.Min(Pokemon.GetTexture(Me.FrontView, ShinyView).Height * 4, 512)))

        Dim textureColor As Color = Color.White
        Dim pForms As List(Of String) = PokemonForms.GetAdditionalDataForms(Pokemon.Number)
        If EntryType = 0 AndAlso pForms Is Nothing Then
            textureColor = New Color(0, 0, 0, 0)
        End If

        If fadeMainImage = 255 Then
            Dim mV As Vector2 = Core.GetMiddlePosition(New Size(0, 0))

            Canvas.DrawLine(Color.Black, New Vector2(mV.X + 40, mV.Y - 40), New Vector2(mV.X + vLineLength, mV.Y - vLineLength), 2)
            Canvas.DrawLine(Color.Black, New Vector2(mV.X + 40, mV.Y + 40), New Vector2(mV.X + vLineLength, mV.Y + vLineLength), 2)
            Canvas.DrawLine(Color.Black, New Vector2(mV.X - 40, mV.Y - 40), New Vector2(mV.X - vLineLength, mV.Y - vLineLength), 2)
            Canvas.DrawLine(Color.Black, New Vector2(mV.X - 40, mV.Y + 40), New Vector2(mV.X - vLineLength, mV.Y + vLineLength), 2)

            If vLineLength = 140 Then
                Canvas.DrawLine(Color.Black, New Vector2(mV.X + 140, mV.Y - 140), New Vector2(mV.X + (140 + mLineLength), mV.Y - 140), 2)
                Canvas.DrawLine(Color.Black, New Vector2(mV.X + 139, mV.Y + 140), New Vector2(mV.X + (140 + mLineLength), mV.Y + 140), 2)
                Canvas.DrawLine(Color.Black, New Vector2(mV.X - 139, mV.Y - 140), New Vector2(mV.X - (140 + mLineLength), mV.Y - 140), 2)
                Canvas.DrawLine(Color.Black, New Vector2(mV.X - 139, mV.Y + 140), New Vector2(mV.X - (140 + mLineLength), mV.Y + 140), 2)
            End If

            If mLineLength = 100 Then
                If EntryType > 1 Then
                    Dim DexEntryText As String = Pokemon.PokedexEntry.Text
                    Dim DexEntrySpecies As String = Pokemon.PokedexEntry.Species

                    Dim FormName As String = PokemonForms.GetFormName(Pokemon)
                    If FormName = "" Then
                        FormName = Pokemon.Name
                    End If

                    If Localization.TokenExists("pokemon_desc_" & FormName) = True Then
                        DexEntryText = Localization.GetString("pokemon_desc_" & FormName, Pokemon.PokedexEntry.Text)
                    End If
                    If Localization.TokenExists("pokemon_species_" & FormName) = True Then
                        DexEntrySpecies = Localization.GetString("pokemon_species_" & FormName, Pokemon.PokedexEntry.Species)
                    End If

                    Core.SpriteBatch.DrawString(FontManager.MainFont, Pokemon.PokedexEntry.Height & " m", New Vector2(CInt(mV.X + 250), CInt(mV.Y - 152)), Color.Black)
                    Core.SpriteBatch.DrawString(FontManager.MainFont, Pokemon.PokedexEntry.Weight & " kg", New Vector2(CInt(mV.X + 250), CInt(mV.Y + 128)), Color.Black)
                    Core.SpriteBatch.DrawString(FontManager.MainFont, DexEntrySpecies, New Vector2(CInt(mV.X - 248 - FontManager.MainFont.MeasureString(DexEntrySpecies).X), CInt(mV.Y - 152)), Color.Black)
                    If Pokemon.Type2 IsNot Nothing AndAlso Pokemon.Type2.Type <> Element.Types.Blank Then
                        Core.SpriteBatch.Draw(TextureManager.GetTexture(Element.GetElementTexturePath()), New Rectangle(CInt(mV.X - 450), CInt(mV.Y + 123), 96, 32), Pokemon.Type1.GetElementImage(), Color.White)
                        Core.SpriteBatch.Draw(TextureManager.GetTexture(Element.GetElementTexturePath()), New Rectangle(CInt(mV.X - 350), CInt(mV.Y + 123), 96, 32), Pokemon.Type2.GetElementImage(), Color.White)
                    Else
                        Core.SpriteBatch.Draw(TextureManager.GetTexture(Element.GetElementTexturePath()), New Rectangle(CInt(mV.X - 350), CInt(mV.Y + 123), 96, 32), Pokemon.Type1.GetElementImage(), Color.White)
                    End If



                    Canvas.DrawRectangle(New Rectangle(CInt(mV.X - FontManager.MainFont.MeasureString(DexEntryText.CropStringToWidth(FontManager.MainFont, 720)).X / 2 - 16), CInt(mV.Y + 192 - 16), CInt(FontManager.MainFont.MeasureString(DexEntryText.CropStringToWidth(FontManager.MainFont, 720)).X + 32), CInt(FontManager.MainFont.MeasureString(DexEntryText.CropStringToWidth(FontManager.MainFont, 720)).Y + 32)), New Color(42, 167, 198, 150))
                    Core.SpriteBatch.DrawString(FontManager.MainFont, DexEntryText.CropStringToWidth(FontManager.MainFont, 720), New Vector2(CInt(mV.X - FontManager.MainFont.MeasureString(DexEntryText.CropStringToWidth(FontManager.MainFont, 720)).X / 2), CInt(mV.Y + 192)), Color.Black)

                Else
                    Core.SpriteBatch.DrawString(FontManager.MainFont, "??? m", New Vector2(CInt(mV.X + 250), CInt(mV.Y - 152)), Color.Black)
                    Core.SpriteBatch.DrawString(FontManager.MainFont, "??? kg", New Vector2(CInt(mV.X + 250), CInt(mV.Y + 128)), Color.Black)
                    Core.SpriteBatch.DrawString(FontManager.MainFont, "??? Pokémon", New Vector2(CInt(mV.X - 248 - FontManager.MainFont.MeasureString("??? Pokémon").X), CInt(mV.Y - 152)), Color.Black)
                    Core.SpriteBatch.DrawString(FontManager.MainFont, "???", New Vector2(CInt(mV.X - 248 - FontManager.MainFont.MeasureString("???").X), CInt(mV.Y + 128)), Color.Black)
                End If
            End If
        End If

        Core.SpriteBatch.Draw(Pokemon.GetTexture(Me.FrontView, ShinyView), New Rectangle(CInt(v.X), CInt(v.Y) - yOffset * 2 + 32, MathHelper.Min(Pokemon.GetTexture(Me.FrontView, ShinyView).Width * 4, 512), MathHelper.Min(Pokemon.GetTexture(Me.FrontView, ShinyView).Height * 4, 512)), New Color(textureColor.R, textureColor.G, textureColor.B, textureColor.A))

    End Sub

    Dim Scroll As Integer = 0
    Dim Cursor As Integer = 0

    Private Sub DrawPage2()
        If Me.HabitatList.Count = 0 Then
            Canvas.DrawGradient(New Rectangle(CInt(Core.windowSize.Width / 2) - 282, CInt(Core.windowSize.Height / 2 - 45), 80, 90), New Color(84, 198, 216), New Color(42, 167, 198, 150), True, -1)
            Canvas.DrawRectangle(New Rectangle(CInt(Core.windowSize.Width / 2) - 202, CInt(Core.windowSize.Height / 2 - 45), 404, 90), New Color(42, 167, 198, 150))
            Canvas.DrawGradient(New Rectangle(CInt(Core.windowSize.Width / 2) + 202, CInt(Core.windowSize.Height / 2 - 45), 80, 90), New Color(42, 167, 198, 150), New Color(84, 198, 216), True, -1)

            Core.SpriteBatch.DrawString(FontManager.MainFont, Localization.GetString("pokedex_habitat_area_unknown", "Area Unknown."), New Vector2(CInt(Core.windowSize.Width / 2) - CInt(FontManager.MainFont.MeasureString(Localization.GetString("pokedex_habitat_area_unknown", "Area Unknown.")).X / 2), CInt(Core.windowSize.Height / 2 - 15)), Color.White)
        Else
            For i = Scroll To Scroll + 4
                If i <= Me.HabitatList.Count - 1 Then
                    Dim p As Integer = i - Scroll

                    Core.SpriteBatch.Draw(Me.texture, New Rectangle(100, 160 + p * 96, 64, 64), New Rectangle(16, 16, 16, 16), Color.White)
                    Core.SpriteBatch.Draw(Me.texture, New Rectangle(100 + 64, 160 + p * 96, 64 * 9, 64), New Rectangle(32, 16, 16, 16), Color.White)
                    Core.SpriteBatch.Draw(Me.texture, New Rectangle(100 + 64 * 10, 160 + p * 96, 64, 64), New Rectangle(16, 16, 16, 16), Color.White, 0.0F, Vector2.Zero, SpriteEffects.FlipHorizontally, 0.0F)

                    Core.SpriteBatch.Draw(HabitatList(i).Texture, New Rectangle(120, 168 + p * 96, 64, 48), Color.White)
                    Core.SpriteBatch.DrawString(FontManager.MainFont, Localization.GetString("Places_" & HabitatList(i).Name, HabitatList(i).Name), New Vector2(200, 176 + p * 96), Color.Black, 0.0F, Vector2.Zero, 1.0F, SpriteEffects.None, 0.0F)

                    Dim encounterTypes As New List(Of Integer)
                    For j = 0 To HabitatList(i).ObtainTypeList.Count - 1
                        If HabitatList(i).ObtainTypeList(j).PokemonID = PokemonForms.GetPokemonDataFileName(Me.Pokemon.Number, Me.Pokemon.AdditionalData) And encounterTypes.Contains(HabitatList(i).ObtainTypeList(j).EncounterType) = False Then
                            encounterTypes.Add(HabitatList(i).ObtainTypeList(j).EncounterType)
                        End If
                    Next

                    For j = 0 To encounterTypes.Count - 1
                        Core.SpriteBatch.Draw(PokedexScreen.Habitat.GetEncounterTypeImage(encounterTypes(j)), New Rectangle(560 + j * 40, 176 + p * 96, 32, 32), Color.White)
                    Next
                End If
            Next

            DrawCursor()
        End If
    End Sub

    Private Sub DrawCursor()
        Dim cPosition As Vector2 = New Vector2(520, 160 + Me.Cursor * 96 - 42)

        Dim t As Texture2D = TextureManager.GetTexture("GUI\Menus\General", New Rectangle(0, 0, 16, 16), "")
        Core.SpriteBatch.Draw(t, New Rectangle(CInt(cPosition.X), CInt(cPosition.Y), 64, 64), Color.White)
    End Sub

    Dim scale As Single = 2.0F

    Private Sub DrawPage3()
        If EvolutionLineConnections Is Nothing OrElse EvolutionLineConnections.Count = 0 OrElse EvolutionLineConnections.Count = 1 And EvolutionLineConnections(0).ConnectionList.Count <= 1 Then
            Canvas.DrawGradient(New Rectangle(CInt(Core.windowSize.Width / 2) - 282, CInt(Core.windowSize.Height / 2 - 45), 80, 90), New Color(84, 198, 216), New Color(42, 167, 198, 150), True, -1)
            Canvas.DrawRectangle(New Rectangle(CInt(Core.windowSize.Width / 2) - 202, CInt(Core.windowSize.Height / 2 - 45), 404, 90), New Color(42, 167, 198, 150))
            Canvas.DrawGradient(New Rectangle(CInt(Core.windowSize.Width / 2) + 202, CInt(Core.windowSize.Height / 2 - 45), 80, 90), New Color(42, 167, 198, 150), New Color(84, 198, 216), True, -1)

            Core.SpriteBatch.DrawString(FontManager.MainFont, Pokemon.GetName(True) & " " & Localization.GetString("pokedex_evolution_no_evolutions", "doesn't evolve."), New Vector2(CInt(Core.windowSize.Width / 2) - CInt(FontManager.MainFont.MeasureString(Pokemon.GetName(True) & " " & Localization.GetString("pokedex_evolution_no_evolutions", "doesn't evolve.")).X / 2), CInt(Core.windowSize.Height / 2 - 15)), Color.White)
        Else
            Dim connectionLines As New List(Of String)
            Dim centerVector As Vector2 = Core.GetMiddlePosition(New Size(CInt(64 * scale), CInt(64 * scale)))

            For e = 0 To EvolutionLineConnections.Count - 1
                For l = 0 To EvolutionLineConnections(e).ConnectionList.Count - 1
                    If EvolutionLineConnections(e).ConnectionList.Count > 1 AndAlso l > 0 Then
                        connectionLines.Add(CStr(EvolutionLineConnections(e).ConnectionList(l - 1).Item1 & "_" & EvolutionLineConnections(e).ConnectionList(l - 1).Item2 & "," & EvolutionLineConnections(e).ConnectionList(l).Item1 & "_" & EvolutionLineConnections(e).ConnectionList(l).Item2))
                    End If
                Next
            Next

            'Draw Lines
            For i = 0 To connectionLines.Count - 1
                Dim LineStart As New Vector2(CInt(connectionLines(i).GetSplit(0, ",").GetSplit(0, "_")), CInt(connectionLines(i).GetSplit(0, ",").GetSplit(1, "_")))
                Dim LineEnd As New Vector2(CInt(connectionLines(i).GetSplit(1, ",").GetSplit(0, "_")), CInt(connectionLines(i).GetSplit(1, ",").GetSplit(1, "_")))

                Canvas.DrawLine(Color.Black, New Vector2(centerVector.X + (LineStart.X * (64 * scale)) + (scale * 32), centerVector.Y + (scale * 32) + (LineStart.Y * (48 * scale))), New Vector2(centerVector.X + (LineEnd.X * (64 * scale)) + (scale * 32), centerVector.Y + (scale * 32) + (LineEnd.Y * (48 * scale))), 2)
            Next

            'Draw Sprites
            For x = CInt(GridMinimum.X) To CInt(GridMaximum.X)
                For y = CInt(GridMinimum.Y) To CInt(GridMaximum.Y)
                    Dim pokemon As Pokemon = Nothing
                    Dim position As Vector2 = New Vector2(0)
                    For c = 0 To EvolutionLineConnections.Count - 1
                        For i = 0 To EvolutionLineConnections(c).ConnectionList.Count - 1
                            If EvolutionLineConnections(c).ConnectionList(i).Item1 = x AndAlso EvolutionLineConnections(c).ConnectionList(i).Item2 = y Then
                                position = New Vector2(EvolutionLineConnections(c).ConnectionList(i).Item1, EvolutionLineConnections(c).ConnectionList(i).Item2)
                                pokemon = EvolutionLineConnections(c).ConnectionList(i).Item3
                            End If
                        Next
                    Next
                    If pokemon IsNot Nothing Then
                        Dim dexID As String = PokemonForms.GetPokemonDataFileName(pokemon.Number, pokemon.AdditionalData, True)
                        Dim pokeTexture = pokemon.GetMenuTexture()
                        Dim pokeTextureScale As Vector2 = New Vector2(CSng(32 / pokeTexture.Width * 2), CSng(32 / pokeTexture.Height * 2))

                        If Pokedex.GetEntryType(Core.Player.PokedexData, dexID) = 0 Then
                            Core.SpriteBatch.Draw(pokeTexture, New Rectangle(CInt(centerVector.X + (position.X * CInt(64 * scale))), CInt(centerVector.Y + (position.Y * (48 * scale))), CInt(pokeTexture.Width * pokeTextureScale.X * scale), CInt(pokeTexture.Height * pokeTextureScale.Y * scale)), Color.Black)
                        Else
                            Core.SpriteBatch.Draw(pokeTexture, New Rectangle(CInt(centerVector.X + (position.X * CInt(64 * scale))), CInt(centerVector.Y + (position.Y * (48 * scale))), CInt(pokeTexture.Width * pokeTextureScale.X * scale), CInt(pokeTexture.Height * pokeTextureScale.Y * scale)), Color.White)
                        End If
                    End If
                Next
            Next

            'Draw Names
            For x = CInt(GridMinimum.X) To CInt(GridMaximum.X)
                For y = CInt(GridMinimum.Y) To CInt(GridMaximum.Y)
                    Dim pokemon As Pokemon = Nothing
                    Dim position As Vector2 = New Vector2(0)
                    For c = 0 To EvolutionLineConnections.Count - 1
                        For i = 0 To EvolutionLineConnections(c).ConnectionList.Count - 1
                            If EvolutionLineConnections(c).ConnectionList(i).Item1 = x AndAlso EvolutionLineConnections(c).ConnectionList(i).Item2 = y Then
                                position = New Vector2(EvolutionLineConnections(c).ConnectionList(i).Item1, EvolutionLineConnections(c).ConnectionList(i).Item2)
                                pokemon = EvolutionLineConnections(c).ConnectionList(i).Item3
                            End If
                        Next
                    Next
                    If pokemon IsNot Nothing Then
                        Dim dexID As String = PokemonForms.GetPokemonDataFileName(pokemon.Number, pokemon.AdditionalData, True)
                        Dim pokeTexture = pokemon.GetMenuTexture()
                        Dim pokeTextureScale As Vector2 = New Vector2(CSng(32 / pokeTexture.Width * 2), CSng(32 / pokeTexture.Height * 2))

                        If Pokedex.GetEntryType(Core.Player.PokedexData, dexID) <> 0 Then
                            Core.SpriteBatch.DrawString(FontManager.MainFont, pokemon.GetName(True), New Vector2(CInt(centerVector.X + (position.X * (64 * scale)) + CInt(pokeTexture.Width * pokeTextureScale.X / 2 * scale) - (FontManager.MainFont.MeasureString(pokemon.GetName(True)).X / 2 * CSng(scale / 2)) + 2), CInt(centerVector.Y + position.Y * (48 * scale) + (64 * scale)) + 2), Color.Black, 0.0F, Vector2.Zero, CInt(scale / 2), SpriteEffects.None, 0.0F)
                            Core.SpriteBatch.DrawString(FontManager.MainFont, pokemon.GetName(True), New Vector2(CInt(centerVector.X + (position.X * (64 * scale)) + CInt(pokeTexture.Width * pokeTextureScale.X / 2 * scale) - (FontManager.MainFont.MeasureString(pokemon.GetName(True)).X / 2 * CSng(scale / 2))), CInt(centerVector.Y + position.Y * (48 * scale) + (64 * scale))), Color.White, 0.0F, Vector2.Zero, CInt(scale / 2), SpriteEffects.None, 0.0F)
                        End If
                    End If
                Next
            Next

        End If
    End Sub

    Public Overrides Sub Update()
        If Controls.Dismiss(True, True, True) = True Then
            If Me._transitionOut = True Then
                SoundManager.PlaySound("select")
                Core.SetScreen(New TransitionScreen(Me, Me.PreScreen, Color.White, False))
            Else
                SoundManager.PlaySound("select")
                Core.SetScreen(Me.PreScreen)
            End If
        End If

        If DexIndex > -1 AndAlso Me.PreScreen.Identification = Identifications.PokedexScreen Then
            If Controls.Up(True, True, False, True, True, True) = True Then
                Dim PDexScreen As PokedexScreen = CType(Me.PreScreen, PokedexScreen)
                If Me.DexIndex > 0 Then
                    Dim index As Integer = Me.DexIndex - 1

                    Dim pokeID As String = PokemonForms.GetPokemonDataFileName(PDexScreen.PokemonList(index).Number, PDexScreen.PokemonList(index).AdditionalData, True)
                    Dim newID As String = ""
                    While newID = "" AndAlso index > 0
                        If Pokedex.GetEntryType(Core.Player.PokedexData, pokeID) > 0 OrElse Pokedex.HasAnyForm(PDexScreen.PokemonList(index).Number) > 0 Then
                            newID = pokeID
                        End If
                        Dim formEntry As Integer = Pokedex.HasAnyForm(PDexScreen.PokemonList(index).Number)
                        If formEntry > 0 Then
                            newID = PDexScreen.PokemonList(index).Number.ToString
                            Exit While
                        Else
                            index -= 1
                            pokeID = PokemonForms.GetPokemonDataFileName(PDexScreen.PokemonList(index).Number, PDexScreen.PokemonList(index).AdditionalData, True)
                            If Pokedex.GetEntryType(Core.Player.PokedexData, pokeID) > 0 Then
                                newID = pokeID
                                Exit While
                            End If
                        End If
                    End While

                    If newID <> "" Then
                        If Pokedex.GetEntryType(Core.Player.PokedexData, newID) > 0 OrElse Pokedex.HasAnyForm(PDexScreen.PokemonList(index).Number) > 0 Then
                            vLineLength = 1
                            mLineLength = 1
                            fadeMainImage = 0
                            LoadPokemonData(index, Nothing, True)
                        End If
                    End If
                End If
            End If

            If Controls.Down(True, True, False, True, True, True) = True Then
                Dim PDexScreen As PokedexScreen = CType(Me.PreScreen, PokedexScreen)
                If Me.DexIndex < PDexScreen.PokemonList.count - 1 Then
                    Dim index As Integer = Me.DexIndex + 1

                    Dim pokeID As String = PokemonForms.GetPokemonDataFileName(PDexScreen.PokemonList(index).Number, PDexScreen.PokemonList(index).AdditionalData, True)
                    Dim newID As String = ""
                    While newID = "" And index < PDexScreen.PokemonList.Count - 1
                        If Pokedex.GetEntryType(Core.Player.PokedexData, pokeID) > 0 OrElse Pokedex.HasAnyForm(PDexScreen.PokemonList(index).Number) > 0 Then
                            newID = pokeID
                        End If
                        Dim formEntry As Integer = Pokedex.HasAnyForm(PDexScreen.PokemonList(index).Number)
                        If formEntry > 0 Then
                            newID = PDexScreen.PokemonList(index).Number.ToString
                            Exit While
                        Else
                            index += 1
                            pokeID = PokemonForms.GetPokemonDataFileName(PDexScreen.PokemonList(index).Number, PDexScreen.PokemonList(index).AdditionalData, True)
                            If Pokedex.GetEntryType(Core.Player.PokedexData, pokeID) > 0 Then
                                newID = pokeID
                                Exit While
                            End If
                        End If
                    End While

                    If newID <> "" Then
                        If Pokedex.GetEntryType(Core.Player.PokedexData, newID) > 0 OrElse Pokedex.HasAnyForm(PDexScreen.PokemonList(index).Number) > 0 Then
                            vLineLength = 1
                            mLineLength = 1
                            fadeMainImage = 0
                            LoadPokemonData(index, Nothing, True)
                        End If
                    End If
                End If
            End If
        End If

        If Controls.ShiftPressed = True Or ControllerHandler.ButtonPressed(Buttons.Back) Then
            If Me.Forms.Count > 0 Then

                Dim OriginalFormIndex As Integer = Me.FormIndex
                Me.FormIndex += 1

                If Me.FormIndex > Me.Forms.Count - 1 Then
                    Me.FormIndex = 0
                End If

                Dim formID As String = Me.Forms(Me.FormIndex)
                If formID <> "" AndAlso Pokedex.GetEntryType(Core.Player.PokedexData, formID) = 0 Then
                    While Pokedex.GetEntryType(Core.Player.PokedexData, formID) = 0
                        Me.FormIndex += 1

                        If Me.FormIndex > Me.Forms.Count - 1 Then
                            Me.FormIndex = 0
                        End If
                        formID = Me.Forms(Me.FormIndex)
                        If FormIndex = OriginalFormIndex Then
                            Exit While
                        End If
                    End While
                End If

                If Me.Forms(Me.FormIndex) IsNot "" Then
                    Dim PokeID As Integer = CInt(Me.Forms(Me.FormIndex).GetSplit(0, "_").GetSplit(0, ";"))
                    Dim PokeAD As String = ""

                    If Me.Forms(Me.FormIndex).Contains("_") Then
                        PokeAD = PokemonForms.GetAdditionalValueFromDataFile(Me.Forms(Me.FormIndex))
                    ElseIf Me.Forms(Me.FormIndex).Contains(";") Then
                        PokeAD = Me.Forms(Me.FormIndex).GetSplit(1, ";")
                    End If

                    Dim newPokemon As Pokemon = Pokemon.GetPokemonByID(PokeID, PokeAD, True)

                    Dim playCry As Boolean = True
                    If FormIndex = OriginalFormIndex Then
                        playCry = False
                    End If
                    LoadPokemonData(-1, newPokemon, playCry)

                End If
            End If
        End If

        UpdateIntro()

        If mLineLength = 100 Then
            If Controls.Right(True, True, False, True, True, True) = True Then
                Me.Page += 1
            End If
            If Controls.Left(True, True, False, True, True, True) = True Then
                Me.Page -= 1
            End If

            If Controls.Accept(True, False, False) = True Then
                If Me.Page = 0 Or Me.Page = 1 Then
                    If New Rectangle(Core.windowSize.Width - 70, CInt(Core.windowSize.Height / 2 - 32), 64, 64).Contains(MouseHandler.MousePosition) = True Then
                        SoundManager.PlaySound("select")
                        Me.Page += 1
                    End If
                End If
                If Me.Page = 1 Or Me.Page = 2 Then
                    If New Rectangle(6, CInt(Core.windowSize.Height / 2 - 32), 64, 64).Contains(MouseHandler.MousePosition) = True Then
                        SoundManager.PlaySound("select")
                        Me.Page -= 1
                    End If
                End If
            End If

            Me.Page = Me.Page.Clamp(0, 2)

            Select Case Me.Page
                Case 0
                    UpdatePage1()
                Case 1
                    UpdatePage2()
                Case 2
                    UpdatePage3()
            End Select
        End If

        PokedexScreen.TileOffset += 1
        If PokedexScreen.TileOffset >= 64 Then
            PokedexScreen.TileOffset = 0
        End If
    End Sub

    Private Sub UpdateIntro()
        If fadeMainImage < 255 Then
            fadeMainImage += 10
            If fadeMainImage >= 255 Then
                fadeMainImage = 255
                If playedCry = False Then
                    playedCry = True
                    Dim crySuffix As String = PokemonForms.GetCrySuffix(Me.Pokemon)
                    SoundManager.PlayPokemonCry(Pokemon.Number, crySuffix)
                End If
            End If
        Else
            If vLineLength < 140 Then
                vLineLength += 10
                If vLineLength >= 140 Then
                    vLineLength = 140
                End If
            Else
                If mLineLength < 100 Then
                    mLineLength += 10
                    If mLineLength >= 100 Then
                        mLineLength = 100
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub UpdatePage1()
        If Controls.Accept(True, True, True) = True Then
            SoundManager.PlaySound("select")

            If EntryType > 2 Then
                If Me.FrontView = False Then
                    Me.ShinyView = Not Me.ShinyView
                End If
                Me.FrontView = Not Me.FrontView
            Else
                Me.FrontView = Not Me.FrontView
            End If


            Me.GetYOffset()
        End If
    End Sub

    Private Sub UpdatePage2()
        If Me.HabitatList.Count > 0 Then
            If Controls.Down(True, True, True, True, True, True) = True Then
                Me.Cursor += 1
            End If
            If Controls.Up(True, True, True, True, True, True) = True Then
                Me.Cursor -= 1
            End If

            If Me.Cursor > 4 Then
                Me.Cursor = 4
                Me.Scroll += 1
            End If
            If Me.Cursor < 0 Then
                Me.Cursor = 0
                Me.Scroll -= 1
            End If

            If Me.HabitatList.Count < 6 Then
                Me.Scroll = 0
            Else
                Me.Scroll = Me.Scroll.Clamp(0, Me.HabitatList.Count - 5)
            End If

            If Me.HabitatList.Count < 5 Then
                Me.Cursor = Me.Cursor.Clamp(0, Me.HabitatList.Count - 1)
            Else
                Me.Cursor = Me.Cursor.Clamp(0, 4)
            End If

            If Controls.Accept(True, False, False) = True Then
                For i = Scroll To Scroll + 4
                    If i <= Me.HabitatList.Count - 1 Then
                        If New Rectangle(100, 160 + (i - Scroll) * 96, 640, 64).Contains(MouseHandler.MousePosition) = True Then
                            If i = Cursor + Scroll Then
                                SoundManager.PlaySound("select")
                                Core.SetScreen(New PokedexScreen(Me, Nothing, Me.HabitatList(Cursor + Scroll)))
                            Else
                                Cursor = i - Scroll
                            End If
                        End If
                    End If
                Next
            End If

            If Controls.Accept(False, True, True) = True Then
                SoundManager.PlaySound("select")
                Core.SetScreen(New PokedexScreen(Me, Nothing, Me.HabitatList(Cursor + Scroll)))
            End If
        End If
    End Sub

    Private Sub UpdatePage3()

        If Controls.Accept(True, False, False) = True AndAlso Me.DexIndex > -1 Then
            Dim centerVector As Vector2 = Core.GetMiddlePosition(New Size(CInt(64 * scale), CInt(64 * scale)))

            Dim pokemon As Pokemon = Nothing
            Dim position As Vector2 = New Vector2(0)
            Dim mPosition As Vector2 = MouseHandler.MousePosition.ToVector2
            For c = 0 To EvolutionLineConnections.Count - 1
                For i = 0 To EvolutionLineConnections(c).ConnectionList.Count - 1
                    Dim rect As New Rectangle(CInt(centerVector.X + (EvolutionLineConnections(c).ConnectionList(i).Item1 * CInt(64 * scale))), CInt(centerVector.Y + (EvolutionLineConnections(c).ConnectionList(i).Item2 * (48 * scale))), CInt(EvolutionLineConnections(c).ConnectionList(i).Item3.GetMenuTexture().Width * CSng(32 / EvolutionLineConnections(c).ConnectionList(i).Item3.GetMenuTexture().Width * 2) * scale), CInt(EvolutionLineConnections(c).ConnectionList(i).Item3.GetMenuTexture().Height * CSng(32 / EvolutionLineConnections(c).ConnectionList(i).Item3.GetMenuTexture().Height * 2) * scale))
                    If rect.Contains(mPosition) Then
                        pokemon = EvolutionLineConnections(c).ConnectionList(i).Item3
                    End If
                Next
            Next
            If pokemon IsNot Nothing Then
                Dim dexID As String = PokemonForms.GetPokemonDataFileName(pokemon.Number, pokemon.AdditionalData, True)

                Dim TempForms As New List(Of String)
                Dim switchIndex As Integer = DexIndex
                If pokemon.Number <> Me.Pokemon.Number Then
                    Dim PDexScreen As PokedexScreen = CType(Me.PreScreen, PokedexScreen)
                    For pEntry = 0 To PDexScreen.PokemonList.Count - 1
                        If PDexScreen.PokemonList(pEntry).Number = pokemon.Number AndAlso PDexScreen.PokemonList(pEntry).AdditionalData = pokemon.AdditionalData Then
                            switchIndex = pEntry
                            Exit For
                        End If
                    Next
                    If switchIndex = DexIndex Then
                        For pEntry = 0 To PDexScreen.PokemonList.Count - 1
                            If PDexScreen.PokemonList(pEntry).Number = pokemon.Number Then
                                switchIndex = pEntry
                                Exit For
                            End If
                        Next
                    End If
                    TempForms.Add(pokemon.Number.ToString)
                    If pokemon.EvolutionLines.Count > 0 Then
                        For e = 0 To pokemon.EvolutionLines.Count - 1
                            Dim PokemonIDs As New List(Of String)
                            Dim DataEntries As String() = pokemon.EvolutionLines(e).Split(",")
                            For i = 0 To DataEntries.Count - 1
                                PokemonIDs.Add(DataEntries(i).GetSplit(0, "\"))
                            Next
                            For Each f As String In PokemonIDs
                                If CInt(f.GetSplit(0, "_").GetSplit(0, ";")) = pokemon.Number Then
                                    If Me.Forms.Contains(f) = False Then
                                        TempForms.Add(f)
                                    End If
                                End If
                            Next
                        Next
                    Else
                        TempForms.AddRange(Me.Forms)
                    End If
                Else
                    If Me.Forms.Count > 0 Then
                        TempForms.AddRange(Me.Forms)
                    End If
                End If
                Dim fIndex As Integer = -1

                For f = 0 To TempForms.Count - 1
                    If TempForms(f) = dexID Then
                        If Pokedex.GetEntryType(Core.Player.PokedexData, dexID) > 0 Then
                            fIndex = f
                        End If
                    End If
                Next

                If fIndex <> -1 AndAlso TempForms(fIndex) IsNot "" Then
                    Me.FormIndex = fIndex
                    Dim PokeID As Integer = CInt(TempForms(Me.FormIndex).GetSplit(0, "_").GetSplit(0, ";"))
                    Dim PokeAD As String = ""

                    If TempForms(Me.FormIndex).Contains("_") Then
                        PokeAD = PokemonForms.GetAdditionalValueFromDataFile(TempForms(Me.FormIndex))
                    ElseIf TempForms(Me.FormIndex).Contains(";") Then
                        PokeAD = TempForms(Me.FormIndex).GetSplit(1, ";")
                    End If

                    Dim newPokemon As Pokemon = Pokemon.GetPokemonByID(PokeID, PokeAD, True)

                    Me.Forms = TempForms

                    LoadPokemonData(switchIndex, newPokemon, True)

                ElseIf Me.DexIndex > -1 Then
                    Dim PDexScreen As PokedexScreen = CType(Me.PreScreen, PokedexScreen)
                    If Pokedex.GetEntryType(Core.Player.PokedexData, PokemonForms.GetPokemonDataFileName(PDexScreen.PokemonList(switchIndex).Number, PDexScreen.PokemonList(switchIndex).AdditionalData, True)) > 0 Then
                        vLineLength = 1
                        mLineLength = 1
                        fadeMainImage = 0
                        LoadPokemonData(switchIndex, Nothing, True)
                    End If
                End If
            End If
        End If

        If Controls.Up(True, False, True, False, False, False) = True Or ControllerHandler.ButtonPressed(Buttons.RightTrigger) = True Or KeyBoardHandler.KeyPressed(Keys.OemPlus) = True Then
            Me.scale += 0.5F
        End If
        If Controls.Down(True, False, True, False, False, False) = True Or ControllerHandler.ButtonPressed(Buttons.LeftTrigger) = True Or KeyBoardHandler.KeyPressed(Keys.OemMinus) = True Then
            Me.scale -= 0.5F
        End If

        Me.scale = Me.scale.Clamp(0.5F, 4.0F)
    End Sub

End Class