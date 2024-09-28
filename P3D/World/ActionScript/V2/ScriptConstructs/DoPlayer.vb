Namespace ScriptVersion2

    Partial Class ScriptComparer

        ' --------------------------------------------------------------------------------------------------------------------------
        ' Contains the <player> constructs.
        ' --------------------------------------------------------------------------------------------------------------------------

        Private Shared Function DoPlayer(ByVal subClass As String) As Object
            Dim command As String = GetSubClassArgumentPair(subClass).Command
            Dim argument As String = GetSubClassArgumentPair(subClass).Argument

            Select Case command.ToLower()
                Case "position"
                    Dim args() As String = argument.Split(CChar(","))
                    If argument <> "" Then
                        Dim s As String = ""
                        For i = 0 To args.Length - 1
                            Select Case args(i)
                                Case "x"
                                    If s <> "" Then
                                        s &= ","
                                    End If
                                    s &= int(Screen.Camera.Position.X)
                                Case "y"
                                    If s <> "" Then
                                        s &= ","
                                    End If
                                    s &= int(Screen.Camera.Position.Y)
                                Case "z"
                                    If s <> "" Then
                                        s &= ","
                                    End If
                                    s &= int(Screen.Camera.Position.Z)
                            End Select
                        Next
                        Return s
                    Else
                        Return int(Screen.Camera.Position.X) & "," & int(Screen.Camera.Position.Y) & "," & int(Screen.Camera.Position.Z)
                    End If
                Case "hasbadge"
                    If Core.Player.Badges.Contains(int(argument)) = True Then
                        Return "true"
                    Else
                        Return "false"
                    End If
                Case "hasfrontieremblem"
                    Dim ID As String = ""
                    Dim CheckType As Boolean = Nothing
                    If argument.Split(",").Count > 0 AndAlso argument <> "" Then
                        If argument.Split(",").Count = 1 Then
                            ID = argument
                        Else
                            ID = argument.GetSplit(0, ",")
                            CheckType = CBool(argument.GetSplit(1, ","))
                        End If
                    End If
                    If CheckType <> Nothing Then
                        If CheckType = False Then
                            Return ReturnBoolean(ActionScript.IsRegistered("frontier_" & ID & "_silver"))
                        Else
                            Return ReturnBoolean(ActionScript.IsRegistered("frontier_" & ID & "_gold"))
                        End If
                    Else
                        If ActionScript.IsRegistered("frontier_" & ID & "_gold") = True Then
                            Return "true"
                        Else
                            If ActionScript.IsRegistered("frontier_" & ID & "_silver") = True Then
                                Return "true"
                            Else
                                Return "false"
                            End If
                        End If
                    End If
                Case "skin"
                    Return Core.Player.Skin
                Case "velocity"
                    Return CInt(CType(Screen.Camera, OverworldCamera)._moved)
                Case "speed"
                    Return Screen.Camera.Speed / 0.04F
                Case "isrunning"
                    Return ReturnBoolean(Core.Player.IsRunning)
                Case "ismoving"
                    Return ReturnBoolean(Screen.Camera.IsMoving())
                Case "facing"
                    Return Screen.Camera.GetPlayerFacingDirection().ToString()
                Case "compass"
                    Select Case Screen.Camera.GetPlayerFacingDirection()
                        Case 0
                            Return "north"
                        Case 1
                            Return "west"
                        Case 2
                            Return "south"
                        Case 3
                            Return "east"
                    End Select
                Case "money"
                    Return Core.Player.Money.ToString()
                Case "name"
                    Return Core.Player.Name
                Case "gender"
                    Return Core.Player.Gender
                Case "bp"
                    Return Core.Player.BP.ToString()
                Case "coins"
                    Return Core.Player.Coins.ToString()
                Case "badges"
                    Return Core.Player.Badges.Count
                Case "thirdperson"
                    Return ReturnBoolean(CType(Screen.Camera, OverworldCamera).ThirdPerson)
                Case "rival", "rivalname"
                    Return Core.Player.RivalName
                Case "rivalskin"
                    Return Core.Player.RivalSkin
                Case "ot"
                    Return Core.Player.OT
                Case "gamejoltid"
                    Return Core.GameJoltSave.GameJoltID
                Case "haspokedex"
                    Return ReturnBoolean(Core.Player.HasPokedex)
                Case "haspokegear"
                    Return ReturnBoolean(Core.Player.HasPokegear)
                Case "isgamejolt"
                    '<player.isgamejolt([bool_checkban=0])
                    If argument <> "" Then
                        If CBool(argument) = True Then
                            ReturnBoolean(Core.Player.IsGameJoltSave = True AndAlso GameJolt.LogInScreen.UserBanned(GameJoltSave.GameJoltID) = False)
                        End If
                    End If
                    Return ReturnBoolean(Core.Player.IsGameJoltSave)
                Case "lastrestplace"
                    Return Core.Player.LastRestPlace & "," & Core.Player.LastRestPlacePosition
            End Select
            Return DEFAULTNULL
        End Function

    End Class

End Namespace