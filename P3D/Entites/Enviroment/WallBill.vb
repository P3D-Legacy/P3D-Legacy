Public Class WallBill

    Inherits Entity

    Protected Overrides Function CalculateCameraDistance(CPosition As Vector3) as Single
        Return MyBase.CalculateCameraDistance(CPosition) - 0.4F
    End Function

    Public Overrides Sub UpdateEntity()
        If Me.Rotation.Y <> Screen.Camera.Yaw Then
            Me.Rotation.Y = Screen.Camera.Yaw
            CreatedWorld = False
        End If

        MyBase.UpdateEntity()
    End Sub

    Public Overrides Sub Render()
        If Me.Model Is Nothing Then
            If HasTransparentPixel() Then
                Dim state = Core.GraphicsDevice.DepthStencilState
                Core.GraphicsDevice.DepthStencilState = DepthStencilState.DepthRead
                Draw(BaseModel, Textures, False)
                Core.GraphicsDevice.DepthStencilState = state
            Else 
                Draw(BaseModel, Textures, False)
            End If
        Else
            UpdateModel()
            Draw(Me.BaseModel, Me.Textures, True, Me.Model)
        End If
    End Sub

End Class