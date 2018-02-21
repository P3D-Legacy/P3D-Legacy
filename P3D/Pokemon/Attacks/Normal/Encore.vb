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
            Me.Name = "Encore"
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
            Me.RemovesFrozen = False

            Me.IsHealingMove = False
            Me.IsRecoilMove = False
            Me.IsPunchingMove = False
            Me.IsDamagingMove = False
            Me.IsProtectMove = False
            Me.IsSoundMove = False

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

            Dim lastMove As Attack = BattleScreen.FieldEffects.OppLastMove
            If own = False Then
                lastMove = BattleScreen.FieldEffects.OwnLastMove
            End If

            If Not lastMove Is Nothing Then
                If lastMove.CurrentPP > 0 Then
                    Dim deductPP As Integer = 3

                    If lastMove.CurrentPP < 3 Then
                        deductPP = lastMove.CurrentPP
                    End If

                    lastMove.CurrentPP -= deductPP

                    BattleScreen.BattleQuery.Add(New TextQueryObject(op.GetDisplayName() & "'s " & lastMove.Name & " lost " & deductPP & " PP!"))
                Else
                    BattleScreen.BattleQuery.Add(New TextQueryObject(Me.Name & " failed!"))
                End If
            Else
                BattleScreen.BattleQuery.Add(New TextQueryObject(Me.Name & " failed!"))
            End If
        End Sub

    End Class

End Namespace