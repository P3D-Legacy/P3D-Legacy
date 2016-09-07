Namespace GameJolt

    Public Class GameJoltStatistics

        Public Shared Sub CreateStatistics()
            Dim stats() As String = {"steps taken"} '{"Obtained BP", "Ride used", "Eggs hatched", "Evolutions", "Moves learned", "Caught Pokemon", "Blackouts", "[53]Status booster used", "[25]Vitamins used", "TMs/HMs used", "[17]Medicine Items used", "[22]Evolution stones used", "[42]Repels used", "Cut used", "Surf used", "Fly used", "Strength used", "Waterfall used", "Flash used", "Rock smash used", "Whirlpool used", "Items found", "GTS Trades", "Wondertrades", "Battle Spot battles", "Trades", "PVP battles", "[2006]Berries picked", "[85]Apricorns picked", "Moves learned", "[4]Poké Balls used", "Wild battles", "Trainer battles", "Safari battles", "Bug-Catching contest battles"}
            For i = 0 To stats.Count - 1
                Dim APICall As New APICall()
                APICall.SetStorageData(GetKey(stats(i)), "0", False)
            Next
        End Sub

        Shared ReadOnly IndicedStats() As String = {"pvp wins", "pvp losses"}
        Shared lastStepTime As Date = Date.Now
        Shared TempSteps As Integer = 0

        Public Shared Sub Track(ByVal statName As String, ByVal addition As Integer)
            If CanTrack(statName) = True Then
                Dim APICall As New APICall()

                If statName.ToLower() = "steps taken" Then
                    addition = TempSteps
                    TempSteps = 0
                End If

                APICall.UpdateStorageData(GetKey(statName), addition.ToString(), "add", False)
                Logger.Debug("Track online statistic: " & statName & " (" & addition.ToString() & ")")
            End If
        End Sub

        Private Shared Function CanTrack(ByVal statName As String) As Boolean
            If IndicedStats.Contains(statName.ToLower()) = False Then
                If statName.ToLower() = "steps taken" Then
                    TempSteps += 1
                    If DateDiff(DateInterval.Second, lastStepTime, Date.Now) >= 20 Then
                        lastStepTime = Date.Now
                        Return True
                    End If
                Else
                    Return True
                End If
            End If
            Return False
        End Function

        Public Shared Sub GetStatisticValue(ByVal statName As String, ByVal ResultFunction As APICall.DelegateCallSub)
            Dim APICall As New APICall(ResultFunction)
            APICall.GetStorageData(GetKey(statName), False)
        End Sub

        Private Shared Function GetKey(ByVal statName As String) As String
            Return "0GJSTAT_" & statName.ToLower()
        End Function

    End Class

End Namespace