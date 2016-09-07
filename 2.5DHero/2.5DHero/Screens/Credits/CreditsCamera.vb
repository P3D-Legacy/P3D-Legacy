Public Class CreditsCamera

    Inherits Camera

    Dim oldX, oldY As Integer

    Public Target As Vector3

    Public ReadOnly Property IsReady As Boolean
        Get
            If New Vector3(CInt(Me.Position.X), CInt(Me.Position.Y), CInt(Me.Position.Z)) = New Vector3(CInt(Me.Target.X), CInt(Me.Target.Y), CInt(Me.Target.Z)) Then
                Return True
            End If
            Return False
        End Get
    End Property

    Public Sub New()
        MyBase.New("Credits")

        Me.Position = New Vector3(0, 2, 0)
        Me.RotationSpeed = 0.04F
        Me.FOV = 60.0F
        Me.Speed = 1.0F

        Me.Yaw = CSng(Math.PI / 4)
        Me.Pitch = -0.2F

        View = Matrix.CreateLookAt(Position, Vector3.Zero, Vector3.Up)
        Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(FOV), Core.GraphicsDevice.Viewport.AspectRatio, 0.01, Me.FarPlane)

        UpdateMatrices()
        UpdateFrustum()
        CreateRay()
    End Sub

    Public Overrides Sub Update()
        Ray = CreateRay()

        MoveCamera()

        UpdateMatrices()
        UpdateFrustum()
    End Sub

    Public Sub MoveCamera()
        If Me.IsReady = True Then
            Me.Position = Me.Target
        Else
            Dim MoveX As Single = 0.0F
            Dim MoveY As Single = 0.0F
            Dim MoveZ As Single = 0.0F

            If Me.Position.X < Me.Target.X Then
                MoveX = Me.Speed
                If Me.Position.X + MoveX > Me.Target.X Then
                    MoveX = Me.Target.X - Me.Position.X
                End If
            End If

            If Me.Position.X > Me.Target.X Then
                MoveX = -Me.Speed
                If Me.Position.X + MoveX < Me.Target.X Then
                    MoveX = Me.Position.X - Me.Target.X
                End If
            End If

            If Me.Position.Y < Me.Target.Y Then
                MoveY = Me.Speed
                If Me.Position.Y + MoveY > Me.Target.Y Then
                    MoveY = Me.Target.Y - Me.Position.Y
                End If
            End If

            If Me.Position.Y > Me.Target.Y Then
                MoveY = -Me.Speed
                If Me.Position.Y + MoveY < Me.Target.Y Then
                    MoveY = Me.Position.Y - Me.Target.Y
                End If
            End If

            If Me.Position.Z < Me.Target.Z Then
                MoveZ = Me.Speed
                If Me.Position.Z + MoveZ > Me.Target.Z Then
                    MoveZ = Me.Target.Z - Me.Position.Z
                End If
            End If

            If Me.Position.Z > Me.Target.Z Then
                MoveZ = -Me.Speed
                If Me.Position.Z + MoveZ < Me.Target.Z Then
                    MoveZ = Me.Position.Z - Me.Target.Z
                End If
            End If

            Me.Position = New Vector3(Me.Position.X + MoveX, Me.Position.Y + MoveY, Me.Position.Z + MoveZ)
        End If
    End Sub

#Region "CameraStuff"

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