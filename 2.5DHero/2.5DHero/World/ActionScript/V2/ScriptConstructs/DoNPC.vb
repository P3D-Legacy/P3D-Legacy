Namespace ScriptVersion2

    Partial Class ScriptComparer

        '--------------------------------------------------------------------------------------------------------------------------
        'Contains the <npc> constructs.
        '--------------------------------------------------------------------------------------------------------------------------

        Private Shared Function DoNPC(ByVal subClass As String) As Object
            Dim command As String = GetSubClassArgumentPair(subClass).Command
            Dim argument As String = GetSubClassArgumentPair(subClass).Argument

            Select Case command.ToLower()
                Case "position"
                    Dim targetNPC As NPC = Screen.Level.GetNPC(int(argument.GetSplit(0)))

                    Dim args() As String = argument.Split(CChar(","))
                    If args.Length > 1 Then
                        Dim s As String = ""
                        For i = 1 To args.Length - 1
                            Select Case args(i)
                                Case "x"
                                    If s <> "" Then
                                        s &= ","
                                    End If
                                    s &= int(targetNPC.Position.X)
                                Case "y"
                                    If s <> "" Then
                                        s &= ","
                                    End If
                                    s &= int(targetNPC.Position.Y)
                                Case "z"
                                    If s <> "" Then
                                        s &= ","
                                    End If
                                    s &= int(targetNPC.Position.Z)
                            End Select
                        Next
                        Return s
                    Else
                        Return int(targetNPC.Position.X) & "," & int(targetNPC.Position.Y) & "," & int(targetNPC.Position.Z)
                    End If
                Case "exists"
                    Dim targetNPC As NPC = Screen.Level.GetNPC(int(argument.GetSplit(0)))

                    If targetNPC Is Nothing Then
                        Return ReturnBoolean(False)
                    Else
                        Return ReturnBoolean(True)
                    End If
                Case "ismoving"
                    Dim targetNPC As NPC = Screen.Level.GetNPC(int(argument.GetSplit(0)))

                    If targetNPC.Moved <> 0.0F Then
                        Return ReturnBoolean(True)
                    Else
                        Return ReturnBoolean(False)
                    End If
                Case "moved"
                    Dim targetNPC As NPC = Screen.Level.GetNPC(int(argument.GetSplit(0)))

                    Return targetNPC.Moved.ToString()
                Case "skin"
                    Dim targetNPC As NPC = Screen.Level.GetNPC(int(argument.GetSplit(0)))

                    Return targetNPC.TextureID
                Case "facing"
                    Dim targetNPC As NPC = Screen.Level.GetNPC(int(argument.GetSplit(0)))

                    Return targetNPC.faceRotation
                Case "id"
                    Dim targetNPC As NPC = Screen.Level.GetNPC(int(argument.GetSplit(0)))

                    Return targetNPC.NPCID
                Case "name"
                    Dim targetNPC As NPC = Screen.Level.GetNPC(int(argument.GetSplit(0)))

                    Return targetNPC.Name
                Case "action"
                    Dim targetNPC As NPC = Screen.Level.GetNPC(int(argument.GetSplit(0)))

                    Return targetNPC.ActionValue
                Case "additionalvalue"
                    Dim targetNPC As NPC = Screen.Level.GetNPC(int(argument.GetSplit(0)))

                    Return targetNPC.AdditionalValue
                Case "movement"
                    Dim targetNPC As NPC = Screen.Level.GetNPC(int(argument.GetSplit(0)))

                    Return targetNPC.Movement.ToString()
                Case "hasmoverectangles"
                    Dim targetNPC As NPC = Screen.Level.GetNPC(int(argument.GetSplit(0)))

                    Return ReturnBoolean((targetNPC.MoveRectangles.Count > 0))
                Case "trainertexture"
                    Dim t As Trainer = New Trainer(argument)
                    Return t.SpriteName
            End Select
            Return DEFAULTNULL
        End Function

    End Class

End Namespace