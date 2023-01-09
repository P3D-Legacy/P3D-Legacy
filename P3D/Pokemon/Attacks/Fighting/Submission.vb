﻿Namespace BattleSystem.Moves.Fighting

    Public Class Submission

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Fighting)
            Me.ID = 66
            Me.OriginalPP = 20
            Me.CurrentPP = 20
            Me.MaxPP = 20
            Me.Power = 80
            Me.Accuracy = 80
            Me.Category = Categories.Physical
            Me.ContestCategory = ContestCategories.Cool
            Me.Name = Localization.GetString("move_name_" & Me.ID,"Submission")
            Me.Description = "The user grabs the target and recklessly dives for the ground. It also hurts the user slightly."
            Me.CriticalChance = 1
            Me.IsHMMove = False
            Me.Target = Targets.OneAdjacentTarget
            Me.Priority = 0
            Me.TimesToAttack = 1
            '#End

            '#SpecialDefinitions
            Me.MakesContact = True
            Me.ProtectAffected = True
            Me.MagicCoatAffected = False
            Me.SnatchAffected = False
            Me.MirrorMoveAffected = True
            Me.KingsrockAffected = True
            Me.CounterAffected = True

            Me.DisabledWhileGravity = False
            Me.UseEffectiveness = True
            Me.ImmunityAffected = True
            Me.HasSecondaryEffect = False
            Me.RemovesFrozen = False

            Me.IsHealingMove = False
            Me.IsRecoilMove = True

            Me.IsDamagingMove = True
            Me.IsProtectMove = False


            Me.IsAffectedBySubstitute = True
            Me.IsOneHitKOMove = False
            Me.IsWonderGuardAffected = True
            '#End

            Me.AIField1 = AIField.Damage
            Me.AIField2 = AIField.Recoil
        End Sub

        Public Overrides Sub MoveRecoil(own As Boolean, BattleScreen As BattleScreen)
            Dim lastDamage As Integer = BattleScreen.FieldEffects.OwnLastDamage
            If own = False Then
                lastDamage = BattleScreen.FieldEffects.OppLastDamage
            End If
            Dim recoilDamage As Integer = CInt(Math.Floor(lastDamage / 4))
            If recoilDamage <= 0 Then
                recoilDamage = 1
            End If

            BattleScreen.Battle.InflictRecoil(own, own, BattleScreen, Me, recoilDamage, "", "move:submission")
        End Sub

    End Class

End Namespace
