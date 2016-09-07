Namespace ScriptVersion2

    Partial Class ScriptComparer

        '--------------------------------------------------------------------------------------------------------------------------
        'Contains the <rival> constructs.
        '--------------------------------------------------------------------------------------------------------------------------

        Private Shared Function DoRival(ByVal subClass As String) As Object
            Dim command As String = GetSubClassArgumentPair(subClass).Command
            Dim argument As String = GetSubClassArgumentPair(subClass).Argument

            Select Case command.ToLower()
                Case "name"
                    Return Core.Player.RivalName
            End Select

            Return DEFAULTNULL
        End Function

    End Class

End Namespace