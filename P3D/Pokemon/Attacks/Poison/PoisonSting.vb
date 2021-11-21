Namespace BattleSystem.Moves.Poison

    Public Class PoisonSting

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Poison)
            Me.ID = 40
            Me.OriginalPP = 35
            Me.CurrentPP = 35
            Me.MaxPP = 35
            Me.Power = 15
            Me.Accuracy = 100
            Me.Category = Categories.Physical
            Me.ContestCategory = ContestCategories.Smart
            Me.Name = "Poison Sting"
            Me.Description = "The user stabs the target with a poisonous stinger. This may also poison the target."
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
            Me.AIField2 = AIField.CanPoison

            Me.EffectChances.Add(30)
        End Sub

        Public Overrides Sub MoveHits(own As Boolean, BattleScreen As BattleScreen)
            Dim chance As Integer = GetEffectChance(0, own, BattleScreen)

            If Core.Random.Next(0, 100) < chance Then
                BattleScreen.Battle.InflictPoison(Not own, own, BattleScreen, False, "", "move:poisonsting")
            End If
        End Sub

        Public Overrides Sub InternalUserPokemonMoveAnimation(ByVal BattleScreen As BattleScreen, ByVal BattleFlip As Boolean, ByVal CurrentPokemon As Pokemon, ByVal CurrentEntity As NPC, ByVal CurrentModel As ModelEntity)
            Dim MoveAnimation As AnimationQueryObject = New AnimationQueryObject(CurrentEntity, BattleFlip)
            Dim StingerEntity As Entity = MoveAnimation.SpawnEntity(Nothing, TextureManager.GetTexture("Textures\Battle\Poison\Stinger"), New Vector3(0.5F), 1.0F)

            MoveAnimation.AnimationPlaySound("Battle\Attacks\Poison\PoisonSting_Start", 0, 0)
            MoveAnimation.AnimationMove(StingerEntity, True, 2.0, 0.0, 0.0, 0.05, False, False, 0.0, 0.0,,, 0)

            BattleScreen.BattleQuery.Add(MoveAnimation)
        End Sub

        Public Overrides Sub InternalOpponentPokemonMoveAnimation(ByVal BattleScreen As BattleScreen, ByVal BattleFlip As Boolean, ByVal CurrentPokemon As Pokemon, ByVal CurrentEntity As NPC, ByVal CurrentModel As ModelEntity)
            Dim MoveAnimation As AnimationQueryObject = New AnimationQueryObject(CurrentEntity, BattleFlip)

            Dim StingerEntity As Entity = MoveAnimation.SpawnEntity(New Vector3(-2.0, 0, 0.0), TextureManager.GetTexture("Textures\Battle\Poison\Stinger"), New Vector3(0.5F), 1)

            MoveAnimation.AnimationMove(StingerEntity, True, 0.0, 0.0, 0.0, 0.05, False, False, 0.0, 0.0,,, 0)

            MoveAnimation.AnimationPlaySound("Battle\Attacks\Poison\PoisonSting_Hit", 1, 0)

            Dim BubbleEntity1 As Entity = MoveAnimation.SpawnEntity(New Vector3(-0.25, -0.25, -0.25), TextureManager.GetTexture("Textures\Battle\Poison\Bubble", New Rectangle(0, 0, 32, 32), ""), New Vector3(0.5F), 1, 1, 1)

            MoveAnimation.AnimationChangeTexture(BubbleEntity1, False, TextureManager.GetTexture("Textures\Battle\Poison\Bubble", New Rectangle(0, 32, 32, 32), ""), 2, 1)

            Dim BubbleEntity2 As Entity = MoveAnimation.SpawnEntity(New Vector3(0.25, -0.25, 0.25), TextureManager.GetTexture("Textures\Battle\Poison\Bubble", New Rectangle(0, 0, 32, 32), ""), New Vector3(0.5F), 1, 2, 1)

            MoveAnimation.AnimationChangeTexture(BubbleEntity1, True, TextureManager.GetTexture("Textures\Battle\Poison\Bubble", New Rectangle(0, 64, 32, 32), ""), 3, 1)
            MoveAnimation.AnimationChangeTexture(BubbleEntity2, False, TextureManager.GetTexture("Textures\Battle\Poison\Bubble", New Rectangle(0, 32, 32, 32), ""), 3, 1)

            Dim BubbleEntity3 As Entity = MoveAnimation.SpawnEntity(New Vector3(0.25, -0.25, -0.25), TextureManager.GetTexture("Textures\Battle\Poison\Bubble", New Rectangle(0, 0, 32, 32), ""), New Vector3(0.5F), 1, 3, 1)

            MoveAnimation.AnimationChangeTexture(BubbleEntity2, True, TextureManager.GetTexture("Textures\Battle\Poison\Bubble", New Rectangle(0, 64, 32, 32), ""), 4, 1)
            MoveAnimation.AnimationChangeTexture(BubbleEntity3, False, TextureManager.GetTexture("Textures\Battle\Poison\Bubble", New Rectangle(0, 32, 32, 32), ""), 4, 1)

            MoveAnimation.AnimationChangeTexture(BubbleEntity3, True, TextureManager.GetTexture("Textures\Battle\Poison\Bubble", New Rectangle(0, 64, 32, 32), ""), 5, 1)

            BattleScreen.BattleQuery.Add(MoveAnimation)
        End Sub

    End Class

End Namespace