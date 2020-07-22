Namespace ScriptVersion2

    Partial Class ScriptCommander

        ' --------------------------------------------------------------------------------------------------------------------------
        ' Contains the @npc commands.
        ' --------------------------------------------------------------------------------------------------------------------------

        Private Shared Sub DoNPC(ByVal subClass As String)
            Dim command As String = ScriptComparer.GetSubClassArgumentPair(subClass).Command
            Dim argument As String = ScriptComparer.GetSubClassArgumentPair(subClass).Argument

            Select Case command.ToLower()
                Case "remove"
                    Dim targetNPC As Entity = Screen.Level.GetNPC(int(argument))

                    Screen.Level.Entities.Remove(targetNPC)

                    IsReady = True
                Case "position", "warp"
                    Dim targetNPC As NPC = Screen.Level.GetNPC(int(argument.GetSplit(0)))

                    Dim PositionData() As String = argument.Split(CChar(","))
                    targetNPC.Position = New Vector3(sng(PositionData(1).Replace("~", CStr(targetNPC.Position.X)).Replace(".", GameController.DecSeparator)),
                                                             sng(PositionData(2).Replace("~", CStr(targetNPC.Position.Y)).Replace(".", GameController.DecSeparator)),
                                                             sng(PositionData(3).Replace("~", CStr(targetNPC.Position.Z)).Replace(".", GameController.DecSeparator)))
                    targetNPC.CreatedWorld = False

                    If targetNPC.InCameraFocus() = True Then
                        Screen.Camera.Update()
                    End If

                    IsReady = True
                Case "addtoposition"
                    Dim targetNPC As NPC = Screen.Level.GetNPC(int(argument.GetSplit(0)))

                    Dim PositionData() As String = argument.Split(CChar(","))
                    targetNPC.Position += New Vector3(sng(PositionData(1).Replace("~", CStr(targetNPC.Position.X)).Replace(".", GameController.DecSeparator)),
                                                         sng(PositionData(2).Replace("~", CStr(targetNPC.Position.Y)).Replace(".", GameController.DecSeparator)),
                                                         sng(PositionData(3).Replace("~", CStr(targetNPC.Position.Z)).Replace(".", GameController.DecSeparator)))
                    targetNPC.CreatedWorld = False

                    If targetNPC.InCameraFocus() = True Then
                        Screen.Camera.Update()
                    End If
                    IsReady = True
                Case "register"
                    NPC.AddNPCData(argument)

                    IsReady = True
                Case "unregister"
                    NPC.RemoveNPCData(argument)

                    IsReady = True
                Case "wearskin"
                    Dim targetNPC As NPC = Screen.Level.GetNPC(int(argument.GetSplit(0)))
                    Dim textureID As String = argument.GetSplit(1)

                    targetNPC.SetupSprite(textureID, "", False)
                    IsReady = True
                Case "setonlineskin"
                    Dim targetNPC As NPC = Screen.Level.GetNPC(int(argument.GetSplit(0)))
                    Dim GameJoltID As String = argument.GetSplit(1)

                    If GameJoltID <> "" Then
                        targetNPC.SetupSprite(targetNPC.TextureID, GameJoltID, True)
                    End If
                    IsReady = True
                Case "move"
                    Dim targetNPC As NPC = Screen.Level.GetNPC(int(argument.GetSplit(0)))
                    Dim steps As Integer = int(argument.GetSplit(1))

                    Screen.Level.UpdateEntities()
                    If ScriptV2.started = False Then
                        If steps < 0 Then
                            If targetNPC.Speed > 0 Then
                                targetNPC.Speed *= -1
                            End If
                        Else
                            If targetNPC.Speed < 0 Then
                                targetNPC.Speed *= -1
                            End If
                        End If

                        steps = steps.ToPositive()

                        targetNPC.Moved += steps
                        ScriptV2.started = True
                    Else
                        If targetNPC.Moved <= 0.0F Then
                            If targetNPC.Speed < 0 Then
                                targetNPC.Speed *= -1
                            End If
                            IsReady = True
                        Else
                            If targetNPC.InCameraFocus() = True Then
                                Screen.Camera.Update()
                            End If
                        End If
                    End If
                Case "setmovey"
                    Dim targetNPC As NPC = Screen.Level.GetNPC(int(argument.GetSplit(0)))
                    Dim steps As Integer = int(argument.GetSplit(1))

                    targetNPC.MoveY = steps

                    IsReady = True
                Case "moveasync"
                    Dim targetNPC As NPC = Screen.Level.GetNPC(int(argument.GetSplit(0)))
                    Dim steps As Integer = int(argument.GetSplit(1))

                    Screen.Level.UpdateEntities()
                    If steps < 0 Then
                        If targetNPC.Speed > 0 Then
                            targetNPC.Speed *= -1
                        End If
                    Else
                        If targetNPC.Speed < 0 Then
                            targetNPC.Speed *= -1
                        End If
                    End If

                    steps = steps.ToPositive()

                    targetNPC.Moved += steps
                    targetNPC.MoveAsync = True

                    IsReady = True
                Case "dance"
                    Dim targetNPC As NPC = Screen.Level.GetNPC(int(argument.GetSplit(0)))
                    Dim steps As Integer = int(argument.GetSplit(1))

                    Screen.Level.UpdateEntities()
                    targetNPC.isDancing = True
                    If ScriptV2.started = False Then
                        If steps < 0 Then
                            If targetNPC.Speed > 0 Then
                                targetNPC.Speed *= -1
                            End If
                        Else
                            If targetNPC.Speed < 0 Then
                                targetNPC.Speed *= -1
                            End If
                        End If

                        steps = steps.ToPositive()

                        targetNPC.Moved += steps
                        ScriptV2.started = True
                    Else
                        If targetNPC.Moved <= 0.0F Then
                            If targetNPC.Speed < 0 Then
                                targetNPC.Speed *= -1
                            End If
                            IsReady = True
                        Else
                            If targetNPC.InCameraFocus() = True Then
                                Screen.Camera.Update()
                            End If
                        End If
                    End If
                Case "danceasync"

                    Dim targetNPC As NPC = Screen.Level.GetNPC(int(argument.GetSplit(0)))
                    Dim steps As Integer = int(argument.GetSplit(1))

                    Screen.Level.UpdateEntities()

                    targetNPC.isDancing = True
                    If steps < 0 Then
                        If targetNPC.Speed > 0 Then
                            targetNPC.Speed *= -1
                        End If
                    Else
                        If targetNPC.Speed < 0 Then
                            targetNPC.Speed *= -1
                        End If
                    End If

                    steps = steps.ToPositive()

                    targetNPC.Moved += steps
                    targetNPC.MoveAsync = True

                    IsReady = True
                Case "turn"
                    Dim targetNPC As NPC = Screen.Level.GetNPC(int(argument.GetSplit(0)))

                    targetNPC.faceRotation = int(argument.GetSplit(1))
                    targetNPC.Update()
                    targetNPC.UpdateEntity()
                    IsReady = True
                Case "spawn"
                    Dim args() As String = argument.Split(CChar(","))

                    ' Required parameters: position
                    ' Optional parameters: actionvalue, additionalvalue, textureid, animateidle, rotation, name, id, movement

                    Dim position As Vector3 = New Vector3(sng(args(0)), sng(args(1)), sng(args(2)))
                    Dim actionValue As Integer = 0
                    Dim additionalValue As String = ""
                    Dim TextureID As String = "0"
                    Dim AnimateIdle As Boolean = False
                    Dim Rotation As Integer = 0
                    Dim Name As String = ""
                    Dim ID As Integer = 0
                    Dim Movement As String = "Still"
                    Dim MoveRectangles As New List(Of Rectangle)

                    If args.Count >= 4 Then
                        actionValue = int(args(3))
                        If args.Count >= 5 Then
                            additionalValue = args(4)
                            If args.Count >= 6 Then
                                TextureID = args(5)
                                If args.Count >= 7 Then
                                    AnimateIdle = CBool(args(6))
                                    If args.Count >= 8 Then
                                        Rotation = int(args(7))
                                        If args.Count >= 9 Then
                                            Name = args(8)
                                            If args.Count >= 10 Then
                                                ID = int(args(9))
                                                If args.Count >= 11 Then
                                                    Movement = args(10)
                                                End If
                                            End If
                                        End If
                                    End If
                                End If
                            End If
                        End If
                    End If

                    If Not Screen.Level.GetNPC(ID) Is Nothing Then
                        Logger.Log(Logger.LogTypes.Message, "ScriptCommander.vb: (@npc." & command & ") An NPC with the ID """ & ID & """ already exists.")
                    End If

                    Dim NPC As NPC = CType(Entity.GetNewEntity("NPC", position, {Nothing}, {0, 0}, True, New Vector3(0.0F), New Vector3(1.0F), BaseModel.BillModel, actionValue, additionalValue, True, New Vector3(1.0F), -1, Screen.Level.LevelFile, "", New Vector3(0), {TextureID, Rotation, Name, ID, AnimateIdle, Movement, MoveRectangles}), NPC)
                    Screen.Level.Entities.Add(NPC)

                    IsReady = True
                Case "setspeed"
                    If argument.CountSeperators(",") > 0 Then
                        Dim targetNPC As NPC = Screen.Level.GetNPC(int(argument.GetSplit(0)))
                        Dim speed As Single = sng(argument.GetSplit(1))

                        If Not targetNPC Is Nothing Then
                            targetNPC.Speed = speed * 0.04F
                        Else
                            Logger.Log(Logger.LogTypes.Warning, "ScriptCommander.vb: (@npc." & command & ") The targeted NPC with ID """ & int(argument.GetSplit(0)) & """ doesn't exist.")
                        End If
                    Else
                        Logger.Log(Logger.LogTypes.Warning, "ScriptCommander.vb: (@npc." & command & ") Invalid argument passed.")
                    End If

                    IsReady = True
                Case Else
                    Logger.Log(Logger.LogTypes.Warning, "ScriptCommander.vb: (@npc." & command & ") Command not found.")
                    IsReady = True
            End Select
        End Sub

    End Class

End Namespace