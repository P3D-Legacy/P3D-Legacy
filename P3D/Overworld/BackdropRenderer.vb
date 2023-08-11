Imports System.Runtime.InteropServices

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
        Private ReadOnly _vertexBufferDataPerInstance As VertexPerInstancePosition()
        Private ReadOnly _vertexBufferPerInstances As VertexBuffer()

        Private _shader As Vector3 = Vector3.One

        Shared Sub New()
            VertexBuffer = New VertexBuffer(Core.GraphicsDevice, VertexPositionNormalTexture.VertexDeclaration, VertexBufferData.Length, BufferUsage.WriteOnly)
            VertexBuffer.SetData(VertexBufferData)

            IndexBuffer = New IndexBuffer(Core.GraphicsDevice, GetType(Short), IndexBufferData.Length, BufferUsage.WriteOnly)
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

            Dim vertexBufferDataPerInstance As New List(Of VertexPerInstancePosition)

            For x = 0 To size.X - 1
                For y = 0 To size.Y - 1
                    vertexBufferDataPerInstance.Add(New VertexPerInstancePosition(New Vector3(x * scale.X, 0 * scale.Y, y * scale.Z)))
                Next
            Next

            _world = Matrix.CreateScale(scale) * Matrix.CreateFromYawPitchRoll(rotation.Y, rotation.X, rotation.Z) * Matrix.CreateTranslation(position + New Vector3(0.5, 0.5, 0.5))
            _vertexBufferDataPerInstance = vertexBufferDataPerInstance.ToArray()
            
            Dim vertexBufferPerInstances As New List(Of VertexBuffer)
            Dim instanceMaxSize = (16 * 1024) \ Marshal.SizeOf(GetType(VertexPerInstancePosition))
            
            For i As Integer = 0 To _vertexBufferDataPerInstance.Length - 1 Step instanceMaxSize
                Dim instanceSize = Math.Min(instanceMaxSize, _vertexBufferDataPerInstance.Length - i)
                Dim tempVertexBuffer = New VertexBuffer(Core.GraphicsDevice, VertexPerInstancePosition.VertexDeclaration, instanceSize, BufferUsage.WriteOnly)
                tempVertexBuffer.SetData(_vertexBufferDataPerInstance, i, instanceSize)
                vertexBufferPerInstances.Add(tempVertexBuffer)
            Next
            
            _vertexBufferPerInstances = vertexBufferPerInstances.ToArray()
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
            
            For Each vertexBufferPerInstance In _vertexBufferPerInstances
                Core.GraphicsDevice.SetVertexBuffers(New VertexBufferBinding(VertexBuffer), New VertexBufferBinding(vertexBufferPerInstance, 0, 1))
                Core.GraphicsDevice.Indices = IndexBuffer
                Core.GraphicsDevice.DrawInstancedPrimitives(PrimitiveType.TriangleList, 0, 0, VertexBuffer.VertexCount, vertexBufferPerInstance.VertexCount)
            Next
            
            ' Post Draw
            Screen.Effect.DiffuseColor = effectDiffuseColor
            Screen.Effect.EnableHardwareInstancing = False
        End Sub

        Public Sub Dispose() Implements IDisposable.Dispose
            For Each buffer In _vertexBufferPerInstances
                buffer.Dispose()
            Next
        End Sub
    End Class
End Class