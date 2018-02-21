Namespace BattleSystem.Moves.Dark

    Public Class BeatUp

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Dark)
            Me.ID = 251
            Me.OriginalPP = 10
            Me.CurrentPP = 10
            Me.MaxPP = 10
            Me.Power = 0
            Me.Accuracy = 100
            Me.Category = Categories.Physical
            Me.ContestCategory = ContestCategories.Smart
            Me.Name = "Beat Up"
            Me.Description = "The user gets all party Pokémon to attack the target. The more party Pokémon, the greater the number of attacks."
            Me.CriticalChance = 1
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
            Me.KingsrockAffected = True
            Me.CounterAffected = True

            Me.DisabledWhileGravity = False
            Me.UseEffectiveness = True
            Me.ImmunityAffected = False
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
            '#End
        End Sub

        Public Overrides Function GetTimesToAttack(own As Boolean, BattleScreen As BattleScreen) As Integer
            Dim i As Integer = 1
            If own = True Then
                For Each p As Pokemon In Core.Player.Pokemons
                    If p.Status = Pokemon.StatusProblems.None Then
                        i += 1
                    End If
                Next
            ElseIf BattleScreen.IsTrainerBattle = True Then
                For Each p As Pokemon In BattleScreen.Trainer.Pokemons
                    If p.Status = Pokemon.StatusProblems.None Then
                        i += 1
                    End If
                Next
            End If
            i = i-1
            Return i
        End Function

        Public Overrides Function GetBasePower(own As Boolean, BattleScreen As BattleScreen) As Integer
            Dim avgTeamBaseAttack As Double = 0.0D
            Dim pokemonCounter As Integer = 0
            If own Then
                For Each pokemon As Pokemon In Core.Player.Pokemons
                    If (Not pokemon.IsEgg) AndAlso pokemon.Status <> Pokemon.StatusProblems.Fainted And pokemon.HP > 0 Then
                        avgTeamBaseAttack += (pokemon.BaseAttack / 10)
                        pokemonCounter += 1
                    End If
                Next
            Else
                For Each pokemon As Pokemon In BattleScreen.Trainer.Pokemons
                    If (Not pokemon.IsEgg) AndAlso pokemon.Status <> Pokemon.StatusProblems.Fainted And pokemon.HP > 0 Then
                        avgTeamBaseAttack += (pokemon.BaseAttack / 10)
                        pokemonCounter += 1
                    End If
                Next
            End If
            If pokemonCounter <> 0 Then
                avgTeamBaseAttack = avgTeamBaseAttack / pokemonCounter
            Else
                avgTeamBaseAttack = 10 'should never meet this case.
            End If
            Return CInt(avgTeamBaseAttack) + 5
        End Function
    End Class

End Namespace
