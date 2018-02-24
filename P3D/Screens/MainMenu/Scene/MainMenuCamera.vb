Namespace Screens.MainMenu.Scene

    Public Class MainMenuCamera

        Inherits GameDevCommon.Rendering.PerspectiveCamera

        Public Sub New()
            Pitch = 0
            FOV = 45
            Position = New Vector3(0, 0, 100)

            CreateView()
            CreateProjection()
        End Sub

        Public Overrides Sub Update()
            Yaw = -0.1F

            CreateView()
        End Sub

    End Class

End Namespace
