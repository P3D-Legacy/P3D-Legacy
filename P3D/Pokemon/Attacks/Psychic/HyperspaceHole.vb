Namespace BattleSystem.Moves.Psychic

    Public Class HyperspaceHole

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Psychic)
            Me.ID = 593
            Me.OriginalPP = 25
            Me.CurrentPP = 25
            Me.MaxPP = 25
            Me.Power = 50
            Me.Accuracy = 0
            Me.Category = Categories.Special
            Me.ContestCategory = ContestCategories.Smart
            Me.Name = "Hyperspace Hole"
            Me.Description = "Using a hyperspace hole, the user appears right next to the target and strikes. This also hits a target using a move such as Protect or Detect."
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
            Me.MirrorMoveAffected = True
            Me.KingsrockAffected = True
            Me.CounterAffected = False

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

        Public Overrides Sub MoveHits(own As Boolean, BattleScreen As BattleScreen)
            Dim op As Pokemon = BattleScreen.OppPokemon

            If own = True Then
                If BattleScreen.FieldEffects.OppDetectCounter > 0 OrElse BattleScreen.FieldEffects.OppProtectCounter > 0 OrElse BattleScreen.FieldEffects.OppKingsShieldCounter > 0 OrElse BattleScreen.FieldEffects.OppSpikyShieldCounter > 0 OrElse BattleScreen.FieldEffects.OppBanefulBunkerCounter > 0 OrElse BattleScreen.FieldEffects.OppCraftyShieldCounter > 0 OrElse BattleScreen.FieldEffects.OppMatBlockCounter > 0 OrElse BattleScreen.FieldEffects.OppWideGuardCounter > 0 OrElse BattleScreen.FieldEffects.OppQuickGuardCounter > 0 Then
                    BattleScreen.BattleQuery.Add(New TextQueryObject("Hyperspace Hole lifted " & op.GetDisplayName() & "'s protection!"))
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
                    BattleScreen.BattleQuery.Add(New TextQueryObject("Hyperspace Hole lifted " & op.GetDisplayName() & "'s protection!"))
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