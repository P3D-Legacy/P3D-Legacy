﻿Namespace BattleSystem.Moves.Grass

    Public Class PetalDance

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Grass)
            Me.ID = 80
            Me.OriginalPP = 10
            Me.CurrentPP = 10
            Me.MaxPP = 10
            Me.Power = 120
            Me.Accuracy = 100
            Me.Category = Categories.Special
            Me.ContestCategory = ContestCategories.Beauty
            Me.Name = Localization.GetString("move_name_" & Me.ID,"Petal Dance")
            Me.Description = "The user attacks the target by scattering petals for two to three turns. The user then becomes confused."
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

            Me.IsDanceMove = True

            Me.IsAffectedBySubstitute = True
            Me.IsOneHitKOMove = False
            Me.IsWonderGuardAffected = True
            '#End

            Me.AIField1 = AIField.Damage
            Me.AIField2 = AIField.MultiTurn
            Me.AIField3 = AIField.ConfuseOwn
        End Sub

        Public Overrides Sub MoveMultiTurn(own As Boolean, BattleScreen As BattleScreen)
            Dim currentTurns As Integer = BattleScreen.FieldEffects.OwnPetalDance
            If own = False Then
                currentTurns = BattleScreen.FieldEffects.OppPetalDance
            End If

            If currentTurns = 0 Then
                Dim turns As Integer = Core.Random.Next(2, 4)
                If own = True Then
                    BattleScreen.FieldEffects.OwnPetalDance = turns
                Else
                    BattleScreen.FieldEffects.OppPetalDance = turns
                End If
            End If
        End Sub

        Private Sub Interruption(ByVal own As Boolean, ByVal BattleScreen As BattleScreen)
            Dim petalDance As Integer = 0
            Dim p As Pokemon
            If own = True Then
                petalDance = BattleScreen.FieldEffects.OwnPetalDance
                p = BattleScreen.OwnPokemon
            Else
                petalDance = BattleScreen.FieldEffects.OppPetalDance
                p = BattleScreen.OppPokemon
            End If

            If petalDance = 1 Then
                BattleScreen.Battle.InflictConfusion(own, own, BattleScreen, p.GetDisplayName() & "'s PetalDance stopped.", "move:petaldance")
            End If

            If own = True Then
                BattleScreen.FieldEffects.OwnPetalDance = 0
            Else
                BattleScreen.FieldEffects.OppPetalDance = 0
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