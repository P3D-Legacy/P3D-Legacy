Namespace BattleSystem.Moves.Fire

    Public Class BurnUp

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Fire)
            Me.ID = 682
            Me.OriginalPP = 5
            Me.CurrentPP = 5
            Me.MaxPP = 5
            Me.Power = 130
            Me.Accuracy = 100
            Me.Category = Categories.Special
            Me.ContestCategory = ContestCategories.Cool
            Me.Name = Localization.GetString("move_name_" & Me.ID,"Burn Up")
            Me.Description = "To inflict massive damage, the user burns itself out. After using this move, the user will no longer be Fire type."
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
            Me.KingsrockAffected = True
            Me.CounterAffected = False

            Me.DisabledWhileGravity = False
            Me.UseEffectiveness = True
            Me.ImmunityAffected = True
            Me.HasSecondaryEffect = False
            Me.RemovesFrozen = True

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

        Public Overrides Function MoveFailBeforeAttack(Own As Boolean, BattleScreen As BattleScreen) As Boolean
            Dim p As Pokemon = BattleScreen.OwnPokemon
            If Own = False Then
                p = BattleScreen.OppPokemon
            End If

            If p.IsType(Element.Types.Fire) = True Then
                Return False
            Else
                Return True
            End If
        End Function

        Public Overrides Sub MoveHits(own As Boolean, BattleScreen As BattleScreen)
            Dim p As Pokemon = BattleScreen.OwnPokemon
            If own = False Then
                p = BattleScreen.OppPokemon
            End If

            p.OriginalType1 = New Element(p.Type1.Type)
            p.OriginalType2 = New Element(p.Type2.Type)

            If p.Type2.Type = Element.Types.Blank Then
                'Pure fire

                p.Type1.Type = Element.Types.Blank
            Else
                'One of the types is fire

                If p.Type1.Type = Element.Types.Fire Then
                    p.Type1.Type = p.Type2.Type
                End If
                p.Type2.Type = Element.Types.Blank
            End If

        End Sub

    End Class

End Namespace