Public Class BaseModel

    Public Shared vertexData As New List(Of VertexPositionNormalTexture)

    Public vertexBuffer As VertexBuffer

    Public ID As Integer = 0

    Public Sub Setup()
        vertexBuffer = New VertexBuffer(Core.GraphicsDevice, GetType(VertexPositionNormalTexture), vertexData.Count, BufferUsage.WriteOnly)
        vertexBuffer.SetData(vertexData.ToArray())
    End Sub

    Public Sub Draw(ByVal Entity As Entity, ByVal Textures() As Texture2D)
        Dim effectDiffuseColor As Vector3 = Screen.Effect.DiffuseColor

        Screen.Effect.World = Entity.World
        Screen.Effect.TextureEnabled = True
        Screen.Effect.Alpha = Entity.Opacity

        Screen.Effect.DiffuseColor = effectDiffuseColor * Entity.Shader

        If Screen.Level.IsDark = True Then
            Screen.Effect.DiffuseColor *= New Vector3(0.5, 0.5, 0.5)
        End If

        Core.GraphicsDevice.SetVertexBuffer(vertexBuffer)

        If CInt(vertexBuffer.VertexCount / 3) > Entity.TextureIndex.Count Then
            Dim newTextureIndex(CInt(vertexBuffer.VertexCount / 3)) As Integer
            For i = 0 To CInt(vertexBuffer.VertexCount / 3)
                If Entity.TextureIndex.Count - 1 >= i Then
                    newTextureIndex(i) = Entity.TextureIndex(i)
                Else
                    newTextureIndex(i) = 0
                End If
            Next
            Entity.TextureIndex = newTextureIndex
        End If

        Dim isEqual As Boolean = True
        If Entity.HasEqualTextures = -1 Then
            Entity.HasEqualTextures = 1
            Dim contains As Integer = Entity.TextureIndex(0)
            For index = 1 To Entity.TextureIndex.Length - 1
                If contains <> Entity.TextureIndex(index) Then
                    Entity.HasEqualTextures = 0
                    Exit For
                End If
            Next
        End If
        If Entity.HasEqualTextures = 0 Then
            isEqual = False
        End If

        If isEqual = True Then
            If Entity.TextureIndex(0) > -1 Then
                Me.ApplyTexture(Textures(Entity.TextureIndex(0)))

                Core.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, CInt(vertexBuffer.VertexCount / 3))

                DebugDisplay.DrawnVertices += CInt(vertexBuffer.VertexCount / 3)
            End If
        Else
            For i = 0 To vertexBuffer.VertexCount - 1 Step 3
                If Entity.TextureIndex(CInt(i / 3)) > -1 Then
                    Me.ApplyTexture(Textures(Entity.TextureIndex(CInt(i / 3))))

                    Core.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, i, 1)
                    DebugDisplay.DrawnVertices += 1
                End If
            Next
        End If

        Screen.Effect.DiffuseColor = effectDiffuseColor
        If DebugDisplay.MaxDistance < Entity.CameraDistance Then DebugDisplay.MaxDistance = CInt(Entity.CameraDistance)
    End Sub

    Private Sub ApplyTexture(ByVal texture As Texture2D)
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
            Case Else
                Return BlockModel
        End Select
    End Function

End Class