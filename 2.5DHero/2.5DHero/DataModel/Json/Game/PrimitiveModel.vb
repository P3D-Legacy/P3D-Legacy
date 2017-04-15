Namespace DataModel.Json.Game

    ''' <summary>
    ''' A data model for a primitive model.
    ''' </summary>
    Public Class PrimitiveModel

        Inherits JsonDataModel

        Public Vertices As VertexPositionNormalTextureModel()

    End Class

    ''' <summary>
    ''' A data model for a <see cref="VertexPositionNormalTexture"/>.
    ''' </summary>
    Public Class VertexPositionNormalTextureModel

        Inherits JsonDataModel

        Public Position As Vector3Model
        Public Normal As Vector3Model
        Public TextureCoordinate As Vector2Model

    End Class

End Namespace