Namespace BattleSystem.Moves.Electric

    Public Class Charge

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Electric)
            Me.ID = 268
            Me.OriginalPP = 20
            Me.CurrentPP = 20
            Me.MaxPP = 20
            Me.Power = 0
            Me.Accuracy = 0
            Me.Category = Categories.Status
            Me.ContestCategory = ContestCategories.Smart
            Me.Name = "Charge"
            Me.Description = "The user boosts the power of the Electric move it uses on the next turn. It also raises the user's Sp. Def stat."
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
            Me.HasSecondaryEffect = True
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

            Me.AIField1 = AIField.RaiseSpDefense
            Me.AIField2 = AIField.Nothing
        End Sub

        Public Overrides Sub MoveHits(own As Boolean, BattleScreen As BattleScreen)
            If own = True Then
                BattleScreen.FieldEffects.OwnCharge = 2
                BattleScreen.BattleQuery.Add(New TextQueryObject(BattleScreen.OwnPokemon.GetDisplayName() & " began charging power."))
            Else
                BattleScreen.FieldEffects.OppCharge = 2
                BattleScreen.BattleQuery.Add(New TextQueryObject(BattleScreen.OppPokemon.GetDisplayName() & " began charging power."))
            End If

            BattleScreen.Battle.RaiseStat(own, own, BattleScreen, "Special Defense", 1, "", "move:charge")
        End Sub

    End Class

End Namespace