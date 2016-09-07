Namespace ScriptVersion2

    Partial Class ScriptComparer

        '--------------------------------------------------------------------------------------------------------------------------
        'Contains the <math> constructs.
        '--------------------------------------------------------------------------------------------------------------------------

        Private Shared Function DoMath(ByVal subClass As String) As Object
            Dim command As String = GetSubClassArgumentPair(subClass).Command
            Dim argument As String = GetSubClassArgumentPair(subClass).Argument

            Select Case command.ToLower()
                Case "int"
                    Return int(argument)
                Case "sng"
                    Return sng(argument)
                Case "dbl"
                    Return dbl(argument)
                Case "abs"
                    Return Math.Abs(dbl(argument))
                Case "ceiling"
                    Return Math.Ceiling(dbl(argument))
                Case "floor"
                    Return Math.Floor(dbl(argument))
                Case "isint", "issng", "isdbl"
                    Return ReturnBoolean(ScriptConversion.IsArithmeticExpression(argument))
                Case "clamp"
                    Dim args() As String = argument.Split(CChar(","))
                    Dim n As Double = dbl(args(0))
                    Dim min As Double = dbl(args(1))
                    Dim max As Double = dbl(args(2))

                    Return n.Clamp(min, max)
                Case "rollover"
                    Dim args() As String = argument.Split(CChar(","))
                    Dim n As Double = dbl(args(0))
                    Dim min As Double = dbl(args(1))
                    Dim max As Double = dbl(args(2))

                    Dim diff As Double = (max - min) + 1

                    If n > max Then
                        While n > max
                            n -= diff
                        End While
                    ElseIf n < min Then
                        While n < min
                            n += diff
                        End While
                    End If

                    Return n
            End Select

            Return DEFAULTNULL
        End Function

    End Class

End Namespace