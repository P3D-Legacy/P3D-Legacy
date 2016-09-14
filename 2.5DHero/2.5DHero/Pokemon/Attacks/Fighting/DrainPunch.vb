Namespace BattleSystem.Moves.Fighting

    Public Class DrainPunch

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Fighting)
            Me.ID = 409
            Me.OriginalPP = 10
            Me.CurrentPP = 10
            Me.MaxPP = 10
            Me.Power = 75
            Me.Accuracy = 100
            Me.Category = Categories.Physical
            Me.ContestCategory = ContestCategories.Beauty
            Me.Name = "Drain Punch"
            Me.Description = "An energy-draining punch. The user's HP is restored by half the damage taken by the target."
            Me.CriticalChance = 1
            Me.IsHMMove = False
            Me.Target = Targets.OneAdjacentTarget
            Me.Priority = 0
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

            Me.IsHealingMove = True
            Me.IsRecoilMove = False
            Me.IsPunchingMove = True
            Me.IsDamagingMove = True
            Me.IsProtectMove = False
            Me.IsSoundMove = False

            Me.IsAffectedBySubstitute = True
            Me.IsOneHitKOMove = False
            Me.IsWonderGuardAffected = True
            '#End

            Me.AIField1 = AIField.Damage
            Me.AIField2 = AIField.Absorbing
        End Sub

        Public Overrides Sub MoveHits(own As Boolean, BattleScreen As BattleScreen)
            Dim p As Pokemon = BattleScreen.OwnPokemon
            Dim op As Pokemon = BattleScreen.OppPokemon
            If own = False Then
                p = BattleScreen.OppPokemon
                op = BattleScreen.OwnPokemon
            End If

            Dim damage As Integer = BattleScreen.FieldEffects.OwnLastDamage
            If own = False Then
                damage = BattleScreen.FieldEffects.OppLastDamage
            End If
            Dim heal As Integer = CInt(Math.Ceiling(damage / 2))

            If heal <= 0 Then
                heal = 1
            End If

            If op.Ability.Name.ToLower() = "liquid ooze" And BattleScreen.FieldEffects.CanUseAbility(Not own, BattleScreen) = True Then
                BattleScreen.Battle.ReduceHP(heal, own, own, BattleScreen, "Liquid Ooze damaged " & p.GetDisplayName() & "!", "liquidooze")
            Else
                If Not p.Item Is Nothing Then
                    If p.Item.Name.ToLower() = "big root" And BattleScreen.FieldEffects.CanUseItem(own) = True And BattleScreen.FieldEffects.CanUseOwnItem(own, BattleScreen) = True Then
                        heal = CInt(Math.Ceiling(damage * (80 / 100)))
                    End If
                End If

                Dim healBlock As Integer = BattleScreen.FieldEffects.OppHealBlock
                If own = False Then
                    healBlock = BattleScreen.FieldEffects.OwnHealBlock
                End If
                If healBlock = 0 Then
                    BattleScreen.Battle.GainHP(heal, own, own, BattleScreen, op.GetDisplayName() & " had its energy drained!", "move:drainpunch")
                End If
            End If
        End Sub

    End Class

End Namespace
