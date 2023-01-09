﻿Namespace BattleSystem.Moves.Bug

    Public Class SilverWind

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Bug)
            Me.ID = 318
            Me.OriginalPP = 5
            Me.CurrentPP = 5
            Me.MaxPP = 5
            Me.Power = 60
            Me.Accuracy = 100
            Me.Category = Categories.Special
            Me.ContestCategory = ContestCategories.Beauty
            Me.Name = Localization.GetString("move_name_" & Me.ID, "Silver Wind")
            Me.Description = "The target is attacked with powdery scales blown by wind. It may also raise all the user's stats."
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
            Me.CounterAffected = True

            Me.DisabledWhileGravity = False
            Me.UseEffectiveness = True
            Me.ImmunityAffected = True
            Me.HasSecondaryEffect = True
            Me.RemovesFrozen = False

            Me.IsHealingMove = False
            Me.IsRecoilMove = False
            Me.IsWindMove = True 'P3D only
            Me.IsDamagingMove = True
            Me.IsProtectMove = False

            Me.IsAffectedBySubstitute = True
            Me.IsOneHitKOMove = False
            Me.IsWonderGuardAffected = True
            '#End

            Me.AIField1 = AIField.Damage
            Me.AIField2 = AIField.CanRaiseAttack
            Me.AIField3 = AIField.CanRaiseSpAttack

            Me.EffectChances.Add(10)
        End Sub

        Public Overrides Sub MoveHits(own As Boolean, BattleScreen As BattleScreen)
            If Core.Random.Next(0, 100) < GetEffectChance(0, own, BattleScreen) Then
                BattleScreen.Battle.RaiseStat(own, own, BattleScreen, "Attack", 1, "", "move:silverwind")
                BattleScreen.Battle.RaiseStat(own, own, BattleScreen, "Defense", 1, "", "move:silverwind")
                BattleScreen.Battle.RaiseStat(own, own, BattleScreen, "Special Attack", 1, "", "move:silverwind")
                BattleScreen.Battle.RaiseStat(own, own, BattleScreen, "Special Defense", 1, "", "move:silverwind")
                BattleScreen.Battle.RaiseStat(own, own, BattleScreen, "Speed", 1, "", "move:silverwind")
            End If
        End Sub

    End Class

End Namespace
