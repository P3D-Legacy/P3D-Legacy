Namespace BattleSystem.Moves.Normal

    Public Class Tackle

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Normal)
            Me.ID = 33
            Me.OriginalPP = 35
            Me.CurrentPP = 35
            Me.MaxPP = 35
            Me.Power = 40
            Me.Accuracy = 100
            Me.Category = Categories.Physical
            Me.ContestCategory = ContestCategories.Tough
            Me.Name = "Tackle"
            Me.Description = "A physical attack in which the user charges and slams into the target with its whole body."
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
            Me.IsPunchingMove = False
            Me.IsDamagingMove = True
            Me.IsProtectMove = False
            Me.IsSoundMove = False

            Me.IsAffectedBySubstitute = True
            Me.IsOneHitKOMove = False
            Me.IsWonderGuardAffected = True
            '#End
        End Sub

        Public Overrides Sub InternalUserPokemonMoveAnimation(ByVal BattleScreen As BattleScreen, ByVal BattleFlip As Boolean, ByVal CurrentPokemon As Pokemon, ByVal CurrentEntity As NPC, ByVal CurrentModel As ModelEntity)
            Dim MoveAnimation As AnimationQueryObject = New AnimationQueryObject(CurrentEntity, BattleFlip, CurrentModel)
            MoveAnimation.AnimationMove(Nothing, False, -0.5F, 0, 0, 0.3F, False, False, 0, 0,,, 2)
            MoveAnimation.AnimationMove(Nothing, False, 0, 0, 0, 0.3F, False, False, 1, 0,,, 2)
            BattleScreen.BattleQuery.Add(MoveAnimation)
        End Sub

        Public Overrides Sub InternalOpponentPokemonMoveAnimation(ByVal BattleScreen As BattleScreen, ByVal BattleFlip As Boolean, ByVal CurrentPokemon As Pokemon, ByVal CurrentEntity As NPC, ByVal CurrentModel As ModelEntity)
            Dim MoveAnimation As AnimationQueryObject = New AnimationQueryObject(CurrentEntity, BattleFlip)
            MoveAnimation.AnimationPlaySound("Battle\Attacks\Normal\Tackle", 0, 0)
            Dim SpawnEntity = MoveAnimation.SpawnEntity(New Vector3(0, 0, 0), TextureManager.GetTexture("Textures\Battle\Normal\Tackle"), New Vector3(0.5F), 1.0F, 0, 2)
            MoveAnimation.AnimationFade(SpawnEntity, True, 1.0F, False, 0.0F, 2, 0)
            BattleScreen.BattleQuery.Add(MoveAnimation)
        End Sub
    End Class

End Namespace