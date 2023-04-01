Namespace BattleSystem.Moves.Normal

    Public Class Pound

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Normal)
            Me.ID = 1
            Me.OriginalPP = 25
            Me.CurrentPP = 25
            Me.MaxPP = 25
            Me.Power = 40
            Me.Accuracy = 100
            Me.Category = Categories.Physical
            Me.ContestCategory = ContestCategories.Tough
            Me.Name = Localization.GetString("move_name_" & Me.ID,"Pound")
            Me.Description = "The target is physically pounded with a long tail or a foreleg, etc."
            Me.CriticalChance = 1
            Me.IsHMMove = False
            Me.Target = Targets.OneAdjacentTarget
            Me.Priority = 0
            Me.TimesToAttack = 1
            '#End

            '#SpecialDefinitions
            'Generic Secondary effects
            Me.HasSecondaryEffect = False
            Me.IsHealingMove = False
            Me.IsDamagingMove = True
            Me.IsProtectMove = False
            Me.IsOneHitKOMove = False
            Me.IsRecoilMove = False
            Me.IsTrappingMove = False
            Me.RemovesOwnFrozen = False

            'Interacts with other moves/effects
            Me.ProtectAffected = True
            Me.MagicCoatAffected = False
            Me.SnatchAffected = False
            Me.MirrorMoveAffected = True
            Me.KingsrockAffected = True
            Me.CounterAffected = True
            Me.IsAffectedBySubstitute = True
            Me.ImmunityAffected = True
            Me.IsWonderGuardAffected = True
            Me.DisabledWhileGravity = False

            'ignore stats status or positioning?
            Me.UseAccEvasion = True
            Me.CanHitInMidAir = False
            Me.CanHitUnderground = False
            Me.CanHitUnderwater = False
            Me.CanHitSleeping = True
            Me.CanGainSTAB = True
            Me.UseOppDefense = True
            Me.UseOppEvasion = True
            Me.UseEffectiveness = True

            'categories
            Me.MakesContact = True
            Me.IsPulseMove = False
            Me.IsBulletMove = False
            Me.IsJawMove = False
            Me.IsDanceMove = False
            Me.IsExplosiveMove = False
            Me.IsPowderMove = False
            Me.IsPunchingMove = False
            Me.IsSlicingMove = False
            Me.IsSoundMove = False
            Me.IsWindMove = False
            '#End

            Me.AIField1 = AIField.Damage
            Me.AIField2 = AIField.Nothing
            Me.AIField3 = AIField.Nothing
        End Sub

        Public Overrides Sub InternalOpponentPokemonMoveAnimation(ByVal BattleScreen As BattleScreen, ByVal BattleFlip As Boolean, ByVal CurrentPokemon As Pokemon, ByVal CurrentEntity As NPC)
            Dim MoveAnimation As AnimationQueryObject = New AnimationQueryObject(CurrentEntity, BattleFlip)
            MoveAnimation.AnimationPlaySound("Battle\Attacks\Normal\Pound", 0.5, 2.5)
            Dim PoundEntity = MoveAnimation.SpawnEntity(New Vector3(0, -0.2, 0), TextureManager.GetTexture("Textures\Battle\Normal\Pound"), New Vector3(0.5F), 1, 0, 3)
            MoveAnimation.AnimationFade(PoundEntity, True, 1.0F, False, 0.0F, 3, 0)
            BattleScreen.BattleQuery.Add(MoveAnimation)
        End Sub

    End Class

End Namespace