Namespace ScriptVersion2

    Partial Class ScriptComparer

        ' --------------------------------------------------------------------------------------------------------------------------
        ' Contains the <camera> constructs.
        ' --------------------------------------------------------------------------------------------------------------------------

        Private Shared Function DoCamera(ByVal subClass As String) As Object
            Dim command As String = GetSubClassArgumentPair(subClass).Command
            Dim argument As String = GetSubClassArgumentPair(subClass).Argument

            Dim c As OverworldCamera = CType(Screen.Camera, OverworldCamera)

            Dim position As Vector3
            If Core.CurrentScreen.Identification = Screen.Identifications.NewGameScreen Then
                position = c.Position
            Else
                position = c.ThirdPersonOffset
            End If
            Select Case command.ToLower(Globalization.CultureInfo.InvariantCulture)
                Case "isfixed"
                    Return ReturnBoolean(c.Fixed)
                Case "x"
                    Return position.X.ToString().ReplaceDecSeparator()
                Case "y"
                    Return position.Y.ToString().ReplaceDecSeparator()
                Case "z"
                    Return position.Z.ToString().ReplaceDecSeparator()
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