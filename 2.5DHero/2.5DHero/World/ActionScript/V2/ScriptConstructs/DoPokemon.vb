Namespace ScriptVersion2

    Partial Class ScriptComparer

        '--------------------------------------------------------------------------------------------------------------------------
        'Contains the <pokemon> constructs.
        '--------------------------------------------------------------------------------------------------------------------------

        Private Shared Function DoPokemon(ByVal subClass As String) As Object
            Dim command As String = GetSubClassArgumentPair(subClass).Command
            Dim argument As String = GetSubClassArgumentPair(subClass).Argument

            Select Case command.ToLower()
                Case "id", "number"
                    Dim index As Integer = int(argument.GetSplit(0))
                    Return Core.Player.Pokemons(index).Number
                Case "data"
                    Dim index As Integer = int(argument.GetSplit(0))
                    Return Core.Player.Pokemons(index).GetSaveData().Replace(",", "§")
                Case "level"
                    Dim index As Integer = int(argument.GetSplit(0))
                    Return Core.Player.Pokemons(index).Level
                Case "hasfullhp"
                    Dim index As Integer = int(argument.GetSplit(0))
                    If Core.Player.Pokemons(index).HP = Core.Player.Pokemons(index).MaxHP Then
                        Return ReturnBoolean(True)
                    Else
                        Return ReturnBoolean(False)
                    End If
                Case "hp"
                    Dim index As Integer = int(argument.GetSplit(0))
                    Return Core.Player.Pokemons(index).HP
                Case "atk"
                    Dim index As Integer = int(argument.GetSplit(0))
                    Return Core.Player.Pokemons(index).Attack
                Case "def"
                    Dim index As Integer = int(argument.GetSplit(0))
                    Return Core.Player.Pokemons(index).Defense
                Case "spatk"
                    Dim index As Integer = int(argument.GetSplit(0))
                    Return Core.Player.Pokemons(index).SpAttack
                Case "spdef"
                    Dim index As Integer = int(argument.GetSplit(0))
                    Return Core.Player.Pokemons(index).SpDefense
                Case "speed"
                    Dim index As Integer = int(argument.GetSplit(0))
                    Return Core.Player.Pokemons(index).Speed
                Case "maxhp"
                    Dim index As Integer = int(argument.GetSplit(0))
                    Return Core.Player.Pokemons(index).MaxHP
                Case "isegg"
                    Dim index As Integer = int(argument.GetSplit(0))
                    Return ReturnBoolean(Core.Player.Pokemons(index).IsEgg())
                Case "additionaldata"
                    Dim index As Integer = int(argument.GetSplit(0))
                    Return Core.Player.Pokemons(index).AdditionalData
                Case "nickname"
                    Dim index As Integer = int(argument.GetSplit(0))
                    Return Core.Player.Pokemons(index).NickName
                Case "hasnickname"
                    Dim index As Integer = int(argument.GetSplit(0))
                    Return (Core.Player.Pokemons(index).NickName = "")
                Case "name"
                    Dim index As Integer = int(argument.GetSplit(0))
                    Return Core.Player.Pokemons(index).OriginalName
                Case "ot"
                    Dim index As Integer = int(argument.GetSplit(0))
                    Return Core.Player.Pokemons(index).OT
                Case "trainer"
                    Dim index As Integer = int(argument.GetSplit(0))
                    Return Core.Player.Pokemons(index).CatchTrainerName
                Case "itemid"
                    Dim index As Integer = int(argument.GetSplit(0))
                    If Core.Player.Pokemons(index).Item Is Nothing Then
                        Return 0
                    Else
                        Return Core.Player.Pokemons(index).Item.ID
                    End If
                Case "friendship"
                    Dim index As Integer = int(argument.GetSplit(0))

                    Return Core.Player.Pokemons(index).Friendship.ToString()
                Case "itemname", "item"
                    Dim index As Integer = int(argument.GetSplit(0))
                    If Core.Player.Pokemons(index).Item Is Nothing Then
                        Return ""
                    Else
                        Return Core.Player.Pokemons(index).Item.Name
                    End If
                Case "catchball"
                    Dim index As Integer = int(argument.GetSplit(0))
                    Return Core.Player.Pokemons(index).CatchBall.ID
                Case "catchmethod"
                    Dim index As Integer = int(argument.GetSplit(0))
                    Return Core.Player.Pokemons(index).CatchMethod
                Case "catchlocation"
                    Dim index As Integer = int(argument.GetSplit(0))
                    Return Core.Player.Pokemons(index).CatchLocation
                Case "hasattack"
                    Dim index As Integer = int(argument.GetSplit(0))
                    Dim attackID As Integer = int(argument.GetSplit(1))

                    Dim has As Boolean = False

                    For Each a As BattleSystem.Attack In Core.Player.Pokemons(index).Attacks
                        If a.ID = attackID Then
                            has = True
                            Exit For
                        End If
                    Next

                    Return ReturnBoolean(has)
                Case "countattacks"
                    Dim index As Integer = int(argument.GetSplit(0))

                    Return Core.Player.Pokemons(index).Attacks.Count
                Case "attackname"
                    Dim pokeIndex As Integer = int(argument.GetSplit(0))
                    Dim moveIndex As Integer = int(argument.GetSplit(1))

                    Return Core.Player.Pokemons(pokeIndex).Attacks(moveIndex).Name
                Case "isshiny"
                    Dim index As Integer = int(argument.GetSplit(0))

                    Return ReturnBoolean(Core.Player.Pokemons(index).IsShiny)
                Case "nature"
                    Dim index As Integer = int(argument.GetSplit(0))

                    Return Core.Player.Pokemons(index).Nature.ToString()
                Case "ownpokemon"
                    Dim index As Integer = int(argument.GetSplit(0))

                    If Core.Player.Pokemons(index).OT = Core.Player.OT Then
                        Return ReturnBoolean(True)
                    Else
                        Return ReturnBoolean(False)
                    End If
                Case "islegendary"
                    Dim index As Integer = int(argument.GetSplit(0))

                    Return ReturnBoolean(Pokemon.Legendaries.Contains(Core.Player.Pokemons(index).Number))
                Case "freeplaceinparty"
                    Return ReturnBoolean((Core.Player.Pokemons.Count < 6))
                Case "nopokemon"
                    Return ReturnBoolean((Core.Player.Pokemons.Count = 0))
                Case "count"
                    Return Core.Player.Pokemons.Count
                Case "countbattle"
                    Dim c As Integer = 0
                    For Each p As Pokemon In Core.Player.Pokemons
                        If p.IsEgg() = False And p.HP > 0 And p.Status <> Pokemon.StatusProblems.Fainted Then
                            c += 1
                        End If
                    Next
                    Return c
                Case "has"
                    Dim has As Boolean = False
                    Dim PokemonID As Integer = int(argument.GetSplit(0))

                    For Each p As Pokemon In Core.Player.Pokemons
                        If p.Number = PokemonID Then
                            has = True
                            Exit For
                        End If
                    Next

                    Return ReturnBoolean(has)
                Case "selected"
                    Return ChoosePokemonScreen.Selected
                Case "selectedmove"
                    Return ChooseAttackScreen.Selected
                Case "hasegg"
                    Dim hasEgg As Boolean = False
                    For Each p As Pokemon In Core.Player.Pokemons
                        If p.IsEgg = True Then
                            hasEgg = True
                            Exit For
                        End If
                    Next
                    Return ReturnBoolean(hasEgg)
                Case "maxpartylevel"
                    Dim maxLevel As Integer = 0
                    For Each p As Pokemon In Core.Player.Pokemons
                        If maxLevel < p.Level Then
                            maxLevel = p.Level
                        End If
                    Next
                    Return maxLevel
                Case "evhp"
                    Dim index As Integer = int(argument.GetSplit(0))
                    Return Core.Player.Pokemons(index).EVHP
                Case "evatk"
                    Dim index As Integer = int(argument.GetSplit(0))
                    Return Core.Player.Pokemons(index).EVAttack
                Case "evdef"
                    Dim index As Integer = int(argument.GetSplit(0))
                    Return Core.Player.Pokemons(index).EVDefense
                Case "evspatk"
                    Dim index As Integer = int(argument.GetSplit(0))
                    Return Core.Player.Pokemons(index).EVSpAttack
                Case "evspdef"
                    Dim index As Integer = int(argument.GetSplit(0))
                    Return Core.Player.Pokemons(index).EVSpDefense
                Case "evspeed"
                    Dim index As Integer = int(argument.GetSplit(0))
                    Return Core.Player.Pokemons(index).EVSpeed
                Case "ivhp"
                    Dim index As Integer = int(argument.GetSplit(0))
                    Return Core.Player.Pokemons(index).IVHP
                Case "ivatk"
                    Dim index As Integer = int(argument.GetSplit(0))
                    Return Core.Player.Pokemons(index).IVAttack
                Case "ivdef"
                    Dim index As Integer = int(argument.GetSplit(0))
                    Return Core.Player.Pokemons(index).IVDefense
                Case "ivspatk"
                    Dim index As Integer = int(argument.GetSplit(0))
                    Return Core.Player.Pokemons(index).IVSpAttack
                Case "ivspdef"
                    Dim index As Integer = int(argument.GetSplit(0))
                    Return Core.Player.Pokemons(index).IVSpDefense
                Case "ivspeed"
                    Dim index As Integer = int(argument.GetSplit(0))
                    Return Core.Player.Pokemons(index).IVSpeed
                Case "itemdata"
                    Dim data As String = ""
                    Dim index As Integer = int(argument)
                    If Not Core.Player.Pokemons(index).Item Is Nothing Then
                        data = Core.Player.Pokemons(index).Item.AdditionalData
                    End If
                    Return data
                Case "counthalloffame"
                    Return HallOfFameScreen.GetHallOfFameCount()
                Case "learnedtutormove"
                    Return ReturnBoolean(TeachMovesScreen.LearnedMove)
                Case "totalexp"
                    Dim index As Integer = int(argument.GetSplit(0))
                    Return Core.Player.Pokemons(index).Experience
                Case "needexp"
                    Dim index As Integer = int(argument.GetSplit(0))
                    Dim p As Pokemon = Core.Player.Pokemons(index)
                    Return p.NeedExperience(p.Level + 1) - p.Experience
                Case "currentexp"
                    Dim index As Integer = int(argument.GetSplit(0))
                    Dim p As Pokemon = Core.Player.Pokemons(index)
                    Return p.Experience - p.NeedExperience(p.Level)
                Case "generatefrontier"
                    Dim level As Integer = int(argument.GetSplit(0))
                    level = level.Clamp(1, CInt(GameModeManager.GetGameRuleValue("MaxLevel", "100")))
                    Dim pokemon_class As Integer = int(argument.GetSplit(1))
                    Dim IDPreset As List(Of Integer) = Nothing

                    If argument.CountSeperators(",") > 1 Then
                        For i = 2 To argument.CountSeperators(",")
                            Dim s As String = argument.GetSplit(i)
                            If s.Contains("-") = True Then
                                Dim min As Integer = int(s.Remove(s.IndexOf("-")))
                                Dim max As Integer = int(s.Remove(0, s.IndexOf("-") + 1))

                                For c = min To max
                                    If IDPreset Is Nothing Then
                                        IDPreset = {c}.ToList()
                                    Else
                                        IDPreset.Add(c)
                                    End If
                                Next
                            Else
                                If IDPreset Is Nothing Then
                                    IDPreset = {int(argument.GetSplit(i))}.ToList()
                                Else
                                    IDPreset.Add(int(argument.GetSplit(i)))
                                End If
                            End If
                        Next
                    End If

                    Return FrontierSpawner.GetPokemon(level, pokemon_class, IDPreset).GetSaveData().Replace(",", "§")
                Case "spawnwild"
                    Return Spawner.GetPokemon(Screen.Level.LevelFile, CType(int(argument), Spawner.EncounterMethods)).GetSaveData().Replace(",", "§")
                Case "spawn"
                    Dim ID As Integer = int(argument.GetSplit(0))
                    Dim level As Integer = int(argument.GetSplit(1))

                    Dim p As Pokemon = Pokemon.GetPokemonByID(ID)
                    p.Generate(level, True)
                    Return p.GetSaveData().Replace(",", "§")
                Case "otmatch"
                    'arguments: has: returns boolean, ID: returns pokedex number, Name: returns name, maxhits: returns the max number of equal chars

                    Dim maxDigits As Integer = 0
                    Dim maxName As String = "[EMPTY]"
                    Dim maxID As Integer = 0

                    Dim checkOT As String = argument.GetSplit(0)
                    While checkOT.Length < 5
                        checkOT = "0" & checkOT
                    End While

                    Dim d As String = checkOT
                    Dim checkDigits() = {d(0), d(1), d(2), d(3), d(4)}

                    Dim ps As List(Of Pokemon) = StorageSystemScreen.GetAllBoxPokemon()
                    For Each p As Pokemon In Core.Player.Pokemons
                        ps.Add(p)
                    Next

                    ps = ps.ToArray().Randomize().ToList()

                    For Each p As Pokemon In ps
                        Dim currentCount As Integer = 0
                        Dim pOT As String = p.OT
                        While pOT.Length < 5
                            pOT = "0" & pOT
                        End While

                        Dim pDigits() As String = {pOT(0), pOT(1), pOT(2), pOT(3), pOT(4)}

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

                    Dim arg As String = argument.Split(CChar(","))(1)

                    Select Case arg.ToLower()
                        Case "has"
                            If maxDigits > 0 Then
                                Return ReturnBoolean(True)
                            Else
                                Return ReturnBoolean(False)
                            End If
                        Case "id", "number"
                            Return maxID
                        Case "name"
                            Return maxName
                        Case "maxhits"
                            Return maxDigits
                    End Select

                    Return "INVALID ARGUMENT"
                Case "randomot"
                    Dim n As String = Core.Random.Next(0, 100000).ToString()
                    While n.Length < 5
                        n = "0" & n
                    End While
                    Return n
                Case "status"
                    Dim index As Integer = int(argument)

                    Return Core.Player.Pokemons(index).Status.ToString()
                Case "canevolve"
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

                    Return ReturnBoolean(p.CanEvolve(trigger, evolutionArg))
                Case "type1"
                    Dim p As Pokemon = Core.Player.Pokemons(int(argument))
                    Return p.Type1.ToString()
                Case "type2"
                    Dim p As Pokemon = Core.Player.Pokemons(int(argument))
                    Return p.Type2.ToString()
                Case "istype"
                    Dim args() As String = argument.Split(CChar(","))

                    Dim p As Pokemon = Core.Player.Pokemons(int(args(0)))
                    Dim checkType As String = args(1)

                    Return ReturnBoolean(p.IsType(New Element(checkType).Type))
                Case "displayname"
                    Dim index As Integer = int(argument.GetSplit(0))
                    Return Core.Player.Pokemons(index).GetDisplayName()
                Case "menusprite"
                    Dim index As Integer = int(argument.GetSplit(0))

                    Dim p As Pokemon = Core.Player.Pokemons(index)

                    Dim pos As Vector2 = PokemonForms.GetMenuImagePosition(p)
                    Dim size As Size = PokemonForms.GetMenuImageSize(p)

                    Dim sheet As String = "GUI\PokemonMenu"
                    If p.IsShiny = True Then
                        sheet = "GUI\PokemonMenuShiny"
                    End If

                    Return sheet & "|" & CStr(pos.X * 32) & "|" & CStr(pos.Y * 32) & "|" & CStr(size.Width) & "|" & CStr(size.Height)
            End Select
            Return DEFAULTNULL
        End Function

    End Class

End Namespace