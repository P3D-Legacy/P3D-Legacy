Imports P3D

Public Class PokemonForms

    Private Shared _pokemonList As New List(Of PokemonForm)
    Const PATH As String = "Data\Forms.dat"
    Public Shared Sub Initialize()
        _pokemonList.Clear()

        If GameModeManager.ActiveGameMode.IsDefaultGamemode = False AndAlso File.Exists(GameController.GamePath & GameModeManager.ActiveGameMode.ContentPath & PATH) = True Then
            LoadForm(GameController.GamePath & GameModeManager.ActiveGameMode.ContentPath & PATH)
        Else
            LoadForm(GameController.GamePath & GameMode.DefaultContentPath & PATH)
        End If
    End Sub

    ''' <summary>
    ''' Loads a move from a file.
    ''' </summary>
    ''' <param name="file">The file to load the move from.</param>
    Private Shared Sub LoadForm(ByVal file As String)
        Dim content() As String = System.IO.File.ReadAllLines(file)

        Dim line As String = ""

        Try
            'Go through lines of the file and set the properties depending on the content.
            'Lines starting with any other character than { (excluding tabs and spaces) are comments.
            For Each l As String In content
                Dim form As New PokemonForm()
                Dim setDexNumber As Boolean = False 'Controls if the form sets its DexNumber.

                If l.Contains("|") = True AndAlso l.StartsWith("{") = True AndAlso l.EndsWith("}") = True Then
                    line = l.Remove(l.Length - 1, 1).Remove(0, 1)
                    Dim arguments() As String = line.Split("|")

                    If arguments.Count >= 1 Then
                        If arguments(0) <> "" AndAlso StringHelper.IsNumeric(arguments(0)) Then
                            setDexNumber = True
                            form.DexNumber = CInt(arguments(0))
                        End If
                        If arguments.Count >= 2 Then
                            form.AdditionalValue = arguments(1)
                            If arguments.Count >= 3 Then
                                If arguments(2).Contains(",") Then
                                    Dim trigger() As String = arguments(2).Split(",")
                                    For i = 0 To trigger.Count - 1
                                        form.InPartyFormTriggers.Add(trigger(i))
                                    Next
                                Else
                                    form.InPartyFormTriggers.Add(arguments(2))
                                End If
                                If arguments.Count >= 4 Then
                                    form.FormNamePrefix = arguments(3)
                                    If arguments.Count >= 5 Then
                                        form.FormNameSuffix = arguments(4)
                                        If arguments.Count >= 6 Then
                                            form.DataFileSuffix = arguments(5)
                                            If arguments.Count >= 7 Then
                                                form.MenuIconFile = arguments(6).GetSplit(0, ",")
                                                Select Case arguments(6).Split(",").Count
                                                    Case 2
                                                        form.MenuIconPosition.X = CInt(arguments(6).GetSplit(1, ","))
                                                    Case 3
                                                        form.MenuIconPosition.X = CInt(arguments(6).GetSplit(1, ","))
                                                        form.MenuIconPosition.Y = CInt(arguments(6).GetSplit(2, ","))
                                                End Select
                                                If arguments.Count >= 8 Then
                                                    form.FrontBackSpriteFileSuffix = arguments(7)
                                                    If arguments.Count >= 9 Then
                                                        form.OverworldSpriteFileSuffix = arguments(8)
                                                        If arguments.Count >= 10 Then
                                                            form.CryFileSuffix = arguments(9)
                                                            If arguments.Count >= 11 Then
                                                                If arguments(10).Contains(",") Then
                                                                    For Each trigger In arguments(10).Split(",")
                                                                        form.WildFormTriggers.Add(trigger)
                                                                    Next
                                                                Else
                                                                    form.WildFormTriggers.Add(arguments(10))
                                                                End If
                                                                If arguments.Count >= 12 Then
                                                                    Select Case arguments(11).ToLower
                                                                        Case "normal"
                                                                            form.TypeChange = Element.Types.Normal
                                                                        Case "fighting"
                                                                            form.TypeChange = Element.Types.Fighting
                                                                        Case "flying"
                                                                            form.TypeChange = Element.Types.Flying
                                                                        Case "poison"
                                                                            form.TypeChange = Element.Types.Poison
                                                                        Case "ground"
                                                                            form.TypeChange = Element.Types.Ground
                                                                        Case "rock"
                                                                            form.TypeChange = Element.Types.Rock
                                                                        Case "bug"
                                                                            form.TypeChange = Element.Types.Bug
                                                                        Case "ghost"
                                                                            form.TypeChange = Element.Types.Ghost
                                                                        Case "steel"
                                                                            form.TypeChange = Element.Types.Steel
                                                                        Case "fire"
                                                                            form.TypeChange = Element.Types.Fire
                                                                        Case "water"
                                                                            form.TypeChange = Element.Types.Water
                                                                        Case "grass"
                                                                            form.TypeChange = Element.Types.Grass
                                                                        Case "electric"
                                                                            form.TypeChange = Element.Types.Electric
                                                                        Case "psychic"
                                                                            form.TypeChange = Element.Types.Psychic
                                                                        Case "ice"
                                                                            form.TypeChange = Element.Types.Ice
                                                                        Case "dragon"
                                                                            form.TypeChange = Element.Types.Dragon
                                                                        Case "dark"
                                                                            form.TypeChange = Element.Types.Dark
                                                                        Case "fairy"
                                                                            form.TypeChange = Element.Types.Fairy
                                                                        Case "shadow"
                                                                            form.TypeChange = Element.Types.Shadow
                                                                        Case Else
                                                                            form.TypeChange = Element.Types.Blank
                                                                    End Select
                                                                End If
                                                            End If
                                                        End If
                                                    End If
                                                End If
                                            End If
                                        End If
                                    End If
                                End If
                            End If
                        End If
                    End If
                    If setDexNumber = True Then
                        _pokemonList.Add(form)
                    Else
                        Debug.Print("PokemonForms.vb: A form needs to at least have a Dex Number set (the first value), however a form loaded from """ & file & """ has no Dex Number set so it will be ignored.")
                    End If
                End If
            Next
        Catch ex As Exception
            'If an error occurs loading a move, log the error.
            Logger.Log(Logger.LogTypes.ErrorMessage, "PokemonForms.vb: Error loading form from file """ & file & """: " & ex.Message & "; Last Key/Value pair successfully loaded: " & line)
        End Try

    End Sub

    ''' <summary>
    ''' Returns the initial Additional Data, if it needs to be set at generation time of the Pokémon.
    ''' </summary>
    Public Shared Function GetInitialAdditionalData(ByVal P As Pokemon) As String
        If _pokemonList.Count > 0 Then
            For Each listP In _pokemonList
                If listP.IsNumber(P.Number) = True Then
                    If listP.GetSeasonFormMatch(True) = "match" OrElse listP.GetGenderFormMatch(P, True) = "match" OrElse listP.GetEnvironmentFormMatch(True) = "match" OrElse (listP.GetSeasonFormMatch(True) = "" AndAlso listP.GetGenderFormMatch(P, True) = "" AndAlso listP.GetEnvironmentFormMatch(True) = "" AndAlso listP.ValueMatch(P.AdditionalData)) Then
                        Return listP.GetInitialAdditionalData(P)
                    End If
                End If
            Next
        End If

        Return ""
    End Function
    Public Shared Function GetFormDataInParty(ByVal P As Pokemon) As String
        If _pokemonList.Count > 0 Then
            For Each listP In _pokemonList
                If listP.IsNumber(P.Number) = True Then
                    If listP.GetSeasonFormMatch() = "match" OrElse listP.GetGenderFormMatch(P) = "match" OrElse listP.GetEnvironmentFormMatch() = "match" OrElse (listP.GetSeasonFormMatch() = "" AndAlso listP.GetGenderFormMatch(P) = "" AndAlso listP.GetEnvironmentFormMatch() = "" AndAlso listP.ValueMatch(P.AdditionalData)) Then
                        Return listP.GetFormDataInParty(P)
                    End If
                End If
            Next
        End If
        Return ""
    End Function

    Public Shared Function GetGenderFormMatch(ByVal P As Pokemon) As String
        If _pokemonList.Count > 0 Then
            For Each listP In _pokemonList
                If listP.IsNumber(P.Number) = True AndAlso listP.GetGenderFormMatch(P) = "match" Then
                    Return listP.GetGenderFormMatch(P)
                End If
            Next
        End If
        Return ""
    End Function
    Public Shared Function GetTypeAdditionFromItem(ByVal P As Pokemon) As String
        If _pokemonList.Count > 0 Then
            For Each listP In _pokemonList
                If listP.IsNumber(P.Number) = True Then
                    Return listP.GetTypeAdditionFromItem(P)
                End If
            Next
        End If
        Return ""
    End Function

    ''' <summary>
    ''' Returns the Animation Name of the Pokémon, the path to its Sprite/Model files.
    ''' </summary>
    Public Shared Function GetAnimationName(ByVal P As Pokemon) As String
        If _pokemonList.Count > 0 Then
            For Each listP In _pokemonList
                If listP.IsNumber(P.Number) = True Then
                    Dim TypeAddition As String = GetTypeAdditionFromItem(P)

                    If GetTypeAdditionFromItem(P) <> "" OrElse listP.GetSeasonFormMatch() = "match" OrElse listP.GetGenderFormMatch(P) = "match" OrElse listP.GetEnvironmentFormMatch() = "match" OrElse (listP.GetSeasonFormMatch() = "" AndAlso listP.GetGenderFormMatch(P) = "" AndAlso listP.GetEnvironmentFormMatch() = "" AndAlso listP.TypeChange = Element.Types.Blank AndAlso listP.ValueMatch(P.AdditionalData) = True) Then
                        Return listP.GetAnimationName(P).ToLower
                    End If
                End If
            Next
        End If

        Return CStr(P.Number)
    End Function

    ''' <summary>
    ''' Returns the Cry Suffix of the Pokémon.
    ''' </summary>
    Public Shared Function GetCrySuffix(ByVal P As Pokemon) As String
        If _pokemonList.Count > 0 Then
            For Each listP In _pokemonList
                If listP.IsNumber(P.Number) = True Then
                    If listP.GetSeasonFormMatch() = "match" OrElse listP.GetGenderFormMatch(P) = "match" OrElse listP.GetEnvironmentFormMatch() = "match" OrElse (listP.GetSeasonFormMatch() = "" AndAlso listP.GetGenderFormMatch(P) = "" AndAlso listP.GetEnvironmentFormMatch() = "" AndAlso listP.ValueMatch(P.AdditionalData)) Then
                        Return listP.GetCrySuffix(P)
                    End If
                End If
            Next
        End If
        Return ""
    End Function

    ''' <summary>
    ''' Returns the English Name of the Pokémon.
    ''' </summary>
    Public Shared Function GetFormName(ByVal P As Pokemon) As String
        If _pokemonList.Count > 0 Then
            For Each listP In _pokemonList
                If listP.IsNumber(P.Number) = True Then
                    If listP.GetSeasonFormMatch() = "match" OrElse listP.GetGenderFormMatch(P) = "match" OrElse listP.GetEnvironmentFormMatch() = "match" OrElse (listP.GetSeasonFormMatch() = "" AndAlso listP.GetGenderFormMatch(P) = "" AndAlso listP.GetEnvironmentFormMatch() = "" AndAlso listP.ValueMatch(P.AdditionalData)) Then
                        Return listP.GetFormName(P)
                    End If
                End If
            Next
        End If
        Return ""
    End Function
    Public Shared Function GetFrontBackSpriteFileSuffix(ByVal P As Pokemon) As String
        If _pokemonList.Count > 0 Then
            For Each listP In _pokemonList
                If listP.IsNumber(P.Number) = True Then
                    If GetTypeAdditionFromItem(P) <> "" OrElse listP.GetSeasonFormMatch() = "match" OrElse listP.GetGenderFormMatch(P) = "match" OrElse listP.GetEnvironmentFormMatch() = "match" OrElse (listP.GetSeasonFormMatch() = "" AndAlso listP.GetGenderFormMatch(P) = "" AndAlso listP.GetEnvironmentFormMatch() = "" AndAlso listP.TypeChange = Element.Types.Blank AndAlso listP.ValueMatch(P.AdditionalData) = True) Then
                        Return listP.GetFrontBackSpriteFileSuffix(P)
                    End If
                End If
            Next
        End If
        Return ""
    End Function

    ''' <summary>
    ''' Returns the name of spritesheet containing the Pokémon's menu sprite.
    ''' </summary>
    Public Shared Function GetSheetName(ByVal P As Pokemon) As String
        If _pokemonList.Count > 0 Then
            For Each listP In _pokemonList
                If listP.IsNumber(P.Number) = True Then
                    If GetTypeAdditionFromItem(P) <> "" OrElse listP.GetSeasonFormMatch() = "match" OrElse listP.GetGenderFormMatch(P) = "match" OrElse listP.GetEnvironmentFormMatch() = "match" OrElse (listP.GetSeasonFormMatch() = "" AndAlso listP.GetGenderFormMatch(P) = "" AndAlso listP.GetEnvironmentFormMatch() = "" AndAlso listP.TypeChange = Element.Types.Blank AndAlso listP.ValueMatch(P.AdditionalData) = True) Then
                        Return listP.GetSheetName(P)
                    End If
                End If
            Next
        End If
        Dim n As Integer = P.Number

        Select Case n
            Case 0 To 151
                Return "Gen1"
            Case 152 To 251
                Return "Gen2"
            Case 252 To 386
                Return "Gen3"
            Case 387 To 493
                Return "Gen4"
            Case 494 To 649
                Return "Gen5"
            Case 650 To 721
                Return "Gen6"
            Case 722 To 809
                Return "Gen7"
            Case Else
                Return "Gen8"
        End Select
    End Function

    ''' <summary>
    ''' Returns the grid coordinates of the Pokémon's menu sprite.
    ''' </summary>
    Public Shared Function GetMenuImagePosition(ByVal P As Pokemon) As Vector2
        If _pokemonList.Count > 0 Then
            For Each listP In _pokemonList
                If listP.IsNumber(P.Number) = True Then
                    If GetTypeAdditionFromItem(P) <> "" OrElse listP.GetSeasonFormMatch() = "match" OrElse listP.GetGenderFormMatch(P) = "match" OrElse listP.GetEnvironmentFormMatch() = "match" OrElse (listP.GetSeasonFormMatch() = "" AndAlso listP.GetGenderFormMatch(P) = "" AndAlso listP.GetEnvironmentFormMatch() = "" AndAlso listP.TypeChange = Element.Types.Blank AndAlso listP.ValueMatch(P.AdditionalData) = True) Then
                        Return listP.GetMenuImagePosition(P)
                    End If
                End If
            Next
        End If

        Dim x As Integer = 0
        Dim y As Integer = 0
        Dim n As Integer = P.Number
        Dim r As Integer = 0

        Select Case n
            Case 0 To 151
                r = n
            Case 152 To 251
                r = n - 151
            Case 252 To 386
                r = n - 251
            Case 387 To 493
                r = n - 386
            Case 494 To 649
                r = n - 493
            Case 650 To 721
                r = n - 649
            Case 722 To 809
                r = n - 721
            Case Else
                r = n - 809
        End Select

        While r > 16
            r -= 16
            y += 1
        End While
        x = r - 1
        Return New Vector2(x, y)
    End Function

    ''' <summary>
    ''' Returns the size of the Pokémon's menu sprite.
    ''' </summary>
    Public Shared Function GetMenuImageSize(ByVal P As Pokemon) As Size
        If _pokemonList.Count > 0 Then
            For Each listP In _pokemonList
                If listP.IsNumber(P.Number) = True AndAlso listP.ValueMatch(P.AdditionalData) = True Then
                    Return listP.GetMenuImageSize(P)
                End If
            Next
        End If
        Dim sheet As String = GetSheetName(P)
        Dim _size As Integer = CInt(TextureManager.GetTexture("GUI\PokemonMenu\" & sheet).Width / 32)
        Return New Size(_size, _size)
    End Function

    ''' <summary>
    ''' Returns the addition to the Pokémon's overworld sprite name.
    ''' </summary>
    Public Shared Function GetOverworldAddition(ByVal P As Pokemon) As String
        If _pokemonList.Count > 0 Then
            For Each listP In _pokemonList

                If listP.IsNumber(P.Number) = True Then
                    If GetTypeAdditionFromItem(P) <> "" OrElse listP.GetSeasonFormMatch() = "match" OrElse listP.GetGenderFormMatch(P) = "match" OrElse listP.GetEnvironmentFormMatch() = "match" OrElse (listP.GetSeasonFormMatch() = "" AndAlso listP.GetGenderFormMatch(P) = "" AndAlso listP.GetEnvironmentFormMatch() = "" AndAlso listP.TypeChange = Element.Types.Blank AndAlso listP.ValueMatch(P.AdditionalData) = True) Then
                        Return listP.GetOverworldAddition(P)
                    End If
                End If
            Next
        End If
        Return ""
    End Function

    ''' <summary>
    ''' Returns the path to the Pokémon's overworld sprite.
    ''' </summary>
    Public Shared Function GetOverworldSpriteName(ByVal P As Pokemon) As String
        Dim path As String = "Pokemon\Overworld\Normal\"
        If P.IsShiny = True Then
            path = "Pokemon\Overworld\Shiny\"
        End If
        path &= P.Number.ToString() & GetOverworldAddition(P)
        Return path
    End Function

    ''' <summary>
    ''' Returns the Pokémon's data file.
    ''' </summary>
    ''' <param name="Number">The number of the Pokémon.</param>
    ''' <param name="AdditionalData">The additional data of the Pokémon.</param>
    Public Shared Function GetPokemonDataFile(ByVal Number As Integer, ByVal AdditionalData As String) As String
        Dim FileName As String = GameModeManager.GetPokemonDataFilePath(Number.ToString() & ".dat")

        Dim Addition As String = ""

        If _pokemonList.Count > 0 Then
            For Each listP In _pokemonList
                If listP.IsNumber(Number) = True AndAlso listP.ValueMatch(AdditionalData) = True Then
                    Addition = listP.GetDataFileAddition(AdditionalData)
                End If
            Next
        End If
        If Addition <> "" Then
            FileName = FileName.Remove(FileName.Length - 4, 4) & Addition & ".dat"
        End If

        If System.IO.File.Exists(FileName) = False Then
            Number = 10
            FileName = GameModeManager.GetPokemonDataFilePath(Number.ToString() & ".dat")
        End If

        Return FileName
    End Function

    Public Shared Function GetPokemonDataFileName(ByVal Number As Integer, ByVal AdditionalData As String, Optional ByVal AlsoCheckNonDataForms As Boolean = False) As String
        Dim FileName As String = Number.ToString()
        Dim FilePath As String = GameModeManager.GetPokemonDataFilePath(FileName & ".dat")

        Dim Addition As String = ""

        If _pokemonList.Count > 0 Then
            For Each listP In _pokemonList
                If listP.IsNumber(Number) = True AndAlso listP.ValueMatch(AdditionalData) = True Then
                    Addition = listP.GetDataFileAddition(AdditionalData)
                End If
            Next
        End If
        If Addition <> "" Then
            FilePath = FilePath.Remove(FilePath.Length - 4, 4) & Addition & ".dat"
            FileName &= Addition
        End If

        If System.IO.File.Exists(FilePath) = False Then
            If System.IO.File.Exists(GameModeManager.GetPokemonDataFilePath(Number.ToString) & ".dat") = False Then
                FileName = "10"
            Else
                FileName = Number.ToString
            End If
        End If

        If Addition = "" AndAlso AdditionalData <> "" AndAlso AlsoCheckNonDataForms = True Then
            If PokemonForms.GetAdditionalDataForms(Number) IsNot Nothing AndAlso PokemonForms.GetAdditionalDataForms(Number).Contains(AdditionalData) Then
                FileName = Number.ToString & ";" & AdditionalData
            End If
        End If

        Return FileName
    End Function

    Public Shared Function GetAdditionalDataForms(ByVal Number As Integer) As List(Of String)
        Dim Forms As New List(Of String)

        If _pokemonList.Count > 0 Then
            For Each listP In _pokemonList
                If listP.IsNumber(Number) = True AndAlso listP.AdditionalValue <> "" AndAlso listP.DataFileSuffix = "" Then
                    Forms.Add(listP.AdditionalValue)
                End If
            Next
        End If

        If Forms.Count > 0 Then
            Return Forms
        End If

        Return Nothing

    End Function

    Public Shared Function GetAdditionalValueFromDataFile(ByVal DataFile As String) As String

        Dim CompareNumber As Integer = CInt(DataFile.GetSplit(0, "_"))
        Dim CompareSuffix As String = DataFile.Remove(0, DataFile.IndexOf("_"))
        If _pokemonList.Count > 0 Then
            For Each listP In _pokemonList
                If listP.IsNumber(CompareNumber) = True AndAlso CompareSuffix = listP.DataFileSuffix Then
                    Return listP.AdditionalValue
                End If
            Next
        End If

        Return ""
    End Function

    Public Shared Function GetDefaultOverworldSpriteAddition(ByVal Number As Integer) As String
        Return ""
    End Function

    Public Shared Function GetDefaultImageAddition(ByVal Number As Integer) As String
        Return ""
    End Function
#Region "PokemonForm"
    Private Class PokemonForm

        Public DexNumber As Integer = -1
        Public InPartyFormTriggers As New List(Of String)
        Public AdditionalValue As String = ""
        Public FormNamePrefix As String = ""
        Public FormNameSuffix As String = ""
        Public DataFileSuffix As String = ""
        Public MenuIconFile As String = ""
        Public MenuIconPosition As New Vector2(-1)
        Public FrontBackSpriteFileSuffix As String = ""
        Public OverworldSpriteFileSuffix As String = ""
        Public CryFileSuffix As String = ""
        Public WildFormTriggers As New List(Of String)
        Public TypeChange As Element.Types = Element.Types.Blank

        Public Overridable Function GetInitialAdditionalData(ByVal P As Pokemon) As String
            If WildFormTriggers.Count > 0 Then
                For i = 0 To WildFormTriggers.Count - 1
                    If WildFormTriggers(i).Contains(";") Then
                        Dim trigger() As String = WildFormTriggers(i).Split(";")
                        If trigger(0).ToLower = "random" Then
                            If trigger(1).Contains("-") Then
                                Dim randomMin As Integer = CInt(trigger(1).GetSplit(0, "-"))
                                Dim randomMax As Integer = CInt(trigger(1).GetSplit(1, "-"))
                                Return CStr(Core.Random.Next(randomMin, randomMax + 1))
                            Else
                                Dim triggercount As Integer = 0
                                For t = 1 To trigger.Count
                                    triggercount += 1
                                Next
                                Dim randomMin As Integer = 1
                                Dim randomMax As Integer = triggercount
                                Return CStr(trigger(Core.Random.Next(randomMin, randomMax)))
                            End If
                        ElseIf trigger(0).ToLower = "gender" Then
                            If GetGenderFormMatch(P, True) = "match" Then
                                If TypeChange <> Element.Types.Blank Then
                                    Return TypeChange.ToString
                                Else
                                    Return AdditionalValue
                                End If
                            End If
                        ElseIf trigger(0).ToLower = "season" Then
                            If GetSeasonFormMatch(true) = "match" Then
                                If TypeChange <> Element.Types.Blank Then
                                    Return TypeChange.ToString
                                Else
                                    Return AdditionalValue
                                End If
                            End If
                        ElseIf trigger(0).ToLower = "environment" Then
                            Dim environmentlist As New List(Of World.EnvironmentTypes)
                            For e = 1 To trigger.Count - 1
                                environmentlist.Add(CType(CInt(trigger(e)), World.EnvironmentTypes))
                            Next
                            If environmentlist.Contains(CType(Screen.Level.EnvironmentType, World.EnvironmentTypes)) Then
                                Return AdditionalValue
                            End If
                        End If
                    End If
                Next
            End If
            Return AdditionalValue

        End Function
        Public Overridable Function GetFormDataInParty(ByVal P As Pokemon) As String
            If InPartyFormTriggers.Count > 0 Then
                For i = 0 To InPartyFormTriggers.Count - 1
                    If InPartyFormTriggers(i).Contains(";") Then
                        Dim trigger() As String = InPartyFormTriggers(i).Split(";")
                        If trigger(0).ToLower = "gender" Then
                            If GetGenderFormMatch(P) = "match" Then
                                If TypeChange <> Element.Types.Blank Then
                                    Return TypeChange.ToString
                                Else
                                    Return AdditionalValue
                                End If
                            End If
                        ElseIf trigger(0).ToLower = "season" Then
                            If GetSeasonFormMatch() = "match" Then
                                If TypeChange <> Element.Types.Blank Then
                                    Return TypeChange.ToString
                                Else
                                    Return AdditionalValue
                                End If
                            End If
                        ElseIf trigger(0).ToLower = "environment" Then
                            Dim environmentlist As New List(Of World.EnvironmentTypes)
                            For e = 1 To trigger.Count - 1
                                environmentlist.Add(CType(CInt(trigger(e)), World.EnvironmentTypes))
                            Next
                            If environmentlist.Contains(CType(Screen.Level.EnvironmentType, World.EnvironmentTypes)) Then
                                If TypeChange <> Element.Types.Blank Then
                                    Return TypeChange.ToString
                                Else
                                    Return AdditionalValue
                                End If
                            End If
                        Else
                            Dim TypeAddition As String = GetTypeAdditionFromItem(P)
                            If TypeAddition <> "" Then
                                Return TypeAddition
                            Else
                                If trigger(0).ToLower = "item" Then
                                    If P.Item IsNot Nothing Then
                                        If P.Item.IsGameModeItem = False Then
                                            If P.Item.ID = CInt(trigger(1)) Then
                                                Return AdditionalValue
                                            End If
                                        Else
                                            If P.Item.gmID = trigger(1) Then
                                                Return AdditionalValue
                                            End If
                                        End If
                                    End If
                                End If
                            End If
                        End If
                    End If
                Next
            End If

            Return ""

        End Function
        Public Function GetTypeAdditionFromItem(ByVal P As Pokemon) As String
            If InPartyFormTriggers.Count > 0 Then
                For i = 0 To InPartyFormTriggers.Count - 1
                    Dim trigger() As String = InPartyFormTriggers(i).Split(";")
                    If trigger(0).ToLower = "item" Then
                        If P.Item IsNot Nothing Then
                            If P.Item.IsGameModeItem = False Then
                                If P.Item.ID = CInt(trigger(1)) Then
                                    If TypeChange <> Element.Types.Blank Then
                                        Return CStr("type;" & TypeChange.ToString)
                                    End If
                                End If
                            Else
                                If P.Item.gmID = trigger(1) Then
                                    If TypeChange <> Element.Types.Blank Then
                                        Return CStr("type;" & TypeChange.ToString)
                                    End If
                                End If
                            End If
                        End If
                    End If
                Next
            End If
            Return ""
        End Function
        Public Function GetSeasonFormMatch(Optional ByVal initial As Boolean = False) As String
            If initial = False Then
                If InPartyFormTriggers.Count > 0 Then
                    For i = 0 To InPartyFormTriggers.Count - 1
                        Dim trigger() As String = InPartyFormTriggers(i).Split(";")
                        If trigger(0).ToLower = "season" Then
                            If World.CurrentSeason = CType(CInt(trigger(1)), World.Seasons) Then
                                Return "match"
                            Else
                                Return "nomatch"
                            End If
                        End If
                    Next
                End If
            Else
                If WildFormTriggers.Count > 0 Then
                    For i = 0 To WildFormTriggers.Count - 1
                        Dim trigger() As String = WildFormTriggers(i).Split(";")
                        If trigger(0).ToLower = "season" Then
                            If World.CurrentSeason = CType(CInt(trigger(1)), World.Seasons) Then
                                Return "match"
                            Else
                                Return "nomatch"
                            End If
                        End If
                    Next
                End If
            End If
            Return ""
        End Function
        Public Function GetEnvironmentFormMatch(Optional ByVal initial As Boolean = False) As String
            If initial = False Then
                If InPartyFormTriggers.Count > 0 Then
                    For i = 0 To InPartyFormTriggers.Count - 1
                        Dim trigger() As String = InPartyFormTriggers(i).Split(";")
                        If trigger(0).ToLower = "environment" Then
                            Dim environmentlist As New List(Of World.EnvironmentTypes)
                            For e = 1 To trigger.Count - 1
                                environmentlist.Add(CType(CInt(trigger(e)), World.EnvironmentTypes))
                            Next
                            If environmentlist.Contains(CType(Screen.Level.EnvironmentType, World.EnvironmentTypes)) Then
                                Return "match"
                            Else
                                Return "nomatch"
                            End If
                        End If
                    Next
                End If
            Else
                If WildFormTriggers.Count > 0 Then
                    For i = 0 To WildFormTriggers.Count - 1
                        Dim trigger() As String = WildFormTriggers(i).Split(";")
                        If trigger(0).ToLower = "environment" Then
                            Dim environmentlist As New List(Of World.EnvironmentTypes)
                            For e = 1 To trigger.Count - 1
                                environmentlist.Add(CType(CInt(trigger(e)), World.EnvironmentTypes))
                            Next
                            If environmentlist.Contains(CType(Screen.Level.EnvironmentType, World.EnvironmentTypes)) Then
                                Return "match"
                            Else
                                Return "nomatch"
                            End If
                        End If
                    Next
                End If
            End If
            Return ""
        End Function
        Public Function GetGenderFormMatch(ByVal P As Pokemon, Optional ByVal initial As Boolean = False) As String
            If initial = False Then
                If InPartyFormTriggers.Count > 0 Then
                    For i = 0 To InPartyFormTriggers.Count - 1
                        Dim trigger() As String = InPartyFormTriggers(i).Split(";")
                        If trigger(0).ToLower = "gender" Then
                            If P.Gender = CType(CInt(trigger(1)), Pokemon.Genders) Then
                                Return "match"
                            Else
                                Return "nomatch"
                            End If
                        End If
                    Next
                End If
            Else
                If WildFormTriggers.Count > 0 Then
                    For i = 0 To WildFormTriggers.Count - 1
                        Dim trigger() As String = WildFormTriggers(i).Split(";")
                        If trigger(0).ToLower = "gender" Then
                            If P.Gender = CType(CInt(trigger(1)), Pokemon.Genders) Then
                                Return "match"
                            Else
                                Return "nomatch"
                            End If
                        End If
                    Next
                End If
            End If
            Return ""
        End Function
        Public Overridable Function GetAnimationName(ByVal P As Pokemon) As String
            Return CStr(P.Number & OverworldSpriteFileSuffix)
        End Function
        Public Overridable Function GetFormName(ByVal P As Pokemon) As String
            Return CStr(FormNamePrefix & P.Name & FormNameSuffix)
        End Function
        Public Overridable Function GetCrySuffix(ByVal P As Pokemon) As String
            Return CryFileSuffix
        End Function
        Public Overridable Function GetFrontBackSpriteFileSuffix(ByVal P As Pokemon) As String
            Return FrontBackSpriteFileSuffix
        End Function

        Public Overridable Function GetSheetName(ByVal P As Pokemon) As String
            Dim n As Integer = P.Number
            If MenuIconFile = "" Then
                Select Case n
                    Case 0 To 151
                        Return "Gen1"
                    Case 152 To 251
                        Return "Gen2"
                    Case 252 To 386
                        Return "Gen3"
                    Case 387 To 493
                        Return "Gen4"
                    Case 494 To 649
                        Return "Gen5"
                    Case 650 To 721
                        Return "Gen6"
                    Case 722 To 809
                        Return "Gen7"
                    Case Else
                        Return "Gen8"
                End Select
            Else
                Return MenuIconFile
            End If
        End Function

        Public Overridable Function GetMenuImagePosition(ByVal P As Pokemon) As Vector2
            Dim x As Integer = 0
            Dim y As Integer = 0
            Dim n As Integer = P.Number
            Dim r As Integer = 0
            Select Case n
                Case 0 To 151
                    r = n
                Case 152 To 251
                    r = n - 151
                Case 252 To 386
                    r = n - 251
                Case 387 To 493
                    r = n - 386
                Case 494 To 649
                    r = n - 493
                Case 650 To 721
                    r = n - 649
                Case 722 To 809
                    r = n - 721
                Case Else
                    r = n - 809
            End Select

            If CInt(MenuIconPosition.X) <> -1 Then
                r = CInt(MenuIconPosition.X)
            End If
            If CInt(MenuIconPosition.Y) <> -1 Then
                y = CInt(MenuIconPosition.Y)
            End If

            While r > 16
                r -= 16
                y += 1
            End While
            If CInt(MenuIconPosition.X) = -1 Then
                x = r - 1
            Else
                x = r
            End If
            Return New Vector2(x, y)

        End Function

        Public Overridable Function GetMenuImageSize(ByVal P As Pokemon) As Size
            Dim sheet As String = GetSheetName(P)
            Dim _size As Integer = CInt(TextureManager.GetTexture("GUI\PokemonMenu\" & sheet).Width / 32)
            Return New Size(_size, _size)
        End Function

        Public Overridable Function GetOverworldAddition(ByVal P As Pokemon) As String
            Return OverworldSpriteFileSuffix
        End Function

        Public Overridable Function GetDataFileAddition(ByVal AdditionalData As String) As String
            If Me.ValueMatch(AdditionalData) Then
                Return DataFileSuffix
            End If
            Return ""
        End Function

        Public Function IsNumber(ByVal number As Integer) As Boolean
            Return Me.DexNumber = number
        End Function
        Public Function ValueMatch(ByVal additionalValue As String) As Boolean
            Return Me.AdditionalValue = additionalValue
        End Function

    End Class

#End Region


End Class
