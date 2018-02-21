Namespace BattleSystem.Moves.Normal

    Public Class Substitute

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Normal)
            Me.ID = 164
            Me.OriginalPP = 10
            Me.CurrentPP = 10
            Me.MaxPP = 10
            Me.Power = 0
            Me.Accuracy = 0
            Me.Category = Categories.Status
            Me.ContestCategory = ContestCategories.Smart
            Me.Name = "Substitute"
            Me.Description = "The user makes a copy of itself using some of its HP. The copy serves as the user’s decoy."
            Me.CriticalChance = 0
            Me.IsHMMove = False
            Me.Target = Targets.Self
            Me.Priority = 0
            Me.TimesToAttack = 1
            '#End

            '#SpecialDefinitions
            Me.MakesContact = False
            Me.ProtectAffected = False
            Me.MagicCoatAffected = False
            Me.SnatchAffected = True
            Me.MirrorMoveAffected = True
            Me.KingsrockAffected = False
            Me.CounterAffected = False

            Me.DisabledWhileGravity = False
            Me.UseEffectiveness = False
            Me.ImmunityAffected = False
            Me.HasSecondaryEffect = True
            Me.RemovesFrozen = False

            Me.IsHealingMove = False
            Me.IsRecoilMove = False
            Me.IsPunchingMove = False
            Me.IsDamagingMove = False
            Me.IsProtectMove = False
            Me.IsSoundMove = False

            Me.IsAffectedBySubstitute = False
            Me.IsOneHitKOMove = False
            Me.IsWonderGuardAffected = False
            '#End
        End Sub

        Public Overrides Sub MoveHits(own As Boolean, BattleScreen As BattleScreen)
            Dim fails As Boolean = False
            Dim looseHP As Integer = 0

            Dim p As Pokemon = BattleScreen.OwnPokemon
            If own = False Then
                p = BattleScreen.OppPokemon
            End If

            If p.MaxHP = 1 Or p.HP <= CInt(Math.Floor(p.MaxHP / 4)) Then
                fails = True
            Else
                If p.MaxHP > 3 Then
                    looseHP = CInt(Math.Floor(p.MaxHP / 4))
                End If
            End If

            If looseHP > 0 And fails = False Then
                If looseHP >= p.HP Then
                    looseHP -= 1
                End If
            End If

            If fails = False Then
                BattleScreen.Battle.ReduceHP(looseHP, own, own, BattleScreen, p.GetDisplayName() & " put in a substitute!", "move:substitute")
                If own = True Then
                    BattleScreen.BattleQuery.Add(New ToggleEntityQueryObject(True, ToggleEntityQueryObject.BattleEntities.OwnPokemon, "Substitute", 0, 1, -1, -1))
                    BattleScreen.FieldEffects.OwnSubstitute = looseHP + 1
                Else
                    BattleScreen.BattleQuery.Add(New ToggleEntityQueryObject(False, ToggleEntityQueryObject.BattleEntities.OwnPokemon, "Substitute", 0, 1, -1, -1))
                    BattleScreen.FieldEffects.OppSubstitute = looseHP + 1
                End If
            Else
                BattleScreen.BattleQuery.Add(New TextQueryObject(Me.Name & " failed!"))
            End If
        End Sub

    End Class

End Namespace