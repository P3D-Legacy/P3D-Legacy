Namespace Construct

    ''' <summary>
    ''' A class that represents a script file.
    ''' </summary>
    Public Class Script

        ''' <summary>
        ''' The origin type of this script.
        ''' </summary>
        Public Enum ScriptOriginTypes
            File
            [String]
            Text
        End Enum

        Private _lines As New List(Of ScriptLine)

        Private _scriptOriginType As ScriptOriginTypes

        Private _originalContent As String = ""
        Private _linePointer As Integer = 0 'Points to the active script line.

        Private _valueHandler As Framework.ValueHandler

        Private _originIdentifier As String = ""

        ''' <summary>
        ''' The Value Handler for this script.
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property ValueHandler() As Framework.ValueHandler
            Get
                Return _valueHandler
            End Get
        End Property

        ''' <summary>
        ''' The identifier of this script.
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property OriginIdentifier() As String
            Get
                Return _originIdentifier
            End Get
        End Property

        ''' <summary>
        ''' The origin type of this script.
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property ScriptOriginType() As ScriptOriginTypes
            Get
                Return _scriptOriginType
            End Get
        End Property

        ''' <summary>
        ''' Points to the active script line.
        ''' </summary>
        ''' <returns></returns>
        Public Property LinePointer() As Integer
            Get
                Return _linePointer
            End Get
            Set(value As Integer)
                _linePointer = value
            End Set
        End Property

        ''' <summary>
        ''' Returns the active script line.
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property ActiveLine() As ScriptLine
            Get
                If _linePointer < _lines.Count() Then
                    Return _lines(_linePointer)
                Else
                    Return Nothing
                End If
            End Get
        End Property

        ''' <summary>
        ''' If this script is completed.
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property IsReady() As Boolean
            Get
                Return _linePointer = _lines.Count()
            End Get
        End Property

        ''' <summary>
        ''' Creates a new instance of a script.
        ''' </summary>
        ''' <param name="scriptContent">The content of the script, separated by new lines.</param>
        ''' <param name="scriptOriginType">The origin of this script.</param>
        ''' <param name="originIdentifier">Identifies where this script came from.</param>
        Public Sub New(ByVal scriptContent As String, ByVal scriptOriginType As ScriptOriginTypes, ByVal originIdentifier As String)
            _scriptOriginType = scriptOriginType
            _originalContent = scriptContent
            _valueHandler = New Framework.ValueHandler()
            _originIdentifier = originIdentifier

            Dim lines As String() = scriptContent.SplitAtNewline()

            For i = 0 To lines.Length - 1
                _lines.Add(New ScriptLine(lines(i), Me))
            Next
        End Sub

        ''' <summary>
        ''' Executes this script until it reaches an end/exit or a line preserves its state.
        ''' </summary>
        Public Sub Execute()
            If _lines.Count > 0 Then
                While True
                    If ActiveLine Is Nothing Then
                        'Script ended, the pointer does not point to a valid script line anymore:
                        Exit While
                    Else
                        If ActiveLine.IsValid = True Then

                            'Check if the line is inside an If block that is not to be executed:
                            If IsInValidIfBranch() = True And IsInValidWhenBranch() = True And IsInValidWhileBranch() = True Then
                                ActiveLine.Execute()
                                If ActiveLine.Preserve = False Then
                                    Dim skipAfterLine As Boolean = ActiveLine.EndExecutionFrame

                                    'Go to next line:
                                    IncrementLinePointer()

                                    'Last line this frame:
                                    If skipAfterLine = True Then
                                        Exit While
                                    End If
                                Else
                                    'Exit the execution of this frame, because the current line is preserving its state.
                                    Exit While
                                End If
                            Else
                                'Just go to the next line if the current line is in an invalid if/when/while block:
                                IncrementLinePointer()
                            End If
                        Else
                            'Ignore the invalid lines:
                            IncrementLinePointer()
                        End If
                    End If
                End While
            End If
        End Sub

        Private Sub IncrementLinePointer()
            If Not ActiveLine Is Nothing Then
                ActiveLine.Reset()
            End If
            _linePointer += 1
        End Sub

        ''' <summary>
        ''' Inserts a new script line at the current line pointer.
        ''' </summary>
        ''' <param name="newLineContent">The new script line content.</param>
        Public Sub InsertNewLine(ByVal newLineContent As String)
            _lines.Insert(_linePointer, New ScriptLine(newLineContent, Me))
        End Sub

#Region "ControlFlow"

        ''' <summary>
        ''' The current script line is a control structure.
        ''' </summary>
        Public Sub ReachedStructure()
            Select Case ActiveLine.LineType
                Case ScriptLine.LineTypes.If
                    ReachedIf()
                Case ScriptLine.LineTypes.ElseIf
                    ReachedElseIf()
                Case ScriptLine.LineTypes.Else
                    ReachedElse()
                Case ScriptLine.LineTypes.EndIf
                    ReachedEndIf()
                Case ScriptLine.LineTypes.Select
                    ReachedSelect()
                Case ScriptLine.LineTypes.When
                    ReachedWhen()
                Case ScriptLine.LineTypes.EndSelect
                    ReachedEndSelect()
                Case ScriptLine.LineTypes.While
                    ReachedWhile()
                Case ScriptLine.LineTypes.ExitWhile
                    ReachedExitWhile()
                Case ScriptLine.LineTypes.EndWhile
                    ReachedEndWhile()

                Case ScriptLine.LineTypes.End
                    ReachedEnd()
                Case ScriptLine.LineTypes.Exit
                    ReachedExit()
            End Select
        End Sub

