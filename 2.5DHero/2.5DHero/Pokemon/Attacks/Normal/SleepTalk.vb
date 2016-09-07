Namespace BattleSystem.Moves.Normal

    Public Class SleepTalk

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Normal)
            Me.ID = 214
            Me.OriginalPP = 10
            Me.CurrentPP = 10
            Me.MaxPP = 10
            Me.Power = 0
            Me.Accuracy = 0
            Me.Category = Categories.Status
            Me.ContestCategory = ContestCategories.Cute
            Me.Name = "Sleep Talk"
            Me.Description = "While it is asleep, the user randomly uses one of the moves it knows."
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
            Dim disabledMoves() As Integer = {214, 274, 117, 340, 448, 383, 91, 291, 19, 264, 382, 118, 119, 467, 166, 130, 214, 143, 76, 13, 253}

            Dim p As Pokemon = BattleScreen.OwnPokemon
            If own = False Then
                p = BattleScreen.OppPokemon
            End If

            If p.Status = Pokemon.StatusProblems.Sleep Then
                Dim countedDisabledMoves As Integer = 0

                For i = 0 To p.Attacks.Count - 1
                    If disabledMoves.Contains(p.Attacks(i).ID) = True Then
                        countedDisabledMoves += 1
                    End If
                Next

                If countedDisabledMoves < p.Attacks.Count Then
                    Dim s As Integer = Core.Random.Next(0, p.Attacks.Count)
                    While disabledMoves.Contains(p.Attacks(s).ID) = True
                        s = Core.Random.Next(0, p.Attacks.Count)
                    End While

                    Dim m As Attack = Attack.GetAttackByID(p.Attacks(s).ID)
                    BattleScreen.Battle.DoAttackRound(BattleScreen, own, m)
                Else
                    'fail:
                    BattleScreen.BattleQuery.Add(New TextQueryObject(Me.Name & " failed!"))
                End If
            Else
                'fail:
                BattleScreen.BattleQuery.Add(New TextQueryObject(Me.Name & " failed!"))
            End If
        End Sub

    End Class

End Namespace