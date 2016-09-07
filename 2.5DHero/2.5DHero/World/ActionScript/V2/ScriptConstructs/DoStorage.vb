Namespace ScriptVersion2

    Partial Class ScriptComparer

        '--------------------------------------------------------------------------------------------------------------------------
        'Contains the <storage> constructs.
        '--------------------------------------------------------------------------------------------------------------------------

        Private Shared Function DoStorage(ByVal subClass As String) As Object
            Dim command As String = GetSubClassArgumentPair(subClass).Command
            Dim argument As String = GetSubClassArgumentPair(subClass).Argument

            Select Case command.ToLower()
                Case "get"
                    Dim type As String = argument.Remove(argument.IndexOf(","))
                    Dim name As String = argument.Remove(0, argument.IndexOf(",") + 1)

                    Return ScriptStorage.GetObject(type, name)
                Case "count"
                    Return ScriptStorage.Count(argument)
            End Select

            Return DEFAULTNULL
        End Function

    End Class

End Namespace