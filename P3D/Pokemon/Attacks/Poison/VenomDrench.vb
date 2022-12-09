Namespace BattleSystem.Moves.Poison

    Public Class VenomDrench

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Poison)
            Me.ID = 599
            Me.OriginalPP = 20
            Me.CurrentPP = 20
            Me.MaxPP = 20
            Me.Power = 0
            Me.Accuracy = 100
            Me.Category = Categories.Status
            Me.ContestCategory = ContestCategories.Smart
            Me.Name = Localization.GetString("move_name_" & Me.ID,"Venom Drench")
            Me.Description = "Opposing Pokémon are drenched in an odd poisonous liquid. This lowers the Attack, Sp. Atk, and Speed stats of a poisoned target."
            Me.CriticalChance = 0
            Me.IsHMMove = False
            Me.Target = Targets.AllAdjacentFoes
            Me.Priority = 0
            Me.TimesToAttack = 1
            '#End

            '#SpecialDefinitions
            Me.MakesContact = False
            Me.ProtectAffected = True
            Me.MagicCoatAffected = True
            Me.SnatchAffected = False
            Me.MirrorMoveAffected = False
            Me.KingsrockAffected = False
            Me.CounterAffected = False

            Me.DisabledWhileGravity = False
            Me.UseEffectiveness = True
            Me.ImmunityAffected = True
            Me.HasSecondaryEffect = False
            Me.RemovesFrozen = False

            Me.IsHealingMove = False
            Me.IsRecoilMove = False
            Me.IsPunchingMove = False
            Me.IsDamagingMove = False
            Me.IsProtectMove = False
            Me.IsSoundMove = False

            Me.IsAffectedBySubstitute = True
            Me.IsOneHitKOMove = False
            Me.IsWonderGuardAffected = False
            '#End

        End Sub

        Public Overrides Sub MoveHits(own As Boolean, BattleScreen As BattleScreen)
            Dim op As Pokemon = BattleScreen.OppPokemon
            If own = False Then
                op = BattleScreen.OwnPokemon
            End If
            If op.Status = Pokemon.StatusProblems.Poison OrElse op.Status = Pokemon.StatusProblems.BadPoison Then
                BattleScreen.Battle.LowerStat(Not own, own, BattleScreen, "Attack", 1, "", "move:venomdrench")
                BattleScreen.Battle.LowerStat(Not own, own, BattleScreen, "Special Attack", 1, "", "move:venomdrench")
                BattleScreen.Battle.LowerStat(Not own, own, BattleScreen, "Speed", 1, "", "move:venomdrench")
            Else
                BattleScreen.BattleQuery.Add(New TextQueryObject(Me.Name & " failed!"))
            End If
        End Sub

    End Class

End Namespace