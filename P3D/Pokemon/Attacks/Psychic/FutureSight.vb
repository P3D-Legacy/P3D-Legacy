Namespace BattleSystem.Moves.Psychic

    Public Class FutureSight

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Psychic)
            Me.ID = 248
            Me.OriginalPP = 10
            Me.CurrentPP = 10
            Me.MaxPP = 10
            Me.Power = 120
            Me.Accuracy = 100
            Me.Category = Categories.Special
            Me.ContestCategory = ContestCategories.Smart
            Me.Name = "Future Sight"
            Me.Description = "Two turns after this move is used, a hunk of psychic energy attacks the target."
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
            Me.UseEffectiveness = True
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

            Me.AIField1 = AIField.Damage
            Me.AIField2 = AIField.MultiTurn
        End Sub

        Public Overrides Sub MoveHits(own As Boolean, BattleScreen As BattleScreen)
            Dim p As Pokemon = BattleScreen.OwnPokemon
            If own = False Then
                p = BattleScreen.OppPokemon
            End If

            If own = True Then
                If BattleScreen.FieldEffects.OwnFutureSightTurns = 0 Then
                    BattleScreen.FieldEffects.OwnFutureSightTurns = 3
                    BattleScreen.FieldEffects.OwnFutureSightID = 0
                    BattleScreen.FieldEffects.OwnFutureSightDamage = MyBase.GetDamage(False, own, Not own, BattleScreen)

                    BattleScreen.BattleQuery.Add(New TextQueryObject(p.GetDisplayName() & " foresaw an attack!"))
                Else
                    BattleScreen.BattleQuery.Add(New TextQueryObject(Me.Name & " failed!"))
                End If
            Else
                If BattleScreen.FieldEffects.OppFutureSightTurns = 0 Then
                    BattleScreen.FieldEffects.OppFutureSightTurns = 3
                    BattleScreen.FieldEffects.OppFutureSightID = 0
                    BattleScreen.FieldEffects.OppFutureSightDamage = MyBase.GetDamage(False, own, Not own, BattleScreen)

                    BattleScreen.BattleQuery.Add(New TextQueryObject(p.GetDisplayName() & " foresaw an attack!"))
                Else
                    BattleScreen.BattleQuery.Add(New TextQueryObject(Me.Name & " failed!"))
                End If
            End If
        End Sub

    End Class

End Namespace