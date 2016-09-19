Public Class ActionScript

    Public Scripts As New List(Of Script)

    Public Class ScriptLevel
        Public WaitingEndWhen As New List(Of Boolean)
        Public Switched As New List(Of Boolean)
        Public WaitingEndIf As New List(Of Boolean)
        Public CanTriggerElse As New List(Of Boolean)
        Public IfIndex As Integer
        Public WhenIndex As Integer

        Public WhileQuery As New List(Of Script)
        Public WhileQueryInitialized As Boolean = False

        Public ScriptVersion As Integer
        Public CurrentLine As Integer
        Public ScriptName As String
    End Class

    ''' <summary>
    ''' Returns the current ScriptLevel based on the script index.
    ''' </summary>
    Public Shared Function CSL() As ScriptLevel
        Return ScriptLevels(ScriptLevelIndex)
    End Function

    Public Shared ScriptLevels(99) As ScriptLevel
    Public Shared ScriptLevelIndex As Integer = -1

    Public reDelay As Single = 0.0F

    Dim LevelRef As Level

    Public Sub New(ByVal LevelRef As Level)
        Me.LevelRef = LevelRef
    End Sub

    Public Shared TempInputDirection As Integer = -1
    Public Shared TempSpin As Boolean = False

    Public Sub Update()
nextScript:
        Dim unlock As Boolean = Me.IsReady

        If Scripts.Count > 0 Then
            Scripts(0).Update()
        End If

        For i = 0 To Scripts.Count - 1
            If i <= Scripts.Count - 1 Then
                Dim s As Script = Scripts(i)

                If s.IsReady = True Then
                    i -= 1

                    Me.AddToWhileQuery(s)
                    Scripts.Remove(s)
                    ScriptLevels(s.Level).CurrentLine += 1

                    If IsReady = False And s.CanContinue = True Then
                        GoTo nextScript
                    End If
                End If
            End If
        Next

        If Me.IsReady = True Then
            If unlock = False Then
                Logger.Debug("Unlock Camera")
                CType(Screen.Camera, OverworldCamera).YawLocked = False
                CType(Screen.Camera, OverworldCamera).ResetCursor()
            End If
            If reDelay > 0.0F Then
                reDelay -= 0.1F

                If reDelay <= 0.0F Then
                    reDelay = 0.0F
                End If
            End If
        End If
    End Sub

    Public ReadOnly Property IsReady() As Boolean
        Get
            If Scripts.Count > 0 Then
                Return False
            Else
                Return True
            End If
        End Get
    End Property

    Public Shared IsInsightScript As Boolean = False

    ''' <summary>
    ''' Starts a script
    ''' </summary>
    ''' <param name="Input">The input string</param>
    ''' <param name="InputType">Type of information; 0: Script path, 1: Text, 2: Direct input</param>
    Public Sub StartScript(ByVal Input As String, ByVal InputType As Integer, Optional ByVal CheckDelay As Boolean = True, Optional ByVal ResetInsight As Boolean = True)
        ScriptLevelIndex += 1

        TempSpin = False

        Dim arr(99) As Boolean
        ScriptLevels(ScriptLevelIndex) = New ScriptLevel() With {.IfIndex = 0, .WhenIndex = 0, .ScriptVersion = 1, .WaitingEndIf = arr.ToList(), .WaitingEndWhen = arr.ToList(), .CurrentLine = 0, .ScriptName = "No script running", .CanTriggerElse = arr.ToList(), .Switched = arr.ToList()}

        Dim l As ScriptLevel = ScriptLevels(ScriptLevelIndex)

        If ResetInsight = True Then
            IsInsightScript = False
        End If

        If reDelay = 0.0F Or CheckDelay = False Then
            Select Case InputType
                Case 0 'Start script from file
                    Logger.Debug("Start script (ID: " & Input & ")")
                    l.ScriptName = "Type: Script; Input: " & Input

                    Dim path As String = GameModeManager.GetScriptPath(Input & ".dat")
                    Security.FileValidation.CheckFileValid(path, False, "ActionScript.vb")

                    If System.IO.File.Exists(path) = True Then

                        Dim Data As String = System.IO.File.ReadAllText(path)

                        Data = Data.Replace(vbNewLine, "^")
                        Dim ScriptData() As String = Data.Split(CChar("^"))

                        AddScriptLines(ScriptData)
                    Else
                        Logger.Log(Logger.LogTypes.ErrorMessage, "ActionScript.vb: The script file """ & path & """ doesn't exist!")
                    End If
                Case 1 'Display text
                    Logger.Debug("Start Script (Text: " & Input & ")")
                    l.ScriptName = "Type: Text; Input: " & Input

                    Dim Data As String = "version=2^@text.show(" & Input & ")^" & ":end"

                    Dim ScriptData() As String = Data.Split(CChar("^"))

                    AddScriptLines(ScriptData)
                Case 2 'Start script from direct input
                    Dim activator As String = Environment.StackTrace.Split(vbNewLine)(3)
                    activator = activator.Remove(activator.IndexOf("("))

                    Logger.Debug("Start Script (DirectInput; " & activator & ")")
                    l.ScriptName = "Type: Direct; Input: " & Input

                    Dim Data As String = Input.Replace(vbNewLine, "^")

                    Dim ScriptData() As String = Data.Split(CChar("^"))

                    AddScriptLines(ScriptData)
            End Select
        End If
    End Sub

    Private Sub AddScriptLines(ByVal ScriptData() As String)
        Dim i As Integer = 0
        Dim l As ScriptLevel = ScriptLevels(ScriptLevelIndex)
        For Each newScript As String In ScriptData
            If i = 0 And newScript.ToLower().StartsWith("version=") Then
                l.ScriptVersion = CInt(newScript.Remove(0, ("version=").Length))
                l.CurrentLine += 1
            Else
                While newScript.StartsWith(" ") = True Or newScript.StartsWith(vbTab) = True
                    newScript = newScript.Remove(0, 1)
                End While
                While newScript.EndsWith(" ") = True Or newScript.EndsWith(vbTab) = True
                    newScript = newScript.Remove(newScript.Length - 1, 1)
                End While
                If newScript <> "" Then
                    Me.Scripts.Insert(i, New Script(newScript, ScriptLevelIndex))
                    i += 1
                End If
            End If
        Next
    End Sub

    Public Sub Switch(ByVal Answer As Object)
        Dim l As ScriptLevel = ScriptLevels(ScriptLevelIndex)
        Dim proceed As Boolean = False
        Dim first As Boolean = True

        While proceed = False
            If Scripts.Count = 0 Then
                Logger.Log(Logger.LogTypes.Warning, "ActionScript.vb: Illegal "":when"" construct. Terminating execution.")
                Exit While
            End If

            Dim s As Script = Scripts(0)

            Select Case s.ScriptType
                Case Script.ScriptTypes.select
                    If first = False Then
                        l.WhenIndex += 1
                        l.WaitingEndWhen(l.WhenIndex) = True
                        l.Switched(l.WhenIndex) = True
                    End If
                Case Script.ScriptTypes.Command
                    If s.ScriptV2.Value.ToLower().StartsWith("options.show(") = True And first = False Then
                        l.WhenIndex += 1
                        l.WaitingEndWhen(l.WhenIndex) = True
                        l.Switched(l.WhenIndex) = True
                    End If
                Case Script.ScriptTypes.SwitchWhen, Script.ScriptTypes.when
                    If l.Switched(l.WhenIndex) = False Then
                        Dim equal As Boolean = False
                        Dim args() As String = Scripts(0).Value.Split(CChar(";"))

                        For Each arg As String In args
                            If (ScriptVersion2.ScriptComparer.EvaluateConstruct(arg).Equals(ScriptVersion2.ScriptComparer.EvaluateConstruct(Answer))) = True Then
                                equal = True
                                Exit For
                            End If
                        Next

                        If equal = True Then
                            l.WaitingEndWhen(l.WhenIndex) = False
                            proceed = True
                        Else
                            l.WaitingEndWhen(l.WhenIndex) = True
                        End If
                    End If
                Case Script.ScriptTypes.SwitchEndWhen, Script.ScriptTypes.endwhen
                    l.WaitingEndWhen(l.WhenIndex) = False
                    l.Switched(l.WhenIndex) = False
                    l.WhenIndex -= 1
                    If l.WaitingEndWhen(l.WhenIndex) = False Then
                        proceed = True
                    End If
            End Select

            Me.AddToWhileQuery(Scripts(0))
            Scripts.RemoveAt(0)
            l.CurrentLine += 1
            first = False
        End While
    End Sub

    Public Sub ChooseIf(ByVal T As Boolean)
        Dim l As ScriptLevel = ScriptLevels(ScriptLevelIndex)
        Dim proceed As Boolean = False

        While proceed = False
            If Scripts.Count = 0 Then
                Logger.Log(Logger.LogTypes.Warning, "ActionScript.vb: Illegal "":if"" construct. Terminating execution.")
                Exit While
            End If
            Dim s As Script = Scripts(0)

            Select Case s.ScriptType
                Case Script.ScriptTypes.if, Script.ScriptTypes.SwitchIf
                    l.IfIndex += 1
                    If l.WaitingEndIf(l.IfIndex - 1) = True Then
                        l.WaitingEndIf(l.IfIndex) = True
                        l.CanTriggerElse(l.IfIndex) = False
                    Else
                        If T = True Then
                            proceed = True
                            l.WaitingEndIf(l.IfIndex) = False
                            l.CanTriggerElse(l.IfIndex) = False
                        Else
                            l.WaitingEndIf(l.IfIndex) = True
                            l.CanTriggerElse(l.IfIndex) = True
                        End If
                    End If
                Case Script.ScriptTypes.else, Script.ScriptTypes.SwitchElse
                    If l.CanTriggerElse(l.IfIndex) = True Then
                        l.WaitingEndIf(l.IfIndex) = False
                        proceed = True
                    Else
                        l.WaitingEndIf(l.IfIndex) = True
                    End If
                Case Script.ScriptTypes.endif, Script.ScriptTypes.SwitchEndIf
                    l.IfIndex -= 1
                    If l.WaitingEndIf(l.IfIndex) = False Then
                        proceed = True
                    End If
            End Select

            Me.AddToWhileQuery(Scripts(0))
            Scripts.RemoveAt(0)
            l.CurrentLine += 1
        End While

        If Scripts.Count > 0 Then
            Scripts(0).Update()
        End If
    End Sub

    Public Sub AddToWhileQuery(ByVal RemovedScript As Script)
        If CSL().WhileQueryInitialized = True And CSL().ScriptVersion = 2 Then
            CSL().WhileQuery.Add(RemovedScript)

            If RemovedScript.ScriptV2.ScriptType = ScriptV2.ScriptTypes.endwhile Then
                Dim i As Integer = 0

                For Each s As Script In CSL().WhileQuery
                    Me.Scripts.Insert(i, s.Clone())
                    i += 1
                Next

                CSL().WhileQuery.Clear()
                CSL().WhileQueryInitialized = False
            End If
        End If
    End Sub

#Region "Registers"

    Public Shared Function IsRegistered(ByVal i As String) As Boolean
        CheckTimeBasedRegisters()
        If Core.Player.RegisterData.Contains(",") = True Then
            Dim Data() As String = Core.Player.RegisterData.Split(CChar(","))

            For Each d As String In Data
                If d.StartsWith("[") = True And d.EndsWith("]") = False And d.Contains("]") = True Then
                    d = d.Remove(0, d.IndexOf("]") + 1)
                    If d = i Then
                        Return True
                    End If
                Else
                    If d = i Then
                        Return True
                    End If
                End If
            Next

            Return False
        Else
            If Core.Player.RegisterData.StartsWith("[") = True And Core.Player.RegisterData.EndsWith("]") = False And Core.Player.RegisterData.Contains("]") = True Then
                Dim d As String = Core.Player.RegisterData.Remove(0, Core.Player.RegisterData.IndexOf("]") + 1)
                If d = i Then
                    Return True
                End If
            Else
                If Core.Player.RegisterData = i Then
                    Return True
                End If
            End If

            Return False
        End If
    End Function

    Private Shared Sub CheckTimeBasedRegisters()
        If Core.Player.RegisterData <> "" Then
            Dim Data As List(Of String) = {Core.Player.RegisterData}.ToList()
            If Core.Player.RegisterData.Contains(",") = True Then
                Data = Core.Player.RegisterData.Split(CChar(",")).ToList()
            End If

            Dim removedRegisters As Boolean = False

            For i = 0 To Data.Count - 1
                If i <= Data.Count - 1 Then
                    Dim d As String = Data(i)

                    If d.StartsWith("[TIME|") = True Then
                        Dim timeString As String = d.Remove(0, ("[TIME|").Length)
                        timeString = timeString.Remove(timeString.IndexOf("]"))

                        Dim timeData() As String = timeString.Split(CChar("|"))

                        Dim regDate As Date = UnixToTime(timeData(0))
                        Dim value As Integer = CInt(timeData(1))
                        Dim format As String = timeData(2)

                        Dim remove As Boolean = False

                        Select Case format
                            Case "days", "day"
                                If DateDiff(DateInterval.Day, regDate, Date.Now) >= value Then
                                    remove = True
                                End If
                            Case "minutes", "minute"
                                If DateDiff(DateInterval.Minute, regDate, Date.Now) >= value Then
                                    remove = True
                                End If
                            Case "seconds", "second"
                                If DateDiff(DateInterval.Second, regDate, Date.Now) >= value Then
                                    remove = True
                                End If
                            Case "years", "year"
                                If CInt(Math.Floor(DateDiff(DateInterval.Day, regDate, Date.Now) / 365)) >= value Then
                                    remove = True
                                End If
                            Case "weeks", "week"
                                If CInt(Math.Floor(DateDiff(DateInterval.Day, regDate, Date.Now) / 7)) >= value Then
                                    remove = True
                                End If
                            Case "months", "month"
                                If DateDiff(DateInterval.Month, regDate, Date.Now) >= value Then
                                    remove = True
                                End If
                            Case "hours", "hour"
                                If DateDiff(DateInterval.Hour, regDate, Date.Now) >= value Then
                                    remove = True
                                End If
                            Case "dayofweek"
                                If DateDiff(DateInterval.Weekday, regDate, Date.Now) >= value Then
                                    remove = True
                                End If
                        End Select

                        If remove = True Then
                            Data.RemoveAt(i)
                            i -= 1
                            removedRegisters = True
                        End If
                    End If
                End If
            Next

            If removedRegisters = True Then
                Dim s As String = ""

                If Data.Count > 0 Then
                    For Each d As String In Data
                        If s <> "" Then
                            s &= ","
                        End If
                        s &= d
                    Next
                End If

                Core.Player.RegisterData = s
            End If
        End If
    End Sub

    Public Shared Function UnixToTime(ByVal strUnixTime As String) As Date
        UnixToTime = DateAdd(DateInterval.Second, Val(strUnixTime), #1/1/1970#)
        If UnixToTime.IsDaylightSavingTime = True Then
            UnixToTime = DateAdd(DateInterval.Hour, 1, UnixToTime)
        End If
    End Function

    Public Shared Function TimeToUnix(ByVal dteDate As Date) As String
        If dteDate.IsDaylightSavingTime = True Then
            dteDate = DateAdd(DateInterval.Hour, -1, dteDate)
        End If
        TimeToUnix = DateDiff(DateInterval.Second, #1/1/1970#, dteDate).ToString()
    End Function

    Public Shared Sub RegisterID(ByVal i As String)
        Dim Data As String = Core.Player.RegisterData

        If Data = "" Then
            Data = i
        Else
            Dim checkData() As String = Data.Split(CChar(","))
            If checkData.Contains(i) = False Then
                Data &= "," & i
            End If
        End If

        Core.Player.RegisterData = Data
    End Sub

    Public Shared Sub RegisterID(ByVal name As String, ByVal type As String, ByVal value As String)
        Dim Data As String = Core.Player.RegisterData

        Dim reg As String = "[" & type.ToUpper() & "|" & value & "]" & name

        If Data = "" Then
            Data = reg
        Else
            Dim checkData() As String = Data.Split(CChar(","))
            If checkData.Contains(reg) = False Then
                Data &= "," & reg
            End If
        End If

        Core.Player.RegisterData = Data
    End Sub

    Public Shared Sub UnregisterID(ByVal i As String)
        Dim checkData() As String = Core.Player.RegisterData.Split(CChar(","))
        Dim Data As String = ""

        Dim checkList As List(Of String) = checkData.ToList()
        checkList.Remove(i)

        checkData = checkList.ToArray()
        For a = 0 To checkData.Count - 1
            If a <> 0 Then
                Data &= ","
            End If

            Data &= checkData(a)
        Next

        Core.Player.RegisterData = Data
    End Sub

    Public Shared Sub UnregisterID(ByVal name As String, ByVal type As String)
        Dim Data() As String = Core.Player.RegisterData.Split(CChar(","))
        Dim newData As String = ""

        For Each line As String In Data
            If line.StartsWith("[") = True And line.Contains("]") = True And line.EndsWith("]") = False Then
                Dim lName As String = line.Remove(0, line.IndexOf("]") + 1)
                Dim lType As String = line.Remove(0, 1)
                lType = lType.Remove(lType.IndexOf("|"))

                If lName <> name Or lType.ToLower() <> type.ToLower() Then
                    If newData <> "" Then
                        newData &= ","
                    End If
                    newData &= line
                End If
            Else
                If newData <> "" Then
                    newData &= ","
                End If
                newData &= line
            End If
        Next

        Core.Player.RegisterData = newData
    End Sub

    Public Shared Sub ChangeRegister(ByVal name As String, ByVal newValue As String)
        Dim Data() As String = Core.Player.RegisterData.Split(CChar(","))
        Dim newData As String = ""

        For Each line As String In Data
            If newData <> "" Then
                newData &= ","
            End If
            If line.StartsWith("[") = True And line.Contains("]") = True And line.EndsWith("]") = False Then
                Dim lName As String = line.Remove(0, line.IndexOf("]") + 1)
                Dim lType As String = line.Remove(0, 1)
                lType = lType.Remove(lType.IndexOf("|"))

                If lName.ToLower() = name.ToLower() Then
                    newData &= "[" & lType & "|" & newValue & "]" & name
                Else
                    newData &= line
                End If
            Else
                newData &= line
            End If
        Next

        Core.Player.RegisterData = newData
    End Sub

    ''' <summary>
    ''' Returns the Value and Type of a Register with value. {Value,Type}
    ''' </summary>
    ''' <param name="Name">The name of the register.</param>
    Public Shared Function GetRegisterValue(ByVal Name As String) As Object()
        Dim registers() As String = Core.Player.RegisterData.Split(CChar(","))
        For Each line As String In registers
            If line.StartsWith("[") = True And line.Contains("]") = True And line.EndsWith("]") = False Then
                Dim lName As String = line.Remove(0, line.IndexOf("]") + 1)

                If lName.ToLower() = Name.ToLower() Then
                    Dim lType As String = line.Remove(0, 1)
                    lType = lType.Remove(lType.IndexOf("|"))
                    Dim lValue As String = line.Remove(0, line.IndexOf("|") + 1)
                    lValue = lValue.Remove(lValue.IndexOf("]"))

                    Return {lValue, lType}
                End If
            End If
        Next

        Return {Nothing, Nothing}
    End Function

#End Region

End Class
