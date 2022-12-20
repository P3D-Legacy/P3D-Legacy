Namespace BattleSystem.Moves.Normal

    Public Class Round

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Normal)
            Me.ID = 496
            Me.OriginalPP = 15
            Me.CurrentPP = 15
            Me.MaxPP = 15
            Me.Power = 60
            Me.Accuracy = 100
            Me.Category = Categories.Special
            Me.ContestCategory = ContestCategories.Beauty
            Me.Name = Localization.GetString("move_name_" & Me.ID,"Round")
            Me.Description = "The user attacks the target with a song. Others can join in the Round to increase the power of the attack."
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
            Me.KingsrockAffected = True
            Me.CounterAffected = False

            Me.DisabledWhileGravity = False
            Me.UseEffectiveness = True
            Me.ImmunityAffected = True
            Me.HasSecondaryEffect = False
            Me.RemovesFrozen = False

            Me.IsHealingMove = False
            Me.IsRecoilMove = False

            Me.IsDamagingMove = True
            Me.IsProtectMove = False
            Me.IsSoundMove = True

            Me.IsAffectedBySubstitute = False
            Me.IsOneHitKOMove = False
            Me.IsWonderGuardAffected = True
            '#End
        End Sub

        Public Overrides Function GetBasePower(own As Boolean, BattleScreen As BattleScreen) As Integer
            If own = True Then
                If BattleScreen.FieldEffects.OppTurnCounts > BattleScreen.FieldEffects.OwnTurnCounts Then
                    If Not BattleScreen.FieldEffects.OppLastMove Is Nothing Then
                        If BattleScreen.FieldEffects.OppLastMove.ID = 496 Then
                            Return Me.Power * 2
                        End If
                    End If
                End If
            Else
                If BattleScreen.FieldEffects.OwnTurnCounts > BattleScreen.FieldEffects.OppTurnCounts Then
                    If Not BattleScreen.FieldEffects.OwnLastMove Is Nothing Then
                        If BattleScreen.FieldEffects.OwnLastMove.ID = 496 Then
                            Return Me.Power * 2
                        End If
                    End If
                End If
            End If
            Return Me.Power
        End Function

    End Class

End Namespace