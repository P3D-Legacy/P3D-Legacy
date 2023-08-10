Public Class BackdropRenderer
    Private ReadOnly _backdrops As New List(Of Backdrop)

    Public Sub AddBackdrop(backdrop As Backdrop)
        _backdrops.Add(backdrop)
    End Sub

    Public Sub Clear()
        _backdrops.Clear()
    End Sub

    Public Sub Update()
        For Each b In _backdrops
            b.Update()
        Next
    End Sub

    Public Sub Draw()
        For Each b In _backdrops
            b.Draw()
        Next
    End Sub

    Public Class Backdrop
        Implements IDisposable

        Public Enum BackdropTypes
            Water
            Grass
            Texture
        End Enum

        Private Shared ReadOnly VertexBufferData As VertexPositionNormalTexture() = { _
            New VertexPositionNormalTexture(New Vector3(-0.5, -0.5, 0.5), Vector3.Up, New Vector2(0, 1)),
            New VertexPositionNormalTexture(New Vector3(-0.5, -0.5, -0.5), Vector3.Up, New Vector2(0, 0)),
            New VertexPositionNormalTexture(New Vector3(0.5, -0.5, -0.5), Vector3.Up, New Vector2(1, 0)),
            New VertexPositionNormalTexture(New Vector3(0.5, -0.5, -0.5), Vector3.Up, New Vector2(1, 0)),
            New VertexPositionNormalTexture(New Vector3(0.5, -0.5, 0.5), Vector3.Up, New Vector2(1, 1)),
            New VertexPositionNormalTexture(New Vector3(-0.5, -0.5, 0.5), Vector3.Up, New Vector2(0, 1))
        }

        Private Shared ReadOnly IndexBufferData As Short() = New Short() {0, 1, 2, 3, 4, 5}

        Private Shared ReadOnly VertexBuffer As VertexBuffer
        Private Shared ReadOnly IndexBuffer As IndexBuffer

        Private ReadOnly _backdropType As BackdropTypes = BackdropTypes.Grass
        Private _backdropTexture As Texture2D = Nothing

        Private ReadOnly _waterAnimation As Animation = Nothing

        Private ReadOnly _world As Matrix
        Private ReadOnly _perInstanceVertexBuffer As VertexBuffer
        Private ReadOnly _vertexBufferBindings As VertexBufferBinding()

        Private _shader As Vector3 = Vector3.One

        Shared Sub New()
            VertexBuffer = New VertexBuffer(GraphicsDevice, GetType(VertexPositionNormalTexture), VertexBufferData.Length, BufferUsage.WriteOnly)
            VertexBuffer.SetData(VertexBufferData)

            IndexBuffer = New IndexBuffer(GraphicsDevice, GetType(Short), IndexBufferData.Length, BufferUsage.WriteOnly)
            IndexBuffer.SetData(IndexBufferData)
        End Sub

        Public Sub New(backdropType As String, position As Vector3, rotation As Vector3, scale As Vector3, size As Vector2, backdropTexture As Texture2D)
            Select Case backdropType.ToLower()
                Case "water"
                    _backdropType = BackdropTypes.Water

                    Dim waterSize = New Size(CInt(TextureManager.GetTexture("Textures\Backdrops\Water").Width / 3), CInt(TextureManager.GetTexture("Textures\Backdrops\Water").Height))
                    _waterAnimation = New Animation(TextureManager.GetTexture("Textures\Backdrops\Water"), 1, 3, waterSize.Width, waterSize.Height, Water.WaterSpeed, 0, 0)
                    _backdropTexture = TextureManager.GetTexture("Textures\Backdrops\Water", _waterAnimation.TextureRectangle, "")
                Case "grass"
                    _backdropType = BackdropTypes.Grass

                    Dim grassSize = New Size(CInt(TextureManager.GetTexture("Textures\Backdrops\Grass").Width / 4), CInt(TextureManager.GetTexture("Textures\Backdrops\Grass").Height))
                    Dim x = 0

                    Select Case World.CurrentSeason
                        Case World.Seasons.Winter
                            x = 0
                        Case World.Seasons.Spring
                            x = grassSize.Width
                        Case World.Seasons.Summer
                            x = grassSize.Width * 2
                        Case World.Seasons.Fall
                            x = grassSize.Width * 3
                    End Select

                    _backdropTexture = TextureManager.GetTexture("Backdrops\Grass", New Rectangle(x, 0, grassSize.Width, grassSize.Height))
                Case "texture"
                    _backdropType = BackdropTypes.Texture
                    _backdropTexture = backdropTexture
            End Select

            Dim perInstanceVertexBufferData As New List(Of VertexPerInstancePosition)

            For x = 0 To size.X - 1
                For y = 0 To size.Y - 1
                    perInstanceVertexBufferData.Add(New VertexPerInstancePosition(New Vector3(x * scale.X, 0 * scale.Y, y * scale.Z)))
                Next
            Next

            _world = Matrix.CreateScale(scale) * Matrix.CreateFromYawPitchRoll(rotation.Y, rotation.X, rotation.Z) * Matrix.CreateTranslation(position + New Vector3(0.5, 0.5, 0.5))

            _perInstanceVertexBuffer = New VertexBuffer(GraphicsDevice, GetType(VertexPerInstancePosition), perInstanceVertexBufferData.Count, BufferUsage.WriteOnly)
            _perInstanceVertexBuffer.SetData(perInstanceVertexBufferData.ToArray())

            _vertexBufferBindings = New VertexBufferBinding() {New VertexBufferBinding(VertexBuffer), New VertexBufferBinding(_perInstanceVertexBuffer, 0, 1)}
        End Sub

        Public Sub Update()
            If _backdropType = BackdropTypes.Water Then
                _waterAnimation.Update(0.005)

                If Core.GameOptions.GraphicStyle = 1 Then
                    _backdropTexture = TextureManager.GetTexture("Textures\Backdrops\Water", _waterAnimation.TextureRectangle, "")
                End If
            End If

            If Screen.Level.World IsNot Nothing Then
                Select Case Screen.Level.World.EnvironmentType
                    Case World.EnvironmentTypes.Outside
                        _shader = New Vector3(1.0F)
                    Case World.EnvironmentTypes.Dark
                        _shader = New Vector3(0.5F)
                    Case Else
                        _shader = New Vector3(1.0F)
                End Select
            End If

            If Screen.Level.LightingType = 6 Then
                _shader = New Vector3(0.5F)
            End If
        End Sub

        Public Sub Draw()
            Screen.Effect.World = _world
            Screen.Effect.View = Screen.Camera.View
            Screen.Effect.Projection = Screen.Camera.Projection
            Screen.Effect.Alpha = 1

            Dim effectDiffuseColor As Vector3 = Screen.Effect.DiffuseColor
            Screen.Effect.DiffuseColor = effectDiffuseColor * _shader

            If Screen.Level.IsDark OrElse Screen.Level.LightingType = 6 Then
                Screen.Effect.DiffuseColor *= New Vector3(0.5, 0.5, 0.5)
            End If

            Screen.Effect.TextureEnabled = True
            Screen.Effect.Texture = _backdropTexture
            Screen.Effect.EnableHardwareInstancing = True
            Screen.Effect.CurrentTechnique.Passes(0).Apply()

            GraphicsDevice.SetVertexBuffers(_vertexBufferBindings)
            GraphicsDevice.Indices = IndexBuffer
            GraphicsDevice.DrawInstancedPrimitives(PrimitiveType.TriangleList, 0, 0, VertexBuffer.VertexCount, _perInstanceVertexBuffer.VertexCount)

            ' Post Draw
            Screen.Effect.DiffuseColor = effectDiffuseColor
            Screen.Effect.EnableHardwareInstancing = False
        End Sub

        Public Sub Dispose() Implements IDisposable.Dispose
            _perInstanceVertexBuffer.Dispose()
        End Sub
    End Class
End Class