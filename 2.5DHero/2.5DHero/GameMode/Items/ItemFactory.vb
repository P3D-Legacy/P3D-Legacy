Namespace GameModes.Items

    ''' <summary>
    ''' A factory to create item instances.
    ''' </summary>
    Class ItemFactory

        Implements IGameModeComponent

        Private _prototypes As Dictionary(Of Integer, ItemPrototype)

        ''' <summary>
        ''' Returns a new instance of an item by Id.
        ''' </summary>
        Public Function GetItem(ByVal id As Integer) As Item
            If id < 0 Then
                Throw New ArgumentException("An Item Id cannot be negative", NameOf(id))
            End If

            If Not _prototypes.Keys.Contains(id) Then
                Dim prototype = LoadPrototype(id)

                If Not prototype Is Nothing Then
                    _prototypes.Add(id, prototype)
                Else
                    Throw New ArgumentException("No item with the id " & id & " can be found in the GameMode.")
                End If
            End If

            Return _prototypes(id).CreateItem()
        End Function

        Private Function LoadPrototype(ByVal id As Integer) As ItemPrototype
            Dim itemPath As String = GameMode.Active.DataFilePath(id & ".dat", GameModeDataFile.ItemFile)

            If IO.File.Exists(itemPath) Then
                Dim fileContent As String = IO.File.ReadAllText(itemPath)
                Dim newDataModel = DataModel.Json.JsonDataModel.FromString(Of DataModel.Json.Game.ItemModel)(fileContent)

                Return New ItemPrototype(itemPath, newDataModel)
            Else
                Return Nothing
            End If
        End Function

        Public Sub FreeResources() Implements IGameModeComponent.FreeResources
            _prototypes.Clear()
        End Sub

        Public Sub Activated(ByVal GameMode As GameMode) Implements IGameModeComponent.Activated
            _prototypes = New Dictionary(Of Integer, ItemPrototype)()
        End Sub

    End Class

End Namespace
