Namespace BattleSystem.Moves.Normal

    Public Class Covet

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Normal)
            Me.ID = 343
            Me.OriginalPP = 25
            Me.CurrentPP = 25
            Me.MaxPP = 25
            Me.Power = 60
            Me.Accuracy = 100
            Me.Category = Categories.Physical
            Me.ContestCategory = ContestCategories.Cute
            Me.Name = Localization.GetString("move_name_" & Me.ID,"Covet")
            Me.Description = "The user endearingly approaches the target, then steals the target's held item."
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
            Me.HasSecondaryEffect = False
            Me.RemovesOwnFrozen = False

            Me.IsHealingMove = False
            Me.IsRecoilMove = False

            Me.IsDamagingMove = True
            Me.IsProtectMove = False


            Me.IsAffectedBySubstitute = True
            Me.IsOneHitKOMove = False
            Me.IsWonderGuardAffected = True
            '#End

            Me.AIField1 = AIField.Damage
            Me.AIField2 = AIField.Nothing
        End Sub

        Public Overrides Sub MoveHits(own As Boolean, BattleScreen As BattleScreen)
            Dim p As Pokemon = BattleScreen.OwnPokemon
            Dim op As Pokemon = BattleScreen.OppPokemon
            If own = False Then
                p = BattleScreen.OppPokemon
                op = BattleScreen.OwnPokemon
            End If

            'Conditions
            If op.Item Is Nothing Then
                Exit Sub
            End If
            If op.Item.IsMegaStone = True Then
                Exit Sub
            End If
            If op.Ability.Name.ToLower() = "multitype" AndAlso op.Item.OriginalName.ToLower().EndsWith(" plate") Then
                Exit Sub
            End If
            If op.Ability.Name.ToLower() = "rks system" AndAlso op.Item.OriginalName.ToLower().EndsWith(" memory") Then
                Exit Sub
            End If
            If op.Item.OriginalName.ToLower() = "griseous orb" And op.Number = 487 Then
                Exit Sub
            End If
            If op.Item.OriginalName.ToLower().EndsWith(" drive") = True AndAlso op.Number = 649 Then
                Exit Sub
            End If
            If op.Item.OriginalName.ToLower().EndsWith(" mail") = True Then
                Exit Sub
            End If

            If p.Item Is Nothing Then
                Dim ItemID As String = op.Item.ID.ToString
                If op.Item.IsGameModeItem = True Then
                    ItemID = op.Item.gmID
                End If

                op.OriginalItem = Item.GetItemByID(ItemID)
                op.OriginalItem.AdditionalData = op.Item.AdditionalData

                If BattleScreen.Battle.RemoveHeldItem(Not own, own, BattleScreen, "Covet stole the item " & op.Item.Name & " from " & op.GetDisplayName() & "!", "move:covet") Then
                    If own = False Then
                        BattleScreen.FieldEffects.StolenItemIDs.Add(ItemID)
                    End If
                    p.Item = Item.GetItemByID(ItemID)
                End If
            End If
        End Sub
    End Class

End Namespace