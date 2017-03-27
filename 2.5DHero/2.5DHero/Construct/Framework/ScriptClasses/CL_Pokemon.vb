Namespace Construct.Framework.Classes

    <ScriptClass("Pokemon")>
    <ScriptDescription("A class to perform several Pokémon and party related operations.")>
    Public Class CL_Pokemon

        Inherits ScriptClass

#Region "Commands"

#Region "PartyManagement"

        <ScriptCommand("Remove")>
        <ScriptDescription("Removes a Pokémon from the party.")>
        Private Function M_Remove(ByVal argument As String) As String
            Dim index As Integer = Int(argument)
            If Game.Core.Player.Pokemons.Count - 1 >= index Then
                Game.Core.Player.Pokemons.RemoveAt(index)
            End If

            Return Core.Null
        End Function

        <ScriptCommand("Add")>
        <ScriptDescription("Adds a Pokémon to the party (either by full data or description).")>
        Private Function M_Add(ByVal argument As String) As String
            'PokemonID,Level,Method,Ball,Location,IsEgg,TrainerName

            If argument.StartsWith("{") = True Or argument.Remove(0, 1).StartsWith(",{") = True Then
                Dim insertIndex As Integer = Game.Core.Player.Pokemons.Count
                If argument.Remove(0, 1).StartsWith(",{") = True Then
                    insertIndex = Int(argument.GetSplit(0))
                End If

                argument = argument.Remove(0, argument.IndexOf("{"))

                Dim p As Pokemon = Pokemon.GetPokemonByData(argument.Replace("§", ","))
                Game.Core.Player.Pokemons.Insert(insertIndex, p)

                Dim pokedexType As Integer = 2
                If p.IsShiny = True Then
                    pokedexType = 3
                End If

                If p.IsEgg() = False Then
                    Game.Core.Player.PokedexData = Pokedex.ChangeEntry(Game.Core.Player.PokedexData, p.Number, pokedexType)
                End If
            Else
                Dim commas As Integer = 0
                For Each c As Char In argument
                    If c = "," Then
                        commas += 1
                    End If
                Next

                Dim PokemonID As Integer = Int(argument.GetSplit(0))
                Dim Level As Integer = Int(argument.GetSplit(1))

                Dim catchMethod As String = "random reason"
                If commas > 1 Then
                    catchMethod = argument.GetSplit(2)
                End If

                Dim catchBall As Item = Game.Item.GetItemByID(1)
                If commas > 2 Then
                    catchBall = Game.Item.GetItemByID(Int(argument.GetSplit(3)))
                End If

                Dim catchLocation As String = Screen.Level.MapName
                If commas > 3 Then
                    catchLocation = argument.GetSplit(4)
                End If

                Dim isEgg As Boolean = False
                If commas > 4 Then
                    isEgg = Bool(argument.GetSplit(5))
                End If

                Dim catchTrainer As String = Game.Core.Player.Name
                If commas > 5 And argument.GetSplit(6) <> "<playername>" Then
                    catchTrainer = argument.GetSplit(6)
                End If

                Dim Pokemon As Pokemon = Pokemon.GetPokemonByID(PokemonID)
                Pokemon.Generate(Level, True)

                Pokemon.CatchTrainerName = catchTrainer
                Pokemon.OT = Game.Core.Player.OT

                Pokemon.CatchLocation = catchLocation
                Pokemon.CatchBall = catchBall
                Pokemon.CatchMethod = catchMethod

                If isEgg = True Then
                    Pokemon.EggSteps = 1
                    Pokemon.SetCatchInfos(Game.Item.GetItemByID(5), "obtained at")
                Else
                    Pokemon.EggSteps = 0
                End If

                Game.Core.Player.Pokemons.Add(Pokemon)

                Dim pokedexType As Integer = 2
                If Pokemon.IsShiny = True Then
                    pokedexType = 3
                End If

                If Pokemon.IsEgg() = False Then
                    Game.Core.Player.PokedexData = Pokedex.ChangeEntry(Game.Core.Player.PokedexData, Pokemon.Number, pokedexType)
                End If
            End If
            Return Core.Null
        End Function

        <ScriptCommand("SetAdditionalValue")>
        <ScriptDescription("Sets the additional value of a Pokémon in the party.")>
        Private Function M_SetAdditionalValue(ByVal argument As String) As String
            Dim Index As Integer = Int(argument.GetSplit(0, ","))
            Dim AdditionalValue As String = argument.GetSplit(1, ",")

            If Game.Core.Player.Pokemons.Count - 1 >= Index Then
                Game.Core.Player.Pokemons(Index).AdditionalData = AdditionalValue
            End If

            Return Core.Null
        End Function

        <ScriptCommand("SetNickname")>
        <ScriptDescription("Sets the nickname of a Pokémon.")>
        Private Function M_SetNickname(ByVal argument As String) As String
            Dim Index As Integer = Int(argument.GetSplit(0, ","))
            Dim NickName As String = argument.GetSplit(1, ",")

            If Game.Core.Player.Pokemons.Count - 1 >= Index Then
                Game.Core.Player.Pokemons(Index).NickName = NickName
            End If

            Return Core.Null
        End Function

        <ScriptCommand("SetStat")>
        <ScriptDescription("Sets a stat of a Pokémon in the party.")>
        Private Function M_SetStat(ByVal argument As String) As String
            Dim Index As Integer = Int(argument.GetSplit(0, ","))
            Dim stat As String = argument.GetSplit(1, ",")
            Dim statValue As Integer = Int(argument.GetSplit(2, ","))

            If Game.Core.Player.Pokemons.Count - 1 >= Index Then
                With Game.Core.Player.Pokemons(Index)
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

            Return Core.Null
        End Function

        <ScriptCommand("Clear")>
        <ScriptDescription("Clears the party.")>
        Private Function M_Clear(ByVal argument As String) As String
            Game.Core.Player.Pokemons.Clear()

            Return Core.Null
        End Function

        <ScriptCommand("RemoveAttack")>
        <ScriptDescription("Removes an attack from a pokémon in the party.")>
        Private Function M_RemoveAttack(ByVal argument As String) As String
            Dim Index As Integer = Int(argument.GetSplit(0, ","))
            Dim attackIndex As Integer = Int(argument.GetSplit(1, ","))

            If Game.Core.Player.Pokemons.Count - 1 >= Index Then
                Dim p As Pokemon = Game.Core.Player.Pokemons(Index)

                If p.Attacks.Count - 1 >= attackIndex Then
                    p.Attacks.RemoveAt(attackIndex)
                End If
            End If

            Return Core.Null
        End Function

        <ScriptCommand("ClearAttacks")>
        <ScriptDescription("Clears all attacks from a Pokémon in the party.")>
        Private Function M_ClearAttacks(ByVal argument As String) As String
            Dim Index As Integer = Int(argument)

            If Game.Core.Player.Pokemons.Count - 1 >= Index Then
                Game.Core.Player.Pokemons(Index).Attacks.Clear()
            End If

            Return Core.Null
        End Function

        <ScriptCommand("AddAttack")>
        <ScriptDescription("Adds an attack to a Pokémon in the party.")>
        Private Function M_AddAttack(ByVal argument As String) As String
            Dim Index As Integer = Int(argument.GetSplit(0, ","))
            Dim attackID As Integer = Int(argument.GetSplit(1, ","))

            If Game.Core.Player.Pokemons.Count - 1 >= Index Then
                Dim p As Pokemon = Game.Core.Player.Pokemons(Index)

                If p.Attacks.Count < 4 Then
                    Dim newAttack As BattleSystem.Attack = BattleSystem.Attack.GetAttackByID(attackID)
                    p.Attacks.Add(newAttack)
                End If
            End If

            Return Core.Null
        End Function

        <ScriptCommand("SetShiny")>
        <ScriptDescription("Sets the shiny status of a Pokémon in the party.")>
        Private Function M_SetShiny(ByVal argument As String) As String
            Dim Index As Integer = Int(argument.GetSplit(0, ","))
            Dim isShiny As Boolean = Bool(argument.GetSplit(1, ","))

            If Game.Core.Player.Pokemons.Count - 1 >= Index Then
                Game.Core.Player.Pokemons(Index).IsShiny = isShiny
            End If

            Return Core.Null
        End Function

        <ScriptCommand("ChangeLevel")>
        <ScriptDescription("Changes the level of a Pokémon in the party.")>
        Private Function M_ChangeLevel(ByVal argument As String) As String
            Dim Index As Integer = Int(argument.GetSplit(0, ","))
            Dim newLevel As Integer = Int(argument.GetSplit(1, ","))

            If Game.Core.Player.Pokemons.Count - 1 >= Index Then
                Game.Core.Player.Pokemons(Index).Level = newLevel
            End If

            Return Core.Null
        End Function

        <ScriptCommand("GainEXP")>
        <ScriptDescription("Adds EXP to a Pokémon in the party.")>
        Private Function M_GainEXP(ByVal argument As String) As String
            Dim Index As Integer = Int(argument.GetSplit(0, ","))
            Dim exp As Integer = Int(argument.GetSplit(1, ","))

            If Game.Core.Player.Pokemons.Count - 1 >= Index Then
                Game.Core.Player.Pokemons(Index).Experience += exp
            End If

            Return Core.Null
        End Function

        <ScriptCommand("SetNature")>
        <ScriptDescription("Sets the nature of a Pokémon in the party.")>
        Private Function M_SetNature(ByVal argument As String) As String
            Dim Index As Integer = Int(argument.GetSplit(0, ","))
            Dim Nature As Pokemon.Natures = Pokemon.Natures.Adamant
            Dim natureStr = argument.GetSplit(1, ",")

            If Converter.IsNumeric(natureStr) = True Then
                Nature = Pokemon.ConvertIDToNature(Int(natureStr))
            Else
                Nature = CType([Enum].Parse(GetType(Pokemon.Natures), natureStr), Pokemon.Natures)
            End If

            If Game.Core.Player.Pokemons.Count - 1 >= Index Then
                Game.Core.Player.Pokemons(Index).Nature = Nature
            End If

            Return Core.Null
        End Function

        <ScriptCommand("Heal")>
        <ScriptDescription("Heals Pokémon from the party.")>
        Private Function M_Heal(ByVal argument As String) As String
            If argument = "" Then
                Game.Core.Player.HealParty()
            Else
                If argument.Contains(",") = True Then
                    Dim data() As String = argument.Split(CChar(","))
                    Dim Members As New List(Of Integer)
                    For Each member As String In data
                        Members.Add(Int(member))
                    Next
                    Game.Core.Player.HealParty(Members.ToArray())
                Else
                    Game.Core.Player.HealParty({Int(argument)})
                End If
            End If

            Return Core.Null
        End Function

        <ScriptCommand("SetFriendship")>
        <ScriptDescription("Sets the friendship value of a Pokémon.")>
        Private Function M_SetFriendShip(ByVal argument As String) As String
            Dim index As Integer = Int(argument.GetSplit(0))
            Dim amount As Integer = Int(argument.GetSplit(1))

            Game.Core.Player.Pokemons(index).Friendship = amount

            Return Core.Null
        End Function

        <ScriptCommand("CalcStats")>
        <ScriptDescription("Recalculates the stats of a Pokémon. Needs to be called after changing IV/EV/Level values.")>
        Private Function M_CalcStats(ByVal argument As String) As String
            Dim index As Integer = Int(argument)
            Game.Core.Player.Pokemons(index).CalculateStats()

            Return Core.Null
        End Function

        <ScriptCommand("LearnAttack")>
        <ScriptDescription("Opens the ""Learn an attack"" screen, when the Pokémon has 4 moves already, otherwise adds the attack.")>
        Private Function M_LearnAttack(ByVal argument As String) As String
            Dim index As Integer = Int(argument.GetSplit(0))
            Dim attackID As Integer = Int(argument.GetSplit(1))

            Game.Core.SetScreen(New LearnAttackScreen(Game.Core.CurrentScreen, Game.Core.Player.Pokemons(index), BattleSystem.Attack.GetAttackByID(attackID)))

            ActiveLine.EndExecutionFrame = True

            Return Core.Null
        End Function

        <ScriptCommand("SetGender")>
        <ScriptDescription("Sets the gender of a Pokémon in the party.")>
        Private Function M_SetGender(ByVal argument As String) As String
            Dim Index As Integer = Int(argument.GetSplit(0, ","))
            Dim Gender As Integer = Int(argument.GetSplit(1, ","))

            If Game.Core.Player.Pokemons.Count - 1 >= Index And Gender >= 0 And Gender <= 2 Then
                Game.Core.Player.Pokemons(Index).Gender = CType(Gender, Pokemon.Genders)
            End If

            Return Core.Null
        End Function

        <ScriptCommand("SetAbility")>
        <ScriptDescription("Sets the ability of a Pokémon in the party.")>
        Private Function M_SetAbility(ByVal argument As String) As String
            Dim Index As Integer = Int(argument.GetSplit(0, ","))
            Dim abilityID As Integer = Int(argument.GetSplit(1, ","))

            If Game.Core.Player.Pokemons.Count - 1 >= Index Then
                Game.Core.Player.Pokemons(Index).Ability = Ability.GetAbilityByID(abilityID)
            End If

            Return Core.Null
        End Function

        <ScriptCommand("SetEV")>
        <ScriptDescription("Sets an EV value of a Pokémon in the party.")>
        Private Function M_SetEV(ByVal argument As String) As String
            Dim Index As Integer = Int(argument.GetSplit(0, ","))
            Dim ev As String = argument.GetSplit(1, ",")
            Dim evValue As Integer = Int(argument.GetSplit(2, ","))

            If Game.Core.Player.Pokemons.Count - 1 >= Index Then
                With Game.Core.Player.Pokemons(Index)
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

            Return Core.Null
        End Function

        <ScriptCommand("SetIV")>
        <ScriptDescription("Sets an IV value of a Pokémon in the party.")>
        Private Function M_SetIV(ByVal argument As String) As String
            Dim Index As Integer = Int(argument.GetSplit(0, ","))
            Dim dv As String = argument.GetSplit(1, ",")
            Dim dvValue As Integer = Int(argument.GetSplit(2, ","))

            If Game.Core.Player.Pokemons.Count - 1 >= Index Then
                With Game.Core.Player.Pokemons(Index)
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

            Return Core.Null
        End Function

        <ScriptCommand("SetOT")>
        <ScriptDescription("Sets the OT of a Pokémon in the party.")>
        Private Function M_SetOT(ByVal argument As String) As String
            Dim Index As Integer = Int(argument.GetSplit(0, ","))
            Dim OT As String = argument.GetSplit(1, ",")

            If Game.Core.Player.Pokemons.Count - 1 >= Index Then
                Game.Core.Player.Pokemons(Index).OT = OT
            End If

            Return Core.Null
        End Function

        <ScriptCommand("SetItem")>
        <ScriptDescription("Changes the item of a Pokémon in the party.")>
        Private Function M_SetItem(ByVal argument As String) As String
            Dim Index As Integer = Int(argument.GetSplit(0, ","))
            Dim newItem As Item = Game.Item.GetItemByID(Int(argument.GetSplit(1, ",")))

            If Game.Core.Player.Pokemons.Count - 1 >= Index Then
                Game.Core.Player.Pokemons(Index).Item = newItem
            End If

            Return Core.Null
        End Function

        <ScriptCommand("RemoveItem")>
        <ScriptDescription("Clears the item of a Pokémon in the party.")>
        Private Function M_RemoveItem(ByVal argument As String) As String
            Dim Index As Integer = Int(argument.GetSplit(0, ","))

            Game.Core.Player.Pokemons(Index).Item = Nothing

            Return Core.Null
        End Function

        <ScriptCommand("SetItemData")>
        <ScriptDescription("Sets the item data of the item of a Pokémon in the party.")>
        Private Function M_SetItemData(ByVal argument As String) As String
            Dim Index As Integer = Int(argument.GetSplit(0, ","))
            Dim itemData As String = argument.GetSplit(1, ",")

            If Game.Core.Player.Pokemons.Count - 1 >= Index Then
                If Not Game.Core.Player.Pokemons(Index).Item Is Nothing Then
                    Game.Core.Player.Pokemons(Index).Item.AdditionalData = itemData
                End If
            End If

            Return Core.Null
        End Function

        <ScriptCommand("SetCatchTrainer")>
        <ScriptDescription("Sets the catch trainer of a Pokémon in the party.")>
        Private Function M_SetCatchTrainer(ByVal argument As String) As String
            Dim Index As Integer = Int(argument.GetSplit(0, ","))
            Dim Trainer As String = argument.GetSplit(1, ",")

            If Game.Core.Player.Pokemons.Count - 1 >= Index Then
                Game.Core.Player.Pokemons(Index).CatchTrainerName = Trainer
            End If

            Return Core.Null
        End Function

        <ScriptCommand("SetCatchBall")>
        <ScriptDescription("Sets the catch ball of a Pokémon in the party.")>
        Private Function M_SetCatchBall(ByVal argument As String) As String
            Dim Index As Integer = Int(argument.GetSplit(0, ","))
            Dim catchBall As Integer = Int(argument.GetSplit(1, ","))

            If Game.Core.Player.Pokemons.Count - 1 >= Index Then
                Game.Core.Player.Pokemons(Index).CatchBall = Game.Item.GetItemByID(catchBall)
            End If

            Return Core.Null
        End Function

        <ScriptCommand("SetCatchMethod")>
        <ScriptDescription("Sets the catch method of a Pokémon in the party.")>
        Private Function M_SetCatchMethod(ByVal argument As String) As String
            Dim Index As Integer = Int(argument.GetSplit(0, ","))
            Dim method As String = argument.GetSplit(1, ",")

            If Game.Core.Player.Pokemons.Count - 1 >= Index Then
                Game.Core.Player.Pokemons(Index).CatchMethod = method
            End If

            Return Core.Null
        End Function

        <ScriptCommand("SetCatchLocation")>
        <ScriptDescription("Sets the catch location of a Pokémon in the party.")>
        Private Function M_SetCatchLocation(ByVal argument As String) As String
            Dim Index As Integer = Int(argument.GetSplit(0, ","))
            Dim place As String = argument.GetSplit(1, ",")

            If Game.Core.Player.Pokemons.Count - 1 >= Index Then
                Game.Core.Player.Pokemons(Index).CatchLocation = place
            End If

            Return Core.Null
        End Function

        <ScriptCommand("Evolve")>
        <ScriptDescription("Tries to forcefully evolve a Pokémon in the party.")>
        Private Function M_Evolve(ByVal argument As String) As String
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

            Dim p As Pokemon = Game.Core.Player.Pokemons(Int(args(0)))

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
                Game.Core.SetScreen(New TransitionScreen(Game.Core.CurrentScreen,
                                                             New EvolutionScreen(Game.Core.CurrentScreen, {Int(args(0))}.ToList(),
                                                                                 evolutionArg,
                                                                                 trigger,
                                                                                 False), Color.Black, False))

                ActiveLine.EndExecutionFrame = True
            Else
                Logger.Debug("065", "The Pokémon is not able to evolve with the given conditions.")
            End If

            Return Core.Null
        End Function

        <ScriptCommand("Reload")>
        <ScriptDescription("Reloads the definition data of a Pokémon in the party.")>
        Private Function M_Reload(ByVal argument As String) As String
            Dim PokemonIndex As Integer = Int(argument)
            If Game.Core.Player.Pokemons.Count - 1 >= PokemonIndex Then
                Game.Core.Player.Pokemons(PokemonIndex).ReloadDefinitions()
            End If

            Return Core.Null
        End Function

        <ScriptCommand("Clone")>
        <ScriptDescription("Clones a Pokémon in the party.")>
        Private Function M_Clone(ByVal argument As String) As String
            Dim PokemonIndex As Integer = Int(argument)

            If Game.Core.Player.Pokemons.Count - 1 >= PokemonIndex And Game.Core.Player.Pokemons.Count < 6 Then
                Game.Core.Player.Pokemons.Add(Game.Core.Player.Pokemons(PokemonIndex))
            End If

            Return Core.Null
        End Function

        <ScriptCommand("Rename")>
        <ScriptDescription("Opens the Rename screen for a Pokémon.")>
        Private Function M_Rename(ByVal argument As String) As String
            Game.Core.SetScreen(New NameObjectScreen(Game.Core.CurrentScreen, Game.Core.Player.Pokemons(Int(argument))))

            Return Core.Null
        End Function

