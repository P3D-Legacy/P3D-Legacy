Namespace BattleSystem.Moves.Normal

    Public Class Entrainment

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Normal)
            Me.ID = 494
            Me.OriginalPP = 15
            Me.CurrentPP = 15
            Me.MaxPP = 15
            Me.Power = 0
            Me.Accuracy = 100
            Me.Category = Categories.Status
            Me.ContestCategory = ContestCategories.Cute
            Me.Name = Localization.GetString("move_name_" & Me.ID,"Entrainment")
            Me.Description = "The user dances with an odd rhythm that compels the target to mimic it, making the target's Ability the same as the user's."
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
            Me.RemovesOwnFrozen = False

            Me.IsRecoilMove = False

            Me.ImmunityAffected = True
            Me.IsDamagingMove = False
            Me.IsProtectMove = False


            Me.IsAffectedBySubstitute = True
            Me.IsOneHitKOMove = False
            Me.IsWonderGuardAffected = False
            '#End

        End Sub

        Public Overrides Sub MoveHits(own As Boolean, BattleScreen As BattleScreen)
            Dim p As Pokemon = BattleScreen.OwnPokemon
            Dim op As Pokemon = BattleScreen.OppPokemon
            If own = False Then
                p = BattleScreen.OppPokemon
                op = BattleScreen.OwnPokemon
            End If

            Dim bannedAbilitiesOpp() As String = {"simple", "truant", "multitype", "stance change", "schooling", "comatose", "shields down", "disguise", "rks system", "battle bond"}
            Dim bannedAbilitiesOwn() As String = {"trace", "forecast", "flower gift", "zen mode", "illusion", "imposter", "power of alchemy", "receiver", "disguise", "power construct"}

            If bannedAbilitiesOpp.Contains(op.Ability.Name.ToLower()) = False AndAlso bannedAbilitiesOwn.Contains(p.Ability.Name.ToLower()) = False Then
                op.Ability = Ability.GetAbilityByID(p.Ability.ID)
                BattleScreen.BattleQuery.Add(New TextQueryObject(op.GetDisplayName() & " acquired " & Localization.GetString("ability_name_" & op.Ability.ID.ToString, op.Ability.Name()) & "!"))
            Else
                BattleScreen.BattleQuery.Add(New TextQueryObject(Me.Name & " failed!"))
            End If
        End Sub

    End Class

End Namespace
