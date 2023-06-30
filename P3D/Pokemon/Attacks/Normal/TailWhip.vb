Namespace BattleSystem.Moves.Normal

    Public Class TailWhip

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Normal)
            Me.ID = 39
            Me.OriginalPP = 30
            Me.CurrentPP = 30
            Me.MaxPP = 30
            Me.Power = 0
            Me.Accuracy = 100
            Me.Category = Categories.Status
            Me.ContestCategory = ContestCategories.Cute
            Me.Name = Localization.GetString("move_name_" & Me.ID,"Tail Whip")
            Me.Description = "The user wags its tail cutely, making opposing Pokémon less wary and lowering their Defense stat."
            Me.CriticalChance = 1
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
            Me.RemovesOwnFrozen = False

            Me.IsHealingMove = False
            Me.IsRecoilMove = False

            Me.IsDamagingMove = False
            Me.IsProtectMove = False


            Me.IsAffectedBySubstitute = True
            Me.IsOneHitKOMove = False
            Me.IsWonderGuardAffected = False
            '#End

            Me.AIField1 = AIField.LowerDefense
            Me.AIField2 = AIField.Nothing
        End Sub

        Public Overrides Sub MoveHits(own As Boolean, BattleScreen As BattleScreen)
            Dim b As Boolean = BattleScreen.Battle.LowerStat(Not own, own, BattleScreen, "Defense", 1, "", "move:tailwhip")
            If b = False Then
                BattleScreen.BattleQuery.Add(New TextQueryObject(Me.Name & " failed!"))
            End If
        End Sub

        Public Overrides Sub InternalUserPokemonMoveAnimation(ByVal BattleScreen As BattleScreen, ByVal BattleFlip As Boolean, ByVal CurrentPokemon As Pokemon, ByVal CurrentEntity As NPC)
            Dim MoveAnimation As AnimationQueryObject = New AnimationQueryObject(CurrentEntity, BattleFlip)
            MoveAnimation.AnimationTurnNPC(2, 0, 0, 1, -1)
            MoveAnimation.AnimationPlaySound("Battle\Attacks\Normal\TailWhip", 1, 0)
            MoveAnimation.AnimationMove(Nothing, False, 0, 0, -0.1, 0.025, False, False, 1, 0.5)
            MoveAnimation.AnimationMove(Nothing, False, 0, 0, 0.2, 0.025, False, False, 1.75, 0.5)
            MoveAnimation.AnimationMove(Nothing, False, 0, 0, -0.2, 0.025, False, False, 2.75, 0.5)
            MoveAnimation.AnimationMove(Nothing, False, 0, 0, 0.2, 0.025, False, False, 3.75, 0.5)
            MoveAnimation.AnimationMove(Nothing, False, 0, 0, -0.1, 0.025, False, False, 4.5, 0.5)
            MoveAnimation.AnimationTurnNPC(2, 5, 0, 3, 1)
            BattleScreen.BattleQuery.Add(MoveAnimation)
        End Sub
    End Class

End Namespace