Namespace Construct.Framework

    ''' <summary>
    ''' A class to convert the internal strings from scripts to other data types.
    ''' </summary>
    Public Class Converter

        ''' <summary>
        ''' Converts an expression into a <see cref="Double"/>.
        ''' </summary>
        ''' <param name="expression"></param>
        ''' <returns></returns>
        Public Shared Function ToDouble(ByVal expression As String) As Double
            Dim doubleResult As Double
            If Double.TryParse(expression.Replace(",", GameController.DecSeparator), doubleResult) = True Then
                Return doubleResult
            Else
                Dim parser As New MathParser(expression)
                If parser.IsValidExpression() = True Then
                    Return parser.Evaluate()
                Else
                    Logger.Debug("050", "Couldn't convert the expression " & expression & " into a number.")
                    Return 0D
                End If
            End If
        End Function

        ''' <summary>
        ''' Converts an expression into an <see cref="Integer"/>.
        ''' </summary>
        ''' <param name="expression">The expression to convert.</param>
        ''' <returns></returns>
        Public Shared Function ToInteger(ByVal expression As String) As Integer
            Return CInt(Math.Round(ToDouble(expression)))
        End Function

        ''' <summary>
        ''' Converts an expression to a <see cref="Single"/>.
        ''' </summary>
        ''' <param name="expression">The expression to convert.</param>
        ''' <returns></returns>
        Public Shared Function ToSingle(ByVal expression As String) As Single
            Return CSng(ToDouble(expression))
        End Function

        ''' <summary>
        ''' Returns if the input expression is a math expression.
        ''' </summary>
        ''' <param name="expression">The expression (infix).</param>
        ''' <returns></returns>
        Public Shared Function IsNumeric(ByVal expression As String) As Boolean
            If Information.IsNumeric(expression) = True Then
                Return True
            End If
            Return New MathParser(expression).IsValidExpression
        End Function

        ''' <summary>
        ''' Parses Infix notation expressions and evaluates them into a numeric value.
        ''' </summary>
        Private Class MathParser

            Private Enum Associativity
                Right
                Left
            End Enum

            Private _infix As String = ""
            Private _postfix As String = ""

            ''' <summary>
            ''' The Infix input.
            ''' </summary>
            ''' <returns></returns>
            Public ReadOnly Property Infix() As String
                Get
                    Return _infix
                End Get
            End Property

            ''' <summary>
            ''' Creates a new instance of the math parser class.
            ''' </summary>
            ''' <param name="infix">The math expression in Infix notation.</param>
            Public Sub New(ByVal infix As String)
                _infix = infix

                'Replace boolean literals:
                _infix = _infix.ToLower().Replace("true", "1")
                _infix = _infix.ToLower().Replace("false", "0")

                If _infix.StartsWith("-") = True Then
                    _infix = "0" & _infix
                End If
            End Sub

            ''' <summary>
            ''' If the expression passed into this instance is a valid math expression.
            ''' </summary>
            ''' <returns></returns>
            Public ReadOnly Property IsValidExpression() As Boolean
                Get
                    Dim retError As Boolean = False
                    Dim postfix As String = GetPostfix(retError)
                    If retError = False Then
                        Evaluate(retError)
                        Return retError = False
                    Else
                        Return False
                    End If
                End Get
            End Property

            ''' <summary>
            ''' Returns the reverse polish notation of the input infix.
            ''' </summary>
            ''' <param name="hasError">If the process runs into an error, it will return "0" and set this value to True.</param>
            ''' <returns></returns>
            Public Function GetPostfix(Optional ByRef hasError As Boolean = False) As String
                If _postfix <> "" Then
                    Return _postfix
                End If

                Dim tokens As List(Of Char) = _infix.ToCharArray().ToList()
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
                            Logger.Debug("051", "Cannot convert """ & _infix.ToString() & """ to an arithmetic expression.")
                            hasError = True
                            Return "0"
                        End If
                    End If
                End While

                While stack.Count > 0
                    If stack(0) = "("c Or stack(0) = ")"c Then
                        Logger.Debug("052", "Cannot convert """ & _infix.ToString() & """ to an arithmetic expression.")
                        hasError = True
                        Return "0"
                    Else
                        output &= stack(0).ToString() & " "
                        stack.RemoveAt(0)
                    End If
                End While

                _postfix = output
                Return _postfix
            End Function

            ''' <summary>
            ''' Evaluates the math expression.
            ''' </summary>
            ''' <param name="hasError">If the process runs into an error, it will return 0 and set this value to True.</param>
            ''' <returns></returns>
            Public Function Evaluate(Optional ByRef hasError As Boolean = False) As Double
                Dim stack As New List(Of Double)
                Dim tokens As List(Of Char) = GetPostfix().ToCharArray().ToList()

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
                                        Logger.Debug("053", "Cannot evaluate """ & _infix.ToString() & """ as an arithmetic expression.")
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
                            Logger.Debug("054", "Cannot evaluate """ & _infix.ToString() & """ as an arithmetic expression.")
                            hasError = True
                            Return 0
                        End If
                    End If
                End While

                If stack.Count = 1 Then
                    Return stack(0)
                Else
                    Logger.Debug("055", "Cannot evaluate """ & _infix.ToString() & """ as an arithmetic expression.")
                    hasError = True
                    Return 0
                End If
            End Function

            ''' <summary>
            ''' If the token is a number or part of one.
            ''' </summary>
            ''' <param name="token">The token.</param>
            ''' <returns></returns>
            Private Function IsNumber(ByVal token As Char) As Boolean
                Return "0123456789.,".ToCharArray().Contains(token)
            End Function

            ''' <summary>
            ''' If the token is an operator.
            ''' </summary>
            ''' <param name="token">The token.</param>
            ''' <returns></returns>
            Private Function IsOperator(ByVal token As Char) As Boolean
                Return "+-*/^%".ToCharArray().Contains(token)
            End Function

            ''' <summary>
            ''' Returns the precedence for an operator.
            ''' </summary>
            ''' <param name="[Operator]">The operator.</param>
            ''' <returns></returns>
            Private Function GetPrecedence(ByVal [Operator] As Char) As Integer
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
            ''' <returns></returns>
            Private Function GetAssociativity(ByVal [Operator] As Char) As Associativity
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
            ''' <param name="expression"></param>
            ''' <param name="hasError"></param>
            ''' <returns></returns>
            Private Function InternalToDouble(ByVal expression As String, Optional ByRef hasError As Boolean = False) As Double
                expression = expression.Replace(".", GameController.DecSeparator)
                expression = expression.Replace(",", GameController.DecSeparator)

                If Information.IsNumeric(expression) = True Then
                    Return Convert.ToDouble(expression)
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

        End Class

#Region "Booleans"

        ''' <summary>
        ''' Converts an expression into a <see cref="Boolean"/>.
        ''' </summary>
        ''' <param name="expression">The expression to convert.</param>
        ''' <returns></returns>
        Public Shared Function ToBoolean(ByVal expression As String) As Boolean
            Dim bool As Boolean = False
            Dim revert As Boolean = False

            If expression.StartsWith("!") = True Then
                expression = expression.Remove(0, 1)
                revert = True
            End If

            Select Case expression.ToLower()
                Case "true", "1"
                    bool = True
                Case Else 'Anything other than the True literal and 1 return False.
                    bool = False
            End Select

            If revert = True Then
                bool = Not bool
            End If
            Return bool
        End Function

        ''' <summary>
        ''' Performs a check if an expression is a valid <see cref="Boolean"/>.
        ''' </summary>
        ''' <param name="expression">The expression to perform the check on.</param>
        ''' <returns></returns>
        Public Shared Function IsBoolean(ByVal expression As String) As Boolean
            Return {"0", "1", "true", "false"}.Contains(expression.ToLower())
        End Function

#End Region

    End Class

End Namespace