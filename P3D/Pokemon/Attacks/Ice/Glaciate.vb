Namespace BattleSystem.Moves.Ice

    Public Class Glaciate

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Ice)
            Me.ID = 549
            Me.OriginalPP = 10
            Me.CurrentPP = 10
            Me.MaxPP = 10
            Me.Power = 65
            Me.Accuracy = 95
            Me.Category = Categories.Special
            Me.ContestCategory = ContestCategories.Beauty
            Me.Name = Localization.GetString("move_name_" & Me.ID,"Glaciate")
            Me.Description = "The user attacks by blowing freezing cold air at opposing Pokémon. This lowers their Speed stat."
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
            Me.IsWindMove = True 'P3D only
            Me.IsDamagingMove = True
            Me.IsProtectMove = False

            Me.IsAffectedBySubstitute = True
            Me.IsOneHitKOMove = False
            Me.IsWonderGuardAffected = True
            '#End

            Me.AIField1 = AIField.Damage
            Me.AIField2 = AIField.LowerSpeed
        End Sub

        Public Overrides Sub MoveHits(own As Boolean, BattleScreen As BattleScreen)
            BattleScreen.Battle.LowerStat(Not own, own, BattleScreen, "Speed", 1, "", "move:glaciate")
        End Sub

    End Class

End Namespace