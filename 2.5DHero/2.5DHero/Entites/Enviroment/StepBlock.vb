Public Class StepBlock

    Inherits Entity

    Public Overrides Function WalkAgainstFunction() As Boolean
        Dim facing As Integer = CInt(Me.Rotation.Y / MathHelper.PiOver2)
        facing -= 2
        If facing < 0 Then
            facing += 4
        End If

        If Screen.Camera.GetPlayerFacingDirection() = facing Then 
            'Dim newPos As New Vector3(Screen.Camera.Position.X + Screen.Camera.moveDirectionX * 2, Screen.Camera.Position.Y, Screen.Camera.Position.Z + Screen.Camera.moveDirectionZ * 2)
            'If CType(Screen.Camera, OverworldCamera).CheckCollision(newPos) = True Then

            'End If
            Screen.Camera.AddToPlannedMovement(New Vector3(0, 0.15F, 0))
            Screen.Camera.Move(1.0F)
            Screen.Level.OverworldPokemon.Visible = False
            Screen.Level.OverworldPokemon.warped = True

            SoundManager.PlaySound("jump_ledge", False)

            Return False
        End If
        Return True
    End Function

    Public Overrides Sub Render()
        Me.Draw(Me.Model, Textures, False)
    End Sub

End Class