Namespace BattleSystem.Moves.Normal

    Public Class Acupressure

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Normal)
            Me.ID = 367
            Me.OriginalPP = 30
            Me.CurrentPP = 30
            Me.MaxPP = 30
            Me.Power = 0
            Me.Accuracy = 0
            Me.Category = Categories.Status
            Me.ContestCategory = ContestCategories.Cool
            Me.Name = "Acupressure"
            Me.Description = "The user applies pressure to stress points, sharply boosting one of its or its allies' stats."
            Me.CriticalChance = 0
            Me.IsHMMove = False
            Me.Target = Targets.Self
            Me.Priority = 0
            Me.TimesToAttack = 1
            '#End

            '#SpecialDefinitions
            Me.MakesContact = False
            Me.ProtectAffected = False
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
            Me.IsSoundMove = False

            Me.IsAffectedBySubstitute = False
            Me.IsOneHitKOMove = False
            Me.IsWonderGuardAffected = False
            Me.UseAccEvasion = False
            '#End

            Me.AIField1 = AIField.RaiseAttack
            Me.AIField2 = AIField.RaiseSpAttack
        End Sub

        Public Overrides Sub MoveHits(own As Boolean, BattleScreen As BattleScreen)
            Dim p As Pokemon = BattleScreen.OwnPokemon
            If own = False Then
                p = BattleScreen.OppPokemon
            End If

            Dim statstoboost As New List(Of String)
            If p.StatAttack < 6 Then
                statstoboost.Add("Attack")
            End If

            If p.StatDefense < 6 Then
                statstoboost.Add("Defense")
            End If

            If p.StatSpAttack < 6 Then
                statstoboost.Add("Special Attack")
            End If

            If p.StatSpDefense < 6 Then
                statstoboost.Add("Special Defense")
            End If

            If p.StatSpeed < 6 Then
                statstoboost.Add("Speed")
            End If

            If p.Accuracy < 6 Then
                statstoboost.Add("Accuracy")
            End If

            If p.Evasion < 6 Then
                statstoboost.Add("Evasion")
            End If

            If statstoboost.Count = 0 Then
                BattleScreen.BattleQuery.Add(New TextQueryObject(Me.Name & " failed!"))
            Else
                BattleScreen.Battle.RaiseStat(own, own, BattleScreen, statstoboost(Core.Random.Next(0, statstoboost.Count)), 2, "", "move:acupressure")
            End If
        End Sub

    End Class

End Namespace