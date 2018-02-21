Namespace BattleSystem.Moves.Water

    Public Class WaterPulse

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Water)
            Me.ID = 352
            Me.OriginalPP = 20
            Me.CurrentPP = 20
            Me.MaxPP = 20
            Me.Power = 60
            Me.Accuracy = 100
            Me.Category = Categories.Special
            Me.ContestCategory = ContestCategories.Beauty
            Me.Name = "Water Pulse"
            Me.Description = "The user attacks the target with a pulsing blast of water. It may also confuse the target."
            Me.CriticalChance = 1
            Me.IsHMMove = False
            Me.Target = Targets.OneTarget
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
            Me.CounterAffected = False

            Me.DisabledWhileGravity = False
            Me.UseEffectiveness = True
            Me.ImmunityAffected = True
            Me.HasSecondaryEffect = True
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
            Me.IsPulseMove = True
            '#End

            Me.AIField1 = AIField.Damage
            Me.AIField2 = AIField.CanConfuse

            Me.EffectChances.Add(20)
        End Sub

        Public Overrides Sub MoveHits(own As Boolean, BattleScreen As BattleScreen)
            If Core.Random.Next(0, 100) < Me.GetEffectChance(0, own, BattleScreen) Then
                BattleScreen.Battle.InflictConfusion(Not own, own, BattleScreen, "", "move:waterpulse")
            End If
        End Sub

    End Class

End Namespace