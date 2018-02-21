Namespace BattleSystem.Moves.Water

    Public Class WaterPledge

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Water)
            Me.ID = 518
            Me.OriginalPP = 10
            Me.CurrentPP = 10
            Me.MaxPP = 10
            Me.Power = 80
            Me.Accuracy = 100
            Me.Category = Categories.Special
            Me.ContestCategory = ContestCategories.Cute
            Me.Name = "Water Pledge"
            Me.Description = "A column of water strikes the target. When combined with its fire equivalent, the damage increases and a rainbow appears."
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
            Me.CounterAffected = False

            Me.DisabledWhileGravity = False
            Me.UseEffectiveness = True
            Me.ImmunityAffected = True
            Me.HasSecondaryEffect = True
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

        Public Overrides Sub BeforeDealingDamage(own As Boolean, BattleScreen As BattleScreen)
            Dim lastMove As Attack = BattleScreen.FieldEffects.OwnLastMove
            If own = False Then
                lastMove = BattleScreen.FieldEffects.OppLastMove
            End If

            If Not lastMove Is Nothing Then
                Select Case lastMove.Name.ToLower()
                    Case "grass pledge", "fire pledge"
                        BattleScreen.BattleQuery.Add(New TextQueryObject("The two moves are joined! It's a combined move!"))
                End Select
            End If
        End Sub

        Public Overrides Function GetBasePower(own As Boolean, BattleScreen As BattleScreen) As Integer
            Dim lastMove As Attack = BattleScreen.FieldEffects.OwnLastMove
            If own = False Then
                lastMove = BattleScreen.FieldEffects.OppLastMove
            End If

            If Not lastMove Is Nothing Then
                Select Case lastMove.Name.ToLower()
                    Case "grass pledge"
                        Return Me.Power * 2
                    Case "fire pledge"
                        Return Me.Power * 2
                End Select
            End If

            Return Me.Power
        End Function

        Public Overrides Sub MoveHits(own As Boolean, BattleScreen As BattleScreen)
            Dim lastMove As Attack = BattleScreen.FieldEffects.OwnLastMove
            If own = False Then
                lastMove = BattleScreen.FieldEffects.OppLastMove
            End If

            If Not lastMove Is Nothing Then
                Select Case lastMove.Name.ToLower()
                    Case "grass pledge"
                        If own = True Then
                            BattleScreen.FieldEffects.OwnGrassPledge = 4
                        Else
                            BattleScreen.FieldEffects.OppGrassPledge = 4
                        End If

                        BattleScreen.BattleQuery.Add(New TextQueryObject("A swamp enveloped the other team!"))
                    Case "fire pledge"
                        If own = True Then
                            BattleScreen.FieldEffects.OwnWaterPledge = 4
                        Else
                            BattleScreen.FieldEffects.OppWaterPledge = 4
                        End If

                        BattleScreen.BattleQuery.Add(New TextQueryObject("A rainbow appeared in the sky!"))
                End Select
            End If
        End Sub

    End Class

End Namespace