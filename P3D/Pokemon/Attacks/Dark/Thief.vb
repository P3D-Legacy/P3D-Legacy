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
            If op.Item.OriginalName.ToLower() = "griseous orb" And op.OriginalName.ToLower.Contains("giratina") Then
                Exit Sub
            End If
            If op.Item.OriginalName.ToLower().EndsWith(" drive") = True AndAlso op.OriginalName.ToLower.Contains("genesect") Then
                Exit Sub
            End If
            If op.Item.IsMail = True Then
                Exit Sub
            End If

            If p.Item Is Nothing Then
                Dim item As Item = op.Item
                If op.OriginalItem Is Nothing Then
                    op.OriginalItem = item
                End If

                If BattleScreen.Battle.RemoveHeldItem(Not own, own, BattleScreen, "Thief stole the item " & op.Item.OneLineName() & " from " & op.GetDisplayName() & "!", "move:thief",, True) Then
                    If own = True AndAlso BattleScreen.FieldEffects.StolenFromOppItems.ContainsKey(BattleScreen.OppPokemonIndex) = False Then
                        BattleScreen.FieldEffects.StolenFromOppItems.Add(BattleScreen.OppPokemonIndex, item)
                    End If
                    p.Item = item

                    If p.Item IsNot Nothing AndAlso p.OriginalItem IsNot Nothing Then
                        Dim pItemID As String
                        If p.Item.IsGameModeItem = True Then
                            pItemID = p.Item.gmID
                        Else
                            pItemID = p.Item.ID.ToString
                        End If
                        Dim pOriginalItemID As String
                        If p.OriginalItem.IsGameModeItem = True Then
                            pOriginalItemID = p.OriginalItem.gmID
                        Else
                            pOriginalItemID = p.OriginalItem.ID.ToString
                        End If

                        If pItemID = pOriginalItemID AndAlso p.Item.AdditionalData = p.OriginalItem.AdditionalData Then
                            p.OriginalItem = Nothing

                            If own = True Then
                                If BattleScreen.FieldEffects.StolenFromOwnItems.ContainsKey(BattleScreen.OwnPokemonIndex) Then
                                    BattleScreen.FieldEffects.StolenFromOwnItems.Remove(BattleScreen.OwnPokemonIndex)
                                End If
                            Else
                                If BattleScreen.FieldEffects.StolenFromOppItems.ContainsKey(BattleScreen.OppPokemonIndex) Then
                                    BattleScreen.FieldEffects.StolenFromOppItems.Remove(BattleScreen.OppPokemonIndex)
                                End If
                            End If
                        End If
                    End If
                Else
                    If op.OriginalItem.ID = op.Item.ID AndAlso op.OriginalItem.AdditionalData = op.Item.AdditionalData Then
                        op.OriginalItem = Nothing
                    End If
                End If
            End If
        End Sub

    End Class

End Namespace
