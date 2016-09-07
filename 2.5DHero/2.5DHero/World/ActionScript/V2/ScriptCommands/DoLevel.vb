Namespace ScriptVersion2

    Partial Class ScriptCommander

        '--------------------------------------------------------------------------------------------------------------------------
        'Contains the @level commands.
        '--------------------------------------------------------------------------------------------------------------------------

        Private Shared Sub DoLevel(ByVal subClass As String)
            Dim command As String = ScriptComparer.GetSubClassArgumentPair(subClass).Command
            Dim argument As String = ScriptComparer.GetSubClassArgumentPair(subClass).Argument

            Select Case command.ToLower()
                Case "wait"
                    If IsNumeric(Value) = False Then
                        Value = argument
                    End If
                    If int(Value) > 0 Then
                        Value = CStr(int(Value) - 1)
                    Else
                        Value = ""
                        IsReady = True
                    End If
                Case "waitforsave"
                    Dim doWait As Boolean = False

                    If Core.Player.IsGameJoltSave = True Then
                        If SaveGameHelpers.GameJoltSaveDone() = False Then
                            doWait = True
                        Else
                            SaveGameHelpers.ResetSaveCounter()
                        End If
                    End If

                    If doWait = False Then
                        IsReady = True
                    End If
                Case "update"
                    Screen.Level.Update()
                    Screen.Level.UpdateEntities()
                    Screen.Camera.Update()

                    IsReady = True
                Case "waitforevents"
                    Dim doWait As Boolean = False
                    For Each e As Entity In Screen.Level.Entities
                        If e.EntityID = "NPC" Then
                            If CType(e, NPC).MoveAsync = True And CType(e, NPC).Moved <> 0.0F Then
                                doWait = True
                                Exit For
                            End If
                        End If
                    Next

                    If doWait = False Then
                        IsReady = True
                    End If
                Case "reload"
                    Screen.Level.WarpData.WarpDestination = Screen.Level.LevelFile
                    Screen.Level.WarpData.WarpPosition = Screen.Camera.Position
                    Screen.Level.WarpData.WarpRotations = 0
                    Screen.Level.WarpData.DoWarpInNextTick = True
                    Screen.Level.WarpData.CorrectCameraYaw = Screen.Camera.Yaw
                    IsReady = True
                Case "setsafari"
                    Screen.Level.IsSafariZone = CBool(argument)
            End Select
        End Sub

    End Class

End Namespace