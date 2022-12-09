﻿Namespace BattleSystem.Moves.Rock

    Public Class StealthRock

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Rock)
            Me.ID = 446
            Me.OriginalPP = 20
            Me.CurrentPP = 20
            Me.MaxPP = 20
            Me.Power = 0
            Me.Accuracy = 0
            Me.Category = Categories.Status
            Me.ContestCategory = ContestCategories.Cool
            Me.Name = Localization.GetString("move_name_" & Me.ID,"Stealth Rock")
            Me.Description = "The user lays a trap of levitating stones around the opposing team. The trap hurts opposing Pokémon that switch into battle."
            Me.CriticalChance = 0
            Me.IsHMMove = False
            Me.Target = Targets.AllFoes
            Me.Priority = 0
            Me.TimesToAttack = 1
            '#End

            '#SpecialDefinitions
            Me.MakesContact = False
            Me.ProtectAffected = False
            Me.MagicCoatAffected = True
            Me.SnatchAffected = False
            Me.MirrorMoveAffected = True
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
            Dim stealthrock As Integer = 0
            If own = True Then
                stealthrock = BattleScreen.FieldEffects.OwnStealthrock
            Else
                stealthrock = BattleScreen.FieldEffects.OppStealthrock
            End If
            If stealthrock < 1 Then
                If own = True Then
                    BattleScreen.FieldEffects.OwnStealthrock += 1
                    BattleScreen.BattleQuery.Add(New TextQueryObject("Pointed stones float in the air around the opposing team!"))
                Else
                    BattleScreen.FieldEffects.OppStealthrock += 1
                    BattleScreen.BattleQuery.Add(New TextQueryObject("Pointed stones float in the air around your team!"))
                End If
            Else
                BattleScreen.BattleQuery.Add(New TextQueryObject(Me.Name & " failed!"))
            End If
        End Sub

    End Class

End Namespace
