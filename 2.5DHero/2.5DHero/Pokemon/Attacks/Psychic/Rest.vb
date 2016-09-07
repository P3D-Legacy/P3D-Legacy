Namespace BattleSystem.Moves.Psychic

    Public Class Rest

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Psychic)
            Me.ID = 156
            Me.OriginalPP = 10
            Me.CurrentPP = 10
            Me.MaxPP = 10
            Me.Power = 0
            Me.Accuracy = 0
            Me.Category = Categories.Status
            Me.ContestCategory = ContestCategories.Cute
            Me.Name = "Rest"
            Me.Description = "The user goes to sleep for two turns. It fully restores the user’s HP and heals any status problem."
            Me.CriticalChance = 0
            Me.IsHMMove = False
            Me.Target = Targets.Self
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
            Me.RemovesFrozen = False
            Me.HasSecondaryEffect = True

            Me.IsHealingMove = True
            Me.IsRecoilMove = False
            Me.IsPunchingMove = False
            Me.IsDamagingMove = False
            Me.IsProtectMove = False
            Me.IsSoundMove = False

            Me.IsAffectedBySubstitute = False
            Me.IsOneHitKOMove = False
            Me.IsWonderGuardAffected = False
            '#End

            Me.AIField1 = AIField.Healing
            Me.AIField2 = AIField.Nothing
        End Sub

        Public Overrides Sub MoveHits(own As Boolean, BattleScreen As BattleScreen)
            Dim fails As Boolean = False
            Dim p As Pokemon = BattleScreen.OwnPokemon
            If own = False Then
                p = BattleScreen.OppPokemon
            End If
            Dim status As Pokemon.StatusProblems = p.Status

            Dim healBlock As Integer = BattleScreen.FieldEffects.OwnHealBlock
            If own = False Then
                healBlock = BattleScreen.FieldEffects.OppHealBlock
            End If

            If healBlock > 0 Then
                fails = True
            Else
                If p.HP >= p.MaxHP Then
                    fails = True
                Else
                    If BattleScreen.Battle.InflictSleep(own, own, BattleScreen, 3, "", "move:rest") = False Then
                        fails = True
                    End If
                End If
            End If

            If fails = True Then
                p.Status = status
                BattleScreen.BattleQuery.Add(New TextQueryObject(Me.Name & " failed!"))
            Else
                BattleScreen.BattleQuery.Add(New TextQueryObject(p.GetDisplayName() & " slept and became healthy."))
                BattleScreen.Battle.GainHP(p.MaxHP - p.HP, own, own, BattleScreen, "", "move:rest")
            End If
        End Sub

    End Class

End Namespace