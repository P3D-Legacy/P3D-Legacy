Namespace BattleSystem.Moves.Ice

    Public Class IcePunch

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Ice)
            Me.ID = 8
            Me.OriginalPP = 15
            Me.CurrentPP = 15
            Me.MaxPP = 15
            Me.Power = 75
            Me.Accuracy = 100
            Me.Category = Categories.Physical
            Me.ContestCategory = ContestCategories.Beauty
            Me.Name = Localization.GetString("move_name_" & Me.ID,"Ice Punch")
            Me.Description = "The target is punched with an icy fist. It may also leave the target frozen."
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
            Me.HasSecondaryEffect = True
            Me.RemovesOwnFrozen = False

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
            Me.AIField2 = AIField.CanFreeze

            EffectChances.Add(10)
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
                BattleScreen.Battle.InflictFreeze(Not own, own, BattleScreen, "", "move:icepunch")
            End If
        End Sub

        Public Overrides Sub InternalOpponentPokemonMoveAnimation(ByVal BattleScreen As BattleScreen, ByVal BattleFlip As Boolean, ByVal CurrentPokemon As Pokemon, ByVal CurrentEntity As NPC)
            Dim MoveAnimation As AnimationQueryObject = New AnimationQueryObject(CurrentEntity, BattleFlip)
            MoveAnimation.AnimationPlaySound("Battle\Attacks\Ice\IcePunch_Crystals", 0, 0)
            Dim maxAmount As Integer = 10
            Dim currentAmount As Integer = 0
            While currentAmount <= maxAmount
                Dim Texture As Texture2D = TextureManager.GetTexture("Textures\Battle\Ice\IcePunch_Crystals", New Rectangle(0, 0, 16, 16), "")
                Dim xPos = CSng(Random.Next(-4, 4) / 8)
                Dim zPos = CSng(Random.Next(-4, 4) / 8)

                Dim Position As New Vector3(xPos, -0.25, zPos)
                Dim Destination As New Vector3(xPos - xPos * 2, 0, zPos - zPos * 2)
                Dim Scale As New Vector3(0.25F)
                Dim startDelay As Double = 5.0 * Random.NextDouble()
                Dim IceEntity = MoveAnimation.SpawnEntity(Position, Texture, Scale, 1.0F, CSng(startDelay))
                MoveAnimation.AnimationMove(IceEntity, False, Destination.X, Destination.Y, Destination.Z, 0.0125F, False, True, CSng(startDelay), 0.0F)
                MoveAnimation.AnimationChangeTexture(IceEntity, False, TextureManager.GetTexture("Textures\Battle\Ice\IcePunch_Crystals", New Rectangle(16, 0, 16, 16), ""), CSng(startDelay + 0.5), 0)
                MoveAnimation.AnimationChangeTexture(IceEntity, False, TextureManager.GetTexture("Textures\Battle\Ice\IcePunch_Crystals", New Rectangle(0, 0, 16, 16), ""), CSng(startDelay + 1), 0)
                MoveAnimation.AnimationChangeTexture(IceEntity, False, TextureManager.GetTexture("Textures\Battle\Ice\IcePunch_Crystals", New Rectangle(16, 0, 16, 16), ""), CSng(startDelay + 1.5), 0)
                MoveAnimation.AnimationRotate(IceEntity, True, 0, 0, 0.125, 0, 0, 3, CSng(startDelay), 0, False, False, True, False)

                Threading.Interlocked.Increment(currentAmount)
            End While
            Dim FistEntity = MoveAnimation.SpawnEntity(New Vector3(0, -0.2, 0), TextureManager.GetTexture("Textures\Battle\Ice\IcePunch_Fist"), New Vector3(0.5F), 1, 5, 3)
            MoveAnimation.AnimationPlaySound("Battle\Attacks\Ice\IcePunch_Fist", 5, 0)
            MoveAnimation.AnimationFade(FistEntity, True, 1.0F, False, 0.0F, 8, 0)

            BattleScreen.BattleQuery.Add(MoveAnimation)
        End Sub
    End Class

End Namespace