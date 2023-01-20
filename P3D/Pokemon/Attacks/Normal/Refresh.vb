Namespace BattleSystem.Moves.Normal

    Public Class Refresh

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Normal)
            Me.ID = 287
            Me.OriginalPP = 20
            Me.CurrentPP = 20
            Me.MaxPP = 20
            Me.Power = 0
            Me.Accuracy = 0
            Me.Category = Categories.Status
            Me.ContestCategory = ContestCategories.Smart
            Me.Name = Localization.GetString("move_name_" & Me.ID,"Refresh")
            Me.Description = "The user rests to cure itself of a poisoning, burn, or paralysis."
            Me.CriticalChance = 0
            Me.IsHMMove = False
            Me.Target = Targets.Self
            Me.Priority = 0
            Me.TimesToAttack = 1
            '#End

            '#SpecialDefinitions
            Me.MakesContact = False
            Me.ProtectAffected = False
            Me.MagicCoatAffected = False
            Me.SnatchAffected = True
            Me.MirrorMoveAffected = True
            Me.KingsrockAffected = False
            Me.CounterAffected = False

            Me.DisabledWhileGravity = False
            Me.UseEffectiveness = False
            Me.ImmunityAffected = False
            Me.HasSecondaryEffect = False
            Me.RemovesFrozen = False

            Me.IsHealingMove = True
            Me.IsRecoilMove = False
            Me.IsPunchingMove = False
            Me.IsDamagingMove = False
            Me.IsProtectMove = False
            Me.IsSoundMove = False

            Me.IsAffectedBySubstitute = False
            Me.IsOneHitKOMove = False
            Me.IsWonderGuardAffected = False
            '#End

            Me.AIField1 = AIField.Healing
            Me.AIField2 = AIField.Nothing
        End Sub

        Public Overrides Sub MoveHits(own As Boolean, BattleScreen As BattleScreen)
            If own = True Then
                If BattleScreen.OwnPokemon.Status = Pokemon.StatusProblems.Poison Or BattleScreen.OwnPokemon.Status = Pokemon.StatusProblems.BadPoison Or BattleScreen.OwnPokemon.Status = Pokemon.StatusProblems.Burn Or BattleScreen.OwnPokemon.Status = Pokemon.StatusProblems.Paralyzed Then
                    BattleScreen.Battle.CureStatusProblem(own, own, BattleScreen, BattleScreen.OwnPokemon.GetDisplayName() & " was cured.", "move:refresh")
                End If
            Else
                If BattleScreen.OppPokemon.Status = Pokemon.StatusProblems.Poison Or BattleScreen.OppPokemon.Status = Pokemon.StatusProblems.BadPoison Or BattleScreen.OppPokemon.Status = Pokemon.StatusProblems.Burn Or BattleScreen.OppPokemon.Status = Pokemon.StatusProblems.Paralyzed Then
                    BattleScreen.Battle.CureStatusProblem(own, own, BattleScreen, BattleScreen.OppPokemon.GetDisplayName() & " was cured.", "move:refresh")
                End If
            End If

        End Sub

    End Class

End Namespace