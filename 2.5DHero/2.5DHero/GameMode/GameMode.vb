Namespace GameModes

    ''' <summary>
    ''' The main class to control a GameMode.
    ''' </summary>
    Partial Class GameMode

        Implements DataModel.IDataModelContainer

        Private _dataModel As DataModel.Json.GameModeData.GameModeModel
        Private _gameModeFolder As String = "" 'The folder where the GameMode is stored.
        Private _components As List(Of IGameModeComponent)

        Private _initializedComponents As Boolean = False

        ''' <summary>
        ''' A shortcut to the active GameMode.
        ''' </summary>
        Public Shared ReadOnly Property Active() As GameMode
            Get
                Return GameCore.State.GameModeManager.ActiveGameMode
            End Get
        End Property

        ''' <summary>
        ''' Creates a new instance of a GameMode.
        ''' </summary>
        ''' <param name="gameModeFile">The path of the main GameMode configuration file (GameMode.dat).</param>
        Public Sub New(ByVal gameModeFile As String)

            Dim fileContent As String = IO.File.ReadAllText(gameModeFile)
            _dataModel = Pokemon3D.DataModel.Json.JsonDataModel.FromString(Of DataModel.Json.GameModeData.GameModeModel)(fileContent)

            _components = New List(Of IGameModeComponent)()
            _gameModeFolder = IO.Path.GetDirectoryName(gameModeFile)
        End Sub

        Private Sub InitializeComponents()
            _initializedComponents = True

            ' Adds all components needed for the GameMode here:

            AddComponent(New Items.ItemFactory())
            AddComponent(New Resources.GameModeTextureManager())
        End Sub

        Private Sub AddComponent(ByVal component As IGameModeComponent)
            _components.Add(component)
            component.Activated(Me)
        End Sub

        ''' <summary>
        ''' The data model of this GameMode.
        ''' </summary>
        Public ReadOnly Property DataModel As DataModel.Json.JsonDataModel Implements DataModel.IDataModelContainer.DataModel
            Get
                Return _dataModel
            End Get
        End Property

        ''' <summary>
        ''' Returns a GameMode Component added to this GameMode.
        ''' </summary>
        ''' <typeparam name="T">The GameMode Component type, has to implement the interface <see cref="IGameModeComponent"/>.</typeparam>
        Public Function GetComponent(Of T)() As T
            If Not _initializedComponents Then
                InitializeComponents()
            End If

            If GetType(T).GetInterfaces().Contains(GetType(IGameModeComponent)) Then
                Return CType(_components.Find(Function(c)
                                                  Return c.GetType() = GetType(T)
                                              End Function), T)
            Else
                Throw New ArgumentException("The input type has to be a GameMode Component type.")
            End If
        End Function

        ''' <summary>
        ''' Frees all resources consumed by this GameMode.
        ''' </summary>
        Public Sub Unload()
            For Each component In _components
                component.FreeResources()
            Next
        End Sub

    End Class

End Namespace
