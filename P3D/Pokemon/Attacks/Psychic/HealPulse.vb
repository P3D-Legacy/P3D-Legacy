Namespace BattleSystem.Moves.Psychic

    Public Class HealPulse

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Psychic)
            Me.ID = 505
            Me.OriginalPP = 10
            Me.CurrentPP = 10
            Me.MaxPP = 10
            Me.Power = 0
            Me.Accuracy = 100
            Me.Category = Categories.Status
            Me.ContestCategory = ContestCategories.Beauty
            Me.Name = Localization.GetString("move_name_" & Me.ID,"Heal Pulse")
            Me.Description = "The user emits a healing pulse which restores the target's HP by up to half of its max HP."
            Me.CriticalChance = 1
            Me.IsHMMove = False
            Me.Target = Targets.OneTarget
            Me.Priority = 0
            Me.TimesToAttack = 1
            '#End

            '#SpecialDefinitions
            Me.MakesContact = False
            Me.ProtectAffected = True
            Me.MagicCoatAffected = True
            Me.SnatchAffected = False
            Me.MirrorMoveAffected = True
            Me.KingsrockAffected = False
            Me.CounterAffected = False

            Me.DisabledWhileGravity = False
            Me.UseEffectiveness = False
            Me.ImmunityAffected = False
            Me.HasSecondaryEffect = False
            Me.RemovesFrozen = False

            Me.IsHealingMove = True
            Me.IsRecoilMove = False
            Me.IsPunchingMove = False
            Me.IsDamagingMove = False
            Me.IsProtectMove = False
            Me.IsSoundMove = False

            Me.IsAffectedBySubstitute = True
            Me.IsOneHitKOMove = False
            Me.IsWonderGuardAffected = False
            Me.IsPulseMove = True
            '#End

            Me.AIField1 = AIField.Support
            Me.AIField2 = AIField.Nothing
        End Sub

        Public Overrides Sub MoveHits(own As Boolean, BattleScreen As BattleScreen)
            Dim p As Pokemon = BattleScreen.OwnPokemon
            Dim op As Pokemon = BattleScreen.OppPokemon
            If own = False Then
                p = BattleScreen.OppPokemon
                op = BattleScreen.OwnPokemon
            End If

            If own = True Then
                If BattleScreen.FieldEffects.OppHealBlock > 0 Then
                    Dim heal As Integer = CInt(Math.Ceiling(op.MaxHP / 2))
                    If p.Ability.Name.ToLower() = "mega launcher" Then
                        heal = CInt(Math.Ceiling(op.MaxHP * (3 / 4)))
                    End If
                    heal = heal.Clamp(0, 999)

                    BattleScreen.Battle.GainHP(heal, Not own, own, BattleScreen, op.GetDisplayName() & " had its HP restored!", "move:healpulse")
                Else
                    BattleScreen.BattleQuery.Add(New TextQueryObject(Me.Name & " failed!"))
                End If
            Else
                If BattleScreen.FieldEffects.OwnHealBlock > 0 Then
                    Dim heal As Integer = CInt(Math.Ceiling(op.MaxHP / 2))
                    If p.Ability.Name.ToLower() = "mega launcher" Then
                        heal = CInt(Math.Ceiling(op.MaxHP * (3 / 4)))
                    End If
                    heal = heal.Clamp(0, 999)

                    BattleScreen.Battle.GainHP(heal, Not own, own, BattleScreen, op.GetDisplayName() & " had its HP restored!", "move:healpulse")
                Else
                    BattleScreen.BattleQuery.Add(New TextQueryObject(Me.Name & " failed!"))
                End If
            End If
        End Sub

    End Class

End Namespace