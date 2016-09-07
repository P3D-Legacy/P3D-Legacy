Namespace BattleSystem.Moves.Dark

    Public Class Punishment

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Dark)
            Me.ID = 386
            Me.OriginalPP = 5
            Me.CurrentPP = 5
            Me.MaxPP = 5
            Me.Power = 0
            Me.Accuracy = 100
            Me.Category = Categories.Physical
            Me.ContestCategory = ContestCategories.Smart
            Me.Name = "Punishment"
            Me.Description = "This attack's power increases the more the target has powered up with stat changes."
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
            Me.MirrorMoveAffected = True
            Me.KingsrockAffected = True
            Me.CounterAffected = False

            Me.DisabledWhileGravity = False
            Me.UseEffectiveness = True
            Me.ImmunityAffected = True
            Me.HasSecondaryEffect = True
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
        End Sub

        Public Overrides Function GetBasePower(own As Boolean, BattleScreen As BattleScreen) As Integer
            Dim p As Pokemon = BattleScreen.OppPokemon
            If own = False Then
                p = BattleScreen.OwnPokemon
            End If

            Dim powerup As Integer = 0

            If p.StatAttack > 0 Then
                powerup += p.StatAttack
            End If
            If p.StatDefense > 0 Then
                powerup += p.StatAttack
            End If
            If p.StatSpAttack > 0 Then
                powerup += p.StatAttack
            End If
            If p.StatSpDefense > 0 Then
                powerup += p.StatAttack
            End If
            If p.StatSpeed > 0 Then
                powerup += p.StatAttack
            End If

            Return (60 + (20 * powerup))
        End Function

    End Class

End Namespace