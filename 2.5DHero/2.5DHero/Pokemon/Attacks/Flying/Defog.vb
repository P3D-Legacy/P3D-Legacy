Namespace BattleSystem.Moves.Flying

    Public Class Defog

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Flying)
            Me.ID = 432
            Me.OriginalPP = 15
            Me.CurrentPP = 15
            Me.MaxPP = 15
            Me.Power = 0
            Me.Accuracy = 0
            Me.Category = Categories.Status
            Me.ContestCategory = ContestCategories.Beauty
            Me.Name = "Defog"
            Me.Description = "A strong wind blows away the target's barriers such as Reflect or Light Screen. This also lowers the target's evasiveness."
            Me.CriticalChance = 1
            Me.IsHMMove = False
            Me.Target = Targets.OneAdjacentTarget
            Me.Priority = 0
            Me.TimesToAttack = 1
            '#End

            '#SpecialDefinitions
            Me.MakesContact = False
            Me.ProtectAffected = True
            Me.MagicCoatAffected = True
            Me.SnatchAffected = True
            Me.MirrorMoveAffected = True
            Me.KingsrockAffected = False
            Me.CounterAffected = False

            Me.DisabledWhileGravity = False
            Me.UseEffectiveness = False
            Me.ImmunityAffected = False
            Me.HasSecondaryEffect = False
            Me.RemovesFrozen = False

            Me.IsHealingMove = False
            Me.IsRecoilMove = False
            Me.IsPunchingMove = False
            Me.IsDamagingMove = False
            Me.IsProtectMove = False
            Me.IsSoundMove = False

            Me.IsAffectedBySubstitute = True
            Me.IsOneHitKOMove = False
            Me.IsWonderGuardAffected = False
            '#End
        End Sub

        Public Overrides Sub MoveHits(own As Boolean, BattleScreen As BattleScreen)
            If BattleScreen.Battle.LowerStat(Not own, own, BattleScreen, "Evasion", 1, "", "move:defog") = False Then
                BattleScreen.BattleQuery.Add(New TextQueryObject(Me.Name & " failed!"))
            End If
            With BattleScreen.FieldEffects
                .OppSafeguard = 0
                .OppMist = 0
                .OppLightScreen = 0
                .OppReflect = 0
                .OppSpikes = 0
                .OppStealthRock = 0
                .OppToxicSpikes = 0
                .OwnSafeguard = 0
                .OwnLightScreen = 0
                .OwnMist = 0
                .OwnReflect = 0
                .OwnSpikes = 0
                .OwnStealthRock = 0
                .OwnToxicSpikes = 0
            End With
        End Sub

    End Class

End Namespace
