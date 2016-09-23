''' <summary>
''' A class to handle wild Pokémon encounters.
''' </summary>
Public Class PokemonEncounter

#Region "Fields and Constants"

    'Stores a reference to the level instance.
    Private _levelReference As Level

#End Region

#Region "Constructors"

    ''' <summary>
    ''' Creates a new instance of the PokemonEncounter class.
    ''' </summary>
    ''' <param name="levelReference">The reference to the level instance.</param>
    Public Sub New(ByVal levelReference As Level)
        Me._levelReference = levelReference
    End Sub

#End Region

#Region "Methods"

    ''' <summary>
    ''' Checks if the player should encounter a wild Pokémon.
    ''' </summary>
    ''' <param name="Position">The position the encounter should happen.</param>
    ''' <param name="Method">The method of the encounter.</param>
    ''' <param name="pokeFile">The source .poke file. If left empty, the game will assume the levelfile as source .poke file.</param>
    Public Sub TryEncounterWildPokemon(ByVal Position As Vector3, ByVal Method As Spawner.EncounterMethods, ByVal pokeFile As String)
        With Me._levelReference

            If .WalkedSteps > 3 Then 'Only after walking 3 steps, try to encounter a wild Pokémon.
                'Compose the correct .poke file from the levelfile, if the pokeFile parameter is empty.
                If pokeFile = "" Then
                    pokeFile = .LevelFile.Remove(.LevelFile.Length - 4, 4) & ".poke"
                End If

                If System.IO.File.Exists(GameModeManager.GetPokeFilePath(pokeFile)) = True Then 'Only try to register a wild battle if the .poke file exists.
                    Dim startRandomValue As Integer = 12
                    Dim minRandomValue As Integer = 5

                    If Core.Player.Pokemons.Count > 0 Then
                        Dim p As Pokemon = Core.Player.Pokemons(0)

                        'Arena Trap/Illuminate/No Guard/Swarm ability:
                        If p.Ability.Name.ToLower() = "arena trap" Or p.Ability.Name.ToLower() = "illuminate" Or p.Ability.Name.ToLower() = "no guard" Or p.Ability.Name.ToLower() = "swarm" Then
                            startRandomValue = 6
                            minRandomValue = 3
                        End If

                        'Intimidate/Keen Eye/Quick Feet/Stench/White Smoke ability:
                        If p.Ability.Name.ToLower() = "intimidate" Or p.Ability.Name.ToLower() = "keen eye" Or p.Ability.Name.ToLower() = "quick feet" Or p.Ability.Name.ToLower() = "stench" Or p.Ability.Name.ToLower() = "white smoke" Then
                            startRandomValue = 24
                            minRandomValue = 10
                        End If

                        'Sand Veil ability:
                        If .WeatherType = 7 And p.Ability.Name.ToLower() = "sand veil" Then
                            If Core.Random.Next(0, 100) < 50 Then
                                Exit Sub
                            End If
                        End If

                        'Snow Cloak ability:
                        If p.Ability.Name.ToLower() = "snow cloak" Then
                            If .WeatherType = 2 Or .WeatherType = 9 Then
                                If Core.Random.Next(0, 100) < 50 Then
                                    Exit Sub
                                End If
                            End If
                        End If
                    End If

                    'Do some shit to determine if the wild Pokémon will be met or not:
                    Dim randomValue As Integer = startRandomValue - .WalkedSteps
                    randomValue = CInt(MathHelper.Clamp(randomValue, minRandomValue, startRandomValue))

                    If Core.Random.Next(0, randomValue * 2) = 0 Then
                        'Don't encounter a Pokémon if the left ctrl key is held down.
                        If GameController.IS_DEBUG_ACTIVE = True Or Core.Player.SandBoxMode = True Then
                            If KeyBoardHandler.KeyDown(Keys.LeftControl) = True Then
                                Exit Sub
                            End If
                        End If

                        'Reset walked steps and set the wild Pokémon data:
                        .WalkedSteps = 0

                        .PokemonEncounterData.Position = Position
                        .PokemonEncounterData.EncounteredPokemon = True
                        .PokemonEncounterData.Method = Method
                        .PokemonEncounterData.PokeFile = pokeFile
                    End If
                End If
            End If
        End With
    End Sub

    ''' <summary>
    ''' Triggers a battle with a wild Pokémon if the requirements are met.
    ''' </summary>
    Public Sub TriggerBattle()
        'If the encounter check is true.
        If Me._levelReference.PokemonEncounterData.EncounteredPokemon = True And Core.CurrentScreen.Identification = Screen.Identifications.OverworldScreen Then
            'If the player met the set position:
            If Screen.Camera.Position.X = Me._levelReference.PokemonEncounterData.Position.X And Screen.Camera.Position.Z = Me._levelReference.PokemonEncounterData.Position.Z Then
                'Make the player stop and set encounter check to false.
                Me._levelReference.PokemonEncounterData.EncounteredPokemon = False
                Screen.Camera.StopMovement()

                'Generate new wild Pokémon:
                Dim Pokemon As Pokemon = Spawner.GetPokemon(Screen.Level.LevelFile, Me._levelReference.PokemonEncounterData.Method, True, Me._levelReference.PokemonEncounterData.PokeFile)

                If Not Pokemon Is Nothing And CType(Core.CurrentScreen, OverworldScreen).TrainerEncountered = False And CType(Core.CurrentScreen, OverworldScreen).ActionScript.IsReady = True Then
                    Screen.Level.RouteSign.Hide() 'When a battle starts, hide the Route sign.

                    'If the player has a repel going and the first Pokémon's level is greater than the wild Pokémon's level, don't start the battle.
                    If Core.Player.RepelSteps > 0 Then
                        Dim p As Pokemon = Core.Player.GetWalkPokemon()
                        If Not p Is Nothing Then
                            If p.Level >= Pokemon.Level Then
                                Exit Sub
                            End If
                        End If
                    End If

                    'Cleanse Tag prevents wild Pokémon if held by first Pokémon in party.
                    If Core.Player.Pokemons(0).Level >= Pokemon.Level Then
                        If Not Core.Player.Pokemons(0).Item Is Nothing Then
                            If Core.Player.Pokemons(0).Item.ID = 94 Then
                                If Core.Random.Next(0, 3) = 0 Then
                                    Exit Sub
                                End If
                            End If
                        End If
                    End If

                     'Pure Incense Lowers the chance of encountering wild Pokémon if held by first Pokémon in party.
                    If Core.Player.Pokemons(0).Level >= Pokemon.Level Then
                        If Not Core.Player.Pokemons(0).Item Is Nothing Then
                            If Core.Player.Pokemons(0).Item.ID = 291 Then
                                If Core.Random.Next(0, 3) = 0 Then
                                    Exit Sub
                                End If
                            End If
                        End If
                    End If

                    'Register wild Pokémon in the Pokédex.
                    Core.Player.PokedexData = Pokedex.ChangeEntry(Core.Player.PokedexData, Pokemon.Number, 1)

                    'Determine wild intro type. If it's a roaming battle, set 12.
                    Dim introType As Integer = Core.Random.Next(0, 10)
                    If BattleSystem.BattleScreen.RoamingBattle = True Then
                        introType = 12
                    End If

                    Dim b As New BattleSystem.BattleScreen(Pokemon, Core.CurrentScreen, Me._levelReference.PokemonEncounterData.Method)
                    Core.SetScreen(New BattleIntroScreen(Core.CurrentScreen, b, introType))
                End If
            End If
        End If
    End Sub

#End Region

End Class
