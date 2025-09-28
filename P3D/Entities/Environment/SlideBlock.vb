﻿Public Class SlideBlock

    Inherits Entity

    Dim TempScriptEntity As ScriptBlock = Nothing

    Public Overrides Function WalkAgainstFunction() As Boolean
        CType(Screen.Camera, OverworldCamera).PreventMovement = True
        Dim facing As Integer = CInt(Me.Rotation.Y / MathHelper.PiOver2)
        facing -= 2
        If facing < 0 Then
            facing += 4
        End If

        Screen.Camera.PlannedMovement = Vector3.Zero

        If Screen.Camera.GetPlayerFacingDirection() = facing And Screen.Camera.IsMoving() = False Then
            CType(Screen.Camera, OverworldCamera).DidWalkAgainst = False

            Dim Steps As Integer = 0

            Dim checkPosition As Vector3 = Screen.Camera.GetForwardMovedPosition()
            checkPosition.Y = checkPosition.Y.ToInteger()

            Dim foundSteps As Boolean = True
            While foundSteps = True
                Dim e As Entity = GetEntity(Screen.Level.Entities, checkPosition, True, {GetType(SlideBlock), GetType(ScriptBlock), GetType(WarpBlock)})
                If Not e Is Nothing Then
                    If e.EntityID = "SlideBlock" Then
                        Steps += 1
                        checkPosition.X += Screen.Camera.GetMoveDirection().X
                        checkPosition.Z += Screen.Camera.GetMoveDirection().Z
                        checkPosition.Y += 1
                    Else
                        If e.EntityID = "ScriptBlock" Then
                            TempScriptEntity = CType(e, ScriptBlock)
                        ElseIf e.EntityID = "WarpBlock" Then
                            CType(e, WarpBlock).WalkAgainstFunction()
                        End If
                        foundSteps = False
                    End If
                Else
                    foundSteps = False
                End If
            End While

            Screen.Level.OverworldPokemon.Visible = False
            Screen.Level.OverworldPokemon.warped = True

            Dim walkSpeed As Single = 1.0F
            If Screen.Camera.Speed <> 0.04F Then
                walkSpeed = Screen.Camera.Speed / 0.04F
            End If

            Dim s As String = "version=2" & Environment.NewLine &
                "@player.stopmovement" & Environment.NewLine &
                "@player.setmovement(" & Screen.Camera.GetMoveDirection().X & ",1," & Screen.Camera.GetMoveDirection().Z & ")" & Environment.NewLine &
                "@player.setspeed(" & walkSpeed & ")" & Environment.NewLine &
                "@player.move(" & Steps & ")" & Environment.NewLine &
                "@player.setmovement(" & Screen.Camera.GetMoveDirection().X & ",0," & Screen.Camera.GetMoveDirection().Z & ")" & Environment.NewLine &
                "@overworldpokemon.hide" & Environment.NewLine &
                "@player.move(1)" & Environment.NewLine &
                "@overworldpokemon.hide" & Environment.NewLine &
                "@player.allowmovement" & Environment.NewLine

            If Not Me.TempScriptEntity Is Nothing Then
                s &= GetScriptStartLine(Me.TempScriptEntity) & Environment.NewLine
                Me.TempScriptEntity = Nothing
            End If

            s &= ":end"

            CType(Core.CurrentScreen, OverworldScreen).ActionScript.StartScript(s, 2, False)
            If CType(Core.CurrentScreen, OverworldScreen).ActionScript.IsReady = True And CType(Screen.Camera, OverworldCamera).PreventMovement = True Then
                CType(Screen.Camera, OverworldCamera).PreventMovement = False
            End If
            Return True
        Else
            CType(Screen.Camera, OverworldCamera).PreventMovement = False
            Return True
        End If

        facing = CInt(Me.Rotation.Y / MathHelper.PiOver2)
        If facing < 0 Then
            facing += 4
        End If
        If Screen.Camera.GetPlayerFacingDirection() = facing Then
            CType(Screen.Camera, OverworldCamera).PreventMovement = False
            Return False
        End If

        CType(Screen.Camera, OverworldCamera).PreventMovement = False
        Return True
    End Function

    Private Function GetScriptStartLine(ByVal ScriptEntity As ScriptBlock) As String
        If Not ScriptEntity Is Nothing Then
            If ScriptEntity.CorrectRotation() = True Then
                Select Case ScriptEntity.GetActivationID()
                    Case 0
                        Return "@script.start(" & ScriptEntity.ScriptID & ")"
                    Case 1
                        Return "@script.text(" & ScriptEntity.ScriptID & ")"
                    Case 2
                        Return "@script.run(" & ScriptEntity.ScriptID & ")"
                End Select
            End If
        End If

        Return ""
    End Function

    Public Overrides Sub WalkOntoFunction()
        CType(Screen.Camera, OverworldCamera).PreventMovement = True
        Dim facing As Integer = CInt(Me.Rotation.Y / MathHelper.PiOver2)

        Screen.Camera.PlannedMovement = Vector3.Zero

        If Screen.Camera.GetPlayerFacingDirection() = facing Then
            CType(Screen.Camera, OverworldCamera).DidWalkAgainst = False

            Dim Steps As Integer = 0

            Dim checkPosition As Vector3 = Screen.Camera.GetForwardMovedPosition()
            checkPosition.Y = checkPosition.Y.ToInteger() - 1

            Dim foundSteps As Boolean = True
            While foundSteps = True
                Dim e As Entity = GetEntity(Screen.Level.Entities, checkPosition, True, {GetType(SlideBlock), GetType(ScriptBlock), GetType(WarpBlock)})
                If Not e Is Nothing Then
                    If e.EntityID = "SlideBlock" Then
                        Steps += 1
                        checkPosition.X += Screen.Camera.GetMoveDirection().X
                        checkPosition.Z += Screen.Camera.GetMoveDirection().Z
                        checkPosition.Y -= 1
                    Else
                        If e.EntityID = "ScriptBlock" Then
                            Me.TempScriptEntity = CType(e, ScriptBlock)
                        ElseIf e.EntityID = "WarpBlock" Then
                            CType(e, WarpBlock).WalkAgainstFunction()
                        End If
                        foundSteps = False
                    End If
                Else
                    foundSteps = False
                End If
            End While

            Screen.Level.OverworldPokemon.Visible = False
            Screen.Level.OverworldPokemon.warped = True

            Dim walkSpeed As Single = 1.0F
            If Screen.Camera.Speed <> 0.04F Then
                walkSpeed = Screen.Camera.Speed / 0.04F
            End If

            Dim s As String = "version=2" & Environment.NewLine &
            "@player.stopmovement" & Environment.NewLine &
            "@player.setmovement(" & Screen.Camera.GetMoveDirection().X & ",0," & Screen.Camera.GetMoveDirection().Z & ")" & Environment.NewLine &
            "@player.setspeed(" & walkSpeed & ")" & Environment.NewLine &
            "@player.move(1)" & Environment.NewLine &
            "@player.setmovement(" & Screen.Camera.GetMoveDirection().X & ",-1," & Screen.Camera.GetMoveDirection().Z & ")" & Environment.NewLine &
            "@player.move(" & Steps & ")" & Environment.NewLine &
            "@overworldpokemon.hide" & Environment.NewLine &
            "@player.allowmovement" & Environment.NewLine

            If Not Me.TempScriptEntity Is Nothing Then
                s &= GetScriptStartLine(Me.TempScriptEntity) & Environment.NewLine
                Me.TempScriptEntity = Nothing
            End If

            s &= ":end"

            CType(Core.CurrentScreen, OverworldScreen).ActionScript.StartScript(s, 2, False)
            If CType(Core.CurrentScreen, OverworldScreen).ActionScript.IsReady = True And CType(Screen.Camera, OverworldCamera).PreventMovement = True Then
                CType(Screen.Camera, OverworldCamera).PreventMovement = False
            End If
        Else
            CType(Screen.Camera, OverworldCamera).PreventMovement = False
        End If
    End Sub

    Public Overrides Sub Render()
        If Me.Model Is Nothing Then
            Me.Draw(Me.BaseModel, Textures, False)
        Else
            UpdateModel()
            Draw(Me.BaseModel, Me.Textures, True, Me.Model)
        End If
    End Sub

End Class