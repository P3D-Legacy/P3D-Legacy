Namespace BattleSystem.Moves.Fire

    Public Class FirePunch

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Fire)
            Me.ID = 7
            Me.OriginalPP = 15
            Me.CurrentPP = 15
            Me.MaxPP = 15
            Me.Power = 75
            Me.Accuracy = 100
            Me.Category = Categories.Physical
            Me.ContestCategory = ContestCategories.Tough
            Me.Name = Localization.GetString("move_name_" & Me.ID,"Fire Punch")
            Me.Description = "The target is punched with a fiery fist. It may also leave the target with a burn."
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
            Me.KingsrockAffected = False
            Me.CounterAffected = True

            Me.DisabledWhileGravity = False
            Me.UseEffectiveness = True
            Me.ImmunityAffected = True
            Me.RemovesOwnFrozen = False
            Me.HasSecondaryEffect = True

            Me.IsHealingMove = False
            Me.IsRecoilMove = False
            Me.IsPunchingMove = True
            Me.IsDamagingMove = True
            Me.IsProtectMove = False


            Me.IsAffectedBySubstitute = True
            Me.IsOneHitKOMove = False
            Me.IsWonderGuardAffected = True
            '#End

            Me.AIField1 = AIField.Damage
            Me.AIField2 = AIField.CanBurn

            Me.EffectChances.Add(10)
        End Sub

        Public Overrides Sub MoveHits(own As Boolean, BattleScreen As BattleScreen)
            Dim p As Pokemon = BattleScreen.OwnPokemon
            Dim op As Pokemon = BattleScreen.OppPokemon
            If own = False Then
                p = BattleScreen.OppPokemon
                op = BattleScreen.OwnPokemon
            End If

            Dim chance As Integer = GetEffectChance(0, own, BattleScreen)
            If Core.Random.Next(0, 100) < chance Then
                BattleScreen.Battle.InflictBurn(Not own, own, BattleScreen, "", "move:firepunch")
            End If
        End Sub

        Public Overrides Sub InternalOpponentPokemonMoveAnimation(ByVal BattleScreen As BattleScreen, ByVal BattleFlip As Boolean, ByVal CurrentPokemon As Pokemon, ByVal CurrentEntity As NPC)
            Dim MoveAnimation = New AnimationQueryObject(CurrentEntity, BattleFlip)

            Dim maxAmount As Integer = 12
            Dim currentAmount As Integer = 0
            While currentAmount <= maxAmount
                Dim Texture As Texture2D = TextureManager.GetTexture("Textures\Battle\Fire\Ember", New Rectangle(0, 64, 32, 32), "")
                Dim xDest = CSng((Random.NextDouble() - 0.5) * 1.5)
                Dim yDest = 0.25F
                Dim zDest = CSng((Random.NextDouble() - 0.5) * 1.5)

                Dim Destination As New Vector3(xDest, yDest, zDest)

                Dim Position As New Vector3(0, 0, 0)

                Dim Scale As New Vector3(0.35F)
                Dim startDelay As Double = 1.5 * Random.NextDouble()
                Dim FlameEntity As Entity = MoveAnimation.SpawnEntity(Position, Texture, Scale, 1.0F, CSng(startDelay))

                MoveAnimation.AnimationMove(FlameEntity, False, Destination.X, Destination.Y, Destination.Z, 0.02F, False, False, CSng(startDelay), 0.0F,,, 0.0075F)
                MoveAnimation.AnimationFade(FlameEntity, True, 0.4F, False, 0.0F, CSng(startDelay) + 1.5F, 0)
                Threading.Interlocked.Increment(currentAmount)
            End While

            Dim FistEntity = MoveAnimation.SpawnEntity(New Vector3(0, -0.2, 0), TextureManager.GetTexture("Textures\Battle\Fire\FirePunch_Fist"), New Vector3(0.5F), 0.0F, 0, 2)
            MoveAnimation.AnimationOscillateMove(FistEntity, False, New Vector3(0, 0.02, 0), 0.03, True, 7, 0, 0.5, 0, New Vector3(0, 1, 0))
            MoveAnimation.AnimationFade(FistEntity, True, 1.0F, False, 0.0F, 7, 0)
            MoveAnimation.AnimationPlaySound("Battle\Attacks\Fire\FirePunch", 0, 0)

            BattleScreen.BattleQuery.Add(MoveAnimation)
        End Sub
    End Class

End Namespace