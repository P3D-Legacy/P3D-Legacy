Namespace BattleSystem.Moves.Special

    Public Class TheDerpMove

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Shadow)
            Me.ID = 999
            Me.OriginalPP = 40
            Me.CurrentPP = 40
            Me.MaxPP = 40
            Me.Power = 100
            Me.Accuracy = 0
            Me.Category = Categories.Physical
            Me.ContestCategory = ContestCategories.Cool
            Me.Name = "TheUltimateDerp"
            Me.Description = "The Pokémon derps around to the max. Nothing can stop it now. Not even a cookie."
            Me.CriticalChance = 0
            Me.IsHMMove = False
            Me.Target = Targets.OneTarget
            Me.Priority = 10
            Me.TimesToAttack = 1
            '#End

            '#SpecialDefinitions
            Me.MakesContact = False
            Me.ProtectAffected = False
            Me.MagicCoatAffected = False
            Me.SnatchAffected = False
            Me.MirrorMoveAffected = False
            Me.KingsrockAffected = False
            Me.CounterAffected = False

            Me.DisabledWhileGravity = False
            Me.UseEffectiveness = True
            Me.ImmunityAffected = False
            Me.HasSecondaryEffect = False
            Me.RemovesFrozen = True

            Me.IsHealingMove = False
            Me.IsRecoilMove = False
            Me.IsPunchingMove = False
            Me.IsDamagingMove = True
            Me.IsProtectMove = False
            Me.IsSoundMove = False

            Me.IsAffectedBySubstitute = False
            Me.IsOneHitKOMove = True
            Me.IsWonderGuardAffected = False
            Me.CanHitUnderwater = True
            Me.CanHitSleeping = True
            Me.CanHitUnderground = True
            Me.CanHitInMidAir = True
            Me.UseAccEvasion = False
            '#End
        End Sub

    End Class

End Namespace