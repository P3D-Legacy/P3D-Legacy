Public Class MapPreviewCamera

    Inherits Camera

    Public oldX, oldY As Single

    Public Sub New()
        MyBase.New("MapPreview")

        Me.Position = MapPreviewScreen.MapViewModePosition

        Me.RotationSpeed = 0.002F
        Me.FOV = 65

        Me.Yaw = 0.0F
        If CInt(Me.Yaw) = CInt(MathHelper.TwoPi) Then
            Me.Yaw = 0
        End If
        Pitch = -0.4432F
        Me.Speed = 0.2F

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

        Me.ChangeSpeed()
        Me.TurnCamera()
        Me.MoveCamera()

        UpdateFrustum()
        UpdateMatrices()
        ResetCursor()
    End Sub

    Private Sub ChangeSpeed()
        If Controls.Up(False, False, True, False, False, False) = True Then
            Me.Speed += 0.01F
            Me.RotationSpeed += 0.0001F
        End If
        If Controls.Down(False, False, True, False, False, False) = True Then
            Me.Speed -= 0.01F
            Me.RotationSpeed -= 0.0001F
        End If

        Me.Speed = Me.Speed.Clamp(0.0F, 1.0F)
        Me.RotationSpeed = Me.RotationSpeed.Clamp(0.001F, 0.007F)
    End Sub

    Private Sub TurnCamera()
        Dim mState As MouseState = Mouse.GetState()
        Dim gState As GamePadState = GamePad.GetState(PlayerIndex.One)

        Dim dx As Single = mState.X - oldX
        If gState.ThumbSticks.Right.X <> 0.0F And Core.GameOptions.GamePadEnabled = True Then
            dx = gState.ThumbSticks.Right.X * 50.0F
        End If

        Dim dy As Single = mState.Y - oldY
        If gState.ThumbSticks.Right.Y <> 0.0F And Core.GameOptions.GamePadEnabled = True Then
            dy = gState.ThumbSticks.Right.Y * 40.0F * -1.0F
        End If

        Yaw += -RotationSpeed * 0.75F * dx

        While Yaw > MathHelper.TwoPi
            Yaw -= MathHelper.TwoPi
        End While
        While Yaw < 0
            Yaw += MathHelper.TwoPi
        End While

        Pitch += -RotationSpeed * dy

        Pitch = MathHelper.Clamp(Pitch, -1.5, 1.5)
    End Sub

    Private Sub MoveCamera()
        If Controls.Up(False, False, False, True, True, False) = True Then
            Dim rotationM As Matrix = Matrix.CreateFromYawPitchRoll(Yaw, Pitch, 0.0F)
            Dim translation As Vector3 = Vector3.Transform(Vector3.Forward, rotationM)
            Me.Position += translation * Me.Speed
        End If
        If Controls.Down(False, False, False, True, True, False) = True Then
            Dim rotationM As Matrix = Matrix.CreateFromYawPitchRoll(Yaw, Pitch, 0.0F)
            Dim translation As Vector3 = Vector3.Transform(Vector3.Backward, rotationM)
            Me.Position += translation * Me.Speed
        End If
        If Controls.Left(False, False, False, True, True, False) = True Then
            Dim rotationM As Matrix = Matrix.CreateFromYawPitchRoll(Yaw, Pitch, 0.0F)
            Dim translation As Vector3 = Vector3.Transform(Vector3.Left, rotationM)
            Me.Position += translation * Me.Speed
        End If
        If Controls.Right(False, False, False, True, True, False) = True Then
            Dim rotationM As Matrix = Matrix.CreateFromYawPitchRoll(Yaw, Pitch, 0.0F)
            Dim translation As Vector3 = Vector3.Transform(Vector3.Right, rotationM)
            Me.Position += translation * Me.Speed
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