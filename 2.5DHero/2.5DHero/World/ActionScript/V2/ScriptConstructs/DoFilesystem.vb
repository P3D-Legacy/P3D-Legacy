Namespace ScriptVersion2

    Partial Class ScriptComparer

        '--------------------------------------------------------------------------------------------------------------------------
        'Contains the <filesystem> constructs.
        '--------------------------------------------------------------------------------------------------------------------------

        Private Shared Function DoFileSystem(ByVal subClass As String) As Object
            Dim command As String = GetSubClassArgumentPair(subClass).Command
            Dim argument As String = GetSubClassArgumentPair(subClass).Argument

            Select Case command.ToLower()
                Case "pathsplit"
                    Dim index As Integer = int(argument.Remove(argument.IndexOf(",")))
                    Dim folderpath As String = argument.Remove(0, argument.IndexOf(",") + 1)

                    Dim folderSplits() As String = folderpath.Split(CChar("\"))

                    Return folderSplits(index)
                Case "pathsplitcount"
                    Return argument.Split(CChar("\")).Length
                Case "pathup"
                    Dim folderPath As String = argument
                    If folderPath.Contains("\") = True Then
                        Return folderPath.Remove(folderPath.LastIndexOf("\"))
                    End If
            End Select

            Return DEFAULTNULL
        End Function

    End Class

End Namespace