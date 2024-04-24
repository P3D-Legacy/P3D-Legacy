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
                    Return Core.Player.Pokedexes(dexIndex).Seen + Core.Player.Pokedexes(dexIndex).Obtained
                Case "getheight"
                    Dim id As Integer = CInt(argument.GetSplit(0, "_").GetSplit(0, ";"))
                    Dim ad As String = ""
                    If PokemonForms.GetAdditionalValueFromDataFile(argument) <> "" Then
                        ad = PokemonForms.GetAdditionalValueFromDataFile(argument)
                    End If

                    If Pokemon.PokemonDataExists(argument) Then
                        Return Pokemon.GetPokemonByID(id, ad).PokedexEntry.Height
                    End If
                Case "getweight"
                    Dim id As Integer = CInt(argument.GetSplit(0, "_").GetSplit(0, ";"))
                    Dim ad As String = ""
                    If PokemonForms.GetAdditionalValueFromDataFile(argument) <> "" Then
                        ad = PokemonForms.GetAdditionalValueFromDataFile(argument)
                    End If

                    If Pokemon.PokemonDataExists(argument) Then
                        Return Pokemon.GetPokemonByID(id, ad).PokedexEntry.Weight
                    End If
                Case "getentry"
                    Dim id As Integer = CInt(argument.GetSplit(0, "_").GetSplit(0, ";"))
                    Dim ad As String = ""
                    If PokemonForms.GetAdditionalValueFromDataFile(argument) <> "" Then
                        ad = PokemonForms.GetAdditionalValueFromDataFile(argument)
                    End If

                    If Pokemon.PokemonDataExists(argument) Then
                        Return Pokemon.GetPokemonByID(id, ad).PokedexEntry.Text
                    End If
                Case "getcolor"
                    Dim id As Integer = CInt(argument.GetSplit(0, "_").GetSplit(0, ";"))
                    Dim ad As String = ""
                    If PokemonForms.GetAdditionalValueFromDataFile(argument) <> "" Then
                        ad = PokemonForms.GetAdditionalValueFromDataFile(argument)
                    End If

                    If Pokemon.PokemonDataExists(argument) Then
                        Return Pokemon.GetPokemonByID(id, ad).PokedexEntry.Color.ToString()
                    End If
                Case "getspecies"
                    Dim id As Integer = CInt(argument.GetSplit(0, "_").GetSplit(0, ";"))
                    Dim ad As String = ""
                    If PokemonForms.GetAdditionalValueFromDataFile(argument) <> "" Then
                        ad = PokemonForms.GetAdditionalValueFromDataFile(argument)
                    End If

                    If Pokemon.PokemonDataExists(argument) Then
                        Return Pokemon.GetPokemonByID(id, ad).PokedexEntry.Species
                    End If
                Case "getname"
                    Dim id As Integer = CInt(argument.GetSplit(0, "_").GetSplit(0, ";"))
                    Dim ad As String = ""
                    If PokemonForms.GetAdditionalValueFromDataFile(argument) <> "" Then
                        ad = PokemonForms.GetAdditionalValueFromDataFile(argument)
                    End If

                    If Pokemon.PokemonDataExists(argument) Then
                        Return Pokemon.GetPokemonByID(id, ad).GetName()
                    End If
                Case "getability"
                    Dim id As Integer = CInt(argument.GetSplit(0, ",").GetSplit(0, "_").GetSplit(0, ";"))
                    Dim ad As String = ""
                    If PokemonForms.GetAdditionalValueFromDataFile(argument.GetSplit(0, ",").GetSplit(0, ";")) <> "" Then
                        ad = PokemonForms.GetAdditionalValueFromDataFile(argument)
                    End If
                    If Pokemon.PokemonDataExists(argument.GetSplit(0, ",")) Then
                        Select Case argument.GetSplit(1)
                            Case "0"
                                Return Pokemon.GetPokemonByID(id, ad).NewAbilities(Core.Random.Next(0, Pokemon.GetPokemonByID(id, ad).NewAbilities.Count)).ID
                            Case "1"
                                Return Pokemon.GetPokemonByID(id, ad).NewAbilities(0).ID
                            Case "2"
                                Return Pokemon.GetPokemonByID(id, ad).NewAbilities(Pokemon.GetPokemonByID(id, ad).NewAbilities.Count - 1).ID
                            Case "3"
                                Return Pokemon.GetPokemonByID(id, ad).HiddenAbility.ID

                        End Select
                    End If
            End Select

            Return DEFAULTNULL
        End Function

    End Class

End Namespace