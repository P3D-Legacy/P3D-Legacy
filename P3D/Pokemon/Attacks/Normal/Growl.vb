Namespace BattleSystem.Moves.Normal

    Public Class Growl

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Normal)
            Me.ID = 45
            Me.OriginalPP = 40
            Me.CurrentPP = 40
            Me.MaxPP = 40
            Me.Power = 0
            Me.Accuracy = 100
            Me.Category = Categories.Status
            Me.ContestCategory = ContestCategories.Cute
            Me.Name = "Growl"
            Me.Description = "The user growls in an endearing way, making the opposing team less wary. The foes' Attack stats are lowered."
            Me.CriticalChance = 0
            Me.IsHMMove = False
            Me.Target = Targets.AllAdjacentFoes
            Me.Priority = 0
            Me.TimesToAttack = 1
            '#End

            '#SpecialDefinitions
            Me.MakesContact = False
            Me.ProtectAffected = True
            Me.MagicCoatAffected = True
            Me.SnatchAffected = False
            Me.MirrorMoveAffected = True
            Me.KingsrockAffected = False
            Me.CounterAffected = False

            Me.DisabledWhileGravity = False
            Me.UseEffectiveness = False
            Me.ImmunityAffected = False
            Me.HasSecondaryEffect = False
            Me.RemovesFrozen = False

            Me.IsHealingMove = False
            Me.IsRecoilMove = False
            Me.IsPunchingMove = False
            Me.IsDamagingMove = False
            Me.IsProtectMove = False
            Me.IsSoundMove = True

            Me.IsAffectedBySubstitute = True
            Me.IsOneHitKOMove = False
            Me.IsWonderGuardAffected = False
            '#End

            Me.AIField1 = AIField.LowerAttack
            Me.AIField2 = AIField.Nothing
        End Sub

        Public Overrides Sub MoveHits(own As Boolean, BattleScreen As BattleScreen)
            Dim b As Boolean = BattleScreen.Battle.LowerStat(Not own, own, BattleScreen, "Attack", 1, "", "move:growl")
            If b = False Then
                BattleScreen.BattleQuery.Add(New TextQueryObject(Me.Name & " failed!"))
            End If
        End Sub

        Public Overrides Sub InternalUserPokemonMoveAnimation(ByVal BattleScreen As BattleScreen, ByVal own As Boolean, ByVal CurrentPokemon As Pokemon, ByVal CurrentEntity As NPC, ByVal CurrentModel As ModelEntity)
            Dim MoveAnimation As AnimationQueryObject = New AnimationQueryObject(CurrentEntity, own)
            MoveAnimation.PlaySound(CStr(CurrentPokemon.Number), 0, 0,, True)
            MoveAnimation.AnimationSpawnFadingEntity(0.25, -0.25, -0.25, "Textures\Battle\Normal\Growl,0,0,32,32", 0.5, 0.5, 0.5, 0.02, False, 1.0, 0, 1)
            MoveAnimation.AnimationSpawnFadingEntity(0.25, -0.25, -0.25, "Textures\Battle\Normal\Growl,0,32,32,32", 0.5, 0.5, 0.5, 0.02, False, 1.0, 1, 1)
            MoveAnimation.AnimationSpawnFadingEntity(0.25, -0.25, -0.25, "Textures\Battle\Normal\Growl,0,0,32,32", 0.5, 0.5, 0.5, 0.02, False, 1.0, 2, 1)
            MoveAnimation.AnimationSpawnFadingEntity(0.25, -0.25, -0.25, "Textures\Battle\Normal\Growl,0,32,32,32", 0.5, 0.5, 0.5, 0.02, False, 1.0, 3, 1)
            BattleScreen.BattleQuery.Add(MoveAnimation)
        End Sub
    End Class

End Namespace