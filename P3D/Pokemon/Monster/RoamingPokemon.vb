Public Class RoamingPokemon

    Public RoamerID As String = ""
    Public WorldID As Integer = -1
    Public LevelFile As String = ""
    Public MusicLoop As String = ""
    Public PokemonReference As Pokemon = Nothing
    Public ScriptPath As String = ""

    Public Sub New(ByVal DataLine As String)
        Dim data() As String = DataLine.Split(CChar("|"))

        Me.RoamerID = data(0)
        Me.PokemonReference = Pokemon.GetPokemonByData(data(6))

        Me.WorldID = CInt(data(3))
        Me.LevelFile = data(4)
        Me.MusicLoop = data(5)

        If data.Length = 9 Then
            ScriptPath = data(8)
        End If
    End Sub

    Public Function CompareData() As String
        Return Me.RoamerID & "|" & PokemonForms.GetPokemonDataFileName(Me.PokemonReference.Number, Me.PokemonReference.AdditionalData, True) & "|" & Me.PokemonReference.Level.ToString() & "|" & Me.WorldID.ToString() & "|"
    End Function

    Public Function GetPokemon() As Pokemon
        Return Me.PokemonReference
    End Function

    Public Shared Sub ShiftRoamingPokemon(ByVal worldID As Integer)
        Logger.Debug("Shift Roaming Pokémon for world ID: " & worldID.ToString())

        Dim newData As String = ""
        For Each line As String In Core.Player.RoamingPokemonData.SplitAtNewline()
            If line <> "" And line.CountSeperators("|") >= 7 Then
                Dim data() As String = line.Split(CChar("|"))

                If newData <> "" Then
                    newData &= Environment.NewLine
                End If

                If CInt(data(3)) = worldID Or worldID = -1 Then
                    Dim regionsFile As String = GameModeManager.GetScriptPath("worldmap\roaming_regions.dat")
                    Security.FileValidation.CheckFileValid(regionsFile, False, "RoamingPokemon.vb")

                    Dim worldList As List(Of String) = System.IO.File.ReadAllLines(regionsFile).ToList()
                    Dim levelList As New List(Of String)

                    For Each worldLine As String In worldList
                        If worldLine.StartsWith(CInt(data(3)).ToString() & "|") = True Then
                            levelList = worldLine.Remove(0, worldLine.IndexOf("|") + 1).Split(CChar(",")).ToList()
                        End If
                    Next

                    Dim currentIndex As Integer = levelList.IndexOf(data(4))
                    Dim nextIndex As Integer = currentIndex + 1
                    If nextIndex > levelList.Count - 1 Then
                        nextIndex = 0
                    End If
                    'RoamerID|PokémonID|Level|regionID|startLevelFile|MusicLoop|Shiny|PokemonData|ScriptPath
                    If data.Length = 8 Then
                        'No Script
                        newData &= data(0) & "|" & data(1) & "|" & data(2) & "|" & CInt(data(3)).ToString() & "|" & levelList(nextIndex) & "|" & data(5) & "|" & data(6) & "|" & data(7) & "|"
                    Else
                        'With Script
                        newData &= data(0) & "|" & data(1) & "|" & data(2) & "|" & CInt(data(3)).ToString() & "|" & levelList(nextIndex) & "|" & data(5) & "|" & data(6) & "|" & data(7) & "|" & data(8)
                    End If

                Else
                    newData &= line
                End If
            End If
        Next

        Core.Player.RoamingPokemonData = newData
    End Sub

    ''' <summary>
    ''' Removes the Pokemon from the list of roaming Pokemon. The Pokemon has to hold the data as Tag.
    ''' </summary>
    ''' <param name="p">The Pokemon containing the Tag.</param>
    Public Shared Function RemoveRoamingPokemon(ByVal p As RoamingPokemon) As String
        Dim compareData As String = p.CompareData()

        Dim newData As String = ""

        For Each line As String In Core.Player.RoamingPokemonData.SplitAtNewline()
            If line.CountSeperators("|") = 7 AndAlso line.StartsWith(compareData.Remove(0, compareData.IndexOf("|") + 1)) = False Then
                If newData <> "" Then
                    newData &= Environment.NewLine
                End If
                newData &= line
            ElseIf line.StartsWith(compareData) = False Then
                If newData <> "" Then
                    newData &= Environment.NewLine
                End If
                newData &= line
            End If
        Next

        Return newData
    End Function

    Public Shared Function ReplaceRoamingPokemon(ByVal p As RoamingPokemon) As String
        Dim compareData As String = p.CompareData()

        Dim newData As String = ""

        For Each line As String In Core.Player.RoamingPokemonData.SplitAtNewline()
            If newData <> "" Then
                newData &= Environment.NewLine
            End If

            If line.CountSeperators("|") = 7 AndAlso line.StartsWith(compareData.Remove(0, compareData.IndexOf("|") + 1)) = False Then
                newData &= line
            ElseIf line.StartsWith(compareData) = False Then
                newData &= line
            Else
                newData &= p.RoamerID & "|" & p.PokemonReference.Number & "|" & p.PokemonReference.Level & "|" & p.WorldID.ToString() & "|" & p.LevelFile & "|" & p.MusicLoop & "|" & p.PokemonReference.IsShiny & "|" & p.PokemonReference.GetSaveData() & "|" & p.ScriptPath
            End If
        Next

        Return newData
    End Function

End Class
