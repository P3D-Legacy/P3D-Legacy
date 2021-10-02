Namespace BattleSystem.Moves.Grass

    Public Class GrassyTerrain

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Grass)
            Me.ID = 580
            Me.OriginalPP = 10
            Me.CurrentPP = 10
            Me.MaxPP = 10
            Me.Power = 0
            Me.Accuracy = 0
            Me.Category = Categories.Status
            Me.ContestCategory = ContestCategories.Beauty
            Me.Name = "Grassy Terrain"
            Me.Description = "The user turns the ground to grass for five turns. This restores the HP of Pokémon on the ground a little every turn and powers up Grass type-moves."
            Me.CriticalChance = 0
            Me.IsHMMove = False
            Me.Target = Targets.All
            Me.Priority = 0
            Me.TimesToAttack = 1
            '#End

            '#SpecialDefinitions
            Me.MakesContact = False
            Me.ProtectAffected = False
            Me.MagicCoatAffected = False
            Me.SnatchAffected = False
            Me.MirrorMoveAffected = False
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
            Dim turns As Integer = BattleCalculation.FieldEffectTurns(BattleScreen, own, Me.Name.ToLower(Globalization.CultureInfo.InvariantCulture))
            If BattleScreen.FieldEffects.GrassyTerrain <= 0 Then

                BattleScreen.FieldEffects.ElectricTerrain = 0
                BattleScreen.FieldEffects.GrassyTerrain = turns
                BattleScreen.FieldEffects.PsychicTerrain = 0
                BattleScreen.FieldEffects.MistyTerrain = 0

                BattleScreen.BattleQuery.Add(New TextQueryObject("Grass grew to cover the battlefield!"))
            Else
                BattleScreen.BattleQuery.Add(New TextQueryObject(Me.Name & " failed!"))
            End If
        End Sub

    End Class

End Namespace