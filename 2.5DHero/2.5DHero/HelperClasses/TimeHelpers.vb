Public Class TimeHelpers

    ''' <summary>
    ''' Converts an amount of seconds to a TimeSpan.
    ''' </summary>
    ''' <param name="Seconds">The seconds to convert.</param>
    Public Shared Function ConvertSecondToTime(ByVal Seconds As Integer) As TimeSpan
        Dim minutes As Integer = 0
        Dim hours As Integer = 0

        While Seconds > 60
            minutes += 1
            Seconds -= 60

            If minutes > 60 Then
                minutes = 0
                hours += 1
            End If
        End While

        Dim reSpan As New TimeSpan(hours, minutes, Seconds)
        Return reSpan
    End Function

    ''' <summary>
    ''' Returns the amount of time the player has played.
    ''' </summary>
    Public Shared Function GetCurrentPlayTime() As TimeSpan
        Dim PTime As TimeSpan = Core.Player.PlayTime

        Dim diff As Integer = CInt(DateDiff(DateInterval.Second, Core.Player.GameStart, Date.Now))
        PTime += ConvertSecondToTime(diff)

        Return PTime
    End Function

    ''' <summary>
    ''' Returns the time to display as string.
    ''' </summary>
    ''' <param name="DateTime">The DateTime to display.</param>
    ''' <param name="ShowSeconds">To show the seconds or not.</param>
    Public Shared Function GetDisplayTime(ByVal DateTime As Date, ByVal ShowSeconds As Boolean) As String
        Return GetDisplayTime(New TimeSpan(DateTime.Hour, DateTime.Minute, DateTime.Second), ShowSeconds)
    End Function

    ''' <summary>
    ''' Returns the time to display as string.
    ''' </summary>
    ''' <param name="Time">The TimeSpan to display.</param>
    ''' <param name="ShowSeconds">To show the seconds or not.</param>
    Public Shared Function GetDisplayTime(ByVal Time As TimeSpan, ByVal ShowSeconds As Boolean) As String
        Dim days As Integer = Time.Days
        Dim hour As Integer = Time.Hours
        If days > 0 Then
            hour += days * 24
        End If

        Dim hours As String = hour.ToString()
        If hours.Length = 1 Then
            hours = "0" & hours
        End If
        Dim minutes As String = Time.Minutes.ToString()
        If minutes.Length = 1 Then
            minutes = "0" & minutes
        End If
        Dim seconds As String = Time.Seconds.ToString()
        If seconds.Length = 1 Then
            seconds = "0" & seconds
        End If

        Dim t As String = hours & ":" & minutes
        If ShowSeconds = True Then
            t &= "." & seconds
        End If
        Return t
    End Function

End Class
