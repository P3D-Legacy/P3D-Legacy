Namespace BattleSystem

    Public Class BattleCamera

        Inherits Camera

        Dim oldX, oldY As Integer

        Public TargetPosition As Vector3
        Public TargetMode As Boolean = True

        Public TargetYaw As Single = 0.0F
        Public TargetSpeed As Single = 0.04F
        Public TargetRotationSpeed As Single = 0.04F
        Public TargetPitch As Single = 0.0F

        Public ReadOnly Property IsReady As Boolean
            Get
                If Me.Position = Me.TargetPosition And Me.TargetSpeed = Me.Speed And Me.TargetYaw = Me.Yaw And Me.TargetPitch = Me.Pitch And Me.TargetRotationSpeed = Me.RotationSpeed Then
                    Return True
                End If
                Return False
            End Get
        End Property

        Public Sub New()
            MyBase.New("BattleV2")

            Me.Position = New Vector3(10, 10, 14)
            Me.RotationSpeed = 0.008F
            Me.FOV = 60.0F
            Me.Speed = 0.04F
            Me.TargetSpeed = 0.04F

            Me.Yaw = CSng(Math.PI / 4)
            Me.TargetYaw = CSng(Math.PI / 4)
            Me.Pitch = -0.6F
            Me.TargetPitch = -0.6F

            View = Matrix.CreateLookAt(Position, Vector3.Zero, Vector3.Up)
            Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(FOV), Core.GraphicsDevice.Viewport.AspectRatio, 0.01, Me.FarPlane)

            UpdateMatrices()
            UpdateFrustum()
            CreateRay()
        End Sub

        Public Overrides Sub Update()
            Ray = CreateRay()

            UpdateCamera()

            UpdateMatrices()
            UpdateFrustum()
        End Sub

        Public Sub UpdateCamera()
            If TargetMode = True Then
                If Me.Position <> Me.TargetPosition Then
                    Dim MoveX As Single = 0.0F
                    Dim MoveY As Single = 0.0F
                    Dim MoveZ As Single = 0.0F

                    If Me.Position.X < Me.TargetPosition.X Then
                        MoveX = Me.Speed
                        If Me.Position.X + MoveX > Me.TargetPosition.X Then
                            Me.Position.X = Me.TargetPosition.X
                            MoveX = 0.0F
                        End If
                    End If

                    If Me.Position.X > Me.TargetPosition.X Then
                        MoveX = -Me.Speed
                        If Me.Position.X + MoveX < Me.TargetPosition.X Then
                            Me.Position.X = Me.TargetPosition.X
                            MoveX = 0.0F
                        End If
                    End If

                    If Me.Position.Y < Me.TargetPosition.Y Then
                        MoveY = Me.Speed
                        If Me.Position.Y + MoveY > Me.TargetPosition.Y Then
                            Me.Position.Y = Me.TargetPosition.Y
                            MoveY = 0.0F
                        End If
                    End If

                    If Me.Position.Y > Me.TargetPosition.Y Then
                        MoveY = -Me.Speed
                        If Me.Position.Y + MoveY < Me.TargetPosition.Y Then
                            Me.Position.Y = Me.TargetPosition.Y
                            MoveY = 0.0F
                        End If
                    End If

                    If Me.Position.Z < Me.TargetPosition.Z Then
                        MoveZ = Me.Speed
                        If Me.Position.Z + MoveZ > Me.TargetPosition.Z Then
                            Me.Position.Z = Me.TargetPosition.Z
                            MoveZ = 0.0F
                        End If
                    End If

                    If Me.Position.Z > Me.TargetPosition.Z Then
                        MoveZ = -Me.Speed
                        If Me.Position.Z + MoveZ < Me.TargetPosition.Z Then
                            Me.Position.Z = Me.TargetPosition.Z
                            MoveZ = 0.0F
                        End If
                    End If

                    Me.Position = New Vector3(Me.Position.X + MoveX, Me.Position.Y + MoveY, Me.Position.Z + MoveZ)
                End If

                If Me.TargetYaw <> Me.Yaw Then
                    If Me.Yaw < Me.TargetYaw Then
                        Me.Yaw += Me.RotationSpeed
                        If Me.Yaw > Me.TargetYaw Then
                            Me.Yaw = Me.TargetYaw
                        End If
                    End If
                    If Me.Yaw > Me.TargetYaw Then
                        Me.Yaw -= Me.RotationSpeed
                        If Me.Yaw < Me.TargetYaw Then
                            Me.Yaw = Me.TargetYaw
                        End If
                    End If
                End If

                If Me.TargetPitch <> Me.Pitch Then
                    If Me.Pitch < Me.TargetPitch Then
                        Me.Pitch += Me.RotationSpeed
                        If Me.Pitch > Me.TargetPitch Then
                            Me.Pitch = Me.TargetPitch
                        End If
                    End If
                    If Me.Pitch > Me.TargetPitch Then
                        Me.Pitch -= Me.RotationSpeed
                        If Me.Pitch < Me.TargetPitch Then
                            Me.Pitch = Me.TargetPitch
                        End If
                    End If
                End If

                If Me.TargetSpeed <> Me.Speed Then
                    If Me.Speed < Me.TargetSpeed Then
                        Me.Speed += 0.005F
                        If Me.Speed > Me.TargetSpeed Then
                            Me.Speed = Me.TargetSpeed
                        End If
                    End If
                    If Me.Speed > Me.TargetSpeed Then
                        Me.Speed -= 0.005F
                        If Me.Speed < Me.TargetSpeed Then
                            Me.Speed = Me.TargetSpeed
                        End If
                    End If
                End If

                If Me.TargetRotationSpeed <> Me.RotationSpeed Then
                    If Me.RotationSpeed < Me.TargetRotationSpeed Then
                        Me.RotationSpeed += 0.005F
                        If Me.RotationSpeed > Me.TargetRotationSpeed Then
                            Me.RotationSpeed = Me.TargetRotationSpeed
                        End If
                    End If
                    If Me.RotationSpeed > Me.TargetRotationSpeed Then
                        Me.RotationSpeed -= 0.005F
                        If Me.RotationSpeed < Me.TargetRotationSpeed Then
                            Me.RotationSpeed = Me.TargetRotationSpeed
                        End If
                    End If
                End If
            End If
        End Sub

