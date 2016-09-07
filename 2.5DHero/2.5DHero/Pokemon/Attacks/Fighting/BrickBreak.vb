Namespace BattleSystem.Moves.Fighting

    Public Class BrickBreak

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Fighting)
            Me.ID = 280
            Me.OriginalPP = 15
            Me.CurrentPP = 15
            Me.MaxPP = 15
            Me.Power = 75
            Me.Accuracy = 100
            Me.Category = Categories.Physical
            Me.ContestCategory = ContestCategories.Cool
            Me.Name = "Brick Break"
            Me.Description = "The user attacks with a swift chop. It can also break barriers, such as Light Screen and Reflect."
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

            Me.AIField1 = AIField.Damage
            Me.AIField2 = AIField.RemoveReflectLightscreen
        End Sub

        Public Overrides Sub MoveHits(own As Boolean, BattleScreen As BattleScreen)
            Dim p As Pokemon = BattleScreen.OppPokemon
            If own = False Then
                p = BattleScreen.OwnPokemon
            End If

            If p.IsType(Element.Types.Ghost) = False Then
                If own = True Then
                    If BattleScreen.FieldEffects.OppLightScreen > 0 Then
                        BattleScreen.FieldEffects.OppLightScreen = 0
                        BattleScreen.BattleQuery.Add(New TextQueryObject("The Light Screen wore off!"))
                    End If
                    If BattleScreen.FieldEffects.OppReflect > 0 Then
                        BattleScreen.FieldEffects.OppReflect = 0
                        BattleScreen.BattleQuery.Add(New TextQueryObject("The Reflect wore off!"))
                    End If
                Else
                    If BattleScreen.FieldEffects.OwnLightScreen > 0 Then
                        BattleScreen.FieldEffects.OwnLightScreen = 0
                        BattleScreen.BattleQuery.Add(New TextQueryObject("The Light Screen wore off!"))
                    End If
                    If BattleScreen.FieldEffects.OwnReflect > 0 Then
                        BattleScreen.FieldEffects.OwnReflect = 0
                        BattleScreen.BattleQuery.Add(New TextQueryObject("The Reflect wore off!"))
                    End If
                End If
            End If
        End Sub

    End Class

End Namespace