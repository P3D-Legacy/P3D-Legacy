Namespace BattleSystem.Moves.Fighting

    Public Class JumpKick

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Fighting)
            Me.ID = 26
            Me.OriginalPP = 10
            Me.CurrentPP = 10
            Me.MaxPP = 10
            Me.Power = 100
            Me.Accuracy = 95
            Me.Category = Categories.Physical
            Me.ContestCategory = ContestCategories.Cool
            Me.Name = "Jump Kick"
            Me.Description = "The user jumps up high, then strikes with a kick. If the kick misses, the user hurts itself."
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

            Me.DisabledWhileGravity = True
            Me.UseEffectiveness = True
            Me.ImmunityAffected = True
            Me.HasSecondaryEffect = False
            Me.RemovesFrozen = False

            Me.IsHealingMove = False
            Me.IsRecoilMove = True
            Me.IsPunchingMove = False
            Me.IsDamagingMove = True
            Me.IsProtectMove = False
            Me.IsSoundMove = False

            Me.IsAffectedBySubstitute = True
            Me.IsOneHitKOMove = False
            Me.IsWonderGuardAffected = True
            '#End

            Me.AIField1 = AIField.Damage
            Me.AIField2 = AIField.Recoil
        End Sub

        Private Sub InflictCrashDamage(ByVal own As Boolean, ByVal BattleScreen As BattleScreen)
            Dim p As Pokemon = BattleScreen.OwnPokemon
            If own = False Then
                p = BattleScreen.OppPokemon
            End If

            BattleScreen.Battle.InflictRecoil(own, own, BattleScreen, Me, CInt(Math.Floor(p.HP / 2)), p.GetDisplayName() & " struggled and crashed!", "move:jumpkick")
        End Sub

        Public Overrides Sub MoveMisses(own As Boolean, BattleScreen As BattleScreen)
            InflictCrashDamage(own, BattleScreen)
        End Sub

        Public Overrides Sub MoveProtectedDetected(own As Boolean, BattleScreen As BattleScreen)
            InflictCrashDamage(own, BattleScreen)
        End Sub

        Public Overrides Sub MoveHasNoEffect(own As Boolean, BattleScreen As BattleScreen)
            InflictCrashDamage(own, BattleScreen)
        End Sub

    End Class

End Namespace