#End Region

        <ScriptCommand("Cry")>
        <ScriptDescription("Plays the cry of a Pokémon.")>
        Private Function M_Cry(ByVal argument As String) As String
            Dim PokemonID As Integer = Int(argument)

            Dim p As Pokemon = Pokemon.GetPokemonByID(PokemonID)
            p.PlayCry()

            Return Core.Null
        End Function

        <ScriptCommand("NewRoaming")>
        <ScriptDescription("Generates a new roaming Pokémon and adds it to the roaming data.")>
        Private Function M_NewRoaming(ByVal argument As String) As String
            'PokémonID,Level,regionID,startLevelFile,MusicLoop

            'TODO: When the new Json format gets implemented, replace the region "ID" with the region name.

            Dim data() As String = argument.Split(CChar(","))
            Dim p As Pokemon = Pokemon.GetPokemonByID(CInt(data(0)))
            p.Generate(CInt(data(1)), True)

            If Game.Core.Player.RoamingPokemonData <> "" Then
                Game.Core.Player.RoamingPokemonData &= vbNewLine
            End If

            Game.Core.Player.RoamingPokemonData &= data(0) & "|" & data(1) & "|" & data(2) & "|" & data(3) & "|" & data(4) & "|" & p.GetSaveData()

            Return Core.Null
        End Function

        <ScriptCommand("Select")>
        <ScriptDescription("Opens the Pokémon selection screen.")>
        Private Function M_Select(ByVal argument As String) As String
            Dim canExit As Boolean = False
            Dim canChooseEgg As Boolean = True
            Dim canChooseFainted As Boolean = True

            If argument <> "" Then
                Dim data() As String = argument.Split(CChar(","))

                If data.Length > 0 Then
                    canExit = Bool(data(0))
                End If
                If data.Length > 1 Then
                    canChooseFainted = Bool(data(1))
                End If
                If data.Length > 2 Then
                    canChooseEgg = Bool(data(2))
                End If
            End If

            Game.Core.SetScreen(New ChoosePokemonScreen(Game.Core.CurrentScreen, New Items.Balls.Pokeball, Nothing, "Choose Pokémon", canExit, canChooseFainted, canChooseEgg))

            ActiveLine.EndExecutionFrame = True

            Return Core.Null
        End Function

        <ScriptCommand("SelectMove")>
        <ScriptDescription("Opens the move selection screen for a Pokémon in the party.")>
        Private Function M_SelectMove(ByVal argument As String) As String
            Dim index As Integer = 0
            Dim canHMMOve As Boolean = True
            Dim canExit As Boolean = False

            If argument.Contains(",") = True Then
                Dim args As List(Of String) = argument.Split(CChar(",")).ToList()

                For i = 0 To args.Count - 1
                    Dim arg As String = args(i)

                    Select Case i
                        Case 0
                            index = Int(arg)
                        Case 1
                            canHMMOve = Bool(arg)
                        Case 2
                            canExit = Bool(arg)
                    End Select
                Next
            Else
                index = Int(argument)
            End If

            Game.Core.SetScreen(New ChooseAttackScreen(Game.Core.CurrentScreen, Game.Core.Player.Pokemons(index), canHMMOve, canExit, Nothing))

            ActiveLine.EndExecutionFrame = True

            Return Core.Null
        End Function

