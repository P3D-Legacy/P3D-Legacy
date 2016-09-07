Namespace BattleSystem.Moves.Flying

    Public Class Bounce

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Flying)
            Me.ID = 340
            Me.OriginalPP = 5
            Me.CurrentPP = 5
            Me.MaxPP = 5
            Me.Power = 85
            Me.Accuracy = 85
            Me.Category = Categories.Physical
            Me.ContestCategory = ContestCategories.Cute
            Me.Name = "Bounce"
            Me.Description = "The user bounces up high, then drops on the target on the second turn. This may also leave the target with paralysis."
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

            Me.DisabledWhileGravity = True
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
        End Sub

        Public Overrides Function GetUseAccEvasion(own As Boolean, BattleScreen As BattleScreen) As Boolean
            Dim bounce As Integer = BattleScreen.FieldEffects.OwnBounceCounter
            If own = False Then
                bounce = BattleScreen.FieldEffects.OppBounceCounter
            End If

            If bounce = 0 Then
                Return False
            Else
                Return True
            End If
        End Function

        Public Overrides Sub PreAttack(Own As Boolean, BattleScreen As BattleScreen)
            Dim bounce As Integer = BattleScreen.FieldEffects.OwnBounceCounter
            If Own = False Then
                bounce = BattleScreen.FieldEffects.OppBounceCounter
            End If

            If bounce = 0 Then
                Me.FocusOppPokemon = False
            Else
                Me.FocusOppPokemon = True
            End If
        End Sub

        Public Overrides Function MoveFailBeforeAttack(Own As Boolean, BattleScreen As BattleScreen) As Boolean
            Dim p As Pokemon = BattleScreen.OwnPokemon
            Dim op As Pokemon = BattleScreen.OppPokemon
            If Own = False Then
                p = BattleScreen.OppPokemon
                op = BattleScreen.OwnPokemon
            End If

            Dim bounce As Integer = BattleScreen.FieldEffects.OwnBounceCounter
            If Own = False Then
                bounce = BattleScreen.FieldEffects.OppBounceCounter
            End If

            If Not p.Item Is Nothing Then
                If p.Item.Name.ToLower() = "power herb" And BattleScreen.FieldEffects.CanUseItem(Own) = True And BattleScreen.FieldEffects.CanUseOwnItem(Own, BattleScreen) = True Then
                    If BattleScreen.Battle.RemoveHeldItem(Own, Own, BattleScreen, "Power Herb pushed the use of Bounce!", "move:bounce") = True Then
                        bounce = 1
                    End If
                End If
            End If

            If bounce = 0 Then
                BattleScreen.BattleQuery.Add(New TextQueryObject(p.GetDisplayName() & " sprang up!"))
                If Own = True Then
                    BattleScreen.FieldEffects.OwnBounceCounter = 1
                Else
                    BattleScreen.FieldEffects.OppBounceCounter = 1
                End If
                Return True
            Else
                If Own = True Then
                    BattleScreen.FieldEffects.OwnBounceCounter = 0
                Else
                    BattleScreen.FieldEffects.OppBounceCounter = 0
                End If
                Return False
            End If
        End Function

        Public Overrides Function DeductPP(own As Boolean, BattleScreen As BattleScreen) As Boolean
            Dim bounce As Integer = BattleScreen.FieldEffects.OwnBounceCounter
            If own = False Then
                bounce = BattleScreen.FieldEffects.OppBounceCounter
            End If

            If bounce = 0 Then
                Return False
            Else
                Return True
            End If
        End Function

        Private Sub MoveFails(own As Boolean, BattleScreen As BattleScreen)
            If own = True Then
                BattleScreen.FieldEffects.OwnBounceCounter = 0
            Else
                BattleScreen.FieldEffects.OppBounceCounter = 0
            End If
        End Sub

        Public Overrides Sub MoveMisses(own As Boolean, BattleScreen As BattleScreen)
            MoveFails(own, BattleScreen)
        End Sub

        Public Overrides Sub AbsorbedBySubstitute(own As Boolean, BattleScreen As BattleScreen)
            MoveFails(own, BattleScreen)
        End Sub

        Public Overrides Sub MoveProtectedDetected(own As Boolean, BattleScreen As BattleScreen)
            MoveFails(own, BattleScreen)
        End Sub

    End Class

End Namespace