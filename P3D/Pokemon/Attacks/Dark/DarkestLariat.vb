Namespace BattleSystem.Moves.Dark

    Public Class DarkestLariat

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Dark)
            Me.ID = 663
            Me.OriginalPP = 10
            Me.CurrentPP = 10
            Me.MaxPP = 10
            Me.Power = 85
            Me.Accuracy = 100
            Me.Category = Categories.Physical
            Me.ContestCategory = ContestCategories.Tough
            Me.Name = "Darkest Lariat"
            Me.Description = "The user swings both arms and hits the target. The target's stat changes don't affect this attack's damage."
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

            Me.IsHealingMove = False
            Me.IsRecoilMove = False
            Me.IsPunchingMove = False
            Me.IsDamagingMove = True
            Me.IsProtectMove = False
            Me.IsSoundMove = False

            Me.IsAffectedBySubstitute = True
            Me.IsOneHitKOMove = False
            Me.IsWonderGuardAffected = True
            Me.IsJawMove = True
            '#End

            Me.AIField1 = AIField.Damage
            Me.AIField2 = AIField.Nothing

        End Sub

    End Class

End Namespace