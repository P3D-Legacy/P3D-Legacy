Namespace GameCore

    ''' <summary>
    ''' Contains accessors to paths of the game.
    ''' </summary>
    Class FileSystem

        Public Const PATH_GAMEMODES As String = "GameModes"
        Public Const PATH_RESOURCES As String = "SharedResources"
        Public Const PATH_LOCALIZATION As String = "Localization"

        Public Const PATH_TEXTURES As String = PATH_RESOURCES & "\Textures"

        ''' <summary>
        ''' The path to the game folder.
        ''' </summary>
        ''' <returns></returns>
        Public Shared ReadOnly Property GamePath() As String
            Get
                Return My.Application.Info.DirectoryPath
            End Get
        End Property

    End Class

End Namespace
