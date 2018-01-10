Public Class BoundingBoxRenderer

#Region "Fields"

    Shared verts(8) As VertexPositionColor
    Shared indices As Int16() = New Int16() {0, 1, 1, 2, 2, 3, 3, 0, 0, 4, 1, 5, 2, 6, 3, 7, 4, 5, 5, 6, 6, 7, 7, 4}
    Shared effect As BasicEffect

#End Region

    Public Shared Sub Render(ByVal box As BoundingBox, ByVal graphicsDevice As GraphicsDevice, ByVal view As Matrix, ByVal projection As Matrix, ByVal color As Color)
        If effect Is Nothing Then
            effect = New BasicEffect(graphicsDevice)
            effect.VertexColorEnabled = True
            effect.LightingEnabled = False
        End If

        Dim corners() As Vector3 = box.GetCorners()
        For i = 0 To 7
            verts(i).Position = corners(i)
            verts(i).Color = color
        Next

        effect.View = view
        effect.Projection = projection

        For Each pass As EffectPass In effect.CurrentTechnique.Passes
            pass.Apply()

            graphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.LineList, verts, 0, 8, indices, 0, CInt(indices.Length / 2))
        Next
    End Sub

    Public Shared Sub Render(ByVal frustum As BoundingFrustum, ByVal graphicsDevice As GraphicsDevice, ByVal view As Matrix, ByVal projection As Matrix, ByVal color As Color)
        If effect Is Nothing Then
            effect = New BasicEffect(graphicsDevice)
            effect.VertexColorEnabled = True
            effect.LightingEnabled = False
        End If

        Dim corners() As Vector3 = frustum.GetCorners()
        For I = 0 To 7
            verts(I).Position = corners(I)
            verts(I).Color = color
        Next

        effect.View = view
        effect.Projection = projection

        For Each pass As EffectPass In effect.CurrentTechnique.Passes
            pass.Apply()

            graphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.LineList, verts, 0, 8, indices, 0, CInt(indices.Length / 2))
        Next

    End Sub

End Class