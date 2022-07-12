Namespace BattleSystem.Moves.Water

    Public Class Whirlpool

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Water)
            Me.ID = 250
            Me.OriginalPP = 15
            Me.CurrentPP = 15
            Me.MaxPP = 15
            Me.Power = 35
            Me.Accuracy = 85
            Me.Category = Categories.Special
            Me.ContestCategory = ContestCategories.Beauty
            Me.Name = "Whirlpool"
            Me.Description = "Traps foes in a violent swirling whirlpool for four to five turns."
            Me.CriticalChance = 1
            Me.IsHMMove = True
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
            Me.AIField2 = AIField.Trap
        End Sub

        Public Overrides Function GetBasePower(own As Boolean, BattleScreen As BattleScreen) As Integer
            Dim dive As Integer = BattleScreen.FieldEffects.OppDiveCounter
            If own = False Then
                dive = BattleScreen.FieldEffects.OwnDiveCounter
            End If

            If dive > 0 Then
                Return Me.Power * 2
            Else
                Return Me.Power
            End If
        End Function

        Public Overrides Sub MoveHits(own As Boolean, BattleScreen As BattleScreen)
            Dim p As Pokemon = BattleScreen.OwnPokemon
            Dim op As Pokemon = BattleScreen.OppPokemon
            If own = False Then
                p = BattleScreen.OppPokemon
                op = BattleScreen.OwnPokemon
            End If

            Dim turns As Integer = 4
            If Core.Random.Next(0, 100) < 50 Then
                turns = 5
            End If

            If Not p.Item Is Nothing Then
                If p.Item.Name.ToLower() = "grip claw" And BattleScreen.FieldEffects.CanUseItem(own) = True And BattleScreen.FieldEffects.CanUseOwnItem(own, BattleScreen) = True Then
                    turns = 5
                End If
            End If

            If own = True Then
                If BattleScreen.FieldEffects.OppWhirlpool = 0 Then
                    BattleScreen.FieldEffects.OppWhirlpool = turns
                    BattleScreen.BattleQuery.Add(New TextQueryObject(op.GetDisplayName() & " was trapped in the vortex!"))
                End If
            Else
                If BattleScreen.FieldEffects.OwnWhirlpool = 0 Then
                    BattleScreen.FieldEffects.OwnWhirlpool = turns
                    BattleScreen.BattleQuery.Add(New TextQueryObject(op.GetDisplayName() & " was trapped in the vortex!"))
                End If
            End If
        End Sub
        Public Overrides Sub InternalOpponentPokemonMoveAnimation(ByVal BattleScreen As BattleScreen, ByVal BattleFlip As Boolean, ByVal CurrentPokemon As Pokemon, ByVal CurrentEntity As NPC, ByVal CurrentModel As Entity)
            Dim MoveAnimation As AnimationQueryObject = New AnimationQueryObject(CurrentEntity, BattleFlip,, True)
            MoveAnimation.AnimationPlaySound("Battle\Attacks\Water\Whirlpool", 0.0F, 0)
            Dim WhirlpoolEntity As Entity = MoveAnimation.SpawnEntity(New Vector3(0, -0.3F, 0), TextureManager.GetTexture("Textures\Battle\Water\Whirlpool"), New Vector3(0.0F), 1.0F, 0.0F, 0.0F)
            MoveAnimation.AnimationRotate(WhirlpoolEntity, False, CSng(MathHelper.Pi * 1.5), 0, 0, CSng(MathHelper.Pi * 1.5), 0, 0, 0, 0, True, False, False, False)
            MoveAnimation.AnimationRotate(WhirlpoolEntity, False, 0, 0, 0.2F, 0, 0, 10.0F, 0.0F, 0.0F, False, False, True, True)
            MoveAnimation.AnimationScale(WhirlpoolEntity, False, True, 1.0F, 1.0F, 1.0F, 0.025F, 0.0F, 0.0F)
            MoveAnimation.AnimationScale(WhirlpoolEntity, True, False, 0.0F, 0.0F, 0.0F, 0.025F, 5.0F, 0.0F)
            BattleScreen.BattleQuery.Add(MoveAnimation)
        End Sub
    End Class

End Namespace