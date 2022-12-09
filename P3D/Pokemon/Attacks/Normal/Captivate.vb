Namespace BattleSystem.Moves.Normal

    Public Class Captivate

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Normal)
            Me.ID = 445
            Me.OriginalPP = 20
            Me.CurrentPP = 20
            Me.MaxPP = 20
            Me.Power = 0
            Me.Accuracy = 100
            Me.Category = Categories.Status
            Me.ContestCategory = ContestCategories.Cute
            Me.Name = Localization.GetString("move_name_" & Me.ID,"Captivate")
            Me.Description = "If any opposing Pokemon is the opposite gender of the user, it is charmed, which harshly lowers its Sp. Atk stat."
            Me.CriticalChance = 0
            Me.IsHMMove = False
            Me.Target = Targets.AllAdjacentFoes
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
            Me.IsSoundMove = True

            Me.IsAffectedBySubstitute = True
            Me.IsOneHitKOMove = False
            Me.IsWonderGuardAffected = False
            '#End

            Me.AIField1 = AIField.LowerAttack
            Me.AIField2 = AIField.Nothing
        End Sub

        Public Overrides Sub MoveHits(own As Boolean, BattleScreen As BattleScreen)
            Dim op As Pokemon = BattleScreen.OppPokemon
            Dim p As Pokemon = BattleScreen.OwnPokemon
            If own = False Then
                op = BattleScreen.OwnPokemon
                p = BattleScreen.OppPokemon
            End If

            If op.Ability.Name.ToLower() = "oblivious" And BattleScreen.FieldEffects.CanUseAbility(Not own, BattleScreen) = True Then
                BattleScreen.BattleQuery.Add(New TextQueryObject(Me.Name & " failed!"))
            Else
                If op.Gender <> Pokemon.Genders.Genderless And op.Gender <> p.Gender Then
                    If BattleScreen.Battle.LowerStat(Not own, own, BattleScreen, "Special Attack", 1, "", "move:captivate") = False Then
                        BattleScreen.BattleQuery.Add(New TextQueryObject(Me.Name & " failed!"))
                    End If
                Else
                    BattleScreen.BattleQuery.Add(New TextQueryObject(Me.Name & " failed!"))
                End If
            End If
        End Sub

    End Class

End Namespace