#Region "If"

        Private IfStructs As New List(Of IfStruct)

        Private Class IfStruct

            Public Enum IfStates
                [If]
                [ElseIf]
                [Else]
            End Enum

            'The result of the initial If condition:
            Public InitialCondition As Boolean = False

            'If any ElseIf condition was entered:
            Public EnteredElseIf As Boolean = False

            'If an Else block has been found:
            Public EncounteredElse As Boolean = False

            'The current block:
            Public CurrentState As IfStates = IfStates.If

            'If the current block is valid. This only needs to be set if the first ElseIf is reached that is True when the If is false.
            Public CurrentElseIfBlockValid As Boolean = False

            'When the parent If statement is invalid, also skip this one.
            Public isInvalidFromParent As Boolean = False

            Public ReadOnly Property IsBranchValid() As Boolean
                Get
                    If isInvalidFromParent = True Then
                        Return False
                    End If
                    Select Case CurrentState
                        Case IfStates.If
                            Return InitialCondition = True
                        Case IfStates.ElseIf
                            Return CurrentElseIfBlockValid = True
                        Case IfStates.Else
                            Return InitialCondition = False And EnteredElseIf = False
                    End Select
                    Return False
                End Get
            End Property

        End Class

        ''' <summary>
        ''' The current script line is an If condition,
        ''' </summary>
        Private Sub ReachedIf()
            'Declare a new IfStruct that represents the If condition block:
            Dim n_ifStruct As New IfStruct()

            'Set if the initial If is true or false:
            n_ifStruct.CurrentState = IfStruct.IfStates.If
            n_ifStruct.InitialCondition = Framework.Parser.ConditionalComparison(ActiveLine.GetQuickValue(1))

            If IfStructs.Count > 0 Then
                If IfStructs.Last().IsBranchValid = False Then
                    n_ifStruct.isInvalidFromParent = True
                End If
            End If

            IfStructs.Add(n_ifStruct)
        End Sub

        ''' <summary>
        ''' The current script line is an elseif condition,
        ''' </summary>
        Private Sub ReachedElseIf()
            'If there is an IfStruct at the top, use it.
            If IfStructs.Count > 0 Then
                Dim n_ifStruct As IfStruct = IfStructs.Last()

                'If there has been an Else condition already, this ElseIf is misplaced, because the Else condition HAS to be the last branch of the If block:
                If n_ifStruct.EncounteredElse = False Then
                    'Set the current block to ElseIf:
                    n_ifStruct.CurrentState = IfStruct.IfStates.ElseIf

                    'Any ElseIf can only be entered if the initial If condition was False:
                    If n_ifStruct.InitialCondition = False Then
                        'Only ONE elseif can be entered per If block, so we check if this would be the first one:
                        If n_ifStruct.EnteredElseIf = False Then

                            'Now we evaluate the expression to check if this elseIf is true:
                            Dim elseIfResult As Boolean = Framework.Parser.ConditionalComparison(ActiveLine.GetQuickValue(1))

                            If elseIfResult = True Then
                                n_ifStruct.CurrentElseIfBlockValid = True
                                n_ifStruct.EnteredElseIf = True
                            Else
                                n_ifStruct.CurrentElseIfBlockValid = False
                            End If
                        End If
                    End If
                Else
                    Logger.Debug("010", "Invalid ""elseif"" structure after an ""else"" structure.")
                End If
            Else
                Logger.Debug("011", "Invalid ""elseif"" structure outside of an ""if"" structure.")
            End If
        End Sub

        ''' <summary>
        ''' The current script line is an else condition.
        ''' </summary>
        Private Sub ReachedElse()
            'If there is an IfStruct at the top, use it.
            If IfStructs.Count > 0 Then
                Dim n_ifStruct As IfStruct = IfStructs.Last()

                'There can only be one Else condition in an If block, so we have to have not encountered one yet:
                If n_ifStruct.EncounteredElse = False Then
                    'Set the current block to Else:
                    n_ifStruct.CurrentState = IfStruct.IfStates.Else
                    n_ifStruct.EncounteredElse = True
                Else
                    Logger.Debug("033", "Invalid ""else"" structure. There can only be one ""else"" structure in one ""if"" block.")
                End If
            Else
                Logger.Debug("034", "Invalid ""else"" structure outside of an ""if"" structure.")
            End If
        End Sub

        ''' <summary>
        ''' The current script line ends an if block.
        ''' </summary>
        Private Sub ReachedEndIf()
            'Remove the top ifstruct from the list.
            If IfStructs.Count > 0 Then
                IfStructs.Remove(IfStructs.Last())
            Else
                Logger.Debug("035", "Invalid ""endif"" structure outside of an ""if"" structure.")
            End If
        End Sub

        ''' <summary>
        ''' Returns True if the script is in a valid if script block branch right now OR not in any at all.
        ''' </summary>
        ''' <returns></returns>
        Private ReadOnly Property IsInValidIfBranch() As Boolean
            Get
                If IfStructs.Count = 0 Or ActiveLine.LineType = ScriptLine.LineTypes.If Or
                                          ActiveLine.LineType = ScriptLine.LineTypes.ElseIf Or
                                          ActiveLine.LineType = ScriptLine.LineTypes.Else Or
                                          ActiveLine.LineType = ScriptLine.LineTypes.EndIf Then
                    Return True
                End If

                Return IfStructs.Last().IsBranchValid
            End Get
        End Property

