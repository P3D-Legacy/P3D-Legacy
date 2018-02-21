Namespace BattleSystem.Moves.Normal

    Public Class Present

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Normal)
            Me.ID = 217
            Me.OriginalPP = 15
            Me.CurrentPP = 15
            Me.MaxPP = 15
            Me.Power = 0
            Me.Accuracy = 90
            Me.Category = Categories.Physical
            Me.ContestCategory = ContestCategories.Cute
            Me.Name = "Present"
            Me.Description = "The user attacks by giving the target a gift with a hidden trap. It restores HP sometimes, however."
            Me.CriticalChance = 1
            Me.IsHMMove = False
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
            Me.CounterAffected = True

            Me.DisabledWhileGravity = False
            Me.UseEffectiveness = True
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
            Me.IsOneHitKOMove = False
            Me.IsWonderGuardAffected = True
            '#End
        End Sub

        Public Overrides Function MoveFailBeforeAttack(Own As Boolean, BattleScreen As BattleScreen) As Boolean
            If Core.Random.Next(0, 100) < 80 Then
                Return False
            Else
                Dim op As Pokemon = BattleScreen.OppPokemon
                If Own = False Then
                    op = BattleScreen.OwnPokemon
                End If

                If op.HP < op.MaxHP And op.HP > 0 Then
                    BattleScreen.Battle.GainHP(CInt(Math.Ceiling(op.MaxHP / 4)), Not Own, Own, BattleScreen, op.GetDisplayName() & " had its HP restored!", "move:present")
                End If

                Return True
            End If
        End Function

        Public Overrides Function GetBasePower(own As Boolean, BattleScreen As BattleScreen) As Integer
            Dim r As Integer = Core.Random.Next(0, 80)
            If r < 40 Then
                Return 40
            ElseIf r >= 40 And r < 70 Then
                Return 80
            ElseIf r >= 70 Then
                Return 120
            End If

            Return 40
        End Function

    End Class

End Namespace