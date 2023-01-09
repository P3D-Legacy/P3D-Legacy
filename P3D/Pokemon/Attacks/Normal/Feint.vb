Namespace BattleSystem.Moves.Normal

    Public Class Feint

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Normal)
            Me.ID = 364
            Me.OriginalPP = 10
            Me.CurrentPP = 10
            Me.MaxPP = 10
            Me.Power = 30
            Me.Accuracy = 100
            Me.Category = Categories.Physical
            Me.ContestCategory = ContestCategories.Smart
            Me.Name = Localization.GetString("move_name_" & Me.ID,"Feint")
            Me.Description = "An attack that hits a target using Protect or Detect. This also lifts the effects of those moves."
            Me.CriticalChance = 0
            Me.IsHMMove = False
            Me.Target = Targets.OneAdjacentTarget
            Me.Priority = 2
            Me.TimesToAttack = 1
            '#End

            '#SpecialDefinitions
            Me.MakesContact = False
            Me.ProtectAffected = False
            Me.MagicCoatAffected = False
            Me.SnatchAffected = False
            Me.MirrorMoveAffected = True
            Me.KingsrockAffected = False
            Me.CounterAffected = True

            Me.DisabledWhileGravity = False
            Me.UseEffectiveness = True
            Me.ImmunityAffected = True
            Me.HasSecondaryEffect = False
            Me.RemovesFrozen = False

            Me.IsHealingMove = False
            Me.IsRecoilMove = False
            Me.IsPunchingMove = True 'P3D Only
            Me.IsDamagingMove = True
            Me.IsProtectMove = False

            Me.IsAffectedBySubstitute = True
            Me.IsOneHitKOMove = False
            Me.IsWonderGuardAffected = True
            '#End

            Me.AIField1 = AIField.Damage
            Me.AIField2 = AIField.Nothing
        End Sub

        Public Overrides Sub MoveHits(own As Boolean, BattleScreen As BattleScreen)
            Dim op As Pokemon = BattleScreen.OppPokemon

            If own = True Then
                If BattleScreen.FieldEffects.OppDetectCounter > 0 OrElse BattleScreen.FieldEffects.OppProtectCounter > 0 OrElse BattleScreen.FieldEffects.OppKingsShieldCounter > 0 OrElse BattleScreen.FieldEffects.OppSpikyShieldCounter > 0 OrElse BattleScreen.FieldEffects.OppBanefulBunkerCounter > 0 OrElse BattleScreen.FieldEffects.OppCraftyShieldCounter > 0 OrElse BattleScreen.FieldEffects.OppMatBlockCounter > 0 OrElse BattleScreen.FieldEffects.OppWideGuardCounter > 0 OrElse BattleScreen.FieldEffects.OppQuickGuardCounter > 0 Then
                    BattleScreen.BattleQuery.Add(New TextQueryObject("Feint lifted " & op.GetDisplayName() & "'s protection!"))
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
                    BattleScreen.BattleQuery.Add(New TextQueryObject("Feint lifted " & op.GetDisplayName() & "'s protection!"))
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