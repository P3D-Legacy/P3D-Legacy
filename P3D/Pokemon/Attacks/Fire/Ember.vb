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
            Me.Name = "Ember"
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
            Me.IsPunchingMove = False
            Me.IsDamagingMove = True
            Me.IsProtectMove = False
            Me.IsSoundMove = False

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

        Public Overrides Sub InternalUserPokemonMoveAnimation(ByVal BattleScreen As BattleScreen, ByVal own As Boolean, ByVal CurrentPokemon As Pokemon, ByVal CurrentEntity As NPC, ByVal CurrentModel As ModelEntity)
            Dim MoveAnimation As AnimationQueryObject = New AnimationQueryObject(CurrentEntity, own)
            MoveAnimation.AnimationSpawnMovingEntity(0.0, 0, 0.0, "Textures\Battle\Fire\FireBall", 0.5, 0.5, 0.5, 2.0, 0.0, 0.0, 0.05, False, True, 0.0, 0.0,, -0.5, 0)
            MoveAnimation.AnimationPlaySound("Battle\Attacks\Ember_Start", 0, 0)
            For i = 0 To 12
                MoveAnimation.AnimationSpawnFadingEntity(CSng(i * 0.2), 0.0, 0.0, "Textures\Battle\Fire\Smoke", 0.2, 0.2, 0.2, 0.02, False, 0.0, CSng(i * 0.2), 0.0)
                i += 1
            Next
            BattleScreen.BattleQuery.Add(MoveAnimation)
        End Sub

        Public Overrides Sub InternalOpponentPokemonMoveAnimation(ByVal BattleScreen As BattleScreen, ByVal own As Boolean, ByVal CurrentPokemon As Pokemon, ByVal CurrentEntity As NPC, ByVal CurrentModel As ModelEntity)
            Dim MoveAnimation As AnimationQueryObject = New AnimationQueryObject(CurrentEntity, own)

            MoveAnimation.AnimationSpawnMovingEntity(2.0, 0, 0.0, "Textures\Battle\Fire\FireBall", 0.5, 0.5, 0.5, 0.0, 0.0, 0.0, 0.05, False, True, 0.0, 0.0, 0.1, 0.5, 0)
            For i = 0 To 12
                MoveAnimation.AnimationSpawnFadingEntity(CSng(3.0 - i * 0.2), 0.0, 0.0, "Textures\Battle\Fire\Smoke", 0.2, 0.2, 0.2, 0.02, False, 0.0, CSng(i * 0.2), 0.0)
                i += 1
            Next
            MoveAnimation.AnimationPlaySound("Battle\Attacks\Ember_Hit", 2, 0)
            MoveAnimation.AnimationSpawnFadingEntity(-0.25, -0.25, -0.25, "Textures\Battle\Fire\Ember,0,0,32,32", 0.5, 0.5, 0.5, 0.02, False, 1.0, 1, 1)
            MoveAnimation.AnimationSpawnFadingEntity(0.25, -0.25, 0.25, "Textures\Battle\Fire\Ember,0,0,32,32", 0.5, 0.5, 0.5, 0.02, False, 1.0, 1, 1)
            MoveAnimation.AnimationSpawnFadingEntity(0.25, -0.25, -0.25, "Textures\Battle\Fire\Ember,0,0,32,32", 0.5, 0.5, 0.5, 0.02, False, 1.0, 1, 1)

            MoveAnimation.AnimationSpawnFadingEntity(-0.25, -0.25, -0.25, "Textures\Battle\Fire\Ember,0,32,32,32", 0.5, 0.5, 0.5, 0.02, False, 1.0, 2, 1)
            MoveAnimation.AnimationSpawnFadingEntity(0.25, -0.25, 0.25, "Textures\Battle\Fire\Ember,0,32,32,32", 0.5, 0.5, 0.5, 0.02, False, 1.0, 2, 1)
            MoveAnimation.AnimationSpawnFadingEntity(0.25, -0.25, -0.25, "Textures\Battle\Fire\Ember,0,32,32,32", 0.5, 0.5, 0.5, 0.02, False, 1.0, 2, 1)

            MoveAnimation.AnimationSpawnFadingEntity(-0.25, -0.25, -0.25, "Textures\Battle\Fire\Ember,0,64,32,32", 0.5, 0.5, 0.5, 0.02, False, 1.0, 3, 1)
            MoveAnimation.AnimationSpawnFadingEntity(0.25, -0.25, 0.25, "Textures\Battle\Fire\Ember,0,64,32,32", 0.5, 0.5, 0.5, 0.02, False, 1.0, 3, 1)
            MoveAnimation.AnimationSpawnFadingEntity(0.25, -0.25, -0.25, "Textures\Battle\Fire\Ember,0,64,32,32", 0.5, 0.5, 0.5, 0.02, False, 1.0, 3, 1)

            MoveAnimation.AnimationSpawnFadingEntity(-0.25, -0.25, -0.25, "Textures\Battle\Fire\Ember,0,96,32,32", 0.5, 0.5, 0.5, 0.02, False, 1.0, 4, 1)
            MoveAnimation.AnimationSpawnFadingEntity(0.25, -0.25, 0.25, "Textures\Battle\Fire\Ember,0,96,32,32", 0.5, 0.5, 0.5, 0.02, False, 1.0, 4, 1)
            MoveAnimation.AnimationSpawnFadingEntity(0.25, -0.25, -0.25, "Textures\Battle\Fire\Ember,0,96,32,32", 0.5, 0.5, 0.5, 0.02, False, 1.0, 4, 1)

            MoveAnimation.AnimationSpawnFadingEntity(-0.25, -0.25, -0.25, "Textures\Battle\Fire\Ember,0,128,32,32", 0.5, 0.5, 0.5, 0.02, False, 1.0, 5, 1)
            MoveAnimation.AnimationSpawnFadingEntity(0.25, -0.25, 0.25, "Textures\Battle\Fire\Ember,0,128,32,32", 0.5, 0.5, 0.5, 0.02, False, 1.0, 5, 1)
            MoveAnimation.AnimationSpawnFadingEntity(0.25, -0.25, -0.25, "Textures\Battle\Fire\Ember,0,128,32,32", 0.5, 0.5, 0.5, 0.02, False, 1.0, 5, 1)

            BattleScreen.BattleQuery.Add(MoveAnimation)
        End Sub
    End Class

End Namespace