Public Class CoreRenderBatch

    Private ReadOnly _vertexData As List(Of VertexPositionNormalTexture) = New List(Of VertexPositionNormalTexture)()
    Private _alpha As Single
    Private _diffuseColor As Vector3
    Private _texture As Texture2D

    Private _canDraw As Boolean = False
    Private _vertexBuffer As DynamicVertexBuffer

    Public Sub BeginBatch()

    End Sub

    Public Sub Draw(entity As Entity)

    End Sub

    Public Sub EndBatch()

    End Sub

    Private Sub Draw()
        If Not _canDraw Then
            Exit Sub
        End If

        Screen.Effect.World = Matrix.Identity
        Screen.Effect.Alpha = _alpha
        Screen.Effect.DiffuseColor = _diffuseColor
        Screen.Effect.TextureEnabled = True
        Screen.Effect.Texture = _texture
        Screen.Effect.CurrentTechnique.Passes(0).Apply()

        _vertexBuffer.SetData(_vertexData.ToArray())
    End Sub

End Class