#End Region

#Region "Constructs"

#Region "PartyManagement"

        <ScriptConstruct("Number")>
        <ScriptDescription("Returns the Pokédex number of a Pokémon in the party.")>
        Private Function F_Number(ByVal argument As String) As String
            Dim index As Integer = Int(argument.GetSplit(0))
            Return ToString(Game.Core.Player.Pokemons(index).Number)
        End Function

        <ScriptConstruct("Gender")>
        <ScriptDescription("Returns the gender of a Pokémon in the party.")>
        Private Function F_Gender(ByVal argument As String) As String
            Dim index As Integer = Int(argument.GetSplit(0))
            Return ToString(CInt(Game.Core.Player.Pokemons(index).Gender))
        End Function

        <ScriptConstruct("Data")>
        <ScriptDescription("The data of a Pokémon in the party.")>
        Private Function F_Data(ByVal argument As String) As String
            Dim index As Integer = Int(argument.GetSplit(0))
            Return Game.Core.Player.Pokemons(index).GetSaveData().Replace(",", "§")
        End Function

        <ScriptConstruct("Level")>
        <ScriptDescription("The level of a Pokémon in the party.")>
        Private Function F_Level(ByVal argument As String) As String
            Dim index As Integer = Int(argument.GetSplit(0))
            Return ToString(Game.Core.Player.Pokemons(index).Level)
        End Function

        <ScriptConstruct("GetStat")>
        <ScriptDescription("Returns a stat of a Pokémon in the party.")>
        Private Function F_GetStat(ByVal argument As String) As String
            Dim Index As Integer = Int(argument.GetSplit(0, ","))
            Dim stat As String = argument.GetSplit(1, ",")
            Dim returnStat As String = Core.Null

            If Game.Core.Player.Pokemons.Count - 1 >= Index Then
                With Game.Core.Player.Pokemons(Index)
                    Select Case stat.ToLower()
                        Case "maxhp", "hp"
                            returnStat = ToString(.MaxHP)
                        Case "chp"
                            returnStat = ToString(.HP)
                        Case "atk", "attack"
                            returnStat = ToString(.Attack)
                        Case "def", "defense"
                            returnStat = ToString(.Defense)
                        Case "spatk", "specialattack", "spattack"
                            returnStat = ToString(.SpAttack)
                        Case "spdef", "specialdefense", "spdefense"
                            returnStat = ToString(.SpDefense)
                        Case "speed"
                            returnStat = ToString(.Speed)
                    End Select
                End With
            End If

            Return returnStat
        End Function

        <ScriptConstruct("IsEgg")>
        <ScriptDescription("If a Pokémon in the party is an egg.")>
        Private Function F_IsEgg(ByVal argument As String) As String
            Dim index As Integer = Int(argument.GetSplit(0))
            Return ToString(Game.Core.Player.Pokemons(index).IsEgg())
        End Function

        <ScriptConstruct("AdditionalData")>
        <ScriptDescription("The additional data of a Pokémon in the party.")>
        Private Function F_AdditionalData(ByVal argument As String) As String
            Dim index As Integer = Int(argument.GetSplit(0))
            Return Game.Core.Player.Pokemons(index).AdditionalData
        End Function

        <ScriptConstruct("Nickname")>
        <ScriptDescription("The nickname of a Pokémon in the party.")>
        Private Function F_Nickname(ByVal argument As String) As String
            Dim index As Integer = Int(argument.GetSplit(0))
            Return Game.Core.Player.Pokemons(index).NickName
        End Function

        <ScriptConstruct("HasNickname")>
        <ScriptDescription("If a Pokémon in the party has a nickname.")>
        Private Function F_HasNickname(ByVal argument As String) As String
            Dim index As Integer = Int(argument.GetSplit(0))
            Return ToString(Game.Core.Player.Pokemons(index).NickName.Length > 0)
        End Function

        <ScriptConstruct("Name")>
        <ScriptDescription("The name of a Pokémon in the party.")>
        Private Function F_Name(ByVal argument As String) As String
            Dim index As Integer = Int(argument.GetSplit(0))
            Return Game.Core.Player.Pokemons(index).OriginalName
        End Function

        <ScriptConstruct("OT")>
        <ScriptDescription("The OT of a Pokémon in the party.")>
        Private Function F_OT(ByVal argument As String) As String
            Dim index As Integer = Int(argument.GetSplit(0))
            Return Game.Core.Player.Pokemons(index).OT
        End Function

        <ScriptConstruct("CatchTrainer")>
        <ScriptDescription("The catch trainer of a Pokémon in the party.")>
        Private Function F_CatchTrainer(ByVal argument As String) As String
            Dim index As Integer = Int(argument.GetSplit(0))
            Return Game.Core.Player.Pokemons(index).CatchTrainerName
        End Function

        <ScriptConstruct("CatchBall")>
        <ScriptDescription("The catch ball of a Pokémon in the party.")>
        Private Function F_CatchBall(ByVal argument As String) As String
            Dim index As Integer = Int(argument.GetSplit(0))
            Return ToString(Game.Core.Player.Pokemons(index).CatchBall.ID)
        End Function

        <ScriptConstruct("CatchMethod")>
        <ScriptDescription("The catch method of a Pokémon in the party.")>
        Private Function F_CatchMethod(ByVal argument As String) As String
            Dim index As Integer = Int(argument.GetSplit(0))
            Return Game.Core.Player.Pokemons(index).CatchMethod
        End Function

        <ScriptConstruct("CatchLocation")>
        <ScriptDescription("The catch location of a Pokémon in the party.")>
        Private Function F_CatchLocation(ByVal argument As String) As String
            Dim index As Integer = Int(argument.GetSplit(0))
            Return Game.Core.Player.Pokemons(index).CatchLocation
        End Function

        <ScriptConstruct("ItemID")>
        <ScriptDescription("The item id of the item of a Pokémon in the party.")>
        Private Function F_ItemID(ByVal argument As String) As String
            Dim index As Integer = Int(argument.GetSplit(0))
            If Game.Core.Player.Pokemons(index).Item Is Nothing Then
                Return Core.Null
            Else
                Return ToString(Game.Core.Player.Pokemons(index).Item.ID)
            End If
        End Function

        <ScriptConstruct("Friendship")>
        <ScriptDescription("The friendship value of a Pokémon in the party.")>
        Private Function F_Friendship(ByVal argument As String) As String
            Dim index As Integer = Int(argument.GetSplit(0))

            Return ToString(Game.Core.Player.Pokemons(index).Friendship)
        End Function

        <ScriptConstruct("ItemName")>
        <ScriptDescription("The item name of an item of a Pokémon in the party.")>
        Private Function F_ItemName(ByVal argument As String) As String
            Dim index As Integer = Int(argument.GetSplit(0))
            If Game.Core.Player.Pokemons(index).Item Is Nothing Then
                Return Core.Null
            Else
                Return Game.Core.Player.Pokemons(index).Item.Name
            End If
        End Function

        <ScriptConstruct("HasAttack")>
        <ScriptDescription("Returns if a Pokémon in the party knows a specific attack.")>
        Private Function F_HasAttack(ByVal argument As String) As String
            Dim index As Integer = Int(argument.GetSplit(0))
            Dim attackID As Integer = Int(argument.GetSplit(1))

            Dim has As Boolean = False

            For Each a As BattleSystem.Attack In Game.Core.Player.Pokemons(index).Attacks
                If a.ID = attackID Then
                    has = True
                    Exit For
                End If
            Next

            Return ToString(has)
        End Function

        <ScriptConstruct("CountAttacks")>
        <ScriptDescription("Returns the amount of moves a Pokémon in the party knows.")>
        Private Function F_CountAttacks(ByVal argument As String) As String
            Dim index As Integer = Int(argument.GetSplit(0))

            Return ToString(Game.Core.Player.Pokemons(index).Attacks.Count)
        End Function

        <ScriptConstruct("AttackName")>
        <ScriptDescription("The name of an attack of a Pokémon in the party.")>
        Private Function F_AttackName(ByVal argument As String) As String
            Dim pokeIndex As Integer = Int(argument.GetSplit(0))
            Dim moveIndex As Integer = Int(argument.GetSplit(1))

            Return Game.Core.Player.Pokemons(pokeIndex).Attacks(moveIndex).Name
        End Function

        <ScriptConstruct("IsShiny")>
        <ScriptDescription("The shiny value of a Pokémon in the party.")>
        Private Function F_IsShiny(ByVal argument As String) As String
            Dim index As Integer = Int(argument.GetSplit(0))

            Return ToString(Game.Core.Player.Pokemons(index).IsShiny)
        End Function

        <ScriptConstruct("Nature")>
        <ScriptDescription("The nature of a Pokémon in the party.")>
        Private Function F_Nature(ByVal argument As String) As String
            Dim index As Integer = Int(argument.GetSplit(0))

            Return Game.Core.Player.Pokemons(index).Nature.ToString()
        End Function

        <ScriptConstruct("IsLegendary")>
        <ScriptDescription("If a Pokémon in the party is legendary.")>
        Private Function F_IsLegendary(ByVal argument As String) As String
            Dim index As Integer = Int(argument.GetSplit(0))

            Return ToString(Pokemon.Legendaries.Contains(Game.Core.Player.Pokemons(index).Number))
        End Function

        <ScriptConstruct("Count")>
        <ScriptDescription("The amount of Pokémon in the party.")>
        Private Function F_Count(ByVal argument As String) As String
            Return ToString(Game.Core.Player.Pokemons.Count)
        End Function

        <ScriptConstruct("CountBattle")>
        <ScriptDescription("The amount of Pokémon in the party that are ready to battle.")>
        Private Function F_CountBattle(ByVal argument As String) As String
            Dim c As Integer = 0
            For Each p As Pokemon In Game.Core.Player.Pokemons
                If p.IsEgg() = False And p.HP > 0 And p.Status <> Pokemon.StatusProblems.Fainted Then
                    c += 1
                End If
            Next
            Return ToString(c)
        End Function

        <ScriptConstruct("Has")>
        <ScriptDescription("If there is a Pokémon with a specific Pokédex number in the party.")>
        Private Function F_Has(ByVal argument As String) As String
            Dim has As Boolean = False
            Dim PokemonID As Integer = Int(argument.GetSplit(0))

            For Each p As Pokemon In Game.Core.Player.Pokemons
                If p.Number = PokemonID Then
                    has = True
                    Exit For
                End If
            Next

            Return ToString(has)
        End Function

        <ScriptConstruct("HasEgg")>
        <ScriptDescription("If there is an egg in the party.")>
        Private Function F_HasEgg(ByVal argument As String) As String
            Dim hasEgg As Boolean = False
            For Each p As Pokemon In Game.Core.Player.Pokemons
                If p.IsEgg = True Then
                    hasEgg = True
                    Exit For
                End If
            Next
            Return ToString(hasEgg)
        End Function

        <ScriptConstruct("MaxPartyLevel")>
        <ScriptDescription("The max level of all Pokémon in the party.")>
        Private Function F_MaxPartyLevel(ByVal argument As String) As String
            Dim maxLevel As Integer = 0
            For Each p As Pokemon In Game.Core.Player.Pokemons
                If maxLevel < p.Level Then
                    maxLevel = p.Level
                End If
            Next
            Return ToString(maxLevel)
        End Function

        <ScriptConstruct("GetEV")>
        <ScriptDescription("Returns an EV value of a Pokémon in the party.")>
        Private Function F_GetEV(ByVal argument As String) As String
            Dim Index As Integer = Int(argument.GetSplit(0, ","))
            Dim stat As String = argument.GetSplit(1, ",")
            Dim returnStat As String = Core.Null

            If Game.Core.Player.Pokemons.Count - 1 >= Index Then
                With Game.Core.Player.Pokemons(Index)
                    Select Case stat.ToLower()
                        Case "hp"
                            returnStat = ToString(.EVHP)
                        Case "atk", "attack"
                            returnStat = ToString(.EVAttack)
                        Case "def", "defense"
                            returnStat = ToString(.EVDefense)
                        Case "spatk", "specialattack", "spattack"
                            returnStat = ToString(.EVSpAttack)
                        Case "spdef", "specialdefense", "spdefense"
                            returnStat = ToString(.EVSpDefense)
                        Case "speed"
                            returnStat = ToString(.EVSpeed)
                    End Select
                End With
            End If

            Return returnStat
        End Function

        <ScriptConstruct("GetIV")>
        <ScriptDescription("Returns an IV value of a Pokémon in the party.")>
        Private Function F_GetIV(ByVal argument As String) As String
            Dim Index As Integer = Int(argument.GetSplit(0, ","))
            Dim stat As String = argument.GetSplit(1, ",")
            Dim returnStat As String = Core.Null

            If Game.Core.Player.Pokemons.Count - 1 >= Index Then
                With Game.Core.Player.Pokemons(Index)
                    Select Case stat.ToLower()
                        Case "hp"
                            returnStat = ToString(.IVHP)
                        Case "atk", "attack"
                            returnStat = ToString(.IVAttack)
                        Case "def", "defense"
                            returnStat = ToString(.IVDefense)
                        Case "spatk", "specialattack", "spattack"
                            returnStat = ToString(.IVSpAttack)
                        Case "spdef", "specialdefense", "spdefense"
                            returnStat = ToString(.IVSpDefense)
                        Case "speed"
                            returnStat = ToString(.IVSpeed)
                    End Select
                End With
            End If

            Return returnStat
        End Function

        <ScriptConstruct("ItemData")>
        <ScriptDescription("Returns the item data of an item of a Pokémon in the party.")>
        Private Function F_ItemData(ByVal argument As String) As String
            Dim data As String = ""
            Dim index As Integer = Int(argument)
            If Not Game.Core.Player.Pokemons(index).Item Is Nothing Then
                data = Game.Core.Player.Pokemons(index).Item.AdditionalData
            End If
            Return data
        End Function

        <ScriptConstruct("TotalEXP")>
        <ScriptDescription("The total EXP of a Pokémon in the party.")>
        Private Function F_TotalEXP(ByVal argument As String) As String
            Dim index As Integer = Int(argument.GetSplit(0))
            Return ToString(Game.Core.Player.Pokemons(index).Experience)
        End Function

        <ScriptConstruct("NeedEXP")>
        <ScriptDescription("The needed EXP to level up of a Pokémon in the party.")>
        Private Function F_NeedEXP(ByVal argument As String) As String
            Dim index As Integer = Int(argument.GetSplit(0))
            Dim p As Pokemon = Game.Core.Player.Pokemons(index)
            Return ToString(p.NeedExperience(p.Level + 1) - p.Experience)
        End Function

        <ScriptConstruct("CurrentEXP")>
        <ScriptDescription("The current EXP of the current level of a Pokémon in the party.")>
        Private Function F_CurrentEXP(ByVal argument As String) As String
            Dim index As Integer = Int(argument.GetSplit(0))
            Dim p As Pokemon = Game.Core.Player.Pokemons(index)
            Return ToString(p.Experience - p.NeedExperience(p.Level))
        End Function

        <ScriptConstruct("Status")>
        <ScriptDescription("The status of a Pokémon in the party.")>
        Private Function F_Status(ByVal argument As String) As String
            Dim index As Integer = Int(argument)

            Return Game.Core.Player.Pokemons(index).Status.ToString()
        End Function

        <ScriptConstruct("CanEvolve")>
        <ScriptDescription("If a Pokémon can evolve right now, given certain conditions.")>
        Private Function F_CanEvolve(ByVal argument As String) As String
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

            Dim p As Pokemon = Game.Core.Player.Pokemons(Int(args(0)))

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

            Return ToString(p.CanEvolve(trigger, evolutionArg))
        End Function

        <ScriptConstruct("Type1")>
        <ScriptDescription("The primary type of a Pokémon in the party.")>
        Private Function F_Type1(ByVal argument As String) As String
            Dim p As Pokemon = Game.Core.Player.Pokemons(Int(argument))
            Return p.Type1.ToString()
        End Function

        <ScriptConstruct("Type2")>
        <ScriptDescription("The secondary type of a Pokémon in the party.")>
        Private Function F_Type2(ByVal argument As String) As String
            Dim p As Pokemon = Game.Core.Player.Pokemons(Int(argument))
            Return p.Type2.ToString()
        End Function

        <ScriptConstruct("IsType")>
        <ScriptDescription("If a Pokémon in the party is of a specific type.")>
        Private Function F_IsType(ByVal argument As String) As String
            Dim args() As String = argument.Split(CChar(","))

            Dim p As Pokemon = Game.Core.Player.Pokemons(Int(args(0)))
            Dim checkType As String = args(1)

            Return ToString(p.IsType(New Element(checkType).Type))
        End Function

        <ScriptConstruct("DisplayName")>
        <ScriptDescription("Returns the display name of a Pokémon in the party.")>
        Private Function F_DisplayName(ByVal argument As String) As String
            Dim index As Integer = Int(argument.GetSplit(0))
            Return Game.Core.Player.Pokemons(index).GetDisplayName()
        End Function

        <ScriptConstruct("MenuSprite")>
        <ScriptDescription("Returns the menu sprite information of a Pokémonn in the party.")>
        Private Function F_MenuSprite(ByVal argument As String) As String
            Dim index As Integer = Int(argument.GetSplit(0))

            Dim p As Pokemon = Game.Core.Player.Pokemons(index)

            Dim pos As Vector2 = PokemonForms.GetMenuImagePosition(p)
            Dim size As Size = PokemonForms.GetMenuImageSize(p)

            Dim sheet As String = "GUI\PokemonMenu"
            If p.IsShiny = True Then
                sheet = "GUI\PokemonMenuShiny"
            End If

            Return sheet & "|" & CStr(pos.X * 32) & "|" & CStr(pos.Y * 32) & "|" & CStr(size.Width) & "|" & CStr(size.Height)
        End Function

