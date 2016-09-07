Namespace GameJolt

    Public Class GTSTradeScreen

        Inherits Screen

        Dim D As GTSDataItem
        Dim E As Emblem = Nothing

        Dim PokeIndex As Integer = -1
        Dim TempPokemon As Pokemon = Nothing

        Public Sub New(ByVal currentScreen As Screen, ByVal D As GTSDataItem)
            Me.PreScreen = currentScreen
            Me.Identification = Identifications.GTSTradeScreen

            Me.CanBePaused = False
            Me.CanChat = False
            Me.CanDrawDebug = True
            Me.CanMuteMusic = True
            Me.CanTakeScreenshot = True
            Me.MouseVisible = True

            Me.D = D
            Me.E = New Emblem(D.FromUserID, 0)
        End Sub

        Public Overrides Sub Update()
            For i = 0 To GTSMainScreen.Furrs.Count - 1
                If i < GTSMainScreen.Furrs.Count Then
                    Dim f As Furr = GTSMainScreen.Furrs(i)
                    If f.IsOutOfBorder() = True Then
                        GTSMainScreen.Furrs.Remove(f)
                        i -= 1
                    Else
                        f.Update()
                    End If
                End If
            Next

            If Core.Random.Next(0, 100) = 0 Then
                GTSMainScreen.Furrs.Add(New Furr())
            End If

            If Controls.Accept(True, False) = True Then
                If New Rectangle(180, 560, 32 * 5 + 64, 32).Contains(MouseHandler.MousePosition) = True Then
                    Core.SetScreen(New ChoosePokemonScreen(Me, Item.GetItemByID(5), AddressOf ChosePokemon, "Chose Pokémon to trade!", True))
                End If
                If New Rectangle(180, 510, 32 * 5 + 64, 32).Contains(MouseHandler.MousePosition) = True Then
                    PokeIndex = -1
                End If
                If New Rectangle(780, 560, 32 * 5 + 64, 32).Contains(MouseHandler.MousePosition) = True Then
                    Core.SetScreen(New PokemonStatusScreen(Me, 0, {D.Pokemon}, D.Pokemon, False))
                End If
                If New Rectangle(520, 560, 32 * 3 + 64, 32).Contains(MouseHandler.MousePosition) = True Then
                    If MeetsCondition() = True Then
                        Core.SetScreen(New TradingScreen(Me, Me.D, Me.PokeIndex))
                    End If
                End If
            End If

            If Controls.Dismiss(True, True) = True Then
                Core.SetScreen(Me.PreScreen)
            End If
        End Sub

        Private Sub ChosePokemon(ByVal PokeIndex As Integer)
            Me.PokeIndex = PokeIndex
        End Sub

        Public Overrides Sub Draw()
            Core.SpriteBatch.Draw(TextureManager.GetTexture("GUI\Menus\GTS"), Core.windowSize, New Rectangle(320, 176, 192, 160), Color.White)

            For Each F As Furr In GTSMainScreen.Furrs
                F.Draw()
            Next

            Core.SpriteBatch.Draw(TextureManager.GetTexture("GUI\Menus\GTS"), New Rectangle(CInt(Core.windowSize.Width / 2 - 104), CInt(32), 208, 96), New Rectangle(304, 0, 208, 96), Color.White)

            Core.SpriteBatch.DrawString(FontManager.InGameFont, "Trade", New Vector2(560, 160), Color.White)

            'DRAW STUFF:

            Dim OwnX As Integer = 0
            Dim OppX As Integer = 600

            If New Rectangle(520, 560, 32 * 3 + 64, 32).Contains(MouseHandler.MousePosition) = True Then
                If MeetsCondition() = True Then
                    OwnX = 600
                    OppX = 0
                End If
            End If

            'Own:
            Canvas.DrawRectangle(New Rectangle(100 + OwnX, 160, 400, 450), New Color(255, 255, 255, 150))

            'User:
            If Core.Player.IsGamejoltSave = True Then
                Dim ownEmblem As Emblem = New Emblem(API.username, Core.GameJoltSave.GameJoltID, Core.GameJoltSave.Points, Core.GameJoltSave.Gender, Core.GameJoltSave.Emblem)

                Dim SpriteSize As New Size(CInt(ownEmblem.SpriteTexture.Width / 3), CInt(ownEmblem.SpriteTexture.Height / 4))
                Core.SpriteBatch.Draw(ownEmblem.SpriteTexture, New Rectangle(110, 90, 64, 64), New Rectangle(0, SpriteSize.Height * 2, SpriteSize.Width, SpriteSize.Height), Color.White)
                Core.SpriteBatch.DrawString(FontManager.InGameFont, ownEmblem.Username, New Vector2(183, 115), Color.Black)
                Core.SpriteBatch.DrawString(FontManager.InGameFont, ownEmblem.Username, New Vector2(180, 112), Color.White)
            End If

            Dim P As Pokemon = Nothing
            If Me.PokeIndex > -1 Then
                P = Core.Player.Pokemons(Me.PokeIndex)
            Else
                If TempPokemon Is Nothing Then
                    TempPokemon = Pokemon.GetPokemonByID(CInt(D.RequestID))
                End If

                P = TempPokemon
            End If

            If Not P Is Nothing Then
                'Pokemon image/data:
                Core.SpriteBatch.Draw(P.GetTexture(True), New Rectangle(100 + OwnX, 164, 128, 128), Color.White)

                If P.GetDisplayName() <> P.OriginalName Then
                    Core.SpriteBatch.DrawString(FontManager.MainFont, P.GetDisplayName(), New Vector2(230 + OwnX, 190), Color.Black)
                    Core.SpriteBatch.DrawString(FontManager.MainFont, "/" & P.OriginalName, New Vector2(240 + OwnX, 220), Color.Black)
                Else
                    Core.SpriteBatch.DrawString(FontManager.MainFont, P.GetDisplayName(), New Vector2(230 + OwnX, 205), Color.Black)
                End If

                If PokeIndex > -1 Then
                    Dim ItemString As String = "None"
                    If Not P.Item Is Nothing Then
                        ItemString = "   " & P.Item.Name
                        Core.SpriteBatch.Draw(P.Item.Texture, New Rectangle(202 + OwnX, 474, 24, 24), Color.White)
                    End If

                    Core.SpriteBatch.DrawString(FontManager.MiniFont, "Level: " & P.Level & vbNewLine & vbNewLine &
                                                 "Gender: " & P.Gender.ToString() & vbNewLine & vbNewLine &
                                                 "OT: " & P.CatchTrainerName & "/" & P.OT & vbNewLine & vbNewLine &
                                                 "Item: " & ItemString, New Vector2(150 + OwnX, 360), Color.Black)

                    DrawButton(New Vector2(180, 520), "Clear", 5)
                Else
                    Dim GenderString As String = "None"
                    If D.RequestGender <> "" Then
                        GenderString = D.RequestGender
                    End If

                    Core.SpriteBatch.DrawString(FontManager.MiniFont, "Request Level: " & D.RequestLevel & vbNewLine & vbNewLine &
                                                 "Request Gender: " & GenderString, New Vector2(150 + OwnX, 360), Color.Black)
                End If


                'Stars:
                GTSMainScreen.DrawStars(P.TradeValue, New Vector2(230 + OwnX, 256))
            End If

            DrawButton(New Vector2(180, 560), "Choose Pokémon", 5)

            'Other:
            Canvas.DrawRectangle(New Rectangle(100 + OppX, 160, 400, 450), New Color(255, 255, 255, 150))

            Core.SpriteBatch.DrawString(FontManager.MiniFont, "Transaction ID: " & D.TradeID, New Vector2(706, 160), Color.Black)

            'User:
            If E.DoneLoading = True Then
                Dim spriteSize As New Size(CInt(E.SpriteTexture.Width / 3), CInt(E.SpriteTexture.Height / 4))
                Core.SpriteBatch.Draw(E.SpriteTexture, New Rectangle(710, 90, 64, 64), New Rectangle(0, spriteSize.Height * 2, spriteSize.Width, spriteSize.Height), Color.White)
                Core.SpriteBatch.DrawString(FontManager.InGameFont, E.Username, New Vector2(783, 115), Color.Black)
                Core.SpriteBatch.DrawString(FontManager.InGameFont, E.Username, New Vector2(780, 112), Color.White)
            Else
                Core.SpriteBatch.DrawString(FontManager.InGameFont, "Loading" & LoadingDots.Dots, New Vector2(783, 115), Color.Black)
                Core.SpriteBatch.DrawString(FontManager.InGameFont, "Loading" & LoadingDots.Dots, New Vector2(780, 112), Color.White)
            End If

            If Not D.Pokemon Is Nothing Then
                'Pokemon image/data:
                Core.SpriteBatch.Draw(D.Pokemon.GetTexture(True), New Rectangle(100 + OppX, 164, 128, 128), Color.White)

                If D.Pokemon.GetDisplayName() <> D.Pokemon.OriginalName Then
                    Core.SpriteBatch.DrawString(FontManager.MainFont, D.Pokemon.GetDisplayName(), New Vector2(230 + OppX, 190), Color.Black)
                    Core.SpriteBatch.DrawString(FontManager.MainFont, "/" & D.Pokemon.OriginalName, New Vector2(240 + OppX, 220), Color.Black)
                Else
                    Core.SpriteBatch.DrawString(FontManager.MainFont, D.Pokemon.GetDisplayName(), New Vector2(230 + OppX, 205), Color.Black)
                End If

                Dim ItemString As String = "None"
                If Not D.Pokemon.Item Is Nothing Then
                    ItemString = "   " & D.Pokemon.Item.Name
                    Core.SpriteBatch.Draw(D.Pokemon.Item.Texture, New Rectangle(202 + OppX, 474, 24, 24), Color.White)
                End If

                Core.SpriteBatch.DrawString(FontManager.MiniFont, "Level: " & D.Pokemon.Level & vbNewLine & vbNewLine &
                                             "Gender: " & D.Pokemon.Gender.ToString() & vbNewLine & vbNewLine &
                                             "OT: " & D.Pokemon.CatchTrainerName & "/" & D.Pokemon.OT & vbNewLine & vbNewLine &
                                             "Item: " & ItemString, New Vector2(150 + OppX, 360), Color.Black)


                'Stars:
                GTSMainScreen.DrawStars(D.Pokemon.TradeValue, New Vector2(230 + OppX, 256))

                DrawButton(New Vector2(780, 560), "View summary", 5)
            End If

            If MeetsCondition() = True Then
                DrawButton(New Vector2(520, 560), "Start", 3)

                Dim v As Integer = P.TradeValue - D.Pokemon.TradeValue

                If v.ToString().EndsWith("5") = True Then
                    v += 5
                End If

                If v > 0 Then
                    Canvas.DrawRectangle(New Rectangle(540, 430, 120, 120), New Color(255, 255, 255, 150))

                    Core.SpriteBatch.DrawString(FontManager.MiniFont, "You get" & vbNewLine & "stars:", New Vector2(560, 435), Color.Black)

                    GTSMainScreen.DrawStars(v, New Vector2(560, 480))
                End If
            Else
                If PokeIndex > -1 Then
                    Dim t As String = "Your chosen Pokémon does not match the" & vbNewLine & "conditions given by your trade partner."
                    If Core.Player.Pokemons(PokeIndex).HasHMMove() = True Then
                        t = "This Pokémon knows an HM move." & vbNewLine & "You can't trade this Pokémon."
                    End If
                    If Core.Player.Pokemons(PokeIndex).IsEgg() = True Then
                        t = "You can't trade an egg."
                    End If
                    Core.SpriteBatch.DrawString(FontManager.MiniFont, t, New Vector2(120, 300), Color.Black)
                End If
            End If

            Core.SpriteBatch.DrawString(FontManager.MiniFont, "Version " & GTSMainScreen.GTSVersion, New Vector2(4, Core.windowSize.Height - 1 - FontManager.MiniFont.MeasureString("Version " & GTSMainScreen.GTSVersion).Y), Color.DarkGray)
        End Sub

        Private Sub DrawButton(ByVal Position As Vector2, ByVal Text As String, ByVal Size As Integer)
            Dim t As Texture2D = TextureManager.GetTexture("GUI\Menus\GTS")

            Dim touching As Boolean = New Rectangle(CInt(Position.X), CInt(Position.Y), 64 + Size * 32, 32).Contains(MouseHandler.MousePosition)

            Dim Y As Integer = 0
            If touching = True Then
                Y = 16
            End If

            Core.SpriteBatch.Draw(t, New Rectangle(CInt(Position.X), CInt(Position.Y), 32, 32), New Rectangle(0, Y, 16, 16), Color.White)

            For i = 1 To Size
                Core.SpriteBatch.Draw(t, New Rectangle(CInt(Position.X + i * 32), CInt(Position.Y), 32, 32), New Rectangle(16, Y, 16, 16), Color.White)
            Next

            Core.SpriteBatch.Draw(t, New Rectangle(CInt(Position.X + Size * 32 + 32), CInt(Position.Y), 32, 32), New Rectangle(32, Y, 16, 16), Color.White)

            Dim sizeX As Integer = Size * 32 + 64
            Dim TextSizeX As Integer = CInt(FontManager.MiniFont.MeasureString(Text).X)

            Core.SpriteBatch.DrawString(FontManager.MiniFont, Text, New Vector2(CSng(Position.X + sizeX / 2 - TextSizeX / 2 - 2), Position.Y + 4), Color.Black)
        End Sub

        Private Function MeetsCondition() As Boolean
            If PokeIndex > -1 Then
                Dim P As Pokemon = Core.Player.Pokemons(PokeIndex)

                If P.Number = CInt(D.RequestID) And P.IsEgg() = False And P.HasHMMove() = False Then
                    If D.RequestGender <> "" Then
                        Select Case D.RequestGender
                            Case "Male"
                                If P.Gender <> Pokemon.Genders.Male Then
                                    Return False
                                End If
                            Case "Female"
                                If P.Gender <> Pokemon.Genders.Female Then
                                    Return False
                                End If
                            Case "Genderless"
                                If P.Gender <> Pokemon.Genders.Genderless Then
                                    Return False
                                End If
                        End Select
                    End If
                    Dim levelMin As Integer = 0
                    Dim levelMax As Integer = 9

                    If Me.D.RequestLevel <> "9 and under" Then
                        levelMax = CInt(Me.D.RequestLevel.Remove(0, Me.D.RequestLevel.IndexOf(" - ") + 3))
                        levelMin = CInt(Me.D.RequestLevel.Remove(Me.D.RequestLevel.IndexOf(" ")))
                    End If

                    If P.Level <= levelMax And P.Level >= levelMin Then
                        Return True
                    End If
                End If
            End If
            Return False
        End Function

        Class TradingScreen

            Inherits Screen

            Dim PokeIndex As Integer = -1
            Dim D As GTSDataItem

            Dim P As Pokemon = Nothing

            Dim UploadDone As Boolean = False

            Public Sub New(ByVal currentScreen As Screen, ByVal D As GTSDataItem, ByVal PokeIndex As Integer)
                Me.PreScreen = currentScreen
                Me.Identification = Identifications.GTSTradingScreen

                Me.CanBePaused = False
                Me.CanChat = False
                Me.CanDrawDebug = True
                Me.CanMuteMusic = True
                Me.CanTakeScreenshot = True
                Me.MouseVisible = True

                Me.PokeIndex = PokeIndex
                Me.D = D

                Me.P = Core.Player.Pokemons(PokeIndex)

                Dim APICall As New APICall(AddressOf RemovedKey)
                APICall.RemoveKey(D.Key, False)
            End Sub

            Private Sub RemovedKey(ByVal result As String)
                Dim l As List(Of API.JoltValue) = API.HandleData(result)

                If CBool(l(0).Value) = True Then
                    Dim newD As New GTSDataItem(Core.GameJoltSave.GameJoltID, D.FromUserID, "", "", "", "", P.GetSaveData(), "Pokemon 3D", "Trade for " & D.Pokemon.GetDisplayName(), GTSDataItem.SecurityCode.Private, GTSDataItem.ActionSwitches.Got, D.TradeID)

                    Dim APICall As New APICall(AddressOf SentPokemon)
                    APICall.SetStorageData(newD.Key, newD.Data, False)
                End If
            End Sub

            Private Sub SentPokemon(ByVal result As String)
                Dim l As List(Of API.JoltValue) = API.HandleData(result)

                If CBool(l(0).Value) = True Then
                    Core.Player.Pokemons.RemoveAt(PokeIndex)
                    D.Pokemon.Friendship = D.Pokemon.BaseFriendship
                    Core.Player.Pokemons.Add(D.Pokemon)

                    If D.Pokemon.IsShiny = True Then
                        Core.Player.PokedexData = Pokedex.ChangeEntry(Core.Player.PokedexData, D.Pokemon.Number, 3)
                    Else
                        Core.Player.PokedexData = Pokedex.ChangeEntry(Core.Player.PokedexData, D.Pokemon.Number, 2)
                    End If

                    Dim v As Integer = P.TradeValue - D.Pokemon.TradeValue

                    If v.ToString().EndsWith("5") = True Then
                        v += 5
                    End If

                    If v > 0 Then
                        Core.Player.GTSStars += CInt(v / 10)
                    End If

                    PlayerStatistics.Track("GTS trades", 1)
                    Core.Player.SaveGame(False)

                    UploadDone = True
                End If
            End Sub

            Dim P1Pos As New Vector2(600, -128)
            Dim P2Pos As New Vector2(472, Core.windowSize.Height)

            Dim Fur As New Furr(128, New Vector2(600, -80), 0.0F)
            Dim state As Integer = 0

            Public Overrides Sub Draw()
                Core.SpriteBatch.Draw(TextureManager.GetTexture("GUI\Menus\GTS"), Core.windowSize, New Rectangle(320, 176, 192, 160), Color.White)

                For Each F As Furr In GTSMainScreen.Furrs
                    F.Draw()
                Next

                If UploadDone = False Then
                    Canvas.DrawRectangle(New Rectangle(CInt(Core.windowSize.Width / 2 - 200), 250, 400, 200), New Color(255, 255, 255, 150))

                    Core.SpriteBatch.DrawString(FontManager.MainFont, "Trading" & LoadingDots.Dots, New Vector2(CSng(Core.windowSize.Width / 2 - FontManager.MainFont.MeasureString("Uploading").X / 2), 300), Color.Black)
                    Core.SpriteBatch.DrawString(FontManager.MainFont, "Transaction ID: " & D.TradeID, New Vector2(CSng(Core.windowSize.Width / 2 - FontManager.MainFont.MeasureString("Transaction ID: " & D.TradeID).X / 2), 370), Color.Black)

                    Core.SpriteBatch.DrawString(FontManager.MiniFont, "Version " & GTSMainScreen.GTSVersion, New Vector2(4, Core.windowSize.Height - 1 - FontManager.MiniFont.MeasureString("Version " & GTSMainScreen.GTSVersion).Y), Color.DarkGray)
                Else
                    Fur.Draw()
                    Core.SpriteBatch.Draw(D.Pokemon.GetTexture(True), New Rectangle(CInt(P1Pos.X), CInt(P1Pos.Y), 128, 128), Color.White)
                    Core.SpriteBatch.Draw(P.GetTexture(True), New Rectangle(CInt(P2Pos.X), CInt(P2Pos.Y), 128, 128), Color.White)

                End If
            End Sub

            Public Overrides Sub Update()
                For i = 0 To GTSMainScreen.Furrs.Count - 1
                    If i < GTSMainScreen.Furrs.Count Then
                        Dim f As Furr = GTSMainScreen.Furrs(i)
                        If f.IsOutOfBorder() = True Then
                            GTSMainScreen.Furrs.Remove(f)
                            i -= 1
                        Else
                            f.Update()
                        End If
                    End If
                Next

                If Core.Random.Next(0, 100) = 0 Then
                    GTSMainScreen.Furrs.Add(New Furr())
                End If

                If UploadDone = True Then
                    Select Case state
                        Case 0
                            If P1Pos.Y < P2Pos.Y Then
                                P1Pos.Y += 2
                                Fur.Position.Y += 2
                                P2Pos.Y -= 2
                                If P1Pos.Y >= P2Pos.Y Then
                                    P2Pos.Y = P1Pos.Y
                                    state = 1
                                End If
                            End If
                        Case 1
                            If Fur.Position.X > P2Pos.X Then
                                Fur.Position.X -= 1
                                If Fur.Position.X <= P2Pos.X Then
                                    Fur.Position.X = P2Pos.X
                                    state = 2
                                End If
                            End If
                        Case 2
                            P1Pos.Y += 2
                            Fur.Position.Y -= 2
                            P2Pos.Y -= 2

                            If P1Pos.Y >= Core.windowSize.Height And P2Pos.Y <= -160 Then
                                If D.Pokemon.CanEvolve(EvolutionCondition.EvolutionTrigger.Trading, "") = True Then
                                    Core.SetScreen(New EvolutionScreen(Me, {Core.Player.Pokemons.Count - 1}.ToList(), P.Number.ToString(), EvolutionCondition.EvolutionTrigger.Trading))
                                End If
                                state = 3
                            End If
                        Case 3
                            Core.SetScreen(Me.PreScreen.PreScreen.PreScreen)
                    End Select
                End If
            End Sub

            Public Overrides Sub ChangeTo()
                If Me.D.SecurityArea = GTSDataItem.SecurityCode.Global Then
                    Core.Player.AddPoints(3, "Traded a Pokémon on the GTS.")
                End If
                GameJolt.Emblem.AchieveEmblem("cyber")
            End Sub

        End Class

    End Class

End Namespace