Namespace ScriptVersion2

    Partial Class ScriptComparer

#Region "DefaultReturn"

        ''' <summary>
        ''' Represents the default void return, if the contruct could not return anything else.
        ''' </summary>
        Public Class DefaultNullObj

            Public Overrides Function ToString() As String
                Return "return:void" 'Just return "void" when this gets used as string to indicate that this type got returned.
            End Function

        End Class

        Private Shared ReadOnly _defaultNull As DefaultNullObj = New DefaultNullObj()
        Public Shared ReadOnly Property DefaultNull() As DefaultNullObj
            Get
                Return _defaultNull
            End Get
        End Property

#End Region

        Structure PairValue
            Dim Command As String
            Dim Argument As String
        End Structure

        ''' <summary>
        ''' Evaluates a script comparison between two expressions.
        ''' </summary>
        ''' <param name="inputString">The string containing the expression to compare.</param>
        Public Shared Function EvaluateScriptComparison(ByVal inputString As String) As Boolean
            Return EvaluateScriptComparison(inputString, False)
        End Function

        ''' <summary>
        ''' Evaluates a script comparison between two expressions.
        ''' </summary>
        ''' <param name="inputString">The string containing the expression to compare.</param>
        ''' <param name="caseSensitive">If the case of the strings should be evaluated.</param>
        Public Shared Function EvaluateScriptComparison(ByVal inputString As String, ByVal caseSensitive As Boolean) As Boolean
            Dim comparer As String = "="

            Dim level As Integer = 0
            Dim setComparer As String = ""
            Dim lastComparer As String = ""
            Dim comparerIndex As Integer = inputString.IndexOf("=")
            Dim lastComparerIndex As Integer = 0
            Dim i As Integer = 0

            Dim countStarts As Integer = 0
            For Each c As Char In inputString
                Select Case c
                    Case CChar("<")
                        countStarts += 1
                    Case CChar(">")
                        countStarts -= 1
                End Select
            Next

            If countStarts < 0 Then
                setComparer = ">"
            ElseIf countStarts > 0 Then
                setComparer = "<"
            Else
                setComparer = "="
            End If

            If setComparer = ">" Then
                For Each c As Char In inputString
                    Select Case c
                        Case CChar("<")
                            level += 1
                        Case CChar(">")
                            level -= 1
                            If level = -1 Then
                                comparerIndex = i
                                Exit For
                            End If
                    End Select
                    i += 1
                Next
            ElseIf setComparer = "<" Then
                Dim started As New List(Of Integer)
                For Each c As Char In inputString
                    Select Case c
                        Case CChar("<")
                            started.Add(i)
                        Case CChar(">")
                            started.RemoveAt(started.Count - 1)
                    End Select

                    i += 1
                Next
                comparerIndex = started(0)
            End If

            If setComparer <> "" Then
                comparer = setComparer
            Else
                comparerIndex = inputString.IndexOf("=")
            End If

            Dim compareValue As Object = inputString.Substring(comparerIndex + 1)
            Dim classValue As String = inputString.Substring(0, comparerIndex)

            Dim resultValue As Object = EvaluateConstruct(classValue)

            compareValue = EvaluateConstruct(compareValue)

            Dim comparisonResult As Boolean = False

            Select Case comparer
                Case "="
                    If caseSensitive = True Or TryCast(resultValue, String) = Nothing Or TryCast(compareValue, String) = Nothing Then
                        If resultValue.Equals(compareValue) Then
                            comparisonResult = True
                        End If
                    Else
                        If ScriptConversion.IsBoolean(CStr(resultValue)) = True And ScriptConversion.IsBoolean(CStr(compareValue)) = True Then
                            If ScriptConversion.ToBoolean(resultValue) = ScriptConversion.ToBoolean(compareValue) Then
                                comparisonResult = True
                            End If
                        Else
                            If CStr(resultValue).ToLower() = CStr(compareValue).ToLower() Then
                                comparisonResult = True
                            End If
                        End If
                    End If
                Case ">"
                    If IsNumeric(resultValue) = True And IsNumeric(compareValue) = True Then
                        If dbl(resultValue) > dbl(compareValue) Then
                            comparisonResult = True
                        End If
                    End If
                Case "<"
                    If IsNumeric(resultValue) = True And IsNumeric(compareValue) = True Then
                        If dbl(resultValue) < dbl(compareValue) Then
                            comparisonResult = True
                        End If
                    End If
            End Select

            Return comparisonResult
        End Function

        ''' <summary>
        ''' Evaluates a complete construct.
        ''' </summary>
        ''' <param name="construct">
        ''' <para>The complete construct. Example: &lt;mainclass.subclass(argument)&gt;.</para>
        ''' <para>If a &lt;not&gt; is put directly in front of the construct, the result will be negated.</para>
        ''' </param>
        Public Shared Function EvaluateConstruct(ByVal construct As Object) As Object
            If TryCast(construct, String) <> Nothing Then
                If CStr(construct) = "" Then
                    Return ""
                End If

                Dim output As String = ""
                Dim input As String = construct.ToString()

                Dim foundNOT As Boolean = False

                While input.Length > 0
                    Dim c As Char = input(0)
                    Dim endIndex As Integer = 0

                    If c = "<" Then
                        Dim level As Integer = 0
                        input = input.Remove(0, 1)

                        For i = 0 To input.Length - 1
                            If input(i) = "<" Then
                                level += 1
                            End If
                            If input(i) = ">" Then
                                If level > 0 Then
                                    level -= 1
                                Else
                                    endIndex = i
                                    Exit For
                                End If
                            End If
                        Next

                        Dim arg As String = input.Substring(0, endIndex)
                        input = input.Remove(0, endIndex + 1)

                        Dim classValue As String = CStr(arg)

                        If classValue.StartsWith("not ") = True Then
                            classValue = classValue.Remove(0, 4)
                            foundNOT = True
                        End If

                        Dim mainClass As String = classValue.Remove(classValue.IndexOf("."))
                        Dim subClass As String = classValue.Remove(0, classValue.IndexOf(".") + 1)

                        Dim resultValue As Object = GetConstructReturnValue(mainClass, subClass)

                        If resultValue.Equals(DefaultNull) Then
                            Logger.Log(Logger.LogTypes.Warning, String.Format("No value was returned from a construct. mainclass: {0}; subclass: {1}", mainClass, subClass))
                            resultValue = arg
                        End If

                        If foundNOT = True Then
                            Dim bools() As String = {"false", "true"}
                            If bools.Contains(resultValue.ToString().ToLower()) = True Then
                                Select Case resultValue.ToString().ToLower()
                                    Case "false"
                                        resultValue = "true"
                                    Case "true"
                                        resultValue = "false"
                                End Select
                            End If
                        End If
                        foundNOT = False

                        output &= resultValue.ToString()
                    Else
                        output &= input(0)
                        input = input.Remove(0, 1)
                    End If
                End While

                Return output
            End If
            Return construct
        End Function

        ''' <summary>
        ''' Returns a SubClass and Argument as Pair.
        ''' </summary>
        ''' <param name="inputString">The string to deconstruct. Example: command(argument)</param>
        Public Shared Function GetSubClassArgumentPair(ByVal inputString As String) As PairValue
            Dim p As New PairValue

            Dim command As String = inputString
            Dim argument As String = ""

            If command.Contains("(") = True And command.EndsWith(")") = True Then
                argument = command.Remove(0, command.IndexOf("(") + 1)
                argument = argument.Remove(argument.Length - 1, 1)
                command = command.Remove(command.IndexOf("("))
            End If

            argument = CStr(EvaluateConstruct(argument))

            p.Command = command
            p.Argument = argument

            Return p
        End Function

        ''' <summary>
        ''' Returns a result for a construct.
        ''' </summary>
        ''' <param name="mainClass">The main class of the contruct.</param>
        ''' <param name="subClass">The sub class of the construct.</param>
        Private Shared Function GetConstructReturnValue(ByVal mainClass As String, ByVal subClass As String) As Object
            Select Case mainClass.ToLower()
                Case "pokemon"
                    Return DoPokemon(subClass)
                Case "overworldpokemon"
                    Return DoOverworldPokemon(subClass)
                Case "player"
                    Return DoPlayer(subClass)
                Case "environment"
                    Return DoEnvironment(subClass)
                Case "register"
                    Return DoRegister(subClass)
                Case "system"
                    Return DoSystem(subClass)
                Case "npc"
                    Return DoNPC(subClass)
                Case "inventory"
                    Return DoInventory(subClass)
                Case "storage"
                    Return DoStorage(subClass)
                Case "phone"
                    Return DoPhone(subClass)
                Case "entity"
                    Return DoEntity(subClass)
                Case "level"
                    Return DoLevel(subClass)
                Case "battle"
                    Return DoBattle(subClass)
                Case "daycare"
                    Return DoDaycare(subClass)
                Case "rival"
                    Return DoRival(subClass)
                Case "math"
                    Return DoMath(subClass)
                Case "pokedex"
                    Return DoPokedex(subClass)
                Case "radio"
                    Return DoRadio(subClass)
                Case "camera"
                    Return DoCamera(subClass)
                Case "filesystem"
                    Return DoFileSystem(subClass)
            End Select
            Return DefaultNull
        End Function

        ''' <summary>
        ''' Returns a string for a boolean.
        ''' </summary>
        ''' <param name="bool">The boolean to convert.</param>
        Public Shared Function ReturnBoolean(ByVal bool As Boolean) As String
            If bool = True Then
                Return "true"
            Else
                Return "false"
            End If
        End Function


        '//////////////////////////////////////////////////////////
        '//
        '// Shortens the ScriptConversion methods to shorter names.
        '//
        '//////////////////////////////////////////////////////////

        Private Shared Function int(ByVal expression As Object) As Integer
            Return ScriptConversion.ToInteger(expression)
        End Function

        Private Shared Function sng(ByVal expression As Object) As Double
            Return ScriptConversion.ToSingle(expression)
        End Function

        Private Shared Function dbl(ByVal expression As Object) As Double
            Return ScriptConversion.ToDouble(expression)
        End Function

    End Class

End Namespace
