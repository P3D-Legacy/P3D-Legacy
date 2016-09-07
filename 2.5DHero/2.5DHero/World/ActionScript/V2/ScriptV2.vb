Public Class ScriptV2

    Public Enum ScriptTypes As Integer
        Command = 100

        [if] = 101
        [when] = 102
        [then] = 103
        [else] = 104
        [endif] = 105
        [end] = 106
        [select] = 107
        [endwhen] = 108
        [return] = 109
        [endscript] = 110
        [while] = 111
        [endwhile] = 112
        [exitwhile] = 113

        Comment = 128
    End Enum

    Public ScriptType As ScriptTypes

    Public Value As String = ""

    Public started As Boolean = False
    Public IsReady As Boolean = False
    Public CanContinue As Boolean = True
    Public RawScriptLine As String = ""

    Public Sub Initialize(ByVal scriptLine As String)
        Me.RawScriptLine = scriptLine

        Dim firstChar As String = scriptLine(0)
        Select Case firstChar
            Case "@"
                Me.ScriptType = ScriptTypes.Command
                Me.Value = scriptLine.Remove(0, 1)
            Case ":"
                Dim structureType As String = scriptLine.Remove(0, 1)

                If structureType.Contains(":") = True Then
                    Me.Value = structureType.Remove(0, structureType.IndexOf(":") + 1)
                    structureType = structureType.Remove(structureType.IndexOf(":"))
                End If

                Select Case structureType.ToLower()
                    Case "if"
                        Me.ScriptType = ScriptTypes.if
                    Case "when"
                        Me.ScriptType = ScriptTypes.when
                    Case "then"
                        Me.ScriptType = ScriptTypes.then
                    Case "else"
                        Me.ScriptType = ScriptTypes.else
                    Case "endif"
                        Me.ScriptType = ScriptTypes.endif
                    Case "end"
                        Me.ScriptType = ScriptTypes.end
                    Case "select"
                        Me.ScriptType = ScriptTypes.select
                    Case "endwhen"
                        Me.ScriptType = ScriptTypes.endwhen
                    Case "return"
                        Me.ScriptType = ScriptTypes.return
                    Case "endscript"
                        Me.ScriptType = ScriptTypes.endscript
                    Case "while"
                        Me.ScriptType = ScriptTypes.while
                    Case "endwhile"
                        Me.ScriptType = ScriptTypes.endwhile
                    Case "exitwhile"
                        Me.ScriptType = ScriptTypes.exitwhile
                    Case Else
                        LogIllegalLine(scriptLine)
                        Me.IsReady = True
                End Select
                Me.CanContinue = True
            Case "#"
                Me.ScriptType = ScriptTypes.Comment
                Me.Value = scriptLine.Remove(0, 1)
                Me.CanContinue = True
            Case Else
                LogIllegalLine(scriptLine)
                Me.IsReady = True
                Me.CanContinue = True
        End Select
    End Sub

    Private Sub LogIllegalLine(ByVal scriptLine As String)
        Logger.Log(Logger.LogTypes.Message, "Illegal script line detected (" & scriptLine & ")")
    End Sub

    Public Sub EndScript(ByVal forceEnd As Boolean)
        ActionScript.ScriptLevelIndex -= 1
        If ActionScript.ScriptLevelIndex = -1 Or forceEnd = True Then
            ActionScript.ScriptLevelIndex = -1
            Dim oS As OverworldScreen = CType(Core.CurrentScreen, OverworldScreen)
            oS.ActionScript.Scripts.Clear()
            oS.ActionScript.reDelay = 1.0F
            Me.IsReady = True
            Screen.TextBox.reDelay = 1.0F
            ActionScript.TempInputDirection = -1
            ActionScript.TempSpin = False
        End If
    End Sub

    Public Sub Update()
        Select Case Me.ScriptType
            Case ScriptTypes.Command
                Me.DoCommand()

            Case ScriptTypes.if
                Me.DoIf()
            Case ScriptTypes.then
                Me.IsReady = True
            Case ScriptTypes.else
                Dim oS As OverworldScreen = CType(Core.CurrentScreen, OverworldScreen)
                oS.ActionScript.ChooseIf(True)

                Me.IsReady = True
            Case ScriptTypes.endif
                Dim oS As OverworldScreen = CType(Core.CurrentScreen, OverworldScreen)
                oS.ActionScript.ChooseIf(True)

                Me.IsReady = True
            Case ScriptTypes.while
                Me.DoWhile()
            Case ScriptTypes.endwhile
                Me.IsReady = True
            Case ScriptTypes.exitwhile
                Me.DoExitWhile()

            Case ScriptTypes.select
                Me.DoSelect()
            Case ScriptTypes.when
                Dim oS As OverworldScreen = CType(Core.CurrentScreen, OverworldScreen)
                oS.ActionScript.Switch("")
                Me.IsReady = True

            Case ScriptTypes.endwhen
                Dim oS As OverworldScreen = CType(Core.CurrentScreen, OverworldScreen)
                oS.ActionScript.Switch("")
                Me.IsReady = True

            Case ScriptTypes.end
                Me.EndScript(False)
            Case ScriptTypes.endscript
                Me.EndScript(True)

            Case ScriptTypes.return
                Me.DoReturn()
                Me.EndScript(False)

            Case ScriptTypes.Comment
                Logger.Debug("ScriptV2.vb: #Comment: """ & Me.Value & """")
                Me.IsReady = True
        End Select
    End Sub

    Private Sub DoWhile()
        'if T equals false, go straight to :endwhile and clear any existing while query, else initialize a while query that collects every single script line that gets executed starting with :while and ending with :endwhile
        'a exitwhile directly goes to :endwhile and clears the query
        Dim T As Boolean = CheckCondition()

        If T = True Then
            ActionScript.CSL().WhileQuery.Clear()
            ActionScript.CSL().WhileQueryInitialized = True
        Else
            ActionScript.CSL().WhileQuery.Clear()
            ActionScript.CSL().WhileQueryInitialized = False

            'Skip to endwhile:

            Dim oS As OverworldScreen = CType(Core.CurrentScreen, OverworldScreen)
            While oS.ActionScript.Scripts.Count > 0 AndAlso oS.ActionScript.Scripts(0).ScriptV2.ScriptType <> ScriptTypes.endwhile
                oS.ActionScript.Scripts.RemoveAt(0)
            End While
        End If

        Me.IsReady = True
    End Sub

    Private Sub DoExitWhile()
        ActionScript.CSL().WhileQuery.Clear()
        ActionScript.CSL().WhileQueryInitialized = True

        'Skip to endwhile:

        Dim oS As OverworldScreen = CType(Core.CurrentScreen, OverworldScreen)
        While oS.ActionScript.Scripts.Count > 0 AndAlso oS.ActionScript.Scripts(0).ScriptV2.ScriptType <> ScriptTypes.endwhile
            oS.ActionScript.Scripts.RemoveAt(0)
        End While

        Me.IsReady = True
    End Sub

    Private Sub DoIf()
        Dim T As Boolean = CheckCondition()

        ActionScript.CSL().WaitingEndIf(ActionScript.CSL().IfIndex + 1) = False
        ActionScript.CSL().CanTriggerElse(ActionScript.CSL().IfIndex + 1) = False

        Dim oS As OverworldScreen = CType(Core.CurrentScreen, OverworldScreen)

        oS.ActionScript.ChooseIf(T)

        Me.IsReady = True
    End Sub

    Private Function CheckCondition() As Boolean
        Dim check As String = Value
        Dim T As Boolean = False
        Dim convertNextValue As Boolean = False

        Dim ors As New List(Of List(Of String))

        Dim currentOr As New List(Of String)
        While check.Contains(" <and>") = True Or check.Contains(" <or> ") = True
            If check.StartsWith(" <and> ") = True Then
                check = check.Remove(0, " <and> ".Length)
            ElseIf check.StartsWith(" <or> ") = True Then
                Dim newOr As New List(Of String)
                newOr.AddRange(currentOr.ToArray())

                ors.Add(newOr)
                currentOr = New List(Of String)

                check = check.Remove(0, " <or> ".Length)
            Else
                If check.StartsWith("<not><") = True Then
                    convertNextValue = True
                    check = check.Remove(0, "<not>".Length)
                End If

                Dim nextStop As Integer = 0

                Dim andStop As Integer = -1
                Dim orStop As Integer = -1

                If check.Contains(" <and> ") = True Then
                    andStop = check.IndexOf(" <and> ")
                End If
                If check.Contains(" <or> ") = True Then
                    orStop = check.IndexOf(" <or> ")
                End If

                If andStop > -1 And orStop = -1 Then
                    nextStop = andStop
                ElseIf orStop > -1 And andStop = -1 Then
                    nextStop = orStop
                Else
                    If andStop < orStop Then
                        nextStop = andStop
                    Else
                        nextStop = orStop
                    End If
                End If

                Dim newCheck As String = check.Remove(nextStop)

                If convertNextValue = True Then
                    convertNextValue = False
                    newCheck = "<not>" & newCheck
                End If

                currentOr.Add(newCheck)

                check = check.Remove(0, nextStop)
            End If
        End While
        currentOr.Add(check)

        ors.Add(currentOr)

        Dim results As New List(Of Boolean)
        For Each checkOR As List(Of String) In ors
            Dim vT As Boolean = True
            For Each c As String In checkOR
                Dim b As Boolean = False
                If c.StartsWith("<not>") = True Then
                    c = c.Remove(0, "<not>".Length)
                    b = True
                End If
                Dim v As Boolean = ScriptVersion2.ScriptComparer.EvaluateScriptComparison(c)
                If b = True Then
                    b = False
                    v = Not v
                End If
                If v = False Then
                    vT = False
                    Exit For
                End If
            Next
            results.Add(vT)
        Next

        For Each result As Boolean In results
            If result = True Then
                T = True
                Exit For
            End If
        Next

        Return T
    End Function

    Private Sub DoSelect()
        ActionScript.CSL().WhenIndex += 1

        Dim oS As OverworldScreen = CType(Core.CurrentScreen, OverworldScreen)
        oS.ActionScript.Switch(ScriptVersion2.ScriptComparer.EvaluateConstruct(Value))

        Me.IsReady = True
    End Sub

    Private Sub DoCommand()
        ScriptVersion2.ScriptCommander.ExecuteCommand(Me, Me.Value)
    End Sub

    Public Shared TempReturn As String = "NULL"

    Private Sub DoReturn()
        TempReturn = ScriptVersion2.ScriptComparer.EvaluateConstruct(Me.Value).ToString()
    End Sub

End Class