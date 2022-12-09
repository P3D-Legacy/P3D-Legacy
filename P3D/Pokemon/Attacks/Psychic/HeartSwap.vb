Namespace BattleSystem.Moves.Psychic

    Public Class HeartSwap

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Psychic)
            Me.ID = 391
            Me.OriginalPP = 10
            Me.CurrentPP = 10
            Me.MaxPP = 10
            Me.Power = 0
            Me.Accuracy = 0
            Me.Category = Categories.Status
            Me.ContestCategory = ContestCategories.Smart
            Me.Name = Localization.GetString("move_name_" & Me.ID,"Heart Swap")
            Me.Description = "Heart Swap switches any increase and/or decrease in stat changes with the target."
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

            Dim recvattack As Integer = op.StatAttack
            Dim recvdefense As Integer = op.StatDefense
            Dim recvspattack As Integer = op.StatSpAttack
            Dim recvspdefense As Integer = op.StatSpDefense
            Dim recvspeed As Integer = op.StatSpeed
            Dim recvaccuracy As Integer = op.Accuracy
            Dim recvevasion As Integer = op.Evasion

            Dim userattack As Integer = p.StatAttack
            Dim userdefense As Integer = p.StatDefense
            Dim userspattack As Integer = p.StatSpAttack
            Dim userspdefense As Integer = p.StatSpDefense
            Dim userspeed As Integer = p.StatSpeed
            Dim useraccuracy As Integer = p.Accuracy
            Dim userevasion As Integer = p.Evasion

            p.StatAttack = recvattack
            p.StatDefense = recvdefense
            p.StatSpAttack = recvspattack
            p.StatSpDefense = recvspdefense
            p.StatSpeed = recvspeed
            p.Accuracy = recvaccuracy
            p.Evasion = recvevasion

            op.StatAttack = userattack
            op.StatDefense = userdefense
            op.StatSpAttack = userspattack
            op.StatSpDefense = userspdefense
            op.StatSpeed = userspeed
            op.Accuracy = useraccuracy
            op.Evasion = userevasion

            BattleScreen.BattleQuery.Add(New TextQueryObject(p.GetDisplayName() & "switched stat changes with the target!"))

        End Sub

    End Class

End Namespace