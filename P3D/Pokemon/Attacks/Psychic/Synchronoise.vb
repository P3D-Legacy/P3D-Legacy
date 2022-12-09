Namespace BattleSystem.Moves.Psychic

    Public Class Synchronoise

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Psychic)
            Me.ID = 485
            Me.OriginalPP = 10
            Me.CurrentPP = 10
            Me.MaxPP = 10
            Me.Power = 120
            Me.Accuracy = 100
            Me.Category = Categories.Special
            Me.ContestCategory = ContestCategories.Smart
            Me.Name = Localization.GetString("move_name_" & Me.ID,"Synchronoise")
            Me.Description = "Using an odd shock wave, the user inflicts damage on any Pokémon of the same type in the area around it."
            Me.CriticalChance = 1
            Me.IsHMMove = False
            Me.Target = Targets.AllAdjacentTargets
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

            Me.AIField1 = AIField.Damage
            Me.AIField2 = AIField.Nothing
        End Sub

        Public Overrides Function MoveFailBeforeAttack(Own As Boolean, BattleScreen As BattleScreen) As Boolean
            Dim p As Pokemon = BattleScreen.OwnPokemon
            Dim op As Pokemon = BattleScreen.OppPokemon
            If own = False Then
                p = BattleScreen.OppPokemon
                op = BattleScreen.OwnPokemon
            End If

            If op.Type2.Type = Element.Types.Blank Then
                'Solo type
                If op.Type1.Type = p.Type1.Type OrElse op.Type1.Type = p.Type2.Type Then
                    Return False
                Else
                    BattleScreen.BattleQuery.Add(New TextQueryObject(Me.Name & " failed!"))
                    Return True
                End If
            Else
                'Dual type
                If op.Type1.Type = p.Type1.Type OrElse op.Type1.Type = p.Type2.Type OrElse op.Type2.Type = p.Type1.Type OrElse op.Type2.Type = p.Type2.Type Then
                    Return False
                Else
                    BattleScreen.BattleQuery.Add(New TextQueryObject(Me.Name & " failed!"))
                    Return True
                End If
            End If

        End Function

    End Class

End Namespace