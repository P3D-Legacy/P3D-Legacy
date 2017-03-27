Namespace GameModes.Items

    ''' <summary>
    ''' An item in the game.
    ''' </summary>
    Class Item

        Implements DataModel.IDataModelContainer

        Private _dataModel As DataModel.Json.Game.ItemModel
        Private _texture As Texture2D

        Public Sub New(ByVal dataModel As DataModel.Json.Game.ItemModel)
            _dataModel = dataModel
        End Sub

        ''' <summary>
        ''' The data model of this item.
        ''' </summary>
        Public ReadOnly Property DataModel() As DataModel.Json.JsonDataModel Implements DataModel.IDataModelContainer.DataModel
            Get
                Return _dataModel
            End Get
        End Property

        ''' <summary>
        ''' Extra data associated with this item instance.
        ''' </summary>
        Public Property Data() As String = ""

        ''' <summary>
        ''' This item's texture.
        ''' </summary>
        Public ReadOnly Property Texture() As Texture2D
            Get
                ' Load the texture if it has not been set before:
                If _texture Is Nothing Then
                    _texture = GameMode.Active.GetComponent(Of Resources.GameModeTextureManager).GetTexture(_dataModel.Texture)
                End If

                Return _texture
            End Get
        End Property

    End Class

End Namespace
