Public Class StrengthRock

    Inherits Entity

    Public Overrides Sub Initialize()
        MyBase.Initialize()

        Me.CanMove = True
    End Sub

    Public Overrides Sub ClickFunction()
        If Screen.Level.UsedStrength = True Then
            Dim text As String = "Pokémon with Strength are~able to move this."
            Screen.TextBox.Show(text, {Me})
            SoundManager.PlaySound("select")
        Else
            Dim pName As String = ""

            For Each p As Pokemon In Core.Player.Pokemons
                If p.IsEgg() = False Then
                    For Each a As BattleSystem.Attack In p.Attacks
                        If a.Name = "Strength" Then
                            pName = p.GetDisplayName()
                            Exit For
                        End If
                    Next
                End If

                If pName <> "" Then
                    Exit For
                End If
            Next

            Dim text As String = Localization.GetString("fieldmove_strength_1", "A Pokémon may be able~to move this.")

            If pName <> "" And Badge.CanUseHMMove(Badge.HMMoves.Strength) = True Or GameController.IS_DEBUG_ACTIVE = True Or Core.Player.SandBoxMode = True Then
                text &= Localization.GetString("fieldmove_strength_2", "*Do you want to use Strength?") & "%" & Localization.GetString("global_yes", "Yes") & "|" & Localization.GetString("global_no", "No") & "%"
            End If
            ChooseBox.CancelIndex = 1
            Screen.TextBox.Show(text, {Me})
            SoundManager.PlaySound("select")
        End If
    End Sub

    Public Overrides Sub ResultFunction(Result As Integer)
        If Result = 0 Then
            Dim useP As Pokemon = Nothing

            For Each p As Pokemon In Core.Player.Pokemons
                If p.IsEgg() = False Then
                    For Each a As BattleSystem.Attack In p.Attacks
                        If a.Name = "Strength" Then
                            useP = p
                            Exit For
                        End If
                    Next
                End If

                If Not useP Is Nothing Then
                    Exit For
                End If
            Next

            Dim pName As String = "MissingNo."
            Dim pNumber As Integer = 23

            If Not useP Is Nothing Then
                pName = useP.GetDisplayName()
                pNumber = useP.Number
            Else
                If GameController.IS_DEBUG_ACTIVE = True Or Core.Player.SandBoxMode = True Then
                    useP = Core.Player.Pokemons(0)
                End If
            End If

            Screen.Level.UsedStrength = True

            SoundManager.PlayPokemonCry(pNumber, PokemonForms.GetCrySuffix(useP))
            Screen.TextBox.Show(pName & " " & Localization.GetString("fieldmove_strength_used", "used~Strength!"), {}, True, False)
            PlayerStatistics.Track("Strength used", 1)
        End If
    End Sub

    Public Overrides Function WalkAgainstFunction() As Boolean
        If Screen.Level.UsedStrength = True And Me.Moved = 0.0F Then
            Dim newPosition As Vector3 = Screen.Camera.GetForwardMovedPosition()
            newPosition.Y = newPosition.Y.ToInteger()
            newPosition.X += Screen.Camera.GetMoveDirection().X
            newPosition.Z += Screen.Camera.GetMoveDirection().Z

            If CheckCollision(newPosition) = True Then
                CType(Screen.Camera, OverworldCamera).IsPushingStrengthRock = True
                Me.Moved = 1
                Me.FaceDirection = Screen.Camera.GetPlayerFacingDirection()
                SoundManager.PlaySound("FieldMove_Strength", False)
            End If
        End If

        Return True
    End Function

    Private Function CheckCollision(ByVal newPosition As Vector3) As Boolean
        newPosition = New Vector3(CInt(newPosition.X), CInt(newPosition.Y), CInt(newPosition.Z))

        Dim HasFloor As Boolean = False

        Dim Position2D As Vector3 = New Vector3(newPosition.X, newPosition.Y - 0.1F, newPosition.Z)
        For Each Floor As Entity In Screen.Level.Floors
            If Floor.boundingBox.Contains(Position2D) = ContainmentType.Contains Then
                HasFloor = True
            End If
        Next

        If HasFloor = False Then
            Return False
        End If

        For Each Entity As Entity In Screen.Level.Entities
            If Entity.boundingBox.Contains(newPosition) = ContainmentType.Contains Then
                If Entity.Collision = True Then
                    Return False
                End If
            End If
        Next

        Return True
    End Function

    Public Overrides Sub UpdateEntity()
        If Me.BaseModel Is BaseModel.getModelbyID(3) Then
            If Me.Rotation.Y <> Screen.Camera.Yaw Then
                Me.Rotation.Y = Screen.Camera.Yaw
                CreatedWorld = False
            End If
        End If

        MyBase.UpdateEntity()
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