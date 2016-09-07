Namespace BattleSystem.Moves.Normal

    Public Class PainSplit

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Normal)
            Me.ID = 220
            Me.OriginalPP = 20
            Me.CurrentPP = 20
            Me.MaxPP = 20
            Me.Power = 0
            Me.Accuracy = 0
            Me.Category = Categories.Status
            Me.ContestCategory = ContestCategories.Smart
            Me.Name = "Pain Split"
            Me.Description = "The user adds its HP to the target's HP, then equally shares the combined HP with the target."
            Me.CriticalChance = 0
            Me.IsHMMove = True
            Me.Target = Targets.OneAdjacentTarget
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

            Me.AIField1 = AIField.Support
            Me.AIField2 = AIField.Nothing
        End Sub

        Public Overrides Sub MoveHits(own As Boolean, BattleScreen As BattleScreen)
            Dim p As Pokemon = BattleScreen.OwnPokemon
            Dim op As Pokemon = BattleScreen.OppPokemon
            If own = False Then
                p = BattleScreen.OppPokemon
                op = BattleScreen.OwnPokemon
            End If

            Dim totalHP As Integer = p.HP + op.HP
            Dim newHP As Integer = CInt(Math.Ceiling(totalHP / 2))

            Dim failed As Boolean = True

            If p.HP < newHP Then
                BattleScreen.Battle.GainHP(newHP - p.HP, own, own, BattleScreen, "", "move:painsplit")
                failed = False
            ElseIf p.HP > newHP Then
                BattleScreen.Battle.ReduceHP(p.HP - newHP, own, own, BattleScreen, "", "move:painsplit")
                failed = False
            End If

            If op.HP < newHP Then
                BattleScreen.Battle.GainHP(newHP - op.HP, Not own, own, BattleScreen, "", "move:painsplit")
                failed = False
            ElseIf op.HP > newHP Then
                BattleScreen.Battle.ReduceHP(op.HP - newHP, Not own, own, BattleScreen, "", "move:painsplit")
                failed = False
            End If

            If failed = False Then
                BattleScreen.BattleQuery.Add(New TextQueryObject("The battlers shared their pain."))
            Else
                BattleScreen.BattleQuery.Add(New TextQueryObject(Me.Name & " failed!"))
            End If
        End Sub

    End Class

End Namespace