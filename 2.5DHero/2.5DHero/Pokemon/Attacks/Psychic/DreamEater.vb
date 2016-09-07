Namespace BattleSystem.Moves.Psychic

    Public Class DreamEater

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Psychic)
            Me.ID = 138
            Me.OriginalPP = 15
            Me.CurrentPP = 15
            Me.MaxPP = 15
            Me.Power = 100
            Me.Accuracy = 100
            Me.Category = Categories.Special
            Me.ContestCategory = ContestCategories.Smart
            Me.Name = "Dream Eater"
            Me.Description = "The user eats the dreams of a sleeping target. It absorbs half the damage caused to heal the user's HP."
            Me.CriticalChance = 1
            Me.IsHMMove = False
            Me.Target = Targets.OneAdjacentTarget
            Me.Priority = 0
            Me.TimesToAttack = 1
            '#End

            '#SpecialDefinitions
            Me.MakesContact = False
            Me.ProtectAffected = True
            Me.MagicCoatAffected = False
            Me.SnatchAffected = False
            Me.MirrorMoveAffected = True
            Me.KingsrockAffected = False
            Me.CounterAffected = False

            Me.DisabledWhileGravity = False
            Me.UseEffectiveness = True
            Me.ImmunityAffected = True
            Me.HasSecondaryEffect = True
            Me.RemovesFrozen = False

            Me.IsHealingMove = True
            Me.IsRecoilMove = False
            Me.IsPunchingMove = False
            Me.IsDamagingMove = True
            Me.IsProtectMove = False
            Me.IsSoundMove = False

            Me.IsAffectedBySubstitute = True
            Me.IsOneHitKOMove = False
            Me.IsWonderGuardAffected = True
            '#End
        End Sub

        Public Overrides Function MoveFailBeforeAttack(Own As Boolean, BattleScreen As BattleScreen) As Boolean
            Dim op As Pokemon = BattleScreen.OppPokemon
            If Own = False Then
                op = BattleScreen.OwnPokemon
            End If

            If op.Status <> Pokemon.StatusProblems.Sleep Then
                BattleScreen.BattleQuery.Add(New TextQueryObject(Me.Name & " failed!"))
                Return True
            Else
                Return False
            End If
        End Function

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
                    BattleScreen.Battle.GainHP(heal, own, own, BattleScreen, op.GetDisplayName() & " had its energy drained!", "move:dreameater")
                End If
            End If
        End Sub

    End Class

End Namespace