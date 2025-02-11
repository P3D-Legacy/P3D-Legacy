Namespace BattleSystem.Moves.Normal

    Public Class EchoedVoice

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Normal)
            Me.ID = 497
            Me.OriginalPP = 15
            Me.CurrentPP = 15
            Me.MaxPP = 15
            Me.Power = 40
            Me.Accuracy = 100
            Me.Category = Categories.Special
            Me.ContestCategory = ContestCategories.Beauty
            Me.Name = Localization.GetString("move_name_" & Me.ID,"Echoed Voice")
            Me.Description = "The user attacks the target with an echoing voice. If this move is used every turn, its power is increased."
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
            Me.RemovesOwnFrozen = False

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
            Dim echoed As Integer = BattleScreen.FieldEffects.OwnEchoedVoice
            If own = False Then
                echoed = BattleScreen.FieldEffects.OppEchoedVoice
            End If

            Dim p As Integer = Me.Power

            If echoed > 0 Then
                For i = 1 To echoed
                    p += 40
                Next
            End If

            Return p
        End Function

        Public Overrides Sub MoveHits(own As Boolean, BattleScreen As BattleScreen)
            Dim lastMove As Attack = BattleScreen.FieldEffects.OwnLastMove
            If own = False Then
                lastMove = BattleScreen.FieldEffects.OppLastMove
            End If

            If lastMove.ID <> 497 Then
                If own = True Then
                    BattleScreen.FieldEffects.OwnEchoedVoice = 0
                Else
                    BattleScreen.FieldEffects.OppEchoedVoice = 0
                End If
            Else
                Dim echoed As Integer = BattleScreen.FieldEffects.OwnEchoedVoice
                If own = False Then
                    echoed = BattleScreen.FieldEffects.OppEchoedVoice
                End If

                If echoed < 4 Then
                    If own = True Then
                        BattleScreen.FieldEffects.OwnEchoedVoice += 1
                    Else
                        BattleScreen.FieldEffects.OppEchoedVoice += 1
                    End If
                End If
            End If
        End Sub

        Private Sub ResetCounter(ByVal own As Boolean, ByVal BattleScreen As BattleScreen)
            If own = True Then
                BattleScreen.FieldEffects.OwnEchoedVoice = 0
            Else
                BattleScreen.FieldEffects.OppEchoedVoice = 0
            End If
        End Sub

        Public Overrides Sub MoveMisses(own As Boolean, BattleScreen As BattleScreen)
            ResetCounter(own, BattleScreen)
        End Sub

        Public Overrides Sub MoveProtectedDetected(own As Boolean, BattleScreen As BattleScreen)
            ResetCounter(own, BattleScreen)
        End Sub

        Public Overrides Sub MoveHasNoEffect(own As Boolean, BattleScreen As BattleScreen)
            ResetCounter(own, BattleScreen)
        End Sub

        Public Overrides Sub InflictedFlinch(own As Boolean, BattleScreen As BattleScreen)
            ResetCounter(own, BattleScreen)
        End Sub

        Public Overrides Sub IsSleeping(own As Boolean, BattleScreen As BattleScreen)
            ResetCounter(own, BattleScreen)
        End Sub

        Public Overrides Sub HurtItselfInConfusion(own As Boolean, BattleScreen As BattleScreen)
            ResetCounter(own, BattleScreen)
        End Sub

        Public Overrides Sub IsParalyzed(own As Boolean, BattleScreen As BattleScreen)
            ResetCounter(own, BattleScreen)
        End Sub

        Public Overrides Sub IsAttracted(own As Boolean, BattleScreen As BattleScreen)
            ResetCounter(own, BattleScreen)
        End Sub

    End Class

End Namespace