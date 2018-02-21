Public Class SecretBaseCamera

    Inherits Camera

    Dim oldX, oldY As Integer

    Public Sub New()
        MyBase.New("SecretBase")

        Me.Position = New Vector3(3, 2, 3)
        Me.RotationSpeed = 0.04F
        Me.FOV = 45.0F
        Me.Speed = 1.0F

        Me.Yaw = 0.0F 'CSng(Math.PI / 4)
        Me.Pitch = -0.4F

        View = Matrix.CreateLookAt(Position, Vector3.Zero, Vector3.Up)
        Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(FOV), Core.GraphicsDevice.Viewport.AspectRatio, 0.01, Me.FarPlane)

        UpdateMatrices()
        UpdateFrustum()
        CreateRay()
        ResetCursor()
    End Sub

    Public Overrides Sub Update()
        Ray = CreateRay()

        MoveCamera()

        UpdateMatrices()
        UpdateFrustum()
        ResetCursor()
    End Sub

#Region "Movement"

    Public Sub MoveCamera()
        Dim MoveX As Single = 0.0F
        Dim MoveZ As Single = 0.0F

        If Controls.Left(True, True, False, True, True, True) = True Then
            MoveX -= Me.Speed
        End If
        If Controls.Right(True, True, False, True, True, True) = True Then
            MoveX += Me.Speed
        End If
        If Controls.Up(True, True, False, True, True, True) = True Then
            MoveZ -= Me.Speed
        End If
        If Controls.Down(True, True, False, True, True, True) = True Then
            MoveZ += Me.Speed
        End If

        If Position.X + MoveX < 0.0F Then
            MoveX = 0.0F
            Position.X = 0.0F
        End If
        If Position.Z + MoveZ < 0.0F Then
            MoveZ = 0.0F
            Position.Z = 0.0F
        End If

        Me.Position = New Vector3(Me.Position.X + MoveX, Me.Position.Y, Me.Position.Z + MoveZ)
    End Sub

#End Region

#Region "CameraStuff"

    Public Sub ResetCursor()
        If Core.GameInstance.IsActive = True Then
            Mouse.SetPosition(CInt(Core.windowSize.Width / 2), CInt(Core.windowSize.Height / 2))
            oldX = CInt(Core.windowSize.Width / 2)
            oldY = CInt(Core.windowSize.Height / 2)
        End If
    End Sub

    Public Sub UpdateFrustum()
        Dim rotation As Matrix = Matrix.CreateRotationX(Pitch) * Matrix.CreateRotationY(Yaw)

        Dim fPosition As New Vector3(Me.Position.X, Me.Position.Y, Me.Position.Z)

        Dim transformed As Vector3 = Vector3.Transform(New Vector3(0, 0, -1), rotation)
        Dim lookAt As Vector3 = fPosition + transformed

        Me.BoundingFrustum = New BoundingFrustum(Matrix.CreateLookAt(fPosition, lookAt, Vector3.Up) * Projection)
    End Sub

    Public Sub UpdateMatrices()
        Dim rotation As Matrix = Matrix.CreateRotationX(Pitch) * Matrix.CreateRotationY(Yaw)

        Dim transformed As Vector3 = Vector3.Transform(New Vector3(0, 0, -1), rotation)
        Dim lookAt As Vector3 = New Vector3(Me.Position.X, Me.Position.Y, Me.Position.Z) + transformed

        View = Matrix.CreateLookAt(Position, lookAt, Vector3.Up)
    End Sub

    Public Function CreateRay() As Ray
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

#End Region

End Class