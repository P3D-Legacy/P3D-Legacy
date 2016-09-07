Namespace BattleSystem.Moves.Poison

    Public Class ToxicSpikes

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Poison)
            Me.ID = 390
            Me.OriginalPP = 20
            Me.CurrentPP = 20
            Me.MaxPP = 20
            Me.Power = 0
            Me.Accuracy = 0
            Me.Category = Categories.Status
            Me.ContestCategory = ContestCategories.Smart
            Me.Name = "Toxic Spikes"
            Me.Description = "The user lays a trap of poison spikes at the opponent's feet. They poison opponents that switch into battle."
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
                spikes = BattleScreen.FieldEffects.OwnToxicSpikes
            Else
                spikes = BattleScreen.FieldEffects.OppToxicSpikes
            End If
            If spikes < 2 Then
                If own = True Then
                    BattleScreen.FieldEffects.OwnToxicSpikes += 1
                    BattleScreen.BattleQuery.Add(New TextQueryObject("Poison spikes were scattered all around the feet of the foe's team!"))
                Else
                    BattleScreen.FieldEffects.OppToxicSpikes += 1
                    BattleScreen.BattleQuery.Add(New TextQueryObject("Poison spikes were scattered all around the feet of your team!"))
                End If
            Else
                BattleScreen.BattleQuery.Add(New TextQueryObject(Me.Name & " failed!"))
            End If
        End Sub

    End Class

End Namespace