Namespace BattleSystem.Moves.Water

    Public Class SparklingAria

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Water)
            Me.ID = 664
            Me.OriginalPP = 10
            Me.CurrentPP = 10
            Me.MaxPP = 10
            Me.Power = 90
            Me.Accuracy = 100
            Me.Category = Categories.Special
            Me.ContestCategory = ContestCategories.Beauty
            Me.Name = Localization.GetString("move_name_" & Me.ID,"Sparkling Aria")
            Me.Description = "The user bursts into song, emitting many bubbles. Any Pokémon suffering from a burn will be healed by the touch of these bubbles."
            Me.CriticalChance = 1
            Me.IsHMMove = False
            Me.Target = Targets.AllAdjacentTargets
            Me.Priority = 0
            Me.TimesToAttack = 1
            '#End

            '#SpecialDefinitions
            Me.MakesContact = False
            Me.ProtectAffected = True
            Me.MagicCoatAffected = False
            Me.SnatchAffected = False
            Me.MirrorMoveAffected = True
            Me.KingsrockAffected = True
            Me.CounterAffected = False

            Me.DisabledWhileGravity = False
            Me.UseEffectiveness = True
            Me.ImmunityAffected = True
            Me.HasSecondaryEffect = False
            Me.RemovesOwnFrozen = False

            Me.IsHealingMove = False
            Me.IsRecoilMove = False

            Me.IsDamagingMove = True
            Me.IsProtectMove = False
            Me.IsSoundMove = True

            Me.IsAffectedBySubstitute = True
            Me.IsOneHitKOMove = False
            Me.IsWonderGuardAffected = True
            '#End
        End Sub

        Public Overrides Sub MoveHits(own As Boolean, BattleScreen As BattleScreen)
            If own = True Then
                If BattleScreen.OppPokemon.Status = Pokemon.StatusProblems.Burn Then
                    BattleScreen.Battle.CureStatusProblem(Not own, own, BattleScreen, BattleScreen.OppPokemon.GetDisplayName() & " was cured of burn.", "move:sparklingaria")
                End If
            Else
                If BattleScreen.OwnPokemon.Status = Pokemon.StatusProblems.Burn Then
                    BattleScreen.Battle.CureStatusProblem(Not own, own, BattleScreen, BattleScreen.OwnPokemon.GetDisplayName() & " was cured of burn.", "move:sparklingaria")
                End If
            End If
        End Sub

    End Class

End Namespace