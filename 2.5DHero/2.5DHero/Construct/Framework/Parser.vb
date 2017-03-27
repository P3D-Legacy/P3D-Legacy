Imports System.Text.RegularExpressions

Namespace Construct.Framework

    ''' <summary>
    ''' Parses script expressions.
    ''' </summary>
    Public Class Parser

        ''' <summary>
        ''' Uses short-circuit evaluation to conditionally return one of two values: True or False.
        ''' </summary>
        ''' <param name="expression">The expression to evaluate. Can contains "and" and "or" parts to link comparisons together.</param>
        ''' <returns></returns>
        Public Shared Function ConditionalComparison(ByVal expression As String) As Boolean
            'The expression is an array of comparisons, separated by either " <and> " or " <or> " (with spaces).
            'We will split the expression into the single comparisons by getting the positions of the <and>s and <or>s.
            'First, we test if it has any <and>s or <or>s at all:

            If expression.ToLower().Contains(" <and> ") = True Or expression.ToLower().Contains(" <or> ") Then
                'If so, we need to separate.
                'The expression gets separated by <or>s, so we have multiple parts with possible <and>s in.
                'Then, those parts get split at the <and>s, and then the split parts get evaluated.
                'Then, we check if any of the <and> parts are true by checking if all split parts in the part in question are true.
                'If so, we return True.
                'Otherwise, the expression is false.

                Dim orParts As String() = Regex.Split(expression, " <or> ", RegexOptions.IgnoreCase)
                For Each orPart As String In orParts
                    If orPart.Length > 0 Then
                        If orPart.ToLower().Contains(" <and> ") = True Then
                            'Contains <and>, this means all parts need to be checked if they are true. If one is false, the whole orPart is false.
                            Dim partFalse As Boolean = False
                            Dim andParts As String() = Regex.Split(orPart, " <and> ", RegexOptions.IgnoreCase)

                            'Now loop through each part. If any is False, the whole orPart is false:
                            For Each andPart As String In andParts
                                If andPart.Length > 0 Then
                                    If CompareExpressions(andPart) = False Then
                                        partFalse = True
                                        Exit For
                                    End If
                                End If
                            Next

                            'If none of the <and> parts are false, the whole expression is True.
                            If partFalse = False Then
                                Return True
                            End If
                        Else
                            'Only one comparison in this orPart, check directly. If it's true, the whole comparison is true:
                            If CompareExpressions(orPart) = True Then
                                Return True
                            End If
                        End If
                    End If
                Next

                'The whole expression does not have a True comparison orPart, so it evaluates to False:
                Return False
            Else
                'It's just a single comparison, we can call CompareExpressions directly:
                Return CompareExpressions(expression)
            End If
        End Function

        Private Const COMPARE_REGEX As String = ".+[=<>!].+"

        ''' <summary>
        ''' Compares two expressions by a comparator and returns the result of the comparison.
        ''' </summary>
        ''' <param name="comparison">The complete comparison.</param>
        ''' <returns></returns>
        Private Shared Function CompareExpressions(ByVal comparison As String) As Boolean
            'The comparison is passed in in the format:
            'value1[comparator]value2.
            'Example:
            '   value1=value2
            '
            'Valid comparators: =, <, >, !

            'First, a single booleanic value is always compared against True:
            If Converter.IsBoolean(comparison) = True Then
                Return CompareExpressions(comparison & "=true")
            End If

            'Check if it matches the basic pattern:
            If Regex.IsMatch(comparison, COMPARE_REGEX) = True Then
                'Now we try to get the comparator position in the string.
                '
                'First thing is that all constructs get replaced so their < and > don't get in the way:
                Dim constructs As List(Of Tuple(Of Integer, Integer)) = GetConstructPositionsInExpression(comparison)

                Dim modifiedComparison As String = comparison

                For i = 0 To constructs.Count() - 1
                    modifiedComparison = modifiedComparison.Remove(
                                    constructs(i).Item1,
                                    constructs(i).Item2)

                    'Insert random, but safe characters:
                    For ii = 1 To constructs(i).Item2
                        modifiedComparison = modifiedComparison.Insert(constructs(i).Item1, "X")
                    Next
                Next

                'Now, the first comparator we find will win the race.
                'If any comparator chars are in the strings to compare, they need to be escaped with \.

                'Collect the first occurences of any comparator in a dictionary:
                Dim comparators As New SortedDictionary(Of Integer, Char)
                For i = 1 To modifiedComparison.Length - 1
                    Dim token As Char = modifiedComparison(i)

                    Dim preToken As Char = modifiedComparison(i - 1)
                    Dim prepreToken As Char
                    If i - 2 > -1 Then
                        prepreToken = modifiedComparison(i - 2)
                    End If

                    'If the comparator is not escaped by \:
                    If preToken <> "\"c Or preToken = "\"c And prepreToken = "\"c Then
                        Select Case token
                            Case "="c, "<"c, ">"c, "!"c
                                If comparators.ContainsValue(token) = False Then
                                    comparators.Add(i, token)
                                End If
                        End Select
                    End If
                Next

                If comparators.Count > 0 Then
                    Dim leftValue As String = EvaluateScriptExpression(comparison.Remove(comparators.Keys(0)))
                    Dim rightValue As String = EvaluateScriptExpression(comparison.Remove(0, comparators.Keys(0) + 1))

                    Dim comparator As Char = comparators.Values(0)

                    'The end result of the comparison:
                    Dim result As Boolean = False

                    Select Case comparator
                        Case "="c, "!"c
                            If Converter.IsBoolean(leftValue) = True And Converter.IsBoolean(rightValue) = True Then
                                result = Converter.ToBoolean(leftValue) = Converter.ToBoolean(rightValue)
                            Else
                                result = leftValue.ToLower() = rightValue.ToLower()
                            End If

                            'Inverts the result:
                            If comparator = "!"c Then
                                result = Not result
                            End If
                        Case "<"c
                            If Converter.IsNumeric(leftValue) = True And Converter.IsNumeric(rightValue) = True Then
                                result = Converter.ToDouble(leftValue) < Converter.ToDouble(rightValue)
                            Else
                                Logger.Debug("020", "The expression """ & leftValue & "<" & rightValue & """ cannot be compared")
                            End If
                        Case ">"c
                            If Converter.IsNumeric(leftValue) = True And Converter.IsNumeric(rightValue) = True Then
                                result = Converter.ToDouble(leftValue) > Converter.ToDouble(rightValue)
                            Else
                                Logger.Debug("021", "The expression """ & leftValue & ">" & rightValue & """ cannot be compared")
                            End If
                    End Select

                    Return result
                Else
                    'No comparators found in the expression:
                    Logger.Debug("022", "Invalid comparison expression:  " & comparison)
                End If
            Else
                Logger.Debug("023", "Invalid comparison expression:  " & comparison)
            End If

            'The default return is False:
            Return False
        End Function

        ''' <summary>
        ''' Returns a list of startindex:length of constructs in an expression.
        ''' </summary>
        ''' <param name="expression">The expressin to search in.</param>
        ''' <returns></returns>
        Private Shared Function GetConstructPositionsInExpression(ByVal expression As String) As List(Of Tuple(Of Integer, Integer))
            Dim output As New List(Of Tuple(Of Integer, Integer))

            If expression Is Nothing Then
                Return output
            End If
            If expression.Length = 0 Then
                Return output
            End If

            Dim tokens = expression.ToCharArray().ToList()
            Dim token As Char
            Dim preToken As Char
            Dim prepreToken As Char

            Dim tempConstruct As String = ""
            Dim startIndex As Integer = -1

            Dim cIndex As Integer = 0
            While tokens.Count > 0
                prepreToken = preToken
                preToken = token
                token = tokens(0)
                tokens.RemoveAt(0)

                If tempConstruct = "" Then
                    'If there's no started construct, then a < can start a new one. Else, just append the character to the output queue.
                    If token = "<"c Then
                        '\ escapes <.
                        If preToken <> "\"c Or preToken = "\"c And prepreToken = "\"c Then
                            tempConstruct &= token.ToString()
                            startIndex = cIndex
                        End If
                    End If
                Else
                    'Instead of the output queue, append the characters to the temp construct while it's active.
                    tempConstruct &= token.ToString()
                    'If a construct has started, check if the character ends it.
                    If token = ">"c Then
                        'Once the construct is valid, append its result to the output queue and reset the temp construct.
                        If IsValidConstruct(tempConstruct) = True Then
                            output.Add(New Tuple(Of Integer, Integer)(startIndex, tempConstruct.Length))
                            tempConstruct = ""
                        End If
                    End If
                End If

                cIndex += 1
            End While

            Return output
        End Function

        ''' <summary>
        ''' Evaluates all constructs in the input expression.
        ''' </summary>
        ''' <param name="expression">The expression to be evaluated.</param>
        ''' <returns></returns>
        Public Shared Function EvaluateScriptExpression(ByVal expression As String) As String
            If expression Is Nothing Then
                Return String.Empty
            End If
            If expression.Length = 0 Then
                Return String.Empty
            End If

            'Gets the list of constructs in this expression (their start index and length):
            Dim constructs As List(Of Tuple(Of Integer, Integer)) = GetConstructPositionsInExpression(expression)
            Dim lengthOffset As Integer = 0

            For Each t As Tuple(Of Integer, Integer) In constructs
                'Get the construct from the expression and evaluate it:
                Dim construct As String = expression.Substring(t.Item1 + lengthOffset, t.Item2)
                Dim evaluatedConstruct As String = EvaluateConstruct(construct)

                'Replace the construct with its evaluated result:
                expression = expression.Remove(t.Item1 + lengthOffset, t.Item2)
                expression = expression.Insert(t.Item1 + lengthOffset, evaluatedConstruct)

                'Substract the construct length from the evaluated length and add that to the offset:
                lengthOffset += evaluatedConstruct.Length - construct.Length
            Next

            'Replace the \ escape:
            expression = expression.Replace("\\", "\")

            Return expression
        End Function

        Private Const VALUE_REGEX As String = "<[\$]{0,1}[0-9,a-z,A-Z]+>"
        Private Const ARRAY_REGEX As String = "<[\$]{0,1}[0-9,a-z,A-Z]+\[[0-9]+\]>"
        Private Const CONSTRUCT_SHORT_REGEX As String = "<[0-9,a-z,A-Z]+\.[0-9,a-z,A-Z]+>"
        Private Const CONSTRUCT_LONG_REGEX As String = "<[0-9, a-z,A-Z]+\.[0-9,a-z,A-Z]+\(.*\)>"

        ''' <summary>
        ''' Checks if an expression is a valid construct.
        ''' </summary>
        ''' <param name="expression">The expression to check.</param>
        ''' <returns></returns>
        Private Shared Function IsValidConstruct(ByVal expression As String) As Boolean
            If expression Is Nothing Then
                Return False
            End If
            If expression.Length = 0 Then
                Return False
            End If

            'EasyValue detection:
            'Technically, an EasyValue is also a construct, but without a . separator.
            'This means it's easy to find, as it also doesn't take any arguments.
            'Check with the regular expression: <[\$]{0,1}[0-9,a-z,A-Z]+>
            If Regex.IsMatch(expression, VALUE_REGEX) = True Then
                If Regex.Match(expression, VALUE_REGEX).Length = expression.Length Then
                    Return True
                End If
            End If

            'Same with EasyArrays:
            'Check with a different regex though:<[\$]{0,1}[0-9,a-z,A-Z]+\[[0-9]+\]>
            If Regex.IsMatch(expression, ARRAY_REGEX) = True Then
                If Regex.Match(expression, ARRAY_REGEX).Length = expression.Length Then
                    Return True
                End If
            End If

            'Has to have equal amount of < and >.
            '< and > between ( and ) don't count.
            'has to be in the format <main.sub>, <main.sub()> or <main.sub(arg)>
            'Closing brackets only apply when the next character is a >.
            'This applies to all constructs: must start with <, end with > and have at least one .
            If expression.StartsWith("<") = False Or expression.EndsWith(">") = False Or expression.Contains(".") = False Then
                Return False
            End If

            If Regex.IsMatch(expression, CONSTRUCT_LONG_REGEX) = False And Regex.IsMatch(expression, CONSTRUCT_SHORT_REGEX) = False Then
                Return False
            Else
                If Regex.IsMatch(expression, CONSTRUCT_LONG_REGEX) = True Then
                    If Regex.Match(expression, CONSTRUCT_LONG_REGEX).Length < expression.Length Then
                        Return False
                    End If
                ElseIf Regex.IsMatch(expression, CONSTRUCT_SHORT_REGEX) = True Then
                    If Regex.Match(expression, CONSTRUCT_SHORT_REGEX).Length < expression.Length Then
                        Return False
                    End If
                End If
            End If

            Dim openCount As Integer = 0
            Dim closeCount As Integer = 0

            Dim isInBrackets As Integer = 0

            Dim tokens As List(Of Char) = expression.ToCharArray().ToList()
            Dim token As Char
            Dim preToken As Char
            Dim prepreToken As Char

            While tokens.Count() > 0
                prepreToken = preToken
                preToken = token
                token = tokens(0)
                tokens.RemoveAt(0)

                'Escape with \, but don't with \\.
                If preToken <> "\"c Or preToken = "\"c And prepreToken = "\"c Then
                    Select Case token
                        Case "<"c
                            If isInBrackets = 0 Then
                                openCount += 1
                            End If
                        Case ">"c
                            If isInBrackets = 0 Then
                                closeCount += 1
                            End If
                        Case "("c
                            isInBrackets += 1
                        Case ")"c
                            If tokens.Count > 0 Then
                                If tokens(0) = ">"c Then
                                    isInBrackets -= 1
                                End If
                            Else
                                'There HAS to be at least a > behind a bracket.
                                Return False
                            End If
                    End Select
                End If
            End While

            'If the conditions are met, it's a valid construct:
            If openCount = closeCount And isInBrackets = 0 And openCount > 0 Then
                Return True
            End If

            Return False
        End Function

        ''' <summary>
        ''' Evaluates a construct.
        ''' </summary>
        ''' <param name="construct">The construct to evaluate.</param>
        ''' <returns></returns>
        Public Shared Function EvaluateConstruct(ByVal construct As String) As String
            'If the input is not valid, just return it.
            If IsValidConstruct(construct) = False Then
                Return construct
            End If

            'Preserve original construct in case the class/sub does not exist.
            Dim originalConstruct As String = construct

            construct = construct.Remove(construct.Length - 1, 1).Remove(0, 1) 'Remove < and >.

            If construct.Contains(".") = False Then
                'Must be an EasyValue:

                'Now, this can either be a simple value or complete array access OR a single item array access.
                'EasyValue names cannot contain [] chars, which means if it contains these, then it's an array access:
                If construct.EndsWith("]") = True And construct.Contains("[") = True Then
                    Dim easyArrayIdentifier As String = construct.Remove(construct.IndexOf("[")) 'Get name of the array.
                    Dim arrayIndex As String = construct.Remove(0, construct.IndexOf("[") + 1) 'Remove the name of the array and the first [.
                    arrayIndex = arrayIndex.Remove(arrayIndex.Length - 1, 1) 'Remove last ]

                    Return EvaluateEasyArrayItem(easyArrayIdentifier, arrayIndex)
                Else
                    Return EvaluateEasyValue(construct)
                End If
            Else
                Dim className As String = construct.Remove(construct.IndexOf("."))
                Dim subName As String = construct.Remove(0, construct.IndexOf(".") + 1)
                Dim argument As String = ""

                'Check if an argument exists:
                If subName.Contains("(") = True And subName.EndsWith(")") = True Then
                    argument = subName.Remove(0, subName.IndexOf("(") + 1)
                    argument = argument.Remove(argument.Length - 1, 1)

                    subName = subName.Remove(subName.IndexOf("("))
                End If

                'If there are constructs in the argument, parse them:
                If argument <> "" Then
                    argument = EvaluateScriptExpression(argument)
                End If

                Return EvaluateConstruct(className, subName, argument)
            End If
        End Function

        Private Shared Function EvaluateConstruct(ByVal className As String, ByVal subName As String, ByVal argument As String) As String
            'Replacing the escaped characters here, so that script classes can use them properly:
            argument = argument.Replace("\<", "<")
            argument = argument.Replace("\>", ">")
            argument = argument.Replace("\(", "(")
            argument = argument.Replace("\)", ")")
            argument = argument.Replace("\=", "=")
            argument = argument.Replace("\!", "!")

            Dim scriptClass As scriptClass = Core.GetInstance().GetClassByName(className)

            If scriptClass IsNot Nothing Then
                Dim scriptSub As scriptSub = scriptClass.GetSubByName(subName, scriptSub.SubTypes.Construct)
                If scriptSub IsNot Nothing Then
                    Return scriptSub.Execute(argument)
                Else
                    'if the sub does not exist, return null:
                    Return Core.Null
                End If
            Else
                'if the class does not exist, return null:
                Return Core.Null
            End If
        End Function

        Private Shared Function EvaluateEasyValue(ByVal valueName As String) As String
            Return Controller.GetInstance().ValueHandler.Value(valueName).ToString()
        End Function

        Private Shared Function EvaluateEasyArrayItem(ByVal valueName As String, ByVal arrayIndex As String) As String
            'Return a single item from an array value:
            Dim intIndex As Integer = Converter.ToInteger(arrayIndex)
            Dim arrayValue = Controller.GetInstance().ValueHandler.Value(valueName)
            If arrayValue.ValueHolderType = ScriptValueHolder.Types.Array Then
                Dim inArray = CType(arrayValue, ScriptArray).Array
                If inArray.Length > intIndex Then
                    Return inArray(intIndex)
                Else
                    Return Core.Null
                End If
            Else
                Return arrayValue.ToString()
            End If
        End Function

        ''' <summary>
        ''' Converts an array into its string representation.
        ''' </summary>
        ''' <param name="arr">The array to convert.</param>
        ''' <returns></returns>
        Public Shared Function ArrayToString(ByVal arr As String()) As String
            If arr Is Nothing Then
                Return "{ }"
            End If
            If arr.Length = 0 Then
                Return "{ }"
            End If
            Dim out As New Text.StringBuilder("{")
            For Each s As String In arr
                If out.Length > 0 Then
                    out.Append(", ")
                End If
                out.Append("'" & s & "'")
            Next
            out.Append("}")
            Return out.ToString()
        End Function

        ''' <summary>
        ''' Parses an input string into an array.
        ''' </summary>
        ''' <param name="stringArr">The raw string.</param>
        ''' <returns></returns>
        Public Shared Function ParseArray(ByVal stringArr As String) As List(Of String)
            'Create a new list that servers as output array:
            Dim arrList As New List(Of String)

            'Tries to convert a string into an array:
            'First, check if the string starts and ends with {}:

            If stringArr.StartsWith("{") = True And stringArr.EndsWith("}") = True Then
                'Remove the brackets:
                stringArr = stringArr.Remove(stringArr.Length - 1, 1).Remove(0, 1)

                'Trim leading and trailing spaces/tabs:
                stringArr = stringArr.Trim()

                'If the string is longer than 0, there is at least one object in the array:
                If stringArr.Length > 0 Then

                    'Checking if the input string only contains one item:
                    Dim tokens As Char() = stringArr.ToCharArray()

                    Dim currentObject As String = ""
                    Dim openedObject As Boolean = False

                    For i = 0 To tokens.Length - 1
                        Dim token As Char = tokens(i)

                        Dim preToken As Char
                        Dim prepreToken As Char
                        If i > 0 Then
                            preToken = tokens(i - 1)
                        End If
                        If i > 1 Then
                            prepreToken = tokens(i - 2)
                        End If

                        Select Case token
                            Case "'"c
                                If preToken <> "\"c Or preToken = "\"c And prepreToken = "\"c Then
                                    If openedObject = False Then
                                        openedObject = True
                                        currentObject = ""
                                    Else
                                        openedObject = False
                                        arrList.Add(EvaluateScriptExpression(currentObject))
                                        currentObject = ""
                                    End If
                                Else
                                    currentObject &= "'"
                                End If
                            Case ","c
                                If openedObject = True Then
                                    currentObject &= ","
                                Else
                                    If currentObject.Length > 0 Then
                                        If preToken <> "\"c Or preToken = "\"c And prepreToken = "\"c Then
                                            arrList.Add(EvaluateScriptExpression(currentObject))
                                            currentObject = ""
                                        Else
                                            currentObject &= ","
                                        End If
                                    End If
                                End If
                            Case "\"c
                                If preToken = "\"c And prepreToken <> "\"c Then
                                    currentObject &= "\"
                                End If
                            Case Else
                                currentObject &= token.ToString()
                        End Select
                    Next

                    If currentObject.Length > 0 And openedObject = False Then
                        arrList.Add(EvaluateScriptExpression(currentObject))
                        currentObject = ""
                    End If
                End If
            End If

            Return arrList
        End Function

    End Class

End Namespace