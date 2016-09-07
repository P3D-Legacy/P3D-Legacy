Namespace BattleSystem.Moves.Normal

    Public Class RapidSpin

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Normal)
            Me.ID = 229
            Me.OriginalPP = 40
            Me.CurrentPP = 40
            Me.MaxPP = 40
            Me.Power = 20
            Me.Accuracy = 100
            Me.Category = Categories.Physical
            Me.ContestCategory = ContestCategories.Cool
            Me.Name = "Rapid Spin"
            Me.Description = "A spin attack that can also eliminate such moves as Bind, Wrap, Leech Seed, and Spikes."
            Me.CriticalChance = 1
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
            Me.UseEffectiveness = True
            Me.ImmunityAffected = True
            Me.HasSecondaryEffect = True
            Me.RemovesFrozen = False

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
        End Sub

        Public Overrides Sub MoveHits(own As Boolean, BattleScreen As BattleScreen)
            With BattleScreen.FieldEffects
                If own = True Then
                    .OwnBind = 0
                    .OwnClamp = 0
                    .OwnFireSpin = 0
                    .OwnLeechSeed = 0
                    .OwnMagmaStorm = 0
                    .OwnSandTomb = 0
                    .OppSpikes = 0
                    .OppStealthRock = 0
                    .OppToxicSpikes = 0
                    .OwnWhirlpool = 0
                    .OwnWrap = 0
                Else
                    .OppBind = 0
                    .OppClamp = 0
                    .OppFireSpin = 0
                    .OppLeechSeed = 0
                    .OppMagmaStorm = 0
                    .OppSandTomb = 0
                    .OwnSpikes = 0
                    .OwnStealthRock = 0
                    .OwnToxicSpikes = 0
                    .OppWhirlpool = 0
                    .OppWrap = 0
                End If
            End With
        End Sub

    End Class

End Namespace