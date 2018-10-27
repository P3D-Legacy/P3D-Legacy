Namespace BattleSystem.Moves.Fairy

    Public Class Geomancy

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Fairy)
            Me.ID = 601
            Me.OriginalPP = 10
            Me.CurrentPP = 10
            Me.MaxPP = 10
            Me.Power = 0
            Me.Accuracy = 0
            Me.Category = Categories.Status
            Me.ContestCategory = ContestCategories.Beauty
            Me.Name = "Geomancy"
            Me.Description = "The user absorbs energy and sharply raises its Sp. Atk, Sp. Def, and Speed stats on the next turn."
            Me.CriticalChance = 0
            Me.IsHMMove = False
            Me.Target = Targets.Self
            Me.Priority = 0
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
            Me.UseEffectiveness = False
            Me.ImmunityAffected = False
            Me.RemovesFrozen = False
            Me.HasSecondaryEffect = False

            Me.IsHealingMove = False
            Me.IsRecoilMove = False
            Me.IsPunchingMove = False
            Me.IsDamagingMove = False
            Me.IsProtectMove = False
            Me.IsSoundMove = False

            Me.IsAffectedBySubstitute = False
            Me.IsOneHitKOMove = False
            Me.IsWonderGuardAffected = False
            '#End

            Me.AIField1 = AIField.CanRaiseSpAttack
            Me.AIField2 = AIField.CanRaiseSpDefense
            Me.AIField3 = AIField.CanRaiseSpeed
        End Sub

        Public Overrides Sub MoveHits(own As Boolean, BattleScreen As BattleScreen)
            Dim b As Boolean = BattleScreen.Battle.RaiseStat(own, own, BattleScreen, "Special Attack", 2, "", "move:geomancy")
            Dim d As Boolean = BattleScreen.Battle.RaiseStat(own, own, BattleScreen, "Special Defense", 2, "", "move:geomancy")
            Dim f As Boolean = BattleScreen.Battle.RaiseStat(own, own, BattleScreen, "Speed", 2, "", "move:geomancy")
            If b = False And d = False And f = False Then
                BattleScreen.BattleQuery.Add(New TextQueryObject(Me.Name & " failed!"))
            End If
        End Sub

    End Class

End Namespace