Option Strict On
Imports Pokemon3D.Scripting.Adapters

Namespace Scripting.V3.Prototypes

    <ScriptPrototype(VariableName:="Camera")>
    Friend NotInheritable Class CameraPrototype

        Private Shared Function GetCamera() As OverworldCamera

            Return CType(Screen.Camera, OverworldCamera)

        End Function

        Private Shared Sub UpdateCamera(fullUpdate As Boolean)
            Dim camera = GetCamera()

            camera.UpdateThirdPersonCamera()
            If fullUpdate Then
                camera.UpdateFrustum()
                camera.UpdateViewMatrix()
                Screen.Level.Entities = (From e In Screen.Level.Entities Order By e.CameraDistance Descending).ToList()
                Screen.Level.UpdateEntities()
            End If
        End Sub

        Private Shared Function ParseFocusType(focusType As String) As OverworldCamera.CameraFocusTypes
            Select Case focusType.ToLowerInvariant()
                Case "player"
                    Return OverworldCamera.CameraFocusTypes.Player
                Case "npc"
                    Return OverworldCamera.CameraFocusTypes.NPC
                Case "entity"
                    Return OverworldCamera.CameraFocusTypes.Entity
            End Select
            Return OverworldCamera.CameraFocusTypes.Player
        End Function

        <ScriptFunction(ScriptFunctionType.Standard, VariableName:="update", IsStatic:=True)>
        Public Shared Function Update(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            UpdateCamera(True)
            ScriptManager.Instance.WaitFrames(1)

            Return Nothing

        End Function

        <ScriptFunction(ScriptFunctionType.Standard, VariableName:="setBehindPlayer", IsStatic:=True)>
        Public Shared Function SetBehindPlayer(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            Dim facing = Screen.Camera.GetPlayerFacingDirection() * MathHelper.PiOver2
            GetCamera().Yaw = facing
            UpdateCamera(True)

            Return facing

        End Function

#Region "Position"

        <ScriptFunction(ScriptFunctionType.Getter, VariableName:="position", IsStatic:=True)>
        Public Shared Function GetPosition(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            Dim position = GetCamera().ThirdPersonOffset
            Return New Vector3Prototype(position)

        End Function

        <ScriptFunction(ScriptFunctionType.Setter, VariableName:="position", IsStatic:=True)>
        Public Shared Function SetPosition(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            If TypeContract.Ensure(parameters, {GetType(Vector3Prototype)}) Then

                Dim camera = GetCamera()
                Dim v3 = CType(parameters(0), Vector3Prototype)

                camera.ThirdPersonOffset = v3.ToVector3()

                UpdateCamera(True)

            End If

            Return NetUndefined.Instance

        End Function

#End Region

#Region "Yaw"

        <ScriptFunction(ScriptFunctionType.Getter, VariableName:="yaw", IsStatic:=True)>
        Public Shared Function GetYaw(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            Return GetCamera().Yaw

        End Function

        <ScriptFunction(ScriptFunctionType.Setter, VariableName:="yaw", IsStatic:=True)>
        Public Shared Function SetYaw(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            If TypeContract.Ensure(parameters, {TypeContract.Number}) Then

                GetCamera().Yaw = CType(parameters(0), Single)
                UpdateCamera(True)

            End If

            Return NetUndefined.Instance

        End Function

#End Region

#Region "Pitch"

        <ScriptFunction(ScriptFunctionType.Getter, VariableName:="pitch", IsStatic:=True)>
        Public Shared Function GetPitch(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            Return GetCamera().Pitch

        End Function

        <ScriptFunction(ScriptFunctionType.Setter, VariableName:="pitch", IsStatic:=True)>
        Public Shared Function SetPitch(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            If TypeContract.Ensure(parameters, {TypeContract.Number}) Then

                GetCamera().Pitch = CType(parameters(0), Single)
                UpdateCamera(True)

            End If

            Return NetUndefined.Instance

        End Function

#End Region

#Region "X, Y, Z"

        <ScriptFunction(ScriptFunctionType.Getter, VariableName:="x", IsStatic:=True)>
        Public Shared Function GetX(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            Return GetCamera().ThirdPersonOffset.X

        End Function

        <ScriptFunction(ScriptFunctionType.Setter, VariableName:="x", IsStatic:=True)>
        Public Shared Function SetX(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            If TypeContract.Ensure(parameters, {TypeContract.Number}) Then

                GetCamera().ThirdPersonOffset.X = CType(parameters(0), Single)
                UpdateCamera(True)

            End If

            Return NetUndefined.Instance

        End Function

        <ScriptFunction(ScriptFunctionType.Getter, VariableName:="y", IsStatic:=True)>
        Public Shared Function GetY(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            Return GetCamera().ThirdPersonOffset.Y

        End Function

        <ScriptFunction(ScriptFunctionType.Setter, VariableName:="y", IsStatic:=True)>
        Public Shared Function SetY(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            If TypeContract.Ensure(parameters, {TypeContract.Number}) Then

                GetCamera().ThirdPersonOffset.Y = CType(parameters(0), Single)
                UpdateCamera(True)

            End If

            Return NetUndefined.Instance

        End Function

        <ScriptFunction(ScriptFunctionType.Getter, VariableName:="z", IsStatic:=True)>
        Public Shared Function GetZ(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            Return GetCamera().ThirdPersonOffset.Z

        End Function

        <ScriptFunction(ScriptFunctionType.Setter, VariableName:="z", IsStatic:=True)>
        Public Shared Function SetZ(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            If TypeContract.Ensure(parameters, {TypeContract.Number}) Then

                GetCamera().ThirdPersonOffset.Z = CType(parameters(0), Single)
                UpdateCamera(True)

            End If

            Return NetUndefined.Instance

        End Function

#End Region

#Region "ThirdPerson"

        <ScriptFunction(ScriptFunctionType.Getter, VariableName:="thirdPerson", IsStatic:=True)>
        Public Shared Function GetThirdPerson(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            Return GetCamera().ThirdPerson

        End Function

        <ScriptFunction(ScriptFunctionType.Setter, VariableName:="thirdPerson", IsStatic:=True)>
        Public Shared Function SetThirdPerson(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            If TypeContract.Ensure(parameters, GetType(Boolean)) Then

                GetCamera().SetThirdPerson(CType(parameters(0), Boolean), False)

            End If

            Return NetUndefined.Instance

        End Function

#End Region

#Region "Fixed"

        <ScriptFunction(ScriptFunctionType.Getter, VariableName:="fixed", IsStatic:=True)>
        Public Shared Function GetFixed(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            Return GetCamera().Fixed

        End Function

        <ScriptFunction(ScriptFunctionType.Setter, VariableName:="fixed", IsStatic:=True)>
        Public Shared Function SetFixed(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            If TypeContract.Ensure(parameters, GetType(Boolean)) Then

                GetCamera().Fixed = CType(parameters(0), Boolean)

            End If

            Return NetUndefined.Instance

        End Function

#End Region

#Region "Focus"

        <ScriptFunction(ScriptFunctionType.Standard, VariableName:="setupFocus", IsStatic:=True)>
        Public Shared Function SetupFocus(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            If TypeContract.Ensure(parameters, {GetType(String), GetType(Integer)}) Then

                Dim helper = New ParamHelper(parameters)

                Dim focusTypeStr = helper.Pop(Of String)
                Dim focusId = helper.Pop(Of Integer)

                GetCamera().SetupFocus(ParseFocusType(focusTypeStr), focusId)

                UpdateCamera(True)

            End If

            Return NetUndefined.Instance

        End Function

        <ScriptFunction(ScriptFunctionType.Standard, VariableName:="resetFocus", IsStatic:=True)>
        Public Shared Function ResetFocus(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            GetCamera().SetupFocus(OverworldCamera.CameraFocusTypes.Player, -1)
            UpdateCamera(True)

            Return NetUndefined.Instance

        End Function

        <ScriptFunction(ScriptFunctionType.Setter, VariableName:="focusType", IsStatic:=True)>
        Public Shared Function SetFocusType(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            If TypeContract.Ensure(parameters, GetType(String)) Then

                GetCamera().CameraFocusType = ParseFocusType(CType(parameters(0), String))
                UpdateCamera(True)

            End If

            Return NetUndefined.Instance

        End Function

        <ScriptFunction(ScriptFunctionType.Setter, VariableName:="focusId", IsStatic:=True)>
        Public Shared Function SetFocusId(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            If TypeContract.Ensure(parameters, GetType(Integer)) Then

                GetCamera().CameraFocusID = CType(parameters(0), Integer)
                UpdateCamera(True)

            End If

            Return NetUndefined.Instance

        End Function

#End Region

    End Class

End Namespace
