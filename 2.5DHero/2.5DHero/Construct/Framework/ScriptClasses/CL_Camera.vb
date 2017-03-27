Namespace Construct.Framework.Classes

    <ScriptClass("Camera")>
    <ScriptDescription("An interface to alter the game's camera.")>
    Public Class CL_Camera

        Inherits ScriptClass

        Public Sub New()
            MyBase.New(True)
        End Sub

#Region "Utilities"

        Private ReadOnly Property DefaultCamera() As Camera
            Get
                Return Screen.Camera
            End Get
        End Property

        Private ReadOnly Property OverworldCamera() As OverworldCamera
            Get
                Return CType(Screen.Camera, OverworldCamera)
            End Get
        End Property

        Private Sub DoCameraUpdate(ByVal fullUpdate As Boolean)
            OverworldCamera.UpdateThirdPersonCamera()

            If fullUpdate = True Then
                OverworldCamera.UpdateFrustum()
                OverworldCamera.UpdateViewMatrix()
                Screen.Level.Entities = (From e In Screen.Level.Entities Order By e.CameraDistance Descending).ToList()
                Screen.Level.UpdateEntities()
            End If
        End Sub

#End Region

#Region "Commands"

        <ScriptCommand("Set")>
        <ScriptDescription("Sets the properties of the camera.")>
        Private Function M_Set(ByVal argument As String) As String
            Dim yaw As Single = Sng(argument.GetSplit(3).Replace("~", CStr(DefaultCamera.Yaw)))
            Dim pitch As Single = Sng(argument.GetSplit(4).Replace("~", CStr(DefaultCamera.Pitch)))

            If Controller.GetInstance().Context = ScriptContext.Overworld Then
                Dim x As Single = Sng(argument.GetSplit(0).Replace("~", CStr(OverworldCamera.ThirdPersonOffset.X)))
                Dim y As Single = Sng(argument.GetSplit(1).Replace("~", CStr(OverworldCamera.ThirdPersonOffset.Y)))
                Dim z As Single = Sng(argument.GetSplit(2).Replace("~", CStr(OverworldCamera.ThirdPersonOffset.Z)))

                OverworldCamera.ThirdPersonOffset = New Vector3(x, y, z)
                OverworldCamera.Yaw = yaw
                OverworldCamera.Pitch = pitch

                DoCameraUpdate(True)
            Else
                Dim x As Single = Sng(argument.GetSplit(0).Replace("~", CStr(DefaultCamera.Position.X)))
                Dim y As Single = Sng(argument.GetSplit(1).Replace("~", CStr(DefaultCamera.Position.Y)))
                Dim z As Single = Sng(argument.GetSplit(2).Replace("~", CStr(DefaultCamera.Position.Z)))

                DefaultCamera.Position = New Vector3(x, y, z)
                DefaultCamera.Yaw = yaw
                DefaultCamera.Pitch = pitch
            End If

            Return Core.Null
        End Function

        <ScriptCommand("Reset", RequiredContext:=ScriptContext.Overworld)>
        <ScriptDescription("Resets the camera to the default properties.")>
        Private Function M_Reset(ByVal argument As String) As String
            Dim fullUpdate As Boolean = True

            OverworldCamera.ThirdPersonOffset = New Vector3(0.0F, 0.3F, 1.5F)
            If argument <> "" Then
                fullUpdate = Bool(argument)
            End If

            DoCameraUpdate(fullUpdate)

            Return Core.Null
        End Function

        <ScriptCommand("SetPosition")>
        <ScriptDescription("Sets the position of the camera.")>
        Private Function M_SetPosition(ByVal argument As String) As String
            If Controller.GetInstance().Context = ScriptContext.Overworld Then
                Dim fullUpdate As Boolean = True

                Dim x As Single = Sng(argument.GetSplit(0).Replace("~", CStr(OverworldCamera.ThirdPersonOffset.X)))
                Dim y As Single = Sng(argument.GetSplit(1).Replace("~", CStr(OverworldCamera.ThirdPersonOffset.Y)))
                Dim z As Single = Sng(argument.GetSplit(2).Replace("~", CStr(OverworldCamera.ThirdPersonOffset.Z)))

                OverworldCamera.ThirdPersonOffset = New Vector3(x, y, z)

                DoCameraUpdate(fullUpdate)
            Else
                Dim x As Single = Sng(argument.GetSplit(0).Replace("~", CStr(DefaultCamera.Position.X)))
                Dim y As Single = Sng(argument.GetSplit(1).Replace("~", CStr(DefaultCamera.Position.Y)))
                Dim z As Single = Sng(argument.GetSplit(2).Replace("~", CStr(DefaultCamera.Position.Z)))

                DefaultCamera.Position = New Vector3(x, y, z)
            End If

            Return Core.Null
        End Function

        <ScriptCommand("SetYaw")>
        <ScriptDescription("Sets the yaw property of the camera.")>
        Private Function M_Setyaw(ByVal argument As String) As String
            Dim yaw As Single = Sng(argument.Replace("~", CStr(DefaultCamera.Yaw)))
            DefaultCamera.Yaw = yaw

            If Controller.GetInstance().Context = ScriptContext.Overworld Then
                DoCameraUpdate(True)
            End If

            Return Core.Null
        End Function

        <ScriptCommand("SetPitch")>
        <ScriptDescription("Sets the pitch property of the camera.")>
        Private Function M_Setpitch(ByVal argument As String) As String
            Dim pitch As Single = Sng(argument.Replace("~", CStr(DefaultCamera.Pitch)))
            DefaultCamera.Pitch = pitch

            If Controller.GetInstance().Context = ScriptContext.Overworld Then
                DoCameraUpdate(True)
            End If

            Return Core.Null
        End Function

        <ScriptCommand("SetX")>
        <ScriptDescription("Sets the x position of the camera.")>
        Private Function M_Setx(ByVal argument As String) As String
            If Controller.GetInstance().Context = ScriptContext.Overworld Then
                Dim x As Single = Sng(argument.Replace("~", CStr(OverworldCamera.ThirdPersonOffset.X)))
                OverworldCamera.ThirdPersonOffset.X = x

                DoCameraUpdate(True)
            Else
                Dim x As Single = Sng(argument.Replace("~", CStr(DefaultCamera.Position.X)))
                DefaultCamera.Position.X = x
            End If

            Return Core.Null
        End Function

        <ScriptCommand("SetY")>
        <ScriptDescription("Sets the y position of the camera.")>
        Private Function M_Sety(ByVal argument As String) As String
            If Controller.GetInstance().Context = ScriptContext.Overworld Then
                Dim y As Single = Sng(argument.Replace("~", CStr(OverworldCamera.ThirdPersonOffset.Y)))
                OverworldCamera.ThirdPersonOffset.Y = y

                DoCameraUpdate(True)
            Else
                Dim y As Single = Sng(argument.Replace("~", CStr(DefaultCamera.Position.Y)))
                DefaultCamera.Position.Y = y
            End If

            Return Core.Null
        End Function

        <ScriptCommand("SetZ")>
        <ScriptDescription("Sets the z position of the camera.")>
        Private Function M_Setz(ByVal argument As String) As String
            If Controller.GetInstance().Context = ScriptContext.Overworld Then
                Dim z As Single = Sng(argument.Replace("~", CStr(OverworldCamera.ThirdPersonOffset.Z)))
                OverworldCamera.ThirdPersonOffset.Z = z

                DoCameraUpdate(True)
            Else
                Dim z As Single = Sng(argument.Replace("~", CStr(DefaultCamera.Position.Z)))
                DefaultCamera.Position.Z = z
            End If

            Return Core.Null
        End Function

        <ScriptCommand("ToggleThirdPerson", RequiredContext:=ScriptContext.Overworld)>
        <ScriptDescription("Toggles the third person property of the camera.")>
        Private Function M_ToggleThirdPerson(ByVal argument As String) As String
            Dim fullUpdate As Boolean = True

            OverworldCamera.SetThirdPerson(Not OverworldCamera.ThirdPerson, False)
            If argument <> "" Then
                fullUpdate = Bool(argument)
            End If

            DoCameraUpdate(fullUpdate)

            Return Core.Null
        End Function

        <ScriptCommand("ActivateThirdPerson", RequiredContext:=ScriptContext.Overworld)>
        <ScriptDescription("Activates the third person property of the camera.")>
        Private Function M_ActivateThirdPerson(ByVal argument As String) As String
            Dim fullUpdate As Boolean = True

            OverworldCamera.SetThirdPerson(True, False)
            If argument <> "" Then
                fullUpdate = Bool(argument)
            End If

            DoCameraUpdate(fullUpdate)

            Return Core.Null
        End Function

        <ScriptCommand("DeactivateThirdPerson", RequiredContext:=ScriptContext.Overworld)>
        <ScriptDescription("Deactivates the third person property of the camera.")>
        Private Function M_DeactivateThirdPerson(ByVal argument As String) As String
            Dim fullUpdate As Boolean = True

            OverworldCamera.SetThirdPerson(False, False)
            If argument <> "" Then
                fullUpdate = Bool(argument)
            End If

            DoCameraUpdate(fullUpdate)

            Return Core.Null
        End Function

        <ScriptCommand("SetThirdPerson", RequiredContext:=ScriptContext.Overworld)>
        <ScriptDescription("Sets the third person property of the camera.")>
        Private Function M_SetThirdPerson(ByVal argument As String) As String
            Dim fullUpdate As Boolean = True

            OverworldCamera.SetThirdPerson(Bool(argument.Split(","c)(0)), False)
            If argument.Split(","c).Length > 1 Then
                fullUpdate = Bool(argument.Split(","c)(1))
            End If

            DoCameraUpdate(fullUpdate)

            Return Core.Null
        End Function

        <ScriptCommand("SetToPlayerFacing")>
        <ScriptDescription("Sets the camera's yaw to face the player.")>
        Private Function M_SetToPlayerFacing(ByVal argument As String) As String
            Dim facing As Integer = Screen.Camera.GetPlayerFacingDirection()
            DefaultCamera.Yaw = facing * MathHelper.PiOver2

            If Controller.GetInstance().Context = ScriptContext.Overworld Then
                DoCameraUpdate(True)
            End If

            Return Core.Null
        End Function

        <ScriptCommand("Fix", RequiredContext:=ScriptContext.Overworld)>
        <ScriptDescription("Activates the fix property of the camera.")>
        Private Function M_Fix(ByVal argument As String) As String
            Dim fullUpdate As Boolean = True

            OverworldCamera.Fixed = True
            If argument <> "" Then
                fullUpdate = Bool(argument)
            End If

            DoCameraUpdate(fullUpdate)

            Return Core.Null
        End Function

        <ScriptCommand("Defix", RequiredContext:=ScriptContext.Overworld)>
        <ScriptDescription("Deactivates the fix property of the camera.")>
        Private Function M_Defix(ByVal argument As String) As String
            Dim fullUpdate As Boolean = True

            OverworldCamera.Fixed = False
            If argument <> "" Then
                fullUpdate = Bool(argument)
            End If

            DoCameraUpdate(fullUpdate)

            Return Core.Null
        End Function

        <ScriptCommand("ToggleFix", RequiredContext:=ScriptContext.Overworld)>
        <ScriptDescription("Toggles the fix property of the camera.")>
        Private Function M_Togglefix(ByVal argument As String) As String
            Dim fullUpdate As Boolean = True

            OverworldCamera.Fixed = Not OverworldCamera.Fixed
            If argument <> "" Then
                fullUpdate = Bool(argument)
            End If

            DoCameraUpdate(fullUpdate)

            Return Core.Null
        End Function

        <ScriptCommand("Update")>
        <ScriptDescription("Updates the camera.")>
        Private Function M_Update(ByVal argument As String) As String
            If Controller.GetInstance().Context = ScriptContext.Overworld Then
                DoCameraUpdate(True)
            Else
                DefaultCamera.Update()
                Screen.Level.Entities = (From e In Screen.Level.Entities Order By e.CameraDistance Descending).ToList()
                Screen.Level.UpdateEntities()
            End If

            ActiveLine.EndExecutionFrame = True

            Return Core.Null
        End Function

        <ScriptCommand("SetFocus", RequiredContext:=ScriptContext.Overworld)>
        <ScriptDescription("Sets the camera focus on a different object.")>
        Private Function M_SetFocus(ByVal argument As String) As String
            Dim focusType = OverworldCamera.CameraFocusTypes.Player
            Select Case argument.Split(","c)(0).ToLower()
                Case "player"
                    focusType = OverworldCamera.CameraFocusTypes.Player
                Case "npc"
                    focusType = OverworldCamera.CameraFocusTypes.NPC
                Case "entity"
                    focusType = OverworldCamera.CameraFocusTypes.Entity
            End Select
            OverworldCamera.SetupFocus(focusType, Int(argument.Split(","c)(1)))

            DoCameraUpdate(True)

            Return Core.Null
        End Function

        <ScriptCommand("SetFocusType", RequiredContext:=ScriptContext.Overworld)>
        <ScriptDescription("Sets the camera focus type.")>
        Private Function M_SetFocusType(ByVal argument As String) As String
            Select Case argument.ToLower()
                Case "player"
                    OverworldCamera.CameraFocusType = OverworldCamera.CameraFocusTypes.Player
                Case "npc"
                    OverworldCamera.CameraFocusType = OverworldCamera.CameraFocusTypes.NPC
                Case "entity"
                    OverworldCamera.CameraFocusType = OverworldCamera.CameraFocusTypes.Entity
            End Select

            DoCameraUpdate(True)

            Return Core.Null
        End Function

        <ScriptCommand("SetFocusID", RequiredContext:=ScriptContext.Overworld)>
        <ScriptDescription("Sets the camera focus Id.")>
        Private Function M_SetFocusID(ByVal argument As String) As String
            OverworldCamera.CameraFocusID = Int(argument)

            DoCameraUpdate(True)

            Return Core.Null
        End Function

        <ScriptCommand("ResetFocus", RequiredContext:=ScriptContext.Overworld)>
        <ScriptDescription("Reset the camera's focus settings to their default.")>
        Private Function M_ResetFocus(ByVal argument As String) As String
            OverworldCamera.CameraFocusType = OverworldCamera.CameraFocusTypes.Player
            OverworldCamera.CameraFocusID = -1

            DoCameraUpdate(True)

            Return Core.Null
        End Function

