Namespace BattleSystem.Moves.Normal

    Public Class Uproar

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Normal)
            Me.ID = 253
            Me.OriginalPP = 10
            Me.CurrentPP = 10
            Me.MaxPP = 10
            Me.Power = 90
            Me.Accuracy = 100
            Me.Category = Categories.Special
            Me.ContestCategory = ContestCategories.Cute
            Me.Name = "Uproar"
            Me.Description = "The user attacks in an uproar for three turns. Over that time, no one can fall asleep."
            Me.CriticalChance = 1
            Me.IsHMMove = False
            Me.Target = Targets.AllAdjacentFoes
            Me.Priority = 0
            Me.TimesToAttack = 1
            '#End

            '#SpecialDefinitions
            Me.MakesContact = False
            Me.ProtectAffected = True
            Me.MagicCoatAffected = False
            Me.SnatchAffected = False
            Me.MirrorMoveAffected = True
            Me.KingsrockAffected = True
            Me.CounterAffected = True

            Me.DisabledWhileGravity = False
            Me.UseEffectiveness = True
            Me.ImmunityAffected = True
            Me.RemovesFrozen = False
            Me.HasSecondaryEffect = False

            Me.IsHealingMove = False
            Me.IsRecoilMove = False
            Me.IsPunchingMove = False
            Me.IsDamagingMove = True
            Me.IsProtectMove = False
            Me.IsSoundMove = True

            Me.IsAffectedBySubstitute = True
            Me.IsOneHitKOMove = False
            Me.IsWonderGuardAffected = True
            '#End
        End Sub

        Public Overrides Sub MoveHits(own As Boolean, BattleScreen As BattleScreen)
            If own = True Then
                If BattleScreen.FieldEffects.OwnUproar = 0 Then
                    BattleScreen.FieldEffects.OwnUproar = 3
                End If
            Else
                If BattleScreen.FieldEffects.OppUproar = 0 Then
                    BattleScreen.FieldEffects.OppUproar = 3
                End If
            End If
        End Sub

        Private Sub StopMove(own As Boolean, BattleScreen As BattleScreen)
            Dim p As Pokemon = BattleScreen.OwnPokemon
            If own = False Then
                p = BattleScreen.OppPokemon
            End If

            If own = True Then
                BattleScreen.FieldEffects.OwnUproar = 0
            Else
                BattleScreen.FieldEffects.OppUproar = 0
            End If
            BattleScreen.BattleQuery.Add(New TextQueryObject(p.GetDisplayName() & "'s uproar stopped."))
        End Sub

        Public Overrides Sub MoveHasNoEffect(own As Boolean, BattleScreen As BattleScreen)
            Me.StopMove(own, BattleScreen)
        End Sub

        Public Overrides Sub MoveFailsSoundproof(own As Boolean, BattleScreen As BattleScreen)
            Me.StopMove(own, BattleScreen)
        End Sub

    End Class

End Namespace