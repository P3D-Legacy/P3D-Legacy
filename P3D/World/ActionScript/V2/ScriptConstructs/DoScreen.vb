Namespace ScriptVersion2

    Partial Class ScriptComparer

        ' --------------------------------------------------------------------------------------------------------------------------
        ' Contains the <screen> constructs.
        ' --------------------------------------------------------------------------------------------------------------------------

        Private Shared Function DoScreen(ByVal subClass As String) As Object
            Dim command As String = GetSubClassArgumentPair(subClass).Command
            Dim argument As String = GetSubClassArgumentPair(subClass).Argument

            Select Case command.ToLower(Globalization.CultureInfo.InvariantCulture)
                Case "selectedskin"
                    Return Screens.MainMenu.NewNewGameScreen.CharacterSelectionScreen.SelectedSkin
            End Select

            Return DefaultNull
        End Function

    End Class

End Namespace