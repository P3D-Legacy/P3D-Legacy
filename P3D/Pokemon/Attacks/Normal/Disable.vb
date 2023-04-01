Namespace BattleSystem.Moves.Normal

    Public Class Disable

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Normal)
            Me.ID = 50
            Me.OriginalPP = 20
            Me.CurrentPP = 20
            Me.MaxPP = 32
            Me.Power = 0
            Me.Accuracy = 100
            Me.Category = Categories.Status
            Me.ContestCategory = ContestCategories.Smart
            Me.Name = Localization.GetString("move_name_" & Me.ID, "Disable")
            Me.Description = "For four turns, the target will be unable to use whichever move it last used."
            Me.CriticalChance = 0
            Me.IsHMMove = False
            Me.Target = Targets.AllAdjacentTargets
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
            Me.IsSoundMove = False

            Me.IsAffectedBySubstitute = True
            Me.IsOneHitKOMove = False
            Me.IsWonderGuardAffected = False
            '#End

            Me.AIField1 = AIField.LowerAttack
            Me.AIField2 = AIField.Nothing
        End Sub

        Public Overrides Sub MoveHits(own As Boolean, BattleScreen As BattleScreen)
            Dim Target As Pokemon = BattleScreen.OppPokemon
            Dim LastMove As Attack = BattleScreen.FieldEffects.OppLastMove
            If own = False Then
                Target = BattleScreen.OwnPokemon
                LastMove = BattleScreen.FieldEffects.OwnLastMove
            End If
            If LastMove IsNot Nothing Then
                If LastMove.Name.ToLower <> "struggle" AndAlso LastMove.Disabled = 0 Then
                    For Each a As BattleSystem.Attack In Target.Attacks
                        If a.ID = LastMove.ID Then
                            a.Disabled = 4
                            BattleScreen.BattleQuery.Add(New TextQueryObject(Target.GetDisplayName() & "'s " & a.Name & " was Disabled!"))
                        End If
                    Next
                Else
                    BattleScreen.BattleQuery.Add(New TextQueryObject(Me.Name & " failed!"))
                End If
            Else
                BattleScreen.BattleQuery.Add(New TextQueryObject(Me.Name & " failed!"))
            End If

        End Sub

    End Class

End Namespace