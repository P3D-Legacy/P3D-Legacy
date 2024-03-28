Namespace BattleSystem.Moves.Normal

    Public Class Encore

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Normal)
            Me.ID = 227
            Me.OriginalPP = 5
            Me.CurrentPP = 5
            Me.MaxPP = 5
            Me.Power = 0
            Me.Accuracy = 100
            Me.Category = Categories.Status
            Me.ContestCategory = ContestCategories.Cute
            Me.Name = Localization.GetString("move_name_" & Me.ID,"Encore")
            Me.Description = "The user compels the target to keep using only the move it last used for three turns."
            Me.CriticalChance = 0
            Me.IsHMMove = False
            Me.Target = Targets.OneAdjacentTarget
            Me.Priority = 0
            Me.TimesToAttack = 1
            '#End

            '#SpecialDefinitions
            Me.MakesContact = False
            Me.ProtectAffected = True
            Me.MagicCoatAffected = True
            Me.SnatchAffected = False
            Me.MirrorMoveAffected = True
            Me.KingsrockAffected = False
            Me.CounterAffected = False

            Me.DisabledWhileGravity = False
            Me.UseEffectiveness = False
            Me.ImmunityAffected = False
            Me.HasSecondaryEffect = False
            Me.RemovesOwnFrozen = False

            Me.IsHealingMove = False
            Me.IsRecoilMove = False

            Me.IsDamagingMove = False
            Me.IsProtectMove = False


            Me.IsAffectedBySubstitute = True
            Me.IsOneHitKOMove = False
            Me.IsWonderGuardAffected = False
            '#End

            Me.AIField1 = AIField.Support
            Me.AIField2 = AIField.Nothing
        End Sub

        Public Overrides Sub MoveHits(own As Boolean, BattleScreen As BattleScreen)
            Dim op As Pokemon = BattleScreen.OppPokemon
            If own = False Then
                op = BattleScreen.OwnPokemon
            End If

            Dim lastMove As Attack

            If own = True Then
                If (BattleScreen.FieldEffects.OppLastMove Is Nothing AndAlso Battle.OppStep.StepType = Battle.RoundConst.StepTypes.Move) OrElse Battle.OppStep.StepType = Battle.RoundConst.StepTypes.Move AndAlso CType(Battle.OppStep.Argument, Attack).ID <> BattleScreen.FieldEffects.OppLastMove.ID Then
                    lastMove = CType(Battle.OppStep.Argument, Attack)
                Else
                    lastMove = BattleScreen.FieldEffects.OppLastMove
                End If
            Else
                If (BattleScreen.FieldEffects.OwnLastMove Is Nothing AndAlso Battle.OwnStep.StepType = Battle.RoundConst.StepTypes.Move) OrElse Battle.OwnStep.StepType = Battle.RoundConst.StepTypes.Move AndAlso CType(Battle.OwnStep.Argument, Attack).ID <> BattleScreen.FieldEffects.OwnLastMove.ID Then
                    lastMove = CType(Battle.OwnStep.Argument, Attack)
                Else
                    lastMove = BattleScreen.FieldEffects.OwnLastMove
                End If
            End If

            If Not lastMove Is Nothing Then
                If own = True Then
                    BattleScreen.FieldEffects.OppEncoreMove = lastMove
                Else
                    BattleScreen.FieldEffects.OwnEncoreMove = lastMove
                End If
                If own = True Then
                    BattleScreen.FieldEffects.OppEncore = 3
                Else
                    BattleScreen.FieldEffects.OwnEncore = 3
                End If
                BattleScreen.BattleQuery.Add(New TextQueryObject(op.GetDisplayName & " received an encore!"))
            Else
                BattleScreen.BattleQuery.Add(New TextQueryObject(Me.Name & " failed!"))
            End If
        End Sub

    End Class

End Namespace