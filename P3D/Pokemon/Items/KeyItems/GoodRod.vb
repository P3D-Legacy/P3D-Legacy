Namespace Items.KeyItems

    <Item(59, "Good Rod")>
    Public Class GoodRod

        Inherits KeyItem

        Public Overrides ReadOnly Property Description As String = "A new, good-quality fishing rod. Use it by any body of water to fish for wild aquatic Pokémon."
        Public Overrides ReadOnly Property CanBeUsed As Boolean = True

        Public Sub New()
            _textureRectangle = New Rectangle(264, 48, 24, 24)
        End Sub

        Public Overrides Sub Use()
            If OldRod.IsInfrontOfWater() = True And Screen.Level.Surfing = False And Screen.Level.Riding = False Then
                Dim s As String = "version=2"

                While Core.CurrentScreen.Identification <> Screen.Identifications.OverworldScreen
                    Core.CurrentScreen = Core.CurrentScreen.PreScreen
                End While

                Dim p As Pokemon = Nothing

                Dim pokeFile As String = "poke\" & Screen.Level.LevelFile.Remove(Screen.Level.LevelFile.Length - 4, 4) & ".poke"
                If GameModeManager.MapFileExists(pokeFile) = True Then
                    p = Spawner.GetPokemon(Screen.Level.LevelFile, Spawner.EncounterMethods.GoodRod, False)
                End If

                If p Is Nothing Then
                    p = Pokemon.GetPokemonByID(129)
                    p.Generate(20, True)
                End If

                Dim PokemonID As Integer = p.Number
                Dim PokemonShiny As String = "N"
                If p.IsShiny = True Then
                    PokemonShiny = "S"
                End If

                If Core.Random.Next(0, 3) <> 0 Or Core.Player.Pokemons(0).Ability.Name.ToLower() = "suction cups" Or Core.Player.Pokemons(0).Ability.Name.ToLower() = "sticky hold" Then
                    Dim LookingOffset As New Vector3(0)

                    Select Case Screen.Camera.GetPlayerFacingDirection()
                        Case 0
                            LookingOffset.Z = -1
                        Case 1
                            LookingOffset.X = -1
                        Case 2
                            LookingOffset.Z = 1
                        Case 3
                            LookingOffset.X = 1
                    End Select

                    Dim spawnPosition As Vector3 = New Vector3(Screen.Camera.Position.X + LookingOffset.X, Screen.Camera.Position.Y, Screen.Camera.Position.Z + LookingOffset.Z)

                    Dim endRotation As Integer = Screen.Camera.GetPlayerFacingDirection() + 2
                    If endRotation > 3 Then
                        endRotation = endRotation - 4
                    End If

                    s &= Environment.NewLine & "@player.showrod(1)" & Environment.NewLine &
                        "@text.show(. . . . . . . . . .)" & Environment.NewLine &
                        "@text.show(Oh!~A bite!)" & Environment.NewLine &
                        "@player.hiderod" & Environment.NewLine &
                        "@npc.spawn(" & spawnPosition.X.ToString().Replace(GameController.DecSeparator, ".") & "," & spawnPosition.Y.ToString().Replace(GameController.DecSeparator, ".") & "," & spawnPosition.Z.ToString().Replace(GameController.DecSeparator, ".") & ",0,...,[POKEMON|" & PokemonShiny & "]" & PokemonID & PokemonForms.GetOverworldAddition(p) & ",0," & endRotation & ",POKEMON,1337,Still)" & Environment.NewLine &
                        "@Level.Update" & Environment.NewLine &
                        "@pokemon.cry(" & PokemonID & ")" & Environment.NewLine &
                        "@level.wait(50)" & Environment.NewLine &
                        "@text.show(The wild " & p.OriginalName & "~attacked!)" & Environment.NewLine &
                        "@npc.remove(1337)" & Environment.NewLine &
                        "@battle.setvar(divebattle,true)" & Environment.NewLine &
                        "@battle.wild(" & p.GetSaveData() & ")" & Environment.NewLine &
                        ":end"
                Else
                    s &= Environment.NewLine & "@player.showrod(1)" & Environment.NewLine &
                        "@text.show(. . . . . . . . . .)" & Environment.NewLine &
                        "@text.show(No, there's nothing here...)" & Environment.NewLine &
                        "@player.hiderod" & Environment.NewLine &
                        ":end"
                End If

                CType(Core.CurrentScreen, OverworldScreen).ActionScript.StartScript(s, 2)
            Else
                Screen.TextBox.Show("Now is not the time~to use that.", {}, True, True)
            End If
        End Sub

    End Class

End Namespace
