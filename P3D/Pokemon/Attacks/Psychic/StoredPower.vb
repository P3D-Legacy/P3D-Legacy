Namespace BattleSystem.Moves.Psychic

    Public Class StoredPower

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Psychic)
            Me.ID = 500
            Me.OriginalPP = 10
            Me.CurrentPP = 10
            Me.MaxPP = 10
            Me.Power = 20
            Me.Accuracy = 100
            Me.Category = Categories.Special
            Me.ContestCategory = ContestCategories.Cool
            Me.Name = Localization.GetString("move_name_" & Me.ID,"Stored Power")
            Me.Description = "The user attacks the target with stored power. The more the user's stats are raised, the greater the move's power."
            Me.CriticalChance = 1
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
        End Sub

        Public Overrides Function GetBasePower(own As Boolean, BattleScreen As BattleScreen) As Integer
            Dim p As Pokemon = BattleScreen.OwnPokemon
            If own = False Then
                p = BattleScreen.OppPokemon
            End If

            Dim powermult As Integer = 1

            If p.StatAttack > 0 Then
                powermult += p.StatAttack
            End If

            If p.StatDefense > 0 Then
                powermult += p.StatDefense
            End If

            If p.StatSpAttack > 0 Then
                powermult += p.StatSpAttack
            End If

            If p.StatSpDefense > 0 Then
                powermult += p.StatSpDefense
            End If

            If p.StatSpeed > 0 Then
                powermult += p.StatSpeed
            End If

            If p.Accuracy > 0 Then
                powermult += p.Accuracy
            End If

            If p.Evasion > 0 Then
                powermult += p.Evasion
            End If

            Return (20 * (powermult))
        End Function

    End Class

End Namespace