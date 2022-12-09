Namespace BattleSystem.Moves.Psychic

    Public Class PowerSwap

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Psychic)
            Me.ID = 384
            Me.OriginalPP = 10
            Me.CurrentPP = 10
            Me.MaxPP = 10
            Me.Power = 0
            Me.Accuracy = 0
            Me.Category = Categories.Status
            Me.ContestCategory = ContestCategories.Smart
            Me.Name = Localization.GetString("move_name_" & Me.ID,"Power Swap")
            Me.Description = "The user employs its psychic power to switch changes to its Attack and Sp. Atk stats with the target."
            Me.CriticalChance = 0
            Me.IsHMMove = False
            Me.Target = Targets.OneAdjacentTarget
            Me.Priority = 0
            Me.TimesToAttack = 1
            '#End

            '#SpecialDefinitions
            Me.MakesContact = False
            Me.ProtectAffected = True
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

            Dim userattack As Integer = p.StatAttack
            Dim recvattack As Integer = op.StatAttack
            Dim userspattack As Integer = p.StatSpAttack
            Dim recvspattack As Integer = op.StatSpAttack

            p.StatAttack = recvattack
            op.StatAttack = userattack
            p.StatSpAttack = recvspattack
            op.StatSpAttack = userspattack

            BattleScreen.BattleQuery.Add(New TextQueryObject(p.GetDisplayName() & " switched all changes to its Attack and Sp. Atk with the target!"))

        End Sub

    End Class

End Namespace