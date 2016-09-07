Public Class Pokedex

    '0 = undiscovered
    '1 = seen
    '2 = caught + seen
    '3 = shiny + caught + seen

    Public Shared AutoDetect As Boolean = True
    Public Const POKEMONCOUNT As Integer = 721

#Region "PlayerData"

    Public Shared Function CountEntries(ByVal Data As String, ByVal Type() As Integer) As Integer
        Dim counts As Integer = 0

        Dim pData() As String = Data.Split(CChar(vbNewLine))
        For Each Entry As String In pData
            Entry = Entry.Remove(0, Entry.IndexOf("{") + 1)
            Entry = Entry.Remove(Entry.Length - 1, 1)

            Dim eID As Integer = CInt(Entry.GetSplit(0, "|"))
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
            If IsNumeric(r) = True Then
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

        Dim pData() As String = Data.Split(CChar(vbNewLine))
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

    Public Shared Function GetEntryType(ByVal Data As String, ByVal ID As Integer) As Integer
        Dim pData() As String = Data.Split(CChar(vbNewLine))

        If pData.Count >= ID Then
            If pData(ID - 1).Contains(ID.ToString() & "|") = True Then
                Dim Entry As String = pData(ID - 1)
                Return CInt(Entry.Remove(Entry.Length - 1, 1).Remove(0, Entry.IndexOf("|") + 1))
            End If
        End If

        For Each Entry As String In pData
            If Entry.Contains(ID.ToString() & "|") = True Then
                Return CInt(Entry.Remove(Entry.Length - 1, 1).Remove(0, Entry.IndexOf("|") + 1))
            End If
        Next

        Return 0
    End Function

    Public Shared Function ChangeEntry(ByVal Data As String, ByVal ID As Integer, ByVal Type As Integer) As String
        If Type = 0 Or AutoDetect = True Then
            If Data.Contains("{" & ID & "|") = True Then
                Dim cEntry As Integer = GetEntryType(Data, ID)
                If cEntry < Type Then
                    Return Data.Replace("{" & ID & "|" & cEntry & "}", "{" & ID & "|" & Type & "}")
                Else
                    Return Data
                End If
            Else
                If Data <> "" Then
                    Data &= vbNewLine
                End If
                Data &= "{" & ID & "|" & Type & "}"
                Return Data
            End If
        End If
        Return Data
    End Function

    Public Shared Function NewPokedex() As String
        Dim Data As String = ""

        For i = 1 To POKEMONCOUNT
            Data &= "{" & i & "|0}"
            If i <> POKEMONCOUNT Then
                Data &= vbNewLine
            End If
        Next

        Return Data
    End Function

    Public Shared Function GetLastSeen(ByVal Data As String) As Integer
        Dim pData() As String = Data.Split(CChar(vbNewLine))
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
        If Pokemon.IsShiny = True Then
            Return ChangeEntry(Data, Pokemon.Number, 3)
        Else
            Return ChangeEntry(Data, Pokemon.Number, 2)
        End If
    End Function

#End Region

#Region "PokedexHandler"

    'The Pokedex screen changes the PokemonList array to add Pokémon not in the array, so this will get used to count things when focussing on the Pokémon in this dex.
    Private _originalPokemonList As New Dictionary(Of Integer, Integer)

    'Fields:
    Public PokemonList As New Dictionary(Of Integer, Integer)
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
            l = l.Replace("[MAX]", POKEMONCOUNT.ToString())

            If l.Contains("-") = True Then
                Dim range() As String = l.Split(CChar("-"))
                Dim min As Integer = CInt(range(0))
                Dim max As Integer = CInt(range(1))

                For j = min To max
                    PokemonList.Add(Place, j)
                    _originalPokemonList.Add(Place, j)

                    Place += 1
                Next
            Else
                PokemonList.Add(Place, CInt(l))
                _originalPokemonList.Add(Place, CInt(l))

                Place += 1
            End If
        Next

        If data.Length >= 4 Then
            Me.IncludeExternalPokemon = CBool(data(3))
        End If

        Me.OriginalCount = Me.PokemonList.Count
    End Sub

    Dim TempPlaces As New Dictionary(Of Integer, Integer)

    Public Function GetPlace(ByVal PokemonNumber As Integer) As Integer
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

    Public Function GetPokemonNumber(ByVal Place As Integer) As Integer
        If PokemonList.ContainsKey(Place) = True Then
            Return PokemonList(Place)
        End If
        Return -1
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
            For Each v As Integer In _originalPokemonList.Values
                If GetEntryType(Core.Player.PokedexData, v) > 1 Then
                    o += 1
                End If
            Next
            Return o
        End Get
    End Property

    Public ReadOnly Property Seen() As Integer
        Get
            Dim o As Integer = 0
            For Each v As Integer In _originalPokemonList.Values
                If GetEntryType(Core.Player.PokedexData, v) = 1 Then
                    o += 1
                End If
            Next
            Return o
        End Get
    End Property

    Public ReadOnly Property Count() As Integer
        Get
            Return Me._originalPokemonList.Keys.Count
        End Get
    End Property

    Public ReadOnly Property HasPokemon(ByVal pokemonNumber As Integer, ByVal originalList As Boolean) As Boolean
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