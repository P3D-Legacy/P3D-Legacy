Namespace BattleSystem.Moves.Ground

    Public Class MudBomb

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Ground)
            Me.ID = 426
            Me.OriginalPP = 10
            Me.CurrentPP = 10
            Me.MaxPP = 10
            Me.Power = 65
            Me.Accuracy = 85
            Me.Category = Categories.Special
            Me.ContestCategory = ContestCategories.Smart
            Me.Name = "Mud Bomb"
            Me.Description = "The user launches a hard-packed mud ball to attack. This may also lower the target's accuracy."
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
			
            Me.EffectChances.Add(30)

            Me.AIField1 = AIField.Damage
            Me.AIField2 = AIField.CanLowerAccuracy
        End Sub

        Public Overrides Sub MoveHits(own As Boolean, BattleScreen As BattleScreen)
            Dim chance As Integer = GetEffectChance(0, own, BattleScreen)
            If Core.Random.Next(0, 100) < chance Then
                BattleScreen.Battle.LowerStat(Not own, own, BattleScreen, "Accuracy", 1, "", "move:mudbomb")
            End If
        End Sub

    End Class

End Namespace
