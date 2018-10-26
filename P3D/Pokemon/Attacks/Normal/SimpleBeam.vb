Namespace BattleSystem.Moves.Normal

    Public Class SimpleBeam

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Normal)
            Me.ID = 493
            Me.OriginalPP = 15
            Me.CurrentPP = 15
            Me.MaxPP = 15
            Me.Power = 0
            Me.Accuracy = 100
            Me.Category = Categories.Status
            Me.ContestCategory = ContestCategories.Cute
            Me.Name = "Simple Beam"
            Me.Description = "The user's mysterious psychic wave changes the target's Ability to Simple."
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
            Me.IsHealingMove = False
            Me.HasSecondaryEffect = False
            Me.RemovesFrozen = False

            Me.IsRecoilMove = False
            Me.IsPunchingMove = False
            Me.ImmunityAffected = True
            Me.IsDamagingMove = False
            Me.IsProtectMove = False
            Me.IsSoundMove = False

            Me.IsAffectedBySubstitute = True
            Me.IsOneHitKOMove = False
            Me.IsWonderGuardAffected = False
            '#End

        End Sub

        Public Overrides Sub MoveHits(own As Boolean, BattleScreen As BattleScreen)
            Dim op As Pokemon = BattleScreen.OppPokemon
            If own = False Then
                op = BattleScreen.OwnPokemon
            End If
            Dim bannedAbilities() As String = {"simple", "truant", "multitype", "stance change", "schooling", "comatose", "shields down", "disguise", "rks system", "battle bond"}
            If bannedAbilities.Contains(op.Ability.Name.ToLower()) = False Then
                op.Ability = Ability.GetAbilityByID(86)
                BattleScreen.BattleQuery.Add(New TextQueryObject(op.GetDisplayName() & " acquired Simple!"))
            Else
                BattleScreen.BattleQuery.Add(New TextQueryObject(Me.Name & " failed!"))
            End If
        End Sub

    End Class

End Namespace
