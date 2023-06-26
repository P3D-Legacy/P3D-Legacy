Namespace BattleSystem.Moves.Ghost

    Public Class ConfuseRay

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Ghost)
            Me.ID = 109
            Me.OriginalPP = 10
            Me.CurrentPP = 10
            Me.MaxPP = 10
            Me.Power = 0
            Me.Accuracy = 100
            Me.Category = Categories.Status
            Me.ContestCategory = ContestCategories.Smart
            Me.Name = Localization.GetString("move_name_" & Me.ID,"Confuse Ray")
            Me.Description = "The target is exposed to a sinister ray that triggers confusion."
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
            Me.MirrorMoveAffected = False
            Me.KingsrockAffected = False
            Me.CounterAffected = False

            Me.DisabledWhileGravity = False
            Me.UseEffectiveness = False
            Me.ImmunityAffected = False
            Me.HasSecondaryEffect = False
            Me.RemovesOwnFrozen = False

            Me.IsHealingMove = False
            Me.IsRecoilMove = False

            Me.IsDamagingMove = False
            Me.IsProtectMove = False


            Me.IsAffectedBySubstitute = True
            Me.IsOneHitKOMove = False
            Me.IsWonderGuardAffected = True
            '#End

            Me.AIField1 = AIField.Confusion
            Me.AIField2 = AIField.Nothing
        End Sub

        Public Overrides Sub MoveHits(own As Boolean, BattleScreen As BattleScreen)
            If BattleScreen.Battle.InflictConfusion(Not own, own, BattleScreen, "", "move:confuseray") = False Then
                BattleScreen.BattleQuery.Add(New TextQueryObject(Me.Name & " failed!"))
            End If
        End Sub

        Public Overrides Sub InternalUserPokemonMoveAnimation(ByVal BattleScreen As BattleScreen, ByVal BattleFlip As Boolean, ByVal CurrentPokemon As Pokemon, ByVal CurrentEntity As NPC)
            Dim MoveAnimation = New AnimationQueryObject(Nothing, BattleFlip)

            MoveAnimation.AnimationPlaySound("Battle\Attacks\Ghost\ConfuseRay_Start", 0.0F, 0)

            Dim RayEntity = MoveAnimation.SpawnEntity(CurrentEntity.Position, TextureManager.GetTexture("Textures\Battle\Ghost\ConfuseRay", New Rectangle(0, 0, 16, 16), ""), New Vector3(0.5F), 0.0F)
            MoveAnimation.AnimationFade(RayEntity, False, 0.025F, True, 1.0F, 0, 0)
            MoveAnimation.AnimationMove(RayEntity, False, 1.5, 0, 0, 0.025, False, False, 0, 0,,, 0.0125)
            MoveAnimation.AnimationOscillateMove(RayEntity, True, New Vector3(0, 0.075, 0), 0.02, True, 1, 0, 0, 1)

            MoveAnimation.AnimationChangeTexture(RayEntity, False, TextureManager.GetTexture("Textures\Battle\Ghost\ConfuseRay", New Rectangle(16, 0, 16, 16), ""), 0.5, 0)
            MoveAnimation.AnimationChangeTexture(RayEntity, False, TextureManager.GetTexture("Textures\Battle\Ghost\ConfuseRay", New Rectangle(0, 0, 16, 16), ""), 1.0, 0)
            MoveAnimation.AnimationChangeTexture(RayEntity, False, TextureManager.GetTexture("Textures\Battle\Ghost\ConfuseRay", New Rectangle(16, 0, 16, 16), ""), 1.5, 0)
            MoveAnimation.AnimationChangeTexture(RayEntity, False, TextureManager.GetTexture("Textures\Battle\Ghost\ConfuseRay", New Rectangle(0, 0, 16, 16), ""), 2.0, 0)
            MoveAnimation.AnimationChangeTexture(RayEntity, False, TextureManager.GetTexture("Textures\Battle\Ghost\ConfuseRay", New Rectangle(16, 0, 16, 16), ""), 2.5, 0)
            MoveAnimation.AnimationChangeTexture(RayEntity, False, TextureManager.GetTexture("Textures\Battle\Ghost\ConfuseRay", New Rectangle(0, 0, 16, 16), ""), 3, 0)
            MoveAnimation.AnimationChangeTexture(RayEntity, False, TextureManager.GetTexture("Textures\Battle\Ghost\ConfuseRay", New Rectangle(16, 0, 16, 16), ""), 3.5, 0)
            MoveAnimation.AnimationChangeTexture(RayEntity, False, TextureManager.GetTexture("Textures\Battle\Ghost\ConfuseRay", New Rectangle(0, 0, 16, 16), ""), 4, 0)

            BattleScreen.BattleQuery.Add(MoveAnimation)
        End Sub
        Public Overrides Sub InternalOpponentPokemonMoveAnimation(ByVal BattleScreen As BattleScreen, ByVal BattleFlip As Boolean, ByVal CurrentPokemon As Pokemon, ByVal CurrentEntity As NPC)
            Dim MoveAnimation = New AnimationQueryObject(Nothing, BattleFlip)
            Dim SpawnPosition As Vector3 = CurrentEntity.Position
            SpawnPosition.X = CurrentEntity.Position.X - 1.5F
            If CurrentEntity.Model IsNot Nothing Then
                SpawnPosition.Y = CurrentEntity.Position.Y - 0.5F
            End If
            MoveAnimation.AnimationPlaySound("Battle\Attacks\Ghost\ConfuseRay_End", 0.0F, 0)

            Dim RayEntity = MoveAnimation.SpawnEntity(SpawnPosition, TextureManager.GetTexture("Textures\Battle\Ghost\ConfuseRay", New Rectangle(0, 0, 16, 16), ""), New Vector3(0.5F), 1.0F)
            MoveAnimation.AnimationMove(RayEntity, False, 1.5, 0, 0, 0.025, False, False, 0, 0,,, 0.0125)
            MoveAnimation.AnimationOscillateMove(RayEntity, False, New Vector3(0, 0.075, 0), 0.02, True, 1, 0, 0, 1)
            MoveAnimation.AnimationOscillateMove(RayEntity, False, New Vector3(0, 0, 0.2), 0.075, True, 1.5, 4, 0, 1)

            MoveAnimation.AnimationChangeTexture(RayEntity, False, TextureManager.GetTexture("Textures\Battle\Ghost\ConfuseRay", New Rectangle(16, 0, 16, 16), ""), 0.5, 0)
            MoveAnimation.AnimationChangeTexture(RayEntity, False, TextureManager.GetTexture("Textures\Battle\Ghost\ConfuseRay", New Rectangle(0, 0, 16, 16), ""), 1.0, 0)
            MoveAnimation.AnimationChangeTexture(RayEntity, False, TextureManager.GetTexture("Textures\Battle\Ghost\ConfuseRay", New Rectangle(16, 0, 16, 16), ""), 1.5, 0)
            MoveAnimation.AnimationChangeTexture(RayEntity, False, TextureManager.GetTexture("Textures\Battle\Ghost\ConfuseRay", New Rectangle(0, 0, 16, 16), ""), 2.0, 0)
            MoveAnimation.AnimationChangeTexture(RayEntity, False, TextureManager.GetTexture("Textures\Battle\Ghost\ConfuseRay", New Rectangle(16, 0, 16, 16), ""), 2.5, 0)
            MoveAnimation.AnimationChangeTexture(RayEntity, False, TextureManager.GetTexture("Textures\Battle\Ghost\ConfuseRay", New Rectangle(0, 0, 16, 16), ""), 3, 0)
            MoveAnimation.AnimationChangeTexture(RayEntity, False, TextureManager.GetTexture("Textures\Battle\Ghost\ConfuseRay", New Rectangle(16, 0, 16, 16), ""), 3.5, 0)
            MoveAnimation.AnimationChangeTexture(RayEntity, False, TextureManager.GetTexture("Textures\Battle\Ghost\ConfuseRay", New Rectangle(0, 0, 16, 16), ""), 4, 0)
            MoveAnimation.AnimationChangeTexture(RayEntity, False, TextureManager.GetTexture("Textures\Battle\Ghost\ConfuseRay", New Rectangle(16, 0, 16, 16), ""), 4.5, 0)
            MoveAnimation.AnimationChangeTexture(RayEntity, False, TextureManager.GetTexture("Textures\Battle\Ghost\ConfuseRay", New Rectangle(0, 0, 16, 16), ""), 5.0, 0)
            MoveAnimation.AnimationChangeTexture(RayEntity, False, TextureManager.GetTexture("Textures\Battle\Ghost\ConfuseRay", New Rectangle(16, 0, 16, 16), ""), 5.5, 0)
            MoveAnimation.AnimationChangeTexture(RayEntity, False, TextureManager.GetTexture("Textures\Battle\Ghost\ConfuseRay", New Rectangle(0, 0, 16, 16), ""), 6.0, 0)
            MoveAnimation.AnimationChangeTexture(RayEntity, False, TextureManager.GetTexture("Textures\Battle\Ghost\ConfuseRay", New Rectangle(16, 0, 16, 16), ""), 6.5, 0)
            MoveAnimation.AnimationChangeTexture(RayEntity, False, TextureManager.GetTexture("Textures\Battle\Ghost\ConfuseRay", New Rectangle(0, 0, 16, 16), ""), 7, 0)

            MoveAnimation.AnimationFade(RayEntity, True, 0.035, False, 0, 6.75, 0)
            BattleScreen.BattleQuery.Add(MoveAnimation)
        End Sub
    End Class

End Namespace
