Namespace BattleSystem.Moves.Dark

    Public Class Thief

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Dark)
            Me.ID = 168
            Me.OriginalPP = 25
            Me.CurrentPP = 25
            Me.MaxPP = 25
            Me.Power = 60
            Me.Accuracy = 100
            Me.Category = Categories.Physical
            Me.ContestCategory = ContestCategories.Tough
            Me.Name = Localization.GetString("move_name_" & Me.ID,"Thief")
            Me.Description = "The user attacks and steals the target's item simultaneously. It can't steal if the user holds an item."
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
                Dim ItemID As String
                If p.Item.IsGameModeItem = True Then
                    ItemID = p.Item.gmID
                Else
                    ItemID = p.Item.ID.ToString
                End If

                op.OriginalItem = Item.GetItemByID(ItemID)
                op.OriginalItem.AdditionalData = op.Item.AdditionalData

                If BattleScreen.Battle.RemoveHeldItem(Not own, own, BattleScreen, "Thief stole the item " & op.Item.Name & " from " & op.GetDisplayName() & "!", "move:thief") Then
                    If own = False Then
                        BattleScreen.FieldEffects.StolenItemIDs.Add(ItemID)
                    End If
                    p.Item = Item.GetItemByID(ItemID)
                End If
            End If
        End Sub

    End Class

End Namespace
