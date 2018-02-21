Namespace BattleSystem.Moves.Poison

    Public Class PoisonTail

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Poison)
            Me.ID = 342
            Me.OriginalPP = 25
            Me.CurrentPP = 25
            Me.MaxPP = 25
            Me.Power = 50
            Me.Accuracy = 100
            Me.Category = Categories.Physical
            Me.ContestCategory = ContestCategories.Smart
            Me.Name = "Poison Tail"
            Me.Description = "The user hits the target with its tail. This may also poison the target. Critical hits land more easily."
            Me.CriticalChance = 2
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
            Me.KingsrockAffected = True
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
            Me.AIField2 = AIField.CanPoison

            Me.EffectChances.Add(10)
        End Sub

        Public Overrides Sub MoveHits(own As Boolean, BattleScreen As BattleScreen)
            Dim chance As Integer = GetEffectChance(0, own, BattleScreen)

            If Core.Random.Next(0, 100) < chance Then
                BattleScreen.Battle.InflictPoison(Not own, own, BattleScreen, False, "", "move:poisontail")
            End If
        End Sub

    End Class

End Namespace