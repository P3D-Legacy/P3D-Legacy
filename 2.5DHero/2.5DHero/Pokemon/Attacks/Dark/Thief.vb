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
            Me.Name = "Thief"
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
            Me.HasSecondaryEffect = True
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
        End Sub

        Public Overrides Sub MoveHits(own As Boolean, BattleScreen As BattleScreen)
            Dim p As Pokemon = BattleScreen.OwnPokemon
            Dim op As Pokemon = BattleScreen.OppPokemon
            If own = False Then
                p = BattleScreen.OppPokemon
                op = BattleScreen.OwnPokemon
            End If

            Dim canSteal As Boolean = True
            If op.Ability.Name.ToLower() = "multitype" Then
                canSteal = False
            End If
            If Not op.Item Is Nothing AndAlso op.Item.Name.ToLower() = "griseous orb" And op.Number = 487 Then
                canSteal = False
            End If

            If canSteal = True Then
                If p.Item Is Nothing And Not op.Item Is Nothing Then
                    Dim ItemID As Integer = op.Item.ID

                    op.OriginalItem = Item.GetItemByID(op.Item.ID)
                    op.OriginalItem.AdditionalData = op.Item.AdditionalData

                    If BattleScreen.Battle.RemoveHeldItem(Not own, own, BattleScreen, "Thief stole the item " & op.Item.Name & " from " & op.GetDisplayName() & "!", "move:thief") = True Then
                        If own = False Then
                            BattleScreen.FieldEffects.StolenItemIDs.Add(ItemID)
                        End If

                        p.Item = Item.GetItemByID(ItemID)
                    End If
                End If
            End If
        End Sub

    End Class

End Namespace