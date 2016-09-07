Namespace BattleSystem.Moves.Flying

    Public Class Tailwind

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Flying)
            Me.ID = 366
            Me.OriginalPP = 15
            Me.CurrentPP = 15
            Me.MaxPP = 15
            Me.Power = 0
            Me.Accuracy = 0
            Me.Category = Categories.Status
            Me.ContestCategory = ContestCategories.Smart
            Me.Name = "Tailwind"
            Me.Description = "The user whips up a turbulent whirlwind that ups the Speed stat of the user and its allies for four turns."
            Me.CriticalChance = 1
            Me.IsHMMove = False
            Me.Target = Targets.AllAllies
            Me.Priority = 0
            Me.TimesToAttack = 1
            '#End

            '#SpecialDefinitions
            Me.MakesContact = False
            Me.ProtectAffected = False
            Me.MagicCoatAffected = False
            Me.SnatchAffected = True
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

            Me.AIField1 = AIField.RaiseSpeed
            Me.AIField2 = AIField.Nothing
        End Sub

        Public Overrides Sub MoveHits(own As Boolean, BattleScreen As BattleScreen)
            If own = True Then
                If BattleScreen.FieldEffects.OwnTailWind = 0 Then
                    BattleScreen.FieldEffects.OwnTailWind = 5
                    BattleScreen.BattleQuery.Add(New TextQueryObject("The Tailwind blew from behind the team!"))
                Else
                    BattleScreen.BattleQuery.Add(New TextQueryObject("Tailwind failed!"))
                End If
            Else
                If BattleScreen.FieldEffects.OppTailWind = 0 Then
                    BattleScreen.FieldEffects.OppTailWind = 5
                    BattleScreen.BattleQuery.Add(New TextQueryObject("The Tailwind blew from behind the team!"))
                Else
                    BattleScreen.BattleQuery.Add(New TextQueryObject("Tailwind failed!"))
                End If
            End If
        End Sub

    End Class

End Namespace