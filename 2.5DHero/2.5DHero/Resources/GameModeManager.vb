''' <summary>
''' Manages the active and loaded GameModes.
''' </summary>
Public Class GameModeManager

    Private Shared _gameModeList As New List(Of GameMode)
    Private Shared _gameModePointer As Integer = 0

    ''' <summary>
    ''' Loads (or reloads) the list of GameModes. The pointer also gets reset.
    ''' </summary>
    Public Shared Sub LoadGameModes()
        _gameModeList.Clear()
        _gameModePointer = 0

        For Each GameModeFolder As String In IO.Directory.GetDirectories(GameController.GamePath & "\GameModes\")
            If IO.File.Exists(GameModeFolder & "\GameMode.dat") = True Then
                AddGameMode(GameModeFolder)
            End If
        Next

        SetGameModePointer("Kolben")
    End Sub

    ''' <summary>
    ''' Returns a GameMode based on its directory path.
    ''' </summary>
    ''' <param name="GameModeDirectory">The directory path of the GameMode.</param>
    ''' <returns></returns>
    Public Shared Function GetGameMode(ByVal GameModeDirectory As String) As GameMode
        For Each GameMode As GameMode In _gameModeList
            If GameMode.DirectoryName = GameModeDirectory Then
                Return GameMode
            End If
        Next

        Return Nothing
    End Function

    ''' <summary>
    ''' Sets the GameModePointer to a new item.
    ''' </summary>
    ''' <param name="GameModeDirectoryName">The directory resembeling the new GameMode.</param>
    Public Shared Sub SetGameModePointer(ByVal GameModeDirectoryName As String)
        For i = 0 To _gameModeList.Count - 1
            Dim GameMode As GameMode = _gameModeList(i)
            If GameMode.DirectoryName = GameModeDirectoryName Then
                _gameModePointer = i
                Logger.Debug("153", "---Set pointer to """ & GameModeDirectoryName & """!---")
                Exit Sub
            End If
        Next
        Logger.Debug("154", "Couldn't find the GameMode """ & GameModeDirectoryName & """!")
    End Sub

    ''' <summary>
    ''' Returns the amount of loaded GameModes.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    Public Shared ReadOnly Property GameModeCount() As Integer
        Get
            Return _gameModeList.Count
        End Get
    End Property

    Public Shared ReadOnly Property GetAllGameModes() As GameMode()
        Get
            Return _gameModeList.ToArray()
        End Get
    End Property

    ''' <summary>
    ''' Checks if a GameMode exists.
    ''' </summary>
    ''' <param name="GameModePath"></param>
    ''' <returns></returns>
    Public Shared Function GameModeExists(ByVal GameModePath As String) As Boolean
        For Each GameMode As GameMode In _gameModeList
            If GameMode.DirectoryName = GameModePath Then
                Return True
            End If
        Next

        Return False
    End Function

    ''' <summary>
    ''' Adds a GameMode to the list.
    ''' </summary>
    ''' <param name="Path">The path of the GameMode directory.</param>
    Private Shared Sub AddGameMode(ByVal Path As String)
        Dim newGameMode As New GameMode(Path & "\GameMode.dat")
        If newGameMode.IsValid = True Then
            _gameModeList.Add(newGameMode)
        End If
    End Sub

#Region "Shared GameModeFunctions"

    ''' <summary>
    ''' Returns the currently active GameMode.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    Public Shared ReadOnly Property ActiveGameMode() As GameMode
        Get
            If _gameModeList.Count - 1 >= _gameModePointer Then
                Return _gameModeList(_gameModePointer)
            End If
            Return Nothing
        End Get
    End Property

    ''' <summary>
    ''' Returns the Value of a chosen GameRule from the currently active GameMode.
    ''' </summary>
    ''' <param name="RuleName">The RuleName to search for.</param>
    ''' <returns></returns>
    Public Shared Function GetGameRuleValue(ByVal RuleName As String, ByVal DefaultValue As String) As String
        For Each rule In ActiveGameMode.GameRules
            If rule.Name.ToLower() = RuleName.ToLower() Then
                Return rule.Value
            End If
        Next
        Return DefaultValue
    End Function

    ''' <summary>
    ''' Returns the correct map path to load a map from.
    ''' </summary>
    ''' <param name="levelfile">The levelfile containing the map.</param>
    ''' <returns></returns>
    Public Shared Function GetMapPath(ByVal levelFile As String) As String
        Return GameController.GamePath & ActiveGameMode.MapPath & levelFile
    End Function

    ''' <summary>
    ''' Returns the correct script file path to load a script from.
    ''' </summary>
    ''' <param name="scriptFile">The file that contains the script information.</param>
    ''' <returns></returns>
    Public Shared Function GetScriptPath(ByVal scriptFile As String) As String
        Return GameController.GamePath & ActiveGameMode.ScriptPath & scriptFile
    End Function

    ''' <summary>
    ''' Returns the correct poke file path to load a Wild Pokémon Definition from.
    ''' </summary>
    ''' <param name="pokeFile">The file that contains the Wild Pokémon Definitions.</param>
    ''' <returns></returns>
    Public Shared Function GetPokeFilePath(ByVal pokeFile As String) As String
        Return GameController.GamePath & ActiveGameMode.PokeFilePath & pokeFile
    End Function

    ''' <summary>
    ''' Returns the correct Content file path to load content from.
    ''' </summary>
    ''' <param name="ContentFile">The stub file path to the Content file.</param>
    ''' <returns></returns>
    Public Shared Function GetContentFilePath(ByVal ContentFile As String) As String
        Return GameController.GamePath & ActiveGameMode.ContentPath & ContentFile
    End Function

    ''' <summary>
    ''' Returns the correct Data file path to load data from.
    ''' </summary>
    ''' <param name="DataFile">The stub file path to the data file.</param>
    ''' <returns></returns>
    Public Shared Function GetDataFilePath(ByVal DataFile As String) As String
        Return GameController.GamePath & ActiveGameMode.DataPath & DataFile
    End Function

    ''' <summary>
    ''' Loads the FileItems for the loaded player file.
    ''' </summary>
    Public Shared Sub LoadFileItems()
        If Not Core.Player Is Nothing Then
            Core.Player.FileItems.Clear()

            If ActiveGameMode.IsDefaultGamemode = False Then
                Dim path As String = GetDataFilePath("items.dat")
                If IO.File.Exists(path) = True Then
                    Dim c() As String = IO.File.ReadAllLines(path)
                    For Each line As String In c
                        If line.StartsWith("{") = True And line.EndsWith("}") = True Then
                            Dim fileItem As New Items.FileItem(line)

                            If fileItem.IsValid = True Then
                                Core.Player.FileItems.Add(fileItem)
                            End If
                        End If
                    Next
                End If
            End If
        End If
    End Sub

#End Region

End Class

''' <summary>
''' Represents a GameMode.
''' </summary>
Public Class GameMode

    Private _loaded As Boolean = False
    Private _usedFileName As String = ""
    Private _dataModel As DataModel.Json.GameModeData.GameModeModel

    ''' <summary>
    ''' Is this GameMode loaded correctly?
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    Public ReadOnly Property IsValid() As Boolean
        Get
            If _loaded = True Then
                If Name.ToLower() = "pokemon 3d" And DirectoryName.ToLower() <> "kolben" Then
                    Logger.Log("248", Logger.LogTypes.Message, "Unofficial GameMode with the name ""Pokemon 3D"" exists (in folder: """ & DirectoryName & """)!")
                    Return False
                Else
                    If StartupMap.Length > 0 And StartupScript.Length > 0 Then
                        Return True
                    End If
                End If
            End If

            Return False
        End Get
    End Property

    ''' <summary>
    ''' The name of the directory this GameMode is placed in. Not the full path.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    Public ReadOnly Property DirectoryName() As String
        Get
            If _usedFileName <> "" Then
                Dim directory As String = _usedFileName.Remove(_usedFileName.LastIndexOf("\"))
                directory = directory.Remove(0, directory.LastIndexOf("\") + 1)
                Return directory
            Else
                Return ""
            End If
        End Get
    End Property

    ''' <summary>
    ''' The name of the directory this GameMode is placed in.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    Public ReadOnly Property Path() As String
        Get
            Return IO.Path.GetDirectoryName(_usedFileName)
            If _usedFileName <> "" Then
                Dim directory As String = _usedFileName.Remove(_usedFileName.LastIndexOf("\") + 1)
                Return directory
            Else
                Return ""
            End If
        End Get
    End Property

    ''' <summary>
    ''' Create a new GameMode.
    ''' </summary>
    ''' <param name="FileName">The file the gamemode should load from.</param>
    Public Sub New(ByVal FileName As String)
        Load(FileName)
    End Sub

    ''' <summary>
    ''' This loads the GameMode.
    ''' </summary>
    ''' <param name="fileName"></param>
    Private Sub Load(ByVal fileName As String)
        If IO.File.Exists(fileName) = True Then
            _usedFileName = fileName
            Try
                Dim jsonData As String = IO.File.ReadAllText(fileName)

                _dataModel = DataModel.Json.JsonDataModel.FromString(Of DataModel.Json.GameModeData.GameModeModel)(jsonData)
                _loaded = True
            Catch ex As Exception
                _loaded = False
                Logger.Log("302", Logger.LogTypes.ErrorMessage, "Error trying to load the GameMode file: " & fileName & "; " & ex.Message)
            End Try
        Else
            _loaded = False
        End If
    End Sub

    ''' <summary>
    ''' Reload the GameMode from an already used file.
    ''' </summary>
    Public Sub Reload()
        Load(_usedFileName)
    End Sub

    ''' <summary>
    ''' Reload the GameMode.
    ''' </summary>
    ''' <param name="FileName">Use this file to reload the GameMode from.</param>
    Public Sub Reload(ByVal FileName As String)
        Load(FileName)
    End Sub

    ''' <summary>
    ''' If this is the default GameMode.
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property IsDefaultGamemode() As Boolean
        Get
            Return (Name = "Pokemon 3D" And DirectoryName = "Kolben")
        End Get
    End Property

#Region "GameMode"

    ''' <summary>
    ''' The name of this GameMode.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    Public ReadOnly Property Name() As String
        Get
            Return _dataModel.Name
        End Get
    End Property

    ''' <summary>
    ''' The Description of the GameMode. This may contain multiple lines.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    Public ReadOnly Property Description() As String
        Get
            Return _dataModel.Description
        End Get
    End Property

    ''' <summary>
    ''' The version of the GameMode. Warning: This doesn't have to be a number!
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    Public ReadOnly Property Version() As String
        Get
            Return _dataModel.Version
        End Get
    End Property

    ''' <summary>
    ''' The author of the GameMode.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    Public ReadOnly Property Author() As String
        Get
            Return _dataModel.Author
        End Get
    End Property

    ''' <summary>
    ''' The MapPath used from this GameMode to load maps from.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    Public ReadOnly Property MapPath() As String
        Get
            Return "\GameModes\" & DirectoryName & "\" & _dataModel.MapPath
        End Get
    End Property

    ''' <summary>
    ''' The ScriptPath from this GameMode to load scripts from.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    Public ReadOnly Property ScriptPath() As String
        Get
            Return "\GameModes\" & DirectoryName & "\" & _dataModel.ScriptPath
        End Get
    End Property

    ''' <summary>
    ''' The .poke file directory from this GameMode.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    Public ReadOnly Property PokeFilePath() As String
        Get
            Return "\GameModes\" & DirectoryName & "\" & _dataModel.PokeFilePath
        End Get
    End Property

    ''' <summary>
    ''' The Datapath to load data from.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    Public ReadOnly Property DataPath() As String
        Get
            Return "\GameModes\" & DirectoryName & "\" & _dataModel.DataPath
        End Get
    End Property

    ''' <summary>
    ''' The content path to load images, sounds and music from.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    Public ReadOnly Property ContentPath() As String
        Get
            Return "\GameModes\" & DirectoryName & "\" & _dataModel.ContentPath
        End Get
    End Property

    ''' <summary>
    ''' The GameRules that apply to this GameMode.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    Public Property GameRules() As List(Of DataModel.Json.GameModeData.GameModeModel.GameRuleModel)
        Get
            Return _dataModel.Gamerules
        End Get
        Set(value As List(Of DataModel.Json.GameModeData.GameModeModel.GameRuleModel))
            _dataModel.Gamerules = value
        End Set
    End Property

    ''' <summary>
    ''' The map that will be loaded when starting a new game.
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property StartupMap() As String
        Get
            Return _dataModel.StartConfiguration.Map
        End Get
    End Property

    ''' <summary>
    ''' The script that will get loaded when starting a new game.
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property StartupScript() As String
        Get
            Return _dataModel.StartConfiguration.Script
        End Get
    End Property

#End Region

End Class