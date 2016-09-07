Public Class PlayerStatistics

    Shared Statistics As New Dictionary(Of String, Integer)

    Public Shared Sub Load(ByVal data As String)
        Statistics.Clear()
        For Each line As String In data.SplitAtNewline()
            If line.Contains(",") = True Then
                Dim statName As String = line.Remove(line.IndexOf(","))
                Dim statValue As Integer = CInt(line.Remove(0, line.IndexOf(",") + 1))

                If Statistics.ContainsKey(statName) = True Then
                    Statistics.Remove(statName)
                End If
                Statistics.Add(statName, statValue)
            End If
        Next
    End Sub

    Public Shared Sub Track(ByVal statName As String, ByVal addition As Integer)
        If Statistics.ContainsKey(statName) = True Then
            Dim currentValue As Integer = Statistics(statName)
            Statistics.Remove(statName)

            Statistics.Add(statName, currentValue + addition)
        Else
            Statistics.Add(statName, addition)
        End If

        If GameJolt.API.LoggedIn = True Then
            GameJolt.GameJoltStatistics.Track(statName, addition)
        End If
    End Sub

    Public Shared Function GetData() As String
        Dim s As String = ""
        For i = 0 To Statistics.Count - 1
            If s <> "" Then
                s &= vbNewLine
            End If
            s &= Statistics.Keys(i) & "," & Statistics.Values(i).ToString()
        Next
        Return s
    End Function

    Public Shared Function CountStatistics() As Integer
        Return Statistics.Count
    End Function

End Class