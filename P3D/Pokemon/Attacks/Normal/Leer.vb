Namespace BattleSystem.Moves.Normal

    Public Class Leer

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Normal)
            Me.ID = 43
            Me.OriginalPP = 30
            Me.CurrentPP = 30
            Me.MaxPP = 30
            Me.Power = 0
            Me.Accuracy = 100
            Me.Category = Categories.Status
            Me.ContestCategory = ContestCategories.Cool
            Me.Name = Localization.GetString("move_name_" & Me.ID,"Leer")
            Me.Description = "The user gains an intimidating leer with sharp eyes. The opposing team’s Defense stats are reduced."
            Me.CriticalChance = 0
            Me.IsHMMove = False
            Me.Target = Targets.AllAdjacentFoes
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
            Me.ImmunityAffected = False
            Me.HasSecondaryEffect = False
            Me.RemovesOwnFrozen = False

            Me.IsHealingMove = False
            Me.IsRecoilMove = False

            Me.IsDamagingMove = False
            Me.IsProtectMove = False


            Me.IsAffectedBySubstitute = True
            Me.IsOneHitKOMove = False
            Me.IsWonderGuardAffected = False
            Me.CanHitSleeping = False
            '#End

            Me.AIField1 = AIField.LowerDefense
            Me.AIField2 = AIField.Nothing
        End Sub

        Public Overrides Sub MoveHits(own As Boolean, BattleScreen As BattleScreen)
            Dim b As Boolean = BattleScreen.Battle.LowerStat(Not own, own, BattleScreen, "Defense", 1, "", "move:leer")
            If b = False Then
                BattleScreen.BattleQuery.Add(New TextQueryObject(Me.Name & " failed!"))
            End If
        End Sub

        Public Overrides Sub InternalUserPokemonMoveAnimation(ByVal BattleScreen As BattleScreen, ByVal BattleFlip As Boolean, ByVal CurrentPokemon As Pokemon, ByVal CurrentEntity As NPC)
            Dim MoveAnimation As AnimationQueryObject = New AnimationQueryObject(CurrentEntity, BattleFlip)
            MoveAnimation.AnimationPlaySound("Battle\Attacks\Normal\Leer", 0, 0)
            Dim SpawnEntity = MoveAnimation.SpawnEntity(New Vector3(0, 0.1, 0.1), TextureManager.GetTexture("Textures\Battle\Normal\Leer"), New Vector3(0.5F), 1.0F, 0, 2)
            MoveAnimation.AnimationScale(SpawnEntity, False, True, 0.7, 0.7, 0.7, 0.05, 0, 0.5)
            MoveAnimation.AnimationScale(SpawnEntity, False, False, 0.5, 0.5, 0.5, 0.05, 0.5, 0.5)
            MoveAnimation.AnimationScale(SpawnEntity, False, True, 0.7, 0.7, 0.7, 0.05, 1.0, 0.5)
            MoveAnimation.AnimationScale(SpawnEntity, False, False, 0.5, 0.5, 0.5, 0.05, 1.5, 0.5)
            MoveAnimation.AnimationFade(SpawnEntity, True, 1.0F, False, 0.0F, 2, 0)
            BattleScreen.BattleQuery.Add(MoveAnimation)
        End Sub
        Public Overrides Sub InternalOpponentPokemonMoveAnimation(ByVal BattleScreen As BattleScreen, ByVal BattleFlip As Boolean, ByVal CurrentPokemon As Pokemon, ByVal CurrentEntity As NPC)
            Dim MoveAnimation As AnimationQueryObject = New AnimationQueryObject(CurrentEntity, BattleFlip)
            MoveAnimation.AnimationMove(Nothing, False, 0, 0, -0.1, 0.025, False, False, 0, 0.5)
            MoveAnimation.AnimationMove(Nothing, False, 0, 0, 0.2, 0.025, False, False, 0.75, 0.5)
            MoveAnimation.AnimationMove(Nothing, False, 0, 0, -0.2, 0.025, False, False, 1.75, 0.5)
            MoveAnimation.AnimationMove(Nothing, False, 0, 0, 0.2, 0.025, False, False, 2.75, 0.5)
            MoveAnimation.AnimationMove(Nothing, False, 0, 0, -0.1, 0.025, False, False, 3.5, 0.5)
            BattleScreen.BattleQuery.Add(MoveAnimation)
        End Sub
    End Class

End Namespace