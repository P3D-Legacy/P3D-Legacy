﻿Namespace BattleSystem.Moves.Grass

    Public Class GrassPledge

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Grass)
            Me.ID = 520
            Me.OriginalPP = 10
            Me.CurrentPP = 10
            Me.MaxPP = 10
            Me.Power = 80
            Me.Accuracy = 100
            Me.Category = Categories.Special
            Me.ContestCategory = ContestCategories.Cool
            Me.Name = Localization.GetString("move_name_" & Me.ID,"Grass Pledge")
            Me.Description = "A column of grass hits opposing Pokémon. When used with its water equivalent, its damage increases into a vast swamp."
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
                    Case "fire pledge", "water pledge"
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
                    Case "fire pledge"
                        Return Me.Power * 2
                    Case "water pledge"
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
                    Case "fire pledge"
                        If own = True Then
                            BattleScreen.FieldEffects.OwnFirePledge = 4
                        Else
                            BattleScreen.FieldEffects.OppFirePledge = 4
                        End If

                        BattleScreen.BattleQuery.Add(New TextQueryObject("A sea of fire enveloped the other team!"))
                    Case "water pledge"
                        If own = True Then
                            BattleScreen.FieldEffects.OwnGrassPledge = 4
                        Else
                            BattleScreen.FieldEffects.OppGrassPledge = 4
                        End If

                        BattleScreen.BattleQuery.Add(New TextQueryObject("A swamp enveloped the other team!"))
                End Select
            End If
        End Sub

    End Class

End Namespace