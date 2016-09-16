Namespace ScriptVersion2

    Partial Class ScriptCommander

        '--------------------------------------------------------------------------------------------------------------------------
        'Contains the @storage commands.
        '--------------------------------------------------------------------------------------------------------------------------

        Private Shared Sub DoStorage(ByVal subClass As String)
            Dim command As String = ScriptComparer.GetSubClassArgumentPair(subClass).Command
            Dim argument As String = ScriptComparer.GetSubClassArgumentPair(subClass).Argument

            Select Case command.ToLower()
                Case "set"
                    Dim type As String = argument.GetSplit(0)
                    Dim name As String = argument.GetSplit(1)
                    Dim value As String = argument.Remove(0, type.Length + name.Length + 2)

                    ScriptStorage.SetObject(type, name, value)
                Case "update"
                    Dim type As String = argument.GetSplit(0)
                    Dim name As String = argument.GetSplit(1)
                    Dim operation As String = argument.GetSplit(2)
                    Dim value As String = argument.Remove(0, type.Length + name.Length + operation.Length + 3)

                    Dim currentValue As String = ScriptStorage.GetObject(type, name).ToString()

                    If IsNumeric(value.Replace(".", GameController.DecSeparator)) = True And IsNumeric(currentValue.Replace(".", GameController.DecSeparator)) = True Then
                        Select Case operation.ToLower()
                            Case "+", "plus", "add", "addition"
                                If ScriptConversion.IsArithmeticExpression(currentValue) = True And ScriptConversion.IsArithmeticExpression(value) = True Then
                                    value = dbl(currentValue.Replace(".", GameController.DecSeparator) & "+" & value.Replace(".", GameController.DecSeparator)).ToString()
                                Else
                                    value = currentValue & value
                                End If
                            Case "-", "minus", "subtract", "subtraction"
                                value = dbl(currentValue.Replace(".", GameController.DecSeparator) & "-" & value.Replace(".", GameController.DecSeparator)).ToString()
                            Case "*", "multiply", "multiplication"
                                value = dbl(currentValue.Replace(".", GameController.DecSeparator) & "*" & value.Replace(".", GameController.DecSeparator)).ToString()
                            Case "/", ":", "divide", "division"
                                value = dbl(currentValue.Replace(".", GameController.DecSeparator) & "/" & value.Replace(".", GameController.DecSeparator)).ToString()
                        End Select

                        ScriptStorage.SetObject(type, name, value)
                    Else
                        ScriptStorage.SetObject(type, name, currentValue & value)
                    End If
                Case "clear"
                    ScriptStorage.Clear()
            End Select

            IsReady = True
        End Sub

    End Class

End Namespace
