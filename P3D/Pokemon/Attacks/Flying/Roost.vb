Namespace BattleSystem.Moves.Flying

    Public Class Roost

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Flying)
            Me.ID = 355
            Me.OriginalPP = 10
            Me.CurrentPP = 10
            Me.MaxPP = 10
            Me.Power = 0
            Me.Accuracy = 0
            Me.Category = Categories.Status
            Me.ContestCategory = ContestCategories.Cool
            Me.Name = "Roost"
            Me.Description = "The user lands and rests its body. It restores the user's HP by up to half of its max HP."
            Me.CriticalChance = 0
            Me.IsHMMove = False
            Me.Target = Targets.Self
            Me.Priority = 0
            Me.TimesToAttack = 1
            '#End

            '#SpecialDefinitions
            Me.MakesContact = False
            Me.ProtectAffected = False
            Me.MagicCoatAffected = False
            Me.SnatchAffected = True
            Me.MirrorMoveAffected = True
            Me.KingsrockAffected = False
            Me.CounterAffected = False

            Me.DisabledWhileGravity = False
            Me.UseEffectiveness = False
            Me.ImmunityAffected = False
            Me.HasSecondaryEffect = False
            Me.RemovesFrozen = False

            Me.IsHealingMove = True
            Me.IsRecoilMove = False
            Me.IsPunchingMove = False
            Me.IsDamagingMove = False
            Me.IsProtectMove = False
            Me.IsSoundMove = False

            Me.IsAffectedBySubstitute = False
            Me.IsOneHitKOMove = False
            Me.IsWonderGuardAffected = False
            '#End

            Me.AIField1 = AIField.Healing
            Me.AIField2 = AIField.Nothing
        End Sub

        Public Overrides Sub MoveHits(own As Boolean, BattleScreen As BattleScreen)
            Dim p As Pokemon = BattleScreen.OwnPokemon
            If own = False Then
                p = BattleScreen.OppPokemon
            End If

            Dim restoreHP As Integer = CInt(Math.Ceiling(p.MaxHP / 2))

            If restoreHP > 0 And p.HP < p.MaxHP And p.HP > 0 Then
                BattleScreen.Battle.GainHP(restoreHP, own, own, BattleScreen, p.GetDisplayName() & "'s HP was restored!", "move:roost")

                If p.IsType(Element.Types.Flying) = True Then
                    p.OriginalType1 = New Element(p.Type1.Type)
                    p.OriginalType2 = New Element(p.Type2.Type)

                    If p.Type2.Type = Element.Types.Blank Then
                        'Pure flying

                        p.Type1.Type = Element.Types.Normal
                    Else
                        'One of the types is flying

                        If p.Type1.Type = Element.Types.Flying Then
                            p.Type1.Type = p.Type2.Type
                        End If
                        p.Type2.Type = Element.Types.Blank
                    End If

                    If own = True Then
                        BattleScreen.FieldEffects.OwnRoostUsed = True
                    Else
                        BattleScreen.FieldEffects.OppRoostUsed = True
                    End If
                End If
            Else
                BattleScreen.BattleQuery.Add(New TextQueryObject(Me.Name & " failed!"))
            End If
        End Sub

    End Class

End Namespace