Namespace Construct.Framework.Classes

    <ScriptClass("Daycare")>
    <ScriptDescription("A class to access the Daycare.")>
    Public Class CL_Daycare

        Inherits ScriptClass

#Region "Commands"

        <ScriptCommand("TakeEgg")>
        <ScriptDescription("Takes the egg out of a daycare.")>
        Private Function M_TakeEgg(ByVal argument As String) As String
            Dim newData As String = ""
            Dim dayCareID As Integer = Int(argument)

            For Each line As String In Game.Core.Player.DaycareData.SplitAtNewline()
                If line.StartsWith(dayCareID.ToString() & "|Egg|") = False Then
                    If newData <> "" Then
                        newData &= vbNewLine
                    End If
                    newData &= line
                Else
                    Dim p As Pokemon = Daycare.ProduceEgg(dayCareID)
                    If Not p Is Nothing Then
                        Game.Core.Player.Pokemons.Add(p)
                    End If
                End If
            Next

            Game.Core.Player.DaycareData = newData

            Return Core.Null
        End Function

        <ScriptCommand("TakePokemon")>
        <ScriptDescription("Takes a Pokémon out of a daycare.")>
        Private Function M_TakePokemon(ByVal argument As String) As String
            Dim newData As String = ""

            Dim dayCareID As Integer = Int(argument.Split(","c)(0))
            Dim PokemonIndex As Integer = Int(argument.Split(","c)(1))

            For Each line As String In Game.Core.Player.DaycareData.SplitAtNewline()
                If line.StartsWith(dayCareID.ToString() & "|" & PokemonIndex.ToString() & "|") = True Then
                    Dim data As String = line.Remove(0, line.IndexOf("{"))
                    Dim startStep As Integer = CInt(line.Split(CChar("|"))(2))
                    Dim p As Pokemon = Pokemon.GetPokemonByData(data)
                    p.GetExperience(Game.Core.Player.DaycareSteps - startStep, True)
                    Game.Core.Player.Pokemons.Add(p)
                Else
                    If newData <> "" Then
                        newData &= vbNewLine
                    End If
                    newData &= line
                End If
            Next

            Game.Core.Player.DaycareData = newData

            Return Core.Null
        End Function

        <ScriptCommand("LeavePokemon")>
        <ScriptDescription("Leaves a Pokémon at a daycare.")>
        Private Function M_LeavePokemon(ByVal argument As String) As String
            Dim dayCareID As Integer = Int(argument.Split(","c)(0))
            Dim PokemonDaycareIndex As Integer = Int(argument.Split(","c)(1))
            Dim PokemonIndex As Integer = Int(argument.Split(","c)(2))

            If Game.Core.Player.DaycareData <> "" Then
                Game.Core.Player.DaycareData &= vbNewLine
            End If

            Game.Core.Player.DaycareData &= dayCareID.ToString() & "|" &
                PokemonDaycareIndex.ToString() & "|" &
                Game.Core.Player.DaycareSteps & "|0|" &
                Game.Core.Player.Pokemons(PokemonIndex).GetSaveData()

            Game.Core.Player.Pokemons.RemoveAt(PokemonIndex)

            Return Core.Null
        End Function

        <ScriptCommand("RemoveEgg")>
        <ScriptDescription("Disposes an egg in a daycare.")>
        Private Function M_RemoveEgg(ByVal argument As String) As String
            Dim newData As String = ""
            Dim dayCareID As Integer = Int(argument)

            For Each line As String In Game.Core.Player.DaycareData.SplitAtNewline()
                If line.StartsWith(dayCareID.ToString() & "|Egg|") = False Then
                    If newData <> "" Then
                        newData &= vbNewLine
                    End If
                    newData &= line
                End If
            Next

            Game.Core.Player.DaycareData = newData

            Return Core.Null
        End Function

        <ScriptCommand("Clean")>
        <ScriptDescription("Cleans a daycare's data storage. Removes empty lines, adjusts Pokémon etc.")>
        Private Function M_Clean(ByVal argument As String) As String
            Dim daycareID As Integer = Int(argument)
            Dim newData As String = ""

            Dim lines As New List(Of String)
            For Each line As String In Game.Core.Player.DaycareData.SplitAtNewline()
                If line.StartsWith(daycareID & "|") = True Then
                    lines.Add(line)
                Else
                    If newData <> "" Then
                        newData &= vbNewLine
                    End If
                    newData &= line
                End If
            Next

            For i = 0 To lines.Count - 1
                Dim line As String = lines(i)
                Dim data() As String = line.Split(CChar("|"))

                If newData <> "" Then
                    newData &= vbNewLine
                End If

                If data(1) = "Egg" Then
                    newData &= daycareID.ToString() & "|Egg|" & data(2)
                Else
                    newData &= daycareID.ToString() & "|" & i.ToString() & "|" & data(2) & "|" & data(3) & "|" & line.Remove(0, line.IndexOf("{"))
                End If
            Next

            Game.Core.Player.DaycareData = newData

            Return Core.Null
        End Function

        <ScriptCommand("ClearData")>
        <ScriptDescription("Clears all data from a daycare.")>
        Private Function M_ClearData(ByVal argument As String) As String
            Dim daycareID As Integer = Int(argument)
            Dim newData As String = ""

            For Each line As String In Game.Core.Player.DaycareData.SplitAtNewline()
                If line.StartsWith(daycareID.ToString() & "|") = False Then
                    If newData <> "" Then
                        newData &= vbNewLine
                    End If
                    newData &= line
                End If
            Next

            Game.Core.Player.DaycareData = newData

            Return Core.Null
        End Function

        <ScriptCommand("Call")>
        <ScriptDescription("Calls a daycare.")>
        Private Function M_Call(ByVal argument As String) As String
            Daycare.TriggerCall(Int(argument))

            Return Core.Null
        End Function

