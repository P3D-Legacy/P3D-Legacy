Namespace BattleSystem.Moves.Normal

    Public Class Scratch

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Normal)
            Me.ID = 10
            Me.OriginalPP = 35
            Me.CurrentPP = 35
            Me.MaxPP = 35
            Me.Power = 40
            Me.Accuracy = 100
            Me.Category = Categories.Physical
            Me.ContestCategory = ContestCategories.Tough
            Me.Name = "Scratch"
            Me.Description = "Hard, pointed, and sharp claws rake the target to inflict damage."
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
        End Sub

        Public Overrides Sub InternalOpponentPokemonMoveAnimation(ByVal BattleScreen As BattleScreen, ByVal BattleFlip As Boolean, ByVal CurrentPokemon As Pokemon, ByVal CurrentEntity As NPC)
            Dim MoveAnimation As AnimationQueryObject = New AnimationQueryObject(CurrentEntity, BattleFlip)
            MoveAnimation.AnimationPlaySound("Battle\Attacks\Normal\Scratch", 0.5, 2.5)
            Dim TextureXOffset As Integer = 0
            If BattleFlip = True Then
                TextureXOffset = 32
            End If
            Dim ScratchEntity = MoveAnimation.SpawnEntity(New Vector3(0, -0.2, 0), TextureManager.GetTexture("Textures\Battle\Normal\Scratch", New Rectangle(TextureXOffset, 0, 32, 32), ""), New Vector3(0.5F), 1, 0, 0.5)
            MoveAnimation.AnimationChangeTexture(ScratchEntity, False, TextureManager.GetTexture("Textures\Battle\Normal\Scratch", New Rectangle(TextureXOffset, 32, 32, 32), ""), 0.5, 0.5)
            MoveAnimation.AnimationChangeTexture(ScratchEntity, True, TextureManager.GetTexture("Textures\Battle\Normal\Scratch", New Rectangle(TextureXOffset, 64, 32, 32), ""), 1, 0.5)
            BattleScreen.BattleQuery.Add(MoveAnimation)
        End Sub

    End Class

End Namespace