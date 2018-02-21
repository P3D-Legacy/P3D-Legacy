Namespace BattleSystem.Moves.Electric

    Public Class Flash

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Electric)
            Me.ID = 148
            Me.OriginalPP = 15
            Me.CurrentPP = 15
            Me.MaxPP = 15
            Me.Power = 60
            Me.Accuracy = 100
            Me.Category = Categories.Special
            Me.ContestCategory = ContestCategories.Beauty
            Me.Name = "Flash"
            Me.Description = "The user flashes a bright light with enormous speed at the target. It cuts the user's accuracy though."
            Me.CriticalChance = 1
            Me.IsHMMove = True
            Me.Target = Targets.OneAdjacentTarget
            Me.Priority = 1
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
            Me.IsHealingMove = False
            Me.RemovesFrozen = False
            Me.IsRecoilMove = False
            Me.IsPunchingMove = False
            Me.ImmunityAffected = True
            Me.IsDamagingMove = True
            Me.IsProtectMove = False
            Me.IsSoundMove = False
            Me.HasSecondaryEffect = True
            Me.IsAffectedBySubstitute = True
            Me.IsOneHitKOMove = False
            Me.IsWonderGuardAffected = True
            Me.CanHitSleeping = False
            '#End

            Me.AIField1 = AIField.Damage
            Me.AIField2 = AIField.HighPriority
        End Sub

        Public Overrides Sub MoveHits(own As Boolean, BattleScreen As BattleScreen)
            BattleScreen.Battle.LowerStat(own, own, BattleScreen, "Accuracy", 1, "", "move:flash")
        End Sub

    End Class

End Namespace