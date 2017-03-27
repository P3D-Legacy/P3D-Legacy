Namespace GameModes.Maps

    ''' <summary>
    ''' A class to hold initial data for a map.
    ''' </summary>
    Class MapPrototype

        Private _dataModel As DataModel.Json.Game.MapModel
        Private _filePath As String

        Public Sub New(ByVal filePath As String, ByVal dataModel As DataModel.Json.Game.MapModel)
            _dataModel = dataModel
            _filePath = filePath
        End Sub

        ''' <summary>
        ''' The data model of this map prototype.
        ''' </summary>
        Public ReadOnly Property DataModel As DataModel.Json.Game.MapModel
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
        ''' Creates a <see cref="Map"/> instance from this prototype.
        ''' </summary>
        Public Function CreateMap() As Map

        End Function

    End Class

End Namespace
