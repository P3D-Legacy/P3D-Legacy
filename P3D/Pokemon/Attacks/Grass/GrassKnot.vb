Namespace BattleSystem.Moves.Grass

    Public Class GrassKnot

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Grass)
            Me.ID = 447
            Me.OriginalPP = 20
            Me.CurrentPP = 20
            Me.MaxPP = 20
            Me.Power = 0
            Me.Accuracy = 100
            Me.Category = Categories.Special
            Me.ContestCategory = ContestCategories.Cute
            Me.Name = Localization.GetString("move_name_" & Me.ID,"Grass Knot")
            Me.Description = "The user snares the target with grass and trips it. The heavier the target, the greater the move's power."
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
            Dim weight As Single = BattleScreen.FieldEffects.GetPokemonWeight(Not own, BattleScreen)

            If weight <= 9.9F Then
                Return 20
            ElseIf weight > 9.9F And weight <= 24.9F Then
                Return 40
            ElseIf weight > 24.9F And weight <= 49.9F Then
                Return 60
            ElseIf weight > 49.9F And weight <= 99.9F Then
                Return 80
            ElseIf weight > 99.9F And weight <= 199.9F Then
                Return 100
            Else
                Return 120
            End If
        End Function

    End Class

End Namespace