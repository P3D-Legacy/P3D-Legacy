Public Class WallBlock

    Inherits Entity

        Protected Overrides Function CalculateCameraDistance(CPosition As Vector3) as Single
        Return MyBase.CalculateCameraDistance(CPosition) - 0.2F
    End Function

    Public Overrides Sub Render()
        Me.Draw(Me.Model, Textures, False)
    End Sub

End Class