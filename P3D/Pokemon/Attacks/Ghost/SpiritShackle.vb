Namespace BattleSystem.Moves.Ghost

    Public Class SpiritShackle

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Ghost)
            Me.ID = 662
            Me.OriginalPP = 10
            Me.CurrentPP = 10
            Me.MaxPP = 10
            Me.Power = 80
            Me.Accuracy = 100
            Me.Category = Categories.Physical
            Me.ContestCategory = ContestCategories.Smart
            Me.Name = "Spirit Shackle"
            Me.Description = "The user attacks while simultaneously stitching the target's shadow to the ground to prevent the target from escaping."
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
            Me.KingsrockAffected = True
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
        End Sub

        Public Overrides Sub MoveHits(own As Boolean, BattleScreen As BattleScreen)
            Dim trapped As Integer = BattleScreen.FieldEffects.OppTrappedCounter
            If own = False Then
                trapped = BattleScreen.FieldEffects.OwnTrappedCounter
            End If

            Dim op As Pokemon = BattleScreen.OppPokemon
            If own = False Then
                op = BattleScreen.OwnPokemon
            End If

            If trapped = 0 Then
                If own = True Then
                    BattleScreen.FieldEffects.OppTrappedCounter = 1
                Else
                    BattleScreen.FieldEffects.OwnTrappedCounter = 1
                End If
                BattleScreen.BattleQuery.Add(New TextQueryObject(op.GetDisplayName() & " can no longer escape!"))
            End If
        End Sub

    End Class

End Namespace