Imports System.IO

Namespace GameModes

    ''' <summary>
    ''' Available GameMode data files.
    ''' </summary>
    Public Enum GameModeDataFile
        PokeFile
        ItemFile
        MoveFile
        RegionFile
        PokemonFile
        MiscFile
    End Enum

    ''' <summary>
    ''' Available GameMode Content files.
    ''' </summary>
    Public Enum GameModeContentFile
        TextureFile
        ModelFile
        SoundFile
        SongFile
        FontFile
        EffectFile
    End Enum

    ' Contains main definitions to reduce clutter in the main file.
    ' Also has handy file & path methods.
    Partial Class GameMode

#Region "File Management"

        ' Main path consts:
        Private Const PATH_DATA As String = "Data\" 'All data files, except maps and scripts.
        Private Const PATH_MAPS As String = "Maps\" 'All map files
        Private Const PATH_SCRIPTS As String = "Scripts\" 'All script files.
        Private Const PATH_CONTENT As String = "Content\" 'All asset files (textures, sounds, music, models, effects, fonts)

        ' Important sub path consts:
        ' Content:
        Private Const PATH_CONTENT_TEXTURES As String = "Textures\"
        Private Const PATH_CONTENT_MODELS As String = "Models\"
        Private Const PATH_CONTENT_SOUNDS As String = "Sounds\"
        Private Const PATH_CONTENT_SONGS As String = "Songs\"
        Private Const PATH_CONTENT_FONTS As String = "Fonts\"
        Private Const PATH_CONTENT_EFFECTS As String = "Effects\"
        ' Data:
        Private Const PATH_DATA_POKE As String = "Poke\" 'Path to .poke files in the data folder.
        Private Const PATH_DATA_ITEMS As String = "Items\" 'Path to all item data files.
        Private Const PATH_DATA_MOVES As String = "Moves\" 'Path to move files.
        Private Const PATH_DATA_REGION As String = "Region\" 'Path to the worldmap and region files.
        Private Const PATH_DATA_POKEMON As String = "Pokemon\" 'Path to Pokémon data files.
        Private Const PATH_DATA_MISC As String = "Misc\" 'Path to misc data files.

        ''' <summary>
        ''' The path to the Texture base folder of this GameMode.
        ''' </summary>
        Public ReadOnly Property TexturePath() As String
            Get
                Return Path.Combine({_gameModeFolder, PATH_CONTENT, PATH_CONTENT_TEXTURES})
            End Get
        End Property

        ''' <summary>
        ''' The path to the Model base folder of this GameMode.
        ''' </summary>
        Public ReadOnly Property ModelPath() As String
            Get
                Return Path.Combine({_gameModeFolder, PATH_CONTENT, PATH_CONTENT_MODELS})
            End Get
        End Property

        Public ReadOnly Property MapFilePath(ByVal mapFile As String) As String
            Get
                If mapFile.StartsWith("\") Then
                    mapFile = mapFile.Remove(0, 1)
                End If

                Return Path.Combine({_gameModeFolder, PATH_MAPS, mapFile})
            End Get
        End Property

        Public ReadOnly Property ScriptFilePath(ByVal scriptFile As String) As String
            Get
                If scriptFile.StartsWith("\") Then
                    scriptFile = scriptFile.Remove(0, 1)
                End If

                Return Path.Combine({_gameModeFolder, PATH_SCRIPTS, scriptFile})
            End Get
        End Property

        ''' <summary>
        ''' Creates the path to a specific content file in the GameMode.
        ''' </summary>
        ''' <param name="contentFile">The content file name (without path).</param>
        ''' <param name="contentFileType">The type of content file.</param>
        Public ReadOnly Property ContentFilePath(ByVal contentFile As String, ByVal contentFileType As GameModeContentFile) As String
            Get
                Dim midPath As String = ""
                Select Case contentFileType
                    Case GameModeContentFile.TextureFile
                        midPath = PATH_CONTENT_TEXTURES
                    Case GameModeContentFile.ModelFile
                        midPath = PATH_CONTENT_MODELS
                    Case GameModeContentFile.SoundFile
                        midPath = PATH_CONTENT_SOUNDS
                    Case GameModeContentFile.SongFile
                        midPath = PATH_CONTENT_SONGS
                    Case GameModeContentFile.FontFile
                        midPath = PATH_CONTENT_FONTS
                    Case GameModeContentFile.EffectFile
                        midPath = PATH_CONTENT_EFFECTS
                End Select

                Return Path.Combine({_gameModeFolder, PATH_CONTENT, midPath, contentFile})
            End Get
        End Property

        ''' <summary>
        ''' Creates the path to a specific data file in the GameMode.
        ''' </summary>
        ''' <param name="dataFile">The data file name (without path).</param>
        ''' <param name="dataFileType">The type of data file.</param>
        Public ReadOnly Property DataFilePath(ByVal dataFile As String, ByVal dataFileType As GameModeDataFile) As String
            Get
                Dim midPath As String = ""
                Select Case dataFileType
                    Case GameModeDataFile.PokeFile
                        midPath = PATH_DATA_POKE
                    Case GameModeDataFile.ItemFile
                        midPath = PATH_DATA_ITEMS
                    Case GameModeDataFile.MoveFile
                        midPath = PATH_DATA_MOVES
                    Case GameModeDataFile.RegionFile
                        midPath = PATH_DATA_REGION
                    Case GameModeDataFile.PokemonFile
                        midPath = PATH_DATA_POKEMON
                    Case GameModeDataFile.MiscFile
                        midPath = PATH_DATA_MISC
                End Select

                Return Path.Combine({_gameModeFolder, PATH_DATA, midPath, dataFile})
            End Get
        End Property

#End Region

#Region "Component access"

        ''' <summary>
        ''' Returns the Texture Manager component.
        ''' </summary>
        Public Function GetTextureManager() As Resources.GameModeTextureManager
            Return GetComponent(Of Resources.GameModeTextureManager)()
        End Function

        ''' <summary>
        ''' Returns the Model Manager component.
        ''' </summary>
        Public Function GetModelManager() As Resources.GameModeModelManager
            Return GetComponent(Of Resources.GameModeModelManager)()
        End Function

#End Region

    End Class

End Namespace
