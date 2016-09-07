Namespace BattleSystem.Moves.Ice

    Public Class Haze

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Ice)
            Me.ID = 114
            Me.OriginalPP = 30
            Me.CurrentPP = 30
            Me.MaxPP = 30
            Me.Power = 0
            Me.Accuracy = 0
            Me.Category = Categories.Status
            Me.ContestCategory = ContestCategories.Beauty
            Me.Name = "Haze"
            Me.Description = "The user creates a haze that eliminates every stat change among all the Pokémon engaged in battle."
            Me.CriticalChance = 0
            Me.IsHMMove = False
            Me.Target = Targets.AllTargets
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
            With BattleScreen.OwnPokemon
                .StatAttack = 0
                .StatDefense = 0
                .StatSpAttack = 0
                .StatSpDefense = 0
                .StatSpeed = 0
                .Accuracy = 0
                .Evasion = 0
            End With
            With BattleScreen.OppPokemon
                .StatAttack = 0
                .StatDefense = 0
                .StatSpAttack = 0
                .StatSpDefense = 0
                .StatSpeed = 0
                .Accuracy = 0
                .Evasion = 0
            End With
            BattleScreen.BattleQuery.Add(New TextQueryObject("All stat changes were eliminated!"))
        End Sub

    End Class

End Namespace