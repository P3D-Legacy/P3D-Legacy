Namespace Items.Medicine

    <Item(32, "Rare Candy")>
    Public Class RareCandy

        Inherits MedicineItem

        Public Overrides ReadOnly Property Description As String = "A candy that is packed with energy. When consumed, it will instantly raise the level of a single Pokémon by one."
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 4800
        Public Overrides ReadOnly Property BattlePointsPrice As Integer = 48
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property PluralName As String = "Rare Candies"

        Public Sub New()
            _textureRectangle = New Rectangle(192, 24, 24, 24)
        End Sub

        Public Overrides Sub Use()
            Dim selScreen = New PartyScreen(Core.CurrentScreen, Me, AddressOf Me.UseOnPokemon, "Use " & Me.Name, True) With {.Mode = Screens.UI.ISelectionScreen.ScreenMode.Selection, .CanExit = True}
            AddHandler selScreen.SelectedObject, AddressOf UseItemhandler

            Core.SetScreen(selScreen)
        End Sub

        Public Overrides Function UseOnPokemon(ByVal PokeIndex As Integer) As Boolean
            Dim Pokemon As Pokemon = Core.Player.Pokemons(PokeIndex)

            If Pokemon.Level < CInt(GameModeManager.GetGameRuleValue("MaxLevel", "100")) And Pokemon.IsEgg() = False Then
                Dim beforeHP As Integer = Pokemon.MaxHP
                Pokemon.LevelUp(False)
                Pokemon.Experience = Pokemon.NeedExperience(Pokemon.Level)
                If Pokemon.Status = P3D.Pokemon.StatusProblems.Fainted Then
                    Pokemon.Status = P3D.Pokemon.StatusProblems.None
                    Pokemon.HP = (Pokemon.MaxHP - beforeHP).Clamp(1, 999)
                End If

                Dim s As String =
                    "version=2" & Environment.NewLine &
                    "@sound.play(success_small,1)" & Environment.NewLine &
                    "@text.show(" & Pokemon.GetDisplayName() & " reached~level " & Pokemon.Level & "!)" & Environment.NewLine

                Dim removedItem As Boolean = False

                If Pokemon.AttackLearns.ContainsKey(Pokemon.Level) = True Then
                    If Pokemon.KnowsMove(Pokemon.AttackLearns(Pokemon.Level)) = False Then
                        If Pokemon.Attacks.Count = 4 Then
                            s &= "@pokemon.learnattack(" & PokeIndex & "," & Pokemon.AttackLearns(Pokemon.Level).ID & ")" & Environment.NewLine

                            Dim t As String = Me.RemoveItem()
                            If t <> "" Then
                                s &= "@text.show(" & t & ")" & Environment.NewLine
                            End If
                            removedItem = True
                        Else
                            Pokemon.Attacks.Add(Pokemon.AttackLearns(Pokemon.Level))

                            s &= "@sound.play(success_small,1)" & Environment.NewLine &
                                 "@text.show(" & Pokemon.GetDisplayName() & " learned~" & Pokemon.AttackLearns(Pokemon.Level).Name & "!" & Me.RemoveItem() & ")" & Environment.NewLine
                            removedItem = True
                            PlayerStatistics.Track("Moves learned", 1)
                        End If
                    End If
                End If

                If Pokemon.CanEvolve(EvolutionCondition.EvolutionTrigger.LevelUp, "") = True Then
                    s &= "@pokemon.evolve(" & PokeIndex & ")" & Environment.NewLine
                End If

                If removedItem = False Then
                    Dim t As String = Me.RemoveItem()
                    If t <> "" Then
                        s &= "@text.show(" & t & ")" & Environment.NewLine
                    End If
                End If

                s &= ":end"

                Dim sc As Screen = Core.CurrentScreen
                While sc.Identification <> Screen.Identifications.OverworldScreen
                    sc = sc.PreScreen
                End While

                Core.SetScreen(sc)

                CType(sc, OverworldScreen).ActionScript.StartScript(s, 2, False)
                PlayerStatistics.Track("[17]Medicine Items used", 1)

                Return True
            Else
                Return False
            End If
        End Function

    End Class

End Namespace
