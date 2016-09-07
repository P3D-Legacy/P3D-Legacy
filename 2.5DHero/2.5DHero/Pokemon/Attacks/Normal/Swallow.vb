Namespace BattleSystem.Moves.Normal

    Public Class Swallow

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Normal)
            Me.ID = 256
            Me.OriginalPP = 10
            Me.CurrentPP = 10
            Me.MaxPP = 10
            Me.Power = 0
            Me.Accuracy = 0
            Me.Category = Categories.Status
            Me.ContestCategory = ContestCategories.Tough
            Me.Name = "Swallow"
            Me.Description = "The power stored using the move Stockpile is absorbed by the user to heal its HP. Storing more power heals more HP."
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

            Me.AIField1 = AIField.Support
            Me.AIField2 = AIField.Nothing
        End Sub

        Public Overrides Function MoveFailBeforeAttack(Own As Boolean, BattleScreen As BattleScreen) As Boolean
            Dim stockpiled As Integer = BattleScreen.FieldEffects.OwnStockpileCount
            If Own = False Then
                stockpiled = BattleScreen.FieldEffects.OppStockpileCount
            End If

            If stockpiled = 0 Then
                BattleScreen.BattleQuery.Add(New TextQueryObject(Me.Name & " failed!"))
                Return True
            Else
                Return False
            End If
        End Function

        Public Overrides Sub MoveHits(own As Boolean, BattleScreen As BattleScreen)
            Dim stockpiled As Integer = BattleScreen.FieldEffects.OwnStockpileCount
            If own = False Then
                stockpiled = BattleScreen.FieldEffects.OppStockpileCount
            End If

            Dim p As Pokemon = BattleScreen.OwnPokemon
            If own = False Then
                p = BattleScreen.OppPokemon
            End If

            If own = True Then
                BattleScreen.FieldEffects.OwnStockpileCount = 0
            Else
                BattleScreen.FieldEffects.OppStockpileCount = 0
            End If

            Dim hpPercentage As Integer = 25
            Select Case stockpiled
                Case 1
                    hpPercentage = 25
                Case 2
                    hpPercentage = 50
                Case 3
                    hpPercentage = 100
            End Select
            Dim hpGain As Integer = CInt(Math.Ceiling((p.MaxHP / 100) * hpPercentage))

            BattleScreen.Battle.GainHP(hpGain, own, own, BattleScreen, p.GetDisplayName() & "'s HP was restored!", "move:swallow")

            BattleScreen.Battle.LowerStat(own, own, BattleScreen, "Defense", stockpiled, p.GetDisplayName() & "'s Defense fell!", "move:spitup")
            BattleScreen.Battle.LowerStat(own, own, BattleScreen, "Special Defense", stockpiled, p.GetDisplayName() & "'s Special Defense fell!", "move:spitup")

            BattleScreen.BattleQuery.Add(New TextQueryObject(p.GetDisplayName() & "'s stockpiled effect wore off!"))
        End Sub

    End Class

End Namespace