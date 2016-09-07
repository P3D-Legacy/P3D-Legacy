Public Class EntityGroup

    Inherits Entity

    Public Size As Size
    Public GroupPosition As Vector3

    Public Sub New(ByVal Entity As Entity, ByVal Rotation As Integer, ByVal Position As Vector3, ByVal Size As Size)
        'MyBase.New(Position.X, Position.Y, Position.Y, Entity.EntityID, Entity.Textures, Entity.TextureIndex, Entity.Collision, Rotation, Entity.Scale, Entity.Model, Entity.ActionValue, Entity.AdditionalValue, New Vector3(1.0F))

        Me.GroupPosition = Position
        Me.Size = Size
    End Sub

End Class