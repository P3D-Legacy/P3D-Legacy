Namespace GameModes

    ''' <summary>
    ''' A class to handle all loaded GameModes.
    ''' </summary>
    Class GameModeManager

        Private Const PATH_GAMEMODES As String = "GameModes"
        Private Const FILE_GAMEMODE_MAIN As String = "GameMode.dat"

        Private _gameModes As List(Of GameMode)
        Private _activeGameModeIndex As Integer = -1

        Public Sub New()
            _gameModes = New List(Of GameMode)()
        End Sub

        ''' <summary>
        ''' Searches the GameMode folder for compatible GameModes and loads them.
        ''' </summary>
        Public Sub LoadGameModes()
            ' Unload and clear all old GameModes first:
            For Each oldGameMode In _gameModes
                oldGameMode.Unload()
            Next
            _gameModes.Clear()

            Dim gameModeFolderPath As String = IO.Path.Combine({GameController.GamePath, PATH_GAMEMODES})
            Dim gameModeFilePath As String = ""

            ' Each GameMode is a seperate folder in the GameMode folder:
            For Each folder As String In IO.Directory.GetDirectories(gameModeFolderPath, "*.*", IO.SearchOption.TopDirectoryOnly)

                'Check if the GameMode.dat file exists inside the selected folder:

                gameModeFilePath = IO.Path.Combine({folder, FILE_GAMEMODE_MAIN})
                If IO.File.Exists(gameModeFilePath) Then
                    Dim newGameMode As GameMode = New GameMode(gameModeFilePath)

                    _gameModes.Add(newGameMode)
                End If

            Next
        End Sub

        ''' <summary>
        ''' Returns the currently active GameMode instance.
        ''' </summary>
        Public ReadOnly Property ActiveGameMode() As GameMode
            Get
                If _activeGameModeIndex = -1 Then
                    Return Nothing
                End If

                Return _gameModes(_activeGameModeIndex)
            End Get
        End Property

        ''' <summary>
        ''' The list of loaded GameModes.
        ''' </summary>
        Public ReadOnly Property GameModes() As GameMode()
            Get
                Return _gameModes.ToArray()
            End Get
        End Property

        ''' <summary>
        ''' Sets the active GameMode with an index based on the loaded GameMode list.
        ''' </summary>
        Public Sub SetActiveGameMode(ByVal index As Integer)
            _activeGameModeIndex = index
        End Sub

    End Class

End Namespace
