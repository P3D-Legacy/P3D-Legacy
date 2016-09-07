Namespace BattleSystem.Moves.Grass

    Public Class Synthesis

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Grass)
            Me.ID = 235
            Me.OriginalPP = 5
            Me.CurrentPP = 5
            Me.MaxPP = 5
            Me.Power = 0
            Me.Accuracy = 0
            Me.Category = Categories.Status
            Me.ContestCategory = ContestCategories.Smart
            Me.Name = "Synthesis"
            Me.Description = "The user restores its own HP. The amount of HP regained varies with the weather."
            Me.CriticalChance = 0
            Me.IsHMMove = False
            Me.Target = Targets.Self
            Me.Priority = 0
            Me.TimesToAttack = 1
            '#End

            '#SpecialDefinitions
            Me.MakesContact = False
            Me.ProtectAffected = False
            Me.MagicCoatAffected = False
            Me.SnatchAffected = True
            Me.MirrorMoveAffected = True
            Me.KingsrockAffected = False
            Me.CounterAffected = False

            Me.DisabledWhileGravity = False
            Me.UseEffectiveness = False
            Me.ImmunityAffected = False
            Me.HasSecondaryEffect = False
            Me.RemovesFrozen = False

            Me.IsHealingMove = True
            Me.IsRecoilMove = False
            Me.IsPunchingMove = False
            Me.IsDamagingMove = False
            Me.IsProtectMove = False
            Me.IsSoundMove = False

            Me.IsAffectedBySubstitute = False
            Me.IsOneHitKOMove = False
            Me.IsWonderGuardAffected = False
            '#End

            Me.AIField1 = AIField.Healing
            Me.AIField2 = AIField.Nothing
        End Sub

        Public Overrides Sub MoveHits(own As Boolean, BattleScreen As BattleScreen)
            Dim p As Pokemon = BattleScreen.OwnPokemon
            If own = False Then
                p = BattleScreen.OppPokemon
            End If

            Dim restoreHP As Integer = 1
            Select Case BattleScreen.FieldEffects.Weather
                Case BattleWeather.WeatherTypes.Sunny
                    restoreHP = CInt(Math.Ceiling((2 / 3) * p.MaxHP))
                Case BattleWeather.WeatherTypes.Clear
                    restoreHP = CInt(Math.Ceiling((1 / 2) * p.MaxHP))
                Case Else
                    restoreHP = CInt(Math.Ceiling((1 / 4) * p.MaxHP))
            End Select

            If p.HP < p.MaxHP And p.HP > 0 Then
                BattleScreen.Battle.GainHP(restoreHP, own, own, BattleScreen, p.GetDisplayName() & "'s HP was restored!", "move:synthesis")
            Else
                BattleScreen.BattleQuery.Add(New TextQueryObject(Me.Name & " failed!"))
            End If
        End Sub

    End Class

End Namespace