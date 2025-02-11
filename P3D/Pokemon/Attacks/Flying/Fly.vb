Namespace BattleSystem.Moves.Flying

    Public Class Fly

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Flying)
            Me.ID = 19
            Me.OriginalPP = 15
            Me.CurrentPP = 15
            Me.MaxPP = 15
            Me.Power = 90
            Me.Accuracy = 95
            Me.Category = Categories.Physical
            Me.ContestCategory = ContestCategories.Smart
            Me.Name = Localization.GetString("move_name_" & Me.ID,"Fly")
            Me.Description = "The user soars, then strikes its target on the second turn. It can also be used for flying to any familiar town."
            Me.CriticalChance = 1
            Me.IsHMMove = True
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

            Me.DisabledWhileGravity = True
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
            Me.AIField2 = AIField.MultiTurn
        End Sub

        Public Overrides Function GetUseAccEvasion(own As Boolean, BattleScreen As BattleScreen) As Boolean
            Dim fly As Integer = BattleScreen.FieldEffects.OwnFlyCounter
            If own = False Then
                fly = BattleScreen.FieldEffects.OppFlyCounter
            End If

            If fly = 0 Then
                Return False
            Else
                Return True
            End If
        End Function

        Public Overrides Sub PreAttack(Own As Boolean, BattleScreen As BattleScreen)
            Dim fly As Integer = BattleScreen.FieldEffects.OwnFlyCounter
            If Own = False Then
                fly = BattleScreen.FieldEffects.OppFlyCounter
            End If

            If fly = 0 Then
                Me.FocusOppPokemon = False
            Else
                Me.FocusOppPokemon = True
            End If
        End Sub

        Public Overrides Function MoveFailBeforeAttack(Own As Boolean, BattleScreen As BattleScreen) As Boolean
            Dim p As Pokemon = BattleScreen.OwnPokemon
            Dim op As Pokemon = BattleScreen.OppPokemon
            If Own = False Then
                p = BattleScreen.OppPokemon
                op = BattleScreen.OwnPokemon
            End If

            Dim fly As Integer = BattleScreen.FieldEffects.OwnFlyCounter
            If Own = False Then
                fly = BattleScreen.FieldEffects.OppFlyCounter
            End If

            If Not p.Item Is Nothing Then
                If p.Item.OriginalName.ToLower() = "power herb" And BattleScreen.FieldEffects.CanUseItem(Own) = True And BattleScreen.FieldEffects.CanUseOwnItem(Own, BattleScreen) = True Then
                    If BattleScreen.Battle.RemoveHeldItem(Own, Own, BattleScreen, "Power Herb pushed the use of Fly!", "move:fly") = True Then
                        fly = 1
                    End If
                End If
            End If

            If fly = 0 Then
                BattleScreen.BattleQuery.Add(New TextQueryObject(p.GetDisplayName() & " flew up high!"))
                If Own = True Then
                    BattleScreen.FieldEffects.OwnFlyCounter = 1
                Else
                    BattleScreen.FieldEffects.OppFlyCounter = 1
                End If
                Return True
            ElseIf fly = 1 Then
                If Own = True Then
                    BattleScreen.FieldEffects.OwnFlyCounter = 2
                Else
                    BattleScreen.FieldEffects.OppFlyCounter = 2
                End If
                Return False
            End If
            Return False
        End Function
        Public Overrides Sub MoveSelected(own As Boolean, BattleScreen As BattleScreen)
            If own = True Then
                BattleScreen.FieldEffects.OwnFlyCounter = 0
            Else
                BattleScreen.FieldEffects.OppFlyCounter = 0
            End If
        End Sub
        Public Overrides Function DeductPP(own As Boolean, BattleScreen As BattleScreen) As Boolean
            Dim fly As Integer = BattleScreen.FieldEffects.OwnFlyCounter
            If own = False Then
                fly = BattleScreen.FieldEffects.OppFlyCounter
            End If

            If fly = 0 Then
                Return False
            Else
                Return True
            End If
        End Function

        Private Sub MoveFails(own As Boolean, BattleScreen As BattleScreen)
            If own = True Then
                BattleScreen.FieldEffects.OwnFlyCounter = 0
            Else
                BattleScreen.FieldEffects.OppFlyCounter = 0
            End If
            Me.FailPokemonMoveAnimation(BattleScreen, own)
        End Sub

        Public Overrides Sub MoveMisses(own As Boolean, BattleScreen As BattleScreen)
            MoveFails(own, BattleScreen)
        End Sub

        Public Overrides Sub AbsorbedBySubstitute(own As Boolean, BattleScreen As BattleScreen)
            MoveFails(own, BattleScreen)
        End Sub

        Public Overrides Sub MoveProtectedDetected(own As Boolean, BattleScreen As BattleScreen)
            MoveFails(own, BattleScreen)
        End Sub

        Public Overrides Sub InflictedFlinch(own As Boolean, BattleScreen As BattleScreen)
            MoveFails(own, BattleScreen)
        End Sub

        Public Overrides Sub IsSleeping(own As Boolean, BattleScreen As BattleScreen)
            MoveFails(own, BattleScreen)
        End Sub

        Public Overrides Sub HurtItselfInConfusion(own As Boolean, BattleScreen As BattleScreen)
            MoveFails(own, BattleScreen)
        End Sub

        Public Overrides Sub IsParalyzed(own As Boolean, BattleScreen As BattleScreen)
            MoveFails(own, BattleScreen)
        End Sub

        Public Overrides Sub IsAttracted(own As Boolean, BattleScreen As BattleScreen)
            MoveFails(own, BattleScreen)
        End Sub
        Public Overrides Sub InternalUserPokemonMoveAnimation(ByVal BattleScreen As BattleScreen, ByVal BattleFlip As Boolean, ByVal CurrentPokemon As Pokemon, ByVal CurrentEntity As NPC)
            Dim fly As Integer = BattleScreen.FieldEffects.OwnFlyCounter
            If BattleFlip = True Then
                fly = BattleScreen.FieldEffects.OppFlyCounter
            End If
            Dim MoveAnimation = New AnimationQueryObject(CurrentEntity, BattleFlip)

            If fly = 0 Then
                MoveAnimation.AnimationPlaySound("Battle\Attacks\Flying\Fly_Start", 0, 0)
                MoveAnimation.AnimationFade(Nothing, False, 0.2F, False, 0.0F, 0, 0)
                Dim FlyEntity = MoveAnimation.SpawnEntity(New Vector3(0), TextureManager.GetTexture("Textures\Battle\Flying\Fly", New Rectangle(0, 0, 32, 32), ""), New Vector3(0.5F), 0.0F)
                MoveAnimation.AnimationFade(FlyEntity, False, 0.2F, True, 1.0F, 0, 0)
                MoveAnimation.AnimationMove(FlyEntity, True, 0.0, 2.0, 0.0, 0.06, False, False, 1.4F, 0.0,,, 0.06, 0)
                MoveAnimation.AnimationChangeTexture(FlyEntity, False, TextureManager.GetTexture("Textures\Battle\Flying\Fly", New Rectangle(0, 32, 32, 32), ""), 1.3F, 0)
                MoveAnimation.AnimationChangeTexture(FlyEntity, False, TextureManager.GetTexture("Textures\Battle\Flying\Fly", New Rectangle(0, 64, 32, 32), ""), 1.4F, 0)
                MoveAnimation.AnimationChangeTexture(FlyEntity, False, TextureManager.GetTexture("Textures\Battle\Flying\Fly", New Rectangle(0, 96, 32, 32), ""), 1.5F, 0)
                MoveAnimation.AnimationChangeTexture(FlyEntity, False, TextureManager.GetTexture("Textures\Battle\Flying\Fly", New Rectangle(0, 128, 32, 32), ""), 1.6F, 0)
                MoveAnimation.AnimationChangeTexture(FlyEntity, False, TextureManager.GetTexture("Textures\Battle\Flying\Fly", New Rectangle(0, 160, 32, 32), ""), 1.7F, 0)

                BattleScreen.BattleQuery.Add(MoveAnimation)
            Else
                MoveAnimation.AnimationPlaySound("Battle\Attacks\Flying\Fly_Start", 0, 0)
                Dim FlyEntity = MoveAnimation.SpawnEntity(New Vector3(0, 0.9, 0), TextureManager.GetTexture("Textures\Battle\Flying\Fly", New Rectangle(0, 0, 32, 32), ""), New Vector3(0.5F), 1.0F)
                MoveAnimation.AnimationMove(FlyEntity, True, 2.0, 0.5, 0, 0.07, False, False, 0.0F, 0.0,,, 0.035, 0)

                BattleScreen.BattleQuery.Add(MoveAnimation)
            End If
        End Sub
        Public Overrides Sub InternalOpponentPokemonMoveAnimation(ByVal BattleScreen As BattleScreen, ByVal BattleFlip As Boolean, ByVal CurrentPokemon As Pokemon, ByVal CurrentEntity As NPC)
            Dim fly As Integer = BattleScreen.FieldEffects.OwnFlyCounter
            If BattleFlip = True Then
                fly = BattleScreen.FieldEffects.OppFlyCounter
            End If

            If fly = 2 Then
                Dim MoveAnimation = New AnimationQueryObject(CurrentEntity, BattleFlip)

                MoveAnimation.AnimationPlaySound("Battle\Attacks\Flying\Fly_Hit", 0, 0)
                Dim FlyEntity = MoveAnimation.SpawnEntity(New Vector3(-2, 0.9, 0), TextureManager.GetTexture("Textures\Battle\Flying\Fly", New Rectangle(0, 0, 32, 32), ""), New Vector3(0.5F), 1.0F)
                MoveAnimation.AnimationMove(FlyEntity, True, 0.0, 0.0, 0.0, 0.07, False, False, 0.0, 0.0,,, 0.035, 3)

                If BattleFlip = False Then
                    MoveAnimation.AnimationFade(BattleScreen.OwnPokemonNPC, False, 1, True, 1.0F, 0, 0)
                Else
                    MoveAnimation.AnimationFade(BattleScreen.OppPokemonNPC, False, 1, True, 1.0F, 0, 0)
                End If

                BattleScreen.BattleQuery.Add(MoveAnimation)
            End If
        End Sub
        Public Overrides Sub InternalFailPokemonMoveAnimation(BattleScreen As BattleScreen, BattleFlip As Boolean, CurrentPokemon As Pokemon, CurrentEntity As NPC)
            Dim MoveAnimation = New AnimationQueryObject(CurrentEntity, BattleFlip)
            Dim FadeDelay As Single = 0.0F
            Dim FadeSpeed As Single = 1.0F

            Dim fly As Integer = BattleScreen.FieldEffects.OwnFlyCounter
            If BattleFlip = True Then
                fly = BattleScreen.FieldEffects.OppFlyCounter
            End If

            If BattleScreen.FieldEffects.Gravity > 0 Then
                FadeDelay = 2.3F
                FadeSpeed = 0.2F
                Dim FlyEntity = MoveAnimation.SpawnEntity(New Vector3(0, 2, 0), TextureManager.GetTexture("Textures\Battle\Flying\Fly", New Rectangle(0, 160, 32, 32), ""), New Vector3(0.5F), 1.0F)
                MoveAnimation.AnimationMove(FlyEntity, False, 0.0, 0.0, 0.0, 0.1F, False, False, 0.0F, 0.0,,, 0.1F, 1)
                MoveAnimation.AnimationChangeTexture(FlyEntity, False, TextureManager.GetTexture("Textures\Battle\Flying\Fly", New Rectangle(0, 128, 32, 32), ""), 0.0F, 0)
                MoveAnimation.AnimationChangeTexture(FlyEntity, False, TextureManager.GetTexture("Textures\Battle\Flying\Fly", New Rectangle(0, 96, 32, 32), ""), 0.1F, 0)
                MoveAnimation.AnimationChangeTexture(FlyEntity, False, TextureManager.GetTexture("Textures\Battle\Flying\Fly", New Rectangle(0, 64, 32, 32), ""), 0.2F, 0)
                MoveAnimation.AnimationChangeTexture(FlyEntity, False, TextureManager.GetTexture("Textures\Battle\Flying\Fly", New Rectangle(0, 32, 32, 32), ""), 0.3F, 0)
                MoveAnimation.AnimationChangeTexture(FlyEntity, False, TextureManager.GetTexture("Textures\Battle\Flying\Fly", New Rectangle(0, 0, 32, 32), ""), 0.4F, 0)
                MoveAnimation.AnimationFade(FlyEntity, True, FadeSpeed, False, 0.0F, FadeDelay + 0.1F, 0, 1)
            End If
            If BattleFlip = False Then
                MoveAnimation.AnimationFade(BattleScreen.OwnPokemonNPC, False, FadeSpeed, True, 1.0F, FadeDelay, 0)
            Else
                MoveAnimation.AnimationFade(BattleScreen.OppPokemonNPC, False, FadeSpeed, True, 1.0F, FadeDelay, 0)
            End If
            BattleScreen.BattleQuery.Add(MoveAnimation)
        End Sub
    End Class

End Namespace