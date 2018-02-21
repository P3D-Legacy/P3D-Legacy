Namespace BattleSystem.Moves.Normal

    Public Class Sketch

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Normal)
            Me.ID = 166
            Me.OriginalPP = 20
            Me.CurrentPP = 20
            Me.MaxPP = 20
            Me.Power = 0
            Me.Accuracy = 0
            Me.Category = Categories.Status
            Me.ContestCategory = ContestCategories.Smart
            Me.Name = "Sketch"
            Me.Description = "It enables the user to permanently learn the move last used by the target. Once used, Sketch disappears."
            Me.CriticalChance = 0
            Me.IsHMMove = False
            Me.Target = Targets.OneAdjacentTarget
            Me.Priority = 0
            Me.TimesToAttack = 1
            '#End

            '#SpecialDefinitions
            Me.MakesContact = False
            Me.ProtectAffected = False
            Me.MagicCoatAffected = False
            Me.SnatchAffected = False
            Me.MirrorMoveAffected = False
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
            Dim lastMove As Attack = BattleScreen.FieldEffects.OppLastMove
            If own = False Then
                lastMove = BattleScreen.FieldEffects.OwnLastMove
            End If

            Dim p As Pokemon = BattleScreen.OwnPokemon
            If own = False Then
                p = BattleScreen.OppPokemon
            End If

            If p.IsTransformed = False Then
                If Not lastMove Is Nothing Then
                    Dim newAttack As BattleSystem.Attack = BattleSystem.Attack.GetAttackByID(lastMove.ID)

                    For Each a As BattleSystem.Attack In p.Attacks
                        If a.ID = 166 Then
                            p.Attacks.Remove(a)
                            Exit For
                        End If
                    Next

                    p.Attacks.Add(newAttack)

                    BattleScreen.BattleQuery.Add(New TextQueryObject(p.GetDisplayName() & " learned " & newAttack.Name & "!"))
                Else
                    BattleScreen.BattleQuery.Add(New TextQueryObject(Me.Name & " failed!"))
                End If
            Else
                BattleScreen.BattleQuery.Add(New TextQueryObject(Me.Name & " failed!"))
            End If
        End Sub

    End Class

End Namespace