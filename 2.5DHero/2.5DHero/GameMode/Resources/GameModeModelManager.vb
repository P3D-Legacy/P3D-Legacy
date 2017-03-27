Namespace GameModes.Resources

    ''' <summary>
    ''' A class that loads and manages models for a GameMode.
    ''' </summary>
    Class GameModeModelManager

        Implements IGameModeComponent

        Private _contentManager As ContentManager
        Private _models As Dictionary(Of String, Model)

        Public Sub Activated(GameMode As GameMode) Implements IGameModeComponent.Activated
            _contentManager = New ContentManager(GameCore.State.GameController.Services, GameMode.ModelPath)
            _models = New Dictionary(Of String, Model)()
        End Sub

        Public Sub FreeResources() Implements IGameModeComponent.FreeResources
            _contentManager.Dispose()

            _models.Clear()
        End Sub

        ''' <summary>
        ''' Loads and returns a model resource.
        ''' </summary>
        Public Function GetModel(ByVal resource As String) As Model
            If Not _models.Keys.Contains(resource) Then
                Dim m As Model = _contentManager.Load(Of Model)(resource)
                _models.Add(resource, m)
            End If

            Return _models(resource)
        End Function

    End Class

End Namespace
