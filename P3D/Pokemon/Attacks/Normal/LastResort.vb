Namespace BattleSystem.Moves.Normal

    Public Class LastResort

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Normal)
            Me.ID = 387
            Me.OriginalPP = 5
            Me.CurrentPP = 5
            Me.MaxPP = 5
            Me.Power = 140
            Me.Accuracy = 100
            Me.Category = Categories.Physical
            Me.ContestCategory = ContestCategories.Cute
            Me.Name = "Last Resort"
            Me.Description = "This move can be used only after the user has used all the other moves it knows in the battle."
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
            Me.RemovesFrozen = False
            Me.HasSecondaryEffect = False

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

        Public Overrides Function MoveFailBeforeAttack(Own As Boolean, BattleScreen As BattleScreen) As Boolean
            Dim moveIDs As New List(Of Integer)
            Dim p As Pokemon = BattleScreen.OwnPokemon
            If Own = False Then
                p = BattleScreen.OppPokemon
            End If

            For Each Attack As BattleSystem.Attack In p.Attacks
                If moveIDs.Contains(Attack.ID) = False Then moveIDs.Add(Attack.ID)
            Next

            Dim usedMoves As Boolean = True
            Dim AllUsedMoves As List(Of Integer) = BattleScreen.FieldEffects.OwnUsedMoves
            If Own = False Then
                AllUsedMoves = BattleScreen.FieldEffects.OppUsedMoves
            End If

            For Each moveID As Integer In moveIDs
                If AllUsedMoves.Contains(moveID) = False Then
                    usedMoves = False
                    Exit For
                End If
            Next

            If usedMoves = False Then
                BattleScreen.BattleQuery.Add(New TextQueryObject(Me.Name & " failed!"))
                Return True
            Else
                Return False
            End If
        End Function

    End Class

End Namespace