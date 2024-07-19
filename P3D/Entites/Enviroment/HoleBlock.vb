Public Class HoleBlock

    Inherits Entity

    Dim ActivateDrop As Boolean = False
    Public Overrides Sub Render()
        If Me.Model Is Nothing Then
            Me.Draw(Me.BaseModel, Textures, False)
        Else
            UpdateModel()
            Draw(Me.BaseModel, Me.Textures, True, Me.Model)
        End If
    End Sub

    Public Overrides Sub Initialize()
        MyBase.Initialize()

        Me.NeedsUpdate = True

    End Sub

    Public Overrides Function WalkIntoFunction() As Boolean
        Me.ActivateDrop = True
        If ActionScript.TempInputDirection = -1 Then
            ActionScript.TempInputDirection = Screen.Camera.GetPlayerFacingDirection()
        End If

        If Screen.Camera.Name = "Overworld" Then
            If CType(Screen.Camera, OverworldCamera).FreeCameraMode = False Then
                CType(Screen.Camera, OverworldCamera).YawLocked = True
            End If
        End If

        Screen.Level.WalkedSteps = 0
        Screen.Level.PokemonEncounterData.EncounteredPokemon = False

        Return False
    End Function

    Public Overrides Sub Update()
        If Core.CurrentScreen.Identification = Screen.Identifications.OverworldScreen Then
            If CType(Screen.Camera, OverworldCamera).IsPushingStrengthRock = False Then
                If Me.ActivateDrop = True AndAlso Screen.Camera.Position.X = Me.Position.X And Screen.Camera.Position.Z = Me.Position.Z And CInt(Screen.Camera.Position.Y) = CInt(Me.Position.Y) Then
                    Screen.Camera.StopMovement()
                    ActivateDrop = False
                    Drop()
                ElseIf CType(Screen.Camera, OverworldCamera)._moved = 0.0F Then
                    ActivateDrop = False
                End If
            ElseIf CType(Screen.Camera, OverworldCamera)._moved = 0.0F Then
                ActivateDrop = False
            End If
        End If

        MyBase.Update()
    End Sub

    Public Function Drop() As Boolean
        If IsValidLink(Me.AdditionalValue) = True And ScriptBlock.TriggeredScriptBlock = False Then
            Dim Destination As String = Me.AdditionalValue.GetSplit(0)

            Dim WarpPosition As New Vector3(CSng(Me.AdditionalValue.GetSplit(1)), CSng(Me.AdditionalValue.GetSplit(2).Replace(".", GameController.DecSeparator)), CSng(Me.AdditionalValue.GetSplit(3)))
            Dim DropDistance As Single = CSng(Me.AdditionalValue.GetSplit(4))
            Dim WarpTurns As Integer = CInt(Me.AdditionalValue.GetSplit(5))
            Dim FallSpeed As String = "2.25"
            If Me.AdditionalValue.Split(",").Count >= 7 Then
                If Me.AdditionalValue.GetSplit(6) <> "" Then
                    FallSpeed = Me.AdditionalValue.GetSplit(6)
                End If
            End If
            Dim CameraFollows As Boolean = False
            If Me.AdditionalValue.Split(",").Count >= 8 Then
                If Me.AdditionalValue.GetSplit(7) <> "" Then
                    CameraFollows = CBool(Me.AdditionalValue.GetSplit(7))
                End If
            End If

            If Me.AdditionalValue.Split(",").Count >= 9 Then
                Dim validRotations As New List(Of Integer)

                Dim rotationData() As String = Me.AdditionalValue.GetSplit(8, ",").Split(CChar("|"))
                For Each Element As String In rotationData
                    validRotations.Add(CInt(Element))
                Next
                If validRotations.Contains(Screen.Camera.GetPlayerFacingDirection()) = False Then
                    Return True
                End If
            End If
            Dim ResetFirstPerson As Boolean = False
            If CType(Screen.Camera, OverworldCamera).ThirdPerson = True Then
                ResetFirstPerson = True
            End If

            If System.IO.File.Exists(GameController.GamePath & "\" & GameModeManager.ActiveGameMode.MapPath & Destination) = True Or System.IO.File.Exists(GameController.GamePath & "\Content\Data\maps\" & Destination) = True Then
                Dim s As String = "version=2" & Environment.NewLine &
                     "@Camera.ActivateThirdPerson" & Environment.NewLine &
                     "@Camera.Reset" & Environment.NewLine &
                     "@Camera.SetPitch(-0.10)" & Environment.NewLine &
                     "@Camera.Update" & Environment.NewLine &
                     "@Camera.Fix" & Environment.NewLine &
                     "@Level.Wait(10)" & Environment.NewLine

                If Me.ActionValue > 0 Then
                    s &= "@Entity.ShowMessageBulb(" & CInt(Me.ActionValue - 1) & "|" & Screen.Camera.Position.X & "|" & CSng(Screen.Camera.Position.Y + 0.65F) & "|" & Screen.Camera.Position.Z & ")" & Environment.NewLine
                End If

                s &= "@Player.SetMovement(0,-1,0)" & Environment.NewLine &
                    "@Player.DoWalkAnimation(0)" & Environment.NewLine &
                    "@Sound.Play(Drop_Fall)" & Environment.NewLine &
                    "@Player.SetSpeed(" & FallSpeed & ")" & Environment.NewLine &
                    "@Player.Move(2)" & Environment.NewLine &
                    "@Player.SetOpacity(0)" & Environment.NewLine &
                    "@Player.ResetMovement" & Environment.NewLine &
                    "@Level.Wait(30)" & Environment.NewLine &
                    "@Screen.FadeOut(35)" & Environment.NewLine
                If CameraFollows = False Then
                    s &= "@Player.Warp(" & Destination & "," & CStr(WarpPosition.X).Replace(GameController.DecSeparator, ".") & "," & CStr(WarpPosition.Y).Replace(GameController.DecSeparator, ".") & "," & CStr(WarpPosition.Z).Replace(GameController.DecSeparator, ".") & "," & WarpTurns & ",3)" & Environment.NewLine &
                    "@Level.Update" & Environment.NewLine &
                    "@Camera.Defix" & Environment.NewLine &
                    "@Camera.Reset" & Environment.NewLine &
                    "@Camera.SetYaw(0)" & Environment.NewLine &
                    "@Camera.Update" & Environment.NewLine &
                    "@Camera.Fix" & Environment.NewLine &
                    "@Player.Warp(" & CStr(WarpPosition.X).Replace(GameController.DecSeparator, ".") & "," & CStr(WarpPosition.Y + DropDistance).Replace(GameController.DecSeparator, ".") & "," & CStr(WarpPosition.Z).Replace(GameController.DecSeparator, ".") & ")" & Environment.NewLine
                Else
                    s &= "@Player.Warp(" & Destination & "," & CStr(WarpPosition.X).Replace(GameController.DecSeparator, ".") & "," & CStr(WarpPosition.Y + DropDistance).Replace(GameController.DecSeparator, ".") & "," & CStr(WarpPosition.Z).Replace(GameController.DecSeparator, ".") & "," & WarpTurns & ",3)" & Environment.NewLine &
                   "@Level.Update" & Environment.NewLine &
                   "@Camera.Defix" & Environment.NewLine &
                   "@Camera.Reset" & Environment.NewLine &
                   "@Camera.SetYaw(0)" & Environment.NewLine &
                   "@Camera.Update" & Environment.NewLine
                End If
                s &= "@Player.SetOpacity(1)" & Environment.NewLine &
                    "@Level.Update" & Environment.NewLine &
                    "@Player.SetMovement(0,-1,0)" & Environment.NewLine &
                    "@Player.SetSpeed(" & FallSpeed & ")" & Environment.NewLine &
                    "@Screen.FadeIn(35)" & Environment.NewLine &
                    "@Player.Move(" & DropDistance & ")" & Environment.NewLine &
                    "@Sound.Play(Drop_Land)" & Environment.NewLine &
                    "@Level.Update" & Environment.NewLine &
                    "@Camera.Update" & Environment.NewLine &
                    "@Camera.Defix" & Environment.NewLine &
                    "@Camera.Reset" & Environment.NewLine &
                    "@Camera.SetThirdPerson(" & ResetFirstPerson & ")" & Environment.NewLine &
                    "@Player.DoWalkAnimation(1)" & Environment.NewLine &
                    ":end"

                CType(Core.CurrentScreen, OverworldScreen).ActionScript.StartScript(s, 2)

            Else
                CallError("Map file """ & GameModeManager.ActiveGameMode.MapPath & Destination & """ does not exist.")
            End If
        End If

        Return False
    End Function

    Public Shared Function IsValidLink(ByVal link As String) As Boolean
        If link <> "" Then
            If link.Contains(",") = True Then
                Dim c As Integer = 0
                For e = 0 To link.Length - 1
                    If link(e) = CChar(",") Then
                        c += 1
                    End If
                Next
                If c >= 5 Then
                    Dim destination As String = link.GetSplit(0)
                    If destination.EndsWith(".dat") = True Then
                        Dim x As String = link.GetSplit(1)
                        Dim y As String = link.GetSplit(2).Replace(".", GameController.DecSeparator)
                        Dim z As String = link.GetSplit(3)
                        Dim d As String = link.GetSplit(4)
                        Dim r As String = link.GetSplit(5)

                        If StringHelper.IsNumeric(x) = True And
                           StringHelper.IsNumeric(y) = True And
                           StringHelper.IsNumeric(z) = True And
                           StringHelper.IsNumeric(d) = True And
                           StringHelper.IsNumeric(r) = True Then
                            Return True
                        Else
                            CallError("Position values are not numeric.")
                            Return False
                        End If
                    Else
                        CallError("Destination file is not a valid map file.")
                        Return False
                    End If
                Else
                    CallError("Not enough or too much arguments to resolve the link.")
                    Return False
                End If
            Else
                CallError("Link does not contain seperators or has wrong seperators.")
                Return False
            End If
        Else
            CallError("Link is empty.")
            Return False
        End If
    End Function

    Private Shared Sub CallError(ByVal ex As String)
        Logger.Log(Logger.LogTypes.ErrorMessage, "WarpBlock.vb: Invalid warp! More information:" & ex)
    End Sub

End Class