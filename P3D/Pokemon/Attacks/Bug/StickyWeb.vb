Namespace BattleSystem.Moves.Bug

    Public Class StickyWeb

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Bug)
            Me.ID = 564
            Me.OriginalPP = 20
            Me.CurrentPP = 20
            Me.MaxPP = 20
            Me.Power = 0
            Me.Accuracy = 0
            Me.Category = Categories.Status
            Me.ContestCategory = ContestCategories.Tough
            Me.Name = "Sticky Web"
            Me.Description = "The user weaves a sticky net around the opposing team, which lowers their Speed stat upon switching into battle."
            Me.CriticalChance = 0
            Me.IsHMMove = False
            Me.Target = Targets.AllFoes
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
            '#End

            Me.AIField1 = AIField.Support
            Me.AIField2 = AIField.Nothing
        End Sub

        Public Overrides Sub MoveHits(own As Boolean, BattleScreen As BattleScreen)
            Dim stickyweb As Integer = 0
            If own = True Then
                stickyweb = BattleScreen.FieldEffects.OwnStickyWeb
            Else
                stickyweb = BattleScreen.FieldEffects.OppStickyWeb
            End If
            If stickyweb < 1 Then
                If own = True Then
                    BattleScreen.FieldEffects.OwnStickyWeb += 1
                    BattleScreen.BattleQuery.Add(New TextQueryObject("A sticky web has been laid beneath the opposite team's feet!"))
                Else
                    BattleScreen.FieldEffects.OppStickyWeb += 1
                    BattleScreen.BattleQuery.Add(New TextQueryObject("A sticky web has been laid beneath your team's feet!"))
                End If
            Else
                BattleScreen.BattleQuery.Add(New TextQueryObject(Me.Name & " failed!"))
            End If
        End Sub

    End Class

End Namespace
