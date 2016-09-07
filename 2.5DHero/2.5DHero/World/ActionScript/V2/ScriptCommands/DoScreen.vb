Namespace ScriptVersion2

    Partial Class ScriptCommander

        '--------------------------------------------------------------------------------------------------------------------------
        'Contains the @screen commands.
        '--------------------------------------------------------------------------------------------------------------------------

        Private Shared Sub DoScreen(ByVal subClass As String)
            Dim command As String = ScriptComparer.GetSubClassArgumentPair(subClass).Command
            Dim argument As String = ScriptComparer.GetSubClassArgumentPair(subClass).Argument

            Select Case command.ToLower()
                Case "storagesystem"
                    Core.SetScreen(New TransitionScreen(Core.CurrentScreen, New StorageSystemScreen(Core.CurrentScreen), Color.Black, False))

                    IsReady = True

                    CanContinue = False
                Case "apricornkurt"
                    Core.SetScreen(New ApricornScreen(Core.CurrentScreen, "Kurt"))

                    IsReady = True

                    CanContinue = False
                Case "trade"
                    Dim storeData As String = CStr(argument.GetSplit(0))
                    Dim canBuy As Boolean = CBool(argument.GetSplit(1))
                    Dim canSell As Boolean = CBool(argument.GetSplit(2))

                    Dim currencyIndicator As String = "P"

                    If argument.CountSplits() > 3 Then
                        currencyIndicator = argument.GetSplit(3)
                    End If

                    Core.SetScreen(New TransitionScreen(Core.CurrentScreen, New TradeScreen(Core.CurrentScreen, storeData, canBuy, canSell, currencyIndicator), Color.Black, False))

                    IsReady = True

                    CanContinue = False
                Case "townmap"
                    If argument.Contains(",") = True Then
                        Dim regions As List(Of String) = argument.Split(CChar(",")).ToList()
                        Core.SetScreen(New TransitionScreen(Core.CurrentScreen, New MapScreen(Core.CurrentScreen, regions, 0, {"view"}), Color.Black, False))
                    Else
                        Dim startRegion As String = argument
                        Core.SetScreen(New TransitionScreen(Core.CurrentScreen, New MapScreen(Core.CurrentScreen, startRegion, {"view"}), Color.Black, False))
                    End If

                    IsReady = True

                    CanContinue = False
                Case "donation"
                    Core.SetScreen(New DonationScreen(Core.CurrentScreen))

                    IsReady = True

                    CanContinue = False
                Case "blackout"
                    Core.SetScreen(New TransitionScreen(Core.CurrentScreen, New BlackOutScreen(Core.CurrentScreen), Color.Black, False))

                    IsReady = True

                    CanContinue = False
                Case "fadein"
                    Dim fadeSpeed As Integer = 5
                    If argument <> "" Then
                        fadeSpeed = int(argument)
                    End If
                    If OverworldScreen.FadeValue > 0 Then
                        OverworldScreen.FadeValue -= fadeSpeed
                        If OverworldScreen.FadeValue <= 0 Then
                            OverworldScreen.FadeValue = 0
                            IsReady = True
                        End If
                    Else
                        IsReady = True
                    End If
                Case "fadeout"
                    Dim fadeSpeed As Integer = 5
                    If argument <> "" Then
                        fadeSpeed = int(argument)
                    End If
                    If OverworldScreen.FadeValue < 255 Then
                        OverworldScreen.FadeValue += fadeSpeed
                        If OverworldScreen.FadeValue >= 255 Then
                            OverworldScreen.FadeValue = 255
                            IsReady = True
                        End If
                    Else
                        IsReady = True
                    End If
                Case "setfade"
                    OverworldScreen.FadeValue = int(argument).Clamp(0, 255)
                    IsReady = True
                Case "showpokemon"
                    Dim PokemonID As Integer = int(argument.GetSplit(0))
                    Dim Shiny As Boolean = CBool(argument.GetSplit(1))
                    Dim Front As Boolean = CBool(argument.GetSplit(2))

                    Screen.PokemonImageView.Show(PokemonID, Shiny, Front)
                    IsReady = True

                    CanContinue = False
                Case "credits"
                    Dim ending As String = "Johto"
                    If argument <> "" Then
                        ending = argument
                    End If

                    Core.SetScreen(New CreditsScreen(Core.CurrentScreen))
                    CType(Core.CurrentScreen, CreditsScreen).InitializeScreen(ending)
                    IsReady = True

                    CanContinue = False
                Case "halloffame"
                    If argument <> "" Then
                        Core.SetScreen(New TransitionScreen(Core.CurrentScreen, New HallOfFameScreen(Core.CurrentScreen, int(argument)), Color.Black, False))
                    Else
                        Core.SetScreen(New TransitionScreen(Core.CurrentScreen, New HallOfFameScreen(Core.CurrentScreen), Color.Black, False))
                    End If
                    IsReady = True

                    CanContinue = False
                Case "teachmoves"
                    Dim args() As String = argument.Split(CChar(","))

                    If args.Length = 1 Then
                        'Show screen with all tutor moves

                        Core.SetScreen(New TeachMovesScreen(Core.CurrentScreen, int(argument)))
                    ElseIf args.Length > 1 Then
                        'Show screen with all moves listed
                        Dim pokeIndex As Integer = int(args(0))

                        Dim moves As New List(Of BattleSystem.Attack)

                        For i = 1 To args.Length - 1
                            If IsNumeric(args(i)) = True Then
                                moves.Add(BattleSystem.Attack.GetAttackByID(int(args(i))))
                            End If
                        Next

                        Core.SetScreen(New TeachMovesScreen(Core.CurrentScreen, pokeIndex, moves.ToArray()))
                    End If

                    IsReady = True

                    CanContinue = False
                Case "mailsystem"
                    Core.SetScreen(New TransitionScreen(Core.CurrentScreen, New MailSystemScreen(Core.CurrentScreen), Color.White, False))

                    IsReady = True

                    CanContinue = False
                Case "pvp"
                    If Core.ServersManager.ID = 0 Then
                        Core.SetScreen(New TransitionScreen(Core.CurrentScreen, New PVPLobbyScreen(Core.CurrentScreen, 1, True), Color.Black, False))
                    Else
                        Core.SetScreen(New TransitionScreen(Core.CurrentScreen, New PVPLobbyScreen(Core.CurrentScreen, 0, False), Color.Black, False))
                    End If

                    IsReady = True

                    CanContinue = False
                Case "input"
                    Dim data() As String = argument.Split(CChar(","))

                    Dim defaultName As String = ""
                    Dim inputMode As InputScreen.InputModes = InputScreen.InputModes.Text
                    Dim currentText As String = ""
                    Dim MaxChars As Integer = 14
                    Dim textureList As New List(Of Texture2D)

                    If data.Length > 0 Then
                        defaultName = data(0)
                    End If
                    If data.Length > 1 Then
                        If ScriptConversion.IsArithmeticExpression(data(1)) = True Then
                            inputMode = CType(int(data(1)), InputScreen.InputModes)
                        Else
                            Select Case data(1).ToLower()
                                Case "text"
                                    inputMode = InputScreen.InputModes.Text
                                Case "name"
                                    inputMode = InputScreen.InputModes.Name
                                Case "numbers"
                                    inputMode = InputScreen.InputModes.Numbers
                            End Select
                        End If
                    End If
                    If data.Length > 2 Then
                        currentText = data(2)
                    End If
                    If data.Length > 3 Then
                        MaxChars = int(data(3))
                    End If
                    If data.Length > 4 Then
                        For i = 4 To data.Length - 1
                            Dim tData() As String = data(i).Split(CChar("|"))
                            If tData.Length = 1 Then
                                textureList.Add(TextureManager.GetTexture(tData(0)))
                            Else
                                textureList.Add(TextureManager.GetTexture(tData(0), New Rectangle(CInt(tData(1)), CInt(tData(2)), CInt(tData(3)), CInt(tData(4))), ""))
                            End If
                        Next
                    End If

                    Core.SetScreen(New InputScreen(Core.CurrentScreen, defaultName, inputMode, currentText, MaxChars, textureList))

                    IsReady = True

                    CanContinue = False
                Case "mysteryevent"
                    Core.SetScreen(New TransitionScreen(Core.CurrentScreen, New MysteryEventScreen(Core.CurrentScreen), Color.White, False))
                    IsReady = True

                    CanContinue = False
                Case "secretbase"
                    Core.SetScreen(New SecretBaseScreen())
                    IsReady = True

                    CanContinue = False
                Case Else
                    IsReady = True
            End Select
        End Sub

    End Class

End Namespace