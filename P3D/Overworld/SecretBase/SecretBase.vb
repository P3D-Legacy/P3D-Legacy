Public Class SecretBase

    Public Enum BaseTypes
        Grass
        Desert
        Mountain
        Dirt
        Cave
    End Enum

    Public BaseType As BaseTypes = BaseTypes.Grass

    Public Sub New()

    End Sub

    Public Sub LoadSecretBaseFromStore(ByVal Level As Level)
        Dim ent As New List(Of Entity)
        Dim floors As New List(Of Entity)

        For x = -1 To 10
            For z = -1 To 10
                If x = -1 Or x = 10 Or z = -1 Or z = 10 Then
                    Dim newEntity As Entity = Entity.GetNewEntity("WallBlock", New Vector3(x, 0, z), {GetBaseTexture(1, BaseTypes.Grass), GetBaseTexture(2, BaseTypes.Grass)}, {0, 0, 0, 0, 0, 0, 0, 0, 1, 1}, True, Vector3.Zero, Vector3.One, BaseModel.BlockModel, 0, "", True, Vector3.One, -1, "", "", Vector3.Zero)
                    ent.Add(newEntity)
                End If

                Dim newFloor As Entity = New Floor(x, 0, z, {GetBaseTexture(0, BaseTypes.Grass)}, {0, 0}, False, 0, Vector3.One, BaseModel.FloorModel, 0, "", True, Vector3.One, False, False, False)
                floors.Add(newFloor)
                'Dim newCeiling As New AllSidesObject(x, 1, z, {GetBaseTexture(2, Me.BaseType)}, {0, 0}, False, 0, New Vector3(1.0F), BaseModel.FloorModel, 0, "", True, New Vector3(1.0F))
                'ent.Add(newCeiling)
            Next
        Next

        ent = (From e In ent Order By e.CameraDistance Descending).ToList()
        floors = (From f In floors Order By f.CameraDistance Descending).ToList()

        Level.Entities = ent
        Level.Floors = floors
    End Sub

    Public Sub GenerateSecretBase()

    End Sub

    Public Sub SaveBaseToStore(ByVal Level As Level)

    End Sub

    Public Shared Function GetBaseTexture(ByVal BaseObject As Integer, ByVal BaseType As BaseTypes) As Texture2D
        Dim x As Integer = BaseObject * 16
        Dim y As Integer = 0

        Select Case BaseType
            Case BaseTypes.Desert
                y = 16
            Case BaseTypes.Mountain
                y = 32
            Case BaseTypes.Dirt
                y = 48
            Case BaseTypes.Cave
                y = 64
        End Select

        Return TextureManager.GetTexture("SecretBase", New Rectangle(x, y, 16, 16), "Textures\")
    End Function

End Class