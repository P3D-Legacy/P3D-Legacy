Namespace BattleSystem.Moves.Normal

    Public Class Conversion2

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Normal)
            Me.ID = 176
            Me.OriginalPP = 30
            Me.CurrentPP = 30
            Me.MaxPP = 30
            Me.Power = 0
            Me.Accuracy = 0
            Me.Category = Categories.Status
            Me.ContestCategory = ContestCategories.Cool
            Me.Name = "Conversion 2"
            Me.Description = "The user changes its type to make itself resistant to the type of the attack the opponent used last."
            Me.CriticalChance = 0
            Me.IsHMMove = False
            Me.Target = Targets.OneAdjacentTarget
            Me.Priority = 0
            Me.TimesToAttack = 1
            '#End

            '#SpecialDefinitions
            Me.MakesContact = False
            Me.ProtectAffected = False
            Me.MagicCoatAffected = False
            Me.SnatchAffected = False
            Me.MirrorMoveAffected = True
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
            If own = False Then
                p = BattleScreen.OppPokemon
            End If

            Dim lastMove As Attack = BattleScreen.FieldEffects.OppLastMove
            If own = False Then
                lastMove = BattleScreen.FieldEffects.OwnLastMove
            End If

            ' Conversion 2 will fail if the last move the target used was Struggle or if there is no type that resists that move.
            If lastMove Is Nothing OrElse lastMove.Name.ToLower = "struggle" Then
                BattleScreen.BattleQuery.Add(New TextQueryObject(Me.Name & " failed!"))
            Else
                Dim AllTypes As New List(Of Element)
                Dim ConsideredTypes As New List(Of Element)

                For i As Integer = 0 To 18
                    AllTypes.Add(New Element(i))
                Next

                ' Conversion 2 will not change the user to its current type. So Remove that combination.
                AllTypes.Remove(p.Type1)
                If p.Type2 IsNot Nothing Then
                    AllTypes.Remove(p.Type2)
                End If

                ' Remaining is the ones that Conversion 2 can change into. Now calculate which type can be considered.
                For Each NewType As Element In AllTypes
                    If BattleCalculation.ReverseTypeEffectiveness(Element.GetElementMultiplier(lastMove.Type, NewType)) < 1 Then
                        ConsideredTypes.Add(NewType)
                    End If
                Next

                If ConsideredTypes.Count > 0 Then
                    Dim SelectedType As Element = ConsideredTypes(Core.Random.Next(0, ConsideredTypes.Count - 1))

                    p.OriginalType1 = p.Type1
                    p.OriginalType2 = p.Type2

                    p.Type1 = SelectedType
                    p.Type2 = New Element(Element.Types.Blank)

                    BattleScreen.BattleQuery.Add(New TextQueryObject(p.GetDisplayName() & " transformed into the " & SelectedType.Type.ToString() & " type!"))
                Else
                    BattleScreen.BattleQuery.Add(New TextQueryObject(Me.Name & " failed!"))
                End If
            End If
        End Sub

    End Class

End Namespace