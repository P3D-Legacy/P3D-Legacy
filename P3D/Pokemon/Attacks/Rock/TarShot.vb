﻿Namespace BattleSystem.Moves.Rock

    Public Class TarShot

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Rock)
            Me.ID = 749
            Me.OriginalPP = 15
            Me.CurrentPP = 15
            Me.MaxPP = 15
            Me.Power = 0
            Me.Accuracy = 100
            Me.Category = Categories.Status
            Me.ContestCategory = ContestCategories.Cool
            Me.Name = Localization.GetString("move_name_" & Me.ID,"Tar Shot")
            Me.Description = "The user pours sticky tar over the target, lowering the target's Speed stat. The target becomes weaker to Fire-type moves."
            Me.CriticalChance = 1
            Me.IsHMMove = False
            Me.Target = Targets.OneAdjacentTarget
            Me.Priority = 0
            Me.TimesToAttack = 1
            '#End

            '#SpecialDefinitions
            Me.MakesContact = False
            Me.ProtectAffected = True
            Me.MagicCoatAffected = True
            Me.SnatchAffected = False
            Me.MirrorMoveAffected = True
            Me.KingsrockAffected = False
            Me.CounterAffected = True

            Me.DisabledWhileGravity = False
            Me.UseEffectiveness = False
            Me.ImmunityAffected = True
            Me.RemovesFrozen = False
            Me.HasSecondaryEffect = False

            Me.IsHealingMove = False
            Me.IsRecoilMove = False
            Me.IsPunchingMove = False
            Me.IsDamagingMove = False
            Me.IsProtectMove = False
            Me.IsSoundMove = False

            Me.IsAffectedBySubstitute = True
            Me.IsOneHitKOMove = False
            Me.IsWonderGuardAffected = True
            '#End

            Me.AIField1 = AIField.Damage
            Me.AIField2 = AIField.CanLowerSpeed
        End Sub
        Public Overrides Sub MoveHits(own As Boolean, BattleScreen As BattleScreen)
            BattleScreen.Battle.LowerStat(Not own, own, BattleScreen, "Speed", 1, "", "move:tarshot")

            With BattleScreen.FieldEffects
                If own = True Then
                    If .OppTarShot = False Then
                        .OppTarShot = True
                        BattleScreen.BattleQuery.Add(New TextQueryObject(BattleScreen.OppPokemon.GetName & " " & "became weaker to fire!"))
                    End If
                Else
                    If .OwnTarShot = False Then
                        .OwnTarShot = True
                        BattleScreen.BattleQuery.Add(New TextQueryObject(BattleScreen.OwnPokemon.GetName & " " & "became weaker to fire!"))
                    End If
                End If
            End With
        End Sub

    End Class

End Namespace