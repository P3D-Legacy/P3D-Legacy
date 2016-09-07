Public Class HistoryScreen

    Public Class HistoryHandler

        Public Shared Sub AddHistoryItem(ByVal Name As String, ByVal Data As String, ByVal IsScriptOrigin As Boolean, ByVal ShowOnTimeline As Boolean)
            If Core.Player.HistoryData <> "" Then
                Core.Player.HistoryData &= vbNewLine
            End If

            Dim dateString As String = ""
            With My.Computer.Clock.LocalTime
                Dim hour As String = .Hour.ToString()
                If hour.Length = 1 Then
                    hour = "0" & hour
                End If
                Dim minute As String = .Minute.ToString()
                If minute.Length = 1 Then
                    minute = "0" & minute
                End If
                Dim second As String = .Second.ToString()
                If second.Length = 1 Then
                    second = "0" & second
                End If
                Dim day As String = .Day.ToString()
                If day.Length = 1 Then
                    day = "0" & day
                End If
                Dim month As String = .Month.ToString()
                If month.Length = 1 Then
                    month = "0" & month
                End If
                Dim year As String = .Year.ToString()

                dateString = day & "-" & month & "-" & year & "_" & hour & "." & minute & "." & second
            End With

            'Date|IsScriptOrigin|Name|Data|ShowOnTimeline
            Core.Player.HistoryData &= dateString & "|" & IsScriptOrigin.ToNumberString() & "|" & Name & "|" & Data & "|" & ShowOnTimeline.ToNumberString()
        End Sub

    End Class

End Class