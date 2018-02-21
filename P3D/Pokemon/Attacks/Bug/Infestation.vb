Namespace BattleSystem.Moves.Bug

    Public Class Infestation

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Bug)
            Me.ID = 611
            Me.OriginalPP = 20
            Me.CurrentPP = 20
            Me.MaxPP = 20
            Me.Power = 20
            Me.Accuracy = 100
            Me.Category = Categories.Special
            Me.ContestCategory = ContestCategories.Cute
            Me.Name = "Infestation"
            Me.Description = "The target is infested and attacked for four to five turns. The target can't flee during this time."
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
            Me.MirrorMoveAffected = False
            Me.KingsrockAffected = True
            Me.CounterAffected = True

            Me.DisabledWhileGravity = False
            Me.UseEffectiveness = True
            Me.ImmunityAffected = True
            Me.HasSecondaryEffect = False
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

            Me.AIField1 = AIField.Damage
            Me.AIField2 = AIField.Trap
        End Sub

        Public Overrides Sub MoveHits(own As Boolean, BattleScreen As BattleScreen)
            Dim p As Pokemon = BattleScreen.OwnPokemon
            Dim op As Pokemon = BattleScreen.OppPokemon
            If own = False Then
                p = BattleScreen.OppPokemon
                op = BattleScreen.OwnPokemon
            End If

            Dim turns As Integer = 4
            If Core.Random.Next(0, 100) < 50 Then
                turns = 5
            End If

            If Not p.Item Is Nothing Then
                If p.Item.Name.ToLower() = "grip claw" And BattleScreen.FieldEffects.CanUseItem(own) = True And BattleScreen.FieldEffects.CanUseOwnItem(own, BattleScreen) = True Then
                    turns = 5
                End If
            End If

            If own = True Then
                If BattleScreen.FieldEffects.OppInfestation = 0 Then
                    BattleScreen.FieldEffects.OppInfestation = turns
                    BattleScreen.BattleQuery.Add(New TextQueryObject(op.GetDisplayName() & " was trapped by " & p.GetDisplayName() & "!"))
                End If
            Else
                If BattleScreen.FieldEffects.OwnInfestation = 0 Then
                    BattleScreen.FieldEffects.OwnInfestation = turns
                    BattleScreen.BattleQuery.Add(New TextQueryObject(op.GetDisplayName() & " was trapped by " & p.GetDisplayName() & "!"))
                End If
            End If
        End Sub

    End Class

End Namespace