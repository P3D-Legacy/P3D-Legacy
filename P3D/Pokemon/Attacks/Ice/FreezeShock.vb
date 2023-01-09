﻿Namespace BattleSystem.Moves.Ice

    Public Class FreezeShock

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Ice)
            Me.ID = 553
            Me.OriginalPP = 5
            Me.CurrentPP = 5
            Me.MaxPP = 5
            Me.Power = 140
            Me.Accuracy = 90
            Me.Category = Categories.Special
            Me.ContestCategory = ContestCategories.Beauty
            Me.Name = Localization.GetString("move_name_" & Me.ID,"Freeze Shock")
            Me.Description = "On the second turn, the user hits the target with electrically charged ice. It may leave the target with paralysis."
            Me.CriticalChance = 1
            Me.IsHMMove = False
            Me.Target = Targets.AllAdjacentTargets
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
            Me.RemovesFrozen = False
            Me.HasSecondaryEffect = True

            Me.IsHealingMove = False
            Me.IsRecoilMove = False

            Me.IsDamagingMove = True
            Me.IsProtectMove = False


            Me.IsAffectedBySubstitute = True
            Me.IsOneHitKOMove = False
            Me.IsWonderGuardAffected = True
            '#End

            Me.AIField1 = AIField.Damage
            Me.AIField2 = AIField.MultiTurn
            Me.AIField3 = AIField.CanParalyse

            EffectChances.Add(30)
        End Sub

        Public Overrides Function MoveFailBeforeAttack(Own As Boolean, BattleScreen As BattleScreen) As Boolean
            Dim p As Pokemon = BattleScreen.OwnPokemon
            Dim op As Pokemon = BattleScreen.OppPokemon
            If Own = False Then
                p = BattleScreen.OppPokemon
                op = BattleScreen.OwnPokemon
            End If

            Dim freezeshock As Integer = BattleScreen.FieldEffects.OwnFreezeShockCounter
            If Own = False Then
                freezeshock = BattleScreen.FieldEffects.OppFreezeShockCounter
            End If

            If Not p.Item Is Nothing Then
                If p.Item.OriginalName.ToLower() = "power herb" And BattleScreen.FieldEffects.CanUseItem(Own) = True And BattleScreen.FieldEffects.CanUseOwnItem(Own, BattleScreen) = True Then
                    If BattleScreen.Battle.RemoveHeldItem(Own, Own, BattleScreen, "Power Herb pushed the use of Freeze Shock!", "move:freezeshock") = True Then
                        freezeshock = 1
                    End If
                End If
            End If

            If freezeshock = 0 Then
                BattleScreen.BattleQuery.Add(New TextQueryObject(p.GetDisplayName() & " became cloaked in a harsh light!"))
                If Own = True Then
                    BattleScreen.FieldEffects.OwnFreezeShockCounter = 1
                Else
                    BattleScreen.FieldEffects.OppFreezeShockCounter = 1
                End If
                Return True
            Else
                If Own = True Then
                    BattleScreen.FieldEffects.OwnFreezeShockCounter = 0
                Else
                    BattleScreen.FieldEffects.OppFreezeShockCounter = 0
                End If
                Return False
            End If
        End Function

        Public Overrides Function DeductPP(own As Boolean, BattleScreen As BattleScreen) As Boolean
            Dim freezeshock As Integer = BattleScreen.FieldEffects.OwnFreezeShockCounter
            If own = False Then
                freezeshock = BattleScreen.FieldEffects.OppFreezeShockCounter
            End If

            If freezeshock = 0 Then
                Return False
            Else
                Return True
            End If
        End Function

        Public Overrides Sub MoveHits(own As Boolean, BattleScreen As BattleScreen)
            If Core.Random.Next(0, 100) < Me.GetEffectChance(0, own, BattleScreen) Then
                BattleScreen.Battle.InflictParalysis(Not own, own, BattleScreen, "", "move:freezeshock")
            End If
        End Sub

    End Class

End Namespace