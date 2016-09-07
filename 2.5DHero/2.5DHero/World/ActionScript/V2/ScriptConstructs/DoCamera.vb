Namespace ScriptVersion2

    Partial Class ScriptComparer

        '--------------------------------------------------------------------------------------------------------------------------
        'Contains the <camera> constructs.
        '--------------------------------------------------------------------------------------------------------------------------

        Private Shared Function DoCamera(ByVal subClass As String) As Object
            Dim command As String = GetSubClassArgumentPair(subClass).Command
            Dim argument As String = GetSubClassArgumentPair(subClass).Argument

            Dim c As OverworldCamera = CType(Screen.Camera, OverworldCamera)

            Select Case command.ToLower()
                Case "isfixed"
                    Return ReturnBoolean(c.Fixed)
                Case "x"
                    Return c.ThirdPersonOffset.X.ToString().ReplaceDecSeparator()
                Case "y"
                    Return c.ThirdPersonOffset.Y.ToString().ReplaceDecSeparator()
                Case "z"
                    Return c.ThirdPersonOffset.Z.ToString().ReplaceDecSeparator()
                Case "yaw"
                    Return c.Yaw.ToString().ReplaceDecSeparator()
                Case "pitch"
                    Return c.Pitch.ToString().ReplaceDecSeparator()
                Case "thirdperson"
                    Return ReturnBoolean(c.ThirdPerson)
            End Select

            Return DEFAULTNULL
        End Function

    End Class

End Namespace