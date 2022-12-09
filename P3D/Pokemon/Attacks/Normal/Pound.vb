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
            Me.RemovesFrozen = False
            Me.HasSecondaryEffect = False

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
            Me.AIField2 = AIField.Nothing
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