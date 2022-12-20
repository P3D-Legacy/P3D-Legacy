Namespace BattleSystem.Moves.Normal

    Public Class Bind

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Normal)
            Me.ID = 20
            Me.OriginalPP = 20
            Me.CurrentPP = 20
            Me.MaxPP = 20
            Me.Power = 15
            Me.Accuracy = 85
            Me.Category = Categories.Physical
            Me.ContestCategory = ContestCategories.Tough
            Me.Name = Localization.GetString("move_name_" & Me.ID,"Bind")
            Me.Description = "Things such as long bodies or tentacles are used to bind and squeeze the target for four to five turns."
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
            Me.HasSecondaryEffect = False
            Me.RemovesFrozen = False

            Me.IsHealingMove = False
            Me.IsRecoilMove = False

            Me.IsDamagingMove = True
            Me.IsProtectMove = False


            Me.IsAffectedBySubstitute = True
            Me.IsOneHitKOMove = False
            Me.IsWonderGuardAffected = True
            '#End

            Me.AIField1 = AIField.Damage
            Me.AIField2 = AIField.Trap
        End Sub

        Public Overrides Sub MoveHits(own As Boolean, BattleScreen As BattleScreen)
            Dim p As Pokemon = BattleScreen.OwnPokemon
            Dim op As Pokemon = BattleScreen.OppPokemon
            If own = False Then
                p = BattleScreen.OppPokemon
                op = BattleScreen.OwnPokemon
            End If

            Dim turns As Integer = 4
            If Core.Random.Next(0, 100) < 50 Then
                turns = 5
            End If

            If Not p.Item Is Nothing Then
                If p.Item.Name.ToLower() = "grip claw" And BattleScreen.FieldEffects.CanUseItem(own) = True And BattleScreen.FieldEffects.CanUseOwnItem(own, BattleScreen) = True Then
                    turns = 5
                End If
            End If

            If own = True Then
                If BattleScreen.FieldEffects.OppBind = 0 Then
                    BattleScreen.FieldEffects.OppBind = turns
                    BattleScreen.BattleQuery.Add(New TextQueryObject(p.GetDisplayName() & " used Bind on " & op.GetDisplayName() & "!"))
                End If
            Else
                If BattleScreen.FieldEffects.OwnBind = 0 Then
                    BattleScreen.FieldEffects.OwnBind = turns
                    BattleScreen.BattleQuery.Add(New TextQueryObject(p.GetDisplayName() & " used Bind on " & op.GetDisplayName() & "!"))
                End If
            End If
        End Sub

        Public Overrides Sub InternalOpponentPokemonMoveAnimation(ByVal BattleScreen As BattleScreen, ByVal BattleFlip As Boolean, ByVal CurrentPokemon As Pokemon, ByVal CurrentEntity As NPC)
            Dim MoveAnimation As AnimationQueryObject = New AnimationQueryObject(CurrentEntity, BattleFlip)
            MoveAnimation.AnimationPlaySound("Battle\Attacks\Normal\Bind", 5.0F, 0)
            Dim BindEntity = MoveAnimation.SpawnEntity(New Vector3(0, -0.2, 0), TextureManager.GetTexture("Textures\Battle\Normal\Bind", New Rectangle(0, 0, 80, 40), ""), New Vector3(1.0F, 0.5F, 1.0F), 1, 0, 0.75)
            MoveAnimation.AnimationChangeTexture(BindEntity, False, TextureManager.GetTexture("Textures\Battle\Normal\Bind", New Rectangle(0, 40, 80, 40), ""), 0.75, 0.75)
            MoveAnimation.AnimationChangeTexture(BindEntity, False, TextureManager.GetTexture("Textures\Battle\Normal\Bind", New Rectangle(0, 80, 80, 40), ""), 1.5, 0.75)
            MoveAnimation.AnimationChangeTexture(BindEntity, False, TextureManager.GetTexture("Textures\Battle\Normal\Bind", New Rectangle(0, 120, 80, 40), ""), 2.25, 0.75)
            MoveAnimation.AnimationChangeTexture(BindEntity, False, TextureManager.GetTexture("Textures\Battle\Normal\Bind", New Rectangle(0, 160, 80, 40), ""), 3, 0.75)
            MoveAnimation.AnimationChangeTexture(BindEntity, False, TextureManager.GetTexture("Textures\Battle\Normal\Bind", New Rectangle(0, 200, 80, 40), ""), 3.75, 0.75)

            MoveAnimation.AnimationScale(Nothing, False, False, 0.75F, 1.0F, 0.75F, 0.02F, 5, 0)
            MoveAnimation.AnimationScale(BindEntity, False, False, 0.75F, 0.5F, 0.75F, 0.02F, 5, 0)
            MoveAnimation.AnimationScale(Nothing, False, True, 1.0F, 1.0F, 1.0F, 0.04F, 7, 0)
            MoveAnimation.AnimationScale(BindEntity, False, True, 1.0F, 0.5F, 1.0F, 0.04F, 7, 0)
            MoveAnimation.AnimationScale(Nothing, False, False, 0.75F, 1.0F, 0.75F, 0.02F, 9, 0)
            MoveAnimation.AnimationScale(BindEntity, False, False, 0.75F, 0.5F, 0.75F, 0.02F, 9, 0)
            MoveAnimation.AnimationScale(Nothing, False, True, 1.0F, 1.0F, 1.0F, 0.04F, 11, 0)
            MoveAnimation.AnimationScale(BindEntity, False, True, 1.0F, 0.5F, 1.0F, 0.04F, 11, 0)
            MoveAnimation.AnimationFade(BindEntity, True, 0.03, False, 0.0, 11, 0)

            BattleScreen.BattleQuery.Add(MoveAnimation)
        End Sub
    End Class

End Namespace