Namespace BattleSystem.Moves.Fairy

    Public Class LightOfRuin

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Fairy)
            Me.ID = 617
            Me.OriginalPP = 5
            Me.CurrentPP = 5
            Me.MaxPP = 5
            Me.Power = 140
            Me.Accuracy = 90
            Me.Category = Categories.Special
            Me.ContestCategory = ContestCategories.Beauty
            Me.Name = Localization.GetString("move_name_" & Me.ID,"Light of Ruin")
            Me.Description = "Drawing power from the Eternal Flower, the user fires a powerful beam of light. This also damages the user quite a lot."
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
            Me.HasSecondaryEffect = False
            Me.RemovesFrozen = False

            Me.IsHealingMove = False
            Me.IsRecoilMove = True
            Me.IsPunchingMove = False
            Me.IsDamagingMove = True
            Me.IsProtectMove = False
            Me.IsSoundMove = False

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
            Dim recoilDamage As Integer = CInt(Math.Floor(lastDamage / 2))
            If recoilDamage <= 0 Then
                recoilDamage = 1
            End If

            BattleScreen.Battle.InflictRecoil(own, own, BattleScreen, Me, recoilDamage, "", "move:lightofruin")
        End Sub

    End Class

End Namespace
