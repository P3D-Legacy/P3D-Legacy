Namespace BattleSystem.Moves.Normal

    Public Class Yawn

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Normal)
            Me.ID = 281
            Me.OriginalPP = 10
            Me.CurrentPP = 10
            Me.MaxPP = 10
            Me.Power = 0
            Me.Accuracy = 0
            Me.Category = Categories.Status
            Me.ContestCategory = ContestCategories.Cute
            Me.Name = "Yawn"
            Me.Description = "The user lets loose a huge yawn that lulls the target into falling asleep on the next turn."
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
            Me.ImmunityAffected = True
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

            Me.AIField1 = AIField.Sleep
            Me.AIField2 = AIField.Nothing
        End Sub

        Public Overrides Sub MoveHits(own As Boolean, BattleScreen As BattleScreen)
            Dim p As Pokemon = BattleScreen.OwnPokemon
            Dim op As Pokemon = BattleScreen.OppPokemon
            If own = False Then
                p = BattleScreen.OppPokemon
                op = BattleScreen.OwnPokemon
            End If

            If ((op.Ability.Name.ToLower() = "insomnia" Or op.Ability.Name.ToLower() = "vital spirit" Or op.Ability.Name.ToLower() = "sweet veil") = True And BattleScreen.FieldEffects.CanUseAbility(Not own, BattleScreen) = True) = True Or op.Status = Pokemon.StatusProblems.Sleep Then
                BattleScreen.BattleQuery.Add(New TextQueryObject(Me.Name & " failed!"))
            Else
                If own = True Then
                    If BattleScreen.FieldEffects.OppYawn = 0 Then
                        BattleScreen.FieldEffects.OppYawn = 2
                        BattleScreen.BattleQuery.Add(New TextQueryObject(op.GetDisplayName() & " grew drowsy."))
                    Else
                        BattleScreen.BattleQuery.Add(New TextQueryObject(Me.Name & " failed!"))
                    End If
                Else
                    If BattleScreen.FieldEffects.OwnYawn = 0 Then
                        BattleScreen.FieldEffects.OwnYawn = 2
                        BattleScreen.BattleQuery.Add(New TextQueryObject(op.GetDisplayName() & " grew drowsy."))
                    Else
                        BattleScreen.BattleQuery.Add(New TextQueryObject(Me.Name & " failed!"))
                    End If
                End If
            End If
        End Sub

    End Class

End Namespace