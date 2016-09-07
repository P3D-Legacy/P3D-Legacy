Namespace BattleSystem.Moves.Fighting

    Public Class Counter

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Fighting)
            Me.ID = 68
            Me.OriginalPP = 20
            Me.CurrentPP = 20
            Me.MaxPP = 20
            Me.Power = 0
            Me.Accuracy = 100
            Me.Category = Categories.Physical
            Me.ContestCategory = ContestCategories.Cool
            Me.Name = "Counter"
            Me.Description = "A retaliation move that counters any physical attack, inflicting double the damage taken."
            Me.CriticalChance = 0
            Me.IsHMMove = False
            Me.Target = Targets.OneAdjacentTarget
            Me.Priority = -5
            Me.TimesToAttack = 1
            '#End

            '#SpecialDefinitions
            Me.MakesContact = True
            Me.ProtectAffected = True
            Me.MagicCoatAffected = False
            Me.SnatchAffected = False
            Me.MirrorMoveAffected = False
            Me.KingsrockAffected = False
            Me.CounterAffected = True

            Me.DisabledWhileGravity = False
            Me.UseEffectiveness = True
            Me.ImmunityAffected = True
            Me.HasSecondaryEffect = False
            Me.RemovesFrozen = False

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
            Dim damagedLastTurn As Boolean = BattleScreen.FieldEffects.OppPokemonDamagedLastTurn
            If Own = False Then
                damagedLastTurn = BattleScreen.FieldEffects.OwnPokemonDamagedThisTurn
            End If

            If damagedLastTurn = True Then
                Dim damage As Integer = BattleScreen.FieldEffects.OwnLastDamage
                If Own = True Then
                    damage = BattleScreen.FieldEffects.OppLastDamage
                End If

                If damage > 0 Then
                    Dim lastMove As Attack = BattleScreen.FieldEffects.OwnLastMove
                    If Own = True Then
                        lastMove = BattleScreen.FieldEffects.OppLastMove
                    End If
                    If Not lastMove Is Nothing Then
                        If lastMove.CounterAffected = True Then
                            Return False
                        End If
                    End If
                End If
            End If

            Return True
        End Function

        Public Overrides Function GetDamage(Critical As Boolean, Own As Boolean, targetPokemon As Boolean, BattleScreen As BattleScreen) As Integer
            Dim damage As Integer = BattleScreen.FieldEffects.OwnLastDamage
            If Own = True Then
                damage = BattleScreen.FieldEffects.OppLastDamage
            End If

            Return damage * 2
        End Function

    End Class

End Namespace