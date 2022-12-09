Namespace BattleSystem.Moves.Normal

    Public Class RockClimb

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Normal)
            Me.ID = 431
            Me.OriginalPP = 20
            Me.CurrentPP = 20
            Me.MaxPP = 20
            Me.Power = 90
            Me.Accuracy = 85
            Me.Category = Categories.Physical
            Me.ContestCategory = ContestCategories.Tough
            Me.Name = Localization.GetString("move_name_" & Me.ID,"Rock Climb")
            Me.Description = "The user attacks the target by smashing into it with incredible force. This may also confuse the target."
            Me.CriticalChance = 1
            Me.IsHMMove = True
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

            Me.AIField1 = AIField.Damage
            Me.AIField2 = AIField.CanConfuse

            Me.EffectChances.Add(20)
        End Sub

        Public Overrides Sub MoveHits(own As Boolean, BattleScreen As BattleScreen)
            If Core.Random.Next(0, 100) < Me.GetEffectChance(0, own, BattleScreen) Then
                BattleScreen.Battle.InflictConfusion(Not own, own, BattleScreen, "", "move:rockclimb")
            End If
        End Sub

    End Class

End Namespace