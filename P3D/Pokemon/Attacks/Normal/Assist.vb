Namespace BattleSystem.Moves.Normal

    Public Class Assist

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Normal)
            Me.ID = 274
            Me.OriginalPP = 20
            Me.CurrentPP = 20
            Me.MaxPP = 20
            Me.Power = 0
            Me.Accuracy = 0
            Me.Category = Categories.Status
            Me.ContestCategory = ContestCategories.Cute
            Me.Name = "Assist"
            Me.Description = "The user hurriedly and randomly uses a move among those known by other Pokémon in the party."
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
            Dim disabledMoves() As Integer = {274, 562, 448, 509, 383, 68, 343, 194, 197, 525, 203, 364, 264, 266, 270, 588, 561, 382, 118, 102, 243, 119, 267, 566, 182, 46, 166, 214, 289, 596, 564, 165, 415, 168, 144, 271, 18}

            Dim p As Pokemon = BattleScreen.OwnPokemon
            If own = False Then
                p = BattleScreen.OppPokemon
            End If

            Dim countedDisabledMoves As Integer = 0

            Dim attackList As New List(Of BattleSystem.Attack)
            If own = True Then
                For Each pp As Pokemon In Core.Player.Pokemons
                    attackList.AddRange(pp.Attacks)
                Next
            Else
                If BattleScreen.IsTrainerBattle Then
                    For Each pp As Pokemon In BattleScreen.Trainer.Pokemons
                        attackList.AddRange(pp.Attacks)
                    Next
                Else
                    attackList.AddRange(BattleScreen.OppPokemon.Attacks)
                End If
            End If

            For i = 0 To attackList.Count - 1
                If disabledMoves.Contains(attackList(i).ID) = True Then
                    countedDisabledMoves += 1
                End If
            Next

            If countedDisabledMoves < attackList.Count Then
                Dim s As Integer = Core.Random.Next(0, attackList.Count)
                While disabledMoves.Contains(attackList(s).ID) = True
                    s = Core.Random.Next(0, attackList.Count)
                End While

                Dim m As Attack = Attack.GetAttackByID(attackList(s).ID)

                BattleScreen.Battle.DoAttackRound(BattleScreen, own, m)
            Else
                'fail:
                BattleScreen.BattleQuery.Add(New TextQueryObject(Me.Name & " failed!"))
            End If
        End Sub

    End Class

End Namespace