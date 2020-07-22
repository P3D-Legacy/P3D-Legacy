Namespace BattleSystem.Moves.Normal

    Public Class WeatherBall

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Normal)
            Me.ID = 311
            Me.OriginalPP = 10
            Me.CurrentPP = 10
            Me.MaxPP = 10
            Me.Power = 50
            Me.Accuracy = 100
            Me.Category = Categories.Special
            Me.ContestCategory = ContestCategories.Beauty
            Me.Name = "Weather Ball"
            Me.Description = "An attack move that varies in power and type depending on the weather."
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
            Me.IsRecoilMove = False
            Me.IsPunchingMove = False
            Me.IsDamagingMove = True
            Me.IsProtectMove = False
            Me.IsSoundMove = False

            Me.IsAffectedBySubstitute = True
            Me.IsOneHitKOMove = False
            Me.IsWonderGuardAffected = True
            Me.IsBulletMove = True
            '#End
        End Sub

        Public Overrides Function GetAttackType(own As Boolean, BattleScreen As BattleScreen) As Element
            Dim a As BattleWeather.WeatherTypes = BattleScreen.FieldEffects.Weather
            Select Case a
                Case BattleWeather.WeatherTypes.Sunny
                    Return New Element(Element.Types.Fire)
                Case BattleWeather.WeatherTypes.Rain
                    Return New Element(Element.Types.Water)
                Case BattleWeather.WeatherTypes.Sandstorm
                    Return New Element(Element.Types.Rock)
                Case BattleWeather.WeatherTypes.Hailstorm
                    Return New Element(Element.Types.Ice)
                Case Else
                    Return New Element(Element.Types.Normal)
            End Select

            Return Me.Type
        End Function

        'Temporary 
        Public Overrides Function GetBasePower(own As Boolean, BattleScreen As BattleScreen) As Integer
            If BattleScreen.FieldEffects.Weather = BattleWeather.WeatherTypes.Sandstorm Or BattleScreen.FieldEffects.Weather = BattleWeather.WeatherTypes.Hailstorm Then
                Return CInt(Me.Power * 2)
            Else If BattleScreen.FieldEffects.Weather = BattleWeather.WeatherTypes.Sunny Or BattleScreen.FieldEffects.Weather = BattleWeather.WeatherTypes.Rain Then
                Return CInt(Me.Power * 3)
            Else
                Return Me.Power
            End If
        End Function

    End Class

End Namespace
