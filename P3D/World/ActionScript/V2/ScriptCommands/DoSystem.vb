Namespace ScriptVersion2

    Partial Class ScriptCommander

        ' --------------------------------------------------------------------------------------------------------------------------
        ' Contains the @system commands.
        ' --------------------------------------------------------------------------------------------------------------------------

        Private Shared Sub DoSystem(ByVal subClass As String)
            Dim command As String = ScriptComparer.GetSubClassArgumentPair(subClass).Command
            Dim argument As String = ScriptComparer.GetSubClassArgumentPair(subClass).Argument

            Select Case command.ToLower(Globalization.CultureInfo.InvariantCulture)
                Case "endnewgame"
                    Dim args As String() = argument.Split(","c)
                    Screens.MainMenu.NewNewGameScreen.EndNewGame(args(0), sng(args(1)), sng(args(2)), sng(args(3)), int(args(4)))
                    IsReady = True
                Case Else
                    IsReady = True
            End Select
        End Sub

    End Class

End Namespace