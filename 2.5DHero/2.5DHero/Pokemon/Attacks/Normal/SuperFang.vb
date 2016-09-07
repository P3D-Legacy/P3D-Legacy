Namespace BattleSystem.Moves.Normal

    Public Class SuperFang

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Normal)
            Me.ID = 162
            Me.OriginalPP = 10
            Me.CurrentPP = 10
            Me.MaxPP = 10
            Me.Power = 0
            Me.Accuracy = 90
            Me.Category = Categories.Physical
            Me.ContestCategory = ContestCategories.Tough
            Me.Name = "Super Fang"
            Me.Description = "The user chomps hard on the target with its sharp front fangs. It cuts the target's HP to half."
            Me.CriticalChance = 0
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
            Me.KingsrockAffected = False
            Me.CounterAffected = True

            Me.DisabledWhileGravity = False
            Me.UseEffectiveness = False
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

        Public Overrides Function GetDamage(Critical As Boolean, Own As Boolean, targetPokemon As Boolean, BattleScreen As BattleScreen) As Integer
            Dim op As Pokemon = BattleScreen.OppPokemon
            If Own = False Then
                op = BattleScreen.OwnPokemon
            End If

            Dim damage As Integer = CInt(Math.Floor(op.HP / 2))

            If damage <= 0 Then
                damage = 1
            End If

            Return damage
        End Function

    End Class

End Namespace