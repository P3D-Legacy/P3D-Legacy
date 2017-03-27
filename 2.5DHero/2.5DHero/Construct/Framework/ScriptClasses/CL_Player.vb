Namespace Construct.Framework.Classes

    <ScriptClass("Player")>
    <ScriptDescription("A class to manipulate the player.")>
    Public Class CL_Player

        Inherits ScriptClass

        Public Sub New()
            MyBase.New(True)
        End Sub

#Region "Commands"

        <ScriptCommand("UseRepel", RequiredContext:=ScriptContext.Overworld)>
        <ScriptDescription("Applies repel steps to the player's repel steps.")>
        Private Function M_UseRepel(ByVal argument As String) As String
            Dim itemID As Integer = Int(argument)
            Dim steps As Integer = itemID
            Select Case itemID
                Case 20
                    steps = 100
                Case 42
                    steps = 200
                Case 43
                    steps = 250
            End Select
            Game.Core.Player.RepelSteps += steps

            Return Core.Null
        End Function

        <ScriptCommand("ReceivePokedex")>
        <ScriptDescription("Makes the Pokédex appear in the player menu.")>
        Private Function M_ReceivePokedex(ByVal argument As String) As String
            Game.Core.Player.HasPokedex = True
            'Register each Pokémon in the player's party in the dex:
            For Each p As Pokemon In Game.Core.Player.Pokemons
                Dim i As Integer = 2
                If p.IsShiny = True Then
                    i = 3
                End If
                Game.Core.Player.PokedexData = Pokedex.ChangeEntry(Game.Core.Player.PokedexData, p.Number, i)
            Next

            Return Core.Null
        End Function

        <ScriptCommand("ReceivePokegear")>
        <ScriptDescription("Makes the Pokégear appear in the player menu.")>
        Private Function M_ReceivePokegear(ByVal argument As String) As String
            Game.Core.Player.HasPokegear = True
            Return Core.Null
        End Function

        <ScriptCommand("WearSkin")>
        <ScriptDescription("Changes the player's skin.")>
        Private Function M_WearSkin(ByVal argument As String) As String
            With Screen.Level.OwnPlayer
                .SetTexture(argument, False)

                .UpdateEntity()
            End With
            Return Core.Null
        End Function

        <ScriptCommand("Move", RequiredContext:=ScriptContext.Overworld)>
        <ScriptDescription("Moves the player.")>
        Private Function M_Move(ByVal argument As String) As String
            If WorkValues.Count = 0 Then
                Screen.Camera.Move(Sng(argument))
                Screen.Level.OverworldPokemon.Visible = False

                ActiveLine.Preserve = True
                WorkValues.Add("started")
            Else
                Screen.Level.UpdateEntities()
                Screen.Camera.Update()
                If Screen.Camera.IsMoving() = False Then
                    ActiveLine.Preserve = False
                    Screen.Level.OverworldPokemon.Visible = False
                End If
            End If

            Return Core.Null
        End Function

        <ScriptCommand("MoveAsync", RequiredContext:=ScriptContext.Overworld)>
        <ScriptDescription("Moves the player async.")>
        Private Function M_MoveAsync(ByVal argument As String) As String
            Screen.Camera.Move(Sng(argument))
            Screen.Level.OverworldPokemon.Visible = False

            Return Core.Null
        End Function

        <ScriptCommand("Turn", RequiredContext:=ScriptContext.Overworld)>
        <ScriptDescription("Turns the player.")>
        Private Function M_Turn(ByVal argument As String) As String
            If WorkValues.Count = 0 Then
                Screen.Camera.Turn(Int(argument))
                Screen.Level.OverworldPokemon.Visible = False

                ActiveLine.Preserve = True
                WorkValues.Add("started")
            Else
                Screen.Camera.Update()
                Screen.Level.UpdateEntities()
                If Screen.Camera.Turning = False Then
                    ActiveLine.Preserve = False
                End If
            End If

            Return Core.Null
        End Function

        <ScriptCommand("TurnAsync", RequiredContext:=ScriptContext.Overworld)>
        <ScriptDescription("Turns the player async.")>
        Private Function M_TurnAsync(ByVal argument As String) As String
            Screen.Camera.Turn(Int(argument))
            Screen.Level.OverworldPokemon.Visible = False

            Return Core.Null
        End Function

        <ScriptCommand("TurnTo", RequiredContext:=ScriptContext.Overworld)>
        <ScriptDescription("Turns the player to a specific direction.")>
        Private Function M_TurnTo(ByVal argument As String) As String
            If WorkValues.Count = 0 Then
                Dim turns As Integer = Int(argument) - Screen.Camera.GetPlayerFacingDirection()
                If turns < 0 Then
                    turns = turns + 4
                End If

                If turns > 0 Then
                    Screen.Camera.Turn(turns)
                    Screen.Level.OverworldPokemon.Visible = False

                    ActiveLine.Preserve = True
                    WorkValues.Add("started")
                End If
            Else
                Screen.Camera.Update()
                Screen.Level.UpdateEntities()
                If Screen.Camera.Turning = False Then
                    ActiveLine.Preserve = False
                End If
            End If

            Return Core.Null
        End Function

        <ScriptCommand("TurnToAsync", RequiredContext:=ScriptContext.Overworld)>
        <ScriptDescription("Turns the player to a specific direction async.")>
        Private Function M_TurnToAsync(ByVal argument As String) As String
            Dim turns As Integer = Int(argument) - Screen.Camera.GetPlayerFacingDirection()
            If turns < 0 Then
                turns = turns + 4
            End If

            If turns > 0 Then
                Screen.Camera.Turn(turns)
                Screen.Level.OverworldPokemon.Visible = False
            End If

            Return Core.Null
        End Function

        <ScriptCommand("Warp", RequiredContext:=ScriptContext.Overworld)>
        <ScriptDescription("Warps the player.")>
        Private Function M_Warp(ByVal argument As String) As String
            Dim commas As Integer = 0
            For Each c As Char In argument
                If c = "," Then
                    commas += 1
                End If
            Next

            Dim cPosition As Vector3 = Screen.Camera.Position

            Select Case commas
                Case 4 'destination map with position and rotation
                    Screen.Level.WarpData.WarpDestination = argument.GetSplit(0)
                    Screen.Level.WarpData.WarpPosition = New Vector3(Sng(argument.GetSplit(1).Replace("~", CStr(cPosition.X))),
                                                                     Sng(argument.GetSplit(2).Replace("~", CStr(cPosition.Y))),
                                                                     Sng(argument.GetSplit(3).Replace("~", CStr(cPosition.Z))))
                    Screen.Level.WarpData.WarpRotations = Int(argument.GetSplit(4))
                    Screen.Level.WarpData.DoWarpInNextTick = True
                    Screen.Level.WarpData.CorrectCameraYaw = Screen.Camera.Yaw
                Case 3 'destination map with position
                    Screen.Level.WarpData.WarpDestination = argument.GetSplit(0)
                    Screen.Level.WarpData.WarpPosition = New Vector3(Sng(argument.GetSplit(1).Replace("~", CStr(cPosition.X))),
                                                                     Sng(argument.GetSplit(2).Replace("~", CStr(cPosition.Y))),
                                                                     Sng(argument.GetSplit(3).Replace("~", CStr(cPosition.Z))))
                    Screen.Level.WarpData.WarpRotations = 0
                    Screen.Level.WarpData.DoWarpInNextTick = True
                    Screen.Level.WarpData.CorrectCameraYaw = Screen.Camera.Yaw
                Case 2 'position
                    Screen.Camera.Position = New Vector3(Sng(argument.GetSplit(0).Replace("~", CStr(cPosition.X))),
                                                         Sng(argument.GetSplit(1).Replace("~", CStr(cPosition.Y))),
                                                         Sng(argument.GetSplit(2).Replace("~", CStr(cPosition.Z))))
                Case 0 'destination map
                    Screen.Level.WarpData.WarpDestination = argument
                    Screen.Level.WarpData.WarpPosition = Screen.Camera.Position
                    Screen.Level.WarpData.WarpRotations = 0
                    Screen.Level.WarpData.DoWarpInNextTick = True
                    Screen.Level.WarpData.CorrectCameraYaw = Screen.Camera.Yaw
            End Select

            Screen.Level.OverworldPokemon.warped = True
            Screen.Level.OverworldPokemon.Visible = False

            Return Core.Null
        End Function

        <ScriptCommand("StopMovement", RequiredContext:=ScriptContext.Overworld)>
        <ScriptDescription("Stops the player's async movement.")>
        Private Function M_StopMovement(ByVal argument As String) As String
            Screen.Camera.StopMovement()

            Return Core.Null
        End Function

        <ScriptCommand("AddMoney")>
        <ScriptDescription("Gives the player money (negative amount removes money).")>
        Private Function M_AddMoney(ByVal argument As String) As String
            Game.Core.Player.Money += Int(argument)

            Return Core.Null
        End Function

        <ScriptCommand("SetMovement", RequiredContext:=ScriptContext.Overworld)>
        <ScriptDescription("Sets the movement of the player.")>
        Private Function M_SetMovement(ByVal argument As String) As String
            Dim movements() As String = argument.Split(CChar(","))

            If movements.Length >= 3 Then
                Screen.Camera.PlannedMovement = New Vector3(Int(movements(0)),
                                                            Sng(movements(1)),
                                                            Int(movements(2)))
            Else
                Logger.Debug("066", "Player.SetMovement requires 3 parameter.")
            End If


            Return Core.Null
        End Function

        <ScriptCommand("ResetMovement", RequiredContext:=ScriptContext.Overworld)>
        <ScriptDescription("Resets the movement of the player to the default.")>
        Private Function M_ResetMovement(ByVal argument As String) As String
            Screen.Camera.PlannedMovement = Vector3.Zero

            Return Core.Null
        End Function

        <ScriptCommand("GetBadge")>
        <ScriptDescription("Adds a badge, plays a sound and shows a textbox.")>
        Private Function M_GetBadge(ByVal argument As String) As String
            If Converter.IsNumeric(argument) = True Then
                If Game.Core.Player.Badges.Contains(Int(argument)) = False Then
                    Game.Core.Player.Badges.Add(Int(argument))
                    SoundManager.PlaySound("badge_acquired", True)
                    Screen.TextBox.TextColor = TextBox.PlayerColor
                    Screen.TextBox.Show(Game.Core.Player.Name & " received the~" & Badge.GetBadgeName(Int(argument)) & "badge.", {}, False, False)

                    Game.Core.Player.AddPoints(10, "Got a badge.")
                End If
            End If

            Return Core.Null
        End Function

        <ScriptCommand("RemoveBadge")>
        <ScriptDescription("Removes a badge from the player.")>
        Private Function M_RemoveBadge(ByVal argument As String) As String
            If Converter.IsNumeric(argument) = True Then
                If Game.Core.Player.Badges.Contains(Int(argument)) = True Then
                    Game.Core.Player.Badges.Remove(Int(argument))
                End If
            End If

            Return Core.Null
        End Function

        <ScriptCommand("AddBadge")>
        <ScriptDescription("Adds a badge to the player.")>
        Private Function M_AddBadge(ByVal argument As String) As String
            If Converter.IsNumeric(argument) = True Then
                If Game.Core.Player.Badges.Contains(Int(argument)) = False Then
                    Game.Core.Player.Badges.Add(Int(argument))
                End If
            End If

            Return Core.Null
        End Function

        <ScriptCommand("AchieveEmblem")>
        <ScriptDescription("Achieves a trophy.")>
        Private Function M_AchieveEmblem(ByVal argument As String) As String
            GameJolt.Emblem.AchieveEmblem(argument)

            Return Core.Null
        End Function

        <ScriptCommand("AddBP")>
        <ScriptDescription("Adds (or removes) BattlePoints.")>
        Private Function M_AddBP(ByVal argument As String) As String
            Dim bp As Integer = Int(argument)

            Game.Core.Player.BP += bp

            If bp > 0 Then
                PlayerStatistics.Track("Obtained BP", bp)
            End If

            Return Core.Null
        End Function

        <ScriptCommand("ShowRod", RequiredContext:=ScriptContext.Overworld)>
        <ScriptDescription("Displays a specific rod.")>
        Private Function M_ShowRod(ByVal argument As String) As String
            If Game.Core.CurrentScreen.Identification = Screen.Identifications.OverworldScreen Then
                OverworldScreen.DrawRodID = Int(argument)
            End If

            Return Core.Null
        End Function

        <ScriptCommand("HideRod", RequiredContext:=ScriptContext.Overworld)>
        <ScriptDescription("Hides a specific rod.")>
        Private Function M_HideRod(ByVal argument As String) As String
            OverworldScreen.DrawRodID = -1

            Return Core.Null
        End Function

        <ScriptCommand("SetOpacity", RequiredContext:=ScriptContext.Overworld)>
        <ScriptDescription("Sets the opacity of the player character.")>
        Private Function M_SetOpacity(ByVal argument As String) As String
            Dim newOpacity As Single = Sng(argument.Replace("~", Screen.Level.OwnPlayer.Opacity.ToString()))
            Screen.Level.OwnPlayer.Opacity = newOpacity

            Return Core.Null
        End Function

        <ScriptCommand("SetGender")>
        <ScriptDescription("Sets the gender of the player. 0/False for Male, 1/True for Female.")>
        Private Function M_SetGender(ByVal argument As String) As String
            Game.Core.Player.Male = Not Bool(argument)

            Return Core.Null
        End Function

        <ScriptCommand("SetName")>
        <ScriptDescription("Sets the name of the player when starting a new game.")>
        Private Function M_SetName(ByVal argument As String) As String
            Game.Core.Player.Name = argument

            Return Core.Null
        End Function

        <ScriptCommand("SetOT")>
        <ScriptDescription("Sets the OT of the player when starting a new game.")>
        Private Function M_SetOT(ByVal argument As String) As String
            While argument.Length < 5
                argument = "0" & argument
            End While
            Game.Core.Player.OT = argument

            Return Core.Null
        End Function

