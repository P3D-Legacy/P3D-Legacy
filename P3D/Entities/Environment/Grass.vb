﻿Public Class Grass

    Inherits Entity

    Protected Overrides Function CalculateCameraDistance(CPosition As Vector3) as Single
        Return MyBase.CalculateCameraDistance(CPosition) - 0.4f
    End Function

    Public Overrides Sub UpdateEntity()
        If Me.Rotation.Y <> Screen.Camera.Yaw Then
            Me.Rotation.Y = Screen.Camera.Yaw
            CreatedWorld = False
        End If

        MyBase.UpdateEntity()
    End Sub

    Public Overrides Function WalkIntoFunction() As Boolean
        Screen.Level.PokemonEncounter.TryEncounterWildPokemon(Me.Position, Spawner.EncounterMethods.Land, Me.AdditionalValue)

        Return False
    End Function

    Public Overrides Sub Render()
        If Me.Model Is Nothing Then
            Me.Draw(Me.BaseModel, Textures, False)
        Else
            UpdateModel()
            Draw(Me.BaseModel, Me.Textures, True, Me.Model)
        End If
    End Sub

    Public Shared Function GetGrassTilesAroundPlayer(ByVal radius As Single) As List(Of Entity)
        Dim l As New List(Of Entity)

        For Each e As Entity In Screen.Level.Entities
            If e.EntityID = "Grass" Then
                If e.Visible = True Then
                    If CInt(e.Position.Y) = CInt(Screen.Camera.Position.Y) Then
                        Dim distance As Single = Vector3.Distance(Screen.Camera.Position, e.Position)

                        If distance <= radius Then
                            l.Add(e)
                        End If
                    End If
                End If
            End If
        Next

        Return l
    End Function

End Class