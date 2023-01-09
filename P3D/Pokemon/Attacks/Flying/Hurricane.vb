﻿Namespace BattleSystem.Moves.Flying

    Public Class Hurricane

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Flying)
            Me.ID = 542
            Me.OriginalPP = 10
            Me.CurrentPP = 10
            Me.MaxPP = 10
            Me.Power = 110
            Me.Accuracy = 70
            Me.Category = Categories.Special
            Me.ContestCategory = ContestCategories.Cute
            Me.Name = Localization.GetString("move_name_" & Me.ID,"Hurricane")
            Me.Description = "The user attacks by wrapping its opponent in a fierce wind that flies up into the sky. It may also confuse the target."
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
            Me.IsWindMove = True
            Me.IsDamagingMove = True
            Me.IsProtectMove = False

            Me.IsAffectedBySubstitute = True
            Me.IsOneHitKOMove = False
            Me.IsWonderGuardAffected = True
            Me.CanHitInMidAir = True
            '#End

            Me.AIField1 = AIField.Damage
            Me.AIField2 = AIField.CanConfuse

            Me.EffectChances.Add(30)
        End Sub

        Public Overrides Sub MoveHits(own As Boolean, BattleScreen As BattleScreen)
            If Core.Random.Next(0, 100) < Me.GetEffectChance(0, own, BattleScreen) Then
                BattleScreen.Battle.InflictConfusion(Not own, own, BattleScreen, "", "move:hurricane")
            End If
        End Sub

        Public Overrides Function GetAccuracy(own As Boolean, BattleScreen As BattleScreen) As Integer
            If BattleScreen.FieldEffects.Weather = BattleWeather.WeatherTypes.Rain Then
                Return 100
            ElseIf BattleScreen.FieldEffects.Weather = BattleWeather.WeatherTypes.Sunny Then
                Return 50
            Else
                Return Me.Accuracy
            End If
        End Function

        Public Overrides Function GetUseAccEvasion(own As Boolean, BattleScreen As BattleScreen) As Boolean
            If BattleScreen.FieldEffects.Weather = BattleWeather.WeatherTypes.Rain Then
                Return False
            Else
                Return True
            End If
        End Function

    End Class

End Namespace
