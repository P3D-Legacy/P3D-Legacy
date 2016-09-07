Namespace ScriptVersion2

    Partial Class ScriptComparer

        '--------------------------------------------------------------------------------------------------------------------------
        'Contains the <radio> constructs.
        '--------------------------------------------------------------------------------------------------------------------------

        Private Shared Function DoRadio(ByVal subClass As String) As Object
            Dim command As String = GetSubClassArgumentPair(subClass).Command
            Dim argument As String = GetSubClassArgumentPair(subClass).Argument

            Select Case command.ToLower()
                Case "currentchannel"
                    If Screen.Level.SelectedRadioStation Is Nothing Then
                        Return ""
                    Else
                        Return Screen.Level.SelectedRadioStation.Name
                    End If
            End Select

            Return DEFAULTNULL
        End Function

    End Class

End Namespace