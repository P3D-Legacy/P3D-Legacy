Namespace BattleSystem.Moves.Normal

    Public Class Attract

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Normal)
            Me.ID = 213
            Me.OriginalPP = 15
            Me.CurrentPP = 15
            Me.MaxPP = 15
            Me.Power = 0
            Me.Accuracy = 100
            Me.Category = Categories.Status
            Me.ContestCategory = ContestCategories.Cute
            Me.Name = Localization.GetString("move_name_" & Me.ID,"Attract")
            Me.Description = "If it is the opposite gender of the user, the target becomes infatuated and less likely to attack."
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
            Me.ImmunityAffected = True
            Me.HasSecondaryEffect = False
            Me.RemovesFrozen = False

            Me.IsHealingMove = False
            Me.IsRecoilMove = False

            Me.IsDamagingMove = False
            Me.IsProtectMove = False


            Me.IsAffectedBySubstitute = True
            Me.IsOneHitKOMove = False
            Me.IsWonderGuardAffected = False
            '#End

            Me.AIField1 = AIField.Infatuation
            Me.AIField2 = AIField.Nothing
        End Sub

        Public Overrides Sub MoveHits(own As Boolean, BattleScreen As BattleScreen)
            Dim p As Pokemon = BattleScreen.OwnPokemon
            Dim op As Pokemon = BattleScreen.OppPokemon
            If own = False Then
                p = BattleScreen.OppPokemon
                op = BattleScreen.OwnPokemon
            End If

            If p.Gender <> Pokemon.Genders.Genderless AndAlso op.Gender <> Pokemon.Genders.Genderless AndAlso op.Gender <> p.Gender Then
                If op.Ability.Name.ToLower() <> "aroma veil" Then
                    If BattleScreen.Battle.InflictInfatuate(Not own, own, BattleScreen, "", "move:attract") = False Then
                        BattleScreen.BattleQuery.Add(New TextQueryObject(Me.Name & " failed!"))
                    End If
                Else
                    BattleScreen.BattleQuery.Add(New TextQueryObject("Aroma Veil protected " & op.GetDisplayName() & " from " & Me.Name & "!"))
                End If
            Else
                BattleScreen.BattleQuery.Add(New TextQueryObject(Me.Name & " failed!"))
            End If
        End Sub

        Public Overrides Sub InternalUserPokemonMoveAnimation(ByVal BattleScreen As BattleScreen, ByVal BattleFlip As Boolean, ByVal CurrentPokemon As Pokemon, ByVal CurrentEntity As NPC)
            Dim MoveAnimation = New AnimationQueryObject(CurrentEntity, BattleFlip)
            For i = 0 To 6
                Dim HeartEntity = MoveAnimation.SpawnEntity(Nothing, TextureManager.GetTexture("Textures\Battle\Normal\Attract"), New Vector3(0.25F), 1.0F, CSng(i * 0.2))

                MoveAnimation.AnimationMove(HeartEntity, True, 2.0, 0.0, 0.0, 0.075, False, False, CSng(i * 0.2), 0.0)
                i += 1
            Next
            MoveAnimation.AnimationPlaySound("Battle\Attacks\Normal\Attract", 0, 0)

            BattleScreen.BattleQuery.Add(MoveAnimation)
        End Sub

        Public Overrides Sub InternalOpponentPokemonMoveAnimation(ByVal BattleScreen As BattleScreen, ByVal BattleFlip As Boolean, ByVal CurrentPokemon As Pokemon, ByVal CurrentEntity As NPC)
            Dim MoveAnimation = New AnimationQueryObject(CurrentEntity, BattleFlip)

            For i = 0 To 6
                Dim HeartEntity = MoveAnimation.SpawnEntity(New Vector3(-2.0, 0.0, 0.0), TextureManager.GetTexture("Textures\Battle\Normal\Attract"), New Vector3(0.25F), 1.0F, CSng(i * 0.2))

                MoveAnimation.AnimationMove(HeartEntity, False, 0.0, 0.0, 0.0, 0.06, False, False, CSng(i * 0.2), 0.0)
                Dim zPos As Single = CSng(Random.Next(-2, 2) * 0.2)
                MoveAnimation.AnimationMove(HeartEntity, False, 0.0, 0.25, zPos, 0.01, False, False, CSng(1 + i * 0.2), 0.0)
                MoveAnimation.AnimationFade(HeartEntity, True, 0.02, False, 0.0, CSng(2 + i * 0.2), 0.0)
                i += 1
            Next

            BattleScreen.BattleQuery.Add(MoveAnimation)
        End Sub
    End Class

End Namespace