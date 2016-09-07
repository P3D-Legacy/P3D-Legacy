Namespace BattleSystem.Moves.Normal

    Public Class Safeguard

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Normal)
            Me.ID = 219
            Me.OriginalPP = 25
            Me.CurrentPP = 25
            Me.MaxPP = 25
            Me.Power = 0
            Me.Accuracy = 0
            Me.Category = Categories.Status
            Me.ContestCategory = ContestCategories.Beauty
            Me.Name = "Safeguard"
            Me.Description = "The user creates a protective field that prevents status problems for five turns."
            Me.CriticalChance = 0
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

        Public Overrides Sub MoveHits(own As Boolean, BattleScreen As BattleScreen)
            Dim guard As Integer = BattleScreen.FieldEffects.OwnSafeguard
            If own = False Then
                guard = BattleScreen.FieldEffects.OppSafeguard
            End If

            If guard = 0 Then
                If own = True Then
                    BattleScreen.FieldEffects.OwnSafeguard = 5

                    BattleScreen.BattleQuery.Add(New TextQueryObject("Your team became cloaked in a mystical veil!"))
                Else
                    BattleScreen.FieldEffects.OppSafeguard = 5

                    BattleScreen.BattleQuery.Add(New TextQueryObject("The other team became cloaked in a mystical veil!"))
                End If
            Else
                BattleScreen.BattleQuery.Add(New TextQueryObject(Me.Name & " failed!"))
            End If
        End Sub

    End Class

End Namespace