Imports GameDevCommon.Rendering
Imports GameDevCommon.Rendering.Composers
Imports GameDevCommon.Rendering.Texture

Namespace Screens.MainMenu.Scene

    Public Class HoOh

        Inherits MainMenuEntity

        Private Shared _random As Random = New Random()

        Private _entities As List(Of MainMenuEntity)
        Private _textures As Texture2D()
        Private _textureIndex As Integer = 0
        Private _animationDelay As Integer = 10

        Public Sub New(entities As List(Of MainMenuEntity))
            MyBase.New(New Vector3(0, -13, 1))
            Rotation.Y = -0.2F
            _entities = entities
        End Sub

        Public Overrides Sub LoadContent()
            Dim t = TextureManager.LoadDirect("GUI\MainMenu\hooh.png")
            Dim colors = New Color(80 * 48 - 1) {}
            Dim textures = New List(Of Texture2D)

            For x = 0 To 4

                t.GetData(0, New Rectangle(x * 80, 0, 80, 48), colors, 0, colors.Length)
                Dim tex = New Texture2D(GraphicsDevice, 80, 48)
                tex.SetData(colors)
                textures.Add(tex)

            Next

            _textures = textures.ToArray()

            Texture = _textures(_textureIndex)

            MyBase.LoadContent()
        End Sub

        Public Overrides Sub Update()
            _animationDelay -= 1
            If _animationDelay = 0 Then
                _animationDelay = 8
                _textureIndex += 1
                If _textureIndex = _textures.Length Then
                    _textureIndex = 0
                End If

                Texture = _textures(_textureIndex)

                For i = 0 To _random.Next(1, 4)

                    Dim particle = New HoOhParticle(Position +
                                                    New Vector3(6, GetParticleY() * 10, 0) +
                                                    New Vector3(_random.Next(-3, 4), _random.Next(-3, 4), 0), _random.Next(0, 4))
                    particle.LoadContent()
                    _entities.Add(particle)

                Next
            End If

            CreateWorld()
            MyBase.Update()
        End Sub

        Protected Overrides Sub CreateGeometry()

            Dim vertices = RectangleComposer.Create(37, 22)
            VertexTransformer.Rotate(vertices, New Vector3(MathHelper.PiOver2, 0, 0))
            Geometry.AddVertices(vertices)

        End Sub

        Private Function GetParticleY() As Integer
            Select Case _textureIndex
                Case 0
                    Return 1
                Case 1
                    Return 0
                Case 2
                    Return -1
                Case 3
                    Return 1
                Case 4
                    Return 1
            End Select
            Return 0
        End Function

    End Class

End Namespace
