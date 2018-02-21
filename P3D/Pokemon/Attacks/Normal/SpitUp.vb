Namespace BattleSystem.Moves.Normal

    Public Class SpitUp

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Normal)
            Me.ID = 255
            Me.OriginalPP = 10
            Me.CurrentPP = 10
            Me.MaxPP = 10
            Me.Power = 0
            Me.Accuracy = 100
            Me.Category = Categories.Special
            Me.ContestCategory = ContestCategories.Tough
            Me.Name = "Spit Up"
            Me.Description = "The power stored using the move Stockpile is released at once in an attack. The more power is stored, the greater the damage."
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

        Public Overrides Function GetBasePower(own As Boolean, BattleScreen As BattleScreen) As Integer
            Dim stockpiled As Integer = BattleScreen.FieldEffects.OwnStockpileCount
            If own = False Then
                stockpiled = BattleScreen.FieldEffects.OppStockpileCount
            End If

            Return stockpiled * 100
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

            BattleScreen.Battle.LowerStat(own, own, BattleScreen, "Defense", stockpiled, p.GetDisplayName() & "'s Defense fell!", "move:spitup")
            BattleScreen.Battle.LowerStat(own, own, BattleScreen, "Special Defense", stockpiled, p.GetDisplayName() & "'s Special Defense fell!", "move:spitup")

            BattleScreen.BattleQuery.Add(New TextQueryObject(p.GetDisplayName() & "'s stockpiled effect wore off!"))
        End Sub

    End Class

End Namespace