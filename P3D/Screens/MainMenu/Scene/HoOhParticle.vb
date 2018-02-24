Imports GameDevCommon.Rendering
Imports GameDevCommon.Rendering.Composers
Imports GameDevCommon.Rendering.Texture

Namespace Screens.MainMenu.Scene

    Public Class HoOhParticle

        Inherits MainMenuEntity

        Private Shared _particleTexture As Texture2D
        Private Shared _particleTextures As Dictionary(Of Integer, Texture2D) = New Dictionary(Of Integer, Texture2D)()

        Private ReadOnly _texVariant As Integer

        Public Sub New(position As Vector3, texVariant As Integer)
            MyBase.New(position)
            _texVariant = texVariant
            IsOpaque = False
        End Sub

        Public Overrides Sub LoadContent()
            If _particleTexture Is Nothing Then
                _particleTexture = TextureManager.LoadDirect("GUI\MainMenu\hoohParticles.png")
                For i = 0 To 3
                    Dim colors = New Color(5 * 5 - 1) {}
                    _particleTexture.GetData(0, New Rectangle(i * 5, 0, 5, 5), colors, 0, colors.Length)
                    Dim tex = New Texture2D(GraphicsDevice, 5, 5)
                    tex.SetData(colors)
                    _particleTextures.Add(i, tex)
                Next
            End If

            Texture = _particleTextures(_texVariant)

            MyBase.LoadContent()
        End Sub

        Public Overrides Sub Update()
            Position.X += 1
            Alpha -= 0.02F

            If Alpha <= 0F Then
                ToBeRemoved = True
            End If

            CreateWorld()
            MyBase.Update()
        End Sub

        Protected Overrides Sub CreateGeometry()

            Dim vertices = RectangleComposer.Create(4, 4)
            VertexTransformer.Rotate(vertices, New Vector3(MathHelper.PiOver2, 0, 0))
            Geometry.AddVertices(vertices)

        End Sub

    End Class

End Namespace
