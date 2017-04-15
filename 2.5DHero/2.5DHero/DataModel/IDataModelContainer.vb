Namespace DataModel

    ''' <summary>
    ''' An interface for objects that implement a datamodel.
    ''' </summary>
    Public Interface IDataModelContainer

        ''' <summary>
        ''' Gets and/or sets the data model.
        ''' </summary>
        ReadOnly Property DataModel As Json.JsonDataModel

    End Interface

End Namespace
