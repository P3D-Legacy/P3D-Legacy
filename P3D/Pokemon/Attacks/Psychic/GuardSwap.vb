Namespace BattleSystem.Moves.Psychic

    Public Class GuardSwap

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Psychic)
            Me.ID = 385
            Me.OriginalPP = 10
            Me.CurrentPP = 10
            Me.MaxPP = 10
            Me.Power = 0
            Me.Accuracy = 0
            Me.Category = Categories.Status
            Me.ContestCategory = ContestCategories.Smart
            Me.Name = Localization.GetString("move_name_" & Me.ID,"Guard Swap")
            Me.Description = "The user employs its psychic power to switch changes to its Defense and Sp. Def stats with the target."
            Me.CriticalChance = 0
            Me.IsHMMove = False
            Me.Target = Targets.Self
            Me.Priority = 0
            Me.TimesToAttack = 1
            '#End

            '#SpecialDefinitions
            Me.MakesContact = False
            Me.ProtectAffected = False
            Me.MagicCoatAffected = True
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
            Me.UseAccEvasion = False
            '#End

            Me.AIField1 = AIField.RaiseAttack
            Me.AIField2 = AIField.RaiseSpAttack
        End Sub

        Public Overrides Sub MoveHits(own As Boolean, BattleScreen As BattleScreen)
            Dim p As Pokemon = BattleScreen.OwnPokemon
            Dim op As Pokemon = BattleScreen.OppPokemon
            If own = False Then
                p = BattleScreen.OppPokemon
                op = BattleScreen.OwnPokemon
            End If

            Dim userdefense As Integer = p.StatDefense
            Dim recvdefense As Integer = op.StatDefense
            Dim userspdefense As Integer = p.StatSpDefense
            Dim recvspdefense As Integer = op.StatSpDefense

            p.StatDefense = recvdefense
            op.StatDefense = userdefense
            p.StatSpDefense = recvspdefense
            op.StatSpDefense = userspdefense

            BattleScreen.BattleQuery.Add(New TextQueryObject(p.GetDisplayName() & "switched all changes to its Defense and Sp. Def with the target!"))

        End Sub

    End Class

End Namespace