#End Region

#Region "Constructs"

        <ScriptConstruct("IsFixed", RequiredContext:=ScriptContext.Overworld)>
        <ScriptDescription("If the camera is fixed.")>
        Private Function F_IsFixed(ByVal argument As String) As String
            Return ToString(OverworldCamera.Fixed)
        End Function

        <ScriptConstruct("X")>
        <ScriptDescription("The x position of the camera.")>
        Private Function F_X(ByVal argument As String) As String
            If Controller.GetInstance().Context = ScriptContext.Overworld Then
                Return ToString(OverworldCamera.ThirdPersonOffset.X)
            Else
                Return ToString(DefaultCamera.Position.X)
            End If
        End Function

        <ScriptConstruct("Y")>
        <ScriptDescription("The y position of the camera.")>
        Private Function F_Y(ByVal argument As String) As String
            If Controller.GetInstance().Context = ScriptContext.Overworld Then
                Return ToString(OverworldCamera.ThirdPersonOffset.Y)
            Else
                Return ToString(DefaultCamera.Position.Y)
            End If
        End Function

        <ScriptConstruct("Z")>
        <ScriptDescription("The z position of the camera.")>
        Private Function F_Z(ByVal argument As String) As String
            If Controller.GetInstance().Context = ScriptContext.Overworld Then
                Return ToString(OverworldCamera.ThirdPersonOffset.Z)
            Else
                Return ToString(DefaultCamera.Position.Z)
            End If
        End Function

        <ScriptConstruct("Yaw")>
        <ScriptDescription("The yaw setting of the camera.")>
        Private Function F_Yaw(ByVal argument As String) As String
            Return ToString(DefaultCamera.Yaw)
        End Function

        <ScriptConstruct("Pitch")>
        <ScriptDescription("The pitch setting of the camera.")>
        Private Function F_Pitch(ByVal argument As String) As String
            Return ToString(DefaultCamera.Pitch)
        End Function

        <ScriptConstruct("ThirdPerson", RequiredContext:=ScriptContext.Overworld)>
        <ScriptDescription("If the camera is in third person mode.")>
        Private Function F_Thirdperson(ByVal argument As String) As String
            Return ToString(OverworldCamera.ThirdPerson)
        End Function

#End Region

    End Class

End Namespace