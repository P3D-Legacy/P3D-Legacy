Namespace BattleSystem.Moves.Fire

    Public Class Ember

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Fire)
            Me.ID = 52
            Me.OriginalPP = 25
            Me.CurrentPP = 25
            Me.MaxPP = 25
            Me.Power = 40
            Me.Accuracy = 100
            Me.Category = Categories.Special
            Me.ContestCategory = ContestCategories.Cute
            Me.Name = Localization.GetString("move_name_" & Me.ID,"Ember")
            Me.Description = "The target is attacked with small flames. It may also leave the target with a burn."
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
            Me.HasSecondaryEffect = True
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
            Me.AIField2 = AIField.CanBurn

            EffectChances.Add(10)
        End Sub

        Public Overrides Sub MoveHits(own As Boolean, BattleScreen As BattleScreen)
            Dim chance As Integer = GetEffectChance(0, own, BattleScreen)
            If Core.Random.Next(0, 100) < chance Then
                BattleScreen.Battle.InflictBurn(Not own, own, BattleScreen, "", "move:ember")
            End If
        End Sub

        Public Overrides Sub InternalUserPokemonMoveAnimation(ByVal BattleScreen As BattleScreen, ByVal BattleFlip As Boolean, ByVal CurrentPokemon As Pokemon, ByVal CurrentEntity As NPC)
            Dim MoveAnimation = New AnimationQueryObject(CurrentEntity, BattleFlip)
            Dim TextureYOffset As Integer = 0
            If BattleFlip = True Then
                TextureYOffset = 32
            End If
            MoveAnimation.AnimationPlaySound("Battle\Attacks\Fire\Ember_Start", 0, 0)
            Dim FireballEntity = MoveAnimation.SpawnEntity(New Vector3(0), TextureManager.GetTexture("Textures\Battle\Fire\FireBall", New Rectangle(0, TextureYOffset, 32, 32), ""), New Vector3(0.5F), 1.0F)
            If BattleFlip = False Then
                MoveAnimation.AnimationMove(FireballEntity, True, 2.0, 0.0, 0.0, 0.05, False, True, 0.0, 0.0,, -0.5)
            Else
                MoveAnimation.AnimationMove(FireballEntity, True, 2.0, 0.0, 0.0, 0.05, False, True, 0.0, 0.0,, 0.5)
            End If

            BattleScreen.BattleQuery.Add(MoveAnimation)
        End Sub

        Public Overrides Sub InternalOpponentPokemonMoveAnimation(ByVal BattleScreen As BattleScreen, ByVal BattleFlip As Boolean, ByVal CurrentPokemon As Pokemon, ByVal CurrentEntity As NPC)
            Dim MoveAnimation = New AnimationQueryObject(CurrentEntity, BattleFlip)
            Dim TextureYOffset As Integer = 0
            If BattleFlip = True Then
                TextureYOffset = 32
            End If
            Dim FireballEntity = MoveAnimation.SpawnEntity(New Vector3(-2.0, 0.0, 0.0), TextureManager.GetTexture("Textures\Battle\Fire\FireBall", New Rectangle(0, TextureYOffset, 32, 32), ""), New Vector3(0.5F), 1.0F)
            If BattleFlip = False Then
                MoveAnimation.AnimationMove(FireballEntity, True, -0.05, 0.0, 0.0, 0.05, False, True, 0.0, 1.0,, -0.5)
            Else
                MoveAnimation.AnimationMove(FireballEntity, True, -0.05, 0.0, 0.0, 0.05, False, True, 0.0, 1.0,, 0.5)
            End If

            MoveAnimation.AnimationPlaySound("Battle\Attacks\Fire\Ember_Hit", 4, 0)

            Dim FireEntity1 As Entity = MoveAnimation.SpawnEntity(New Vector3(-0.25, -0.25, -0.25), TextureManager.GetTexture("Textures\Battle\Fire\Ember", New Rectangle(0, 0, 32, 32), ""), New Vector3(0.5F), 1, 3, 0)
            Dim FireEntity2 As Entity = MoveAnimation.SpawnEntity(New Vector3(0, -0.25, 0), TextureManager.GetTexture("Textures\Battle\Fire\Ember", New Rectangle(0, 0, 32, 32), ""), New Vector3(0.5F), 1, 3, 0)
            Dim FireEntity3 As Entity = MoveAnimation.SpawnEntity(New Vector3(0.25, -0.25, 0.25), TextureManager.GetTexture("Textures\Battle\Fire\Ember", New Rectangle(0, 0, 32, 32), ""), New Vector3(0.5F), 1, 3, 0)

            MoveAnimation.AnimationChangeTexture(FireEntity1, False, TextureManager.GetTexture("Textures\Battle\Fire\Ember", New Rectangle(0, 32, 32, 32), ""), 3.75, 0)
            MoveAnimation.AnimationChangeTexture(FireEntity2, False, TextureManager.GetTexture("Textures\Battle\Fire\Ember", New Rectangle(0, 32, 32, 32), ""), 3.75, 0)
            MoveAnimation.AnimationChangeTexture(FireEntity3, False, TextureManager.GetTexture("Textures\Battle\Fire\Ember", New Rectangle(0, 32, 32, 32), ""), 3.75, 0)

            MoveAnimation.AnimationChangeTexture(FireEntity1, False, TextureManager.GetTexture("Textures\Battle\Fire\Ember", New Rectangle(0, 64, 32, 32), ""), 4.5, 0)
            MoveAnimation.AnimationChangeTexture(FireEntity2, False, TextureManager.GetTexture("Textures\Battle\Fire\Ember", New Rectangle(0, 64, 32, 32), ""), 4.5, 0)
            MoveAnimation.AnimationChangeTexture(FireEntity3, False, TextureManager.GetTexture("Textures\Battle\Fire\Ember", New Rectangle(0, 64, 32, 32), ""), 4.5, 0)

            MoveAnimation.AnimationChangeTexture(FireEntity1, False, TextureManager.GetTexture("Textures\Battle\Fire\Ember", New Rectangle(0, 96, 32, 32), ""), 5.25, 0)
            MoveAnimation.AnimationChangeTexture(FireEntity2, False, TextureManager.GetTexture("Textures\Battle\Fire\Ember", New Rectangle(0, 96, 32, 32), ""), 5.25, 0)
            MoveAnimation.AnimationChangeTexture(FireEntity3, False, TextureManager.GetTexture("Textures\Battle\Fire\Ember", New Rectangle(0, 96, 32, 32), ""), 5.25, 0)

            MoveAnimation.AnimationChangeTexture(FireEntity1, True, TextureManager.GetTexture("Textures\Battle\Fire\Ember", New Rectangle(0, 128, 32, 32), ""), 6, 0)
            MoveAnimation.AnimationChangeTexture(FireEntity2, True, TextureManager.GetTexture("Textures\Battle\Fire\Ember", New Rectangle(0, 128, 32, 32), ""), 6, 0)
            MoveAnimation.AnimationChangeTexture(FireEntity3, True, TextureManager.GetTexture("Textures\Battle\Fire\Ember", New Rectangle(0, 128, 32, 32), ""), 6, 0)

            BattleScreen.BattleQuery.Add(MoveAnimation)
        End Sub
    End Class

End Namespace