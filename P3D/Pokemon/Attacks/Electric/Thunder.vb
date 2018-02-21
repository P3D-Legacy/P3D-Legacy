Namespace BattleSystem.Moves.Electric

    Public Class Thunder

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Electric)
            Me.ID = 87
            Me.OriginalPP = 10
            Me.CurrentPP = 10
            Me.MaxPP = 10
            Me.Power = 110
            Me.Accuracy = 70
            Me.Category = Categories.Special
            Me.ContestCategory = ContestCategories.Cool
            Me.Name = "Thunder"
            Me.Description = "A wicked thunderbolt is dropped on the target to inflict damage. It may also leave the target with paralysis."
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
            Me.KingsrockAffected = False
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
            Me.CanHitInMidAir = True
            '#End

            Me.AIField1 = AIField.Damage
            Me.AIField2 = AIField.CanParalyse

            EffectChances.Add(30)
        End Sub

        Public Overrides Sub MoveHits(own As Boolean, BattleScreen As BattleScreen)
            Dim p As Pokemon = BattleScreen.OwnPokemon
            Dim op As Pokemon = BattleScreen.OppPokemon
            If own = False Then
                p = BattleScreen.OppPokemon
                op = BattleScreen.OwnPokemon
            End If

            Dim chance As Integer = GetEffectChance(0, own, BattleScreen)
            If Core.Random.Next(0, 100) < chance Then
                BattleScreen.Battle.InflictParalysis(Not own, own, BattleScreen, "", "move:thunder")
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