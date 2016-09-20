Public Class GameModeManager

    Private Shared GameModeList As New List(Of GameMode)
    Private Shared GameModePointer As Integer = 0
    Public Shared Initialized As Boolean = False

    ''' <summary>
    ''' Loads (or reloads) the list of GameModes. The pointer also gets reset.
    ''' </summary>
    Public Shared Sub LoadGameModes()
        GameModeList.Clear()
        GameModePointer = 0

        CreateKolbenMode()

        For Each GameModeFolder As String In System.IO.Directory.GetDirectories(GameController.GamePath & "\GameModes\")
            If System.IO.File.Exists(GameModeFolder & "\GameMode.dat") = True Then
                AddGameMode(GameModeFolder)
            End If
        Next

        SetGameModePointer("Kolben")
        Initialized = True
    End Sub

    Public Shared Function GetGameMode(ByVal GameModeDirectory As String) As GameMode
        For Each GameMode As GameMode In GameModeList
            If GameMode.DirectoryName = GameModeDirectory Then
                Return GameMode
            End If
        Next

        Return Nothing
    End Function

    ''' <summary>
    ''' Creates the GameModes directory.
    ''' </summary>
    Public Shared Sub CreateGameModesFolder()
        If System.IO.Directory.Exists(GameController.GamePath & "\GameModes") = False Then
            System.IO.Directory.CreateDirectory(GameController.GamePath & "\GameModes")
        End If
    End Sub

    ''' <summary>
    ''' Sets the GameModePointer to a new item.
    ''' </summary>
    ''' <param name="GameModeDirectoryName">The directory resembeling the new GameMode.</param>
    Public Shared Sub SetGameModePointer(ByVal GameModeDirectoryName As String)
        For i = 0 To GameModeList.Count - 1
            Dim GameMode As GameMode = GameModeList(i)
            If GameMode.DirectoryName = GameModeDirectoryName Then
                GameModePointer = i
                Logger.Debug("---Set pointer to """ & GameModeDirectoryName & """!---")
                Exit Sub
            End If
        Next
        Logger.Debug("Couldn't find the GameMode """ & GameModeDirectoryName & """!")
    End Sub

    ''' <summary>
    ''' Returns the amount of loaded GameModes.
    ''' </summary>
    Public Shared ReadOnly Property GameModeCount() As Integer
        Get
            Return GameModeList.Count
        End Get
    End Property

    ''' <summary>
    ''' Checks if a GameMode exists.
    ''' </summary>
    Public Shared Function GameModeExists(ByVal GameModePath As String) As Boolean
        For Each GameMode As GameMode In GameModeList
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
            GameModeList.Add(newGameMode)
        End If
    End Sub

    ''' <summary>
    ''' Creates the default Kolben GameMode.
    ''' </summary>
    Public Shared Sub CreateKolbenMode()
        Dim doCreateKolbenMode As Boolean = False
        If System.IO.Directory.Exists(GameController.GamePath & "\GameModes\Kolben") = True Then
            System.IO.Directory.Delete(GameController.GamePath & "\GameModes\Kolben", True)
        End If
        If System.IO.Directory.Exists(GameController.GamePath & "\GameModes\Kolben") = False Then
            doCreateKolbenMode = True
            System.IO.Directory.CreateDirectory(GameController.GamePath & "\GameModes\Kolben")
        End If
        If doCreateKolbenMode = False Then
            If System.IO.File.Exists(GameController.GamePath & "\GameModes\Kolben\GameMode.dat") = False Then
                doCreateKolbenMode = True
            End If
        End If

        If doCreateKolbenMode = True Then
            Dim kolbenMode As GameMode = GameMode.GetKolbenGameMode()
            kolbenMode.SaveToFile(GameController.GamePath & "\GameModes\Kolben\GameMode.dat")
        End If
    End Sub

#Region "Shared GameModeFunctions"

    ''' <summary>
    ''' Returns the currently active GameMode.
    ''' </summary>
    Public Shared ReadOnly Property ActiveGameMode() As GameMode
        Get
            If GameModeList.Count - 1 >= GameModePointer Then
                Return GameModeList(GameModePointer)
            End If
            Return Nothing
        End Get
    End Property

    ''' <summary>
    ''' Returns the GameRules of the currently active GameMode.
    ''' </summary>
    Public Shared Function GetGameRules() As List(Of GameMode.GameRule)
        Return ActiveGameMode.GameRules
    End Function

    ''' <summary>
    ''' Returns the Value of a chosen GameRule from the currently active GameMode.
    ''' </summary>
    ''' <param name="RuleName">The RuleName to search for.</param>
    Public Shared Function GetGameRuleValue(ByVal RuleName As String, ByVal DefaultValue As String) As String
start:
        Dim rules As List(Of GameMode.GameRule) = GetGameRules()
        For Each rule As GameMode.GameRule In rules
            If rule.RuleName.ToLower() = RuleName.ToLower() Then
                Return rule.RuleValue
            End If
        Next

        ActiveGameMode.GameRules.Add(New GameMode.GameRule(RuleName, DefaultValue))
        GoTo start

        Return ""
    End Function

    ''' <summary>
    ''' Returns the correct map path to load a map from.
    ''' </summary>
    ''' <param name="levelfile">The levelfile containing the map.</param>
    Public Shared Function GetMapPath(ByVal levelFile As String) As String
        If ActiveGameMode.IsDefaultGamemode = True Then
            Return GameController.GamePath & GameMode.DefaultMapPath & levelFile
        End If

        If System.IO.File.Exists(GameController.GamePath & ActiveGameMode.MapPath & levelFile) = True Then
            Return GameController.GamePath & ActiveGameMode.MapPath & levelFile
        End If

        If GameController.GamePath & GameMode.DefaultMapPath & levelFile <> GameController.GamePath & ActiveGameMode.MapPath & levelFile Then
            Logger.Log(Logger.LogTypes.Message, "Map file: """ & ActiveGameMode.MapPath & levelFile & """ does not exist in the GameMode. The game tries to load the normal file at ""\maps\" & levelFile & """.")
        End If

        Return GameController.GamePath & GameMode.DefaultMapPath & levelFile
    End Function

    ''' <summary>
    ''' Returns the correct script file path to load a script from.
    ''' </summary>
    ''' <param name="scriptFile">The file that contains the script information.</param>
    Public Shared Function GetScriptPath(ByVal scriptFile As String) As String
        If ActiveGameMode.IsDefaultGamemode = True Then
            Return GameController.GamePath & GameMode.DefaultScriptPath & scriptFile
        End If

        If System.IO.File.Exists(GameController.GamePath & ActiveGameMode.ScriptPath & scriptFile) = True Then
            Return GameController.GamePath & ActiveGameMode.ScriptPath & scriptFile
        End If

        If GameController.GamePath & GameMode.DefaultScriptPath & scriptFile <> GameController.GamePath & ActiveGameMode.ScriptPath & scriptFile Then
            Logger.Log(Logger.LogTypes.Message, "Script file: """ & ActiveGameMode.ScriptPath & scriptFile & """ does not exist in the GameMode. The game tries to load the normal file at ""\Scripts\" & scriptFile & """.")
        End If

        Return GameController.GamePath & GameMode.DefaultScriptPath & scriptFile
    End Function

    ''' <summary>
    ''' Returns the correct poke file path to load a Wild Pokémon Definition from.
    ''' </summary>
    ''' <param name="pokeFile">The file that contains the Wild Pokémon Definitions.</param>
    Public Shared Function GetPokeFilePath(ByVal pokeFile As String) As String
        If ActiveGameMode.IsDefaultGamemode = True Then
            Return GameController.GamePath & GameMode.DefaultPokeFilePath & pokeFile
        End If

        If System.IO.File.Exists(GameController.GamePath & ActiveGameMode.PokeFilePath & pokeFile) = True Then
            Return GameController.GamePath & ActiveGameMode.PokeFilePath & pokeFile
        End If

        If GameController.GamePath & GameMode.DefaultPokeFilePath & pokeFile <> GameController.GamePath & ActiveGameMode.PokeFilePath & pokeFile Then
            Logger.Log(Logger.LogTypes.Message, "Poke file: """ & ActiveGameMode.PokeFilePath & pokeFile & """ does not exist in the GameMode. The game tries to load the normal file at ""\maps\poke\" & pokeFile & """.")
        End If

        Return GameController.GamePath & GameMode.DefaultPokeFilePath & pokeFile
    End Function

    ''' <summary>
    ''' Returns the correct Pokémon data file path to load a Pokémon from.
    ''' </summary>
    ''' <param name="PokemonDataFile">The file which contains the Pokémon information.</param>
    Public Shared Function GetPokemonDataFilePath(ByVal PokemonDataFile As String) As String
        If ActiveGameMode.IsDefaultGamemode = True Then
            Return GameController.GamePath & GameMode.DefaultPokemonDataPath & PokemonDataFile
        End If

        If System.IO.File.Exists(GameController.GamePath & ActiveGameMode.PokemonDataPath & PokemonDataFile) = True Then
            Return GameController.GamePath & ActiveGameMode.PokemonDataPath & PokemonDataFile
        End If

        If GameController.GamePath & GameMode.DefaultPokemonDataPath & PokemonDataFile <> GameController.GamePath & ActiveGameMode.PokemonDataPath & PokemonDataFile Then
            'Logger.Log(Logger.LogTypes.Message, "Pokemon data file: """ & ActiveGameMode.PokemonDataPath & PokemonDataFile & """ does not exist in the GameMode. The game tries to load the normal file at ""\Content\Pokemon\Data\" & PokemonDataFile & """.")
        End If

        Return GameController.GamePath & GameMode.DefaultPokemonDataPath & PokemonDataFile
    End Function

    Public Shared Function GetLocalizationsPath(ByVal TokensFile As String) As String
        If System.IO.File.Exists(GameController.GamePath & ActiveGameMode.LocalizationsPath & TokensFile) = True Then
            Return GameController.GamePath & ActiveGameMode.LocalizationsPath & TokensFile
        End If

        Return GameController.GamePath & GameMode.DefaultLocalizationsPath & TokensFile
    End Function

    ''' <summary>
    ''' Returns the correct Content file path to load content from.
    ''' </summary>
    ''' <param name="ContentFile">The stub file path to the Content file.</param>
    Public Shared Function GetContentFilePath(ByVal ContentFile As String) As String
        If ActiveGameMode.IsDefaultGamemode = True Then
            Return GameController.GamePath & GameMode.DefaultContentPath & ContentFile
        End If

        If System.IO.File.Exists(GameController.GamePath & ActiveGameMode.ContentPath & ContentFile) = True Then
            Return GameController.GamePath & ActiveGameMode.ContentPath & ContentFile
        End If

        Return GameController.GamePath & GameMode.DefaultContentPath & ContentFile
    End Function

    ''' <summary>
    ''' Checks if a map file exists either in the active GameMode or the default GameMode.
    ''' </summary>
    ''' <param name="levelFile">The map file to look for.</param>
    Public Shared Function MapFileExists(ByVal levelFile As String) As Boolean
        Dim path As String = GameController.GamePath & ActiveGameMode.MapPath & levelFile
        Dim defaultPath As String = GameController.GamePath & GameMode.DefaultMapPath & levelFile
        If ActiveGameMode.IsDefaultGamemode = True Then
            path = GameController.GamePath & GameMode.DefaultMapPath & levelFile
        End If

        Return System.IO.File.Exists(path) Or System.IO.File.Exists(defaultPath)
    End Function

    ''' <summary>
    ''' Checks if a Content file exists either in the active GameMode or the default GameMode.
    ''' </summary>
    ''' <param name="contentFile">The Content file to look for.</param>
    Public Shared Function ContentFileExists(ByVal contentFile As String) As Boolean
        Dim path As String = GameController.GamePath & ActiveGameMode.ContentPath & contentFile
        Dim defaultPath As String = GameController.GamePath & GameMode.DefaultContentPath & contentFile
        If ActiveGameMode.IsDefaultGamemode = True Then
            path = GameController.GamePath & GameMode.DefaultContentPath & contentFile
        End If

        Return System.IO.File.Exists(path) Or System.IO.File.Exists(defaultPath)
    End Function

#End Region

End Class

Public Class GameMode

    Private _loaded As Boolean = False
    Private _usedFileName As String = ""

    ''' <summary>
    ''' Is this GameMode loaded correctly?
    ''' </summary>
    Public ReadOnly Property IsValid() As Boolean
        Get
            If _loaded = True Then
                If Me.Name.ToLower() = "pokemon 3d" And Me.DirectoryName.ToLower() <> "kolben" Then
                    Logger.Log(Logger.LogTypes.Message, "Unofficial GameMode with the name ""Pokemon 3D"" exists (in folder: """ & Me.DirectoryName & """)!")
                    Return False
                End If
                'If _name <> "" And _description <> "" And _version <> "" And _author <> "" And _mapPath <> "" And _scriptPath <> "" And _pokeFilePath <> "" And
                '    _pokemonDataPath <> "" And _startMap <> "" And _startLocationName <> "" And _pokemonAppear <> "" And _introMusic <> "" And _skinColors.Count > 0 And _skinFiles.Count > 0 And _skinNames.Count > 0 Then
                '    Return True
                'End If
                Return True
            End If

            Return False
        End Get
    End Property

    ''' <summary>
    ''' The name of the directory this GameMode is placed in. Not the full path.
    ''' </summary>
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
    Public ReadOnly Property Path() As String
        Get
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
    ''' Create a new GameMode.
    ''' </summary>
    ''' <param name="Name">The name of the new GameMode.</param>
    ''' <param name="Description">The description of the new GameMode.</param>
    ''' <param name="Version">The version of the new GameMode. Warning: This doesn't have to be a number!</param>
    ''' <param name="Author">The author of the new GameMode.</param>
    ''' <param name="MapPath">The MapPath used from the new GameMode to load maps from.</param>
    ''' <param name="ScriptPath">The ScriptPath used from the new GameMode to load scripts from.</param>
    ''' <param name="PokemonDataPath">The Pokemon-Datapath to load Pokemon data from.</param>
    ''' <param name="ContentPath">The path to load images, sound and music from.</param>
    ''' <param name="GameRules">The GameRules that apply to the new GameMode.</param>
    ''' <param name="StartMap">The start map for the new GameMode.</param>
    ''' <param name="StartPosition">The start position for the new GameMode.</param>
    ''' <param name="StartRotation">The start rotation for the new GameMode.</param>
    ''' <param name="StartLocationName">The start location name for the new GameMode.</param>
    ''' <param name="PokemonAppear">The Pokémon that appear on the new game screen for the new GameMode.</param>
    ''' <param name="IntroMusic">The intro music that plays on the new game screen for the new GameMode.</param>
    ''' <param name="SkinColors">The skin colors for the new GameMode. Must be the same amount as SkinFiles and SkinNames.</param>
    ''' <param name="SkinFiles">The skin files for the new GameMode. Must be the same amount as SkinColors and SkinNames.</param>
    ''' <param name="SkinNames">The skin names for the new GameMode. Must be the same amount as SkinFiles and SkinColors.</param>
    Public Sub New(ByVal Name As String, ByVal Description As String, ByVal Version As String, ByVal Author As String, ByVal MapPath As String, ByVal ScriptPath As String, ByVal PokeFilePath As String, ByVal PokemonDataPath As String, ByVal ContentPath As String, ByVal LocalizationsPath As String, ByVal GameRules As List(Of GameRule),
                   ByVal StartMap As String, ByVal StartPosition As Vector3, ByVal StartRotation As Single, ByVal StartLocationName As String, ByVal StartDialogue As String, ByVal StartColor As Color, ByVal PokemonAppear As String, ByVal IntroMusic As String, ByVal SkinColors As List(Of Color), ByVal SkinFiles As List(Of String), ByVal SkinNames As List(Of String))
        Me._name = Name
        Me._description = Description
        Me._version = Version
        Me._author = Author
        Me._mapPath = MapPath
        Me._scriptPath = ScriptPath
        Me._pokeFilePath = PokeFilePath
        Me._pokemonDataPath = PokemonDataPath
        Me._contentPath = ContentPath
        Me._localizationsPath = LocalizationsPath
        Me._gameRules = GameRules

        Me._startMap = StartMap
        Me._startPosition = StartPosition
        Me._startRotation = StartRotation
        Me._startLocationName = StartLocationName
        Me._startDialogue = StartDialogue
        Me._startColor = StartColor
        Me._pokemonAppear = PokemonAppear
        Me._introMusic = IntroMusic
        Me._skinColors = SkinColors
        Me._skinFiles = SkinFiles
        Me._skinNames = SkinNames

        Me._loaded = True
    End Sub

    ''' <summary>
    ''' This loads the GameMode.
    ''' </summary>
    Private Sub Load(ByVal FileName As String)
        If System.IO.File.Exists(FileName) = True Then
            Dim Data() As String = System.IO.File.ReadAllLines(FileName)

            For Each line As String In Data
                If line <> "" And line.CountSeperators("|") > 0 Then
                    Dim Pointer As String = line.Remove(line.IndexOf("|"))
                    Dim Value As String = line.Remove(0, line.IndexOf("|") + 1)

                    Select Case Pointer.ToLower()
                        Case "name"
                            Me._name = Value
                        Case "description"
                            Me._description = Value
                        Case "version"
                            Me._version = Value
                        Case "author"
                            Me._author = Value
                        Case "mappath"
                            Me._mapPath = Value
                        Case "scriptpath"
                            Me._scriptPath = Value
                        Case "pokefilepath"
                            Me._pokeFilePath = Value
                        Case "pokemondatapath"
                            Me._pokemonDataPath = Value
                        Case "contentpath"
                            Me._contentPath = Value
                        Case "localizationspath"
                            Me._localizationsPath = Value
                        Case "gamerules"
                            If Value <> "" And Value.Contains("(") And Value.Contains(")") And Value.Contains("|") = True Then
                                Dim rules() As String = Value.Split(CChar(")"))
                                For Each rule As String In rules
                                    If rule.StartsWith("(") = True Then
                                        rule = rule.Remove(0, 1)

                                        _gameRules.Add(New GameRule(rule.GetSplit(0, "|"), rule.GetSplit(1, "|")))
                                    End If
                                Next
                            End If
                        Case "startmap"
                            Me._startMap = Value
                        Case "startposition"
                            Dim PositionList() As String = Value.Split(CChar(","))
                            If PositionList.Length >= 3 Then
                                Me._startPosition = New Vector3(CSng(PositionList(0).Replace(".", GameController.DecSeparator)), CSng(PositionList(1).Replace(".", GameController.DecSeparator)), CSng(PositionList(2).Replace(".", GameController.DecSeparator)))
                            Else
                                Me._startPosition = Vector3.Zero
                            End If
                        Case "startrotation"
                            Me._startRotation = CSng(Value.Replace(".", GameController.DecSeparator))
                        Case "startlocationname"
                            Me._startLocationName = Value
                        Case "startdialogue"
                            Me._startDialogue = Value
                        Case "startcolor"
                            If Value <> "" And Value.CountSplits(",") = 3 Then
                                Dim c() As String = Value.Split(CChar(","))
                                Me._startColor = New Color(CInt(c(0)), CInt(c(1)), CInt(c(2)))
                            Else
                                Me._startColor = New Color(59, 123, 165)
                            End If
                        Case "pokemonappear"
                            Me._pokemonAppear = Value

                            If CInt(Value) = 0 Then
                                Me._pokemonRange = {1, 252}
                            Else
                                If Value.Contains("-") = True Then
                                    Dim v1 As Integer = CInt(Value.GetSplit(0, "-"))
                                    Dim v2 As Integer = CInt(Value.GetSplit(1, "-")) + 1

                                    Me._pokemonRange = {v1, v2}
                                Else
                                    Me._pokemonRange = {CInt(Value), CInt(Value) + 1}
                                End If
                            End If
                        Case "intromusic"
                            Me._introMusic = Value
                        Case "skincolors"
                            Dim l As New List(Of Color)
                            For Each color As String In Value.Split(CChar(","))
                                Dim c As New Color(CInt(color.GetSplit(0, ";")), CInt(color.GetSplit(1, ";")), CInt(color.GetSplit(2, ";")))
                                l.Add(c)
                            Next
                            If l.Count > 0 Then
                                Me._skinColors = l
                            End If
                        Case "skinfiles"
                            Dim l As New List(Of String)
                            For Each skin As String In Value.Split(CChar(","))
                                l.Add(skin)
                            Next
                            If l.Count > 0 Then
                                Me._skinFiles = l
                            End If
                        Case "skinnames"
                            Dim l As New List(Of String)
                            For Each skin As String In Value.Split(CChar(","))
                                l.Add(skin)
                            Next
                            If l.Count > 0 Then
                                Me._skinNames = l
                            End If
                    End Select
                End If
            Next

            _loaded = True

            Me._usedFileName = FileName
        End If
    End Sub

    ''' <summary>
    ''' Reload the GameMode from an already used file.
    ''' </summary>
    Public Sub Reload()
        Load(Me._usedFileName)
    End Sub

    ''' <summary>
    ''' Reload the GameMode.
    ''' </summary>
    ''' <param name="FileName">Use this file to reload the GameMode from.</param>
    Public Sub Reload(ByVal FileName As String)
        Load(FileName)
    End Sub

    ''' <summary>
    ''' Returns the default Kolben Game Mode.
    ''' </summary>
    Public Shared Function GetKolbenGameMode() As GameMode
        Dim SkinColors As List(Of Color) = {New Color(248, 176, 32), New Color(248, 216, 88), New Color(56, 88, 200), New Color(216, 96, 112), New Color(56, 88, 152), New Color(239, 90, 156)}.ToList()
        Dim SkinFiles As List(Of String) = {"Ethan", "Lyra", "Nate", "Rosa", "Hilbert", "Hilda"}.ToList()
        Dim SkinNames As List(Of String) = {"Ethan", "Lyra", "Nate", "Rosa", "Hilbert", "Hilda"}.ToList()

        Dim gameMode As New GameMode("Pokemon 3D", "The normal game mode.", GameController.GAMEVERSION, "Kolben Games", "\maps\", "\Scripts\", "\maps\poke\", "\Content\Pokemon\Data\", "\Content\", "\Content\Localization\", New List(Of GameRule),
                                     "yourroom.dat", New Vector3(1.0F, 0.1F, 3.0F), MathHelper.PiOver2, "Your Room", "", New Color(59, 123, 165), "0", "welcome", SkinColors, SkinFiles, SkinNames)

        Dim gameRules As New List(Of GameRule)
        gameRules.Add(New GameRule("MaxLevel", "100"))
        gameRules.Add(New GameRule("OnlyCaptureFirst", "0"))
        gameRules.Add(New GameRule("ForceRename", "0"))
        gameRules.Add(New GameRule("DeathInsteadOfFaint", "0"))
        gameRules.Add(New GameRule("CanUseHealItems", "1"))
        gameRules.Add(New GameRule("Difficulty", "0"))
        gameRules.Add(New GameRule("LockDifficulty", "0"))
        gameRules.Add(New GameRule("GameOverAt0Pokemon", "0"))
        gameRules.Add(New GameRule("CanGetAchievements", "1"))
        gameRules.Add(New GameRule("ShowFollowPokemon", "1"))

        GameMode.GameRules = gameRules

        Return GameMode
    End Function

    ''' <summary>
    ''' Export this GameMode to a file.
    ''' </summary>
    ''' <param name="File">The file this GameMode should get exported to.</param>
    Public Sub SaveToFile(ByVal File As String)
        Dim s As String = "Name|" & Me._name & vbNewLine &
            "Description|" & Me._description & vbNewLine &
            "Version|" & Me._version & vbNewLine &
            "Author|" & Me._author & vbNewLine &
            "MapPath|" & Me._mapPath & vbNewLine &
            "ScriptPath|" & Me._scriptPath & vbNewLine &
            "PokeFilePath|" & Me._pokeFilePath & vbNewLine &
            "PokemonDataPath|" & Me._pokemonDataPath & vbNewLine &
            "ContentPath|" & Me._contentPath & vbNewLine &
            "LocalizationsPath|" & Me._localizationsPath & vbNewLine

        Dim GameRuleString As String = "Gamerules|"
        For Each rule As GameRule In Me._gameRules
            GameRuleString &= "(" & rule.RuleName & "|" & rule.RuleValue & ")"
        Next

        s &= GameRuleString & vbNewLine &
            "StartMap|" & Me._startMap & vbNewLine &
            "StartPosition|" & Me._startPosition.X.ToString().Replace(GameController.DecSeparator, ".") & "," & Me._startPosition.Y.ToString().Replace(GameController.DecSeparator, ".") & "," & Me._startPosition.Z.ToString().Replace(GameController.DecSeparator, ".") & vbNewLine &
            "StartRotation|" & Me._startRotation.ToString().Replace(GameController.DecSeparator, ".") & vbNewLine &
            "StartLocationName|" & Me._startLocationName & vbNewLine &
            "StartDialogue|" & Me._startDialogue & vbNewLine &
            "StartColor|" & Me._startColor.R & "," & Me._startColor.G & "," & Me._startColor.B & vbNewLine &
            "PokemonAppear|" & Me._pokemonAppear & vbNewLine &
            "IntroMusic|" & Me._introMusic & vbNewLine

        Dim SkinColorsString As String = "SkinColors|"
        Dim iSC As Integer = 0
        For Each SkinColor As Color In _skinColors
            If iSC > 0 Then
                SkinColorsString &= ","
            End If

            SkinColorsString &= SkinColor.R & ";" & SkinColor.G & ";" & SkinColor.B

            iSC += 1
        Next

        s &= SkinColorsString & vbNewLine

        Dim SkinFilesString As String = "SkinFiles|"
        Dim iSF As Integer = 0
        For Each SkinFile As String In Me._skinFiles
            If iSF > 0 Then
                SkinFilesString &= ","
            End If

            SkinFilesString &= SkinFile

            iSF += 1
        Next

        s &= SkinFilesString & vbNewLine

        Dim SkinNamesString As String = "SkinNames|"
        Dim iSN As Integer = 0
        For Each SkinName As String In Me._skinNames
            If iSN > 0 Then
                SkinNamesString &= ","
            End If

            SkinNamesString &= SkinName

            iSN += 1
        Next

        s &= SkinNamesString

        Dim folder As String = System.IO.Path.GetDirectoryName(File)
        If System.IO.Directory.Exists(folder) = False Then
            System.IO.Directory.CreateDirectory(folder)
        End If

        System.IO.File.WriteAllText(File, s)
    End Sub

    Public ReadOnly Property IsDefaultGamemode() As Boolean
        Get
            Return (Me.Name = "Pokemon 3D")
        End Get
    End Property

#Region "Paths"

    Public Const DefaultContentPath As String = "\Content\"
    Public Const DefaultMapPath As String = "\maps\"
    Public Const DefaultScriptPath As String = "\Scripts\"
    Public Const DefaultPokeFilePath As String = "\maps\poke\"
    Public Const DefaultPokemonDataPath As String = "\Content\Pokemon\Data\"
    Public Const DefaultLocalizationsPath As String = "\Content\Localization\"

#End Region

#Region "GameMode"

    Private _name As String = ""
    Private _description As String = ""
    Private _version As String = ""
    Private _author As String = ""
    Private _mapPath As String = ""
    Private _scriptPath As String = ""
    Private _pokeFilePath As String = ""
    Private _pokemonDataPath As String = ""
    Private _localizationsPath As String = ""
    Private _contentPath As String = ""
    Private _gameRules As New List(Of GameRule)

    ''' <summary>
    ''' The name of this GameMode.
    ''' </summary>
    Public Property Name() As String
        Get
            Return Me._name
        End Get
        Set(value As String)
            Me._name = value
        End Set
    End Property

    ''' <summary>
    ''' The Description of the GameMode. This may contain multiple lines.
    ''' </summary>
    Public Property Description() As String
        Get
            Return Me._description
        End Get
        Set(value As String)
            Me._description = value
        End Set
    End Property

    ''' <summary>
    ''' The version of the GameMode. Warning: This doesn't have to be a number!
    ''' </summary>
    Public Property Version() As String
        Get
            Return Me._version
        End Get
        Set(value As String)
            Me._version = value
        End Set
    End Property

    ''' <summary>
    ''' The author of the GameMode.
    ''' </summary>
    Public Property Author() As String
        Get
            Return Me._author
        End Get
        Set(value As String)
            Me._author = value
        End Set
    End Property

    ''' <summary>
    ''' The MapPath used from this GameMode to load maps from.
    ''' </summary>
    Public Property MapPath() As String
        Get
            Return Me._mapPath.Replace("$Mode", "\GameModes\" & Me.DirectoryName)
        End Get
        Set(value As String)
            Me._mapPath = value
        End Set
    End Property

    ''' <summary>
    ''' The ScriptPath from this GameMode to load scripts from.
    ''' </summary>
    Public Property ScriptPath() As String
        Get
            Return Me._scriptPath.Replace("$Mode", "\GameModes\" & Me.DirectoryName)
        End Get
        Set(value As String)
            Me._scriptPath = value
        End Set
    End Property

    ''' <summary>
    ''' The .poke file directory from this GameMode.
    ''' </summary>
    Public Property PokeFilePath() As String
        Get
            Return Me._pokeFilePath.Replace("$Mode", "\GameModes\" & Me.DirectoryName)
        End Get
        Set(value As String)
            Me._pokeFilePath = value
        End Set
    End Property

    ''' <summary>
    ''' The Pokemon-Datapath to load Pokemon data from.
    ''' </summary>
    Public Property PokemonDataPath() As String
        Get
            Return Me._pokemonDataPath.Replace("$Mode", "\GameModes\" & Me.DirectoryName)
        End Get
        Set(value As String)
            Me._pokemonDataPath = value
        End Set
    End Property

    ''' <summary>
    ''' The content path to load images, sounds and music from.
    ''' </summary>
    Public Property ContentPath() As String
        Get
            Return Me._contentPath.Replace("$Mode", "\GameModes\" & Me.DirectoryName)
        End Get
        Set(value As String)
            Me._contentPath = value
        End Set
    End Property

    ''' <summary>
    ''' The localizations path to load additional tokens from. Tokens that are already existing get overritten.
    ''' </summary>
    Public Property LocalizationsPath() As String
        Get
            Return Me._localizationsPath.Replace("$Mode", "\GameModes\" & Me.DirectoryName)
        End Get
        Set(value As String)
            Me._localizationsPath = value
        End Set
    End Property

    ''' <summary>
    ''' The GameRules that apply to this GameMode.
    ''' </summary>
    Public Property GameRules() As List(Of GameRule)
        Get
            Return Me._gameRules
        End Get
        Set(value As List(Of GameRule))
            Me._gameRules = value
        End Set
    End Property

#End Region

#Region "StartUp"

    Private _startMap As String = ""
    Private _startPosition As Vector3
    Private _startRotation As Single
    Private _startLocationName As String = ""
    Private _startDialogue As String = ""
    Private _startColor As Color = New Color(59, 123, 165)
    Private _pokemonAppear As String = ""
    Private _introMusic As String = ""
    Private _skinColors As New List(Of Color)
    Private _skinFiles As New List(Of String)
    Private _skinNames As New List(Of String)
    Private _pokemonRange() As Integer

    ''' <summary>
    ''' The start map for this GameMode.
    ''' </summary>
    Public Property StartMap() As String
        Get
            Return Me._startMap
        End Get
        Set(value As String)
            Me._startMap = value
        End Set
    End Property

    ''' <summary>
    ''' The start position for this GameMode.
    ''' </summary>
    Public Property StartPosition() As Vector3
        Get
            Return Me._startPosition
        End Get
        Set(value As Vector3)
            Me._startPosition = value
        End Set
    End Property

    ''' <summary>
    ''' The start rotation for this GameMode.
    ''' </summary>
    Public Property StartRotation() As Single
        Get
            Return Me._startRotation
        End Get
        Set(value As Single)
            Me._startRotation = value
        End Set
    End Property

    ''' <summary>
    ''' The start location name for this GameMode.
    ''' </summary>
    Public Property StartLocationName() As String
        Get
            Return Me._startLocationName
        End Get
        Set(value As String)
            Me._startLocationName = value
        End Set
    End Property

    ''' <summary>
    ''' The dialogue said in the intro of the game. Split in 3 different texts: intro dialogue, after Pokémon jumped out, after name + character choose.
    ''' </summary>
    Public Property StartDialogue() As String
        Get
            Return Me._startDialogue
        End Get
        Set(value As String)
            Me._startDialogue = value
        End Set
    End Property

    ''' <summary>
    ''' The default background color in the intro sequence.
    ''' </summary>
    Public Property StartColor() As Color
        Get
            Return Me._startColor
        End Get
        Set(value As Color)
            Me._startColor = value
        End Set
    End Property

    ''' <summary>
    ''' The Pokémon that appear on the new game screen for this GameMode.
    ''' </summary>
    Public Property PokemonAppear() As String
        Get
            Return Me._pokemonAppear
        End Get
        Set(value As String)
            Me._pokemonAppear = value

            If CInt(value) = 0 Then
                Me._pokemonRange = {1, 252}
            Else
                If value.Contains("-") = True Then
                    Dim v1 As Integer = CInt(value.GetSplit(0, "-"))
                    Dim v2 As Integer = CInt(value.GetSplit(1, "-")) + 1

                    Me._pokemonRange = {v1, v2}
                Else
                    Me._pokemonRange = {CInt(value), CInt(value) + 1}
                End If
            End If
        End Set
    End Property

    ''' <summary>
    ''' The Pokémon range that will appear on the new game screen for this GameMode.
    ''' </summary>
    Public ReadOnly Property PokemonRange() As Integer()
        Get
            Return Me._pokemonRange
        End Get
    End Property

    ''' <summary>
    ''' The intro music that plays on the new game screen for this GameMode.
    ''' </summary>
    Public Property IntroMusic() As String
        Get
            Return Me._introMusic
        End Get
        Set(value As String)
            Me._introMusic = value
        End Set
    End Property

    ''' <summary>
    ''' The skin colors for this GameMode. Must be the same amount as SkinFiles and SkinNames.
    ''' </summary>
    Public Property SkinColors() As List(Of Color)
        Get
            Return Me._skinColors
        End Get
        Set(value As List(Of Color))
            Me._skinColors = value
        End Set
    End Property

    ''' <summary>
    ''' The skin files for this GameMode. Must be the same amount as SkinNames and SkinColors.
    ''' </summary>
    Public Property SkinFiles() As List(Of String)
        Get
            Return Me._skinFiles
        End Get
        Set(value As List(Of String))
            Me._skinFiles = value
        End Set
    End Property

    ''' <summary>
    ''' The skin names for this GameMode. Must be the same amount as SkinFiles and SkinColors.
    ''' </summary>
    Public Property SkinNames() As List(Of String)
        Get
            Return Me._skinNames
        End Get
        Set(value As List(Of String))
            Me._skinNames = value
        End Set
    End Property

#End Region

    Class GameRule

        Private _ruleName As String = "EMPTY"
        Private _ruleValue As String = "EMPTY"

        ''' <summary>
        ''' Creates a new GameRule.
        ''' </summary>
        ''' <param name="Name">The name of the game rule.</param>
        ''' <param name="Value">The value of the game rule.</param>
        Public Sub New(ByVal Name As String, ByVal Value As String)
            Me._ruleName = Name
            Me._ruleValue = Value
        End Sub

        ''' <summary>
        ''' The name of this GameRule.
        ''' </summary>
        Public Property RuleName() As String
            Get
                Return _ruleName
            End Get
            Set(value As String)
                Me._ruleName = value
            End Set
        End Property

        ''' <summary>
        ''' The Value of this GameRule.
        ''' </summary>
        Public Property RuleValue() As String
            Get
                Return Me._ruleValue
            End Get
            Set(value As String)
                Me._ruleValue = value
            End Set
        End Property

    End Class

End Class