#End Region

#Region "Select"

        Private WhenStructs As New List(Of WhenStruct)

        Private Class WhenStruct

            'The expression after the :select.
            Public selectExpression As String = ""
            'The expression after the last :when.
            Public lastWhenExpression As String = ""

            'When the parent Select statement is invalid, also skip this one.
            Public isInvalidFromParent As Boolean = False

            ''' <summary>
            ''' If the last when branch is valid.
            ''' </summary>
            ''' <returns></returns>
            Public ReadOnly Property IsBranchValid() As Boolean
                Get
                    If isInvalidFromParent = True Then
                        Return False
                    End If
                    Return Framework.Parser.ConditionalComparison(selectExpression & "=" & lastWhenExpression)
                End Get
            End Property

        End Class

        ''' <summary>
        ''' The current script line starts a select block.
        ''' </summary>
        Private Sub ReachedSelect()
            Dim n_when As New WhenStruct()
            n_when.selectExpression = ActiveLine.GetQuickValue(1)

            If WhenStructs.Count > 0 Then
                If WhenStructs.Last().IsBranchValid = False Then
                    n_when.isInvalidFromParent = True
                End If
            End If

            WhenStructs.Add(n_when)
        End Sub

        ''' <summary>
        ''' The current script line branches a select block.
        ''' </summary>
        Private Sub ReachedWhen()
            'If there is a WhenStruct at the top, use it.
            If WhenStructs.Count > 0 Then
                Dim n_when As WhenStruct = WhenStructs.Last()

                n_when.lastWhenExpression = ActiveLine.GetQuickValue(1)
            Else
                Logger.Debug("036", "Invalid ""when"" structure outside of a ""select"" structure.")
            End If
        End Sub

        ''' <summary>
        ''' The current script line ends a select block.
        ''' </summary>
        Private Sub ReachedEndSelect()
            'If there is a WhenStruct at the top, use it.
            If WhenStructs.Count > 0 Then
                WhenStructs.Remove(WhenStructs.Last())
            Else
                Logger.Debug("037", "Invalid ""endselect"" structure outside of a ""select"" structure.")
            End If
        End Sub

        ''' <summary>
        ''' Returns True if the script is in a valid when script block branch right now OR not in any at all.
        ''' </summary>
        ''' <returns></returns>
        Private ReadOnly Property IsInValidWhenBranch() As Boolean
            Get
                If WhenStructs.Count = 0 Or ActiveLine.LineType = ScriptLine.LineTypes.Select Or
                                            ActiveLine.LineType = ScriptLine.LineTypes.When Or
                                            ActiveLine.LineType = ScriptLine.LineTypes.EndSelect Then
                    Return True
                End If

                Return WhenStructs.Last().IsBranchValid
            End Get
        End Property

#End Region

#Region "While"

        Private WhileStructs As New List(Of WhileStruct)

        Private Class WhileStruct

            'The line pointer pointing to the :while script line.
            Public WhilePointer As Integer

            Public WhileExpression As String

            'Once the condition on the while is false, this is set to True.
            'The script will skip over all elements In the While Loop And Not repeat the :while when :endwhile is hit.
            Public SkipWhile As Boolean = False

            Public ReadOnly Property IsWhileValid() As Boolean
                Get
                    Return Framework.Parser.ConditionalComparison(WhileExpression)
                End Get
            End Property

        End Class

        Private Sub ReachedWhile()
            'The script can reach a :while statement at two states:
            'First reach or rematch.
            'At first reach, create a WhileStruct.
            'This is checked by looking if the last whileStruct (if there is one) has the same line pointer as the currently active line pointer.

            Dim firstReach As Boolean = True

            If WhileStructs.Count > 0 Then
                If WhileStructs.Last().WhilePointer = _linePointer Then
                    firstReach = False
                End If
            End If

            If firstReach = True Then
                'Creates a new WhileStruct and assigns the while pointer.
                Dim n_while As New WhileStruct()
                n_while.WhilePointer = _linePointer
                n_while.WhileExpression = ActiveLine.GetQuickValue(1)

                WhileStructs.Add(n_while)
            End If

            'When the expression behind :while is evaluates to False, ignore until :endwhile is reached.
            If WhileStructs.Last().IsWhileValid = False Then
                WhileStructs.Last().SkipWhile = True
            End If
        End Sub

        Private Sub ReachedExitWhile()
            'Exit while searches for the closing :endwhile and sets the line pointer to that line.
            'The search needs to account for eventual embedded :while and :endwhile structures.
            'Afterwards in the above Execute method, the line pointer gets incremented by 1, making the script skip past the whole :while loop.
            'The structure needs to also take into account that it could be used inside an If or Select statement and close them if needed.

            Dim whileLevel As Integer = 0 'Keeps track of the while level. Only detect :endwhile at level 0.
            Dim ifLevel As Integer = 0 'Keeps track of eventual closed :if:s.
            Dim selectLevel As Integer = 0 'Keeps track of eventual closed :select:s.

            Dim l_pointer As Integer = _linePointer

            While l_pointer < _lines.Count
                Select Case _lines(l_pointer).LineType
                    Case ScriptLine.LineTypes.While
                        whileLevel += 1
                    Case ScriptLine.LineTypes.EndWhile
                        If whileLevel > 0 Then
                            whileLevel -= 1
                        Else
                            'Level is 0, this is the endwhile we need.
                            'Don't need to do anything, as the l_pointer variable holds the new line pointer.
                            Exit While
                        End If
                    Case ScriptLine.LineTypes.If
                        ifLevel += 1
                    Case ScriptLine.LineTypes.EndIf
                        ifLevel -= 1
                    Case ScriptLine.LineTypes.Select
                        selectLevel += 1
                    Case ScriptLine.LineTypes.EndSelect
                        selectLevel -= 1
                End Select

                l_pointer += 1
            End While

            'All If and Select levels must be 0 or smaller, else there's a syntax error.
            'Also, if smaller than 0, the count of If and Select blocks must be larger/equal the level * -1.
            If ifLevel < 0 Then
                If IfStructs.Count >= ifLevel * -1 Then
                    'Remove all closed :if:s inside the :while that just got ended with :exitwhile:
                    For i = -1 To ifLevel Step -1
                        IfStructs.RemoveAt(IfStructs.Count - 1)
                    Next
                Else
                    Logger.Debug("038", "Invalid :if branching detected!")
                End If
            ElseIf ifLevel > 0 Then
                Logger.Debug("039", "Invalid :if branching detected!")
            End If

            If selectLevel < 0 Then
                If WhenStructs.Count >= selectLevel * -1 Then
                    'Remove all closed :select:s inside the :while that just got ended with :exitwhile:
                    For i = -1 To selectLevel Step -1
                        WhenStructs.RemoveAt(WhenStructs.Count - 1)
                    Next
                Else
                    Logger.Debug("040", "Invalid :select branching detected!")
                End If
            ElseIf selectLevel > 0 Then
                Logger.Debug("041", "Invalid :select branching detected!")
            End If

            'Set the new line pointer.
            _linePointer = l_pointer

            'Check if the line is actually a :endwhile structure:
            If ActiveLine.LineType <> ScriptLine.LineTypes.EndWhile Then
                Logger.Debug("042", "A missing :endwhile has been detected!")
            End If
        End Sub

        Private Sub ReachedEndWhile()
            'Skip to the prior :while loop:
            If WhileStructs.Count > 0 Then
                'Set the line pointer to the :while structure and substract 1 because the line pointer will get incremented right after this call.
                _linePointer = WhileStructs.Last().WhilePointer - 1
            Else
                Logger.Debug("043", "Invalid ""endwhile"" structure outside of a ""while"" structure.")
            End If
        End Sub

        Private ReadOnly Property IsInValidWhileBranch() As Boolean
            Get
                If WhileStructs.Count = 0 Then
                    Return True
                End If

                If WhileStructs.Last().SkipWhile = True Then
                    If ActiveLine.LineType = ScriptLine.LineTypes.EndWhile Then
                        'If the line type is an :endwhile, remove the WhileStruct.
                        'Doing so, the execute method will still skip this :endwhile structure, but will start to evaluate the next script line again.
                        WhileStructs.Remove(WhileStructs.Last())
                    End If
                    Return False
                Else
                    Return True
                End If
            End Get
        End Property

#End Region

#Region "End/Exit"

        Private Sub ReachedEnd()
            'Exits this script.
            'set the line pointer to the last line, so that the script will be over next time the Execute function loops.
            _linePointer = _lines.Count() - 1
        End Sub

        Private Sub ReachedExit()
            'Terminates all current script executions:
            _linePointer = _lines.Count() - 1
            Controller.GetInstance().Reset()
        End Sub

#End Region

#End Region

    End Class

End Namespace