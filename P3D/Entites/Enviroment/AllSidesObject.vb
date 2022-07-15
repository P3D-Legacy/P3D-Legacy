Public Class AllSidesObject

    Inherits Entity

    Public Shared ReadOnly Dictionary As New Dictionary(Of String, Entity())

    Private _updateOnce As Boolean = False

    Public Overrides Sub Render()
        If Not _updateOnce Then
            _updateOnce = True

            If TypeOf BaseModel Is BlockModel OrElse TypeOf BaseModel Is CubeModel Then
                For Each entity As Entity In GetEntity(Position + Vector3.Left)
                    If TypeOf entity.BaseModel Is BlockModel OrElse TypeOf entity.BaseModel Is CubeModel Then
                        If entity.Scale = Scale Then
                            Select Case GetRotationFromVector(Rotation)
                                Case 0
                                    TextureIndex = CreateNewTextureIndex()
                                    TextureIndex(4) = -1
                                    TextureIndex(5) = -1
                                Case 1
                                    TextureIndex = CreateNewTextureIndex()
                                    TextureIndex(6) = -1
                                    TextureIndex(7) = -1
                                Case 2
                                    TextureIndex = CreateNewTextureIndex()
                                    TextureIndex(2) = -1
                                    TextureIndex(3) = -1
                                Case 3
                                    TextureIndex = CreateNewTextureIndex()
                                    TextureIndex(0) = -1
                                    TextureIndex(1) = -1
                            End Select
                        End If
                        Exit For
                    End If
                Next

                For Each entity As Entity In GetEntity(Position + Vector3.Right)
                    If TypeOf entity.BaseModel Is BlockModel OrElse TypeOf entity.BaseModel Is CubeModel Then
                        If entity.Scale = Scale Then
                            Select Case GetRotationFromVector(Rotation)
                                Case 0
                                    TextureIndex = CreateNewTextureIndex()
                                    TextureIndex(2) = -1
                                    TextureIndex(3) = -1
                                Case 1
                                    TextureIndex = CreateNewTextureIndex()
                                    TextureIndex(0) = -1
                                    TextureIndex(1) = -1
                                Case 2
                                    TextureIndex = CreateNewTextureIndex()
                                    TextureIndex(4) = -1
                                    TextureIndex(5) = -1
                                Case 3
                                    TextureIndex = CreateNewTextureIndex()
                                    TextureIndex(6) = -1
                                    TextureIndex(7) = -1
                            End Select
                        End If
                        Exit For
                    End If
                Next

                For Each entity As Entity In GetEntity(Position + Vector3.Forward)
                    If TypeOf entity.BaseModel Is BlockModel OrElse TypeOf entity.BaseModel Is CubeModel Then
                        If entity.Scale = Scale Then
                            Select Case GetRotationFromVector(Rotation)
                                Case 0
                                    TextureIndex = CreateNewTextureIndex()
                                    TextureIndex(6) = -1
                                    TextureIndex(7) = -1
                                Case 1
                                    TextureIndex = CreateNewTextureIndex()
                                    TextureIndex(2) = -1
                                    TextureIndex(3) = -1
                                Case 2
                                    TextureIndex = CreateNewTextureIndex()
                                    TextureIndex(0) = -1
                                    TextureIndex(1) = -1
                                Case 3
                                    TextureIndex = CreateNewTextureIndex()
                                    TextureIndex(4) = -1
                                    TextureIndex(5) = -1
                            End Select
                        End If
                        Exit For
                    End If
                Next

                For Each entity As Entity In GetEntity(Position + Vector3.Backward)
                    If TypeOf entity.BaseModel Is BlockModel OrElse TypeOf entity.BaseModel Is CubeModel Then
                        If entity.Scale = Scale Then
                            Select Case GetRotationFromVector(Rotation)
                                Case 0
                                    TextureIndex = CreateNewTextureIndex()
                                    TextureIndex(0) = -1
                                    TextureIndex(1) = -1
                                Case 1
                                    TextureIndex = CreateNewTextureIndex()
                                    TextureIndex(4) = -1
                                    TextureIndex(5) = -1
                                Case 2
                                    TextureIndex = CreateNewTextureIndex()
                                    TextureIndex(6) = -1
                                    TextureIndex(7) = -1
                                Case 3
                                    TextureIndex = CreateNewTextureIndex()
                                    TextureIndex(2) = -1
                                    TextureIndex(3) = -1
                            End Select
                        End If
                        Exit For
                    End If
                Next
            End If
        End If

        If Me.Model Is Nothing Then
            Me.Draw(Me.BaseModel, Textures, True)
        Else
            UpdateModel()
            Draw(Me.BaseModel, Me.Textures, True, Me.Model)
        End If
    End Sub

    Private Shadows Function GetEntity(targetPosition As Vector3) As Entity()
        Dim positionString As String = targetPosition.ToString()
        If Dictionary.ContainsKey(positionString) = False Then
            Dim temp = New List(Of Entity)

            SyncLock Screen.Level.EntityReadWriteSync
                For Each entity As Entity In Screen.Level.Entities
                    If entity.ID = -1 AndAlso TypeOf entity Is AllSidesObject AndAlso entity.Position = targetPosition Then
                        temp.Add(entity)
                    End If
                Next
            End SyncLock

            Dictionary.Add(positionString, temp.ToArray())
        End If

        Return Dictionary(positionString)
    End Function

    Private Function CreateNewTextureIndex() As Integer()
        Dim temp = New Integer(BaseModel.vertexBuffer.VertexCount / 3 - 1) {}

        For i As Integer = 0 To TextureIndex.Length - 1
            If i >= temp.Length Then
                Exit For
            End If

            temp(i) = TextureIndex(i)
        Next

        Return temp
    End Function

End Class