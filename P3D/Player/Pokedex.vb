Public Class Pokedex

    '0 = undiscovered
    '1 = seen
    '2 = caught + seen
    '3 = shiny + caught + seen

    Public Shared AutoDetect As Boolean = True
    Public Shared PokemonMaxCount As Integer = 1010
    Public Shared PokemonCount As Integer = 1010
    Public Shared PokemonIDs As New List(Of String)

#Region "PlayerData"

    Public Shared Function CountEntries(ByVal Data As String, ByVal Type() As Integer) As Integer
        Dim counts As Integer = 0

        Dim pData() As String = Data.Split(CChar(Environment.NewLine))
        For Each Entry As String In pData
            Entry = Entry.Remove(0, Entry.IndexOf("{") + 1)
            Entry = Entry.Remove(Entry.Length - 1, 1)

            Dim eID As String = Entry.GetSplit(0, "|")
            Dim eType As Integer = CInt(Entry.GetSplit(1, "|"))

            If Type.Contains(eType) = True Then
                counts += 1
            End If
        Next

        Return counts
    End Function

    Public Shared Function CountEntries(ByVal Data As String, ByVal Type() As Integer, ByVal Range() As String) As Integer
        Dim IDs As New List(Of Integer)

        For Each r As String In Range
            If StringHelper.IsNumeric(r) = True Then
                If IDs.Contains(CInt(r)) = False Then
                    IDs.Add(CInt(r))
                End If
            Else
                If r.Contains("-") = True Then
                    Dim min As Integer = CInt(r.Remove(r.IndexOf("-")))
                    Dim max As Integer = CInt(r.Remove(0, r.IndexOf("-") + 1))

                    For i = min To max
                        If IDs.Contains(i) = False Then
                            IDs.Add(i)
                        End If
                    Next
                End If
            End If
        Next

        Dim counts As Integer = 0

        Dim pData() As String = Data.Split(CChar(Environment.NewLine))
        For Each Entry As String In pData
            Entry = Entry.Remove(0, Entry.IndexOf("{") + 1)
            Entry = Entry.Remove(Entry.Length - 1, 1)

            Dim eID As Integer = CInt(Entry.GetSplit(0, "|"))
            Dim eType As Integer = CInt(Entry.GetSplit(1, "|"))

            If IDs.Contains(eID) = True Then
                If Type.Contains(eType) = True Then
                    counts += 1
                End If
            End If
        Next

        Return counts
    End Function

    Public Shared Function GetEntryType(ByVal Data As String, ByVal ID As String) As Integer
        Dim pData() As String = Data.Split(CChar(Environment.NewLine))

        For Each Entry As String In pData
            If Entry.Contains(ID & "|") = True Then
                Return CInt(Entry.Remove(Entry.Length - 1, 1).Remove(0, Entry.IndexOf("|") + 1))
            End If
        Next

        Return 0
    End Function

    Public Shared Function ChangeEntry(ByVal Data As String, ByVal ID As String, ByVal Type As Integer, Optional ForceChange As Boolean = False) As String
        If ForceChange = True OrElse Type = 0 Or AutoDetect = True Then
            If Data.Contains("{" & ID & "|") = True Then
                Dim cOriginalEntry As String = ""
                If ID.Contains(";") Then
                    cOriginalEntry = GetEntryType(Data, ID.GetSplit(0, ";")).ToString
                End If

                If ID.Contains("_") Then
                    If Pokemon.GetPokemonByID(CInt(ID.GetSplit(0, "_").GetSplit(0, ";"))).DexForms.Contains(ID.GetSplit(1, "_")) Then
                        cOriginalEntry = GetEntryType(Data, ID.GetSplit(0, "_").GetSplit(0, ";")).ToString
                    End If
                End If

                Dim cEntry As Integer = GetEntryType(Data, ID)
                Dim cData As String = Data
                If cOriginalEntry <> "" Then
                    If CInt(cOriginalEntry) < Type Then
                        If Data.Contains("{" & ID.GetSplit(0, ";").GetSplit(0, "_") & "|") = False Then
                            cData &= Environment.NewLine & "{" & ID.GetSplit(0, ";").GetSplit(0, "_") & "|" & 0 & "}"
                        End If
                    End If
                End If

                If ForceChange = True OrElse cEntry < Type Then
                    Return cData.Replace("{" & ID & "|" & cEntry & "}", "{" & ID & "|" & Type & "}")
                Else
                    Return cData
                End If
            Else
                Dim cData As String = Data
                If cData <> "" Then
                    cData &= Environment.NewLine
                End If

                If ID.Contains("_") Then
                    If Pokemon.GetPokemonByID(CInt(ID.GetSplit(0, "_").GetSplit(0, ";"))).DexForms.Contains(ID.GetSplit(1, "_")) Then
                        If cData.Contains("{" & ID.GetSplit(0, "_").GetSplit(0, ";") & "|") = False Then
                            cData &= "{" & ID.GetSplit(0, "_").GetSplit(0, ";") & "|" & 0 & "}" & Environment.NewLine
                        End If
                    End If
                End If

                If ID.Contains(";") Then
                    If cData.Contains("{" & ID.GetSplit(0, ";") & "|") = False Then
                        cData &= "{" & ID.GetSplit(0, ";") & "|" & 0 & "}" & Environment.NewLine
                    End If
                End If
                cData &= "{" & ID & "|" & Type & "}"
                Return cData
            End If
        End If
        Return Data
    End Function

    Public Shared Function NewPokedex() As String
        Dim Data As String = ""
        Dim IDs As New List(Of String)
        For Each file As String In System.IO.Directory.GetFiles(GameController.GamePath & "\" & GameModeManager.ActiveGameMode.ContentPath & "Pokemon\Data\", "*.dat")
            Dim id As String = file.Remove(file.Length - 4, 4).Remove(0, CStr(GameController.GamePath & "\" & GameModeManager.ActiveGameMode.ContentPath & "Pokemon\Data\").Length)
            If id.Contains("_") Then
                While id.GetSplit(0, "_").Length < 3
                    id = 0 & id
                End While
            End If
            IDs.Add(id)
        Next

        Dim formIDs As New List(Of String)
        For Each id As String In IDs
            If id.Contains("_") = False Then
                Dim baseID As String = id.GetSplit(0, "_")
                While baseID.StartsWith("0")
                    baseID = baseID.Remove(0, 1)
                End While

                Dim AdditionalDataForms As List(Of String) = PokemonForms.GetAdditionalDataForms(CInt(baseID))
                If AdditionalDataForms IsNot Nothing Then
                    For i = 0 To AdditionalDataForms.Count - 1
                        formIDs.Add(baseID & ";" & AdditionalDataForms(i))
                    Next
                End If
            End If
        Next
        IDs.AddRange(formIDs)

        PokemonCount = IDs.Count
        PokemonIDs = (From id In IDs Order By CInt(id.GetSplit(0, "_").GetSplit(0, ";"))).ToList()

        For i = 0 To PokemonCount - 1
            Dim entry As String = PokemonIDs(i)
            While entry.StartsWith("0")
                entry = entry.Remove(0, 1)
            End While

            Data &= "{" & entry & "|0}"
            If i <> PokemonCount - 1 Then
                Data &= Environment.NewLine
            End If
        Next

        Return Data
    End Function

    Public Shared Function GetLastSeen(ByVal Data As String) As Integer
        Dim pData() As String = Data.Split(CChar(Environment.NewLine))
        Dim lastSeen As Integer = 1

        For Each Entry As String In pData
            Entry = Entry.Remove(0, Entry.IndexOf("{") + 1)
            Entry = Entry.Remove(Entry.Length - 1, 1)

            Dim eID As Integer = CInt(Entry.GetSplit(0, "|"))
            Dim eType As Integer = CInt(Entry.GetSplit(1, "|"))

            If eType > 0 Then
                lastSeen = eID
            End If
        Next

        Return lastSeen
    End Function

    Public Shared Sub Load()
        Core.Player.Pokedexes.Clear()

        Dim path As String = GameModeManager.GetContentFilePath("Data\pokedex.dat")
        Security.FileValidation.CheckFileValid(path, False, "Pokedex.vb")

        Dim lines() As String = System.IO.File.ReadAllLines(path)
        For Each PokedexData As String In lines
            Core.Player.Pokedexes.Add(New Pokedex(PokedexData))
        Next
    End Sub

    Public Shared Function RegisterPokemon(ByVal Data As String, ByVal Pokemon As Pokemon) As String
        Dim dexID As String = PokemonForms.GetPokemonDataFileName(Pokemon.Number, Pokemon.AdditionalData)
        If dexID.Contains("_") = False Then
            If PokemonForms.GetAdditionalDataForms(Pokemon.Number) IsNot Nothing AndAlso PokemonForms.GetAdditionalDataForms(Pokemon.Number).Contains(Pokemon.AdditionalData) Then
                dexID = Pokemon.Number & ";" & Pokemon.AdditionalData
            Else
                dexID = Pokemon.Number.ToString
            End If
        End If

        If Pokemon.IsShiny = True Then
            Return ChangeEntry(Data, dexID, 3)
        Else
            Return ChangeEntry(Data, dexID, 2)
        End If
    End Function

#End Region

#Region "PokedexHandler"

    'The Pokedex screen changes the PokemonList array to add Pokémon not in the array, so this will get used to count things when focussing on the Pokémon in this dex.
    Private _originalPokemonList As New Dictionary(Of Integer, String)

    'Fields:
    Public PokemonList As New Dictionary(Of Integer, String)
    Public Name As String = ""
    Public Activation As String = ""
    Public OriginalCount As Integer = 0
    Public IncludeExternalPokemon As Boolean = False 'for the pokedex screen, if true, this pokedex view will include all Pokémon seen/caught at the end.

    Public Sub New(ByVal input As String)

        Dim data() As String = input.Split(CChar("|"))

        Me.Name = data(0)
        Me.Activation = data(1)

        Dim pokemonData() As String = data(2).Split(CChar(","))

        Dim Place As Integer = 1

        For Each l As String In pokemonData
            l = l.Replace("[MAX]", PokemonMaxCount.ToString())

            If l.Contains("-") = True AndAlso l.Contains("_") = False Then
                Dim range() As String = l.Split(CChar("-"))
                Dim min As Integer = CInt(range(0))
                Dim max As Integer = CInt(range(1))

                For j = min To max
                    PokemonList.Add(Place, j.ToString)
                    _originalPokemonList.Add(Place, j.ToString)

                    Place += 1
                Next
            Else
                PokemonList.Add(Place, l)
                _originalPokemonList.Add(Place, l)

                Place += 1
            End If
        Next

        If data.Length >= 4 Then
            Me.IncludeExternalPokemon = CBool(data(3))
        End If

        Me.OriginalCount = Me.PokemonList.Count
    End Sub

    Dim TempPlaces As New Dictionary(Of String, Integer)

    Public Function GetPlace(ByVal PokemonNumber As String) As Integer
        If TempPlaces.ContainsKey(PokemonNumber) = True Then
            Return TempPlaces(PokemonNumber)
        End If

        If PokemonList.ContainsValue(PokemonNumber) = True Then
            For i = 0 To PokemonList.Values.Count - 1
                If PokemonList.Values(i) = PokemonNumber Then
                    TempPlaces.Add(PokemonNumber, PokemonList.Keys(i))
                    Return PokemonList.Keys(i)
                End If
            Next
        End If

        Return -1
    End Function

    Public Function GetPokemonNumber(ByVal Place As Integer) As String
        If PokemonList.ContainsKey(Place) = True Then
            Return PokemonList(Place)
        End If
        Return "-1"
    End Function

    Public ReadOnly Property IsActivated() As Boolean
        Get
            If Me.Activation = "0" Then
                Return True
            Else
                If ActionScript.IsRegistered(Me.Activation) = True Then
                    Return True
                End If
            End If
            Return False
        End Get
    End Property

    Public ReadOnly Property Obtained() As Integer
        Get
            Dim o As Integer = 0
            For Each v As String In _originalPokemonList.Values
                If GetEntryType(Core.Player.PokedexData, v) > 1 Then
                    o += 1
                Else
                    If v.Contains("_") = False AndAlso v.Contains(";") = False Then
                        Dim Forms As List(Of String) = PokemonForms.GetCountForms(CInt(v))
                        Dim addCount As Boolean = False
                        If Forms IsNot Nothing Then
                            For f = 0 To Forms.Count - 1
                                If GetEntryType(Core.Player.PokedexData, Forms(f)) > 1 Then
                                    addCount = True
                                End If
                            Next
                        End If
                        If addCount = True Then
                            o += 1
                        End If
                    End If
                End If
            Next
            Return o
        End Get
    End Property

    Public ReadOnly Property Seen() As Integer
        Get
            Dim o As Integer = 0
            For Each v As String In _originalPokemonList.Values
                If GetEntryType(Core.Player.PokedexData, v) = 1 Then
                    o += 1
                Else
                    If v.Contains("_") = False AndAlso v.Contains(";") = False Then
                        Dim Forms As List(Of String) = PokemonForms.GetCountForms(CInt(v))
                        Dim addCount As Boolean = False
                        If Forms IsNot Nothing Then
                            For f = 0 To Forms.Count - 1
                                If GetEntryType(Core.Player.PokedexData, Forms(f)) = 1 Then
                                    addCount = True
                                End If
                            Next
                            If addCount = True Then
                                o += 1
                            End If
                        End If
                    End If
                End If
            Next
            Return o
        End Get
    End Property

    Public Shared Function HasAnyForm(ID As Integer) As Integer
        If GetEntryType(Core.Player.PokedexData, ID.ToString) > 0 Then
            Return GetEntryType(Core.Player.PokedexData, ID.ToString)
        Else
            Dim p As Pokemon = Pokemon.GetPokemonByID(ID, "", True)
            Dim EntryType As Integer = 0
            If p.DexForms.Count > 0 AndAlso p.DexForms(0) <> " " Then
                For i = 0 To p.DexForms.Count - 1
                    Dim pAD As String = PokemonForms.GetAdditionalValueFromDataFile(CStr(p.Number & "_" & p.DexForms(i)))

                    If pAD <> "" Then
                        If GetEntryType(Core.Player.PokedexData, CStr(p.Number & "_" & p.DexForms(i))) > 0 Then
                            Dim Forms As List(Of String) = PokemonForms.GetCountForms(CInt(p.Number))
                            If Forms IsNot Nothing Then
                                If Forms.Contains(CStr(p.Number & "_" & p.DexForms(i))) = True Then
                                    If GetEntryType(Core.Player.PokedexData, CStr(p.Number & "_" & p.DexForms(i))) > EntryType Then
                                        EntryType = GetEntryType(Core.Player.PokedexData, CStr(p.Number & "_" & p.DexForms(i)))
                                    End If
                                Else
                                    If 1 > EntryType Then
                                        EntryType = 1
                                    End If
                                End If
                            Else
                                If 1 > EntryType Then
                                    EntryType = 1
                                End If
                            End If
                        End If
                    End If
                Next
            Else
                Dim ADlist As List(Of String) = PokemonForms.GetAdditionalDataForms(p.Number)
                If ADlist IsNot Nothing AndAlso ADlist.Count > 0 Then
                    For i = 0 To ADlist.Count - 1
                        If GetEntryType(Core.Player.PokedexData, CStr(p.Number & ";" & ADlist(i))) > 0 Then
                            Dim Forms As List(Of String) = PokemonForms.GetCountForms(CInt(p.Number))
                            If Forms IsNot Nothing Then
                                If Forms.Contains(CStr(p.Number & ";" & ADlist(i))) = True Then
                                    If GetEntryType(Core.Player.PokedexData, CStr(p.Number & ";" & ADlist(i))) > EntryType Then
                                        EntryType = GetEntryType(Core.Player.PokedexData, CStr(p.Number & ";" & ADlist(i)))
                                    End If
                                Else
                                    If 1 > EntryType Then
                                        EntryType = 1
                                    End If
                                End If
                            Else
                                If 1 > EntryType Then
                                    EntryType = 1
                                End If
                            End If
                        End If
                    Next
                End If
            End If
            Return EntryType
        End If

    End Function

    Public ReadOnly Property Count() As Integer
        Get
            Return Me._originalPokemonList.Keys.Count
        End Get
    End Property

    Public ReadOnly Property HasPokemon(ByVal pokemonNumber As String, ByVal originalList As Boolean) As Boolean
        Get
            If originalList = True Then
                Return _originalPokemonList.ContainsValue(pokemonNumber)
            Else
                Return PokemonList.ContainsValue(pokemonNumber)
            End If
        End Get
    End Property

#End Region

End Class