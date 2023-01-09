﻿Namespace BattleSystem.Moves.Steel

    Public Class MetalBurst

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Steel)
            Me.ID = 368
            Me.OriginalPP = 10
            Me.CurrentPP = 10
            Me.MaxPP = 10
            Me.Power = 0
            Me.Accuracy = 100
            Me.Category = Categories.Physical
            Me.ContestCategory = ContestCategories.Cool
            Me.Name = Localization.GetString("move_name_" & Me.ID,"Metal Burst")
            Me.Description = "The user retaliates with much greater power against the target that last inflicted damage on it."
            Me.CriticalChance = 1
            Me.IsHMMove = False
            Me.Target = Targets.Self
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

            Me.IsDamagingMove = True
            Me.IsProtectMove = False


            Me.IsAffectedBySubstitute = True
            Me.IsOneHitKOMove = False
            Me.IsWonderGuardAffected = True
            '#End

            Me.AIField1 = AIField.Damage
            Me.AIField2 = AIField.Nothing
        End Sub

        Public Overrides Function MoveFailBeforeAttack(Own As Boolean, BattleScreen As BattleScreen) As Boolean
            If BattleScreen.FieldEffects.MovesFirst(Own) Then
                Return True
            End If
            Dim damage As Integer = BattleScreen.FieldEffects.OwnLastDamage
            If Own = True Then
                damage = BattleScreen.FieldEffects.OppLastDamage
            End If
            If damage > 0 Then
                Dim lastMove As Attack = BattleScreen.FieldEffects.OwnLastMove
                If Own = True Then
                    lastMove = BattleScreen.FieldEffects.OppLastMove
                End If
                If Not lastMove Is Nothing Then
                    If lastMove.Category = Categories.Special Or lastMove.Category = Categories.Physical Then
                        Return False
                    End If
                End If
            End If
            Return True
        End Function

        Public Overrides Function GetDamage(Critical As Boolean, Own As Boolean, targetPokemon As Boolean, BattleScreen As BattleScreen, Optional ExtraParameter As String = "") As Integer
            If Own = True Then
                Return CInt(BattleScreen.FieldEffects.OppLastDamage * 1.5)
            Else
                Return CInt(BattleScreen.FieldEffects.OwnLastDamage * 1.5)
            End If
        End Function

    End Class

End Namespace