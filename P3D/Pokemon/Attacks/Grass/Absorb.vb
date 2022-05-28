﻿Namespace BattleSystem.Moves.Grass

    Public Class Absorb

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Grass)
            Me.ID = 71
            Me.OriginalPP = 25
            Me.CurrentPP = 25
            Me.MaxPP = 25
            Me.Power = 20
            Me.Accuracy = 100
            Me.Category = Categories.Special
            Me.ContestCategory = ContestCategories.Smart
            Me.Name = "Absorb"
            Me.Description = "Inflicts damage on the target, then restores the user's HP based on the damage inflicted."
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
            Me.HasSecondaryEffect = False
            Me.RemovesFrozen = False

            Me.IsHealingMove = True
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
            Me.AIField2 = AIField.Absorbing
        End Sub

        Public Overrides Sub MoveHits(own As Boolean, BattleScreen As BattleScreen)
            Dim p As Pokemon = BattleScreen.OwnPokemon
            Dim op As Pokemon = BattleScreen.OppPokemon
            If own = False Then
                p = BattleScreen.OppPokemon
                op = BattleScreen.OwnPokemon
            End If

            Dim damage As Integer = BattleScreen.FieldEffects.OwnLastDamage
            If own = False Then
                damage = BattleScreen.FieldEffects.OppLastDamage
            End If
            Dim heal As Integer = CInt(Math.Ceiling(damage / 2))

            If heal <= 0 Then
                heal = 1
            End If

            If op.Ability.Name.ToLower() = "liquid ooze" And BattleScreen.FieldEffects.CanUseAbility(Not own, BattleScreen) = True Then
                BattleScreen.Battle.ReduceHP(heal, own, own, BattleScreen, "Liquid Ooze damaged " & p.GetDisplayName() & "!", "liquidooze")
            Else
                If Not p.Item Is Nothing Then
                    If p.Item.Name.ToLower() = "big root" And BattleScreen.FieldEffects.CanUseItem(own) = True And BattleScreen.FieldEffects.CanUseOwnItem(own, BattleScreen) = True Then
                        heal = CInt(Math.Ceiling(damage * (80 / 100)))
                    End If
                End If

                Dim healBlock As Integer = BattleScreen.FieldEffects.OppHealBlock
                If own = False Then
                    healBlock = BattleScreen.FieldEffects.OwnHealBlock
                End If
                If healBlock = 0 Then
                    BattleScreen.Battle.GainHP(heal, own, own, BattleScreen, op.GetDisplayName() & " had its energy drained!", "move:absorb")
                End If
            End If
        End Sub

        Public Overrides Sub InternalOpponentPokemonMoveAnimation(ByVal BattleScreen As BattleScreen, ByVal BattleFlip As Boolean, ByVal CurrentPokemon As Pokemon, ByVal CurrentEntity As NPC, ByVal CurrentModel As ModelEntity)
            Dim MoveAnimation As AnimationQueryObject = New AnimationQueryObject(CurrentEntity, BattleFlip)
            Dim maxAmount As Integer = 12
            Dim currentAmount As Integer = 0
            MoveAnimation.AnimationPlaySound("Battle\Attacks\Grass\Absorb", 0, 0)
            While currentAmount <= maxAmount
                Dim yPos As Single = CSng(Random.Next(-1, 3) * 0.15)
                Dim zPos As Single = CSng(Random.Next(-3, 3) * 0.15)
                Dim AbsorbEntity = MoveAnimation.SpawnEntity(New Vector3(0.0, 0.0, 0.0), TextureManager.GetTexture("Textures\Battle\Grass\Absorb"), New Vector3(0.35F), 1, CSng(currentAmount * 0.8))
                MoveAnimation.AnimationMove(AbsorbEntity, True, -1.5, yPos, zPos, 0.03, False, True, CSng(currentAmount * 0.8), 0.0, 0.1, 0.5,, 0.005F)

                Threading.Interlocked.Increment(currentAmount)
            End While

            BattleScreen.BattleQuery.Add(MoveAnimation)
        End Sub
    End Class



End Namespace
