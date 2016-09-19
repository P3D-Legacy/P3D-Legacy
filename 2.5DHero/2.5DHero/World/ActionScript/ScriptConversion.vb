''' <summary>
''' Used to convert strings into various types.
''' </summary>
Public Class ScriptConversion

    Enum Associativity
        Right
        Left
    End Enum

    ''' <summary>
    ''' Converts an expression into a <see cref="Double"/>.
    ''' </summary>
    Public Shared Function ToDouble(ByVal expression As Object) As Double
        Dim input As String = expression.ToString()
        Dim retError As Boolean = False
        Dim retDbl As Double = 0D

        retDbl = InternalToDouble(input, retError)

        If retError = False Then
            Return retDbl
        Else
            If IsArithmeticExpression(expression) = True Then
                Dim postFix As String = ToPostfix(expression.ToString())

                Return EvaluatePostfix(postFix)
            Else
                Return 0D
            End If
        End If
    End Function

    ''' <summary>
    ''' If the input can be evaluated into a number.
    ''' </summary>
    Public Shared Function IsArithmeticExpression(ByVal expression As Object) As Boolean
        Dim retError As Boolean = False
        Dim postfix As String = ToPostfix(expression.ToString(), retError)
        If retError = False Then
            EvaluatePostfix(postfix, retError)
            Return retError = False
        Else
            Return False
        End If
    End Function

    ''' <summary>
    ''' Evaluates a math expression in the postfix notation. Example: 2 3 +
    ''' </summary>
    ''' <param name="input">The postfix string.</param>
    Private Shared Function EvaluatePostfix(ByVal input As String, Optional ByRef hasError As Boolean = False) As Double
        Dim stack As New List(Of Double)
        Dim tokens As List(Of Char) = input.ToCharArray().ToList()

        Dim cNumber As String = ""

        While tokens.Count > 0
            Dim token As Char = tokens(0)
            tokens.RemoveAt(0)

            If IsNumber(token) = True Then
                cNumber &= token.ToString()
            ElseIf cNumber.Length > 0 Then
                stack.Insert(0, InternalToDouble(cNumber))
                cNumber = ""
            End If

            If cNumber.Length > 0 And tokens.Count = 0 Then
                stack.Insert(0, InternalToDouble(cNumber))
                cNumber = ""
            End If

            If IsOperator(token) = True Then
                If stack.Count >= 2 Then
                    Dim v2 As Double = stack(0)
                    Dim v1 As Double = stack(1)

                    stack.RemoveAt(0)
                    stack.RemoveAt(0)

                    Dim result As Double = 0

                    Select Case token.ToString()
                        Case "+"
                            result = v1 + v2
                        Case "-"
                            result = v1 - v2
                        Case "*"
                            result = v1 * v2
                        Case "/"
                            If v2 = 0 Then
                                Logger.Log(Logger.LogTypes.Warning, "Script.vb: Cannot evaluate """ & input.ToString() & """ as an arithmetic expression.")
                                hasError = True
                                Return 0
                            Else
                                result = v1 / v2
                            End If
                        Case "^"
                            result = v1 ^ v2
                        Case "%"
                            result = v1 Mod v2
                    End Select

                    stack.Insert(0, result)
                Else
                    Logger.Log(Logger.LogTypes.Warning, "Script.vb: Cannot evaluate """ & input.ToString() & """ as an arithmetic expression.")
                    hasError = True
                    Return 0
                End If
            End If
        End While

        If stack.Count = 1 Then
            Return stack(0)
        Else
            Logger.Log(Logger.LogTypes.Warning, "Script.vb: Cannot evaluate """ & input.ToString() & """ as an arithmetic expression.")
            hasError = True
            Return 0
        End If
    End Function

    ''' <summary>
    ''' Converts an infix notation to postfix notation.
    ''' </summary>
    ''' <param name="input">The infix notation. Example: 2+3</param>
    Private Shared Function ToPostfix(ByVal input As String, Optional ByRef hasError As Boolean = False) As String
        If input.Trim().StartsWith("-") Then
            input = "0" & input
        End If

        Dim tokens As List(Of Char) = input.ToCharArray().ToList()
        Dim stack As New List(Of Char)

        Dim output As String = ""
        Dim cNumber As String = ""

        While tokens.Count > 0
            Dim token As Char = tokens(0)
            tokens.RemoveAt(0)

            'Token is a number:
            If IsNumber(token) = True Then
                cNumber &= token.ToString()
            ElseIf cNumber.Length > 0 Then
                output &= cNumber.ToString() & " " 'Add to the output
                cNumber = ""
            End If

            If cNumber.Length > 0 And tokens.Count = 0 Then
                output &= cNumber.ToString() & " "
                cNumber = ""
            End If

            'Token is an operator
            If IsOperator(token) = True Then
                Dim o1 As Char = token

                While stack.Count > 0 AndAlso IsOperator(stack(0)) = True AndAlso ((GetAssociativity(o1) = Associativity.Left And GetPrecedence(o1) <= GetPrecedence(stack(0))) Or (GetAssociativity(o1) = Associativity.Right And GetPrecedence(o1) < GetPrecedence(stack(0))))
                    output &= stack(0).ToString() & " "
                    stack.RemoveAt(0)
                End While

                stack.Insert(0, o1)
            End If
            'Token is a left parenthesis
            If token = "("c Then
                stack.Insert(0, token)
            End If
            'Token is a right parenthesis
            If token = ")"c Then
                If stack.Count > 0 Then
                    While stack.Count > 0
                        If stack(0) = "("c Then
                            stack.RemoveAt(0)
                            Exit While
                        Else
                            output &= stack(0).ToString() & " "
                            stack.RemoveAt(0)
                        End If
                    End While
                Else
                    Logger.Log(Logger.LogTypes.Warning, "Script.vb: Cannot convert """ & input.ToString() & """ to an arithmetic expression.")
                    hasError = True
                    Return "0"
                End If
            End If
        End While

        While stack.Count > 0
            If stack(0) = "("c Or stack(0) = ")"c Then
                Logger.Log(Logger.LogTypes.Warning, "Script.vb: Cannot convert """ & input.ToString() & """ to an arithmetic expression.")
                hasError = True
                Return "0"
            Else
                output &= stack(0).ToString() & " "
                stack.RemoveAt(0)
            End If
        End While

        Return output
    End Function

    ''' <summary>
    ''' If the token is a number or part of one.
    ''' </summary>
    ''' <param name="token">The token.</param>
    Private Shared Function IsNumber(ByVal token As Char) As Boolean
        Return "0123456789.,".ToCharArray().Contains(token)
    End Function

    ''' <summary>
    ''' If the token is an operator.
    ''' </summary>
    ''' <param name="token">The token.</param>
    Private Shared Function IsOperator(ByVal token As Char) As Boolean
        Return "+-*/^%".ToCharArray().Contains(token)
    End Function

    ''' <summary>
    ''' Returns the precedence for an operator.
    ''' </summary>
    ''' <param name="[Operator]">The operator.</param>
    Private Shared Function GetPrecedence(ByVal [Operator] As Char) As Integer
        Select Case [Operator]
            Case "+"c, "-"c
                Return 2
            Case "*"c, "/"c, "%"c
                Return 3
            Case "^"c
                Return 4
        End Select

        Return -1
    End Function

    ''' <summary>
    ''' Returns if an operator has a Left or Right Associativity.
    ''' </summary>
    ''' <param name="[Operator]">The operator</param>
    Private Shared Function GetAssociativity(ByVal [Operator] As Char) As Associativity
        Select Case [Operator]
            Case "^"c
                Return Associativity.Right
            Case Else
                Return Associativity.Left
        End Select
    End Function

    ''' <summary>
    ''' Tries to convert a single number into a <see cref="Double"/>.
    ''' </summary>
    Private Shared Function InternalToDouble(ByVal expression As String, Optional ByRef hasError As Boolean = False) As Double
        expression = expression.Replace(".", My.Application.Culture.NumberFormat.NumberDecimalSeparator)

        If IsNumeric(expression) = True Then
            Return System.Convert.ToDouble(expression)
        Else
            If expression.ToLower() = "false" Then
                Return 0
            ElseIf expression.ToLower() = "true" Then
                Return 1
            Else
                hasError = True
                Return 0
            End If
        End If
    End Function

    ''' <summary>
    ''' Converts an expression into an <see cref="Integer"/>.
    ''' </summary>
    ''' <param name="expression">The expression to convert.</param>
    Public Shared Function ToInteger(ByVal expression As Object) As Integer
        Return CInt(Math.Round(ToDouble(expression)))
    End Function

    ''' <summary>
    ''' Converts an expression to a <see cref="Single"/>.
    ''' </summary>
    ''' <param name="expression">The expression to convert.</param>
    Public Shared Function ToSingle(ByVal expression As Object) As Single
        Return CSng(ToDouble(expression))
    End Function

    ''' <summary>
    ''' Converts an expression into a <see cref="Boolean"/>.
    ''' </summary>
    ''' <param name="expression">The expression to convert.</param>
    Public Shared Function ToBoolean(ByVal expression As Object) As Boolean
        Select Case expression.ToString().ToLower()
            Case "true", "1"
                Return True
            Case Else
                Return False
        End Select
    End Function

    ''' <summary>
    ''' Performs a check if an expression is a valid <see cref="Boolean"/>.
    ''' </summary>
    ''' <param name="expression">The expression to perform the check on.</param>
    Public Shared Function IsBoolean(ByVal expression As Object) As Boolean
        Dim s As String = expression.ToString()
        Dim validBools() As String = {"0", "1", "true", "false"}
        Return validBools.Contains(s.ToLower())
    End Function

End Class
