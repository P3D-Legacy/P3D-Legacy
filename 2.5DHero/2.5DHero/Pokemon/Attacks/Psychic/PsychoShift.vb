Namespace BattleSystem.Moves.Psychic

    Public Class PsychoShift

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Psychic)
            Me.ID = 375
            Me.OriginalPP = 10
            Me.CurrentPP = 10
            Me.MaxPP = 10
            Me.Power = 0
            Me.Accuracy = 100
            Me.Category = Categories.Status
            Me.ContestCategory = ContestCategories.Cool
            Me.Name = "Psycho Shift"
            Me.Description = "Using its psychic power of suggestion, the user transfers its status conditions to the target."
            Me.CriticalChance = 0
            Me.IsHMMove = False
            Me.Target = Targets.OneAdjacentTarget
            Me.Priority = 0
            Me.TimesToAttack = 1
            '#End

            '#SpecialDefinitions
            Me.MakesContact = False
            Me.ProtectAffected = True
            Me.MagicCoatAffected = False
            Me.SnatchAffected = True
            Me.MirrorMoveAffected = True
            Me.KingsrockAffected = False
            Me.CounterAffected = False

            Me.DisabledWhileGravity = False
            Me.UseEffectiveness = False
            Me.ImmunityAffected = False
            Me.RemovesFrozen = False
            Me.HasSecondaryEffect = True

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
        End Sub

        Public Overrides Sub MoveHits(own As Boolean, BattleScreen As BattleScreen)
            Dim fails As Boolean = False
            Dim p As Pokemon = BattleScreen.OwnPokemon
            Dim o As Pokemon = BattleScreen.OppPokemon
            If own = False Then
                p = BattleScreen.OppPokemon
                o = BattleScreen.OwnPokemon
            End If
            Dim status As Pokemon.StatusProblems = p.Status

            If o.status <> Pokemon.StatusProblems.none Then
                fails = True
            End If

            Select Case p.Status
                Case Pokemon.StatusProblems.Poison
                    fails = Not BattleScreen.Battle.InflictPoison(Not own, own, BattleScreen, False, "", "move:psychoshift")
                Case Pokemon.StatusProblems.BadPoison
                    fails = Not BattleScreen.Battle.InflictPoison(Not own, own, BattleScreen, True, "", "move:psychoshift")
                Case Pokemon.StatusProblems.Sleep
                    fails = Not BattleScreen.Battle.InflictSleep(Not own, own, BattleScreen, -1, "", "move:psychoshift")
                Case Pokemon.StatusProblems.Paralyzed
                    fails = Not BattleScreen.Battle.InflictParalysis(Not own, own, BattleScreen, "", "move:psychoshift")
                Case Pokemon.StatusProblems.Freeze
                    fails = Not BattleScreen.Battle.InflictFreeze(Not own, own, BattleScreen, "", "move:psychoshift")
                Case Pokemon.StatusProblems.Burn
                    fails = Not BattleScreen.Battle.InflictBurn(Not own, own, BattleScreen, "", "move:psychoshift")
                Case Else
                    fails = True
            End Select

            If fails = True Then
                BattleScreen.BattleQuery.Add(New TextQueryObject(Me.Name & " failed!"))
            Else
                BattleScreen.Battle.CureStatusProblem(own, own, BattleScreen, "", "move:psychoshift")
            End If


        End Sub

    End Class

End Namespace