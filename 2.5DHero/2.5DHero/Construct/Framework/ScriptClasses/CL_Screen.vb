Namespace Construct.Framework.Classes

    <ScriptClass("Screen")>
    <ScriptDescription("A class to change the screen.")>
    Public Class CL_Screen

        Inherits ScriptClass

#Region "Commands"

#Region "New screens"

        <ScriptCommand("SkinSelection")>
        <ScriptDescription("Displays the skin selection screen.")>
        Private Function M_SkinSelection(ByVal argument As String) As String
            Dim skins = argument.Split(","c)
            SetScreen(New Screens.MainMenu.NewNewGameScreen.CharacterSelectionScreen(CurrentScreen, skins))

            ActiveLine.EndExecutionFrame = True

            Return Core.Null
        End Function

        <ScriptCommand("StorageSystem")>
        <ScriptDescription("Opens the storage system screen.")>
        Private Function M_StorageSystem(ByVal argument As String) As String
            Game.Core.SetScreen(New TransitionScreen(Game.Core.CurrentScreen, New StorageSystemScreen(Game.Core.CurrentScreen), Color.Black, False))

            ActiveLine.EndExecutionFrame = True

            Return Core.Null
        End Function

        <ScriptCommand("Apricorns")>
        <ScriptDescription("Opens the apricorn screen.")>
        Private Function M_Apricorns(ByVal argument As String) As String
            Game.Core.SetScreen(New ApricornScreen(Game.Core.CurrentScreen, argument))

            ActiveLine.EndExecutionFrame = True

            Return Core.Null
        End Function

        <ScriptCommand("Trade")>
        <ScriptDescription("Opens the trading screen (shop interface).")>
        Private Function M_Trade(ByVal argument As String) As String
            Dim storeData As String = CStr(argument.GetSplit(0))
            Dim canBuy As Boolean = Bool(argument.GetSplit(1))
            Dim canSell As Boolean = Bool(argument.GetSplit(2))

            Dim currencyIndicator As String = "P"

            If argument.CountSplits() > 3 Then
                currencyIndicator = argument.GetSplit(3)
            End If

            Game.Core.SetScreen(New TransitionScreen(Game.Core.CurrentScreen, New TradeScreen(Game.Core.CurrentScreen, storeData, canBuy, canSell, currencyIndicator), Color.Black, False))

            ActiveLine.EndExecutionFrame = True

            Return Core.Null
        End Function

        <ScriptCommand("TownMap")>
        <ScriptDescription("Opens the town map of a region.")>
        Private Function M_TownMap(ByVal argument As String) As String
            If argument.Contains(",") = True Then
                Dim regions As List(Of String) = argument.Split(CChar(",")).ToList()
                Game.Core.SetScreen(New TransitionScreen(Game.Core.CurrentScreen, New MapScreen(Game.Core.CurrentScreen, regions, 0, {"view"}), Color.Black, False))
            Else
                Dim startRegion As String = argument
                Game.Core.SetScreen(New TransitionScreen(Game.Core.CurrentScreen, New MapScreen(Game.Core.CurrentScreen, startRegion, {"view"}), Color.Black, False))
            End If

            ActiveLine.EndExecutionFrame = True

            Return Core.Null
        End Function

        <ScriptCommand("Donation")>
        <ScriptDescription("Opens the donation screen.")>
        Private Function M_Donation(ByVal argument As String) As String
            Game.Core.SetScreen(New DonationScreen(Game.Core.CurrentScreen))

            ActiveLine.EndExecutionFrame = True

            Return Core.Null
        End Function

        <ScriptCommand("Blackout")>
        <ScriptDescription("Opens the blackout screen.")>
        Private Function M_Blackout(ByVal argument As String) As String
            Game.Core.SetScreen(New TransitionScreen(Game.Core.CurrentScreen, New BlackOutScreen(Game.Core.CurrentScreen), Color.Black, False))

            ActiveLine.EndExecutionFrame = True

            Return Core.Null
        End Function

        <ScriptCommand("Credits")>
        <ScriptDescription("Opens the credits screen.")>
        Private Function M_Credits(ByVal argument As String) As String
            Dim ending As String = "Johto"
            If argument <> "" Then
                ending = argument
            End If

            Game.Core.SetScreen(New CreditsScreen(Game.Core.CurrentScreen))
            CType(Game.Core.CurrentScreen, CreditsScreen).InitializeScreen(ending)

            ActiveLine.EndExecutionFrame = True

            Return Core.Null
        End Function

        <ScriptCommand("HallOfFame")>
        <ScriptDescription("Opens the hall of fame screen.")>
        Private Function M_HallOfFame(ByVal argument As String) As String
            If argument <> "" Then
                Game.Core.SetScreen(New TransitionScreen(Game.Core.CurrentScreen, New HallOfFameScreen(Game.Core.CurrentScreen, Int(argument)), Color.Black, False))
            Else
                Game.Core.SetScreen(New TransitionScreen(Game.Core.CurrentScreen, New HallOfFameScreen(Game.Core.CurrentScreen), Color.Black, False))
            End If

            ActiveLine.EndExecutionFrame = True

            Return Core.Null
        End Function

        <ScriptCommand("TeachMoves")>
        <ScriptDescription("Opens the teach moves screen.")>
        Private Function M_TeachMoves(ByVal argument As String) As String
            Dim args() As String = argument.Split(CChar(","))

            If args.Length = 1 Then
                'Show screen with all tutor moves

                Game.Core.SetScreen(New TeachMovesScreen(Game.Core.CurrentScreen, Int(argument)))
            ElseIf args.Length > 1 Then
                'Show screen with all moves listed
                Dim pokeIndex As Integer = Int(args(0))

                Dim moves As New List(Of BattleSystem.Attack)

                For i = 1 To args.Length - 1
                    If IsNumeric(args(i)) = True Then
                        moves.Add(BattleSystem.Attack.GetAttackByID(Int(args(i))))
                    End If
                Next

                Game.Core.SetScreen(New TeachMovesScreen(Game.Core.CurrentScreen, pokeIndex, moves.ToArray()))
            End If

            ActiveLine.EndExecutionFrame = True

            Return Core.Null
        End Function

        <ScriptCommand("MailSystem")>
        <ScriptDescription("Opens the main system screen.")>
        Private Function M_MailSystem(ByVal argument As String) As String
            Game.Core.SetScreen(New TransitionScreen(Game.Core.CurrentScreen, New MailSystemScreen(Game.Core.CurrentScreen), Color.White, False))

            ActiveLine.EndExecutionFrame = True

            Return Core.Null
        End Function

        <ScriptCommand("Input")>
        <ScriptDescription("Opens the input overlay screen.")>
        Private Function M_Input(ByVal argument As String) As String
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
                If Converter.IsNumeric(data(1)) = True Then
                    inputMode = CType(Int(data(1)), InputScreen.InputModes)
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
                MaxChars = Int(data(3))
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

            Game.Core.SetScreen(New InputScreen(Game.Core.CurrentScreen, defaultName, inputMode, currentText, MaxChars, textureList))

            ActiveLine.EndExecutionFrame = True

            Return Core.Null
        End Function

        <ScriptCommand("MysteryEvent")>
        <ScriptDescription("Opens the mystery events screen.")>
        Private Function M_MysteryEvent(ByVal argument As String) As String
            Game.Core.SetScreen(New TransitionScreen(Game.Core.CurrentScreen, New MysteryEventScreen(Game.Core.CurrentScreen), Color.White, False))

            ActiveLine.EndExecutionFrame = True

            Return Core.Null
        End Function

