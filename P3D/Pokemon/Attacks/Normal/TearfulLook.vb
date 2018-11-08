Namespace BattleSystem.Moves.Normal

    Public Class TearfulLook

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Normal)
            Me.ID = 715
            Me.OriginalPP = 20
            Me.CurrentPP = 20
            Me.MaxPP = 20
            Me.Power = 0
            Me.Accuracy = 0
            Me.Category = Categories.Status
            Me.ContestCategory = ContestCategories.Cute
            Me.Name = "Tearful Look"
            Me.Description = "The user gets teary eyed to make the target lose its combative spirit. This lowers the target's Attack and Sp. Atk stats."
            Me.CriticalChance = 0
            Me.IsHMMove = False
            Me.Target = Targets.OneAdjacentTarget
            Me.Priority = 0
            Me.TimesToAttack = 1
            '#End

            '#SpecialDefinitions
            Me.MakesContact = False
            Me.ProtectAffected = False
            Me.MagicCoatAffected = True
            Me.SnatchAffected = False
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

            Me.AIField1 = AIField.LowerDefense
            Me.AIField2 = AIField.LowerSpDefense
        End Sub

        Public Overrides Sub MoveHits(own As Boolean, BattleScreen As BattleScreen)
            Dim b As Boolean = BattleScreen.Battle.LowerStat(Not own, own, BattleScreen, "Defense", 1, "", "move:tearfullook")
            Dim d As Boolean = BattleScreen.Battle.LowerStat(Not own, own, BattleScreen, "Special Defense", 1, "", "move:tearfullook")
            If b = False AndAlso DisabledWhileGravity = False Then
                BattleScreen.BattleQuery.Add(New TextQueryObject(Me.Name & " failed!"))
            End If
        End Sub

    End Class

End Namespace