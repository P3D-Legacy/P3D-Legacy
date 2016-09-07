Namespace BattleSystem.Moves.Dragon

    Public Class Outrage

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Dragon)
            Me.ID = 200
            Me.OriginalPP = 10
            Me.CurrentPP = 10
            Me.MaxPP = 10
            Me.Power = 120
            Me.Accuracy = 100
            Me.Category = Categories.Physical
            Me.ContestCategory = ContestCategories.Cool
            Me.Name = "Outrage"
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

            Me.AIField1 = AIField.Damage
            Me.AIField2 = AIField.MultiTurn
            Me.AIField3 = AIField.ConfuseOwn
        End Sub

        Public Overrides Sub MoveHits(own As Boolean, BattleScreen As BattleScreen)
            Dim currentTurns As Integer = BattleScreen.FieldEffects.OwnOutrage
            If own = False Then
                currentTurns = BattleScreen.FieldEffects.OppOutrage
            End If

            If currentTurns = 0 Then
                Dim turns As Integer = Core.Random.Next(2, 4)
                If own = True Then
                    BattleScreen.FieldEffects.OwnOutrage = turns
                Else
                    BattleScreen.FieldEffects.OppOutrage = turns
                End If
            End If
        End Sub

        Private Sub Interruption(ByVal own As Boolean, ByVal BattleScreen As BattleScreen)
            If own = True Then
                BattleScreen.FieldEffects.OwnOutrage = 0
            Else
                BattleScreen.FieldEffects.OppOutrage = 0
            End If

            BattleScreen.Battle.InflictConfusion(own, own, BattleScreen, "", "move:outrage")
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

    End Class

End Namespace