#End Region

        <ScriptConstruct("Selected")>
        <ScriptDescription("Returns the last selected Pokémon index.")>
        Private Function F_Selected(ByVal argument As String) As String
            Return ToString(ChoosePokemonScreen.Selected)
        End Function

        <ScriptConstruct("SelectedMove")>
        <ScriptDescription("Returns the last selected attack index.")>
        Private Function F_SelectedMove(ByVal argument As String) As String
            Return ToString(ChooseAttackScreen.Selected)
        End Function

        <ScriptConstruct("LearnedTutorMove")>
        <ScriptDescription("Returns if the Pokémon learned a tutor move in the last attempt.")>
        Private Function F_LearnedTutorMove(ByVal argument As String) As String
            Return ToString(TeachMovesScreen.LearnedMove)
        End Function

        <ScriptConstruct("GenerateFrontier")>
        <ScriptDescription("Returns a generated frontier Pokémon.")>
        Private Function F_GenerateFrontier(ByVal argument As String) As String
            Dim level As Integer = Int(argument.GetSplit(0))
            level = level.Clamp(1, CInt(OldGameModeManager.GetGameRuleValue("MaxLevel", "100")))
            Dim pokemon_class As Integer = Int(argument.GetSplit(1))
            Dim IDPreset As List(Of Integer) = Nothing

            If argument.CountSeperators(",") > 1 Then
                For i = 2 To argument.CountSeperators(",")
                    Dim s As String = argument.GetSplit(i)
                    If s.Contains("-") = True Then
                        Dim min As Integer = Int(s.Remove(s.IndexOf("-")))
                        Dim max As Integer = Int(s.Remove(0, s.IndexOf("-") + 1))

                        For c = min To max
                            If IDPreset Is Nothing Then
                                IDPreset = {c}.ToList()
                            Else
                                IDPreset.Add(c)
                            End If
                        Next
                    Else
                        If IDPreset Is Nothing Then
                            IDPreset = {Int(argument.GetSplit(i))}.ToList()
                        Else
                            IDPreset.Add(Int(argument.GetSplit(i)))
                        End If
                    End If
                Next
            End If

            Return FrontierSpawner.GetPokemon(level, pokemon_class, IDPreset).GetSaveData().Replace(",", "§")
        End Function

        <ScriptConstruct("SpawnWild")>
        <ScriptDescription("Generates a Pokémon from the current map.")>
        Private Function F_SpawnWild(ByVal argument As String) As String
            Return Spawner.GetPokemon(Screen.Level.LevelFile, CType(Int(argument), Spawner.EncounterMethods)).GetSaveData().Replace(",", "§")
        End Function

        <ScriptConstruct("Spawn")>
        <ScriptDescription("Generates a Pokémon based on ID and level.")>
        Private Function F_Spawn(ByVal argument As String) As String
            Dim ID As Integer = Int(argument.GetSplit(0))
            Dim level As Integer = Int(argument.GetSplit(1))

            Dim p As Pokemon = Pokemon.GetPokemonByID(ID)
            p.Generate(level, True)
            Return p.GetSaveData().Replace(",", "§")
        End Function

        <ScriptConstruct("OTMatch")>
        <ScriptDescription("If a passed in OT is represented in the player's box Pokémon.")>
        Private Function F_OTMatch(ByVal argument As String) As String
            'TODO: Speed up (do not load every single pokémon)
            'arguments: has: returns boolean, ID: returns pokedex number, Name: returns name, maxhits: returns the max number of equal chars

            Dim checkOT As String = argument.GetSplit(0)
            Dim returnMode As String = argument.GetSplit(1)

            Dim maxDigits As Integer = 0
            Dim maxName As String = "[EMPTY]"
            Dim maxID As Integer = 0

            While checkOT.Length < 5
                checkOT = "0" & checkOT
            End While

            Dim checkDigits() As Char = checkOT.ToCharArray()

            Dim ps As List(Of Pokemon) = StorageSystemScreen.GetAllBoxPokemon()
            For Each p As Pokemon In Game.Core.Player.Pokemons
                ps.Add(p)
            Next

            ps = ps.ToArray().Randomize().ToList()

            For Each p As Pokemon In ps
                Dim currentCount As Integer = 0
                Dim pOT As String = p.OT
                While pOT.Length < 5
                    pOT = "0" & pOT
                End While

                Dim pDigits() As Char = pOT.ToCharArray()

                For i = 4 To 0 Step -1
                    If pDigits(i) = checkDigits(i) Then
                        currentCount += 1
                    Else
                        Exit For
                    End If
                Next

                If currentCount > maxDigits Then
                    maxDigits = currentCount
                    maxName = p.GetDisplayName()
                    maxID = p.Number
                End If
            Next

            Select Case returnMode.ToLower()
                Case "has"
                    If maxDigits > 0 Then
                        Return ToString(True)
                    Else
                        Return ToString(False)
                    End If
                Case "id", "number"
                    Return ToString(maxID)
                Case "name"
                    Return maxName
                Case "maxhits"
                    Return ToString(maxDigits)
            End Select

            Return Core.Null
        End Function

#End Region

    End Class

End Namespace