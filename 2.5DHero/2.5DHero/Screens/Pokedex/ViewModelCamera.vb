Public Class ViewModelCamera

    Inherits Camera

    Public oldX, oldY As Single

    Public Sub New()
        MyBase.New("ViewModel")

        Me.Position = New Vector3(0, 1.86F, 3.3F)

        Me.RotationSpeed = CSng(Core.Player.startRotationSpeed / 10000)
        Me.FOV = 65

        Me.Yaw = 0.0F
        If CInt(Me.Yaw) = CInt(MathHelper.TwoPi) Then
            Me.Yaw = 0
        End If
        Pitch = -0.4432F

        Turning = False

        View = Matrix.CreateLookAt(Position, Vector3.Zero, Vector3.Up)
        Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(FOV), Core.GraphicsDevice.Viewport.AspectRatio, 0.01, Me.FarPlane)

        UpdateMatrices()
        UpdateFrustum()
        createRay()
        ResetCursor()
    End Sub

    Public Overrides Sub Update()
        Me.Ray = createRay()

        Pitch = MathHelper.Clamp(Pitch, -1.5, 1.5)

        ScrollThirdPerson()

        UpdateFrustum()
        UpdateMatrices()
        ResetCursor()
    End Sub

    Private scrollSpeed As Single = 0.0F
    Private multi As Integer = 1

    Private Sub ScrollThirdPerson()
        If Controls.Down(True, False, True, False, False) = True Then
            If scrollSpeed = 0.0F Or multi <> 1 Then
                scrollSpeed = 0.01F
            End If

            multi = 1
            scrollSpeed += scrollSpeed.Clamp(0, 0.01)
        End If

        If Controls.Up(True, False, True, False, False) = True Then
            If scrollSpeed = 0.0F Or multi <> -1 Then
                scrollSpeed = 0.01F
            End If

            multi = -1
            scrollSpeed += scrollSpeed.Clamp(0, 0.01)
        End If

        scrollSpeed = scrollSpeed.Clamp(0, 0.08)

        If scrollSpeed > 0.0F Then
            Me.Position.Y += scrollSpeed * multi
            Me.Position.Z += scrollSpeed * multi

            Me.Position.Y = Me.Position.Y.Clamp(1.0F, 4.7F)
            Me.Position.Z = Me.Position.Z.Clamp(1.0F, 6.0F)

            scrollSpeed -= 0.001F
            If scrollSpeed <= 0.0F Then
                scrollSpeed = 0.0F
            End If
        End If
    End Sub

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

End Class