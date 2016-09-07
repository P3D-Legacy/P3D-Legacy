Namespace BattleSystem.Moves.Psychic

    Public Class SkillSwap

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Psychic)
            Me.ID = 285
            Me.OriginalPP = 10
            Me.CurrentPP = 10
            Me.MaxPP = 10
            Me.Power = 0
            Me.Accuracy = 0
            Me.Category = Categories.Status
            Me.ContestCategory = ContestCategories.Smart
            Me.Name = "Skill Swap"
            Me.Description = "The user employs its psychic power to exchange Abilities with the target."
            Me.CriticalChance = 0
            Me.IsHMMove = False
            Me.Target = Targets.OneAdjacentTarget
            Me.Priority = 0
            Me.TimesToAttack = 1
            '#End

            '#SpecialDefinitions
            Me.MakesContact = False
            Me.ProtectAffected = True
            Me.MagicCoatAffected = False
            Me.SnatchAffected = False
            Me.MirrorMoveAffected = True
            Me.KingsrockAffected = False
            Me.CounterAffected = False

            Me.DisabledWhileGravity = False
            Me.UseEffectiveness = False
            Me.ImmunityAffected = False
            Me.RemovesFrozen = False
            Me.HasSecondaryEffect = True

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
            Dim p As Pokemon = BattleScreen.OwnPokemon
            Dim op As Pokemon = BattleScreen.OppPokemon
            If own = False Then
                p = BattleScreen.OppPokemon
                op = BattleScreen.OwnPokemon
            End If

            Dim bannedAbilities() As String = {"wonder guard", "multitype", "illusion", "stance change"}

            If bannedAbilities.Contains(p.Ability.Name.ToLower()) = False And bannedAbilities.Contains(op.Ability.Name.ToLower()) = False Then
                Dim pAbility As Integer = p.Ability.ID
                Dim opAbility As Integer = op.Ability.ID

                p.OriginalAbility = Ability.GetAbilityByID(p.Ability.ID)
                op.OriginalAbility = Ability.GetAbilityByID(op.Ability.ID)

                p.Ability = Ability.GetAbilityByID(opAbility)
                op.Ability = Ability.GetAbilityByID(pAbility)

                BattleScreen.BattleQuery.Add(New TextQueryObject(p.GetDisplayName() & " swapped abilities with its target!"))
            Else
                'fails
                BattleScreen.BattleQuery.Add(New TextQueryObject(Me.Name & " failed!"))
            End If
        End Sub

    End Class

End Namespace