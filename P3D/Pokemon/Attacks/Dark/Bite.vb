Namespace BattleSystem.Moves.Dark

    Public Class Bite

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Dark)
            Me.ID = 44
            Me.OriginalPP = 25
            Me.CurrentPP = 25
            Me.MaxPP = 25
            Me.Power = 60
            Me.Accuracy = 100
            Me.Category = Categories.Physical
            Me.ContestCategory = ContestCategories.Tough
            Me.Name = "Bite"
            Me.Description = "The target is bitten with viciously sharp fangs. It may make the target flinch."
            Me.CriticalChance = 1
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
            Me.KingsrockAffected = False
            Me.CounterAffected = True

            Me.DisabledWhileGravity = False
            Me.UseEffectiveness = True
            Me.ImmunityAffected = True
            Me.HasSecondaryEffect = True
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
            Me.IsJawMove = True
            '#End

            Me.AIField1 = AIField.Damage
            Me.AIField2 = AIField.CanFlinch

            Me.EffectChances.Add(30)
        End Sub

        Public Overrides Sub MoveHits(own As Boolean, BattleScreen As BattleScreen)
            Dim substitute As Integer = BattleScreen.FieldEffects.OppSubstitute
            If own = False Then
                substitute = BattleScreen.FieldEffects.OwnSubstitute
            End If

            If substitute = 0 Then
                Dim chance As Integer = GetEffectChance(0, own, BattleScreen)

                If Core.Random.Next(0, 100) < chance Then
                    BattleScreen.Battle.InflictFlinch(Not own, own, BattleScreen, "", "move:bite")
                End If
            End If
        End Sub

    End Class

End Namespace