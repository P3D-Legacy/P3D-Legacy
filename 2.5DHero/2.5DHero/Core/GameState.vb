Namespace GameCore

    ''' <summary>
    ''' A class to hold static references to the current game state.
    ''' </summary>
    Class State

        ' Private constructor, this class must not get instantiated.
        Private Sub New() : End Sub

        Private Shared _gameController As GameController
        Private Shared _gameModeManager As GameModes.GameModeManager
        Private Shared _currentScreen As Screen

        ''' <summary>
        ''' The game controller (main <see cref="Game"/> type).
        ''' </summary>
        Public Shared ReadOnly Property GameController() As GameController
            Get
                Return _gameController
            End Get
        End Property

        ''' <summary>
        ''' Reference to the active GameModeManager.
        ''' </summary>
        Public Shared ReadOnly Property GameModeManager() As GameModes.GameModeManager
            Get
                Return _gameModeManager
            End Get
        End Property

        ''' <summary>
        ''' Returns the currently active screen instance.
        ''' </summary>
        Public Shared ReadOnly Property CurrentScreen() As Screen
            Get
                Return _currentScreen
            End Get
        End Property

        ''' <summary>
        ''' Initializes the game state.
        ''' Called once at the start of the game.
        ''' </summary>
        Public Shared Sub Initialize(ByVal gameController As GameController)
            _gameController = gameController

            _gameModeManager = New GameModes.GameModeManager()
        End Sub

    End Class

End Namespace
