Namespace BattleSystem.Moves.Water

    Public Class WaterGun

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Water)
            Me.ID = 55
            Me.OriginalPP = 25
            Me.CurrentPP = 25
            Me.MaxPP = 25
            Me.Power = 40
            Me.Accuracy = 100
            Me.Category = Categories.Special
            Me.ContestCategory = ContestCategories.Cute
            Me.Name = Localization.GetString("move_name_" & Me.ID,"Water Gun")
            Me.Description = "The target is blasted with a forceful shot of water."
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
            Me.RemovesOwnFrozen = False

            Me.IsHealingMove = False
            Me.IsRecoilMove = False

            Me.IsDamagingMove = True
            Me.IsProtectMove = False


            Me.IsAffectedBySubstitute = True
            Me.IsOneHitKOMove = False
            Me.IsWonderGuardAffected = True
            '#End
        End Sub

        Public Overrides Sub InternalUserPokemonMoveAnimation(ByVal BattleScreen As BattleScreen, ByVal BattleFlip As Boolean, ByVal CurrentPokemon As Pokemon, ByVal CurrentEntity As NPC)
            Dim MoveAnimation = New AnimationQueryObject(CurrentEntity, BattleFlip)
            Dim WaterEntity = MoveAnimation.SpawnEntity(New Vector3(0), TextureManager.GetTexture("Textures\Battle\Water\WaterGun", New Rectangle(0, 0, 16, 16), ""), New Vector3(0.5F), 0.75F)
            MoveAnimation.AnimationMove(WaterEntity, True, 2, 0.5, 0, 0.075, False, False, 0, 0,,,, 0.05)
            MoveAnimation.AnimationPlaySound("Battle\Attacks\Water\Watergun_Start", 0, 0)
            BattleScreen.BattleQuery.Add(MoveAnimation)
        End Sub
        Public Overrides Sub InternalOpponentPokemonMoveAnimation(ByVal BattleScreen As BattleScreen, ByVal BattleFlip As Boolean, ByVal CurrentPokemon As Pokemon, ByVal CurrentEntity As NPC)
            Dim MoveAnimation = New AnimationQueryObject(CurrentEntity, BattleFlip)
            Dim WaterEntity = MoveAnimation.SpawnEntity(New Vector3(-2, 1, 0), TextureManager.GetTexture("Textures\Battle\Water\WaterGun", New Rectangle(0, 0, 16, 16), ""), New Vector3(0.5F), 0.5F)
            MoveAnimation.AnimationMove(WaterEntity, True, 0, 0, 0, 0.075, False, False, 0, 0,,,, 0.035)

            MoveAnimation.AnimationPlaySound("Battle\Attacks\Water\Watergun_Hit", 3, 0)
            Dim HitEntity = MoveAnimation.SpawnEntity(New Vector3(0, -0.2, 0), TextureManager.GetTexture("Textures\Battle\Water\WaterGun", New Rectangle(0, 16, 16, 16), ""), New Vector3(0.5F), 0.5F, 3, 1)
            MoveAnimation.AnimationFade(HitEntity, True, 1.0F, False, 0.0F, 5, 0)

            Dim WaterDrop1Position As Vector3 = New Vector3(-0.25, 0.25, -0.25)
            Dim WaterDrop2Position As Vector3 = New Vector3(0, 0.25, 0)
            Dim WaterDrop3Position As Vector3 = New Vector3(0.25, 0.25, 0.25)

            Dim WaterDropEntity1 As Entity = MoveAnimation.SpawnEntity(WaterDrop1Position, TextureManager.GetTexture("Textures\Battle\Water\WaterGun", New Rectangle(0, 32, 16, 16), ""), New Vector3(0.5F), 0.75F, 5, 0)
            Dim WaterDropEntity2 As Entity = MoveAnimation.SpawnEntity(WaterDrop2Position, TextureManager.GetTexture("Textures\Battle\Water\WaterGun", New Rectangle(0, 32, 16, 16), ""), New Vector3(0.5F), 0.75F, 5, 0)
            Dim WaterDropEntity3 As Entity = MoveAnimation.SpawnEntity(WaterDrop3Position, TextureManager.GetTexture("Textures\Battle\Water\WaterGun", New Rectangle(0, 32, 16, 16), ""), New Vector3(0.5F), 0.75F, 5, 0)

            MoveAnimation.AnimationMove(WaterDropEntity1, True, WaterDrop1Position.X, -0.25, WaterDrop1Position.Z, 0.05F, False, False, 5, 0)
            MoveAnimation.AnimationMove(WaterDropEntity2, True, WaterDrop2Position.X, -0.25, WaterDrop2Position.Z, 0.05F, False, False, 5, 0)
            MoveAnimation.AnimationMove(WaterDropEntity3, True, WaterDrop3Position.X, -0.25, WaterDrop3Position.Z, 0.05F, False, False, 5, 0)

            BattleScreen.BattleQuery.Add(MoveAnimation)
        End Sub
    End Class

End Namespace