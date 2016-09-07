Namespace BattleSystem.Moves.Dragon

    Public Class DragonTail

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Dragon)
            Me.ID = 525
            Me.OriginalPP = 10
            Me.CurrentPP = 10
            Me.MaxPP = 10
            Me.Power = 60
            Me.Accuracy = 90
            Me.Category = Categories.Physical
            Me.ContestCategory = ContestCategories.Cool
            Me.Name = "Dragon Tail"
            Me.Description = "The target is knocked away, and a different Pokémon is dragged out. In the wild, this ends a battle against a single Pokémon."
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
            Dim op As Pokemon = BattleScreen.OppPokemon
            If own = False Then
                op = BattleScreen.OwnPokemon
            End If

            'Not fainted:
            If op.HP > 0 And op.Status <> Pokemon.StatusProblems.Fainted Then
                Dim substitude As Integer = BattleScreen.FieldEffects.OppSubstitute
                If own = False Then
                    substitude = BattleScreen.FieldEffects.OwnSubstitute
                End If

                'substitude:
                If substitude <= 0 Then

                    'suction cups ability:
                    If op.Ability.Name.ToLower() <> "suction cups" Then
                        Dim ingrain As Integer = BattleScreen.FieldEffects.OppIngrain
                        If own = False Then
                            ingrain = BattleScreen.FieldEffects.OwnIngrain
                        End If

                        'check ingrain set up:
                        If ingrain <= 0 Then

                            If BattleScreen.IsTrainerBattle = True Or BattleScreen.IsPVPBattle = True Or BattleScreen.IsRemoteBattle = True Then
                                If own = True Then
                                    If BattleScreen.Trainer.CountUseablePokemon > 1 Then
                                        BattleScreen.Battle.SwitchOutOpp(BattleScreen, -1, "")
                                    End If
                                Else
                                    If Core.Player.CountFightablePokemon > 1 Then
                                        BattleScreen.Battle.SwitchOutOwn(BattleScreen, -1, -1)
                                    End If
                                End If
                            Else
                                If own = True Then
                                    BattleScreen.BattleQuery.Add(New EndBattleQueryObject(False))
                                Else
                                    If Core.Player.CountFightablePokemon > 1 Then
                                        BattleScreen.Battle.SwitchOutOwn(BattleScreen, -1, -1)
                                    End If
                                End If
                            End If
                        End If
                    End If
                End If
            End If
        End Sub

    End Class

End Namespace