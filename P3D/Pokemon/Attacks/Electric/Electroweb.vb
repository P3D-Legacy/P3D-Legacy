Namespace BattleSystem.Moves.Electric

    Public Class Electroweb

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Electric)
            Me.ID = 527
            Me.OriginalPP = 15
            Me.CurrentPP = 15
            Me.MaxPP = 15
            Me.Power = 55
            Me.Accuracy = 95
            Me.Category = Categories.Special
            Me.ContestCategory = ContestCategories.Beauty
            Me.Name = Localization.GetString("move_name_" & Me.ID,"Electroweb")
            Me.Description = "The user attacks and captures opposing Pokémon by using an electric net. This lowers their Speed stat."
            Me.CriticalChance = 1
            Me.IsHMMove = False
            Me.Target = Targets.AllAdjacentFoes
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
            Me.RemovesFrozen = False
            Me.HasSecondaryEffect = True

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
            Me.AIField2 = AIField.LowerSpeed
        End Sub

        Public Overrides Sub MoveHits(own As Boolean, BattleScreen As BattleScreen)
            BattleScreen.Battle.LowerStat(Not own, own, BattleScreen, "Speed", 1, "", "move:electroweb")
        End Sub

    End Class

End Namespace