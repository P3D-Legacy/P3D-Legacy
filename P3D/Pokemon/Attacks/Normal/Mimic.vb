Namespace BattleSystem.Moves.Normal

    Public Class Mimic

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Normal)
            Me.ID = 102
            Me.OriginalPP = 10
            Me.CurrentPP = 10
            Me.MaxPP = 10
            Me.Power = 0
            Me.Accuracy = 0
            Me.Category = Categories.Status
            Me.ContestCategory = ContestCategories.Cute
            Me.Name = "Mimic"
            Me.Description = "The user copies the target's last move. The move can be used during battle until the Pokémon is switched out."
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
            Me.SnatchAffected = False
            Me.MirrorMoveAffected = False
            Me.KingsrockAffected = False
            Me.CounterAffected = False

            Me.DisabledWhileGravity = False
            Me.UseEffectiveness = False
            Me.ImmunityAffected = False
            Me.HasSecondaryEffect = False
            Me.RemovesFrozen = False

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

            Me.AIField1 = AIField.Support
            Me.AIField2 = AIField.Nothing
        End Sub

        Public Overrides Sub MoveHits(own As Boolean, BattleScreen As BattleScreen)
            Dim p As Pokemon = BattleScreen.OwnPokemon
            Dim op As Pokemon = BattleScreen.OppPokemon
            If own = False Then
                p = BattleScreen.OppPokemon
                op = BattleScreen.OwnPokemon
            End If

            Dim moveToCopy As BattleSystem.Attack = BattleSystem.Attack.GetAttackByID(op.Attacks.Last.ID)

            Dim failsMoves() As Integer = {165, 166, 118, 448}

            If p.KnowsMove(moveToCopy) = False And failsMoves.Contains(moveToCopy.ID) = False Then
                p.OriginalMoves = New List(Of BattleSystem.Attack)
                p.OriginalMoves.AddRange(p.Attacks.ToArray())

                For Each m As BattleSystem.Attack In p.Attacks
                    If m.ID = Me.ID Then
                        p.Attacks.Remove(m)
                        Exit For
                    End If
                Next

                p.Attacks.Add(moveToCopy)

                BattleScreen.BattleQuery.Add(New TextQueryObject(p.GetDisplayName() & " learned " & moveToCopy.Name & "!"))
            Else
                BattleScreen.BattleQuery.Add(New TextQueryObject(Me.Name & " failed!"))
            End If
        End Sub

    End Class

End Namespace