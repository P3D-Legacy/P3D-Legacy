Namespace BattleSystem.Moves.Grass

    Public Class VineWhip

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Grass)
            Me.ID = 22
            Me.OriginalPP = 25
            Me.CurrentPP = 25
            Me.MaxPP = 25
            Me.Power = 45
            Me.Accuracy = 100
            Me.Category = Categories.Physical
            Me.ContestCategory = ContestCategories.Cool
            Me.Name = "Vine Whip"
            Me.Description = "The target is struck with slender, whiplike vines to inflict damage."
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
            MoveAnimation.AnimationPlaySound("Battle\Attacks\Grass\VineWhip_Start", 0.5, 2.5)
            MoveAnimation.AnimationMove(Nothing, False, 0, 0, -0.1, 0.025, False, False, 0, 0.5)
            MoveAnimation.AnimationMove(Nothing, False, 0, 0, 0, 0.025, False, False, 0.75, 0)
            BattleScreen.BattleQuery.Add(MoveAnimation)
        End Sub

        Public Overrides Sub InternalOpponentPokemonMoveAnimation(ByVal BattleScreen As BattleScreen, ByVal BattleFlip As Boolean, ByVal CurrentPokemon As Pokemon, ByVal CurrentEntity As NPC, ByVal CurrentModel As ModelEntity)
            Dim MoveAnimation As AnimationQueryObject = New AnimationQueryObject(CurrentEntity, BattleFlip)
            MoveAnimation.AnimationPlaySound("Battle\Attacks\Grass\VineWhip_Hit", 0, 2.5)
            Dim TextureXOffset As Integer = 0
            If BattleFlip = True Then
                TextureXOffset = 32
            End If
            Dim VineWhipEntity = MoveAnimation.SpawnEntity(New Vector3(0, -0.2, 0), TextureManager.GetTexture("Textures\Battle\Grass\VineWhip", New Rectangle(TextureXOffset, 0, 32, 32), ""), New Vector3(0.5F), 1, 0, 0.5)
            MoveAnimation.AnimationChangeTexture(VineWhipEntity, False, TextureManager.GetTexture("Textures\Battle\Grass\VineWhip", New Rectangle(TextureXOffset, 32, 32, 32), ""), 0.5, 0.5)
            MoveAnimation.AnimationChangeTexture(VineWhipEntity, False, TextureManager.GetTexture("Textures\Battle\Grass\VineWhip", New Rectangle(TextureXOffset, 64, 32, 32), ""), 1, 0.5)
            MoveAnimation.AnimationChangeTexture(VineWhipEntity, True, TextureManager.GetTexture("Textures\Battle\Grass\VineWhip", New Rectangle(TextureXOffset, 96, 32, 32), ""), 1.5, 0.5)
            BattleScreen.BattleQuery.Add(MoveAnimation)
        End Sub
    End Class

End Namespace