Namespace BattleSystem.Moves.Electric

    Public Class ElectroBall

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Electric)
            Me.ID = 486
            Me.OriginalPP = 10
            Me.CurrentPP = 10
            Me.MaxPP = 10
            Me.Power = 0
            Me.Accuracy = 100
            Me.Category = Categories.Special
            Me.ContestCategory = ContestCategories.Cool
            Me.Name = Localization.GetString("move_name_" & Me.ID,"Electro Ball")
            Me.Description = "The user hurls an electric orb at the target. The faster the user is than the target, the greater the move's power."
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
            Me.IsBulletMove = True
            '#End
        End Sub

        Public Overrides Function GetBasePower(own As Boolean, BattleScreen As BattleScreen) As Integer
            Dim p As Pokemon = BattleScreen.OwnPokemon
            Dim op As Pokemon = BattleScreen.OppPokemon
            If own = False Then
                p = BattleScreen.OppPokemon
                op = BattleScreen.OwnPokemon
            End If

            Dim p_Speed As Integer = BattleCalculation.DetermineBattleSpeed(own, BattleScreen)
            Dim op_Speed As Integer = BattleCalculation.DetermineBattleSpeed(Not own, BattleScreen)

            Dim ratio As Integer = CInt(Math.Ceiling(100 * (op_Speed / p_Speed)))

            If ratio > 50 Then
                Return 60
            Else
                If ratio > 34 Then
                    Return 80
                Else
                    If ratio > 25 Then
                        Return 120
                    Else
                        Return 150
                    End If
                End If
            End If

        End Function

    End Class

End Namespace