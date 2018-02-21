Namespace BattleSystem.Moves.Grass

    Public Class LeechSeed

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Grass)
            Me.ID = 73
            Me.OriginalPP = 10
            Me.CurrentPP = 10
            Me.MaxPP = 10
            Me.Power = 0
            Me.Accuracy = 90
            Me.Category = Categories.Status
            Me.ContestCategory = ContestCategories.Smart
            Me.Name = "Leech Seed"
            Me.Description = "A seed is planted on the target. It steals some HP from the target every turn."
            Me.CriticalChance = 0
            Me.IsHMMove = False
            Me.Target = Targets.OneAdjacentTarget
            Me.Priority = 0
            Me.TimesToAttack = 1
            '#End

            '#SpecialDefinitions
            Me.MakesContact = False
            Me.ProtectAffected = True
            Me.MagicCoatAffected = True
            Me.SnatchAffected = False
            Me.MirrorMoveAffected = True
            Me.KingsrockAffected = False
            Me.CounterAffected = False

            Me.DisabledWhileGravity = False
            Me.UseEffectiveness = False
            Me.ImmunityAffected = True
            Me.HasSecondaryEffect = False
            Me.RemovesFrozen = False

            Me.IsHealingMove = False
            Me.IsRecoilMove = False
            Me.IsPunchingMove = False
            Me.IsDamagingMove = False
            Me.IsProtectMove = False
            Me.IsSoundMove = False

            Me.IsAffectedBySubstitute = False
            Me.IsOneHitKOMove = False
            Me.IsWonderGuardAffected = True
            Me.IsPowderMove = True
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

            Dim failed As Boolean = False

            If op.Type1.Type <> Element.Types.Grass And op.Type2.Type <> Element.Types.Grass Then
                If own = True Then
                    If BattleScreen.FieldEffects.OppLeechSeed = 0 Then
                        BattleScreen.FieldEffects.OppLeechSeed = 1
                    Else
                        failed = True
                    End If
                Else
                    If BattleScreen.FieldEffects.OwnLeechSeed = 0 Then
                        BattleScreen.FieldEffects.OwnLeechSeed = 1
                    Else
                        failed = True
                    End If
                End If
            Else
                failed = True
            End If

            If failed = False Then
                BattleScreen.BattleQuery.Add(New TextQueryObject(op.GetDisplayName() & " was seeded!"))
            Else
                BattleScreen.BattleQuery.Add(New TextQueryObject(Me.Name & " failed!"))
            End If
        End Sub

    End Class

End Namespace