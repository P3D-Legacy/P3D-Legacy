Public Class BaseModel

    Public Property ID As Integer = 0
    Public Property VertexBuffer As VertexBuffer

    Protected Sub Setup(vertexData As VertexPositionNormalTexture())
        VertexBuffer = New VertexBuffer(Core.GraphicsDevice, GetType(VertexPositionNormalTexture), vertexData.Length, BufferUsage.WriteOnly)
        VertexBuffer.SetData(vertexData)
    End Sub

    Public Sub Draw(entity As Entity, textures() As Texture2D)
        Dim effectDiffuseColor As Vector3 = Screen.Effect.DiffuseColor

        Screen.Effect.World = entity.World
        Screen.Effect.TextureEnabled = True
        Screen.Effect.Alpha = entity.Opacity

        Screen.Effect.DiffuseColor = effectDiffuseColor * entity.Shader * entity.Color

        If Screen.Level.IsDark = True Then
            Screen.Effect.DiffuseColor *= New Vector3(0.5, 0.5, 0.5)
        End If

        Core.GraphicsDevice.SetVertexBuffer(VertexBuffer)

        Dim triangleCount = CInt(VertexBuffer.VertexCount / 3)

        If triangleCount > entity.TextureIndex.Length Then
            Dim newTextureIndex = New Integer(triangleCount - 1) {}

            For i As Integer = entity.TextureIndex.Length - 1 To 0 Step -1
                newTextureIndex(i) = entity.TextureIndex(i)
            Next

            entity.TextureIndex = newTextureIndex
        End If

        Dim isEqual = True

        If entity.HasEqualTextures = -1 Then
            entity.HasEqualTextures = 1
            Dim contains As Integer = entity.TextureIndex(0)
            For index = 1 To entity.TextureIndex.Length - 1
                If contains <> entity.TextureIndex(index) Then
                    entity.HasEqualTextures = 0
                    Exit For
                End If
            Next
        End If

        If entity.HasEqualTextures = 0 Then
            isEqual = False
        End If

        If isEqual Then
            If entity.TextureIndex(0) > -1 Then
                ApplyTexture(textures(entity.TextureIndex(0)))
                Core.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, triangleCount)
                DebugDisplay.DrawnVertices += triangleCount
            End If
        Else
            For i As Integer = triangleCount - 1 To 0 Step -1
                If entity.TextureIndex(i) > -1 Then
                    ApplyTexture(textures(entity.TextureIndex(i)))
                    Core.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, i * 3, 1)
                    DebugDisplay.DrawnVertices += 1
                End If
            Next

        End If

        Screen.Effect.DiffuseColor = effectDiffuseColor

        If DebugDisplay.MaxDistance < entity.CameraDistance Then
            DebugDisplay.MaxDistance = CInt(entity.CameraDistance)
        End If
    End Sub

    Private Sub ApplyTexture(texture As Texture2D)
        Screen.Effect.Texture = texture
        Screen.Effect.CurrentTechnique.Passes(0).Apply()
    End Sub

    Public Shared FloorModel As FloorModel = New FloorModel()
    Public Shared BlockModel As BlockModel = New BlockModel()
    Public Shared SlideModel As SlideModel = New SlideModel()
    Public Shared BillModel As BillModel = New BillModel()
    Public Shared SignModel As SignModel = New SignModel()
    Public Shared CornerModel As CornerModel = New CornerModel()
    Public Shared InsideCornerModel As InsideCornerModel = New InsideCornerModel()
    Public Shared StepModel As StepModel = New StepModel()
    Public Shared InsideStepModel As InsideStepModel = New InsideStepModel()
    Public Shared CliffModel As CliffModel = New CliffModel()
    Public Shared CliffInsideModel As CliffInsideModel = New CliffInsideModel()
    Public Shared CliffCornerModel As CliffCornerModel = New CliffCornerModel()
    Public Shared CubeModel As CubeModel = New CubeModel()
    Public Shared CrossModel As CrossModel = New CrossModel()
    Public Shared DoubleFloorModel As DoubleFloorModel = New DoubleFloorModel()
    Public Shared PyramidModel As PyramidModel = New PyramidModel()
    Public Shared StairsModel As StairsModel = New StairsModel()
    Public Shared DiagonalWallModel As DiagonalWallModel = New DiagonalWallModel()
    Public Shared HalfDiagonalWallModel As HalfDiagonalWallModel = New HalfDiagonalWallModel()
    Public Shared OutsideStepModel As OutsideStepModel = New OutsideStepModel()
    Public Shared WallModel As WallModel = New WallModel()
    Public Shared CeilingModel As CeilingModel = New CeilingModel()

    Public Shared Function getModelbyID(ByVal ID As Integer) As BaseModel
        Select Case ID
            Case 0
                Return FloorModel
            Case 1
                Return BlockModel
            Case 2
                Return SlideModel
            Case 3
                Return BillModel
            Case 4
                Return SignModel
            Case 5
                Return CornerModel
            Case 6
                Return InsideCornerModel
            Case 7
                Return StepModel
            Case 8
                Return InsideStepModel
            Case 9
                Return CliffModel
            Case 10
                Return CliffInsideModel
            Case 11
                Return CliffCornerModel
            Case 12
                Return CubeModel
            Case 13
                Return CrossModel
            Case 14
                Return DoubleFloorModel
            Case 15
                Return PyramidModel
            Case 16
                Return StairsModel
            Case 17
                Return DiagonalWallModel
            Case 18
                Return HalfDiagonalWallModel
            Case 19
                Return OutsideStepModel
            Case 20
                Return WallModel
            Case 21
                Return CeilingModel
            Case Else
                Return BlockModel
        End Select
    End Function

End Class
