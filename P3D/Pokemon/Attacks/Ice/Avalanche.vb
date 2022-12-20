Namespace BattleSystem.Moves.Ice

    Public Class Avalanche

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Ice)
            Me.ID = 419
            Me.OriginalPP = 10
            Me.CurrentPP = 10
            Me.MaxPP = 10
            Me.Power = 60
            Me.Accuracy = 100
            Me.Category = Categories.Physical
            Me.ContestCategory = ContestCategories.Cool
            Me.Name = Localization.GetString("move_name_" & Me.ID,"Avalanche")
            Me.Description = "The power of this attack move is doubled if the user has been hurt by the target in the same turn."
            Me.CriticalChance = 1
            Me.IsHMMove = False
            Me.Target = Targets.OneAdjacentTarget
            Me.Priority = -4
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
            Me.RemovesFrozen = False
            Me.HasSecondaryEffect = False

            Me.IsHealingMove = False
            Me.IsRecoilMove = False

            Me.IsDamagingMove = True
            Me.IsProtectMove = False


            Me.IsAffectedBySubstitute = True
            Me.IsOneHitKOMove = False
            Me.IsWonderGuardAffected = True
            '#End
        End Sub

        Public Overrides Function GetBasePower(own As Boolean, BattleScreen As BattleScreen) As Integer
            If own = True Then
                If BattleScreen.FieldEffects.OppTurnCounts > BattleScreen.FieldEffects.OwnTurnCounts Then
                    If Not BattleScreen.FieldEffects.OppLastMove Is Nothing Then
                        If BattleScreen.FieldEffects.OppLastMove.IsDamagingMove = True Then
                            Return Me.Power * 2
                        End If
                    End If
                End If
            Else
                If BattleScreen.FieldEffects.OwnTurnCounts > BattleScreen.FieldEffects.OppTurnCounts Then
                    If Not BattleScreen.FieldEffects.OwnLastMove Is Nothing Then
                        If BattleScreen.FieldEffects.OwnLastMove.IsDamagingMove = True Then
                            Return Me.Power * 2
                        End If
                    End If
                End If
            End If
            Return Me.Power
        End Function

    End Class

End Namespace