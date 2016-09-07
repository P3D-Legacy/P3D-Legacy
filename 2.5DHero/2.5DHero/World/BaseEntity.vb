''' <summary>
''' The base for XmlEntity.vb and Entity.vb
''' </summary>
''' <remarks></remarks>
Public MustInherit Class BaseEntity

    Public Enum EntityTypes
        XmlEntity
        Entity
    End Enum

    Private _entityType As EntityTypes

    ''' <summary>
    ''' The type of entity.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property EntityType() As EntityTypes
        Get
            Return Me._entityType
        End Get
    End Property

    ''' <summary>
    ''' Creates a new instance of the BaseEntity class.
    ''' </summary>
    ''' <param name="EntityType">The type of entity the super of this class is.</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal EntityType As EntityTypes)
        Me._entityType = EntityType
    End Sub

    ''' <summary>
    ''' Converts the BaseEntity into an XmlEntity.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ToXmlEntity() As XmlLevel.XmlEntity
        If Me._entityType = EntityTypes.Entity Then
            Throw New Exceptions.InvalidEntityTypeException("Entity", "XmlEntity")
        End If

        Return CType(Me, XmlLevel.XmlEntity)
    End Function

    ''' <summary>
    ''' Converts the BaseEntity into an Entity.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ToEntity() As Entity
        If Me._entityType = EntityTypes.XmlEntity Then
            Throw New Exceptions.InvalidEntityTypeException("XmlEntity", "Entity")
        End If

        Return CType(Me, Entity)
    End Function

End Class