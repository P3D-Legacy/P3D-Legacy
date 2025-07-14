Namespace BattleSystem.Moves.Ghost

    Public Class RageFist

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Ghost)
            Me.ID = 889
            Me.OriginalPP = 10
            Me.CurrentPP = 10
            Me.MaxPP = 10
            Me.Power = 50
            Me.Accuracy = 100
            Me.Category = Categories.Physical
            Me.ContestCategory = ContestCategories.Tough
            Me.Name = Localization.GetString("move_name_" & Me.ID, "Rage Fist")
            Me.Description = "The user converts its rage into energy to attack. The more times the user has been hit by attacks, the greater the move's power."
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

            Me.DisabledWhileGravity = False
            Me.UseEffectiveness = True
            Me.ImmunityAffected = True
            Me.RemovesOwnFrozen = False
            Me.HasSecondaryEffect = False

            Me.IsHealingMove = False
            Me.IsRecoilMove = False
            Me.IsPunchingMove = True
            Me.IsDamagingMove = True
            Me.IsProtectMove = False


            Me.IsAffectedBySubstitute = True
            Me.IsOneHitKOMove = False
            Me.IsWonderGuardAffected = True
            '#End

            Me.AIField1 = AIField.Damage
            Me.AIField2 = AIField.Nothing
        End Sub

        Public Overrides Function GetBasePower(ByVal own As Boolean, ByVal BattleScreen As BattleScreen) As Integer
            If own = True Then
                Return Me.Power + BattleScreen.FieldEffects.OwnRageFistPower
            Else
                Return Me.Power + BattleScreen.FieldEffects.OppRageFistPower
            End If
        End Function
        Public Overrides Sub HurtItselfInConfusion(ByVal own As Boolean, ByVal BattleScreen As BattleScreen)
            If own = True Then
                If BattleScreen.FieldEffects.OwnRageFistPower < 350 Then
                    BattleScreen.FieldEffects.OwnRageFistPower += 50
                End If
            Else
                If BattleScreen.FieldEffects.OppRageFistPower < 350 Then
                    BattleScreen.FieldEffects.OppRageFistPower += 50
                End If
            End If

        End Sub
    End Class

End Namespace