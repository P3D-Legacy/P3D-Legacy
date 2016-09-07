Namespace BattleSystem.Moves.Dark

    Public Class Taunt

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Dark)
            Me.ID = 269
            Me.OriginalPP = 20
            Me.CurrentPP = 20
            Me.MaxPP = 20
            Me.Power = 0
            Me.Accuracy = 100
            Me.Category = Categories.Status
            Me.ContestCategory = ContestCategories.Smart
            Me.Name = "Taunt"
            Me.Description = "The target is taunted into a rage that allows it to use only attack moves for three turns."
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

            Me.IsAffectedBySubstitute = False
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

            If op.Ability.Name.ToLower() = "oblivious" And BattleScreen.FieldEffects.CanUseAbility(Not own, BattleScreen) = True Then
                BattleScreen.BattleQuery.Add(New TextQueryObject(Me.Name & " failed!"))
            Else
                If own = True Then

                    If BattleScreen.FieldEffects.OppTaunt = 0 Then
                        If BattleScreen.OppPokemon.Ability.Name.ToLower() <> "aroma veil" Then
                            BattleScreen.FieldEffects.OppTaunt = 3
                            BattleScreen.BattleQuery.Add(New TextQueryObject(op.GetDisplayName() & " fell for the Taunt."))
                        Else
                            BattleScreen.BattleQuery.Add(New TextQueryObject("Aroma Veil protected " & BattleScreen.OppPokemon.GetDisplayName() & " from " & Me.Name & "!"))
                        End If
                    Else
                        BattleScreen.BattleQuery.Add(New TextQueryObject(Me.Name & " failed!"))
                    End If
                Else
                    If BattleScreen.FieldEffects.OwnTaunt = 0 Then
                        If BattleScreen.OwnPokemon.Ability.Name.ToLower() <> "aroma veil" Then
                            BattleScreen.FieldEffects.OwnTaunt = 3
                            BattleScreen.BattleQuery.Add(New TextQueryObject(op.GetDisplayName() & " fell for the Taunt."))
                        Else
                            BattleScreen.BattleQuery.Add(New TextQueryObject("Aroma Veil protected " & BattleScreen.OwnPokemon.GetDisplayName() & " from " & Me.Name & "!"))
                        End If
                    Else
                        BattleScreen.BattleQuery.Add(New TextQueryObject(Me.Name & " failed!"))
                    End If
                End If
            End If
        End Sub

    End Class

End Namespace