Namespace BattleSystem.Moves.Flying

    Public Class Gust

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Flying)
            Me.ID = 16
            Me.OriginalPP = 35
            Me.CurrentPP = 35
            Me.MaxPP = 35
            Me.Power = 40
            Me.Accuracy = 100
            Me.Category = Categories.Special
            Me.ContestCategory = ContestCategories.Smart
            Me.Name = Localization.GetString("move_name_" & Me.ID,"Gust")
            Me.Description = "A gust of wind is whipped up by wings and launched at the target to inflict damage."
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
            Me.IsWindMove = True
            Me.IsDamagingMove = True
            Me.IsProtectMove = False

            Me.IsAffectedBySubstitute = True
            Me.IsOneHitKOMove = False
            Me.IsWonderGuardAffected = True
            Me.CanHitInMidAir = True
            '#End
        End Sub

        Public Overrides Function GetBasePower(own As Boolean, BattleScreen As BattleScreen) As Integer
            Dim fly As Integer = 0
            Dim bounce As Integer = 0

            If own = True Then
                fly = BattleScreen.FieldEffects.OppFlyCounter
                bounce = BattleScreen.FieldEffects.OppBounceCounter
            Else
                fly = BattleScreen.FieldEffects.OwnFlyCounter
                bounce = BattleScreen.FieldEffects.OwnBounceCounter
            End If

            If fly > 0 Or bounce > 0 Then
                Return Me.Power * 2
            Else
                Return Me.Power
            End If
        End Function
        Public Overrides Sub InternalUserPokemonMoveAnimation(ByVal BattleScreen As BattleScreen, ByVal BattleFlip As Boolean, ByVal CurrentPokemon As Pokemon, ByVal CurrentEntity As NPC)
            Dim MoveAnimation = New AnimationQueryObject(CurrentEntity, BattleFlip)

            MoveAnimation.AnimationPlaySound("Battle\Attacks\Flying\Gust", 0, 0)
            Dim GustEntity = MoveAnimation.SpawnEntity(New Vector3(0), TextureManager.GetTexture("Textures\Battle\Flying\Gust", New Rectangle(0, 0, 32, 64), ""), New Vector3(0.5F, 1.0F, 0.5F), 1.0F)
            MoveAnimation.AnimationMove(GustEntity, True, 2.0, 0.0, 0.0, 0.04, False, False, 0.0, 0.0)

            MoveAnimation.AnimationChangeTexture(GustEntity, False, TextureManager.GetTexture("Textures\Battle\Flying\Gust", New Rectangle(32, 0, 32, 64), ""), 0.5, 0)
            MoveAnimation.AnimationChangeTexture(GustEntity, False, TextureManager.GetTexture("Textures\Battle\Flying\Gust", New Rectangle(0, 0, 32, 64), ""), 1, 0)
            MoveAnimation.AnimationChangeTexture(GustEntity, False, TextureManager.GetTexture("Textures\Battle\Flying\Gust", New Rectangle(32, 0, 32, 64), ""), 1.5, 0)
            MoveAnimation.AnimationChangeTexture(GustEntity, False, TextureManager.GetTexture("Textures\Battle\Flying\Gust", New Rectangle(0, 0, 32, 64), ""), 2.0, 0)
            MoveAnimation.AnimationChangeTexture(GustEntity, False, TextureManager.GetTexture("Textures\Battle\Flying\Gust", New Rectangle(32, 0, 32, 64), ""), 2.5, 0)
            MoveAnimation.AnimationChangeTexture(GustEntity, False, TextureManager.GetTexture("Textures\Battle\Flying\Gust", New Rectangle(0, 0, 32, 64), ""), 3.0, 0)
            MoveAnimation.AnimationChangeTexture(GustEntity, False, TextureManager.GetTexture("Textures\Battle\Flying\Gust", New Rectangle(32, 0, 32, 64), ""), 3.5, 0)
            MoveAnimation.AnimationChangeTexture(GustEntity, False, TextureManager.GetTexture("Textures\Battle\Flying\Gust", New Rectangle(0, 0, 32, 64), ""), 4.0, 0.5)

            BattleScreen.BattleQuery.Add(MoveAnimation)
        End Sub

        Public Overrides Sub InternalOpponentPokemonMoveAnimation(ByVal BattleScreen As BattleScreen, ByVal BattleFlip As Boolean, ByVal CurrentPokemon As Pokemon, ByVal CurrentEntity As NPC)
            Dim MoveAnimation = New AnimationQueryObject(CurrentEntity, BattleFlip)

            MoveAnimation.AnimationPlaySound("Battle\Attacks\Flying\Gust", 0, 0)
            Dim GustEntity = MoveAnimation.SpawnEntity(New Vector3(-2, 0, 0), TextureManager.GetTexture("Textures\Battle\Flying\Gust", New Rectangle(0, 0, 32, 64), ""), New Vector3(0.5F, 1.0F, 0.5F), 1.0F)
            MoveAnimation.AnimationMove(GustEntity, False, -0.05, 0.0, 0.0, 0.04, False, False, 0.0, 0.0)

            MoveAnimation.AnimationChangeTexture(GustEntity, False, TextureManager.GetTexture("Textures\Battle\Flying\Gust", New Rectangle(32, 0, 32, 64), ""), 0.5, 0)
            MoveAnimation.AnimationChangeTexture(GustEntity, False, TextureManager.GetTexture("Textures\Battle\Flying\Gust", New Rectangle(0, 0, 32, 64), ""), 1, 0)
            MoveAnimation.AnimationChangeTexture(GustEntity, False, TextureManager.GetTexture("Textures\Battle\Flying\Gust", New Rectangle(32, 0, 32, 64), ""), 1.5, 0)
            MoveAnimation.AnimationChangeTexture(GustEntity, False, TextureManager.GetTexture("Textures\Battle\Flying\Gust", New Rectangle(0, 0, 32, 64), ""), 2.0, 0)
            MoveAnimation.AnimationChangeTexture(GustEntity, False, TextureManager.GetTexture("Textures\Battle\Flying\Gust", New Rectangle(32, 0, 32, 64), ""), 2.5, 0)
            MoveAnimation.AnimationChangeTexture(GustEntity, False, TextureManager.GetTexture("Textures\Battle\Flying\Gust", New Rectangle(0, 0, 32, 64), ""), 3.0, 0)
            MoveAnimation.AnimationChangeTexture(GustEntity, False, TextureManager.GetTexture("Textures\Battle\Flying\Gust", New Rectangle(32, 0, 32, 64), ""), 3.5, 0)
            MoveAnimation.AnimationChangeTexture(GustEntity, False, TextureManager.GetTexture("Textures\Battle\Flying\Gust", New Rectangle(0, 0, 32, 64), ""), 4.0, 0.5)
            MoveAnimation.AnimationChangeTexture(GustEntity, False, TextureManager.GetTexture("Textures\Battle\Flying\Gust", New Rectangle(32, 0, 32, 64), ""), 4.5, 0)
            MoveAnimation.AnimationChangeTexture(GustEntity, False, TextureManager.GetTexture("Textures\Battle\Flying\Gust", New Rectangle(0, 0, 32, 64), ""), 5.0, 0)
            MoveAnimation.AnimationChangeTexture(GustEntity, False, TextureManager.GetTexture("Textures\Battle\Flying\Gust", New Rectangle(32, 0, 32, 64), ""), 5.5, 0)
            MoveAnimation.AnimationChangeTexture(GustEntity, True, TextureManager.GetTexture("Textures\Battle\Flying\Gust", New Rectangle(0, 0, 32, 64), ""), 6.0, 0.5)

            BattleScreen.BattleQuery.Add(MoveAnimation)
        End Sub
    End Class

End Namespace