Namespace BattleSystem.Moves.Electric

    Public Class ThunderFang

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Electric)
            Me.ID = 422
            Me.OriginalPP = 15
            Me.CurrentPP = 15
            Me.MaxPP = 15
            Me.Power = 65
            Me.Accuracy = 95
            Me.Category = Categories.Physical
            Me.ContestCategory = ContestCategories.Cool
            Me.Name = "Thunder Fang"
            Me.Description = "The user bites with electrified fangs. It may also make the target flinch or leave it with paralysis."
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
            Me.KingsrockAffected = True
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
            Me.AIField2 = AIField.CanParalyse
            Me.AIField3 = AIField.CanFlinch

            EffectChances.Add(10)
            EffectChances.Add(10)
        End Sub

        Public Overrides Sub MoveHits(own As Boolean, BattleScreen As BattleScreen)
            If BattleScreen.FieldEffects.MovesFirst(own) = True Then
                If Core.Random.Next(0, 100) < Me.GetEffectChance(1, own, BattleScreen) Then
                    BattleScreen.Battle.InflictFlinch(Not own, own, BattleScreen, "", "move:thunderfang")
                Else
                    Dim chance As Integer = GetEffectChance(0, own, BattleScreen)
                    If Core.Random.Next(0, 100) < chance Then
                        BattleScreen.Battle.InflictParalysis(Not own, own, BattleScreen, "", "move:thunderfang")
                    End If
                End If
            Else
                Dim chance As Integer = GetEffectChance(0, own, BattleScreen)
                If Core.Random.Next(0, 100) < chance Then
                    BattleScreen.Battle.InflictParalysis(Not own, own, BattleScreen, "", "move:thunderfang")
                End If
            End If
        End Sub

    End Class

End Namespace