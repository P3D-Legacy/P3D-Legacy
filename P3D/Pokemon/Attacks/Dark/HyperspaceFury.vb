Namespace BattleSystem.Moves.Dark

    Public Class HyperspaceFury

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Dark)
            Me.ID = 621
            Me.OriginalPP = 25
            Me.CurrentPP = 25
            Me.MaxPP = 25
            Me.Power = 100
            Me.Accuracy = 0
            Me.Category = Categories.Physical
            Me.ContestCategory = ContestCategories.Tough
            Me.Name = "Hyperspace Fury"
            Me.Description = "Using its many arms, the user unleashes a barrage of attacks that ignore the effects of moves like Protect and Detect. But the user's Defense stat falls."
            Me.CriticalChance = 1
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
            Me.KingsrockAffected = True
            Me.CounterAffected = True

            Me.DisabledWhileGravity = False
            Me.UseEffectiveness = True
            Me.ImmunityAffected = True
            Me.RemovesFrozen = False
            Me.HasSecondaryEffect = True

            Me.IsHealingMove = False
            Me.IsRecoilMove = False
            Me.IsPunchingMove = False
            Me.IsDamagingMove = True
            Me.IsProtectMove = False
            Me.IsSoundMove = False

            Me.IsAffectedBySubstitute = True
            Me.IsOneHitKOMove = False
            Me.IsWonderGuardAffected = True
            Me.UseAccEvasion = False
            Me.CanHitUnderwater = False
            Me.CanHitUnderground = False
            Me.CanHitInMidAir = False
            '#End

            Me.AIField1 = AIField.Damage
            Me.AIField2 = AIField.CannotMiss
        End Sub

        Public Overrides Function MoveFailBeforeAttack(Own As Boolean, BattleScreen As BattleScreen) As Boolean
            Dim p As Pokemon = BattleScreen.OwnPokemon
            If Own = False Then
                p = BattleScreen.OppPokemon
            End If
            If p.Number = 720 And p.AdditionalData = "unbound" Then
                Return False
            Else
                BattleScreen.BattleQuery.Add(New TextQueryObject("But " & p.GetDisplayName() & " can't use the move!"))
                Return True
            End If
        End Function

        Public Overrides Sub MoveHits(own As Boolean, BattleScreen As BattleScreen)
            Dim op As Pokemon = BattleScreen.OppPokemon

            If own = True Then
                If BattleScreen.FieldEffects.OppDetectCounter > 0 OrElse BattleScreen.FieldEffects.OppProtectCounter > 0 OrElse BattleScreen.FieldEffects.OppKingsShieldCounter > 0 OrElse BattleScreen.FieldEffects.OppSpikyShieldCounter > 0 OrElse BattleScreen.FieldEffects.OppBanefulBunkerCounter > 0 OrElse BattleScreen.FieldEffects.OppCraftyShieldCounter > 0 OrElse BattleScreen.FieldEffects.OppMatBlockCounter > 0 OrElse BattleScreen.FieldEffects.OppWideGuardCounter > 0 OrElse BattleScreen.FieldEffects.OppQuickGuardCounter > 0 Then
                    BattleScreen.BattleQuery.Add(New TextQueryObject("Hyperspace Fury lifted " & op.GetDisplayName() & "'s protection!"))
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
                    BattleScreen.BattleQuery.Add(New TextQueryObject("Hyperspace Fury lifted " & op.GetDisplayName() & "'s protection!"))
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

            BattleScreen.Battle.LowerStat(own, own, BattleScreen, "Defense", 1, "", "move:hyperspacefury")
        End Sub

    End Class

End Namespace