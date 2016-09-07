Namespace BattleSystem.Moves.Normal

    Public Class Metronome

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Normal)
            Me.ID = 118
            Me.OriginalPP = 10
            Me.CurrentPP = 10
            Me.MaxPP = 10
            Me.Power = 0
            Me.Accuracy = 0
            Me.Category = Categories.Status
            Me.ContestCategory = ContestCategories.Cute
            Me.Name = "Metronome"
            Me.Description = "The user waggles a finger and stimulates its brain into randomly using nearly any move."
            Me.CriticalChance = 0
            Me.IsHMMove = False
            Me.Target = Targets.Self
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
            Me.ImmunityAffected = False
            Me.RemovesFrozen = False
            Me.HasSecondaryEffect = False

            Me.IsHealingMove = False
            Me.IsRecoilMove = False
            Me.IsPunchingMove = False
            Me.IsDamagingMove = False
            Me.IsProtectMove = False
            Me.IsSoundMove = False

            Me.IsAffectedBySubstitute = False
            Me.IsOneHitKOMove = False
            Me.IsWonderGuardAffected = False
            '#End
        End Sub

        Public Shared Function GetMetronomeMove() As Attack
            Dim moveID As Integer = Core.Random.Next(1, MOVE_COUNT + 1)

            Dim forbiddenIDs As List(Of Integer) = {68, 102, 119, 144, 165, 166, 168, 173, 182, 194, 197, 203, 214, 243, 264, 266, 267, 270, 271, 274, 289, 343, 364, 382, 383, 415, 448, 476, 469, 495, 501, 511, 516, 546, 547, 548, 553, 554, 555, 557}.ToList()

            While forbiddenIDs.Contains(moveID) = True
                moveID = Core.Random.Next(1, MOVE_COUNT + 1)
            End While

            Return Attack.GetAttackByID(moveID)
        End Function

    End Class

End Namespace