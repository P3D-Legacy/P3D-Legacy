Namespace BattleSystem.Moves.Steel

    Public Class HeavySlam

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Steel)
            Me.ID = 484
            Me.OriginalPP = 10
            Me.CurrentPP = 10
            Me.MaxPP = 10
            Me.Power = 0
            Me.Accuracy = 100
            Me.Category = Categories.Physical
            Me.ContestCategory = ContestCategories.Cool
            Me.Name = "Heavy Slam"
            Me.Description = "The user slams into the target with its heavy body. The more the user outweighs the target, the greater its damage."
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
            Dim userWeight As Single = BattleScreen.FieldEffects.GetPokemonWeight(own, BattleScreen)
            Dim targetWeight As Single = BattleScreen.FieldEffects.GetPokemonWeight(Not own, BattleScreen)

            If targetWeight <= (1 / 5) * userWeight Then
                Return 120
            End If

            If targetWeight > (1 / 5) * userWeight And targetWeight <= (1 / 4) * userWeight Then
                Return 100
            End If

            If targetWeight > (1 / 4) * userWeight And targetWeight <= (1 / 3) * userWeight Then
                Return 80
            End If

            If targetWeight > (1 / 3) * userWeight And targetWeight <= (1 / 2) * userWeight Then
                Return 60
            End If

            If targetWeight > (1 / 2) * userWeight Then
                Return 40
            End If

            Return 40
        End Function

    End Class

End Namespace