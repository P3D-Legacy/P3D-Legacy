Namespace BattleSystem.Moves.Dark

    Public Class PartingShot

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Dark)
            Me.ID = 575
            Me.OriginalPP = 20
            Me.CurrentPP = 20
            Me.MaxPP = 20
            Me.Power = 0
            Me.Accuracy = 100
            Me.Category = Categories.Status
            Me.ContestCategory = ContestCategories.Cool
            Me.Name = "Parting Shot"
            Me.Description = "With a parting threat, the user lowers the target's Attack and Sp. Atk stats. Then it switches with a party Pokémon."
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
            Me.KingsrockAffected = True
            Me.CounterAffected = False

            Me.DisabledWhileGravity = False
            Me.UseEffectiveness = False
            Me.ImmunityAffected = False
            Me.RemovesFrozen = False
            Me.HasSecondaryEffect = False

            Me.IsHealingMove = False
            Me.IsRecoilMove = False
            Me.IsPunchingMove = False
            Me.IsDamagingMove = False
            Me.IsProtectMove = False
            Me.IsSoundMove = True

            Me.IsAffectedBySubstitute = True
            Me.IsOneHitKOMove = False
            Me.IsWonderGuardAffected = False
            '#End
        End Sub

        Public Overrides Sub MoveHits(own As Boolean, BattleScreen As BattleScreen)
            Dim b As Boolean = BattleScreen.Battle.LowerStat(Not own, own, BattleScreen, "Attack", 1, "", "move:partingshot")
            Dim d As Boolean = BattleScreen.Battle.LowerStat(Not own, own, BattleScreen, "Special Attack", 1, "", "move:partingshot")
            If b = False And d = False Then
                BattleScreen.BattleQuery.Add(New TextQueryObject(Me.Name & " failed!"))
            End If
        End Sub

        Public Overrides Sub MoveSwitch(own As Boolean, BattleScreen As BattleScreen)
            If own = True Then
                If Core.Player.CountFightablePokemon > 1 Then
                    BattleScreen.Battle.SwitchOutOwn(BattleScreen, GetPokemonIndex(BattleScreen, own), -1)
                End If
            Else
                If BattleScreen.IsTrainerBattle = True Or BattleScreen.IsRemoteBattle = True Or BattleScreen.IsPVPBattle = True Then
                    If BattleScreen.Trainer.CountUseablePokemon > 1 Then
                        BattleScreen.Battle.SwitchOutOpp(BattleScreen, GetPokemonIndex(BattleScreen, own))
                    End If
                End If
            End If
        End Sub

        Private Function GetPokemonIndex(ByVal BattleScreen As BattleScreen, ByVal own As Boolean) As Integer
            If own = True Then
                Dim i As Integer = 0
                While Core.Player.Pokemons(i).HP <= 0 Or Core.Player.Pokemons(i).Status = Pokemon.StatusProblems.Fainted Or i = BattleScreen.OwnPokemonIndex Or Core.Player.Pokemons(i).IsEgg() = True
                    i += 1
                End While
                Return i
            Else
                Dim i As Integer = 0
                While BattleScreen.Trainer.Pokemons(i).HP <= 0 Or BattleScreen.Trainer.Pokemons(i).Status = Pokemon.StatusProblems.Fainted Or i = BattleScreen.OppPokemonIndex Or BattleScreen.Trainer.Pokemons(i).IsEgg() = True
                    i += 1
                End While
                Return i
            End If
            Return -1
        End Function

    End Class

End Namespace
