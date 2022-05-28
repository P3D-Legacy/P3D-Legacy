﻿Namespace ScriptVersion2

    Partial Class ScriptCommander

        ' --------------------------------------------------------------------------------------------------------------------------
        ' Contains the @player commands.
        ' --------------------------------------------------------------------------------------------------------------------------

        Private Shared Sub DoPlayer(ByVal subClass As String)
            Dim command As String = ScriptComparer.GetSubClassArgumentPair(subClass).Command
            Dim argument As String = ScriptComparer.GetSubClassArgumentPair(subClass).Argument

            Select Case command.ToLower()
                Case "receivepokedex"
                    Core.Player.HasPokedex = True
                    For Each p As Pokemon In Core.Player.Pokemons
                        Dim i As Integer = 2
                        If p.IsShiny = True Then
                            i = 3
                        End If
                        Core.Player.PokedexData = Pokedex.ChangeEntry(Core.Player.PokedexData, p.Number, i)
                    Next
                    IsReady = True
                Case "receivepokegear"
                    Core.Player.HasPokegear = True
                    IsReady = True
                Case "renamerival"
                    Dim RivalTexture As String = GameModeManager.ActiveGameMode.ContentPath & "Textures\" & Core.Player.RivalSkin
                    Dim RivalName As String = Core.Player.RivalName
                    Core.SetScreen(New NameObjectScreen(Core.CurrentScreen, TextureManager.GetTexture(RivalTexture, New Rectangle(0, 64, 32, 32)), False, False, RivalName, "???", AddressOf Script.NameRival))
                    IsReady = True
                    CanContinue = False
                Case "setrivalskin"
                    Core.Player.RivalSkin = argument
                    IsReady = True
                Case "wearskin"
                    With Screen.Level.OwnPlayer
                        Dim TextureID As String = argument
                        .SetTexture(TextureID, False)

                        .UpdateEntity()
                    End With
                    IsReady = True
                Case "setskin"
                    Core.Player.Skin = argument
                    With Screen.Level.OwnPlayer
                        Dim TextureID As String = argument
                        .SetTexture(TextureID, False)

                        .UpdateEntity()
                    End With
                    IsReady = True
                Case "setspeed"
                    Dim speed As Single = sng(argument)

                    Screen.Camera.Speed = speed * 0.04F
                    IsReady = True
                Case "resetspeed"
                    Screen.Camera.Speed = 0.04F
                    IsReady = True
                Case "move"
                    If Started = False Then
                        Screen.Camera.Move(sng(argument))
                        Started = True
                        Screen.Level.OverworldPokemon.Visible = False
                    Else
                        Screen.Level.UpdateEntities()
                        Screen.Camera.Update()
                        If Screen.Camera.IsMoving() = False Then
                            IsReady = True
                            Screen.Level.OverworldPokemon.Visible = False
                        End If
                    End If
                Case "moveasnyc", "moveasync"
                    Screen.Camera.Move(sng(argument))
                    IsReady = True
                    Screen.Level.OverworldPokemon.Visible = False
                Case "turn"
                    If Started = False Then
                        Screen.Camera.Turn(int(argument))
                        Started = True
                        Screen.Level.OverworldPokemon.Visible = False
                    Else
                        Screen.Camera.Update()
                        Screen.Level.UpdateEntities()
                        If Screen.Camera.Turning = False Then
                            IsReady = True
                        End If
                    End If
                Case "dance"
                    Screen.Level.OwnPlayer().isDancing = True
                    If Started = False Then
                        Screen.Camera.Move(sng(argument))
                        Started = True
                        Screen.Level.OverworldPokemon.Visible = False
                    Else
                        Screen.Level.UpdateEntities()
                        Screen.Camera.Update()
                        If Screen.Camera.IsMoving() = False Then
                            IsReady = True
                            Screen.Level.OverworldPokemon.Visible = False
                        End If
                    End If
                Case "turnasync"
                    Screen.Camera.Turn(int(argument))
                    IsReady = True
                    Screen.Level.OverworldPokemon.Visible = False
                Case "turntoasync"
                    Dim turns As Integer = int(argument) - Screen.Camera.GetPlayerFacingDirection()
                    If turns < 0 Then
                        turns = turns + 4
                    End If

                    If turns > 0 Then
                        Screen.Camera.Turn(turns)
                        Started = True
                        Screen.Level.OverworldPokemon.Visible = False
                    End If

                    IsReady = True
                Case "turnto"
                    If Started = False Then
                        Dim turns As Integer = int(argument) - Screen.Camera.GetPlayerFacingDirection()
                        If turns < 0 Then
                            turns = turns + 4
                        End If

                        If turns > 0 Then
                            Screen.Camera.Turn(turns)
                            Started = True
                            Screen.Level.OverworldPokemon.Visible = False
                        Else
                            IsReady = True
                        End If
                    Else
                        Screen.Camera.Update()
                        Screen.Level.UpdateEntities()
                        If Screen.Camera.Turning = False Then
                            IsReady = True
                        End If
                    End If
                Case "warp"
                    Dim commas As Integer = 0
                    For Each c As Char In argument
                        If c = "," Then
                            commas += 1
                        End If
                    Next

                    Dim cPosition As Vector3 = Screen.Camera.Position

                    Select Case commas
                        Case 4
                            Screen.Level.WarpData.WarpDestination = argument.GetSplit(0)
                            Screen.Level.WarpData.WarpPosition = New Vector3(sng(argument.GetSplit(1).Replace("~", CStr(cPosition.X)).Replace(".", GameController.DecSeparator)),
                                                                          sng(argument.GetSplit(2).Replace("~", CStr(cPosition.Y)).Replace(".", GameController.DecSeparator)),
                                                                          sng(argument.GetSplit(3).Replace("~", CStr(cPosition.Z)).Replace(".", GameController.DecSeparator)))
                            Screen.Level.WarpData.WarpRotations = int(argument.GetSplit(4))
                            Screen.Level.WarpData.DoWarpInNextTick = True
                            Screen.Level.WarpData.CorrectCameraYaw = Screen.Camera.Yaw
                        Case 3
                            Screen.Level.WarpData.WarpDestination = argument.GetSplit(0)
                            Screen.Level.WarpData.WarpPosition = New Vector3(sng(argument.GetSplit(1).Replace("~", CStr(cPosition.X)).Replace(".", GameController.DecSeparator)),
                                                                          sng(argument.GetSplit(2).Replace("~", CStr(cPosition.Y)).Replace(".", GameController.DecSeparator)),
                                                                          sng(argument.GetSplit(3).Replace("~", CStr(cPosition.Z)).Replace(".", GameController.DecSeparator)))
                            Screen.Level.WarpData.WarpRotations = 0
                            Screen.Level.WarpData.DoWarpInNextTick = True
                            Screen.Level.WarpData.CorrectCameraYaw = Screen.Camera.Yaw
                        Case 2
                            Screen.Camera.Position = New Vector3(sng(argument.GetSplit(0).Replace("~", CStr(cPosition.X)).Replace(".", GameController.DecSeparator)),
                                                                 sng(argument.GetSplit(1).Replace("~", CStr(cPosition.Y)).Replace(".", GameController.DecSeparator)),
                                                                 sng(argument.GetSplit(2).Replace("~", CStr(cPosition.Z)).Replace(".", GameController.DecSeparator)))
                        Case 0
                            Screen.Level.WarpData.WarpDestination = argument
                            Screen.Level.WarpData.WarpPosition = Screen.Camera.Position
                            Screen.Level.WarpData.WarpRotations = 0
                            Screen.Level.WarpData.DoWarpInNextTick = True
                            Screen.Level.WarpData.CorrectCameraYaw = Screen.Camera.Yaw
                    End Select

                    Screen.Level.OverworldPokemon.warped = True
                    Screen.Level.OverworldPokemon.Visible = False

                    IsReady = True
                Case "stopmovement"
                    Screen.Camera.StopMovement()

                    IsReady = True
                Case "money", "addmoney"
                    Core.Player.Money += int(argument)

                    IsReady = True
                Case "setmovement"
                    Dim movements() As String = argument.Split(CChar(","))

                    Screen.Camera.PlannedMovement = New Vector3(int(movements(0)),
                                                                sng(movements(1)),
                                                                int(movements(2)))
                    IsReady = True
                Case "resetmovement"
                    Screen.Camera.PlannedMovement = Vector3.Zero

                    IsReady = True
                Case "getbadge"
                    If StringHelper.IsNumeric(argument) Then
                        If Core.Player.Badges.Contains(int(argument)) = False Then
                            Core.Player.Badges.Add(int(argument))
                            SoundManager.PlaySound("Receive_Badge", True)
                            Screen.TextBox.TextColor = TextBox.PlayerColor
                            Screen.TextBox.Show(Core.Player.Name & " received the~" & Badge.GetBadgeName(int(argument)) & " Badge.", {}, False, False)

                            Core.Player.AddPoints(10, "Got a badge.")
                        End If
                    End If

                    IsReady = True

                    CanContinue = False
                Case "removebadge"
                    If StringHelper.IsNumeric(argument) Then
                        If Core.Player.Badges.Contains(int(argument)) = True Then
                            Core.Player.Badges.Remove(int(argument))
                        End If
                    End If

                    IsReady = True
                Case "addbadge"
                    If StringHelper.IsNumeric(argument) Then
                        If Core.Player.Badges.Contains(int(argument)) = False Then
                            Core.Player.Badges.Add(int(argument))
                        End If
                    End If

                    IsReady = True
                Case "achieveemblem"
                    GameJolt.Emblem.AchieveEmblem(argument)

                    IsReady = True
                Case "addbp"
                    Dim bp As Integer = int(argument)

                    Core.Player.BP += bp

                    If bp > 0 Then
                        PlayerStatistics.Track("Obtained BP", bp)
                    End If

                    IsReady = True
                Case "showrod"
                    If Core.CurrentScreen.Identification = Screen.Identifications.OverworldScreen Then
                        OverworldScreen.DrawRodID = int(argument)
                    End If

                    IsReady = True
                Case "hiderod"
                    OverworldScreen.DrawRodID = -1

                    IsReady = True
                Case "save"
                    Core.Player.SaveGame(False)

                    IsReady = True
                Case "setname"
                    Core.Player.Name = argument
                    IsReady = True
                Case "setrivalname"
                    Core.Player.RivalName = argument
                    IsReady = True
                Case "setgender"
                    Select Case argument
                        Case "0", "Male", "male"
                            Core.Player.Gender = "Male"
                        Case "1", "Female", "female"
                            Core.Player.Gender = "Female"
                        Case Else
                            Core.Player.Gender = "Other"
                    End Select
                    IsReady = True
                Case "setopacity"
                    Dim newOpacity As Single = sng(argument.Replace("~", Screen.Level.OwnPlayer.Opacity.ToString().Replace(".", GameController.DecSeparator)))
                    Screen.Level.OwnPlayer.Opacity = newOpacity
                    IsReady = True
                Case Else
                    IsReady = True
            End Select
        End Sub

    End Class

End Namespace