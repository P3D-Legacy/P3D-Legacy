Public Class WallBlock

    Inherits Entity

        Protected Overrides Function CalculateCameraDistance(CPosition As Vector3) as Single
        Return MyBase.CalculateCameraDistance(CPosition) - 0.2F
    End Function

    Public Overrides Sub Render()
        If Me.Model Is Nothing Then
            Me.Draw(Me.BaseModel, Textures, False)
        Else
            Draw(Me.BaseModel, Me.Textures, True, Me.Model)
        End If
    End Sub

End Class