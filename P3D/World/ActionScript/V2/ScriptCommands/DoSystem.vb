Namespace ScriptVersion2

    Partial Class ScriptCommander

        ' --------------------------------------------------------------------------------------------------------------------------
        ' Contains the @system commands.
        ' --------------------------------------------------------------------------------------------------------------------------

        Private Shared Sub DoSystem(ByVal subClass As String)
            Dim command As String = ScriptComparer.GetSubClassArgumentPair(subClass).Command
            Dim argument As String = ScriptComparer.GetSubClassArgumentPair(subClass).Argument

            Select Case command.ToLower()
                Case "endnewgame"
                    Dim args As String() = argument.Split(","c)
                    Screens.MainMenu.NewNewGameScreen.EndNewGame(args(0), sng(args(1)), sng(args(2)), sng(args(3)), int(args(4)))
                    IsReady = True
                Case "replacetextures"
                    Dim path As String = argument.GetSplit(0, ",")
                    If argument.Split(",").Count = 1 OrElse (argument.Split(",").Count > 1 AndAlso CBool(argument.GetSplit(1, ",")) = True) Then
                        ContentPackManager.Load(GameController.GamePath & GameModeManager.ActiveGameMode.ContentPath & "Data\" & argument & ".dat", True)
                        Screen.Level.WarpData.WarpDestination = Screen.Level.LevelFile
                        Screen.Level.WarpData.WarpPosition = Screen.Camera.Position
                        Screen.Level.WarpData.WarpRotations = 0
                        Screen.Level.WarpData.DoWarpInNextTick = True
                        Screen.Level.WarpData.CorrectCameraYaw = Screen.Camera.Yaw
                        Screen.Level.WarpData.WarpSound = Nothing
                    End If
                    IsReady = True
                Case Else
                    IsReady = True
            End Select
        End Sub

    End Class

End Namespace