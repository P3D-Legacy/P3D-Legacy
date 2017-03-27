Imports System.Text.RegularExpressions

Namespace Construct

    ''' <summary>
    ''' A class that represents a single line in a <see cref="Script"/>.
    ''' </summary>
    Public Class ScriptLine

        ''' <summary>
        ''' The type of action to be performed by a script line.
        ''' </summary>
        Public Enum LineTypes
            ''' <summary>
            ''' The default LineType, no action for this line.
            ''' </summary>
            None

            ''' <summary>
            ''' A comment is prefaced by a #.
            ''' </summary>
            Comment

            ''' <summary>
            ''' An empty line does not contain any characters apart from optional spaces and tabs.
            ''' </summary>
            EmptyLine

            ''' <summary>
            ''' At the start of the file (or implicit version=1), must start with "version=", then a numeric value. Obsolete.
            ''' </summary>
            VersionDeclaration

            ''' <summary>
            ''' A command starts with "@", has at least one "." and is at least 4 characters long.
            ''' </summary>
            Command

            ''' <summary>
            ''' A value assignment.
            ''' </summary>
            ValueAssignment

            ''' <summary>
            ''' Creates a new empty array in a value.
            ''' </summary>
            EmptyArrayAssignment

            ''' <summary>
            ''' Assigns an array of values to a variable.
            ''' </summary>
            ArrayAssignment

            ''' <summary>
            ''' Assigns a value to a single item in an array.
            ''' </summary>
            ArrayItemAssignment

            ''' <summary>
            ''' The end of a script, must be ":end"
            ''' </summary>
            [End]

            ''' <summary>
            ''' The end of the entire script stack, must ":exit"
            ''' </summary>
            [Exit]

            ''' <summary>
            ''' Start of a While loop. Must start with :while: and have a value behind. If no comparison is given, the value is compared to "True"
            ''' </summary>
            [While]

            ''' <summary>
            ''' Exits a while loop, must be ":exitwhile"
            ''' </summary>
            ExitWhile

            ''' <summary>
            ''' Ends a while loop, must be ":endwhile"
            ''' </summary>
            EndWhile

            ''' <summary>
            ''' Starts an if condition, must be :if: and have a value behind. If no comparison is given, the value is compared to "True"
            ''' </summary>
            [If]

            ''' <summary>
            ''' Branches an if condition, must be :elseif: and have a value behind. If no comparison is given, the value is compared to "True"
            ''' </summary>
            [ElseIf]

            ''' <summary>
            ''' Branches an if condition if the :if: and all :elseif: are not used. Must be ":else"
            ''' </summary>
            [Else]

            ''' <summary>
            ''' Closes an if condition, must be ":endif"
            ''' </summary>
            [EndIf]

            ''' <summary>
            ''' Starts a selection condition, must start with :select: and have a value behind.
            ''' </summary>
            [Select]

            ''' <summary>
            ''' Evaluates a selection condition, must start with :when: and have a value behind.
            ''' </summary>
            [When]

            ''' <summary>
            ''' Ends a selection condition, must be ":endselect".
            ''' </summary>
            EndSelect
        End Enum

        Private _parent As Script = Nothing

        Private _originalLine As String = String.Empty
        Private _quickValues As New List(Of String) 'Contains values depending on the line type.

        Private _isValid As Boolean = False 'Valid means not to be ignored.
        Private _preserve As Boolean = False 'This can be set to True by a command to let this command execute until its set to False again.
        Private _endExecutionFrame As Boolean = False 'This makes the controller execute this line, but then close the execution until next frame.

        Private _lineType As LineTypes = LineTypes.None

        'A list of values for scripts to store stuff in temporarly
        Public workValues As New List(Of String)

        ''' <summary>
        ''' If this script line is a structure.
        ''' </summary>
        ''' <returns></returns>
        Private ReadOnly Property IsStructure() As Boolean
            Get
                'All non-structure LineTypes:
                Return {LineTypes.Comment,
                    LineTypes.Command,
                    LineTypes.VersionDeclaration,
                    LineTypes.EmptyLine,
                    LineTypes.ValueAssignment,
                    LineTypes.ArrayAssignment,
                    LineTypes.EmptyArrayAssignment,
                    LineTypes.ArrayItemAssignment}.Contains(_lineType) = False
            End Get
        End Property

        ''' <summary>
        ''' The line type of this script line.
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property LineType() As LineTypes
            Get
                Return _lineType
            End Get
        End Property

        ''' <summary>
        ''' If this script line should be considered to be executed (RIP script line).
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property IsValid() As Boolean
            Get
                Return _isValid
            End Get
        End Property

        ''' <summary>
        ''' If this script line should be preserved as the active line.
        ''' </summary>
        ''' <returns></returns>
        Public Property Preserve() As Boolean
            Get
                Return _preserve
            End Get
            Set(value As Boolean)
                _preserve = value
            End Set
        End Property

        ''' <summary>
        ''' If the script line should stop execution for this frame after this line.
        ''' </summary>
        ''' <returns></returns>
        Public Property EndExecutionFrame() As Boolean
            Get
                Return _endExecutionFrame
            End Get
            Set(value As Boolean)
                _endExecutionFrame = value
            End Set
        End Property

        ''' <summary>
        ''' Returns a quick value from an index.
        ''' </summary>
        ''' <param name="index">The index.</param>
        ''' <returns></returns>
        Public Function GetQuickValue(ByVal index As Integer) As String
            If _quickValues.Count() - 1 <= index Then
                Return _quickValues(index)
            Else
                Return Framework.Core.Null
            End If
        End Function

        ''' <summary>
        ''' The input line.
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property OriginalLineContent() As String
            Get
                Return _originalLine
            End Get
        End Property

        ''' <summary>
        ''' Creates a new instance of a script line.
        ''' </summary>
        ''' <param name="lineContent">The content of this line.</param>
        ''' <param name="parent">The parent script.</param>
        Public Sub New(ByVal lineContent As String, ByVal parent As Script)
            _parent = parent
            _originalLine = lineContent

            ParseLine(lineContent)
        End Sub

#Region "Parsing"

        Private Sub ParseLine(ByVal lineContent As String)
            'Cut spaces/tabs at the beginning of the line:
            While lineContent.StartsWith(" ") Or lineContent.StartsWith(vbTab)
                lineContent = lineContent.Remove(0, 1)
            End While

            If lineContent.Length = 0 Then
                'Length = 0 means empty line.
                Me._lineType = LineTypes.EmptyLine
            ElseIf lineContent.ToLower().StartsWith("version=") = True Then
                Me._lineType = LineTypes.VersionDeclaration
                Logger.Debug("002", "Version declarations of scripts are obsolete.")
            Else
                Dim leadingChar As Char = lineContent(0)

                Select Case leadingChar
                    Case "@"c
                        If lineContent.EndsWith(";") Then 'remove trailing semicolon:
                            lineContent = lineContent.Remove(lineContent.Length - 1, 1)
                            Logger.Debug("003", "I don't think you wanted to put the ; there, you C nerd! I removed it for you ;)")
                        End If
                        ParseCommand(lineContent)
                    Case "<"c
                        ParseValueAssignment(lineContent)
                    Case ":"c
                        ParseStructure(lineContent)
                    Case "#"c
                        ParseComment(lineContent)
                    Case Else
                        Logger.Debug("004", "Invalid script line leading char: " & leadingChar.ToString() & ". A script line must start with #, @, < or :.")
                End Select
            End If
        End Sub

        Private Sub ParseCommand(ByVal lineContent As String)
            'Command syntax: @mainclass.subclass(arguments)
            'The argument and brackets are optional, but if arguments are used, both brackets must be used for the command to function.
            'The brackets can be used without an argument, like this: @mainclass.subclass()

            If lineContent.Length >= 4 And lineContent.Contains(".") = True Then
                _lineType = LineTypes.Command
                _isValid = True

                lineContent = lineContent.Remove(0, 1) 'Remove @

                Dim className As String = lineContent.Remove(lineContent.IndexOf("."))
                Dim subName As String = lineContent.Remove(0, lineContent.IndexOf(".") + 1)
                Dim argument As String = ""

                'Check if an argument exists:
                If subName.Contains("(") = True And subName.EndsWith(")") = True Then
                    argument = subName.Remove(0, subName.IndexOf("(") + 1)
                    argument = argument.Remove(argument.Length - 1, 1)

                    subName = subName.Remove(subName.IndexOf("("))
                End If

                _quickValues.AddRange({className, subName, argument})
            Else
                Logger.Debug("005", "Invalid command line: " & lineContent)
            End If
        End Sub

        Private Sub ParseStructure(ByVal lineContent As String)
            'Structure syntax (general):
            ':structure
            'If arguments are given, they are separated by another :.
            'Example: :structure:argument

            If lineContent.EndsWith(":") = False Then 'Must be more than ":" and not end with a ":".
                Dim structureType As String = lineContent.Remove(0, 1) 'Removes the first ":".
                Dim argument As String = ""

                'Has an argument:
                If structureType.Contains(":") = True Then
                    argument = structureType.Remove(0, structureType.IndexOf(":") + 1)

                    structureType = structureType.Remove(structureType.IndexOf(":"))
                End If

                _isValid = True

                'Determine the structure type:
                Select Case structureType.ToLower()
                    Case "end"
                        _lineType = LineTypes.End
                    Case "exit"
                        _lineType = LineTypes.Exit

                    Case "while"
                        _lineType = LineTypes.While
                    Case "exitwhile"
                        _lineType = LineTypes.ExitWhile
                    Case "endwhile"
                        _lineType = LineTypes.EndWhile

                    Case "if"
                        _lineType = LineTypes.If
                    Case "elseif"
                        _lineType = LineTypes.ElseIf
                    Case "else"
                        _lineType = LineTypes.Else
                    Case "endif"
                        _lineType = LineTypes.EndIf

                    Case "select"
                        _lineType = LineTypes.Select
                    Case "when"
                        _lineType = LineTypes.When
                    Case "endselect"
                        _lineType = LineTypes.EndSelect
                    Case Else
                        _isValid = False
                End Select

                _quickValues.AddRange({structureType, argument})
            Else
                Logger.Debug("006", "Invalid structure line: " & lineContent)
            End If
        End Sub

        Private Sub ParseComment(ByVal lineContent As String)
            _lineType = LineTypes.Comment
            Dim comment As String = lineContent.Remove(0, 1)
            _quickValues.Add(comment)
        End Sub

        Private Const VALUEASSIGN_REGEX As String = "<[0-9,a-z,A-Z]+>[\+\-\*/&]{0,1}=.+"
        Private Const VALUEASSIGN_EMPTYARR As String = "<[0-9,a-z,A-Z]+\[\]>=\[[0-9]+\]"
        Private Const VALUEASSIGN_NEWARR As String = "<[0-9,a-z,A-Z]+\[\]>=\{.+\}"
        Private Const VALUEASSIGN_ARRITEM As String = "<[0-9,a-z,A-Z]+\[[0-9]+\]>=.+"

        Private Sub ParseValueAssignment(ByVal lineContent As String)
            'A value assignment looks like this:
            '   <varName>=variable
            '   where variable can be a static value or a construct.
            '   Examples:
            '       <1stPName>=<pokemon.getname(0)>
            '       <height>=10.2
            'If the value has been assigned already, an operation can be performed on numeric values:
            '       <var1>+=20
            '       This works with -=, +=, /= and *=.
            'Additionally, a concatenation can be performed, to force a string concatenation:
            '       <var1>&=20
            'Also, a value assignment can be an array.
            'An array assignment can take two specific forms:
            '       <arr[]>=[20]
            'or
            '       <arr[]>={<items>}
            'or
            '       <arr[index]>=value

            If Regex.IsMatch(lineContent, VALUEASSIGN_REGEX) = True Then
                _isValid = True
                _lineType = LineTypes.ValueAssignment

                Dim valueName As String = lineContent.Remove(lineContent.IndexOf(">")).Remove(0, 1) 'Get rid of the <, > and everything afterwards.
                Dim assignment As String = lineContent.Remove(0, lineContent.IndexOf("=") + 1) 'Get rid of everything before the "=" (including the "=").

                Dim valueOperator As String = lineContent.Remove(0, valueName.Length + 2)
                valueOperator = valueOperator.Remove(valueOperator.IndexOf("="))

                _quickValues.AddRange({valueName, assignment, valueOperator})
            ElseIf Regex.IsMatch(lineContent, VALUEASSIGN_EMPTYARR) = True Then
                _isValid = True
                _lineType = LineTypes.EmptyArrayAssignment

                Dim valueName As String = lineContent.Remove(lineContent.IndexOf("[")).Remove(0, 1) 'Get rid of the < and everything after [.
                Dim assignment As String = lineContent.Remove(0, lineContent.IndexOf("=") + 2) 'Get rid of everything before the "=[" (including the "=[").

                assignment = assignment.Remove(assignment.Length - 1, 1) 'Removes the last ].

                _quickValues.AddRange({valueName, assignment})
            ElseIf Regex.IsMatch(lineContent, VALUEASSIGN_NEWARR) = True Then
                _isValid = True
                _lineType = LineTypes.ArrayAssignment

                Dim valueName As String = lineContent.Remove(lineContent.IndexOf("[")).Remove(0, 1) 'Get rid of the < and everything after [.
                Dim assignment As String = lineContent.Remove(0, lineContent.IndexOf("=") + 1) 'Get rid of everything before the "=" (including the "=").

                _quickValues.AddRange({valueName, assignment})
            ElseIf Regex.IsMatch(lineContent, VALUEASSIGN_ARRITEM) = True Then
                _isValid = True
                _lineType = LineTypes.ArrayItemAssignment

                Dim valueName As String = lineContent.Remove(lineContent.IndexOf("[")).Remove(0, 1) 'Get rid of the < and everything after [.
                Dim assignment As String = lineContent.Remove(0, lineContent.IndexOf("=") + 1) 'Get rid of everything before the "=" (including the "=").

                Dim itemIndex As String = lineContent.Remove(0, lineContent.IndexOf("[") + 1)
                itemIndex = itemIndex.Remove(itemIndex.IndexOf("]"))

                _quickValues.AddRange({valueName, assignment, itemIndex})
            Else
                Logger.Debug("007", "Invalid value assignment. Has to match the pattern: " & VALUEASSIGN_REGEX)
            End If
        End Sub

#End Region

        ''' <summary>
        ''' Executes this script line.
        ''' </summary>
        Public Sub Execute()
            If IsStructure = True Then
                'Let the script handle any control structures:
                _parent.ReachedStructure()
            Else
                Select Case _lineType
                    Case LineTypes.Command
                        Framework.Core.GetInstance().ExecuteCommand(_quickValues(0), _quickValues(1), _quickValues(2))
                    Case LineTypes.ValueAssignment
                        Framework.Core.GetInstance().AssignValue(_quickValues(0), _quickValues(1), _quickValues(2))
                    Case LineTypes.ArrayAssignment
                        Framework.Core.GetInstance().AssignArray(_quickValues(0), _quickValues(1))
                    Case LineTypes.EmptyArrayAssignment
                        Framework.Core.GetInstance().CreateNewEmptyArray(_quickValues(0), _quickValues(1))
                    Case LineTypes.ArrayItemAssignment
                        Framework.Core.GetInstance().AssignArrayItem(_quickValues(0), _quickValues(1), _quickValues(2))
                End Select
            End If
        End Sub

        ''' <summary>
        ''' Resets the script line.
        ''' </summary>
        Public Sub Reset()
            _preserve = False
            _endExecutionFrame = False
            workValues.Clear()
        End Sub

    End Class

End Namespace