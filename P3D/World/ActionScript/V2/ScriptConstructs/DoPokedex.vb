Namespace ScriptVersion2

    Partial Class ScriptComparer

        ' --------------------------------------------------------------------------------------------------------------------------
        ' Contains the <pokedex> constructs.
        ' --------------------------------------------------------------------------------------------------------------------------

        Private Shared Function DoPokedex(ByVal subClass As String) As Object
            Dim command As String = GetSubClassArgumentPair(subClass).Command
            Dim argument As String = GetSubClassArgumentPair(subClass).Argument

            Select Case command.ToLower()
                Case "caught"
                    Return Pokedex.CountEntries(Core.Player.PokedexData, {2, 3}).ToString()
                Case "shiny"
                    Return Pokedex.CountEntries(Core.Player.PokedexData, {3}).ToString()
                Case "seen"
                    Return Pokedex.CountEntries(Core.Player.PokedexData, {1}).ToString()
                Case "dexcaught"
                    Dim dexIndex As Integer = int(argument)
                    Return Core.Player.Pokedexes(dexIndex).Obtained
                Case "dexseen"
                    Dim dexIndex As Integer = int(argument)
                    Return Core.Player.Pokedexes(dexIndex).Seen
                Case "getheight"
                    Dim number As Integer = int(argument)
                    If Pokemon.PokemonDataExists(number) Then
                        Return Pokemon.GetPokemonByID(number).PokedexEntry.Height
                    End If
                Case "getweight"
                    Dim number As Integer = int(argument)
                    If Pokemon.PokemonDataExists(number) Then
                        Return Pokemon.GetPokemonByID(number).PokedexEntry.Weight
                    End If
                Case "getentry"
                    Dim number As Integer = int(argument)
                    If Pokemon.PokemonDataExists(number) Then
                        Return Pokemon.GetPokemonByID(number).PokedexEntry.Text
                    End If
                Case "getcolor"
                    Dim number As Integer = int(argument)
                    If Pokemon.PokemonDataExists(number) Then
                        Return Pokemon.GetPokemonByID(number).PokedexEntry.Color.ToString()
                    End If
                Case "getspecies"
                    Dim number As Integer = int(argument)
                    If Pokemon.PokemonDataExists(number) Then
                        Return Pokemon.GetPokemonByID(number).PokedexEntry.Species
                    End If
                Case "getname"
                    Dim number As Integer = int(argument)
                    If Pokemon.PokemonDataExists(number) Then
                        Return Pokemon.GetPokemonByID(number).GetName()
                    End If
                Case "getability"
                    Dim number As Integer = int(argument.GetSplit(0))
                    If Pokemon.PokemonDataExists(number) Then
                        Select Case argument.GetSplit(1)
                            Case "0"
                                Return Pokemon.GetPokemonByID(number).NewAbilities(Core.Random.Next(0, Pokemon.GetPokemonByID(number).NewAbilities.Count)).ID
                            Case "1"
                                Return Pokemon.GetPokemonByID(number).NewAbilities(0).ID
                            Case "2"
                                Return Pokemon.GetPokemonByID(number).NewAbilities(Pokemon.GetPokemonByID(number).NewAbilities.Count - 1).ID
                            Case "3"
                                Return Pokemon.GetPokemonByID(number).HiddenAbility.ID

                        End Select
                    End If
            End Select

            Return DEFAULTNULL
        End Function

    End Class

End Namespace