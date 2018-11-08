Namespace BattleSystem.Moves.Normal

    Public Class RevelationDance

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Normal)
            Me.ID = 686
            Me.OriginalPP = 15
            Me.CurrentPP = 15
            Me.MaxPP = 15
            Me.Power = 90
            Me.Accuracy = 100
            Me.Category = Categories.Special
            Me.ContestCategory = ContestCategories.Beauty
            Me.Name = "Revelation Dance"
            Me.Description = "The user attacks the target by dancing very hard. The user's type determines the type of this move."
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
            Me.AIField2 = AIField.Nothing
        End Sub

        Public Overrides Function GetAttackType(own As Boolean, BattleScreen As BattleScreen) As Element
            Dim p As Pokemon = BattleScreen.OwnPokemon
            If own = False Then
                p = BattleScreen.OppPokemon
            End If

            Return New Element(p.Type1.Type)

            Return Me.Type
        End Function

    End Class

End Namespace