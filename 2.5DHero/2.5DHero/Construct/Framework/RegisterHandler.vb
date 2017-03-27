Imports System.Text.RegularExpressions

Namespace Construct.Framework

    ''' <summary>
    ''' Handles game registers.
    ''' </summary>
    Public Class RegisterHandler

        'Before, register could be one of the following:
        '    - just a name that got saved to save a booleanic state
        '    - a value store that stays active over a game save. Format: [DATATYPE|VALUE]Registername
        '    - a time based register that gets removed once the time runs out. Format: [TIME|registerDate|TimeDiff|TimeFormat]RegisterName
        '    - All register are separated by commas.
        '
        'New register format:
        '    - All registers start with a name.
        '    - Registers are either VALUE or TIME based.
        '    - If registers are time based, they got registerDate, TimeDiff and TimeFormat as arguments.
        '    - If registers are value based, they got their value.
        '    - Value based registers do not save their value type anymore.
        '    - All registers are separated by new lines.
        '
        'Example:
        '   Value based (empty value):  register1|VALUE|
        '   Value based (with value):   register2|VALUE|Value
        '   Time based:                 register3|TIME|<unixtimestamp>|2|Day
        '
        'If a value request is issued for a time based register, return the time data (everything behind |TIME|).

        Const REGISTER_NAME_REGEX As String = "^[a-z0-9A-Z_/\\\. \-]+$"

        ''' <summary>
        ''' Just a shortener for the name.
        ''' </summary>
        ''' <returns></returns>
        Private Shared Property RegisterData() As String
            Get
                Return Game.Core.Player.RegisterData
            End Get
            Set(value As String)
                Game.Core.Player.RegisterData = value
            End Set
        End Property

        ''' <summary>
        ''' Creates a new VALUE type register.
        ''' </summary>
        ''' <param name="registerName">The name of the new register.</param>
        ''' <param name="registerValue">The value of the new register.</param>
        Public Shared Sub NewRegister(ByVal registerName As String, ByVal registerValue As String)
            If IsValidName(registerName) = True Then
                CheckTimeRegisters()

                If IsRegistered(registerName) = False Then
                    'If the register does not exist yet, create it.
                    If RegisterData.Length > 0 Then
                        RegisterData &= vbNewLine
                    End If
                    RegisterData &= registerName & "|VALUE|" & registerValue
                Else
                    'If the register exists already, change its value.
                    SetRegisterValue(registerName, registerValue)
                End If
            Else
                Log_InvalidRegisterName(registerName)
            End If
        End Sub

        ''' <summary>
        ''' Creates a new TIME type register.
        ''' </summary>
        ''' <param name="registerName">The name of the new register.</param>
        ''' <param name="timeUnit">The unit of time.</param>
        ''' <param name="timeValue">The amount of time.</param>
        Public Shared Sub NewRegister(ByVal registerName As String, ByVal timeUnit As String, ByVal timeValue As String)
            If IsValidName(registerName) = True Then
                CheckTimeRegisters()

                If IsRegistered(registerName) = False Then
                    'Gets the current UNIX Timestamp:
                    Dim unixTime As Double = Math.Floor((Date.Now - New Date(1970, 1, 1, 0, 0, 0)).TotalSeconds)

                    If RegisterData.Length > 0 Then
                        RegisterData &= vbNewLine
                    End If
                    RegisterData &= registerName & "|TIME|" & unixTime.ToString() & "|" & timeValue & "|" & timeUnit
                Else
                    'If the register exists already, change its value.
                    'Gets the current UNIX Timestamp:
                    Dim unixTime As Double = Math.Floor((Date.Now - New Date(1970, 1, 1, 0, 0, 0)).TotalSeconds)
                    SetRegisterValue(registerName, unixTime.ToString() & "|" & timeValue & "|" & timeUnit)
                End If
            Else
                Log_InvalidRegisterName(registerName)
            End If
        End Sub

        ''' <summary>
        ''' Removes a register from the player's register.
        ''' </summary>
        ''' <param name="registerName">The name of the register to remove.</param>
        Public Shared Sub RemoveRegister(ByVal registerName As String)
            Dim data As List(Of String) = RegisterData.SplitAtNewline().ToList()
            Dim resultSB As New Text.StringBuilder()

            For i = 0 To data.Count - 1
                If data(i).ToLower().StartsWith(registerName.ToLower() & "|") = False Then
                    If resultSB.Length > 0 Then
                        resultSB.AppendLine()
                    End If
                    resultSB.Append(data(i))
                End If
            Next
            RegisterData = resultSB.ToString()
        End Sub

        ''' <summary>
        ''' Returns the value stored for a register.
        ''' </summary>
        ''' <param name="registerName">The register name.</param>
        ''' <returns></returns>
        Public Shared Function GetRegisterValue(ByVal registerName As String) As String
            If IsValidName(registerName) = True Then
                CheckTimeRegisters()

                Dim data As List(Of String) = RegisterData.SplitAtNewline().ToList()
                For i = 0 To data.Count - 1
                    If data(i).ToLower().StartsWith(registerName.ToLower() & "|") = True Then
                        Dim registerData As String() = data(i).Split(CChar("|"))

                        If registerData(1).ToLower() = "time" Then
                            'If the requsted register is a time register, return the time data:
                            Return registerData(2) & "|" & registerData(3) & "|" & registerData(4)
                        Else
                            'Otherwise, return the normal value data:
                            Return registerData(2)
                        End If
                    End If
                Next
                Return Core.Null
            Else
                Log_InvalidRegisterName(registerName)
                Return Core.Null
            End If
        End Function

        ''' <summary>
        ''' Sets the value of a register.
        ''' </summary>
        ''' <param name="registerName">The name of the register.</param>
        ''' <param name="registerValue">The new value of the register.</param>
        Public Shared Sub SetRegisterValue(ByVal registerName As String, ByVal registerValue As String)
            Dim data As List(Of String) = RegisterData.SplitAtNewline().ToList()
            Dim resultSB As New Text.StringBuilder()

            For i = 0 To data.Count - 1
                If resultSB.Length > 0 Then
                    resultSB.AppendLine()
                End If

                If data(i).ToLower().StartsWith(registerName.ToLower()) = True Then
                    Dim regData As String() = data(i).Split(CChar("|"))
                    resultSB.Append(regData(0) & "|" & regData(1) & "|" & registerValue)
                Else
                    resultSB.Append(data(i))
                End If
            Next

            RegisterData = resultSB.ToString()
        End Sub

        ''' <summary>
        ''' Returns if a specific register is registered.
        ''' </summary>
        ''' <param name="registerName">The register name.</param>
        ''' <returns></returns>
        Public Shared Function IsRegistered(ByVal registerName As String) As Boolean
            If IsValidName(registerName) = True Then
                CheckTimeRegisters()

                If RegisterData.ToLower().Contains(registerName.ToLower() & "|") = False Then
                    Return False
                End If

                Dim data As List(Of String) = RegisterData.SplitAtNewline().ToList()
                For i = 0 To data.Count - 1
                    If data(i).ToLower().StartsWith(registerName.ToLower() & "|") = True Then
                        Return True
                    End If
                Next
                Return False
            Else
                Log_InvalidRegisterName(registerName)
                Return False
            End If
        End Function

        ''' <summary>
        ''' Checks if a string is a valid register name.
        ''' </summary>
        ''' <param name="registerName">The register name to check.</param>
        ''' <returns></returns>
        Private Shared Function IsValidName(ByVal registerName As String) As Boolean
            Return Regex.IsMatch(registerName, REGISTER_NAME_REGEX, RegexOptions.Multiline)
        End Function

        ''' <summary>
        ''' Checks for eventual time based registers that need to be removed and does so.
        ''' </summary>
        Private Shared Sub CheckTimeRegisters()
            Dim data As List(Of String) = RegisterData.SplitAtNewline().ToList()
            Dim resultSB As New Text.StringBuilder()

            For i = 0 To data.Count - 1
                If data(i).Contains("|") = True Then
                    Dim registerData As String() = data(i).Split(CChar("|"))
                    Dim remove As Boolean = False

                    If registerData(1).ToLower() = "time" Then
                        Dim registerDate As Date = New Date(1970, 1, 1, 0, 0, 0).AddSeconds(CDbl(registerData(2)))
                        Dim diffType As String = registerData(4)
                        Dim diff As Integer = CInt(registerData(3))

                        Select Case diffType.ToLower()
                            Case "years", "year"
                                If DateDiff(DateInterval.Year, registerDate, Date.Now) >= diff Then
                                    remove = True
                                End If
                            Case "months", "month"
                                If DateDiff(DateInterval.Month, registerDate, Date.Now) >= diff Then
                                    remove = True
                                End If
                            Case "weeks", "week"
                                If DateDiff(DateInterval.WeekOfYear, registerDate, Date.Now) >= diff Then
                                    remove = True
                                End If
                            Case "days", "day"
                                If DateDiff(DateInterval.Day, registerDate, Date.Now) >= diff Then
                                    remove = True
                                End If
                            Case "hours", "hour"
                                If DateDiff(DateInterval.Hour, registerDate, Date.Now) >= diff Then
                                    remove = True
                                End If
                            Case "minutes", "minute"
                                If DateDiff(DateInterval.Minute, registerDate, Date.Now) >= diff Then
                                    remove = True
                                End If
                            Case "seconds", "second"
                                If DateDiff(DateInterval.Second, registerDate, Date.Now) >= diff Then
                                    remove = True
                                End If
                            Case Else
                                remove = True
                        End Select
                    End If

                    If remove = False Then
                        If resultSB.Length > 0 Then
                            resultSB.AppendLine()
                        End If
                        resultSB.Append(data(i))
                    End If
                End If
            Next
            RegisterData = resultSB.ToString()
        End Sub

        Private Shared Sub Log_InvalidRegisterName(ByVal registerName As String)
            Logger.Debug("018", registerName & " is not a valid identifier for a register!")
        End Sub

        ''' <summary>
        ''' Converts the old register format into the new one.
        ''' </summary>
        ''' <param name="data">The old register data.</param>
        ''' <returns></returns>
        Public Shared Function ConvertFromOldRegisterFormat(ByVal data As String) As String
            'Determine, if the data IS in the old format, otherwise return the data directly:
            'This is done by checking if the data has only one line which contains a lot of data.
            'We just take a wild guess here, it's possible that there's only one register, which is a value register, which contains a "," in its data value. But what are the odds of that.

            Dim testData As String() = data.SplitAtNewline()
            Dim lineDataCount As Integer = 0 'How many lines actually contain data.
            Dim isOldFormat As Boolean = False

            For Each l As String In testData
                If l.Length > 0 Then
                    lineDataCount += 1
                End If
            Next
            If lineDataCount = 1 And testData(0).Length > 0 Then
                If testData(0).Contains(",") = True Then
                    isOldFormat = True
                End If
            End If

            'If it's the new format (not the old format), return the data input.
            If isOldFormat = False Then
                Return data
            End If

            Dim lines() = data.Split(CChar(","))
            Dim outputL As New List(Of String)

            For Each register As String In lines
                If register.StartsWith("[TIME|") = True And register.Contains("]") = True And register.EndsWith("]") = False Then
                    'Time based register
                    Dim definition As String = register.Remove(0, 1)
                    definition = definition.Remove(definition.IndexOf("]"))

                    Dim definitionData As String() = definition.Split(CChar("|"))

                    'proceed only when in the correct format
                    If definitionData.Length = 4 Then
                        Dim registerName As String = register.Remove(0, register.IndexOf("]") + 1)
                        outputL.Add(registerName & "|TIME|" & definitionData(1) & "|" & definitionData(2) & "|" & definitionData(3))
                    End If
                ElseIf register.StartsWith("[") = True And register.Contains("]") = True And register.Contains("|") = True And register.EndsWith("]") = False Then
                    'If it wasn't a time based register, it's a value register
                    Dim definition As String = register.Remove(0, 1)
                    definition = definition.Remove(definition.IndexOf("]"))

                    Dim definitionData As String() = definition.Split(CChar("|"))

                    'proceed only when in the correct format
                    If definitionData.Length = 2 Then
                        Dim registerName As String = register.Remove(0, register.IndexOf("]") + 1)

                        outputL.Add(registerName & "|VALUE|" & definitionData(1))
                    End If
                Else
                    'Normal register
                    outputL.Add(register & "|VALUE|")
                End If
            Next

            Dim output As String = ""
            For Each line As String In outputL
                If output.Length > 0 Then
                    output &= vbNewLine
                End If
                output &= line
            Next
            Logger.Log("183", Logger.LogTypes.Message, "Converted old register format to the new one.")
            Return output
        End Function

    End Class

End Namespace