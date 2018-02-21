Namespace BattleSystem.Moves.Fighting

    Public Class CircleThrow

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Fighting)
            Me.ID = 509
            Me.OriginalPP = 10
            Me.CurrentPP = 10
            Me.MaxPP = 10
            Me.Power = 60
            Me.Accuracy = 90
            Me.Category = Categories.Physical
            Me.ContestCategory = ContestCategories.Cool
            Me.Name = "Circle Throw"
            Me.Description = "	The target is thrown, and a different Pokémon is dragged out. In the wild, this ends a battle against a single Pokémon."
            Me.CriticalChance = 1
            Me.IsHMMove = False
            Me.Target = Targets.OneAdjacentTarget
            Me.Priority = -6
            Me.TimesToAttack = 1
            '#End

            '#SpecialDefinitions
            Me.MakesContact = True
            Me.ProtectAffected = True
            Me.MagicCoatAffected = False
            Me.SnatchAffected = False
            Me.MirrorMoveAffected = True
            Me.KingsrockAffected = False
            Me.CounterAffected = True

            Me.DisabledWhileGravity = False
            Me.UseEffectiveness = True
            Me.ImmunityAffected = True
            Me.HasSecondaryEffect = False
            Me.RemovesFrozen = False

            Me.IsHealingMove = False
            Me.IsRecoilMove = False
            Me.IsPunchingMove = False
            Me.IsDamagingMove = True
            Me.IsProtectMove = False
            Me.IsSoundMove = False

            Me.IsAffectedBySubstitute = True
            Me.IsOneHitKOMove = False
            Me.IsWonderGuardAffected = True
            '#End

            Me.AIField1 = AIField.Damage
            Me.AIField2 = AIField.Nothing
        End Sub

        Public Overrides Sub MoveHits(own As Boolean, BattleScreen As BattleScreen)
            Dim p As Pokemon = BattleScreen.OwnPokemon    'p is the phazed pokemon
            If own = True Then
                p = BattleScreen.OppPokemon
            End If

            'Not fainted:
            If p.HP > 0 And p.Status <> Pokemon.StatusProblems.Fainted Then
                Dim substitude As Integer = BattleScreen.FieldEffects.OwnSubstitute
                If own = True Then
                    substitude = BattleScreen.FieldEffects.OppSubstitute
                End If

                'substitute:
                If substitude <= 0 Then

                    'suction cups ability:
                    If p.Ability.Name.ToLower() <> "suction cups" Then
                        Dim ingrain As Integer = BattleScreen.FieldEffects.OwnIngrain
                        If own = True Then
                            ingrain = BattleScreen.FieldEffects.OppIngrain
                        End If

                        'check ingrain set up:
                        If ingrain <= 0 Then

                            If BattleCalculation.CanSwitch(BattleScreen, Not own) = True Then
                                 BattleScreen.BattleQuery.Add(New TextQueryObject(p.GetDisplayName() & " got thrown away!"))
                                If BattleScreen.IsPVPBattle = True Or BattleScreen.IsTrainerBattle = True Or BattleScreen.IsRemoteBattle = True Then
                                    'trainer battle
                                    If own = True Then
                                        If BattleScreen.Trainer.CountUseablePokemon > 1 Then
                                            Dim i As Integer = Core.Random.Next(0, BattleScreen.Trainer.Pokemons.count)
                                            While BattleScreen.Trainer.Pokemons(i).Status = Pokemon.StatusProblems.Fainted OrElse BattleScreen.OppPokemonIndex = i OrElse BattleScreen.Trainer.Pokemons(i).HP <= 0
                                                i = Core.Random.Next(0, BattleScreen.Trainer.Pokemons.Count - 1)
                                            End While
                                            BattleScreen.Battle.SwitchOutOpp(BattleScreen, i, "")
                                        Else
                                            BattleScreen.BattleQuery.Add(New TextQueryObject(Me.Name & " failed!"))
                                        End If
                                    Else
                                        If Core.Player.CountFightablePokemon > 1 Then
                                             Dim i As Integer = Core.Random.Next(0, Core.Player.Pokemons.Count)
                                            While Core.Player.Pokemons(i).Status = Pokemon.StatusProblems.Fainted OrElse BattleScreen.OwnPokemonIndex = i OrElse Core.Player.Pokemons(i).HP <= 0
                                                i = Core.Random.Next(0, Core.Player.Pokemons.Count - 1)
                                            End While
                                            BattleScreen.Battle.SwitchOutOwn(BattleScreen, i, -1)
                                        Else
                                            BattleScreen.BattleQuery.Add(New TextQueryObject(Me.Name & " failed!"))
                                        End If
                                    End If
                                Else
                                    'wild battle

                                    If own = True Then
                                        BattleScreen.BattleQuery.Add(New EndBattleQueryObject(False))
                                    Else
                                        If Core.Player.CountFightablePokemon > 1 Then
                                            BattleScreen.Battle.SwitchOutOwn(BattleScreen, -1, -1)
                                        Else
                                            BattleScreen.BattleQuery.Add(New TextQueryObject(Me.Name & " failed!"))
                                        End If
                                    End If
                                End If
                            Else
                                BattleScreen.BattleQuery.Add(New TextQueryObject(Me.Name & " failed!"))
                            End If
                        End If
                    End If
                End If
            End If
        End Sub

    End Class

End Namespace
