Namespace Construct.Framework.Classes

    <ScriptClass("OverworldPokemon")>
    <ScriptDescription("A class to manipulate the overworld Pokémon.")>
    Public Class CL_OverworldPokemon

        Inherits ScriptClass

        Private Function CanAccessOverworldPokemon() As Boolean
            If Screen.Level IsNot Nothing Then
                If Screen.Level.OverworldPokemon IsNot Nothing Then
                    If Screen.Level.OverworldPokemon.PokemonReference IsNot Nothing Then
                        Return True
                    End If
                End If
            End If
            Return False
        End Function

#Region "Commands"

        <ScriptCommand("Hide")>
        <ScriptDescription("Hide the overworld Pokémon.")>
        Private Function M_Hide(ByVal argument As String) As String
            If CanAccessOverworldPokemon() Then
                Screen.Level.OverworldPokemon.Visible = False
            End If

            Return Core.Null
        End Function

        <ScriptCommand("Show")>
        <ScriptDescription("Show the overworld Pokémon.")>
        Private Function M_Show(ByVal argument As String) As String
            If CanAccessOverworldPokemon() Then
                Screen.Level.OverworldPokemon.Visible = True
            End If

            Return Core.Null
        End Function

#End Region

#Region "Constructs"

        <ScriptConstruct("Position")>
        <ScriptDescription("Returns the current position of the overworld Pokémon.")>
        Private Function F_Position(ByVal argument As String) As String
            If CanAccessOverworldPokemon() Then
                With Screen.Level.OverworldPokemon
                    Dim args() As String = argument.Split(CChar(","))
                    If argument <> "" Then
                        Dim s As String = ""
                        For i = 0 To args.Length - 1
                            Select Case args(i)
                                Case "x"
                                    If s <> "" Then
                                        s &= ","
                                    End If
                                    s &= Int(ToString( .Position.X))
                                Case "y"
                                    If s <> "" Then
                                        s &= ","
                                    End If
                                    s &= Int(ToString( .Position.Y))
                                Case "z"
                                    If s <> "" Then
                                        s &= ","
                                    End If
                                    s &= Int(ToString( .Position.Z))
                            End Select
                        Next
                        Return s
                    Else
                        Return Int(ToString( .Position.X)) & "," & Int(ToString( .Position.Y)) & "," & Int(ToString( .Position.Z))
                    End If
                End With
            End If
            Return Core.Null
        End Function

        <ScriptConstruct("Visible")>
        <ScriptDescription("Returns if the overworld Pokémon is currently visible.")>
        Private Function F_Visible(ByVal argument As String) As String
            If CanAccessOverworldPokemon() Then
                Return ToString(Screen.Level.OverworldPokemon.IsVisible())
            End If
            Return Core.Null
        End Function

        <ScriptConstruct("Skin")>
        <ScriptDescription("Returns the skin of the overworld Pokémon.")>
        Private Function F_Skin(ByVal argument As String) As String
            If CanAccessOverworldPokemon() Then
                Dim shinyString As String = "Normal"
                If Screen.Level.OverworldPokemon.PokemonReference.IsShiny = True Then
                    shinyString = "Shiny"
                End If
                Dim addition As String = PokemonForms.GetOverworldAddition(Screen.Level.OverworldPokemon.PokemonReference)
                Return "Pokemon\Overworld\" & shinyString & "\" & Screen.Level.OverworldPokemon.PokemonReference.Number & addition
            End If
            Return Core.Null
        End Function

#End Region

    End Class

End Namespace