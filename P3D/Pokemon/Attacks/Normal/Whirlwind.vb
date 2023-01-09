﻿Namespace BattleSystem.Moves.Normal

    Public Class Whirlwind

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Normal)
            Me.ID = 18
            Me.OriginalPP = 20
            Me.CurrentPP = 20
            Me.MaxPP = 20
            Me.Power = 0
            Me.Accuracy = 0
            Me.Category = Categories.Status
            Me.ContestCategory = ContestCategories.Smart
            Me.Name = Localization.GetString("move_name_" & Me.ID,"Whirlwind")
            Me.Description = "The target is blown away, and a different Pokémon is dragged out. In the wild, this ends a battle against a single Pokémon."
            Me.CriticalChance = 0
            Me.IsHMMove = False
            Me.Target = Targets.OneAdjacentTarget
            Me.Priority = -6
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
            Me.ImmunityAffected = True
            Me.HasSecondaryEffect = False
            Me.RemovesFrozen = False

            Me.IsHealingMove = False
            Me.IsRecoilMove = False
            Me.IsWindMove = True
            Me.IsDamagingMove = False
            Me.IsProtectMove = False

            Me.IsAffectedBySubstitute = True
            Me.IsOneHitKOMove = False
            Me.IsWonderGuardAffected = False
            Me.CanHitInMidAir = True
            Me.UseAccEvasion = False
            '#End

            Me.AIField1 = AIField.Support
            Me.AIField2 = AIField.Nothing
        End Sub

        Public Overrides Sub MoveHits(own As Boolean, BattleScreen As BattleScreen)
            'end wild battles when you use it (if your level is higher)
            'when used against trainer (or you), the move will drag out another Pokémon.
            'check for canswitch

            Dim p As Pokemon = BattleScreen.OwnPokemon
            If own = True Then
                p = BattleScreen.OppPokemon
            End If

            If BattleCalculation.CanSwitch(BattleScreen, Not own) = True Then
                BattleScreen.BattleQuery.Add(New TextQueryObject(p.GetDisplayName() & " got blown away!"))
                If BattleScreen.IsPVPBattle = True Or BattleScreen.IsTrainerBattle = True Or BattleScreen.IsRemoteBattle = True Then
                    'trainer battle
                    If own = True Then
                        If BattleScreen.Trainer.CountUseablePokemon > 1 Then
                            Dim i As Integer = Core.Random.Next(0, BattleScreen.Trainer.Pokemons.count)
                            While BattleScreen.Trainer.Pokemons(i).Status = Pokemon.StatusProblems.Fainted OrElse BattleScreen.OppPokemonIndex = i OrElse BattleScreen.Trainer.Pokemons(i).HP <= 0
                                i = Core.Random.Next(0, BattleScreen.Trainer.Pokemons.Count - 1)
                            End While
                            BattleScreen.Battle.SwitchOutOpp(BattleScreen, i, "")
                        Else
                            BattleScreen.BattleQuery.Add(New TextQueryObject(Me.Name & " failed!"))
                        End If
                    Else
                        If Core.Player.CountFightablePokemon > 1 Then
                            Dim i As Integer = Core.Random.Next(0, Core.Player.Pokemons.Count)
                            While Core.Player.Pokemons(i).Status = Pokemon.StatusProblems.Fainted OrElse BattleScreen.OwnPokemonIndex = i OrElse Core.Player.Pokemons(i).HP <= 0
                                i = Core.Random.Next(0, Core.Player.Pokemons.Count - 1)
                            End While
                            BattleScreen.Battle.SwitchOutOwn(BattleScreen, i, -1)
                        Else
                            BattleScreen.BattleQuery.Add(New TextQueryObject(Me.Name & " failed!"))
                        End If
                    End If
                Else
                    'wild battle

                    If own = True Then
                        BattleScreen.BattleQuery.Add(New EndBattleQueryObject(False))
                    Else
                        If Core.Player.CountFightablePokemon > 1 Then
                            BattleScreen.Battle.SwitchOutOwn(BattleScreen, -1, -1)
                        Else
                            BattleScreen.BattleQuery.Add(New TextQueryObject(Me.Name & " failed!"))
                        End If
                    End If
                End If
            Else
                BattleScreen.BattleQuery.Add(New TextQueryObject(Me.Name & " failed!"))
            End If
        End Sub

    End Class

End Namespace
