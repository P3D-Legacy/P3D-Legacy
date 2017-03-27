Namespace Construct.Framework.Classes

    <ScriptClass("DateTime")>
    <ScriptDescription("A helper class to supply date and time information.")>
    Public Class CL_DateTime

        Inherits ScriptClass

        Private Function GetDate(ByVal argument As String) As Date
            If argument.Length > 0 Then
                Return New Date(1970, 1, 1, 0, 0, 0).AddSeconds(Converter.ToDouble(argument))
            Else
                Return Date.Now
            End If
        End Function

        <ScriptConstruct("Year")>
        <ScriptDescription("Returns the local time year.")>
        Private Function F_Year(ByVal argument As String) As String
            Return ToString(GetDate(argument).Year)
        End Function

        <ScriptConstruct("WeekOfYear")>
        <ScriptDescription("Returns the local time week of the year.")>
        Private Function F_WeekOfYear(ByVal argument As String) As String
            Return ToString(CInt(((GetDate(argument).DayOfYear - (GetDate(argument).DayOfWeek - 1)) / 7) + 1))
        End Function

        <ScriptConstruct("Month")>
        <ScriptDescription("Returns the local time month.")>
        Private Function F_Month(ByVal argument As String) As String
            Return ToString(GetDate(argument).Month)
        End Function

        <ScriptConstruct("DayOfYear")>
        <ScriptDescription("Returns the local time day of the year.")>
        Private Function F_DayOfYear(ByVal argument As String) As String
            Return ToString(GetDate(argument).DayOfYear)
        End Function

        <ScriptConstruct("DayOfWeek")>
        <ScriptDescription("Returns the local time day of the week.")>
        Private Function F_DayOfWeek(ByVal argument As String) As String
            Return GetDate(argument).DayOfWeek.ToString()
        End Function

        <ScriptConstruct("Day")>
        <ScriptDescription("Returns the local time day.")>
        Private Function F_Day(ByVal argument As String) As String
            Return ToString(GetDate(argument).Day)
        End Function

        <ScriptConstruct("Hour")>
        <ScriptDescription("Returns the local time hour.")>
        Private Function F_Hour(ByVal argument As String) As String
            Return ToString(GetDate(argument).Hour)
        End Function

        <ScriptConstruct("Minute")>
        <ScriptDescription("Returns the local time minute.")>
        Private Function F_Minute(ByVal argument As String) As String
            Return ToString(GetDate(argument).Minute)
        End Function

        <ScriptConstruct("Second")>
        <ScriptDescription("Returns the local time second.")>
        Private Function F_Second(ByVal argument As String) As String
            Return ToString(GetDate(argument).Second)
        End Function

        <ScriptConstruct("Now")>
        <ScriptDescription("Returns a UNIX time stamp of the local time with precision to the second.")>
        Private Function F_Now(ByVal argument As String) As String
            Dim unixTime As Double = Math.Floor((Date.Now - New Date(1970, 1, 1, 0, 0, 0)).TotalSeconds)
            Return ToString(unixTime)
        End Function

    End Class

End Namespace