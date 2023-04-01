Namespace BattleSystem.Moves.Rock

    Public Class Rollout

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Rock)
            Me.ID = 205
            Me.OriginalPP = 20
            Me.CurrentPP = 20
            Me.MaxPP = 20
            Me.Power = 30
            Me.Accuracy = 90
            Me.Category = Categories.Physical
            Me.ContestCategory = ContestCategories.Cute
            Me.Name = Localization.GetString("move_name_" & Me.ID,"Rollout")
            Me.Description = "The user continually rolls into the target over five turns. It becomes stronger each time it hits."
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
            Me.HasSecondaryEffect = False
            Me.RemovesOwnFrozen = False

            Me.IsHealingMove = False
            Me.IsRecoilMove = False

            Me.IsDamagingMove = True
            Me.IsProtectMove = False


            Me.IsAffectedBySubstitute = True
            Me.IsOneHitKOMove = False
            Me.IsWonderGuardAffected = True
            '#End

            Me.AIField1 = AIField.Damage
            Me.AIField2 = AIField.MultiTurn
        End Sub

        Public Overrides Function GetBasePower(own As Boolean, BattleScreen As BattleScreen) As Integer
            Dim rollout As Integer = BattleScreen.FieldEffects.OwnRolloutCounter
            If own = False Then
                rollout = BattleScreen.FieldEffects.OppRolloutCounter
            End If

            Dim p As Integer = Me.Power

            If rollout > 0 Then
                For i = 1 To rollout
                    p *= 2
                Next
            End If

            Dim defensecurl As Integer = BattleScreen.FieldEffects.OwnDefenseCurl
            If own = False Then
                defensecurl = BattleScreen.FieldEffects.OppDefenseCurl
            End If

            If defensecurl > 0 Then
                p *= 2
            End If

            Return p
        End Function

        Public Overrides Sub MoveMultiTurn(own As Boolean, BattleScreen As BattleScreen)
            Dim rollout As Integer = BattleScreen.FieldEffects.OwnRolloutCounter
            If own = False Then
                rollout = BattleScreen.FieldEffects.OppRolloutCounter
            End If

            If rollout = 5 Then
                If own = True Then
                    BattleScreen.FieldEffects.OwnRolloutCounter = 0
                Else
                    BattleScreen.FieldEffects.OppRolloutCounter = 0
                End If
            Else
                If own = True Then
                    BattleScreen.FieldEffects.OwnRolloutCounter += 1
                Else
                    BattleScreen.FieldEffects.OppRolloutCounter += 1
                End If
            End If
        End Sub
        Public Overrides Function DeductPP(own As Boolean, BattleScreen As BattleScreen) As Boolean
            Dim rollout As Integer = BattleScreen.FieldEffects.OwnRolloutCounter
            If own = False Then
                rollout = BattleScreen.FieldEffects.OppRolloutCounter
            End If

            If rollout > 0 Then
                Return False
            Else
                Return True
            End If
        End Function

        Private Sub Interruption(ByVal own As Boolean, ByVal BattleScreen As BattleScreen)
            If own = True Then
                BattleScreen.FieldEffects.OwnRolloutCounter = 0
            Else
                BattleScreen.FieldEffects.OppRolloutCounter = 0
            End If
        End Sub

        Public Overrides Sub MoveHasNoEffect(own As Boolean, BattleScreen As BattleScreen)
            Interruption(own, BattleScreen)
        End Sub

        Public Overrides Sub MoveProtectedDetected(own As Boolean, BattleScreen As BattleScreen)
            Interruption(own, BattleScreen)
        End Sub

        Public Overrides Sub MoveMisses(own As Boolean, BattleScreen As BattleScreen)
            Interruption(own, BattleScreen)
        End Sub

        Public Overrides Sub InflictedFlinch(own As Boolean, BattleScreen As BattleScreen)
            Interruption(own, BattleScreen)
        End Sub

        Public Overrides Sub IsSleeping(own As Boolean, BattleScreen As BattleScreen)
            Interruption(own, BattleScreen)
        End Sub

        Public Overrides Sub HurtItselfInConfusion(own As Boolean, BattleScreen As BattleScreen)
            Interruption(own, BattleScreen)
        End Sub

        Public Overrides Sub IsAttracted(own As Boolean, BattleScreen As BattleScreen)
            Interruption(own, BattleScreen)
        End Sub
    End Class

End Namespace