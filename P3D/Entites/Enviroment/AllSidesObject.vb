Public Class AllSidesObject

    Inherits Entity

    Public Overrides Sub Render()
        If Me.Model Is Nothing Then
            Me.Draw(Me.BaseModel, Textures, True)
        Else
            Draw(Me.BaseModel, Me.Textures, True, Me.Model)
        End If
    End Sub

End Class