Namespace BattleSystem.Moves.Electric

    Public Class MagnetRise

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Electric)
            Me.ID = 393
            Me.OriginalPP = 10
            Me.CurrentPP = 10
            Me.MaxPP = 10
            Me.Power = 0
            Me.Accuracy = 0
            Me.Category = Categories.Status
            Me.ContestCategory = ContestCategories.Smart
            Me.Name = "Magnet Rise"
            Me.Description = "The user levitates using electrically generated magnetism for five turns."
            Me.CriticalChance = 1
            Me.IsHMMove = False
            Me.Target = Targets.Self
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

            Me.AIField1 = AIField.Support
            Me.AIField2 = AIField.Nothing
        End Sub

        Public Overrides Sub MoveHits(own As Boolean, BattleScreen As BattleScreen)
            Dim p As Pokemon = BattleScreen.OwnPokemon
            If own = False Then
                p = BattleScreen.OppPokemon
            End If
            If own = True Then
                If BattleScreen.FieldEffects.OwnMagnetRise = 0 Then
                    BattleScreen.FieldEffects.OwnMagnetRise = 5
                    BattleScreen.BattleQuery.Add(New TextQueryObject(p.GetDisplayName() & " levitated on electromagnetism!"))
                Else
                    BattleScreen.BattleQuery.Add(New TextQueryObject("but it failed!"))
                End If
            Else
                If BattleScreen.FieldEffects.OppMagnetRise = 0 Then
                    BattleScreen.FieldEffects.OppMagnetRise = 5
                    BattleScreen.BattleQuery.Add(New TextQueryObject(p.GetDisplayName() & " levitated on electromagnetism!"))
                Else
                    BattleScreen.BattleQuery.Add(New TextQueryObject("but it failed!"))
                End If
            End If
        End Sub

    End Class

End Namespace
