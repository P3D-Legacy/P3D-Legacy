Namespace BattleSystem.Moves.Steel

    Public Class DoomDesire

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Steel)
            Me.ID = 353
            Me.OriginalPP = 10
            Me.CurrentPP = 10
            Me.MaxPP = 10
            Me.Power = 140
            Me.Accuracy = 100
            Me.Category = Categories.Special
            Me.ContestCategory = ContestCategories.Beauty
            Me.Name = Localization.GetString("move_name_" & Me.ID,"Doom Desire")
            Me.Description = "Two turns after this move is used, the user blasts the target with a concentrated bundle of light."
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
                    BattleScreen.FieldEffects.OwnFutureSightTurns = 2
                    BattleScreen.FieldEffects.OwnFutureSightID = 1
                    BattleScreen.FieldEffects.OwnFutureSightDamage = MyBase.GetDamage(False, own, Not own, BattleScreen)

                    BattleScreen.BattleQuery.Add(New TextQueryObject(p.GetDisplayName() & " foresaw an attack!"))
                Else
                    BattleScreen.BattleQuery.Add(New TextQueryObject(Me.Name & " failed!"))
                End If
            Else
                If BattleScreen.FieldEffects.OppFutureSightTurns = 0 Then
                    BattleScreen.FieldEffects.OppFutureSightTurns = 2
                    BattleScreen.FieldEffects.OppFutureSightID = 1
                    BattleScreen.FieldEffects.OppFutureSightDamage = MyBase.GetDamage(False, own, Not own, BattleScreen)

                    BattleScreen.BattleQuery.Add(New TextQueryObject(p.GetDisplayName() & " foresaw an attack!"))
                Else
                    BattleScreen.BattleQuery.Add(New TextQueryObject(Me.Name & " failed!"))
                End If
            End If
        End Sub

    End Class

End Namespace