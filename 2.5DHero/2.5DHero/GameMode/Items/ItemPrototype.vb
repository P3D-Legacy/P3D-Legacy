Namespace GameModes.Items

    ''' <summary>
    ''' A class to hold intial data of an item.
    ''' </summary>
    Class ItemPrototype

        Private _dataModel As DataModel.Json.Game.ItemModel
        Private _filePath As String

        Public Sub New(ByVal filePath As String, ByVal dataModel As DataModel.Json.Game.ItemModel)
            _dataModel = dataModel
            _filePath = filePath
        End Sub

        ''' <summary>
        ''' The data model of this item prototype.
        ''' </summary>
        Public ReadOnly Property DataModel As DataModel.Json.Game.ItemModel
            Get
                Return _dataModel
            End Get
        End Property

        ''' <summary>
        ''' The path where this prototype got its datamodel from.
        ''' </summary>
        Public ReadOnly Property FilePath As String
            Get
                Return _filePath
            End Get
        End Property

        ''' <summary>
        ''' Creates an <see cref="Item"/> instance from this prototype.
        ''' </summary>
        Public Function CreateItem() As Item
            Return New Item(_dataModel)
        End Function

    End Class

End Namespace
