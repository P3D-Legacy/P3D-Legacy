Public Class StepBlock

    Inherits Entity

    Public Overrides Function WalkAgainstFunction() As Boolean
        Dim facing As Integer = CInt(Me.Rotation.Y / MathHelper.PiOver2)
        facing -= 2
        If facing < 0 Then
            facing += 4
        End If

        Dim CanJumpLedge As Boolean = True
        For Each Entity As Entity In Screen.Level.Entities
            If Entity.Collision = True And Entity.GetType <> GetType(StepBlock) Then
                If Entity.boundingBox.Contains(Me.Position) = ContainmentType.Contains Then
                    CanJumpLedge = False
                End If
            End If
        Next

        Dim checkBlockedPosition As Vector3 = Screen.Camera.GetForwardMovedPosition()
        checkBlockedPosition.X += Screen.Camera.GetMoveDirection().X
        checkBlockedPosition.Z += Screen.Camera.GetMoveDirection().Z
        Dim blockEntity As Entity = GetEntity(Screen.Level.Entities, checkBlockedPosition, True, {GetType(NPC), GetType(StrengthRock)})

        If blockEntity IsNot Nothing Then
            If blockEntity.Collision = True Then
                CanJumpLedge = False
            End If
        End If

        If CanJumpLedge = True Then

            If Screen.Camera.GetPlayerFacingDirection() = facing Then
                'Dim newPos As New Vector3(Screen.Camera.Position.X + Screen.Camera.moveDirectionX * 2, Screen.Camera.Position.Y, Screen.Camera.Position.Z + Screen.Camera.moveDirectionZ * 2)
                'If CType(Screen.Camera, OverworldCamera).CheckCollision(newPos) = True Then

                'End If
                Screen.Camera.AddToPlannedMovement(New Vector3(0, 0.15F, 0))
                Screen.Camera.Move(1.0F)
                Screen.Level.OverworldPokemon.Visible = False
                Screen.Level.OverworldPokemon.warped = True

                Dim delay As Date = Date.Now
                If Screen.Level.Riding = True Then
                    delay = Date.Now.AddMilliseconds(401)
                ElseIf Core.Player.IsRunning() = True Then
                    delay = Date.Now.AddMilliseconds(540)
                Else
                    delay = Date.Now.AddMilliseconds(867)
                End If

                SoundManager.PlaySound("jump_ledge", False, delay)

                Return False
            End If
        End If
        Return True
    End Function

    Public Overrides Sub Render()
        If Me.Model Is Nothing Then
            Me.Draw(Me.BaseModel, Textures, False)
        Else
            UpdateModel()
            Draw(Me.BaseModel, Me.Textures, True, Me.Model)
        End If
    End Sub

End Class