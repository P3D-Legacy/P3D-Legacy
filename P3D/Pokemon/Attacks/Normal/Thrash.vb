Namespace BattleSystem.Moves.Normal

    Public Class Thrash

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Normal)
            Me.ID = 37
            Me.OriginalPP = 10
            Me.CurrentPP = 10
            Me.MaxPP = 10
            Me.Power = 120
            Me.Accuracy = 100
            Me.Category = Categories.Physical
            Me.ContestCategory = ContestCategories.Tough
            Me.Name = Localization.GetString("move_name_" & Me.ID,"Thrash")
            Me.Description = "The user rampages and attacks for two to three turns. It then becomes confused, however."
            Me.CriticalChance = 1
            Me.IsHMMove = False
            Me.Target = Targets.OneTarget
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
            Me.AIField2 = AIField.ConfuseOwn
        End Sub

        Public Overrides Sub MoveMultiTurn(own As Boolean, BattleScreen As BattleScreen)
            Dim currentTurns As Integer = BattleScreen.FieldEffects.OwnThrash
            If own = False Then
                currentTurns = BattleScreen.FieldEffects.OppThrash
            End If

            If currentTurns = 0 Then
                Dim turns As Integer = Core.Random.Next(2, 4)
                If own = True Then
                    BattleScreen.FieldEffects.OwnThrash = turns
                Else
                    BattleScreen.FieldEffects.OppThrash = turns
                End If
            End If
        End Sub

        Private Sub Interruption(ByVal own As Boolean, ByVal BattleScreen As BattleScreen)
            Dim thrash As Integer = 0
            Dim p As Pokemon
            If own = True Then
                thrash = BattleScreen.FieldEffects.OwnThrash
                p = BattleScreen.OwnPokemon
            Else
                thrash = BattleScreen.FieldEffects.OppThrash
                p = BattleScreen.OppPokemon
            End If

            If thrash = 1 Then
                BattleScreen.Battle.InflictConfusion(own, own, BattleScreen, p.GetDisplayName() & "'s Thrash stopped.", "move:thrash")
            End If
            If own = True Then
                BattleScreen.FieldEffects.OwnThrash = 0
            Else
                BattleScreen.FieldEffects.OppThrash = 0
            End If

        End Sub
        Public Overrides Function DeductPP(own As Boolean, BattleScreen As BattleScreen) As Boolean
            Dim thrash As Integer = BattleScreen.FieldEffects.OwnThrash
            If own = False Then
                thrash = BattleScreen.FieldEffects.OppThrash
            End If

            If thrash > 0 Then
                Return False
            Else
                Return True
            End If
        End Function
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