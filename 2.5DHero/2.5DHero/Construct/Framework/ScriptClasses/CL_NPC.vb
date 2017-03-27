Namespace Construct.Framework.Classes

    <ScriptClass("NPC")>
    <ScriptDescription("A class to handle NPC actions.")>
    Public Class CL_NPC

        Inherits ScriptClass

        Public Sub New()
            MyBase.New(True)
        End Sub

#Region "Commands"

        <ScriptCommand("Remove")>
        <ScriptDescription("Removes an NPC from the map.")>
        Private Function M_Remove(ByVal argument As String) As String
            Dim targetNPC As Entity = Screen.Level.GetNPC(Int(argument))

            Screen.Level.Entities.Remove(targetNPC)

            Return Core.Null
        End Function

        <ScriptCommand("Warp")>
        <ScriptDescription("Changes the position of an NPC.")>
        Private Function M_Warp(ByVal argument As String) As String
            Dim targetNPC As NPC = Screen.Level.GetNPC(Int(argument.GetSplit(0)))

            Dim PositionData() As String = argument.Split(CChar(","))
            targetNPC.Position = New Vector3(Sng(PositionData(1).Replace("~", CStr(targetNPC.Position.X))),
                                             Sng(PositionData(2).Replace("~", CStr(targetNPC.Position.Y))),
                                             Sng(PositionData(3).Replace("~", CStr(targetNPC.Position.Z))))
            targetNPC.CreatedWorld = False

            If targetNPC.InCameraFocus() = True Then
                Screen.Camera.Update()
            End If

            Return Core.Null
        End Function

        <ScriptCommand("AddToPosition")>
        <ScriptDescription("Adds to the position of an NPC.")>
        Private Function M_AddToPosition(ByVal argument As String) As String
            Dim targetNPC As NPC = Screen.Level.GetNPC(Int(argument.GetSplit(0)))

            Dim PositionData() As String = argument.Split(CChar(","))
            targetNPC.Position += New Vector3(Sng(PositionData(1).Replace("~", CStr(targetNPC.Position.X))),
                                              Sng(PositionData(2).Replace("~", CStr(targetNPC.Position.Y))),
                                              Sng(PositionData(3).Replace("~", CStr(targetNPC.Position.Z))))
            targetNPC.CreatedWorld = False

            If targetNPC.InCameraFocus() = True Then
                Screen.Camera.Update()
            End If

            Return Core.Null
        End Function

        <ScriptCommand("Register")>
        <ScriptDescription("Registers an NPC save string.")>
        Private Function M_Register(ByVal argument As String) As String
            NPC.AddNPCData(argument)

            Return Core.Null
        End Function

        <ScriptCommand("Unregister")>
        <ScriptDescription("Unregisters an NPC save string.")>
        Private Function M_Unregister(ByVal argument As String) As String
            NPC.RemoveNPCData(argument)

            Return Core.Null
        End Function

        <ScriptCommand("WearSkin")>
        <ScriptDescription("Changes the skin of an NPC.")>
        Private Function M_WearSkin(ByVal argument As String) As String
            Dim targetNPC As NPC = Screen.Level.GetNPC(Int(argument.GetSplit(0)))
            Dim textureID As String = argument.GetSplit(1)

            targetNPC.SetupSprite(textureID, "", False)

            Return Core.Null
        End Function

        <ScriptCommand("SetOnlineSkin")>
        <ScriptDescription("Sets the skin of an NPC to an online sprite.")>
        Private Function M_SetOnlineSkin(ByVal argument As String) As String
            Dim targetNPC As NPC = Screen.Level.GetNPC(Int(argument.GetSplit(0)))
            Dim GameJoltID As String = argument.GetSplit(1)

            If GameJoltID <> "" Then
                targetNPC.SetupSprite(targetNPC.TextureID, GameJoltID, True)
            End If

            Return Core.Null
        End Function

        <ScriptCommand("Move")>
        <ScriptDescription("Moves an NPC.")>
        Private Function M_Move(ByVal argument As String) As String
            Dim targetNPC As NPC = Screen.Level.GetNPC(Int(argument.GetSplit(0)))
            Dim steps As Integer = Int(argument.GetSplit(1))

            Screen.Level.UpdateEntities()
            If ActiveLine.workValues.Count = 0 Then
                If steps < 0 Then
                    If targetNPC.Speed > 0 Then
                        targetNPC.Speed *= -1
                    End If
                Else
                    If targetNPC.Speed < 0 Then
                        targetNPC.Speed *= -1
                    End If
                End If

                steps = Math.Abs(steps)

                targetNPC.Moved += steps
                ActiveLine.workValues.Add("started")
                ActiveLine.Preserve = True
            Else
                If targetNPC.Moved <= 0.0F Then
                    If targetNPC.Speed < 0 Then
                        targetNPC.Speed *= -1
                    End If
                    ActiveLine.Preserve = False
                Else
                    If targetNPC.InCameraFocus() = True Then
                        Screen.Camera.Update()
                    End If
                End If
            End If

            Return Core.Null
        End Function

        <ScriptCommand("SetMoveY")>
        <ScriptDescription("Sets the Y movement direction for the next movement of an NPC.")>
        Private Function M_SetMoveY(ByVal argument As String) As String
            Dim targetNPC As NPC = Screen.Level.GetNPC(Int(argument.GetSplit(0)))
            Dim steps As Integer = Int(argument.GetSplit(1))

            targetNPC.MoveY = steps

            Return Core.Null
        End Function

        <ScriptCommand("MoveAsync")>
        <ScriptDescription("Makes an NPC move async, this script line does not get preserved.")>
        Private Function M_MoveAsync(ByVal argument As String) As String
            Dim targetNPC As NPC = Screen.Level.GetNPC(Int(argument.GetSplit(0)))
            Dim steps As Integer = Int(argument.GetSplit(1))

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

            steps = Math.Abs(steps)

            targetNPC.Moved += steps
            targetNPC.MoveAsync = True

            Return Core.Null
        End Function

        <ScriptCommand("Turn")>
        <ScriptDescription("Turns an NPC to a specific direction.")>
        Private Function M_Turn(ByVal argument As String) As String
            Dim targetNPC As NPC = Screen.Level.GetNPC(Int(argument.GetSplit(0)))

            targetNPC.faceRotation = Int(argument.GetSplit(1))
            targetNPC.Update()
            targetNPC.UpdateEntity()

            Return Core.Null
        End Function

        <ScriptCommand("Spawn")>
        <ScriptDescription("Makes it possible to spawn a completely new NPC to the map.")>
        Private Function M_Spawn(ByVal argument As String) As String
            Dim args() As String = argument.Split(CChar(","))

            'required parameters: position
            'optional parameters: actionvalue, additionalvalue, textureid, animateidle, rotation, name, id, movement

            Dim position As Vector3 = New Vector3(Sng(args(0)), Sng(args(1)), Sng(args(2)))
            'Set default parameters:
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
                actionValue = Int(args(3))
                If args.Count >= 5 Then
                    additionalValue = args(4)
                    If args.Count >= 6 Then
                        TextureID = args(5)
                        If args.Count >= 7 Then
                            AnimateIdle = Bool(args(6))
                            If args.Count >= 8 Then
                                Rotation = Int(args(7))
                                If args.Count >= 9 Then
                                    Name = args(8)
                                    If args.Count >= 10 Then
                                        ID = Int(args(9))
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

            'Cannot add an NPC with the same ID twice to a map:
            If Not Screen.Level.GetNPC(ID) Is Nothing Then
                Logger.Debug("067", "@npc." & Command() & ") An NPC With the ID """ & ID & """ already exists.")
            End If

            Dim NPC As NPC = CType(Entity.GetNewEntity("NPC", position, {Nothing}, {0, 0}, True, New Vector3(0.0F), New Vector3(1.0F), BaseModel.BillModel, actionValue, additionalValue, True, New Vector3(1.0F), -1, Screen.Level.LevelFile, "", New Vector3(0), {TextureID, Rotation, Name, ID, AnimateIdle, Movement, MoveRectangles}), NPC)
            Screen.Level.Entities.Add(NPC)

            Return Core.Null
        End Function

        <ScriptCommand("SetSpeed")>
        <ScriptDescription("Sets the movement speed of an NPC.")>
        Private Function M_SetSpeed(ByVal argument As String) As String
            If argument.CountSeperators(",") > 0 Then
                Dim targetNPC As NPC = Screen.Level.GetNPC(Int(argument.GetSplit(0)))
                Dim speed As Single = Sng(argument.GetSplit(1))

                If Not targetNPC Is Nothing Then
                    If IsNumeric(speed) = True Then
                        targetNPC.Speed = speed * 0.04F
                    Else
                        Logger.Debug("068", "(@npc." & Command() & ") The argument for ""speed"" did not have the correct data type (int)")
                    End If
                Else
                    Logger.Debug("069", "(@npc." & Command() & ") The targeted NPC with ID """ & Int(argument.GetSplit(0)) & """ doesn't exist.")
                End If
            Else
                Logger.Debug("070", "(@npc." & Command() & ") Invalid argument passed.")
            End If

            Return Core.Null
        End Function

#End Region

#Region "Constructs"

        <ScriptConstruct("Position")>
        <ScriptDescription("Returns position information of an NPC.")>
        Private Function F_Position(ByVal argument As String) As String
            Dim targetNPC As NPC = Screen.Level.GetNPC(Int(argument.GetSplit(0)))

            If Not targetNPC Is Nothing Then
                Dim args() As String = argument.Split(CChar(","))
                If args.Length > 1 Then
                    Dim s As String = ""
                    For i = 1 To args.Length - 1
                        Select Case args(i)
                            Case "x"
                                If s <> "" Then
                                    s &= ","
                                End If
                                s &= ToString(Int(targetNPC.Position.X.ToString()))
                            Case "y"
                                If s <> "" Then
                                    s &= ","
                                End If
                                s &= ToString(Int(targetNPC.Position.Y.ToString()))
                            Case "z"
                                If s <> "" Then
                                    s &= ","
                                End If
                                s &= ToString(Int(targetNPC.Position.Z.ToString()))
                        End Select
                    Next
                    Return s
                Else
                    Return Int(targetNPC.Position.X.ToString()) & "," & Int(targetNPC.Position.Y.ToString()) & "," & Int(targetNPC.Position.Z.ToString())
                End If
            Else
                Logger.Debug("071", "No NPC with the ID " & argument.GetSplit(0) & " found.")
                Return Core.Null
            End If
        End Function

        <ScriptConstruct("Exists")>
        <ScriptDescription("Returns wether an NPC with a specific ID exists.")>
        Private Function F_Exists(ByVal argument As String) As String
            Dim targetNPC As NPC = Screen.Level.GetNPC(Int(argument.GetSplit(0)))

            Return ToString(targetNPC IsNot Nothing)
        End Function

        <ScriptConstruct("IsMoving")>
        <ScriptDescription("Returns if the NPC is moving right now.")>
        Private Function F_IsMoving(ByVal argument As String) As String
            Dim targetNPC As NPC = Screen.Level.GetNPC(Int(argument.GetSplit(0)))

            If Not targetNPC Is Nothing Then
                Return ToString(targetNPC.Moved <> 0F)
            Else
                Logger.Debug("072", "No NPC with the ID " & argument.GetSplit(0) & " found.")
                Return Core.Null
            End If
        End Function

        <ScriptConstruct("Moved")>
        <ScriptDescription("Returns the steps the NPC is going to take (usable with moveasync).")>
        Private Function F_Moved(ByVal argument As String) As String
            Dim targetNPC As NPC = Screen.Level.GetNPC(Int(argument.GetSplit(0)))

            If Not targetNPC Is Nothing Then
                Return ToString(targetNPC.Moved)
            Else
                Logger.Debug("073", "No NPC with the ID " & argument.GetSplit(0) & " found.")
                Return Core.Null
            End If
        End Function

        <ScriptConstruct("Skin")>
        <ScriptDescription("Returns the skin of an NPC.")>
        Private Function F_Skin(ByVal argument As String) As String
            Dim targetNPC As NPC = Screen.Level.GetNPC(Int(argument.GetSplit(0)))

            If Not targetNPC Is Nothing Then
                Return targetNPC.TextureID
            Else
                Logger.Debug("074", "No NPC with the ID " & argument.GetSplit(0) & " found.")
                Return Core.Null
            End If
        End Function

        <ScriptConstruct("Facing")>
        <ScriptDescription("Returns the facing of an NPC.")>
        Private Function F_Facing(ByVal argument As String) As String
            Dim targetNPC As NPC = Screen.Level.GetNPC(Int(argument.GetSplit(0)))

            If Not targetNPC Is Nothing Then
                Return ToString(targetNPC.faceRotation)
            Else
                Logger.Debug("075", "No NPC with the ID " & argument.GetSplit(0) & " found.")
                Return Core.Null
            End If
        End Function

        <ScriptConstruct("ID")>
        <ScriptDescription("Returns the NPC ID. (This seems pointless because the input is the NPC ID...).")>
        Private Function F_ID(ByVal argument As String) As String
            Dim targetNPC As NPC = Screen.Level.GetNPC(Int(argument.GetSplit(0)))

            If Not targetNPC Is Nothing Then
                Return ToString(targetNPC.NPCID)
            Else
                Logger.Debug("076", "No NPC with the ID " & argument.GetSplit(0) & " found.")
                Return Core.Null
            End If
        End Function

        <ScriptConstruct("Name")>
        <ScriptDescription("Returns the name of an NPC.")>
        Private Function F_Name(ByVal argument As String) As String
            Dim targetNPC As NPC = Screen.Level.GetNPC(Int(argument.GetSplit(0)))

            If Not targetNPC Is Nothing Then
                Return targetNPC.Name
            Else
                Logger.Debug("077", "No NPC with the ID " & argument.GetSplit(0) & " found.")
                Return Core.Null
            End If
        End Function

        <ScriptConstruct("Action")>
        <ScriptDescription("Returns the action value of an NPC.")>
        Private Function F_Action(ByVal argument As String) As String
            Dim targetNPC As NPC = Screen.Level.GetNPC(Int(argument.GetSplit(0)))

            If Not targetNPC Is Nothing Then
                Return ToString(targetNPC.ActionValue)
            Else
                Logger.Debug("078", "No NPC with the ID " & argument.GetSplit(0) & " found.")
                Return Core.Null
            End If
        End Function

        <ScriptConstruct("AdditionalValue")>
        <ScriptDescription("Returns the additional value of an NPC.")>
        Private Function F_AdditionalValue(ByVal argument As String) As String
            Dim targetNPC As NPC = Screen.Level.GetNPC(Int(argument.GetSplit(0)))

            If Not targetNPC Is Nothing Then
                Return targetNPC.AdditionalValue
            Else
                Logger.Debug("079", "No NPC with the ID " & argument.GetSplit(0) & " found.")
                Return Core.Null
            End If
        End Function

        <ScriptConstruct("Movement")>
        <ScriptDescription("Returns the movement value of an NPC.")>
        Private Function F_Movement(ByVal argument As String) As String
            Dim targetNPC As NPC = Screen.Level.GetNPC(Int(argument.GetSplit(0)))

            If Not targetNPC Is Nothing Then
                Return targetNPC.Movement.ToString()
            Else
                Logger.Debug("080", "No NPC with the ID " & argument.GetSplit(0) & " found.")
                Return Core.Null
            End If
        End Function

        <ScriptConstruct("HasMoveRectangles")>
        <ScriptDescription("Returns if there are movement rectangles defined for the NPC.")>
        Private Function F_HasMoveRectangles(ByVal argument As String) As String
            Dim targetNPC As NPC = Screen.Level.GetNPC(Int(argument.GetSplit(0)))

            If Not targetNPC Is Nothing Then
                Return ToString(targetNPC.MoveRectangles.Count > 0)
            Else
                Logger.Debug("081", "No NPC with the ID " & argument.GetSplit(0) & " found.")
                Return Core.Null
            End If
        End Function

        <ScriptConstruct("TrainerTexture")>
        <ScriptDescription("Returns the texture used by a trainer in battle.")>
        Private Function F_TrainerTexture(ByVal argument As String) As String
            Dim t As Trainer = New Trainer(argument)
            Return t.SpriteName
        End Function

#End Region

    End Class

End Namespace