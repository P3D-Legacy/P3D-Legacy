Namespace BattleSystem.Moves.Normal

    Public Class Snore

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Normal)
            Me.ID = 173
            Me.OriginalPP = 15
            Me.CurrentPP = 15
            Me.MaxPP = 15
            Me.Power = 50
            Me.Accuracy = 100
            Me.Category = Categories.Special
            Me.ContestCategory = ContestCategories.Cute
            Me.Name = "Snore"
            Me.Description = "An attack that can be used only if the user is asleep. The harsh noise may make the target flinch."
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
            Me.IsSoundMove = True

            Me.IsAffectedBySubstitute = True
            Me.IsOneHitKOMove = False
            Me.IsWonderGuardAffected = True
            '#End

            Me.AIField1 = AIField.Damage
            Me.AIField2 = AIField.Flinch

            Me.EffectChances.Add(30)
        End Sub

        Public Overrides Function MoveFailBeforeAttack(Own As Boolean, BattleScreen As BattleScreen) As Boolean
            Dim p As Pokemon = BattleScreen.OwnPokemon
            If Own = False Then
                p = BattleScreen.OppPokemon
            End If

            If p.Status = Pokemon.StatusProblems.Sleep Then
                Return False
            Else
                BattleScreen.BattleQuery.Add(New TextQueryObject(Me.Name & " failed!"))
                Return True
            End If
        End Function

        Public Overrides Sub MoveHits(own As Boolean, BattleScreen As BattleScreen)
            Dim chance As Integer = GetEffectChance(0, own, BattleScreen)

            If Core.Random.Next(0, 100) < chance Then
                BattleScreen.Battle.InflictFlinch(Not own, own, BattleScreen, "", "move:snore")
            End If
        End Sub

    End Class

End Namespace