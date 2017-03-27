Public Class NewGameCamera

    Inherits Camera

    Public Sub New()
        MyBase.New("NewGame")
        Me.Position = New Vector3(13, 2, 14)
        Me.Speed = 0.0008F

        Yaw = CSng(Core.Random.NextDouble() * MathHelper.TwoPi)
        Pitch = -0.2F

        View = Matrix.CreateLookAt(Position, Vector3.Zero, Vector3.Up)
        Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0), Core.GraphicsDevice.Viewport.AspectRatio, 0.01, 16)

        Update()
    End Sub

    Public Overrides Sub Update()
        Ray = createRay()

        UpdateMatrices()
        UpdateFrustrum()
    End Sub

    Public Function createRay() As Ray
        Dim centerX As Integer = CInt(Core.windowSize.Width / 2)
        Dim centerY As Integer = CInt(Core.windowSize.Height / 2)

        Dim nearSource As Vector3 = New Vector3(centerX, centerY, 0)
        Dim farSource As Vector3 = New Vector3(centerX, centerY, 1)

        Dim nearPoint As Vector3 = Core.GraphicsDevice.Viewport.Unproject(nearSource, Projection, View, Matrix.Identity)
        Dim farPoint As Vector3 = Core.GraphicsDevice.Viewport.Unproject(farSource, Projection, View, Matrix.Identity)

        Dim direction As Vector3 = farPoint - nearPoint
        direction.Normalize()

        Return New Ray(nearPoint, direction)
    End Function

    Private Sub UpdateFrustrum()
        Dim rotation As Matrix = Matrix.CreateRotationX(Pitch) * Matrix.CreateRotationY(Yaw)

        Dim fPosition As New Vector3(Me.Position.X, Me.Position.Y, Me.Position.Z)

        Dim transformed As Vector3 = Vector3.Transform(New Vector3(0, 0, -1), rotation)
        Dim lookAt As Vector3 = fPosition + transformed

        Me.BoundingFrustum = New BoundingFrustum(Matrix.CreateLookAt(fPosition, lookAt, Vector3.Up) * Projection)
    End Sub

    Public Sub UpdateMatrices()
        Dim rotation As Matrix = Matrix.CreateRotationX(Pitch) * Matrix.CreateRotationY(Yaw)

        Dim transformed As Vector3 = Vector3.Transform(New Vector3(0, 0, -1), rotation)
        Dim lookAt As Vector3 = Position + transformed

        View = Matrix.CreateLookAt(Position, lookAt, Vector3.Up)
    End Sub

End Class