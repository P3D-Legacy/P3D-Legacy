Namespace BattleSystem.Moves.Normal

    Public Class Guillotine

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Normal)
            Me.ID = 12
            Me.OriginalPP = 5
            Me.CurrentPP = 5
            Me.MaxPP = 5
            Me.Power = 0
            Me.Accuracy = 0
            Me.Category = Categories.Physical
            Me.ContestCategory = ContestCategories.Cool
            Me.Name = "Guillotine"
            Me.Description = "A vicious, tearing attack with big pincers. The target will faint instantly if this attack hits."
            Me.CriticalChance = 0
            Me.IsHMMove = False
            Me.Target = Targets.OneAdjacentTarget
            Me.Priority = 0
            Me.TimesToAttack = 1
            '#End

            '#SpecialDefinitions
            Me.MakesContact = True
            Me.ProtectAffected = True
            Me.MagicCoatAffected = False
            Me.SnatchAffected = False
            Me.MirrorMoveAffected = True
            Me.KingsrockAffected = True
            Me.CounterAffected = True

            Me.DisabledWhileGravity = False
            Me.UseEffectiveness = False
            Me.ImmunityAffected = True
            Me.HasSecondaryEffect = False
            Me.RemovesFrozen = False

            Me.IsHealingMove = False
            Me.IsRecoilMove = False
            Me.IsPunchingMove = False
            Me.IsDamagingMove = True
            Me.IsProtectMove = False
            Me.IsSoundMove = False

            Me.IsAffectedBySubstitute = True
            Me.IsOneHitKOMove = True
            Me.IsWonderGuardAffected = False
            Me.UseAccEvasion = False
            '#End

            Me.AIField1 = AIField.Damage
            Me.AIField2 = AIField.OHKO
        End Sub

        Public Overrides Function GetAccuracy(own As Boolean, BattleScreen As BattleScreen) As Integer
            Dim p As Pokemon = BattleScreen.OwnPokemon
            Dim op As Pokemon = BattleScreen.OppPokemon
            If own = False Then
                p = BattleScreen.OppPokemon
                op = BattleScreen.OwnPokemon
            End If

            Dim acc As Integer = ((p.Level - op.Level) + 30)
            Return acc
        End Function

        Public Overrides Function MoveFailBeforeAttack(Own As Boolean, BattleScreen As BattleScreen) As Boolean
            Dim p As Pokemon = BattleScreen.OwnPokemon
            Dim op As Pokemon = BattleScreen.OppPokemon
            If Own = False Then
                p = BattleScreen.OppPokemon
                op = BattleScreen.OwnPokemon
            End If

            If op.Level > p.Level Then
                BattleScreen.BattleQuery.Add(New TextQueryObject(Me.Name & " failed!"))
                Return True
            Else
                Return False
            End If
        End Function

    End Class

End Namespace