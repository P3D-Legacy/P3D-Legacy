Namespace BattleSystem.Moves.Normal

    Public Class MindReader

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Normal)
            Me.ID = 170
            Me.OriginalPP = 5
            Me.CurrentPP = 5
            Me.MaxPP = 5
            Me.Power = 0
            Me.Accuracy = 0
            Me.Category = Categories.Status
            Me.ContestCategory = ContestCategories.Smart
            Me.Name = "Mind Reader"
            Me.Description = "The user senses the target's movements with its mind to ensure its next attack does not miss the target."
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
            Me.IsHealingMove = False
            Me.RemovesFrozen = False
            Me.IsRecoilMove = False
            Me.IsPunchingMove = False
            Me.ImmunityAffected = False
            Me.IsDamagingMove = False
            Me.IsProtectMove = False
            Me.IsSoundMove = False
            Me.HasSecondaryEffect = False
            Me.IsAffectedBySubstitute = True
            Me.IsOneHitKOMove = False
            Me.IsWonderGuardAffected = False
            '#End

            Me.AIField1 = AIField.Support
            Me.AIField2 = AIField.Nothing
        End Sub

        Public Overrides Sub MoveHits(own As Boolean, BattleScreen As BattleScreen)
            Dim lockedOn As Integer = BattleScreen.FieldEffects.OppLockOn
            If own = False Then
                lockedOn = BattleScreen.FieldEffects.OwnLockOn
            End If

            If lockedOn = 0 Then
                Dim p As Pokemon = BattleScreen.OwnPokemon
                Dim op As Pokemon = BattleScreen.OppPokemon
                If own = False Then
                    p = BattleScreen.OppPokemon
                    op = BattleScreen.OwnPokemon
                End If

                If own = True Then
                    BattleScreen.FieldEffects.OppLockOn = 1
                Else
                    BattleScreen.FieldEffects.OwnLockOn = 1
                End If

                BattleScreen.BattleQuery.Add(New TextQueryObject(p.GetDisplayName() & " took aim on " & op.GetDisplayName() & "!"))
            Else
                BattleScreen.BattleQuery.Add(New TextQueryObject(Me.Name & " failed!"))
            End If
        End Sub

    End Class

End Namespace