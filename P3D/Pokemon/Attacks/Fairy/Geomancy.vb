Namespace BattleSystem.Moves.Fairy

    Public Class Geomancy

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Fairy)
            Me.ID = 601
            Me.OriginalPP = 10
            Me.CurrentPP = 10
            Me.MaxPP = 10
            Me.Power = 0
            Me.Accuracy = 0
            Me.Category = Categories.Status
            Me.ContestCategory = ContestCategories.Beauty
            Me.Name = "Geomancy"
            Me.Description = "The user absorbs energy and sharply raises its Sp. Atk, Sp. Def, and Speed stats on the next turn."
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
            Me.RemovesFrozen = False
            Me.HasSecondaryEffect = False

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

            Me.AIField1 = AIField.CanRaiseSpAttack
            Me.AIField2 = AIField.CanRaiseSpDefense
            Me.AIField3 = AIField.CanRaiseSpeed
        End Sub

        Public Overrides Sub MoveHits(own As Boolean, BattleScreen As BattleScreen)

            BattleScreen.Battle.RaiseStat(own, own, BattleScreen, "Special Attack", 2, "", "move:geomancy")
            BattleScreen.Battle.RaiseStat(own, own, BattleScreen, "Special Defense", 2, "", "move:geomancy")
            BattleScreen.Battle.RaiseStat(own, own, BattleScreen, "Speed", 2, "", "move:geomancy")
        End Sub

        Public Overrides Function MoveFailBeforeAttack(Own As Boolean, BattleScreen As BattleScreen) As Boolean
            Dim p As Pokemon = BattleScreen.OwnPokemon
            Dim op As Pokemon = BattleScreen.OppPokemon
            If Own = False Then
                p = BattleScreen.OppPokemon
                op = BattleScreen.OwnPokemon
            End If

            Dim geomancy As Integer = BattleScreen.FieldEffects.OwnGeomancyCounter
            If Own = False Then
                geomancy = BattleScreen.FieldEffects.OppGeomancyCounter
            End If

            If Not p.Item Is Nothing Then
                If p.Item.Name.ToLower() = "power herb" And BattleScreen.FieldEffects.CanUseItem(Own) = True And BattleScreen.FieldEffects.CanUseOwnItem(Own, BattleScreen) = True Then
                    If BattleScreen.Battle.RemoveHeldItem(Own, Own, BattleScreen, "Power Herb pushed the use of Geomancy!", "move:geomancy") = True Then
                        geomancy = 1
                    End If
                End If
            End If

            If geomancy = 0 Then
                BattleScreen.BattleQuery.Add(New TextQueryObject(p.GetDisplayName() & " is absorbing power!"))
                If Own = True Then
                    BattleScreen.FieldEffects.OwnGeomancyCounter = 1
                Else
                    BattleScreen.FieldEffects.OppGeomancyCounter = 1
                End If
                Return True
            Else
                If Own = True Then
                    BattleScreen.FieldEffects.OwnGeomancyCounter = 0
                Else
                    BattleScreen.FieldEffects.OppGeomancyCounter = 0
                End If
                Return False
            End If
        End Function
        Public Overrides Sub MoveSelected(own As Boolean, BattleScreen As BattleScreen)
            If own = True Then
                BattleScreen.FieldEffects.OwnGeomancyCounter = 0
            Else
                BattleScreen.FieldEffects.OppGeomancyCounter = 0
            End If
        End Sub
        Public Overrides Function DeductPP(own As Boolean, BattleScreen As BattleScreen) As Boolean
            Dim geomancy As Integer = BattleScreen.FieldEffects.OwnGeomancyCounter
            If own = False Then
                geomancy = BattleScreen.FieldEffects.OppGeomancyCounter
            End If

            If geomancy = 0 Then
                Return False
            Else
                Return True
            End If
        End Function

        Private Sub MoveFails(own As Boolean, BattleScreen As BattleScreen)
            If own = True Then
                BattleScreen.FieldEffects.OwnGeomancyCounter = 0
            Else
                BattleScreen.FieldEffects.OppGeomancyCounter = 0
            End If
        End Sub

        Public Overrides Sub InflictedFlinch(own As Boolean, BattleScreen As BattleScreen)
            MoveFails(own, BattleScreen)
        End Sub

        Public Overrides Sub IsSleeping(own As Boolean, BattleScreen As BattleScreen)
            MoveFails(own, BattleScreen)
        End Sub

        Public Overrides Sub HurtItselfInConfusion(own As Boolean, BattleScreen As BattleScreen)
            MoveFails(own, BattleScreen)
        End Sub

        Public Overrides Sub IsAttracted(own As Boolean, BattleScreen As BattleScreen)
            MoveFails(own, BattleScreen)
        End Sub
    End Class

End Namespace