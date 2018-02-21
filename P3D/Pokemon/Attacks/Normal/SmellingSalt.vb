Namespace BattleSystem.Moves.Normal

    Public Class SmellingSalt

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Normal)
            Me.ID = 265
            Me.OriginalPP = 10
            Me.CurrentPP = 10
            Me.MaxPP = 10
            Me.Power = 70
            Me.Accuracy = 100
            Me.Category = Categories.Physical
            Me.ContestCategory = ContestCategories.Smart
            Me.Name = "Smelling Salt"
            Me.Description = "This attack inflicts double damage on a target with paralysis. It also cures the target's paralysis, however."
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
            Me.KingsrockAffected = False
            Me.CounterAffected = True

            Me.DisabledWhileGravity = False
            Me.UseEffectiveness = True
            Me.ImmunityAffected = True
            Me.RemovesFrozen = False
            Me.HasSecondaryEffect = True

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
            Dim op As Pokemon = BattleScreen.OppPokemon
            If own = False Then
                op = BattleScreen.OwnPokemon
            End If

            If op.Status = Pokemon.StatusProblems.Paralyzed Then
                Return Me.Power * 2
            Else
                Return Me.Power
            End If
        End Function

        Public Overrides Sub MoveHits(own As Boolean, BattleScreen As BattleScreen)
            If own = True Then
                If BattleScreen.OppPokemon.Status = Pokemon.StatusProblems.Paralyzed Then
                    BattleScreen.Battle.CureStatusProblem(Not own, own, BattleScreen, BattleScreen.OppPokemon.GetDisplayName() & " was cured of paralysis.", "move:smellingsalt")
                End If
            Else
                If BattleScreen.OppPokemon.Status = Pokemon.StatusProblems.Paralyzed Then
                    BattleScreen.Battle.CureStatusProblem(Not own, own, BattleScreen, BattleScreen.OwnPokemon.GetDisplayName() & " was cured of paralysis.", "move:smellingsalt")
                End If
            End If
        End Sub

    End Class

End Namespace