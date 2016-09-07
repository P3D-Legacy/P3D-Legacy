Namespace XmlLevel

    Public Class FacePlayerPropertyListener

        Inherits XmlPropertyListener

        Public Sub New(ByVal XmlEntityReference As XmlEntity)
            MyBase.New(XmlEntityReference, "faceplayer")
        End Sub

        Public Overrides Sub UpdateEntity()
            If XmlEntity.Rotation.Y <> Screen.Camera.Yaw Then
                Dim v As Vector3 = XmlEntity.Rotation
                XmlEntity.Rotation = New Vector3(v.X, Screen.Camera.Yaw, v.Z)
                XmlEntity.CreatedWorld = False
            End If
        End Sub

    End Class

End Namespace