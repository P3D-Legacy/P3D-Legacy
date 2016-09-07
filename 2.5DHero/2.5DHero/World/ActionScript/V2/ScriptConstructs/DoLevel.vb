Namespace ScriptVersion2

    Partial Class ScriptComparer

        '--------------------------------------------------------------------------------------------------------------------------
        'Contains the <level> constructs.
        '--------------------------------------------------------------------------------------------------------------------------

        Private Shared Function DoLevel(ByVal subClass As String) As Object
            Dim command As String = GetSubClassArgumentPair(subClass).Command
            Dim argument As String = GetSubClassArgumentPair(subClass).Argument

            Select Case command.ToLower()
                Case "mapfile", "levelfile"
                    Return Screen.Level.LevelFile
                Case "filename"
                    Dim filename As String = System.IO.Path.GetFileNameWithoutExtension(Screen.Level.LevelFile)
                    Return filename
                Case "riding"
                    Return ReturnBoolean(Screen.Level.Riding)
                Case "surfing"
                    Return ReturnBoolean(Screen.Level.Surfing)
            End Select

            Return DEFAULTNULL
        End Function

    End Class

End Namespace