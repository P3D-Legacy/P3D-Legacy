Namespace BattleSystem.Moves.Normal

    Public Class Bestow

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Normal)
            Me.ID = 516
            Me.OriginalPP = 15
            Me.CurrentPP = 15
            Me.MaxPP = 15
            Me.Power = 0
            Me.Accuracy = 0
            Me.Category = Categories.Status
            Me.ContestCategory = ContestCategories.Cute
            Me.Name = "Bestow"
            Me.Description = "The user passes its held item to the target when the target isn't holding an item."
            Me.CriticalChance = 0
            Me.IsHMMove = False
            Me.Target = Targets.OneAdjacentTarget
            Me.Priority = 0
            Me.TimesToAttack = 1
            '#End

            '#SpecialDefinitions
            Me.MakesContact = False
            Me.ProtectAffected = False
            Me.MagicCoatAffected = False
            Me.SnatchAffected = False
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

            Me.IsAffectedBySubstitute = True
            Me.IsOneHitKOMove = False
            Me.IsWonderGuardAffected = False
            Me.UseAccEvasion = False
            '#End

            Me.AIField1 = AIField.Support
            Me.AIField2 = AIField.CannotMiss
        End Sub

        Public Overrides Sub MoveHits(own As Boolean, BattleScreen As BattleScreen)
            Dim p As Pokemon = BattleScreen.OwnPokemon
            Dim op As Pokemon = BattleScreen.OppPokemon
            If own = False Then
                p = BattleScreen.OppPokemon
                op = BattleScreen.OwnPokemon
            End If

            If p.Item Is Nothing Then
                Exit Sub
            End If
            If p.Item.IsMegaStone = True Then
                Exit Sub
            End If
            If p.Ability.Name.ToLower() = "multitype" AndAlso p.Item.Name.ToLower().EndsWith(" plate") Then
                Exit Sub
            End If
            If p.Ability.Name.ToLower() = "rks system" AndAlso p.Item.Name.ToLower().EndsWith(" memory") Then
                Exit Sub
            End If
            If p.Item.Name.ToLower() = "griseous orb" And p.Number = 487 Then
                Exit Sub
            End If
            If p.Item.Name.ToLower().EndsWith(" drive") = True AndAlso p.Number = 649 Then
                Exit Sub
            End If
            If p.Item.Name.ToLower().EndsWith(" mail") = True Then
                Exit Sub
            End If

            If op.Item Is Nothing Then
                Dim ItemID As Integer = p.Item.ID

                p.OriginalItem = Item.GetItemByID(p.Item.ID)
                p.OriginalItem.AdditionalData = p.Item.AdditionalData

                If BattleScreen.Battle.RemoveHeldItem(own, own, BattleScreen, op.GetDisplayName() & " received the item " & p.Item.Name & " from " & p.GetDisplayName() & "!", "move:bestow") Then
                    If own = False Then
                        BattleScreen.FieldEffects.StolenItemIDs.Add(ItemID)
                    End If
                    op.Item = Item.GetItemByID(ItemID)
                End If
            End If

        End Sub

    End Class

End Namespace