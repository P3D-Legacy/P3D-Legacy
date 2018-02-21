Namespace BattleSystem.Moves.Normal

    Public Class Growth

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Normal)
            Me.ID = 74
            Me.OriginalPP = 40
            Me.CurrentPP = 40
            Me.MaxPP = 40
            Me.Power = 0
            Me.Accuracy = 0
            Me.Category = Categories.Status
            Me.ContestCategory = ContestCategories.Beauty
            Me.Name = "Growth"
            Me.Description = "The user's body grows all at once, raising the Attack and Sp. Atk stats."
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

            Me.AIField1 = AIField.RaiseSpAttack
            Me.AIField2 = AIField.Nothing
        End Sub

        Public Overrides Sub MoveHits(own As Boolean, BattleScreen As BattleScreen)
            Dim statRaise As Integer = 1
            If BattleScreen.FieldEffects.Weather = BattleWeather.WeatherTypes.Sunny Then
                statRaise = 2
            End If

            Dim b As Boolean = False

            If BattleScreen.Battle.RaiseStat(own, own, BattleScreen, "Attack", statRaise, "", "move:growth") = True Then
                b = True
            End If
            If BattleScreen.Battle.RaiseStat(own, own, BattleScreen, "Special Attack", statRaise, "", "move:growth") = True Then
                b = True
            End If

            If b = False Then
                BattleScreen.BattleQuery.Add(New TextQueryObject(Me.Name & " failed!"))
            End If
        End Sub

    End Class

End Namespace