Namespace BattleSystem.Moves.Ghost

    Public Class Curse

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Ghost)
            Me.ID = 174
            Me.OriginalPP = 10
            Me.CurrentPP = 10
            Me.MaxPP = 10
            Me.Power = 0
            Me.Accuracy = 0
            Me.Category = Categories.Status
            Me.ContestCategory = ContestCategories.Tough
            Me.Name = "Curse"
            Me.Description = "A move that works differently for the Ghost type than for all other types."
            Me.CriticalChance = 0
            Me.IsHMMove = False
            Me.Target = Targets.Self
            Me.Priority = 0
            Me.TimesToAttack = 1
            '#End

            '#SpecialDefinitions
            Me.MakesContact = False
            Me.ProtectAffected = False
            Me.MagicCoatAffected = False
            Me.SnatchAffected = False
            Me.MirrorMoveAffected = False
            Me.KingsrockAffected = False
            Me.CounterAffected = False

            Me.DisabledWhileGravity = False
            Me.UseEffectiveness = False
            Me.ImmunityAffected = False
            Me.HasSecondaryEffect = True
            Me.RemovesFrozen = False

            Me.IsHealingMove = False
            Me.IsRecoilMove = False
            Me.IsPunchingMove = False
            Me.IsDamagingMove = False
            Me.IsProtectMove = False
            Me.IsSoundMove = False

            Me.IsAffectedBySubstitute = True
            Me.IsOneHitKOMove = False
            Me.IsWonderGuardAffected = True
            '#End

            Me.AIField1 = AIField.RaiseAttack
            Me.AIField2 = AIField.RaiseDefense
        End Sub

        Public Overrides Sub MoveHits(own As Boolean, BattleScreen As BattleScreen)
            Dim p As Pokemon = BattleScreen.OwnPokemon
            Dim op As Pokemon = BattleScreen.OppPokemon
            If own = False Then
                p = BattleScreen.OppPokemon
                op = BattleScreen.OwnPokemon
            End If

            Dim isGhost As Boolean = False
            If p.Type1.Type = Element.Types.Ghost Or p.Type2.Type = Element.Types.Ghost Then
                isGhost = True
            End If

            If isGhost = True Then
                Dim cursed As Boolean = False
                If own = True Then
                    If BattleScreen.FieldEffects.OppCurse > 0 Then
                        cursed = True
                    End If
                Else
                    If BattleScreen.FieldEffects.OwnCurse > 0 Then
                        cursed = True
                    End If
                End If
                If cursed = True Then
                    BattleScreen.BattleQuery.Add(New TextQueryObject(Me.Name & " failed!"))
                Else
                    If own = True Then
                        BattleScreen.FieldEffects.OppCurse = 1
                    Else
                        BattleScreen.FieldEffects.OwnCurse = 1
                    End If

                    BattleScreen.Battle.ReduceHP(CInt(p.MaxHP / 2), own, own, BattleScreen, p.GetDisplayName() & " cut its own HP and laid a curse on " & op.GetDisplayName() & "!", "move:curse")
                End If
            Else
                Dim failed As Boolean = False

                If BattleScreen.Battle.LowerStat(own, own, BattleScreen, "Speed", 1, "", "move:curse") = False Then
                    failed = True
                End If

                If failed = False Then
                    BattleScreen.Battle.RaiseStat(own, own, BattleScreen, "Attack", 1, "", "move:curse")
                    BattleScreen.Battle.RaiseStat(own, own, BattleScreen, "Defense", 1, "", "move:curse")
                Else
                    BattleScreen.BattleQuery.Add(New TextQueryObject(Me.Name & " failed!"))
                End If
            End If
        End Sub

    End Class

End Namespace