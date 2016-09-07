Namespace BattleSystem.Moves.Normal

    Public Class HealBell

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Normal)
            Me.ID = 215
            Me.OriginalPP = 5
            Me.CurrentPP = 5
            Me.MaxPP = 5
            Me.Power = 0
            Me.Accuracy = 0
            Me.Category = Categories.Status
            Me.ContestCategory = ContestCategories.Beauty
            Me.Name = "Heal Bell"
            Me.Description = "The user makes a soothing bell chime to heal the status problems of all the party Pokémon."
            Me.CriticalChance = 0
            Me.IsHMMove = False
            Me.Target = Targets.AllAllies
            Me.Priority = 0
            Me.TimesToAttack = 1
            '#End

            '#SpecialDefinitions
            Me.MakesContact = False
            Me.ProtectAffected = False
            Me.MagicCoatAffected = False
            Me.SnatchAffected = True
            Me.MirrorMoveAffected = True
            Me.KingsrockAffected = False
            Me.CounterAffected = False

            Me.DisabledWhileGravity = False
            Me.UseEffectiveness = False
            Me.ImmunityAffected = False
            Me.HasSecondaryEffect = False
            Me.RemovesFrozen = False

            Me.IsHealingMove = False
            Me.IsRecoilMove = False
            Me.IsPunchingMove = False
            Me.IsDamagingMove = False
            Me.IsProtectMove = False
            Me.IsSoundMove = True

            Me.IsAffectedBySubstitute = False
            Me.IsOneHitKOMove = False
            Me.IsWonderGuardAffected = False
            '#End

            Me.AIField1 = AIField.CureStatus
            Me.AIField2 = AIField.Nothing
        End Sub

        Public Overrides Sub MoveHits(own As Boolean, BattleScreen As BattleScreen)
            Dim healed As New List(Of Pokemon)

            Dim p As Pokemon = BattleScreen.OwnPokemon
            If own = False Then
                p = BattleScreen.OppPokemon
            End If

            If p.Status <> Pokemon.StatusProblems.None And p.Status <> Pokemon.StatusProblems.Fainted Then
                If p.Ability.Name.ToLower() <> "soundproof" And p.Ability.Name.ToLower() <> "cacophony" Then
                    If BattleScreen.FieldEffects.CanUseAbility(own, BattleScreen) = True Then
                        healed.Add(p)
                    End If
                End If
            End If

            If own = True Then
                For Each tp As Pokemon In Core.Player.Pokemons
                    If Not tp Is p Then
                        If tp.Status <> Pokemon.StatusProblems.None And tp.Status <> Pokemon.StatusProblems.Fainted Then
                            If tp.Ability.Name.ToLower() <> "soundproof" And tp.Ability.Name.ToLower() <> "cacophony" Then
                                If BattleScreen.FieldEffects.CanUseAbility(own, BattleScreen) = True Then
                                    healed.Add(tp)
                                End If
                            End If
                        End If
                    End If
                Next
            Else
                If BattleScreen.IsTrainerBattle = True Or BattleScreen.IsPVPBattle = True Or BattleScreen.IsRemoteBattle = True Then
                    For Each tp As Pokemon In BattleScreen.Trainer.Pokemons
                        If Not tp Is p Then
                            If tp.Status <> Pokemon.StatusProblems.None And tp.Status <> Pokemon.StatusProblems.Fainted Then
                                If tp.Ability.Name.ToLower() <> "soundproof" And tp.Ability.Name.ToLower() <> "cacophony" Then
                                    If BattleScreen.FieldEffects.CanUseAbility(own, BattleScreen) = True Then
                                        healed.Add(tp)
                                    End If
                                End If
                            End If
                        End If
                    Next
                End If
            End If

            BattleScreen.BattleQuery.Add(New TextQueryObject("A bell chimed!"))

            If healed.Count > 0 Then
                For i = 0 To healed.Count - 1
                    Dim statusName As String = "poisoning"
                    Select Case healed(i).Status
                        Case Pokemon.StatusProblems.BadPoison, Pokemon.StatusProblems.Poison
                            statusName = "poisong"
                        Case Pokemon.StatusProblems.Burn
                            statusName = "burn"
                        Case Pokemon.StatusProblems.Freeze
                            statusName = "freezing"
                        Case Pokemon.StatusProblems.Paralyzed
                            statusName = "paralyzis"
                    End Select

                    healed(i).Status = Pokemon.StatusProblems.None
                    BattleScreen.BattleQuery.Add(New TextQueryObject(healed(i).GetDisplayName() & " was cured of its " & statusName & "!"))
                Next
            Else
                BattleScreen.BattleQuery.Add(New TextQueryObject(Me.Name & " failed!"))
            End If
        End Sub

    End Class

End Namespace