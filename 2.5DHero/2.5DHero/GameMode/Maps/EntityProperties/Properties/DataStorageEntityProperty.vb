Namespace GameModes.Maps.EntityProperties

    ''' <summary>
    ''' A property to just store data.
    ''' </summary>
    ''' <remarks>This class can be used by GameMode creators to store data in entities.</remarks>
    Class DataStorageEntityProperty

        Inherits EntityProperty

        Public Sub New(ByVal params As EntityPropertyDataCreationStruct)
            MyBase.New(params) : End Sub

        'Don't put any members into this class, because this property might be used
        'very often by a lot of entities.
        'So we want to reduce overhead as much as possible.

    End Class

End Namespace
