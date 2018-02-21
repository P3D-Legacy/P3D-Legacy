Namespace BattleSystem.Moves.Normal

    Public Class Struggle

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Normal)
            Me.ID = 165
            Me.OriginalPP = 1
            Me.CurrentPP = 1
            Me.MaxPP = 1
            Me.Power = 50
            Me.Accuracy = 0
            Me.Category = Categories.Physical
            Me.ContestCategory = ContestCategories.Cool
            Me.Name = "Struggle"
            Me.Description = "An attack that is used in desperation only if the user has no PP. It also hurts the user slightly."
            Me.CriticalChance = 0
            Me.IsHMMove = False
            Me.Target = Targets.OneAdjacentTarget
            Me.Priority = 0
            Me.TimesToAttack = 1
            '#End

            '#SpecialDefinitions
            Me.MakesContact = True
            Me.ProtectAffected = True
            Me.MagicCoatAffected = False
            Me.SnatchAffected = False
            Me.MirrorMoveAffected = True
            Me.KingsrockAffected = True
            Me.CounterAffected = True

            Me.DisabledWhileGravity = False
            Me.UseEffectiveness = False
            Me.ImmunityAffected = False
            Me.RemovesFrozen = False
            Me.HasSecondaryEffect = False

            Me.IsHealingMove = False
            Me.IsRecoilMove = False
            Me.IsPunchingMove = False
            Me.IsDamagingMove = True
            Me.IsProtectMove = False
            Me.IsSoundMove = False

            Me.IsAffectedBySubstitute = True
            Me.IsOneHitKOMove = False
            Me.IsWonderGuardAffected = False
            Me.CanGainSTAB = False
            Me.UseAccEvasion = False
            '#End
        End Sub

        Public Overrides Sub MoveSelected(own As Boolean, BattleScreen As BattleScreen)
            BattleScreen.BattleQuery.Add(New TextQueryObject(BattleScreen.OwnPokemon.GetDisplayName() & " has no PP left!"))
        End Sub

        Public Overrides Sub MoveHits(own As Boolean, BattleScreen As BattleScreen)
            Dim p As Pokemon = BattleScreen.OwnPokemon
            If own = False Then
                p = BattleScreen.OppPokemon
            End If

            Dim recoilDamage As Integer = CInt(Math.Ceiling(p.MaxHP / 4))
            If recoilDamage <= 0 Then
                recoilDamage = 1
            End If

            BattleScreen.Battle.ReduceHP(recoilDamage, own, own, BattleScreen, p.GetDisplayName() & " is damaged by recoil!", "move:struggle")
        End Sub

    End Class

End Namespace