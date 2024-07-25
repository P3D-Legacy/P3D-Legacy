Public Class Spawner

    ''' <summary>
    ''' Methods to encounter a wild Pokémon.
    ''' </summary>
    Public Enum EncounterMethods As Integer
        Land = 0
        Headbutt = 1
        Surfing = 2
        OldRod = 3
        GoodRod = 31
        SuperRod = 32
        [Event] = 4
        RockSmash = 5
    End Enum

    ''' <summary>
    ''' Spawns a Pokémon
    ''' </summary>
    ''' <param name="LevelFile">The levelfile that contains this Pokémon</param>
    ''' <param name="Method">The encounter method.</param>
    Public Shared Function GetPokemon(ByVal LevelFile As String, ByVal Method As EncounterMethods, Optional ByVal CanEncounterRoaming As Boolean = True, Optional ByVal InputPokeFile As String = "") As Pokemon
        If CanEncounterRoaming = True Then
            Dim roamingPokemon As RoamingPokemon = CheckForRoaming(LevelFile, Method)

            If Not roamingPokemon Is Nothing Then
                Logger.Debug("Roaming Pokemon (" & PokemonForms.GetPokemonDataFileName(roamingPokemon.PokemonReference.Number, roamingPokemon.PokemonReference.AdditionalData, True) & ") appears!")
                BattleSystem.BattleScreen.RoamingBattle = True
                BattleSystem.BattleScreen.RoamingPokemonStorage = roamingPokemon
                Return roamingPokemon.GetPokemon()
            End If
        End If

        Dim Pokemons As New List(Of String)
        Dim Chances As New List(Of Integer)
        Dim LevelCaps As New List(Of String)

        If InputPokeFile = "" Then
            pokeFile = LevelFile.Remove(LevelFile.Length - 4, 4) & ".poke"
        Else
            pokeFile = InputPokeFile
        End If

        Dim pokeFilePath As String = GameModeManager.GetPokeFilePath(pokeFile)

        If System.IO.File.Exists(pokeFilePath) = True Then
            Security.FileValidation.CheckFileValid(pokeFilePath, False, "Spawner.vb")
            Dim Data() As String = System.IO.File.ReadAllLines(pokeFilePath)

            Dim i As Integer = 0
            For Each Line As String In Data
                If Line.StartsWith("{") = True And Line.EndsWith("}") = True Then
                    Line = Line.Remove(0, Line.IndexOf("{") + 1)
                    Line = Line.Remove(Line.LastIndexOf("}"))

                    Dim splits() As String = Line.Split(CChar("|"))

                    Dim PMethod As Integer = CInt(splits(0))
                    Dim Pokemon As String = splits(1)
                    Dim Chance As Integer = CInt(splits(2))
                    Dim DayTime() As String = splits(3).Split(CChar(","))
                    Dim levelCap As String = splits(4)

                    'Optional Season and Weather checks:
                    Dim Weathers() As String = {"-1"}
                    Dim Seasons() As String = {"-1"}

                    If splits.Length > 5 Then
                        Weathers = splits(5).Split(",")
                    End If
                    If splits.Length > 6 Then
                        Seasons = splits(6).Split(",")
                    End If

                    If Weathers.Contains("-1") = True Or Weathers.Contains(CInt(World.GetWeatherFromWeatherType(Screen.Level.WeatherType)).ToString()) = True Then
                        If Seasons.Contains("-1") = True Or Seasons.Contains(CInt(World.CurrentSeason).ToString()) = True Then
                            If DayTime.Contains("-1") = True Or DayTime.Contains(CInt(World.GetTime()).ToString()) = True Then
                                If Method = PMethod Then
                                    Pokemons.Add(Pokemon)
                                    Chances.Add(Chance)
                                    LevelCaps.Add(levelCap)
                                End If
                            End If
                        End If
                    End If

                    i += 1
                End If
            Next

            If Pokemons.Count > 0 Then
                Dim Pokemon As Pokemon = CalculatePokemon(Pokemons, Chances, LevelCaps)
                Return Pokemon
            Else
                Return Nothing
            End If
        Else
            Return Nothing
        End If
    End Function

    Private Shared pokeFile As String = ""

    Private Shared Function CheckForRoaming(ByVal LevelFile As String, ByVal Method As Integer) As RoamingPokemon
        If Method = 0 Or Method = 2 Then
            Dim roamingData() As String = Core.Player.RoamingPokemonData.SplitAtNewline()
            If roamingData.Length > 0 Then
                Dim possibleEncounters As New List(Of String)

                For Each cLine As String In roamingData
                    If cLine <> "" Then
                        If cLine.CountSeperators("|") >= 8 Then
                            possibleEncounters.Add(cLine)
                        End If
                    End If
                Next

                possibleEncounters = possibleEncounters.Randomize()

                If possibleEncounters.Count > 0 Then
                    For i = 0 To possibleEncounters.Count - 1
                        Dim line As String = possibleEncounters(i)
                        Dim data() As String = line.Split(CChar("|"))

                        If data(4).ToLower() = LevelFile.ToLower() Then
                            Return New RoamingPokemon(line)
                        End If
                    Next
                End If
            End If
        End If

        Return Nothing
    End Function

    Private Shared Function CalculatePokemon(ByVal Pokemons As List(Of String), ByVal Chances As List(Of Integer), ByVal LevelCaps As List(Of String)) As Pokemon
        Dim totalNumber As Integer = 0
        For Each c As Integer In Chances
            totalNumber += c
        Next

        Dim r As Integer = Core.Random.Next(0, totalNumber + 1)

        Dim x As Integer = 0
        For i = 0 To Chances.Count - 1
            x += Chances(i)
            If r < x Then
                Dim levelCap() As String = LevelCaps(i).Split(CChar(","))
                Dim minLevel As Integer = CInt(levelCap(0))
                Dim maxLevel As Integer
                If levelCap.Count > 1 Then
                    maxLevel = CInt(levelCap(1))
                Else
                    maxLevel = minLevel
                End If
                If maxLevel < minLevel Then
                    maxLevel = minLevel
                End If

                Dim level As Integer = Core.Random.Next(minLevel, maxLevel + 1)

                'Hustle/Pressure/Vital Spirit:
                If Core.Player.Pokemons.Count > 0 Then
                    Dim abilityName As String = Core.Player.Pokemons(0).Ability.Name.ToLower()
                    If abilityName = "hustle" Or abilityName = "pressure" Or abilityName = "vital spirit" Then
                        If Core.Random.Next(0, 100) < 50 Then
                            level = maxLevel
                        End If
                    End If
                End If

                Dim addLevel As Integer = 0
                If Core.Player.DifficultyMode = 0 Then
                    addLevel = CInt(Math.Ceiling(level * CSng(GameModeManager.GetGameRuleValue("LevelMultiplier", "1.0").InsertDecSeparator) - level))
                ElseIf Core.Player.DifficultyMode = 1 Then
                    addLevel = CInt(Math.Ceiling(level * CSng(GameModeManager.GetGameRuleValue("LevelMultiplier", "1.1").InsertDecSeparator) - level))
                ElseIf Core.Player.DifficultyMode = 2 Then
                    addLevel = CInt(Math.Ceiling(level * CSng(GameModeManager.GetGameRuleValue("LevelMultiplier", "1.2").InsertDecSeparator) - level))
                End If
                level += addLevel
                If level > CInt(GameModeManager.GetGameRuleValue("MaxLevel", "100")) Then
                    level = CInt(GameModeManager.GetGameRuleValue("MaxLevel", "100"))
                End If

                Dim PkID As Integer = CInt(Pokemons(i).Split(CChar("_"))(0))
                Dim PkAD As String = ""
                Dim p As Pokemon = Pokemon.GetPokemonByID(PkID)
                For Each region As String In Screen.Level.RegionalForm.ToLower().Split(CChar(","))
                    If p.RegionalForms <> Nothing AndAlso p.RegionalForms.ToLower().Contains(region) Then
                        PkAD = region
                    End If
                Next
                If Pokemons(i).Contains("_") Then
                    PkAD = Pokemons(i).Split(CChar("_"))(1)
                End If
                p = Pokemon.GetPokemonByID(PkID, PkAD)
                p.Generate(level, True, PkAD)
                p.ReloadDefinitions()
                p.CalculateStats()

                BattleSystem.BattleScreen.TempPokeFile = pokeFile

                Return p
            End If
        Next
        Return Nothing
    End Function

End Class
