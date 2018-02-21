Namespace BattleSystem.Moves.Dark

    Public Class Torment

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Dark)
            Me.ID = 259
            Me.OriginalPP = 15
            Me.CurrentPP = 15
            Me.MaxPP = 15
            Me.Power = 0
            Me.Accuracy = 100
            Me.Category = Categories.Status
            Me.ContestCategory = ContestCategories.Tough
            Me.Name = "Torment"
            Me.Description = "The user torments and enrages the target, making it incapable of using the same move twice in a row."
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

            Me.IsAffectedBySubstitute = True
            Me.IsOneHitKOMove = False
            Me.IsWonderGuardAffected = False
            '#End

            Me.AIField1 = AIField.Support
            Me.AIField2 = AIField.Nothing
        End Sub

        Public Overrides Sub MoveHits(own As Boolean, BattleScreen As BattleScreen)
            Dim torment As Integer = BattleScreen.FieldEffects.OppTorment
            If own = False Then
                torment = BattleScreen.FieldEffects.OwnTorment
            End If

            If torment = 0 Then
                If own = True Then
                    If BattleScreen.OppPokemon.Ability.Name.ToLower() <> "aroma veil" Then
                        BattleScreen.FieldEffects.OppTorment = 1
                    Else
                        BattleScreen.BattleQuery.Add(New TextQueryObject("Aroma Veil protected " & BattleScreen.OppPokemon.GetDisplayName() & " from " & Me.Name & "!"))
                    End If
                Else
                    If BattleScreen.OwnPokemon.Ability.Name.ToLower() <> "aroma veil" Then
                        BattleScreen.FieldEffects.OwnTorment = 1
                    Else
                        BattleScreen.BattleQuery.Add(New TextQueryObject("Aroma Veil protected " & BattleScreen.OwnPokemon.GetDisplayName() & " from " & Me.Name & "!"))
                    End If
                End If
            Else
                BattleScreen.BattleQuery.Add(New TextQueryObject(Me.Name & " failed!"))
            End If
        End Sub

    End Class

End Namespace