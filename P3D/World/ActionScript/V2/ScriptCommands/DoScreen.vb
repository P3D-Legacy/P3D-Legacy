Namespace ScriptVersion2

    Partial Class ScriptCommander

        ' --------------------------------------------------------------------------------------------------------------------------
        ' Contains the @screen commands.
        ' --------------------------------------------------------------------------------------------------------------------------

        Private Shared Sub DoScreen(ByVal subClass As String)
            Dim command As String = ScriptComparer.GetSubClassArgumentPair(subClass).Command
            Dim argument As String = ScriptComparer.GetSubClassArgumentPair(subClass).Argument

            Select Case command.ToLower()
                Case "showmessagebox"
                    '@screen.showmessagebox(str_message|[intArr_RGB_background]|[intArr_RGB_font]|[intArr_RGB_border])
                    Dim messageBox As New UI.MessageBox(CurrentScreen)
                    Dim colorsplit() As String = argument.Split("|")
                    Select Case argument.Split("|").Count
                        Case 1
                            messageBox.Show(argument.Replace("~", Environment.NewLine).Replace("*", Environment.NewLine & Environment.NewLine))
                        Case 2
                            messageBox.Show(colorsplit(0).Replace("~", Environment.NewLine).Replace("*", Environment.NewLine & Environment.NewLine), New Color(CInt(colorsplit(1).GetSplit(0)), CInt(colorsplit(1).GetSplit(1)), CInt(colorsplit(1).GetSplit(2))))
                        Case 3
                            messageBox.Show(colorsplit(0).Replace("~", Environment.NewLine).Replace("*", Environment.NewLine & Environment.NewLine), New Color(CInt(colorsplit(1).GetSplit(0)), CInt(colorsplit(1).GetSplit(1)), CInt(colorsplit(1).GetSplit(2))), New Color(CInt(colorsplit(2).GetSplit(0)), CInt(colorsplit(2).GetSplit(1)), CInt(colorsplit(2).GetSplit(2))))
                        Case 4
                            messageBox.Show(colorsplit(0).Replace("~", Environment.NewLine).Replace("*", Environment.NewLine & Environment.NewLine), New Color(CInt(colorsplit(1).GetSplit(0)), CInt(colorsplit(1).GetSplit(1)), CInt(colorsplit(1).GetSplit(2))), New Color(CInt(colorsplit(2).GetSplit(0)), CInt(colorsplit(2).GetSplit(1)), CInt(colorsplit(2).GetSplit(2))), New Color(CInt(colorsplit(3).GetSplit(0)), CInt(colorsplit(3).GetSplit(1)), CInt(colorsplit(3).GetSplit(2))))
                    End Select
                    IsReady = True
                    CanContinue = False
                Case "storagesystem"
                    Core.SetScreen(New TransitionScreen(Core.CurrentScreen, New StorageSystemScreen(Core.CurrentScreen), Color.Black, False))

                    IsReady = True

                    CanContinue = False
                Case "apricornkurt"
                    Core.SetScreen(New ApricornScreen(Core.CurrentScreen, "Kurt"))

                    IsReady = True

                    CanContinue = False
                Case "trade"
                    Dim storeData As String = CStr(argument.GetSplit(0))    ' e.g. Item ID
                    Dim canBuy As Boolean = CBool(argument.GetSplit(1))     ' 
                    Dim canSell As Boolean = CBool(argument.GetSplit(2))    ' 

                    Dim currencyIndicator As String = "P"

                    If argument.CountSplits() > 3 Then
                        currencyIndicator = argument.GetSplit(3)            ' p for PokéDollars, bp for Battle Points.
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
                    Dim fadeLimit As Integer = 0
                    If argument <> "" Then
                        If argument.Contains(",") Then
                            fadeSpeed = int(argument.GetSplit(0, ","))
                            fadeLimit = int(argument.GetSplit(1, ","))
                        Else
                            fadeSpeed = int(argument)
                        End If
                    End If
                    If OverworldScreen.FadeValue > fadeLimit Then
                        OverworldScreen.FadeValue -= fadeSpeed
                        If OverworldScreen.FadeValue <= fadeLimit Then
                            OverworldScreen.FadeValue = fadeLimit
                            IsReady = True
                        End If
                    Else
                        IsReady = True
                    End If
                Case "fadeout"
                    Dim fadeSpeed As Integer = 5
                    Dim fadeLimit As Integer = 255
                    If argument <> "" Then
                        If argument.Contains(",") Then
                            fadeSpeed = int(argument.GetSplit(0, ","))
                            fadeLimit = int(argument.GetSplit(1, ","))
                        Else
                            fadeSpeed = int(argument)
                        End If
                    End If
                    If OverworldScreen.FadeValue < fadeLimit Then
                        OverworldScreen.FadeValue += fadeSpeed
                        If OverworldScreen.FadeValue >= fadeLimit Then
                            OverworldScreen.FadeValue = fadeLimit
                            IsReady = True
                        End If
                    Else
                        IsReady = True
                    End If
                Case "fadeoutcolor"
                    If Not String.IsNullOrEmpty(argument) Then
                        Dim colorR As Integer = int(argument.GetSplit(0))
                        Dim colorG As Integer = int(argument.GetSplit(1))
                        Dim colorB As Integer = int(argument.GetSplit(2))
                        OverworldScreen.FadeColor = New Color(colorR, colorG, colorB)
                    Else
                        OverworldScreen.FadeColor = Color.Black
                    End If
                    IsReady = True
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
                Case "showimage"
                    '@screen.showimage(str_texture,[str_sfxname],[int_x],[int_y],[int_w],[int_h])
                    Dim Texture As Texture2D = TextureManager.GetTexture(argument.GetSplit(0))
                    Dim Sound As String = ""
                    If argument.Split(",").Count > 1 Then
                        Sound = argument.GetSplit(1)
                        If argument.Split(",").Count > 2 Then
                            Texture = TextureManager.GetTexture(argument.GetSplit(0), New Rectangle(int(argument.GetSplit(2)), int(argument.GetSplit(3)), int(argument.GetSplit(4)), int(argument.GetSplit(5))), "")
                        End If
                    End If
                    Screen.ImageView.Show(Texture, Sound)
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
                        ' Show screen with all tutor moves.

                        Core.SetScreen(New TeachMovesScreen(Core.CurrentScreen, int(argument)))
                    ElseIf args.Length > 1 Then
                        ' Show screen with all moves listed.
                        Dim pokeIndex As Integer = int(args(0))

                        Dim moves As New List(Of BattleSystem.Attack)

                        For i = 1 To args.Length - 1
                            If StringHelper.IsNumeric(args(i)) Then
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
                                Case "pokemon"
                                    inputMode = InputScreen.InputModes.Pokemon
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
                Case "voltorbflip"
                    If Core.Player.Inventory.GetItemAmount(54) > 0 Then
                        If Core.Player.Coins < 50000 Then
                            Core.SetScreen(New VoltorbFlip.VoltorbFlipScreen(CurrentScreen))
                            IsReady = True
                            CanContinue = False

                            If CurrentScreen.Identification = Screen.Identifications.OverworldScreen Then
                                If VoltorbFlip.VoltorbFlipScreen.TotalCoins > 0 Then
                                    Screen.TextBox.Show("You've won" & " " & VoltorbFlip.VoltorbFlipScreen.TotalCoins & " " & "Coins!")
                                    Core.Player.Coins += VoltorbFlip.VoltorbFlipScreen.TotalCoins
                                    VoltorbFlip.VoltorbFlipScreen.TotalCoins = 0
                                Else
                                    Screen.TextBox.Show("Too bad, you didn't win~any Coins!*Better luck next time!")
                                End If
                            End If
                        Else
                            Screen.TextBox.Show("Your Coin Case is already full!")
                            IsReady = True
                        End If
                    Else
                        Screen.TextBox.Show("You don't have a Coin Case!~Come back when you have one!")
                        IsReady = True
                    End If
                Case "skinselection"
                    If Screens.MainMenu.NewNewGameScreen.CharacterSelectionScreen.SelectedSkin <> "" Then
                        IsReady = True
                    Else
                        SetScreen(New Screens.MainMenu.NewNewGameScreen.CharacterSelectionScreen(CurrentScreen))
                    End If
                Case Else
                    IsReady = True
            End Select
        End Sub

    End Class

End Namespace