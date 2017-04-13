Namespace GameJolt

    Public Class RegisterBattleScreen

        Inherits Screen

        Public Const REGISTERBATTLEVERSION As String = "1"

        Enum ScreenStates
            MainMenu
            TeamRegister
            UploadingTeam
            PreparingBattle
            ChooseTeam
        End Enum

        Dim ScreenState As ScreenStates = ScreenStates.MainMenu
        Dim MainMenuItems() As String = {"Team Management", "Start Battle", "Quit"}
        Dim TeamRegisterMenuItems() As String = {"Register Team", "Back"}
        Dim texture As Texture2D = Nothing
        Dim MainCursor As Integer = 0
        Dim TeamRegisterCursor As Integer = 0

        Dim OwnTeam As New List(Of Pokemon)
        Dim TeamDownloaded As Boolean = False
        Dim HasTeamUploaded As Boolean = False
        Dim OwnBattleStrength As Integer = 0
        Dim BattleBoxPokemon As New List(Of Pokemon)
        Dim UseBattleBox As Boolean = False

        Dim ChooseTeamCursor As Integer = 0

        Public Sub New(ByVal currentScreen As Screen)
            Me.PreScreen = currentScreen
            Me.Identification = Identifications.RegisterBattleScreen
            Me.texture = TextureManager.GetTexture("GUI\Menus\General")
            Me.MouseVisible = True
            Me.CanBePaused = False

            MusicPlayer.GetInstance().Play("system\lobby", True)

            Dim APICall As New APICall(AddressOf GotKeys)
            APICall.GetKeys(False, "RegisterBattleV" & REGISTERBATTLEVERSION & "|" & Core.GameJoltSave.GameJoltID & "|*")
        End Sub

        Private Sub GotKeys(ByVal result As String)
            Me.BattleBoxPokemon = StorageSystemScreen.GetBattleBoxPokemon()

            Dim list As List(Of API.JoltValue) = API.HandleData(result)

            If list(0).Value = "true" Then
                If list(1).Value <> "" Then
                    Me.OwnBattleStrength = CInt(list(1).Value.Split(CChar("|"))(2))

                    Dim APICall As New APICall(AddressOf GotOwnTeamData)
                    APICall.GetStorageData(list(1).Value, False)
                Else
                    TeamDownloaded = True
                End If
            End If
        End Sub

        Private Sub GotOwnTeamData(ByVal result As String)
            Me.OwnTeam.Clear()
            Dim list As List(Of API.JoltValue) = API.HandleData(result)

            If list(0).Value = "true" Then
                HasTeamUploaded = True

                Dim data As String = result.Remove(0, 22)
                data = data.Remove(data.LastIndexOf(""""))

                Dim allData As String = data
                Dim pokemonData As New List(Of String)

                For Each line As String In allData.SplitAtNewline()
                    If line.StartsWith("{") = True And line.EndsWith("}") = True Then
                        pokemonData.Add(line.Replace("\""", """"))
                    End If
                Next

                For Each dataEntry As String In pokemonData
                    OwnTeam.Add(Pokemon.GetPokemonByData(dataEntry))
                Next
            End If
            TeamDownloaded = True
        End Sub

        Public Overrides Sub Draw()
            Canvas.DrawGradient(Core.windowSize, New Color(10, 145, 227), New Color(6, 77, 139), False, -1)
            Canvas.DrawGradient(New Rectangle(0, 0, CInt(Core.windowSize.Width), 65), New Color(0, 24, 114), New Color(13, 138, 228), False, -1)
            Core.SpriteBatch.DrawString(FontManager.MainFont, "Battle Spot", New Vector2(CSng(Core.windowSize.Width / 2 - FontManager.MainFont.MeasureString("Battle Spot").X / 2), 20), New Color(196, 231, 255))
            Canvas.DrawRectangle(New Rectangle(0, 65, Core.windowSize.Width, 1), New Color(0, 24, 114))

            If TeamDownloaded = True Then
                Select Case Me.ScreenState
                    Case ScreenStates.MainMenu
                        DrawMainMenu()
                    Case ScreenStates.TeamRegister
                        DrawTeamRegistration()
                    Case ScreenStates.UploadingTeam
                        DrawUploadTeam()
                    Case ScreenStates.PreparingBattle
                        DrawPreparingBattle()
                    Case ScreenStates.ChooseTeam
                        DrawChooseTeam()
                End Select
            Else
                Dim t As String = "Downloading data, please wait" & LoadingDots.Dots
                Core.SpriteBatch.DrawString(FontManager.MainFont, t, New Vector2(CSng(Core.windowSize.Width / 2 - FontManager.MainFont.MeasureString(t).X / 2), CSng(Core.windowSize.Height / 2 - 10)), Color.White)
            End If
        End Sub

        Public Overrides Sub Update()
            If TeamDownloaded = True Then
                Select Case Me.ScreenState
                    Case ScreenStates.MainMenu
                        UpdateMainMenu()
                    Case ScreenStates.TeamRegister
                        UpdateTeamRegisterMenu()
                    Case ScreenStates.PreparingBattle
                        UpdatePreparingBattle()
                    Case ScreenStates.ChooseTeam
                        UpdateChooseTeam()
                End Select
            End If
        End Sub

#Region "MainMenu"

        Private Sub DrawMainMenu()
            Canvas.DrawRectangle(New Rectangle(CInt(Core.windowSize.Width / 2 - 400), 100, 800, 240), New Color(177, 228, 247, 200))

            Dim t As String = "Battle Spot allows you to battle against the registered teams of" & vbNewLine & "other players. These battles will be held against the computer."
            Select Case Me.MainCursor
                Case 0
                    t &= vbNewLine & vbNewLine & "The Team Management gives you the option to set up your own" & vbNewLine & "team others can battle against."
                Case 1
                    t &= vbNewLine & vbNewLine & "Start a battle against a random team registered by another player." & vbNewLine & "You have to register your own team first."
                Case 2
                    t &= vbNewLine & vbNewLine & "Quit the Battle Spot."
            End Select

            Core.SpriteBatch.DrawString(FontManager.MainFont, t, New Vector2(CSng(Core.windowSize.Width / 2 - FontManager.MainFont.MeasureString(t).X / 2), 220 - FontManager.MainFont.MeasureString(t).Y / 2), Color.Black)

            For i = 0 To Me.MainMenuItems.Count - 1
                Core.SpriteBatch.Draw(Me.texture, New Rectangle(CInt(Core.windowSize.Width / 2 - (64 * 5) / 2), 400 + i * 96, 64, 64), New Rectangle(16, 16, 16, 16), Color.White)
                Core.SpriteBatch.Draw(Me.texture, New Rectangle(CInt(Core.windowSize.Width / 2 - (64 * 5) / 2) + 64, 400 + i * 96, 64 * 3, 64), New Rectangle(32, 16, 16, 16), Color.White)
                Core.SpriteBatch.Draw(Me.texture, New Rectangle(CInt(Core.windowSize.Width / 2 - (64 * 5) / 2) + 64 * 4, 400 + i * 96, 64, 64), New Rectangle(16, 16, 16, 16), Color.White, 0.0F, Vector2.Zero, SpriteEffects.FlipHorizontally, 0.0F)

                Core.SpriteBatch.DrawString(FontManager.MainFont, Me.MainMenuItems(i), New Vector2(CInt(Core.windowSize.Width / 2 - (64 * 5) / 2) + 20, 416 + i * 96), Color.Black, 0.0F, Vector2.Zero, 1.25F, SpriteEffects.None, 0.0F)
            Next

            DrawMainCursor()
        End Sub

        Private Sub DrawMainCursor()
            Dim cPosition As Vector2 = New Vector2(CInt(Core.windowSize.Width / 2 - (64 * 4) / 2) + 160, 400 + Me.MainCursor * 96 - 42)

            Dim t As Texture2D = TextureManager.GetTexture("GUI\Menus\General", New Rectangle(0, 0, 16, 16), "")
            Core.SpriteBatch.Draw(t, New Rectangle(CInt(cPosition.X), CInt(cPosition.Y), 64, 64), Color.White)
        End Sub

        Private Sub UpdateMainMenu()
            If Controls.Up(True, True, True, True, True, True) = True Then
                Me.MainCursor -= 1
                If Controls.ShiftDown() = True Then
                    Me.MainCursor -= 4
                End If
            End If
            If Controls.Down(True, True, True, True, True, True) = True Then
                Me.MainCursor += 1
                If Controls.ShiftDown() = True Then
                    Me.MainCursor += 4
                End If
            End If

            Me.MainCursor = Me.MainCursor.Clamp(0, Me.MainMenuItems.Count - 1)

            If Controls.Accept(True, False, False) = True Then
                For i = 0 To Me.MainMenuItems.Count - 1
                    If New Rectangle(CInt(Core.windowSize.Width / 2 - (64 * 4) / 2), 400 + i * 96, 64 * 4, 64).Contains(MouseHandler.MousePosition) = True Then
                        If i = MainCursor Then
                            Me.SelectMainMenuEntry()
                        Else
                            MainCursor = i
                        End If
                    End If
                Next
            End If

            If Controls.Accept(False, True, True) = True Then
                Me.SelectMainMenuEntry()
            End If
            If Controls.Dismiss(True, True, True) = True Then
                Core.SetScreen(New TransitionScreen(Me, Me.PreScreen, Color.White, False))
            End If
        End Sub

        Private Sub SelectMainMenuEntry()
            Select Case MainCursor
                Case 0
                    Me.TeamRegisterCursor = 0
                    Me.ScreenState = ScreenStates.TeamRegister
                Case 1
                    If Me.HasTeamUploaded = True Then
                        Me.ScreenState = ScreenStates.ChooseTeam
                    End If
                Case 2
                    Core.SetScreen(New TransitionScreen(Me, Me.PreScreen, Color.White, False))
            End Select
        End Sub

#End Region

#Region "TeamRegistration"

        Private Sub DrawTeamRegistration()
            Canvas.DrawRectangle(New Rectangle(CInt(Core.windowSize.Width / 2 - 400), 100, 800, 240), New Color(177, 228, 247, 200))

            Dim t As String = "When you register your team, the game will store a copy of your" & vbNewLine & "Pokémon online so that other players can download that team and" & vbNewLine & "battle against it." & vbNewLine & vbNewLine & "If you register a new team, the old one will be overwritten with" & vbNewLine & "the new one."

            Core.SpriteBatch.DrawString(FontManager.MainFont, t, New Vector2(CSng(Core.windowSize.Width / 2 - FontManager.MainFont.MeasureString(t).X / 2), 180 - FontManager.MainFont.MeasureString(t).Y / 2), Color.Black)

            If HasTeamUploaded = True Then
                Core.SpriteBatch.DrawString(FontManager.MainFont, "Registered Team:", New Vector2(CSng(Core.windowSize.Width / 2 - FontManager.MainFont.MeasureString(t).X / 2), 335 - FontManager.MainFont.MeasureString(t).Y / 2), Color.Black)

                Dim startPos As New Vector2(CSng(Core.windowSize.Width / 2 - FontManager.MainFont.MeasureString(t).X / 2), 335 - FontManager.MainFont.MeasureString(t).Y / 2)

                For i = 0 To Me.OwnTeam.Count - 1
                    Dim p As Pokemon = Me.OwnTeam(i)
                    Core.SpriteBatch.Draw(p.GetMenuTexture(), New Rectangle(CInt(startPos.X) + 200 + i * 68, CInt(startPos.Y) - 26, 64, 64), Color.White)
                Next
            Else
                Core.SpriteBatch.DrawString(FontManager.MainFont, "No Team registered.", New Vector2(CSng(Core.windowSize.Width / 2 - FontManager.MainFont.MeasureString(t).X / 2), 335 - FontManager.MainFont.MeasureString(t).Y / 2), Color.Black)
            End If

            For i = 0 To Me.TeamRegisterMenuItems.Count - 1
                Core.SpriteBatch.Draw(Me.texture, New Rectangle(CInt(Core.windowSize.Width / 2 - (64 * 5) / 2), 400 + i * 96, 64, 64), New Rectangle(16, 16, 16, 16), Color.White)
                Core.SpriteBatch.Draw(Me.texture, New Rectangle(CInt(Core.windowSize.Width / 2 - (64 * 5) / 2) + 64, 400 + i * 96, 64 * 3, 64), New Rectangle(32, 16, 16, 16), Color.White)
                Core.SpriteBatch.Draw(Me.texture, New Rectangle(CInt(Core.windowSize.Width / 2 - (64 * 5) / 2) + 64 * 4, 400 + i * 96, 64, 64), New Rectangle(16, 16, 16, 16), Color.White, 0.0F, Vector2.Zero, SpriteEffects.FlipHorizontally, 0.0F)

                Core.SpriteBatch.DrawString(FontManager.MainFont, Me.TeamRegisterMenuItems(i), New Vector2(CInt(Core.windowSize.Width / 2 - (64 * 5) / 2) + 20, 416 + i * 96), Color.Black, 0.0F, Vector2.Zero, 1.25F, SpriteEffects.None, 0.0F)
            Next

            DrawTeamRegisterCursor()
        End Sub

        Private Sub DrawTeamRegisterCursor()
            Dim cPosition As Vector2 = New Vector2(CInt(Core.windowSize.Width / 2 - (64 * 4) / 2) + 160, 400 + Me.TeamRegisterCursor * 96 - 42)

            Dim t As Texture2D = TextureManager.GetTexture("GUI\Menus\General", New Rectangle(0, 0, 16, 16), "")
            Core.SpriteBatch.Draw(t, New Rectangle(CInt(cPosition.X), CInt(cPosition.Y), 64, 64), Color.White)
        End Sub

        Private Sub UpdateTeamRegisterMenu()
            If Controls.Up(True, True, True, True, True, True) = True Then
                Me.TeamRegisterCursor -= 1
                If Controls.ShiftDown() = True Then
                    Me.TeamRegisterCursor -= 4
                End If
            End If
            If Controls.Down(True, True, True, True, True, True) = True Then
                Me.TeamRegisterCursor += 1
                If Controls.ShiftDown() = True Then
                    Me.TeamRegisterCursor += 4
                End If
            End If

            Me.TeamRegisterCursor = Me.TeamRegisterCursor.Clamp(0, Me.TeamRegisterMenuItems.Count - 1)

            If Controls.Accept(True, False, False) = True Then
                For i = 0 To Me.TeamRegisterMenuItems.Count - 1
                    If New Rectangle(CInt(Core.windowSize.Width / 2 - (64 * 4) / 2), 400 + i * 96, 64 * 4, 64).Contains(MouseHandler.MousePosition) = True Then
                        If i = TeamRegisterCursor Then
                            Me.SelectTeamRegisterMenuEntry()
                        Else
                            TeamRegisterCursor = i
                        End If
                    End If
                Next
            End If

            If Controls.Accept(False, True, True) = True Then
                Me.SelectTeamRegisterMenuEntry()
            End If
            If Controls.Dismiss(True, True, True) = True Then
                Me.ScreenState = ScreenStates.MainMenu
            End If
        End Sub

        Private Sub SelectTeamRegisterMenuEntry()
            Select Case Me.TeamRegisterCursor
                Case 0
                    Me.RegisterTeam()
                Case 1
                    Me.ScreenState = ScreenStates.MainMenu
            End Select
        End Sub

        Private Sub RegisterTeam()
            Me.ScreenState = ScreenStates.UploadingTeam

            If Me.HasTeamUploaded = True Then
                Dim APICall As New APICall(AddressOf DeletedOldTeam)
                APICall.RemoveKey("RegisterBattleV" & REGISTERBATTLEVERSION & "|" & Core.GameJoltSave.GameJoltID & "|" & Me.OwnBattleStrength, False)
            Else
                DeletedOldTeam("success:""true""")
            End If
        End Sub

        Private Sub DeletedOldTeam(ByVal result As String)
            Dim list As List(Of API.JoltValue) = API.HandleData(result)
            If list(0).Value = "true" Then
                Dim storageData As String = ""
                For Each p As Pokemon In Core.Player.Pokemons
                    If p.IsEgg() = False Then
                        If storageData <> "" Then
                            storageData &= vbNewLine
                        End If
                        storageData &= p.GetSaveData()
                    End If
                Next
                Dim APICall As New APICall(AddressOf FinishedUploadingTeam)
                APICall.SetStorageData("RegisterBattleV" & REGISTERBATTLEVERSION & "|" & Core.GameJoltSave.GameJoltID & "|" & GetBattleStrength(Core.Player.Pokemons), storageData, False)
            End If
        End Sub

        Private Sub FinishedUploadingTeam(ByVal result As String)
            Dim list As List(Of API.JoltValue) = API.HandleData(result)
            If list(0).Value = "true" Then
                If Me.HasTeamUploaded = False Then
                    Dim APICall1 As New APICall(AddressOf GotRegisteredCount)
                    APICall1.GetStorageData("0RegisterBattleV" & REGISTERBATTLEVERSION & "_Counter", False)
                End If

                Me.ScreenState = ScreenStates.MainMenu
                Me.TeamDownloaded = False
                Me.HasTeamUploaded = False

                Dim APICall As New APICall(AddressOf GotKeys)
                APICall.GetKeys(False, "RegisterBattleV" & REGISTERBATTLEVERSION & "|" & Core.GameJoltSave.GameJoltID & "|*")
            End If
        End Sub

        Private Sub GotRegisteredCount(ByVal result As String)
            Dim list As List(Of API.JoltValue) = API.HandleData(result)
            If list(0).Value = "true" Then
                Dim APICall As New APICall()
                APICall.SetStorageData("0RegisterBattleV" & REGISTERBATTLEVERSION & "_Counter", CStr(CInt(list(1).Value) + 1), False)
            End If
        End Sub

#End Region

#Region "TeamUpload"

        Private Sub DrawUploadTeam()
            Dim t As String = "Registering, please wait" & LoadingDots.Dots
            Core.SpriteBatch.DrawString(FontManager.MainFont, t, New Vector2(CSng(Core.windowSize.Width / 2 - FontManager.MainFont.MeasureString(t).X / 2), CSng(Core.windowSize.Height / 2 - 10)), Color.White)
        End Sub

#End Region

#Region "ChooseTeam"

        Private Sub DrawChooseTeam()
            Dim t As String = "Choose your team:"
            Core.SpriteBatch.DrawString(FontManager.MainFont, t, New Vector2(CSng(Core.windowSize.Width / 2 - FontManager.MainFont.MeasureString(t).X / 2), 100), Color.White)

            Dim startPos As New Vector2(CSng(Core.windowSize.Width / 2) - 400, 300)
            Canvas.DrawGradient(New Rectangle(CInt(Core.windowSize.Width / 2) - 410, 230, 290, 360), New Color(203, 40, 41), New Color(238, 128, 128), False, -1)
            For i = 0 To 5
                Dim x As Integer = i
                Dim y As Integer = 0

                While x > 1
                    x -= 2
                    y += 1
                End While

                Canvas.DrawBorder(2, New Rectangle(CInt(startPos.X) + x * 140, y * 100 + CInt(startPos.Y), 128, 80), New Color(230, 230, 230))

                If BattleBoxPokemon.Count - 1 >= i Then
                    Core.SpriteBatch.Draw(BattleBoxPokemon(i).GetMenuTexture(), New Rectangle(CInt(startPos.X) + x * 140 + 32, y * 100 + CInt(startPos.Y) + 10, 64, 64), Color.White)
                End If

                Core.SpriteBatch.DrawString(FontManager.MainFont, "Battle Box", New Vector2(CInt(startPos.X) + 80, CInt(startPos.Y) - 45), Color.White)
            Next

            startPos = New Vector2(CSng(Core.windowSize.Width / 2) + 130, 300)
            Canvas.DrawGradient(New Rectangle(CInt(Core.windowSize.Width / 2) + 120, 230, 290, 360), New Color(84, 198, 216), New Color(42, 167, 198), False, -1)
            For i = 0 To 5
                Dim x As Integer = i
                Dim y As Integer = 0

                While x > 1
                    x -= 2
                    y += 1
                End While

                Canvas.DrawBorder(2, New Rectangle(CInt(startPos.X) + x * 140, y * 100 + CInt(startPos.Y), 128, 80), New Color(230, 230, 230))

                If Core.Player.Pokemons.Count - 1 >= i Then
                    Core.SpriteBatch.Draw(Core.Player.Pokemons(i).GetMenuTexture(), New Rectangle(CInt(startPos.X) + x * 140 + 32, y * 100 + CInt(startPos.Y) + 10, 64, 64), Color.White)
                End If

                Core.SpriteBatch.DrawString(FontManager.MainFont, "Team", New Vector2(CInt(startPos.X) + 106, CInt(startPos.Y) - 45), Color.White)
            Next

            DrawChooseTeamCursor()
        End Sub

        Private Sub DrawChooseTeamCursor()
            Dim cPosition As Vector2 = New Vector2(CSng(Core.windowSize.Width / 2) - 280, 190)
            If ChooseTeamCursor = 1 Then
                cPosition = New Vector2(CSng(Core.windowSize.Width / 2) + 250, 190)
            End If

            Dim t As Texture2D = TextureManager.GetTexture("GUI\Menus\General", New Rectangle(0, 0, 16, 16), "")
            Core.SpriteBatch.Draw(t, New Rectangle(CInt(cPosition.X), CInt(cPosition.Y), 64, 64), Color.White)
        End Sub

        Private Sub UpdateChooseTeam()
            If Controls.Left(True, True) = True Then
                Me.ChooseTeamCursor = 0
            End If
            If Controls.Right(True, True) = True Then
                Me.ChooseTeamCursor = 1
            End If

            Dim hasBattleBoxPokemon As Boolean = False
            For Each p As Pokemon In Me.BattleBoxPokemon
                If p.IsEgg() = False Then
                    hasBattleBoxPokemon = True
                    Exit For
                End If
            Next

            If Controls.Accept(True, False, False) = True Then
                If New Rectangle(CInt(Core.windowSize.Width / 2) + 120, 230, 290, 360).Contains(MouseHandler.MousePosition) = True Then
                    If ChooseTeamCursor = 0 Then
                        ChooseTeamCursor = 1
                    Else
                        UseBattleBox = False
                        Me.PrepareBattle()
                    End If
                End If
                If New Rectangle(CInt(Core.windowSize.Width / 2) - 410, 230, 290, 360).Contains(MouseHandler.MousePosition) = True Then
                    If ChooseTeamCursor = 1 Then
                        ChooseTeamCursor = 0
                    Else
                        If hasBattleBoxPokemon = True Then
                            UseBattleBox = True
                            Me.PrepareBattle()
                        End If
                    End If
                End If
            End If

            If hasBattleBoxPokemon = True Or ChooseTeamCursor = 1 Then
                If Controls.Accept(False, True, True) = True Then
                    If ChooseTeamCursor = 0 Then
                        UseBattleBox = True
                    Else
                        UseBattleBox = False
                    End If
                    Me.PrepareBattle()
                End If
            End If

            If Controls.Dismiss(True, True, True) = True Then
                Me.ScreenState = ScreenStates.MainMenu
            End If
        End Sub

#End Region

#Region "PreparingBattle"

        Private Class TeamKey

            Public OriginalKey As String = ""
            Public GameJoltID As String = ""
            Public BattleStrength As Integer = 0

            Public DifferenceValue As Integer = 0

            Public Sub New(ByVal Key As String)
                Me.OriginalKey = Key

                Dim data() As String = Key.Split(CChar("|"))
                Me.GameJoltID = data(1)
                Me.BattleStrength = CInt(data(2))
            End Sub

            Public Sub SetDifferenceValue(ByVal ownBattleStrength As Integer)
                Me.DifferenceValue = 100 - Math.Abs(ownBattleStrength - BattleStrength)
            End Sub

        End Class

        Private Sub PrepareBattle()
            Me.ScreenState = ScreenStates.PreparingBattle
            Me.LoadedOppTeam = False
            Me.OppTeam.Clear()
            Me.OppEmblem = Nothing

            'Get the list of uploaded teams.

            Dim APICall As New APICall(AddressOf GotTeamList)
            APICall.GetKeys(False, "RegisterBattleV" & REGISTERBATTLEVERSION & "|*|*")
        End Sub

        Private Sub GotTeamList(ByVal result As String)
            Dim list As List(Of API.JoltValue) = API.HandleData(result)

            If list(0).Value = "true" Then
                list.RemoveAt(0)

                Dim TeamList As New List(Of TeamKey)
                Dim OwnbattleStrength As Integer = GetBattleStrength(Core.Player.Pokemons)

                For Each v As API.JoltValue In list
                    If v.Value.Contains("|" & Core.GameJoltSave.GameJoltID & "|") = False Then
                        TeamList.Add(New TeamKey(v.Value))
                    End If
                Next

                For Each TeamKey As TeamKey In TeamList
                    TeamKey.SetDifferenceValue(OwnbattleStrength)
                Next

                Dim totalNumber As Integer = 0
                For Each c In TeamList
                    totalNumber += c.DifferenceValue
                Next

                Dim r As Integer = Core.Random.Next(0, totalNumber + 1)

                Dim x As Integer = 0
                For i = 0 To TeamList.Count - 1
                    x += TeamList(i).DifferenceValue
                    If r < x Then
                        LoadOpponent(TeamList(i))
                        Exit For
                    End If
                Next
            End If
        End Sub

        Private Sub LoadOpponent(ByVal TeamKey As TeamKey)
            OppEmblem = New Emblem(TeamKey.GameJoltID, 0)

            Dim APICall As New APICall(AddressOf ReceivedOppPokemonData)
            APICall.GetStorageData(TeamKey.OriginalKey, False)
        End Sub

        Private Sub ReceivedOppPokemonData(ByVal result As String)
            While OppEmblem.DoneLoading = False : End While
            Me.OppTeam.Clear()
            Dim list As List(Of API.JoltValue) = API.HandleData(result)

            If list(0).Value = "true" Then
                Dim data As String = result.Remove(0, 22)
                data = data.Remove(data.LastIndexOf(""""))

                Dim allData As String = data
                Dim pokemonData As New List(Of String)

                For Each line As String In allData.SplitAtNewline()
                    If line.StartsWith("{") = True And line.EndsWith("}") = True Then
                        pokemonData.Add(line.Replace("\""", """"))
                    End If
                Next

                For Each dataEntry As String In pokemonData
                    Me.OppTeam.Add(Pokemon.GetPokemonByData(dataEntry))
                Next
            End If
            LoadedOppTeam = True
        End Sub

        Dim OppTeam As New List(Of Pokemon)
        Dim OppEmblem As Emblem
        Dim LoadedOppTeam As Boolean = False
        Dim TempOriginalTeam As New List(Of Pokemon)

        Private Sub DrawPreparingBattle()
            Dim t As String = "Preparing the battle, please wait" & LoadingDots.Dots
            Core.SpriteBatch.DrawString(FontManager.MainFont, t, New Vector2(CSng(Core.windowSize.Width / 2 - FontManager.MainFont.MeasureString(t).X / 2), CSng(Core.windowSize.Height / 2 - 10)), Color.White)
        End Sub

        Private Sub UpdatePreparingBattle()
            If OppTeam.Count > 0 And Not OppEmblem Is Nothing And LoadedOppTeam = True Then
                Dim t As New Trainer()
                t.Pokemons = OppTeam
                t.TrainerType = Emblem.GetPlayerTitle(Emblem.GetPlayerLevel(OppEmblem.Points), OppEmblem.GameJoltID, OppEmblem.Gender)
                t.DoubleTrainer = False
                t.Name = OppEmblem.Username
                t.Money = 0
                t.SpriteName = Emblem.GetPlayerSpriteFile(Emblem.GetPlayerLevel(OppEmblem.Points), OppEmblem.GameJoltID, OppEmblem.Gender)
                t.Region = "Johto"
                t.TrainerFile = ""
                t.Items = New List(Of Item)
                t.Gender = CInt(OppEmblem.Gender)
                t.IntroType = 11
                t.OutroMessage = ". . ."
                t.GameJoltID = OppEmblem.GameJoltID

                For Each p As Pokemon In t.Pokemons
                    p.Level = 50
                    p.CalculateStats()
                    p.FullRestore()
                Next

                TempOriginalTeam.Clear()
                For Each p As Pokemon In Core.Player.Pokemons
                    TempOriginalTeam.Add(Pokemon.GetPokemonByData(p.GetSaveData()))
                Next

                If UseBattleBox = True Then
                    Core.Player.Pokemons.Clear()
                    Core.Player.Pokemons.AddRange(Me.BattleBoxPokemon.ToArray())
                End If

                For Each p As Pokemon In Core.Player.Pokemons
                    p.Level = 50
                    p.CalculateStats()
                    p.FullRestore()
                Next

                Dim b As New BattleSystem.BattleScreen(t, Core.CurrentScreen, 0)
                b.IsPVPBattle = True
                b.PVPGameJoltID = OppEmblem.GameJoltID
                Core.SetScreen(New BattleIntroScreen(Core.CurrentScreen, b, t, t.GetIniMusicName(), t.IntroType))
                PlayerStatistics.Track("Battle Spot battles", 1)
            End If
        End Sub

#End Region

        Public Overrides Sub ChangeTo()
            If Me.ScreenState = ScreenStates.PreparingBattle Then
                Me.ScreenState = ScreenStates.MainMenu
                Me.OwnTeam.Clear()
                Me.TeamDownloaded = False
                Me.HasTeamUploaded = False
                MusicPlayer.GetInstance().Play("system\lobby", False)
                Core.Player.Pokemons.Clear()
                Core.Player.Pokemons.AddRange(TempOriginalTeam.ToArray())
                Dim APICall As New APICall(AddressOf GotKeys)
                APICall.GetKeys(False, "RegisterBattleV" & REGISTERBATTLEVERSION & "|" & Core.GameJoltSave.GameJoltID & "|*")
            End If
        End Sub

        Public Shared Function GetBattleStrength(ByVal team As List(Of Pokemon)) As Integer
            Dim pCount As Integer = team.Count

            '(BaseStats + EVs * (780 / 186) + IVs * (780 / 186)) / (3 * 780) * 100 = x

            Dim x As Integer = 0
            Dim c As Double = 0
            For Each p As Pokemon In team
                c = 0
                c += p.BaseHP + p.BaseAttack + p.BaseDefense + p.BaseSpAttack + p.BaseSpDefense + p.BaseSpeed
                c += (p.IVHP + p.IVAttack + p.IVDefense + p.IVSpAttack + p.IVSpDefense + p.IVSpeed) * (780 / 186)
                c += (p.EVHP + p.EVAttack + p.EVDefense + p.EVSpAttack + p.EVSpDefense + p.EVSpeed) * (780 / 186)
                c = c / (3 * 780) * 100
                x += CInt(c)
            Next

            Return CInt(x / pCount)
        End Function

    End Class

End Namespace