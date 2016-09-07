Namespace BattleSystem.Moves.Bug

    Public Class FuryCutter

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Bug)
            Me.ID = 210
            Me.OriginalPP = 20
            Me.CurrentPP = 20
            Me.MaxPP = 20
            Me.Power = 40
            Me.Accuracy = 95
            Me.Category = Categories.Physical
            Me.ContestCategory = ContestCategories.Cool
            Me.Name = "Fury Cutter"
            Me.Description = "The target is slashed with scythes or claws. Its power increases if it hits in succession."
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

        Public Overrides Function GetBasePower(own As Boolean, BattleScreen As BattleScreen) As Integer
            Dim fury As Integer = BattleScreen.FieldEffects.OwnFuryCutter
            If own = False Then
                fury = BattleScreen.FieldEffects.OppFuryCutter
            End If

            Dim p As Integer = Me.Power

            If fury > 0 Then
                For i = 1 To fury
                    p *= 2
                Next
            End If

            Return p
        End Function

        Public Overrides Sub MoveHits(own As Boolean, BattleScreen As BattleScreen)
            Dim lastMove As Attack = BattleScreen.FieldEffects.OwnLastMove
            If own = False Then
                lastMove = BattleScreen.FieldEffects.OppLastMove
            End If

            If lastMove.ID <> 210 Then
                If own = True Then
                    BattleScreen.FieldEffects.OwnFuryCutter = 0
                Else
                    BattleScreen.FieldEffects.OppFuryCutter = 0
                End If
            Else
                Dim fury As Integer = BattleScreen.FieldEffects.OwnFuryCutter
                If own = False Then
                    fury = BattleScreen.FieldEffects.OppFuryCutter
                End If

                If fury < 4 Then
                    If own = True Then
                        BattleScreen.FieldEffects.OwnFuryCutter += 1
                    Else
                        BattleScreen.FieldEffects.OppFuryCutter += 1
                    End If
                End If
            End If
        End Sub

        Private Sub ResetCounter(ByVal own As Boolean, ByVal BattleScreen As BattleScreen)
            If own = True Then
                BattleScreen.FieldEffects.OwnFuryCutter = 0
            Else
                BattleScreen.FieldEffects.OppFuryCutter = 0
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

    End Class

End Namespace