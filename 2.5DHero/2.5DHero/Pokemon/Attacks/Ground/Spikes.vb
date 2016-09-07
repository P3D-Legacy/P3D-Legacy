Namespace BattleSystem.Moves.Ground

    Public Class Spikes

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Ground)
            Me.ID = 191
            Me.OriginalPP = 20
            Me.CurrentPP = 20
            Me.MaxPP = 20
            Me.Power = 0
            Me.Accuracy = 0
            Me.Category = Categories.Status
            Me.ContestCategory = ContestCategories.Smart
            Me.Name = "Spikes"
            Me.Description = "The user lays a trap of spikes at the opposing team's feet. The trap hurts Pokémon that switch into battle."
            Me.CriticalChance = 0
            Me.IsHMMove = False
            Me.Target = Targets.AllFoes
            Me.Priority = 0
            Me.TimesToAttack = 1
            '#End

            '#SpecialDefinitions
            Me.MakesContact = False
            Me.ProtectAffected = False
            Me.MagicCoatAffected = True
            Me.SnatchAffected = False
            Me.MirrorMoveAffected = True
            Me.KingsrockAffected = False
            Me.CounterAffected = False

            Me.DisabledWhileGravity = False
            Me.UseEffectiveness = False
            Me.ImmunityAffected = False
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
            Me.IsWonderGuardAffected = False
            '#End

            Me.AIField1 = AIField.Support
            Me.AIField2 = AIField.Nothing
        End Sub

        Public Overrides Sub MoveHits(own As Boolean, BattleScreen As BattleScreen)
            Dim spikes As Integer = 0
            If own = True Then
                spikes = BattleScreen.FieldEffects.OwnSpikes
            Else
                spikes = BattleScreen.FieldEffects.OppSpikes
            End If
            If spikes < 3 Then
                If own = True Then
                    BattleScreen.FieldEffects.OwnSpikes += 1
                    BattleScreen.BattleQuery.Add(New TextQueryObject("Spikes were scattered all around the feet of the foe's team!"))
                Else
                    BattleScreen.FieldEffects.OppSpikes += 1
                    BattleScreen.BattleQuery.Add(New TextQueryObject("Spikes were scattered all around the feet of your team!"))
                End If
            Else
                BattleScreen.BattleQuery.Add(New TextQueryObject(Me.Name & " failed!"))
            End If
        End Sub

    End Class

End Namespace