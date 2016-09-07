Namespace BattleSystem.Moves.Ice

    Public Class Mist

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Ice)
            Me.ID = 54
            Me.OriginalPP = 30
            Me.CurrentPP = 30
            Me.MaxPP = 30
            Me.Power = 0
            Me.Accuracy = 0
            Me.Category = Categories.Status
            Me.ContestCategory = ContestCategories.Beauty
            Me.Name = "Mist"
            Me.Description = "The user cloaks its body with a white mist that prevents any of its stats from being cut for five turns."
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
            Me.IsProtectMove = False
            Me.IsSoundMove = False

            Me.IsAffectedBySubstitute = False
            Me.IsOneHitKOMove = False
            Me.IsWonderGuardAffected = False
            '#End

            Me.AIField1 = AIField.Support
            Me.AIField2 = AIField.Nothing
        End Sub

        Public Overrides Sub MoveHits(own As Boolean, BattleScreen As BattleScreen)
            Dim b As Boolean = True

            If own = True Then
                If BattleScreen.FieldEffects.OwnMist = 0 Then
                    BattleScreen.FieldEffects.OwnMist = 5
                    BattleScreen.BattleQuery.Add(New TextQueryObject("Your team became shrouded in mist!"))
                Else
                    b = False
                End If
            Else
                If BattleScreen.FieldEffects.OppMist = 0 Then
                    BattleScreen.FieldEffects.OppMist = 5
                    BattleScreen.BattleQuery.Add(New TextQueryObject("The opponent team became shrouded in mist!"))
                Else
                    b = False
                End If
            End If


            If b = False Then
                BattleScreen.BattleQuery.Add(New TextQueryObject(Me.Name & " failed!"))
            End If
        End Sub

    End Class

End Namespace