Namespace BattleSystem.Moves.Normal

    Public Class PayDay

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Normal)
            Me.ID = 6
            Me.OriginalPP = 20
            Me.CurrentPP = 20
            Me.MaxPP = 20
            Me.Power = 40
            Me.Accuracy = 100
            Me.Category = Categories.Physical
            Me.ContestCategory = ContestCategories.Smart
            Me.Name = Localization.GetString("move_name_" & Me.ID,"Pay Day")
            Me.Description = "Numerous coins are hurled at the target to inflict damage. Money is earned after the battle."
            Me.CriticalChance = 1
            Me.IsHMMove = False
            Me.Target = Targets.OneAdjacentTarget
            Me.Priority = 0
            Me.TimesToAttack = 1
            '#End

            '#SpecialDefinitions
            Me.MakesContact = True
            Me.ProtectAffected = True
            Me.MagicCoatAffected = False
            Me.SnatchAffected = False
            Me.MirrorMoveAffected = True
            Me.KingsrockAffected = True
            Me.CounterAffected = True

            Me.DisabledWhileGravity = False
            Me.UseEffectiveness = True
            Me.ImmunityAffected = True
            Me.RemovesFrozen = False
            Me.HasSecondaryEffect = False

            Me.IsHealingMove = False
            Me.IsRecoilMove = False

            Me.IsDamagingMove = True
            Me.IsProtectMove = False


            Me.IsAffectedBySubstitute = True
            Me.IsOneHitKOMove = False
            Me.IsWonderGuardAffected = True
            '#End
        End Sub

        Public Overrides Sub MoveHits(own As Boolean, BattleScreen As BattleScreen)
            Dim p As Pokemon = BattleScreen.OwnPokemon
            Dim op As Pokemon = BattleScreen.OppPokemon
            If own = False Then
                p = BattleScreen.OppPokemon
                op = BattleScreen.OwnPokemon
            End If

            Dim coinAmount As Integer = p.Level * 5

            If Not p.Item Is Nothing Then
                If p.Item.OriginalName.ToLower() = "amulet coin" Or p.Item.OriginalName.ToLower() = "luck incense" Then
                    coinAmount *= 2
                End If
            End If

            For Each mysteryEvent As MysteryEventScreen.MysteryEvent In MysteryEventScreen.ActivatedMysteryEvents
                If mysteryEvent.EventType = MysteryEventScreen.EventTypes.MoneyMultiplier Then
                    coinAmount = CInt(coinAmount * CDbl(mysteryEvent.Value.Replace(".", GameController.DecSeparator)))
                End If
            Next

            If own = True Then
                BattleScreen.FieldEffects.OwnPayDayCounter += coinAmount
            Else
                BattleScreen.FieldEffects.OppPayDayCounter += coinAmount
            End If

            BattleScreen.BattleQuery.Add(New TextQueryObject("Coins were scattered everywhere!"))
        End Sub

    End Class

End Namespace