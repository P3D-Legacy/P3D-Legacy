Namespace BattleSystem

    Public Class ConfusionAttack

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Blank)
            Me.ID = 0
            Me.OriginalPP = 1
            Me.CurrentPP = 1
            Me.MaxPP = 1
            Me.Power = 40
            Me.Accuracy = -1
            Me.ContestCategory = ContestCategories.Tough
            Me.Name = Localization.GetString("move_name_" & Me.ID,"ConfusionAttack")
            Me.Description = "Hits to the face."
            Me.CriticalChance = 1
            Me.IsHMMove = False
            Me.Target = Targets.OneAdjacentTarget
            Me.Priority = 0
            Me.TimesToAttack = 1
            '#End

            '#SpecialDefinitions
            Me.MakesContact = False
            Me.ProtectAffected = False
            Me.MagicCoatAffected = False
            Me.SnatchAffected = False
            Me.MirrorMoveAffected = False
            Me.KingsrockAffected = False
            Me.CounterAffected = False
            Me.DisabledWhileGravity = False
            Me.UseEffectiveness = False
            Me.IsHealingMove = False
            Me.RemovesOwnFrozen = False
            Me.IsRecoilMove = False

            Me.ImmunityAffected = True
            Me.IsDamagingMove = True
            Me.IsProtectMove = False

            Me.HasSecondaryEffect = False
            Me.IsAffectedBySubstitute = False
            '#End
        End Sub

    End Class

End Namespace