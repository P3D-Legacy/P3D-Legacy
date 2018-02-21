Namespace BattleSystem.Moves.Ground

    Public Class Bonemerang

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Ground)
            Me.ID = 155
            Me.OriginalPP = 10
            Me.CurrentPP = 10
            Me.MaxPP = 10
            Me.Power = 50
            Me.Accuracy = 90
            Me.Category = Categories.Physical
            Me.ContestCategory = ContestCategories.Tough
            Me.Name = "Bonemerang"
            Me.Description = "The user throws the bone it holds. The bone loops to hit the target twice, coming and going."
            Me.CriticalChance = 1
            Me.IsHMMove = False
            Me.Target = Targets.OneAdjacentTarget
            Me.Priority = 0
            Me.TimesToAttack = 2
            '#End

            '#SpecialDefinitions
            Me.MakesContact = False
            Me.ProtectAffected = True
            Me.MagicCoatAffected = False
            Me.SnatchAffected = False
            Me.MirrorMoveAffected = True
            Me.KingsrockAffected = False
            Me.CounterAffected = True
            Me.DisabledWhileGravity = False
            Me.UseEffectiveness = True
            Me.IsHealingMove = False
            Me.RemovesFrozen = False
            Me.IsRecoilMove = False
            Me.IsPunchingMove = False
            Me.ImmunityAffected = True
            Me.IsDamagingMove = True
            Me.IsProtectMove = False
            Me.IsSoundMove = False
            Me.HasSecondaryEffect = False
            Me.IsAffectedBySubstitute = True
            Me.IsOneHitKOMove = False
            Me.IsWonderGuardAffected = True
            '#End
        End Sub

    End Class

End Namespace