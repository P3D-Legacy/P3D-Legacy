Public Class NPCTradeScreen

    Inherits Screen

    Dim PlayerPokemon As Pokemon = Nothing 'The Pokémon you offer.

    Dim NPCPokemon As Pokemon = Nothing 'The Pokémon your trade partner wants to trade.
    Dim NPCTrainerName As String = "Trainer" 'The name of your trade partner.

    Dim TextMessage As String = ""
    Dim ownPokemonPosition As Integer = 0
    Dim oppPokemonPosition As Integer = 0
    Dim tState As Integer = 0
    Dim messageDelay As Integer = 220

    Public Sub New(ByVal currentScreen As Screen, ByVal PlayerPokemon As Pokemon, ByVal NPCPokemon As Pokemon, ByVal NPCTrainerName As String, Optional ByVal TextMessage As String = "")
        Me.Identification = Identifications.DirectTradeScreen
        Me.PreScreen = currentScreen

        ownPokemonPosition = Core.windowSize.Height
        tState = 0
        messageDelay = 220
        Me.CanChat = False

        Me.MouseVisible = False
        Me.CanBePaused = False

        Me.PlayerPokemon = PlayerPokemon
        Me.NPCPokemon = NPCPokemon
        Me.NPCTrainerName = NPCTrainerName
        Me.TextMessage = TextMessage

        MusicManager.Play("evolution", True)
    End Sub

    Public Overrides Sub Draw()
        Core.SpriteBatch.Draw(TextureManager.GetTexture("GUI\Menus\GTS"), Core.windowSize, New Rectangle(320, 176, 192, 160), Color.White)
        Select Case tState
            Case 0
                Core.SpriteBatch.Draw(Me.PlayerPokemon.GetTexture(True), New Rectangle(CInt(Core.windowSize.Width / 2 - MathHelper.Min(CInt(PlayerPokemon.GetTexture(True).Width * 3 / 2), 144)), ownPokemonPosition, MathHelper.Min(PlayerPokemon.GetTexture(True).Width * 3, 288), MathHelper.Min(PlayerPokemon.GetTexture(True).Height * 3, 288)), Color.White)
            Case 1
                Core.SpriteBatch.Draw(Me.PlayerPokemon.GetTexture(False), New Rectangle(CInt(Core.windowSize.Width / 2 - MathHelper.Min(CInt(PlayerPokemon.GetTexture(False).Width * 3 / 2), 144)), CInt(Core.windowSize.Height / 2 - 128), MathHelper.Min(PlayerPokemon.GetTexture(False).Width * 3, 288), MathHelper.Min(PlayerPokemon.GetTexture(False).Height * 3, 288)), Color.White)

                Dim t As String = Localization.GetString("trade_screen_trade_SendingPokemon", "Sending [YOURPOKEMON] to [OTHERPLAYER].~Good-bye, [YOURPOKEMON]!").Replace("[YOURPOKEMON]", PlayerPokemon.GetDisplayName()).Replace("[OTHERPLAYER]", NPCTrainerName).Replace("~", Environment.NewLine).Replace("*", Environment.NewLine)

                Core.SpriteBatch.DrawString(FontManager.MainFont, t, New Vector2(CInt(Core.windowSize.Width / 2 - FontManager.MainFont.MeasureString(t).X / 2) + 2, CInt(Core.windowSize.Height / 2 + 192) + 2), Color.Black)
                Core.SpriteBatch.DrawString(FontManager.MainFont, t, New Vector2(CInt(Core.windowSize.Width / 2 - FontManager.MainFont.MeasureString(t).X / 2), CInt(Core.windowSize.Height / 2 + 192)), Color.White)
            Case 2
                Core.SpriteBatch.Draw(Me.PlayerPokemon.GetTexture(False), New Rectangle(CInt(Core.windowSize.Width / 2 - MathHelper.Min(CInt(PlayerPokemon.GetTexture(False).Width * 3 / 2), 144)), ownPokemonPosition, MathHelper.Min(PlayerPokemon.GetTexture(False).Width * 3, 288), MathHelper.Min(PlayerPokemon.GetTexture(False).Height * 3, 288)), Color.White)
            Case 3
                Core.SpriteBatch.Draw(NPCPokemon.GetTexture(True), New Rectangle(CInt(Core.windowSize.Width / 2 - MathHelper.Min(CInt(NPCPokemon.GetTexture(True).Width * 3 / 2), 144)), oppPokemonPosition, MathHelper.Min(NPCPokemon.GetTexture(True).Width * 3, 288), MathHelper.Min(NPCPokemon.GetTexture(True).Height * 3, 288)), Color.White)
            Case 4
                Core.SpriteBatch.Draw(NPCPokemon.GetTexture(True), New Rectangle(CInt(Core.windowSize.Width / 2 - MathHelper.Min(CInt(NPCPokemon.GetTexture(True).Width * 3 / 2), 144)), CInt(Core.windowSize.Height / 2 - 128), MathHelper.Min(NPCPokemon.GetTexture(True).Width * 3, 288), MathHelper.Min(NPCPokemon.GetTexture(True).Height * 3, 288)), Color.White)

                Dim t As String = Localization.GetString("trade_screen_trade_ReceivedPokemon", "[OTHERPLAYER] sent over [THEIRPOKEMON].").Replace("[OTHERPLAYER]", NPCTrainerName).Replace("[THEIRPOKEMON]", NPCPokemon.GetDisplayName())

                Core.SpriteBatch.DrawString(FontManager.MainFont, t, New Vector2(CInt(Core.windowSize.Width / 2 - FontManager.MainFont.MeasureString(t).X / 2) + 2, CInt(Core.windowSize.Height / 2 + 192) + 2), Color.Black)
                Core.SpriteBatch.DrawString(FontManager.MainFont, t, New Vector2(CInt(Core.windowSize.Width / 2 - FontManager.MainFont.MeasureString(t).X / 2), CInt(Core.windowSize.Height / 2 + 192)), Color.White)
        End Select
    End Sub

    Public Overrides Sub Update()
        Select Case tState
            Case 0
                If ownPokemonPosition > CInt(Core.windowSize.Height / 2 - 128) Then
                    ownPokemonPosition -= 4
                    If ownPokemonPosition <= CInt(Core.windowSize.Height / 2 - 128) Then
                        ownPokemonPosition = CInt(Core.windowSize.Height / 2 - 128)
                        tState = 1
                        SoundManager.PlayPokemonCry(PlayerPokemon.Number, PokemonForms.GetCrySuffix(PlayerPokemon))
                    End If
                End If
            Case 1
                If messageDelay > 0 Then
                    messageDelay -= 1
                    If messageDelay <= 0 Then
                        messageDelay = 220
                        tState = 2
                    End If
                End If
            Case 2
                If ownPokemonPosition > -288 Then
                    ownPokemonPosition -= 4
                    If ownPokemonPosition <= -288 Then
                        ownPokemonPosition = -288
                        tState = 3
                        oppPokemonPosition = -288
                    End If
                End If
            Case 3
                If oppPokemonPosition < CInt(Core.windowSize.Height / 2 - 128) Then
                    oppPokemonPosition += 4
                    If oppPokemonPosition >= CInt(Core.windowSize.Height / 2 - 128) Then
                        oppPokemonPosition = CInt(Core.windowSize.Height / 2 - 128)
                        tState = 4
                        SoundManager.PlayPokemonCry(NPCPokemon.Number, PokemonForms.GetCrySuffix(NPCPokemon))
                    End If
                End If
            Case 4
                If messageDelay > 0 Then
                    messageDelay -= 1
                    If messageDelay = 180 Then
                        SoundManager.PlaySound("success", True)
                    End If
                    If messageDelay <= 0 Then
                        messageDelay = 220
                        EndTrade()
                    End If
                End If
        End Select
    End Sub

    Private Sub EndTrade()
        Core.SetScreen(New TransitionScreen(Core.CurrentScreen, CurrentScreen.PreScreen, Color.Black, False, 15))
        SoundManager.PlaySound("success_small")
        Dim text As String = Me.TextMessage
        If text = "" Then
            text = Localization.GetString("trade_screen_trade_TradedWithNPC", "<Player.Name> traded~[THEIRPOKEMON] for~[YOURPOKEMON]!").Replace("[THEIRPOKEMON]", NPCPokemon.GetName).Replace("[YOURPOKEMON]", PlayerPokemon.GetName)
        End If
        Screen.TextBox.Show(text, {}, False, False)

        Dim p As Pokemon = Core.Player.Pokemons(Core.Player.Pokemons.Count - 1)
        If p.CanEvolve(EvolutionCondition.EvolutionTrigger.Trading, PokemonForms.GetPokemonDataFileName(NPCPokemon.Number, NPCPokemon.AdditionalData)) Then
            Core.SetScreen(New EvolutionScreen(Me, {Core.Player.Pokemons.Count - 1}.ToList(), PokemonForms.GetPokemonDataFileName(NPCPokemon.Number, NPCPokemon.AdditionalData), EvolutionCondition.EvolutionTrigger.Trading))
        End If
    End Sub


End Class