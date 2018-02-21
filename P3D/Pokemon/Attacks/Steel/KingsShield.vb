Namespace BattleSystem.Moves.Steel

    Public Class KingsShield

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Steel)
            Me.ID = 588
            Me.OriginalPP = 10
            Me.CurrentPP = 10
            Me.MaxPP = 10
            Me.Power = 0
            Me.Accuracy = 0
            Me.Category = Categories.Status
            Me.ContestCategory = ContestCategories.Smart
            Me.Name = "King's Shield"
            Me.Description = "The user takes a defensive stance while it protects itself from damage. It also harshly lowers the Attack stat of any attacker who makes direct contact."
            Me.CriticalChance = 0
            Me.IsHMMove = False
            Me.Target = Targets.Self
            Me.Priority = 3
            Me.TimesToAttack = 1
            '#End

            '#SpecialDefinitions
            Me.MakesContact = False
            Me.ProtectAffected = False
            Me.MagicCoatAffected = False
            Me.SnatchAffected = True
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
            Me.IsProtectMove = True
            Me.IsSoundMove = False

            Me.IsAffectedBySubstitute = False
            Me.IsOneHitKOMove = False
            Me.IsWonderGuardAffected = False
            '#End

            Me.AIField1 = AIField.Support
            Me.AIField2 = AIField.Nothing
        End Sub

        Public Overrides Function MoveFailBeforeAttack(Own As Boolean, BattleScreen As BattleScreen) As Boolean
            Dim chance As Double = 100D
            Dim protects As Integer = BattleScreen.FieldEffects.OwnProtectMovesCount
            If Own = False Then
                protects = BattleScreen.FieldEffects.OppProtectMovesCount
            End If

            If protects > 0 Then
                For i = 1 To protects
                    chance /= 2
                Next
            End If

            If Core.Random.Next(0, 100) < chance Then
                Return False
            Else
                BattleScreen.BattleQuery.Add(New TextQueryObject(Me.Name & " failed!"))
                Return True
            End If
        End Function

        Public Overrides Sub MoveHits(own As Boolean, BattleScreen As BattleScreen)
            If own = True Then
                BattleScreen.FieldEffects.OwnProtectMovesCount += 1

                BattleScreen.FieldEffects.OwnKingsShieldCounter = 1
            Else
                BattleScreen.FieldEffects.OppProtectMovesCount += 1

                BattleScreen.FieldEffects.OppKingsShieldCounter = 1
            End If

            Dim p As Pokemon = BattleScreen.OwnPokemon
            If own = False Then
                p = BattleScreen.OppPokemon
            End If

            BattleScreen.BattleQuery.Add(New TextQueryObject(p.GetDisplayName() & " protected itself!"))
        End Sub

    End Class

End Namespace