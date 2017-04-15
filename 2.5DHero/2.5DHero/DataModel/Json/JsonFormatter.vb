Namespace DataModel.Json

    ''' <summary>
    ''' A class to handle Json formatting.
    ''' </summary>
    Public Class JsonFormatter

        ''' <summary>
        ''' Sanitizes unformatted Json.
        ''' </summary>
        ''' <param name="json">The unformatted Json.</param>
        ''' <param name="space">The characters which are used to indent.</param>
        ''' <returns></returns>
        Public Shared Function FormatMultiline(ByVal json As String, ByVal space As String) As String
            If json.Contains(vbNewLine) = True Then
                'The input Json already contains new lines.
                Return json
            End If

            Dim sb As New Text.StringBuilder()
            Dim isString As Boolean = False
            Dim isEscaped As Boolean = False
            Dim indentLevel As Integer = 0

            For Each c As Char In json
                If isString = True Then
                    Select Case c
                        Case """"c
                            If isEscaped = False Then
                                isString = False
                            Else
                                isEscaped = False
                            End If
                            sb.Append(c)
                        Case "\"c
                            If isEscaped = False Then
                                isEscaped = True
                            Else
                                isEscaped = False
                                sb.Append(c)
                                sb.Append(c)
                            End If
                        Case Else
                            If isEscaped = False Then
                                sb.Append(c)
                            Else
                                isEscaped = False
                            End If
                    End Select
                Else
                    Select Case c
                        Case "}"c, "]"c
                            indentLevel -= 1
                            sb.AppendLine()
                            AppendIndent(sb, indentLevel, space)
                            sb.Append(c)
                        Case "{"c, "["c
                            indentLevel += 1
                            sb.Append(c)
                            sb.AppendLine()
                            AppendIndent(sb, indentLevel, space)
                        Case ","c
                            sb.Append(c)
                            sb.AppendLine()
                            AppendIndent(sb, indentLevel, space)
                        Case """"c
                            sb.Append(c)
                            isString = True
                        Case Else
                            sb.Append(c)
                    End Select
                End If
            Next

            Return sb.ToString()
        End Function

        Private Shared Sub AppendIndent(ByRef sb As Text.StringBuilder, ByVal level As Integer, ByVal space As String)
            If level > 0 Then
                For i = 1 To level
                    sb.Append(space)
                Next
            End If
        End Sub

    End Class

End Namespace