#End Region

#Region "Constructs"

        <ScriptConstruct("Position", RequiredContext:=ScriptContext.Overworld)>
        <ScriptDescription("Returns the player's position.")>
        Private Function F_Position(ByVal argument As String) As String
            If argument = "" Then
                argument = "x,y,z"
            End If

            Dim args() As String = argument.Split(CChar(","))
            Dim s As String = ""
            For i = 0 To args.Length - 1
                Select Case args(i)
                    Case "x"
                        If s <> "" Then
                            s &= ","
                        End If
                        s &= Int(ToString(Screen.Camera.Position.X))
                    Case "y"
                        If s <> "" Then
                            s &= ","
                        End If
                        s &= Int(ToString(Screen.Camera.Position.Y))
                    Case "z"
                        If s <> "" Then
                            s &= ","
                        End If
                        s &= Int(ToString(Screen.Camera.Position.Z))
                End Select
            Next
            Return s
        End Function

        <ScriptConstruct("HasBadge")>
        <ScriptDescription("Returns if the player owns a specific badge.")>
        Private Function F_HasBadge(ByVal argument As String) As String
            If Game.Core.Player.Badges.Contains(Int(argument)) = True Then
                Return "true"
            Else
                Return "false"
            End If
        End Function

        <ScriptConstruct("Skin")>
        <ScriptDescription("Returns the current skin of the player.")>
        Private Function F_Skin(ByVal argument As String) As String
            Return Game.Core.Player.Skin
        End Function

        <ScriptConstruct("IsMoving", RequiredContext:=ScriptContext.Overworld)>
        <ScriptDescription("Returns if the player is moving.")>
        Private Function F_IsMoving(ByVal argument As String) As String
            Return ToString(Screen.Camera.IsMoving())
        End Function

        <ScriptConstruct("Facing", RequiredContext:=ScriptContext.Overworld)>
        <ScriptDescription("Returns the current facing of the player.")>
        Private Function F_Facing(ByVal argument As String) As String
            Return ToString(Screen.Camera.GetPlayerFacingDirection())
        End Function

        <ScriptConstruct("Compass", RequiredContext:=ScriptContext.Overworld)>
        <ScriptDescription("Returns the descriptive direction of the player.")>
        Private Function F_Compass(ByVal argument As String) As String
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
            Return Core.Null
        End Function

        <ScriptConstruct("Money")>
        <ScriptDescription("Returns the current amount of money the player has.")>
        Private Function F_Money(ByVal argument As String) As String
            Return ToString(Game.Core.Player.Money)
        End Function

        <ScriptConstruct("Name")>
        <ScriptDescription("Returns the player's name.")>
        Private Function F_Name(ByVal argument As String) As String
            Return Game.Core.Player.Name
        End Function

        <ScriptConstruct("Gender")>
        <ScriptDescription("Returns 0 for male and 1 for female.")>
        Private Function F_Gender(ByVal argument As String) As String
            Return ToString(CInt(Not Game.Core.Player.Male))
        End Function

        <ScriptConstruct("BP")>
        <ScriptDescription("Returns the amount of BP the player has.")>
        Private Function F_BP(ByVal argument As String) As String
            Return Game.Core.Player.BP.ToString()
        End Function

        <ScriptConstruct("Badges")>
        <ScriptDescription("Returns the amount of badges the player has.")>
        Private Function F_Badges(ByVal argument As String) As String
            Return ToString(Game.Core.Player.Badges.Count)
        End Function

        <ScriptConstruct("OT")>
        <ScriptDescription("Returns the OT of the player.")>
        Private Function F_OT(ByVal argument As String) As String
            Return Game.Core.Player.OT
        End Function

        <ScriptConstruct("GameJoltID")>
        <ScriptDescription("Returns the GameJoltID of the player.")>
        Private Function F_GameJoltID(ByVal argument As String) As String
            Return Game.Core.GameJoltSave.GameJoltID
        End Function

        <ScriptConstruct("HasPokedex")>
        <ScriptDescription("Returns if the player has a Pokédex.")>
        Private Function F_HasPokedex(ByVal argument As String) As String
            Return ToString(Game.Core.Player.HasPokedex)
        End Function

        <ScriptConstruct("HasPokegear")>
        <ScriptDescription("Returns if the player has a Pokégear.")>
        Private Function F_HasPokegear(ByVal argument As String) As String
            Return ToString(Game.Core.Player.HasPokegear)
        End Function

#End Region

    End Class

End Namespace