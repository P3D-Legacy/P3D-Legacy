Namespace BattleSystem.Moves.Water

    Public Class Octazooka

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Water)
            Me.ID = 190
            Me.OriginalPP = 10
            Me.CurrentPP = 10
            Me.MaxPP = 10
            Me.Power = 65
            Me.Accuracy = 85
            Me.Category = Categories.Special
            Me.ContestCategory = ContestCategories.Tough
            Me.Name = "Octazooka"
            Me.Description = "The user attacks by spraying ink in the target's face or eyes. It may also lower the target's accuracy."
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

            Me.IsHealingMove = False
            Me.IsRecoilMove = False
            Me.IsPunchingMove = False
            Me.IsDamagingMove = True
            Me.IsProtectMove = False
            Me.IsSoundMove = False

            Me.IsAffectedBySubstitute = True
            Me.IsOneHitKOMove = False
            Me.IsWonderGuardAffected = True
            Me.IsBulletMove = True
            '#End

            Me.AIField1 = AIField.Damage
            Me.AIField2 = AIField.CanLowerAccuracy

            Me.EffectChances.Add(50)
        End Sub

        Public Overrides Sub MoveHits(own As Boolean, BattleScreen As BattleScreen)
            If Core.Random.Next(0, 100) < Me.GetEffectChance(0, own, BattleScreen) Then
                BattleScreen.Battle.LowerStat(Not own, own, BattleScreen, "Accuracy", 1, "", "move:octazooka")
            End If
        End Sub

    End Class

End Namespace