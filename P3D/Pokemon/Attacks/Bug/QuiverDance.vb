Namespace BattleSystem.Moves.Bug

    Public Class QuiverDance

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Bug)
            Me.ID = 483
            Me.OriginalPP = 20
            Me.CurrentPP = 20
            Me.MaxPP = 20
            Me.Power = 0
            Me.Accuracy = 0
            Me.Category = Categories.Status
            Me.ContestCategory = ContestCategories.Beauty
            Me.Name = Localization.GetString("move_name_" & Me.ID,"Quiver Dance")
            Me.Description = "The user lightly performs a beautiful, mystic dance. This boosts the user's Sp. Atk, Sp. Def, and Speed stats."
            Me.CriticalChance = 0
            Me.IsHMMove = False
            Me.Target = Targets.self
            Me.Priority = 0
            Me.TimesToAttack = 1
            '#End

            '#SpecialDefinitions
            Me.MakesContact = False
            Me.ProtectAffected = False
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
            Me.IsSoundMove = False

            Me.IsAffectedBySubstitute = False
            Me.IsOneHitKOMove = False
            Me.IsWonderGuardAffected = False
            Me.UseAccEvasion = False
            '#End

            Me.AIField1 = AIField.RaiseSpAttack
            Me.AIField2 = AIField.RaiseSpDefense
        End Sub

        Public Overrides Sub MoveHits(own As Boolean, BattleScreen As BattleScreen)
            Dim b As Boolean = BattleScreen.Battle.RaiseStat(own, own, BattleScreen, "Special Attack", 1, "", "move:quiverdance")
            Dim c As Boolean = BattleScreen.Battle.RaiseStat(own, own, BattleScreen, "Special Defense", 1, "", "move:quiverdance")
            Dim d As Boolean = BattleScreen.Battle.RaiseStat(own, own, BattleScreen, "Speed", 1, "", "move:quiverdance")
            If b = False And c = False And d = False Then
                BattleScreen.BattleQuery.Add(New TextQueryObject(Me.Name & " failed!"))
            End If
        End Sub

    End Class

End Namespace