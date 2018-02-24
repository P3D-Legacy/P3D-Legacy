Imports GameDevCommon.Rendering
Imports GameDevCommon.Rendering.Composers
Imports GameDevCommon.Rendering.Texture

Namespace Screens.MainMenu.Scene

    Public Class Ground

        Inherits MainMenuEntity

        Public Sub New()
            MyBase.New(New Vector3(0, -13, 0))
        End Sub

        Public Overrides Sub LoadContent()
            Texture = TextureManager.LoadDirect("GUI\MainMenu\ground.png")

            MyBase.LoadContent()
        End Sub

        Public Overrides Sub Update()
            Position.X += 0.2F
            If Position.X >= 32.0F Then
                Position.X = 0F
            End If

            Position.Y = -50

            CreateWorld()
            MyBase.Update()
        End Sub

        Protected Overrides Sub CreateGeometry()

            Dim vertices = RectangleComposer.Create(320, 96, New TextureRectangle(0, 0, 10, 1))
            VertexTransformer.Rotate(vertices, New Vector3(MathHelper.PiOver2, 0, 0))
            Geometry.AddVertices(vertices)

        End Sub

    End Class

End Namespace
