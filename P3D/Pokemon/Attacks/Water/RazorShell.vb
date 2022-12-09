Namespace BattleSystem.Moves.Water

    Public Class RazorShell

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Water)
            Me.ID = 534
            Me.OriginalPP = 10
            Me.CurrentPP = 10
            Me.MaxPP = 10
            Me.Power = 75
            Me.Accuracy = 95
            Me.Category = Categories.Physical
            Me.ContestCategory = ContestCategories.Tough
            Me.Name = Localization.GetString("move_name_" & Me.ID,"Razor Shell")
            Me.Description = "The user cuts its target with sharp shells. This may also lower the target's Defense stat."
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
            '#End

            Me.AIField1 = AIField.Damage
            Me.AIField2 = AIField.CanLowerDefense

            Me.EffectChances.Add(50)
        End Sub

        Public Overrides Sub MoveHits(own As Boolean, BattleScreen As BattleScreen)
            Dim chance As Integer = GetEffectChance(0, own, BattleScreen)

            If Core.Random.Next(0, 100) < chance Then
                BattleScreen.Battle.LowerStat(Not own, own, BattleScreen, "Defense", 1, "", "move:razorshell")
            End If
        End Sub

    End Class

End Namespace