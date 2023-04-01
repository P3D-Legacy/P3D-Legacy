Namespace BattleSystem.Moves.Water

    Public Class Clamp

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Water)
            Me.ID = 128
            Me.OriginalPP = 10
            Me.CurrentPP = 10
            Me.MaxPP = 10
            Me.Power = 35
            Me.Accuracy = 85
            Me.Category = Categories.Physical
            Me.ContestCategory = ContestCategories.Tough
            Me.Name = Localization.GetString("move_name_" & Me.ID,"Clamp")
            Me.Description = "The target is clamped and squeezed by the user's very thick and sturdy shell for four to five turns."
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
            Me.RemovesOwnFrozen = False

            Me.IsHealingMove = False
            Me.IsRecoilMove = False

            Me.IsDamagingMove = True
            Me.IsProtectMove = False


            Me.IsAffectedBySubstitute = True
            Me.IsOneHitKOMove = False
            Me.IsWonderGuardAffected = True
            '#End

            Me.AIField1 = AIField.Damage
            Me.AIField2 = AIField.Trap
        End Sub

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
                If p.Item.OriginalName.ToLower() = "grip claw" And BattleScreen.FieldEffects.CanUseItem(own) = True And BattleScreen.FieldEffects.CanUseOwnItem(own, BattleScreen) = True Then
                    turns = 5
                End If
            End If

            If own = True Then
                If BattleScreen.FieldEffects.OppClamp = 0 Then
                    BattleScreen.FieldEffects.OppClamp = turns
                    BattleScreen.BattleQuery.Add(New TextQueryObject(p.GetDisplayName() & " clamped " & op.GetDisplayName() & "!"))
                End If
            Else
                If BattleScreen.FieldEffects.OwnClamp = 0 Then
                    BattleScreen.FieldEffects.OwnClamp = turns
                    BattleScreen.BattleQuery.Add(New TextQueryObject(p.GetDisplayName() & " clamped " & op.GetDisplayName() & "!"))
                End If
            End If
        End Sub

        Public Overrides Sub InternalOpponentPokemonMoveAnimation(ByVal BattleScreen As BattleScreen, ByVal BattleFlip As Boolean, ByVal CurrentPokemon As Pokemon, ByVal CurrentEntity As NPC)
            Dim MoveAnimation As AnimationQueryObject = New AnimationQueryObject(CurrentEntity, BattleFlip)
            Dim offsetLeft As Single = -0.35
            Dim offsetRight As Single = 0.35
            If BattleFlip = True Then
                offsetLeft = 0.35
                offsetRight = -0.35
            End If
            MoveAnimation.AnimationPlaySound("Battle\Attacks\Water\Clamp", 0, 0)
            Dim ClampEntityLeft = MoveAnimation.SpawnEntity(New Vector3(offsetLeft, -0.1, offsetLeft), TextureManager.GetTexture("Textures\Battle\Water\Clamp_Left", New Rectangle(0, 0, 24, 64), ""), New Vector3(0.28F, 0.75F, 0.28F), 0.75F)
            Dim ClampEntityRight = MoveAnimation.SpawnEntity(New Vector3(offsetRight, -0.1, offsetRight), TextureManager.GetTexture("Textures\Battle\Water\Clamp_Right", New Rectangle(0, 0, 24, 64), ""), New Vector3(0.28F, 0.75F, 0.28F), 0.75F)
            MoveAnimation.AnimationMove(ClampEntityLeft, False, -0.1, -0.1, -0.1, 0.02, False, False, 0, 0)
            MoveAnimation.AnimationMove(ClampEntityRight, False, 0.1, -0.1, 0.1, 0.02, False, False, 0, 0)
            MoveAnimation.AnimationMove(ClampEntityLeft, True, -0.35, -0.1, -0.35, 0.02, False, False, 2, 0)
            MoveAnimation.AnimationMove(ClampEntityRight, True, 0.35, -0.1, 0.35, 0.02, False, False, 2, 0)
            Dim SpawnEntity = MoveAnimation.SpawnEntity(New Vector3(0, -0.2, 0), TextureManager.GetTexture("Textures\Battle\Normal\Tackle"), New Vector3(0.5F), 1.0F, 2.5, 2)
            MoveAnimation.AnimationFade(SpawnEntity, True, 1.0F, False, 0.0F, 4.5F, 0)

            BattleScreen.BattleQuery.Add(MoveAnimation)
        End Sub
    End Class

End Namespace