#End Region

        <ScriptCommand("FadeIn")>
        <ScriptDescription("Fades in the screen.")>
        Private Function M_FadeIn(ByVal argument As String) As String
            ActiveLine.Preserve = True

            Dim fadeSpeed As Integer = 5
            If argument <> "" Then
                fadeSpeed = Int(argument)
            End If

            If Controller.GetInstance().Context = ScriptContext.Overworld Then
                If OverworldScreen.FadeValue > 0 Then
                    OverworldScreen.FadeValue -= fadeSpeed
                    If OverworldScreen.FadeValue <= 0 Then
                        OverworldScreen.FadeValue = 0
                        ActiveLine.Preserve = False
                    End If
                Else
                    ActiveLine.Preserve = False
                End If
            ElseIf Controller.GetInstance().Context = ScriptContext.NewGame Then
                If Screens.MainMenu.NewNewGameScreen.FadeValue > 0 Then
                    Screens.MainMenu.NewNewGameScreen.FadeValue -= fadeSpeed
                    If Screens.MainMenu.NewNewGameScreen.FadeValue <= 0 Then
                        Screens.MainMenu.NewNewGameScreen.FadeValue = 0
                        ActiveLine.Preserve = False
                    End If
                Else
                    ActiveLine.Preserve = False
                End If
            End If

            Return Core.Null
        End Function

        <ScriptCommand("FadeOut")>
        <ScriptDescription("Fades out the screen.")>
        Private Function M_FadeOut(ByVal argument As String) As String
            ActiveLine.Preserve = True

            Dim fadeSpeed As Integer = 5
            If argument <> "" Then
                fadeSpeed = Int(argument)
            End If

            If Controller.GetInstance().Context = ScriptContext.Overworld Then
                If OverworldScreen.FadeValue < 255 Then
                    OverworldScreen.FadeValue += fadeSpeed
                    If OverworldScreen.FadeValue >= 255 Then
                        OverworldScreen.FadeValue = 255
                        ActiveLine.Preserve = False
                    End If
                Else
                    ActiveLine.Preserve = False
                End If
            ElseIf Controller.GetInstance().Context = ScriptContext.NewGame Then
                If Screens.MainMenu.NewNewGameScreen.FadeValue < 255 Then
                    Screens.MainMenu.NewNewGameScreen.FadeValue += fadeSpeed
                    If Screens.MainMenu.NewNewGameScreen.FadeValue >= 255 Then
                        Screens.MainMenu.NewNewGameScreen.FadeValue = 255
                        ActiveLine.Preserve = False
                    End If
                Else
                    ActiveLine.Preserve = False
                End If
            End If

            Return Core.Null
        End Function

        <ScriptCommand("SetFade")>
        <ScriptDescription("Sets the fade of the screen.")>
        Private Function M_SetFade(ByVal argument As String) As String
            If Controller.GetInstance().Context = ScriptContext.Overworld Then
                OverworldScreen.FadeValue = Int(argument).Clamp(0, 255)
            ElseIf Controller.GetInstance().Context = ScriptContext.NewGame Then
                Screens.MainMenu.NewNewGameScreen.FadeValue = Int(argument).Clamp(0, 255)
            End If

            Return Core.Null
        End Function

        <ScriptCommand("ShowPokemon")>
        <ScriptDescription("Displays a Pokémon preview on the screen.")>
        Private Function M_ShowPokemon(ByVal argument As String) As String
            Dim PokemonID As Integer = Int(argument.GetSplit(0))
            Dim Shiny As Boolean = Bool(argument.GetSplit(1))
            Dim Front As Boolean = Bool(argument.GetSplit(2))

            Screen.PokemonImageView.Show(PokemonID, Shiny, Front)

            ActiveLine.EndExecutionFrame = True

            Return Core.Null
        End Function

#End Region

#Region "Constructs"

        <ScriptConstruct("SelectedSkin")>
        <ScriptDescription("Returns the skin that was selected on the skin selection screen.")>
        Private Function F_SelectedSkin(ByVal argument As String) As String
            Return Screens.MainMenu.NewNewGameScreen.CharacterSelectionScreen.SelectedSkin
        End Function

#End Region

    End Class

End Namespace