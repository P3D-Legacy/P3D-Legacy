Namespace BattleSystem.Moves.Normal

    Public Class Stockpile

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Normal)
            Me.ID = 254
            Me.OriginalPP = 20
            Me.CurrentPP = 20
            Me.MaxPP = 20
            Me.Power = 0
            Me.Accuracy = 0
            Me.Category = Categories.Status
            Me.ContestCategory = ContestCategories.Tough
            Me.Name = "Stockpile"
            Me.Description = "The user charges up power and raises both its Defense and Sp. Def. The move can be used three times."
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

            Me.AIField1 = AIField.RaiseDefense
            Me.AIField2 = AIField.RaiseSpDefense
        End Sub

        Public Overrides Sub MoveHits(own As Boolean, BattleScreen As BattleScreen)
            Dim stockpiled As Integer = BattleScreen.FieldEffects.OwnStockpileCount
            If own = False Then
                stockpiled = BattleScreen.FieldEffects.OppStockpileCount
            End If

            Dim p As Pokemon = BattleScreen.OwnPokemon
            If own = False Then
                p = BattleScreen.OppPokemon
            End If

            If stockpiled < 3 Then
                If own = True Then
                    BattleScreen.FieldEffects.OwnStockpileCount += 1
                Else
                    BattleScreen.FieldEffects.OppStockpileCount += 1
                End If
                BattleScreen.BattleQuery.Add(New TextQueryObject(p.GetDisplayName() & " stockpiled " & (stockpiled + 1).ToString() & "!"))

                BattleScreen.Battle.RaiseStat(own, own, BattleScreen, "Defense", 1, "", "move:stockpile")
                BattleScreen.Battle.RaiseStat(own, own, BattleScreen, "Special Defense", 1, "", "move:stockpile")
            Else
                BattleScreen.BattleQuery.Add(New TextQueryObject(Me.Name & " failed!"))
            End If
        End Sub

    End Class

End Namespace