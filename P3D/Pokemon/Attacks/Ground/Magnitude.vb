Namespace BattleSystem.Moves.Ground

    Public Class Magnitude

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Ground)
            Me.ID = 222
            Me.OriginalPP = 30
            Me.CurrentPP = 30
            Me.MaxPP = 30
            Me.Power = 0
            Me.Accuracy = 100
            Me.Category = Categories.Physical
            Me.ContestCategory = ContestCategories.Tough
            Me.Name = "Magnitude"
            Me.Description = "The user looses a ground- shaking quake affecting everyone around the user. Its power varies."
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
            Me.CanHitUnderground = True
            '#End
        End Sub

        Shared UsedLevel As Integer = 0

        Public Overrides Function GetBasePower(own As Boolean, BattleScreen As BattleScreen) As Integer
            Dim dig As Integer = BattleScreen.FieldEffects.OppDigCounter
            If own = False Then
                dig = BattleScreen.FieldEffects.OwnDigCounter
            End If

            Dim basepower As Integer = 0
            Dim magLevel As Integer = 4

            Dim r As Integer = Core.Random.Next(0, 100)
            If r < 5 Then
                basepower = 10
                magLevel = 4
            ElseIf r >= 5 And r < 15 Then
                basepower = 30
                magLevel = 5
            ElseIf r >= 15 And r < 35 Then
                basepower = 50
                magLevel = 6
            ElseIf r >= 35 And r < 65 Then
                basepower = 70
                magLevel = 7
            ElseIf r >= 65 And r < 85 Then
                basepower = 90
                magLevel = 8
            ElseIf r >= 85 And r < 95 Then
                basepower = 110
                magLevel = 9
            ElseIf r >= 95 Then
                basepower = 150
                magLevel = 10
            End If

            If dig > 0 Then
                basepower *= 2
            Else
                basepower *= 2
            End If

            UsedLevel = magLevel

            Return basepower
        End Function

        Public Overrides Sub MoveHits(own As Boolean, BattleScreen As BattleScreen)
            BattleScreen.BattleQuery.Add(New TextQueryObject("Magnitude " & UsedLevel.ToString() & "!"))
        End Sub

    End Class

End Namespace