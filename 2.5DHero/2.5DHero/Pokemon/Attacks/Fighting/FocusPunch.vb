Namespace BattleSystem.Moves.Fighting

    Public Class FocusPunch

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Fighting)
            Me.ID = 264
            Me.OriginalPP = 20
            Me.CurrentPP = 20
            Me.MaxPP = 20
            Me.Power = 150
            Me.Accuracy = 100
            Me.Category = Categories.Physical
            Me.ContestCategory = ContestCategories.Tough
            Me.Name = "Focus Punch"
            Me.Description = "The user focuses its mind before launching a punch. It will fail if the user is hit before it is used."
            Me.CriticalChance = 1
            Me.IsHMMove = False
            Me.Target = Targets.OneAdjacentTarget
            Me.Priority = -3
            Me.TimesToAttack = 1
            '#End

            '#SpecialDefinitions
            Me.MakesContact = True
            Me.ProtectAffected = True
            Me.MagicCoatAffected = False
            Me.SnatchAffected = False
            Me.MirrorMoveAffected = True
            Me.KingsrockAffected = False
            Me.CounterAffected = True

            Me.DisabledWhileGravity = False
            Me.UseEffectiveness = True
            Me.ImmunityAffected = True
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

        Public Overrides Function MoveFailBeforeAttack(Own As Boolean, BattleScreen As BattleScreen) As Boolean
            If Own = True Then
                If BattleScreen.FieldEffects.OwnPokemonDamagedThisTurn = True Then
                    BattleScreen.BattleQuery.Add(New TextQueryObject(BattleScreen.OwnPokemon.GetDisplayName() & " lost its focus and couldn't move!"))
                    Return True
                Else
                    Return False
                End If
            Else
                If BattleScreen.FieldEffects.OppPokemonDamagedThisTurn = True Then
                    BattleScreen.BattleQuery.Add(New TextQueryObject(BattleScreen.OppPokemon.GetDisplayName() & " lost its focus and couldn't move!"))
                    Return True
                Else
                    Return False
                End If
            End If
        End Function

    End Class

End Namespace