Namespace BattleSystem.Moves.Poison

    Public Class ClearSmog

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Poison)
            Me.ID = 499
            Me.OriginalPP = 15
            Me.CurrentPP = 15
            Me.MaxPP = 15
            Me.Power = 50
            Me.Accuracy = 0
            Me.Category = Categories.Special
            Me.ContestCategory = ContestCategories.Beauty
            Me.Name = "Clear Smog"
            Me.Description = "The user attacks by throwing a clump of special mud. All stat changes are returned to normal."
            Me.CriticalChance = 0
            Me.IsHMMove = False
            Me.Target = Targets.OneAdjacentTarget
            Me.Priority = 0
            Me.TimesToAttack = 1
            '#End

            '#SpecialDefinitions
            Me.MakesContact = False
            Me.ProtectAffected = True
            Me.MagicCoatAffected = False
            Me.SnatchAffected = False
            Me.MirrorMoveAffected = True
            Me.KingsrockAffected = False
            Me.CounterAffected = True

            Me.DisabledWhileGravity = False
            Me.UseEffectiveness = True
            Me.ImmunityAffected = True
            Me.HasSecondaryEffect = False
            Me.RemovesFrozen = False

            Me.IsHealingMove = False
            Me.IsRecoilMove = False
            Me.IsPunchingMove = False
            Me.IsDamagingMove = True
            Me.IsProtectMove = False
            Me.IsSoundMove = False

            Me.IsAffectedBySubstitute = True
            Me.IsOneHitKOMove = False
            Me.IsWonderGuardAffected = False
            Me.UseAccEvasion = False
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
