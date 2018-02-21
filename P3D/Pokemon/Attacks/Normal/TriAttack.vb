Namespace BattleSystem.Moves.Normal

    Public Class TriAttack

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Normal)
            Me.ID = 161
            Me.OriginalPP = 10
            Me.CurrentPP = 10
            Me.MaxPP = 10
            Me.Power = 80
            Me.Accuracy = 100
            Me.Category = Categories.Special
            Me.ContestCategory = ContestCategories.Beauty
            Me.Name = "Tri attack"
            Me.Description = "The user strikes with a simultaneous three-beam attack. May also burn, freeze, or leave the target with paralysis."
            Me.CriticalChance = 1
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
            Me.CounterAffected = False

            Me.DisabledWhileGravity = False
            Me.UseEffectiveness = True
            Me.ImmunityAffected = True
            Me.RemovesFrozen = False
            Me.HasSecondaryEffect = True

            Me.IsHealingMove = False
            Me.IsRecoilMove = False
            Me.IsPunchingMove = False
            Me.IsDamagingMove = True
            Me.IsProtectMove = False
            Me.IsSoundMove = False

            Me.IsAffectedBySubstitute = True
            Me.IsOneHitKOMove = False
            Me.IsWonderGuardAffected = True
            '#End

            Me.EffectChances.Add(20)
        End Sub

        Public Overrides Sub MoveHits(own As Boolean, BattleScreen As BattleScreen)
            If Core.Random.Next(0, 100) < GetEffectChance(0, own, BattleScreen) Then
                Dim r As Integer = Core.Random.Next(0, 3)
                Select Case r
                    Case 0
                        BattleScreen.Battle.InflictParalysis(Not own, own, BattleScreen, "", "move:triattack")
                    Case 1
                        BattleScreen.Battle.InflictBurn(Not own, own, BattleScreen, "", "move:triattack")
                    Case 2
                        BattleScreen.Battle.InflictFreeze(Not own, own, BattleScreen, "", "move:triattack")
                End Select
            End If
        End Sub

    End Class

End Namespace