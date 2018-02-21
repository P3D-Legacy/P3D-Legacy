Namespace BattleSystem.Moves.Electric

    Public Class FusionBolt

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Electric)
            Me.ID = 559
            Me.OriginalPP = 5
            Me.CurrentPP = 5
            Me.MaxPP = 5
            Me.Power = 100
            Me.Accuracy = 100
            Me.Category = Categories.Special
            Me.ContestCategory = ContestCategories.Beauty
            Me.Name = "Fusion Bolt"
            Me.Description = "The user throws down a giant thunderbolt. This attack does greater damage when influenced by an enormous flame."
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
                If lastMove.Name.ToLower() = "fusion flare" Or lastMove.Name.ToLower() = "blue flare" Then
                    Return Me.Power * 2
                End If
            End If

            Return Me.Power
        End Function

    End Class

End Namespace