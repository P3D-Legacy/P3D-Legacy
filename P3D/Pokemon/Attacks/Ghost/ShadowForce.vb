Namespace BattleSystem.Moves.Ghost

    Public Class ShadowForce

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Ghost)
            Me.ID = 467
            Me.OriginalPP = 5
            Me.CurrentPP = 5
            Me.MaxPP = 5
            Me.Power = 120
            Me.Accuracy = 100
            Me.Category = Categories.Physical
            Me.ContestCategory = ContestCategories.Cool
            Me.Name = "Shadow Force"
            Me.Description = "The user disappears, then strikes the target on the next turn. This move hits even if the target protects itself."
            Me.CriticalChance = 1
            Me.IsHMMove = False
            Me.Target = Targets.OneAdjacentTarget
            Me.Priority = 0
            Me.TimesToAttack = 1
            '#End

            '#SpecialDefinitions
            Me.MakesContact = True
            Me.ProtectAffected = False
            Me.MagicCoatAffected = False
            Me.SnatchAffected = False
            Me.MirrorMoveAffected = True
            Me.KingsrockAffected = True
            Me.CounterAffected = True

            Me.DisabledWhileGravity = False
            Me.UseEffectiveness = True
            Me.ImmunityAffected = True
            Me.RemovesFrozen = False
            Me.HasSecondaryEffect = False

            Me.IsHealingMove = False
            Me.IsRecoilMove = False
            Me.IsPunchingMove = False
            Me.IsDamagingMove = True
            Me.IsProtectMove = False
            Me.IsSoundMove = False

            Me.IsAffectedBySubstitute = True
            Me.IsOneHitKOMove = False
            Me.IsWonderGuardAffected = True
            Me.UseAccEvasion = True
            Me.CanHitUnderwater = False
            Me.CanHitUnderground = False
            Me.CanHitInMidAir = False
            '#End

            Me.AIField1 = AIField.Damage
            Me.AIField2 = AIField.MultiTurn
        End Sub

        Public Overrides Function GetUseAccEvasion(own As Boolean, BattleScreen As BattleScreen) As Boolean
            Dim shadowforce As Integer = BattleScreen.FieldEffects.OwnShadowForceCounter
            If own = False Then
                shadowforce = BattleScreen.FieldEffects.OppShadowForceCounter
            End If

            If shadowforce = 0 Then
                Return False
            Else
                Return True
            End If
        End Function

        Public Overrides Sub PreAttack(Own As Boolean, BattleScreen As BattleScreen)
            Dim shadowforce As Integer = BattleScreen.FieldEffects.OwnShadowForceCounter
            If Own = False Then
                shadowforce = BattleScreen.FieldEffects.OppShadowForceCounter
            End If

            If shadowforce = 0 Then
                Me.FocusOppPokemon = False
            Else
                Me.FocusOppPokemon = True
            End If
        End Sub

        Public Overrides Function MoveFailBeforeAttack(Own As Boolean, BattleScreen As BattleScreen) As Boolean
            Dim shadowforceCounter As Integer = BattleScreen.FieldEffects.OwnShadowForceCounter

            If Own = False Then
                shadowforceCounter = BattleScreen.FieldEffects.OppShadowForceCounter
            End If

            Dim p As Pokemon = BattleScreen.OwnPokemon
            If Own = False Then
                p = BattleScreen.OppPokemon
            End If

            Dim hasToCharge As Boolean = True
            If Not p.Item Is Nothing Then
                If p.Item.Name.ToLower(Globalization.CultureInfo.InvariantCulture) = "power herb" And BattleScreen.FieldEffects.CanUseItem(Own) = True And BattleScreen.FieldEffects.CanUseOwnItem(Own, BattleScreen) = True Then
                    If BattleScreen.Battle.RemoveHeldItem(Own, Own, BattleScreen, "Power Herb pushed the use of Shadow Force!", "move:shadowforce") = True Then
                        hasToCharge = False
                    End If
                End If
            End If

            If shadowforceCounter = 0 And hasToCharge = True Then
                If Own = True Then
                    BattleScreen.FieldEffects.OwnShadowForceCounter = 1
                Else
                    BattleScreen.FieldEffects.OppShadowForceCounter = 1
                End If

                BattleScreen.BattleQuery.Add(New TextQueryObject(p.GetDisplayName() & " vanished instantly!"))

                Return True
            Else
                If Own = True Then
                    BattleScreen.FieldEffects.OwnShadowForceCounter = 0
                Else
                    BattleScreen.FieldEffects.OppShadowForceCounter = 0
                End If

                Return False
            End If
        End Function

        Public Overrides Sub MoveSelected(own As Boolean, BattleScreen As BattleScreen)
            If own = True Then
                BattleScreen.FieldEffects.OwnShadowForceCounter = 0
            Else
                BattleScreen.FieldEffects.OppShadowForceCounter = 0
            End If
        End Sub
        Public Overrides Function DeductPP(own As Boolean, BattleScreen As BattleScreen) As Boolean
            Dim ShadowForce As Integer = BattleScreen.FieldEffects.OwnShadowForceCounter
            If own = False Then
                ShadowForce = BattleScreen.FieldEffects.OppShadowForceCounter
            End If

            If ShadowForce = 0 Then
                Return False
            Else
                Return True
            End If
        End Function

        Private Sub MoveFails(own As Boolean, BattleScreen As BattleScreen)
            If own = True Then
                BattleScreen.FieldEffects.OwnShadowForceCounter = 0
            Else
                BattleScreen.FieldEffects.OppShadowForceCounter = 0
            End If
        End Sub

        Public Overrides Sub MoveMisses(own As Boolean, BattleScreen As BattleScreen)
            MoveFails(own, BattleScreen)
        End Sub

        Public Overrides Sub AbsorbedBySubstitute(own As Boolean, BattleScreen As BattleScreen)
            MoveFails(own, BattleScreen)
        End Sub

        Public Overrides Sub InflictedFlinch(own As Boolean, BattleScreen As BattleScreen)
            MoveFails(own, BattleScreen)
        End Sub

        Public Overrides Sub IsSleeping(own As Boolean, BattleScreen As BattleScreen)
            MoveFails(own, BattleScreen)
        End Sub

        Public Overrides Sub HurtItselfInConfusion(own As Boolean, BattleScreen As BattleScreen)
            MoveFails(own, BattleScreen)
        End Sub

        Public Overrides Sub IsAttracted(own As Boolean, BattleScreen As BattleScreen)
            MoveFails(own, BattleScreen)
        End Sub

        Public Overrides Sub MoveHits(own As Boolean, BattleScreen As BattleScreen)
            Dim op As Pokemon = BattleScreen.OppPokemon

            If own = True Then
                If BattleScreen.FieldEffects.OppDetectCounter > 0 OrElse BattleScreen.FieldEffects.OppProtectCounter > 0 OrElse BattleScreen.FieldEffects.OppKingsShieldCounter > 0 OrElse BattleScreen.FieldEffects.OppSpikyShieldCounter > 0 OrElse BattleScreen.FieldEffects.OppBanefulBunkerCounter > 0 OrElse BattleScreen.FieldEffects.OppCraftyShieldCounter > 0 OrElse BattleScreen.FieldEffects.OppMatBlockCounter > 0 OrElse BattleScreen.FieldEffects.OppWideGuardCounter > 0 OrElse BattleScreen.FieldEffects.OppQuickGuardCounter > 0 Then
                    BattleScreen.BattleQuery.Add(New TextQueryObject("Shadow Force lifted " & op.GetDisplayName() & "'s protection!"))
                End If
                BattleScreen.FieldEffects.OppDetectCounter = 0
                BattleScreen.FieldEffects.OppProtectCounter = 0
                BattleScreen.FieldEffects.OppKingsShieldCounter = 0
                BattleScreen.FieldEffects.OppSpikyShieldCounter = 0
                BattleScreen.FieldEffects.OppBanefulBunkerCounter = 0
                BattleScreen.FieldEffects.OppCraftyShieldCounter = 0
                BattleScreen.FieldEffects.OppMatBlockCounter = 0
                BattleScreen.FieldEffects.OppWideGuardCounter = 0
                BattleScreen.FieldEffects.OppQuickGuardCounter = 0
            Else
                op = BattleScreen.OwnPokemon
                If BattleScreen.FieldEffects.OwnDetectCounter > 0 OrElse BattleScreen.FieldEffects.OwnProtectCounter > 0 OrElse BattleScreen.FieldEffects.OwnKingsShieldCounter > 0 OrElse BattleScreen.FieldEffects.OwnSpikyShieldCounter > 0 OrElse BattleScreen.FieldEffects.OwnBanefulBunkerCounter > 0 OrElse BattleScreen.FieldEffects.OwnCraftyShieldCounter > 0 OrElse BattleScreen.FieldEffects.OwnMatBlockCounter > 0 OrElse BattleScreen.FieldEffects.OwnWideGuardCounter > 0 OrElse BattleScreen.FieldEffects.OwnQuickGuardCounter > 0 Then
                    BattleScreen.BattleQuery.Add(New TextQueryObject("Shadow Force lifted " & op.GetDisplayName() & "'s protection!"))
                End If
                BattleScreen.FieldEffects.OwnDetectCounter = 0
                BattleScreen.FieldEffects.OwnProtectCounter = 0
                BattleScreen.FieldEffects.OwnKingsShieldCounter = 0
                BattleScreen.FieldEffects.OwnSpikyShieldCounter = 0
                BattleScreen.FieldEffects.OwnBanefulBunkerCounter = 0
                BattleScreen.FieldEffects.OwnCraftyShieldCounter = 0
                BattleScreen.FieldEffects.OwnMatBlockCounter = 0
                BattleScreen.FieldEffects.OwnWideGuardCounter = 0
                BattleScreen.FieldEffects.OwnQuickGuardCounter = 0
            End If

        End Sub

    End Class

End Namespace