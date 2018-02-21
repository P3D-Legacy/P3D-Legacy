Namespace BattleSystem.Moves.Normal

    Public Class NobleRoar

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Normal)
            Me.ID = 568
            Me.OriginalPP = 40
            Me.CurrentPP = 40
            Me.MaxPP = 40
            Me.Power = 0
            Me.Accuracy = 100
            Me.Category = Categories.Status
            Me.ContestCategory = ContestCategories.Cute
            Me.Name = "Noble Roar"
            Me.Description = "Letting out a noble roar, the user intimidates the target and lowers its Attack and Sp. Atk stats."
            Me.CriticalChance = 0
            Me.IsHMMove = False
            Me.Target = Targets.AllAdjacentFoes
            Me.Priority = 0
            Me.TimesToAttack = 1
            '#End

            '#SpecialDefinitions
            Me.MakesContact = False
            Me.ProtectAffected = True
            Me.MagicCoatAffected = True
            Me.SnatchAffected = False
            Me.MirrorMoveAffected = True
            Me.KingsrockAffected = True
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
            Me.IsSoundMove = True

            Me.IsAffectedBySubstitute = True
            Me.IsOneHitKOMove = False
            Me.IsWonderGuardAffected = False
            '#End

            Me.AIField1 = AIField.LowerAttack
            Me.AIField2 = AIField.LowerSpAttack
        End Sub

        Public Overrides Sub MoveHits(own As Boolean, BattleScreen As BattleScreen)
            Dim b As Boolean = BattleScreen.Battle.LowerStat(Not own, own, BattleScreen, "Attack", 1, "", "move:nobleroar")
            Dim d As Boolean = BattleScreen.Battle.LowerStat(Not own, own, BattleScreen, "Special Attack", 1, "", "move:nobleroar")
            If b = False Or d = False Then
                BattleScreen.BattleQuery.Add(New TextQueryObject(Me.Name & " failed!"))
            End If
        End Sub

    End Class

End Namespace