Public Class WallBlock
    Inherits Entity
    
    Private _updateOnce As Boolean = False

    Protected Overrides Function CalculateCameraDistance(CPosition As Vector3) As Single
        Return MyBase.CalculateCameraDistance(CPosition) - 0.2F
    End Function

    Public Overrides Sub Render()
        If Not _updateOnce Then
            _updateOnce = True
            
            If ID = -1 Then
                If TypeOf BaseModel Is BlockModel OrElse TypeOf BaseModel Is CubeModel Then
                    For Each entity As Entity In GetEntityByPosition(Position + (Vector3.Left * Scale))
                        If TypeOf entity.BaseModel Is BlockModel OrElse TypeOf entity.BaseModel Is CubeModel Then
                            If entity.Scale = Scale Then
                                Select Case Rotation
                                    Case Vector3.Zero
                                        TextureIndex = CreateNewTextureIndex()
                                        TextureIndex(4) = -1
                                        TextureIndex(5) = -1
                                    Case New Vector3(0, MathHelper.PiOver2, 0)
                                        TextureIndex = CreateNewTextureIndex()
                                        TextureIndex(6) = -1
                                        TextureIndex(7) = -1
                                    Case New Vector3(0, MathHelper.Pi, 0)
                                        TextureIndex = CreateNewTextureIndex()
                                        TextureIndex(2) = -1
                                        TextureIndex(3) = -1
                                    Case New Vector3(0, MathHelper.Pi * 1.5, 0)
                                        TextureIndex = CreateNewTextureIndex()
                                        TextureIndex(0) = -1
                                        TextureIndex(1) = -1
                                End Select
                            End If
                            Exit For
                        End If
                    Next

                    For Each entity As Entity In GetEntityByPosition(Position + (Vector3.Right * Scale))
                        If TypeOf entity.BaseModel Is BlockModel OrElse TypeOf entity.BaseModel Is CubeModel Then
                            If entity.Scale = Scale Then
                                Select Case Rotation
                                    Case Vector3.Zero
                                        TextureIndex = CreateNewTextureIndex()
                                        TextureIndex(2) = -1
                                        TextureIndex(3) = -1
                                    Case New Vector3(0, MathHelper.PiOver2, 0)
                                        TextureIndex = CreateNewTextureIndex()
                                        TextureIndex(0) = -1
                                        TextureIndex(1) = -1
                                    Case New Vector3(0, MathHelper.Pi, 0)
                                        TextureIndex = CreateNewTextureIndex()
                                        TextureIndex(4) = -1
                                        TextureIndex(5) = -1
                                    Case New Vector3(0, MathHelper.Pi * 1.5, 0)
                                        TextureIndex = CreateNewTextureIndex()
                                        TextureIndex(6) = -1
                                        TextureIndex(7) = -1
                                End Select
                            End If
                            Exit For
                        End If
                    Next

                    For Each entity As Entity In GetEntityByPosition(Position + (Vector3.Forward * Scale))
                        If TypeOf entity.BaseModel Is BlockModel OrElse TypeOf entity.BaseModel Is CubeModel Then
                            If entity.Scale = Scale Then
                                Select Case Rotation
                                    Case Vector3.Zero
                                        TextureIndex = CreateNewTextureIndex()
                                        TextureIndex(6) = -1
                                        TextureIndex(7) = -1
                                    Case New Vector3(0, MathHelper.PiOver2, 0)
                                        TextureIndex = CreateNewTextureIndex()
                                        TextureIndex(2) = -1
                                        TextureIndex(3) = -1
                                    Case New Vector3(0, MathHelper.Pi, 0)
                                        TextureIndex = CreateNewTextureIndex()
                                        TextureIndex(0) = -1
                                        TextureIndex(1) = -1
                                    Case New Vector3(0, MathHelper.Pi * 1.5, 0)
                                        TextureIndex = CreateNewTextureIndex()
                                        TextureIndex(4) = -1
                                        TextureIndex(5) = -1
                                End Select
                            End If
                            Exit For
                        End If
                    Next

                    For Each entity As Entity In GetEntityByPosition(Position + (Vector3.Backward * Scale))
                        If TypeOf entity.BaseModel Is BlockModel OrElse TypeOf entity.BaseModel Is CubeModel Then
                            If entity.Scale = Scale Then
                                Select Case Rotation
                                    Case Vector3.Zero
                                        TextureIndex = CreateNewTextureIndex()
                                        TextureIndex(0) = -1
                                        TextureIndex(1) = -1
                                    Case New Vector3(0, MathHelper.PiOver2, 0)
                                        TextureIndex = CreateNewTextureIndex()
                                        TextureIndex(4) = -1
                                        TextureIndex(5) = -1
                                    Case New Vector3(0, MathHelper.Pi, 0)
                                        TextureIndex = CreateNewTextureIndex()
                                        TextureIndex(6) = -1
                                        TextureIndex(7) = -1
                                    Case New Vector3(0, MathHelper.Pi * 1.5, 0)
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
        End If

        If Model Is Nothing Then
            Draw(BaseModel, Textures, False)
        Else
            UpdateModel()
            Draw(BaseModel, Textures, True, Model)
        End If
    End Sub

End Class