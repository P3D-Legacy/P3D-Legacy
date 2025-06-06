﻿Namespace ScriptVersion2

    Partial Class ScriptCommander

        ' --------------------------------------------------------------------------------------------------------------------------
        ' Contains the @pokemon commands.
        ' --------------------------------------------------------------------------------------------------------------------------

        Private Shared Sub DoPokemon(ByVal subClass As String)
            Dim command As String = ScriptComparer.GetSubClassArgumentPair(subClass).Command
            Dim argument As String = ScriptComparer.GetSubClassArgumentPair(subClass).Argument

            Select Case command.ToLower()
                Case "cry"
                    Dim PokemonID As String = argument.GetSplit(0)
                    Dim PokemonAddition As String = "xXx"
                    If PokemonID.Contains("_") Then
                        PokemonAddition = PokemonForms.GetAdditionalValueFromDataFile(argument.GetSplit(0))
                        PokemonID = argument.GetSplit(0).GetSplit(0, "_")
                    End If
                    If PokemonID.Contains(";") Then
                        PokemonAddition = argument.GetSplit(0).GetSplit(1, ";")
                        PokemonID = argument.GetSplit(0).GetSplit(0, ";")
                    End If

                    Dim p As Pokemon = Pokemon.GetPokemonByID(CInt(PokemonID), PokemonAddition, True)
                    p.PlayCry()
                Case "remove"
                    Dim index As Integer = int(argument)
                    If Core.Player.Pokemons.Count - 1 >= index Then
                        Logger.Debug("Remove Pokémon (" & Core.Player.Pokemons(index).GetDisplayName() & ") at index " & index)
                        Core.Player.Pokemons.RemoveAt(index)
                    End If
                Case "add"
                    ' @Pokemon.Add([PartyIndex], PokemonData)
                    ' @Pokemon.Add(PokemonID, Level, [Method], [BallID], [Location], [isEgg], [trainerName], [heldItem], [isShiny])

                    If argument.StartsWith("{") = True Or argument.Remove(0, 1).StartsWith(",{") = True Then
                        Dim insertIndex As Integer = Core.Player.Pokemons.Count
                        If argument.Remove(0, 1).StartsWith(",{") = True Then
                            insertIndex = int(argument.GetSplit(0))
                        End If

                        argument = argument.Remove(0, argument.IndexOf("{"))

                        Dim p As Pokemon = Pokemon.GetPokemonByData(argument.Replace("§", ",").Replace("«", "[").Replace("»", "]"))
                        Core.Player.Pokemons.Insert(insertIndex, p)

                        Dim pokedexType As Integer = 2
                        If p.IsShiny = True Then
                            pokedexType = 3
                        End If

                        If p.IsEgg() = False Then
                            Dim dexID As String = PokemonForms.GetPokemonDataFileName(p.Number, p.AdditionalData, True)

                            Core.Player.PokedexData = Pokedex.ChangeEntry(Core.Player.PokedexData, dexID, pokedexType)
                        End If
                    Else
                        Dim commas As Integer = 0
                        For Each c As Char In argument
                            If c = "," Then
                                commas += 1
                            End If
                        Next

                        Dim PokemonID As String = argument.GetSplit(0)
                        Dim PokemonAddition As String = "xXx"
                        If PokemonID.Contains("_") Then
                            PokemonAddition = PokemonForms.GetAdditionalValueFromDataFile(argument.GetSplit(0))
                            PokemonID = argument.GetSplit(0).GetSplit(0, "_")
                        End If
                        If PokemonID.Contains(";") Then
                            PokemonAddition = argument.GetSplit(0).GetSplit(1, ";")
                            PokemonID = argument.GetSplit(0).GetSplit(0, ";")
                        End If
                        Dim Level As Integer = int(argument.GetSplit(1))

                        Dim catchMethod As String = Localization.GetString("CatchMethod_Empty", "Somehow obtained at")
                        If commas > 1 Then
                            catchMethod = ScriptCommander.Parse(argument.GetSplit(2)).ToString()
                        End If

                        Dim catchBall As Item = Item.GetItemByID(5.ToString)
                        If commas > 2 Then
                            catchBall = Item.GetItemByID(argument.GetSplit(3))
                        End If

                        Dim catchLocation As String = Localization.GetString("Places_" & Screen.Level.MapName, Screen.Level.MapName)
                        If commas > 3 Then
                            catchLocation = ScriptCommander.Parse(argument.GetSplit(4)).ToString()
                        End If

                        Dim isEgg As Boolean = False
                        If commas > 4 Then
                            isEgg = CBool(argument.GetSplit(5))
                        End If

                        Dim catchTrainer As String = Core.Player.Name
                        If commas > 5 And argument.GetSplit(6) <> "<playername>" And argument.GetSplit(6) <> "<player.name>" Then
                            catchTrainer = ScriptCommander.Parse(argument.GetSplit(6)).ToString()
                        End If

                        Dim heldItem As String = 0.ToString
                        If commas > 6 Then
                            heldItem = argument.GetSplit(7)
                        End If

                        Dim isShiny As Boolean = False
                        If Core.Random.Next(0, P3D.Pokemon.MasterShinyRate + 1) = 0 Then
                            isShiny = True
                        End If
                        If commas > 7 Then
                            isShiny = CBool(argument.GetSplit(8))
                        End If

                        Dim Pokemon As Pokemon = Pokemon.GetPokemonByID(int(PokemonID), PokemonAddition)
                        Pokemon.Generate(Level, True, PokemonAddition)

                        Pokemon.CatchTrainerName = catchTrainer
                        Pokemon.OT = Core.Player.OT

                        Pokemon.CatchLocation = catchLocation
                        Pokemon.CatchBall = catchBall
                        Pokemon.CatchMethod = catchMethod

                        If isEgg = True Then
                            Pokemon.EggSteps = 1
                            Pokemon.SetCatchInfos(Item.GetItemByID(5.ToString), Localization.GetString("CatchMethod_Obtained", "Obtained at"))
                        Else
                            Pokemon.EggSteps = 0
                        End If

                        If heldItem <> 0.ToString Then
                            Pokemon.Item = Item.GetItemByID(heldItem)
                        End If

                        Pokemon.IsShiny = isShiny

                        Core.Player.Pokemons.Add(Pokemon)

                        Dim pokedexType As Integer = 2
                        If Pokemon.IsShiny = True Then
                            pokedexType = 3
                        End If

                        If Pokemon.IsEgg() = False Then
                            Dim dexID As String = PokemonForms.GetPokemonDataFileName(Pokemon.Number, Pokemon.AdditionalData, True)

                            Core.Player.PokedexData = Pokedex.ChangeEntry(Core.Player.PokedexData, dexID, pokedexType)
                        End If
                    End If
                Case "setadditionalvalue", "setadditionaldata"
                    Dim Index As Integer = int(argument.GetSplit(0, ","))
                    Dim AdditionalValue As String = argument.GetSplit(1, ",")

                    If Core.Player.Pokemons.Count - 1 >= Index Then
                        Core.Player.Pokemons(Index).AdditionalData = AdditionalValue
                    End If
                Case "setnickname"
                    Dim Index As Integer = int(argument.GetSplit(0, ","))
                    Dim NickName As String = argument.GetSplit(1, ",")

                    If Core.Player.Pokemons.Count - 1 >= Index Then
                        Core.Player.Pokemons(Index).NickName = NickName
                    End If
                Case "setstat"
                    Dim Index As Integer = int(argument.GetSplit(0, ","))
                    Dim stat As String = argument.GetSplit(1, ",")
                    Dim statValue As Integer = int(argument.GetSplit(2, ","))

                    If Core.Player.Pokemons.Count - 1 >= Index Then
                        With Core.Player.Pokemons(Index)
                            Select Case stat.ToLower()
                                Case "maxhp", "hp"
                                    .MaxHP = statValue
                                Case "chp"
                                    .HP = statValue
                                Case "atk", "attack"
                                    .Attack = statValue
                                Case "def", "defense"
                                    .Defense = statValue
                                Case "spatk", "specialattack", "spattack"
                                    .SpAttack = statValue
                                Case "spdef", "specialdefense", "spdefense"
                                    .SpDefense = statValue
                                Case "speed"
                                    .Speed = statValue
                            End Select
                        End With
                    End If
                Case "clear"
                    Core.Player.Pokemons.Clear()
                Case "removeattack"
                    Dim Index As Integer = int(argument.GetSplit(0, ","))
                    Dim attackIndex As Integer = int(argument.GetSplit(1, ","))

                    If Core.Player.Pokemons.Count - 1 >= Index Then
                        Dim p As Pokemon = Core.Player.Pokemons(Index)

                        If p.Attacks.Count - 1 >= attackIndex Then
                            p.Attacks.RemoveAt(attackIndex)
                        End If
                    End If
                Case "removeattackid"
                    Dim Index As Integer = int(argument.GetSplit(0, ","))
                    Dim attackId As Integer = int(argument.GetSplit(1, ","))

                    If Core.Player.Pokemons.Count - 1 >= Index Then
                        Dim p As Pokemon = Core.Player.Pokemons(Index)

                        For a = 0 To (p.Attacks.Count - 1)
                            If p.Attacks(a).ID = attackId Then
                                p.Attacks.RemoveAt(a)
                                Exit For
                            End If
                        Next
                    End If
                Case "clearattacks"
                    Dim Index As Integer = int(argument)

                    If Core.Player.Pokemons.Count - 1 >= Index Then
                        Core.Player.Pokemons(Index).Attacks.Clear()
                    End If
                Case "addattack"
                    Dim Index As Integer = int(argument.GetSplit(0, ","))
                    Dim attackID As Integer = int(argument.GetSplit(1, ","))

                    If Core.Player.Pokemons.Count - 1 >= Index Then
                        Dim p As Pokemon = Core.Player.Pokemons(Index)

                        If p.Attacks.Count < 4 Then
                            Dim newAttack As BattleSystem.Attack = BattleSystem.Attack.GetAttackByID(attackID)
                            p.Attacks.Add(newAttack)
                        End If
                    End If
                Case "setshiny"
                    Dim Index As Integer = int(argument.GetSplit(0, ","))
                    Dim isShiny As Boolean = CBool(argument.GetSplit(1, ","))

                    If Core.Player.Pokemons.Count - 1 >= Index Then
                        Core.Player.Pokemons(Index).IsShiny = isShiny
                    End If
                Case "setshinyall"
                    Dim isShiny As Boolean = CBool(argument.GetSplit(0, ","))
                    For i = 0 To Core.Player.Pokemons.Count - 1
                        Core.Player.Pokemons(i).IsShiny = isShiny
                    Next
                Case "changelevel"
                    Dim Index As Integer = int(argument.GetSplit(0, ","))
                    Dim newLevel As Integer = int(argument.GetSplit(1, ","))

                    If Core.Player.Pokemons.Count - 1 >= Index Then
                        Core.Player.Pokemons(Index).Level = newLevel
                    End If
                Case "gainexp"
                    Dim Index As Integer = int(argument.GetSplit(0, ","))
                    Dim exp As Integer = int(argument.GetSplit(1, ","))

                    If Core.Player.Pokemons.Count - 1 >= Index Then
                        Core.Player.Pokemons(Index).Experience += exp
                    End If
                Case "setnature"
                    Dim Index As Integer = int(argument.GetSplit(0, ","))
                    Dim Nature As Pokemon.Natures = Pokemon.ConvertIDToNature(int(argument.GetSplit(1, ",")))

                    If Core.Player.Pokemons.Count - 1 >= Index Then
                        Core.Player.Pokemons(Index).Nature = Nature
                    End If
                Case "npctrade"
                    Dim splits() As String = argument.Split(CChar("|"))
                    Script.SaveNPCTrade = splits

                    Dim selScreen = New PartyScreen(Core.CurrentScreen, Item.GetItemByID(5.ToString), AddressOf Script.DoNPCTrade, "Choose Pokémon for trade", True) With {.Mode = Screens.UI.ISelectionScreen.ScreenMode.Selection, .CanExit = True}
                    AddHandler selScreen.SelectedObject, AddressOf Script.DoNPCTradeHandler

                    Core.SetScreen(selScreen)

                    CType(Core.CurrentScreen, PartyScreen).ExitedSub = AddressOf Script.ExitedNPCTrade

                    CanContinue = False
                Case "rename"
                    Dim index As String = argument
                    Dim renameOTcheck As Boolean = False
                    Dim canRename As Boolean = True

                    If argument.Contains(",") = True Then
                        index = argument.GetSplit(0)
                        renameOTcheck = CBool(argument.GetSplit(1))
                    End If

                    Dim PokemonIndex As Integer = 0
                    Select Case index.ToLower()
                        Case "last"
                            PokemonIndex = Core.Player.Pokemons.Count - 1
                        Case Else
                            PokemonIndex = int(index)
                    End Select

                    If renameOTcheck = True Then
                        If Core.Player.Pokemons(PokemonIndex).OT = Core.Player.OT Then
                            canRename = False
                        End If
                    End If

                    If Core.Player.Pokemons(PokemonIndex).IsEgg() = False Then
                        If canRename = True Then
                            Core.SetScreen(New NameObjectScreen(Core.CurrentScreen, Core.Player.Pokemons(PokemonIndex)))
                        Else
                            Screen.TextBox.Show("I cannot rename this~Pokémon because the~OT is different!*Did you receive it in~a trade or something?")
                        End If
                    Else
                        Screen.TextBox.Show("I cannot rename~this egg...")
                    End If

                    CanContinue = False
                Case "read"
                    Dim p As Pokemon = Core.Player.Pokemons(int(argument))

                    Dim message As String = "Hm... I see your~" & p.GetDisplayName()
                    Dim addmessage As String = "~is very stable with~"

                    If p.EVAttack > p.EVDefense And p.EVAttack > p.EVHP And p.EVAttack > p.EVSpAttack And p.EVAttack > p.EVSpDefense And p.EVAttack > p.EVSpeed Then
                        addmessage &= "performing physical moves."
                    End If
                    If p.EVDefense > p.EVAttack And p.EVDefense > p.EVHP And p.EVDefense > p.EVSpAttack And p.EVDefense > p.EVSpDefense And p.EVDefense > p.EVSpeed Then
                        addmessage &= "taking hits."
                    End If
                    If p.EVHP > p.EVAttack And p.EVHP > p.EVDefense And p.EVHP > p.EVSpAttack And p.EVHP > p.EVSpDefense And p.EVHP > p.EVSpeed Then
                        addmessage &= "taking damage."
                    End If
                    If p.EVSpAttack > p.EVAttack And p.EVSpAttack > p.EVDefense And p.EVSpAttack > p.EVHP And p.EVSpAttack > p.EVSpDefense And p.EVSpAttack > p.EVSpeed Then
                        addmessage &= "performing complex strategies."
                    End If
                    If p.EVSpDefense > p.EVAttack And p.EVSpDefense > p.EVDefense And p.EVSpDefense > p.EVHP And p.EVSpDefense > p.EVSpAttack And p.EVSpDefense > p.EVSpeed Then
                        addmessage &= "breaking strategies."
                    End If
                    If p.EVSpeed > p.EVAttack And p.EVSpeed > p.EVDefense And p.EVSpeed > p.EVHP And p.EVSpeed > p.EVSpAttack And p.EVSpeed > p.EVSpDefense Then
                        addmessage &= "speeding the others out."
                    End If

                    If addmessage = "~is very stable with~" Then
                        addmessage = "~is very well balanced."
                    End If

                    message &= addmessage

                    message &= "*...~...*What that means?~I am not sure..."

                    Screen.TextBox.Show(message, {}, False, False)

                    CanContinue = False
                Case "heal"
                    If argument = "" Then
                        Core.Player.HealParty()
                    Else
                        If argument.Contains(",") = True Then
                            Dim data() As String = argument.Split(CChar(","))
                            Dim Members As New List(Of Integer)
                            For Each member As String In data
                                Members.Add(int(member))
                            Next
                            Core.Player.HealParty(Members.ToArray())
                        Else
                            Core.Player.HealParty({int(argument)})
                        End If
                    End If
                Case "setfriendship"
                    Dim index As Integer = int(argument.GetSplit(0))
                    Dim amount As Integer = int(argument.GetSplit(1))

                    Core.Player.Pokemons(index).Friendship = amount
                Case "addfriendship"
                    Dim index As Integer = int(argument.GetSplit(0))
                    Dim amount As Integer = int(argument.GetSplit(1))

                    Core.Player.Pokemons(index).Friendship += amount
                Case "select"
                    Dim canExit As Boolean = False
                    Dim canChooseEgg As Boolean = True
                    Dim canChooseFainted As Boolean = True
                    Dim canLearnAttack As Integer = -1
                    Dim selectButtonText As String = Localization.GetString("global_select", "Select")

                    If argument <> "" Then
                        Dim data() As String = argument.Split(CChar(","))

                        If data.Length > 0 Then
                            canExit = CBool(data(0))
                        End If
                        If data.Length > 1 Then
                            canChooseFainted = CBool(data(1))
                        End If
                        If data.Length > 2 Then
                            canChooseEgg = CBool(data(2))
                        End If
                        If data.Length > 3 Then
                            canLearnAttack = CInt(data(3))
                        End If
                        If data.Length > 4 Then
                            selectButtonText = data(4)
                        End If
                    End If

                    Dim selScreen = New PartyScreen(Core.CurrentScreen, Item.GetItemByID(5.ToString), Nothing, Localization.GetString("party_screen_ChoosePokemon", "Choose Pokémon"), canExit, canChooseFainted, canChooseEgg) With {.Mode = Screens.UI.ISelectionScreen.ScreenMode.Selection, .CanExit = canExit, .SelectButtonText = selectButtonText}

                    If canLearnAttack <> -1 Then
                        selScreen = New PartyScreen(Core.CurrentScreen, Item.GetItemByID(5.ToString), Nothing, Localization.GetString("global_Learn", "Learn") & " " & BattleSystem.Attack.GetAttackByID(canLearnAttack).Name, canExit, canChooseFainted, canChooseEgg) With {.Mode = Screens.UI.ISelectionScreen.ScreenMode.Selection, .CanExit = canExit, .SelectButtonText = selectButtonText}
                        selScreen.SetupLearnAttack(BattleSystem.Attack.GetAttackByID(canLearnAttack), 2, Nothing)
                    End If
                    AddHandler selScreen.SelectedObject, Nothing

                    Core.SetScreen(selScreen)

                    CanContinue = False
                Case "selectmove"
                    Dim index As Integer = 0
                    Dim canHMMOve As Boolean = True
                    Dim canExit As Boolean = False

                    If argument.Contains(",") = True Then
                        Dim args As List(Of String) = argument.Split(CChar(",")).ToList()

                        For i = 0 To args.Count - 1
                            Dim arg As String = args(i)

                            Select Case i
                                Case 0
                                    index = int(arg)
                                Case 1
                                    canHMMOve = CBool(arg)
                                Case 2
                                    canExit = CBool(arg)
                            End Select
                        Next
                    Else
                        index = int(argument)
                    End If

                    Core.SetScreen(New ChooseAttackScreen(Core.CurrentScreen, Core.Player.Pokemons(index), canHMMOve, canExit, Nothing))

                    CanContinue = False
                Case "calcstats"
                    Dim index As Integer = int(argument)
                    Core.Player.Pokemons(index).CalculateStats()
                Case "learnattack"
                    Dim index As Integer = int(argument.GetSplit(0))
                    Dim attackID As Integer = int(argument.GetSplit(1))

                    Core.SetScreen(New LearnAttackScreen(Core.CurrentScreen, Core.Player.Pokemons(index), BattleSystem.Attack.GetAttackByID(attackID)))

                    CanContinue = False
                Case "setgender"
                    Dim Index As Integer = int(argument.GetSplit(0, ","))
                    Dim Gender As Integer = int(argument.GetSplit(1, ","))

                    If Core.Player.Pokemons.Count - 1 >= Index And Gender >= 0 And Gender <= 2 Then
                        Core.Player.Pokemons(Index).Gender = CType(Gender, Pokemon.Genders)
                    End If
                Case "setability"
                    Dim Index As Integer = int(argument.GetSplit(0, ","))
                    Dim abilityID As Integer = int(argument.GetSplit(1, ","))

                    If Core.Player.Pokemons.Count - 1 >= Index Then
                        Core.Player.Pokemons(Index).Ability = Ability.GetAbilityByID(abilityID)
                    End If
                Case "addev"
                    Dim Index As Integer = int(argument.GetSplit(0, ","))
                    Dim ev As String = argument.GetSplit(1, ",")
                    Dim evValue As Integer = int(argument.GetSplit(2, ","))

                    If Core.Player.Pokemons.Count - 1 >= Index Then
                        With Core.Player.Pokemons(Index)
                            Dim TotalEV As Integer = .EVHP + .EVAttack + .EVDefense + .EVSpAttack + .EVSpDefense + .EVSpeed
                            If TotalEV + evValue > 510 Then
                                evValue = 510 - TotalEV
                            End If

                            Select Case ev.ToLower()
                                Case "hp"
                                    If .EVHP + evValue > 255 Then
                                        evValue = 255 - .EVHP
                                    End If
                                    .EVHP += evValue
                                Case "atk", "attack"
                                    If .EVAttack + evValue > 255 Then
                                        evValue = 255 - .EVAttack
                                    End If
                                    .EVAttack += evValue
                                Case "def", "defense"
                                    If .EVDefense + evValue > 255 Then
                                        evValue = 255 - .EVDefense
                                    End If
                                    .EVDefense += evValue
                                Case "spatk", "specialattack", "spattack"
                                    If .EVSpAttack + evValue > 255 Then
                                        evValue = 255 - .EVSpAttack
                                    End If
                                    .EVSpAttack += evValue
                                Case "spdef", "specialdefense", "spdefense"
                                    If .EVSpDefense + evValue > 255 Then
                                        evValue = 255 - .EVSpDefense
                                    End If
                                    .EVSpDefense += evValue
                                Case "speed"
                                    If .EVSpeed + evValue > 255 Then
                                        evValue = 255 - .EVSpeed
                                    End If
                                    .EVSpeed += evValue
                            End Select
                        End With
                    End If
                Case "setev"
                    Dim Index As Integer = int(argument.GetSplit(0, ","))
                    Dim ev As String = argument.GetSplit(1, ",")
                    Dim evValue As Integer = int(argument.GetSplit(2, ","))

                    If Core.Player.Pokemons.Count - 1 >= Index Then
                        With Core.Player.Pokemons(Index)
                            Select Case ev.ToLower()
                                Case "hp"
                                    .EVHP = evValue
                                Case "atk", "attack"
                                    .EVAttack = evValue
                                Case "def", "defense"
                                    .EVDefense = evValue
                                Case "spatk", "specialattack", "spattack"
                                    .EVSpAttack = evValue
                                Case "spdef", "specialdefense", "spdefense"
                                    .EVSpDefense = evValue
                                Case "speed"
                                    .EVSpeed = evValue
                            End Select
                        End With
                    End If
                Case "setallevs"
                    Dim Index As Integer = int(argument.GetSplit(0, ","))
                    If Core.Player.Pokemons.Count - 1 >= Index Then
                        With Core.Player.Pokemons(Index)
                            .EVHP = Clamp(int(argument.GetSplit(1, ",")), 0, 255)
                            .EVAttack = Clamp(int(argument.GetSplit(2, ",")), 0, 255)
                            .EVDefense = Clamp(int(argument.GetSplit(3, ",")), 0, 255)
                            .EVSpAttack = Clamp(int(argument.GetSplit(4, ",")), 0, 255)
                            .EVSpDefense = Clamp(int(argument.GetSplit(5, ",")), 0, 255)
                            .EVSpeed = Clamp(int(argument.GetSplit(6, ",")), 0, 255)
                        End With
                    End If
                Case "setiv"
                    Dim Index As Integer = int(argument.GetSplit(0, ","))
                    Dim dv As String = argument.GetSplit(1, ",")
                    Dim dvValue As Integer = int(argument.GetSplit(2, ","))

                    If Core.Player.Pokemons.Count - 1 >= Index Then
                        With Core.Player.Pokemons(Index)
                            Select Case dv.ToLower()
                                Case "hp"
                                    .IVHP = dvValue
                                Case "atk", "attack"
                                    .IVAttack = dvValue
                                Case "def", "defense"
                                    .IVDefense = dvValue
                                Case "spatk", "specialattack", "spattack"
                                    .IVSpAttack = dvValue
                                Case "spdef", "specialdefense", "spdefense"
                                    .IVSpDefense = dvValue
                                Case "speed"
                                    .IVSpeed = dvValue
                            End Select
                        End With
                    End If
                Case "setallivs"
                    Dim Index As Integer = int(argument.GetSplit(0, ","))
                    If Core.Player.Pokemons.Count - 1 >= Index Then
                        With Core.Player.Pokemons(Index)
                            .IVHP = Clamp(int(argument.GetSplit(1, ",")), 0, 31)
                            .IVAttack = Clamp(int(argument.GetSplit(2, ",")), 0, 31)
                            .IVDefense = Clamp(int(argument.GetSplit(3, ",")), 0, 31)
                            .IVSpAttack = Clamp(int(argument.GetSplit(4, ",")), 0, 31)
                            .IVSpDefense = Clamp(int(argument.GetSplit(5, ",")), 0, 31)
                            .IVSpeed = Clamp(int(argument.GetSplit(6, ",")), 0, 31)
                        End With
                    End If
                Case "registerhalloffame"
                    Dim count As Integer = -1
                    Dim NewHallOfFameData As String = ""
                    If Core.Player.HallOfFameData <> "" Then
                        Dim data() As String = Core.Player.HallOfFameData.SplitAtNewline()

                        For Each l As String In data
                            Dim id As Integer = CInt(l.Remove(l.IndexOf(",")))
                            If id > count Then
                                count = id
                            End If
                        Next

                        For Each l As String In data
                            Dim id As Integer = CInt(l.Remove(l.IndexOf(",")))
                            If id > (count - 19) OrElse id = 0 Then 'last 20 entries saved, plus the first entry
                                NewHallOfFameData &= l & Environment.NewLine
                            End If
                        Next
                    End If

                    count += 1

                    Dim time As String = TimeHelpers.GetDisplayTime(TimeHelpers.GetCurrentPlayTime(), True)

                    Dim newData As String

                    If Core.Player.IsGameJoltSave Then
                        newData = count & ",(" & Core.Player.Name & "|" & time & "|" & GameJoltSave.Points & "|" & Core.Player.OT & "|" & Core.Player.Skin & ")"
                    Else
                        newData = count & ",(" & Core.Player.Name & "|" & time & "|" & Core.Player.Points & "|" & Core.Player.OT & "|" & Core.Player.Skin & ")"
                    End If

                    For Each p As Pokemon In Core.Player.Pokemons
                        If p.IsEgg() = False Then
                            Dim pData As String = p.GetHallOfFameData()
                            newData &= Environment.NewLine & count & "," & pData
                        End If
                    Next

                    NewHallOfFameData &= newData
                    Core.Player.HallOfFameData = NewHallOfFameData
                Case "setot"
                    Dim Index As Integer = int(argument.GetSplit(0, ","))
                    Dim OT As String = argument.GetSplit(1, ",")

                    If Core.Player.Pokemons.Count - 1 >= Index Then
                        Core.Player.Pokemons(Index).OT = OT
                    End If
                Case "setitem"
                    Dim Index As Integer = int(argument.GetSplit(0, ","))
                    Dim newItem As Item = Item.GetItemByID(argument.GetSplit(1, ","))

                    If Core.Player.Pokemons.Count - 1 >= Index Then
                        Core.Player.Pokemons(Index).Item = newItem
                    End If
                Case "removeitem"
                    Dim Index As Integer = int(argument.GetSplit(0, ","))

                    Core.Player.Pokemons(Index).Item = Nothing
                Case "setitemdata"
                    Dim Index As Integer = int(argument.GetSplit(0, ","))
                    Dim itemData As String = argument.Remove(0, argument.IndexOf(",") + 1)

                    If Core.Player.Pokemons.Count - 1 >= Index Then
                        If Not Core.Player.Pokemons(Index).Item Is Nothing Then
                            Core.Player.Pokemons(Index).Item.AdditionalData = itemData
                        End If
                    End If
                Case "setcatchtrainer"
                    Dim Index As Integer = int(argument.GetSplit(0, ","))
                    Dim Trainer As String = argument.GetSplit(1, ",")

                    If Core.Player.Pokemons.Count - 1 >= Index Then
                        Core.Player.Pokemons(Index).CatchTrainerName = Trainer
                    End If
                Case "setcatchball"
                    Dim Index As Integer = int(argument.GetSplit(0, ","))
                    Dim catchBall As String = argument.GetSplit(1, ",")

                    If Core.Player.Pokemons.Count - 1 >= Index Then
                        Core.Player.Pokemons(Index).CatchBall = Item.GetItemByID(catchBall)
                    End If
                Case "setcatchmethod"
                    Dim Index As Integer = int(argument.GetSplit(0, ","))
                    Dim methodLocalization As String = argument.GetSplit(1, ",")

                    If argument.GetSplit(1, ",").StartsWith("<system.token(") AndAlso argument.GetSplit(1, ",").EndsWith(")>") Then
                        methodLocalization = Localization.GetString(methodLocalization.Remove(0, "<system.token(".Length).Remove(argument.GetSplit(2).Length - 2, 2))
                    End If

                    If Core.Player.Pokemons.Count - 1 >= Index Then
                        Core.Player.Pokemons(Index).CatchMethod = methodLocalization
                    End If
                Case "setcatchplace", "setcatchlocation"
                    Dim Index As Integer = int(argument.GetSplit(0, ","))
                    Dim placeLocalization As String = argument.GetSplit(1, ",")

                    If argument.GetSplit(1, ",").StartsWith("<system.token(") AndAlso argument.GetSplit(1, ",").EndsWith(")>") Then
                        placeLocalization = Localization.GetString(placeLocalization.Remove(0, "<system.token(".Length).Remove(argument.GetSplit(2).Length - 2, 2))
                    ElseIf Localization.TokenExists("Places_" & Screen.Level.MapName) = True Then
                        placeLocalization = Localization.GetString("Places_" & Screen.Level.MapName, Screen.Level.MapName)
                    End If

                    If Core.Player.Pokemons.Count - 1 >= Index Then
                        Core.Player.Pokemons(Index).CatchLocation = placeLocalization
                    End If
                Case "newroaming"
                    ' RoamerID,PokémonID,Level,regionID,startLevelFile,MusicLoop,[Shiny],[ScriptPath]
                    Dim data() As String = argument.Split(CChar(","))

                    Dim PokemonID As String = data(1)
                    Dim PokemonAddition As String = "xXx"
                    If PokemonID.Contains("_") Then
                        PokemonAddition = PokemonForms.GetAdditionalValueFromDataFile(data(1))
                        PokemonID = data(1).GetSplit(0, "_")
                    End If
                    If PokemonID.Contains(";") Then
                        PokemonAddition = data(1).GetSplit(1, ";")
                        PokemonID = data(1).GetSplit(0, ";")
                    End If

                    Dim p As Pokemon = Pokemon.GetPokemonByID(CInt(PokemonID), PokemonAddition)
                    p.Generate(CInt(data(2)), True, PokemonAddition)

                    If data.Length > 6 AndAlso data(6) <> "" AndAlso data(6) <> "-1" Then
                        p.IsShiny = CBool(data(6))
                    End If

                    Dim ScriptPath As String = ""
                    If data.Length > 7 AndAlso data(7) <> "" Then
                        ScriptPath = data(7)
                    End If

                    If Core.Player.RoamingPokemonData <> "" Then
                        Core.Player.RoamingPokemonData &= Environment.NewLine
                    End If

                    Core.Player.RoamingPokemonData &= data(0) & "|" & data(1) & "|" & data(2) & "|" & data(3) & "|" & data(4) & "|" & data(5) & "|" & p.IsShiny & "|" & p.GetSaveData() & "|" & ScriptPath
                Case "evolve"
                    Dim args() As String = argument.Split(CChar(","))
                    Dim triggerStr As String = "level"
                    If args.Count > 1 Then
                        triggerStr = args(1)
                    End If
                    Dim trigger As EvolutionCondition.EvolutionTrigger = EvolutionCondition.EvolutionTrigger.LevelUp
                    Dim evolutionArg As String = ""

                    If args.Count > 2 Then
                        evolutionArg = args(2)
                    End If

                    Dim p As Pokemon = Core.Player.Pokemons(int(args(0)))

                    Select Case triggerStr
                        Case "level", "levelup", "level up", "level-up"
                            trigger = EvolutionCondition.EvolutionTrigger.LevelUp
                        Case "none"
                            trigger = EvolutionCondition.EvolutionTrigger.None
                        Case "itemuse", "item use", "item", "item-use"
                            trigger = EvolutionCondition.EvolutionTrigger.ItemUse
                        Case "trade", "trading"
                            trigger = EvolutionCondition.EvolutionTrigger.Trading
                    End Select

                    If p.CanEvolve(trigger, evolutionArg) = True Then
                        Core.SetScreen(New TransitionScreen(Core.CurrentScreen,
                                                             New EvolutionScreen(Core.CurrentScreen, {int(args(0))}.ToList(),
                                                                                 evolutionArg,
                                                                                 trigger,
                                                                                 False), Color.Black, False))

                        CanContinue = False
                    Else
                        Logger.Log(Logger.LogTypes.Message, "ScriptCommander.vb: The Pokémon is not able to evolve with the given conditions.")
                    End If
                Case "levelup"
                    Dim args() As String = argument.Split(CChar(","))
                    Dim p As Pokemon = Core.Player.Pokemons(int(args(0)))
                    Dim amount As Integer = 1

                    If args.Count > 1 Then
                        amount = CInt(args(1))
                    End If
                    Dim originalLevel As Integer = p.Level

                    Dim AttackLearnList As New List(Of BattleSystem.Attack)

                    If originalLevel < CInt(GameModeManager.GetGameRuleValue("MaxLevel", "100")) AndAlso p.IsEgg() = False Then
                        If originalLevel + amount > CInt(GameModeManager.GetGameRuleValue("MaxLevel", "100")) Then
                            amount = CInt(GameModeManager.GetGameRuleValue("MaxLevel", "100")) - originalLevel
                        End If

                        p.Level += amount
                        If amount > 0 Then
                            For i = 1 To amount
                                If p.AttackLearns.ContainsKey(originalLevel + i) = True Then
                                    Dim aList As List(Of BattleSystem.Attack) = p.AttackLearns(originalLevel + i)
                                    For a = 0 To aList.Count - 1
                                        If AttackLearnList.Contains(aList(a)) = False AndAlso p.KnowsMove(aList(a)) = False Then
                                            AttackLearnList.Add(aList(a))
                                        End If
                                    Next
                                End If
                            Next
                        End If
                        Dim s As String = "version=2" & Environment.NewLine

                        If amount > 0 Then
                            s &= "@text.show(" & p.GetDisplayName() & " reached~level " & p.Level & "!)" & Environment.NewLine
                        End If

                        Dim currentMaxHP As Integer = p.MaxHP

                        p.CalculateStats()

                        'Heals the Pokémon by the HP difference.
                        Dim HPDifference As Integer = p.MaxHP - currentMaxHP
                        If HPDifference > 0 Then
                            p.Heal(HPDifference)
                        End If

                        If p.CanEvolve(EvolutionCondition.EvolutionTrigger.LevelUp, "") = True Then
                            s &= "@pokemon.evolve(" & int(args(0)) & ")" & Environment.NewLine
                        End If

                        If AttackLearnList.Count > 0 Then
                            For i = 0 To AttackLearnList.Count - 1
                                If p.Attacks.Count < 4 Then
                                    s &= "@text.show(" & p.GetDisplayName() & " learned " & AttackLearnList(i).Name & "!)" & Environment.NewLine
                                    p.Attacks.Add(AttackLearnList(i))
                                    PlayerStatistics.Track("Moves learned", 1)
                                Else
                                    s &= "@pokemon.learnattack(" & int(args(0)) & "," & AttackLearnList(i).ID & ")" & Environment.NewLine
                                End If
                            Next
                        End If
                        s &= ":end"

                        If CurrentScreen.Identification = Screen.Identifications.OverworldScreen Then
                            CType(CurrentScreen, OverworldScreen).ActionScript.StartScript(s, 2, False)
                        End If
                    End If
                Case "reload"
                    Dim PokemonIndex As Integer = int(argument)
                    If Core.Player.Pokemons.Count - 1 >= PokemonIndex Then
                        Core.Player.Pokemons(PokemonIndex).ReloadDefinitions()
                        Core.Player.Pokemons(PokemonIndex).CalculateStats()
                    End If
                Case "reloadall"
                    For i = 0 To Core.Player.Pokemons.Count - 1
                        Core.Player.Pokemons(i).ReloadDefinitions()
                        Core.Player.Pokemons(i).CalculateStats()
                    Next

                ''Just debug testing tools.
                ''Make sure X and Y megas hold the correct stone. Other megas may have no stone.
                Case "megaevolve"
                    Dim p As Pokemon = Core.Player.Pokemons(int(argument))
                    If p.Item IsNot Nothing Then
                        Select Case p.Item.ID
                            Case 516, 529
                                p.AdditionalData = "mega_x"
                            Case 517, 530
                                p.AdditionalData = "mega_y"
                            Case Else
                                p.AdditionalData = "mega"
                        End Select
                    Else
                        p.AdditionalData = "mega"
                    End If
                    p.ReloadDefinitions()
                    p.CalculateStats()
                    p.LoadAltAbility()
                Case "megaevolveall"
                    For i = 0 To Core.Player.Pokemons.Count - 1
                        Dim p As Pokemon = Core.Player.Pokemons(i)
                        If p.Item IsNot Nothing Then
                            Select Case p.Item.ID
                                Case 516, 529
                                    p.AdditionalData = "mega_x"
                                Case 517, 530
                                    p.AdditionalData = "mega_y"
                                Case Else
                                    p.AdditionalData = "mega"
                            End Select
                        Else
                            p.AdditionalData = "mega"
                        End If
                        p.ReloadDefinitions()
                        p.CalculateStats()
                        p.LoadAltAbility()
                    Next

                Case "clone"
                    Dim PokemonIndex As Integer = int(argument)

                    If Core.Player.Pokemons.Count - 1 >= PokemonIndex And Core.Player.Pokemons.Count < 6 Then
                        Core.Player.Pokemons.Add(Core.Player.Pokemons(PokemonIndex))
                    End If
                Case "sendtostorage"
                    ' @Pokemon.SendToStorage(PokeIndex, [BoxIndex])

                    Dim Data() As String = argument.Split(CChar(","))

                    If Data.Length = 1 Then
                        Dim PokemonIndex As Integer = int(Data(0))

                        If Core.Player.Pokemons.Count - 1 >= PokemonIndex Then
                            StorageSystemScreen.DepositPokemon(Core.Player.Pokemons(PokemonIndex))
                            Core.Player.Pokemons.RemoveAt(PokemonIndex)
                        End If
                    ElseIf Data.Length = 2 Then
                        Dim PokemonIndex As Integer = int(Data(0))

                        If Core.Player.Pokemons.Count - 1 >= PokemonIndex Then
                            StorageSystemScreen.DepositPokemon(Core.Player.Pokemons(PokemonIndex), int(Data(1)))
                            Core.Player.Pokemons.RemoveAt(PokemonIndex)
                        End If
                    End If
                Case "addtostorage"
                    ' @Pokemon.AddToStorage([BoxIndex], PokemonData)
                    ' @Pokemon.AddToStorage(PokemonID, Level, [Method], [BallID], [Location], [isEgg], [trainerName], [heldItem], [isShiny])

                    If argument.StartsWith("{") = True Or argument.Remove(0, argument.IndexOf(",")).StartsWith(",{") = True Then
                        Dim insertIndex As Integer = -1
                        If argument.Remove(0, argument.IndexOf(",")).StartsWith(",{") = True Then
                            insertIndex = int(argument.GetSplit(0))
                        End If

                        argument = argument.Remove(0, argument.IndexOf("{"))

                        Dim p As Pokemon = Pokemon.GetPokemonByData(argument.Replace("§", ",").Replace("«", "[").Replace("»", "]"))
                        StorageSystemScreen.DepositPokemon(p, insertIndex)

                        Dim pokedexType As Integer = 2
                        If p.IsShiny = True Then
                            pokedexType = 3
                        End If

                        If p.IsEgg() = False Then
                            Dim dexID As String = PokemonForms.GetPokemonDataFileName(p.Number, p.AdditionalData, True)
                            Core.Player.PokedexData = Pokedex.ChangeEntry(Core.Player.PokedexData, dexID, pokedexType)
                        End If
                    Else
                        Dim commas As Integer = 0
                        For Each c As Char In argument
                            If c = "," Then
                                commas += 1
                            End If
                        Next

                        Dim PokemonID As String = argument.GetSplit(0)
                        Dim PokemonAddition As String = "xXx"
                        If PokemonID.Contains("_") Then
                            PokemonAddition = PokemonForms.GetAdditionalValueFromDataFile(argument.GetSplit(0))
                            PokemonID = argument.GetSplit(0).GetSplit(0, "_")
                        End If
                        If PokemonID.Contains(";") Then
                            PokemonAddition = argument.GetSplit(0).GetSplit(1, ";")
                            PokemonID = argument.GetSplit(0).GetSplit(0, ";")
                        End If
                        Dim Level As Integer = int(argument.GetSplit(1))

                        Dim catchMethod As String = "random reason"
                        If commas > 1 Then
                            catchMethod = ScriptCommander.Parse(argument.GetSplit(2)).ToString()
                        End If

                        Dim catchBall As Item = Item.GetItemByID(5.ToString)
                        If commas > 2 Then
                            catchBall = Item.GetItemByID(argument.GetSplit(3))
                        End If

                        Dim catchLocation As String = Screen.Level.MapName
                        If commas > 3 Then
                            catchLocation = ScriptCommander.Parse(argument.GetSplit(4)).ToString()
                        End If

                        Dim isEgg As Boolean = False
                        If commas > 4 Then
                            isEgg = CBool(argument.GetSplit(5))
                        End If

                        Dim catchTrainer As String = Core.Player.Name
                        If commas > 5 And argument.GetSplit(6) <> "<playername>" And argument.GetSplit(6) <> "<player.name>" Then
                            catchTrainer = ScriptCommander.Parse(argument.GetSplit(6)).ToString()
                        End If

                        Dim heldItem As String = 0.ToString
                        If commas > 6 Then
                            heldItem = argument.GetSplit(7)
                        End If

                        Dim isShiny As Boolean = False
                        If Core.Random.Next(0, P3D.Pokemon.MasterShinyRate + 1) = 0 Then
                            isShiny = True
                        End If
                        If commas > 7 Then
                            isShiny = CBool(argument.GetSplit(8))
                        End If

                        Dim Pokemon As Pokemon = Pokemon.GetPokemonByID(int(PokemonID), PokemonAddition)
                        Pokemon.Generate(Level, True, PokemonAddition)

                        Pokemon.CatchTrainerName = catchTrainer
                        Pokemon.OT = Core.Player.OT

                        Pokemon.CatchLocation = catchLocation
                        Pokemon.CatchBall = catchBall
                        Pokemon.CatchMethod = catchMethod

                        If isEgg = True Then
                            Pokemon.EggSteps = 1
                            Pokemon.SetCatchInfos(Item.GetItemByID(5.ToString), Localization.GetString("CatchMethod_Obtained", "Obtained at"))
                        Else
                            Pokemon.EggSteps = 0
                        End If

                        If heldItem <> 0.ToString Then
                            Pokemon.Item = Item.GetItemByID(heldItem)
                        End If

                        Pokemon.IsShiny = isShiny

                        StorageSystemScreen.DepositPokemon(Pokemon)

                        Dim pokedexType As Integer = 2
                        If Pokemon.IsShiny = True Then
                            pokedexType = 3
                        End If

                        If Pokemon.IsEgg() = False Then
                            Dim dexID As String = PokemonForms.GetPokemonDataFileName(Pokemon.Number, Pokemon.AdditionalData)
                            If dexID.Contains("_") = False Then
                                If PokemonForms.GetAdditionalDataForms(Pokemon.Number) IsNot Nothing AndAlso PokemonForms.GetAdditionalDataForms(Pokemon.Number).Contains(Pokemon.AdditionalData) Then
                                    dexID = Pokemon.Number & ";" & Pokemon.AdditionalData
                                Else
                                    dexID = Pokemon.Number.ToString
                                End If
                            End If

                            Core.Player.PokedexData = Pokedex.ChangeEntry(Core.Player.PokedexData, dexID, pokedexType)
                        End If
                    End If
                Case "addsteps"
                    ' @Pokemon.AddSteps(PokemonIndex, StepsToAdd)

                    Dim Index As Integer = int(argument.GetSplit(0, ","))
                    Dim StepsToAdd As Integer = int(argument.GetSplit(1, ","))

                    If Core.Player.Pokemons.Count - 1 >= Index Then
                        Core.Player.Pokemons(Index).EggSteps += StepsToAdd
                    End If
                Case "setsteps"
                    ' @Pokemon.SetSteps(PokemonIndex, StepsToSet)

                    Dim Index As Integer = int(argument.GetSplit(0, ","))
                    Dim StepsToSet As Integer = int(argument.GetSplit(1, ","))

                    If Core.Player.Pokemons.Count - 1 >= Index Then
                        Core.Player.Pokemons(Index).EggSteps = StepsToSet
                    End If
                Case "hatch"
                    ' @Pokemon.Hatch(PartyIndex,[CanRename],[Message])
                    Dim Index As Integer = int(argument.GetSplit(0, ","))
                    Dim CanRename As Boolean = True
                    Dim Message As String = ""
                    Dim Pokemon As Pokemon = Nothing
                    If argument.Split(",").Count > 1 Then
                        CanRename = CBool(argument.GetSplit(1, ","))
                        If argument.Split(",").Count > 2 Then
                            Message = CStr(argument.GetSplit(2, ","))
                        End If
                    End If
                    If Core.Player.Pokemons.Count - 1 >= Index Then
                        Pokemon = Core.Player.Pokemons(Index)
                        Core.Player.Pokemons.Remove(Pokemon)
                    End If
                    If Pokemon.IsEgg() = True Then
                        Screen.TextBox.Show("Huh?")
                        SetScreen(New TransitionScreen(CType(CurrentScreen, OverworldScreen), New HatchEggScreen(CType(CurrentScreen, OverworldScreen), {Pokemon}.ToList, CanRename, Message), Color.White, False))
                        CanContinue = False
                    End If
                Case "setstatus"
                    Dim Index As Integer = int(argument.GetSplit(0, ","))
                    Dim Status As Integer = -1
                    Select Case argument.GetSplit(1, ",")
                        Case "brn"
                            Status = Pokemon.StatusProblems.Burn
                        Case "frz"
                            Status = Pokemon.StatusProblems.Freeze
                        Case "prz"
                            Status = Pokemon.StatusProblems.Paralyzed
                        Case "psn"
                            Status = Pokemon.StatusProblems.Poison
                        Case "bpsn"
                            Status = Pokemon.StatusProblems.BadPoison
                        Case "slp"
                            Status = Pokemon.StatusProblems.Sleep
                        Case "fnt"
                            Status = Pokemon.StatusProblems.Fainted
                        Case "none"
                            Status = Pokemon.StatusProblems.None
                        Case Else
                            Status = Pokemon.StatusProblems.None
                    End Select
                    If Status <> -1 AndAlso Core.Player.Pokemons.Count - 1 >= Index Then
                        Core.Player.Pokemons(Index).Status = CType(Status, Pokemon.StatusProblems)
                        If Status = Pokemon.StatusProblems.Fainted Then
                            Core.Player.Pokemons(Index).HP = 0
                        End If
                    End If
                Case "ride"
                    Dim Index As Integer = -1
                    If argument <> "" Then
                        If Core.Player.Pokemons(Index).KnowsMove(BattleSystem.Attack.GetAttackByID(560)) = False Then
                            Logger.Log(Logger.LogTypes.ErrorMessage, "The specified Pokémon does not know the move Ride. The specified index is: " & Index.ToString & ".")
                        Else
                            Index = CInt(argument)
                        End If
                    End If

                    If Index = -1 Then
                        For p = 0 To Core.Player.Pokemons.Count - 1
                            If Core.Player.Pokemons(p).KnowsMove(BattleSystem.Attack.GetAttackByID(560)) Then
                                Index = p
                                Exit For
                            End If
                        Next
                    End If

                    If Index <> -1 Then
                        If Screen.Level.Riding = True Then
                            If Screen.Level.RideType = 3 Then
                                Screen.TextBox.Show(Localization.GetString("fieldmove_ride_cannot_walk", "You cannot walk here!"), {}, True, False)
                            Else
                                Screen.Level.Riding = False
                                Screen.Level.OwnPlayer.SetTexture(Core.Player.TempRideSkin, True)
                                Core.Player.Skin = Core.Player.TempRideSkin
                                Screen.ChooseBox.Showing = False


                                If Screen.Level.IsRadioOn = False OrElse GameJolt.PokegearScreen.StationCanPlay(Screen.Level.SelectedRadioStation) = False Then
                                    MusicManager.Play(Screen.Level.MusicLoop, True, 0.01F)
                                End If
                            End If
                        Else
                            If Screen.Level.Surfing = False And Screen.Camera.IsMoving() = False And Screen.Camera.Turning = False And Screen.Level.CanRide() = True Then
                                Screen.ChooseBox.Showing = False

                                Screen.Level.Riding = True
                                Core.Player.TempRideSkin = Core.Player.Skin

                                Dim skin As String = "[POKEMON|"
                                If Core.Player.Pokemons(Index).IsShiny = True Then
                                    skin &= "S]"
                                Else
                                    skin &= "N]"
                                End If
                                skin &= Core.Player.Pokemons(Index).Number & PokemonForms.GetOverworldAddition(Core.Player.Pokemons(Index))

                                Screen.Level.OwnPlayer.SetTexture(skin, False)

                                SoundManager.PlayPokemonCry(Core.Player.Pokemons(Index).Number, PokemonForms.GetCrySuffix(Core.Player.Pokemons(Index)))

                                Screen.TextBox.Show(Core.Player.Pokemons(Index).GetDisplayName() & " " & Localization.GetString("fieldmove_ride_used", "used~Ride!"), {}, True, False)
                                PlayerStatistics.Track("Ride used", 1)

                                If Screen.Level.IsRadioOn = False OrElse GameJolt.PokegearScreen.StationCanPlay(Screen.Level.SelectedRadioStation) = False Then
                                    MusicManager.Play("ride", True)
                                End If
                            Else
                                Screen.TextBox.Show(Localization.GetString("fieldmove_ride_cannot_ride", "You cannot Ride here!"), {}, True, False)
                            End If
                        End If
                    End If

            End Select

            IsReady = True
        End Sub

    End Class

End Namespace