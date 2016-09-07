Namespace BattleSystem.Moves.Normal

    Public Class BatonPass

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Normal)
            Me.ID = 226
            Me.OriginalPP = 40
            Me.CurrentPP = 40
            Me.MaxPP = 40
            Me.Power = 0
            Me.Accuracy = 0
            Me.Category = Categories.Status
            Me.ContestCategory = ContestCategories.Cute
            Me.Name = "Baton Pass"
            Me.Description = "The user switches places with a party Pokémon in waiting and passes along any stat changes."
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
            Me.SnatchAffected = False
            Me.MirrorMoveAffected = False
            Me.KingsrockAffected = False
            Me.CounterAffected = False

            Me.DisabledWhileGravity = False
            Me.UseEffectiveness = False
            Me.ImmunityAffected = False
            Me.HasSecondaryEffect = False
            Me.RemovesFrozen = False

            Me.IsHealingMove = False
            Me.IsRecoilMove = False
            Me.IsPunchingMove = False
            Me.IsDamagingMove = False
            Me.IsProtectMove = False
            Me.IsSoundMove = False

            Me.IsAffectedBySubstitute = False
            Me.IsOneHitKOMove = False
            Me.IsWonderGuardAffected = False
            '#End

            Me.AIField1 = AIField.Support
            Me.AIField2 = AIField.Nothing
        End Sub

        Public Overrides Sub MoveHits(own As Boolean, BattleScreen As BattleScreen)
            If own = True Then
                If Core.Player.CountFightablePokemon > 1 Then
                    BattleScreen.FieldEffects.OwnUsedBatonPass = True

                    BattleScreen.Battle.SwitchOutOwn(BattleScreen, GetPokemonIndex(BattleScreen, own), -1)
                Else
                    BattleScreen.BattleQuery.Add(New TextQueryObject(Me.Name & " failed!"))
                End If
            Else
                If BattleScreen.IsTrainerBattle = True Or BattleScreen.IsRemoteBattle = True Or BattleScreen.IsPVPBattle = True Then
                    If BattleScreen.Trainer.CountUseablePokemon > 1 Then
                        BattleScreen.FieldEffects.OppUsedBatonPass = True

                        BattleScreen.Battle.SwitchOutOpp(BattleScreen, GetPokemonIndex(BattleScreen, own))
                    Else
                        BattleScreen.BattleQuery.Add(New TextQueryObject(Me.Name & " failed!"))
                    End If
                Else
                    BattleScreen.BattleQuery.Add(New TextQueryObject(Me.Name & " failed!"))
                End If
            End If
        End Sub

        Private Function GetPokemonIndex(ByVal BattleScreen As BattleScreen, ByVal own As Boolean) As Integer
            If own = True Then
                Dim i As Integer = 0
                While Core.Player.Pokemons(i).HP <= 0 Or Core.Player.Pokemons(i).Status = Pokemon.StatusProblems.Fainted Or i = BattleScreen.OwnPokemonIndex
                    i += 1
                End While
                Return i
            Else
                Dim i As Integer = 0
                While BattleScreen.Trainer.Pokemons(i).HP <= 0 Or BattleScreen.Trainer.Pokemons(i).Status = Pokemon.StatusProblems.Fainted Or i = BattleScreen.OwnPokemonIndex
                    i += 1
                End While
                Return i
            End If
            Return -1
        End Function

    End Class

End Namespace