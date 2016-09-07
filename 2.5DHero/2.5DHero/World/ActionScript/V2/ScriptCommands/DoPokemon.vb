Namespace ScriptVersion2

    Partial Class ScriptCommander

        '--------------------------------------------------------------------------------------------------------------------------
        'Contains the @pokemon commands.
        '--------------------------------------------------------------------------------------------------------------------------

        Private Shared Sub DoPokemon(ByVal subClass As String)
            Dim command As String = ScriptComparer.GetSubClassArgumentPair(subClass).Command
            Dim argument As String = ScriptComparer.GetSubClassArgumentPair(subClass).Argument

            Select Case command.ToLower()
                Case "cry"
                    Dim PokemonID As Integer = int(argument)

                    Dim p As Pokemon = Pokemon.GetPokemonByID(PokemonID)
                    p.PlayCry()
                Case "remove"
                    Dim index As Integer = int(argument)
                    If Core.Player.Pokemons.Count - 1 >= index Then
                        Logger.Debug("Remove Pokémon (" & Core.Player.Pokemons(index).GetDisplayName() & ") at index " & index)
                        Core.Player.Pokemons.RemoveAt(index)
                    End If
                Case "add"
                    'PokemonID,Level,Method,Ball,Location,IsEgg,TrainerName

                    If argument.StartsWith("{") = True Or argument.Remove(0, 1).StartsWith(",{") = True Then
                        Dim insertIndex As Integer = Core.Player.Pokemons.Count
                        If argument.Remove(0, 1).StartsWith(",{") = True Then
                            insertIndex = int(argument.GetSplit(0))
                        End If

                        argument = argument.Remove(0, argument.IndexOf("{"))

                        Dim p As Pokemon = Pokemon.GetPokemonByData(argument.Replace("§", ","))
                        Core.Player.Pokemons.Insert(insertIndex, p)

                        Dim pokedexType As Integer = 2
                        If p.IsShiny = True Then
                            pokedexType = 3
                        End If

                        If p.IsEgg() = False Then
                            Core.Player.PokedexData = Pokedex.ChangeEntry(Core.Player.PokedexData, p.Number, pokedexType)
                        End If
                    Else
                        Dim commas As Integer = 0
                        For Each c As Char In argument
                            If c = "," Then
                                commas += 1
                            End If
                        Next

                        Dim PokemonID As Integer = int(argument.GetSplit(0))
                        Dim Level As Integer = int(argument.GetSplit(1))

                        Dim catchMethod As String = "random reason"
                        If commas > 1 Then
                            catchMethod = argument.GetSplit(2)
                        End If

                        Dim catchBall As Item = Item.GetItemByID(1)
                        If commas > 2 Then
                            catchBall = Item.GetItemByID(int(argument.GetSplit(3)))
                        End If

                        Dim catchLocation As String = Screen.Level.MapName
                        If commas > 3 Then
                            catchLocation = argument.GetSplit(4)
                        End If

                        Dim isEgg As Boolean = False
                        If commas > 4 Then
                            isEgg = CBool(argument.GetSplit(5))
                        End If

                        Dim catchTrainer As String = Core.Player.Name
                        If commas > 5 And argument.GetSplit(6) <> "<playername>" Then
                            catchTrainer = argument.GetSplit(6)
                        End If

                        Dim Pokemon As Pokemon = Pokemon.GetPokemonByID(PokemonID)
                        Pokemon.Generate(Level, True)

                        Pokemon.CatchTrainerName = catchTrainer
                        Pokemon.OT = Core.Player.OT

                        Pokemon.CatchLocation = catchLocation
                        Pokemon.CatchBall = catchBall
                        Pokemon.CatchMethod = catchMethod

                        If isEgg = True Then
                            Pokemon.EggSteps = 1
                            Pokemon.SetCatchInfos(Item.GetItemByID(5), "obtained at")
                        Else
                            Pokemon.EggSteps = 0
                        End If

                        Core.Player.Pokemons.Add(Pokemon)

                        Dim pokedexType As Integer = 2
                        If Pokemon.IsShiny = True Then
                            pokedexType = 3
                        End If

                        If Pokemon.IsEgg() = False Then
                            Core.Player.PokedexData = Pokedex.ChangeEntry(Core.Player.PokedexData, Pokemon.Number, pokedexType)
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

                    Core.SetScreen(New ChoosePokemonScreen(Core.CurrentScreen, Item.GetItemByID(5), AddressOf Script.DoNPCTrade, "Choose trade Pokémon", True))
                    CType(Core.CurrentScreen, ChoosePokemonScreen).ExitedSub = AddressOf Script.ExitedNPCTrade

                    CanContinue = False
                Case "hide"
                    Screen.Level.OverworldPokemon.Visible = False
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
                    End If

                    Core.SetScreen(New ChoosePokemonScreen(Core.CurrentScreen, New Items.Balls.Pokeball, Nothing, "Choose Pokémon", canExit, canChooseFainted, canChooseEgg))

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
                Case "registerhalloffame"
                    Dim count As Integer = -1

                    If Core.Player.HallOfFameData <> "" Then
                        Dim data() As String = Core.Player.HallOfFameData.SplitAtNewline()

                        For Each l As String In data
                            Dim id As Integer = CInt(l.Remove(l.IndexOf(",")))
                            If id > count Then
                                count = id
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
                            Dim pData As String = p.GetSaveData()
                            newData &= vbNewLine & count & "," & pData
                        End If
                    Next

                    If Core.Player.HallOfFameData <> "" Then
                        Core.Player.HallOfFameData &= vbNewLine
                    End If

                    Core.Player.HallOfFameData &= newData
                Case "setot"
                    Dim Index As Integer = int(argument.GetSplit(0, ","))
                    Dim OT As String = argument.GetSplit(1, ",")

                    If Core.Player.Pokemons.Count - 1 >= Index Then
                        Core.Player.Pokemons(Index).OT = OT
                    End If
                Case "setitem"
                    Dim Index As Integer = int(argument.GetSplit(0, ","))
                    Dim newItem As Item = Item.GetItemByID(int(argument.GetSplit(1, ",")))

                    If Core.Player.Pokemons.Count - 1 >= Index Then
                        Core.Player.Pokemons(Index).Item = newItem
                    End If
                Case "removeitem"
                    Dim Index As Integer = int(argument.GetSplit(0, ","))

                    Core.Player.Pokemons(Index).Item = Nothing
                Case "setitemdata"
                    Dim Index As Integer = int(argument.GetSplit(0, ","))
                    Dim itemData As String = argument.GetSplit(1, ",")

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
                    Dim catchBall As Integer = int(argument.GetSplit(1, ","))

                    If Core.Player.Pokemons.Count - 1 >= Index Then
                        Core.Player.Pokemons(Index).CatchBall = Item.GetItemByID(catchBall)
                    End If
                Case "setcatchmethod"
                    Dim Index As Integer = int(argument.GetSplit(0, ","))
                    Dim method As String = argument.GetSplit(1, ",")

                    If Core.Player.Pokemons.Count - 1 >= Index Then
                        Core.Player.Pokemons(Index).CatchMethod = method
                    End If
                Case "setcatchplace", "setcatchlocation"
                    Dim Index As Integer = int(argument.GetSplit(0, ","))
                    Dim place As String = argument.GetSplit(1, ",")

                    If Core.Player.Pokemons.Count - 1 >= Index Then
                        Core.Player.Pokemons(Index).CatchLocation = place
                    End If
                Case "newroaming"
                    'PokémonID,Level,regionID,startLevelFile,MusicLoop
                    Dim data() As String = argument.Split(CChar(","))
                    Dim p As Pokemon = Pokemon.GetPokemonByID(CInt(data(0)))
                    p.Generate(CInt(data(1)), True)

                    If Core.Player.RoamingPokemonData <> "" Then
                        Core.Player.RoamingPokemonData &= vbNewLine
                    End If

                    Core.Player.RoamingPokemonData &= data(0) & "|" & data(1) & "|" & data(2) & "|" & data(3) & "|" & data(4) & "|" & p.GetSaveData()
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
                Case "reload"
                    Dim PokemonIndex As Integer = int(argument)
                    If Core.Player.Pokemons.Count - 1 >= PokemonIndex Then
                        Core.Player.Pokemons(PokemonIndex).ReloadDefinitions()
                    End If
                Case "clone"
                    Dim PokemonIndex As Integer = int(argument)

                    If Core.Player.Pokemons.Count - 1 >= PokemonIndex And Core.Player.Pokemons.Count < 6 Then
                        Core.Player.Pokemons.Add(Core.Player.Pokemons(PokemonIndex))
                    End If
            End Select

            IsReady = True
        End Sub

    End Class

End Namespace