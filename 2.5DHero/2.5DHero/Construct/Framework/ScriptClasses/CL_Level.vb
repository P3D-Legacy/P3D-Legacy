Namespace Construct.Framework.Classes

    <ScriptClass("Level")>
    <ScriptDescription("A class to handle level actions.")>
    Public Class CL_Level

        Inherits ScriptClass

#Region "Commands"

        <ScriptCommand("Load")>
        <ScriptDescription("Loads a level.")>
        Private Function M_Load(ByVal argument As String) As String
            Screen.Level = New Level()
            Screen.Level.Load(argument)

            Return Core.Null
        End Function

        <ScriptCommand("Wait")>
        <ScriptDescription("Makes the game idle for a specified amount of ticks.")>
        Private Function M_Wait(ByVal argument As String) As String
            If WorkValues.Count = 0 Then
                If Converter.IsNumeric(argument) = True Then
                    WorkValues.Add(argument)
                    ActiveLine.Preserve = True
                End If
            Else
                Dim waitTime As Integer = CInt(WorkValues(0))
                waitTime -= 1
                If waitTime <= 0 Then
                    ActiveLine.Preserve = False
                Else
                    WorkValues(0) = ToString(waitTime)
                End If
            End If

            Return Core.Null
        End Function

        <ScriptCommand("WaitForSave")>
        <ScriptDescription("If the GameJolt save process is active right now, wait for it to finish.")>
        Private Function M_WaitForSave(ByVal argument As String) As String
            Dim doWait As Boolean = False

            If Game.Core.Player.IsGameJoltSave = True Then
                If SaveGameHelpers.GameJoltSaveDone = False Then
                    doWait = True
                Else
                    SaveGameHelpers.ResetSaveCounter()
                End If
            End If

            ActiveLine.Preserve = doWait

            Return Core.Null
        End Function

        <ScriptCommand("Update")>
        <ScriptDescription("Forces a level update.")>
        Private Function M_Update(ByVal argument As String) As String
            Screen.Level.Update()
            Screen.Level.UpdateEntities()
            Screen.Camera.Update()

            Return Core.Null
        End Function

        <ScriptCommand("WaitForEvents")>
        <ScriptDescription("Waits for async walk events to finish.")>
        Private Function M_WaitForEvents(ByVal argument As String) As String
            Dim doWait As Boolean = False
            For Each e As Entity In Screen.Level.Entities
                If e.EntityID = "NPC" Then
                    If CType(e, NPC).MoveAsync = True And CType(e, NPC).Moved <> 0.0F Then
                        doWait = True
                        Exit For
                    End If
                End If
            Next

            ActiveLine.Preserve = doWait

            Return Core.Null
        End Function

        <ScriptCommand("Reload")>
        <ScriptDescription("Reloads the current level.")>
        Private Function M_Reload(ByVal argument As String) As String
            Screen.Level.WarpData.WarpDestination = Screen.Level.LevelFile
            Screen.Level.WarpData.WarpPosition = Screen.Camera.Position
            Screen.Level.WarpData.WarpRotations = 0
            Screen.Level.WarpData.DoWarpInNextTick = True
            Screen.Level.WarpData.CorrectCameraYaw = Screen.Camera.Yaw

            Return Core.Null
        End Function

        <ScriptCommand("SetSafari")>
        <ScriptDescription("Sets the safari status of the current level.")>
        Private Function M_SetSafari(ByVal argument As String) As String
            Screen.Level.IsSafariZone = Bool(argument)

            Return Core.Null
        End Function

#End Region

#Region "Constructs"

        <ScriptConstruct("File")>
        <ScriptDescription("Returns the full file path of the current level.")>
        Private Function F_File(ByVal argument As String) As String
            Return Screen.Level.LevelFile
        End Function

        <ScriptConstruct("FileName")>
        <ScriptDescription("Returns the file name (without extension) of the current level.")>
        Private Function F_FileName(ByVal argument As String) As String
            Return IO.Path.GetFileNameWithoutExtension(Screen.Level.LevelFile)
        End Function

        <ScriptConstruct("Riding")>
        <ScriptDescription("Returns if the player is currently riding a Pokémon.")>
        Private Function F_Riding(ByVal argument As String) As String
            Return ToString(Screen.Level.Riding)
        End Function

        <ScriptConstruct("Surfing")>
        <ScriptDescription("Returns if the player is currently surfing on a Pokémon.")>
        Private Function F_Surfing(ByVal argument As String) As String
            Return ToString(Screen.Level.Surfing)
        End Function

#End Region

    End Class

End Namespace