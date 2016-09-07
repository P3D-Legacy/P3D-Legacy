Namespace BattleSystem.Moves.Dark

    Public Class KnockOff

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Dark)
            Me.ID = 282
            Me.OriginalPP = 25
            Me.CurrentPP = 25
            Me.MaxPP = 25
            Me.Power = 65
            Me.Accuracy = 100
            Me.Category = Categories.Physical
            Me.ContestCategory = ContestCategories.Smart
            Me.Name = "Knock Off"
            Me.Description = "The user slaps down the target's held item, and that item can't be used in that battle. The move does more damage if the target has a held item."
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
            Me.KingsrockAffected = False
            Me.CounterAffected = True

            Me.DisabledWhileGravity = False
            Me.UseEffectiveness = True
            Me.ImmunityAffected = True
            Me.RemovesFrozen = False
            Me.HasSecondaryEffect = False

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
        End Sub

        Public Overrides Function GetBasePower(own As Boolean, BattleScreen As BattleScreen) As Integer
            If BattleScreen.Battle.RemoveHeldItem(Not own, own, BattleScreen, "", "", True) = True Then
                Return CInt(Me.Power * 1.5F)
            End If

            Return Me.Power
        End Function

        Public Overrides Sub MoveHits(own As Boolean, BattleScreen As BattleScreen)
            Dim p As Pokemon = BattleScreen.OwnPokemon
            Dim op As Pokemon = BattleScreen.OppPokemon
            If own = False Then
                p = BattleScreen.OppPokemon
                op = BattleScreen.OwnPokemon
            End If

            If BattleScreen.Battle.RemoveHeldItem(Not own, own, BattleScreen, "", "", True) = True Then
                op.OriginalItem = Item.GetItemByID(op.Item.ID)
                op.OriginalItem.AdditionalData = op.Item.AdditionalData

                BattleScreen.Battle.RemoveHeldItem(Not own, own, BattleScreen, p.GetDisplayName() & " knocked off the " & op.GetDisplayName() & "'s " & op.OriginalItem.Name & "!", "move:knockoff")
            End If
        End Sub

    End Class

End Namespace