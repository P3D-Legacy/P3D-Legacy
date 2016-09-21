''' <summary>
''' The base for XmlEntity.vb and Entity.vb
''' </summary>
Public MustInherit Class BaseEntity

    Public Enum EntityTypes
        XmlEntity
        Entity
    End Enum

    Private _entityType As EntityTypes

    ''' <summary>
    ''' Creates a new instance of the BaseEntity class.
    ''' </summary>
    ''' <param name="EntityType">The type of entity the super of this class is.</param>
    Public Sub New(ByVal EntityType As EntityTypes)
        Me._entityType = EntityType
    End Sub

End Class
