Namespace BattleSystem.Moves.Water

    Public Class AquaRing

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Water)
            Me.ID = 392
            Me.OriginalPP = 20
            Me.CurrentPP = 20
            Me.MaxPP = 20
            Me.Power = 0
            Me.Accuracy = 0
            Me.Category = Categories.Status
            Me.ContestCategory = ContestCategories.Beauty
            Me.Name = "Aqua Ring"
            Me.Description = "The user envelops itself in a veil made of water. It regains some HP on every turn."
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
            If own = True Then
                If BattleScreen.FieldEffects.OwnAquaRing = 0 Then
                    BattleScreen.FieldEffects.OwnAquaRing = 1
                    BattleScreen.BattleQuery.Add(New TextQueryObject(BattleScreen.OwnPokemon.GetDisplayName() & " surrounded itself with a veil of water!"))
                Else
                    BattleScreen.BattleQuery.Add(New TextQueryObject(Me.Name & " failed!"))
                End If
            Else
                If BattleScreen.FieldEffects.OppAquaRing = 0 Then
                    BattleScreen.FieldEffects.OppAquaRing = 1
                    BattleScreen.BattleQuery.Add(New TextQueryObject(BattleScreen.OppPokemon.GetDisplayName() & " surrounded itself with a veil of water!"))
                Else
                    BattleScreen.BattleQuery.Add(New TextQueryObject(Me.Name & " failed!"))
                End If
            End If
        End Sub

    End Class

End Namespace