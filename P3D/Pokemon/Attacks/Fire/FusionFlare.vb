Namespace BattleSystem.Moves.Fire

    Public Class FusionFlare

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Fire)
            Me.ID = 558
            Me.OriginalPP = 5
            Me.CurrentPP = 5
            Me.MaxPP = 5
            Me.Power = 100
            Me.Accuracy = 100
            Me.Category = Categories.Special
            Me.ContestCategory = ContestCategories.Beauty
            Me.Name = "Fusion Flare"
            Me.Description = "The user brings down a giant flame. This attack does greater damage when influenced by an enormous thunderbolt."
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
            Me.HasSecondaryEffect = False
            Me.RemovesFrozen = True

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

        Public Overrides Function GetBasePower(own As Boolean, BattleScreen As BattleScreen) As Integer
            Dim lastMove As Attack = BattleScreen.FieldEffects.OwnLastMove
            If own = False Then
                lastMove = BattleScreen.FieldEffects.OppLastMove
            End If

            If Not lastMove Is Nothing Then
                If lastMove.Name.ToLower() = "fusion bolt" Or lastMove.Name.ToLower() = "bolt strike" Then
                    Return Me.Power * 2
                End If
            End If

            Return Me.Power
        End Function

    End Class

End Namespace