#End Region

#Region "Constructs"

        <ScriptConstruct("PokemonID")>
        <ScriptDescription("Returns the Pokemon ID for a pokemon in a daycare.")>
        Private Function F_PokemonID(ByVal argument As String) As String
            Dim dayCareID As Integer = Int(argument.Split(","c)(0))
            Dim PokemonIndex As Integer = Int(argument.Split(","c)(1))

            For Each line As String In Game.Core.Player.DaycareData.SplitAtNewline()
                If line.StartsWith(dayCareID.ToString() & "|" & PokemonIndex.ToString() & "|") = True Then
                    Dim data As String = line.Remove(0, line.IndexOf("{"))
                    Dim p As Pokemon = Pokemon.GetPokemonByData(data)
                    Return ToString(p.Number)
                End If
            Next

            Return ToString(0)
        End Function

        <ScriptConstruct("PokemonName")>
        <ScriptDescription("Returns the Pokemon name in a daycare.")>
        Private Function F_PokemonName(ByVal argument As String) As String
            Dim dayCareID As Integer = Int(argument.Split(","c)(0))
            Dim PokemonIndex As Integer = Int(argument.Split(","c)(1))

            For Each line As String In Game.Core.Player.DaycareData.SplitAtNewline()
                If line.StartsWith(dayCareID.ToString() & "|" & PokemonIndex.ToString() & "|") = True Then
                    Dim data As String = line.Remove(0, line.IndexOf("{"))
                    Dim p As Pokemon = Pokemon.GetPokemonByData(data)
                    Return p.GetDisplayName()
                End If
            Next

            Return "MissingNo."
        End Function

        <ScriptConstruct("ShinyIndicator")>
        <ScriptDescription("Returns N or S for a Pokémon in a daycare.")>
        Private Function F_ShinyIndicator(ByVal argument As String) As String
            Dim dayCareID As Integer = Int(argument.Split(","c)(0))
            Dim PokemonIndex As Integer = Int(argument.Split(","c)(1))

            For Each line As String In Game.Core.Player.DaycareData.SplitAtNewline()
                If line.StartsWith(dayCareID.ToString() & "|" & PokemonIndex.ToString() & "|") = True Then
                    Dim data As String = line.Remove(0, line.IndexOf("{"))
                    Dim p As Pokemon = Pokemon.GetPokemonByData(data)

                    If p.IsShiny = True Then
                        Return "S"
                    Else
                        Return "N"
                    End If
                End If
            Next

            Return "N"
        End Function

        <ScriptConstruct("PokemonSprite")>
        <ScriptDescription("Returns the Pokémon sprite for a Pokémon in a daycare usable by an NPC.")>
        Private Function F_PokemonSprite(ByVal argument As String) As String
            Dim dayCareID As Integer = Int(argument.Split(","c)(0))
            Dim PokemonIndex As Integer = Int(argument.Split(","c)(1))

            For Each line As String In Game.Core.Player.DaycareData.SplitAtNewline()
                If line.StartsWith(dayCareID.ToString() & "|" & PokemonIndex.ToString() & "|") = True Then
                    Dim data As String = line.Remove(0, line.IndexOf("{"))
                    Dim p As Pokemon = Pokemon.GetPokemonByData(data)

                    Dim shiny As String = "N"
                    If p.IsShiny = True Then
                        shiny = "S"
                    End If

                    Return "[POKEMON|" & shiny & "]" & p.Number.ToString() & PokemonForms.GetOverworldAddition(p)
                End If
            Next

            Return "[POKEMON|N]10"
        End Function

        <ScriptConstruct("CountPokemon")>
        <ScriptDescription("Returns the amount of Pokémon in a daycare.")>
        Private Function F_CountPokemon(ByVal argument As String) As String
            Dim count As Integer = 0
            If Game.Core.Player.DaycareData <> "" Then
                Dim dayCareID As Integer = Int(argument)
                For Each line As String In Game.Core.Player.DaycareData.SplitAtNewline()
                    If line.StartsWith(dayCareID.ToString() & "|") = True Then
                        count += 1
                    End If
                Next
            End If
            Return ToString(count)
        End Function

        <ScriptConstruct("HasPokemon")>
        <ScriptDescription("If a daycare has a Pokémon.")>
        Private Function F_HasPokemon(ByVal argument As String) As String
            Dim count As Integer = 0
            If Game.Core.Player.DaycareData <> "" Then
                Dim dayCareID As Integer = Int(argument)
                For Each line As String In Game.Core.Player.DaycareData.SplitAtNewline()
                    If line.StartsWith(dayCareID.ToString() & "|") = True Then
                        count += 1
                    End If
                Next
            End If

            Return ToString(count > 0)
        End Function

        <ScriptConstruct("CanSwim")>
        <ScriptDescription("Returns if a Pokémon can swim.")>
        Private Function F_CanSwim(ByVal argument As String) As String
            Dim dayCareID As Integer = Int(argument.Split(","c)(0))
            Dim PokemonIndex As Integer = Int(argument.Split(","c)(1))

            For Each line As String In Game.Core.Player.DaycareData.SplitAtNewline()
                If line.StartsWith(dayCareID.ToString() & "|" & PokemonIndex.ToString() & "|") = True Then
                    Dim data As String = line.Remove(0, line.IndexOf("{"))
                    Dim p As Pokemon = Pokemon.GetPokemonByData(data)

                    Return ToString(p.CanSwim)
                End If
            Next

            Return ToString(False)
        End Function

        <ScriptConstruct("HasEgg")>
        <ScriptDescription("Returns if a daycare has an egg.")>
        Private Function F_HasEgg(ByVal argument As String) As String
            Dim dayCareID As Integer = Int(argument)

            For Each line As String In Game.Core.Player.DaycareData.SplitAtNewline()
                If line.StartsWith(dayCareID.ToString() & "|Egg|") = True Then
                    Return ToString(True)
                End If
            Next
            Return ToString(False)
        End Function

        <ScriptConstruct("GrownLevels")>
        <ScriptDescription("Returns the level a Pokémon in a daycare has grown.")>
        Private Function F_GrownLevels(ByVal argument As String) As String
            Dim dayCareID As Integer = Int(argument.Split(","c)(0))
            Dim PokemonIndex As Integer = Int(argument.Split(","c)(1))

            For Each line As String In Game.Core.Player.DaycareData.SplitAtNewline()
                If line.StartsWith(dayCareID.ToString() & "|" & PokemonIndex.ToString() & "|") = True Then
                    Dim data As String = line.Remove(0, line.IndexOf("{"))
                    Dim startStep As Integer = CInt(line.Split(CChar("|"))(2))
                    Dim p As Pokemon = Pokemon.GetPokemonByData(data)
                    Dim startLevel As Integer = p.Level
                    p.GetExperience(Game.Core.Player.DaycareSteps - startStep, True)

                    Return ToString(p.Level - startLevel)
                End If
            Next

            Return Core.Null
        End Function

        <ScriptConstruct("CurrentLevel")>
        <ScriptDescription("Returns the current level of a Pokémon in the daycare.")>
        Private Function F_CurrentLevel(ByVal argument As String) As String
            Dim dayCareID As Integer = Int(argument.Split(","c)(0))
            Dim PokemonIndex As Integer = Int(argument.Split(","c)(1))

            For Each line As String In Game.Core.Player.DaycareData.SplitAtNewline()
                If line.StartsWith(dayCareID.ToString() & "|" & PokemonIndex.ToString() & "|") = True Then
                    Dim data As String = line.Remove(0, line.IndexOf("{"))
                    Dim startStep As Integer = CInt(line.Split(CChar("|"))(2))
                    Dim p As Pokemon = Pokemon.GetPokemonByData(data)
                    p.GetExperience(Game.Core.Player.DaycareSteps - startStep, True)

                    Return ToString(p.Level)
                End If
            Next

            Return Core.Null
        End Function

        <ScriptConstruct("CanBreed")>
        <ScriptDescription("Returns the percentage that the Pokémon in the daycare can breed.")>
        Private Function F_CanBreed(ByVal argument As String) As String
            Dim dayCareID As Integer = Int(argument.Split(","c)(0))

            Return ToString(Daycare.CanBreed(dayCareID))
        End Function

#End Region

    End Class

End Namespace