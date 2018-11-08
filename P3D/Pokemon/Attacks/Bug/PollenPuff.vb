Namespace BattleSystem.Moves.Bug

    Public Class PollenPuff

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Bug)
            Me.ID = 676
            Me.OriginalPP = 15
            Me.CurrentPP = 15
            Me.MaxPP = 15
            Me.Power = 90
            Me.Accuracy = 100
            Me.Category = Categories.Special
            Me.ContestCategory = ContestCategories.Beauty
            Me.Name = "Pollen Puff"
            Me.Description = "The user attacks the enemy with a pollen puff that explodes. If the target is an ally, it gives the ally a pollen puff that restores its HP instead."
            Me.CriticalChance = 1
            Me.IsHMMove = False
            Me.Target = Targets.OneAdjacentFoe
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
            Me.RemovesFrozen = False
            Me.HasSecondaryEffect = False

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
        End Sub

    End Class

End Namespace
