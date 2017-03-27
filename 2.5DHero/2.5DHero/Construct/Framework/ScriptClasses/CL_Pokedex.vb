Namespace Construct.Framework.Classes

    <ScriptClass("Pokedex")>
    <ScriptDescription("A class to operate the Pokédex.")>
    Public Class CL_Pokedex

        Inherits ScriptClass

#Region "Commands"

        <ScriptCommand("SetAutoDetect")>
        <ScriptDescription("(De)Activates the auto detect feature of the Pokédex.")>
        Private Function M_SetAutoDetect(ByVal argument As String) As String
            Pokedex.AutoDetect = Bool(argument)

            Return Core.Null
        End Function

#End Region

#Region "Constructs"

        <ScriptConstruct("Caught")>
        <ScriptDescription("Returns how many Pokémon the player has registered as caught.")>
        Private Function F_Caught(ByVal argument As String) As String
            If Converter.IsNumeric(argument) = True Then
                Return ToString(Game.Core.Player.Pokedexes(Int(argument)).Obtained)
            Else
                Return ToString(Pokedex.CountEntries(Game.Core.Player.PokedexData, {2, 3}))
            End If
        End Function

        <ScriptConstruct("Shiny")>
        <ScriptDescription("Returns how many Pokémon the player has registered as shiny.")>
        Private Function F_Shiny(ByVal argument As String) As String
            If Converter.IsNumeric(argument) = True Then
                Return ToString(Game.Core.Player.Pokedexes(Int(argument)).Shiny)
            Else
                Return ToString(Pokedex.CountEntries(Game.Core.Player.PokedexData, {3}))
            End If
        End Function

        <ScriptConstruct("Seen")>
        <ScriptDescription("Returns how many Pokémon the player has registered as seen.")>
        Private Function F_Seen(ByVal argument As String) As String
            If Converter.IsNumeric(argument) = True Then
                Return ToString(Game.Core.Player.Pokedexes(Int(argument)).Seen)
            Else
                Return ToString(Pokedex.CountEntries(Game.Core.Player.PokedexData, {1}))
            End If
        End Function

#Region "PokémonInfo"

        Private Function GetPokemon(ByVal arg As String) As Pokemon
            Return Pokemon.GetPokemonByID(Int(arg))
        End Function

        <ScriptConstruct("Name")>
        <ScriptDescription("Returns the name of a Pokémon.")>
        Private Function F_Name(ByVal argument As String) As String
            If Converter.IsNumeric(argument) = True Then
                Dim p = GetPokemon(argument)
                If Not p Is Nothing Then
                    Return p.GetDisplayName()
                End If
            End If
            Return Core.Null
        End Function

        <ScriptConstruct("Height")>
        <ScriptDescription("Returns the height of a Pokémon.")>
        Private Function F_Height(ByVal argument As String) As String
            If Converter.IsNumeric(argument) = True Then
                Dim p = GetPokemon(argument)
                If Not p Is Nothing Then
                    Return ToString(p.PokedexEntry.Height)
                End If
            End If
            Return Core.Null
        End Function

        <ScriptConstruct("Weight")>
        <ScriptDescription("Returns the weight of a Pokémon.")>
        Private Function F_Weight(ByVal argument As String) As String
            If Converter.IsNumeric(argument) = True Then
                Dim p = GetPokemon(argument)
                If Not p Is Nothing Then
                    Return ToString(p.PokedexEntry.Weight)
                End If
            End If
            Return Core.Null
        End Function

        <ScriptConstruct("Species")>
        <ScriptDescription("Returns the species of a Pokémon.")>
        Private Function F_Species(ByVal argument As String) As String
            If Converter.IsNumeric(argument) = True Then
                Dim p = GetPokemon(argument)
                If Not p Is Nothing Then
                    Return p.PokedexEntry.Species
                End If
            End If
            Return Core.Null
        End Function

        <ScriptConstruct("Type1")>
        <ScriptDescription("Returns the first type of a Pokémon.")>
        Private Function F_Type1(ByVal argument As String) As String
            If Converter.IsNumeric(argument) = True Then
                Dim p = GetPokemon(argument)
                If Not p Is Nothing Then
                    Return p.Type1.ToString()
                End If
            End If
            Return Core.Null
        End Function

        <ScriptConstruct("Type2")>
        <ScriptDescription("Returns the second type of a Pokémon.")>
        Private Function F_Type2(ByVal argument As String) As String
            If Converter.IsNumeric(argument) = True Then
                Dim p = GetPokemon(argument)
                If Not p Is Nothing Then
                    Return p.Type2.ToString()
                End If
            End If
            Return Core.Null
        End Function

        <ScriptConstruct("Color")>
        <ScriptDescription("Returns the color of a Pokémon.")>
        Private Function F_Color(ByVal argument As String) As String
            If Converter.IsNumeric(argument) = True Then
                Dim p = GetPokemon(argument)
                If Not p Is Nothing Then
                    Return p.PokedexEntry.Color.R.ToString() & "," &
                           p.PokedexEntry.Color.G.ToString() & "," &
                           p.PokedexEntry.Color.B.ToString()
                End If
            End If
            Return Core.Null
        End Function

#End Region

#End Region

    End Class

End Namespace