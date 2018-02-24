Imports GameDevCommon.Rendering
Imports GameDevCommon.Rendering.Composers
Imports GameDevCommon.Rendering.Texture

Namespace Screens.MainMenu.Scene

    Public Class LugiaParticle

        Inherits MainMenuEntity

        Private Shared _particleTexture As Texture2D
        Dim _phase As Single

        Public Sub New(position As Vector3)
            MyBase.New(position)
            IsOpaque = False
        End Sub

        Public Overrides Sub LoadContent()
            If _particleTexture Is Nothing Then
                _particleTexture = TextureManager.LoadDirect("GUI\MainMenu\lugiaParticle.png")
            End If

            Texture = _particleTexture

            MyBase.LoadContent()
        End Sub

        Public Overrides Sub Update()
            _phase += 0.4F

            Position.X += 1.2F
            Position.Y += CType(Math.Sin(_phase), Single)
            Alpha -= 0.01F

            If Alpha <= 0F Then
                ToBeRemoved = True
            End If

            CreateWorld()
            MyBase.Update()
        End Sub

        Protected Overrides Sub CreateGeometry()

            Dim vertices = RectangleComposer.Create(8, 8)
            VertexTransformer.Rotate(vertices, New Vector3(MathHelper.PiOver2, 0, 0))
            Geometry.AddVertices(vertices)

        End Sub

    End Class

End Namespace
