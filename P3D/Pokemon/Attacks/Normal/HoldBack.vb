﻿Namespace BattleSystem.Moves.Normal

    Public Class HoldBack

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Normal)
            Me.ID = 610
            Me.OriginalPP = 40
            Me.CurrentPP = 40
            Me.MaxPP = 40
            Me.Power = 40
            Me.Accuracy = 100
            Me.Category = Categories.Physical
            Me.ContestCategory = ContestCategories.Cool
            Me.Name = Localization.GetString("move_name_" & Me.ID,"Hold Back")
            Me.Description = "The user holds back when it attacks, and the target is left with at least 1 HP."
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
            Me.IsRecoilMove = False

            Me.IsDamagingMove = True
            Me.IsProtectMove = False


            Me.IsAffectedBySubstitute = True
            Me.IsOneHitKOMove = False
            Me.IsWonderGuardAffected = True
            '#End

            Me.AIField1 = AIField.Damage
            Me.AIField2 = AIField.Nothing
        End Sub

        Public Overrides Function GetDamage(Critical As Boolean, Own As Boolean, targetPokemon As Boolean, BattleScreen As BattleScreen, Optional ExtraParameter As String = "") As Integer
            Dim d As Integer = MyBase.GetDamage(Critical, Own, targetPokemon, BattleScreen)

            Dim subst As Integer = BattleScreen.FieldEffects.OppSubstitute
            If Own = False Then
                subst = BattleScreen.FieldEffects.OwnSubstitute
            End If

            If subst = 0 Then
                Dim op As Pokemon = BattleScreen.OppPokemon
                If Own = False Then
                    op = BattleScreen.OwnPokemon
                End If

                If d >= op.HP Then
                    d = op.HP - 1
                End If
            End If

            Return d
        End Function

    End Class

End Namespace