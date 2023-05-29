Namespace BattleSystem.Moves.Psychic

    Public Class Trick

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Psychic)
            Me.ID = 271
            Me.OriginalPP = 10
            Me.CurrentPP = 10
            Me.MaxPP = 10
            Me.Power = 0
            Me.Accuracy = 100
            Me.Category = Categories.Status
            Me.ContestCategory = ContestCategories.Smart
            Me.Name = Localization.GetString("move_name_" & Me.ID,"Trick")
            Me.Description = "The user catches the target off guard and swaps its held item with its own."
            Me.CriticalChance = 0
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
            Me.KingsrockAffected = False
            Me.CounterAffected = False

            Me.DisabledWhileGravity = False
            Me.UseEffectiveness = False
            Me.ImmunityAffected = False
            Me.RemovesOwnFrozen = False
            Me.HasSecondaryEffect = False

            Me.IsHealingMove = False
            Me.IsRecoilMove = False

            Me.IsDamagingMove = False
            Me.IsProtectMove = False


            Me.IsAffectedBySubstitute = True
            Me.IsOneHitKOMove = False
            Me.IsWonderGuardAffected = False
            '#End

            Me.AIField1 = AIField.Support
            Me.AIField2 = AIField.Nothing
        End Sub

        Public Overrides Sub MoveHits(own As Boolean, BattleScreen As BattleScreen)
            Dim p As Pokemon = BattleScreen.OwnPokemon
            Dim op As Pokemon = BattleScreen.OppPokemon
            If own = False Then
                p = BattleScreen.OppPokemon
                op = BattleScreen.OwnPokemon
            End If
            Dim CanSwitchItems As Boolean = True
            If p.Item Is Nothing And op.Item Is Nothing Then
                CanSwitchItems = False
            End If
            If BattleScreen.FieldEffects.CanUseAbility(Not own, BattleScreen) AndAlso op.Ability.Name.ToLower() = "sticky hold" Then
                CanSwitchItems = False
            End If
            If Not p.Item Is Nothing AndAlso p.Item.OriginalName.ToLower() = "griseous orb" AndAlso p.Number = 487 Then
                CanSwitchItems = False
            End If
            If Not op.Item Is Nothing AndAlso op.Item.OriginalName.ToLower() = "griseous orb" AndAlso op.Number = 487 Then
                CanSwitchItems = False
            End If
            If Not p.Item Is Nothing AndAlso p.Item.OriginalName.ToLower().EndsWith(" drive") = True AndAlso p.Number = 649 Then
                CanSwitchItems = False
            End If
            If Not op.Item Is Nothing AndAlso op.Item.OriginalName.ToLower().EndsWith(" drive") = True AndAlso op.Number = 649 Then
                CanSwitchItems = False
            End If
            If Not p.Item Is Nothing AndAlso p.Item.OriginalName.ToLower().EndsWith(" plate") = True AndAlso p.Number = 493 Then
                CanSwitchItems = False
            End If
            If Not op.Item Is Nothing AndAlso op.Item.OriginalName.ToLower().EndsWith(" plate") = True AndAlso op.Number = 493 Then
                CanSwitchItems = False
            End If
            If Not p.Item Is Nothing AndAlso p.Item.OriginalName.ToLower().EndsWith(" memory") = True AndAlso p.Number = 773 Then
                CanSwitchItems = False
            End If
            If Not op.Item Is Nothing AndAlso op.Item.OriginalName.ToLower().EndsWith(" memory") = True AndAlso op.Number = 773 Then
                CanSwitchItems = False
            End If
            If Not p.Item Is Nothing AndAlso p.Item.OriginalName.ToLower().EndsWith(" mail") = True Then
                CanSwitchItems = False
            End If
            If Not op.Item Is Nothing AndAlso op.Item.OriginalName.ToLower().EndsWith(" mail") = True Then
                CanSwitchItems = False
            End If
            If (p.Item IsNot Nothing AndAlso p.Item.IsMegaStone) OrElse (op.Item IsNot Nothing AndAlso op.Item.IsMegaStone) Then
                CanSwitchItems = False
            End If

            If CanSwitchItems Then
                Dim i1 As Item = Nothing
                Dim i2 As Item = Nothing
                If own = True Then
                    If p.Item IsNot Nothing Then
                        p.OriginalItem = p.Item
                    End If
                Else
                    If op.Item IsNot Nothing Then
                        op.OriginalItem = op.Item
                    End If
                End If
                If p.Item IsNot Nothing Then
                    i1 = p.Item
                End If
                If op.Item IsNot Nothing Then
                    i2 = op.Item
                End If
                p.Item = i2
                op.Item = i1

                If p.Item IsNot Nothing AndAlso p.OriginalItem IsNot Nothing Then
                    Dim pItemID As String = p.Item.ID.ToString
                    Dim pOriginalItemID As String = p.OriginalItem.ID.ToString
                    If p.Item.IsGameModeItem = True Then
                        pItemID = p.Item.gmID
                    End If
                    If p.OriginalItem.IsGameModeItem = True Then
                        pOriginalItemID = p.Item.gmID
                    End If

                    If pItemID = pOriginalItemID AndAlso p.Item.AdditionalData = p.OriginalItem.AdditionalData Then
                        p.OriginalItem = Nothing
                    End If
                End If
                If op.Item IsNot Nothing AndAlso op.OriginalItem IsNot Nothing Then
                    Dim opItemID As String = op.Item.ID.ToString
                    Dim opOriginalItemID As String = op.OriginalItem.ID.ToString
                    If op.Item.IsGameModeItem = True Then
                        opItemID = op.Item.gmID
                    End If
                    If op.OriginalItem.IsGameModeItem = True Then
                        opOriginalItemID = op.Item.gmID
                    End If

                    If op.Item.ID = op.OriginalItem.ID AndAlso op.Item.AdditionalData = op.OriginalItem.AdditionalData Then
                        p.OriginalItem = Nothing
                    End If
                End If

                BattleScreen.BattleQuery.Add(New TextQueryObject(p.GetDisplayName() & " switched items with " & op.GetDisplayName() & "."))
            Else
                BattleScreen.BattleQuery.Add(New TextQueryObject(Me.Name & " failed!"))
            End If
        End Sub

    End Class

End Namespace