#Region "CameraStuff"

        Public Sub UpdateFrustum()
            Dim rotation As Matrix = Matrix.CreateRotationX(Pitch) * Matrix.CreateRotationY(Yaw)

            Dim fPosition As New Vector3(Me.Position.X, Me.Position.Y, Me.Position.Z)
            fPosition += GetBattleMapOffset()

            Dim transformed As Vector3 = Vector3.Transform(New Vector3(0, 0, -1), rotation)
            Dim lookAt As Vector3 = fPosition + transformed

            Me.BoundingFrustum = New BoundingFrustum(Matrix.CreateLookAt(fPosition, lookAt, Vector3.Up) * Projection)
        End Sub

        Public Sub UpdateMatrices()
            Dim rotation As Matrix = Matrix.CreateRotationX(Pitch) * Matrix.CreateRotationY(Yaw)

            Dim transformed As Vector3 = Vector3.Transform(New Vector3(0, 0, -1), rotation)
            Dim lookAt As Vector3 = New Vector3(Me.Position.X, Me.Position.Y, Me.Position.Z) + GetBattleMapOffset() + transformed

            View = Matrix.CreateLookAt(Position + GetBattleMapOffset(), lookAt, Vector3.Up)
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

        Private Function GetBattleMapOffset() As Vector3
            Dim v As New Vector3(0)
            Dim s As Screen = Core.CurrentScreen
            While s.Identification <> Screen.Identifications.BattleScreen And Not s.PreScreen Is Nothing
                s = s.PreScreen
            End While
            If s.Identification = Screen.Identifications.BattleScreen Then
                v = CType(s, BattleScreen).BattleMapOffset
            End If
            Return v
        End Function

        Public ReadOnly Property CPosition() As Vector3
            Get
                Return Me.Position + GetBattleMapOffset()
            End Get
        End Property

#End Region

    End Class

End Namespace