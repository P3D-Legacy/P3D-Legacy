Namespace BattleSystem.Moves.Flying

    Public Class Chatter

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Flying)
            Me.ID = 448
            Me.OriginalPP = 20
            Me.CurrentPP = 20
            Me.MaxPP = 20
            Me.Power = 65
            Me.Accuracy = 100
            Me.Category = Categories.Special
            Me.ContestCategory = ContestCategories.Cute
            Me.Name = Localization.GetString("move_name_" & Me.ID,"Chatter")
            Me.Description = "The user attacks using a sound wave based on words it has learned. This confuses the target."
            Me.CriticalChance = 1
            Me.IsHMMove = False
            Me.Target = Targets.OneTarget
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
            Me.CounterAffected = True

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
            Me.IsSoundMove = True

            Me.IsAffectedBySubstitute = True
            Me.IsOneHitKOMove = False
            Me.IsWonderGuardAffected = True
            '#End

            Me.AIField1 = AIField.Damage
            Me.AIField2 = AIField.Confusion
        End Sub

        Public Overrides Sub MoveHits(own As Boolean, BattleScreen As BattleScreen)
            BattleScreen.Battle.InflictConfusion(Not own, own, BattleScreen, "", "move:chatter")
        End Sub

    End Class

End Namespace