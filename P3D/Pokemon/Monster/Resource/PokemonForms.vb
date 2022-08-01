Imports P3D

Public Class PokemonForms

    Private Shared _pokemonList As New List(Of PokemonForm)

    Public Shared Sub Initialize()
        _pokemonList.Clear()
        _pokemonList.AddRange({New Venusaur(), New Charizard(), New Blastoise(), New Beedrill(), New Pidgeot(), New Nidoran(), New Alakazam(), New Slowbro(), New Gengar(), New Kangaskhan(), New Pinsir(), New Gyarados(), New Aerodactyl(), New Mewtwo(),
                               New Pichu(), New Unown(), New Ampharos(), New Steelix(), New Scizor(), New Heracross(), New Houndoom(), New Tyranitar(),
                               New Sceptile(), New Blaziken(), New Swampert(), New Gardevoir(), New Sableye(), New Mawile(), New Aggron(), New Medicham(), New Manectric(), New Sharpedo(), New Camerupt(), New Altaria(), New Banette(), New Absol(), New Glalie(), New Salamence(), New Metagross(), New Latias(), New Latios(), New Kyogre(), New Groudon(), New Rayquaza(), New Deoxys(),
                               New Burmy(), New Shellos(), New Gastrodon(), New Lopunny(), New Garchomp(), New Lucario(), New Abomasnow(), New Gallade(), New Rotom(), New Dialga(), New Arceus(),
                               New Audino(), New Basculin(), New Deerling(), New Sawsbuck(), New Frillish(), New Jellicent(), New Tornadus(), New Thundurus(), New Landorus(), New Kyurem(),
                               New Vivillon(), New Pyroar(), New Flabebe(), New Floette(), New Florges(), New Aegislash(), New Diancie(), New Hoopa(),
                               New Rattata(), New Raticate(), New Raichu(), New Sandshrew(), New Sandslash(), New Vulpix(), New Ninetales(), New Diglett(), New Dugtrio(), New Meowth(), New Persian(), New Geodude(), New Graveler(), New Golem(), New Grimer(), New Muk(), New Exeggutor(), New Marowak(),
                               New Ponyta(), New Rapidash(), New Slowpoke(), New Farfetch(), New Weezing(), New MrMime(), New Articuno(), New Zapdos(), New Moltres(), New Slowking(), New Corsola(), New Zigzagoon(), New Linoone(), New Darumaka(), New Darmanitan(), New Yamask(), New Stunfisk(),
                               New Growlithe(), New Arcanine(), New Voltorb(), New Electrode(), New Typhlosion(), New Qwilfish(), New Sneasel(), New Samurott(), New Lilligant(), New Zorua(), New Zoroark(), New Braviary(), New Sliggoo(), New Goodra(), New Avalugg(), New Decidueye()})

    End Sub

    ''' <summary>
    ''' Returns the initial Additional Data, if it needs to be set at generation time of the Pokémon.
    ''' </summary>
    Public Shared Function GetInitialAdditionalData(ByVal P As Pokemon) As String
        For Each listP In _pokemonList
            If listP.IsNumber(P.Number) = True Then
                Return listP.GetInitialAdditionalData(P)
            End If
        Next

        Return ""
    End Function

    ''' <summary>
    ''' Returns the Animation Name of the Pokémon, the path to its Sprite/Model files.
    ''' </summary>
    Public Shared Function GetAnimationName(ByVal P As Pokemon) As String
        For Each listP In _pokemonList
            If listP.IsNumber(P.Number) = True Then
                Dim _name As String = listP.GetAnimationName(P).ToLower()
                If _name.StartsWith("mega ") Then
                    _name = _name.Remove(0, 5)
                    If _name.EndsWith(" x_mega_x") OrElse _name.EndsWith(" y_mega_y") Then
                        _name = _name.Remove(_name.Length - 9, 2)
                    End If
                ElseIf _name.StartsWith("primal ") Then
                    _name = _name.Remove(0, 7)
                End If
                Return _name
            End If
        Next

        Return P.OriginalName
    End Function

    ''' <summary>
    ''' Returns the name of spritesheet containing the Pokémon's menu sprite.
    ''' </summary>
    Public Shared Function GetSheetName(ByVal P As Pokemon) As String
        For Each listP In _pokemonList
            If listP.IsNumber(P.Number) = True Then
                Return listP.GetSheetName(P)
            End If
        Next

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
        For Each listP In _pokemonList
            If listP.IsNumber(P.Number) = True Then
                Return listP.GetMenuImagePosition(P)
            End If
        Next

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
        For Each listP In _pokemonList
            If listP.IsNumber(P.Number) = True Then
                Return listP.GetMenuImageSize(P)
            End If
        Next
        Dim sheet As String = GetSheetName(P)
        Dim _size As Integer = CInt(TextureManager.GetTexture("GUI\PokemonMenu\" & sheet).Width / 32)
        Return New Size(_size, _size)
    End Function

    ''' <summary>
    ''' Returns the addition to the Pokémon's overworld sprite name.
    ''' </summary>
    Public Shared Function GetOverworldAddition(ByVal P As Pokemon) As String
        For Each listP In _pokemonList
            If listP.IsNumber(P.Number) = True Then
                Return listP.GetOverworldAddition(P)
            End If
        Next

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

        For Each listP In _pokemonList
            If listP.IsNumber(Number) = True Then
                Addition = listP.GetDataFileAddition(AdditionalData)
            End If
        Next

        If Addition <> "" Then
            FileName = FileName.Remove(FileName.Length - 4, 4) & Addition & ".dat"
        End If

        If System.IO.File.Exists(FileName) = False Then
            Number = 10
            FileName = GameModeManager.GetPokemonDataFilePath(Number.ToString() & ".dat")
        End If

        Return FileName
    End Function

    Public Shared Function GetDefaultOverworldSpriteAddition(ByVal Number As Integer) As String
        Return ""
    End Function

    Public Shared Function GetDefaultImageAddition(ByVal Number As Integer) As String
        Return ""
    End Function

    Private MustInherit Class PokemonForm

        Private _numbers As New List(Of Integer)

        Public Sub New(ByVal Number As Integer)
            Me._numbers.Add(Number)
        End Sub

        Public Sub New(ByVal Numbers() As Integer)
            Me._numbers.AddRange(Numbers)
        End Sub

        Public Overridable Function GetInitialAdditionalData(ByVal P As Pokemon) As String
            Return ""
        End Function

        Public Overridable Function GetAnimationName(ByVal P As Pokemon) As String
            Return P.OriginalName
        End Function

        Public Overridable Function GetSheetName(ByVal P As Pokemon) As String
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

            While r > 16
                r -= 16
                y += 1
            End While
            x = r - 1
            Return New Vector2(x, y)
        End Function

        Public Overridable Function GetMenuImageSize(ByVal P As Pokemon) As Size
            Dim sheet As String = GetSheetName(P)
            Dim _size As Integer = CInt(TextureManager.GetTexture("GUI\PokemonMenu\" & sheet).Width / 32)
            Return New Size(_size, _size)
        End Function

        Public Overridable Function GetOverworldAddition(ByVal P As Pokemon) As String
            Return ""
        End Function

        Public Overridable Function GetDataFileAddition(ByVal AdditionalData As String) As String
            Return ""
        End Function

        Public Function IsNumber(ByVal number As Integer) As Boolean
            Return Me._numbers.Contains(number)
        End Function

    End Class

#Region "Classes"

#Region "Megas"

    Private Class Venusaur
        Inherits PokemonForm
        Public Sub New()
            MyBase.New(3)
        End Sub
        Public Overrides Function GetSheetName(P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return "Megas"
                Case Else
                    Return MyBase.GetSheetName(P)
            End Select
        End Function
        Public Overrides Function GetMenuImagePosition(ByVal P As Pokemon) As Vector2
            Select Case P.AdditionalData
                Case "mega"
                    Return New Vector2(0, 0)
                Case Else
                    Return MyBase.GetMenuImagePosition(P)
            End Select
        End Function
        Public Overrides Function GetDataFileAddition(ByVal AdditionalData As String) As String
            Select Case AdditionalData.ToLower()
                Case "mega"
                    Return "_mega"
                Case Else
                    Return ""
            End Select
        End Function
        Public Overrides Function GetAnimationName(ByVal P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return P.OriginalName & "_mega"
                Case Else
                    Return P.OriginalName
            End Select
        End Function

        Public Overrides Function GetOverworldAddition(ByVal P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return "_mega"
                Case Else
                    Return ""
            End Select
        End Function
    End Class

    Private Class Charizard
        Inherits PokemonForm
        Public Sub New()
            MyBase.New(6)
        End Sub
        Public Overrides Function GetSheetName(P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega_x", "mega_y"
                    Return "Megas"
                Case Else
                    Return MyBase.GetSheetName(P)
            End Select
        End Function
        Public Overrides Function GetMenuImagePosition(ByVal P As Pokemon) As Vector2
            Select Case P.AdditionalData
                Case "mega_x"
                    Return New Vector2(1, 0)
                Case "mega_y"
                    Return New Vector2(2, 0)
                Case Else
                    Return MyBase.GetMenuImagePosition(P)
            End Select
        End Function
        Public Overrides Function GetDataFileAddition(ByVal AdditionalData As String) As String
            Select Case AdditionalData.ToLower()
                Case "mega_x"
                    Return "_mega_x"
                Case "mega_y"
                    Return "_mega_y"
                Case Else
                    Return ""
            End Select
        End Function
        Public Overrides Function GetAnimationName(ByVal P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega_x"
                    Return P.OriginalName & "_mega_x"
                Case "mega_y"
                    Return P.OriginalName & "_mega_y"
                Case Else
                    Return P.OriginalName
            End Select
        End Function

        Public Overrides Function GetOverworldAddition(ByVal P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega_x"
                    Return "_mega_x"
                Case "mega_y"
                    Return "_mega_y"
                Case Else
                    Return ""
            End Select
        End Function
    End Class

    Private Class Blastoise
        Inherits PokemonForm
        Public Sub New()
            MyBase.New(9)
        End Sub
        Public Overrides Function GetSheetName(P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return "Megas"
                Case Else
                    Return MyBase.GetSheetName(P)
            End Select
        End Function
        Public Overrides Function GetMenuImagePosition(ByVal P As Pokemon) As Vector2
            Select Case P.AdditionalData
                Case "mega"
                    Return New Vector2(3, 0)
                Case Else
                    Return MyBase.GetMenuImagePosition(P)
            End Select
        End Function
        Public Overrides Function GetDataFileAddition(ByVal AdditionalData As String) As String
            Select Case AdditionalData.ToLower()
                Case "mega"
                    Return "_mega"
                Case Else
                    Return ""
            End Select
        End Function
        Public Overrides Function GetAnimationName(ByVal P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return P.OriginalName & "_mega"
                Case Else
                    Return P.OriginalName
            End Select
        End Function

        Public Overrides Function GetOverworldAddition(ByVal P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return "_mega"
                Case Else
                    Return ""
            End Select
        End Function
    End Class

    Private Class Beedrill
        Inherits PokemonForm
        Public Sub New()
            MyBase.New(15)
        End Sub
        Public Overrides Function GetSheetName(P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return "Megas"
                Case Else
                    Return MyBase.GetSheetName(P)
            End Select
        End Function
        Public Overrides Function GetMenuImagePosition(ByVal P As Pokemon) As Vector2
            Select Case P.AdditionalData
                Case "mega"
                    Return New Vector2(12, 1)
                Case Else
                    Return MyBase.GetMenuImagePosition(P)
            End Select
        End Function
        Public Overrides Function GetDataFileAddition(ByVal AdditionalData As String) As String
            Select Case AdditionalData.ToLower()
                Case "mega"
                    Return "_mega"
                Case Else
                    Return ""
            End Select
        End Function
        Public Overrides Function GetAnimationName(ByVal P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return P.OriginalName & "_mega"
                Case Else
                    Return P.OriginalName
            End Select
        End Function

        Public Overrides Function GetOverworldAddition(ByVal P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return "_mega"
                Case Else
                    Return ""
            End Select
        End Function
    End Class

    Private Class Pidgeot
        Inherits PokemonForm
        Public Sub New()
            MyBase.New(18)
        End Sub
        Public Overrides Function GetSheetName(P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return "Megas"
                Case Else
                    Return MyBase.GetSheetName(P)
            End Select
        End Function
        Public Overrides Function GetMenuImagePosition(ByVal P As Pokemon) As Vector2
            Select Case P.AdditionalData
                Case "mega"
                    Return New Vector2(13, 1)
                Case Else
                    Return MyBase.GetMenuImagePosition(P)
            End Select
        End Function
        Public Overrides Function GetDataFileAddition(ByVal AdditionalData As String) As String
            Select Case AdditionalData.ToLower()
                Case "mega"
                    Return "_mega"
                Case Else
                    Return ""
            End Select
        End Function
        Public Overrides Function GetAnimationName(ByVal P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return P.OriginalName & "_mega"
                Case Else
                    Return P.OriginalName
            End Select
        End Function

        Public Overrides Function GetOverworldAddition(ByVal P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return "_mega"
                Case Else
                    Return ""
            End Select
        End Function
    End Class

    Private Class Alakazam
        Inherits PokemonForm
        Public Sub New()
            MyBase.New(65)
        End Sub
        Public Overrides Function GetSheetName(P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return "Megas"
                Case Else
                    Return MyBase.GetSheetName(P)
            End Select
        End Function
        Public Overrides Function GetMenuImagePosition(ByVal P As Pokemon) As Vector2
            Select Case P.AdditionalData
                Case "mega"
                    Return New Vector2(4, 0)
                Case Else
                    Return MyBase.GetMenuImagePosition(P)
            End Select
        End Function
        Public Overrides Function GetDataFileAddition(ByVal AdditionalData As String) As String
            Select Case AdditionalData.ToLower()
                Case "mega"
                    Return "_mega"
                Case Else
                    Return ""
            End Select
        End Function
        Public Overrides Function GetAnimationName(ByVal P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return P.OriginalName & "_mega"
                Case Else
                    Return P.OriginalName
            End Select
        End Function

        Public Overrides Function GetOverworldAddition(ByVal P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return "_mega"
                Case Else
                    Return ""
            End Select
        End Function
    End Class

    Private Class Slowbro
        Inherits PokemonForm
        Public Sub New()
            MyBase.New(80)
        End Sub
        Public Overrides Function GetSheetName(P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return "Megas"
                Case "galar"
                    Return "Regional"
                Case Else
                    Return MyBase.GetSheetName(P)
            End Select
        End Function
        Public Overrides Function GetMenuImagePosition(ByVal P As Pokemon) As Vector2
            Select Case P.AdditionalData
                Case "mega"
                    Return New Vector2(14, 1)
                Case "galar"
                    Return New Vector2(4, 2)
                Case Else
                    Return MyBase.GetMenuImagePosition(P)
            End Select
        End Function
        Public Overrides Function GetDataFileAddition(ByVal AdditionalData As String) As String
            Select Case AdditionalData.ToLower()
                Case "mega"
                    Return "_mega"
                Case "galar"
                    Return "_" & AdditionalData.ToLower()
                Case Else
                    Return ""
            End Select
        End Function
        Public Overrides Function GetAnimationName(ByVal P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return P.OriginalName & "_mega"
                Case "galar"
                    Return P.OriginalName & "_" & P.AdditionalData.ToLower()
                Case Else
                    Return P.OriginalName
            End Select
        End Function

        Public Overrides Function GetOverworldAddition(ByVal P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return "_mega"
                Case "galar"
                    Return "_" & P.AdditionalData.ToLower()
                Case Else
                    Return ""
            End Select
        End Function
    End Class

    Private Class Gengar
        Inherits PokemonForm
        Public Sub New()
            MyBase.New(94)
        End Sub
        Public Overrides Function GetSheetName(P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return "Megas"
                Case Else
                    Return MyBase.GetSheetName(P)
            End Select
        End Function
        Public Overrides Function GetMenuImagePosition(ByVal P As Pokemon) As Vector2
            Select Case P.AdditionalData
                Case "mega"
                    Return New Vector2(5, 0)
                Case Else
                    Return MyBase.GetMenuImagePosition(P)
            End Select
        End Function
        Public Overrides Function GetDataFileAddition(ByVal AdditionalData As String) As String
            Select Case AdditionalData.ToLower()
                Case "mega"
                    Return "_mega"
                Case Else
                    Return ""
            End Select
        End Function
        Public Overrides Function GetAnimationName(ByVal P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return P.OriginalName & "_mega"
                Case Else
                    Return P.OriginalName
            End Select
        End Function

        Public Overrides Function GetOverworldAddition(ByVal P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return "_mega"
                Case Else
                    Return ""
            End Select
        End Function
    End Class

    Private Class Kangaskhan
        Inherits PokemonForm
        Public Sub New()
            MyBase.New(115)
        End Sub
        Public Overrides Function GetSheetName(P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return "Megas"
                Case Else
                    Return MyBase.GetSheetName(P)
            End Select
        End Function
        Public Overrides Function GetMenuImagePosition(ByVal P As Pokemon) As Vector2
            Select Case P.AdditionalData
                Case "mega"
                    Return New Vector2(6, 0)
                Case Else
                    Return MyBase.GetMenuImagePosition(P)
            End Select
        End Function
        Public Overrides Function GetDataFileAddition(ByVal AdditionalData As String) As String
            Select Case AdditionalData.ToLower()
                Case "mega"
                    Return "_mega"
                Case Else
                    Return ""
            End Select
        End Function
        Public Overrides Function GetAnimationName(ByVal P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return P.OriginalName & "_mega"
                Case Else
                    Return P.OriginalName
            End Select
        End Function

        Public Overrides Function GetOverworldAddition(ByVal P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return "_mega"
                Case Else
                    Return ""
            End Select
        End Function
    End Class

    Private Class Pinsir
        Inherits PokemonForm
        Public Sub New()
            MyBase.New(127)
        End Sub
        Public Overrides Function GetSheetName(P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return "Megas"
                Case Else
                    Return MyBase.GetSheetName(P)
            End Select
        End Function
        Public Overrides Function GetMenuImagePosition(ByVal P As Pokemon) As Vector2
            Select Case P.AdditionalData
                Case "mega"
                    Return New Vector2(7, 0)
                Case Else
                    Return MyBase.GetMenuImagePosition(P)
            End Select
        End Function
        Public Overrides Function GetDataFileAddition(ByVal AdditionalData As String) As String
            Select Case AdditionalData.ToLower()
                Case "mega"
                    Return "_mega"
                Case Else
                    Return ""
            End Select
        End Function
        Public Overrides Function GetAnimationName(ByVal P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return P.OriginalName & "_mega"
                Case Else
                    Return P.OriginalName
            End Select
        End Function

        Public Overrides Function GetOverworldAddition(ByVal P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return "_mega"
                Case Else
                    Return ""
            End Select
        End Function
    End Class

    Private Class Gyarados
        Inherits PokemonForm
        Public Sub New()
            MyBase.New(130)
        End Sub
        Public Overrides Function GetSheetName(P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return "Megas"
                Case Else
                    Return MyBase.GetSheetName(P)
            End Select
        End Function
        Public Overrides Function GetMenuImagePosition(ByVal P As Pokemon) As Vector2
            Select Case P.AdditionalData
                Case "mega"
                    Return New Vector2(8, 0)
                Case Else
                    Return MyBase.GetMenuImagePosition(P)
            End Select
        End Function
        Public Overrides Function GetDataFileAddition(ByVal AdditionalData As String) As String
            Select Case AdditionalData.ToLower()
                Case "mega"
                    Return "_mega"
                Case Else
                    Return ""
            End Select
        End Function
        Public Overrides Function GetAnimationName(ByVal P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return P.OriginalName & "_mega"
                Case Else
                    Return P.OriginalName
            End Select
        End Function

        Public Overrides Function GetOverworldAddition(ByVal P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return "_mega"
                Case Else
                    Return ""
            End Select
        End Function
    End Class

    Private Class Aerodactyl
        Inherits PokemonForm
        Public Sub New()
            MyBase.New(142)
        End Sub
        Public Overrides Function GetSheetName(P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return "Megas"
                Case Else
                    Return MyBase.GetSheetName(P)
            End Select
        End Function
        Public Overrides Function GetMenuImagePosition(ByVal P As Pokemon) As Vector2
            Select Case P.AdditionalData
                Case "mega"
                    Return New Vector2(9, 0)
                Case Else
                    Return MyBase.GetMenuImagePosition(P)
            End Select
        End Function
        Public Overrides Function GetDataFileAddition(ByVal AdditionalData As String) As String
            Select Case AdditionalData.ToLower()
                Case "mega"
                    Return "_mega"
                Case Else
                    Return ""
            End Select
        End Function
        Public Overrides Function GetAnimationName(ByVal P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return P.OriginalName & "_mega"
                Case Else
                    Return P.OriginalName
            End Select
        End Function

        Public Overrides Function GetOverworldAddition(ByVal P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return "_mega"
                Case Else
                    Return ""
            End Select
        End Function
    End Class

    Private Class Mewtwo
        Inherits PokemonForm
        Public Sub New()
            MyBase.New(150)
        End Sub
        Public Overrides Function GetSheetName(P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega_x", "mega_y"
                    Return "Megas"
                Case Else
                    Return MyBase.GetSheetName(P)
            End Select
        End Function
        Public Overrides Function GetMenuImagePosition(ByVal P As Pokemon) As Vector2
            Select Case P.AdditionalData
                Case "mega_x"
                    Return New Vector2(10, 0)
                Case "mega_y"
                    Return New Vector2(11, 0)
                Case Else
                    Return MyBase.GetMenuImagePosition(P)
            End Select
        End Function
        Public Overrides Function GetDataFileAddition(ByVal AdditionalData As String) As String
            Select Case AdditionalData.ToLower()
                Case "mega_x"
                    Return "_mega_x"
                Case "mega_y"
                    Return "_mega_y"
                Case Else
                    Return ""
            End Select
        End Function
        Public Overrides Function GetAnimationName(ByVal P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega_x"
                    Return P.OriginalName & "_mega_x"
                Case "mega_y"
                    Return P.OriginalName & "_mega_y"
                Case Else
                    Return P.OriginalName
            End Select
        End Function

        Public Overrides Function GetOverworldAddition(ByVal P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega_x"
                    Return "_mega_x"
                Case "mega_y"
                    Return "_mega_y"
                Case Else
                    Return ""
            End Select
        End Function
    End Class

    Private Class Ampharos
        Inherits PokemonForm
        Public Sub New()
            MyBase.New(181)
        End Sub
        Public Overrides Function GetSheetName(P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return "Megas"
                Case Else
                    Return MyBase.GetSheetName(P)
            End Select
        End Function
        Public Overrides Function GetMenuImagePosition(ByVal P As Pokemon) As Vector2
            Select Case P.AdditionalData
                Case "mega"
                    Return New Vector2(12, 0)
                Case Else
                    Return MyBase.GetMenuImagePosition(P)
            End Select
        End Function
        Public Overrides Function GetDataFileAddition(ByVal AdditionalData As String) As String
            Select Case AdditionalData.ToLower()
                Case "mega"
                    Return "_mega"
                Case Else
                    Return ""
            End Select
        End Function
        Public Overrides Function GetAnimationName(ByVal P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return P.OriginalName & "_mega"
                Case Else
                    Return P.OriginalName
            End Select
        End Function

        Public Overrides Function GetOverworldAddition(ByVal P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return "_mega"
                Case Else
                    Return ""
            End Select
        End Function
    End Class

    Private Class Steelix

        Inherits PokemonForm

        Public Sub New()
            MyBase.New(208)
        End Sub
        Public Overrides Function GetSheetName(P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return "Megas"
                Case Else
                    Return MyBase.GetSheetName(P)
            End Select
        End Function
        Public Overrides Function GetMenuImagePosition(ByVal P As Pokemon) As Vector2
            Select Case P.AdditionalData
                Case "mega"
                    Return New Vector2(15, 1)
                Case Else
                    Return MyBase.GetMenuImagePosition(P)
            End Select
        End Function
        Public Overrides Function GetDataFileAddition(ByVal AdditionalData As String) As String
            Select Case AdditionalData.ToLower()
                Case "mega"
                    Return "_mega"
                Case Else
                    Return ""
            End Select
        End Function
        Public Overrides Function GetAnimationName(ByVal P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return P.OriginalName & "_mega"
                Case Else
                    Return P.OriginalName
            End Select
        End Function
        Public Overrides Function GetOverworldAddition(ByVal P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return "_mega"
                Case Else
                    Return ""
            End Select
        End Function
    End Class

    Private Class Scizor
        Inherits PokemonForm
        Public Sub New()
            MyBase.New(212)
        End Sub
        Public Overrides Function GetSheetName(P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return "Megas"
                Case Else
                    Return MyBase.GetSheetName(P)
            End Select
        End Function
        Public Overrides Function GetMenuImagePosition(ByVal P As Pokemon) As Vector2
            Select Case P.AdditionalData
                Case "mega"
                    Return New Vector2(13, 0)
                Case Else
                    Return MyBase.GetMenuImagePosition(P)
            End Select
        End Function
        Public Overrides Function GetDataFileAddition(ByVal AdditionalData As String) As String
            Select Case AdditionalData.ToLower()
                Case "mega"
                    Return "_mega"
                Case Else
                    Return ""
            End Select
        End Function
        Public Overrides Function GetAnimationName(ByVal P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return P.OriginalName & "_mega"
                Case Else
                    Return P.OriginalName
            End Select
        End Function
        Public Overrides Function GetOverworldAddition(ByVal P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return "_mega"
                Case Else
                    Return ""
            End Select
        End Function
    End Class

    Private Class Heracross
        Inherits PokemonForm
        Public Sub New()
            MyBase.New(214)
        End Sub
        Public Overrides Function GetSheetName(P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return "Megas"
                Case Else
                    Return MyBase.GetSheetName(P)
            End Select
        End Function
        Public Overrides Function GetMenuImagePosition(ByVal P As Pokemon) As Vector2
            Select Case P.AdditionalData
                Case "mega"
                    Return New Vector2(14, 0)
                Case Else
                    Return MyBase.GetMenuImagePosition(P)
            End Select
        End Function
        Public Overrides Function GetDataFileAddition(ByVal AdditionalData As String) As String
            Select Case AdditionalData.ToLower()
                Case "mega"
                    Return "_mega"
                Case Else
                    Return ""
            End Select
        End Function
        Public Overrides Function GetAnimationName(ByVal P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return P.OriginalName & "_mega"
                Case Else
                    Return P.OriginalName
            End Select
        End Function

        Public Overrides Function GetOverworldAddition(ByVal P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return "_mega"
                Case Else
                    Return ""
            End Select
        End Function
    End Class

    Private Class Houndoom
        Inherits PokemonForm
        Public Sub New()
            MyBase.New(229)
        End Sub
        Public Overrides Function GetSheetName(P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return "Megas"
                Case Else
                    Return MyBase.GetSheetName(P)
            End Select
        End Function
        Public Overrides Function GetMenuImagePosition(ByVal P As Pokemon) As Vector2
            Select Case P.AdditionalData
                Case "mega"
                    Return New Vector2(15, 0)
                Case Else
                    Return MyBase.GetMenuImagePosition(P)
            End Select
        End Function
        Public Overrides Function GetDataFileAddition(ByVal AdditionalData As String) As String
            Select Case AdditionalData.ToLower()
                Case "mega"
                    Return "_mega"
                Case Else
                    Return ""
            End Select
        End Function
        Public Overrides Function GetAnimationName(ByVal P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return P.OriginalName & "_mega"
                Case Else
                    Return P.OriginalName
            End Select
        End Function

        Public Overrides Function GetOverworldAddition(ByVal P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return "_mega"
                Case Else
                    Return ""
            End Select
        End Function
    End Class

    Private Class Tyranitar
        Inherits PokemonForm
        Public Sub New()
            MyBase.New(248)
        End Sub
        Public Overrides Function GetSheetName(P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return "Megas"
                Case Else
                    Return MyBase.GetSheetName(P)
            End Select
        End Function
        Public Overrides Function GetMenuImagePosition(ByVal P As Pokemon) As Vector2
            Select Case P.AdditionalData
                Case "mega"
                    Return New Vector2(0, 1)
                Case Else
                    Return MyBase.GetMenuImagePosition(P)
            End Select
        End Function
        Public Overrides Function GetDataFileAddition(ByVal AdditionalData As String) As String
            Select Case AdditionalData.ToLower()
                Case "mega"
                    Return "_mega"
                Case Else
                    Return ""
            End Select
        End Function
        Public Overrides Function GetAnimationName(ByVal P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return P.OriginalName & "_mega"
                Case Else
                    Return P.OriginalName
            End Select
        End Function

        Public Overrides Function GetOverworldAddition(ByVal P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return "_mega"
                Case Else
                    Return ""
            End Select
        End Function
    End Class

    Private Class Sceptile
        Inherits PokemonForm
        Public Sub New()
            MyBase.New(254)
        End Sub
        Public Overrides Function GetSheetName(P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return "Megas"
                Case Else
                    Return MyBase.GetSheetName(P)
            End Select
        End Function
        Public Overrides Function GetMenuImagePosition(ByVal P As Pokemon) As Vector2
            Select Case P.AdditionalData
                Case "mega"
                    Return New Vector2(0, 2)
                Case Else
                    Return MyBase.GetMenuImagePosition(P)
            End Select
        End Function
        Public Overrides Function GetDataFileAddition(ByVal AdditionalData As String) As String
            Select Case AdditionalData.ToLower()
                Case "mega"
                    Return "_mega"
                Case Else
                    Return ""
            End Select
        End Function
        Public Overrides Function GetAnimationName(ByVal P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return P.OriginalName & "_mega"
                Case Else
                    Return P.OriginalName
            End Select
        End Function

        Public Overrides Function GetOverworldAddition(ByVal P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return "_mega"
                Case Else
                    Return ""
            End Select
        End Function
    End Class

    Private Class Blaziken
        Inherits PokemonForm
        Public Sub New()
            MyBase.New(257)
        End Sub
        Public Overrides Function GetSheetName(P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return "Megas"
                Case Else
                    Return MyBase.GetSheetName(P)
            End Select
        End Function
        Public Overrides Function GetMenuImagePosition(ByVal P As Pokemon) As Vector2
            Select Case P.AdditionalData
                Case "mega"
                    Return New Vector2(1, 1)
                Case Else
                    Return MyBase.GetMenuImagePosition(P)
            End Select
        End Function
        Public Overrides Function GetDataFileAddition(ByVal AdditionalData As String) As String
            Select Case AdditionalData.ToLower()
                Case "mega"
                    Return "_mega"
                Case Else
                    Return ""
            End Select
        End Function
        Public Overrides Function GetAnimationName(ByVal P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return P.OriginalName & "_mega"
                Case Else
                    Return P.OriginalName
            End Select
        End Function

        Public Overrides Function GetOverworldAddition(ByVal P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return "_mega"
                Case Else
                    Return ""
            End Select
        End Function
    End Class

    Private Class Swampert
        Inherits PokemonForm
        Public Sub New()
            MyBase.New(260)
        End Sub
        Public Overrides Function GetSheetName(P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return "Megas"
                Case Else
                    Return MyBase.GetSheetName(P)
            End Select
        End Function
        Public Overrides Function GetMenuImagePosition(ByVal P As Pokemon) As Vector2
            Select Case P.AdditionalData
                Case "mega"
                    Return New Vector2(1, 2)
                Case Else
                    Return MyBase.GetMenuImagePosition(P)
            End Select
        End Function
        Public Overrides Function GetDataFileAddition(ByVal AdditionalData As String) As String
            Select Case AdditionalData.ToLower()
                Case "mega"
                    Return "_mega"
                Case Else
                    Return ""
            End Select
        End Function
        Public Overrides Function GetAnimationName(ByVal P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return P.OriginalName & "_mega"
                Case Else
                    Return P.OriginalName
            End Select
        End Function

        Public Overrides Function GetOverworldAddition(ByVal P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return "_mega"
                Case Else
                    Return ""
            End Select
        End Function
    End Class

    Private Class Gardevoir
        Inherits PokemonForm
        Public Sub New()
            MyBase.New(282)
        End Sub
        Public Overrides Function GetSheetName(P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return "Megas"
                Case Else
                    Return MyBase.GetSheetName(P)
            End Select
        End Function
        Public Overrides Function GetMenuImagePosition(ByVal P As Pokemon) As Vector2
            Select Case P.AdditionalData
                Case "mega"
                    Return New Vector2(2, 1)
                Case Else
                    Return MyBase.GetMenuImagePosition(P)
            End Select
        End Function
        Public Overrides Function GetDataFileAddition(ByVal AdditionalData As String) As String
            Select Case AdditionalData.ToLower()
                Case "mega"
                    Return "_mega"
                Case Else
                    Return ""
            End Select
        End Function
        Public Overrides Function GetAnimationName(ByVal P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return P.OriginalName & "_mega"
                Case Else
                    Return P.OriginalName
            End Select
        End Function

        Public Overrides Function GetOverworldAddition(ByVal P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return "_mega"
                Case Else
                    Return ""
            End Select
        End Function
    End Class

    Private Class Sableye
        Inherits PokemonForm
        Public Sub New()
            MyBase.New(302)
        End Sub
        Public Overrides Function GetSheetName(P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return "Megas"
                Case Else
                    Return MyBase.GetSheetName(P)
            End Select
        End Function
        Public Overrides Function GetMenuImagePosition(ByVal P As Pokemon) As Vector2
            Select Case P.AdditionalData
                Case "mega"
                    Return New Vector2(2, 2)
                Case Else
                    Return MyBase.GetMenuImagePosition(P)
            End Select
        End Function
        Public Overrides Function GetDataFileAddition(ByVal AdditionalData As String) As String
            Select Case AdditionalData.ToLower()
                Case "mega"
                    Return "_mega"
                Case Else
                    Return ""
            End Select
        End Function
        Public Overrides Function GetAnimationName(ByVal P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return P.OriginalName & "_mega"
                Case Else
                    Return P.OriginalName
            End Select
        End Function

        Public Overrides Function GetOverworldAddition(ByVal P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return "_mega"
                Case Else
                    Return ""
            End Select
        End Function
    End Class

    Private Class Mawile
        Inherits PokemonForm
        Public Sub New()
            MyBase.New(303)
        End Sub
        Public Overrides Function GetSheetName(P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return "Megas"
                Case Else
                    Return MyBase.GetSheetName(P)
            End Select
        End Function
        Public Overrides Function GetMenuImagePosition(ByVal P As Pokemon) As Vector2
            Select Case P.AdditionalData
                Case "mega"
                    Return New Vector2(3, 1)
                Case Else
                    Return MyBase.GetMenuImagePosition(P)
            End Select
        End Function
        Public Overrides Function GetDataFileAddition(ByVal AdditionalData As String) As String
            Select Case AdditionalData.ToLower()
                Case "mega"
                    Return "_mega"
                Case Else
                    Return ""
            End Select
        End Function
        Public Overrides Function GetAnimationName(ByVal P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return P.OriginalName & "_mega"
                Case Else
                    Return P.OriginalName
            End Select
        End Function

        Public Overrides Function GetOverworldAddition(ByVal P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return "_mega"
                Case Else
                    Return ""
            End Select
        End Function
    End Class

    Private Class Aggron
        Inherits PokemonForm
        Public Sub New()
            MyBase.New(306)
        End Sub
        Public Overrides Function GetSheetName(P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return "Megas"
                Case Else
                    Return MyBase.GetSheetName(P)
            End Select
        End Function
        Public Overrides Function GetMenuImagePosition(ByVal P As Pokemon) As Vector2
            Select Case P.AdditionalData
                Case "mega"
                    Return New Vector2(4, 1)
                Case Else
                    Return MyBase.GetMenuImagePosition(P)
            End Select
        End Function
        Public Overrides Function GetDataFileAddition(ByVal AdditionalData As String) As String
            Select Case AdditionalData.ToLower()
                Case "mega"
                    Return "_mega"
                Case Else
                    Return ""
            End Select
        End Function
        Public Overrides Function GetAnimationName(ByVal P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return P.OriginalName & "_mega"
                Case Else
                    Return P.OriginalName
            End Select
        End Function

        Public Overrides Function GetOverworldAddition(ByVal P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return "_mega"
                Case Else
                    Return ""
            End Select
        End Function
    End Class

    Private Class Medicham
        Inherits PokemonForm
        Public Sub New()
            MyBase.New(308)
        End Sub
        Public Overrides Function GetSheetName(P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return "Megas"
                Case Else
                    Return MyBase.GetSheetName(P)
            End Select
        End Function
        Public Overrides Function GetMenuImagePosition(ByVal P As Pokemon) As Vector2
            Select Case P.AdditionalData
                Case "mega"
                    Return New Vector2(5, 1)
                Case Else
                    Return MyBase.GetMenuImagePosition(P)
            End Select
        End Function
        Public Overrides Function GetDataFileAddition(ByVal AdditionalData As String) As String
            Select Case AdditionalData.ToLower()
                Case "mega"
                    Return "_mega"
                Case Else
                    Return ""
            End Select
        End Function
        Public Overrides Function GetAnimationName(ByVal P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return P.OriginalName & "_mega"
                Case Else
                    Return P.OriginalName
            End Select
        End Function

        Public Overrides Function GetOverworldAddition(ByVal P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return "_mega"
                Case Else
                    Return ""
            End Select
        End Function
    End Class

    Private Class Manectric
        Inherits PokemonForm
        Public Sub New()
            MyBase.New(310)
        End Sub
        Public Overrides Function GetSheetName(P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return "Megas"
                Case Else
                    Return MyBase.GetSheetName(P)
            End Select
        End Function
        Public Overrides Function GetMenuImagePosition(ByVal P As Pokemon) As Vector2
            Select Case P.AdditionalData
                Case "mega"
                    Return New Vector2(6, 1)
                Case Else
                    Return MyBase.GetMenuImagePosition(P)
            End Select
        End Function
        Public Overrides Function GetDataFileAddition(ByVal AdditionalData As String) As String
            Select Case AdditionalData.ToLower()
                Case "mega"
                    Return "_mega"
                Case Else
                    Return ""
            End Select
        End Function
        Public Overrides Function GetAnimationName(ByVal P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return P.OriginalName & "_mega"
                Case Else
                    Return P.OriginalName
            End Select
        End Function

        Public Overrides Function GetOverworldAddition(ByVal P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return "_mega"
                Case Else
                    Return ""
            End Select
        End Function
    End Class

    Private Class Sharpedo
        Inherits PokemonForm
        Public Sub New()
            MyBase.New(319)
        End Sub
        Public Overrides Function GetSheetName(P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return "Megas"
                Case Else
                    Return MyBase.GetSheetName(P)
            End Select
        End Function
        Public Overrides Function GetMenuImagePosition(ByVal P As Pokemon) As Vector2
            Select Case P.AdditionalData
                Case "mega"
                    Return New Vector2(3, 2)
                Case Else
                    Return MyBase.GetMenuImagePosition(P)
            End Select
        End Function
        Public Overrides Function GetDataFileAddition(ByVal AdditionalData As String) As String
            Select Case AdditionalData.ToLower()
                Case "mega"
                    Return "_mega"
                Case Else
                    Return ""
            End Select
        End Function
        Public Overrides Function GetAnimationName(ByVal P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return P.OriginalName & "_mega"
                Case Else
                    Return P.OriginalName
            End Select
        End Function

        Public Overrides Function GetOverworldAddition(ByVal P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return "_mega"
                Case Else
                    Return ""
            End Select
        End Function
    End Class

    Private Class Camerupt
        Inherits PokemonForm
        Public Sub New()
            MyBase.New(323)
        End Sub
        Public Overrides Function GetSheetName(P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return "Megas"
                Case Else
                    Return MyBase.GetSheetName(P)
            End Select
        End Function
        Public Overrides Function GetMenuImagePosition(ByVal P As Pokemon) As Vector2
            Select Case P.AdditionalData
                Case "mega"
                    Return New Vector2(4, 2)
                Case Else
                    Return MyBase.GetMenuImagePosition(P)
            End Select
        End Function
        Public Overrides Function GetDataFileAddition(ByVal AdditionalData As String) As String
            Select Case AdditionalData.ToLower()
                Case "mega"
                    Return "_mega"
                Case Else
                    Return ""
            End Select
        End Function
        Public Overrides Function GetAnimationName(ByVal P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return P.OriginalName & "_mega"
                Case Else
                    Return P.OriginalName
            End Select
        End Function

        Public Overrides Function GetOverworldAddition(ByVal P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return "_mega"
                Case Else
                    Return ""
            End Select
        End Function
    End Class

    Private Class Altaria
        Inherits PokemonForm
        Public Sub New()
            MyBase.New(334)
        End Sub
        Public Overrides Function GetSheetName(P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return "Megas"
                Case Else
                    Return MyBase.GetSheetName(P)
            End Select
        End Function
        Public Overrides Function GetMenuImagePosition(ByVal P As Pokemon) As Vector2
            Select Case P.AdditionalData
                Case "mega"
                    Return New Vector2(5, 2)
                Case Else
                    Return MyBase.GetMenuImagePosition(P)
            End Select
        End Function
        Public Overrides Function GetDataFileAddition(ByVal AdditionalData As String) As String
            Select Case AdditionalData.ToLower()
                Case "mega"
                    Return "_mega"
                Case Else
                    Return ""
            End Select
        End Function
        Public Overrides Function GetAnimationName(ByVal P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return P.OriginalName & "_mega"
                Case Else
                    Return P.OriginalName
            End Select
        End Function

        Public Overrides Function GetOverworldAddition(ByVal P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return "_mega"
                Case Else
                    Return ""
            End Select
        End Function
    End Class

    Private Class Banette
        Inherits PokemonForm
        Public Sub New()
            MyBase.New(354)
        End Sub
        Public Overrides Function GetSheetName(P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return "Megas"
                Case Else
                    Return MyBase.GetSheetName(P)
            End Select
        End Function
        Public Overrides Function GetMenuImagePosition(ByVal P As Pokemon) As Vector2
            Select Case P.AdditionalData
                Case "mega"
                    Return New Vector2(7, 1)
                Case Else
                    Return MyBase.GetMenuImagePosition(P)
            End Select
        End Function
        Public Overrides Function GetDataFileAddition(ByVal AdditionalData As String) As String
            Select Case AdditionalData.ToLower()
                Case "mega"
                    Return "_mega"
                Case Else
                    Return ""
            End Select
        End Function
        Public Overrides Function GetAnimationName(ByVal P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return P.OriginalName & "_mega"
                Case Else
                    Return P.OriginalName
            End Select
        End Function

        Public Overrides Function GetOverworldAddition(ByVal P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return "_mega"
                Case Else
                    Return ""
            End Select
        End Function
    End Class

    Private Class Absol
        Inherits PokemonForm
        Public Sub New()
            MyBase.New(359)
        End Sub
        Public Overrides Function GetSheetName(P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return "Megas"
                Case Else
                    Return MyBase.GetSheetName(P)
            End Select
        End Function
        Public Overrides Function GetMenuImagePosition(ByVal P As Pokemon) As Vector2
            Select Case P.AdditionalData
                Case "mega"
                    Return New Vector2(8, 1)
                Case Else
                    Return MyBase.GetMenuImagePosition(P)
            End Select
        End Function
        Public Overrides Function GetDataFileAddition(ByVal AdditionalData As String) As String
            Select Case AdditionalData.ToLower()
                Case "mega"
                    Return "_mega"
                Case Else
                    Return ""
            End Select
        End Function
        Public Overrides Function GetAnimationName(ByVal P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return P.OriginalName & "_mega"
                Case Else
                    Return P.OriginalName
            End Select
        End Function

        Public Overrides Function GetOverworldAddition(ByVal P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return "_mega"
                Case Else
                    Return ""
            End Select
        End Function
    End Class

    Private Class Glalie
        Inherits PokemonForm
        Public Sub New()
            MyBase.New(362)
        End Sub
        Public Overrides Function GetSheetName(P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return "Megas"
                Case Else
                    Return MyBase.GetSheetName(P)
            End Select
        End Function
        Public Overrides Function GetMenuImagePosition(ByVal P As Pokemon) As Vector2
            Select Case P.AdditionalData
                Case "mega"
                    Return New Vector2(6, 2)
                Case Else
                    Return MyBase.GetMenuImagePosition(P)
            End Select
        End Function
        Public Overrides Function GetDataFileAddition(ByVal AdditionalData As String) As String
            Select Case AdditionalData.ToLower()
                Case "mega"
                    Return "_mega"
                Case Else
                    Return ""
            End Select
        End Function
        Public Overrides Function GetAnimationName(ByVal P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return P.OriginalName & "_mega"
                Case Else
                    Return P.OriginalName
            End Select
        End Function

        Public Overrides Function GetOverworldAddition(ByVal P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return "_mega"
                Case Else
                    Return ""
            End Select
        End Function
    End Class

    Private Class Salamence
        Inherits PokemonForm
        Public Sub New()
            MyBase.New(373)
        End Sub
        Public Overrides Function GetSheetName(P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return "Megas"
                Case Else
                    Return MyBase.GetSheetName(P)
            End Select
        End Function
        Public Overrides Function GetMenuImagePosition(ByVal P As Pokemon) As Vector2
            Select Case P.AdditionalData
                Case "mega"
                    Return New Vector2(7, 2)
                Case Else
                    Return MyBase.GetMenuImagePosition(P)
            End Select
        End Function
        Public Overrides Function GetDataFileAddition(ByVal AdditionalData As String) As String
            Select Case AdditionalData.ToLower()
                Case "mega"
                    Return "_mega"
                Case Else
                    Return ""
            End Select
        End Function
        Public Overrides Function GetAnimationName(ByVal P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return P.OriginalName & "_mega"
                Case Else
                    Return P.OriginalName
            End Select
        End Function

        Public Overrides Function GetOverworldAddition(ByVal P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return "_mega"
                Case Else
                    Return ""
            End Select
        End Function
    End Class

    Private Class Metagross
        Inherits PokemonForm
        Public Sub New()
            MyBase.New(376)
        End Sub
        Public Overrides Function GetSheetName(P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return "Megas"
                Case Else
                    Return MyBase.GetSheetName(P)
            End Select
        End Function
        Public Overrides Function GetMenuImagePosition(ByVal P As Pokemon) As Vector2
            Select Case P.AdditionalData
                Case "mega"
                    Return New Vector2(8, 2)
                Case Else
                    Return MyBase.GetMenuImagePosition(P)
            End Select
        End Function
        Public Overrides Function GetDataFileAddition(ByVal AdditionalData As String) As String
            Select Case AdditionalData.ToLower()
                Case "mega"
                    Return "_mega"
                Case Else
                    Return ""
            End Select
        End Function
        Public Overrides Function GetAnimationName(ByVal P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return P.OriginalName & "_mega"
                Case Else
                    Return P.OriginalName
            End Select
        End Function

        Public Overrides Function GetOverworldAddition(ByVal P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return "_mega"
                Case Else
                    Return ""
            End Select
        End Function
    End Class

    Private Class Latias
        Inherits PokemonForm
        Public Sub New()
            MyBase.New(380)
        End Sub
        Public Overrides Function GetSheetName(P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return "Megas"
                Case Else
                    Return MyBase.GetSheetName(P)
            End Select
        End Function
        Public Overrides Function GetMenuImagePosition(ByVal P As Pokemon) As Vector2
            Select Case P.AdditionalData
                Case "mega"
                    Return New Vector2(9, 2)
                Case Else
                    Return MyBase.GetMenuImagePosition(P)
            End Select
        End Function
        Public Overrides Function GetDataFileAddition(ByVal AdditionalData As String) As String
            Select Case AdditionalData.ToLower()
                Case "mega"
                    Return "_mega"
                Case Else
                    Return ""
            End Select
        End Function
        Public Overrides Function GetAnimationName(ByVal P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return P.OriginalName & "_mega"
                Case Else
                    Return P.OriginalName
            End Select
        End Function
        Public Overrides Function GetOverworldAddition(ByVal P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return "_mega"
                Case Else
                    Return ""
            End Select
        End Function
    End Class

    Private Class Latios
        Inherits PokemonForm
        Public Sub New()
            MyBase.New(381)
        End Sub
        Public Overrides Function GetSheetName(P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return "Megas"
                Case Else
                    Return MyBase.GetSheetName(P)
            End Select
        End Function
        Public Overrides Function GetMenuImagePosition(ByVal P As Pokemon) As Vector2
            Select Case P.AdditionalData
                Case "mega"
                    Return New Vector2(10, 2)
                Case Else
                    Return MyBase.GetMenuImagePosition(P)
            End Select
        End Function
        Public Overrides Function GetDataFileAddition(ByVal AdditionalData As String) As String
            Select Case AdditionalData.ToLower()
                Case "mega"
                    Return "_mega"
                Case Else
                    Return ""
            End Select
        End Function
        Public Overrides Function GetAnimationName(ByVal P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return P.OriginalName & "_mega"
                Case Else
                    Return P.OriginalName
            End Select
        End Function
        Public Overrides Function GetOverworldAddition(ByVal P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return "_mega"
                Case Else
                    Return ""
            End Select
        End Function
    End Class

    Private Class Kyogre
        Inherits PokemonForm
        Public Sub New()
            MyBase.New(382)
        End Sub
        Public Overrides Function GetSheetName(P As Pokemon) As String
            Select Case P.AdditionalData
                Case "primal"
                    Return "OtherForms"
                Case Else
                    Return MyBase.GetSheetName(P)
            End Select
        End Function
        Public Overrides Function GetMenuImagePosition(ByVal P As Pokemon) As Vector2
            Select Case P.AdditionalData
                Case "primal"
                    Return New Vector2(3, 3)
                Case Else
                    Return MyBase.GetMenuImagePosition(P)
            End Select
        End Function
        Public Overrides Function GetDataFileAddition(ByVal AdditionalData As String) As String
            Select Case AdditionalData.ToLower()
                Case "primal"
                    Return "_primal"
                Case Else
                    Return ""
            End Select
        End Function
        Public Overrides Function GetAnimationName(ByVal P As Pokemon) As String
            Select Case P.AdditionalData
                Case "primal"
                    Return P.OriginalName & "_primal"
                Case Else
                    Return P.OriginalName
            End Select
        End Function

        Public Overrides Function GetOverworldAddition(ByVal P As Pokemon) As String
            Select Case P.AdditionalData
                Case "primal"
                    Return "_primal"
                Case Else
                    Return ""
            End Select
        End Function
    End Class

    Private Class Groudon
        Inherits PokemonForm
        Public Sub New()
            MyBase.New(383)
        End Sub
        Public Overrides Function GetSheetName(P As Pokemon) As String
            Select Case P.AdditionalData
                Case "primal"
                    Return "OtherForms"
                Case Else
                    Return MyBase.GetSheetName(P)
            End Select
        End Function
        Public Overrides Function GetMenuImagePosition(ByVal P As Pokemon) As Vector2
            Select Case P.AdditionalData
                Case "primal"
                    Return New Vector2(4, 3)
                Case Else
                    Return MyBase.GetMenuImagePosition(P)
            End Select
        End Function
        Public Overrides Function GetDataFileAddition(ByVal AdditionalData As String) As String
            Select Case AdditionalData.ToLower()
                Case "primal"
                    Return "_primal"
                Case Else
                    Return ""
            End Select
        End Function
        Public Overrides Function GetAnimationName(ByVal P As Pokemon) As String
            Select Case P.AdditionalData
                Case "primal"
                    Return P.OriginalName & "_primal"
                Case Else
                    Return P.OriginalName
            End Select
        End Function

        Public Overrides Function GetOverworldAddition(ByVal P As Pokemon) As String
            Select Case P.AdditionalData
                Case "primal"
                    Return "_primal"
                Case Else
                    Return ""
            End Select
        End Function
    End Class

    Private Class Rayquaza
        Inherits PokemonForm
        Public Sub New()
            MyBase.New(384)
        End Sub
        Public Overrides Function GetSheetName(P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return "Megas"
                Case Else
                    Return MyBase.GetSheetName(P)
            End Select
        End Function
        Public Overrides Function GetMenuImagePosition(ByVal P As Pokemon) As Vector2
            Select Case P.AdditionalData
                Case "mega"
                    Return New Vector2(11, 2)
                Case Else
                    Return MyBase.GetMenuImagePosition(P)
            End Select
        End Function
        Public Overrides Function GetDataFileAddition(ByVal AdditionalData As String) As String
            Select Case AdditionalData.ToLower()
                Case "mega"
                    Return "_mega"
                Case Else
                    Return ""
            End Select
        End Function
        Public Overrides Function GetAnimationName(ByVal P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return P.OriginalName & "_mega"
                Case Else
                    Return P.OriginalName
            End Select
        End Function
        Public Overrides Function GetOverworldAddition(ByVal P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return "_mega"
                Case Else
                    Return ""
            End Select
        End Function
    End Class

    Private Class Lopunny
        Inherits PokemonForm
        Public Sub New()
            MyBase.New(428)
        End Sub
        Public Overrides Function GetSheetName(P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return "Megas"
                Case Else
                    Return MyBase.GetSheetName(P)
            End Select
        End Function
        Public Overrides Function GetMenuImagePosition(ByVal P As Pokemon) As Vector2
            Select Case P.AdditionalData
                Case "mega"
                    Return New Vector2(12, 2)
                Case Else
                    Return MyBase.GetMenuImagePosition(P)
            End Select
        End Function
        Public Overrides Function GetDataFileAddition(ByVal AdditionalData As String) As String
            Select Case AdditionalData.ToLower()
                Case "mega"
                    Return "_mega"
                Case Else
                    Return ""
            End Select
        End Function
        Public Overrides Function GetAnimationName(ByVal P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return P.OriginalName & "_mega"
                Case Else
                    Return P.OriginalName
            End Select
        End Function

        Public Overrides Function GetOverworldAddition(ByVal P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return "_mega"
                Case Else
                    Return ""
            End Select
        End Function
    End Class

    Private Class Garchomp
        Inherits PokemonForm
        Public Sub New()
            MyBase.New(445)
        End Sub
        Public Overrides Function GetSheetName(P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return "Megas"
                Case Else
                    Return MyBase.GetSheetName(P)
            End Select
        End Function
        Public Overrides Function GetMenuImagePosition(ByVal P As Pokemon) As Vector2
            Select Case P.AdditionalData
                Case "mega"
                    Return New Vector2(9, 1)
                Case Else
                    Return MyBase.GetMenuImagePosition(P)
            End Select
        End Function
        Public Overrides Function GetDataFileAddition(ByVal AdditionalData As String) As String
            Select Case AdditionalData.ToLower()
                Case "mega"
                    Return "_mega"
                Case Else
                    Return ""
            End Select
        End Function
        Public Overrides Function GetAnimationName(ByVal P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return P.OriginalName & "_mega"
                Case Else
                    Return P.OriginalName
            End Select
        End Function

        Public Overrides Function GetOverworldAddition(ByVal P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return "_mega"
                Case Else
                    Return ""
            End Select
        End Function
    End Class

    Private Class Lucario
        Inherits PokemonForm
        Public Sub New()
            MyBase.New(448)
        End Sub
        Public Overrides Function GetSheetName(P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return "Megas"
                Case Else
                    Return MyBase.GetSheetName(P)
            End Select
        End Function
        Public Overrides Function GetMenuImagePosition(ByVal P As Pokemon) As Vector2
            Select Case P.AdditionalData
                Case "mega"
                    Return New Vector2(10, 1)
                Case Else
                    Return MyBase.GetMenuImagePosition(P)
            End Select
        End Function
        Public Overrides Function GetDataFileAddition(ByVal AdditionalData As String) As String
            Select Case AdditionalData.ToLower()
                Case "mega"
                    Return "_mega"
                Case Else
                    Return ""
            End Select
        End Function
        Public Overrides Function GetAnimationName(ByVal P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return P.OriginalName & "_mega"
                Case Else
                    Return P.OriginalName
            End Select
        End Function

        Public Overrides Function GetOverworldAddition(ByVal P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return "_mega"
                Case Else
                    Return ""
            End Select
        End Function
    End Class

    Private Class Abomasnow
        Inherits PokemonForm
        Public Sub New()
            MyBase.New(460)
        End Sub
        Public Overrides Function GetSheetName(P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return "Megas"
                Case Else
                    Return MyBase.GetSheetName(P)
            End Select
        End Function
        Public Overrides Function GetMenuImagePosition(ByVal P As Pokemon) As Vector2
            Select Case P.AdditionalData
                Case "mega"
                    Return New Vector2(11, 1)
                Case Else
                    Return MyBase.GetMenuImagePosition(P)
            End Select
        End Function
        Public Overrides Function GetDataFileAddition(ByVal AdditionalData As String) As String
            Select Case AdditionalData.ToLower()
                Case "mega"
                    Return "_mega"
                Case Else
                    Return ""
            End Select
        End Function
        Public Overrides Function GetAnimationName(ByVal P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return P.OriginalName & "_mega"
                Case Else
                    Return P.OriginalName
            End Select
        End Function

        Public Overrides Function GetOverworldAddition(ByVal P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return "_mega"
                Case Else
                    Return ""
            End Select
        End Function
    End Class

    Private Class Gallade
        Inherits PokemonForm
        Public Sub New()
            MyBase.New(475)
        End Sub
        Public Overrides Function GetSheetName(P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return "Megas"
                Case Else
                    Return MyBase.GetSheetName(P)
            End Select
        End Function
        Public Overrides Function GetMenuImagePosition(ByVal P As Pokemon) As Vector2
            Select Case P.AdditionalData
                Case "mega"
                    Return New Vector2(13, 2)
                Case Else
                    Return MyBase.GetMenuImagePosition(P)
            End Select
        End Function
        Public Overrides Function GetDataFileAddition(ByVal AdditionalData As String) As String
            Select Case AdditionalData.ToLower()
                Case "mega"
                    Return "_mega"
                Case Else
                    Return ""
            End Select
        End Function
        Public Overrides Function GetAnimationName(ByVal P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return P.OriginalName & "_mega"
                Case Else
                    Return P.OriginalName
            End Select
        End Function

        Public Overrides Function GetOverworldAddition(ByVal P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return "_mega"
                Case Else
                    Return ""
            End Select
        End Function
    End Class

    Private Class Dialga
        'Leaving this untouched because Primal Dialga is stupid and not canon - Omega
        Inherits PokemonForm

        Public Sub New()
            MyBase.New(483)
        End Sub

        Public Overrides Function GetAnimationName(ByVal P As Pokemon) As String
            If P.AdditionalData.ToLower() = "primal" Then
                Return P.OriginalName & "_primal"
            Else
                Return MyBase.GetAnimationName(P)
            End If
        End Function

        Public Overrides Function GetMenuImagePosition(ByVal P As Pokemon) As Vector2
            If P.AdditionalData.ToLower() = "primal" Then
                Return New Vector2(14, 26)
            Else
                Return MyBase.GetMenuImagePosition(P)
            End If
        End Function

        Public Overrides Function GetOverworldAddition(ByVal P As Pokemon) As String
            If P.AdditionalData.ToLower() = "primal" Then
                Return "_primal"
            Else
                Return MyBase.GetOverworldAddition(P)
            End If
        End Function
        Public Overrides Function GetDataFileAddition(ByVal AdditionalData As String) As String
            Select Case AdditionalData.ToLower()
                Case "primal"
                    Return "_primal"
                Case Else
                    Return ""
            End Select
        End Function
    End Class

    Private Class Audino
        Inherits PokemonForm
        Public Sub New()
            MyBase.New(531)
        End Sub
        Public Overrides Function GetSheetName(P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return "Megas"
                Case Else
                    Return MyBase.GetSheetName(P)
            End Select
        End Function
        Public Overrides Function GetMenuImagePosition(ByVal P As Pokemon) As Vector2
            Select Case P.AdditionalData
                Case "mega"
                    Return New Vector2(14, 2)
                Case Else
                    Return MyBase.GetMenuImagePosition(P)
            End Select
        End Function
        Public Overrides Function GetDataFileAddition(ByVal AdditionalData As String) As String
            Select Case AdditionalData.ToLower()
                Case "mega"
                    Return "_mega"
                Case Else
                    Return ""
            End Select
        End Function
        Public Overrides Function GetAnimationName(ByVal P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return P.OriginalName & "_mega"
                Case Else
                    Return P.OriginalName
            End Select
        End Function

        Public Overrides Function GetOverworldAddition(ByVal P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return "_mega"
                Case Else
                    Return ""
            End Select
        End Function
    End Class

    Private Class Diancie
        Inherits PokemonForm
        Public Sub New()
            MyBase.New(719)
        End Sub
        Public Overrides Function GetSheetName(P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return "Megas"
                Case Else
                    Return MyBase.GetSheetName(P)
            End Select
        End Function
        Public Overrides Function GetMenuImagePosition(ByVal P As Pokemon) As Vector2
            Select Case P.AdditionalData
                Case "mega"
                    Return New Vector2(15, 2)
                Case Else
                    Return MyBase.GetMenuImagePosition(P)
            End Select
        End Function
        Public Overrides Function GetDataFileAddition(ByVal AdditionalData As String) As String
            Select Case AdditionalData.ToLower()
                Case "mega"
                    Return "_mega"
                Case Else
                    Return ""
            End Select
        End Function
        Public Overrides Function GetAnimationName(ByVal P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return P.OriginalName & "_mega"
                Case Else
                    Return P.OriginalName
            End Select
        End Function

        Public Overrides Function GetOverworldAddition(ByVal P As Pokemon) As String
            Select Case P.AdditionalData
                Case "mega"
                    Return "_mega"
                Case Else
                    Return ""
            End Select
        End Function
    End Class

#End Region

#Region "Gender"

    Private Class Nidoran

        Inherits PokemonForm

        Public Sub New()
            MyBase.New({29, 32})
        End Sub

        Public Overrides Function GetAnimationName(ByVal P As Pokemon) As String
            If P.Number = 29 Then
                Return "Nidoran_f"
            Else
                Return "Nidoran_m"
            End If
        End Function

    End Class

    Private Class Frillish

        Inherits PokemonForm

        Public Sub New()
            MyBase.New(592)
        End Sub

        Public Overrides Function GetSheetName(P As Pokemon) As String
            Select Case P.Gender
                Case Pokemon.Genders.Female
                    Return "Gender"
                Case Else
                    Return MyBase.GetSheetName(P)
            End Select
        End Function

        Public Overrides Function GetMenuImagePosition(ByVal P As Pokemon) As Vector2
            Select Case P.Gender
                Case Pokemon.Genders.Female
                    Return New Vector2(1, 0)
                Case Else
                    Return MyBase.GetMenuImagePosition(P)
            End Select
        End Function

        Public Overrides Function GetAnimationName(ByVal P As Pokemon) As String
            Select Case P.Gender
                Case Pokemon.Genders.Female
                    Return P.OriginalName & "_f"
                Case Else
                    Return P.OriginalName & "_m"
            End Select
        End Function

        Public Overrides Function GetOverworldAddition(ByVal P As Pokemon) As String
            Select Case P.Gender
                Case Pokemon.Genders.Female
                    Return "_f"
                Case Else
                    Return "_m"
            End Select
        End Function

    End Class

    Private Class Jellicent

        Inherits PokemonForm

        Public Sub New()
            MyBase.New(593)
        End Sub

        Public Overrides Function GetSheetName(P As Pokemon) As String
            Select Case P.Gender
                Case Pokemon.Genders.Female
                    Return "Gender"
                Case Else
                    Return MyBase.GetSheetName(P)
            End Select
        End Function

        Public Overrides Function GetMenuImagePosition(ByVal P As Pokemon) As Vector2
            Select Case P.Gender
                Case Pokemon.Genders.Female
                    Return New Vector2(2, 0)
                Case Else
                    Return MyBase.GetMenuImagePosition(P)
            End Select
        End Function

        Public Overrides Function GetAnimationName(ByVal P As Pokemon) As String
            Select Case P.Gender
                Case Pokemon.Genders.Female
                    Return P.OriginalName & "_f"
                Case Else
                    Return P.OriginalName & "_m"
            End Select
        End Function

        Public Overrides Function GetOverworldAddition(ByVal P As Pokemon) As String
            Select Case P.Gender
                Case Pokemon.Genders.Female
                    Return "_f"
                Case Else
                    Return "_m"
            End Select
        End Function

    End Class

    Private Class Pyroar

        Inherits PokemonForm

        Public Sub New()
            MyBase.New(668)
        End Sub

        Public Overrides Function GetSheetName(P As Pokemon) As String
            Select Case P.Gender
                Case Pokemon.Genders.Female
                    Return "Gender"
                Case Else
                    Return MyBase.GetSheetName(P)
            End Select
        End Function

        Public Overrides Function GetMenuImagePosition(ByVal P As Pokemon) As Vector2
            Select Case P.Gender
                Case Pokemon.Genders.Female
                    Return New Vector2(3, 0)
                Case Else
                    Return MyBase.GetMenuImagePosition(P)
            End Select
        End Function

        Public Overrides Function GetAnimationName(ByVal P As Pokemon) As String
            Select Case P.Gender
                Case Pokemon.Genders.Male
                    Return P.OriginalName & "_male"
                Case Else
                    Return P.OriginalName & "_female"
            End Select
        End Function

        Public Overrides Function GetOverworldAddition(ByVal P As Pokemon) As String
            Select Case P.Gender
                Case Pokemon.Genders.Male
                    Return "_male"
                Case Else
                    Return "_female"
            End Select
        End Function
    End Class

#End Region

#Region "Aesthetic"
    Private Class Pichu

        Inherits PokemonForm

        Public Sub New()
            MyBase.New(172)
        End Sub
        Public Overrides Function GetSheetName(P As Pokemon) As String
            Select Case P.AdditionalData
                Case "spiky-eared"
                    Return "OtherForms"
                Case Else
                    Return MyBase.GetSheetName(P)
            End Select
        End Function
        Public Overrides Function GetAnimationName(ByVal P As Pokemon) As String
            If P.AdditionalData.ToLower() = "spiky-eared" Then
                Return P.OriginalName & "_spiky-eared"
            Else
                Return MyBase.GetAnimationName(P)
            End If
        End Function

        Public Overrides Function GetOverworldAddition(ByVal P As Pokemon) As String
            If P.AdditionalData.ToLower() = "spiky-eared" Then
                Return "_spiky-eared"
            Else
                Return MyBase.GetOverworldAddition(P)
            End If
        End Function

        Public Overrides Function GetMenuImagePosition(ByVal P As Pokemon) As Vector2
            If P.AdditionalData.ToLower() = "spiky-eared" Then
                Return New Vector2(0, 2)
            Else
                Return MyBase.GetMenuImagePosition(P)
            End If
        End Function

    End Class

    Private Class Unown

        Inherits PokemonForm

        Public Sub New()
            MyBase.New(201)
        End Sub

        Public Overrides Function GetInitialAdditionalData(ByVal P As Pokemon) As String
            Return CStr(Core.Random.Next(0, 28))
        End Function

        Public Overrides Function GetSheetName(P As Pokemon) As String
            Return "Unown"
        End Function

        Public Overrides Function GetAnimationName(ByVal P As Pokemon) As String
            Dim AlphabetArray() As String = {"a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", "question", "exclamation"}
            If CInt(P.AdditionalData) > 0 Then
                Return "Unown_" & AlphabetArray(CInt(P.AdditionalData))
            End If
            Return "Unown"
        End Function

        Public Overrides Function GetMenuImagePosition(ByVal P As Pokemon) As Vector2
            Dim x As Integer = 0
            Dim y As Integer = 0
            Select Case CInt(P.AdditionalData)
                Case 0 To 15
                    y = 0
                    x = CInt(P.AdditionalData)
                Case Else
                    y = 1
                    x = CInt(P.AdditionalData) - 16
            End Select
            Return New Vector2(x, y)
        End Function

        Public Overrides Function GetOverworldAddition(ByVal P As Pokemon) As String
            Dim alphabet() As String = {"a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", "question", "exclamation"}
            Return "-" & alphabet(CInt(P.AdditionalData))
        End Function

    End Class

    Private Class Shellos

        Inherits PokemonForm

        Public Sub New()
            MyBase.New(422)
        End Sub

        Public Overrides Function GetSheetName(P As Pokemon) As String
            Select Case P.AdditionalData
                Case "1"
                    Return "OtherForms"
                Case Else
                    Return MyBase.GetSheetName(P)
            End Select
        End Function

        Public Overrides Function GetMenuImagePosition(ByVal P As Pokemon) As Vector2
            If P.AdditionalData = "1" Then
                Return New Vector2(5, 4)
            Else
                Return MyBase.GetMenuImagePosition(P)
            End If
        End Function

        Public Overrides Function GetAnimationName(ByVal P As Pokemon) As String
            If P.AdditionalData = "1" Then
                Return P.OriginalName & "e"
            Else
                Return P.OriginalName & "w"
            End If
        End Function

        Public Overrides Function GetOverworldAddition(ByVal P As Pokemon) As String
            If P.AdditionalData = "1" Then
                Return "e"
            Else
                Return "w"
            End If
        End Function

    End Class

    Private Class Gastrodon

        Inherits PokemonForm

        Public Sub New()
            MyBase.New(423)
        End Sub

        Public Overrides Function GetSheetName(P As Pokemon) As String
            Select Case P.AdditionalData
                Case "1"
                    Return "OtherForms"
                Case Else
                    Return MyBase.GetSheetName(P)
            End Select
        End Function

        Public Overrides Function GetMenuImagePosition(ByVal P As Pokemon) As Vector2
            If P.AdditionalData = "1" Then
                Return New Vector2(6, 4)
            Else
                Return MyBase.GetMenuImagePosition(P)
            End If
        End Function

        Public Overrides Function GetAnimationName(ByVal P As Pokemon) As String
            If P.AdditionalData = "1" Then
                Return P.OriginalName & "e"
            Else
                Return P.OriginalName & "w"
            End If
        End Function

        Public Overrides Function GetOverworldAddition(ByVal P As Pokemon) As String
            If P.AdditionalData = "1" Then
                Return "e"
            Else
                Return "w"
            End If
        End Function

    End Class

    Private Class Deerling

        Inherits PokemonForm

        Public Sub New()
            MyBase.New(585)
        End Sub

        Public Overrides Function GetSheetName(P As Pokemon) As String
            Select Case World.CurrentSeason
                Case World.Seasons.Summer, World.Seasons.Fall, World.Seasons.Winter
                    Return "OtherForms"
                Case Else
                    Return MyBase.GetSheetName(P)
            End Select
        End Function

        Public Overrides Function GetMenuImagePosition(ByVal P As Pokemon) As Vector2
            Select Case World.CurrentSeason
                Case World.Seasons.Summer
                    Return New Vector2(3, 5)
                Case World.Seasons.Fall
                    Return New Vector2(4, 5)
                Case World.Seasons.Winter
                    Return New Vector2(5, 5)
                Case Else
                    Return MyBase.GetMenuImagePosition(P)
            End Select
        End Function

        Public Overrides Function GetAnimationName(ByVal P As Pokemon) As String
            Select Case World.CurrentSeason
                Case World.Seasons.Fall
                    Return P.OriginalName & "_fa"
                Case World.Seasons.Spring
                    Return P.OriginalName & "_sp"
                Case World.Seasons.Summer
                    Return P.OriginalName & "_su"
                Case World.Seasons.Winter
                    Return P.OriginalName & "_wi"
            End Select
            Return P.OriginalName & "_fa"
        End Function

        Public Overrides Function GetOverworldAddition(ByVal P As Pokemon) As String
            Select Case World.CurrentSeason
                Case World.Seasons.Fall
                    Return "_fa"
                Case World.Seasons.Spring
                    Return "_sp"
                Case World.Seasons.Summer
                    Return "_su"
                Case World.Seasons.Winter
                    Return "_wi"
            End Select
            Return "_fa"
        End Function

    End Class

    Private Class Sawsbuck

        Inherits PokemonForm

        Public Sub New()
            MyBase.New(586)
        End Sub

        Public Overrides Function GetSheetName(P As Pokemon) As String
            Select Case World.CurrentSeason
                Case World.Seasons.Summer, World.Seasons.Fall, World.Seasons.Winter
                    Return "OtherForms"
                Case Else
                    Return MyBase.GetSheetName(P)
            End Select
        End Function

        Public Overrides Function GetMenuImagePosition(ByVal P As Pokemon) As Vector2
            Select Case World.CurrentSeason
                Case World.Seasons.Summer
                    Return New Vector2(6, 5)
                Case World.Seasons.Fall
                    Return New Vector2(7, 5)
                Case World.Seasons.Winter
                    Return New Vector2(8, 5)
                Case Else
                    Return MyBase.GetMenuImagePosition(P)
            End Select
        End Function

        Public Overrides Function GetAnimationName(ByVal P As Pokemon) As String
            Select Case World.CurrentSeason
                Case World.Seasons.Fall
                    Return P.OriginalName & "_fa"
                Case World.Seasons.Spring
                    Return P.OriginalName & "_sp"
                Case World.Seasons.Summer
                    Return P.OriginalName & "_su"
                Case World.Seasons.Winter
                    Return P.OriginalName & "_wi"
            End Select
            Return P.OriginalName & "_fa"
        End Function

        Public Overrides Function GetOverworldAddition(ByVal P As Pokemon) As String
            Select Case World.CurrentSeason
                Case World.Seasons.Fall
                    Return "_fa"
                Case World.Seasons.Spring
                    Return "_sp"
                Case World.Seasons.Summer
                    Return "_su"
                Case World.Seasons.Winter
                    Return "_wi"
            End Select
            Return "_fa"
        End Function

    End Class

    Private Class Vivillon
        Inherits PokemonForm

        Public Sub New()
            MyBase.New(666)
        End Sub

        Public Overrides Function GetSheetName(P As Pokemon) As String
            Return "Vivillon"
        End Function

        Public Overrides Function GetMenuImagePosition(ByVal P As Pokemon) As Vector2
            Select Case P.AdditionalData
                Case "archipelago"
                    Return New Vector2(1, 0)
                Case "continental"
                    Return New Vector2(2, 0)
                Case "elegant"
                    Return New Vector2(3, 0)
                Case "fancy"
                    Return New Vector2(4, 0)
                Case "garden"
                    Return New Vector2(5, 0)
                Case "high_planes"
                    Return New Vector2(6, 0)
                Case "icy_snow"
                    Return New Vector2(7, 0)
                Case "jungle"
                    Return New Vector2(8, 0)
                Case "marine"
                    Return New Vector2(9, 0)
                Case "meadow"
                    Return New Vector2(0, 0)
                Case "modern"
                    Return New Vector2(10, 0)
                Case "monsoon"
                    Return New Vector2(11, 0)
                Case "ocean"
                    Return New Vector2(12, 0)
                Case "pokeball"
                    Return New Vector2(13, 0)
                Case "polar"
                    Return New Vector2(14, 0)
                Case "river"
                    Return New Vector2(15, 0)
                Case "sandstorm"
                    Return New Vector2(0, 1)
                Case "savanna"
                    Return New Vector2(1, 1)
                Case "sun"
                    Return New Vector2(2, 1)
                Case "tundra"
                    Return New Vector2(3, 1)
                Case Else
                    Return New Vector2(0, 0)
            End Select
        End Function

        Public Overrides Function GetOverworldAddition(ByVal P As Pokemon) As String
            Select Case P.AdditionalData
                Case "archipelago"
                    Return "_archipelago"
                Case "continental"
                    Return "_continental"
                Case "elegant"
                    Return "_elegant"
                Case "fancy"
                    Return "_fancy"
                Case "garden"
                    Return "_garden"
                Case "high_planes"
                    Return "_high_planes"
                Case "icy_snow"
                    Return "_icy_snow"
                Case "jungle"
                    Return "_jungle"
                Case "marine"
                    Return "_marine"
                Case "meadow"
                    Return "_meadow"
                Case "modern"
                    Return "_modern"
                Case "monsoon"
                    Return "_monsoon"
                Case "ocean"
                    Return "_ocean"
                Case "pokeball"
                    Return "_pokeball"
                Case "polar"
                    Return "_polar"
                Case "river"
                    Return "_river"
                Case "sandstorm"
                    Return "_sandstorm"
                Case "savanna"
                    Return "_savanna"
                Case "sun"
                    Return "_sun"
                Case "tundra"
                    Return "_tundra"
                Case Else
                    Return "_meadow"
            End Select
        End Function
    End Class

    Private Class Flabebe
        Inherits PokemonForm
        Public Sub New()
            MyBase.New(669)
        End Sub

        Public Overrides Function GetSheetName(P As Pokemon) As String
            Select Case P.AdditionalData
                Case "yellow", "blue", "orange", "white"
                    Return "OtherForms"
                Case Else
                    Return MyBase.GetSheetName(P)
            End Select
        End Function

        Public Overrides Function GetMenuImagePosition(ByVal P As Pokemon) As Vector2
            Select Case P.AdditionalData
                Case "yellow"
                    Return New Vector2(0, 7)
                Case "blue"
                    Return New Vector2(1, 7)
                Case "orange"
                    Return New Vector2(2, 7)
                Case "white"
                    Return New Vector2(3, 7)
                Case Else
                    Return MyBase.GetMenuImagePosition(P)
            End Select
        End Function

        Public Overrides Function GetAnimationName(ByVal P As Pokemon) As String
            Select Case P.AdditionalData
                Case "yellow"
                    Return "flabebe_yellow"
                Case "blue"
                    Return "flabebe_blue"
                Case "orange"
                    Return "flabebe_orange"
                Case "white"
                    Return "flabebe_white"
                Case Else
                    Return "flabebe"
            End Select
        End Function

        Public Overrides Function GetOverworldAddition(ByVal P As Pokemon) As String
            Select Case P.AdditionalData
                Case "yellow"
                    Return "_yellow"
                Case "blue"
                    Return "_blue"
                Case "orange"
                    Return "_orange"
                Case "white"
                    Return "_white"
                Case Else
                    Return "_red"
            End Select
        End Function

    End Class
    Private Class Floette
        Inherits PokemonForm
        Public Sub New()
            MyBase.New(670)
        End Sub

        Public Overrides Function GetSheetName(P As Pokemon) As String
            Select Case P.AdditionalData
                Case "yellow", "blue", "orange", "white"
                    Return "OtherForms"
                Case Else
                    Return MyBase.GetSheetName(P)
            End Select
        End Function

        Public Overrides Function GetMenuImagePosition(ByVal P As Pokemon) As Vector2
            Select Case P.AdditionalData
                Case "yellow"
                    Return New Vector2(5, 7)
                Case "blue"
                    Return New Vector2(6, 7)
                Case "orange"
                    Return New Vector2(7, 7)
                Case "white"
                    Return New Vector2(8, 7)
                Case "eternal"
                    Return New Vector2(4, 7)
                Case Else
                    Return MyBase.GetMenuImagePosition(P)
            End Select
        End Function

        Public Overrides Function GetAnimationName(ByVal P As Pokemon) As String
            Select Case P.AdditionalData
                Case "yellow"
                    Return P.OriginalName & "_yellow"
                Case "blue"
                    Return P.OriginalName & "_blue"
                Case "orange"
                    Return P.OriginalName & "_orange"
                Case "white"
                    Return P.OriginalName & "_white"
                Case "eternal"
                    Return P.OriginalName & "_eternal"
                Case Else
                    Return P.OriginalName
            End Select
        End Function

        Public Overrides Function GetOverworldAddition(ByVal P As Pokemon) As String
            Select Case P.AdditionalData
                Case "yellow"
                    Return "_yellow"
                Case "blue"
                    Return "_blue"
                Case "orange"
                    Return "_orange"
                Case "white"
                    Return "_white"
                Case "eternal"
                    Return "_eternal"
                Case Else
                    Return "_red"
            End Select
        End Function

        Public Overrides Function GetDataFileAddition(ByVal AdditionalData As String) As String
            Select Case AdditionalData
                Case "eternal"
                    Return "_eternal"
                Case Else
                    Return ""
            End Select
        End Function

    End Class
    Private Class Florges
        Inherits PokemonForm
        Public Sub New()
            MyBase.New(671)
        End Sub

        Public Overrides Function GetSheetName(P As Pokemon) As String
            Select Case P.AdditionalData
                Case "yellow", "blue", "orange", "white"
                    Return "OtherForms"
                Case Else
                    Return MyBase.GetSheetName(P)
            End Select
        End Function

        Public Overrides Function GetMenuImagePosition(ByVal P As Pokemon) As Vector2
            Select Case P.AdditionalData
                Case "yellow"
                    Return New Vector2(9, 7)
                Case "blue"
                    Return New Vector2(10, 7)
                Case "orange"
                    Return New Vector2(11, 7)
                Case "white"
                    Return New Vector2(12, 7)
                Case Else
                    Return MyBase.GetMenuImagePosition(P)
            End Select
        End Function

        Public Overrides Function GetAnimationName(ByVal P As Pokemon) As String
            Select Case P.AdditionalData
                Case "yellow"
                    Return P.OriginalName & "_yellow"
                Case "blue"
                    Return P.OriginalName & "_blue"
                Case "orange"
                    Return P.OriginalName & "_orange"
                Case "white"
                    Return P.OriginalName & "_white"
                Case Else
                    Return P.OriginalName
            End Select
        End Function

        Public Overrides Function GetOverworldAddition(ByVal P As Pokemon) As String
            Select Case P.AdditionalData
                Case "yellow"
                    Return "_yellow"
                Case "blue"
                    Return "_blue"
                Case "orange"
                    Return "_orange"
                Case "white"
                    Return "_white"
                Case Else
                    Return "_red"
            End Select
        End Function

    End Class

#End Region

#Region "Other"
    Private Class Deoxys

        Inherits PokemonForm

        Public Sub New()
            MyBase.New(386)
        End Sub

        Public Overrides Function GetSheetName(P As Pokemon) As String
            Select Case P.AdditionalData
                Case "attack", "defense", "speed"
                    Return "OtherForms"
                Case Else
                    Return MyBase.GetSheetName(P)
            End Select
        End Function

        Public Overrides Function GetAnimationName(ByVal P As Pokemon) As String
            Select Case P.AdditionalData.ToLower()
                Case "attack", "defense", "speed"
                    Return P.OriginalName & "_" & P.AdditionalData.ToLower()
                Case Else
                    Return P.OriginalName
            End Select
        End Function

        Public Overrides Function GetMenuImagePosition(ByVal P As Pokemon) As Vector2
            Select Case P.AdditionalData.ToLower()
                Case "attack"
                    Return New Vector2(5, 3)
                Case "defense"
                    Return New Vector2(6, 3)
                Case "speed"
                    Return New Vector2(7, 3)
                Case Else
                    Return MyBase.GetMenuImagePosition(P)
            End Select
        End Function

        Public Overrides Function GetOverworldAddition(ByVal P As Pokemon) As String
            Select Case P.AdditionalData.ToLower()
                Case "attack", "defense", "speed"
                    Return "_" & P.AdditionalData.ToLower()
                Case Else
                    Return ""
            End Select
        End Function

        Public Overrides Function GetDataFileAddition(ByVal AdditionalData As String) As String
            Select Case AdditionalData.ToLower()
                Case "attack", "defense", "speed"
                    Return "_" & AdditionalData.ToLower()
                Case Else
                    Return ""
            End Select
        End Function

    End Class

    Private Class Burmy

        Inherits PokemonForm

        Public Sub New()
            MyBase.New(412)
        End Sub

        'TODO: Finish forms.
        Public Overrides Function GetInitialAdditionalData(ByVal P As Pokemon) As String
            Select Case Screen.Level.EnvironmentType
                Case 0, 5 'Outside, Forest
                    Return "plant"
                Case 2, 3 'Cave, Underwater
                    Return "sandy"
                Case 1 'Inside, Dark
                    Return "trash"
            End Select

            Return "plant"
        End Function

        Public Overrides Function GetSheetName(P As Pokemon) As String
            Select Case P.AdditionalData
                Case "sandy", "trash"
                    Return "OtherForms"
                Case Else
                    Return MyBase.GetSheetName(P)
            End Select
        End Function

    End Class

    Private Class Rotom

        Inherits PokemonForm

        Public Sub New()
            MyBase.New(479)
        End Sub

        Public Overrides Function GetSheetName(P As Pokemon) As String
            Select Case P.AdditionalData
                Case "fan", "frost", "heat", "mow", "wash"
                    Return "OtherForms"
                Case Else
                    Return MyBase.GetSheetName(P)
            End Select
        End Function

        Public Overrides Function GetAnimationName(ByVal P As Pokemon) As String
            Select Case P.AdditionalData.ToLower()
                Case "fan", "frost", "heat", "mow", "wash"
                    Return P.OriginalName & "_" & P.AdditionalData.ToLower()
                Case Else
                    Return P.OriginalName
            End Select
        End Function

        Public Overrides Function GetMenuImagePosition(ByVal P As Pokemon) As Vector2
            Select Case P.AdditionalData.ToLower()
                Case "fan"
                    Return New Vector2(12, 4)
                Case "frost"
                    Return New Vector2(11, 4)
                Case "heat"
                    Return New Vector2(9, 4)
                Case "mow"
                    Return New Vector2(13, 4)
                Case "wash"
                    Return New Vector2(10, 4)
                Case Else
                    Return MyBase.GetMenuImagePosition(P)
            End Select
        End Function

        Public Overrides Function GetOverworldAddition(ByVal P As Pokemon) As String
            Select Case P.AdditionalData.ToLower()
                Case "fan", "frost", "heat", "mow", "wash"
                    Return "_" & P.AdditionalData.ToLower()
                Case Else
                    Return ""
            End Select
        End Function

        Public Overrides Function GetDataFileAddition(ByVal AdditionalData As String) As String
            Select Case AdditionalData.ToLower()
                Case "fan", "frost", "heat", "mow", "wash"
                    Return "_" & AdditionalData.ToLower()
                Case Else
                    Return ""
            End Select
        End Function

    End Class

    Private Class Arceus

        Inherits PokemonForm

        Public Sub New()
            MyBase.New(493)
        End Sub

        Public Overrides Function GetSheetName(P As Pokemon) As String
            Return "Arceus"
        End Function

        Private Function GetTypeAdditionFromPlate(ByVal P As Pokemon) As Tuple(Of String, Integer, Integer)
            If Not P.Item Is Nothing Then
                If P.Item.IsPlate = False Then
                    Return New Tuple(Of String, Integer, Integer)("", 0, 0)
                Else
                    Select Case P.Item.ID
                        Case 267
                            Return New Tuple(Of String, Integer, Integer)("dragon", 14, 0)
                        Case 268
                            Return New Tuple(Of String, Integer, Integer)("dark", 15, 0)
                        Case 269
                            Return New Tuple(Of String, Integer, Integer)("ground", 8, 0)
                        Case 270
                            Return New Tuple(Of String, Integer, Integer)("fighting", 6, 0)
                        Case 271
                            Return New Tuple(Of String, Integer, Integer)("fire", 1, 0)
                        Case 272
                            Return New Tuple(Of String, Integer, Integer)("ice", 5, 0)
                        Case 273
                            Return New Tuple(Of String, Integer, Integer)("bug", 11, 0)
                        Case 274
                            Return New Tuple(Of String, Integer, Integer)("steel", 0, 1)
                        Case 275
                            Return New Tuple(Of String, Integer, Integer)("grass", 4, 0)
                        Case 276
                            Return New Tuple(Of String, Integer, Integer)("psychic", 10, 0)
                        Case 277
                            Return New Tuple(Of String, Integer, Integer)("fairy", 1, 1)
                        Case 278
                            Return New Tuple(Of String, Integer, Integer)("flying", 9, 0)
                        Case 279
                            Return New Tuple(Of String, Integer, Integer)("water", 2, 0)
                        Case 280
                            Return New Tuple(Of String, Integer, Integer)("ghost", 13, 0)
                        Case 281
                            Return New Tuple(Of String, Integer, Integer)("rock", 12, 0)
                        Case 282
                            Return New Tuple(Of String, Integer, Integer)("poison", 7, 0)
                        Case 283
                            Return New Tuple(Of String, Integer, Integer)("electric", 3, 0)
                        Case Else
                            Return New Tuple(Of String, Integer, Integer)("", 0, 0)
                    End Select
                End If
            Else
                Return New Tuple(Of String, Integer, Integer)("", 0, 0)
            End If
        End Function

        Public Overrides Function GetAnimationName(ByVal P As Pokemon) As String
            Dim typeAddition As String = GetTypeAdditionFromPlate(P).Item1
            If typeAddition <> "" Then
                typeAddition = "_" & typeAddition
            End If

            Return "Arceus" & typeAddition
        End Function

        Public Overrides Function GetMenuImagePosition(ByVal P As Pokemon) As Vector2
            Dim data = GetTypeAdditionFromPlate(P)
            Return New Vector2(data.Item2, data.Item3)
        End Function

        Public Overrides Function GetOverworldAddition(ByVal P As Pokemon) As String
            Dim typeAddition As String = GetTypeAdditionFromPlate(P).Item1
            If typeAddition <> "" Then
                typeAddition = "_" & typeAddition
            End If
            Return typeAddition
        End Function

    End Class

    Private Class Basculin

        Inherits PokemonForm

        Public Sub New()
            MyBase.New(550)
        End Sub

        Public Overrides Function GetSheetName(P As Pokemon) As String
            Select Case P.AdditionalData.ToLower()
                Case "blue", "white"
                    Return "OtherForms"
                Case Else
                    Return MyBase.GetSheetName(P)
            End Select
        End Function

        Public Overrides Function GetDataFileAddition(AdditionalData As String) As String
            Select Case AdditionalData.ToLower()
                Case "blue", "white"
                    Return "_" & AdditionalData.ToLower()
                Case Else
                    Return ""
            End Select
        End Function

        Public Overrides Function GetAnimationName(P As Pokemon) As String
            Select Case P.AdditionalData.ToLower()
                Case "blue", "white"
                    Return P.OriginalName & "_" & P.AdditionalData.ToLower()
                Case Else
                    Return P.OriginalName
            End Select
        End Function

        Public Overrides Function GetMenuImagePosition(P As Pokemon) As Vector2
            Select Case P.AdditionalData.ToLower()
                Case "blue"
                    Return New Vector2(0, 5)
                Case "White"
                    Return New Vector2(15, 4)
                Case Else
                    Return MyBase.GetMenuImagePosition(P)
            End Select
        End Function

        Public Overrides Function GetOverworldAddition(P As Pokemon) As String
            Select Case P.AdditionalData.ToLower()
                Case "blue", "white"
                    Return "_" & P.AdditionalData.ToLower()
                Case Else
                    Return ""
            End Select
        End Function

    End Class

    Private Class Tornadus

        Inherits PokemonForm

        Public Sub New()
            MyBase.New(641)
        End Sub

        Public Overrides Function GetSheetName(P As Pokemon) As String
            Select Case P.AdditionalData
                Case "therian"
                    Return "OtherForms"
                Case Else
                    Return MyBase.GetSheetName(P)
            End Select
        End Function

        Public Overrides Function GetAnimationName(P As Pokemon) As String
            If P.AdditionalData.ToLower() = "therian" Then
                Return P.OriginalName & "_therian"
            Else
                Return MyBase.GetAnimationName(P)
            End If
        End Function

        Public Overrides Function GetMenuImagePosition(P As Pokemon) As Vector2
            If P.AdditionalData.ToLower() = "therian" Then
                Return New Vector2(9, 5)
            Else
                Return MyBase.GetMenuImagePosition(P)
            End If
        End Function

        Public Overrides Function GetOverworldAddition(P As Pokemon) As String
            If P.AdditionalData.ToLower() = "therian" Then
                Return "_therian"
            Else
                Return MyBase.GetOverworldAddition(P)
            End If
        End Function

    End Class

    Private Class Thundurus

        Inherits PokemonForm

        Public Sub New()
            MyBase.New(642)
        End Sub

        Public Overrides Function GetSheetName(P As Pokemon) As String
            Select Case P.AdditionalData
                Case "therian"
                    Return "OtherForms"
                Case Else
                    Return MyBase.GetSheetName(P)
            End Select
        End Function

        Public Overrides Function GetAnimationName(P As Pokemon) As String
            If P.AdditionalData.ToLower() = "therian" Then
                Return P.OriginalName & "_therian"
            Else
                Return MyBase.GetAnimationName(P)
            End If
        End Function

        Public Overrides Function GetMenuImagePosition(P As Pokemon) As Vector2
            If P.AdditionalData.ToLower() = "therian" Then
                Return New Vector2(10, 5)
            Else
                Return MyBase.GetMenuImagePosition(P)
            End If
        End Function

        Public Overrides Function GetOverworldAddition(P As Pokemon) As String
            If P.AdditionalData.ToLower() = "therian" Then
                Return "_therian"
            Else
                Return MyBase.GetOverworldAddition(P)
            End If
        End Function

    End Class

    Private Class Landorus

        Inherits PokemonForm

        Public Sub New()
            MyBase.New(645)
        End Sub

        Public Overrides Function GetSheetName(P As Pokemon) As String
            Select Case P.AdditionalData
                Case "therian"
                    Return "OtherForms"
                Case Else
                    Return MyBase.GetSheetName(P)
            End Select
        End Function

        Public Overrides Function GetAnimationName(P As Pokemon) As String
            If P.AdditionalData.ToLower() = "therian" Then
                Return P.OriginalName & "_therian"
            Else
                Return MyBase.GetAnimationName(P)
            End If
        End Function

        Public Overrides Function GetMenuImagePosition(P As Pokemon) As Vector2
            If P.AdditionalData.ToLower() = "therian" Then
                Return New Vector2(11, 5)
            Else
                Return MyBase.GetMenuImagePosition(P)
            End If
        End Function

        Public Overrides Function GetOverworldAddition(P As Pokemon) As String
            If P.AdditionalData.ToLower() = "therian" Then
                Return "_therian"
            Else
                Return MyBase.GetOverworldAddition(P)
            End If
        End Function

    End Class

    Private Class Kyurem

        Inherits PokemonForm

        Public Sub New()
            MyBase.New(646)
        End Sub

        Public Overrides Function GetSheetName(P As Pokemon) As String
            Select Case P.AdditionalData
                Case "black", "white"
                    Return "OtherForms"
                Case Else
                    Return MyBase.GetSheetName(P)
            End Select
        End Function

        Public Overrides Function GetAnimationName(ByVal P As Pokemon) As String
            Select Case P.AdditionalData.ToLower()
                Case "black"
                    Return P.OriginalName & "_black"
                Case "white"
                    Return P.OriginalName & "_white"
                Case Else
                    Return P.OriginalName
            End Select
        End Function

        Public Overrides Function GetOverworldAddition(P As Pokemon) As String
            Select Case P.AdditionalData.ToLower()
                Case "black"
                    Return "_black"
                Case "white"
                    Return "_white"
                Case Else
                    Return ""
            End Select
        End Function

        Public Overrides Function GetMenuImagePosition(ByVal P As Pokemon) As Vector2
            Select Case P.AdditionalData.ToLower()
                Case "black"
                    Return New Vector2(13, 5)
                Case "white"
                    Return New Vector2(12, 5)
                Case Else
                    Return MyBase.GetMenuImagePosition(P)
            End Select
        End Function

    End Class

    Private Class Aegislash

        Inherits PokemonForm

        Public Sub New()
            MyBase.New(681)
        End Sub

        Public Overrides Function GetSheetName(P As Pokemon) As String
            Select Case P.AdditionalData
                Case "blade"
                    Return "OtherForms"
                Case Else
                    Return MyBase.GetSheetName(P)
            End Select
        End Function

        Public Overrides Function GetMenuImagePosition(ByVal P As Pokemon) As Vector2
            Select Case P.AdditionalData
                Case "blade"
                    Return New Vector2(6, 8)
                Case Else
                    Return MyBase.GetMenuImagePosition(P)
            End Select
        End Function

        Public Overrides Function GetAnimationName(ByVal P As Pokemon) As String
            Select Case P.AdditionalData
                Case "blade"
                    Return P.OriginalName & "_blade"
                Case Else
                    Return P.OriginalName
            End Select
        End Function

        Public Overrides Function GetOverworldAddition(ByVal P As Pokemon) As String
            Select Case P.AdditionalData
                Case "blade"
                    Return "_blade"
                Case Else
                    Return ""
            End Select
        End Function

        Public Overrides Function GetDataFileAddition(ByVal AdditionalData As String) As String
            Select Case AdditionalData.ToLower()
                Case "blade"
                    Return "_blade"
                Case Else
                    Return ""
            End Select
        End Function

    End Class

    Private Class Hoopa
        Inherits PokemonForm
        Public Sub New()
            MyBase.New(720)
        End Sub
        Public Overrides Function GetSheetName(P As Pokemon) As String
            Select Case P.AdditionalData
                Case "unbound"
                    Return "OtherForms"
                Case Else
                    Return MyBase.GetSheetName(P)
            End Select
        End Function
        Public Overrides Function GetMenuImagePosition(ByVal P As Pokemon) As Vector2
            Select Case P.AdditionalData
                Case "unbound"
                    Return New Vector2(0, 9)
                Case Else
                    Return MyBase.GetMenuImagePosition(P)
            End Select
        End Function
        Public Overrides Function GetDataFileAddition(ByVal AdditionalData As String) As String
            Select Case AdditionalData.ToLower()
                Case "unbound"
                    Return "_unbound"
                Case Else
                    Return ""
            End Select
        End Function
        Public Overrides Function GetAnimationName(ByVal P As Pokemon) As String
            Select Case P.AdditionalData
                Case "unbound"
                    Return P.OriginalName & "_unbound"
                Case Else
                    Return P.OriginalName
            End Select
        End Function

        Public Overrides Function GetOverworldAddition(ByVal P As Pokemon) As String
            Select Case P.AdditionalData
                Case "unbound"
                    Return "_unbound"
                Case Else
                    Return ""
            End Select
        End Function
    End Class

#End Region

#Region "Regionals"

#Region "Alola"
    Private Class Rattata

        Inherits PokemonForm

        Public Sub New()
            MyBase.New(19)
        End Sub

        Public Overrides Function GetSheetName(P As Pokemon) As String
            Select Case P.AdditionalData
                Case "alola"
                    Return "Regional"
                Case Else
                    Return MyBase.GetSheetName(P)
            End Select
        End Function

        Public Overrides Function GetAnimationName(ByVal P As Pokemon) As String
            Select Case P.AdditionalData.ToLower()
                Case "alola"
                    Return P.OriginalName & "_" & P.AdditionalData.ToLower()
                Case Else
                    Return P.OriginalName
            End Select
        End Function

        Public Overrides Function GetMenuImagePosition(ByVal P As Pokemon) As Vector2
            Select Case P.AdditionalData.ToLower()
                Case "alola"
                    Return New Vector2(0, 0)
                Case Else
                    Return MyBase.GetMenuImagePosition(P)
            End Select
        End Function

        Public Overrides Function GetOverworldAddition(ByVal P As Pokemon) As String
            Select Case P.AdditionalData.ToLower()
                Case "alola"
                    Return "_" & P.AdditionalData.ToLower()
                Case Else
                    Return ""
            End Select
        End Function

        Public Overrides Function GetDataFileAddition(ByVal AdditionalData As String) As String
            Select Case AdditionalData.ToLower()
                Case "alola"
                    Return "_" & AdditionalData.ToLower()
                Case Else
                    Return ""
            End Select
        End Function

    End Class

    Private Class Raticate

        Inherits PokemonForm

        Public Sub New()
            MyBase.New(20)
        End Sub

        Public Overrides Function GetSheetName(P As Pokemon) As String
            Select Case P.AdditionalData
                Case "alola"
                    Return "Regional"
                Case Else
                    Return MyBase.GetSheetName(P)
            End Select
        End Function

        Public Overrides Function GetAnimationName(ByVal P As Pokemon) As String
            Select Case P.AdditionalData.ToLower()
                Case "alola"
                    Return P.OriginalName & "_" & P.AdditionalData.ToLower()
                Case Else
                    Return P.OriginalName
            End Select
        End Function

        Public Overrides Function GetMenuImagePosition(ByVal P As Pokemon) As Vector2
            Select Case P.AdditionalData.ToLower()
                Case "alola"
                    Return New Vector2(1, 0)
                Case Else
                    Return MyBase.GetMenuImagePosition(P)
            End Select
        End Function

        Public Overrides Function GetOverworldAddition(ByVal P As Pokemon) As String
            Select Case P.AdditionalData.ToLower()
                Case "alola"
                    Return "_" & P.AdditionalData.ToLower()
                Case Else
                    Return ""
            End Select
        End Function

        Public Overrides Function GetDataFileAddition(ByVal AdditionalData As String) As String
            Select Case AdditionalData.ToLower()
                Case "alola"
                    Return "_" & AdditionalData.ToLower()
                Case Else
                    Return ""
            End Select
        End Function

    End Class

    Private Class Raichu

        Inherits PokemonForm

        Public Sub New()
            MyBase.New(26)
        End Sub

        Public Overrides Function GetSheetName(P As Pokemon) As String
            Select Case P.AdditionalData
                Case "alola"
                    Return "Regional"
                Case Else
                    Return MyBase.GetSheetName(P)
            End Select
        End Function

        Public Overrides Function GetAnimationName(ByVal P As Pokemon) As String
            Select Case P.AdditionalData.ToLower()
                Case "alola"
                    Return P.OriginalName & "_" & P.AdditionalData.ToLower()
                Case Else
                    Return P.OriginalName
            End Select
        End Function

        Public Overrides Function GetMenuImagePosition(ByVal P As Pokemon) As Vector2
            Select Case P.AdditionalData.ToLower()
                Case "alola"
                    Return New Vector2(2, 0)
                Case Else
                    Return MyBase.GetMenuImagePosition(P)
            End Select
        End Function

        Public Overrides Function GetOverworldAddition(ByVal P As Pokemon) As String
            Select Case P.AdditionalData.ToLower()
                Case "alola"
                    Return "_" & P.AdditionalData.ToLower()
                Case Else
                    Return ""
            End Select
        End Function

        Public Overrides Function GetDataFileAddition(ByVal AdditionalData As String) As String
            Select Case AdditionalData.ToLower()
                Case "alola"
                    Return "_" & AdditionalData.ToLower()
                Case Else
                    Return ""
            End Select
        End Function

    End Class

    Private Class Sandshrew

        Inherits PokemonForm

        Public Sub New()
            MyBase.New(27)
        End Sub

        Public Overrides Function GetSheetName(P As Pokemon) As String
            Select Case P.AdditionalData
                Case "alola"
                    Return "Regional"
                Case Else
                    Return MyBase.GetSheetName(P)
            End Select
        End Function

        Public Overrides Function GetAnimationName(ByVal P As Pokemon) As String
            Select Case P.AdditionalData.ToLower()
                Case "alola"
                    Return P.OriginalName & "_" & P.AdditionalData.ToLower()
                Case Else
                    Return P.OriginalName
            End Select
        End Function

        Public Overrides Function GetMenuImagePosition(ByVal P As Pokemon) As Vector2
            Select Case P.AdditionalData.ToLower()
                Case "alola"
                    Return New Vector2(3, 0)
                Case Else
                    Return MyBase.GetMenuImagePosition(P)
            End Select
        End Function

        Public Overrides Function GetOverworldAddition(ByVal P As Pokemon) As String
            Select Case P.AdditionalData.ToLower()
                Case "alola"
                    Return "_" & P.AdditionalData.ToLower()
                Case Else
                    Return ""
            End Select
        End Function

        Public Overrides Function GetDataFileAddition(ByVal AdditionalData As String) As String
            Select Case AdditionalData.ToLower()
                Case "alola"
                    Return "_" & AdditionalData.ToLower()
                Case Else
                    Return ""
            End Select
        End Function

    End Class

    Private Class Sandslash

        Inherits PokemonForm

        Public Sub New()
            MyBase.New(28)
        End Sub

        Public Overrides Function GetSheetName(P As Pokemon) As String
            Select Case P.AdditionalData
                Case "alola"
                    Return "Regional"
                Case Else
                    Return MyBase.GetSheetName(P)
            End Select
        End Function

        Public Overrides Function GetAnimationName(ByVal P As Pokemon) As String
            Select Case P.AdditionalData.ToLower()
                Case "alola"
                    Return P.OriginalName & "_" & P.AdditionalData.ToLower()
                Case Else
                    Return P.OriginalName
            End Select
        End Function

        Public Overrides Function GetMenuImagePosition(ByVal P As Pokemon) As Vector2
            Select Case P.AdditionalData.ToLower()
                Case "alola"
                    Return New Vector2(4, 0)
                Case Else
                    Return MyBase.GetMenuImagePosition(P)
            End Select
        End Function

        Public Overrides Function GetOverworldAddition(ByVal P As Pokemon) As String
            Select Case P.AdditionalData.ToLower()
                Case "alola"
                    Return "_" & P.AdditionalData.ToLower()
                Case Else
                    Return ""
            End Select
        End Function

        Public Overrides Function GetDataFileAddition(ByVal AdditionalData As String) As String
            Select Case AdditionalData.ToLower()
                Case "alola"
                    Return "_" & AdditionalData.ToLower()
                Case Else
                    Return ""
            End Select
        End Function

    End Class

    Private Class Vulpix

        Inherits PokemonForm

        Public Sub New()
            MyBase.New(37)
        End Sub

        Public Overrides Function GetSheetName(P As Pokemon) As String
            Select Case P.AdditionalData
                Case "alola"
                    Return "Regional"
                Case Else
                    Return MyBase.GetSheetName(P)
            End Select
        End Function

        Public Overrides Function GetAnimationName(ByVal P As Pokemon) As String
            Select Case P.AdditionalData.ToLower()
                Case "alola"
                    Return P.OriginalName & "_" & P.AdditionalData.ToLower()
                Case Else
                    Return P.OriginalName
            End Select
        End Function

        Public Overrides Function GetMenuImagePosition(ByVal P As Pokemon) As Vector2
            Select Case P.AdditionalData.ToLower()
                Case "alola"
                    Return New Vector2(5, 0)
                Case Else
                    Return MyBase.GetMenuImagePosition(P)
            End Select
        End Function

        Public Overrides Function GetOverworldAddition(ByVal P As Pokemon) As String
            Select Case P.AdditionalData.ToLower()
                Case "alola"
                    Return "_" & P.AdditionalData.ToLower()
                Case Else
                    Return ""
            End Select
        End Function

        Public Overrides Function GetDataFileAddition(ByVal AdditionalData As String) As String
            Select Case AdditionalData.ToLower()
                Case "alola"
                    Return "_" & AdditionalData.ToLower()
                Case Else
                    Return ""
            End Select
        End Function

    End Class

    Private Class Ninetales

        Inherits PokemonForm

        Public Sub New()
            MyBase.New(38)
        End Sub

        Public Overrides Function GetSheetName(P As Pokemon) As String
            Select Case P.AdditionalData
                Case "alola"
                    Return "Regional"
                Case Else
                    Return MyBase.GetSheetName(P)
            End Select
        End Function

        Public Overrides Function GetAnimationName(ByVal P As Pokemon) As String
            Select Case P.AdditionalData.ToLower()
                Case "alola"
                    Return P.OriginalName & "_" & P.AdditionalData.ToLower()
                Case Else
                    Return P.OriginalName
            End Select
        End Function

        Public Overrides Function GetMenuImagePosition(ByVal P As Pokemon) As Vector2
            Select Case P.AdditionalData.ToLower()
                Case "alola"
                    Return New Vector2(6, 0)
                Case Else
                    Return MyBase.GetMenuImagePosition(P)
            End Select
        End Function

        Public Overrides Function GetOverworldAddition(ByVal P As Pokemon) As String
            Select Case P.AdditionalData.ToLower()
                Case "alola"
                    Return "_" & P.AdditionalData.ToLower()
                Case Else
                    Return ""
            End Select
        End Function

        Public Overrides Function GetDataFileAddition(ByVal AdditionalData As String) As String
            Select Case AdditionalData.ToLower()
                Case "alola"
                    Return "_" & AdditionalData.ToLower()
                Case Else
                    Return ""
            End Select
        End Function

    End Class

    Private Class Diglett

        Inherits PokemonForm

        Public Sub New()
            MyBase.New(50)
        End Sub

        Public Overrides Function GetSheetName(P As Pokemon) As String
            Select Case P.AdditionalData
                Case "alola"
                    Return "Regional"
                Case Else
                    Return MyBase.GetSheetName(P)
            End Select
        End Function

        Public Overrides Function GetAnimationName(ByVal P As Pokemon) As String
            Select Case P.AdditionalData.ToLower()
                Case "alola"
                    Return P.OriginalName & "_" & P.AdditionalData.ToLower()
                Case Else
                    Return P.OriginalName
            End Select
        End Function

        Public Overrides Function GetMenuImagePosition(ByVal P As Pokemon) As Vector2
            Select Case P.AdditionalData.ToLower()
                Case "alola"
                    Return New Vector2(7, 0)
                Case Else
                    Return MyBase.GetMenuImagePosition(P)
            End Select
        End Function

        Public Overrides Function GetOverworldAddition(ByVal P As Pokemon) As String
            Select Case P.AdditionalData.ToLower()
                Case "alola"
                    Return "_" & P.AdditionalData.ToLower()
                Case Else
                    Return ""
            End Select
        End Function

        Public Overrides Function GetDataFileAddition(ByVal AdditionalData As String) As String
            Select Case AdditionalData.ToLower()
                Case "alola"
                    Return "_" & AdditionalData.ToLower()
                Case Else
                    Return ""
            End Select
        End Function

    End Class

    Private Class Dugtrio

        Inherits PokemonForm

        Public Sub New()
            MyBase.New(51)
        End Sub

        Public Overrides Function GetSheetName(P As Pokemon) As String
            Select Case P.AdditionalData
                Case "alola"
                    Return "Regional"
                Case Else
                    Return MyBase.GetSheetName(P)
            End Select
        End Function

        Public Overrides Function GetAnimationName(ByVal P As Pokemon) As String
            Select Case P.AdditionalData.ToLower()
                Case "alola"
                    Return P.OriginalName & "_" & P.AdditionalData.ToLower()
                Case Else
                    Return P.OriginalName
            End Select
        End Function

        Public Overrides Function GetMenuImagePosition(ByVal P As Pokemon) As Vector2
            Select Case P.AdditionalData.ToLower()
                Case "alola"
                    Return New Vector2(8, 0)
                Case Else
                    Return MyBase.GetMenuImagePosition(P)
            End Select
        End Function

        Public Overrides Function GetOverworldAddition(ByVal P As Pokemon) As String
            Select Case P.AdditionalData.ToLower()
                Case "alola"
                    Return "_" & P.AdditionalData.ToLower()
                Case Else
                    Return ""
            End Select
        End Function

        Public Overrides Function GetDataFileAddition(ByVal AdditionalData As String) As String
            Select Case AdditionalData.ToLower()
                Case "alola"
                    Return "_" & AdditionalData.ToLower()
                Case Else
                    Return ""
            End Select
        End Function

    End Class

    Private Class Meowth

        Inherits PokemonForm

        Public Sub New()
            MyBase.New(52)
        End Sub

        Public Overrides Function GetSheetName(P As Pokemon) As String
            Select Case P.AdditionalData
                Case "alola", "galar"
                    Return "Regional"
                Case Else
                    Return MyBase.GetSheetName(P)
            End Select
        End Function

        Public Overrides Function GetAnimationName(ByVal P As Pokemon) As String
            Select Case P.AdditionalData.ToLower()
                Case "alola", "galar"
                    Return P.OriginalName & "_" & P.AdditionalData.ToLower()
                Case Else
                    Return P.OriginalName
            End Select
        End Function

        Public Overrides Function GetMenuImagePosition(ByVal P As Pokemon) As Vector2
            Select Case P.AdditionalData.ToLower()
                Case "alola"
                    Return New Vector2(9, 0)
                Case "galar"
                    Return New Vector2(0, 2)
                Case Else
                    Return MyBase.GetMenuImagePosition(P)
            End Select
        End Function

        Public Overrides Function GetOverworldAddition(ByVal P As Pokemon) As String
            Select Case P.AdditionalData.ToLower()
                Case "alola", "galar"
                    Return "_" & P.AdditionalData.ToLower()
                Case Else
                    Return ""
            End Select
        End Function

        Public Overrides Function GetDataFileAddition(ByVal AdditionalData As String) As String
            Select Case AdditionalData.ToLower()
                Case "alola", "galar"
                    Return "_" & AdditionalData.ToLower()
                Case Else
                    Return ""
            End Select
        End Function

    End Class

    Private Class Persian

        Inherits PokemonForm

        Public Sub New()
            MyBase.New(53)
        End Sub

        Public Overrides Function GetSheetName(P As Pokemon) As String
            Select Case P.AdditionalData
                Case "alola"
                    Return "Regional"
                Case Else
                    Return MyBase.GetSheetName(P)
            End Select
        End Function

        Public Overrides Function GetAnimationName(ByVal P As Pokemon) As String
            Select Case P.AdditionalData.ToLower()
                Case "alola"
                    Return P.OriginalName & "_" & P.AdditionalData.ToLower()
                Case Else
                    Return P.OriginalName
            End Select
        End Function

        Public Overrides Function GetMenuImagePosition(ByVal P As Pokemon) As Vector2
            Select Case P.AdditionalData.ToLower()
                Case "alola"
                    Return New Vector2(10, 0)
                Case Else
                    Return MyBase.GetMenuImagePosition(P)
            End Select
        End Function

        Public Overrides Function GetOverworldAddition(ByVal P As Pokemon) As String
            Select Case P.AdditionalData.ToLower()
                Case "alola"
                    Return "_" & P.AdditionalData.ToLower()
                Case Else
                    Return ""
            End Select
        End Function

        Public Overrides Function GetDataFileAddition(ByVal AdditionalData As String) As String
            Select Case AdditionalData.ToLower()
                Case "alola"
                    Return "_" & AdditionalData.ToLower()
                Case Else
                    Return ""
            End Select
        End Function

    End Class

    Private Class Geodude

        Inherits PokemonForm

        Public Sub New()
            MyBase.New(74)
        End Sub

        Public Overrides Function GetSheetName(P As Pokemon) As String
            Select Case P.AdditionalData
                Case "alola"
                    Return "Regional"
                Case Else
                    Return MyBase.GetSheetName(P)
            End Select
        End Function

        Public Overrides Function GetAnimationName(ByVal P As Pokemon) As String
            Select Case P.AdditionalData.ToLower()
                Case "alola"
                    Return P.OriginalName & "_" & P.AdditionalData.ToLower()
                Case Else
                    Return P.OriginalName
            End Select
        End Function

        Public Overrides Function GetMenuImagePosition(ByVal P As Pokemon) As Vector2
            Select Case P.AdditionalData.ToLower()
                Case "alola"
                    Return New Vector2(11, 0)
                Case Else
                    Return MyBase.GetMenuImagePosition(P)
            End Select
        End Function

        Public Overrides Function GetOverworldAddition(ByVal P As Pokemon) As String
            Select Case P.AdditionalData.ToLower()
                Case "alola"
                    Return "_" & P.AdditionalData.ToLower()
                Case Else
                    Return ""
            End Select
        End Function

        Public Overrides Function GetDataFileAddition(ByVal AdditionalData As String) As String
            Select Case AdditionalData.ToLower()
                Case "alola"
                    Return "_" & AdditionalData.ToLower()
                Case Else
                    Return ""
            End Select
        End Function

    End Class

    Private Class Graveler

        Inherits PokemonForm

        Public Sub New()
            MyBase.New(75)
        End Sub

        Public Overrides Function GetSheetName(P As Pokemon) As String
            Select Case P.AdditionalData
                Case "alola"
                    Return "Regional"
                Case Else
                    Return MyBase.GetSheetName(P)
            End Select
        End Function

        Public Overrides Function GetAnimationName(ByVal P As Pokemon) As String
            Select Case P.AdditionalData.ToLower()
                Case "alola"
                    Return P.OriginalName & "_" & P.AdditionalData.ToLower()
                Case Else
                    Return P.OriginalName
            End Select
        End Function

        Public Overrides Function GetMenuImagePosition(ByVal P As Pokemon) As Vector2
            Select Case P.AdditionalData.ToLower()
                Case "alola"
                    Return New Vector2(12, 0)
                Case Else
                    Return MyBase.GetMenuImagePosition(P)
            End Select
        End Function

        Public Overrides Function GetOverworldAddition(ByVal P As Pokemon) As String
            Select Case P.AdditionalData.ToLower()
                Case "alola"
                    Return "_" & P.AdditionalData.ToLower()
                Case Else
                    Return ""
            End Select
        End Function

        Public Overrides Function GetDataFileAddition(ByVal AdditionalData As String) As String
            Select Case AdditionalData.ToLower()
                Case "alola"
                    Return "_" & AdditionalData.ToLower()
                Case Else
                    Return ""
            End Select
        End Function

    End Class

    Private Class Golem

        Inherits PokemonForm

        Public Sub New()
            MyBase.New(76)
        End Sub

        Public Overrides Function GetSheetName(P As Pokemon) As String
            Select Case P.AdditionalData
                Case "alola"
                    Return "Regional"
                Case Else
                    Return MyBase.GetSheetName(P)
            End Select
        End Function

        Public Overrides Function GetAnimationName(ByVal P As Pokemon) As String
            Select Case P.AdditionalData.ToLower()
                Case "alola"
                    Return P.OriginalName & "_" & P.AdditionalData.ToLower()
                Case Else
                    Return P.OriginalName
            End Select
        End Function

        Public Overrides Function GetMenuImagePosition(ByVal P As Pokemon) As Vector2
            Select Case P.AdditionalData.ToLower()
                Case "alola"
                    Return New Vector2(13, 0)
                Case Else
                    Return MyBase.GetMenuImagePosition(P)
            End Select
        End Function

        Public Overrides Function GetOverworldAddition(ByVal P As Pokemon) As String
            Select Case P.AdditionalData.ToLower()
                Case "alola"
                    Return "_" & P.AdditionalData.ToLower()
                Case Else
                    Return ""
            End Select
        End Function

        Public Overrides Function GetDataFileAddition(ByVal AdditionalData As String) As String
            Select Case AdditionalData.ToLower()
                Case "alola"
                    Return "_" & AdditionalData.ToLower()
                Case Else
                    Return ""
            End Select
        End Function

    End Class

    Private Class Grimer

        Inherits PokemonForm

        Public Sub New()
            MyBase.New(88)
        End Sub

        Public Overrides Function GetSheetName(P As Pokemon) As String
            Select Case P.AdditionalData
                Case "alola"
                    Return "Regional"
                Case Else
                    Return MyBase.GetSheetName(P)
            End Select
        End Function

        Public Overrides Function GetAnimationName(ByVal P As Pokemon) As String
            Select Case P.AdditionalData.ToLower()
                Case "alola"
                    Return P.OriginalName & "_" & P.AdditionalData.ToLower()
                Case Else
                    Return P.OriginalName
            End Select
        End Function

        Public Overrides Function GetMenuImagePosition(ByVal P As Pokemon) As Vector2
            Select Case P.AdditionalData.ToLower()
                Case "alola"
                    Return New Vector2(14, 0)
                Case Else
                    Return MyBase.GetMenuImagePosition(P)
            End Select
        End Function

        Public Overrides Function GetOverworldAddition(ByVal P As Pokemon) As String
            Select Case P.AdditionalData.ToLower()
                Case "alola"
                    Return "_" & P.AdditionalData.ToLower()
                Case Else
                    Return ""
            End Select
        End Function

        Public Overrides Function GetDataFileAddition(ByVal AdditionalData As String) As String
            Select Case AdditionalData.ToLower()
                Case "alola"
                    Return "_" & AdditionalData.ToLower()
                Case Else
                    Return ""
            End Select
        End Function

    End Class

    Private Class Muk

        Inherits PokemonForm

        Public Sub New()
            MyBase.New(89)
        End Sub

        Public Overrides Function GetSheetName(P As Pokemon) As String
            Select Case P.AdditionalData
                Case "alola"
                    Return "Regional"
                Case Else
                    Return MyBase.GetSheetName(P)
            End Select
        End Function

        Public Overrides Function GetAnimationName(ByVal P As Pokemon) As String
            Select Case P.AdditionalData.ToLower()
                Case "alola"
                    Return P.OriginalName & "_" & P.AdditionalData.ToLower()
                Case Else
                    Return P.OriginalName
            End Select
        End Function

        Public Overrides Function GetMenuImagePosition(ByVal P As Pokemon) As Vector2
            Select Case P.AdditionalData.ToLower()
                Case "alola"
                    Return New Vector2(15, 0)
                Case Else
                    Return MyBase.GetMenuImagePosition(P)
            End Select
        End Function

        Public Overrides Function GetOverworldAddition(ByVal P As Pokemon) As String
            Select Case P.AdditionalData.ToLower()
                Case "alola"
                    Return "_" & P.AdditionalData.ToLower()
                Case Else
                    Return ""
            End Select
        End Function

        Public Overrides Function GetDataFileAddition(ByVal AdditionalData As String) As String
            Select Case AdditionalData.ToLower()
                Case "alola"
                    Return "_" & AdditionalData.ToLower()
                Case Else
                    Return ""
            End Select
        End Function

    End Class

    Private Class Exeggutor

        Inherits PokemonForm

        Public Sub New()
            MyBase.New(103)
        End Sub

        Public Overrides Function GetSheetName(P As Pokemon) As String
            Select Case P.AdditionalData
                Case "alola"
                    Return "Regional"
                Case Else
                    Return MyBase.GetSheetName(P)
            End Select
        End Function

        Public Overrides Function GetAnimationName(ByVal P As Pokemon) As String
            Select Case P.AdditionalData.ToLower()
                Case "alola"
                    Return P.OriginalName & "_" & P.AdditionalData.ToLower()
                Case Else
                    Return P.OriginalName
            End Select
        End Function

        Public Overrides Function GetMenuImagePosition(ByVal P As Pokemon) As Vector2
            Select Case P.AdditionalData.ToLower()
                Case "alola"
                    Return New Vector2(0, 1)
                Case Else
                    Return MyBase.GetMenuImagePosition(P)
            End Select
        End Function

        Public Overrides Function GetOverworldAddition(ByVal P As Pokemon) As String
            Select Case P.AdditionalData.ToLower()
                Case "alola"
                    Return "_" & P.AdditionalData.ToLower()
                Case Else
                    Return ""
            End Select
        End Function

        Public Overrides Function GetDataFileAddition(ByVal AdditionalData As String) As String
            Select Case AdditionalData.ToLower()
                Case "alola"
                    Return "_" & AdditionalData.ToLower()
                Case Else
                    Return ""
            End Select
        End Function

    End Class

    Private Class Marowak

        Inherits PokemonForm

        Public Sub New()
            MyBase.New(105)
        End Sub

        Public Overrides Function GetSheetName(P As Pokemon) As String
            Select Case P.AdditionalData
                Case "alola"
                    Return "Regional"
                Case Else
                    Return MyBase.GetSheetName(P)
            End Select
        End Function

        Public Overrides Function GetAnimationName(ByVal P As Pokemon) As String
            Select Case P.AdditionalData.ToLower()
                Case "alola"
                    Return P.OriginalName & "_" & P.AdditionalData.ToLower()
                Case Else
                    Return P.OriginalName
            End Select
        End Function

        Public Overrides Function GetMenuImagePosition(ByVal P As Pokemon) As Vector2
            Select Case P.AdditionalData.ToLower()
                Case "alola"
                    Return New Vector2(1, 1)
                Case Else
                    Return MyBase.GetMenuImagePosition(P)
            End Select
        End Function

        Public Overrides Function GetOverworldAddition(ByVal P As Pokemon) As String
            Select Case P.AdditionalData.ToLower()
                Case "alola"
                    Return "_" & P.AdditionalData.ToLower()
                Case Else
                    Return ""
            End Select
        End Function

        Public Overrides Function GetDataFileAddition(ByVal AdditionalData As String) As String
            Select Case AdditionalData.ToLower()
                Case "alola"
                    Return "_" & AdditionalData.ToLower()
                Case Else
                    Return ""
            End Select
        End Function

    End Class

#End Region

#Region "Galar"
    Private Class Ponyta

        Inherits PokemonForm

        Public Sub New()
            MyBase.New(77)
        End Sub

        Public Overrides Function GetSheetName(P As Pokemon) As String
            Select Case P.AdditionalData
                Case "galar"
                    Return "Regional"
                Case Else
                    Return MyBase.GetSheetName(P)
            End Select
        End Function

        Public Overrides Function GetAnimationName(ByVal P As Pokemon) As String
            Select Case P.AdditionalData.ToLower()
                Case "galar"
                    Return P.OriginalName & "_" & P.AdditionalData.ToLower()
                Case Else
                    Return P.OriginalName
            End Select
        End Function

        Public Overrides Function GetMenuImagePosition(ByVal P As Pokemon) As Vector2
            Select Case P.AdditionalData.ToLower()
                Case "galar"
                    Return New Vector2(1, 2)
                Case Else
                    Return MyBase.GetMenuImagePosition(P)
            End Select
        End Function

        Public Overrides Function GetOverworldAddition(ByVal P As Pokemon) As String
            Select Case P.AdditionalData.ToLower()
                Case "galar"
                    Return "_" & P.AdditionalData.ToLower()
                Case Else
                    Return ""
            End Select
        End Function

        Public Overrides Function GetDataFileAddition(ByVal AdditionalData As String) As String
            Select Case AdditionalData.ToLower()
                Case "galar"
                    Return "_" & AdditionalData.ToLower()
                Case Else
                    Return ""
            End Select
        End Function

    End Class

    Private Class Rapidash

        Inherits PokemonForm

        Public Sub New()
            MyBase.New(78)
        End Sub

        Public Overrides Function GetSheetName(P As Pokemon) As String
            Select Case P.AdditionalData
                Case "galar"
                    Return "Regional"
                Case Else
                    Return MyBase.GetSheetName(P)
            End Select
        End Function

        Public Overrides Function GetAnimationName(ByVal P As Pokemon) As String
            Select Case P.AdditionalData.ToLower()
                Case "galar"
                    Return P.OriginalName & "_" & P.AdditionalData.ToLower()
                Case Else
                    Return P.OriginalName
            End Select
        End Function

        Public Overrides Function GetMenuImagePosition(ByVal P As Pokemon) As Vector2
            Select Case P.AdditionalData.ToLower()
                Case "galar"
                    Return New Vector2(2, 2)
                Case Else
                    Return MyBase.GetMenuImagePosition(P)
            End Select
        End Function

        Public Overrides Function GetOverworldAddition(ByVal P As Pokemon) As String
            Select Case P.AdditionalData.ToLower()
                Case "galar"
                    Return "_" & P.AdditionalData.ToLower()
                Case Else
                    Return ""
            End Select
        End Function

        Public Overrides Function GetDataFileAddition(ByVal AdditionalData As String) As String
            Select Case AdditionalData.ToLower()
                Case "galar"
                    Return "_" & AdditionalData.ToLower()
                Case Else
                    Return ""
            End Select
        End Function

    End Class

    Private Class Slowpoke

        Inherits PokemonForm

        Public Sub New()
            MyBase.New(79)
        End Sub

        Public Overrides Function GetSheetName(P As Pokemon) As String
            Select Case P.AdditionalData
                Case "galar"
                    Return "Regional"
                Case Else
                    Return MyBase.GetSheetName(P)
            End Select
        End Function

        Public Overrides Function GetAnimationName(ByVal P As Pokemon) As String
            Select Case P.AdditionalData.ToLower()
                Case "galar"
                    Return P.OriginalName & "_" & P.AdditionalData.ToLower()
                Case Else
                    Return P.OriginalName
            End Select
        End Function

        Public Overrides Function GetMenuImagePosition(ByVal P As Pokemon) As Vector2
            Select Case P.AdditionalData.ToLower()
                Case "galar"
                    Return New Vector2(3, 2)
                Case Else
                    Return MyBase.GetMenuImagePosition(P)
            End Select
        End Function

        Public Overrides Function GetOverworldAddition(ByVal P As Pokemon) As String
            Select Case P.AdditionalData.ToLower()
                Case "galar"
                    Return "_" & P.AdditionalData.ToLower()
                Case Else
                    Return ""
            End Select
        End Function

        Public Overrides Function GetDataFileAddition(ByVal AdditionalData As String) As String
            Select Case AdditionalData.ToLower()
                Case "galar"
                    Return "_" & AdditionalData.ToLower()
                Case Else
                    Return ""
            End Select
        End Function

    End Class

    Private Class Farfetch 'd

        Inherits PokemonForm

        Public Sub New()
            MyBase.New(83)
        End Sub

        Public Overrides Function GetSheetName(P As Pokemon) As String
            Select Case P.AdditionalData
                Case "galar"
                    Return "Regional"
                Case Else
                    Return MyBase.GetSheetName(P)
            End Select
        End Function

        Public Overrides Function GetAnimationName(ByVal P As Pokemon) As String
            Select Case P.AdditionalData.ToLower()
                Case "galar"
                    Return P.OriginalName & "_" & P.AdditionalData.ToLower()
                Case Else
                    Return P.OriginalName
            End Select
        End Function

        Public Overrides Function GetMenuImagePosition(ByVal P As Pokemon) As Vector2
            Select Case P.AdditionalData.ToLower()
                Case "galar"
                    Return New Vector2(5, 2)
                Case Else
                    Return MyBase.GetMenuImagePosition(P)
            End Select
        End Function

        Public Overrides Function GetOverworldAddition(ByVal P As Pokemon) As String
            Select Case P.AdditionalData.ToLower()
                Case "galar"
                    Return "_" & P.AdditionalData.ToLower()
                Case Else
                    Return ""
            End Select
        End Function

        Public Overrides Function GetDataFileAddition(ByVal AdditionalData As String) As String
            Select Case AdditionalData.ToLower()
                Case "galar"
                    Return "_" & AdditionalData.ToLower()
                Case Else
                    Return ""
            End Select
        End Function

    End Class

    Private Class Weezing

        Inherits PokemonForm

        Public Sub New()
            MyBase.New(110)
        End Sub

        Public Overrides Function GetSheetName(P As Pokemon) As String
            Select Case P.AdditionalData
                Case "galar"
                    Return "Regional"
                Case Else
                    Return MyBase.GetSheetName(P)
            End Select
        End Function

        Public Overrides Function GetAnimationName(ByVal P As Pokemon) As String
            Select Case P.AdditionalData.ToLower()
                Case "galar"
                    Return P.OriginalName & "_" & P.AdditionalData.ToLower()
                Case Else
                    Return P.OriginalName
            End Select
        End Function

        Public Overrides Function GetMenuImagePosition(ByVal P As Pokemon) As Vector2
            Select Case P.AdditionalData.ToLower()
                Case "galar"
                    Return New Vector2(6, 2)
                Case Else
                    Return MyBase.GetMenuImagePosition(P)
            End Select
        End Function

        Public Overrides Function GetOverworldAddition(ByVal P As Pokemon) As String
            Select Case P.AdditionalData.ToLower()
                Case "galar"
                    Return "_" & P.AdditionalData.ToLower()
                Case Else
                    Return ""
            End Select
        End Function

        Public Overrides Function GetDataFileAddition(ByVal AdditionalData As String) As String
            Select Case AdditionalData.ToLower()
                Case "galar"
                    Return "_" & AdditionalData.ToLower()
                Case Else
                    Return ""
            End Select
        End Function

    End Class

    Private Class MrMime

        Inherits PokemonForm

        Public Sub New()
            MyBase.New(122)
        End Sub

        Public Overrides Function GetSheetName(P As Pokemon) As String
            Select Case P.AdditionalData
                Case "galar"
                    Return "Regional"
                Case Else
                    Return MyBase.GetSheetName(P)
            End Select
        End Function

        Public Overrides Function GetAnimationName(ByVal P As Pokemon) As String
            Select Case P.AdditionalData.ToLower()
                Case "galar"
                    Return P.OriginalName & "_" & P.AdditionalData.ToLower()
                Case Else
                    Return P.OriginalName
            End Select
        End Function

        Public Overrides Function GetMenuImagePosition(ByVal P As Pokemon) As Vector2
            Select Case P.AdditionalData.ToLower()
                Case "galar"
                    Return New Vector2(7, 2)
                Case Else
                    Return MyBase.GetMenuImagePosition(P)
            End Select
        End Function

        Public Overrides Function GetOverworldAddition(ByVal P As Pokemon) As String
            Select Case P.AdditionalData.ToLower()
                Case "galar"
                    Return "_" & P.AdditionalData.ToLower()
                Case Else
                    Return ""
            End Select
        End Function

        Public Overrides Function GetDataFileAddition(ByVal AdditionalData As String) As String
            Select Case AdditionalData.ToLower()
                Case "galar"
                    Return "_" & AdditionalData.ToLower()
                Case Else
                    Return ""
            End Select
        End Function

    End Class

    Private Class Articuno

        Inherits PokemonForm

        Public Sub New()
            MyBase.New(144)
        End Sub

        Public Overrides Function GetSheetName(P As Pokemon) As String
            Select Case P.AdditionalData
                Case "galar"
                    Return "Regional"
                Case Else
                    Return MyBase.GetSheetName(P)
            End Select
        End Function

        Public Overrides Function GetAnimationName(ByVal P As Pokemon) As String
            Select Case P.AdditionalData.ToLower()
                Case "galar"
                    Return P.OriginalName & "_" & P.AdditionalData.ToLower()
                Case Else
                    Return P.OriginalName
            End Select
        End Function

        Public Overrides Function GetMenuImagePosition(ByVal P As Pokemon) As Vector2
            Select Case P.AdditionalData.ToLower()
                Case "galar"
                    Return New Vector2(8, 2)
                Case Else
                    Return MyBase.GetMenuImagePosition(P)
            End Select
        End Function

        Public Overrides Function GetOverworldAddition(ByVal P As Pokemon) As String
            Select Case P.AdditionalData.ToLower()
                Case "galar"
                    Return "_" & P.AdditionalData.ToLower()
                Case Else
                    Return ""
            End Select
        End Function

        Public Overrides Function GetDataFileAddition(ByVal AdditionalData As String) As String
            Select Case AdditionalData.ToLower()
                Case "galar"
                    Return "_" & AdditionalData.ToLower()
                Case Else
                    Return ""
            End Select
        End Function

    End Class

    Private Class Zapdos

        Inherits PokemonForm

        Public Sub New()
            MyBase.New(145)
        End Sub

        Public Overrides Function GetSheetName(P As Pokemon) As String
            Select Case P.AdditionalData
                Case "galar"
                    Return "Regional"
                Case Else
                    Return MyBase.GetSheetName(P)
            End Select
        End Function

        Public Overrides Function GetAnimationName(ByVal P As Pokemon) As String
            Select Case P.AdditionalData.ToLower()
                Case "galar"
                    Return P.OriginalName & "_" & P.AdditionalData.ToLower()
                Case Else
                    Return P.OriginalName
            End Select
        End Function

        Public Overrides Function GetMenuImagePosition(ByVal P As Pokemon) As Vector2
            Select Case P.AdditionalData.ToLower()
                Case "galar"
                    Return New Vector2(9, 2)
                Case Else
                    Return MyBase.GetMenuImagePosition(P)
            End Select
        End Function

        Public Overrides Function GetOverworldAddition(ByVal P As Pokemon) As String
            Select Case P.AdditionalData.ToLower()
                Case "galar"
                    Return "_" & P.AdditionalData.ToLower()
                Case Else
                    Return ""
            End Select
        End Function

        Public Overrides Function GetDataFileAddition(ByVal AdditionalData As String) As String
            Select Case AdditionalData.ToLower()
                Case "galar"
                    Return "_" & AdditionalData.ToLower()
                Case Else
                    Return ""
            End Select
        End Function

    End Class

    Private Class Moltres

        Inherits PokemonForm

        Public Sub New()
            MyBase.New(146)
        End Sub

        Public Overrides Function GetSheetName(P As Pokemon) As String
            Select Case P.AdditionalData
                Case "galar"
                    Return "Regional"
                Case Else
                    Return MyBase.GetSheetName(P)
            End Select
        End Function

        Public Overrides Function GetAnimationName(ByVal P As Pokemon) As String
            Select Case P.AdditionalData.ToLower()
                Case "galar"
                    Return P.OriginalName & "_" & P.AdditionalData.ToLower()
                Case Else
                    Return P.OriginalName
            End Select
        End Function

        Public Overrides Function GetMenuImagePosition(ByVal P As Pokemon) As Vector2
            Select Case P.AdditionalData.ToLower()
                Case "galar"
                    Return New Vector2(10, 2)
                Case Else
                    Return MyBase.GetMenuImagePosition(P)
            End Select
        End Function

        Public Overrides Function GetOverworldAddition(ByVal P As Pokemon) As String
            Select Case P.AdditionalData.ToLower()
                Case "galar"
                    Return "_" & P.AdditionalData.ToLower()
                Case Else
                    Return ""
            End Select
        End Function

        Public Overrides Function GetDataFileAddition(ByVal AdditionalData As String) As String
            Select Case AdditionalData.ToLower()
                Case "galar"
                    Return "_" & AdditionalData.ToLower()
                Case Else
                    Return ""
            End Select
        End Function

    End Class

    Private Class Slowking

        Inherits PokemonForm

        Public Sub New()
            MyBase.New(199)
        End Sub

        Public Overrides Function GetSheetName(P As Pokemon) As String
            Select Case P.AdditionalData
                Case "galar"
                    Return "Regional"
                Case Else
                    Return MyBase.GetSheetName(P)
            End Select
        End Function

        Public Overrides Function GetAnimationName(ByVal P As Pokemon) As String
            Select Case P.AdditionalData.ToLower()
                Case "galar"
                    Return P.OriginalName & "_" & P.AdditionalData.ToLower()
                Case Else
                    Return P.OriginalName
            End Select
        End Function

        Public Overrides Function GetMenuImagePosition(ByVal P As Pokemon) As Vector2
            Select Case P.AdditionalData.ToLower()
                Case "galar"
                    Return New Vector2(11, 2)
                Case Else
                    Return MyBase.GetMenuImagePosition(P)
            End Select
        End Function

        Public Overrides Function GetOverworldAddition(ByVal P As Pokemon) As String
            Select Case P.AdditionalData.ToLower()
                Case "galar"
                    Return "_" & P.AdditionalData.ToLower()
                Case Else
                    Return ""
            End Select
        End Function

        Public Overrides Function GetDataFileAddition(ByVal AdditionalData As String) As String
            Select Case AdditionalData.ToLower()
                Case "galar"
                    Return "_" & AdditionalData.ToLower()
                Case Else
                    Return ""
            End Select
        End Function

    End Class

    Private Class Corsola

        Inherits PokemonForm

        Public Sub New()
            MyBase.New(222)
        End Sub

        Public Overrides Function GetSheetName(P As Pokemon) As String
            Select Case P.AdditionalData
                Case "galar"
                    Return "Regional"
                Case Else
                    Return MyBase.GetSheetName(P)
            End Select
        End Function

        Public Overrides Function GetAnimationName(ByVal P As Pokemon) As String
            Select Case P.AdditionalData.ToLower()
                Case "galar"
                    Return P.OriginalName & "_" & P.AdditionalData.ToLower()
                Case Else
                    Return P.OriginalName
            End Select
        End Function

        Public Overrides Function GetMenuImagePosition(ByVal P As Pokemon) As Vector2
            Select Case P.AdditionalData.ToLower()
                Case "galar"
                    Return New Vector2(12, 2)
                Case Else
                    Return MyBase.GetMenuImagePosition(P)
            End Select
        End Function

        Public Overrides Function GetOverworldAddition(ByVal P As Pokemon) As String
            Select Case P.AdditionalData.ToLower()
                Case "galar"
                    Return "_" & P.AdditionalData.ToLower()
                Case Else
                    Return ""
            End Select
        End Function

        Public Overrides Function GetDataFileAddition(ByVal AdditionalData As String) As String
            Select Case AdditionalData.ToLower()
                Case "galar"
                    Return "_" & AdditionalData.ToLower()
                Case Else
                    Return ""
            End Select
        End Function

    End Class

    Private Class Zigzagoon

        Inherits PokemonForm

        Public Sub New()
            MyBase.New(263)
        End Sub

        Public Overrides Function GetSheetName(P As Pokemon) As String
            Select Case P.AdditionalData
                Case "galar"
                    Return "Regional"
                Case Else
                    Return MyBase.GetSheetName(P)
            End Select
        End Function

        Public Overrides Function GetAnimationName(ByVal P As Pokemon) As String
            Select Case P.AdditionalData.ToLower()
                Case "galar"
                    Return P.OriginalName & "_" & P.AdditionalData.ToLower()
                Case Else
                    Return P.OriginalName
            End Select
        End Function

        Public Overrides Function GetMenuImagePosition(ByVal P As Pokemon) As Vector2
            Select Case P.AdditionalData.ToLower()
                Case "galar"
                    Return New Vector2(13, 2)
                Case Else
                    Return MyBase.GetMenuImagePosition(P)
            End Select
        End Function

        Public Overrides Function GetOverworldAddition(ByVal P As Pokemon) As String
            Select Case P.AdditionalData.ToLower()
                Case "galar"
                    Return "_" & P.AdditionalData.ToLower()
                Case Else
                    Return ""
            End Select
        End Function

        Public Overrides Function GetDataFileAddition(ByVal AdditionalData As String) As String
            Select Case AdditionalData.ToLower()
                Case "galar"
                    Return "_" & AdditionalData.ToLower()
                Case Else
                    Return ""
            End Select
        End Function

    End Class

    Private Class Linoone

        Inherits PokemonForm

        Public Sub New()
            MyBase.New(264)
        End Sub

        Public Overrides Function GetSheetName(P As Pokemon) As String
            Select Case P.AdditionalData
                Case "galar"
                    Return "Regional"
                Case Else
                    Return MyBase.GetSheetName(P)
            End Select
        End Function

        Public Overrides Function GetAnimationName(ByVal P As Pokemon) As String
            Select Case P.AdditionalData.ToLower()
                Case "galar"
                    Return P.OriginalName & "_" & P.AdditionalData.ToLower()
                Case Else
                    Return P.OriginalName
            End Select
        End Function

        Public Overrides Function GetMenuImagePosition(ByVal P As Pokemon) As Vector2
            Select Case P.AdditionalData.ToLower()
                Case "galar"
                    Return New Vector2(14, 2)
                Case Else
                    Return MyBase.GetMenuImagePosition(P)
            End Select
        End Function

        Public Overrides Function GetOverworldAddition(ByVal P As Pokemon) As String
            Select Case P.AdditionalData.ToLower()
                Case "galar"
                    Return "_" & P.AdditionalData.ToLower()
                Case Else
                    Return ""
            End Select
        End Function

        Public Overrides Function GetDataFileAddition(ByVal AdditionalData As String) As String
            Select Case AdditionalData.ToLower()
                Case "galar"
                    Return "_" & AdditionalData.ToLower()
                Case Else
                    Return ""
            End Select
        End Function

    End Class

    Private Class Darumaka

        Inherits PokemonForm

        Public Sub New()
            MyBase.New(554)
        End Sub

        Public Overrides Function GetSheetName(P As Pokemon) As String
            Select Case P.AdditionalData
                Case "galar"
                    Return "Regional"
                Case Else
                    Return MyBase.GetSheetName(P)
            End Select
        End Function

        Public Overrides Function GetAnimationName(ByVal P As Pokemon) As String
            Select Case P.AdditionalData.ToLower()
                Case "galar"
                    Return P.OriginalName & "_" & P.AdditionalData.ToLower()
                Case Else
                    Return P.OriginalName
            End Select
        End Function

        Public Overrides Function GetMenuImagePosition(ByVal P As Pokemon) As Vector2
            Select Case P.AdditionalData.ToLower()
                Case "galar"
                    Return New Vector2(15, 2)
                Case Else
                    Return MyBase.GetMenuImagePosition(P)
            End Select
        End Function

        Public Overrides Function GetOverworldAddition(ByVal P As Pokemon) As String
            Select Case P.AdditionalData.ToLower()
                Case "galar"
                    Return "_" & P.AdditionalData.ToLower()
                Case Else
                    Return ""
            End Select
        End Function

        Public Overrides Function GetDataFileAddition(ByVal AdditionalData As String) As String
            Select Case AdditionalData.ToLower()
                Case "galar"
                    Return "_" & AdditionalData.ToLower()
                Case Else
                    Return ""
            End Select
        End Function

    End Class

    Private Class Darmanitan

        Inherits PokemonForm

        Public Sub New()
            MyBase.New(555)
        End Sub

        Public Overrides Function GetSheetName(P As Pokemon) As String
            Select Case P.AdditionalData
                Case "galar"
                    Return "Regional"
                Case Else
                    Return MyBase.GetSheetName(P)
            End Select
        End Function

        Public Overrides Function GetAnimationName(ByVal P As Pokemon) As String
            Select Case P.AdditionalData.ToLower()
                Case "galar"
                    Return P.OriginalName & "_" & P.AdditionalData.ToLower()
                Case Else
                    Return P.OriginalName
            End Select
        End Function

        Public Overrides Function GetMenuImagePosition(ByVal P As Pokemon) As Vector2
            Select Case P.AdditionalData.ToLower()
                Case "galar"
                    Return New Vector2(0, 3)
                Case Else
                    Return MyBase.GetMenuImagePosition(P)
            End Select
        End Function

        Public Overrides Function GetOverworldAddition(ByVal P As Pokemon) As String
            Select Case P.AdditionalData.ToLower()
                Case "galar"
                    Return "_" & P.AdditionalData.ToLower()
                Case Else
                    Return ""
            End Select
        End Function

        Public Overrides Function GetDataFileAddition(ByVal AdditionalData As String) As String
            Select Case AdditionalData.ToLower()
                Case "galar"
                    Return "_" & AdditionalData.ToLower()
                Case Else
                    Return ""
            End Select
        End Function

    End Class

    Private Class Yamask

        Inherits PokemonForm

        Public Sub New()
            MyBase.New(562)
        End Sub

        Public Overrides Function GetSheetName(P As Pokemon) As String
            Select Case P.AdditionalData
                Case "galar"
                    Return "Regional"
                Case Else
                    Return MyBase.GetSheetName(P)
            End Select
        End Function

        Public Overrides Function GetAnimationName(ByVal P As Pokemon) As String
            Select Case P.AdditionalData.ToLower()
                Case "galar"
                    Return P.OriginalName & "_" & P.AdditionalData.ToLower()
                Case Else
                    Return P.OriginalName
            End Select
        End Function

        Public Overrides Function GetMenuImagePosition(ByVal P As Pokemon) As Vector2
            Select Case P.AdditionalData.ToLower()
                Case "galar"
                    Return New Vector2(1, 3)
                Case Else
                    Return MyBase.GetMenuImagePosition(P)
            End Select
        End Function

        Public Overrides Function GetOverworldAddition(ByVal P As Pokemon) As String
            Select Case P.AdditionalData.ToLower()
                Case "galar"
                    Return "_" & P.AdditionalData.ToLower()
                Case Else
                    Return ""
            End Select
        End Function

        Public Overrides Function GetDataFileAddition(ByVal AdditionalData As String) As String
            Select Case AdditionalData.ToLower()
                Case "galar"
                    Return "_" & AdditionalData.ToLower()
                Case Else
                    Return ""
            End Select
        End Function

    End Class

    Private Class Stunfisk

        Inherits PokemonForm

        Public Sub New()
            MyBase.New(618)
        End Sub

        Public Overrides Function GetSheetName(P As Pokemon) As String
            Select Case P.AdditionalData
                Case "galar"
                    Return "Regional"
                Case Else
                    Return MyBase.GetSheetName(P)
            End Select
        End Function

        Public Overrides Function GetAnimationName(ByVal P As Pokemon) As String
            Select Case P.AdditionalData.ToLower()
                Case "galar"
                    Return P.OriginalName & "_" & P.AdditionalData.ToLower()
                Case Else
                    Return P.OriginalName
            End Select
        End Function

        Public Overrides Function GetMenuImagePosition(ByVal P As Pokemon) As Vector2
            Select Case P.AdditionalData.ToLower()
                Case "galar"
                    Return New Vector2(2, 3)
                Case Else
                    Return MyBase.GetMenuImagePosition(P)
            End Select
        End Function

        Public Overrides Function GetOverworldAddition(ByVal P As Pokemon) As String
            Select Case P.AdditionalData.ToLower()
                Case "galar"
                    Return "_" & P.AdditionalData.ToLower()
                Case Else
                    Return ""
            End Select
        End Function

        Public Overrides Function GetDataFileAddition(ByVal AdditionalData As String) As String
            Select Case AdditionalData.ToLower()
                Case "galar"
                    Return "_" & AdditionalData.ToLower()
                Case Else
                    Return ""
            End Select
        End Function

    End Class

#End Region

#Region "Hisui"

    Private Class Growlithe

        Inherits PokemonForm

        Public Sub New()
            MyBase.New(58)
        End Sub

        Public Overrides Function GetSheetName(P As Pokemon) As String
            Select Case P.AdditionalData
                Case "hisui"
                    Return "Regional"
                Case Else
                    Return MyBase.GetSheetName(P)
            End Select
        End Function

        Public Overrides Function GetAnimationName(ByVal P As Pokemon) As String
            Select Case P.AdditionalData.ToLower()
                Case "hisui"
                    Return P.OriginalName & "_" & P.AdditionalData.ToLower()
                Case Else
                    Return P.OriginalName
            End Select
        End Function

        Public Overrides Function GetMenuImagePosition(ByVal P As Pokemon) As Vector2
            Select Case P.AdditionalData.ToLower()
                Case "hisui"
                    Return New Vector2(0, 4)
                Case Else
                    Return MyBase.GetMenuImagePosition(P)
            End Select
        End Function

        Public Overrides Function GetOverworldAddition(ByVal P As Pokemon) As String
            Select Case P.AdditionalData.ToLower()
                Case "hisui"
                    Return "_" & P.AdditionalData.ToLower()
                Case Else
                    Return ""
            End Select
        End Function

        Public Overrides Function GetDataFileAddition(ByVal AdditionalData As String) As String
            Select Case AdditionalData.ToLower()
                Case "hisui"
                    Return "_" & AdditionalData.ToLower()
                Case Else
                    Return ""
            End Select
        End Function

    End Class

    Private Class Arcanine

        Inherits PokemonForm

        Public Sub New()
            MyBase.New(59)
        End Sub

        Public Overrides Function GetSheetName(P As Pokemon) As String
            Select Case P.AdditionalData
                Case "hisui"
                    Return "Regional"
                Case Else
                    Return MyBase.GetSheetName(P)
            End Select
        End Function

        Public Overrides Function GetAnimationName(ByVal P As Pokemon) As String
            Select Case P.AdditionalData.ToLower()
                Case "hisui"
                    Return P.OriginalName & "_" & P.AdditionalData.ToLower()
                Case Else
                    Return P.OriginalName
            End Select
        End Function

        Public Overrides Function GetMenuImagePosition(ByVal P As Pokemon) As Vector2
            Select Case P.AdditionalData.ToLower()
                Case "hisui"
                    Return New Vector2(1, 4)
                Case Else
                    Return MyBase.GetMenuImagePosition(P)
            End Select
        End Function

        Public Overrides Function GetOverworldAddition(ByVal P As Pokemon) As String
            Select Case P.AdditionalData.ToLower()
                Case "hisui"
                    Return "_" & P.AdditionalData.ToLower()
                Case Else
                    Return ""
            End Select
        End Function

        Public Overrides Function GetDataFileAddition(ByVal AdditionalData As String) As String
            Select Case AdditionalData.ToLower()
                Case "hisui"
                    Return "_" & AdditionalData.ToLower()
                Case Else
                    Return ""
            End Select
        End Function

    End Class

    Private Class Voltorb

        Inherits PokemonForm

        Public Sub New()
            MyBase.New(100)
        End Sub

        Public Overrides Function GetSheetName(P As Pokemon) As String
            Select Case P.AdditionalData
                Case "hisui"
                    Return "Regional"
                Case Else
                    Return MyBase.GetSheetName(P)
            End Select
        End Function

        Public Overrides Function GetAnimationName(ByVal P As Pokemon) As String
            Select Case P.AdditionalData.ToLower()
                Case "hisui"
                    Return P.OriginalName & "_" & P.AdditionalData.ToLower()
                Case Else
                    Return P.OriginalName
            End Select
        End Function

        Public Overrides Function GetMenuImagePosition(ByVal P As Pokemon) As Vector2
            Select Case P.AdditionalData.ToLower()
                Case "hisui"
                    Return New Vector2(2, 4)
                Case Else
                    Return MyBase.GetMenuImagePosition(P)
            End Select
        End Function

        Public Overrides Function GetOverworldAddition(ByVal P As Pokemon) As String
            Select Case P.AdditionalData.ToLower()
                Case "hisui"
                    Return "_" & P.AdditionalData.ToLower()
                Case Else
                    Return ""
            End Select
        End Function

        Public Overrides Function GetDataFileAddition(ByVal AdditionalData As String) As String
            Select Case AdditionalData.ToLower()
                Case "hisui"
                    Return "_" & AdditionalData.ToLower()
                Case Else
                    Return ""
            End Select
        End Function

    End Class

    Private Class Electrode

        Inherits PokemonForm

        Public Sub New()
            MyBase.New(101)
        End Sub

        Public Overrides Function GetSheetName(P As Pokemon) As String
            Select Case P.AdditionalData
                Case "hisui"
                    Return "Regional"
                Case Else
                    Return MyBase.GetSheetName(P)
            End Select
        End Function

        Public Overrides Function GetAnimationName(ByVal P As Pokemon) As String
            Select Case P.AdditionalData.ToLower()
                Case "hisui"
                    Return P.OriginalName & "_" & P.AdditionalData.ToLower()
                Case Else
                    Return P.OriginalName
            End Select
        End Function

        Public Overrides Function GetMenuImagePosition(ByVal P As Pokemon) As Vector2
            Select Case P.AdditionalData.ToLower()
                Case "hisui"
                    Return New Vector2(3, 4)
                Case Else
                    Return MyBase.GetMenuImagePosition(P)
            End Select
        End Function

        Public Overrides Function GetOverworldAddition(ByVal P As Pokemon) As String
            Select Case P.AdditionalData.ToLower()
                Case "hisui"
                    Return "_" & P.AdditionalData.ToLower()
                Case Else
                    Return ""
            End Select
        End Function

        Public Overrides Function GetDataFileAddition(ByVal AdditionalData As String) As String
            Select Case AdditionalData.ToLower()
                Case "hisui"
                    Return "_" & AdditionalData.ToLower()
                Case Else
                    Return ""
            End Select
        End Function

    End Class

    Private Class Typhlosion

        Inherits PokemonForm

        Public Sub New()
            MyBase.New(157)
        End Sub

        Public Overrides Function GetSheetName(P As Pokemon) As String
            Select Case P.AdditionalData
                Case "hisui"
                    Return "Regional"
                Case Else
                    Return MyBase.GetSheetName(P)
            End Select
        End Function

        Public Overrides Function GetAnimationName(ByVal P As Pokemon) As String
            Select Case P.AdditionalData.ToLower()
                Case "hisui"
                    Return P.OriginalName & "_" & P.AdditionalData.ToLower()
                Case Else
                    Return P.OriginalName
            End Select
        End Function

        Public Overrides Function GetMenuImagePosition(ByVal P As Pokemon) As Vector2
            Select Case P.AdditionalData.ToLower()
                Case "hisui"
                    Return New Vector2(4, 4)
                Case Else
                    Return MyBase.GetMenuImagePosition(P)
            End Select
        End Function

        Public Overrides Function GetOverworldAddition(ByVal P As Pokemon) As String
            Select Case P.AdditionalData.ToLower()
                Case "hisui"
                    Return "_" & P.AdditionalData.ToLower()
                Case Else
                    Return ""
            End Select
        End Function

        Public Overrides Function GetDataFileAddition(ByVal AdditionalData As String) As String
            Select Case AdditionalData.ToLower()
                Case "hisui"
                    Return "_" & AdditionalData.ToLower()
                Case Else
                    Return ""
            End Select
        End Function

    End Class

    Private Class Qwilfish

        Inherits PokemonForm

        Public Sub New()
            MyBase.New(211)
        End Sub

        Public Overrides Function GetSheetName(P As Pokemon) As String
            Select Case P.AdditionalData
                Case "hisui"
                    Return "Regional"
                Case Else
                    Return MyBase.GetSheetName(P)
            End Select
        End Function

        Public Overrides Function GetAnimationName(ByVal P As Pokemon) As String
            Select Case P.AdditionalData.ToLower()
                Case "hisui"
                    Return P.OriginalName & "_" & P.AdditionalData.ToLower()
                Case Else
                    Return P.OriginalName
            End Select
        End Function

        Public Overrides Function GetMenuImagePosition(ByVal P As Pokemon) As Vector2
            Select Case P.AdditionalData.ToLower()
                Case "hisui"
                    Return New Vector2(5, 4)
                Case Else
                    Return MyBase.GetMenuImagePosition(P)
            End Select
        End Function

        Public Overrides Function GetOverworldAddition(ByVal P As Pokemon) As String
            Select Case P.AdditionalData.ToLower()
                Case "hisui"
                    Return "_" & P.AdditionalData.ToLower()
                Case Else
                    Return ""
            End Select
        End Function

        Public Overrides Function GetDataFileAddition(ByVal AdditionalData As String) As String
            Select Case AdditionalData.ToLower()
                Case "hisui"
                    Return "_" & AdditionalData.ToLower()
                Case Else
                    Return ""
            End Select
        End Function

    End Class

    Private Class Sneasel

        Inherits PokemonForm

        Public Sub New()
            MyBase.New(52159)
        End Sub

        Public Overrides Function GetSheetName(P As Pokemon) As String
            Select Case P.AdditionalData
                Case "hisui"
                    Return "Regional"
                Case Else
                    Return MyBase.GetSheetName(P)
            End Select
        End Function

        Public Overrides Function GetAnimationName(ByVal P As Pokemon) As String
            Select Case P.AdditionalData.ToLower()
                Case "hisui"
                    Return P.OriginalName & "_" & P.AdditionalData.ToLower()
                Case Else
                    Return P.OriginalName
            End Select
        End Function

        Public Overrides Function GetMenuImagePosition(ByVal P As Pokemon) As Vector2
            Select Case P.AdditionalData.ToLower()
                Case "hisui"
                    Return New Vector2(6, 4)
                Case Else
                    Return MyBase.GetMenuImagePosition(P)
            End Select
        End Function

        Public Overrides Function GetOverworldAddition(ByVal P As Pokemon) As String
            Select Case P.AdditionalData.ToLower()
                Case "hisui"
                    Return "_" & P.AdditionalData.ToLower()
                Case Else
                    Return ""
            End Select
        End Function

        Public Overrides Function GetDataFileAddition(ByVal AdditionalData As String) As String
            Select Case AdditionalData.ToLower()
                Case "hisui"
                    Return "_" & AdditionalData.ToLower()
                Case Else
                    Return ""
            End Select
        End Function

    End Class

    Private Class Samurott

        Inherits PokemonForm

        Public Sub New()
            MyBase.New(503)
        End Sub

        Public Overrides Function GetSheetName(P As Pokemon) As String
            Select Case P.AdditionalData
                Case "hisui"
                    Return "Regional"
                Case Else
                    Return MyBase.GetSheetName(P)
            End Select
        End Function

        Public Overrides Function GetAnimationName(ByVal P As Pokemon) As String
            Select Case P.AdditionalData.ToLower()
                Case "hisui"
                    Return P.OriginalName & "_" & P.AdditionalData.ToLower()
                Case Else
                    Return P.OriginalName
            End Select
        End Function

        Public Overrides Function GetMenuImagePosition(ByVal P As Pokemon) As Vector2
            Select Case P.AdditionalData.ToLower()
                Case "hisui"
                    Return New Vector2(7, 4)
                Case Else
                    Return MyBase.GetMenuImagePosition(P)
            End Select
        End Function

        Public Overrides Function GetOverworldAddition(ByVal P As Pokemon) As String
            Select Case P.AdditionalData.ToLower()
                Case "hisui"
                    Return "_" & P.AdditionalData.ToLower()
                Case Else
                    Return ""
            End Select
        End Function

        Public Overrides Function GetDataFileAddition(ByVal AdditionalData As String) As String
            Select Case AdditionalData.ToLower()
                Case "hisui"
                    Return "_" & AdditionalData.ToLower()
                Case Else
                    Return ""
            End Select
        End Function

    End Class

    Private Class Lilligant

        Inherits PokemonForm

        Public Sub New()
            MyBase.New(549)
        End Sub

        Public Overrides Function GetSheetName(P As Pokemon) As String
            Select Case P.AdditionalData
                Case "hisui"
                    Return "Regional"
                Case Else
                    Return MyBase.GetSheetName(P)
            End Select
        End Function

        Public Overrides Function GetAnimationName(ByVal P As Pokemon) As String
            Select Case P.AdditionalData.ToLower()
                Case "hisui"
                    Return P.OriginalName & "_" & P.AdditionalData.ToLower()
                Case Else
                    Return P.OriginalName
            End Select
        End Function

        Public Overrides Function GetMenuImagePosition(ByVal P As Pokemon) As Vector2
            Select Case P.AdditionalData.ToLower()
                Case "hisui"
                    Return New Vector2(8, 4)
                Case Else
                    Return MyBase.GetMenuImagePosition(P)
            End Select
        End Function

        Public Overrides Function GetOverworldAddition(ByVal P As Pokemon) As String
            Select Case P.AdditionalData.ToLower()
                Case "hisui"
                    Return "_" & P.AdditionalData.ToLower()
                Case Else
                    Return ""
            End Select
        End Function

        Public Overrides Function GetDataFileAddition(ByVal AdditionalData As String) As String
            Select Case AdditionalData.ToLower()
                Case "hisui"
                    Return "_" & AdditionalData.ToLower()
                Case Else
                    Return ""
            End Select
        End Function

    End Class

    Private Class Zorua

        Inherits PokemonForm

        Public Sub New()
            MyBase.New(570)
        End Sub

        Public Overrides Function GetSheetName(P As Pokemon) As String
            Select Case P.AdditionalData
                Case "hisui"
                    Return "Regional"
                Case Else
                    Return MyBase.GetSheetName(P)
            End Select
        End Function

        Public Overrides Function GetAnimationName(ByVal P As Pokemon) As String
            Select Case P.AdditionalData.ToLower()
                Case "hisui"
                    Return P.OriginalName & "_" & P.AdditionalData.ToLower()
                Case Else
                    Return P.OriginalName
            End Select
        End Function

        Public Overrides Function GetMenuImagePosition(ByVal P As Pokemon) As Vector2
            Select Case P.AdditionalData.ToLower()
                Case "hisui"
                    Return New Vector2(9, 4)
                Case Else
                    Return MyBase.GetMenuImagePosition(P)
            End Select
        End Function

        Public Overrides Function GetOverworldAddition(ByVal P As Pokemon) As String
            Select Case P.AdditionalData.ToLower()
                Case "hisui"
                    Return "_" & P.AdditionalData.ToLower()
                Case Else
                    Return ""
            End Select
        End Function

        Public Overrides Function GetDataFileAddition(ByVal AdditionalData As String) As String
            Select Case AdditionalData.ToLower()
                Case "hisui"
                    Return "_" & AdditionalData.ToLower()
                Case Else
                    Return ""
            End Select
        End Function

    End Class

    Private Class Zoroark

        Inherits PokemonForm

        Public Sub New()
            MyBase.New(571)
        End Sub

        Public Overrides Function GetSheetName(P As Pokemon) As String
            Select Case P.AdditionalData
                Case "hisui"
                    Return "Regional"
                Case Else
                    Return MyBase.GetSheetName(P)
            End Select
        End Function

        Public Overrides Function GetAnimationName(ByVal P As Pokemon) As String
            Select Case P.AdditionalData.ToLower()
                Case "hisui"
                    Return P.OriginalName & "_" & P.AdditionalData.ToLower()
                Case Else
                    Return P.OriginalName
            End Select
        End Function

        Public Overrides Function GetMenuImagePosition(ByVal P As Pokemon) As Vector2
            Select Case P.AdditionalData.ToLower()
                Case "hisui"
                    Return New Vector2(10, 4)
                Case Else
                    Return MyBase.GetMenuImagePosition(P)
            End Select
        End Function

        Public Overrides Function GetOverworldAddition(ByVal P As Pokemon) As String
            Select Case P.AdditionalData.ToLower()
                Case "hisui"
                    Return "_" & P.AdditionalData.ToLower()
                Case Else
                    Return ""
            End Select
        End Function

        Public Overrides Function GetDataFileAddition(ByVal AdditionalData As String) As String
            Select Case AdditionalData.ToLower()
                Case "hisui"
                    Return "_" & AdditionalData.ToLower()
                Case Else
                    Return ""
            End Select
        End Function

    End Class

    Private Class Braviary

        Inherits PokemonForm

        Public Sub New()
            MyBase.New(628)
        End Sub

        Public Overrides Function GetSheetName(P As Pokemon) As String
            Select Case P.AdditionalData
                Case "hisui"
                    Return "Regional"
                Case Else
                    Return MyBase.GetSheetName(P)
            End Select
        End Function

        Public Overrides Function GetAnimationName(ByVal P As Pokemon) As String
            Select Case P.AdditionalData.ToLower()
                Case "hisui"
                    Return P.OriginalName & "_" & P.AdditionalData.ToLower()
                Case Else
                    Return P.OriginalName
            End Select
        End Function

        Public Overrides Function GetMenuImagePosition(ByVal P As Pokemon) As Vector2
            Select Case P.AdditionalData.ToLower()
                Case "hisui"
                    Return New Vector2(11, 4)
                Case Else
                    Return MyBase.GetMenuImagePosition(P)
            End Select
        End Function

        Public Overrides Function GetOverworldAddition(ByVal P As Pokemon) As String
            Select Case P.AdditionalData.ToLower()
                Case "hisui"
                    Return "_" & P.AdditionalData.ToLower()
                Case Else
                    Return ""
            End Select
        End Function

        Public Overrides Function GetDataFileAddition(ByVal AdditionalData As String) As String
            Select Case AdditionalData.ToLower()
                Case "hisui"
                    Return "_" & AdditionalData.ToLower()
                Case Else
                    Return ""
            End Select
        End Function

    End Class

    Private Class Sliggoo

        Inherits PokemonForm

        Public Sub New()
            MyBase.New(705)
        End Sub

        Public Overrides Function GetSheetName(P As Pokemon) As String
            Select Case P.AdditionalData
                Case "hisui"
                    Return "Regional"
                Case Else
                    Return MyBase.GetSheetName(P)
            End Select
        End Function

        Public Overrides Function GetAnimationName(ByVal P As Pokemon) As String
            Select Case P.AdditionalData.ToLower()
                Case "hisui"
                    Return P.OriginalName & "_" & P.AdditionalData.ToLower()
                Case Else
                    Return P.OriginalName
            End Select
        End Function

        Public Overrides Function GetMenuImagePosition(ByVal P As Pokemon) As Vector2
            Select Case P.AdditionalData.ToLower()
                Case "hisui"
                    Return New Vector2(12, 4)
                Case Else
                    Return MyBase.GetMenuImagePosition(P)
            End Select
        End Function

        Public Overrides Function GetOverworldAddition(ByVal P As Pokemon) As String
            Select Case P.AdditionalData.ToLower()
                Case "hisui"
                    Return "_" & P.AdditionalData.ToLower()
                Case Else
                    Return ""
            End Select
        End Function

        Public Overrides Function GetDataFileAddition(ByVal AdditionalData As String) As String
            Select Case AdditionalData.ToLower()
                Case "hisui"
                    Return "_" & AdditionalData.ToLower()
                Case Else
                    Return ""
            End Select
        End Function

    End Class

    Private Class Goodra

        Inherits PokemonForm

        Public Sub New()
            MyBase.New(706)
        End Sub

        Public Overrides Function GetSheetName(P As Pokemon) As String
            Select Case P.AdditionalData
                Case "hisui"
                    Return "Regional"
                Case Else
                    Return MyBase.GetSheetName(P)
            End Select
        End Function

        Public Overrides Function GetAnimationName(ByVal P As Pokemon) As String
            Select Case P.AdditionalData.ToLower()
                Case "hisui"
                    Return P.OriginalName & "_" & P.AdditionalData.ToLower()
                Case Else
                    Return P.OriginalName
            End Select
        End Function

        Public Overrides Function GetMenuImagePosition(ByVal P As Pokemon) As Vector2
            Select Case P.AdditionalData.ToLower()
                Case "hisui"
                    Return New Vector2(13, 4)
                Case Else
                    Return MyBase.GetMenuImagePosition(P)
            End Select
        End Function

        Public Overrides Function GetOverworldAddition(ByVal P As Pokemon) As String
            Select Case P.AdditionalData.ToLower()
                Case "hisui"
                    Return "_" & P.AdditionalData.ToLower()
                Case Else
                    Return ""
            End Select
        End Function

        Public Overrides Function GetDataFileAddition(ByVal AdditionalData As String) As String
            Select Case AdditionalData.ToLower()
                Case "hisui"
                    Return "_" & AdditionalData.ToLower()
                Case Else
                    Return ""
            End Select
        End Function

    End Class

    Private Class Avalugg

        Inherits PokemonForm

        Public Sub New()
            MyBase.New(713)
        End Sub

        Public Overrides Function GetSheetName(P As Pokemon) As String
            Select Case P.AdditionalData
                Case "hisui"
                    Return "Regional"
                Case Else
                    Return MyBase.GetSheetName(P)
            End Select
        End Function

        Public Overrides Function GetAnimationName(ByVal P As Pokemon) As String
            Select Case P.AdditionalData.ToLower()
                Case "hisui"
                    Return P.OriginalName & "_" & P.AdditionalData.ToLower()
                Case Else
                    Return P.OriginalName
            End Select
        End Function

        Public Overrides Function GetMenuImagePosition(ByVal P As Pokemon) As Vector2
            Select Case P.AdditionalData.ToLower()
                Case "hisui"
                    Return New Vector2(14, 4)
                Case Else
                    Return MyBase.GetMenuImagePosition(P)
            End Select
        End Function

        Public Overrides Function GetOverworldAddition(ByVal P As Pokemon) As String
            Select Case P.AdditionalData.ToLower()
                Case "hisui"
                    Return "_" & P.AdditionalData.ToLower()
                Case Else
                    Return ""
            End Select
        End Function

        Public Overrides Function GetDataFileAddition(ByVal AdditionalData As String) As String
            Select Case AdditionalData.ToLower()
                Case "hisui"
                    Return "_" & AdditionalData.ToLower()
                Case Else
                    Return ""
            End Select
        End Function

    End Class

    Private Class Decidueye

        Inherits PokemonForm

        Public Sub New()
            MyBase.New(724)
        End Sub

        Public Overrides Function GetSheetName(P As Pokemon) As String
            Select Case P.AdditionalData
                Case "hisui"
                    Return "Regional"
                Case Else
                    Return MyBase.GetSheetName(P)
            End Select
        End Function

        Public Overrides Function GetAnimationName(ByVal P As Pokemon) As String
            Select Case P.AdditionalData.ToLower()
                Case "hisui"
                    Return P.OriginalName & "_" & P.AdditionalData.ToLower()
                Case Else
                    Return P.OriginalName
            End Select
        End Function

        Public Overrides Function GetMenuImagePosition(ByVal P As Pokemon) As Vector2
            Select Case P.AdditionalData.ToLower()
                Case "hisui"
                    Return New Vector2(15, 4)
                Case Else
                    Return MyBase.GetMenuImagePosition(P)
            End Select
        End Function

        Public Overrides Function GetOverworldAddition(ByVal P As Pokemon) As String
            Select Case P.AdditionalData.ToLower()
                Case "hisui"
                    Return "_" & P.AdditionalData.ToLower()
                Case Else
                    Return ""
            End Select
        End Function

        Public Overrides Function GetDataFileAddition(ByVal AdditionalData As String) As String
            Select Case AdditionalData.ToLower()
                Case "hisui"
                    Return "_" & AdditionalData.ToLower()
                Case Else
                    Return ""
            End Select
        End Function

    End Class

#End Region

#End Region

#End Region

End Class
