Namespace BattleSystem.Moves.Electric

    Public Class Flash

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Electric)
            Me.ID = 148
            Me.OriginalPP = 15
            Me.CurrentPP = 15
            Me.MaxPP = 15
            Me.Power = 60
            Me.Accuracy = 100
            Me.Category = Categories.Special
            Me.ContestCategory = ContestCategories.Beauty
            Me.Name = Localization.GetString("move_name_" & Me.ID,"Flash")
            Me.Description = "The user flashes a bright light with enormous speed at the target. It cuts the user's accuracy though."
            Me.CriticalChance = 1
            Me.IsHMMove = True
            Me.Target = Targets.OneAdjacentTarget
            Me.Priority = 1
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
            Me.IsHealingMove = False
            Me.RemovesOwnFrozen = False
            Me.IsRecoilMove = False

            Me.ImmunityAffected = True
            Me.IsDamagingMove = True
            Me.IsProtectMove = False

            Me.HasSecondaryEffect = True
            Me.IsAffectedBySubstitute = True
            Me.IsOneHitKOMove = False
            Me.IsWonderGuardAffected = True
            Me.CanHitSleeping = False
            '#End

            Me.AIField1 = AIField.Damage
            Me.AIField2 = AIField.HighPriority
        End Sub

        Public Overrides Sub MoveHits(own As Boolean, BattleScreen As BattleScreen)
            BattleScreen.Battle.LowerStat(own, own, BattleScreen, "Accuracy", 1, "", "move:flash")
        End Sub

        Public Overrides Sub InternalOpponentPokemonMoveAnimation(ByVal BattleScreen As BattleScreen, ByVal BattleFlip As Boolean, ByVal CurrentPokemon As Pokemon, ByVal CurrentEntity As NPC)
            Dim MoveAnimation As AnimationQueryObject = New AnimationQueryObject(CurrentEntity, BattleFlip, True)
            MoveAnimation.AnimationPlaySound("Battle\Attacks\Electric\Flash", 0.0F, 0)
            MoveAnimation.AnimationBackground(TextureManager.GetTexture("Textures\Battle\Electric\FlashBackground"), 0, 0, 0.25F, 0.9F, 0.2F, 0.025F, True, 1, 0, 6)
            MoveAnimation.AnimationColor(CurrentEntity, False, 0.2F, True, 0, 0, New Vector3(0.1), Nothing, 0.025F)

            BattleScreen.BattleQuery.Add(MoveAnimation)
        End Sub

    End Class

End Namespace