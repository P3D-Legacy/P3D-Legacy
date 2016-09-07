Namespace ScriptVersion2

    Partial Class ScriptComparer

        '--------------------------------------------------------------------------------------------------------------------------
        'Contains the <overworldpokemon> constructs.
        '--------------------------------------------------------------------------------------------------------------------------

        Private Shared Function DoOverworldPokemon(ByVal subClass As String) As Object
            Dim command As String = GetSubClassArgumentPair(subClass).Command
            Dim argument As String = GetSubClassArgumentPair(subClass).Argument

            Select Case command.ToLower()
                Case "position"
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
                                        s &= int( .Position.X)
                                    Case "y"
                                        If s <> "" Then
                                            s &= ","
                                        End If
                                        s &= int( .Position.Y)
                                    Case "z"
                                        If s <> "" Then
                                            s &= ","
                                        End If
                                        s &= int( .Position.Z)
                                End Select
                            Next
                            Return s
                        Else
                            Return int( .Position.X) & "," & int( .Position.Y) & "," & int( .Position.Z)
                        End If
                    End With
                Case "visible"
                    Return ReturnBoolean(Screen.Level.OverworldPokemon.IsVisible())
                Case "skin"
                    If Screen.Level.OverworldPokemon.PokemonReference IsNot Nothing Then
                        Dim shinyString As String = "Normal"
                        If Screen.Level.OverworldPokemon.PokemonReference.IsShiny = True Then
                            shinyString = "Shiny"
                        End If
                        Dim addition As String = PokemonForms.GetOverworldAddition(Screen.Level.OverworldPokemon.PokemonReference)
                        Return "Pokemon\Overworld\" & shinyString & "\" & Screen.Level.OverworldPokemon.PokemonReference.Number & addition
                    End If
            End Select

            Return DEFAULTNULL
        End Function

    End Class

End Namespace