Namespace BattleSystem.Moves.Fire

    Public Class Incinerate

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Fire)
            Me.ID = 510
            Me.OriginalPP = 15
            Me.CurrentPP = 15
            Me.MaxPP = 15
            Me.Power = 60
            Me.Accuracy = 100
            Me.Category = Categories.Special
            Me.ContestCategory = ContestCategories.Tough
            Me.Name = Localization.GetString("move_name_" & Me.ID,"Incinerate")
            Me.Description = "The user attacks opposing Pokémon with fire. If a Pokémon is holding a certain item, such as a Berry, the item becomes burned up and unusable."
            Me.CriticalChance = 1
            Me.IsHMMove = False
            Me.Target = Targets.AllAdjacentFoes
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
            Me.HasSecondaryEffect = True
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

            If Not op.Item Is Nothing Then
                If op.Item.IsBerry = True OrElse op.Item.OriginalName.ToLower().EndsWith(" gem") Then
                    Dim ItemID As Integer = op.Item.ID
                    BattleScreen.Battle.RemoveHeldItem(Not own, own, BattleScreen, op.GetDisplayName() & "'s " & op.Item.OneLineName() & " got burned up!", "move:incinerate")
                End If
            End If
        End Sub

    End Class

End Namespace