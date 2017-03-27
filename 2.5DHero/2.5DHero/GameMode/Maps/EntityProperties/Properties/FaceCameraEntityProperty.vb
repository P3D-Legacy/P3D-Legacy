Namespace GameModes.Maps.EntityProperties

    ''' <summary>
    ''' This entity property makes the entity always have the same yaw rotation as the camera.
    ''' </summary>
    Class FaceCameraEntityProperty

        Inherits EntityProperty

        Public Sub New(ByVal params As EntityPropertyDataCreationStruct)
            MyBase.New(params)
        End Sub

        Public Overrides Sub Update()
            MyBase.Update()

            If Parent.Rotation.Y <> Screen.Camera.Yaw Then
                Parent.Rotation = New Vector3(Parent.Rotation.X, Screen.Camera.Yaw, Parent.Rotation.Z)
            End If
        End Sub

    End Class

End Namespace
