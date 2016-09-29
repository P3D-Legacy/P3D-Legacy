Namespace BattleSystem.Moves.Dark

    Public Class Switcheroo

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Dark)
            Me.ID = 415
            Me.OriginalPP = 10
            Me.CurrentPP = 10
            Me.MaxPP = 10
            Me.Power = 0
            Me.Accuracy = 100
            Me.Category = Categories.Status
            Me.ContestCategory = ContestCategories.Cool
            Me.Name = "Switcheroo"
            Me.Description = "The user trades held items with the target faster than the eye can follow."
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
            Me.MirrorMoveAffected = True
            Me.KingsrockAffected = False
            Me.CounterAffected = False

            Me.DisabledWhileGravity = False
            Me.UseEffectiveness = False
            Me.ImmunityAffected = False
            Me.RemovesFrozen = False
            Me.HasSecondaryEffect = False

            Me.IsHealingMove = False
            Me.IsRecoilMove = False
            Me.IsPunchingMove = False
            Me.IsDamagingMove = False
            Me.IsProtectMove = False
            Me.IsSoundMove = False

            Me.IsAffectedBySubstitute = True
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

            If p.Item Is Nothing And op.Item Is Nothing Then
                BattleScreen.BattleQuery.Add(New TextQueryObject(Me.Name & " failed!"))
            Else
                If p.Ability.Name.ToLower() = "sticky hold" Or op.Ability.Name.ToLower() = "sticky hold" Then
                    BattleScreen.BattleQuery.Add(New TextQueryObject(Me.Name & " failed!"))
                Else
                    If Not p.Item Is Nothing AndAlso p.Item.Name.ToLower() = "griseous orb" AndAlso p.Number = 487 Then
                        BattleScreen.BattleQuery.Add(New TextQueryObject(Me.Name & " failed!"))
                    Else
                        If Not op.Item Is Nothing AndAlso op.Item.Name.ToLower() = "griseous orb" AndAlso op.Number = 487 Then
                            BattleScreen.BattleQuery.Add(New TextQueryObject(Me.Name & " failed!"))
                        Else
                            If CheckMultitypePlate(p, op) = False Then
                                BattleScreen.BattleQuery.Add(New TextQueryObject(Me.Name & " failed!"))
                            Else
                                If Not p.Item Is Nothing AndAlso p.Item.Name.ToLower().EndsWith(" drive") = True AndAlso p.Number = 649 Then
                                    BattleScreen.BattleQuery.Add(New TextQueryObject(Me.Name & " failed!"))
                                Else
                                    If Not op.Item Is Nothing AndAlso op.Item.Name.ToLower().EndsWith(" drive") = True AndAlso p.Number = 649 Then
                                        BattleScreen.BattleQuery.Add(New TextQueryObject(Me.Name & " failed!"))
                                        If p.Item.IsMegaStone OrElse op.Item.IsMegaStone Then
                                            BattleScreen.BattleQuery.Add(New TextQueryObject(Me.Name & " failed!"))
                                        Else
                                            Dim i1 As Item = p.Item
                                            Dim i2 As Item = op.Item
                                            p.Item = i2
                                            op.Item = i1
                                            BattleScreen.BattleQuery.Add(New TextQueryObject(p.GetDisplayName() & " switched items with " & op.GetDisplayName() & "."))
                                        End If
                                    End If
                                End If
                            End If
                        End If
                    End If
                End If
            End If
        End Sub

        Private Function CheckMultitypePlate(ByVal p As Pokemon, ByVal op As Pokemon) As Boolean
            If p.Ability.Name.ToLower() <> "multitype" And op.Ability.Name.ToLower() = "multitype" Then
                Return True
            Else
                If Not p.Item Is Nothing Then
                    If p.Item.Name.ToLower().EndsWith(" plate") = True Then
                        Return False
                    End If
                End If
                If Not op.Item Is Nothing Then
                    If op.Item.Name.ToLower().EndsWith(" plate") = True Then
                        Return False
                    End If
                End If
            End If

            Return True
        End Function

    End Class

End Namespace
